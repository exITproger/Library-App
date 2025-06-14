using System;
using System.Drawing;
using System.Windows.Forms;

namespace Library_App
{
    public partial class CentralOmensForm : Form
    {
        private Size baseFormSize = new Size(1000, 700);
        private float baseFontSize = 14f;

        public CentralOmensForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = Screen.PrimaryScreen.Bounds;

            InitializeCustomUI();
            this.Resize += CentralOmensForm_Resize;
        }

        private void InitializeCustomUI()
        {
            this.Text = "Центральный округ — Русские приметы";
            this.BackgroundImage = Properties.Resources.CentralBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // --- Заголовок ---
            Label title = new Label
            {
                Text = "Центральный округ\nРусские приметы",
                Font = new Font("Comic Sans MS", 36f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(title);

            // Обновляем позицию заголовка после добавления
            title.Location = new Point((this.ClientSize.Width - title.Width) / 2, 50);

            // --- Примета 1 (левая сторона) ---
            PictureBox swallowPictureBox = new PictureBox
            {
                Image = Properties.Resources.Swallow,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Size = new Size(400, 400),
                Location = new Point(250, 400)
            };
            this.Controls.Add(swallowPictureBox);

            Label superstition1 = new Label
            {
                Text = "1. Если ласточка летает низко\n" +
                       "— это к скорому теплу, потому что\n" +
                       "ласточки чувствуют перемену погоды.",
                Font = new Font("Comic Sans MS", 16f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(50, 850)
            };
            this.Controls.Add(superstition1);

            // --- Примета 2 (центр) ---
            PictureBox spiderPictureBox = new PictureBox
            {
                Image = Properties.Resources.Spider,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(350, 600),
                Location = new Point(this.Width / 2 - 120, 250)
            };
            this.Controls.Add(spiderPictureBox);

            Label superstition2 = new Label
            {
                Text = "2. Если в доме паук сплетет\n" +
                       "паутину в углу — это к деньгам\n" +
                       "и благополучию в доме.",
                Font = new Font("Comic Sans MS", 16f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(this.Width / 2 - 350, 850)
            };
            this.Controls.Add(superstition2);

            // --- Примета 3 (правая сторона) ---
            PictureBox mirrorPictureBox = new PictureBox
            {
                Image = Properties.Resources.Mirror,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(400, 400),
                Location = new Point(this.Width - 650, 400)
            };
            this.Controls.Add(mirrorPictureBox);

            Label superstition3 = new Label
            {
                Text = "3. Разбить зеркало — плохая примета, \n" +
                       "потому что считается, что это \n" +
                       "может принести семь лет несчастья.",
                Font = new Font("Comic Sans MS", 16f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(this.Width - 1000, 850)
            };
            this.Controls.Add(superstition3);

            // --- Кнопка закрытия слева сверху ---
            // --- Кнопка закрытия слева сверху ---
            PictureBox closePictureBox = new PictureBox
            {
                Name = "closePictureBox",
                Image = Properties.Resources.BackButton,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(250, 250),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent,
                Location = new Point(7, 7)
            };
            closePictureBox.Click += (sender, e) => this.Close();
            this.Controls.Add(closePictureBox);

            // --- Кнопка "Далее" справа сверху ---
            PictureBox nextPictureBox = new PictureBox
            {
                Name = "nextPictureBox",
                Image = Properties.Resources.NextButton,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(250, 250),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent,
                Location = new Point(this.ClientSize.Width - 260, 7)
            };
            nextPictureBox.Click += (sender, e) =>
            {
                CentralTraditionsForm regionMapForm = new CentralTraditionsForm();
                this.Hide();
                regionMapForm.ShowDialog();
                this.Show();
            };
            this.Controls.Add(nextPictureBox);

            // Выводим кнопки поверх всех элементов
            closePictureBox.BringToFront();
            nextPictureBox.BringToFront();
        }

        private void CentralOmensForm_Resize(object sender, EventArgs e)
        {
            // Вычисляем масштаб один раз
            float scaleX = (float)this.Width / baseFormSize.Width;
            float scaleY = (float)this.Height / baseFormSize.Height;
            float scale = Math.Min(scaleX, scaleY);

            foreach (Control control in this.Controls)
            {
                if (control is Label lbl)
                {
                    // Рассчитываем новый размер шрифта
                    float newFontSize = Math.Max(10, Math.Min(baseFontSize * scale, 48));

                    // Сохраняем стиль шрифта
                    FontStyle currentStyle = lbl.Font.Style;

                    // Меняем размер шрифта
                    lbl.Font = new Font(
                        lbl.Font.FontFamily,
                        newFontSize,
                        currentStyle
                    );
                }
            }
        }
    }
}