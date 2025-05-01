namespace Enrollment_System
{
    partial class StudentHistory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ExitButton = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.PicBoxID = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.DataGridHistory = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.no_enrollment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.history_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.academic_year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.semester = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.year_level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.previous_section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.current_section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.effective_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.changed_by = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColOpen1 = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ExitButton)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxID)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.BackColor = System.Drawing.Color.Transparent;
            this.ExitButton.Image = global::Enrollment_System.Properties.Resources.XButton;
            this.ExitButton.Location = new System.Drawing.Point(1076, 7);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(25, 26);
            this.ExitButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ExitButton.TabIndex = 1;
            this.ExitButton.TabStop = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(44)))), ((int)(((byte)(26)))));
            this.panel1.BackgroundImage = global::Enrollment_System.Properties.Resources.BACKGROUNDCOLOR;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ExitButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1111, 40);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 19);
            this.label1.TabIndex = 65;
            this.label1.Text = "Student Enrollment History";
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(44)))), ((int)(((byte)(26)))));
            this.panel10.BackgroundImage = global::Enrollment_System.Properties.Resources.BACKGROUNDCOLOR;
            this.panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel10.Controls.Add(this.PicBoxID);
            this.panel10.Location = new System.Drawing.Point(23, 63);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(228, 357);
            this.panel10.TabIndex = 32;
            // 
            // PicBoxID
            // 
            this.PicBoxID.BackColor = System.Drawing.Color.White;
            this.PicBoxID.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PicBoxID.Location = new System.Drawing.Point(15, 15);
            this.PicBoxID.Name = "PicBoxID";
            this.PicBoxID.Size = new System.Drawing.Size(199, 326);
            this.PicBoxID.TabIndex = 31;
            this.PicBoxID.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.DataGridHistory);
            this.panel2.Location = new System.Drawing.Point(284, 128);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(791, 292);
            this.panel2.TabIndex = 33;
            // 
            // DataGridHistory
            // 
            this.DataGridHistory.AllowUserToAddRows = false;
            this.DataGridHistory.AllowUserToResizeColumns = false;
            this.DataGridHistory.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            this.DataGridHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridHistory.BackgroundColor = System.Drawing.Color.White;
            this.DataGridHistory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DataGridHistory.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DataGridHistory.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DataGridHistory.ColumnHeadersHeight = 40;
            this.DataGridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DataGridHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.no_enrollment,
            this.history_id,
            this.academic_year,
            this.semester,
            this.year_level,
            this.previous_section,
            this.current_section,
            this.effective_date,
            this.changed_by,
            this.ColOpen1});
            this.DataGridHistory.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridHistory.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridHistory.GridColor = System.Drawing.SystemColors.Control;
            this.DataGridHistory.Location = new System.Drawing.Point(0, 0);
            this.DataGridHistory.Name = "DataGridHistory";
            this.DataGridHistory.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridHistory.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridHistory.RowHeadersVisible = false;
            this.DataGridHistory.RowHeadersWidth = 50;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Gray;
            this.DataGridHistory.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.DataGridHistory.RowTemplate.Height = 30;
            this.DataGridHistory.RowTemplate.ReadOnly = true;
            this.DataGridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridHistory.Size = new System.Drawing.Size(791, 292);
            this.DataGridHistory.TabIndex = 2;
            this.DataGridHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridHistory_CellContentClick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(274, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 58);
            this.label3.TabIndex = 64;
            this.label3.Text = "History";
            // 
            // no_enrollment
            // 
            this.no_enrollment.Frozen = true;
            this.no_enrollment.HeaderText = "No.";
            this.no_enrollment.Name = "no_enrollment";
            this.no_enrollment.ReadOnly = true;
            this.no_enrollment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.no_enrollment.Width = 50;
            // 
            // history_id
            // 
            this.history_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.history_id.FillWeight = 10F;
            this.history_id.Frozen = true;
            this.history_id.HeaderText = "ID";
            this.history_id.Name = "history_id";
            this.history_id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.history_id.Visible = false;
            this.history_id.Width = 53;
            // 
            // academic_year
            // 
            this.academic_year.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.academic_year.HeaderText = "School Year";
            this.academic_year.Name = "academic_year";
            this.academic_year.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.academic_year.Width = 133;
            // 
            // semester
            // 
            this.semester.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.semester.HeaderText = "Semester";
            this.semester.Name = "semester";
            this.semester.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.semester.Width = 117;
            // 
            // year_level
            // 
            this.year_level.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.year_level.HeaderText = "Year Level";
            this.year_level.Name = "year_level";
            this.year_level.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.year_level.Width = 122;
            // 
            // previous_section
            // 
            this.previous_section.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.previous_section.FillWeight = 200F;
            this.previous_section.HeaderText = "Previous Section";
            this.previous_section.Name = "previous_section";
            this.previous_section.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.previous_section.Width = 174;
            // 
            // current_section
            // 
            this.current_section.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.current_section.HeaderText = "Current Section";
            this.current_section.Name = "current_section";
            this.current_section.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.current_section.Width = 166;
            // 
            // effective_date
            // 
            this.effective_date.HeaderText = "Effective Date";
            this.effective_date.Name = "effective_date";
            // 
            // changed_by
            // 
            this.changed_by.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.changed_by.HeaderText = "Changed By";
            this.changed_by.Name = "changed_by";
            this.changed_by.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.changed_by.Width = 133;
            // 
            // ColOpen1
            // 
            this.ColOpen1.HeaderText = "";
            this.ColOpen1.Image = global::Enrollment_System.Properties.Resources.EditButton;
            this.ColOpen1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.ColOpen1.MinimumWidth = 2;
            this.ColOpen1.Name = "ColOpen1";
            this.ColOpen1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColOpen1.Width = 40;
            // 
            // StudentHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Enrollment_System.Properties.Resources.COLORPDMBACKGROUND11;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1111, 456);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StudentHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StudentHistory";
            this.Load += new System.EventHandler(this.StudentHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ExitButton)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel10.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxID)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ExitButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox PicBoxID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView DataGridHistory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_enrollment;
        private System.Windows.Forms.DataGridViewTextBoxColumn history_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn academic_year;
        private System.Windows.Forms.DataGridViewTextBoxColumn semester;
        private System.Windows.Forms.DataGridViewTextBoxColumn year_level;
        private System.Windows.Forms.DataGridViewTextBoxColumn previous_section;
        private System.Windows.Forms.DataGridViewTextBoxColumn current_section;
        private System.Windows.Forms.DataGridViewTextBoxColumn effective_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn changed_by;
        private System.Windows.Forms.DataGridViewImageColumn ColOpen1;
    }
}