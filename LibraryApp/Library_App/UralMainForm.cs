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
    public partial class UralMainForm : Form
    {
        public UralMainForm()
        {
            InitializeComponent();
        }

        private void btnOpenTest_Click(object sender, EventArgs e)
        {
            TestUralForm1 testUralForm1 = new TestUralForm1();
            Hide();
            testUralForm1.ShowDialog();
            Show();
        }
    }
}
