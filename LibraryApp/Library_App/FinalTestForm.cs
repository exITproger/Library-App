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

        private Bitmap[] draggableImages;
        private Point[] draggablePositions;
        private bool[] isDragging;
        private Point dragOffset;
        private int draggingIndex = -1;
        private int correctPlacementsCount = 0;
        private bool[] isCorrectlyPlaced;

        private RectangleF[] regionBounds;

        private SizeF scaleFactors;

        public FinalTestForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Карта регионов";
            this.BackColor = Color.White;
            LoadImages();

            // Создаём кнопку выхода
            // Создаём PictureBox для выхода вместо кнопки
            exitPictureBox = new PictureBox();
            exitPictureBox.Image = Properties.Resources.назад; // Предполагается, что у вас есть ресурс с иконкой
            exitPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            exitPictureBox.Size = new Size(200, 100); // Размер иконки
            exitPictureBox.BackColor = Color.Transparent; // Прозрачный фон
            exitPictureBox.Location = new Point(20, 20);
            exitPictureBox.Cursor = Cursors.Hand; // Курсор-рука при наведении
            exitPictureBox.Click += (s, e) =>
            {
                Close();
                /*
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
                */
            };

            isCorrectlyPlaced = new bool[draggableImages.Length];
            for (int i = 0; i < isCorrectlyPlaced.Length; i++)
                isCorrectlyPlaced[i] = false;

            this.Controls.Add(exitPictureBox);
            this.MouseDown += FinalTestForm_MouseDown;
            this.MouseMove += FinalTestForm_MouseMove;
            this.MouseUp += FinalTestForm_MouseUp;

            this.Resize += RegionMapForm_Resize;

            CalculateScaleFactor();
        }

        

        private List<int> GetDrawOrderIndices()
        {
            return Enumerable.Range(0, draggableImages.Length)
                .OrderBy(i => draggablePositions[i].Y + draggableImages[i].Height * 0.25f) // по нижней границе иконки
                .ToList();
        }


        private void FinalTestForm_MouseDown(object sender, MouseEventArgs e)
        {
            var drawOrder = GetDrawOrderIndices();
            // Перебираем в обратном порядке — от верхней иконки к нижней
            for (int idx = drawOrder.Count - 1; idx >= 0; idx--)
            {
                int i = drawOrder[idx];
                int scaledWidth = (int)(draggableImages[i].Width * 0.25);
                int scaledHeight = (int)(draggableImages[i].Height * 0.25);
                var rect = new Rectangle(draggablePositions[i], new Size(scaledWidth, scaledHeight));
                if (rect.Contains(e.Location))
                {
                    isDragging[i] = true;
                    dragOffset = new Point(e.X - draggablePositions[i].X, e.Y - draggablePositions[i].Y);
                    draggingIndex = i;
                    break;
                }
            }
        }


        private void FinalTestForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingIndex != -1 && isDragging[draggingIndex])
            {
                draggablePositions[draggingIndex] = new Point(e.X - dragOffset.X, e.Y - dragOffset.Y);
                Cursor = Cursors.Hand;
                this.Invalidate(); // Перерисовать
            }
            else
            {
                Cursor = Cursors.Default;
            }
        }


        private void FinalTestForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (draggingIndex != -1)
            {
                int iconWidth = (int)(draggableImages[draggingIndex].Width * 0.25);
                int iconHeight = (int)(draggableImages[draggingIndex].Height * 0.25);
                Point iconPos = draggablePositions[draggingIndex];

                int lowerPartHeight = (int)(iconHeight * 0.15);
                Rectangle lowerPartRect = new Rectangle(
                    iconPos.X,
                    iconPos.Y + iconHeight - lowerPartHeight,
                    iconWidth,
                    lowerPartHeight);

                int pixelsInRegion = 0;
                int totalPixels = lowerPartRect.Width * lowerPartRect.Height;

                for (int px = 0; px < lowerPartRect.Width; px++)
                {
                    for (int py = 0; py < lowerPartRect.Height; py++)
                    {
                        int formX = lowerPartRect.X + px;
                        int formY = lowerPartRect.Y + py;

                        float mapX = (formX - basePosition.X) / scaleFactors.Width;
                        float mapY = (formY - basePosition.Y) / scaleFactors.Height;

                        for (int i = 0; i < regionImages.Length; i++)
                        {
                            var regionImg = regionImages[i];
                            var regionPos = regionPositions[i];

                            int localX = (int)(mapX - regionPos.X);
                            int localY = (int)(mapY - regionPos.Y);

                            if (localX >= 0 && localY >= 0 && localX < regionImg.Width && localY < regionImg.Height)
                            {
                                Color pixelColor = regionImg.GetPixel(localX, localY);
                                if (pixelColor.A > 128 &&
                                    !(pixelColor.R > 240 && pixelColor.G > 240 && pixelColor.B > 240))
                                {
                                    if (i == draggingIndex)
                                    {
                                        pixelsInRegion++;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }

                PointF iconCenter = new PointF(iconPos.X + iconWidth / 2f, iconPos.Y + iconHeight / 2f);
                float centerMapX = (iconCenter.X - basePosition.X) / scaleFactors.Width;
                float centerMapY = (iconCenter.Y - basePosition.Y) / scaleFactors.Height;

                bool centerInRegion = false;
                for (int i = 0; i < regionImages.Length; i++)
                {
                    var regionPos = regionPositions[i];
                    var regionImg = regionImages[i];

                    int localX = (int)(centerMapX - regionPos.X);
                    int localY = (int)(centerMapY - regionPos.Y);

                    if (localX >= 0 && localY >= 0 && localX < regionImg.Width && localY < regionImg.Height)
                    {
                        Color pixelColor = regionImg.GetPixel(localX, localY);
                        if (pixelColor.A > 128 &&
                            !(pixelColor.R > 240 && pixelColor.G > 240 && pixelColor.B > 240) &&
                            i == draggingIndex)
                        {
                            centerInRegion = true;
                            break;
                        }
                    }
                }

                bool currentlyCorrect = ((float)pixelsInRegion / totalPixels) >= 0.15f || centerInRegion;

                // Проверяем текущее и предыдущее состояние
                if (currentlyCorrect && !isCorrectlyPlaced[draggingIndex])
                {
                    correctPlacementsCount++;
                    isCorrectlyPlaced[draggingIndex] = true;
                    isDragging[draggingIndex] = false; // фиксируем, нельзя двигать дальше
                }
                else if (!currentlyCorrect && isCorrectlyPlaced[draggingIndex])
                {
                    correctPlacementsCount--;
                    isCorrectlyPlaced[draggingIndex] = false;
                    // Разрешаем снова перетаскивать
                    isDragging[draggingIndex] = false;
                }
                else
                {
                    // Просто снимаем флаг перетаскивания, если ничего не изменилось
                    isDragging[draggingIndex] = false;
                }

                draggingIndex = -1;

                this.Invalidate(); // Обновить форму для отрисовки счётчика

                // Проверяем, все ли регионы размещены правильно
                if (correctPlacementsCount == 8)
                {
                    // Запускаем таймер, который откроет форму поздравления через секунду
                    Timer timer = new Timer();
                    timer.Interval = 1000; // 1 секунда
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        ShowCongratulationsForm();
                    };
                    timer.Start();
                }
            }
        }

        private void ShowCongratulationsForm()
        {
            // Определяем размеры формы в зависимости от разрешения экрана
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

            // Создаем форму
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

            // Текст поздравления
            Label label = new Label()
            {
                Text = "Вы успешно разместили все регионы России!",
                Font = new Font("Arial", fontSize, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Padding = new Padding(20)
            };

            // Размер кнопок
            Size buttonSize = new Size((int)(formWidth * 0.3), (int)(formHeight * 0.1));

            // Кнопка "На карту регионов"
            Button btnReturn = new Button()
            {
                Text = "На карту регионов",
                DialogResult = DialogResult.Retry,
                Size = buttonSize,
                Font = new Font("Arial", fontSize - 2),
                BackColor = Color.LightGreen,
                FlatStyle = FlatStyle.Flat
            };

            // Кнопка "Выйти"
            Button btnExit = new Button()
            {
                Text = "Выйти",
                DialogResult = DialogResult.Cancel,
                Size = buttonSize,
                Font = new Font("Arial", fontSize - 2),
                BackColor = Color.LightCoral,
                FlatStyle = FlatStyle.Flat
            };

            // Основной контейнер
            TableLayoutPanel mainPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };

            // Настройка строк
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 60));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

            // Контейнер для кнопок (используем Panel вместо FlowLayoutPanel для лучшей совместимости)
            Panel buttonContainer = new Panel()
            {
                Dock = DockStyle.Fill
            };

            // Позиционируем кнопки вручную
            btnReturn.Location = new Point(
                (buttonContainer.Width - (btnReturn.Width + btnExit.Width + formWidth / 20)) / 2,
                (buttonContainer.Height - btnReturn.Height) / 2);

            btnExit.Location = new Point(
                btnReturn.Right + formWidth / 20,
                btnReturn.Top);

            // Обработчик изменения размера для правильного позиционирования кнопок
            buttonContainer.Resize += (sender, e) =>
            {
                btnReturn.Location = new Point(
                    (buttonContainer.Width - (btnReturn.Width + btnExit.Width + formWidth / 20)) / 2,
                    (buttonContainer.Height - btnReturn.Height) / 2);

                btnExit.Location = new Point(
                    btnReturn.Right + formWidth / 20,
                    btnReturn.Top);
            };

            // Добавляем кнопки в контейнер
            buttonContainer.Controls.Add(btnReturn);
            buttonContainer.Controls.Add(btnExit);

            // Добавляем элементы в основной контейнер
            mainPanel.Controls.Add(label, 0, 0);
            mainPanel.Controls.Add(new Panel(), 0, 1); // Разделитель
            mainPanel.Controls.Add(buttonContainer, 0, 2);

            // Добавляем основной контейнер на форму
            congratsForm.Controls.Add(mainPanel);

            // Обработка результатов
            DialogResult result = congratsForm.ShowDialog(this);

            if (result == DialogResult.Retry)
            {
                ReturnToRussiaMap();
            }
            else if (result == DialogResult.Cancel)
            {
                // Закрыть все формы кроме главной (совместимый способ)
                Form[] openForms = Application.OpenForms.Cast<Form>().ToArray();
                foreach (Form form in openForms)
                {
                    if (form.GetType() != typeof(MainMenuForm))
                        form.Close();
                }

                // Находим главное меню
                MainMenuForm mainMenu = null;
                foreach (Form form in Application.OpenForms)
                {
                    if (form is MainMenuForm)
                    {
                        mainMenu = (MainMenuForm)form;
                        break;
                    }
                }

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
            // Закрыть все формы кроме главной, если она у вас есть в списке открытых
            foreach (Form form in Application.OpenForms)
            {
                if (!(form is RegionMapForm || form is MainMenuForm || form is MapForm))
                    form.Close();
            }

            // Проверим, открыто ли главное меню
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

            isDragging = new bool[draggableImages.Length];

            // Задаём минимальное расстояние между иконками
            int minDistance = 60;

            // Область для спавна (горизонтально)
            int spawnY = this.ClientSize.Height - 300;
            int leftBound = 50;
            int rightBound = this.ClientSize.Width - 150; // оставим запас справа

            var rand = new Random();
            draggablePositions = new Point[draggableImages.Length];

            for (int i = 0; i < draggableImages.Length; i++)
            {
                int iconWidth = (int)(draggableImages[i].Width * 0.25);

                Point pos;
                bool intersects;
                int attempts = 0;
                do
                {
                    attempts++;
                    if (attempts > 1000)
                    {
                        // Если не удаётся найти позицию (маленькая область), просто ставим подряд
                        pos = new Point(leftBound + i * (iconWidth + minDistance), spawnY);
                        break;
                    }

                    int x = rand.Next(leftBound, rightBound - iconWidth);
                    pos = new Point(x, spawnY);

                    intersects = false;
                    for (int j = 0; j < i; j++)
                    {
                        int otherIconWidth = (int)(draggableImages[j].Width * 0.25);
                        var rect1 = new Rectangle(pos, new Size(iconWidth, draggableImages[i].Height / 4));
                        var rect2 = new Rectangle(draggablePositions[j], new Size(otherIconWidth, draggableImages[j].Height / 4));

                        if (rect1.IntersectsWith(rect2) || Distance(pos, draggablePositions[j]) < minDistance)
                        {
                            intersects = true;
                            break;
                        }
                    }

                } while (intersects);

                draggablePositions[i] = pos;
            }

            regionBounds = new RectangleF[regionImages.Length];

            for (int i = 0; i < regionImages.Length; i++)
            {
                float x = regionPositions[i].X;
                float y = regionPositions[i].Y;
                float w = regionImages[i].Width;
                float h = regionImages[i].Height;

                regionBounds[i] = new RectangleF(x, y, w, h); // в оригинальных координатах
            }

        }
        private double Distance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
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

            string[] regionNames = { "Северо-Западный", "Центральный", "Поволжье", "Южный", "Северо-Кавказский", "Уральский", "Сибирский", "Дальневосточный" };

            using (Font font = new Font("Arial", 16, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.Aqua))
            using (Brush shadowBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0))) // полупрозрачная тень
            using (Pen outlinePen = new Pen(Color.Black, 4))
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
                        textX -= 80 * scaleFactor; // увеличенный сдвиг
                    }
                    if (text == "Поволжье")
                    {
                        textX -= 50 * scaleFactor; // увеличенный сдвиг
                    }
                    if (text == "Северо-Западный")
                    {
                        textY += 100 * scaleFactor; // увеличенный сдвиг
                    }
                    if (text == "Дальневосточный")
                    {
                        textX -= 80 * scaleFactor; // увеличенный сдвиг
                    }
                    

                    // Контур текста через GraphicsPath
                    using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                    {
                        path.AddString(text, font.FontFamily, (int)font.Style, g.DpiY * font.Size / 72, new PointF(textX, textY), StringFormat.GenericDefault);
                        g.DrawPath(outlinePen, path);
                        g.FillPath(textBrush, path);
                    }
                }
                var sortedIndices = Enumerable.Range(0, draggableImages.Length)
                .OrderBy(i => draggablePositions[i].Y) // сначала ниже, потом выше — выше будут поверх
                .ToList();

                foreach (int i in sortedIndices)
                {
                    int scaledWidth = (int)(draggableImages[i].Width * 0.25);
                    int scaledHeight = (int)(draggableImages[i].Height * 0.25);
                    g.DrawImage(draggableImages[i], new Rectangle(draggablePositions[i], new Size(scaledWidth, scaledHeight)));
                }


            }

            using (Font font = new Font("Arial", 30, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.Black))
            {
                string topRightText = $"Верно расставлено: {correctPlacementsCount}";

                var textSize = e.Graphics.MeasureString(topRightText, font);

                float x = this.ClientSize.Width - textSize.Width - this.ClientSize.Width / 20; // 20 доля отступ справа
                float y = this.ClientSize.Height / 25; // 25 доля пикселей от верхнего края

                e.Graphics.DrawString(topRightText, font, brush, new PointF(x, y));
            }



        }

        private void FinalTestForm_Load(object sender, EventArgs e)
        {
            ShowTaskForm(); // ← Окно появится поверх
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