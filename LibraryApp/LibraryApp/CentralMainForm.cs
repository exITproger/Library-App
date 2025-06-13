using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class CentralMainForm : Form
    {
        private Label titleLabel; // Заголовок
        private Label descriptionLabel; // Описание
        private PictureBox mapPictureBox; // Изображение карты

        private Size baseFormSize = new Size(1000, 700);
        private float baseFontSize = 14f;

        private PictureBox closePictureBox; // Делаем полем класса
        private PictureBox nextPictureBox; // Делаем полем класса
        private List<PictureBox> imageBoxes = new List<PictureBox>(); // Список для дополнительных изображений

        public CentralMainForm()
        {
            InitializeComponent();

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
            this.BackgroundImage = Properties.Resources.CentralBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // --- PictureBox с изображением карты ---
            mapPictureBox = new PictureBox();
            mapPictureBox.Image = Properties.Resources.CentralMapForm;
            mapPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            mapPictureBox.Dock = DockStyle.Left;
            mapPictureBox.Width = this.ClientSize.Width / 2;
            mapPictureBox.BackColor = Color.Transparent;

            titleLabel = new Label();
            titleLabel.Text = "Центральный округ";
            titleLabel.Font = new Font("Comic Sans MS", 36f, FontStyle.Bold);
            titleLabel.ForeColor = Color.White;
            titleLabel.BackColor = Color.Transparent; // Можно использовать полупрозрачный фон для теста
            titleLabel.AutoSize = true;
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(titleLabel);
            titleLabel.BringToFront();

            // --- Label для описания ---
            descriptionLabel = new Label();
            descriptionLabel.BackColor = Color.Transparent;
            descriptionLabel.ForeColor = Color.White;
            descriptionLabel.Text = "В этом округе преобладает русское население.\n\n" +
                                    "Русские - хозяева расписных теремов\n" +
                                    "В сказочных книгах русские живут:\n\n" +
                                    "• В избушках с петушками на крыше\n" +
                                    "• Пьют чай с баранками у самовара\n" +
                                    "• Водят хороводы вокруг берёзки";
            descriptionLabel.Font = new Font("Comic Sans MS", 16f, FontStyle.Regular);
            descriptionLabel.AutoSize = false;
            descriptionLabel.TextAlign = ContentAlignment.TopLeft;
            descriptionLabel.Size = new Size(this.ClientSize.Width / 2, this.ClientSize.Height);

            // --- Кнопка закрытия слева сверху ---
            closePictureBox = new PictureBox();
            closePictureBox.Name = "closePictureBox";
            closePictureBox.Image = Properties.Resources.BackButton;
            closePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            closePictureBox.Size = new Size(250, 250);
            closePictureBox.Cursor = Cursors.Hand;
            closePictureBox.BackColor = Color.Transparent;
            closePictureBox.Click += (sender, e) => this.Close();

            // --- Кнопка перехода справа сверху ---
            nextPictureBox = new PictureBox();
            nextPictureBox.Name = "nextPictureBox";
            nextPictureBox.Image = Properties.Resources.NextButton;
            nextPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            nextPictureBox.Size = new Size(250, 250);
            nextPictureBox.Cursor = Cursors.Hand;
            nextPictureBox.BackColor = Color.Transparent;
            nextPictureBox.Click += (sender, e) =>
            {
                CentralCharacterForm regionMapForm = new CentralCharacterForm();
                this.Hide();
                regionMapForm.ShowDialog();
                this.Show();
            };

            // --- Добавление шести изображений ---
            PictureBox image1 = new PictureBox
            {
                Image = Properties.Resources.Bereza,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(250, 400),
                Location = new Point(410, 100),
                Tag = new Point(410, 100) // Базовая позиция
            };
            this.Controls.Add(image1);
            imageBoxes.Add(image1);
            
            PictureBox image5 = new PictureBox
            {
                Image = Properties.Resources.Samovar,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(500, 500),
                Location = new Point(800, 400),
                Tag = new Point(800, 400)
            };
            this.Controls.Add(image5);
            imageBoxes.Add(image5);

            PictureBox image6 = new PictureBox
            {
                Image = Properties.Resources.House,
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Size = new Size(700, 700),
                Location = new Point(300, 400),
                Tag = new Point(300, 400)
            };
            this.Controls.Add(image6);
            imageBoxes.Add(image6);

            // --- Добавление элементов на форму ---
            this.Controls.Add(mapPictureBox);
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
    if (titleLabel == null || descriptionLabel == null) return;

    float scaleX = (float)this.Width / baseFormSize.Width;
    float scaleY = (float)this.Height / baseFormSize.Height;
    float scale = Math.Min(scaleX, scaleY);

    float newTitleFontSize = Math.Max(10, Math.Min(baseFontSize * scale * 2, 48));
    float newDescriptionFontSize = Math.Max(10, Math.Min(baseFontSize * scale, 36));

    titleLabel.Font = new Font(titleLabel.Font.FontFamily, newTitleFontSize, titleLabel.Font.Style);
    descriptionLabel.Font = new Font(descriptionLabel.Font.FontFamily, newDescriptionFontSize, descriptionLabel.Font.Style);

    // --- Центрирование заголовка по горизонтали и немного ниже сверху ---
    int titleTopMargin = (int)(50 * scale); // Отступ сверху с учётом масштаба

    titleLabel.Location = new Point(
        (this.ClientSize.Width - titleLabel.Width) / 2,
        titleTopMargin
    );

    // Размеры картинки карты
    mapPictureBox.Width = this.ClientSize.Width / 2;
    mapPictureBox.Height = this.ClientSize.Height;

    // Размер и расположение описания
    descriptionLabel.Width = this.ClientSize.Width / 2;
    descriptionLabel.Height = this.ClientSize.Height;

    int offsetX = 100; // Пиксели для смещения вправо
    int offsetY = 400; // Пиксели сверху

    descriptionLabel.Location = new Point(
        this.ClientSize.Width / 2 + offsetX,
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

    // Масштабирование дополнительных изображений
    foreach (var image in imageBoxes)
    {
        Point baseLocation = (Point)image.Tag;
        int newX = (int)(baseLocation.X * scaleX);
        int newY = (int)(baseLocation.Y * scaleY);
        int newWidth = (int)(image.Size.Width); 
        int newHeight = (int)(image.Size.Height);
        image.Location = new Point(newX, newY);
        image.Size = new Size(newWidth, newHeight);
    }
}
    }
}