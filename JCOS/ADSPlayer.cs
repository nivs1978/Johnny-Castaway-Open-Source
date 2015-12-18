/*
ADSPlayer.cs

This file is part of Johnny Castaway Open Source.

Copyright (c) 2015 Hans Milling

Johnny Castaway Open Source is free software: you can redistribute it and/or modify it under the terms of the
GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
or (at your option) any later version.
Johnny Castaway Open Source is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details. You should have received a copy of the
GNU General Public License along with Johnny Castaway Open Source. If not, see http://www.gnu.org/licenses/.

*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.Media;
using System.IO;
using System.Diagnostics;

namespace SCRANTIC
{
  public class ADSPlayer
  {
    private Graphics g;
    private Resource.ADS ads;
    Dictionary<UInt16, TTMPlayer> ttms = new Dictionary<ushort, TTMPlayer>();

    private BackgroundWorker bwads = new BackgroundWorker();
    public Bitmap screen = null;

    public bool Playing { get; set; }

    Random rand = new Random();
    List<UInt32> lastplayed = new List<UInt32>();

    //public bool busy = false;

    public event EventHandler<BitmapEventArgs> UpdateEvent;
    public event EventHandler CompleteEvent = null;

    public class BitmapEventArgs : EventArgs
    {
      public Bitmap Image;
      public BitmapEventArgs(Bitmap bitmap)
      {
        Image = bitmap;
      }
    }

    protected virtual void OnThresholdReached(BitmapEventArgs e)
    {
      EventHandler<BitmapEventArgs> handler = UpdateEvent;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (this.UpdateEvent != null)
        this.UpdateEvent(this, new BitmapEventArgs((Bitmap)e.UserState));
    }

    private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if ((e.Cancelled == true))
      {
      }

      else if (!(e.Error == null))
      {
        //MessageBox.Show("Error: " + e.Error.Message);
      }

      else
      {
        if (this.CompleteEvent != null)
          this.CompleteEvent(this, new EventArgs());
      }
      //busy = false;
    }

    public ADSPlayer(Resource.ADS ads)
    {
      screen = new Bitmap(640, 480);
      g = Graphics.FromImage(screen);
      this.ads = ads;
      bwads.WorkerReportsProgress = true;
      bwads.WorkerSupportsCancellation = true;
      bwads.DoWork += play;
      bwads.RunWorkerCompleted += bw_RunWorkerCompleted;
      //Debug.WriteLine("Properties:");
      //Debug.WriteLine("id\tname:");
      foreach (Resource.ADS.ResourceProperty rp in ads.resourceproperties)
      {
        //Debug.WriteLine(rp.id.ToString("X4") + "\t" + rp.name);
        Resource.TTM ttm = (Resource.TTM)ResourceManager.get(rp.name);
        TTMPlayer ttmplayer = new TTMPlayer(ttm, screen);
        //ttmplayer.bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        ttmplayer.bw.ProgressChanged += bw_ProgressChanged;
        ttms.Add(rp.id, ttmplayer);
        /*foreach (UInt16 key in ttm.tags.Keys)
        {
          Debug.WriteLine("\t" + key.ToString("X4") + "\t" + ttm.tags[key]);
        }*/
      }
    }
    
    public void playAndWait(TTMPlayer ttmplayer, UInt16 instructionno)
    {
      ttmplayer.bw.RunWorkerAsync(instructionno);
      System.Threading.Thread.Sleep(20);
      while (ttmplayer.Playing)
        System.Threading.Thread.Sleep(20);
    }
    
    public void playTTMInstruction(Instruction instruction)
    {
      TTMPlayer ttmplayer = ttms[instruction.data[0]];
      //System.Diagnostics.Debug.WriteLine("ttm: " + instruction.data[0].ToString("X2") + ", sequence: " + instruction.data[1].ToString("X2"));
      playAndWait(ttmplayer, instruction.data[1]);
      byte repeat = (byte)(instruction.data[2] & 0xff);
      if (repeat > 0)
      {
        //System.Diagnostics.Debug.WriteLine("repeating: " + repeat);
        for (int i = 1; i < repeat; i++)
        {
          //System.Diagnostics.Debug.WriteLine("ttm: " + instruction.data[0].ToString("X2") + ", sequence: " + instruction.data[1].ToString("X2"));
          playAndWait(ttmplayer, instruction.data[1]);
          //                System.Threading.Thread.Sleep(100);
        }
      }
      lastplayed.Add(getTouple(instruction.data[0], instruction.data[1]));
    }

    private UInt32 getTouple(UInt16 a, UInt16 b)
    {
      return ((UInt32)a * 65536 + (UInt32)b);
    }

    public void runADS(UInt16 no)
    {
      bwads.RunWorkerAsync(no);
    }

    public void play(object sender, DoWorkEventArgs e)
    {
      Playing = true;
      UInt16 sequenceno = (UInt16)e.Argument;

      int length = ads.sequences[sequenceno].Count;
      List<Instruction> randoms = new List<Instruction>();
      List<Instruction> playback = new List<Instruction>();
      Dictionary<UInt32, int> bookmark = new Dictionary<uint, int>();
      //bool israndom = false;
      for (int inst = 0; inst < length; inst++)
      {
        Instruction instruction = ads.sequences[sequenceno][inst];
        //System.Diagnostics.Debug.WriteLine(inst + "ADS: " + instruction.ToString());
        switch (instruction.code)
        {
          case Instruction.SKIP_NEXT_IF: // 1350 Skip next instruction if previous wasn't a specific one.
          case Instruction.SKIP_NEXT_IF2:
            UInt32 key = getTouple(instruction.data[0], instruction.data[1]);
            if (!bookmark.ContainsKey(key))
              bookmark.Add(key, inst);
            bool runnext = lastplayed.Contains(getTouple(instruction.data[0], instruction.data[1]));
            while (ads.sequences[sequenceno][inst + 1].code == Instruction.OR)
            {
              inst += 2;
              instruction = ads.sequences[sequenceno][inst];
              key = getTouple(instruction.data[0], instruction.data[1]);
              if (!bookmark.ContainsKey(key))
                bookmark.Add(key, inst);
              runnext |= lastplayed.Contains(getTouple(instruction.data[0], instruction.data[1]));
            }

            if (!runnext)
            {
              while (ads.sequences[sequenceno][++inst].code != Instruction.PLAY_SEQUENCES) ; // Skip until next play sequence
              continue;
            }
            lastplayed.Clear();
            break;
          case Instruction.RANDOM_START:
            inst++;
            while (ads.sequences[sequenceno][inst].code != Instruction.RANDOM_END)
            {
              if (ads.sequences[sequenceno][inst].code == Instruction.ADD_TTM)
              {
                for (int i = 0; i < ads.sequences[sequenceno][inst].data[3]; i++)
                  randoms.Add(ads.sequences[sequenceno][inst]);
              }
              inst++;
            }
            break;
          case Instruction.PLAY_SEQUENCES:
            int rands = randoms.Count;
            if (rands > 0)
            {
              int no = rand.Next(rands);
              Instruction i = randoms[no];
              playTTMInstruction(i);
            }
            else
            {
              foreach (Instruction i in playback)
              {
                playTTMInstruction(i);
              }
            }
            randoms.Clear();
            playback.Clear();
            //            israndom = false;
            foreach (UInt32 i in lastplayed)
            {
              if (bookmark.ContainsKey(i))
              {
                int idx = bookmark[i];
                inst = idx - 1; // Jump to bookmark if the bookmark is earlier in the script
                break;
              }
            }
            break;
          case Instruction.ADD_TTM:
            //if (israndom)
            //for (int i = 0; i < instruction.data[3]; i++)
            //  randoms.Add(instruction);
            //else
            playback.Add(instruction);
            break;
          // case Instruction.ADD_RANDOM_LOOP:
          //  inst++;
          // randomloops.Add(getTouple(ads.sequences[sequenceno][inst].data[0], ads.sequences[sequenceno][inst].data[1]));
          //break;
          default:
            Debug.WriteLine("Unhandled instruction : " + instruction.code.ToString("X4"));
            break;
        }
        //        System.Threading.Thread.Sleep(1000);
      }
      Playing = false;
    }
  }
}
