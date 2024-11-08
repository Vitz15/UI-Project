using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UI_Project
{
    public partial class Drugandstock : Form
    {

        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public Drugandstock()
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

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void clearform()
        {
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text != "" && textBox4.Text != "")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    string formattedDate2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");

                    using (MySqlConnection koneksi = new MySqlConnection(alamat))
                    {
                        koneksi.Open();

                        int jumlahDikurangi = Convert.ToInt32(textBox2.Text); // Total jumlah stok yang ingin dikurangi
                        string drugName = comboBox1.Text;

                        // Mengambil stok berdasarkan batch
                        string selectBatchQuery = "SELECT nomor_bench, jumlah FROM stok_masuk WHERE nama_obat = @nama AND jumlah > 0 ORDER BY nomor_bench ASC;";
                        MySqlCommand selectBatchCommand = new MySqlCommand(selectBatchQuery, koneksi);
                        selectBatchCommand.Parameters.AddWithValue("@nama", drugName);

                        // Membaca semua batch terlebih dahulu
                        List<Tuple<int, int>> batches = new List<Tuple<int, int>>();
                        using (MySqlDataReader reader = selectBatchCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int nomorBench = reader.GetInt32("nomor_bench");
                                int jumlahBatch = reader.GetInt32("jumlah");
                                batches.Add(Tuple.Create(nomorBench, jumlahBatch));
                            }
                        }

                        // Cek apakah stok di semua batch mencukupi
                        int totalStokBatch = batches.Sum(batch => batch.Item2);
                        if (totalStokBatch < jumlahDikurangi)
                        {
                            MessageBox.Show("Stok tidak cukup pada batch yang tersedia.");
                            return; // Batalkan proses jika stok batch tidak mencukupi
                        }

                        // Lanjutkan ke proses INSERT jika stok mencukupi
                        string query = "INSERT INTO obat (id_obat, nama_obat, stok, harga_jual, tanggal_kadaluarsa, tgl_masuk, kategori, satuan, deskripsi) " +
                                       "VALUES (@id, @nama_obat, @stok, @harga_jual, @tanggal_kadaluarsa, @masuk, @kategori, @satuan, @deskripsi);";

                        using (MySqlCommand perintah = new MySqlCommand(query, koneksi))
                        {
                            perintah.Parameters.AddWithValue("@id", textBox8.Text);
                            perintah.Parameters.AddWithValue("@nama_obat", comboBox1.Text);
                            perintah.Parameters.AddWithValue("@stok", textBox2.Text);
                            perintah.Parameters.AddWithValue("@harga_jual", textBox4.Text);
                            perintah.Parameters.AddWithValue("@tanggal_kadaluarsa", formattedDate);
                            perintah.Parameters.AddWithValue("@masuk", formattedDate2);
                            perintah.Parameters.AddWithValue("@kategori", textBox7.Text);
                            perintah.Parameters.AddWithValue("@satuan", textBox6.Text);
                            perintah.Parameters.AddWithValue("@deskripsi", textBox5.Text);

                            int res = perintah.ExecuteNonQuery();

                            if (res == 1)
                            {
                                // Lakukan pengurangan stok per batch
                                foreach (var batch in batches)
                                {
                                    if (jumlahDikurangi <= 0) break;

                                    int nomorBench = batch.Item1;
                                    int jumlahBatch = batch.Item2;

                                    int jumlahPengurangan = Math.Min(jumlahDikurangi, jumlahBatch);

                                    string updateBatchQuery = "UPDATE stok_masuk SET jumlah = jumlah - @jumlahPengurangan WHERE nomor_bench = @nomorBench AND nama_obat = @namaObat;";
                                    MySqlCommand updateBatchCommand = new MySqlCommand(updateBatchQuery, koneksi);
                                    updateBatchCommand.Parameters.AddWithValue("@jumlahPengurangan", jumlahPengurangan);
                                    updateBatchCommand.Parameters.AddWithValue("@nomorBench", nomorBench);
                                    updateBatchCommand.Parameters.AddWithValue("@namaObat", drugName);

                                    updateBatchCommand.ExecuteNonQuery();
                                    jumlahDikurangi -= jumlahPengurangan;
                                }

                                // Perbarui stok pada tabel obat
                                string updateObatQuery = "UPDATE obat SET stok = stok WHERE nama_obat = @nama;";
                                MySqlCommand updateObatCommand = new MySqlCommand(updateObatQuery, koneksi);
                                updateObatCommand.Parameters.AddWithValue("@nama", drugName);

                                int obatUpdateRes = updateObatCommand.ExecuteNonQuery();

                                if (obatUpdateRes > 0)
                                {
                                    MessageBox.Show("Insert Data Sukses dan Stok Berhasil di update.");
                                }
                                else
                                {
                                    MessageBox.Show("Insert Data Sukses tetapi gagal memperbarui stok di tabel obat.");
                                }

                                Drugandstock_Load(null, null); // Refresh tampilan data stok
                            }
                            else
                            {
                                MessageBox.Show("Gagal Insert Data . . . ");
                            }
                        }
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





        }
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox8.Text != "")
                {
                    if (MessageBox.Show("Anda Yakin Menghapus Data Ini ??", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        query = string.Format("Delete from obat where id_obat = '{0}'", textBox8.Text);
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
                    Drugandstock_Load(null, null);
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

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
            
           
        }

        private void label38_Click(object sender, EventArgs e)
        {
            Salestransaction sales = new Salestransaction();
            sales.Show();
        }

        private void label36_Click(object sender, EventArgs e)
        {
            CustomerM customer = new CustomerM();
            customer.Show();
        }


        private void MoveExpiredMedicines()
        {
            try
            {
                using (koneksi)
                {
                    koneksi.Open();

                    // Mengatur kolom deskripsi menjadi 'expired' untuk obat yang sudah kadaluarsa
                    string updateDescriptionQuery = "UPDATE obat SET deskripsi = 'expired' WHERE tanggal_kadaluarsa < @currentDate";
                    using (MySqlCommand updateCommand = new MySqlCommand(updateDescriptionQuery, koneksi))
                    {
                        updateCommand.Parameters.AddWithValue("@currentDate", DateTime.Now);
                        updateCommand.ExecuteNonQuery();
                    }

                    // Mengambil obat yang sudah kadaluarsa
                    string selectQuery = "SELECT id_obat, nama_obat, stok, harga_jual, tanggal_kadaluarsa, kategori, satuan FROM obat WHERE tanggal_kadaluarsa < @currentDate";
                    using (MySqlCommand perintah = new MySqlCommand(selectQuery, koneksi))
                    {
                        perintah.Parameters.AddWithValue("@currentDate", DateTime.Now);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(perintah))
                        {
                            DataTable expiredMedicines = new DataTable();
                            adapter.Fill(expiredMedicines);

                            // Memindahkan ke tabel pemusnahan dengan deskripsi 'Expired'
                            foreach (DataRow row in expiredMedicines.Rows)
                            {
                                DateTime expiredDate = Convert.ToDateTime(row["tanggal_kadaluarsa"]);
                                string insertQuery = "INSERT INTO pemusnahan (nama_obat, tgl_kadaluarsa, jumlah, deskripsi) VALUES (@nama_obat, @tgl_kadaluarsa, @jumlah, 'Expired')";
                                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, koneksi))
                                {
                                    insertCommand.Parameters.AddWithValue("@nama_obat", row["nama_obat"]);
                                    insertCommand.Parameters.AddWithValue("@tgl_kadaluarsa", expiredDate);
                                    insertCommand.Parameters.AddWithValue("@jumlah", row["stok"]);

                                    insertCommand.ExecuteNonQuery();
                                }
                            }

                            // Menghapus obat kadaluarsa dari tabel obat
                            if (expiredMedicines.Rows.Count > 0)
                            {
                                // Membuat daftar nama obat yang kadaluarsa dan menghitung total stok
                                string expiredNames = string.Join(", ", expiredMedicines.AsEnumerable().Select(row => row["nama_obat"].ToString()));
                                int totalExpiredStock = expiredMedicines.AsEnumerable().Sum(row => Convert.ToInt32(row["stok"]));

                                string deleteObatQuery = "DELETE FROM obat WHERE tanggal_kadaluarsa < @currentDate";
                                using (MySqlCommand deleteObatCommand = new MySqlCommand(deleteObatQuery, koneksi))
                                {
                                    deleteObatCommand.Parameters.AddWithValue("@currentDate", DateTime.Now);
                                    deleteObatCommand.ExecuteNonQuery();

                                    // Menampilkan notifikasi dengan nama obat dan total stok yang telah kadaluarsa
                                    MessageBox.Show($"Nama obat yang kedaluwarsa: {expiredNames}\nTotal stok yang kedaluwarsa: {totalExpiredStock}", "Notifikasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }






        private void Drugandstock_Load(object sender, EventArgs e)
        {
            MoveExpiredMedicines();
            try
            {
                koneksi.Open();
                query = string.Format("SELECT id_obat,tgl_masuk, nama_obat, kategori, harga_jual, stok, satuan, tanggal_kadaluarsa, deskripsi FROM obat ORDER BY id_obat ASC");
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);
                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
                ds.Clear();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Width = 70;
                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[1].Width = 120;
                dataGridView1.Columns[1].HeaderText = "Date of Entry";
                dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[2].HeaderText = "Drug Name";
                dataGridView1.Columns[3].Width = 120;
                dataGridView1.Columns[3].HeaderText = "Category";
                dataGridView1.Columns[4].Width = 120;
                dataGridView1.Columns[4].HeaderText = "Price";
                dataGridView1.Columns[5].Width = 120;
                dataGridView1.Columns[5].HeaderText = "Stok";
                dataGridView1.Columns[6].Width = 120;
                dataGridView1.Columns[6].HeaderText = "Unit";
                dataGridView1.Columns[7].Width = 120;
                dataGridView1.Columns[7].HeaderText = "Expired Date";
                dataGridView1.Columns[8].Width = 127;
                dataGridView1.Columns[8].HeaderText = "Description";

                comboBox1.Items.Clear();

                string queryObat = "SELECT DISTINCT nama_obat FROM stok_masuk";
                perintah = new MySqlCommand(queryObat, koneksi);
                MySqlDataReader reader = perintah.ExecuteReader();

                // Menambahkan nama obat ke ComboBox
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["nama_obat"].ToString());
                }
                reader.Close();

                dateTimePicker1.Value = DateTime.Now;
                    dateTimePicker2.Value = DateTime.Now;
                    textBox8.Clear();
                    textBox2.Clear();
                    textBox4.Clear();
                    textBox7.Clear();
                    textBox6.Clear();
                    textBox5.Clear();
                    button5.Enabled = true;
                koneksi.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text != "")
                {
                    query = string.Format("select * from obat where nama_obat = '{0}'", textBox3.Text);
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
                            textBox8.Text = kolom["id_obat"].ToString();
                            textBox2.Text = kolom["stok"].ToString();
                        
                            textBox4.Text = kolom["harga_jual"].ToString();
                            textBox7.Text = kolom["kategori"].ToString();
                            textBox6.Text = kolom["satuan"].ToString();
                            textBox5.Text = kolom["deskripsi"].ToString();
                            comboBox1.Text = kolom["nama_obat"].ToString();

                        }
                        textBox3.Enabled = true;
                        dataGridView1.DataSource = ds.Tables[0];
                        button5.Enabled = true;
                        button4.Enabled = true;
                        button3.Enabled = true;

                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Ada !!");
                        Drugandstock_Load(null, null);
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

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Drugandstock_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text != "" && textBox2.Text != ""  && textBox4.Text != "" && textBox7.Text != "" && textBox6.Text != "" && textBox5.Text != "" && textBox8.Text !="")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                    query = "UPDATE obat SET nama_obat = @nama_obat, stok = @stok, harga_jual = @harga_jual, tanggal_kadaluarsa = @tanggal_kadaluarsa, kategori = @kategori, satuan = @satuan, deskripsi = @deskripsi WHERE id_obat = @id";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query untuk mencegah SQL Injection
                    
                    perintah.Parameters.AddWithValue("@nama_obat", comboBox1.Text);
                    perintah.Parameters.AddWithValue("@stok", textBox2.Text);
                    perintah.Parameters.AddWithValue("@harga_jual", textBox4.Text);
                    perintah.Parameters.AddWithValue("@tanggal_kadaluarsa", formattedDate);
                    perintah.Parameters.AddWithValue("@kategori", textBox7.Text);
                    perintah.Parameters.AddWithValue("@satuan", textBox6.Text);
                    perintah.Parameters.AddWithValue("@deskripsi", textBox5.Text);
                    perintah.Parameters.AddWithValue("@id", textBox8.Text); // Asumsi textBox1 untuk id

                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Update Data Suksess ...");
                        Drugandstock_Load(null, null);
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

        private void label10_Click(object sender, EventArgs e)
        {
            StockManagement stok = new StockManagement();
            stok.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            drugDisposal dis = new drugDisposal();
            dis.Show();
        }

        private void label6_Click(object sender, EventArgs e)
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
                    string query = "SELECT harga_jual FROM stok_masuk WHERE nama_obat = @nama_obat";
                    MySqlCommand command = new MySqlCommand(query, koneksi);
                    command.Parameters.AddWithValue("@nama_obat", comboBox1.SelectedItem.ToString());

                    // Buka koneksi
                    koneksi.Open();

                    // Eksekusi query dan ambil stok dan harga jual
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                     
                        textBox4.Text = reader["harga_jual"].ToString();

                    }
                    else
                    {

                        textBox4.Text = "0";
                        dateTimePicker1.Value = DateTime.Today;
                        dateTimePicker2.Value = DateTime.Today;
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

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
