using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class MapForm : Form
    {
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
        private const float MapScale = 0.8f;
        private const float PixelTolerance = 0.15f;

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

            LoadImages();

            CalculateScaleFactor(true);

            startTime = DateTime.Now;

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

            this.Load += MapForm_Load;
            
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
            Hide();
            mapHintForm.ShowDialog();
            Show();
        }
        private void ShowTaskForm()
        {
            using (var taskForm = new TaskForm())
            {
                taskForm.ShowDialog(this);
            }
        }
        private void SkipButton_Click(object sender, EventArgs e)
        {
            RegionMapForm regionMapForm = new RegionMapForm();
            this.Hide();
            regionMapForm.ShowDialog();
            this.Show();
        }


        private void CalculateScaleFactor(bool initial = false)
        {
            scaleFactor = MapScale * Math.Min(
                (this.ClientSize.Width - 200) / 1520f,
                (this.ClientSize.Height - 200) / 880f
            );

            baseOutlinePosition = new PointF(
                (this.ClientSize.Width - outlineMap.Width * scaleFactor) / 2,
                Math.Max(20, this.ClientSize.Height / 15));

            if (initial)
            {
                CreatePuzzlePieces();
                ShufflePieces();
                previousScaleFactor = scaleFactor; // Инициализируем previousScaleFactor
            }
            else
            {
                UpdatePuzzlePiecePositions();
            }
        }

        private void LoadImages()
        {
            outlineMap = Properties.Resources.map_background2;

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

            List<PointF> positions = new List<PointF>();
            int[] sides = new int[] { 0, 0, 1, 1, 2, 2, 3, 3 }; // 0=Top, 1=Bottom, 2=Left, 3=Right
            sides = sides.OrderBy(x => rand.Next()).ToArray(); // перемешиваем стороны

            foreach (int side in sides)
            {
                float x = 0, y = 0;

                switch (side)
                {
                    case 0: // Верх
                        x = rand.Next(padding, (int)(w - pieceWidth - padding));
                        y = padding;
                        break;
                    case 1: // Низ
                        x = rand.Next(padding, (int)(w - pieceWidth - padding));
                        y = h - pieceHeight - padding;
                        break;
                    case 2: // Лево
                        x = padding;
                        y = rand.Next(padding, (int)(h - pieceHeight - padding));
                        break;
                    case 3: // Право
                        x = w - pieceWidth - padding;
                        y = rand.Next(padding, (int)(h - pieceHeight - padding));
                        break;
                }

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
                        TimeSpan timeTaken = DateTime.Now - startTime;
                        //MessageBox.Show($"Поздравляем! Вы собрали карту за {timeTaken:mm\\:ss}!");
                        await Task.Delay(3000); // Задержка 3 секунды
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
