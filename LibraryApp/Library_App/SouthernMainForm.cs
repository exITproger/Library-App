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
    public partial class SouthernMainForm : Form
    {
        public SouthernMainForm()
        {
            InitializeComponent();
        }

        private void btnOpenTest_Click(object sender, EventArgs e)
        {
            TestSouthernForm1 testSouthernForm1 = new TestSouthernForm1();
            Hide();
            testSouthernForm1.ShowDialog();
            Show();
        }
    }
}
