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
    public partial class FareastenMainForm : Form
    {
        public FareastenMainForm()
        {
            InitializeComponent();
        }

        private void btnOpenTest_Click(object sender, EventArgs e)
        {
            TestFareastenForm1 testFareastenForm1 = new TestFareastenForm1();
            Hide();
            testFareastenForm1.ShowDialog();
            Show();
        }
    }
}
