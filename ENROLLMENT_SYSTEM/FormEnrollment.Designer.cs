namespace Enrollment_System
{
    partial class FormEnrollment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel5 = new System.Windows.Forms.Panel();
            this.BtnExit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.BtnLogout = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.LblWelcome = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.BtnDataBase = new System.Windows.Forms.Button();
            this.BtnCourses = new System.Windows.Forms.Button();
            this.BtnEnrollment = new System.Windows.Forms.Button();
            this.BtnHome = new System.Windows.Forms.Button();
            this.BtnPI = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.DataGridEnrollment = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnAddNewEnrollment = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.enrollment_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.last_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.first_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.middle_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.course_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.academic_year = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.semester = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.year_level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColOpen = new System.Windows.Forms.DataGridViewImageColumn();
            this.ColClose = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridEnrollment)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.BtnExit);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(1250, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(50, 36);
            this.panel5.TabIndex = 0;
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
            this.BtnExit.Location = new System.Drawing.Point(11, 5);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(32, 27);
            this.BtnExit.TabIndex = 28;
            this.BtnExit.Text = "x";
            this.BtnExit.UseVisualStyleBackColor = false;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(219)))), ((int)(((byte)(13)))));
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1300, 36);
            this.panel1.TabIndex = 11;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.BtnLogout);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1140, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(160, 72);
            this.panel4.TabIndex = 28;
            // 
            // BtnLogout
            // 
            this.BtnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnLogout.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLogout.ForeColor = System.Drawing.Color.White;
            this.BtnLogout.Location = new System.Drawing.Point(70, 25);
            this.BtnLogout.Name = "BtnLogout";
            this.BtnLogout.Size = new System.Drawing.Size(79, 22);
            this.BtnLogout.TabIndex = 5;
            this.BtnLogout.Text = "LOGOUT";
            this.BtnLogout.UseVisualStyleBackColor = true;
            this.BtnLogout.Click += new System.EventHandler(this.BtnLogout_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.LblWelcome);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(204, 72);
            this.panel6.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(21, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 33);
            this.label1.TabIndex = 26;
            this.label1.Text = "Welcome!";
            // 
            // LblWelcome
            // 
            this.LblWelcome.AutoSize = true;
            this.LblWelcome.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblWelcome.ForeColor = System.Drawing.Color.White;
            this.LblWelcome.Location = new System.Drawing.Point(22, 36);
            this.LblWelcome.Name = "LblWelcome";
            this.LblWelcome.Size = new System.Drawing.Size(64, 25);
            this.LblWelcome.TabIndex = 27;
            this.LblWelcome.Text = "Admin";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.panel9);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(204, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(936, 72);
            this.panel7.TabIndex = 30;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.BtnDataBase);
            this.panel9.Controls.Add(this.BtnCourses);
            this.panel9.Controls.Add(this.BtnEnrollment);
            this.panel9.Controls.Add(this.BtnHome);
            this.panel9.Controls.Add(this.BtnPI);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(936, 72);
            this.panel9.TabIndex = 0;
            // 
            // BtnDataBase
            // 
            this.BtnDataBase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnDataBase.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnDataBase.FlatAppearance.BorderSize = 0;
            this.BtnDataBase.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnDataBase.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnDataBase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDataBase.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDataBase.ForeColor = System.Drawing.Color.White;
            this.BtnDataBase.Location = new System.Drawing.Point(789, 18);
            this.BtnDataBase.Name = "BtnDataBase";
            this.BtnDataBase.Size = new System.Drawing.Size(141, 37);
            this.BtnDataBase.TabIndex = 29;
            this.BtnDataBase.Text = "DATABASE INFORMATION";
            this.BtnDataBase.UseVisualStyleBackColor = true;
            this.BtnDataBase.Click += new System.EventHandler(this.BtnDataBase_Click);
            // 
            // BtnCourses
            // 
            this.BtnCourses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCourses.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnCourses.FlatAppearance.BorderSize = 0;
            this.BtnCourses.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnCourses.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnCourses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCourses.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCourses.ForeColor = System.Drawing.Color.White;
            this.BtnCourses.Location = new System.Drawing.Point(365, 18);
            this.BtnCourses.Name = "BtnCourses";
            this.BtnCourses.Size = new System.Drawing.Size(84, 37);
            this.BtnCourses.TabIndex = 3;
            this.BtnCourses.Text = "COURSES";
            this.BtnCourses.UseVisualStyleBackColor = true;
            this.BtnCourses.Click += new System.EventHandler(this.BtnCourses_Click);
            // 
            // BtnEnrollment
            // 
            this.BtnEnrollment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnEnrollment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnEnrollment.FlatAppearance.BorderSize = 0;
            this.BtnEnrollment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnEnrollment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnEnrollment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnEnrollment.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnEnrollment.ForeColor = System.Drawing.Color.White;
            this.BtnEnrollment.Location = new System.Drawing.Point(480, 18);
            this.BtnEnrollment.Name = "BtnEnrollment";
            this.BtnEnrollment.Size = new System.Drawing.Size(92, 37);
            this.BtnEnrollment.TabIndex = 4;
            this.BtnEnrollment.Text = "ENROLLMENT";
            this.BtnEnrollment.UseVisualStyleBackColor = true;
            // 
            // BtnHome
            // 
            this.BtnHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnHome.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnHome.FlatAppearance.BorderSize = 0;
            this.BtnHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnHome.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnHome.ForeColor = System.Drawing.Color.White;
            this.BtnHome.Location = new System.Drawing.Point(256, 18);
            this.BtnHome.Name = "BtnHome";
            this.BtnHome.Size = new System.Drawing.Size(85, 37);
            this.BtnHome.TabIndex = 1;
            this.BtnHome.Text = "HOME";
            this.BtnHome.UseVisualStyleBackColor = true;
            this.BtnHome.Click += new System.EventHandler(this.BtnHome_Click);
            // 
            // BtnPI
            // 
            this.BtnPI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPI.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnPI.FlatAppearance.BorderSize = 0;
            this.BtnPI.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnPI.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnPI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPI.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPI.ForeColor = System.Drawing.Color.White;
            this.BtnPI.Location = new System.Drawing.Point(611, 18);
            this.BtnPI.Name = "BtnPI";
            this.BtnPI.Size = new System.Drawing.Size(141, 37);
            this.BtnPI.TabIndex = 2;
            this.BtnPI.Text = "PERSONAL INFORMATION";
            this.BtnPI.UseVisualStyleBackColor = true;
            this.BtnPI.Click += new System.EventHandler(this.BtnPI_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(44)))), ((int)(((byte)(26)))));
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 36);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1300, 72);
            this.panel3.TabIndex = 12;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 108);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1300, 642);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.DataGridEnrollment);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1292, 604);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Enrollment";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // DataGridEnrollment
            // 
            this.DataGridEnrollment.AllowUserToAddRows = false;
            this.DataGridEnrollment.BackgroundColor = System.Drawing.Color.White;
            this.DataGridEnrollment.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DataGridEnrollment.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridEnrollment.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridEnrollment.ColumnHeadersHeight = 35;
            this.DataGridEnrollment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DataGridEnrollment.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.enrollment_id,
            this.student_no,
            this.last_name,
            this.first_name,
            this.middle_name,
            this.course_name,
            this.academic_year,
            this.semester,
            this.year_level,
            this.status,
            this.ColOpen,
            this.ColClose});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridEnrollment.DefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridEnrollment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridEnrollment.EnableHeadersVisualStyles = false;
            this.DataGridEnrollment.GridColor = System.Drawing.SystemColors.Control;
            this.DataGridEnrollment.Location = new System.Drawing.Point(3, 50);
            this.DataGridEnrollment.Name = "DataGridEnrollment";
            this.DataGridEnrollment.RowHeadersVisible = false;
            this.DataGridEnrollment.Size = new System.Drawing.Size(1286, 551);
            this.DataGridEnrollment.TabIndex = 1;
            this.DataGridEnrollment.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridEnrollment_CellContentClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnAddNewEnrollment);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1286, 47);
            this.panel2.TabIndex = 0;
            // 
            // BtnAddNewEnrollment
            // 
            this.BtnAddNewEnrollment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddNewEnrollment.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAddNewEnrollment.Location = new System.Drawing.Point(5, 9);
            this.BtnAddNewEnrollment.Name = "BtnAddNewEnrollment";
            this.BtnAddNewEnrollment.Size = new System.Drawing.Size(137, 31);
            this.BtnAddNewEnrollment.TabIndex = 1;
            this.BtnAddNewEnrollment.Text = "New Enrollment";
            this.BtnAddNewEnrollment.UseVisualStyleBackColor = true;
            this.BtnAddNewEnrollment.Click += new System.EventHandler(this.BtnAddAcademic_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::Enrollment_System.Properties.Resources.XButton;
            this.pictureBox1.Location = new System.Drawing.Point(1251, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 26);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(1292, 604);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Payment";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::Enrollment_System.Properties.Resources.EditButton;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = global::Enrollment_System.Properties.Resources.RemoveButton;
            this.dataGridViewImageColumn2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            // 
            // enrollment_id
            // 
            this.enrollment_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.enrollment_id.FillWeight = 10F;
            this.enrollment_id.HeaderText = "ID";
            this.enrollment_id.Name = "enrollment_id";
            this.enrollment_id.Width = 53;
            // 
            // student_no
            // 
            this.student_no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.student_no.HeaderText = "Student No.";
            this.student_no.Name = "student_no";
            this.student_no.Width = 131;
            // 
            // last_name
            // 
            this.last_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.last_name.HeaderText = "Last Name";
            this.last_name.Name = "last_name";
            // 
            // first_name
            // 
            this.first_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.first_name.HeaderText = "First Name";
            this.first_name.Name = "first_name";
            // 
            // middle_name
            // 
            this.middle_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.middle_name.HeaderText = "Middle Name";
            this.middle_name.Name = "middle_name";
            this.middle_name.Width = 144;
            // 
            // course_name
            // 
            this.course_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.course_name.HeaderText = "Course";
            this.course_name.Name = "course_name";
            // 
            // academic_year
            // 
            this.academic_year.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.academic_year.HeaderText = "School Year";
            this.academic_year.Name = "academic_year";
            // 
            // semester
            // 
            this.semester.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.semester.HeaderText = "Semester";
            this.semester.Name = "semester";
            this.semester.Width = 117;
            // 
            // year_level
            // 
            this.year_level.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.year_level.HeaderText = "Year Level";
            this.year_level.Name = "year_level";
            this.year_level.Width = 122;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.status.HeaderText = "Status";
            this.status.Name = "status";
            this.status.Width = 89;
            // 
            // ColOpen
            // 
            this.ColOpen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColOpen.HeaderText = "";
            this.ColOpen.Image = global::Enrollment_System.Properties.Resources.EditButton;
            this.ColOpen.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.ColOpen.Name = "ColOpen";
            this.ColOpen.Width = 5;
            // 
            // ColClose
            // 
            this.ColClose.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColClose.HeaderText = "";
            this.ColClose.Image = global::Enrollment_System.Properties.Resources.RemoveButton;
            this.ColClose.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.ColClose.Name = "ColClose";
            this.ColClose.Width = 5;
            // 
            // FormEnrollment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 750);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormEnrollment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormEnrollment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridEnrollment)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button BtnLogout;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblWelcome;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button BtnCourses;
        private System.Windows.Forms.Button BtnEnrollment;
        private System.Windows.Forms.Button BtnHome;
        private System.Windows.Forms.Button BtnPI;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button BtnDataBase;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView DataGridEnrollment;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.Button BtnAddNewEnrollment;
        private System.Windows.Forms.DataGridViewTextBoxColumn enrollment_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn student_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn last_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn first_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn middle_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn course_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn academic_year;
        private System.Windows.Forms.DataGridViewTextBoxColumn semester;
        private System.Windows.Forms.DataGridViewTextBoxColumn year_level;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewImageColumn ColOpen;
        private System.Windows.Forms.DataGridViewImageColumn ColClose;
    }
}