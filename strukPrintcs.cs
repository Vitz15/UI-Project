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
    public partial class strukPrintcs : Form
    {
        public strukPrintcs()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            strukTransaction1.SetParameterValue("Customer", textBox1.Text);
            crystalReportViewer1.ReportSource = strukTransaction1;
            crystalReportViewer1.Refresh();

        }
    }
}
