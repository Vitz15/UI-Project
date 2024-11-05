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
            CalculateFinancialData();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CalculateFinancialData()
        {
            try
            {
                if (koneksi.State != ConnectionState.Closed)
                {
                    koneksi.Close();
                }
                koneksi.Open();

                // Mendapatkan bulan dan tahun yang dipilih
                int selectedMonth = comboBoxMonth.SelectedIndex + 1;
                int selectedYear = Convert.ToInt32(comboBoxYear.SelectedItem);

                // Mendapatkan tanggal awal dan akhir bulan
                DateTime startDate = new DateTime(selectedYear, selectedMonth, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                DataTable dailyData = new DataTable();
                dailyData.Columns.Add("Tanggal", typeof(string));
                dailyData.Columns.Add("Total Pendapatan", typeof(decimal));
                dailyData.Columns.Add("Total Pengeluaran", typeof(decimal));
                dailyData.Columns.Add("Laba Bersih", typeof(decimal));

                decimal totalPendapatan = 0;
                decimal totalPengeluaran = 0;

                // Loop melalui setiap hari dalam bulan
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    // Menghitung pendapatan
                    string revenueQuery = @"SELECT SUM(total_harga) 
                                         FROM transaksi 
                                         WHERE DATE(tgl_transaksi) = @currentDate";

                    decimal pendapatanHari = 0;
                    using (MySqlCommand cmdRevenue = new MySqlCommand(revenueQuery, koneksi))
                    {
                        cmdRevenue.Parameters.AddWithValue("@currentDate", date);
                        object result = cmdRevenue.ExecuteScalar();
                        pendapatanHari = result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                    }

                    // Menghitung pengeluaran
                    string expensesQuery = @"SELECT SUM(total_harga_beli) 
                                           FROM stok_masuk 
                                           WHERE DATE(tgl_masuk) = @currentDate";

                    decimal pengeluaranHari = 0;
                    using (MySqlCommand cmdExpenses = new MySqlCommand(expensesQuery, koneksi))
                    {
                        cmdExpenses.Parameters.AddWithValue("@currentDate", date);
                        object result = cmdExpenses.ExecuteScalar();
                        pengeluaranHari = result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                    }

                    decimal labaBersih = pendapatanHari - pengeluaranHari;

                    // Menambahkan data harian ke DataTable
                    dailyData.Rows.Add(
                        date.ToString("yyyy-MM-dd"),
                        pendapatanHari,
                        pengeluaranHari,
                        labaBersih
                    );

                    totalPendapatan += pendapatanHari;
                    totalPengeluaran += pengeluaranHari;
                }

                // Menambahkan baris total
                dailyData.Rows.Add("Total", totalPendapatan, totalPengeluaran, totalPendapatan - totalPengeluaran);

                // Menampilkan data di DataGridView
                dataGridView1.DataSource = dailyData;

                // Simpan laporan keuangan
                string insertQuery = @"INSERT INTO laporan_keuangan 
                                     (tgl_laporan, total_pendapatan, total_pengeluaran, laba_bersih) 
                                     VALUES (@Date, @Revenue, @Expenses, @NetProfit)";

                using (MySqlCommand cmd = new MySqlCommand(insertQuery, koneksi))
                {
                    cmd.Parameters.AddWithValue("@Date", endDate);
                    cmd.Parameters.AddWithValue("@Revenue", totalPendapatan);
                    cmd.Parameters.AddWithValue("@Expenses", totalPengeluaran);
                    cmd.Parameters.AddWithValue("@NetProfit", totalPendapatan - totalPengeluaran);
                    cmd.ExecuteNonQuery();
                }

                // Tampilkan hasil total
                MessageBox.Show($"Total Pendapatan: {totalPendapatan:N0}\n" +
                              $"Total Pengeluaran: {totalPengeluaran:N0}\n" +
                              $"Laba Bersih: {(totalPendapatan - totalPengeluaran):N0}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (koneksi.State != ConnectionState.Closed)
                {
                    koneksi.Close();
                }
            }

        }


        private void comboBoxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMonth.SelectedIndex >= 0 && comboBoxYear.SelectedItem != null)
            {
                CalculateFinancialData();
            }
        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMonth.SelectedIndex >= 0 && comboBoxYear.SelectedItem != null)
            {
                CalculateFinancialData();
            }
        }

        private void FinancialS_Load(object sender, EventArgs e)
        {
            string[] months = new string[]
            {
                "Januari", "Februari", "Maret", "April", "Mei", "Juni",
                "Juli", "Agustus", "September", "Oktober", "November", "Desember"
            };

            comboBoxMonth.Items.AddRange(months);
            comboBoxMonth.SelectedIndex = DateTime.Now.Month - 1;

            int currentYear = DateTime.Now.Year;
            for (int year = 2000; year <= currentYear; year++)
            {
                comboBoxYear.Items.Add(year);
            }
            comboBoxYear.SelectedItem = currentYear;

            dataGridView1.AutoGenerateColumns = true;
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns[0].Width = 80; 
                dataGridView1.Columns[1].Width = 295; 
                dataGridView1.Columns[2].Width = 295;
                dataGridView1.Columns[3].Width = 300;
            }

           
            dataGridView1.RowTemplate.Height = 30; 

        }

    }
}

