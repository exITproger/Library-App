using Library_App;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Library_App
{
    public partial class TestCentralForm2 : Form
    {
        private Timer animationTimer;
        private Dictionary<Button, AnimationState> buttonStates = new Dictionary<Button, AnimationState>();
        // Цвета для анимации (изменяй под себя)
        private Color normalColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(0)))), ((int)(((byte)(13)))));
        private Color hoverColor = Color.LightBlue;
        private PictureBox backgroundImage;
        public TestCentralForm2()
        {
            // Создаем PictureBox и задаем фоновое изображение
            backgroundImage = new PictureBox();
            backgroundImage.Dock = DockStyle.Fill;
            backgroundImage.SizeMode = PictureBoxSizeMode.Zoom;
            backgroundImage.Image = Properties.Resources.CentralBackground1;
            this.Controls.Add(backgroundImage);
            InitializeComponent();
            // Отправляем картинку на задний план
            backgroundImage.SendToBack();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            lblAsk1.Paint += (sender, e) =>
            {
                int borderWidth = 5; // Толщина рамки
                Color borderColor = ColorTranslator.FromHtml("#5b0207"); // Цвет рамки

                ControlPaint.DrawBorder(
                    e.Graphics,
                    lblAsk1.ClientRectangle,
                    borderColor,
                    borderWidth, ButtonBorderStyle.Solid, // Верх
                    borderColor,
                    borderWidth, ButtonBorderStyle.Solid, // Право
                    borderColor,
                    borderWidth, ButtonBorderStyle.Solid, // Низ
                    borderColor,
                    0, ButtonBorderStyle.Solid // Лево
                );
            };
            tableLayoutPanel1.Paint += (sender, e) =>
            {
                int borderWidth = 5; // Толщина рамки
                Color borderColor = ColorTranslator.FromHtml("#5b0207"); // Цвет рамки

                ControlPaint.DrawBorder(
                    e.Graphics,
                    tableLayoutPanel1.ClientRectangle,
                    borderColor,
                    borderWidth, ButtonBorderStyle.Solid, // Верх
                    borderColor,
                    0, ButtonBorderStyle.Solid, // Право
                    borderColor,
                    borderWidth, ButtonBorderStyle.Solid, // Низ
                    borderColor,
                    borderWidth, ButtonBorderStyle.Solid // Лево
                );
            };
            this.Resize += TestCentralForm1_Resize;

            // Меняем заголовок: фиксируем высоту и dock top
            lblAsk1.Height = 80;
            lblAsk1.Dock = DockStyle.Top;
            lblAsk1.TextAlign = ContentAlignment.MiddleCenter;

            // Снимаем Dock у таблицы, чтобы вручную позиционировать и задавать размер
            tableLayoutPanel1.Dock = DockStyle.None;

            AdjustLayout();
            // Инициализация состояний кнопок
            foreach (var btn in new Button[] { btnVar1, btnVar2, btnVar3, btnVar4 })
            {
                buttonStates[btn] = new AnimationState() { CurrentColor = normalColor, TargetColor = normalColor };
                btn.BackColor = normalColor;
                btn.Paint += (sender, e) =>
                {
                    int borderWidth = 5; // Толщина рамки
                    Color borderColor = ColorTranslator.FromHtml("#5b0207"); // Цвет рамки

                    ControlPaint.DrawBorder(
                        e.Graphics,
                        btn.ClientRectangle,
                        borderColor,
                        borderWidth, ButtonBorderStyle.Solid,
                        borderColor,
                        borderWidth, ButtonBorderStyle.Solid,
                        borderColor,
                        borderWidth, ButtonBorderStyle.Solid,
                        borderColor,
                        borderWidth, ButtonBorderStyle.Solid
                    );
                };
                //btn.MouseEnter += Btn_MouseEnter;
                //btn.MouseLeave += Btn_MouseLeave;
            }

            animationTimer = new Timer();
            animationTimer.Interval = 15; // 15 мс для плавности
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
            this.Shown += (s, e) => this.ActiveControl = null;
        }

        private void TestCentralForm1_Resize(object sender, EventArgs e)
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
        private void Btn_MouseEnter(object sender, System.EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                buttonStates[btn].TargetColor = hoverColor;
            }
        }

        private void Btn_MouseLeave(object sender, System.EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                buttonStates[btn].TargetColor = normalColor;
            }
        }

        private void AnimationTimer_Tick(object sender, System.EventArgs e)
        {
            bool needInvalidate = false;
            foreach (var kvp in buttonStates)
            {
                Button btn = kvp.Key;
                AnimationState state = kvp.Value;

                // Плавно приближаемся к target цвету
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
                this.Invalidate(); // обновляем форму
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
        // Класс для хранения текущего и целевого цвета
        private class AnimationState
        {
            public Color CurrentColor;
            public Color TargetColor;
        }
        private void btnVar1_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar1, Color.Red);
        }

        private async void btnVar2_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar2, Color.Green);
            // Задержка 1.0 секунды (1000 миллисекунд)
            await System.Threading.Tasks.Task.Delay(1000);

            using (var choiceForm = new ChoiceFormC())
            {
                choiceForm.ShowDialog();

                if (choiceForm.Result == ChoiceFormC.ChoiceResult.ReturnToDistrict)
                {
                    ReturnToDistrict();
                }
                else if (choiceForm.Result == ChoiceFormC.ChoiceResult.ReturnToRussiaMap)
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
                if (!(form is CentralMainForm || form is RegionMapForm || form is MainMenuForm || form is MapForm))
                {
                    form.Close();
                    GC.Collect();
                }
            }

            // Проверим, открыто ли главное меню
            var centralMainMenu = Application.OpenForms.OfType<CentralMainForm>().FirstOrDefault();
            if (centralMainMenu == null)
            {
                centralMainMenu = new CentralMainForm();
                centralMainMenu.Show();
            }
            else
            {
                centralMainMenu.BringToFront();
            }

            this.Close();
            GC.Collect();
        }
        private void ReturnToRussiaMap()
        {
            // Закрыть все формы кроме главной, если она у вас есть в списке открытых
            foreach (Form form in Application.OpenForms)
            {
                if (!(form is RegionMapForm || form is MainMenuForm || form is MapForm))
                {
                    form.Close();
                    GC.Collect();
                }    
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
            GC.Collect();
        }

        private void btnVar3_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar3, Color.Red);
        }

        private void btnVar4_Click(object sender, EventArgs e)
        {
            SetButtonColor(btnVar4, Color.Red);
        }

        // Общий метод установки цвета с обновлением анимации
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
    public class ChoiceFormC : Form
    {
        public enum ChoiceResult
        {
            None,
            ReturnToDistrict,
            ReturnToRussiaMap
        }

        public ChoiceResult Result { get; private set; } = ChoiceResult.None;

        public ChoiceFormC()
        {


            // Определяем размеры формы в зависимости от разрешения экрана
            var screen = Screen.PrimaryScreen.WorkingArea;
            int formWidth, formHeight, fontSize;

            if (screen.Width > 3500)
            {
                formWidth = 1200;
                formHeight = 800;
                fontSize = 28;
            }
            else if (screen.Width > 2500)
            {
                formWidth = 1000;
                formHeight = 700;
                fontSize = 24;
            }
            else if (screen.Width > 1900)
            {
                formWidth = 800;
                formHeight = 600;
                fontSize = 20;
            }
            else
            {
                formWidth = 600;
                formHeight = 400;
                fontSize = 16;
            }

            this.ClientSize = new Size(formWidth, formHeight);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "Выбор действия";
            this.BackColor = Color.WhiteSmoke;
            this.Padding = new Padding(20);

            // Основной контейнер
            var mainPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3
            };
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

            // Заголовок
            var label = new Label()
            {
                Text = "Выберите действие:",
                Font = new Font("Bold", fontSize, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.DarkSlateBlue
            };
            mainPanel.Controls.Add(label, 0, 0);

            // Разделитель
            mainPanel.Controls.Add(new Panel(), 0, 1);

            // Контейнер для кнопок
            var buttonPanel = new Panel()
            {
                Dock = DockStyle.Fill
            };

            // Размер кнопок
            Size buttonSize = new Size((int)(formWidth * 0.35), (int)(formHeight * 0.2));

            var btnDistrict = new Button()
            {
                Text = "Вернуться к округу",
                DialogResult = DialogResult.OK,
                Size = buttonSize,
                Font = new Font("Bold", fontSize - 2),
                BackColor = Color.LightSteelBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnDistrict.Click += (s, e) =>
            {
                Result = ChoiceResult.ReturnToDistrict;

            };

            var btnRussia = new Button()
            {
                Text = "Вернуться к карте России",
                DialogResult = DialogResult.Cancel,
                Size = buttonSize,
                Font = new Font("Bold", fontSize - 2),
                BackColor = Color.LightSkyBlue,
                FlatStyle = FlatStyle.Flat
            };
            btnRussia.Click += (s, e) =>
            {
                Result = ChoiceResult.ReturnToRussiaMap;
                this.Close();
            };

            // Расположение кнопок при изменении размера
            buttonPanel.Resize += (s, e) =>
            {
                int spacing = formWidth / 20;
                int totalWidth = btnDistrict.Width + btnRussia.Width + spacing;

                int xStart = (buttonPanel.Width - totalWidth) / 2;
                int yStart = (buttonPanel.Height - btnDistrict.Height) / 2;

                btnDistrict.Location = new Point(xStart, yStart);
                btnRussia.Location = new Point(xStart + btnDistrict.Width + spacing, yStart);
            };

            buttonPanel.Controls.Add(btnDistrict);
            buttonPanel.Controls.Add(btnRussia);

            mainPanel.Controls.Add(buttonPanel, 0, 2);
            this.Controls.Add(mainPanel);

        }
        
    }


}