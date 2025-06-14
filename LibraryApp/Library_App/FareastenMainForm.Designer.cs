namespace Library_App
{
    partial class FareastenMainForm
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
            this.btnOpenTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenTest
            // 
            this.btnOpenTest.Location = new System.Drawing.Point(655, 184);
            this.btnOpenTest.Name = "btnOpenTest";
            this.btnOpenTest.Size = new System.Drawing.Size(75, 23);
            this.btnOpenTest.TabIndex = 0;
            this.btnOpenTest.Text = "button1";
            this.btnOpenTest.UseVisualStyleBackColor = true;
            this.btnOpenTest.Click += new System.EventHandler(this.btnOpenTest_Click);
            // 
            // FareastenMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.btnOpenTest);
            this.Name = "FareastenMainForm";
            this.Text = "FareastenMainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpenTest;
    }
}