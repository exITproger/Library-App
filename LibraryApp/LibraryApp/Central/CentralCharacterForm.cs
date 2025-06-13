using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class CentralCharacterForm : Form
    {
        private Label label; // Теперь label — поле класса
        private Size baseFormSize = new Size(1000, 700);
        private float baseFontSize = 14f;
        private PictureBox nextPictureBox; // Делаем nextPictureBox полем класса
        private PictureBox closePictureBox; // Делаем closePictureBox полем класса
        private Label titleLabel;

        public CentralCharacterForm()
        {
            InitializeComponent();

            // Настройка формы на полноэкранный режим
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None; // Убираем рамку окна
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = Screen.PrimaryScreen.Bounds; // Открытие на весь экран

            InitializeCustomUI();
            this.Resize += CentralCharacterForm_Resize;

            this.PerformLayout();
            this.CentralCharacterForm_Resize(null, EventArgs.Empty); // Инициализируем расположение кнопок
        }

        private void InitializeCustomUI()
        {
            this.Text = "Центральный округ — Народный костюм";
            this.BackgroundImage = Properties.Resources.CentralBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            titleLabel = new Label
            {
                Text = "Народный костюм",
                Font = new Font("Comic Sans MS", 36f, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };
            titleLabel.Location = new Point(
                (this.ClientSize.Width - titleLabel.Width) / 2,
                (this.ClientSize.Height - titleLabel.Height) / 2
            );
            this.Controls.Add(titleLabel);
            // --- Создание TableLayoutPanel ---
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.ColumnCount = 2;
            tableLayoutPanel.RowCount = 1;
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            tableLayoutPanel.BackgroundImage = Properties.Resources.CentralBackground;
            tableLayoutPanel.BackgroundImageLayout = ImageLayout.Stretch;

            // PictureBox с изображением
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = Properties.Resources.Russian;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.BackColor = Color.Transparent;

            // Label с текстом
            label = new Label();
            label.BackColor = Color.Transparent;
            label.ForeColor = Color.White;
            label.Text = "👦 Русский Молодец (Мужская одежда):\n" +
                         "1. Рубаха (косоворотка): Рубашка с разрезом сбоку, часто с красной вышивкой.\n" +
                         "2. Штаны: Тёмные, простые, заправлены в сапоги.\n" +
                         "3. Пояс: Яркий тканый кушак – обязательно!\n" +
                         "4. Верхняя одежда: Кафтан или зипун (куртка).\n" +
                         "5. Головной убор: Летом – шапочка, зимой – ушанка или меховая шапка.\n" +
                         "6. Обувь: Сапоги или лапти, зимой – валенки.\n\n" +
                         "👧 Русская Красавица (Женская одежда):\n" +
                         "1. Рубаха: Длинная с красивой вышивкой.\n" +
                         "2. Сарафан: Яркое платье без рукавов.\n" +
                         "3. Передник: Нарядный фартук поверх сарафана.\n" +
                         "4. Пояс: Плетёный или тканый, с кисточками.\n" +
                         "5. Верхняя одежда: Душегрейка – безрукавка с мехом или вышивкой.\n" +
                         "6. Головной убор:\n" +
                         "   • Девушки: Коса с лентами, венок.\n" +
                         "   • Женщины: Кокошник или платок.\n" +
                         "7. Обувь: Туфельки, сапожки или лапти; зимой – валенки.";
            label.AutoSize = false;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.MaximumSize = new Size(0, 0);
            label.Font = new Font("Comic Sans MS", 16f, FontStyle.Bold | FontStyle.Italic);

            // Добавление элементов в TableLayoutPanel
            tableLayoutPanel.Controls.Add(pictureBox, 0, 0);
            tableLayoutPanel.Controls.Add(label, 1, 0);

            // --- Кнопка закрытия слева сверху ---
            closePictureBox = new PictureBox();
            closePictureBox.Name = "closePictureBox";
            closePictureBox.Image = Properties.Resources.BackButton;
            closePictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            closePictureBox.Size = new Size(250, 250);
            closePictureBox.Cursor = Cursors.Hand;
            closePictureBox.BackColor = Color.Transparent;
            closePictureBox.Click += (sender, e) => this.Close();

            nextPictureBox = new PictureBox();
            nextPictureBox.Name = "nextPictureBox";
            nextPictureBox.Image = Properties.Resources.NextButton;
            nextPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            nextPictureBox.Size = new Size(250, 250);
            nextPictureBox.Cursor = Cursors.Hand;
            nextPictureBox.BackColor = Color.Transparent;
            nextPictureBox.Click += (sender, e) =>
            {
                CentralOmensForm regionMapForm = new CentralOmensForm();
                this.Hide();
                regionMapForm.ShowDialog();
                this.Show();
            };

            // --- Добавление элементов на форму ---
            this.Controls.Add(tableLayoutPanel); // Сначала добавляем TableLayoutPanel
            this.Controls.Add(closePictureBox);
            this.Controls.Add(nextPictureBox);

            // --- Вывод кнопок поверх всего ---
            closePictureBox.BringToFront();
            nextPictureBox.BringToFront(); // Убедитесь, что кнопка находится на переднем плане
        }

        private void CentralCharacterForm_Resize(object sender, EventArgs e)
        {
            if (titleLabel == null || label == null) return;

            // Обновляем базовые размеры при каждом ресайзе (если нужно адаптивности)
            float scaleX = (float)this.ClientSize.Width / baseFormSize.Width;
            float scaleY = (float)this.ClientSize.Height / baseFormSize.Height;
            float scale = Math.Min(scaleX, scaleY);

            // --- Масштабируем шрифты ---
            titleLabel.Font = new Font(
                titleLabel.Font.FontFamily,
                Math.Max(14, baseFontSize * scale * 1.5f),
                titleLabel.Font.Style
            );

            label.Font = new Font(
                label.Font.FontFamily,
                Math.Max(14, baseFontSize * scale * 1f),
                label.Font.Style
            );

            // Центрирование заголовка
            titleLabel.Location = new Point(
                (this.ClientSize.Width - titleLabel.Width) / 2,
                (int)(20 * scale)
            );

            // Расположение текста
            label.Width = this.ClientSize.Width / 2;
            label.Height = this.ClientSize.Height;

            int offsetX = (this.ClientSize.Width - label.Width) / 2;
            int offsetY = titleLabel.Bottom + (int)(20 * scale);

            label.Location = new Point(offsetX, offsetY);

            // Позиционирование кнопок
            int marginFromTop = (int)(-20 * scale);
            int marginFromRight = (int)(10 * scale);
            int marginFromLeft = (int)(10 * scale);

            nextPictureBox.Location = new Point(
                this.ClientSize.Width - nextPictureBox.Width - marginFromRight,
                marginFromTop
            );

            closePictureBox.Location = new Point(
                marginFromLeft,
                marginFromTop
            );
        }
    }
}