
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Library_App
{
    public partial class RegionMapForm : Form
    {
        private Bitmap backgroundMap;
        private Bitmap[] regionImages;
        private Point[] regionPositions;
        private float scaleFactor = 1.0f;
        private PointF basePosition;
        private PictureBox exitButton;

        private SizeF scaleFactors;
        // Добавляем в начало класса новые константы
        private const float HoverScaleFactor = 1.1f; // Увеличение при наведении (10%)
        private const int AnimationDuration = 200; // Длительность анимации в миллисекундах
        private System.Windows.Forms.Timer animationTimer;
        private float currentButtonScale = 1.0f;
        private float targetButtonScale = 1.0f;
        private const int ReferenceScreenWidth = 2560;
        private const int ReferenceButtonWidth = 200;
        private const float ButtonAspectRatio = 0.4f; // Соотношение высоты к ширине
        public RegionMapForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Карта регионов";
            this.BackColor = Color.White;
            this.MouseClick += RegionMapForm_MouseClick;
            LoadImages();
            /*
            // Создаём кнопку выхода
            exitButton = new Button();
            exitButton.Text = "Выход";
            exitButton.Font = new Font("Arial", 16, FontStyle.Bold);
            exitButton.BackColor = Color.LightGray;
            exitButton.AutoSize = true;
            exitButton.Location = new Point(20, 20);
            */
            // Создаем кнопку выхода как PictureBox
            exitButton = new PictureBox();
            exitButton.Image = Properties.Resources.выход; // Ваше изображение кнопки
            exitButton.SizeMode = PictureBoxSizeMode.StretchImage;
            exitButton.BackColor = Color.Transparent;
            exitButton.Cursor = Cursors.Hand;
            exitButton.Click += (s, e) =>
            {
                // Закрыть все формы кроме главной, если она у вас есть в списке открытых
                foreach (Form form in Application.OpenForms)
                {
                    if (!(form is MainMenuForm))
                        form.Close();
                }

                // Проверим, открыто ли главное меню
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
            };
            // В конструкторе после создания exitButton добавляем:
            exitButton.MouseEnter += (s, e) => {
                targetButtonScale = HoverScaleFactor;
                StartButtonAnimation();
            };
            exitButton.MouseLeave += (s, e) => {
                targetButtonScale = 1.0f;
                StartButtonAnimation();
            };
            // Инициализируем таймер в конструкторе
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 16; // ~60 FPS
            animationTimer.Tick += (s, e) => UpdateButtonAnimation();

            this.Controls.Add(exitButton);

            this.Resize += RegionMapForm_Resize;

            CalculateScaleFactor();
            UpdateExitButtonSizeAndPosition();
        }
        // Новые методы для анимации
        private void StartButtonAnimation()
        {
            if (!animationTimer.Enabled)
            {
                animationTimer.Start();
            }
        }

        private void UpdateButtonAnimation()
        {
            const float animationSpeed = 0.2f;
            currentButtonScale += (targetButtonScale - currentButtonScale) * animationSpeed;

            // Если почти достигли целевого масштаба, останавливаем анимацию
            if (Math.Abs(currentButtonScale - targetButtonScale) < 0.01f)
            {
                currentButtonScale = targetButtonScale;
                animationTimer.Stop();
            }

            UpdateExitButtonSizeAndPosition();
        }
        // Модифицируем метод UpdateExitButtonSizeAndPosition
        private void UpdateExitButtonSizeAndPosition()
        {
            if (exitButton != null && exitButton.Image != null)
            {
                // Рассчитываем базовую ширину кнопки пропорционально ширине экрана
                float scale = (float)this.ClientSize.Width / ReferenceScreenWidth;
                int baseButtonWidth = (int)(ReferenceButtonWidth * scale);

                // Ограничиваем минимальный и максимальный размер
                baseButtonWidth = Math.Max(80, Math.Min(baseButtonWidth, 250));

                // Применяем текущий масштаб анимации
                int buttonWidth = (int)(baseButtonWidth * currentButtonScale);
                int buttonHeight = (int)(buttonWidth * ButtonAspectRatio);

                exitButton.Size = new Size(buttonWidth, buttonHeight);

                // Позиционируем в верхнем левом углу с отступом 2% от ширины экрана
                int margin = (int)(this.ClientSize.Width * 0.02);
                // Корректируем позицию с учетом увеличения, чтобы кнопка не смещалась
                int offsetX = (int)((baseButtonWidth - buttonWidth) / 2);
                int offsetY = (int)((baseButtonWidth * ButtonAspectRatio - buttonHeight) / 2);
                exitButton.Location = new Point(margin + offsetX, margin + offsetY);
            }
        }
        public static bool IsPointInPolygon(PointF[] polygon, PointF point)
        {
            int polygonLength = polygon.Length;
            bool inside = false;

            for (int i = 0, j = polygonLength - 1; i < polygonLength; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                    (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private void RegionMapForm_MouseClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < regionImages.Length; i++)
            {
                var img = regionImages[i];
                var pos = regionPositions[i];

                float x = basePosition.X + pos.X * scaleFactors.Width;
                float y = basePosition.Y + pos.Y * scaleFactors.Height;
                float w = img.Width * scaleFactors.Width;
                float h = img.Height * scaleFactors.Height;

                var regionRect = new RectangleF(x, y, w, h);

                if (regionRect.Contains(e.Location))
                {
                    // Точка внутри прямоугольника региона — проверим альфу пикселя
                    // Преобразуем координаты клика в координаты изображения региона
                    float localX = (e.X - x) / scaleFactors.Width;
                    float localY = (e.Y - y) / scaleFactors.Height;

                    if (IsClickOnRegion(img, new PointF(localX, localY)))
                    {
                        Form regionForm = null;
                        switch (i)
                        {
                            case 0: regionForm = new NorthwesternMainForm(); break;
                            case 1: regionForm = new CentralMainForm(); break;
                            case 2: regionForm = new VolgaMainForm(); break;
                            case 3: regionForm = new SouthernMainForm(); break;
                            case 4: regionForm = new NorthcaucasianMainForm(); break;
                            case 5: regionForm = new UralMainForm(); break;
                            case 6: regionForm = new SiberianMainForm(); break;
                            case 7: regionForm = new FareastenMainForm(); break;

                        }

                        if (regionForm != null)
                            Hide();
                            regionForm.ShowDialog();
                            Show();

                        break;
                    }
                }
            }
        }



        private bool IsClickOnRegion(Bitmap regionImage, PointF clickPointOnRegion)
        {
            int x = (int)clickPointOnRegion.X;
            int y = (int)clickPointOnRegion.Y;

            if (x < 0 || y < 0 || x >= regionImage.Width || y >= regionImage.Height)
                return false;

            Color pixelColor = regionImage.GetPixel(x, y);
            return pixelColor.A > 10; // если альфа больше небольшого порога
        }

        private void LoadImages()
        {
            backgroundMap = Properties.Resources.map_background2;

            regionImages = new Bitmap[]
            {
                Properties.Resources.northwestern,
                Properties.Resources.central,
                Properties.Resources.volga,
                Properties.Resources.southern,
                Properties.Resources.northcaucasian,
                Properties.Resources.ural,
                Properties.Resources.siberian,
                Properties.Resources.fareasten
            };

            regionPositions = new Point[]
            {
                new Point(50, 0),
                new Point(112, 378),
                new Point(215, 449),
                new Point(0, 544),
                new Point(69, 668),
                new Point(421, 282),
                new Point(596, 79),
                new Point(968, 110),
            };
        }

        private void RegionMapForm_Resize(object sender, EventArgs e)
        {
            CalculateScaleFactor();
            UpdateExitButtonSizeAndPosition();
            this.Invalidate();
        }

        private void CalculateScaleFactor()
        {
            float widthScale = this.ClientSize.Width / (float)backgroundMap.Width;

            scaleFactors = new SizeF(widthScale, widthScale);

            float scaledHeight = backgroundMap.Height * scaleFactors.Height;
            float offsetY = (this.ClientSize.Height - scaledHeight) / 2;

            basePosition = new PointF(0, offsetY);
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Рисуем фон — растягиваем по ширине и высоте без сохранения пропорций
            g.DrawImage(backgroundMap, basePosition.X, basePosition.Y,
                backgroundMap.Width * scaleFactors.Width,
                backgroundMap.Height * scaleFactors.Height);

            string[] regionNames = { "Северо-Западный", "Центральный", "Поволжье", "Южный", "Северо-Кавказский", "Уральский", "Сибирский", "Дальневосточный" };


            // Базовый размер шрифта, масштабируем
            float baseFontSize = 16f;
            float fontScale = (scaleFactors.Width + scaleFactors.Height) / 2f;
            float scaledFontSize = baseFontSize * fontScale;
            using (Brush textBrush = new SolidBrush(Color.Aqua))
            using (Font font = new Font("Arial", scaledFontSize, FontStyle.Bold, GraphicsUnit.Pixel))
            using (Pen outlinePen = new Pen(Color.Black, 3 * fontScale)) // масштабируемая обвод
            {
                for (int i = 0; i < regionImages.Length; i++)
                {
                    var img = regionImages[i];
                    var pos = regionPositions[i];

                    float x = basePosition.X + pos.X * scaleFactors.Width;
                    float y = basePosition.Y + pos.Y * scaleFactors.Height;

                    float w = img.Width * scaleFactors.Width;
                    float h = img.Height * scaleFactors.Height;

                    // Рисуем регион
                    g.DrawImage(img, x, y, w, h);

                    // Центр региона
                    float centerX = x + w / 2;
                    float centerY = y + h / 2;

                    var text = regionNames[i];
                    var textSize = g.MeasureString(text, font);

                    float textX = centerX - textSize.Width / 2;
                    float textY = centerY - textSize.Height / 2;

                    // Дополнительный сдвиг "Центральный" влево
                    if (text == "Центральный")
                    {
                        textX -= 80 * fontScale; // увеличенный сдвиг
                    }
                    if (text == "Поволжье")
                    {
                        textX -= 50 * fontScale; // увеличенный сдвиг
                    }
                    if (text == "Северо-Западный")
                    {
                        textY += 100 * fontScale; // увеличенный сдвиг
                    }
                    if (text == "Дальневосточный")
                    {
                        textX -= 80 * fontScale; // увеличенный сдвиг
                    }

                    // Контур текста через GraphicsPath
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path.AddString(text, font.FontFamily, (int)font.Style, g.DpiY * font.Size / 72, new PointF(textX, textY), StringFormat.GenericDefault);
                        g.DrawPath(outlinePen, path);
                        g.FillPath(textBrush, path);
                    }
                }
            }
        }

    }
}