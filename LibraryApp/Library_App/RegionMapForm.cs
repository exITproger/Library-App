
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
        private PictureBox rightTopButton; // Новая кнопка в правом верхнем углу
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
        private void CloseForm()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (!(form is MainMenuForm))
                    form.Close();
            }

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
            GC.Collect();
        }
        public RegionMapForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Инициализация таймера ДО его использования
            animationTimer = new System.Windows.Forms.Timer();
            animationTimer.Interval = 16; // ~60 FPS
            animationTimer.Tick += (s, e) => UpdateButtonAnimation();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Карта регионов";
            this.BackColor = Color.White;
            this.MouseClick += RegionMapForm_MouseClick;
            LoadImages();

            // Кнопка выхода
            exitButton = new PictureBox();
            exitButton.Image = Properties.Resources.Выход_new;
            exitButton.SizeMode = PictureBoxSizeMode.StretchImage;
            exitButton.BackColor = Color.Transparent;
            exitButton.Cursor = Cursors.Hand;
            exitButton.Click += (s, e) => CloseForm();

            // Новая кнопка в правом верхнем углу
            rightTopButton = new PictureBox();
            rightTopButton.Image = Properties.Resources.Пройти_тест_new; 
            rightTopButton.SizeMode = PictureBoxSizeMode.StretchImage;
            rightTopButton.BackColor = Color.Transparent;
            rightTopButton.Cursor = Cursors.Hand;
            rightTopButton.Click += (s, e) =>
            {
                FinalTestForm finalTestForm = new FinalTestForm();
                finalTestForm.ShowDialog();
            };

            // Настройка анимации для обеих кнопок
            SetupButtonHoverEffects(exitButton);
            SetupButtonHoverEffects(rightTopButton);

            this.Controls.Add(exitButton);
            this.Controls.Add(rightTopButton);

            this.Resize += RegionMapForm_Resize;

            CalculateScaleFactor();
            UpdateButtonsSizeAndPosition();
        }

        private void SetupButtonHoverEffects(PictureBox button)
        {
            button.MouseEnter += (s, e) =>
            {
                targetButtonScale = HoverScaleFactor;
                StartButtonAnimation();
            };

            button.MouseLeave += (s, e) =>
            {
                targetButtonScale = 1.0f;
                StartButtonAnimation();
            };
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

            if (Math.Abs(currentButtonScale - targetButtonScale) < 0.01f)
            {
                currentButtonScale = targetButtonScale;
                animationTimer.Stop();
            }

            UpdateButtonsSizeAndPosition();
        }
        // Модифицируем метод UpdateExitButtonSizeAndPosition
        private void UpdateButtonsSizeAndPosition()
        {
            if (exitButton != null && exitButton.Image != null &&
                rightTopButton != null && rightTopButton.Image != null)
            {
                float scale = (float)this.ClientSize.Width / ReferenceScreenWidth;
                int baseButtonWidth = (int)(ReferenceButtonWidth * scale);
                baseButtonWidth = Math.Max(80, Math.Min(baseButtonWidth, 250));

                int buttonWidth = (int)(baseButtonWidth * currentButtonScale);
                int buttonHeight = (int)(buttonWidth * ButtonAspectRatio);

                // Размеры кнопок
                exitButton.Size = new Size(buttonWidth, buttonHeight);
                rightTopButton.Size = new Size(buttonWidth, buttonHeight);

                // Позиционирование кнопки выхода
                int margin = (int)(this.ClientSize.Width * 0.02);
                int offsetX = (int)((baseButtonWidth - buttonWidth) / 2);
                int offsetY = (int)((baseButtonWidth * ButtonAspectRatio - buttonHeight) / 2);
                exitButton.Location = new Point(margin + offsetX, margin + offsetY);

                // Позиционирование правой верхней кнопки
                int rightMargin = (int)(this.ClientSize.Width * 0.02);
                rightTopButton.Location = new Point(
                    this.ClientSize.Width - rightMargin - buttonWidth - offsetX,
                    margin + offsetY);
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
                            regionForm.ShowDialog();

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
                new Point(112, 377),
                new Point(214, 449),
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
            UpdateButtonsSizeAndPosition();
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
            float scaledFontSize = baseFontSize * Math.Min(scaleFactors.Width, scaleFactors.Height);
            // Ограничиваем минимальный и максимальный размер шрифта
            scaledFontSize = Math.Max(10f, Math.Min(24f, scaledFontSize));
            using (Brush textBrush = new SolidBrush(Color.Black))
            using (Font font = new Font("Bold", scaledFontSize, FontStyle.Bold, GraphicsUnit.Point))
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

                    // Просто рисуем текст без обводки
                    g.DrawString(text, font, textBrush, textX, textY);
                }
            }
        }

        private void RegionMapForm_Load(object sender, EventArgs e)
        {
            ShowHintForm(); // ← Окно появится поверх
        }
        private void ShowHintForm()
        {
            using (var taskForm = new TaskRegionMapForm())
            {
                taskForm.ShowDialog(this);
            }
        }
    }
}