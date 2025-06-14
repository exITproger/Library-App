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
    public partial class SiberianMainForm : Form
    {
        public SiberianMainForm()
        {
            InitializeComponent();
        }

        private void btnOpenTest_Click(object sender, EventArgs e)
        {
            TestSiberianForm1 testSiberianForm1 = new TestSiberianForm1();
            Hide();
            testSiberianForm1.ShowDialog();
            Show();
        }
    }
}
