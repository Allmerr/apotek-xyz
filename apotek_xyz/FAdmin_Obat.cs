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
    public partial class FAdmin_Obat : Form
    {
        SqlCommand cmd;

        public static string id;
        public FAdmin_Obat()
        {
            InitializeComponent();
            display_dgv();
        }

        public void refresh()
        {
            txtKodeObat.Clear();
            txtNamaObat.Clear();
            dtpExpiredDate.Value = DateTime.Now;
            txtJumlah.Clear();
            txtHargaPerUnit.Clear();

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

                var cmd = new SqlCommand("usp_list_master_obat ''",conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteNonQuery();

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

        private void btnKelolaActivity_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_Home newpage = new FAdmin_Home();
            newpage.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            FLogin newpage = new FLogin();
            newpage.Show();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            if(MessageBox.Show("Yakin Ingin Menambah Data!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if(conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    DateTime date = dtpExpiredDate.Value;
                    cmd = new SqlCommand($"usp_insert_master_obat '{txtKodeObat.Text}', '{txtNamaObat.Text}', '{date.Date.ToString("yyyy-MM-dd")}', '{txtJumlah.Text}', '{txtHargaPerUnit.Text}'", conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Berhasil Menambah Obat!");
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
            else
            {
            }
            display_dgv();
        }

        private void btnKelolaUser_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_User newpage = new FAdmin_User();
            newpage.Show();
        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                id = dgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtKodeObat.Text = dgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtNamaObat.Text = dgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                dtpExpiredDate.Value = DateTime.Parse(dgv.Rows[e.RowIndex].Cells[3].Value.ToString());
                txtJumlah.Text = dgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtHargaPerUnit.Text = dgv.Rows[e.RowIndex].Cells[5].Value.ToString();

                btnTambah.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception)
            {

            }
            
        }

        private void txtSeacrh_TextChanged(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cmd = new SqlCommand($"usp_list_master_obat '{txtSeacrh.Text}'", conn);
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

        private void lb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            refresh();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            if (MessageBox.Show("Yakin Ingin Mengubah Data!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if(conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    DateTime date = dtpExpiredDate.Value;
                    cmd = new SqlCommand($"usp_update_master_obat '{id}', '{txtKodeObat.Text}', '{txtNamaObat.Text}', '{date.Date.ToString("yyyy-MM-dd")}', '{txtJumlah.Text}', '{txtHargaPerUnit.Text}'", conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Berhasil Mengubah Obat!");
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
            refresh();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            if (MessageBox.Show("Yakin Ingin Menghapus Data!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if(conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    cmd = new SqlCommand($"usp_delete_master_obat '{txtKodeObat.Text}', '{txtNamaObat.Text}'", conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhaasil Menghapus Obat!");
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
            refresh();
        }

        private void btnKelolaLaporan_Click(object sender, EventArgs e)
        {
            this.Hide();
            FAdmin_Laporan newpage = new FAdmin_Laporan();
            newpage.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
