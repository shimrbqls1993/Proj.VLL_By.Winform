namespace Proj.VVL.Views.Kiwoom.View
{
    partial class RecommandTickerForm
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
            tableLayoutPanel1 = new TableLayoutPanel();
            dataGridView1 = new DataGridView();
            tableLayoutPanel2 = new TableLayoutPanel();
            textBox_Code = new TextBox();
            label2 = new Label();
            label1 = new Label();
            button_Save = new Button();
            button_Delete = new Button();
            textBox_Name = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(dataGridView1, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Size = new Size(800, 451);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(794, 354);
            dataGridView1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5944586F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 67.25441F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.Controls.Add(textBox_Code, 1, 1);
            tableLayoutPanel2.Controls.Add(label2, 0, 1);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(button_Save, 2, 0);
            tableLayoutPanel2.Controls.Add(button_Delete, 2, 1);
            tableLayoutPanel2.Controls.Add(textBox_Name, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 363);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(794, 85);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // textBox_Code
            // 
            textBox_Code.Font = new Font("맑은 고딕", 13.8F);
            textBox_Code.Location = new Point(105, 47);
            textBox_Code.Margin = new Padding(5);
            textBox_Code.Name = "textBox_Code";
            textBox_Code.Size = new Size(523, 38);
            textBox_Code.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("맑은 고딕", 13.8F, FontStyle.Bold);
            label2.Location = new Point(5, 47);
            label2.Margin = new Padding(5);
            label2.Name = "label2";
            label2.Size = new Size(70, 31);
            label2.TabIndex = 1;
            label2.Text = "Code";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("맑은 고딕", 13.8F, FontStyle.Bold);
            label1.Location = new Point(5, 5);
            label1.Margin = new Padding(5);
            label1.Name = "label1";
            label1.Size = new Size(78, 31);
            label1.TabIndex = 0;
            label1.Text = "Name";
            // 
            // button_Save
            // 
            button_Save.Location = new Point(639, 5);
            button_Save.Margin = new Padding(5);
            button_Save.Name = "button_Save";
            button_Save.Size = new Size(150, 32);
            button_Save.TabIndex = 2;
            button_Save.Text = "Save";
            button_Save.UseVisualStyleBackColor = true;
            // 
            // button_Delete
            // 
            button_Delete.Location = new Point(639, 47);
            button_Delete.Margin = new Padding(5);
            button_Delete.Name = "button_Delete";
            button_Delete.Size = new Size(150, 32);
            button_Delete.TabIndex = 3;
            button_Delete.Text = "Delete";
            button_Delete.UseVisualStyleBackColor = true;
            button_Delete.Click += button_Delete_Click;
            // 
            // textBox_Name
            // 
            textBox_Name.Font = new Font("맑은 고딕", 13.8F);
            textBox_Name.Location = new Point(105, 5);
            textBox_Name.Margin = new Padding(5);
            textBox_Name.Name = "textBox_Name";
            textBox_Name.Size = new Size(523, 38);
            textBox_Name.TabIndex = 4;
            // 
            // RecommandTickerForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 451);
            Controls.Add(tableLayoutPanel1);
            Name = "RecommandTickerForm";
            Text = "RecommandTickerForm";
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label2;
        private Label label1;
        private Button button_Save;
        private Button button_Delete;
        private TextBox textBox_Code;
        private TextBox textBox_Name;
    }
}