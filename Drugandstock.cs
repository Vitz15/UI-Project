﻿using System;
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
                if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                    string formattedDate2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");

                    // Gunakan parameterized query
                    query = "INSERT INTO obat (id_obat, nama_obat, stok, harga_jual,harga_beli_satuan, tanggal_kadaluarsa,tgl_masuk, kategori, satuan, deskripsi) " +
                            "VALUES (@id, @nama_obat, @stok, @harga_jual,@satuanharga, @tanggal_kadaluarsa,@masuk, @kategori, @satuan, @deskripsi);";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query
                    perintah.Parameters.AddWithValue("@id", textBox8.Text);
                    perintah.Parameters.AddWithValue("@nama_obat", textBox1.Text);
                    perintah.Parameters.AddWithValue("@stok", textBox2.Text);
                    perintah.Parameters.AddWithValue("@harga_jual", textBox4.Text);
                    perintah.Parameters.AddWithValue("@satuanharga", textBox9.Text);
                    perintah.Parameters.AddWithValue("@tanggal_kadaluarsa", formattedDate);
                    perintah.Parameters.AddWithValue("@masuk", formattedDate2);
                    perintah.Parameters.AddWithValue("@kategori", textBox7.Text);
                    perintah.Parameters.AddWithValue("@satuan", textBox6.Text);
                    perintah.Parameters.AddWithValue("@deskripsi", textBox5.Text);

                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Insert Data Sukses ...");
                        Drugandstock_Load(null, null);
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
                query = string.Format("SELECT id_obat,tgl_masuk, nama_obat, kategori,harga_beli_satuan, harga_jual, stok, satuan, tanggal_kadaluarsa, deskripsi FROM obat ORDER BY id_obat ASC");
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);

                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Width = 50;
                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[1].Width = 120;
                dataGridView1.Columns[1].HeaderText = "Date of Entry";
                dataGridView1.Columns[2].Width = 120;
                dataGridView1.Columns[2].HeaderText = "Drug Name";
                dataGridView1.Columns[3].Width = 120;
                dataGridView1.Columns[3].HeaderText = "Category";
                dataGridView1.Columns[4].Width = 120;
                dataGridView1.Columns[4].HeaderText = "Unit Purchase Price";
                dataGridView1.Columns[5].Width = 100;
                dataGridView1.Columns[5].HeaderText = "Price";
                dataGridView1.Columns[6].Width = 80;
                dataGridView1.Columns[6].HeaderText = "Stok";
                dataGridView1.Columns[7].Width = 80;
                dataGridView1.Columns[7].HeaderText = "Unit";
                dataGridView1.Columns[8].Width = 120;
                dataGridView1.Columns[8].HeaderText = "Expired Date";
                dataGridView1.Columns[9].Width = 127;
                dataGridView1.Columns[9].HeaderText = "Description";

                dateTimePicker1.Value = DateTime.Now;
                dateTimePicker2.Value = DateTime.Now;
                textBox8.Clear();
                textBox1.Clear();
                textBox2.Clear();
                textBox4.Clear();
                textBox7.Clear();
                textBox6.Clear();
                textBox5.Clear();
                textBox9.Clear();
                button5.Enabled = true;


              

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
                            textBox9.Text = kolom["harga_beli_satuan"].ToString();
                            textBox4.Text = kolom["harga_jual"].ToString();
                            textBox7.Text = kolom["kategori"].ToString();
                            textBox6.Text = kolom["satuan"].ToString();
                            textBox5.Text = kolom["deskripsi"].ToString();
                            textBox1.Text = kolom["nama_obat"].ToString();

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
                if (textBox1.Text != "" && textBox2.Text != "" && textBox9.Text != "" && textBox4.Text != "" && textBox7.Text != "" && textBox6.Text != "" && textBox5.Text != "" && textBox8.Text !="")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                    query = "UPDATE obat SET nama_obat = @nama_obat, stok = @stok,harga_beli_satuan = @harga_satuan, harga_jual = @harga_jual, tanggal_kadaluarsa = @tanggal_kadaluarsa, kategori = @kategori, satuan = @satuan, deskripsi = @deskripsi WHERE id_obat = @id";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query untuk mencegah SQL Injection
                    
                    perintah.Parameters.AddWithValue("@nama_obat", textBox1.Text);
                    perintah.Parameters.AddWithValue("@stok", textBox2.Text);
                    perintah.Parameters.AddWithValue("@harga_satuan", textBox9.Text);
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

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(textBox9.Text, out decimal unitPurchasePrice))
            {
                // Kalkulasi finalPrice dengan menambahkan 2000
                int finalPrice = (int)(unitPurchasePrice + 2000); // Casting ke integer

                // Tampilkan hasil di TextBox
                textBox4.Text = finalPrice.ToString(); // Tampilkan sebagai string
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
