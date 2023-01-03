namespace anosono
{
    partial class Form3
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1__1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1__2 = new System.Windows.Forms.Button();
            this.textBox1__2 = new System.Windows.Forms.TextBox();
            this.textBox1__1 = new System.Windows.Forms.TextBox();
            this.button1__1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox2__1 = new System.Windows.Forms.TextBox();
            this.button2__1 = new System.Windows.Forms.Button();
            this.button0__1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1__1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1__1);
            this.groupBox1.Controls.Add(this.button1__2);
            this.groupBox1.Controls.Add(this.textBox1__2);
            this.groupBox1.Controls.Add(this.textBox1__1);
            this.groupBox1.Controls.Add(this.button1__1);
            this.groupBox1.Location = new System.Drawing.Point(10, 3);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(593, 209);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "新しいプロジェクト";
            // 
            // panel1__1
            // 
            this.panel1__1.AllowDrop = true;
            this.panel1__1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1__1.Controls.Add(this.label1);
            this.panel1__1.Location = new System.Drawing.Point(161, 116);
            this.panel1__1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1__1.Name = "panel1__1";
            this.panel1__1.Size = new System.Drawing.Size(417, 94);
            this.panel1__1.TabIndex = 4;
            this.panel1__1.Visible = false;
            this.panel1__1.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel1_DragDrop);
            this.panel1__1.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel1_DragEnter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(126, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "ここにファイルをドロップ";
            // 
            // button1__2
            // 
            this.button1__2.Location = new System.Drawing.Point(381, 74);
            this.button1__2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1__2.Name = "button1__2";
            this.button1__2.Size = new System.Drawing.Size(203, 38);
            this.button1__2.TabIndex = 3;
            this.button1__2.Text = "新しいプロジェクトフォルダを作成";
            this.button1__2.UseVisualStyleBackColor = true;
            this.button1__2.Visible = false;
            this.button1__2.Click += new System.EventHandler(this.button1__2_Click);
            // 
            // textBox1__2
            // 
            this.textBox1__2.Enabled = false;
            this.textBox1__2.Location = new System.Drawing.Point(225, 83);
            this.textBox1__2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1__2.Name = "textBox1__2";
            this.textBox1__2.Size = new System.Drawing.Size(150, 23);
            this.textBox1__2.TabIndex = 2;
            this.textBox1__2.Text = "YoloXTrainning000";
            this.textBox1__2.Visible = false;
            // 
            // textBox1__1
            // 
            this.textBox1__1.Enabled = false;
            this.textBox1__1.Location = new System.Drawing.Point(120, 43);
            this.textBox1__1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1__1.Name = "textBox1__1";
            this.textBox1__1.Size = new System.Drawing.Size(464, 23);
            this.textBox1__1.TabIndex = 1;
            this.textBox1__1.Visible = false;
            // 
            // button1__1
            // 
            this.button1__1.Location = new System.Drawing.Point(5, 20);
            this.button1__1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1__1.Name = "button1__1";
            this.button1__1.Size = new System.Drawing.Size(99, 58);
            this.button1__1.TabIndex = 0;
            this.button1__1.Text = "プロジェクトフォルダの作成場所を指定";
            this.button1__1.UseVisualStyleBackColor = true;
            this.button1__1.Click += new System.EventHandler(this.button1__1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox2__1);
            this.groupBox2.Controls.Add(this.button2__1);
            this.groupBox2.Location = new System.Drawing.Point(1, 227);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(593, 78);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "既存のプロジェクト";
            // 
            // textBox2__1
            // 
            this.textBox2__1.Location = new System.Drawing.Point(124, 32);
            this.textBox2__1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2__1.Name = "textBox2__1";
            this.textBox2__1.Size = new System.Drawing.Size(464, 23);
            this.textBox2__1.TabIndex = 4;
            this.textBox2__1.Visible = false;
            // 
            // button2__1
            // 
            this.button2__1.Location = new System.Drawing.Point(5, 20);
            this.button2__1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2__1.Name = "button2__1";
            this.button2__1.Size = new System.Drawing.Size(99, 59);
            this.button2__1.TabIndex = 4;
            this.button2__1.Text = "既存のプロジェクトフォルダを指定";
            this.button2__1.UseVisualStyleBackColor = true;
            this.button2__1.Click += new System.EventHandler(this.button2__1_Click);
            // 
            // button0__1
            // 
            this.button0__1.Location = new System.Drawing.Point(317, 309);
            this.button0__1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button0__1.Name = "button0__1";
            this.button0__1.Size = new System.Drawing.Size(287, 38);
            this.button0__1.TabIndex = 4;
            this.button0__1.Text = "決定して開始";
            this.button0__1.UseVisualStyleBackColor = true;
            this.button0__1.Visible = false;
            this.button0__1.Click += new System.EventHandler(this.button0__1_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 364);
            this.Controls.Add(this.button0__1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form3";
            this.Text = "Form3";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1__1.ResumeLayout(false);
            this.panel1__1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button button1__1;
        private TextBox textBox1__1;
        private TextBox textBox1__2;
        private Button button1__2;
        private Button button0__1;
        private TextBox textBox2__1;
        private Button button2__1;
        private Panel panel1__1;
        private Label label1;
    }
}