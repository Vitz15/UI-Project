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
    public partial class Salestransaction : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public Salestransaction()
        {
            alamat = "server=localhost; database=db_apotek; username=root; password=; Convert Zero Datetime=True; Allow Zero Datetime=True;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
           
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
            try
            {
                if (textBox1.Text != "")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    koneksi.Open();

                    int jumlahDikurangi = Convert.ToInt32(textBox1.Text); // Total jumlah yang dibeli
                    string drugName = comboBox1.Text;

                    // Mengambil stok dari semua batch dengan nama obat yang sesuai
                    string selectBatchQuery = "SELECT id_obat, stok FROM obat WHERE nama_obat = @namaObat AND stok > 0 ORDER BY id_obat ASC;";
                    MySqlCommand selectBatchCommand = new MySqlCommand(selectBatchQuery, koneksi);
                    selectBatchCommand.Parameters.AddWithValue("@namaObat", drugName);

                    // Membaca semua batch yang tersedia untuk obat tersebut
                    List<Tuple<string, int>> batches = new List<Tuple<string, int>>();
                    using (MySqlDataReader reader = selectBatchCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string idObat = reader.GetString("id_obat");
                            int stokBatch = reader.GetInt32("stok");
                            batches.Add(Tuple.Create(idObat, stokBatch));
                        }
                    }

                    // Cek apakah stok total cukup
                    int totalStok = batches.Sum(batch => batch.Item2);
                    if (totalStok < jumlahDikurangi)
                    {
                        MessageBox.Show("Obat tidak cukup di stok.");
                        return; // Batalkan transaksi jika stok tidak cukup
                    }

                    // Jika stok cukup, lanjutkan dengan query INSERT untuk menambahkan transaksi
                    query = "INSERT INTO transaksi (id_transaksi, nama_pelanggan, nama_obat, tgl_transaksi, jumlah, harga_satuan, total_harga) " +
                            "VALUES (@id, @pelanggan, @obat, @tgl, @jmlh, @harga_satu, @total_harga);";

                    perintah = new MySqlCommand(query, koneksi);

                    perintah.Parameters.AddWithValue("@id", textBox7.Text);
                    perintah.Parameters.AddWithValue("@tgl", formattedDate);
                    perintah.Parameters.AddWithValue("@pelanggan", comboBox2.Text);
                    perintah.Parameters.AddWithValue("@obat", comboBox1.Text);
                    perintah.Parameters.AddWithValue("@jmlh", textBox1.Text);
                    perintah.Parameters.AddWithValue("@harga_satu", textBox3.Text);
                    perintah.Parameters.AddWithValue("@total_harga", textBox4.Text);

                    int res = perintah.ExecuteNonQuery();

                    if (res == 1)
                    {
                        // Lakukan pengurangan stok per batch
                        foreach (var batch in batches)
                        {
                            if (jumlahDikurangi <= 0) break;

                            string idObat = batch.Item1;
                            int stokBatch = batch.Item2;

                            int jumlahPengurangan = Math.Min(jumlahDikurangi, stokBatch); // Kurangi stok sesuai batch

                            // Update stok untuk batch ini
                            string updateBatchQuery = "UPDATE obat SET stok = stok - @jumlahPengurangan WHERE id_obat = @idObat;";
                            MySqlCommand updateBatchCommand = new MySqlCommand(updateBatchQuery, koneksi);
                            updateBatchCommand.Parameters.AddWithValue("@jumlahPengurangan", jumlahPengurangan);
                            updateBatchCommand.Parameters.AddWithValue("@idObat", idObat);

                            updateBatchCommand.ExecuteNonQuery(); // Kurangi stok pada batch ini
                            jumlahDikurangi -= jumlahPengurangan; // Kurangi jumlah sisa yang perlu dikurangi
                        }

                        MessageBox.Show("Insert Data Sukses dan Stok Berhasil di update.");

                        // Refresh DataGridView atau tampilan data
                        Salestransaction_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Gagal Insert Data . . . ");
                    }
                }
                else
                {
                    MessageBox.Show("Data Tidak Lengkap!!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                koneksi.Close(); // Pastikan koneksi ditutup
            }







        }
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
   
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    if (MessageBox.Show("Anda Yakin Menghapus Data Ini ??", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        query = string.Format("Delete from transaksi where id_transaksi = '{0}'", textBox7.Text);
                        ds.Clear();
                        koneksi.Open();
                        perintah = new MySqlCommand(query, koneksi);
                        adapter = new MySqlDataAdapter(perintah);
                        int res = perintah.ExecuteNonQuery();
                        koneksi.Close();
                        if (res == 1)
                        {
                            MessageBox.Show("Delete Data Suksess ...");
                        }
                        else
                        {
                            MessageBox.Show("Gagal Delete data");
                        }
                    }
                    Salestransaction_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Data Yang Anda Pilih Tidak Ada !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                textBox4.Text = totalPrice.ToString(); // Format sebagai mata uang
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

        private void label36_Click(object sender, EventArgs e)
        {
            SubscribedCostumer customer = new SubscribedCostumer();
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
        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
         
            if ((e.ColumnIndex == 5 || e.ColumnIndex == 6) && e.Value != null)
            {
            
                if (decimal.TryParse(e.Value.ToString(), out decimal value))
                {
                    e.Value = $"Rp {value:N0}"; 
                    e.FormattingApplied = true;
                }
            }
        }
        private void Salestransaction_Load(object sender, EventArgs e)
        {
            dataGridView1.CellFormatting += DataGridView1_CellFormatting;
            try
            {
                // Pastikan koneksi ditutup sebelum membuka yang baru
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }

                koneksi.Open();
                query = "SELECT id_transaksi, nama_pelanggan, nama_obat, tgl_transaksi, jumlah, harga_satuan, total_harga FROM transaksi ORDER BY id_transaksi ASC";
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);

                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();  // Tutup koneksi setelah selesai dengan data transaksi
                dataGridView1.DataSource = ds.Tables[0];

                // Atur lebar dan header kolom
                dataGridView1.Columns[0].Width = 100;
                dataGridView1.Columns[0].HeaderText = "Transaction Id";

                dataGridView1.Columns[1].Width = 170;
                dataGridView1.Columns[1].HeaderText = "Customer Name";

                dataGridView1.Columns[2].Width = 170;
                dataGridView1.Columns[2].HeaderText = "Drug Name";

                dataGridView1.Columns[3].Width = 170;
                dataGridView1.Columns[3].HeaderText = "Date"; // Kolom tanggal

                dataGridView1.Columns[4].Width = 170;
                dataGridView1.Columns[4].HeaderText = "Amount";

                dataGridView1.Columns[5].Width = 170;
                dataGridView1.Columns[5].HeaderText = "Price";

                dataGridView1.Columns[6].Width = 157;
                dataGridView1.Columns[6].HeaderText = "Total Price";

                // Pengisian data ComboBox
                comboBox1.Items.Clear();
                comboBox2.Items.Clear();

                // Buka koneksi baru untuk mengambil data obat
                koneksi.Open();
                string queryObat = "SELECT DISTINCT nama_obat FROM obat";
                perintah = new MySqlCommand(queryObat, koneksi);
                MySqlDataReader reader = perintah.ExecuteReader();

                // Menambahkan nama obat ke ComboBox
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["nama_obat"].ToString());
                }
                reader.Close();

                // Query untuk mengambil nama pelanggan
                string queryPelangganCombined = @"
                SELECT DISTINCT nama_pelanggan FROM pelanggan
                UNION
                SELECT DISTINCT nama_pelanggan FROM pelanggan_umum";

                perintah = new MySqlCommand(queryPelangganCombined, koneksi);
                reader = perintah.ExecuteReader();

                // Add customer names to ComboBox2
                while (reader.Read())
                {
                    comboBox2.Items.Add(reader["nama_pelanggan"].ToString());
                }
                reader.Close();
                koneksi.Close();

                // Reset form inputs
                dateTimePicker1.Value = DateTime.Now;
                textBox3.Clear();
                textBox1.Clear();
                textBox4.Clear();
                button5.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Pastikan ComboBox memiliki nilai terpilih
                if (comboBox1.SelectedItem != null)
                {
                    // Query untuk mendapatkan stok dan harga jual berdasarkan nama obat yang dipilih
                    string query = "SELECT stok, harga_jual FROM obat WHERE nama_obat = @nama_obat";
                    MySqlCommand command = new MySqlCommand(query, koneksi);
                    command.Parameters.AddWithValue("@nama_obat", comboBox1.SelectedItem.ToString());

                    // Buka koneksi
                    koneksi.Open();

                    // Eksekusi query dan ambil stok dan harga jual
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Tampilkan stok di TextBox (misalnya textBox4)
                        textBox4.Text = reader["stok"].ToString();

                        // Tampilkan harga jual di TextBox (misalnya textBoxHargaJual)
                        textBox3.Text = reader["harga_jual"].ToString();
                    }
                    else
                    {
                        // Jika tidak ada hasil, setel TextBox ke 0 atau kosong
                        textBox4.Text = "0";
                        textBox3.Text = "0";
                    }

                    // Tutup reader setelah selesai
                    reader.Close();

                    // Tutup koneksi setelah selesai
                    koneksi.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }
        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Salestransaction_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox6.Text != "")
                {
                    query = string.Format("select * from transaksi where nama_pelanggan = '{0}'", textBox6.Text);
                    ds.Clear();
                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);
                    adapter = new MySqlDataAdapter(perintah);
                    perintah.ExecuteNonQuery();
                    adapter.Fill(ds);
                    koneksi.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow kolom in ds.Tables[0].Rows)
                        {
                            textBox7.Text = kolom["id_transaksi"].ToString();
                            comboBox2.Text = kolom["nama_pelanggan"].ToString();
                            comboBox1.Text = kolom["nama_obat"].ToString();
                            textBox1.Text = kolom["jumlah"].ToString();
                            textBox3.Text = kolom["harga_satuan"].ToString();
                            textBox4.Text = kolom["total_harga"].ToString();

                        }
                        textBox6.Enabled = true;
                        dataGridView1.DataSource = ds.Tables[0];
                        button5.Enabled = true;
                        button3.Enabled = true;

                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Ada !!");
                        Salestransaction_Load(null, null);
                    }

                }
                else
                {
                    MessageBox.Show("Data Yang Anda Pilih Tidak Ada !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox7.Text != "" && comboBox2.Text != "" && comboBox1.Text != "" && textBox1.Text != "" && textBox3.Text != "" && textBox4.Text != "" )
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                    query = "UPDATE transaksi SET nama_pelanggan = @pelanggan, nama_obat = @obat, tgl_transaksi = @tgl, jumlah = @jumlah, harga_satuan = @hargasatu, total_harga = @total WHERE id_transaksi = @id";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query untuk mencegah SQL Injection

                    perintah.Parameters.AddWithValue("@id", textBox7.Text);
                    perintah.Parameters.AddWithValue("@pelanggan", comboBox2.Text);
                    perintah.Parameters.AddWithValue("@obat", comboBox1.Text);
                    perintah.Parameters.AddWithValue("@tgl", formattedDate);
                    perintah.Parameters.AddWithValue("@jumlah", textBox1.Text);
                    perintah.Parameters.AddWithValue("@hargasatu", textBox3.Text);
                    perintah.Parameters.AddWithValue("@total", textBox4.Text);

                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Update Data Suksess ...");
                        Salestransaction_Load(null, null);
                    }
                    else
                    {
                        MessageBox.Show("Gagal Update Data . . . ");
                    }
                }
                else
                {
                    MessageBox.Show("Data Tidak lengkap !!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {
            drugDisposal dis = new drugDisposal();
            dis.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            strukPrintcs strukPrint = new strukPrintcs();
            strukPrint.Show();
        }

        private void label11_Click_1(object sender, EventArgs e)
        {
            StockManagement stok = new StockManagement();
            stok.Show();
        }
    }
}
