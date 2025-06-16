using Library_App;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Library_App
{
    public partial class TestVolgaForm1 : Form
    {
        private Timer animationTimer;
        private Dictionary<Button, AnimationState> buttonStates = new Dictionary<Button, AnimationState>();
        private Color normalColor = SystemColors.Control;
        private Color hoverColor = Color.LightBlue;
        private PictureBox backgroundImage;
        public TestVolgaForm1()
        {
            // Создаем PictureBox и задаем фоновое изображение
            backgroundImage = new PictureBox();
            backgroundImage.Dock = DockStyle.Fill;
            backgroundImage.SizeMode = PictureBoxSizeMode.Zoom;
            backgroundImage.Image = Properties.Resources.VolgaregionBackground;
            this.Controls.Add(backgroundImage);
            InitializeComponent();
            // Отправляем картинку на задний план
            backgroundImage.SendToBack();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;

            this.Resize += TestForm_Resize;

            lblAsk1.Height = 80;
            lblAsk1.Dock = DockStyle.Top;
            lblAsk1.TextAlign = ContentAlignment.MiddleCenter;

            tableLayoutPanel1.Dock = DockStyle.None;

            AdjustLayout();

            foreach (var btn in new Button[] { btnVar1, btnVar2, btnVar3, btnVar4 })
            {
                buttonStates[btn] = new AnimationState() { CurrentColor = normalColor, TargetColor = normalColor };
                btn.BackColor = normalColor;
                btn.MouseEnter += Btn_MouseEnter;
                btn.MouseLeave += Btn_MouseLeave;
            }

            animationTimer = new Timer();
            animationTimer.Interval = 15;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
            this.Shown += (s, e) => this.ActiveControl = null;
        }

        private void TestForm_Resize(object sender, EventArgs e)
        {
            AdjustLayout();
        }

        private void AdjustLayout()
        {
            lblAsk1.Dock = DockStyle.None;
            lblAsk1.TextAlign = ContentAlignment.MiddleCenter;
            lblAsk1.Height = 80;
            float headerFontSize = Math.Min(lblAsk1.Height * 0.7f, 24f);
            lblAsk1.Font = new Font("Microsoft Sans Serif", headerFontSize, FontStyle.Bold);

            // Вычисляем ширину таблицы пропорционально ширине окна
            int targetWidth = (int)(this.ClientSize.Width * 1200f / 2560f);
            // Высота по фиксированному соотношению 3:2
            int targetHeight = (int)(targetWidth * 800f / 1200f);

            tableLayoutPanel1.Size = new Size(targetWidth, targetHeight);

            // Центрируем по горизонтали и вертикали
            int totalHeight = lblAsk1.Height + tableLayoutPanel1.Height;
            int topOffset = (this.ClientSize.Height - totalHeight) / 2;
            int centerX = (this.ClientSize.Width - tableLayoutPanel1.Width) / 2;

            lblAsk1.Width = tableLayoutPanel1.Width;
            lblAsk1.Location = new Point(centerX, topOffset);
            tableLayoutPanel1.Location = new Point(centerX, lblAsk1.Bottom);

            // Размер одной ячейки
            int cellWidth = tableLayoutPanel1.ClientSize.Width / tableLayoutPanel1.ColumnCount;
            int cellHeight = tableLayoutPanel1.ClientSize.Height / tableLayoutPanel1.RowCount;

            foreach (Button btn in new Button[] { btnVar1, btnVar2, btnVar3, btnVar4 })
            {
                btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.AutoSize = false;
                btn.AutoEllipsis = false;
                btn.FlatStyle = FlatStyle.Standard;

                // Вставляем переносы по словам, чтобы текст не выходил за ширину кнопки
                btn.Text = InsertLineBreaks(btn.Text, cellWidth);

                // Подбираем шрифт, чтобы текст помещался по высоте с учётом переноса строк
                float fontSize = 24f;
                using (Graphics g = btn.CreateGraphics())
                {
                    while (fontSize > 6f)
                    {
                        using (Font testFont = new Font("Microsoft Sans Serif", fontSize))
                        {
                            SizeF textSize = g.MeasureString(btn.Text, testFont, cellWidth);
                            if (textSize.Height <= cellHeight * 0.9f)
                            {
                                btn.Font = new Font("Microsoft Sans Serif", fontSize);
                                break;
                            }
                        }
                        fontSize -= 0.5f;
                    }
                }
            }
        }


        private string InsertLineBreaks(string text, int maxWidth)
        {
            // Грубая разбивка на строки по пробелам для переноса, можно улучшить
            string[] words = text.Split(' ');
            List<string> lines = new List<string>();
            string currentLine = "";

            using (Graphics g = this.CreateGraphics())
            {
                foreach (string word in words)
                {
                    string testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
                    SizeF size = g.MeasureString(testLine, this.Font);
                    if (size.Width > maxWidth * 0.9f)
                    {
                        if (!string.IsNullOrEmpty(currentLine))
                            lines.Add(currentLine);
                        currentLine = word;
                    }
                    else
                    {
                        currentLine = testLine;
                    }
                }
                if (!string.IsNullOrEmpty(currentLine))
                    lines.Add(currentLine);
            }

            return string.Join("\n", lines);
        }


        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                buttonStates[btn].TargetColor = hoverColor;
            }
        }

        private void Btn_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                buttonStates[btn].TargetColor = normalColor;
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            bool needInvalidate = false;
            foreach (var kvp in buttonStates)
            {
                Button btn = kvp.Key;
                AnimationState state = kvp.Value;

                Color current = state.CurrentColor;
                Color target = state.TargetColor;

                if (current != target)
                {
                    int r = Approach(current.R, target.R, 10);
                    int g = Approach(current.G, target.G, 10);
                    int b = Approach(current.B, target.B, 10);

                    Color newColor = Color.FromArgb(r, g, b);
                    btn.BackColor = newColor;
                    state.CurrentColor = newColor;
                    needInvalidate = true;
                }
            }

            if (needInvalidate)
                this.Invalidate();
        }

        private int Approach(int current, int target, int step)
        {
            if (current < target)
            {
                current += step;
                if (current > target) current = target;
            }
            else if (current > target)
            {
                current -= step;
                if (current < target) current = target;
            }
            return current;
        }

        private class AnimationState
        {
            public Color CurrentColor;
            public Color TargetColor;
        }

        private void btnVar1_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar1, Color.Red);
        }

        private void btnVar2_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar2, Color.Red);
        }

        private async void btnVar3_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar3, Color.Green);

            await System.Threading.Tasks.Task.Delay(1000);

            TestVolgaForm2 form2 = new TestVolgaForm2();

            form2.ShowDialog();

        }


        private void btnVar4_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar4, Color.Red);
        }

        private void SetButtonColor(Button btn, Color color)
        {
            if (buttonStates.TryGetValue(btn, out var state))
            {
                state.CurrentColor = color;
                state.TargetColor = color;
            }
            btn.BackColor = color;
        }
    }
}