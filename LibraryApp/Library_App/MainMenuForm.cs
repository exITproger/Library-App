using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Library_App
{
    public partial class MainMenuForm : Form
    {
        private PictureBox btnStart; // Изменено на PictureBox для лучшего контроля изображения
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
            /*
            // Градиентный фон
            this.Paint += (s, e) =>
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(70, 130, 180),
                    Color.FromArgb(135, 206, 250),
                    45f))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            };
            */
            this.Paint += (s, e) =>
            {
                // Градиент от темно-синего к синему (но темнее кнопок)
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(10, 40, 80),     // Глубокий темно-синий
                    Color.FromArgb(30, 70, 120),    // Средний синий (но темнее кнопок)
                    45f))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }

                // Легкая текстура для глубины (не перегружает фон)
                using (var texture = new HatchBrush(
                    System.Drawing.Drawing2D.HatchStyle.Percent10,
                    Color.FromArgb(20, Color.Black),
                    Color.Transparent))
                {
                    e.Graphics.FillRectangle(texture, this.ClientRectangle);
                }

                // Дополнительное затемнение по краям для эффекта "углубления"
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path.AddRectangle(this.ClientRectangle);
                    using (var pgb = new System.Drawing.Drawing2D.PathGradientBrush(path))
                    {
                        pgb.CenterColor = Color.FromArgb(0, 0, 0, 0);
                        pgb.SurroundColors = new Color[] { Color.FromArgb(40, 0, 0, 0) };
                        e.Graphics.FillRectangle(pgb, this.ClientRectangle);
                    }
                }
            };
            // Рассчитываем начальный размер кнопок
            CalculateButtonSize();

            // Кнопка "Начать"
            btnStart = CreateImageButton(Properties.Resources.Начать);
            btnStart.Click += (s, e) =>
            {
                MapForm mapForm = new MapForm();

                mapForm.ShowDialog();
            };

            // Кнопка "Выход"
            btnExit = CreateImageButton(Properties.Resources.выход);
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
            };
        }

        private PictureBox CreateImageButton(Image image)
        {
            var button = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom, // Ключевое изменение - Zoom вместо Stretch
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