using Library_App;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_App
{
    public partial class MapForm : Form
    {
        private System.Windows.Forms.Timer timer;
        private List<PuzzlePiece> pieces = new List<PuzzlePiece>();
        private PuzzlePiece selectedPiece = null;
        private Button backButton;
        private Button hintButton;
        private Point offset;
        private Bitmap outlineMap;
        private int correctPieces = 0;
        private DateTime startTime;
        private float scaleFactor = 1.0f;
        private float previousScaleFactor = 1.0f;
        private PointF baseOutlinePosition = new PointF(400, 50);
        private const float MapScale = 0.9f;
        private const float PixelTolerance = 0.15f;
        private string[] regionNames = new string[]
        {
            "Северо-Западный", "Центральный", "Приволжский", "Южный",
            "Северо-Кавказский", "Уральский", "Сибирский", "Дальневосточный"
        };

        private PictureBox btnBack;
        private PictureBox btnHint;
        private PictureBox btnSkip;
        Size backButtonOriginalSize = new Size(1300, 520);
        Size otherButtonsOriginalSize = new Size(1509, 520);

        private Size currentButtonSize;
        private float buttonWidthRatio = 100f / 1520f; // примерное соотношение кнопки к ширине карты

        private Bitmap[] districtImages;

        private Point[] districtCenters = new Point[]
        {
            new Point(50, 0),   // Northwestern
            new Point(112, 378),   // Central
            new Point(215, 449),    // Volga
            new Point(0, 544),   // Southern
            new Point(69, 668),   // Northcaucasian
            new Point(421, 282),   // Ural
            new Point(596, 79),  // Siberian
            new Point(968, 110),  // Fareast (самый правый)
        };

        public MapForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Собери карту России - Федеральные округа";
            this.BackColor = Color.White;
            this.Resize += MainForm_Resize;

            // Инициализация таймера
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // Обновление каждую секунду
            timer.Tick += Timer_Tick;
            timer.Start();

            LoadImages();

            CalculateScaleFactor(true);
            // Инициализация кнопок (вместо старых Button)
            InitializeButtons();
            startTime = DateTime.Now;
            /*
            // === КНОПКА НАЗАД ===
            backButton = new Button();
            backButton.Text = "← Назад";
            backButton.Font = new Font("Arial", 30, FontStyle.Bold);
            backButton.BackColor = Color.LightGray;
            backButton.Location = new Point(20, 20);
            backButton.AutoSize = true;
            backButton.Click += (s, e) => this.Close();
            this.Controls.Add(backButton);

            // === КНОПКА ПОДСКАЗКА ===
            Button hintButton = new Button();
            hintButton.Text = "Подсказка";
            hintButton.Font = new Font("Arial", 25, FontStyle.Regular);
            hintButton.BackColor = Color.LightGray;
            hintButton.AutoSize = true;
            hintButton.Location = new Point(20, backButton.Bottom + 20);
            hintButton.Click += HintButton_Click;
            this.Controls.Add(hintButton);

            // === КНОПКА "ПРОПУСТИТЬ" ===
            Button skipButton = new Button();
            skipButton.Text = "Пропустить";
            skipButton.Font = new Font("Arial", 25, FontStyle.Regular);
            skipButton.BackColor = Color.LightGray;
            skipButton.AutoSize = true;
            skipButton.Location = new Point(20, hintButton.Bottom + 20);
            skipButton.Click += SkipButton_Click;
            this.Controls.Add(skipButton);
            */
            

            this.Load += MapForm_Load;

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Просто вызываем перерисовку, чтобы обновить время
            this.Invalidate();
        }
        private void InitializeButtons()
        {
            CalculateButtonSize();

            // Создаём кнопки с изображениями из ресурсов (нужно добавить эти картинки в ресурсы)
            btnBack = CreateImageButton(Properties.Resources.назад);   // например, стрелка назад
            btnHint = CreateImageButton(Properties.Resources.Подсказка);    // иконка подсказки
            btnSkip = CreateImageButton(Properties.Resources.пропустить);    // иконка пропустить

            btnBack.Click += (s, e) => this.Close();
            btnHint.Click += HintButton_Click;
            btnSkip.Click += SkipButton_Click;

            this.Controls.Add(btnBack);
            this.Controls.Add(btnHint);
            this.Controls.Add(btnSkip);

            UpdateButtonPositions();

            // Обновляем размеры и позиции при изменении размера формы
            this.Resize += (s, e) =>
            {
                CalculateButtonSize();
                UpdateButtonSizes();
                UpdateButtonPositions();
            };
        }

        private PictureBox CreateImageButton(Image image)
        {
            var button = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Size = currentButtonSize,
                Cursor = Cursors.Hand
            };

            button.MouseEnter += (s, e) => ScaleButton(button, 1.05f);
            button.MouseLeave += (s, e) => ScaleButton(button, 1.0f);
            button.MouseDown += (s, e) => ScaleButton(button, 0.95f);
            button.MouseUp += (s, e) => ScaleButton(button, 1.05f);

            return button;
        }

        private void CalculateButtonSize()
        {
            int buttonWidth = (int)(this.ClientSize.Width * buttonWidthRatio);
            int buttonHeight = buttonWidth; // квадратные кнопки, можно менять
            currentButtonSize = new Size(buttonWidth, buttonHeight);
        }

        private void UpdateButtonSizes()
        {
            if (btnBack != null)
                btnBack.Size = currentButtonSize;

            if (btnHint != null)
                btnHint.Size = new Size(
                    (int)(currentButtonSize.Width * 1.1f),
                    (int)(currentButtonSize.Height * 1.15f));

            if (btnSkip != null)
                btnSkip.Size = new Size(
                    (int)(currentButtonSize.Width * 1.1f),
                    (int)(currentButtonSize.Height * 1.15f));
        }

        private void UpdateButtonPositions()
        {
            // Базовые значения отступов для 2K (2560x1440)
            const int referencePaddingL = 10;    // Левый отступ
            const int referencePaddingH = -50;  // Вертикальный отступ между кнопками
            const int referencePadding1 = -20;  // Верхний отступ

            // Вычисляем масштабный коэффициент
            float scaleX = (float)this.ClientSize.Width / 2560f;
            float scaleY = (float)this.ClientSize.Height / 1440f;
            float scale = Math.Min(scaleX, scaleY); // Берем минимальный масштаб

            // Масштабируем отступы
            int paddingL = (int)(referencePaddingL * scale);
            int paddingH = (int)(referencePaddingH * scale);
            int padding1 = (int)(referencePadding1 * scale);

            if (btnBack != null)
                btnBack.Location = new Point(paddingL, padding1);

            if (btnHint != null)
                btnHint.Location = new Point(paddingL, btnBack.Bottom + paddingH);





            if (btnSkip != null)
                btnSkip.Location = new Point(paddingL, btnHint.Bottom + paddingH);
        }

        private void ScaleButton(PictureBox button, float scale)
        {
            int newWidth = (int)(currentButtonSize.Width * scale*1);
            int newHeight = (int)(currentButtonSize.Height * scale);

            // Центрируем кнопку при изменении размера (чтобы она не сдвигала соседей)
            int deltaX = (newWidth - button.Width) / 2;
            int deltaY = (newHeight - button.Height) / 2;

            button.SuspendLayout();

            // Смещаем позицию, чтобы сохранить центр в том же месте
            button.Location = new Point(button.Location.X - deltaX, button.Location.Y - deltaY);



            button.Size = new Size(newWidth, newHeight);

            button.ResumeLayout();
        }

        private void MapForm_Load(object sender, EventArgs e)
        {
            ShowTaskForm(); // ← Окно появится поверх
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            CalculateScaleFactor();
            this.Invalidate();
        }
        private void HintButton_Click(object sender, EventArgs e)
        {
            MapHintForm mapHintForm = new MapHintForm();

            mapHintForm.ShowDialog();

        }
        private void ShowTaskForm()
        {
            TaskForm taskForm = new TaskForm();
            taskForm.ShowDialog();
        }
        private void SkipButton_Click(object sender, EventArgs e)
        {
            RegionMapForm regionMapForm = new RegionMapForm();

            regionMapForm.ShowDialog();

        }


        private void CalculateScaleFactor(bool initial = false)
        {
            // Рассчитываем масштаб с учетом сохранения пропорций
            float widthScale = (this.ClientSize.Width - 200) / (float)outlineMap.Width;
            float heightScale = (this.ClientSize.Height - 200) / (float)outlineMap.Height;
            scaleFactor = MapScale * Math.Min(widthScale, heightScale);

            // Центрируем карту по вертикали
            baseOutlinePosition = new PointF(
                (this.ClientSize.Width - outlineMap.Width * scaleFactor) / 2,
                (this.ClientSize.Height - outlineMap.Height * scaleFactor) / 2); // Центр по высоте

            if (initial)
            {
                CreatePuzzlePieces();
                ShufflePieces();
                previousScaleFactor = scaleFactor;
            }
            else
            {
                UpdatePuzzlePiecePositions();
            }
        }

        private void LoadImages()
        {
            outlineMap = Properties.Resources.map_copy; // было map_background2

            districtImages = new Bitmap[]
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

            if (outlineMap == null || districtImages == null || districtImages.Length != 8)
                throw new Exception("Ошибка загрузки изображений карты или округов.");
        }

        private void CreatePuzzlePieces()
        {
            pieces.Clear();

            for (int i = 0; i < 8; i++)
            {
                var image = districtImages[i];
                if (image == null) continue;

                PuzzlePiece piece = new PuzzlePiece(new Bitmap(image), i + 1)
                {
                    CorrectPosition = new PointF(
                        baseOutlinePosition.X + districtCenters[i].X * scaleFactor,
                        baseOutlinePosition.Y + districtCenters[i].Y * scaleFactor),

                    BaseSize = new SizeF(image.Width, image.Height),
                    IsCorrectlyPlaced = false,
                    Position = new PointF(0, 0)
                };

                pieces.Add(piece);
            }
        }

        private void UpdatePuzzlePiecePositions()
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                pieces[i].CorrectPosition = new PointF(
                    baseOutlinePosition.X + districtCenters[i].X * scaleFactor,
                    baseOutlinePosition.Y + districtCenters[i].Y * scaleFactor);

                if (pieces[i].IsCorrectlyPlaced)
                {
                    pieces[i].Position = pieces[i].CorrectPosition;
                }
                else
                {
                    float scaleRatio = scaleFactor / previousScaleFactor;

                    var pos = pieces[i].Position;

                    // Переводим текущую позицию в относительные координаты
                    float relativeX = (pos.X - baseOutlinePosition.X) / previousScaleFactor;
                    float relativeY = (pos.Y - baseOutlinePosition.Y) / previousScaleFactor;

                    // Обновляем позицию с новым масштабом и базовой позицией
                    pieces[i].Position = new PointF(
                        baseOutlinePosition.X + relativeX * scaleFactor,
                        baseOutlinePosition.Y + relativeY * scaleFactor);
                }
            }

            previousScaleFactor = scaleFactor; // обновляем масштаб после пересчёта
        }

        private void ShufflePieces()
        {
            Random rand = new Random();
            int padding = 20;

            float pieceWidth = districtImages[0].Width * scaleFactor;
            float pieceHeight = districtImages[0].Height * scaleFactor;

            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

            // Calculate the left margin (20% of screen width)
            int leftMargin = (int)(w * 0.2);

            // Adjust available width
            int availableWidth = w - leftMargin - padding;

            List<PointF> positions = new List<PointF>();

            // We'll use 4 sides (top, bottom, right, and left-but-only-the-right-part)
            // Since we're leaving left 20% empty, we'll adjust the left side spawn
            int[] sides = new int[] { 0, 0, 1, 1, 2, 3, 3, 3 }; // 0=Top, 1=Bottom, 2=Right, 3=Left (adjusted)
            sides = sides.OrderBy(x => rand.Next()).ToArray();

            foreach (int side in sides)
            {
                float x = 0, y = 0;

                switch (side)
                {
                    case 0: // Top (within right 80%)
                        x = rand.Next(leftMargin + padding, (int)(leftMargin + availableWidth - pieceWidth - padding));
                        y = padding;
                        break;
                    case 1: // Bottom (within right 80%)
                        x = rand.Next(leftMargin + padding, (int)(leftMargin + availableWidth - pieceWidth - padding));
                        y = h - pieceHeight - padding;
                        break;
                    case 2: // Right edge
                        x = w - pieceWidth - padding;
                        y = rand.Next(padding, (int)(h - pieceHeight - padding));
                        break;
                    case 3: // Left side (but only the part after 20%)
                        x = leftMargin + padding;
                        y = rand.Next(padding, (int)(h - pieceHeight - padding));
                        break;
                }

                // Double-check bounds
                x = Math.Max(leftMargin + padding, Math.Min(w - pieceWidth - padding, x));
                y = Math.Max(padding, Math.Min(h - pieceHeight - padding, y));

                positions.Add(new PointF(x, y));
            }

            for (int i = 0; i < pieces.Count; i++)
            {
                pieces[i].Position = positions[i];
            }
        }






        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(outlineMap,
                baseOutlinePosition.X,
                baseOutlinePosition.Y,
                outlineMap.Width * scaleFactor,
                outlineMap.Height * scaleFactor);

            // Сначала — правильно положенные
            foreach (var piece in pieces)
                if (piece.IsCorrectlyPlaced)
                    DrawPiece(g, piece, piece.CorrectPosition);

            // Потом — ещё не размещённые
            foreach (var piece in pieces)
                if (!piece.IsCorrectlyPlaced)
                    DrawPiece(g, piece, piece.Position);

            /*foreach (var piece in pieces)
            {
                var correctRect = new RectangleF(
                    piece.CorrectPosition.X,
                    piece.CorrectPosition.Y,
                    piece.BaseSize.Width * scaleFactor,
                    piece.BaseSize.Height * scaleFactor);

                using (var pen = new Pen(Color.Green, 3))
                {
                    e.Graphics.DrawRectangle(pen, Rectangle.Round(correctRect));
                }
            }*/
            DrawInfo(g);
        }

        private void DrawPiece(Graphics g, PuzzlePiece piece, PointF position)
        {
            var destRect = new RectangleF(
                position.X, position.Y,
                piece.BaseSize.Width * scaleFactor,
                piece.BaseSize.Height * scaleFactor);

            g.DrawImage(piece.Image, destRect,
                new RectangleF(0, 0, piece.Image.Width, piece.Image.Height),
                GraphicsUnit.Pixel);

            // Убраны обводка и цифра
            if (piece.IsCorrectlyPlaced)
            {
                string label = regionNames[piece.Number - 1];

                float baseFontSize = 16f;
                float scaledFontSize = baseFontSize * scaleFactor;

                using (Font font = new Font("Segoe UI", scaledFontSize, FontStyle.Bold, GraphicsUnit.Pixel))
                {
                    SizeF textSize = g.MeasureString(label, font);

                    float textX = destRect.X + (destRect.Width - textSize.Width) / 2;
                    float textY = destRect.Y + (destRect.Height - textSize.Height) / 2;

                    // Сдвиг влево, если подпись — "Центральный"
                    if (label.Trim().Equals("Центральный", StringComparison.OrdinalIgnoreCase))
                    {
                        textX -= 60 * scaleFactor; // на 60 пикселей влево (можно подправить)
                    }

                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        g.DrawString(label, font, textBrush, textX, textY);
                    }
                }
            }
        }

        private void DrawInfo(Graphics g)
        {
            string info = $"Собрано: {correctPieces}/8 федеральных округов";
            TimeSpan timeTaken = DateTime.Now - startTime;
            string timeInfo = $"Время: {timeTaken:mm\\:ss}";

            var font = new Font("Arial", 16 * scaleFactor);
            SizeF infoSize = g.MeasureString(info, font);

            float panelWidth = infoSize.Width + 40;
            float panelHeight = infoSize.Height * 2 + 20;

            float x = this.ClientSize.Width - panelWidth - 20; // справа
            float y = 20;

            g.FillRectangle(Brushes.WhiteSmoke, x, y, panelWidth, panelHeight);
            g.DrawRectangle(Pens.Gray, x, y, panelWidth, panelHeight);

            g.DrawString(info, font, Brushes.Black, x + 20, y + 10);
            g.DrawString(timeInfo, new Font("Arial", 14 * scaleFactor), Brushes.DarkBlue, x + 20, y + 10 + infoSize.Height + 5);
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            timer.Stop();
            timer.Dispose();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            for (int i = pieces.Count - 1; i >= 0; i--)
            {
                var piece = pieces[i];
                if (!piece.IsCorrectlyPlaced)
                {
                    float width = piece.BaseSize.Width * scaleFactor;
                    float height = piece.BaseSize.Height * scaleFactor;

                    if (e.X >= piece.Position.X && e.X <= piece.Position.X + width &&
                        e.Y >= piece.Position.Y && e.Y <= piece.Position.Y + height)
                    {
                        selectedPiece = piece;
                        offset = new Point(
                            (int)(e.X - piece.Position.X),
                            (int)(e.Y - piece.Position.Y));
                        this.Cursor = Cursors.Hand;
                        this.Invalidate();
                        break;
                    }
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (selectedPiece != null)
            {
                float width = selectedPiece.BaseSize.Width * scaleFactor;
                float height = selectedPiece.BaseSize.Height * scaleFactor;

                float newX = e.X - offset.X;
                float newY = e.Y - offset.Y;

                // Ограничиваем по X
                newX = Math.Max(0, Math.Min(this.ClientSize.Width - width, newX));

                // Ограничиваем по Y
                newY = Math.Max(0, Math.Min(this.ClientSize.Height - height, newY));

                selectedPiece.Position = new PointF(newX, newY);
                this.Invalidate();
            }
        }


        protected override async void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (selectedPiece != null)
            {
                if (IsCloseEnough(selectedPiece))
                {
                    selectedPiece.IsCorrectlyPlaced = true;
                    selectedPiece.Position = selectedPiece.CorrectPosition;
                    correctPieces++;

                    if (correctPieces == 8)
                    {
                        timer.Stop();
                        TimeSpan timeTaken = DateTime.Now - startTime;
                        //MessageBox.Show($"Поздравляем! Вы собрали карту за {timeTaken:mm\\:ss}!");
                        await Task.Delay(1500); // Задержка 1,5 секунды
                        // Показ формы с кликабельными регионами
                        RegionMapForm regionMapForm = new RegionMapForm();
                        this.Hide();
                        regionMapForm.ShowDialog();
                        this.Show();
                    }
                }

                selectedPiece = null;
                this.Cursor = Cursors.Default;
                this.Invalidate();
            }
        }

        private bool IsCloseEnough(PuzzlePiece piece)
        {
            float width = piece.BaseSize.Width * scaleFactor;
            float height = piece.BaseSize.Height * scaleFactor;

            PointF centerCurrent = new PointF(
                piece.Position.X + width / 2,
                piece.Position.Y + height / 2);

            PointF centerCorrect = new PointF(
                piece.CorrectPosition.X + width / 2,
                piece.CorrectPosition.Y + height / 2);

            float dx = centerCurrent.X - centerCorrect.X;
            float dy = centerCurrent.Y - centerCorrect.Y;

            float distance = (float)Math.Sqrt(dx * dx + dy * dy);

            float tolerance = Math.Min(width, height) * PixelTolerance;

            Console.WriteLine($"Piece {piece.Number}: Distance = {distance}, Tolerance = {tolerance}");

            return distance <= tolerance;
        }
    }

    public class PuzzlePiece
    {
        public Bitmap Image { get; set; }
        public PointF Position { get; set; }
        public PointF CorrectPosition { get; set; }
        public bool IsCorrectlyPlaced { get; set; }
        public int Number { get; set; }
        public SizeF BaseSize { get; set; }

        public PuzzlePiece(Bitmap image, int number)
        {
            Image = image;
            Number = number;
            IsCorrectlyPlaced = false;
        }
    }
}