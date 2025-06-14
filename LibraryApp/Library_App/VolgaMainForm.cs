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
    public partial class VolgaMainForm : Form
    {
        public VolgaMainForm()
        {
            InitializeComponent();
        }
        private void btnTextOpen_Click(object sender, EventArgs e)
        {
            TestVolgaForm1 testVolgaForm1 = new TestVolgaForm1();
            Hide();
            testVolgaForm1.ShowDialog();
            Show();
        }
    }
}
