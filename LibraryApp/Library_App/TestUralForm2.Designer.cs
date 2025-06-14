using System.Drawing;
using System.Windows.Forms;

namespace Library_App
{
    partial class TestUralForm2
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private void InitializeComponent()
        {
            this.lblAsk1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnVar1 = new System.Windows.Forms.Button();
            this.btnVar2 = new System.Windows.Forms.Button();
            this.btnVar3 = new System.Windows.Forms.Button();
            this.btnVar4 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAsk1
            // 
            this.lblAsk1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAsk1.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAsk1.Location = new System.Drawing.Point(0, 0);
            this.lblAsk1.Name = "lblAsk1";
            this.lblAsk1.Size = new System.Drawing.Size(1523, 100);
            this.lblAsk1.TabIndex = 1;
            this.lblAsk1.Text = "Дополните примету центрального округа «Если в доме паук сплетет паутину в углу …»";
            this.lblAsk1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnVar1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnVar2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnVar3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnVar4, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 100);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Padding = new Padding(20);
            this.tableLayoutPanel1.BackColor = Color.WhiteSmoke;
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1523, 786);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnVar1
            // 
            this.btnVar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVar1.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.btnVar1.Margin = new Padding(10);
            this.btnVar1.BackColor = Color.White;
            this.btnVar1.FlatStyle = FlatStyle.Flat;
            this.btnVar1.FlatAppearance.BorderColor = Color.Gray;
            this.btnVar1.FlatAppearance.BorderSize = 2;
            this.btnVar1.Name = "btnVar1";
            this.btnVar1.Text = "«это к богатому урожаю»";
            this.btnVar1.Click += new System.EventHandler(this.btnVar1_Click);
            // 
            // btnVar2
            // 
            this.btnVar2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVar2.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.btnVar2.Margin = new Padding(10);
            this.btnVar2.BackColor = Color.White;
            this.btnVar2.FlatStyle = FlatStyle.Flat;
            this.btnVar2.FlatAppearance.BorderColor = Color.Gray;
            this.btnVar2.FlatAppearance.BorderSize = 2;
            this.btnVar2.Name = "btnVar2";
            this.btnVar2.Text = "«это к деньгам и благополучию в доме»";
            this.btnVar2.Click += new System.EventHandler(this.btnVar2_Click);
            // 
            // btnVar3
            // 
            this.btnVar3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVar3.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.btnVar3.Margin = new Padding(10);
            this.btnVar3.BackColor = Color.White;
            this.btnVar3.FlatStyle = FlatStyle.Flat;
            this.btnVar3.FlatAppearance.BorderColor = Color.Gray;
            this.btnVar3.FlatAppearance.BorderSize = 2;
            this.btnVar3.Name = "btnVar3";
            this.btnVar3.Text = "«это к несчастью»";
            this.btnVar3.Click += new System.EventHandler(this.btnVar3_Click);
            // 
            // btnVar4
            // 
            this.btnVar4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVar4.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.btnVar4.Margin = new Padding(10);
            this.btnVar4.BackColor = Color.White;
            this.btnVar4.FlatStyle = FlatStyle.Flat;
            this.btnVar4.FlatAppearance.BorderColor = Color.Gray;
            this.btnVar4.FlatAppearance.BorderSize = 2;
            this.btnVar4.Name = "btnVar4";
            this.btnVar4.Text = "«это к удаче на долгие годы»";
            this.btnVar4.Click += new System.EventHandler(this.btnVar4_Click);
            // 
            // TestCentralForm2
            // 
            this.ClientSize = new System.Drawing.Size(1523, 886);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblAsk1);
            this.Name = "TestCentralForm2";
            this.Text = "Вопрос";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.BackColor = Color.WhiteSmoke;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblAsk1;
        private System.Windows.Forms.Button btnVar1;
        private System.Windows.Forms.Button btnVar2;
        private System.Windows.Forms.Button btnVar3;
        private System.Windows.Forms.Button btnVar4;
    }
}