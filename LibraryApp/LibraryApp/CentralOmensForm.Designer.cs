using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class CentralOmensForm : Form
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        partial void InitializeComponent();
    }
}