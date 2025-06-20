﻿using System.Drawing;
using System.Windows.Forms;

namespace Library_App
{
    partial class TestVolgaForm1
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestVolgaForm1));
            this.lblAsk1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnVar2 = new System.Windows.Forms.Button();
            this.btnVar1 = new System.Windows.Forms.Button();
            this.btnVar4 = new System.Windows.Forms.Button();
            this.btnVar3 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAsk1
            // 
            this.lblAsk1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(124)))), ((int)(((byte)(30)))));
            this.lblAsk1.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAsk1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAsk1.Location = new System.Drawing.Point(0, 0);
            this.lblAsk1.Name = "lblAsk1";
            this.lblAsk1.Size = new System.Drawing.Size(1117, 100);
            this.lblAsk1.TabIndex = 1;
            this.lblAsk1.Text = "Что из перечисленного НЕ является татарской традицией?";
            this.lblAsk1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(124)))), ((int)(((byte)(30)))));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnVar2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnVar1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnVar4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnVar3, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 100);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(20);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1117, 591);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnVar2
            // 
            this.btnVar2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVar2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVar2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(180)))), ((int)(((byte)(62)))));
            this.btnVar2.FlatAppearance.BorderSize = 5;
            this.btnVar2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVar2.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.btnVar2.Location = new System.Drawing.Point(30, 305);
            this.btnVar2.Margin = new System.Windows.Forms.Padding(10);
            this.btnVar2.Name = "btnVar2";
            this.btnVar2.Size = new System.Drawing.Size(518, 256);
            this.btnVar2.TabIndex = 1;
            this.btnVar2.Text = "Свадебный обряд «никах»";
            this.btnVar2.UseVisualStyleBackColor = false;
            this.btnVar2.Click += new System.EventHandler(this.btnVar2_Click);
            // 
            // btnVar1
            // 
            this.btnVar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVar1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(180)))), ((int)(((byte)(62)))));
            this.btnVar1.FlatAppearance.BorderSize = 5;
            this.btnVar1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVar1.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.btnVar1.Location = new System.Drawing.Point(30, 30);
            this.btnVar1.Margin = new System.Windows.Forms.Padding(10);
            this.btnVar1.Name = "btnVar1";
            this.btnVar1.Size = new System.Drawing.Size(518, 255);
            this.btnVar1.TabIndex = 0;
            this.btnVar1.Text = "Праздник Сабантуй с борьбой";
            this.btnVar1.UseVisualStyleBackColor = false;
            this.btnVar1.Click += new System.EventHandler(this.btnVar1_Click);
            // 
            // btnVar4
            // 
            this.btnVar4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVar4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVar4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(180)))), ((int)(((byte)(62)))));
            this.btnVar4.FlatAppearance.BorderSize = 5;
            this.btnVar4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVar4.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.btnVar4.Location = new System.Drawing.Point(568, 305);
            this.btnVar4.Margin = new System.Windows.Forms.Padding(10);
            this.btnVar4.Name = "btnVar4";
            this.btnVar4.Size = new System.Drawing.Size(519, 256);
            this.btnVar4.TabIndex = 3;
            this.btnVar4.Text = "Праздник Ураза-Байрам";
            this.btnVar4.UseVisualStyleBackColor = false;
            this.btnVar4.Click += new System.EventHandler(this.btnVar4_Click);
            // 
            // btnVar3
            // 
            this.btnVar3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnVar3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVar3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(180)))), ((int)(((byte)(62)))));
            this.btnVar3.FlatAppearance.BorderSize = 5;
            this.btnVar3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVar3.Font = new System.Drawing.Font("Segoe UI", 22F);
            this.btnVar3.Location = new System.Drawing.Point(568, 30);
            this.btnVar3.Margin = new System.Windows.Forms.Padding(10);
            this.btnVar3.Name = "btnVar3";
            this.btnVar3.Size = new System.Drawing.Size(519, 255);
            this.btnVar3.TabIndex = 2;
            this.btnVar3.Text = "Праздник Масленица";
            this.btnVar3.UseVisualStyleBackColor = false;
            this.btnVar3.Click += new System.EventHandler(this.btnVar3_Click);
            // 
            // TestVolgaForm1
            // 
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1117, 691);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblAsk1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestVolgaForm1";
            this.Text = "Татарские традиции - Вопрос 1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblAsk1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnVar1;
        private System.Windows.Forms.Button btnVar2;
        private System.Windows.Forms.Button btnVar3;
        private System.Windows.Forms.Button btnVar4;
    }
}