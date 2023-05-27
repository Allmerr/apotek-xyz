using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apotek_xyz
{
    public partial class FAdmin_Laporan : Form
    {
        SqlCommand cmd;
        public FAdmin_Laporan()
        {
            InitializeComponent();
            display_dgv();
        }


        public void display_dgv()
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if(conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd = new SqlCommand("usp_list_transaksi ''",conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteNonQuery();

                dgv.DataSource = dt;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }

        public void display_chart()
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if(conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                DateTime date1 = dtp1.Value;
                DateTime date2 = dtp2.Value;
                cmd = new SqlCommand($"usp_list_transaksi_between_date '{date1.Date.ToString("yyyy-MM-dd")}', '{date2.Date.ToString("yyyy-MM-dd")}'", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                chart.DataSource = ds;
                chart.Series["laporan"].XValueMember = "Tgl_Transaksi";
                chart.Series["laporan"].YValueMembers = "Total_Bayar";
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }

        private void btnKelolaActivity_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_Home newpage = new FAdmin_Home();
            newpage.Show();
        }

        private void btnKelolaUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_User newpage = new FAdmin_User();
            newpage.Show();
        }

        private void btnKelolaObat_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_Obat newpage = new FAdmin_Obat();
            newpage.Show();
        }

        private void btnKelolaLaporan_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            FLogin newpage = new FLogin();
            newpage.Show();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                foreach (var item in chart.Series)
                {
                    item.Points.Clear(); 
                }
                 
                DateTime date1 = dtp1.Value;
                DateTime date2 = dtp2.Value;

                cmd = new SqlCommand($"usp_list_transaksi_between_date '{date1.Date.ToString("yyyy-MM-dd")}', '{date2.Date.ToString("yyyy-MM-dd")}'", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteNonQuery();

                dgv.DataSource = dt;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.chart.Series["laporan"].Points.AddXY(dt.Rows[i]["Tgl_Transaksi"].ToString(), dt.Rows[i]["Total_Bayar"].ToString());
                }

            
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
        }

        private void chart_Click(object sender, EventArgs e)
        {

        }
    }
}
