using Library_App;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Library_App
{
    public partial class FinalTestForm : Form
    {
        private Bitmap backgroundMap;
        private Bitmap[] regionImages;
        private Point[] regionPositions;
        private float scaleFactor = 1.0f;
        private PointF basePosition;
        private PictureBox exitPictureBox;
        private readonly float[][] regionMargins = new float[][]
        {
            new float[] {0.25f, 0.6f, 0.25f, 0.1f}, // Северо-Западный
            new float[] {0.15f, 0.15f, 0.15f, 0.15f},  // Центральный
            new float[] {0.15f, 0.1f, 0.15f, 0.25f}, // Поволжье
            new float[] {0.2f, 0.1f, 0.1f, 0.8f},  // Южный
            new float[] {0.2f, 0.5f, 0.4f, 0.15f},    // Северо-Кавказский
            new float[] {0.1f, 0.1f, 0.3f, 0.1f},     // Уральский
            new float[] {0.3f, 0.2f, 0.05f, 0.15f},   // Сибирский
            new float[] {0.25f, 0.2f, 0.15f, 0.2f}    // Дальневосточный
        };

        private Bitmap[] draggableImages;
        private Point[] draggablePositions;
        private bool[] isDragging;
        private Point dragOffset;
        private int draggingIndex = -1;
        private int correctPlacementsCount = 0;
        private bool[] isCorrectlyPlaced;

        private RectangleF[] regionBounds;
        private SizeF scaleFactors;
        private float iconScale = 0.2f;
        private const int BaseScreenWidth = 2560;

        // Кэш для отрисованных элементов
        private Bitmap cachedBackground;
        private bool needRedrawBackground = true;

        public FinalTestForm()
        {
            InitializeComponent();
            InitializeFormSettings();
            LoadImages();
            InitializeExitButton();
            InitializeEventHandlers();
        }

        private void InitializeFormSettings()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Карта регионов";
            this.BackColor = Color.White;
        }

        private void InitializeExitButton()
        {
            exitPictureBox = new PictureBox
            {
                Image = Properties.Resources.назад,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(200, 100),
                BackColor = Color.Transparent,
                Location = new Point(20, 20),
                Cursor = Cursors.Hand
            };
            exitPictureBox.Click += (s, e) => Close();
            this.Controls.Add(exitPictureBox);
        }

        private void InitializeEventHandlers()
        {
            this.MouseDown += FinalTestForm_MouseDown;
            this.MouseMove += FinalTestForm_MouseMove;
            this.MouseUp += FinalTestForm_MouseUp;
            this.Resize += RegionMapForm_Resize;
            this.Paint += FinalTestForm_Paint;
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

            draggableImages = new Bitmap[]
            {
                Properties.Resources.northwestern_w,
                Properties.Resources.central_w,
                Properties.Resources.volga_w,
                Properties.Resources.southern_w,
                Properties.Resources.northcaucasian_w,
                Properties.Resources.ural_w,
                Properties.Resources.siberian_w,
                Properties.Resources.fareasten_w,
            };

            InitializeDraggableItems();
            InitializeRegionBounds();
            CalculateScaleFactor();
            CalculateIconScale();
        }

        private void InitializeDraggableItems()
        {
            int columns = this.ClientSize.Width < 1920 ? 4 : 8;
            float dynamicIconScale = Math.Min(0.25f, 0.15f + (this.ClientSize.Width / 3840f));
            iconScale = dynamicIconScale;

            int spawnY = columns == 8 ? this.ClientSize.Height * 3 / 5 : this.ClientSize.Height * 1 / 3;
            int leftBound = 50;
            int rightBound = this.ClientSize.Width - 50;

            var rand = new Random();
            draggablePositions = new Point[draggableImages.Length];
            isDragging = new bool[draggableImages.Length];
            isCorrectlyPlaced = new bool[draggableImages.Length];

            int minDistance = 60;
            for (int i = 0; i < draggableImages.Length; i++)
            {
                int iconWidth = (int)(draggableImages[i].Width * iconScale);
                Point pos;
                bool intersects;
                int attempts = 0;

                do
                {
                    attempts++;
                    if (attempts > 1000)
                    {
                        pos = new Point(leftBound + i * (iconWidth + minDistance), spawnY);
                        break;
                    }

                    int x = rand.Next(leftBound, rightBound - iconWidth);
                    pos = new Point(x, spawnY);

                    intersects = false;
                    for (int j = 0; j < i; j++)
                    {
                        int otherIconWidth = (int)(draggableImages[j].Width * iconScale);
                        var rect1 = new Rectangle(pos, new Size(iconWidth, (int)(draggableImages[i].Height * iconScale * 0.25)));
                        var rect2 = new Rectangle(draggablePositions[j], new Size(otherIconWidth, (int)(draggableImages[j].Height * iconScale * 0.25)));

                        if (rect1.IntersectsWith(rect2) || Distance(pos, draggablePositions[j]) < minDistance)
                        {
                            intersects = true;
                            break;
                        }
                    }
                } while (intersects);

                draggablePositions[i] = pos;
            }
        }
        
        private void InitializeRegionBounds()
        {
            regionBounds = new RectangleF[regionImages.Length];
            for (int i = 0; i < regionImages.Length; i++)
            {
                regionBounds[i] = new RectangleF(
                    regionPositions[i].X,
                    regionPositions[i].Y,
                    regionImages[i].Width,
                    regionImages[i].Height);
            }
        }
        /*private void InitializeRegionBounds()
        {
            // Массив с отступами для каждого региона: Left, Top, Right, Bottom
            float[][] regionMargins = new float[][]
            {
                new float[] {0.25f, 0.4f, 0.25f, 0.1f}, // Северо-Западный
                new float[] {0.1f, 0.15f, 0.1f, 0.05f},  // Центральный
                new float[] {0.05f, 0.05f, 0.05f, 0.15f}, // Поволжье
                new float[] {0.2f, 0.15f, 0.15f, 0.15f},  // Южный
                new float[] {0.1f, 0.1f, 0.1f, 0.15f},    // Северо-Кавказский
                new float[] {0.1f, 0.1f, 0.3f, 0.1f},     // Уральский
                new float[] {0.3f, 0.2f, 0.05f, 0.15f},   // Сибирский
                new float[] {0.25f, 0.2f, 0.15f, 0.2f}    // Дальневосточный
            };

            regionBounds = new RectangleF[regionImages.Length];
            for (int i = 0; i < regionImages.Length; i++)
            {
                float leftMargin = regionMargins[i][0];
                float topMargin = regionMargins[i][1];
                float rightMargin = regionMargins[i][2];
                float bottomMargin = regionMargins[i][3];

                // Рассчитываем исходные границы без учета масштаба
                float originalX = regionPositions[i].X + regionImages[i].Width * leftMargin;
                float originalY = regionPositions[i].Y + regionImages[i].Height * topMargin;
                float originalWidth = regionImages[i].Width * (1 - leftMargin - rightMargin);
                float originalHeight = regionImages[i].Height * (1 - topMargin - bottomMargin);

                // Применяем масштабирование
                float scaledX = basePosition.X + originalX * scaleFactors.Width;
                float scaledY = basePosition.Y + originalY * scaleFactors.Height;
                float scaledWidth = originalWidth * scaleFactors.Width;
                float scaledHeight = originalHeight * scaleFactors.Height;

                regionBounds[i] = new RectangleF(scaledX, scaledY, scaledWidth, scaledHeight);
            }
        }*/
        private double Distance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private void RegionMapForm_Resize(object sender, EventArgs e)
        {
            CalculateScaleFactor();
            CalculateIconScale();
            RespawnDraggableIcons();
            needRedrawBackground = true;
            this.Invalidate();
        }

        private void RespawnDraggableIcons()
        {
            int columns = this.ClientSize.Width < 1920 ? 4 : 8;
            int iconsPerRow = columns;
            int spawnY = columns == 8 ? this.ClientSize.Height * 3 / 5 : this.ClientSize.Height * 1 / 3;
            int leftBound = (int)(50 * iconScale);
            int rightBound = this.ClientSize.Width - (int)(50 * iconScale);

            for (int i = 0; i < draggableImages.Length; i++)
            {
                int row = i / iconsPerRow;
                int col = i % iconsPerRow;

                int iconWidth = (int)(draggableImages[i].Width * iconScale);
                int iconHeight = (int)(draggableImages[i].Height * iconScale);

                int x = leftBound + col * (rightBound - leftBound) / iconsPerRow;
                int y = spawnY + row * (iconHeight + 20);

                if (!isCorrectlyPlaced[i])
                {
                    draggablePositions[i] = new Point(x, y);
                }
            }
        }

        private void CalculateScaleFactor()
        {
            float widthScale = this.ClientSize.Width / (float)backgroundMap.Width;
            scaleFactors = new SizeF(widthScale, widthScale);

            float scaledHeight = backgroundMap.Height * scaleFactors.Height;
            float offsetY = (this.ClientSize.Height - scaledHeight) / 2;

            basePosition = new PointF(0, offsetY);
        }

        private void CalculateIconScale()
        {
            iconScale = 0.25f * (this.ClientSize.Width / (float)BaseScreenWidth);
            iconScale = Math.Max(0.15f, Math.Min(0.4f, iconScale));
        }

        private List<int> GetDrawOrderIndices()
        {
            return Enumerable.Range(0, draggableImages.Length)
                .OrderBy(i => draggablePositions[i].Y + draggableImages[i].Height * 0.25f)
                .ToList();
        }

        private void FinalTestForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var drawOrder = GetDrawOrderIndices();
            for (int idx = drawOrder.Count - 1; idx >= 0; idx--)
            {
                int i = drawOrder[idx];
                if (isCorrectlyPlaced[i]) continue;

                int scaledWidth = (int)(draggableImages[i].Width * iconScale);
                int scaledHeight = (int)(draggableImages[i].Height * iconScale);
                var rect = new Rectangle(draggablePositions[i], new Size(scaledWidth, scaledHeight));

                if (rect.Contains(e.Location))
                {
                    isDragging[i] = true;
                    dragOffset = new Point(e.X - draggablePositions[i].X, e.Y - draggablePositions[i].Y);
                    draggingIndex = i;
                    this.Invalidate();
                    break;
                }
            }
        }

        private void FinalTestForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingIndex != -1 && isDragging[draggingIndex])
            {
                draggablePositions[draggingIndex] = new Point(e.X - dragOffset.X, e.Y - dragOffset.Y);
                this.Invalidate();
            }
        }

        private RectangleF GetMarginAdjustedRegionRect(int index)
        {
            var margins = regionMargins[index];
            var pos = regionPositions[index];
            var img = regionImages[index];

            float leftMargin = img.Width * margins[0];
            float topMargin = img.Height * margins[1];
            float rightMargin = img.Width * margins[2];
            float bottomMargin = img.Height * margins[3];

            return new RectangleF(
                pos.X + leftMargin,
                pos.Y + topMargin,
                img.Width - leftMargin - rightMargin,
                img.Height - topMargin - bottomMargin
            );
        }

        private void FinalTestForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (draggingIndex == -1) return;

            int iconWidth = (int)(draggableImages[draggingIndex].Width * iconScale);
            int iconHeight = (int)(draggableImages[draggingIndex].Height * iconScale);
            Point iconPos = draggablePositions[draggingIndex];

            int lowerPartHeight = (int)(iconHeight * 0.15);
            if (draggingIndex == 3 || draggingIndex == 4) // Южный и Северо-Кавказский
            {
                lowerPartHeight = (int)(iconHeight * 0.45);
            }

            Rectangle lowerPartRect = new Rectangle(
                iconPos.X,
                iconPos.Y + iconHeight - lowerPartHeight,
                iconWidth,
                lowerPartHeight);

            int pixelsInRegion = 0;
            int totalPixels = lowerPartRect.Width * lowerPartRect.Height;

            RectangleF marginRect = GetMarginAdjustedRegionRect(draggingIndex);

            for (int px = 0; px < lowerPartRect.Width; px++)
            {
                for (int py = 0; py < lowerPartRect.Height; py++)
                {
                    int formX = lowerPartRect.X + px;
                    int formY = lowerPartRect.Y + py;

                    float mapX = (formX - basePosition.X) / scaleFactors.Width;
                    float mapY = (formY - basePosition.Y) / scaleFactors.Height;

                    // Проверяем попадание в урезанный хитбокс нужного региона
                    if (marginRect.Contains(mapX, mapY))
                    {
                        // Дополнительно можно проверить пиксельную прозрачность и цвет региона для точности
                        var regionImg = regionImages[draggingIndex];
                        var regionPos = regionPositions[draggingIndex];
                        int localX = (int)(mapX - regionPos.X);
                        int localY = (int)(mapY - regionPos.Y);

                        if (localX >= 0 && localY >= 0 && localX < regionImg.Width && localY < regionImg.Height)
                        {
                            Color pixelColor = regionImg.GetPixel(localX, localY);
                            if (pixelColor.A > 128 && !(pixelColor.R > 240 && pixelColor.G > 240 && pixelColor.B > 240))
                            {
                                pixelsInRegion++;
                            }
                        }
                    }
                }
            }

            PointF iconCenter = new PointF(iconPos.X + iconWidth / 2f, iconPos.Y + iconHeight / 2f);
            float centerMapX = (iconCenter.X - basePosition.X) / scaleFactors.Width;
            float centerMapY = (iconCenter.Y - basePosition.Y) / scaleFactors.Height;

            bool centerInRegion = marginRect.Contains(centerMapX, centerMapY);

            float threshold = 0.15f;
            if (draggingIndex == 3 || draggingIndex == 4)
            {
                threshold = 0.01f;
            }

            bool currentlyCorrect = ((float)pixelsInRegion / totalPixels) >= threshold || centerInRegion;

            if (currentlyCorrect && !isCorrectlyPlaced[draggingIndex])
            {
                correctPlacementsCount++;
                isCorrectlyPlaced[draggingIndex] = true;
            }
            else if (!currentlyCorrect && isCorrectlyPlaced[draggingIndex])
            {
                correctPlacementsCount--;
                isCorrectlyPlaced[draggingIndex] = false;
            }

            isDragging[draggingIndex] = false;
            draggingIndex = -1;
            this.Invalidate();

            if (correctPlacementsCount == 8)
            {
                Timer timer = new Timer { Interval = 1000 };
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    ShowCongratulationsForm();
                };
                timer.Start();
            }
        }

        private void FinalTestForm_Paint(object sender, PaintEventArgs e)
        {
            if (needRedrawBackground || cachedBackground == null)
            {
                cachedBackground = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                using (var g = Graphics.FromImage(cachedBackground))
                {
                    DrawBackground(g);
                    DrawRegions(g);

                }
                needRedrawBackground = false;
            }

            e.Graphics.DrawImage(cachedBackground, Point.Empty);
            DrawDraggableItems(e.Graphics);
            DrawCounter(e.Graphics);

        }

        private void DrawBackground(Graphics g)
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(backgroundMap, basePosition.X, basePosition.Y,
                backgroundMap.Width * scaleFactors.Width,
                backgroundMap.Height * scaleFactors.Height);
        }

        private void DrawRegions(Graphics g)
        {
            string[] regionNames = { "Северо-Западный", "Центральный", "Поволжье", "Южный",
                   "Северо-Кавказский", "Уральский", "Сибирский", "Дальневосточный" };

            // Адаптивный размер шрифта (основанный на масштабе или размере окна)
            float baseFontSize = 16f;
            float scaledFontSize = baseFontSize * Math.Min(scaleFactors.Width, scaleFactors.Height);
            float fontScale = (scaleFactors.Width + scaleFactors.Height) / 2f;
            // Ограничиваем минимальный и максимальный размер шрифта
            scaledFontSize = Math.Max(10f, Math.Min(24f, scaledFontSize));

            using (Font font = new Font("Bold", scaledFontSize, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.FloralWhite))
            {
                for (int i = 0; i < regionImages.Length; i++)
                {
                    var img = regionImages[i];
                    var pos = regionPositions[i];

                    // Позиция и размер региона с учетом масштаба
                    float x = basePosition.X + pos.X * scaleFactors.Width;
                    float y = basePosition.Y + pos.Y * scaleFactors.Height;
                    float w = img.Width * scaleFactors.Width;
                    float h = img.Height * scaleFactors.Height;

                    // Рисуем изображение региона
                    g.DrawImage(img, x, y, w, h);

                    // Получаем прямоугольник с учетом отступов
                    RectangleF regionRect = GetMarginAdjustedRegionRect(i);
                    float regionX = basePosition.X + regionRect.X * scaleFactors.Width;
                    float regionY = basePosition.Y + regionRect.Y * scaleFactors.Height;
                    float regionW = regionRect.Width * scaleFactors.Width;
                    float regionH = regionRect.Height * scaleFactors.Height;

                    var text = regionNames[i];
                    var textSize = g.MeasureString(text, font);

                    // Центр региона с учетом отступов
                    float centerX = regionX + regionW / 2;
                    float centerY = regionY + regionH / 2;

                    // Позиция текста (по центру региона с отступами)
                    float textX = centerX - textSize.Width / 2;
                    float textY = centerY - textSize.Height / 2;

                    // Индивидуальные корректировки позиции для конкретных регионов
                    switch (text)
                    {
                        case "Центральный":
                            textX -= 80 * fontScale;
                            break;
                        case "Поволжье":
                            textX -= 50 * fontScale;
                            textY += 30 * fontScale;
                            break;
                        case "Сибирский":
                            textX -= 50 * fontScale;
                            textY -= 20 * fontScale;
                            break;
                        case "Уральский":
                            textX += 40 * fontScale;
                            break;
                        case "Северо-Западный":
                            textY -= 30 * fontScale;
                            if (this.ClientSize.Width < 1900) textX += 5 * fontScale;
                            break;
                        case "Южный":
                            textY+= 80 * fontScale;
                            break;
                        case "Северо-Кавказский":
                            textY -= 30 * fontScale;
                            break;
                        case "Дальневосточный":
                            textX -= 140 * fontScale;
                            break;
                        
                    }

                    

                    // Рисуем текст
                    g.DrawString(text, font, textBrush, textX, textY);
                }
            }
        }

        private void DrawDraggableItems(Graphics g)
        {
            var sortedIndices = Enumerable.Range(0, draggableImages.Length)
                .OrderBy(i => draggablePositions[i].Y)
                .ToList();

            foreach (int i in sortedIndices)
            {
                int scaledWidth = (int)(draggableImages[i].Width * iconScale);
                int scaledHeight = (int)(draggableImages[i].Height * iconScale);
                g.DrawImage(draggableImages[i], new Rectangle(draggablePositions[i], new Size(scaledWidth, scaledHeight)));
            }
        }

        private void DrawCounter(Graphics g)
        {
            using (Font font = new Font("Bold", 30, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.Black))
            {
                string topRightText = $"Верно расставлено: {correctPlacementsCount}";
                var textSize = g.MeasureString(topRightText, font);

                float x = this.ClientSize.Width - textSize.Width - this.ClientSize.Width / 20;
                float y = this.ClientSize.Height / 25;

                g.DrawString(topRightText, font, brush, new PointF(x, y));
            }
        }

        private void ShowCongratulationsForm()
        {
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int formWidth, formHeight;

            if (screenWidth > 3500)
            {
                formWidth = 2000;
                formHeight = 1500;
            }
            else if (screenWidth > 2500)
            {
                formWidth = 1200;
                formHeight = 900;
            }
            else if (screenWidth > 1900)
            {
                formWidth = 800;
                formHeight = 600;
            }
            else
            {
                formWidth = 600;
                formHeight = 400;
            }

            Form congratsForm = new Form()
            {
                Text = "Поздравляем!",
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(formWidth, formHeight),
                BackColor = Color.LightSkyBlue,
                Padding = new Padding(20)
            };

            // Определяем размер шрифта
            int fontSize;
            if (formWidth > 3500)
                fontSize = 32;
            else if (formWidth > 2500)
                fontSize = 28;
            else if (formWidth > 1900)
                fontSize = 24;
            else if (formWidth > 1200)
                fontSize = 20;
            else if (formWidth > 800)
                fontSize = 18;
            else
                fontSize = 14;

            Label label = new Label()
            {
                Text = "Вы успешно разместили все регионы России!",
                Font = new Font("Bold", fontSize, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            Size buttonSize = new Size((int)(formWidth * 0.3), (int)(formHeight * 0.1));

            Button btnReturn = new Button()
            {
                Text = "На карту регионов",
                DialogResult = DialogResult.Retry,
                Size = buttonSize,
                Font = new Font("Bold", fontSize - 2),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };

            Button btnExit = new Button()
            {
                Text = "Выйти",
                DialogResult = DialogResult.Cancel,
                Size = buttonSize,
                Font = new Font("Bold", fontSize - 2),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };

            TableLayoutPanel mainPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };

            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

            Panel buttonContainer = new Panel() { Dock = DockStyle.Fill };

            btnReturn.Location = new Point(
                (buttonContainer.Width - (btnReturn.Width + btnExit.Width + formWidth / 20)) / 2,
                (buttonContainer.Height - btnReturn.Height) / 2);

            btnExit.Location = new Point(
                btnReturn.Right + formWidth / 20,
                btnReturn.Top);

            buttonContainer.Resize += (sender, e) =>
            {
                btnReturn.Location = new Point(
                    (buttonContainer.Width - (btnReturn.Width + btnExit.Width + formWidth / 20)) / 2,
                    (buttonContainer.Height - btnReturn.Height) / 2);

                btnExit.Location = new Point(
                    btnReturn.Right + formWidth / 20,
                    btnReturn.Top);
            };

            buttonContainer.Controls.Add(btnReturn);
            buttonContainer.Controls.Add(btnExit);

            mainPanel.Controls.Add(label, 0, 0);
            mainPanel.Controls.Add(new Panel(), 0, 1);
            mainPanel.Controls.Add(buttonContainer, 0, 2);

            congratsForm.Controls.Add(mainPanel);

            DialogResult result = congratsForm.ShowDialog(this);

            if (result == DialogResult.Retry)
            {
                ReturnToRussiaMap();
            }
            else if (result == DialogResult.Cancel)
            {
                Form[] openForms = Application.OpenForms.Cast<Form>().ToArray();
                foreach (Form form in openForms)
                {
                    if (form.GetType() != typeof(MainMenuForm))
                        form.Close();
                }

                MainMenuForm mainMenu = Application.OpenForms.OfType<MainMenuForm>().FirstOrDefault();
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
            }
        }

        private void ReturnToRussiaMap()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (!(form is RegionMapForm || form is MainMenuForm || form is MapForm))
                    form.Close();
            }

            var open = Application.OpenForms.OfType<RegionMapForm>().FirstOrDefault();
            if (open == null)
            {
                open = new RegionMapForm();
                open.Show();
            }
            else
            {
                open.BringToFront();
            }

            this.Close();
        }

        private void FinalTestForm_Load(object sender, EventArgs e)
        {
            ShowTaskForm();
        }

        private void ShowTaskForm()
        {
            using (var taskForm = new TaskFormFinal())
            {
                taskForm.ShowDialog(this);
            }
        }
    }
}