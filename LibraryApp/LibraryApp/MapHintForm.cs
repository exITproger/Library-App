using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class MapHintForm : Form
    {
        private Timer countdownTimer;
        private int secondsRemaining = 10;

        private Label timerLabel;
        private Button backButton;
        private PictureBox mapPictureBox;

        public MapHintForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            /*
            // === Таймер Label ===
            timerLabel = new Label
            {
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.DarkRed,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(timerLabel);

            // === Кнопка Назад ===
            backButton = new Button
            {
                Text = "← Назад",
                Font = new Font("Arial", 30, FontStyle.Bold),
                BackColor = Color.LightGray,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Location = new Point(20, 70)
            };
            backButton.Click += (s, e) => this.Close();
            this.Controls.Add(backButton);
            */
            // Общие настройки стиля
            Font commonFontBold = new Font("Segoe UI", 24, FontStyle.Bold);
            Color labelForeColor = Color.DarkRed;
            Color buttonBackColor = Color.FromArgb(240, 240, 240);
            Color buttonHoverColor = Color.FromArgb(210, 210, 210);
            Color buttonTextColor = Color.FromArgb(30, 30, 30);
            int buttonWidth = 180;
            int buttonHeight = 60;

            // Создаём Label с современным стилем
            timerLabel = new Label
            {
                Font = commonFontBold,
                ForeColor = labelForeColor,
                AutoSize = true,
                Location = new Point(20, 20)
            };
            this.Controls.Add(timerLabel);

            // Функция для создания кнопок с закруглёнными углами и эффектом наведения
            Button CreateStyledButton(string text, Font font, Point location, EventHandler onClick)
            {
                var btn = new Button();
                btn.Text = text;
                btn.Font = font;
                btn.BackColor = buttonBackColor;
                btn.ForeColor = buttonTextColor;
                btn.Size = new Size(buttonWidth, buttonHeight);
                btn.Location = location;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
                btn.AutoSize = false;
                btn.Click += onClick;

                btn.MouseEnter += (s, e) => { btn.BackColor = buttonHoverColor; };
                btn.MouseLeave += (s, e) => { btn.BackColor = buttonBackColor; };

                // Закругляем углы
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                int radius = 12;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
                path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
                path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
                path.CloseAllFigures();
                btn.Region = new Region(path);

                return btn;
            }

            // Создаём кнопку Назад в стиле
            backButton = CreateStyledButton("← Назад", new Font("Segoe UI", 22, FontStyle.Bold), new Point(20, 70), (s, e) => this.Close());
            this.Controls.Add(backButton);

            // === Картинка MapFull ===
            mapPictureBox = new PictureBox
            {
                Image = Properties.Resources.map_full, 
                SizeMode = PictureBoxSizeMode.Zoom,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(0, 130),
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height - 130)
            };
            this.Controls.Add(mapPictureBox);

            // Обработка изменения размера
            this.Resize += (s, e) =>
            {
                mapPictureBox.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - 130);
            };

            // === Таймер ===
            countdownTimer = new Timer();
            countdownTimer.Interval = 1000; // 1 секунда
            countdownTimer.Tick += CountdownTimer_Tick;
            countdownTimer.Start();

            UpdateTimerLabel();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            secondsRemaining--;
            UpdateTimerLabel();

            if (secondsRemaining <= 0)
            {
                countdownTimer.Stop();
                this.Close();
            }
        }

        private void UpdateTimerLabel()
        {
            timerLabel.Text = $"Подсказка исчезнет через: {secondsRemaining} сек";
        }
    }
}
