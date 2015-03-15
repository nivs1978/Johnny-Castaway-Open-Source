using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SCRANTIC
{
  public class Instruction
  {
    // Instructions from TTM resources
    public const UInt16 SAVE_BACKGROUND = 0x0020;
    public const UInt16 DRAW_BACKGROUND = 0x0080;
    public const UInt16 PURGE           = 0x0110;
    public const UInt16 UPDATE          = 0x0FF0;
    public const UInt16 DELAY           = 0x1020;
    public const UInt16 SLOT_IMAGE      = 0x1050;
    public const UInt16 SLOT_PALETTE    = 0x1060;
    public const UInt16 SET_SCENE       = 0x1110;
    public const UInt16 SET_FRAME0      = 0x2000;
    public const UInt16 SET_FRAME1      = 0x2010;
    public const UInt16 SET_WINDOW1     = 0x4000;
    public const UInt16 FADE_OUT        = 0x4110;
    public const UInt16 FADE_IN         = 0x4120;
    public const UInt16 SAVE_IMAGE0     = 0x4200;
    public const UInt16 SAVE_IMAGE1     = 0x4210;
    public const UInt16 DRAW_WHITE_LINE = 0xA0A0;
    public const UInt16 SET_WINDOW0     = 0xA100;
    public const UInt16 DRAW_BUBBLE     = 0xA400;
    public const UInt16 DRAW_SPRITE0    = 0xA500;
    public const UInt16 DRAW_SPRITE1    = 0xA510;
    public const UInt16 DRAW_SPRITE2    = 0xA520;
    public const UInt16 DRAW_SPRITE3    = 0xA530;
    public const UInt16 UNKNOWN2        = 0xA600;
    public const UInt16 DRAW_SCREEN     = 0xB600;
    public const UInt16 LOAD_SOUND      = 0xC020;
    public const UInt16 SELECT_SOUND    = 0xC030;
    public const UInt16 DESELECT_SOUND  = 0xC040;
    public const UInt16 PLAY_SOUND      = 0xC050;
    public const UInt16 STOP_SOUND      = 0xC060;
    public const UInt16 LOAD_SCREEN     = 0xF010;
    public const UInt16 LOAD_IMAGE      = 0xF020;
    public const UInt16 LOAD_PALETTE    = 0xF050;
    
    // Instructions from ADS resources
    public const UInt16 SKIP_NEXT_IF    = 0x1350;
    public const UInt16 SKIP_NEXT_IF2   = 0x1360;
    public const UInt16 OR              = 0x1430;
    public const UInt16 PLAY_SEQUENCES  = 0x1510;
    public const UInt16 PLAY_SEQUENCES2 = 0x1515;
    public const UInt16 ADD_TTM         = 0x2005;
    public const UInt16 RANDOM_START    = 0x3010;
    public const UInt16 RANDOM_UNKNOWN1 = 0x3020;
    public const UInt16 RANDOM_END      = 0x30FF;


    public UInt16 code;
    public string  name;
    public List<UInt16> data = new List<UInt16>();

    private string data2Str()
    {
      StringBuilder sb = new StringBuilder();
      bool first = true;
      foreach (UInt16 d in data)
      {
        if (first)
          first = false;
        else
          sb.Append(",");
        sb.Append(d.ToString());
      }
      return sb.ToString();
    }

    private string data2HexStr()
    {
      StringBuilder sb = new StringBuilder();
      bool first = true;
      foreach (UInt16 d in data)
      {
        if (first)
          first = false;
        else
          sb.Append(" ");
        sb.Append(d.ToString("X4"));
      }
      return sb.ToString();
    }

    public override string ToString()
    {
      string ret = "";
      switch (code)
      {
            case 0x0020:
                ret = "save background" ;
                break;
            case 0x0080:
                ret = "draw background" ;
                break;
            case 0x00c0:
                ret = "undefined "+code.ToString("X");
                break;
            case 0x0110:
                ret = "purge saved images";
                break;
            case 0x0400:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x0500:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x0510:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x0ff0:
                ret = "update";
                break;
            case 0x1020:
                ret = "delay (delay)";
                break;
            case 0x1050:
                ret = "select image (image)";
                break;
            case 0x1060:
                ret = "select palette (palette)";
                break;
            case 0x1070:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x1100:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x1110:
                ret = "set scene ("+data[0].ToString("X4")+")";
                break;
            case 0x1120:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x1200:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x2000:
                ret = "set frame 0 (?, frame)";
                break;
            case 0x2010:
                ret = "set frame 1 (?, frame)";
                break;
            case 0x2300:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x2310:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x2320:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x2400:
                ret = "undefined " + code.ToString("X");
                break;
            case 0x4000:
                ret = "set window (x, y, w, h)";
                break;
            case 0x4110:
                ret = "fade out (first, n, steps, delay)";
                break;
            case 0x4120:
                ret = "fade in (first, n, steps, delay)";
                break;
            case 0x4200:
                ret = "save image (x, y, w, h)";
                break;
            case 0x4210:
                ret = "save image (x, y, w, h)";
                break;
            case 0xa010:
                ret = "undefined " + code.ToString("X");
                break;
            case 0xa030:
                ret = "undefined " + code.ToString("X");
                break;
            case 0xa090:
                ret = "undefined " + code.ToString("X");
                break;
            case 0xA0A0:
                ret = "draw white line ";
                break;
            case 0xA0B0:
                ret = "undefined " + code.ToString("X");
                break;
            case 0xA100:
                ret = "set window (x, y, w, h)";
                break;
            case 0xA400:
                ret = "draw circle (x, y, w, h)";
                break;
            case 0xA500:
                ret = "draw sprite 0 (x, y, frame, image)";
                break;
            case 0xa510:
                ret = "draw sprite 1 (x, y, frame, image)";
                break;
            case 0xa520:
                ret = "draw sprite 2 (x, y, frame, image)";
                break;
            case 0xa530:
                ret = "draw sprite 3 (x, y, frame, image)";
                break;
            case 0xa5a0:
                ret = "undefined " + code.ToString("X");
                break;
            case 0xa600:
                ret = "undefined " + code.ToString("X");
                break;
            case 0xb600:
                ret = "draw screen (x, y, w, h, ?, ?)";
                break;
            case 0xc020:
                ret = "load sound resource";
                break;
            case 0xc030:
                ret = "select sound (sound)";
                break;
            case 0xc040:
                ret = "deselect sound (sound)";
                break;
            case 0xc050:
                string[] sounds = { "0", "Splash", "horn!", "cursing", "stretching", "puzzled", "humming", "grumble", "hurt", "breath", "thunder", "seagull attack", "exert", "swipe", "plunge", "short humm", "woman scholding", "spring", "mermaid", "seagull", "woosh", "flump", "sigh", "chimes" };
                if (data[0] < sounds.Length)
                  ret = "play sound (" + sounds[data[0]] + ")";
                else
                  ret = "play sound (" + data[0] + ")";
                break;
            case 0xc060:
                ret = "stop sound (sound)";
                break;
            case 0xf010:
                ret = "load screen resource ("+name+")";
                break;
            case 0xf020:
                ret = "load image resource ("+name+")";
                break;
            case 0xf040:
                ret = "undefined " + code.ToString("X");
                break;
            case 0xf050:
                ret = "load palette resource";
                break;
            default:
                ret = "unknown " + code.ToString("X");
                break;
      }

      return code.ToString("X4") + " - " + data2HexStr()/*+"\t"+ret*/;
    }
  }
}
