﻿namespace Enrollment_System
{
    partial class FormPayment
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
            this.PBPaymentQr = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ExitButton = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BtnUpload = new System.Windows.Forms.Button();
            this.BtnConfirm = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.PBProfPayment = new System.Windows.Forms.PictureBox();
            this.panel10 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PBPaymentQr)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExitButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBProfPayment)).BeginInit();
            this.panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // PBPaymentQr
            // 
            this.PBPaymentQr.BackColor = System.Drawing.Color.Transparent;
            this.PBPaymentQr.BackgroundImage = global::Enrollment_System.Properties.Resources.PAYMENTQR_removebg_preview;
            this.PBPaymentQr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PBPaymentQr.Location = new System.Drawing.Point(74, 99);
            this.PBPaymentQr.Name = "PBPaymentQr";
            this.PBPaymentQr.Size = new System.Drawing.Size(368, 375);
            this.PBPaymentQr.TabIndex = 2;
            this.PBPaymentQr.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(44)))), ((int)(((byte)(26)))));
            this.panel1.BackgroundImage = global::Enrollment_System.Properties.Resources.BACKGROUNDCOLOR;
            this.panel1.Controls.Add(this.ExitButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(911, 40);
            this.panel1.TabIndex = 1;
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.BackColor = System.Drawing.Color.Transparent;
            this.ExitButton.Image = global::Enrollment_System.Properties.Resources.XButton;
            this.ExitButton.Location = new System.Drawing.Point(876, 7);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(25, 26);
            this.ExitButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ExitButton.TabIndex = 1;
            this.ExitButton.TabStop = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = global::Enrollment_System.Properties.Resources.SCANNER;
            this.pictureBox1.Location = new System.Drawing.Point(11, 71);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(490, 420);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // BtnUpload
            // 
            this.BtnUpload.BackColor = System.Drawing.Color.Transparent;
            this.BtnUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnUpload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnUpload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUpload.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUpload.ForeColor = System.Drawing.Color.White;
            this.BtnUpload.Location = new System.Drawing.Point(31, 414);
            this.BtnUpload.Name = "BtnUpload";
            this.BtnUpload.Size = new System.Drawing.Size(131, 37);
            this.BtnUpload.TabIndex = 24;
            this.BtnUpload.Text = "UPLOAD";
            this.BtnUpload.UseVisualStyleBackColor = false;
            this.BtnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.BtnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnConfirm.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnConfirm.ForeColor = System.Drawing.Color.White;
            this.BtnConfirm.Location = new System.Drawing.Point(178, 414);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(117, 37);
            this.BtnConfirm.TabIndex = 25;
            this.BtnConfirm.Text = "CONFIRM";
            this.BtnConfirm.UseVisualStyleBackColor = false;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(69, 362);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 19);
            this.label2.TabIndex = 40;
            this.label2.Text = "UPLOAD PROF OF PAYMENT";
            // 
            // PBProfPayment
            // 
            this.PBProfPayment.BackColor = System.Drawing.Color.Transparent;
            this.PBProfPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PBProfPayment.Location = new System.Drawing.Point(14, 25);
            this.PBProfPayment.Name = "PBProfPayment";
            this.PBProfPayment.Size = new System.Drawing.Size(294, 316);
            this.PBProfPayment.TabIndex = 41;
            this.PBProfPayment.TabStop = false;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(44)))), ((int)(((byte)(26)))));
            this.panel10.BackgroundImage = global::Enrollment_System.Properties.Resources.BACKGROUNDCOLOR;
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Controls.Add(this.PBProfPayment);
            this.panel10.Controls.Add(this.label2);
            this.panel10.Controls.Add(this.BtnUpload);
            this.panel10.Controls.Add(this.BtnConfirm);
            this.panel10.Location = new System.Drawing.Point(530, 56);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(323, 480);
            this.panel10.TabIndex = 42;
            // 
            // FormPayment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Enrollment_System.Properties.Resources.BACKGROUNDCOLOR;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(911, 557);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.PBPaymentQr);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormPayment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormPayment";
            ((System.ComponentModel.ISupportInitialize)(this.PBPaymentQr)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ExitButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBProfPayment)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox ExitButton;
        private System.Windows.Forms.PictureBox PBPaymentQr;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button BtnUpload;
        private System.Windows.Forms.Button BtnConfirm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox PBProfPayment;
        private System.Windows.Forms.Panel panel10;
    }
}