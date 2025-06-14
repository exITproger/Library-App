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
    public partial class NorthcaucasianMainForm : Form
    {
        public NorthcaucasianMainForm()
        {
            InitializeComponent();
        }

        private void btnOpenTest_Click(object sender, EventArgs e)
        {
            TestNorthcaucasianForm1 testNorthcaucasianForm1 = new TestNorthcaucasianForm1();
            Hide();
            testNorthcaucasianForm1.ShowDialog();
            Show();
        }
    }
}