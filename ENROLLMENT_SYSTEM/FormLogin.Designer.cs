namespace Enrollment_System
{
    partial class FormLogin
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.LblSay = new System.Windows.Forms.Label();
            this.LblPDM = new System.Windows.Forms.Label();
            this.PbPDM = new System.Windows.Forms.PictureBox();
            this.LblLogin = new System.Windows.Forms.Label();
            this.LblEmail = new System.Windows.Forms.Label();
            this.LblPass = new System.Windows.Forms.Label();
            this.TxtEmail = new System.Windows.Forms.TextBox();
            this.TxtPass = new System.Windows.Forms.TextBox();
            this.BtnReg = new System.Windows.Forms.Button();
            this.ChkShowPass = new System.Windows.Forms.CheckBox();
            this.Btn_Login = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbPDM)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(44)))), ((int)(((byte)(21)))));
            this.panel1.Controls.Add(this.LblSay);
            this.panel1.Controls.Add(this.LblPDM);
            this.panel1.Controls.Add(this.PbPDM);
            this.panel1.Location = new System.Drawing.Point(-2, -5);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(360, 440);
            this.panel1.TabIndex = 0;
            // 
            // LblSay
            // 
            this.LblSay.AutoSize = true;
            this.LblSay.Font = new System.Drawing.Font("Agency FB", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblSay.ForeColor = System.Drawing.Color.Orange;
            this.LblSay.Location = new System.Drawing.Point(63, 262);
            this.LblSay.Name = "LblSay";
            this.LblSay.Size = new System.Drawing.Size(224, 18);
            this.LblSay.TabIndex = 3;
            this.LblSay.Text = "\"Where quality education is a right, not privilage.\"";
            // 
            // LblPDM
            // 
            this.LblPDM.AutoSize = true;
            this.LblPDM.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPDM.ForeColor = System.Drawing.Color.Orange;
            this.LblPDM.Location = new System.Drawing.Point(14, 226);
            this.LblPDM.Name = "LblPDM";
            this.LblPDM.Size = new System.Drawing.Size(331, 25);
            this.LblPDM.TabIndex = 2;
            this.LblPDM.Text = "PAMBAYANG DALUHASAAN NG MARILAO";
            // 
            // PbPDM
            // 
            this.PbPDM.BackgroundImage = global::Enrollment_System.Properties.Resources.image_removebg_preview;
            this.PbPDM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PbPDM.Location = new System.Drawing.Point(111, 75);
            this.PbPDM.Name = "PbPDM";
            this.PbPDM.Size = new System.Drawing.Size(142, 137);
            this.PbPDM.TabIndex = 0;
            this.PbPDM.TabStop = false;
            // 
            // LblLogin
            // 
            this.LblLogin.AutoSize = true;
            this.LblLogin.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblLogin.ForeColor = System.Drawing.Color.White;
            this.LblLogin.Location = new System.Drawing.Point(410, 70);
            this.LblLogin.Name = "LblLogin";
            this.LblLogin.Size = new System.Drawing.Size(90, 39);
            this.LblLogin.TabIndex = 1;
            this.LblLogin.Text = "LOGIN";
            // 
            // LblEmail
            // 
            this.LblEmail.AutoSize = true;
            this.LblEmail.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblEmail.ForeColor = System.Drawing.Color.White;
            this.LblEmail.Location = new System.Drawing.Point(413, 133);
            this.LblEmail.Name = "LblEmail";
            this.LblEmail.Size = new System.Drawing.Size(45, 19);
            this.LblEmail.TabIndex = 2;
            this.LblEmail.Text = "Email";
            // 
            // LblPass
            // 
            this.LblPass.AutoSize = true;
            this.LblPass.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblPass.ForeColor = System.Drawing.Color.White;
            this.LblPass.Location = new System.Drawing.Point(413, 190);
            this.LblPass.Name = "LblPass";
            this.LblPass.Size = new System.Drawing.Size(71, 19);
            this.LblPass.TabIndex = 3;
            this.LblPass.Text = "Password";
            // 
            // TxtEmail
            // 
            this.TxtEmail.BackColor = System.Drawing.Color.Orange;
            this.TxtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtEmail.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtEmail.ForeColor = System.Drawing.Color.White;
            this.TxtEmail.Location = new System.Drawing.Point(417, 155);
            this.TxtEmail.Name = "TxtEmail";
            this.TxtEmail.Size = new System.Drawing.Size(172, 23);
            this.TxtEmail.TabIndex = 1;
            // 
            // TxtPass
            // 
            this.TxtPass.BackColor = System.Drawing.Color.Orange;
            this.TxtPass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtPass.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtPass.ForeColor = System.Drawing.Color.White;
            this.TxtPass.Location = new System.Drawing.Point(417, 212);
            this.TxtPass.Name = "TxtPass";
            this.TxtPass.PasswordChar = '*';
            this.TxtPass.Size = new System.Drawing.Size(172, 23);
            this.TxtPass.TabIndex = 2;
            // 
            // BtnReg
            // 
            this.BtnReg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnReg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnReg.ForeColor = System.Drawing.Color.White;
            this.BtnReg.Location = new System.Drawing.Point(417, 288);
            this.BtnReg.Name = "BtnReg";
            this.BtnReg.Size = new System.Drawing.Size(75, 32);
            this.BtnReg.TabIndex = 4;
            this.BtnReg.Text = "REGISTER";
            this.BtnReg.UseVisualStyleBackColor = true;
            this.BtnReg.Click += new System.EventHandler(this.BtnReg_Click);
            // 
            // ChkShowPass
            // 
            this.ChkShowPass.AutoSize = true;
            this.ChkShowPass.ForeColor = System.Drawing.Color.White;
            this.ChkShowPass.Location = new System.Drawing.Point(417, 239);
            this.ChkShowPass.Name = "ChkShowPass";
            this.ChkShowPass.Size = new System.Drawing.Size(93, 17);
            this.ChkShowPass.TabIndex = 3;
            this.ChkShowPass.Text = "Show password";
            this.ChkShowPass.UseVisualStyleBackColor = true;
            this.ChkShowPass.CheckedChanged += new System.EventHandler(this.ChkShowPass_CheckedChanged);
            // 
            // Btn_Login
            // 
            this.Btn_Login.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Login.ForeColor = System.Drawing.Color.White;
            this.Btn_Login.Location = new System.Drawing.Point(514, 288);
            this.Btn_Login.Name = "Btn_Login";
            this.Btn_Login.Size = new System.Drawing.Size(75, 32);
            this.Btn_Login.TabIndex = 5;
            this.Btn_Login.Text = "LOGIN";
            this.Btn_Login.UseVisualStyleBackColor = true;
            this.Btn_Login.Click += new System.EventHandler(this.Btn_Login_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(44)))), ((int)(((byte)(21)))));
            this.BtnExit.FlatAppearance.BorderSize = 0;
            this.BtnExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(44)))), ((int)(((byte)(21)))));
            this.BtnExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(151)))), ((int)(((byte)(44)))), ((int)(((byte)(21)))));
            this.BtnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnExit.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExit.ForeColor = System.Drawing.Color.White;
            this.BtnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnExit.Location = new System.Drawing.Point(606, 12);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(32, 27);
            this.BtnExit.TabIndex = 10;
            this.BtnExit.Text = "x";
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(5F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Orange;
            this.ClientSize = new System.Drawing.Size(650, 400);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.Btn_Login);
            this.Controls.Add(this.ChkShowPass);
            this.Controls.Add(this.BtnReg);
            this.Controls.Add(this.TxtPass);
            this.Controls.Add(this.TxtEmail);
            this.Controls.Add(this.LblPass);
            this.Controls.Add(this.LblEmail);
            this.Controls.Add(this.LblLogin);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "FormLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLogin";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbPDM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LblSay;
        private System.Windows.Forms.Label LblPDM;
        private System.Windows.Forms.PictureBox PbPDM;
        private System.Windows.Forms.Label LblLogin;
        private System.Windows.Forms.Label LblEmail;
        private System.Windows.Forms.Label LblPass;
        private System.Windows.Forms.TextBox TxtEmail;
        private System.Windows.Forms.TextBox TxtPass;
        private System.Windows.Forms.Button BtnReg;
        private System.Windows.Forms.CheckBox ChkShowPass;
        private System.Windows.Forms.Button Btn_Login;
        private System.Windows.Forms.Button BtnExit;
    }
}

