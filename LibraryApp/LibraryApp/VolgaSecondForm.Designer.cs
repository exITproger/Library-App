namespace LibraryApp
{
    partial class VolgaSecondForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VolgaSecondForm));
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelText = new System.Windows.Forms.Panel();
            this.lblWomanClothes = new System.Windows.Forms.Label();
            this.lblManClothes = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelImages = new System.Windows.Forms.Panel();
            this.picMan = new System.Windows.Forms.PictureBox();
            this.picWoman = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panelMain.SuspendLayout();
            this.panelText.SuspendLayout();
            this.panelImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWoman)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelText);
            this.panelMain.Controls.Add(this.panelImages);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1904, 1041);
            this.panelMain.TabIndex = 0;
            // 
            // panelText
            // 
            this.panelText.Controls.Add(this.button2);
            this.panelText.Controls.Add(this.button1);
            this.panelText.Controls.Add(this.lblWomanClothes);
            this.panelText.Controls.Add(this.lblManClothes);
            this.panelText.Controls.Add(this.lblTitle);
            this.panelText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelText.Location = new System.Drawing.Point(800, 0);
            this.panelText.Name = "panelText";
            this.panelText.Padding = new System.Windows.Forms.Padding(20);
            this.panelText.Size = new System.Drawing.Size(1104, 1041);
            this.panelText.TabIndex = 1;
            // 
            // lblWomanClothes
            // 
            this.lblWomanClothes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblWomanClothes.Location = new System.Drawing.Point(20, 550);
            this.lblWomanClothes.Name = "lblWomanClothes";
            this.lblWomanClothes.Size = new System.Drawing.Size(1080, 400);
            this.lblWomanClothes.TabIndex = 2;
            this.lblWomanClothes.Text = resources.GetString("lblWomanClothes.Text");
            // 
            // lblManClothes
            // 
            this.lblManClothes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblManClothes.Location = new System.Drawing.Point(20, 100);
            this.lblManClothes.Name = "lblManClothes";
            this.lblManClothes.Size = new System.Drawing.Size(1080, 400);
            this.lblManClothes.TabIndex = 1;
            this.lblManClothes.Text = resources.GetString("lblManClothes.Text");
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(411, 31);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Приволжский округ - Одежда";
            // 
            // panelImages
            // 
            this.panelImages.Controls.Add(this.picWoman);
            this.panelImages.Controls.Add(this.picMan);
            this.panelImages.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelImages.Location = new System.Drawing.Point(0, 0);
            this.panelImages.Name = "panelImages";
            this.panelImages.Size = new System.Drawing.Size(800, 1041);
            this.panelImages.TabIndex = 0;
            // 
            // picMan
            // 
            this.picMan.Image = global::LibraryApp.Properties.Resources.volga_m;
            this.picMan.Location = new System.Drawing.Point(167, 0);
            this.picMan.Name = "picMan";
            this.picMan.Size = new System.Drawing.Size(403, 512);
            this.picMan.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMan.TabIndex = 0;
            this.picMan.TabStop = false;
            // 
            // picWoman
            // 
            this.picWoman.Image = global::LibraryApp.Properties.Resources.volga_w;
            this.picWoman.Location = new System.Drawing.Point(167, 494);
            this.picWoman.Name = "picWoman";
            this.picWoman.Size = new System.Drawing.Size(403, 512);
            this.picWoman.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picWoman.TabIndex = 1;
            this.picWoman.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(997, 988);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 50);
            this.button1.TabIndex = 8;
            this.button1.Text = "К тесту";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(891, 988);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 50);
            this.button2.TabIndex = 9;
            this.button2.Text = "Назад";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // VolgaSecondForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VolgaSecondForm";
            this.Text = "Одежда Приволжского округа";
            this.panelMain.ResumeLayout(false);
            this.panelText.ResumeLayout(false);
            this.panelText.PerformLayout();
            this.panelImages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWoman)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelImages;
        private System.Windows.Forms.Panel panelText;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblManClothes;
        private System.Windows.Forms.Label lblWomanClothes;
        private System.Windows.Forms.PictureBox picWoman;
        private System.Windows.Forms.PictureBox picMan;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}