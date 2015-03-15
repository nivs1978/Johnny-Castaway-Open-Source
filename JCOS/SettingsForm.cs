using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SCRANTIC
{
  public partial class SettingsForm : Form
  {
    INI ini = new INI();
    string inipath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "JCOS");
    public SettingsForm()
    {
      InitializeComponent();
    }

    private void SettingsForm_Load(object sender, EventArgs e)
    {
      ini.load(Path.Combine(inipath, Program.INIFILE));
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      ini.save();
      Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}
