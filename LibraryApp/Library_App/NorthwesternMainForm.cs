using System;
using System.Drawing;
using System.Windows.Forms;

namespace Library_App
{
    public partial class NorthwesternMainForm : Form
    {
        private PictureBox pictureBox;

        public NorthwesternMainForm()
        {
            InitializeComponent();

            // Устанавливаем форму в полноэкранный режим
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            InitializePictureBox();
            LoadImageFromResources();
            this.Load += Form_Load;
        }

        private void InitializePictureBox()
        {
            pictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.None,
                Anchor = AnchorStyles.None,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pictureBox);
            pictureBox.BringToFront();
        }

        private void LoadImageFromResources()
        {
            try
            {
                var image = Properties.Resources.komi_main;
                if (image != null)
                {
                    pictureBox.Image = image;
                }
                else
                {
                    MessageBox.Show("Изображение не найдено в ресурсах", "Ошибка",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения:\n{ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateImageSizeAndPosition()
        {
            if (pictureBox?.Image == null || this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0)
                return;

            try
            {
                float aspectRatio = (float)pictureBox.Image.Height / pictureBox.Image.Width;
                int newWidth = this.ClientSize.Width;
                int newHeight = (int)(newWidth * aspectRatio);

                if (newHeight > this.ClientSize.Height)
                {
                    newHeight = this.ClientSize.Height;
                    newWidth = (int)(newHeight / aspectRatio);
                }

                pictureBox.Size = new Size(newWidth, newHeight);
                pictureBox.Left = (this.ClientSize.Width - newWidth) / 2;
                pictureBox.Top = (this.ClientSize.Height - newHeight) / 2;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления размеров: {ex.Message}");
            }
        }

        private void Form_Load(object sender, EventArgs e)
        {
            UpdateImageSizeAndPosition();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateImageSizeAndPosition();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateImageSizeAndPosition();
        }
    }
}