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
    public partial class StockManagement : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public StockManagement()
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox6.Text != "" && textBox2.Text != "" && textBox4.Text != "")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    string formattedDate2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");

                    // Gunakan parameterized query
                    query = "INSERT INTO stok_masuk (id_stok_masuk,tgl_masuk, nama_obat, jumlah, harga_beli_satuan, total_harga_beli, tgl_kadaluarsa, supplier,harga_jual, nomor_bench) " +
                            "VALUES (@id_stok,@masuk, @nama_obat, @jumlah, @harga_satuan, @total_harga, @tgl, @supplier,@jual, @no_bench);";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query
                    perintah.Parameters.AddWithValue("@id_stok", textBox1.Text);
                    perintah.Parameters.AddWithValue("@masuk", formattedDate2);
                    perintah.Parameters.AddWithValue("@nama_obat", comboBox1.Text);
                    perintah.Parameters.AddWithValue("@jumlah", textBox4.Text);
                    perintah.Parameters.AddWithValue("@harga_satuan", textBox3.Text);
                    perintah.Parameters.AddWithValue("@total_harga", textBox5.Text);
                    perintah.Parameters.AddWithValue("@tgl", formattedDate);
                    perintah.Parameters.AddWithValue("@supplier", textBox6.Text);
                    perintah.Parameters.AddWithValue("@jual", textBox7.Text);
                    perintah.Parameters.AddWithValue("@no_bench", textBox2.Text);

                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Insert Data Sukses ...");
                        StockManagement_Load(null, null);
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    if (MessageBox.Show("Anda Yakin Menghapus Data Ini ??", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        query = string.Format("Delete from stok_masuk where id_stok_masuk = '{0}'", textBox1.Text);
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
                    StockManagement_Load(null, null);
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                StockManagement_Load(null, null);
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
                if (textBox1.Text != "" && textBox7.Text != "" && comboBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "" && textBox3.Text != "" && textBox5.Text != "" && textBox6.Text != "")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    string formattedDate2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");

                    query = "UPDATE stok_masuk SET nama_obat = @nama_obat, tgl_masuk = @masuk,harga_jual = @jual, nomor_bench = @no_bench, jumlah = @jumlah, harga_beli_satuan = @harga_satuan, total_harga_beli = @total_harga, tgl_kadaluarsa = @tgl, supplier = @supplier WHERE id_stok_masuk = @id_stok";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query untuk mencegah SQL Injection

                    perintah.Parameters.AddWithValue("@id_stok", textBox1.Text);
                    perintah.Parameters.AddWithValue("@nama_obat", comboBox1.Text);
                    perintah.Parameters.AddWithValue("@masuk", formattedDate2);
                    perintah.Parameters.AddWithValue("@jual", textBox7.Text);
                    perintah.Parameters.AddWithValue("@jumlah", textBox4.Text);
                    perintah.Parameters.AddWithValue("@harga_satuan", textBox3.Text);
                    perintah.Parameters.AddWithValue("@total_harga", textBox5.Text);
                    perintah.Parameters.AddWithValue("@tgl", formattedDate);
                    perintah.Parameters.AddWithValue("@supplier", textBox6.Text);
                    perintah.Parameters.AddWithValue("@no_bench", textBox2.Text);

                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Update Data Suksess ...");
                        StockManagement_Load(null, null);
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox8.Text != "")
                {
                    query = string.Format("select * from stok_masuk where nomor_bench = '{0}'", textBox8.Text);
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
                            textBox1.Text = kolom["id_stok_masuk"].ToString();
                            comboBox1.Text = kolom["nama_obat"].ToString();
                            textBox4.Text = kolom["jumlah"].ToString();
                            textBox3.Text = kolom["harga_beli_satuan"].ToString();
                            textBox5.Text = kolom["total_harga_beli"].ToString();
                            textBox6.Text = kolom["supplier"].ToString();
                            textBox2.Text = kolom["nomor_bench"].ToString();
                            textBox7.Text = kolom["harga_jual"].ToString();

                        }
                        textBox8.Enabled = true;
                        dataGridView1.DataSource = ds.Tables[0];
                        button5.Enabled = true;
                        button4.Enabled = true;
                        button3.Enabled = true;

                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Ada !!");
                        StockManagement_Load(null, null);
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Pastikan ComboBox memiliki nilai terpilih
                if (comboBox1.SelectedItem != null)
                {
                    // Query untuk mendapatkan stok dan harga jual berdasarkan nama obat yang dipilih
                    string query = "SELECT harga_beli_satuan, harga_jual FROM obat WHERE nama_obat = @nama_obat";
                    MySqlCommand command = new MySqlCommand(query, koneksi);
                    command.Parameters.AddWithValue("@nama_obat", comboBox1.SelectedItem.ToString());

                    // Buka koneksi
                    koneksi.Open();

                    // Eksekusi query dan ambil stok dan harga jual
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        textBox3.Text = reader["harga_beli_satuan"].ToString();
                        textBox7.Text = reader["harga_jual"].ToString();

                    }
                    else
                    {
                        // Jika tidak ada hasil, setel TextBox ke 0 atau kosong
                        textBox3.Text = "0";
                        textBox7.Text = "0";
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
        private void CalculateTotalPrice()
        {
            decimal quantity;
            decimal pricePerUnit;

            // Cek dan ambil nilai dari textBox2 (jumlah) dan textBox3 (harga per unit)
            if (decimal.TryParse(textBox4.Text, out quantity) && decimal.TryParse(textBox3.Text, out pricePerUnit))
            {
                // Hitung total harga
                decimal totalPrice = quantity * pricePerUnit;
                textBox5.Text = totalPrice.ToString(); // Format sebagai mata uang
            }
            else
            {
                textBox5.Text = "0"; // Reset total harga jika input tidak valid
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            drugDisposal dis = new drugDisposal();
            dis.Show();
        }

        private void MoveExpiredMedicines()
        {
            try
            {
                using (koneksi)
                {
                    koneksi.Open();

                    // Mengambil obat yang sudah kadaluarsa dari tabel stok
                    string selectQuery = "SELECT nama_obat, jumlah FROM stok_masuk WHERE tgl_kadaluarsa < @currentDate";
                    using (MySqlCommand perintah = new MySqlCommand(selectQuery, koneksi))
                    {
                        perintah.Parameters.AddWithValue("@currentDate", DateTime.Now);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(perintah))
                        {
                            DataTable expiredStock = new DataTable();
                            adapter.Fill(expiredStock);

                            // Menghapus obat kadaluarsa dari tabel stok
                            if (expiredStock.Rows.Count > 0)
                            {
                                string deleteStokQuery = "DELETE FROM stok_masuk WHERE tgl_kadaluarsa < @currentDate";
                                using (MySqlCommand deleteStokCommand = new MySqlCommand(deleteStokQuery, koneksi))
                                {
                                    deleteStokCommand.Parameters.AddWithValue("@currentDate", DateTime.Now);
                                    deleteStokCommand.ExecuteNonQuery();
                                }
                               
                            }
                            else
                            {
                            
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void StockManagement_Load(object sender, EventArgs e)
        {
            MoveExpiredMedicines();
            try
            {
                koneksi.Open();
                query = string.Format("SELECT id_stok_masuk,tgl_masuk, nama_obat, nomor_bench, jumlah, harga_beli_satuan, harga_jual, total_harga_beli, tgl_kadaluarsa, supplier  FROM stok_masuk ORDER BY id_stok_masuk ASC");
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);

                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Width = 60;
                dataGridView1.Columns[0].HeaderText = "Incoming Stock Id";
                dataGridView1.Columns[1].Width = 100;
                dataGridView1.Columns[1].HeaderText = "Date of Entry";
                dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[2].HeaderText = "Drug Name";
                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[3].HeaderText = "Bench Number";
                dataGridView1.Columns[4].Width = 80;
                dataGridView1.Columns[4].HeaderText = "Amount";
                dataGridView1.Columns[5].Width = 120;
                dataGridView1.Columns[5].HeaderText = "Unit Purchase Price";
                dataGridView1.Columns[6].Width = 120;
                dataGridView1.Columns[6].HeaderText = "Price";
                dataGridView1.Columns[7].Width = 120;
                dataGridView1.Columns[7].HeaderText = "Total Price";
                dataGridView1.Columns[8].Width = 100;
                dataGridView1.Columns[8].HeaderText = "Expired Date";
                dataGridView1.Columns[9].Width = 130;
                dataGridView1.Columns[9].HeaderText = "Supplier";

                comboBox1.Items.Clear();

                koneksi.Open();

                // Mengambil nama obat saja untuk ditambahkan ke ComboBox1
                string queryObat = "SELECT DISTINCT nama_obat FROM obat";
                perintah = new MySqlCommand(queryObat, koneksi);
                MySqlDataReader reader = perintah.ExecuteReader();

                // Menambahkan nama obat ke ComboBox1
                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["nama_obat"].ToString());
                }
                reader.Close();
                koneksi.Close();

                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                textBox3.Clear();
                textBox7.Clear();
                textBox1.Clear();
                textBox2.Clear();
                textBox4.Clear();
                textBox6.Clear();
                textBox5.Clear();
                button5.Enabled = true;




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
