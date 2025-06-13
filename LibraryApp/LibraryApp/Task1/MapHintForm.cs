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
