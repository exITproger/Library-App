
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Library_App
{
    public partial class TaskForm : Form
    {
        public TaskForm()
        {
            // Получаем размеры экрана
            var screen = Screen.PrimaryScreen.WorkingArea;
            int formWidth, formHeight, fontSize;

            // Адаптация под разрешение экрана
            if (screen.Width > 3500)
            {
                formWidth = 1200;
                formHeight = 900;
                fontSize = 28;
            }
            else if (screen.Width > 2500)
            {
                formWidth = 900;
                formHeight = 600;
                fontSize = 24;
            }
            else if (screen.Width > 1900)
            {
                formWidth = 600;
                formHeight = 400;
                fontSize = 20;
            }
            else
            {
                formWidth = 400;
                formHeight = 300;
                fontSize = 16;
            }

            // Настройка формы
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Size = new Size(formWidth, formHeight);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Padding = new Padding(20);

            // Метка с инструкцией
            Label label = new Label()
            {
                Text = "Соберите карту России,\nразместив все округа на правильные места.",
                Font = new Font("Arial", fontSize, FontStyle.Regular),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.DarkSlateGray
            };

            // Кнопка "OK"
            Button okButton = new Button()
            {
                Text = "OK",
                Font = new Font("Arial", fontSize - 2, FontStyle.Bold),
                Dock = DockStyle.Bottom,
                Height = (int)(formHeight * 0.15),
                BackColor = Color.LightSteelBlue,
                FlatStyle = FlatStyle.Flat
            };
            okButton.Click += (s, e) => this.Close();

            // Добавление элементов
            this.Controls.Add(label);
            this.Controls.Add(okButton);
        }
    }
}
