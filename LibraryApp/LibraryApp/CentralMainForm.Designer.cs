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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private System.Windows.Forms.PictureBox pictureBoxCentral;
        private System.Windows.Forms.Label labelFacts;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CentralMainForm));
            this.pictureBoxCentral = new System.Windows.Forms.PictureBox();
            this.labelFacts = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCentral)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCentral
            // 
            this.pictureBoxCentral.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxCentral.Image = global::LibraryApp.Properties.Resources.central;
            this.pictureBoxCentral.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxCentral.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBoxCentral.Name = "pictureBoxCentral";
            this.pictureBoxCentral.Size = new System.Drawing.Size(835, 1041);
            this.pictureBoxCentral.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCentral.TabIndex = 0;
            this.pictureBoxCentral.TabStop = false;
            // 
            // labelFacts
            // 
            this.labelFacts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFacts.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.labelFacts.Location = new System.Drawing.Point(835, 0);
            this.labelFacts.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFacts.Name = "labelFacts";
            this.labelFacts.Padding = new System.Windows.Forms.Padding(15, 13, 15, 13);
            this.labelFacts.Size = new System.Drawing.Size(1069, 1041);
            this.labelFacts.TabIndex = 1;
            this.labelFacts.Text = resources.GetString("labelFacts.Text");
            // 
            // CentralMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.labelFacts);
            this.Controls.Add(this.pictureBoxCentral);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "CentralMainForm";
            this.Text = "Центральный федеральный округ";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCentral)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion
    }
}