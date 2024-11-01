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

namespace UI_Project
{
    public partial class SubscribedCostumer : Form
    {
        private MySqlConnection koneksi;
        private MySqlDataAdapter adapter;
        private MySqlCommand perintah;
        private DataSet ds = new DataSet();
        private string alamat, query;
        public SubscribedCostumer()
        {
            alamat = "server=localhost; database=db_apotek; username=root; password=; Convert Zero Datetime=True; Allow Zero Datetime=True;";
            koneksi = new MySqlConnection(alamat);
            InitializeComponent();
        }

        private void SubscribedCostumer_Load(object sender, EventArgs e)
        {
            GenerateNewId();
            try
            {
                koneksi.Open();
                query = string.Format("SELECT id_pelanggan,tgl_registrasi, nama_pelanggan, email, no_telp, alamat FROM pelanggan ORDER BY id_pelanggan ASC");
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);

                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Width = 150;
                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[1].Width = 150;
                dataGridView1.Columns[1].HeaderText = "Date";
                dataGridView1.Columns[2].Width = 150;
                dataGridView1.Columns[2].HeaderText = "Name";
                dataGridView1.Columns[3].Width = 150;
                dataGridView1.Columns[3].HeaderText = "Email";
                dataGridView1.Columns[4].Width = 150;
                dataGridView1.Columns[4].HeaderText = "Phone Number";
                dataGridView1.Columns[5].Width = 165;
                dataGridView1.Columns[5].HeaderText = "Address";


                dateTimePicker1.Value = DateTime.Now;
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox6.Clear();
                button3.Enabled = true;




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
                string lastIdQuery = "SELECT id_pelanggan FROM pelanggan ORDER BY id_pelanggan DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(lastIdQuery, koneksi);
                object result = cmd.ExecuteScalar();

                string newId;

                if (result != null)
                {
                    // Extract the numeric part and increment
                    string lastId = result.ToString();
                    string numericPart = lastId.Replace("SUB", ""); // Remove the prefix
                    int lastIdNumber = int.Parse(numericPart);
                    newId = "SUB" + (lastIdNumber + 1).ToString("D3"); // Format as 3 digits with leading zeros
                }
                else
                {
                    // Start with "NONSUB001" if no records exist
                    newId = "SUB001";
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

        private void button2_Click(object sender, EventArgs e)
        {
            CustomerM cus = new CustomerM();
            cus.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Drugandstock drugandstock = new Drugandstock();
            drugandstock.Show();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            StockManagement stok = new StockManagement();
            stok.Show();
        }

        private void label38_Click(object sender, EventArgs e)
        {
            Salestransaction sales = new Salestransaction();
            sales.Show();
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

        private void button7_Click(object sender, EventArgs e)
        {
            GenerateNewId();
            try
            {
                if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                    // Gunakan parameterized query
                    query = "INSERT INTO pelanggan (id_pelanggan, nama_pelanggan, alamat, email, no_telp, tgl_registrasi)" +
                            "VALUES (@id, @pelanggan,@alamat,@email, @no, @tgl);";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query
                    perintah.Parameters.AddWithValue("@id", textBox1.Text);
                    perintah.Parameters.AddWithValue("@pelanggan", textBox2.Text);
                    perintah.Parameters.AddWithValue("@alamat", textBox6.Text);
                    perintah.Parameters.AddWithValue("@email", textBox4.Text);
                    perintah.Parameters.AddWithValue("@no", textBox3.Text);
                    perintah.Parameters.AddWithValue("@tgl", formattedDate);

                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Insert Data Sukses ...");
                        SubscribedCostumer_Load(null, null);
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                SubscribedCostumer_Load(null, null);
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
                        query = string.Format("Delete from pelanggan where id_pelanggan= '{0}'", textBox1.Text);
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
                    SubscribedCostumer_Load(null, null);
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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox6.Text != "")
                {
                    string formattedDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                    query = "UPDATE pelanggan SET tgl_registrasi = @tgl, nama_pelanggan = @pelanggan, alamat = @alamat, email = @email, no_telp = @no WHERE id_pelanggan = @id";

                    koneksi.Open();
                    perintah = new MySqlCommand(query, koneksi);

                    // Menambahkan parameter ke query untuk mencegah SQL Injection

                    perintah.Parameters.AddWithValue("@id", textBox1.Text);
                    perintah.Parameters.AddWithValue("@tgl", formattedDate);
                    perintah.Parameters.AddWithValue("@pelanggan", textBox2.Text);
                    perintah.Parameters.AddWithValue("@no", textBox3.Text);
                    perintah.Parameters.AddWithValue("@alamat", textBox6.Text);
                    perintah.Parameters.AddWithValue("@email", textBox4.Text);

                    adapter = new MySqlDataAdapter(perintah);
                    int res = perintah.ExecuteNonQuery();
                    koneksi.Close();

                    if (res == 1)
                    {
                        MessageBox.Show("Update Data Suksess ...");
                        SubscribedCostumer_Load(null, null);
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
                    query = string.Format("select * from pelanggan where nama_pelanggan = '{0}'", textBox7.Text);
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
                            textBox1.Text = kolom["id_pelanggan"].ToString();
                            textBox2.Text = kolom["nama_pelanggan"].ToString();
                            textBox3.Text = kolom["no_telp"].ToString();
                            textBox4.Text = kolom["email"].ToString();
                            textBox6.Text = kolom["alamat"].ToString();


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
                        SubscribedCostumer_Load(null, null);
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

        private void label13_Click(object sender, EventArgs e)
        {
            drugDisposal dis = new drugDisposal();
            dis.Show();
        }
    }
}
