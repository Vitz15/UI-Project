using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UI_Project
{
    public partial class FinancialS : Form
    {

        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public FinancialS()
        {
            alamat = "server=localhost; database=db_apotek; username=root; password=; Convert Zero Datetime=True; Allow Zero Datetime=True;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {
            StockManagement stok = new StockManagement();
            stok.Show();
        }

        private void label4_Click_1(object sender, EventArgs e)
        {
            Drugandstock drug = new Drugandstock();
            drug.Show();
        }

        private void label38_Click_1(object sender, EventArgs e)
        {
            Salestransaction sales = new Salestransaction();
            sales.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void label36_Click_1(object sender, EventArgs e)
        {
            CustomerM customer = new CustomerM();
            customer.Show();
        }

        private void label40_Click_1(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            drugDisposal dis = new drugDisposal();
            dis.Show();
        }

        private void FinancialS_Load(object sender, EventArgs e)
        {
            string[] months = new string[]
            {
                "Januari", "Februari", "Maret", "April", "Mei", "Juni",
                "Juli", "Agustus", "September", "Oktober", "November", "Desember"
            };

            comboBoxMonth.Items.AddRange(months);
            comboBoxMonth.SelectedIndex = 0; // Mengatur bulan default ke Januari

            // Mengisi ComboBox tahun dengan tahun dari 2000 hingga tahun saat ini
            int currentYear = DateTime.Now.Year;
            for (int year = 2000; year <= currentYear; year++)
            {
                comboBoxYear.Items.Add(year);
            }
            comboBoxYear.SelectedItem = currentYear; // Mengatur tahun default ke tahun saat ini
            try
            {
                koneksi.Open();
                query = string.Format("SELECT tgl_laporan, total_pendapatan, total_pengeluaran, laba_bersih FROM laporan_keuangan ORDER BY tgl_laporan ASC");
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);

                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Width = 250;
                dataGridView1.Columns[0].HeaderText = "Report Date";
                dataGridView1.Columns[1].Width = 250;
                dataGridView1.Columns[1].HeaderText = "Total Revenue";
                dataGridView1.Columns[2].Width = 250;
                dataGridView1.Columns[2].HeaderText = "Total Expenses";
                dataGridView1.Columns[3].Width = 237;
                dataGridView1.Columns[3].HeaderText = "Net Profit";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
