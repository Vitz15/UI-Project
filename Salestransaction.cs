using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UI_Project
{
    public partial class Salestransaction : Form
    {
        public Salestransaction()
        {
            InitializeComponent();
            string[] baris = new string[4];
            ListViewItem item;
            baris[0] = "Paracetamol";
            baris[1] = "2.300";
            baris[2] = "2";
            baris[3] = "Rp5.600,00";

            item = new ListViewItem(baris);
            listView1.Items.Add(item);

            string[] baris2 = new string[4];
            ListViewItem item2;
            baris2[0] = "Ibuprofen";
            baris2[1] = "2.600";
            baris2[2] = "10";
            baris2[3] = "Rp26.000,00";

            item = new ListViewItem(baris2);
            listView1.Items.Add(item);

            string[] baris3 = new string[4];
            ListViewItem item3;
            baris3[0] = "Antasida";
            baris3[1] = "1.300";
            baris3[2] = "10";
            baris3[3] = "Rp13.000,00";

            item = new ListViewItem(baris3);
            listView1.Items.Add(item);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Drugandstock drug = new Drugandstock();
            drug.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] baris = new string[4];
            ListViewItem item;

            // Ambil nilai dari TextBox
            baris[0] = textBox2.Text; // Nama obat
            baris[1] = textBox3.Text; // Jumlah
            baris[2] = textBox1.Text; // Harga per unit
            baris[3] = textBox4.Text; // Total harga

            // Buat item baru dan tambahkan ke ListView
            item = new ListViewItem(baris);
            listView1.Items.Add(item);

            // Kosongkan form setelah menambahkan item
            clearform();
        }
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                textBox2.Text = selectedItem.SubItems[0].Text;
                textBox1.Text = selectedItem.SubItems[1].Text;
                textBox3.Text = selectedItem.SubItems[2].Text;
                textBox4.Text = selectedItem.SubItems[3].Text;
            }
        }
        private void clearform()
        {
            textBox2.Text = "";
            textBox1.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

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

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }
        private void CalculateTotalPrice()
        {
            decimal quantity;
            decimal pricePerUnit;

            // Cek dan ambil nilai dari textBox2 (jumlah) dan textBox3 (harga per unit)
            if (decimal.TryParse(textBox1.Text, out quantity) && decimal.TryParse(textBox3.Text, out pricePerUnit))
            {
                // Hitung total harga
                decimal totalPrice = quantity * pricePerUnit;
                textBox4.Text = totalPrice.ToString("C"); // Format sebagai mata uang
            }
            else
            {
                textBox4.Text = "0"; // Reset total harga jika input tidak valid
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Proses Pembayaran Berhasil", "Payment Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void label36_Click(object sender, EventArgs e)
        {
            CustomerM customer = new CustomerM();
            customer.Show();
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
