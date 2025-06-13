using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryApp
{
    public partial class VolgaSecondForm : Form
    {
        public VolgaSecondForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VolgaMainForm cc = new VolgaMainForm();
            Close();
            cc.ShowDialog();
            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestTatarForm1 cc = new TestTatarForm1();
            Close();
            cc.ShowDialog();
            Show();
        }
    }
}
