namespace LibraryApp
{
    partial class VolgaMainForm
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
            this.btnTextOpen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTextOpen
            // 
            this.btnTextOpen.Location = new System.Drawing.Point(799, 347);
            this.btnTextOpen.Name = "btnTextOpen";
            this.btnTextOpen.Size = new System.Drawing.Size(75, 23);
            this.btnTextOpen.TabIndex = 0;
            this.btnTextOpen.Text = "button1";
            this.btnTextOpen.UseVisualStyleBackColor = true;
            this.btnTextOpen.Click += new System.EventHandler(this.btnTextOpen_Click);
            // 
            // VolgaMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.btnTextOpen);
            this.Name = "VolgaMainForm";
            this.Text = "VolgaMainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTextOpen;
    }
}