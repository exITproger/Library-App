using System;
using System.Drawing;
using System.Windows.Forms;

namespace Library_App
{
    public partial class MainMenuForm : Form
    {
        private PictureBox btnStart;
        private PictureBox btnExit;
        private Size originalImageSize = new Size(1509, 520);
        private float buttonWidthRatio = 800f / 2560f;
        private Size currentButtonSize;

        public MainMenuForm()
        {
            InitializeComponent();
            InitializeUI();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void InitializeUI()
        {
            // Устанавливаем фон формы
            this.BackgroundImage = Properties.Resources.aaa;
            this.BackgroundImageLayout = ImageLayout.Zoom; // Сохраняем пропорции

            // Рассчитываем начальный размер кнопок
            CalculateButtonSize();

            // Кнопка "Начать"
            btnStart = CreateImageButton(Properties.Resources.Начать_new);
            btnStart.Click += (s, e) =>
            {
                MapForm mapForm = new MapForm();
                Hide();
                mapForm.ShowDialog();
                Show();
            };

            // Кнопка "Выход"
            btnExit = CreateImageButton(Properties.Resources.Выход_new);
            btnExit.Click += (s, e) => Application.Exit();

            // Размещаем кнопки
            UpdateButtonPositions();

            // Добавляем кнопки на форму
            this.Controls.Add(btnStart);
            this.Controls.Add(btnExit);

            // Обработчик изменения размера окна
            this.Resize += (s, e) =>
            {
                CalculateButtonSize();
                UpdateButtonSizes();
                UpdateButtonPositions();
                this.Invalidate(); // Обновляем фон
            };
        }

        private PictureBox CreateImageButton(Image image)
        {
            var button = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Size = currentButtonSize,
                Cursor = Cursors.Hand
            };

            // Анимация
            button.MouseEnter += (s, e) => ScaleButton(button, 1.05f);
            button.MouseLeave += (s, e) => ScaleButton(button, 1.0f);
            button.MouseDown += (s, e) => ScaleButton(button, 0.95f);
            button.MouseUp += (s, e) => ScaleButton(button, 1.05f);

            return button;
        }

        private void CalculateButtonSize()
        {
            int buttonWidth = (int)(this.ClientSize.Width * buttonWidthRatio);
            int buttonHeight = (int)(buttonWidth * originalImageSize.Height / (float)originalImageSize.Width);
            currentButtonSize = new Size(buttonWidth, buttonHeight);
        }

        private void UpdateButtonSizes()
        {
            if (btnStart != null) btnStart.Size = currentButtonSize;
            if (btnExit != null) btnExit.Size = currentButtonSize;
        }

        private void UpdateButtonPositions()
        {
            if (btnStart == null || btnExit == null) return;

            int verticalSpacing = this.ClientSize.Height / 20;

            btnStart.Location = new Point(
                (this.ClientSize.Width - btnStart.Width) / 2,
                this.ClientSize.Height / 2 - btnStart.Height - verticalSpacing);

            btnExit.Location = new Point(
                (this.ClientSize.Width - btnExit.Width) / 2,
                this.ClientSize.Height / 2 + verticalSpacing);
        }

        private void ScaleButton(PictureBox button, float scale)
        {
            int newWidth = (int)(currentButtonSize.Width * scale);
            int newHeight = (int)(currentButtonSize.Height * scale);

            button.SuspendLayout();
            button.Size = new Size(newWidth, newHeight);
            button.ResumeLayout();

            UpdateButtonPositions();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Application.Exit();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}