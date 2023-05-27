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
    public partial class FApoteker : Form
    {
        SqlCommand cmd;
        public string id_obat;


        public static string id;
        public FApoteker()
        {
            InitializeComponent();
            display_dgv();
            display_cmb_nama_obat();
        }

        public void display_dgv()
        {
            var conn = new SqlConnection(connection.getKoneksi());

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cmd = new SqlCommand("usp_list_resep ''", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteNonQuery();

                dgv.DataSource = dt;

                btnTambah.Enabled = true;
                //btnUpdate.Enabled = false;
                Hapus.Enabled = false;

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

        public void display_cmb_nama_obat()
        {
            var conn = new SqlConnection(connection.getKoneksi());

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cmd = new SqlCommand($"usp_search_obatonly_obat", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                cmd.ExecuteNonQuery();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbNamaObat.Items.Add(dt.Rows[i]["Nama_Obat"].ToString());
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

        public void clear()
        {
            txtNoResep.Clear();
            dtpTglResep.Value = DateTime.Now;
            txtNamaDokter.Clear();
            txtNamaPasien.Clear();
            cmbNamaObat.Text = "";
            txtQuantity.Clear();

            display_dgv();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            if (MessageBox.Show("Yakin Ingin Menambah Data!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }


                    // check apakah obat tersedia
                    DataTable data = Config.query($"SELECT * FROM Tbl_Obat where Id_Obat = '{id_obat}'");
                    if( Convert.ToInt32(data.Rows[0]["jumlah"].ToString()) >= Convert.ToInt32( txtQuantity.Text ))
                    {
                        DateTime date = dtpTglResep.Value;
                        cmd = new SqlCommand($"usp_insert_resep '{id_obat}', '{txtNoResep.Text}', '{date.Date.ToString("yyyy-MM-dd")}', '{txtNamaDokter.Text}', '{txtNamaPasien.Text}', '{cmbNamaObat.Text}', '{txtQuantity.Text}'", conn);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Berhasil Menambah Resep!");

                        display_dgv();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show($"Stok obat yang dimiliki kurang! \n{data.Rows[0]["Nama_Obat"].ToString()} memiliki stok {data.Rows[0]["Jumlah"].ToString()}", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());

            if (MessageBox.Show("Yakin Ingin Mengupdate Data!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    DateTime date = dtpTglResep.Value;
                    cmd = new SqlCommand($"usp_update_resep '{id}' ,'{txtNoResep.Text}', '{date.Date.ToString("yyyy-MM-dd")}', '{txtNamaDokter.Text}', '{txtNamaPasien.Text}', '{cmbNamaObat.Text}', '{txtQuantity.Text}'", conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Berhasil Mengubah Resep!");

                    display_dgv();
                    clear();
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
        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            id = dgv.Rows[e.RowIndex].Cells["Id_Resep"].Value.ToString();
            id_obat= dgv.Rows[e.RowIndex].Cells["Id_Obat"].Value.ToString();
            txtNoResep.Text = dgv.Rows[e.RowIndex].Cells["No_Resep"].Value.ToString();
            dtpTglResep.Value = DateTime.Parse(dgv.Rows[e.RowIndex].Cells["Tgl_Resep"].Value.ToString());
            txtNamaDokter.Text = dgv.Rows[e.RowIndex].Cells["Nama_Dokter"].Value.ToString();
            txtNamaPasien.Text = dgv.Rows[e.RowIndex].Cells["Nama_Pasien"].Value.ToString();
            cmbNamaObat.Text = dgv.Rows[e.RowIndex].Cells["Nama_ObatDibeli"].Value.ToString();
            txtQuantity.Text = dgv.Rows[e.RowIndex].Cells["Jumlah_ObatDibeli"].Value.ToString();

            btnTambah.Enabled = false;
            //btnUpdate.Enabled = true;
            Hapus.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());


            if (MessageBox.Show("Yakin Ingin Menghpus Data!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    DateTime date = dtpTglResep.Value;
                    cmd = new SqlCommand($"usp_delete_resep '{id}' , '{txtNoResep.Text}'", conn);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Berhasil Menghapus Resep!");

                    display_dgv();
                    clear();
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
        }

        private void lb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            clear();
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

                cmd = new SqlCommand($"usp_list_resep '{txtSeacrh.Text}'", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteNonQuery();

                dgv.DataSource = dt;

                btnTambah.Enabled = true;
                //btnUpdate.Enabled = false;
                Hapus.Enabled = false;

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

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            FLogin newpage = new FLogin();
            newpage.Show();
        }

        private void FApoteker_Load(object sender, EventArgs e)
        {

        }

        private void cmbNamaObat_SelectedValueChanged(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());


                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    if(cmbNamaObat.Text != "")
                    {
                        
                        DataTable data = Config.query($"select * from Tbl_Obat where Nama_Obat = '{cmbNamaObat.Text}' and Is_Deleted = 0");

                        id_obat = data.Rows[0]["Id_Obat"].ToString();
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
    }
}
