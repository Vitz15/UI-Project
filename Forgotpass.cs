using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI_Project
{
    public partial class Forgotpass : Form
    {
        public Forgotpass()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Newpass pass = new Newpass();
            pass.Show();
        }
    }
}
