namespace SoftPhone
{
    partial class Form1
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
            this.lb_log = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lb_log
            // 
            this.lb_log.FormattingEnabled = true;
            this.lb_log.Location = new System.Drawing.Point(12, 12);
            this.lb_log.Name = "lb_log";
            this.lb_log.Size = new System.Drawing.Size(311, 576);
            this.lb_log.TabIndex = 0;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(711, 608);
            this.Controls.Add(this.lb_log);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox lb_log;
    }
}

