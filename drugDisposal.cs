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
    public partial class drugDisposal : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public drugDisposal()
        {
            alamat = "server=localhost; database=db_apotek; username=root; password=; Convert Zero Datetime=True; Allow Zero Datetime=True;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                drugDisposal_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textBox1.Text))
                {
                    // Query untuk mengambil data dari tabel obat
                    string selectQuery = "SELECT tanggal_kadaluarsa, stok FROM obat WHERE nama_obat = @nama";

                    // Membuka koneksi ke database jika belum terbuka
                    if (koneksi.State == ConnectionState.Closed)
                    {
                        koneksi.Open();
                    }

                    // Mengambil data tgl_kadaluarsa dan stok dari tabel obat
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, koneksi))
                    {
                        selectCommand.Parameters.AddWithValue("@nama", comboBox1.Text);

                        using (MySqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                DateTime tglKadaluarsa = reader.GetDateTime("tanggal_kadaluarsa");
                                int stok = reader.GetInt32("stok");

                                // Tutup reader sebelum menjalankan query INSERT
                                reader.Close();

                                // Query untuk memasukkan data ke tabel pemusnahan
                                string insertQuery = "INSERT INTO pemusnahan (nama_obat, tgl_kadaluarsa, jumlah, deskripsi) " +
                                                     "VALUES (@nama, @tgl, @jumlah, @deskripsi);";

                                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, koneksi))
                                {
                                    // Menambahkan parameter ke query
                                    insertCommand.Parameters.AddWithValue("@nama", comboBox1.Text);
                                    insertCommand.Parameters.AddWithValue("@tgl", tglKadaluarsa);
                                    insertCommand.Parameters.AddWithValue("@jumlah", stok);
                                    insertCommand.Parameters.AddWithValue("@deskripsi", textBox1.Text); // Deskripsi konstan

                                    int res = insertCommand.ExecuteNonQuery();

                                    if (res == 1)
                                    {
                                        // Pengurangan stok di tabel obat
                                        string updateObatQuery = "UPDATE obat SET stok = stok - @jumlah WHERE nama_obat = @nama";
                                        using (MySqlCommand updateObatCommand = new MySqlCommand(updateObatQuery, koneksi))
                                        {
                                            updateObatCommand.Parameters.AddWithValue("@nama", comboBox1.Text);
                                            updateObatCommand.Parameters.AddWithValue("@jumlah", stok);
                                            updateObatCommand.ExecuteNonQuery();
                                        }

                                        // Hapus obat jika stok habis
                                        string deleteObatQuery = "DELETE FROM obat WHERE nama_obat = @nama AND stok <= 0";
                                        using (MySqlCommand deleteObatCommand = new MySqlCommand(deleteObatQuery, koneksi))
                                        {
                                            deleteObatCommand.Parameters.AddWithValue("@nama", comboBox1.Text);
                                            deleteObatCommand.ExecuteNonQuery();
                                        }

                                        // Hapus stok di tabel stok_masuk
                                        string deleteStokMasukQuery = "DELETE FROM stok_masuk WHERE nama_obat = @nama";
                                        using (MySqlCommand deleteStokMasukCommand = new MySqlCommand(deleteStokMasukQuery, koneksi))
                                        {
                                            deleteStokMasukCommand.Parameters.AddWithValue("@nama", comboBox1.Text);
                                            deleteStokMasukCommand.ExecuteNonQuery();
                                        }

                                        MessageBox.Show("Insert Data Sukses ...");
                                        drugDisposal_Load(null, null);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Gagal Insert Data . . . ");
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Obat tidak ditemukan dalam database.");
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
            finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }



        }

        private void label40_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
        }

        private void label37_Click(object sender, EventArgs e)
        {
            FinancialS financial = new FinancialS();
            financial.Show();
        }

        private void label36_Click(object sender, EventArgs e)
        {
            CustomerM customer = new CustomerM();
            customer.Show();
        }

        private void label38_Click(object sender, EventArgs e)
        {
            Salestransaction sales = new Salestransaction();
            sales.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            StockManagement stok = new StockManagement();
            stok.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Drugandstock drug = new Drugandstock();
            drug.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void drugDisposal_Load(object sender, EventArgs e)
        {
            try
            {
                if (koneksi.State == ConnectionState.Closed)
                {
                    koneksi.Open();
                }

                query = "SELECT nama_obat, DATE_FORMAT(tgl_kadaluarsa, '%Y-%m-%d') AS tgl_kadaluarsa, jumlah, deskripsi FROM pemusnahan ORDER BY nama_obat ASC";
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);

                ds.Clear();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                dataGridView1.Columns[0].Width = 225;
                dataGridView1.Columns[0].HeaderText = "Drug Name";
                dataGridView1.Columns[1].Width = 225;
                dataGridView1.Columns[1].HeaderText = "Expired Date";
                dataGridView1.Columns[2].Width = 225;
                dataGridView1.Columns[2].HeaderText = "Amount";
                dataGridView1.Columns[3].Width = 227;
                dataGridView1.Columns[3].HeaderText = "Description";

                string queryObat = "SELECT DISTINCT nama_obat FROM obat";
                perintah = new MySqlCommand(queryObat, koneksi);
                using (MySqlDataReader reader = perintah.ExecuteReader())
                {
                    comboBox1.Items.Clear();
                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["nama_obat"].ToString());
                    }
                }

                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                if (koneksi.State == ConnectionState.Open)
                {
                    koneksi.Close();
                }
            }

        }
    }
}
