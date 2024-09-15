using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace UI_Project
{
    public partial class FinancialS : Form
    {
        public FinancialS()
        {
            InitializeComponent();
            CreateChart();
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

        private void label4_Click(object sender, EventArgs e)
        {
            Drugandstock drug = new Drugandstock();
            drug.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
        }
        private void CreateChart()
        {
            // Set judul grafik
            chart1.Titles.Add("Grafik Penjualan");

            // Menambahkan Seri (data) ke chart
            Series series = new Series("Penjualan 2024");
            series.ChartType = SeriesChartType.Line; // Bisa diubah ke tipe lain (Bar, Column, dll.)

            // Menambahkan data ke seri
            series.Points.AddXY("Januari", 400);
            series.Points.AddXY("Februari", 600);
            series.Points.AddXY("Maret", 700);
            series.Points.AddXY("April", 500);
            series.Points.AddXY("Mei", 900);

            // Menambahkan seri ke chart
            chart1.Series.Add(series);

            // Mengatur sumbu X dan Y
            chart1.ChartAreas[0].AxisX.Title = "Bulan";
            chart1.ChartAreas[0].AxisY.Title = "Jumlah Penjualan";

            // Mengatur tampilan
            chart1.Series[0].BorderWidth = 3;
            chart1.Series[0].Color = System.Drawing.Color.Blue;
        }
        private void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private void label40_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
        }
    }
}
