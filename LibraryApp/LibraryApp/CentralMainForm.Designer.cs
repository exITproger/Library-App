using System.Windows.Forms;

namespace LibraryApp
{
    partial class CentralMainForm
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

        private void InitializeComponent()
        {
            this.testCenral = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // testCenral
            // 
            this.testCenral.Location = new System.Drawing.Point(73, 61);
            this.testCenral.Name = "testCenral";
            this.testCenral.Size = new System.Drawing.Size(75, 23);
            this.testCenral.TabIndex = 0;
            this.testCenral.Text = "button1";
            this.testCenral.UseVisualStyleBackColor = true;
            this.testCenral.Click += new System.EventHandler(this.testCenral_Click);
            // 
            // CentralMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.testCenral);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CentralMainForm";
            this.Text = "Центральный федеральный округ";
            this.ResumeLayout(false);

        }



        #endregion

        private Button testCenral;
    }
}