using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_App
{
    public partial class NorthwesternMainForm : Form
    {
        public NorthwesternMainForm()
        {
            InitializeComponent();
        }

        private void btnOpenTest_Click(object sender, EventArgs e)
        {
            TestNorthwesternForm1 testNorthwesternForm1 = new TestNorthwesternForm1();
            Hide();
            testNorthwesternForm1.ShowDialog();
            Show();
        }
    }
}