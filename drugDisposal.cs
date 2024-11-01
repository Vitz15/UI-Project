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


        private void drugDisposal_Load(object sender, EventArgs e)
        {
            try
            {
                koneksi.Open();
                // Ambil data dari tabel pemusnahan dengan memformat tanggal kadaluarsa ke format yyyy-mm-dd
                query = "SELECT nama_obat, DATE_FORMAT(tgl_kadaluarsa, '%Y-%m-%d') AS tgl_kadaluarsa, jumlah FROM pemusnahan ORDER BY nama_obat ASC";
                perintah = new MySqlCommand(query, koneksi);
                adapter = new MySqlDataAdapter(perintah);

                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);

                ds.Clear();
                adapter.Fill(ds);
                koneksi.Close();

                // Tampilkan data di DataGridView untuk pemusnahan
                dataGridView1.DataSource = ds.Tables[0];

                // Atur lebar kolom dan header text untuk DataGridView
                dataGridView1.Columns[0].Width = 300;
                dataGridView1.Columns[0].HeaderText = "Drug Name";
                dataGridView1.Columns[1].Width = 300;
                dataGridView1.Columns[1].HeaderText = "Expired Date";
                dataGridView1.Columns[2].Width = 300;
                dataGridView1.Columns[2].HeaderText = "Amount";

                // Refresh DataGridView
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
