using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apotek_xyz
{
    public partial class FAdmin_Home : Form
    {
        SqlCommand cmd;
        public FAdmin_Home()
        {
            InitializeComponent();
            display_dgv();
            display_cmbTanggal();
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

                var sql = $"usp_list_log ''";
                cmd = new SqlCommand(sql, conn);
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
            }
            
        }

        public void display_cmbTanggal()
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                var sql = $"usp_list_log ''";
                var cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteNonQuery();

                string[] arr = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    arr[i] = dt.Rows[i]["waktu"].ToString().Substring(0, 10);
                }

                string[] date = arr.Distinct().ToArray();
                for (int i = 0; i < date.Length; i++)
                {
                    cmbTanggal.Items.Add(date[i]);
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
            }
            

        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if(conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string date = cmbTanggal.Text;
                string[] dates = date.Split('/');

                

                SqlCommand cmd = new SqlCommand($"usp_search_bydate_log '{dates[0]}', '{dates[1]}', '{dates[2]}'",conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                cmd.ExecuteNonQuery();
                sda.Fill(dt);

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

        private void cmbTanggal_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            FLogin newpage = new FLogin();
            newpage.Show();
        }

        private void btnKelolaUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_User newpage = new FAdmin_User();
            newpage.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_Obat newpage = new FAdmin_Obat();
            newpage.Show();
        }

        private void btnKelolaLaporan_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_Laporan newpage = new FAdmin_Laporan();
            newpage.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            display_dgv();
        }
    }
}
