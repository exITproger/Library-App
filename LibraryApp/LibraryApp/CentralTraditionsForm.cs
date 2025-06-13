using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class CentralTraditionsForm : Form
    {
        private Label titleLabel; // Заголовок
        private Label descriptionLabel; // Описание

        private Size baseFormSize = new Size(1000, 700);
        private float baseFontSize = 14f;

        private PictureBox closePictureBox; // Кнопка "Назад"
        private PictureBox nextPictureBox; // Кнопка "Вперед"
        private List<PictureBox> imageBoxes = new List<PictureBox>(); // Список для дополнительных изображений

        public CentralTraditionsForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            // Настройка формы на полноэкранный режим
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None; // Убираем рамку окна
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = Screen.PrimaryScreen.Bounds; // Открытие на весь экран

            InitializeCustomUI();
            this.Resize += CentralMainForm_Resize;

            this.PerformLayout();
            this.CentralMainForm_Resize(null, EventArgs.Empty); // Инициализируем расположение кнопок
        }

        private void InitializeCustomUI()
        {
            this.Text = "Центральный округ";
            this.BackgroundImage = Properties.Resources.CentralBackground; // Красный фон с узорами
            this.BackgroundImageLayout = ImageLayout.Stretch;

            titleLabel = new Label
            {
                Text = "Центральный округ\nТрадиции и обычаи",
                Font = new Font("Comic Sans MS", 36f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(titleLabel);

            // Обновляем позицию заголовка после добавления
            titleLabel.Location = new Point((this.ClientSize.Width - titleLabel.Width) / 2, 50);

            // --- Label для описания ---
            descriptionLabel = new Label();
            descriptionLabel.BackColor = Color.Transparent;
            descriptionLabel.ForeColor = Color.White;
            descriptionLabel.Text = "Семейные ценности – почитание старших, крепкие родственные связи.\n\n" +
                                    "Православные праздники – Рождество (колядки), Пасха (красные яйца, куличи), Масленица (блины, сжигание чучела).\n\n" +
                                    "Свадьба – выкуп невесты, каравай, крики «Горько!» (чтобы молодые поцеловались).\n\n" +
                                    "Баня – обязательное использование веников, обливание холодной водой.\n\n" +
                                    "Народные промыслы – хохлома, гжель, дымковская игрушка.\n\n" +
                                    "Интересный обычай: На Троицу дома украшают березовыми ветками – символом жизни.";
            descriptionLabel.Font = new Font("Comic Sans MS", 16f, FontStyle.Regular);
            descriptionLabel.AutoSize = false;
            descriptionLabel.TextAlign = ContentAlignment.MiddleCenter; // Выравнивание по центру
            descriptionLabel.Size = new Size(this.ClientSize.Width / 2, this.ClientSize.Height );

            // --- Кнопка "Назад" сверху слева ---
            closePictureBox = new PictureBox();
            closePictureBox.Name = "closePictureBox";
            closePictureBox.Image = Properties.Resources.BackButton;
            closePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            closePictureBox.Size = new Size(250, 250);
            closePictureBox.Cursor = Cursors.Hand;
            closePictureBox.BackColor = Color.Transparent;
            closePictureBox.Click += (sender, e) => this.Close();

            // --- Кнопка "Вперед" сверху справа ---
            nextPictureBox = new PictureBox();
            nextPictureBox.Name = "nextPictureBox";
            nextPictureBox.Image = Properties.Resources.NextButton;
            nextPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            nextPictureBox.Size = new Size(250, 250);
            nextPictureBox.Cursor = Cursors.Hand;
            nextPictureBox.BackColor = Color.Transparent;
            nextPictureBox.Click += (sender, e) =>
            {
                RegionMapForm regionMapForm = new RegionMapForm();
                this.Hide();
                regionMapForm.ShowDialog();
                this.Show();
            };

            // --- Добавление иконок слева и справа от текста ---
            PictureBox leftIconTop = new PictureBox
            {
                Image = Properties.Resources.Scarecrow,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(400, 400),
                Location = new Point(50, 250),
                Tag = new Point(50, 250)
            };
            this.Controls.Add(leftIconTop);
            imageBoxes.Add(leftIconTop);

            PictureBox rightIconTop = new PictureBox
            {
                Image = Properties.Resources.Barrel,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(400, 400),
                Location = new Point(this.ClientSize.Width - 420, 250),
                Tag = new Point(this.ClientSize.Width - 420, 250)
            };
            this.Controls.Add(rightIconTop);
            imageBoxes.Add(rightIconTop);

            PictureBox leftIconBottom = new PictureBox
            {
                Image = Properties.Resources.EasterBasket,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(400, 400),
                Location = new Point(20, this.ClientSize.Height - 650),
                Tag = new Point(20, this.ClientSize.Height - 650)
            };
            this.Controls.Add(leftIconBottom);
            imageBoxes.Add(leftIconBottom);

            PictureBox rightIconBottom = new PictureBox
            {
                Image = Properties.Resources.Matryoshka,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(350, 350),
                Location = new Point(this.ClientSize.Width - 350, this.ClientSize.Height - 700),
                Tag = new Point(this.ClientSize.Width - 350, this.ClientSize.Height - 700)
            };
            this.Controls.Add(rightIconBottom);
            imageBoxes.Add(rightIconBottom);

            // --- Добавление элементов на форму ---
            this.Controls.Add(descriptionLabel); // Теперь добавляем напрямую

            this.Controls.Add(closePictureBox);
            this.Controls.Add(nextPictureBox);
            this.Controls.Add(titleLabel);

            // --- Вывод кнопок и текста поверх всего ---
            titleLabel.BringToFront();
            closePictureBox.BringToFront();
            nextPictureBox.BringToFront();

            // Вызов обновления расположения
            CentralMainForm_Resize(null, EventArgs.Empty);
        }

        private void CentralMainForm_Resize(object sender, EventArgs e)
        {

            float scaleX = (float)this.Width / baseFormSize.Width;
            float scaleY = (float)this.Height / baseFormSize.Height;
            float scale = Math.Min(scaleX, scaleY);

            float newTitleFontSize = Math.Max(10, Math.Min(baseFontSize * scale * 2, 48));
            float newDescriptionFontSize = Math.Max(10, Math.Min(baseFontSize * scale, 36));
            
            descriptionLabel.Font = new Font(descriptionLabel.Font.FontFamily, newDescriptionFontSize, descriptionLabel.Font.Style);
            

            // Размеры и расположение описания
            descriptionLabel.Width = this.ClientSize.Width / 2;
            descriptionLabel.Height = this.ClientSize.Height;

            // Центрирование текста по горизонтали
            int offsetX = (this.ClientSize.Width - descriptionLabel.Width) / 2;
            int offsetY = 0; // Уменьшаем offsetY для поднятия текста выше

            descriptionLabel.Location = new Point(
                offsetX,
                offsetY
            );

            // Расположение кнопок
            int marginFromTop = -20;
            int marginFromRight = 10;
            int marginFromLeft = 10;

            if (nextPictureBox != null)
            {
                nextPictureBox.Location = new Point(
                    this.ClientSize.Width - nextPictureBox.Width - marginFromRight,
                    marginFromTop
                );
            }

            if (closePictureBox != null)
            {
                closePictureBox.Location = new Point(
                    marginFromLeft,
                    marginFromTop
                );
            }
        }
    }
}