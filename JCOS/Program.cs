/*
Program.cs

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
using System.Windows.Forms;

namespace SCRANTIC
{
  static class Program
  {
    public static string INIFILE = "ScrAntic.ini";
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
      try
      {

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        if (args.Length > 0)
        {
          string arg = args[0].ToLower().Trim();
          string handler = null;

          // Handle cases where arguments are separated by colon.
          // Examples: /c:1234567 or /P:1234567
          if (arg.Length > 2)
          {
            handler = arg.Substring(3).Trim();
            arg = arg.Substring(0, 2);
          }
          else if (args.Length > 1)
            handler = args[1];

          if (arg == "/c") // Configuration mode, show configuration form
          {
            Application.Run(new SettingsForm());
          }
          else if (arg == "/p") // Preview mode, handler given to preview window
          {
            if (arg == null)
            {
              MessageBox.Show("Sorry, but the expected window handle was not provided.",
                  "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return;
            }

            IntPtr previewWndHandle = new IntPtr(long.Parse(handler));
            Application.Run(new JohnnyCastawayForm(previewWndHandle));

          }
          else if (arg == "/s") // Run screensaverin full screen mode
          {
            ShowScreenSaver();
            Application.Run();
          }
        }
        else // If no parameters give, run in configuration mode
        {
          Application.Run(new SettingsForm());
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Johnny Castaway Open Source");
      }
    }

    static void ShowScreenSaver()
    {
      foreach (Screen screen in Screen.AllScreens)
      {
        if (screen.Primary)
        {
          JohnnyCastawayForm screensaver = new JohnnyCastawayForm(screen.Bounds);
          screensaver.Show();
        } else
          {
            Form black = new Form();
            black.StartPosition = FormStartPosition.Manual;
            black.Bounds = screen.Bounds;
            black.FormBorderStyle = FormBorderStyle.None;
            black.Show();
          }
      }
    }

  }
}
