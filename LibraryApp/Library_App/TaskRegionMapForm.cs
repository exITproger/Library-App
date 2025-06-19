using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Library_App
{
    public partial class TaskRegionMapForm : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref uint pvParam, uint fWinIni);

        private const uint SPI_SETBEEP = 0x0002;
        private const uint SPIF_SENDCHANGE = 0x0002;

        private bool SetBeepEnabled(bool enable)
        {
            uint beepEnabled = enable ? 1u : 0u;
            return SystemParametersInfo(SPI_SETBEEP, 0, ref beepEnabled, SPIF_SENDCHANGE);
        }
        public TaskRegionMapForm()
        {
            SetBeepEnabled(false); // Отключить звук
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
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.WhiteSmoke;
            this.Size = new Size(formWidth, formHeight);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Padding = new Padding(20);

            // Метка с инструкцией
            Label label = new Label()
            {
                Text = "Нажимай на регион,\nчтобы узнать про него подробнее.",
                Font = new Font("Comic Sans MS", fontSize, FontStyle.Regular),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.DarkSlateGray
            };

            // Кнопка "OK"
            Button okButton = new Button()
            {
                Text = "OK",
                Font = new Font("Comic Sans MS", fontSize - 2, FontStyle.Bold),
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
