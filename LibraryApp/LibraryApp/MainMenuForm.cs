using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class MainMenuForm : Form
    {
        private Button btnStart;
        private Button btnExit;

        public MainMenuForm()
        {
            InitializeComponent();
            InitializeUI();
            this.WindowState = FormWindowState.Maximized; // Во весь экран
            this.FormBorderStyle = FormBorderStyle.None;   // Убираем рамки
        }

        private void InitializeUI()
        {
            // Настройка формы
            this.Text = "Библиотека";
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.DoubleBuffered = true; // Уменьшает мерцание

            // Градиентный фон
            this.Paint += (s, e) =>
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    this.ClientRectangle,
                    Color.FromArgb(70, 130, 180),  // SteelBlue
                    Color.FromArgb(135, 206, 250),  // LightSkyBlue
                    45f))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            };

            // Кнопка "Начать"
            btnStart = new Button
            {
                Text = "НАЧАТЬ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(76, 175, 80), // Зелёный
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Size = new Size(400, 120),
                Cursor = Cursors.Hand
            };
            btnStart.Click += (s, e) =>
            {
                MapForm mapForm = new MapForm();
                Hide();
                mapForm.ShowDialog();
                Show();
            };
            /*btnStart.Click += (s, e) =>
            {
                FinalTestForm mapForm = new FinalTestForm();
                Hide();
                mapForm.ShowDialog();
                Show();
            };*/
            AnimateButton(btnStart);

            // Кнопка "Выход"
            btnExit = new Button
            {
                Text = "ВЫХОД",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(244, 67, 54), // Красный
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Size = new Size(400, 120),
                Cursor = Cursors.Hand
            };
            btnExit.Click += (s, e) => Application.Exit();
            AnimateButton(btnExit);

            // Размещаем кнопки
            UpdateButtonPositions();

            // Добавляем кнопки на форму
            this.Controls.Add(btnStart);
            this.Controls.Add(btnExit);

            // Обработчик изменения размера окна
            this.Resize += (s, e) => UpdateButtonPositions();
        }

        // Центрируем кнопки при изменении размера
        private void UpdateButtonPositions()
        {
            btnStart.Location = new Point(
                (this.ClientSize.Width - btnStart.Width) / 2,
                this.ClientSize.Height / 2 - btnStart.Height - 20);

            btnExit.Location = new Point(
                (this.ClientSize.Width - btnExit.Width) / 2,
                this.ClientSize.Height / 2 + 20);
        }

        // Анимация кнопок
        private void AnimateButton(Button button)
        {
            // Эффект при наведении
            button.MouseEnter += (s, e) =>
            {
                button.BackColor = ControlPaint.Light(button.BackColor, 0.2f);
                button.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            };

            // Эффект при уходе курсора
            button.MouseLeave += (s, e) =>
            {
                button.BackColor = (button == btnStart)
                    ? Color.FromArgb(76, 175, 80)
                    : Color.FromArgb(244, 67, 54);
                button.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            };

            // Эффект нажатия
            button.MouseDown += (s, e) =>
            {
                button.BackColor = ControlPaint.Dark(button.BackColor, 0.3f);
            };

            button.MouseUp += (s, e) =>
            {
                button.BackColor = ControlPaint.Light(button.BackColor, 0.2f);
            };
        }

        // Закрытие при нажатии Esc
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