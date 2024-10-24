namespace Proj.VVL.View.Kiwoom
{
    partial class OcxContainerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OcxContainerForm));
            axkhOpenapi = new AxKHOpenAPILib.AxKHOpenAPI();
            ((System.ComponentModel.ISupportInitialize)axkhOpenapi).BeginInit();
            SuspendLayout();
            // 
            // axkhOpenapi
            // 
            axkhOpenapi.Dock = DockStyle.Fill;
            axkhOpenapi.Enabled = true;
            axkhOpenapi.Location = new Point(0, 0);
            axkhOpenapi.Margin = new Padding(2, 2, 2, 2);
            axkhOpenapi.Name = "axkhOpenapi";
            axkhOpenapi.OcxState = (AxHost.State)resources.GetObject("axkhOpenapi.OcxState");
            axkhOpenapi.Size = new Size(560, 270);
            axkhOpenapi.TabIndex = 0;
            // 
            // OcxContainerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(560, 270);
            Controls.Add(axkhOpenapi);
            Margin = new Padding(2, 2, 2, 2);
            Name = "OcxContainerForm";
            Text = "OxcContainerForm";
            ((System.ComponentModel.ISupportInitialize)axkhOpenapi).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public AxKHOpenAPILib.AxKHOpenAPI axkhOpenapi;
    }
}