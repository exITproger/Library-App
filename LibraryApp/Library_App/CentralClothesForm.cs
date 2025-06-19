using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Library_App
{
    public partial class CentralClothesForm : Form
    {
        private PictureBox btnBack;
        private PictureBox btnForward;
        private PictureBox btnExit;
        private const int DesignWidth = 1920;
        private const int DesignHeight = 1080;
        private readonly Point btnBackOriginalLocation = new Point(50, 50);
        private readonly Point btnForwardOriginalLocation = new Point(1720, 50);
        private readonly Point btnExitOriginalLocation = new Point(885, 990);
        private readonly Size btnOriginalSize = new Size(150, 150);
        private readonly Size size = new Size(70, 70);

        public CentralClothesForm()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;

            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            try
            {
                this.BackgroundImage = Properties.Resources.CentralClother;
                this.BackgroundImageLayout = ImageLayout.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки фонового изображения:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.BackColor = Color.Black;
            }

            InitializeButtons();
            LoadButtonImages();

            this.Load += Form_Load;
            this.Resize += Form_Resize;
        }

        private void InitializeButtons()
        {
            btnBack = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = btnOriginalSize,
                Location = btnBackOriginalLocation,
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.None,
                Cursor = Cursors.Hand
            };

            btnBack.Click += BtnBack_Click;

            btnForward = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = btnOriginalSize,
                Location = btnForwardOriginalLocation,
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.None,
                Cursor = Cursors.Hand
            };

            btnForward.Click += BtnForward_Click;
            btnExit = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = size,
                Location = btnExitOriginalLocation,
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.None,
                Cursor = Cursors.Hand
            };
            btnExit.Click += BtnExit_Click;
            this.Controls.Add(btnBack);
            this.Controls.Add(btnForward);
            this.Controls.Add(btnExit);
            btnBack.BringToFront();
            btnForward.BringToFront();
            btnExit.BringToFront();
        }
        private void LoadButtonImages()
        {
            try
            {
                btnBack.Image = Properties.Resources.CentralBack;
                btnForward.Image = Properties.Resources.CentralNext;
                btnExit.Image = Properties.Resources.дом_new;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображений кнопок:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateControlsSizeAndPosition()
        {
            if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0) return;

            float scaleX = (float)this.ClientSize.Width / DesignWidth;
            float scaleY = (float)this.ClientSize.Height / DesignHeight;
            float scale = Math.Min(scaleX, scaleY);

            UpdateButtonPositions(scale);
        }

        private void UpdateButtonPositions(float scale)
        {
            UpdateSingleButtonPosition(btnBack, btnBackOriginalLocation, scale);
            UpdateSingleButtonPosition(btnForward, btnForwardOriginalLocation, scale);
            UpdateSingleButtonPosition(btnExit, btnExitOriginalLocation, scale);
        }

        private void UpdateSingleButtonPosition(PictureBox button, Point originalLocation, float scale)
        {
            if (button == null) return;

            int newWidth = (int)(btnOriginalSize.Width * scale);
            int newHeight = 0;
            if (button != btnExit)
            {
                newHeight = (int)(btnOriginalSize.Height * scale);
            }
            else { newHeight = (int)(size.Height * scale); }
            button.Size = new Size(newWidth, newHeight);

            int newX = (int)(originalLocation.X * scale);
            int newY = (int)(originalLocation.Y * scale);
            button.Location = new Point(newX, newY);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            LoadButtonImages();
            UpdateControlsSizeAndPosition();
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            UpdateControlsSizeAndPosition();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            // Действие для кнопки "Назад"
            Close();
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (!(form is MainMenuForm))
                    form.Close();
            }

            var mainMenu = Application.OpenForms.OfType<MainMenuForm>().FirstOrDefault();
            if (mainMenu == null)
            {
                mainMenu = new MainMenuForm();
                mainMenu.Show();
            }
            else
            {
                mainMenu.BringToFront();
            }

            this.Close();
            GC.Collect();
        }
        private void BtnForward_Click(object sender, EventArgs e)
        {
            // Действие для кнопки "Вперед"
            CentralOmensForm centralOmensForm = new CentralOmensForm();
            
            centralOmensForm.ShowDialog();
            
        }
    }
}