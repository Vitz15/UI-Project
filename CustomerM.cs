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
    public partial class CustomerM : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public CustomerM()
        {
            alamat = "server=localhost; database=db_apotek; username=root; password=; Convert Zero Datetime=True; Allow Zero Datetime=True;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
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
            
        }
       
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
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

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void CustomerM_Load(object sender, EventArgs e)
        {
            GenerateNewId();
            try
            {
                koneksi.Open();
                query = string.Format("SELECT Id, tgl_transaksi, nama_pelanggan, no_telp FROM pelanggan_umum ORDER BY Id ASC");
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);

                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Width = 200;
                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[1].Width = 250;
                dataGridView1.Columns[1].HeaderText = "Date";
                dataGridView1.Columns[2].Width = 250;
                dataGridView1.Columns[2].HeaderText = "Name";
                dataGridView1.Columns[3].Width = 215;
                dataGridView1.Columns[3].HeaderText = "Phone Number";
               

                dateTimePicker1.Value = DateTime.Now;
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                button3.Enabled = true;




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {
            StockManagement stok = new StockManagement();
            stok.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerM_Load(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "")
                {
                    if (MessageBox.Show("Anda Yakin Menghapus Data Ini ??", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        query = string.Format("Delete from pelanggan_umum where Id = '{0}'", textBox1.Text);
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
                    CustomerM_Load(null, null);
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
        private void GenerateNewId()
        {
            try
            {
                koneksi.Open();

                // Query to get the last ID
                string lastIdQuery = "SELECT Id FROM pelanggan_umum ORDER BY Id DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(lastIdQuery, koneksi);
                object result = cmd.ExecuteScalar();

                string newId;

                if (result != null)
                {
                    // Extract the numeric part and increment
                    string lastId = result.ToString();
                    string numericPart = lastId.Replace("NONSUB", ""); // Remove the prefix
                    int lastIdNumber = int.Parse(numericPart);
                    newId = "NONSUB" + (lastIdNumber + 1).ToString("D3"); // Format as 3 digits with leading zeros
                }
                else
                {
                    // Start with "NONSUB001" if no records exist
                    newId = "NONSUB001";
                }

                koneksi.Close();

                // Assign the new ID to the textBox1
                textBox1.Text = newId;
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


        private void button7_Click(object sender, EventArgs e)
        {
            GenerateNewId();
            try
            {
                // Pastikan textBox2 dan textBox3 diisi
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                    // Gunakan parameterized query untuk mencegah SQL Injection
                    query = "INSERT INTO pelanggan_umum (Id, nama_pelanggan, no_telp, tgl_transaksi)" +
                            "VALUES (@id, @pelanggan, @no, @tgl);";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query
                    perintah.Parameters.AddWithValue("@id", textBox1.Text);
                    perintah.Parameters.AddWithValue("@pelanggan", textBox2.Text);
                    perintah.Parameters.AddWithValue("@no", textBox3.Text);  // Nomor telepon diambil dari textBox3
                    perintah.Parameters.AddWithValue("@tgl", formattedDate);

                    // Eksekusi query
                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Insert Data Sukses ...");
                        CustomerM_Load(null, null); // Memuat ulang data
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                    query = "UPDATE pelanggan_umum SET tgl_transaksi = @tgl, nama_pelanggan = @pelanggan, no_telp = @no WHERE Id = @id";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query untuk mencegah SQL Injection

                    perintah.Parameters.AddWithValue("@id", textBox1.Text);
                    perintah.Parameters.AddWithValue("@tgl", formattedDate);
                    perintah.Parameters.AddWithValue("@pelanggan", textBox2.Text);
                    perintah.Parameters.AddWithValue("@no", textBox3.Text);

                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Update Data Suksess ...");
                        CustomerM_Load(null, null);
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox7.Text != "")
                {
                    query = string.Format("select * from pelanggan_umum where nama_pelanggan = '{0}'", textBox7.Text);
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
                            textBox1.Text = kolom["Id"].ToString();
                            textBox2.Text = kolom["nama_pelanggan"].ToString();
                            textBox3.Text = kolom["no_telp"].ToString();
                          

                        }
                        textBox7.Enabled = true;
                        dataGridView1.DataSource = ds.Tables[0];
                        button3.Enabled = true;
                        button4.Enabled = true;
                        button5.Enabled = true;

                    }
                    else
                    {
                        MessageBox.Show("Data Tidak Ada !!");
                        CustomerM_Load(null, null);
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

        private void label13_Click_1(object sender, EventArgs e)
        {
            drugDisposal dis = new drugDisposal();
            dis.Show();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            SubscribedCostumer subs = new SubscribedCostumer();
            subs.Show();
        }
    }
}
