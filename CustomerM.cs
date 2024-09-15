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
    public partial class CustomerM : Form
    {
        public CustomerM()
        {
            InitializeComponent();
            string[] baris = new string[4];
            ListViewItem item;
            baris[0] = "Joni";
            baris[1] = "084736472343";
            baris[2] = "Antasida";
            baris[3] = "5";

            item = new ListViewItem(baris);
            listView1.Items.Add(item);

            string[] baris2 = new string[4];
            ListViewItem item2;
            baris2[0] = "Brandon";
            baris2[1] = "087364756277";
            baris2[2] = "Ibuprofen";
            baris2[3] = "20";

            item = new ListViewItem(baris2);
            listView1.Items.Add(item);

            string[] baris3 = new string[4];
            ListViewItem item3;
            baris3[0] = "Anto";
            baris3[1] = "089573867486";
            baris3[2] = "Amoxcilin";
            baris3[3] = "5";

            item = new ListViewItem(baris3);
            listView1.Items.Add(item);
        }

        private void label38_Click(object sender, EventArgs e)
        {
            Salestransaction sales = new Salestransaction();
            sales.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Drugandstock drugandstock = new Drugandstock();
            drugandstock.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] baris = new string[4];
            ListViewItem item;
            baris[0] = textBox1.Text;
            baris[1] = textBox2.Text;
            baris[2] = textBox4.Text;
            baris[3] = textBox3.Text;

            item = new ListViewItem(baris);
            listView1.Items.Add(item);
            clearform();
        }
        private void clearform()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "Rp";
            textBox3.Text = "";
        }
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                textBox1.Text = selectedItem.SubItems[0].Text;
                textBox2.Text = selectedItem.SubItems[1].Text;
                textBox4.Text = selectedItem.SubItems[2].Text;
                textBox3.Text = selectedItem.SubItems[3].Text;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                listView1.Items.Remove(listView1.SelectedItems[0]);

                clearform();
            }
            else
            {
                MessageBox.Show("Pilih baris yang ingin dihapus.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {
            FinancialS financial = new FinancialS();
            financial.Show();
        }

        private void label40_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
        }
    }
}
