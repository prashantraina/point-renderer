namespace PointRenderer
{
    partial class MainForm
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
            this.viewportControl = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // viewportControl
            // 
            this.viewportControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.viewportControl.Location = new System.Drawing.Point(12, 84);
            this.viewportControl.Name = "viewportControl";
            this.viewportControl.Size = new System.Drawing.Size(926, 603);
            this.viewportControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 699);
            this.Controls.Add(this.viewportControl);
            this.Name = "MainForm";
            this.Text = "Point Renderer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel viewportControl;
    }
}

