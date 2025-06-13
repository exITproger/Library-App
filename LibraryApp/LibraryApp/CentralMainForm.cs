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
    public partial class CentralMainForm : Form
    {
        public CentralMainForm()
        {
            InitializeComponent();
        }

        private void btbTestCentral_Click(object sender, EventArgs e)
        {
            TestCentralForm1 cc = new TestCentralForm1();
            Hide();
            cc.ShowDialog();
            Show();
        }
    }
}
