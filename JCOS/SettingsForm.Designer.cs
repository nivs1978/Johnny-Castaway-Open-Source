/*
SettingsForm.Designer.cs

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
  partial class SettingsForm
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
      this.btnAddHour = new System.Windows.Forms.Button();
      this.btnSubHour = new System.Windows.Forms.Button();
      this.lblStartOfDayText = new System.Windows.Forms.Label();
      this.lblStartOfDay = new System.Windows.Forms.Label();
      this.ckhLoadBackground = new System.Windows.Forms.CheckBox();
      this.chkPassword = new System.Windows.Forms.CheckBox();
      this.chkSounds = new System.Windows.Forms.CheckBox();
      this.pbLogo = new System.Windows.Forms.PictureBox();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // btnAddHour
      // 
      this.btnAddHour.Font = new System.Drawing.Font("Webdings", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.btnAddHour.Location = new System.Drawing.Point(80, 9);
      this.btnAddHour.Margin = new System.Windows.Forms.Padding(0);
      this.btnAddHour.Name = "btnAddHour";
      this.btnAddHour.Size = new System.Drawing.Size(15, 22);
      this.btnAddHour.TabIndex = 0;
      this.btnAddHour.Text = "5";
      this.btnAddHour.UseVisualStyleBackColor = true;
      // 
      // btnSubHour
      // 
      this.btnSubHour.Font = new System.Drawing.Font("Webdings", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
      this.btnSubHour.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
      this.btnSubHour.Location = new System.Drawing.Point(80, 31);
      this.btnSubHour.Margin = new System.Windows.Forms.Padding(0);
      this.btnSubHour.Name = "btnSubHour";
      this.btnSubHour.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
      this.btnSubHour.Size = new System.Drawing.Size(15, 22);
      this.btnSubHour.TabIndex = 1;
      this.btnSubHour.Text = "6";
      this.btnSubHour.UseVisualStyleBackColor = true;
      // 
      // lblStartOfDayText
      // 
      this.lblStartOfDayText.AutoSize = true;
      this.lblStartOfDayText.Location = new System.Drawing.Point(14, 23);
      this.lblStartOfDayText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblStartOfDayText.Name = "lblStartOfDayText";
      this.lblStartOfDayText.Size = new System.Drawing.Size(61, 13);
      this.lblStartOfDayText.TabIndex = 2;
      this.lblStartOfDayText.Text = "Start of day";
      // 
      // lblStartOfDay
      // 
      this.lblStartOfDay.AutoSize = true;
      this.lblStartOfDay.Location = new System.Drawing.Point(102, 23);
      this.lblStartOfDay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
      this.lblStartOfDay.Name = "lblStartOfDay";
      this.lblStartOfDay.Size = new System.Drawing.Size(45, 13);
      this.lblStartOfDay.TabIndex = 3;
      this.lblStartOfDay.Text = "9:00 am";
      this.lblStartOfDay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // ckhLoadBackground
      // 
      this.ckhLoadBackground.AutoSize = true;
      this.ckhLoadBackground.Location = new System.Drawing.Point(55, 55);
      this.ckhLoadBackground.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.ckhLoadBackground.Name = "ckhLoadBackground";
      this.ckhLoadBackground.Size = new System.Drawing.Size(111, 17);
      this.ckhLoadBackground.TabIndex = 4;
      this.ckhLoadBackground.Text = "&Load Background";
      this.ckhLoadBackground.UseVisualStyleBackColor = true;
      // 
      // chkPassword
      // 
      this.chkPassword.AutoSize = true;
      this.chkPassword.Location = new System.Drawing.Point(55, 77);
      this.chkPassword.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.chkPassword.Name = "chkPassword";
      this.chkPassword.Size = new System.Drawing.Size(72, 17);
      this.chkPassword.TabIndex = 5;
      this.chkPassword.Text = "&Password";
      this.chkPassword.UseVisualStyleBackColor = true;
      // 
      // chkSounds
      // 
      this.chkSounds.AutoSize = true;
      this.chkSounds.Location = new System.Drawing.Point(55, 99);
      this.chkSounds.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.chkSounds.Name = "chkSounds";
      this.chkSounds.Size = new System.Drawing.Size(62, 17);
      this.chkSounds.TabIndex = 6;
      this.chkSounds.Text = "&Sounds";
      this.chkSounds.UseVisualStyleBackColor = true;
      // 
      // pbLogo
      // 
      this.pbLogo.BackColor = System.Drawing.Color.Black;
      this.pbLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pbLogo.Image = global::SCRANTIC.Properties.Resources.LOGO;
      this.pbLogo.Location = new System.Drawing.Point(16, 72);
      this.pbLogo.Margin = new System.Windows.Forms.Padding(0);
      this.pbLogo.Name = "pbLogo";
      this.pbLogo.Size = new System.Drawing.Size(32, 32);
      this.pbLogo.TabIndex = 7;
      this.pbLogo.TabStop = false;
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(16, 122);
      this.btnOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(60, 23);
      this.btnOK.TabIndex = 8;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Location = new System.Drawing.Point(95, 122);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(60, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // SettingsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(170, 154);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.pbLogo);
      this.Controls.Add(this.chkSounds);
      this.Controls.Add(this.chkPassword);
      this.Controls.Add(this.ckhLoadBackground);
      this.Controls.Add(this.lblStartOfDay);
      this.Controls.Add(this.lblStartOfDayText);
      this.Controls.Add(this.btnSubHour);
      this.Controls.Add(this.btnAddHour);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SettingsForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Screen Antics";
      this.Load += new System.EventHandler(this.SettingsForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnAddHour;
    private System.Windows.Forms.Button btnSubHour;
    private System.Windows.Forms.Label lblStartOfDayText;
    private System.Windows.Forms.Label lblStartOfDay;
    private System.Windows.Forms.CheckBox ckhLoadBackground;
    private System.Windows.Forms.CheckBox chkPassword;
    private System.Windows.Forms.CheckBox chkSounds;
    private System.Windows.Forms.PictureBox pbLogo;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
  }
}