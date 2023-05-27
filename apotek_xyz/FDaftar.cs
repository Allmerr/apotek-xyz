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
    public partial class FDaftar : Form
    {
        SqlCommand cmd;
        public FDaftar()
        {
            InitializeComponent();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDaftar_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }


                if (txtUsername.Text != "" && txtPassword.Text != "")
                {
                    var sql = $"usp_insert_user 'Costumer', '{txtNamaUser.Text}', '{txtAlamat.Text}', '{txtTelpon.Text}', '{txtUsername.Text}', '{txtPassword.Text}'";
                    cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    MessageBox.Show("Berhasil Mendaftar!");
                    FLogin newpage = new FLogin();
                    newpage.Show();
                }
                else
                {
                    MessageBox.Show("Mohon isi data diri!");
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            FLogin newpage = new FLogin();
            newpage.Show();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtNamaUser.Clear();
            txtAlamat.Clear();
            txtTelpon.Clear();
            txtUsername.Clear();
            txtPassword.Clear();
        }
    }
}
