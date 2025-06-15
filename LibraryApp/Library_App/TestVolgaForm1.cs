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

        public TestVolgaForm1()
        {
            InitializeComponent();

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
        }

        private void TestForm_Resize(object sender, EventArgs e)
        {
            AdjustLayout();
        }

        private void AdjustLayout()
        {
            float headerFontSize = Math.Min(lblAsk1.Height * 0.7f, 36f);
            lblAsk1.Font = new Font("Microsoft Sans Serif", headerFontSize, FontStyle.Bold);

            int availableHeight = this.ClientSize.Height - lblAsk1.Height;
            int tableWidth = this.ClientSize.Width / 2;
            int tableHeight = availableHeight / 2;

            tableLayoutPanel1.Size = new Size(tableWidth, tableHeight);

            int tableX = (this.ClientSize.Width - tableWidth) / 2;
            int tableY = lblAsk1.Bottom + (availableHeight - tableHeight) / 2;

            tableLayoutPanel1.Location = new Point(tableX, tableY);

            int cellWidth = tableLayoutPanel1.ClientSize.Width / tableLayoutPanel1.ColumnCount;
            int cellHeight = tableLayoutPanel1.ClientSize.Height / tableLayoutPanel1.RowCount;

            foreach (Button btn in new Button[] { btnVar1, btnVar2, btnVar3, btnVar4 })
            {
                float fontSize = 24f;
                Size textSize;
                using (Graphics g = btn.CreateGraphics())
                {
                    while (fontSize > 6f)
                    {
                        using (Font testFont = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular))
                        {
                            textSize = Size.Ceiling(g.MeasureString(btn.Text, testFont));
                            if (textSize.Width <= cellWidth * 0.9 && textSize.Height <= cellHeight * 0.9)
                            {
                                btn.Font = new Font("Microsoft Sans Serif", fontSize, FontStyle.Regular);
                                break;
                            }
                        }
                        fontSize -= 0.5f;
                    }
                }
            }
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