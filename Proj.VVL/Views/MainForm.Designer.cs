namespace Proj.VVL
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            setupToolStripMenuItem = new ToolStripMenuItem();
            loginToolStripMenuItem = new ToolStripMenuItem();
            logoutToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            consoleLogToolStripMenuItem = new ToolStripMenuItem();
            관심종목ToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            Label_LoginStatus = new ToolStripStatusLabel();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            dataGridView_Publishing = new DataGridView();
            panel_Debug = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            ChartViewer = new LiveChartsCore.SkiaSharpView.WinForms.CartesianChart();
            tableLayoutPanel4 = new TableLayoutPanel();
            button12 = new Button();
            button11 = new Button();
            button10 = new Button();
            button9 = new Button();
            button8 = new Button();
            button7 = new Button();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_Publishing).BeginInit();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.Silver;
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { setupToolStripMenuItem, viewToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(8, 3, 0, 3);
            menuStrip1.Size = new Size(1545, 30);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // setupToolStripMenuItem
            // 
            setupToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loginToolStripMenuItem, logoutToolStripMenuItem });
            setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            setupToolStripMenuItem.Size = new Size(62, 24);
            setupToolStripMenuItem.Text = "Setup";
            // 
            // loginToolStripMenuItem
            // 
            loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            loginToolStripMenuItem.Size = new Size(140, 26);
            loginToolStripMenuItem.Text = "Login";
            loginToolStripMenuItem.Click += loginToolStripMenuItem_Click;
            // 
            // logoutToolStripMenuItem
            // 
            logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            logoutToolStripMenuItem.Size = new Size(140, 26);
            logoutToolStripMenuItem.Text = "Logout";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { consoleLogToolStripMenuItem, 관심종목ToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(56, 24);
            viewToolStripMenuItem.Text = "View";
            // 
            // consoleLogToolStripMenuItem
            // 
            consoleLogToolStripMenuItem.Name = "consoleLogToolStripMenuItem";
            consoleLogToolStripMenuItem.Size = new Size(177, 26);
            consoleLogToolStripMenuItem.Text = "Console Log";
            // 
            // 관심종목ToolStripMenuItem
            // 
            관심종목ToolStripMenuItem.Name = "관심종목ToolStripMenuItem";
            관심종목ToolStripMenuItem.Size = new Size(177, 26);
            관심종목ToolStripMenuItem.Text = "관심종목";
            관심종목ToolStripMenuItem.Click += 관심종목ToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { Label_LoginStatus });
            statusStrip1.Location = new Point(0, 807);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 18, 0);
            statusStrip1.Size = new Size(1545, 26);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // Label_LoginStatus
            // 
            Label_LoginStatus.Name = "Label_LoginStatus";
            Label_LoginStatus.Size = new Size(34, 20);
            Label_LoginStatus.Text = "text";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 30);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel1.Size = new Size(1545, 777);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 26F));
            tableLayoutPanel2.Controls.Add(dataGridView_Publishing, 0, 0);
            tableLayoutPanel2.Controls.Add(panel_Debug, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(4, 4);
            tableLayoutPanel2.Margin = new Padding(4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(764, 769);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // dataGridView_Publishing
            // 
            dataGridView_Publishing.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_Publishing.Dock = DockStyle.Fill;
            dataGridView_Publishing.Location = new Point(3, 3);
            dataGridView_Publishing.Name = "dataGridView_Publishing";
            dataGridView_Publishing.RowHeadersWidth = 51;
            dataGridView_Publishing.Size = new Size(758, 378);
            dataGridView_Publishing.TabIndex = 3;
            // 
            // panel_Debug
            // 
            panel_Debug.BorderStyle = BorderStyle.FixedSingle;
            panel_Debug.Dock = DockStyle.Fill;
            panel_Debug.Location = new Point(4, 388);
            panel_Debug.Margin = new Padding(4);
            panel_Debug.Name = "panel_Debug";
            panel_Debug.Size = new Size(756, 377);
            panel_Debug.TabIndex = 4;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(ChartViewer, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(776, 4);
            tableLayoutPanel3.Margin = new Padding(4);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 93.00184F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 6.99815845F));
            tableLayoutPanel3.Size = new Size(765, 769);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // ChartViewer
            // 
            ChartViewer.BorderStyle = BorderStyle.FixedSingle;
            ChartViewer.Dock = DockStyle.Fill;
            ChartViewer.Location = new Point(3, 3);
            ChartViewer.Name = "ChartViewer";
            ChartViewer.Size = new Size(759, 709);
            ChartViewer.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 12;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.333333F));
            tableLayoutPanel4.Controls.Add(button12, 11, 0);
            tableLayoutPanel4.Controls.Add(button11, 10, 0);
            tableLayoutPanel4.Controls.Add(button10, 9, 0);
            tableLayoutPanel4.Controls.Add(button9, 8, 0);
            tableLayoutPanel4.Controls.Add(button8, 7, 0);
            tableLayoutPanel4.Controls.Add(button7, 6, 0);
            tableLayoutPanel4.Controls.Add(button6, 5, 0);
            tableLayoutPanel4.Controls.Add(button5, 4, 0);
            tableLayoutPanel4.Controls.Add(button4, 3, 0);
            tableLayoutPanel4.Controls.Add(button3, 2, 0);
            tableLayoutPanel4.Controls.Add(button2, 1, 0);
            tableLayoutPanel4.Controls.Add(button1, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(4, 719);
            tableLayoutPanel4.Margin = new Padding(4);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(757, 46);
            tableLayoutPanel4.TabIndex = 3;
            // 
            // button12
            // 
            button12.Dock = DockStyle.Fill;
            button12.Location = new Point(697, 4);
            button12.Margin = new Padding(4);
            button12.Name = "button12";
            button12.Size = new Size(56, 38);
            button12.TabIndex = 11;
            button12.Text = "button12";
            button12.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            button11.Dock = DockStyle.Fill;
            button11.Location = new Point(634, 4);
            button11.Margin = new Padding(4);
            button11.Name = "button11";
            button11.Size = new Size(55, 38);
            button11.TabIndex = 10;
            button11.Text = "button11";
            button11.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            button10.Dock = DockStyle.Fill;
            button10.Location = new Point(571, 4);
            button10.Margin = new Padding(4);
            button10.Name = "button10";
            button10.Size = new Size(55, 38);
            button10.TabIndex = 9;
            button10.Text = "button10";
            button10.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            button9.Dock = DockStyle.Fill;
            button9.Location = new Point(508, 4);
            button9.Margin = new Padding(4);
            button9.Name = "button9";
            button9.Size = new Size(55, 38);
            button9.TabIndex = 8;
            button9.Text = "button9";
            button9.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            button8.Dock = DockStyle.Fill;
            button8.Location = new Point(445, 4);
            button8.Margin = new Padding(4);
            button8.Name = "button8";
            button8.Size = new Size(55, 38);
            button8.TabIndex = 7;
            button8.Text = "button8";
            button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Dock = DockStyle.Fill;
            button7.Location = new Point(382, 4);
            button7.Margin = new Padding(4);
            button7.Name = "button7";
            button7.Size = new Size(55, 38);
            button7.TabIndex = 6;
            button7.Text = "button7";
            button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Dock = DockStyle.Fill;
            button6.Location = new Point(319, 4);
            button6.Margin = new Padding(4);
            button6.Name = "button6";
            button6.Size = new Size(55, 38);
            button6.TabIndex = 5;
            button6.Text = "1m";
            button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Dock = DockStyle.Fill;
            button5.Location = new Point(256, 4);
            button5.Margin = new Padding(4);
            button5.Name = "button5";
            button5.Size = new Size(55, 38);
            button5.TabIndex = 4;
            button5.Text = "5m";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Dock = DockStyle.Fill;
            button4.Location = new Point(193, 4);
            button4.Margin = new Padding(4);
            button4.Name = "button4";
            button4.Size = new Size(55, 38);
            button4.TabIndex = 3;
            button4.Text = "10m";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Location = new Point(130, 4);
            button3.Margin = new Padding(4);
            button3.Name = "button3";
            button3.Size = new Size(55, 38);
            button3.TabIndex = 2;
            button3.Text = "1H";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.Location = new Point(67, 4);
            button2.Margin = new Padding(4);
            button2.Name = "button2";
            button2.Size = new Size(55, 38);
            button2.TabIndex = 1;
            button2.Text = "1D";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(4, 4);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(55, 38);
            button1.TabIndex = 0;
            button1.Text = "1M";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1545, 833);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_Publishing).EndInit();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem setupToolStripMenuItem;
        private ToolStripMenuItem loginToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem consoleLogToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel Label_LoginStatus;
        private ToolStripMenuItem logoutToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private ToolStripMenuItem 관심종목ToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel2;
        private DataGridView dataGridView_Publishing;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel_Debug;
        private LiveChartsCore.SkiaSharpView.WinForms.CartesianChart ChartViewer;
        private TableLayoutPanel tableLayoutPanel4;
        private Button button12;
        private Button button11;
        private Button button10;
        private Button button9;
        private Button button8;
        private Button button7;
        private Button button6;
        private Button button5;
        private Button button4;
        private Button button3;
        private Button button2;
    }
}
