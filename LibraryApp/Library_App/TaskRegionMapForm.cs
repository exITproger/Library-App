using System;
using System.Drawing;
using System.Windows.Forms;

namespace Library_App
{
    public partial class TaskRegionMapForm : Form
    {
        public TaskRegionMapForm()
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.Size = new Size(600, 400);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;

            Label label = new Label();
            label.Text = "Нажимай на регион,\n чтобы узнать про него подробнее.";
            label.Font = new Font("Arial", 20, FontStyle.Regular);
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;

            Button okButton = new Button();
            okButton.Text = "OK";
            okButton.Font = new Font("Arial", 18, FontStyle.Bold);
            okButton.Dock = DockStyle.Bottom;
            okButton.Height = 40;
            okButton.Click += (s, e) => this.Close();

            this.Controls.Add(label);
            this.Controls.Add(okButton);
        }
    }
}