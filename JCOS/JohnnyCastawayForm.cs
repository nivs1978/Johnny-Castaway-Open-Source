/*
JohnnyCastawayForm.cs

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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Threading;
using SCRANTIC.Resource;
using System.Diagnostics;

namespace SCRANTIC
{
  public partial class JohnnyCastawayForm : Form
  {
    // For now the working scripts are hardcoded. In the future they will be read from the ADS files pointed by the FILES.VIN resource
    int count = 0;
    Bitmap latest = null;

    Random random = new Random();
    List<KeyValuePair<string, List<UInt16>>> scenes = new List<KeyValuePair<string, List<UInt16>>>() {
     { new KeyValuePair<string, List<UInt16>>("FISHING.ADS", new List<UInt16>() { 0x01/*,2,3,4,5,6,7,8*/} )  },
     { new KeyValuePair<string, List<UInt16>>("WALKSTUF.ADS", new List<UInt16>() { 0x03/*,2,3,4,5,6,7,8*/} )  },
     { new KeyValuePair<string, List<UInt16>>("MISCGAG.ADS", new List<UInt16>() { 0x01 } ) },
     { new KeyValuePair<string, List<UInt16>>("ACTIVITY.ADS", new List<UInt16>() { 0x01, 0x0C, 0x06, 0x08, 0x09/*,2,3,4,5,6,7,8*/} )  }
    };

    private Point mouseLocation;
    private Random rand = new Random();
    private bool previewMode = false;
    private Bitmap introsrc;
    private Bitmap introbmp;

    [DllImport("user32.dll")]
    static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

    BackgroundWorker bwintro = null;

    public JohnnyCastawayForm()
    {
      InitializeComponent();
    }

    public JohnnyCastawayForm(Rectangle Bounds)
    {
      InitializeComponent();
      this.Bounds = Bounds;
    }

    public JohnnyCastawayForm(IntPtr PreviewWndHandle)
    {
      InitializeComponent();

      // Set the preview window as the parent of this window
      SetParent(this.Handle, PreviewWndHandle);

      // Make this a child window so it will close when the parent dialog closes
      // GWL_STYLE = -16, WS_CHILD = 0x40000000
      SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

      // Place our window inside the parent
      Rectangle ParentRect;
      GetClientRect(PreviewWndHandle, out ParentRect);
      Size = ParentRect.Size;
      Location = new Point(0, 0);

      previewMode = true;
    }

    private void JohnnyCastawayForm_Load(object sender, EventArgs e)
    {
      Cursor.Hide();
      //TopMost = true;
      if (!previewMode)
      {
        pbScreen.Left = (Screen.PrimaryScreen.WorkingArea.Width - 640) / 2;
        pbScreen.Top = (Screen.PrimaryScreen.WorkingArea.Height - 480) / 2;
        pbScreen.Width = 640;
        pbScreen.Height = 480;
      }
      else
      {
        Width = 190;
        Height = 140;
        pbScreen.Left = 0;
        pbScreen.Top = 0;
        pbScreen.Width = Width;
        pbScreen.Height = Height;
      }
        bwintro = new BackgroundWorker();
      bwintro.DoWork += bwintro_DoWork;
      bwintro.ProgressChanged += bwintro_ProgressChanged;
      bwintro.RunWorkerCompleted += bw_RunWorkerCompleted;
      bwintro.WorkerReportsProgress = true;
      bwintro.WorkerSupportsCancellation = true;
      bwintro.RunWorkerAsync();
    }

    private void bwintro_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      introsrc = ((Resource.SCR)ResourceManager.get("INTRO.SCR")).image;
      introbmp = new Bitmap(640,480);

      Graphics g = Graphics.FromImage(introbmp);
      g.Clear(Color.Black);
      g.Save();
      bwintro.ReportProgress(0, new Bitmap(introbmp));

      for (int i = 0; i < 30; i++)
      {
        if (bwintro.CancellationPending)
          return;
        Bitmap circle = ClipToCircle(introsrc, new Point(320, 240), i*10);
        if (!previewMode)
          g.DrawImageUnscaled(circle, new Point(0, 0));
        else
          g.DrawImage(circle, -48, -35, 285, 210);
        Thread.Sleep(25);
        bwintro.ReportProgress(0, new Bitmap(introbmp));
      }
    }

    private void bwintro_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
      count++;
      lbLog.Items.Add(count.ToString());
      lbLog.SelectedIndex = (lbLog.Items.Count - 1);
      lbLog.SelectedIndex = -1; 
      pbScreen.Image = (Bitmap)e.UserState;
    }

    public void updateImage(object sender, ADSPlayer.BitmapEventArgs e)
    {
      latest = e.Image;
      pbScreen.Image = (Bitmap)e.Image;
    }

    public void complete(object sender, EventArgs e)
    {
      playRandomADS();
    }

    public void playRandomADS()
    {
      pbScreen.Image = new Bitmap(640, 480);
      pbScreen.Refresh();
      System.Threading.Thread.Sleep(1000);
      int total = scenes.Count;
      int sceneidx = random.Next(total);
      string adsname = scenes[sceneidx].Key;
      int scriptidx = random.Next(scenes[sceneidx].Value.Count);
      Log.write("Playing " + adsname + " sequence " + scriptidx);
      UInt16 scriptno = scenes[sceneidx].Value[scriptidx];
      ADS ads = (ADS)ResourceManager.get(adsname);
      ADSPlayer player = new ADSPlayer(ads);
      player.UpdateEvent += updateImage;
      player.CompleteEvent += complete;
      Debug.WriteLine("ADS: " + adsname + ", no: " + scriptno);
      player.runADS(scriptno);
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
        if (!previewMode)
        {
          // Run main screensaver
          playRandomADS();
        }
      }
    }

    public Bitmap ClipToCircle(Bitmap original, PointF center, float radius)
    {
      Bitmap copy = new Bitmap(original);
      using (Graphics g = Graphics.FromImage(copy))
      {
        g.Clear(Color.Black);
        RectangleF r = new RectangleF(center.X - radius, center.Y - radius, radius * 2, radius * 2);
        GraphicsPath path = new GraphicsPath();
        path.AddEllipse(r);
        g.Clip = new Region(path);
        g.DrawImage(original, 0, 0);
        return copy;
      }
    }

    private void doExit()
    {
      if (!previewMode)
      {
        if (bwintro != null)
        {
          if (!bwintro.CancellationPending)
            bwintro.CancelAsync();
          Application.Exit();
        }
      }
    }

    private void JohnnyCastawayForm_KeyPress(object sender, KeyPressEventArgs e)
    {
      doExit();
    }

    private void JohnnyCastawayForm_MouseClick(object sender, MouseEventArgs e)
    {
      doExit();
    }

    private void JohnnyCastawayForm_MouseMove(object sender, MouseEventArgs e)
    {
      if (!mouseLocation.IsEmpty && !previewMode)
      {
        if (Math.Abs(mouseLocation.X - e.X) > 5 ||
            Math.Abs(mouseLocation.Y - e.Y) > 5)
          doExit();
      }
      mouseLocation = e.Location;
    }

  }
}
