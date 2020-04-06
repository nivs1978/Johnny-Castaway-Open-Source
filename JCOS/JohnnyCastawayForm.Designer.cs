/*
JohnnyCastawayForm.Designer.cs

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
namespace SCRANTIC
{
  partial class JohnnyCastawayForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.pbScreen = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.pbScreen)).BeginInit();
      this.SuspendLayout();
      // 
      // pbScreen
      // 
      this.pbScreen.Location = new System.Drawing.Point(94, 105);
      this.pbScreen.Margin = new System.Windows.Forms.Padding(2);
      this.pbScreen.Name = "pbScreen";
      this.pbScreen.Size = new System.Drawing.Size(480, 390);
      this.pbScreen.TabIndex = 1;
      this.pbScreen.TabStop = false;
      // 
      // JohnnyCastawayForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.Black;
      this.ClientSize = new System.Drawing.Size(640, 480);
      this.Controls.Add(this.pbScreen);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Margin = new System.Windows.Forms.Padding(2);
      this.Name = "JohnnyCastawayForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Form1";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.JohnnyCastawayForm_FormClosing);
      this.Load += new System.EventHandler(this.JohnnyCastawayForm_Load);
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.JohnnyCastawayForm_KeyPress);
      this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.JohnnyCastawayForm_MouseClick);
      this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.JohnnyCastawayForm_MouseMove);
      ((System.ComponentModel.ISupportInitialize)(this.pbScreen)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox pbScreen;
  }
}

