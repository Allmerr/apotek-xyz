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
    public partial class FKasir : Form
    {
        DataTable data = new DataTable();
        SqlCommand cmd;
        public static string id_obat;
        public static string id_resep;

        public static int nomor_rows;


        public FKasir()
        {
            InitializeComponent();
            display_cmb_no_resep();
            display();
            init_cmb_nama_obat();
        }

        public void init_cmb_nama_obat()
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                DataTable dt = Config.query($"select * from Tbl_Obat where Is_Deleted = 0");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbNamaObat.Items.Add(dt.Rows[i]["Nama_Obat"].ToString());
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        public void display_cmb_no_resep()
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cmd = new SqlCommand("usp_search_noresep_resep", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteNonQuery();


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbNoResep.Items.Add(dt.Rows[i]["No_Resep"].ToString());
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
            
        public void display()
        {
            data.Columns.Add("Type Resep");
            data.Columns.Add("Id Resep");
            data.Columns.Add("No Resep");
            data.Columns.Add("Tgl Resep");
            data.Columns.Add("Nama Pasien");
            data.Columns.Add("Nama Dokter");
            data.Columns.Add("Id Obat");
            data.Columns.Add("Nama Obat");
            data.Columns.Add("Harga_Per_Obat");
            data.Columns.Add("Jumlah Obat Dibeli");

            dgv.DataSource = data;
        }

        public void clear()
        {
            cmbNoResep.Text = "";
            dtpTglResep.Value = DateTime.Now;
            txtNamaPasien.Clear();
            txtNamaDokter.Clear();
            cmbNamaObat.Text = "";
            txtQuantity.Clear();
            txtHarga.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            FLogin newpage = new FLogin();
            newpage.Show();
        }

        public void jumlah()
        {
            int harga = 0;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                harga += Convert.ToInt32(dgv.Rows[i].Cells[8].Value.ToString()) * Convert.ToInt32(dgv.Rows[i].Cells[9].Value.ToString());
            }
            lblTotal.Text = Convert.ToString(harga);
        }

        private void cmbNoResep_SelectedValueChanged(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cmd = new SqlCommand($"usp_search_obat '{cmbNoResep.Text}'", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteNonQuery();



                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txtNamaPasien.Text = dt.Rows[i]["Nama_Pasien"].ToString();
                    txtNamaDokter.Text = dt.Rows[i]["Nama_Dokter"].ToString();
                    cmbNamaObat.Text = dt.Rows[i]["Nama_ObatDibeli"].ToString();
                    txtHarga.Text = dt.Rows[i]["Harga"].ToString();
                    txtQuantity.Text = dt.Rows[i]["Jumlah_ObatDibeli"].ToString();




                    id_obat = dt.Rows[i]["Id_Obat"].ToString();
                    id_resep = dt.Rows[i]["Id_Resep"].ToString();



                    lblTotal.Visible = true;
                    lblKembalian.Visible = true;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void txtBayar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int total = int.Parse(lblTotal.Text);
                int bayar = int.Parse(txtBayar.Text);

                if (bayar >= total)
                {
                    btnPrintSave.Enabled = true;
                    txtBayar.BackColor = Color.Green;
                }
                else
                {
                    btnPrintSave.Enabled = false;
                    txtBayar.BackColor = Color.Red;
                }

                lblKembalian.Text = Convert.ToString(total - bayar);
            }
            catch (Exception)
            {

            }
            
        }

        private void btnBayar_Click(object sender, EventArgs e)
        {
        }

        private void cmbTypeResep_SelectedValueChanged(object sender, EventArgs e)
        {
            if(cmbTypeResep.Text != "Resep")
            {
                // non resep
                cmbNoResep.Enabled = false;
                dtpTglResep.Enabled = false;
                txtNamaPasien.Enabled = false;
                txtNamaDokter.Enabled = false;
                cmbNamaObat.Enabled = true;
                txtQuantity.Enabled = true;
                txtHarga.Enabled = false;
            }
            else
            {
                cmbNoResep.Enabled = true;
                dtpTglResep.Enabled = true;
                txtNamaPasien.Enabled = false;
                txtNamaDokter.Enabled = false;
                cmbNamaObat.Enabled = false;
                txtQuantity.Enabled = false;
                txtHarga.Enabled = false;
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            data.Rows.Add(cmbTypeResep.Text, id_resep ,cmbNoResep.Text, date.Date.ToString("yyyy-MM-dd") ,txtNamaPasien.Text, txtNamaDokter.Text, id_obat ,cmbNamaObat.Text, txtHarga.Text, txtQuantity.Text);
            this.dgv.DataSource = data;
            clear();
            jumlah();

            id_resep = "";
            id_obat = "";
        }

        private void btnPrintSave_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            if (MessageBox.Show("Yakin Ingin Membayar!", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        DateTime date = dtpTglResep.Value;
                        cmd = new SqlCommand($"usp_insert_transaksi '{date.Date.ToString("yyyy-MM-dd")}-{FLogin.id}', '{date.Date.ToString("yyyy-MM-dd")}', '{lblTotal.Text}', '{FLogin.id}', '{dgv.Rows[i].Cells[6].Value.ToString()}', '{dgv.Rows[i].Cells[1].Value.ToString()}'", conn);
                        cmd.ExecuteNonQuery();
                    }
                    

                    MessageBox.Show("Berhasil Membayar!");
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

        private void btnKosongkan_Click(object sender, EventArgs e)
        {
            dgv.Rows.RemoveAt(nomor_rows);

            //    dgv.Rows.RemoveAt(dgv.SelectedRows[0].Index);
        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                nomor_rows = int.Parse(dgv.Rows[e.RowIndex].Index.ToString());

            }
            catch (Exception)
            {

            }
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

                DataTable dt = Config.query($"select * from Tbl_Obat where Is_Deleted = 0 and Nama_Obat = '{cmbNamaObat.Text}'");


                txtHarga.Text = dt.Rows[0]["Harga"].ToString();
                //txtQuantity.Text = dt.Rows[0]["Jumlah"].ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
