using Library_App;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Library_App
{
    public partial class TestVolgaForm2 : Form
    {
        private Timer animationTimer;
        private Dictionary<Button, AnimationState> buttonStates = new Dictionary<Button, AnimationState>();
        private Color normalColor = SystemColors.Control;
        private Color hoverColor = Color.LightBlue;

        public TestVolgaForm2()
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

        private async void btnVar3_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar3, Color.Green);
            // Задержка 1.0 секунды (1000 миллисекунд)
            await System.Threading.Tasks.Task.Delay(1000);

            using (var choiceForm = new ChoiceFormV())
            {
                choiceForm.ShowDialog();

                if (choiceForm.Result == ChoiceFormV.ChoiceResult.ReturnToDistrict)
                {
                    ReturnToDistrict();
                }
                else if (choiceForm.Result == ChoiceFormV.ChoiceResult.ReturnToRussiaMap)
                {
                    ReturnToRussiaMap();
                }
            }
        }

        private void ReturnToDistrict()
        {
            // Закрыть все формы кроме главной, если она у вас есть в списке открытых
            foreach (Form form in Application.OpenForms)
            {
                if (!(form is VolgaMainForm || form is RegionMapForm || form is MainMenuForm || form is MapForm))
                    form.Close();
            }

            // Проверим, открыто ли главное меню
            var volgaMainMenu = Application.OpenForms.OfType<VolgaMainForm>().FirstOrDefault();
            if (volgaMainMenu == null)
            {
                volgaMainMenu = new VolgaMainForm();
                volgaMainMenu.Show();
            }
            else
            {
                volgaMainMenu.BringToFront();
            }

            this.Close();
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
            var regionMapForm = Application.OpenForms.OfType<RegionMapForm>().FirstOrDefault();
            if (regionMapForm == null)
            {
                regionMapForm = new RegionMapForm();
                regionMapForm.Show();
            }
            else
            {
                regionMapForm.BringToFront();
            }

            this.Close();
        }

        private async void btnVar2_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar2, Color.Red);
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
    public class ChoiceFormV : Form
    {
        public enum ChoiceResult
        {
            None,
            ReturnToDistrict,
            ReturnToRussiaMap
        }

        public ChoiceResult Result { get; private set; } = ChoiceResult.None;

        public ChoiceFormV()
        {
            // Размеры - 30% от экрана
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Size = new Size((int)(screen.Width * 0.3), (int)(screen.Height * 0.3));
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Выбор действия";

            // Метка с текстом
            var label = new Label()
            {
                Text = "Выберите действие:",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = this.ClientSize.Height / 2,
                Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold)
            };
            this.Controls.Add(label);

            // Панель для кнопок
            var panel = new Panel()
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                Padding = new Padding(10)
            };
            this.Controls.Add(panel);

            var btnDistrict = new Button()
            {
                Text = "Вернуться к округу",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Left,
                Width = this.ClientSize.Width / 2 - 15,
                Margin = new Padding(5)
            };
            btnDistrict.Click += (s, e) =>
            {
                Result = ChoiceResult.ReturnToDistrict;
                this.Close();
            };
            panel.Controls.Add(btnDistrict);

            var btnRussia = new Button()
            {
                Text = "Вернуться к карте России",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Right,
                Width = this.ClientSize.Width / 2 - 15,
                Margin = new Padding(5)
            };
            btnRussia.Click += (s, e) =>
            {
                Result = ChoiceResult.ReturnToRussiaMap;
                this.Close();
            };
            panel.Controls.Add(btnRussia);
        }
    }
}