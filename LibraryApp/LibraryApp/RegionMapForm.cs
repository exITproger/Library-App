using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class RegionMapForm : Form
    {
        private Bitmap backgroundMap;
        private Bitmap[] regionImages;
        private Point[] regionPositions;
        private float scaleFactor = 1.0f;
        private PointF basePosition;
        private Button exitButton;

        private SizeF scaleFactors;

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
            // Общие настройки для кнопок (если ещё не объявлены)
            Font commonFontBold = new Font("Segoe UI", 16, FontStyle.Bold);
            Color buttonBackColor = Color.FromArgb(240, 240, 240);
            Color buttonHoverColor = Color.FromArgb(210, 210, 210);
            Color buttonTextColor = Color.FromArgb(30, 30, 30);
            int buttonWidth = 140;
            int buttonHeight = 50;

            // Функция создания кнопки с закруглёнными углами и эффектом наведения
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

            // Создаём кнопку выхода в том же стиле
            exitButton = CreateStyledButton("Выход", commonFontBold, new Point(20, 20), (s, e) => Application.Exit());
            this.Controls.Add(exitButton);
            
            exitButton.Click += (s, e) =>
            {
                // Создаём копию списка форм, которые нужно закрыть
                var formsToClose = Application.OpenForms
                                    .OfType<Form>()
                                    .Where(form => !(form is MainMenuForm))
                                    .ToList();

                // Закрываем формы из списка
                foreach (var form in formsToClose)
                {
                    form.Close();
                }

                // Проверяем, есть ли главное меню
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

                Close();
            };



            this.Controls.Add(exitButton);

            this.Resize += RegionMapForm_Resize;

            CalculateScaleFactor();
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

            string[] regionNames = {
        "Северо-Западный", "Центральный", "Поволжье", "Южный",
        "Северо-Кавказский", "Уральский", "Сибирский", "Дальневосточный"
    };

            // Базовый размер шрифта, масштабируем
            float baseFontSize = 16f;
            float fontScale = (scaleFactors.Width + scaleFactors.Height) / 2f;
            float scaledFontSize = baseFontSize * fontScale;

            using (Font font = new Font("Arial", scaledFontSize, FontStyle.Bold, GraphicsUnit.Pixel))
            using (Brush textBrush = new SolidBrush(Color.Aqua))
            using (Pen outlinePen = new Pen(Color.Black, 3 * fontScale)) // масштабируемая обводка
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
                    // Контур + заливка (один вызов)
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path.AddString(text, font.FontFamily, (int)font.Style,
                            g.DpiY * font.Size / 72, new PointF(textX, textY), StringFormat.GenericDefault);

                        g.DrawPath(outlinePen, path);
                        g.FillPath(textBrush, path);
                    }
                }
            }
        }



    }
}
