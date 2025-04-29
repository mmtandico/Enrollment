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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ExitButton = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.PicBoxID = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DataGridHistory = new System.Windows.Forms.DataGridView();
            this.enrollment_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no_enrollment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.courseCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.academic_year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.semester = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.year_level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColOpen1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ExitButton)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxID)).BeginInit();
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
            // panel2
            // 
            this.panel2.Controls.Add(this.DataGridHistory);
            this.panel2.Location = new System.Drawing.Point(284, 128);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(791, 292);
            this.panel2.TabIndex = 33;
            // 
            // PicBoxID
            // 
            this.PicBoxID.BackColor = System.Drawing.Color.White;
            this.PicBoxID.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PicBoxID.Location = new System.Drawing.Point(15, 15);
            this.PicBoxID.Name = "PicBoxID";
            this.PicBoxID.Size = new System.Drawing.Size(199, 193);
            this.PicBoxID.TabIndex = 31;
            this.PicBoxID.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(274, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 58);
            this.label3.TabIndex = 64;
            this.label3.Text = "History";
            // 
            // DataGridHistory
            // 
            this.DataGridHistory.AllowUserToAddRows = false;
            this.DataGridHistory.AllowUserToResizeColumns = false;
            this.DataGridHistory.AllowUserToResizeRows = false;
            dataGridViewCellStyle31.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle31.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle31.SelectionBackColor = System.Drawing.Color.White;
            this.DataGridHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle31;
            this.DataGridHistory.BackgroundColor = System.Drawing.Color.White;
            this.DataGridHistory.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DataGridHistory.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DataGridHistory.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle32.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle32.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle32.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle32.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle32.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridHistory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle32;
            this.DataGridHistory.ColumnHeadersHeight = 40;
            this.DataGridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DataGridHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.enrollment_id,
            this.no_enrollment,
            this.student_no,
            this.courseCode,
            this.academic_year,
            this.semester,
            this.year_level,
            this.status,
            this.ColOpen1});
            this.DataGridHistory.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle33.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle33.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle33.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle33.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle33.SelectionForeColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle33.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridHistory.DefaultCellStyle = dataGridViewCellStyle33;
            this.DataGridHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridHistory.GridColor = System.Drawing.SystemColors.Control;
            this.DataGridHistory.Location = new System.Drawing.Point(0, 0);
            this.DataGridHistory.Name = "DataGridHistory";
            this.DataGridHistory.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle34.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle34.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle34.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle34.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle34.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle34.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridHistory.RowHeadersDefaultCellStyle = dataGridViewCellStyle34;
            this.DataGridHistory.RowHeadersVisible = false;
            this.DataGridHistory.RowHeadersWidth = 50;
            dataGridViewCellStyle35.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle35.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle35.SelectionForeColor = System.Drawing.Color.Gray;
            this.DataGridHistory.RowsDefaultCellStyle = dataGridViewCellStyle35;
            this.DataGridHistory.RowTemplate.Height = 30;
            this.DataGridHistory.RowTemplate.ReadOnly = true;
            this.DataGridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridHistory.Size = new System.Drawing.Size(791, 292);
            this.DataGridHistory.TabIndex = 2;
            this.DataGridHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridHistory_CellContentClick);
            // 
            // enrollment_id
            // 
            this.enrollment_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.enrollment_id.FillWeight = 10F;
            this.enrollment_id.Frozen = true;
            this.enrollment_id.HeaderText = "ID";
            this.enrollment_id.Name = "enrollment_id";
            this.enrollment_id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.enrollment_id.Visible = false;
            this.enrollment_id.Width = 53;
            // 
            // no_enrollment
            // 
            this.no_enrollment.Frozen = true;
            this.no_enrollment.HeaderText = "No.";
            this.no_enrollment.Name = "no_enrollment";
            this.no_enrollment.Width = 50;
            // 
            // student_no
            // 
            this.student_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.student_no.FillWeight = 200F;
            this.student_no.Frozen = true;
            this.student_no.HeaderText = "Student No.";
            this.student_no.Name = "student_no";
            this.student_no.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.student_no.Width = 131;
            // 
            // courseCode
            // 
            this.courseCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.courseCode.HeaderText = "Course";
            this.courseCode.Name = "courseCode";
            this.courseCode.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.courseCode.Width = 95;
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
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.status.HeaderText = "Status";
            this.status.Name = "status";
            this.status.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.status.Width = 89;
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
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBoxID)).EndInit();
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
        private System.Windows.Forms.DataGridViewTextBoxColumn enrollment_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn no_enrollment;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn courseCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn academic_year;
        private System.Windows.Forms.DataGridViewTextBoxColumn semester;
        private System.Windows.Forms.DataGridViewTextBoxColumn year_level;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewImageColumn ColOpen1;
        private System.Windows.Forms.Label label1;
    }
}