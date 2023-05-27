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
    public partial class FAdmin_User : Form
    {
        SqlCommand cmd;

        public static string id;
        public static string username;
        public FAdmin_User()
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

                var sql = $"usp_list_user ''";
                cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                dgv.DataSource = dt;

                btnTambah.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;

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

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());

            try
            {
                if(conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if(MessageBox.Show("Yakin Ingin Menamabah Data!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cmd = new SqlCommand($"usp_insert_user '{cmbTipeUser.Text}', '{txtNama.Text}', '{txtAlamat.Text}', '{txtTelepon.Text}', '{txtUsername.Text}', '{txtPassword.Text}'",conn);
                        cmd.ExecuteNonQuery();
                    }

                display_dgv();
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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            FLogin newpage = new FLogin();
            newpage.Show();
        }

        private void btnKelolaActivity_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_Home newpage = new FAdmin_Home();
            newpage.Show();
        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                id = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                username = dgv.Rows[e.RowIndex].Cells[5].Value.ToString();

                cmbTipeUser.Text = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtNama.Text = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtTelepon.Text = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtAlamat.Text = dgv.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtUsername.Text = dgv.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtPassword.Text = dgv.Rows[e.RowIndex].Cells[6].Value.ToString();

                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnTambah.Enabled = false;
            }
            catch (Exception)
            {

            }
            
        }
        
        public void refresh()
        {
            cmbTipeUser.Text = "";
            txtNama.Clear();
            txtTelepon.Clear();
            txtAlamat.Clear();
            txtUsername.Clear();
            txtPassword.Clear();

            display_dgv();
        }
        private void lb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            refresh();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if(conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if (MessageBox.Show("Yakin Ingin Mengubah Data!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new SqlCommand($"usp_update_user '{id}','{cmbTipeUser.Text}', '{txtNama.Text}', '{txtAlamat.Text}', '{txtTelepon.Text}', '{txtUsername.Text}', '{txtPassword.Text}'",conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Mengubah Data!");
                }
                display_dgv();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if(conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (MessageBox.Show("Yakin Ingin Menghapus Data!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new SqlCommand($"usp_delete_user '{id}', '{txtUsername.Text}'", conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil Mengubah Data!");
                }
                display_dgv();  
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

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if(conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cmd = new SqlCommand($"usp_list_user '{txtSeacrh.Text}'", conn);
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

        private void btnKelolaObat_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_Obat newpage = new FAdmin_Obat();
            newpage.Show();
        }

        private void btnKelolaUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_User newpage = new FAdmin_User();
            newpage.Show();
        }

        private void btnKelolaLaporan_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_Laporan newpage = new FAdmin_Laporan();
            newpage.Show();
        }
    }
}
