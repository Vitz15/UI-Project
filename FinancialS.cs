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

                // Query untuk menghitung total pendapatan dari tabel transaksi
                string revenueQuery = @"SELECT COALESCE(SUM(total_harga), 0) as total_pendapatan FROM transaksi WHERE tgl_transaksi >= @startDate AND tgl_transaksi <= @endDate";

                // Query untuk menghitung total pengeluaran dari tabel stok_masuk
                string expenseQuery = @"SELECT COALESCE(SUM(total_harga_beli), 0) as total_pengeluaran FROM stok_masuk WHERE tgl_masuk >= @startDate AND tgl_masuk <= @endDate";

                decimal totalPendapatan = 0;
                decimal totalPengeluaran = 0;

                // Menghitung total pendapatan
                using (MySqlCommand cmdRevenue = new MySqlCommand(revenueQuery, koneksi))
                {
                    cmdRevenue.Parameters.AddWithValue("@startDate", startDate);
                    cmdRevenue.Parameters.AddWithValue("@endDate", endDate);
                    object result = cmdRevenue.ExecuteScalar();
                    totalPendapatan = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }

                // Menghitung total pengeluaran
                using (MySqlCommand cmdExpense = new MySqlCommand(expenseQuery, koneksi))
                {
                    cmdExpense.Parameters.AddWithValue("@startDate", startDate);
                    cmdExpense.Parameters.AddWithValue("@endDate", endDate);
                    object result = cmdExpense.ExecuteScalar();
                    totalPengeluaran = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }

                // Menghitung laba bersih
                decimal labaBersih = totalPendapatan - totalPengeluaran;

                // Menyimpan atau memperbarui data di tabel laporan_keuangan
                string upsertQuery = @"INSERT INTO laporan_keuangan (tgl_laporan, total_pendapatan, total_pengeluaran, laba_bersih) VALUES (@tgl_laporan, @total_pendapatan, @total_pengeluaran, @laba_bersih) ON DUPLICATE KEY UPDATE total_pendapatan = VALUES(total_pendapatan), total_pengeluaran = VALUES(total_pengeluaran), laba_bersih = VALUES(laba_bersih)";

                using (MySqlCommand cmdUpsert = new MySqlCommand(upsertQuery, koneksi))
                {
                    cmdUpsert.Parameters.AddWithValue("@tgl_laporan", startDate);
                    cmdUpsert.Parameters.AddWithValue("@total_pendapatan", totalPendapatan);
                    cmdUpsert.Parameters.AddWithValue("@total_pengeluaran", totalPengeluaran);
                    cmdUpsert.Parameters.AddWithValue("@laba_bersih", labaBersih);
                    cmdUpsert.ExecuteNonQuery();
                }

                // Tampilkan hasil di MessageBox untuk debugging
                MessageBox.Show($"Periode: {startDate:yyyy/MM/dd} - {endDate:yyyy/MM/dd}\n" + $"Total Pendapatan: {totalPendapatan:N0}\n" + $"Total Pengeluaran: {totalPengeluaran:N0}\n" + $"Laba Bersih: {labaBersih:N0}");

                // Refresh DataGridView
                RefreshDataGridView();
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

        private void RefreshDataGridView()
        {
            try
            {
                if (koneksi.State != ConnectionState.Closed)
                {
                    koneksi.Close();
                }
                koneksi.Open();

                query = @"
            SELECT 
                DATE_FORMAT(tgl_laporan, '%Y/%m/%d') as 'Tanggal Laporan',
                total_pendapatan as 'Total Pendapatan',
                total_pengeluaran as 'Total Pengeluaran',
                laba_bersih as 'Laba Bersih'
            FROM laporan_keuangan 
            ORDER BY tgl_laporan DESC";

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, koneksi))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;

                    // Format currency untuk kolom nominal
                    dataGridView1.Columns["Total Pendapatan"].DefaultCellStyle.Format = "N0";
                    dataGridView1.Columns["Total Pengeluaran"].DefaultCellStyle.Format = "N0";
                    dataGridView1.Columns["Laba Bersih"].DefaultCellStyle.Format = "N0";

                    // Auto-size columns
                    dataGridView1.AutoResizeColumns();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing data: " + ex.Message);
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
            comboBoxMonth.SelectedIndex = DateTime.Now.Month - 1; // Set to current month

            int currentYear = DateTime.Now.Year;
            for (int year = 2000; year <= currentYear; year++)
            {
                comboBoxYear.Items.Add(year);
            }
            comboBoxYear.SelectedItem = currentYear;

            // Set column headers
            dataGridView1.AutoGenerateColumns = true;
            RefreshDataGridView();
        }

    }
}

