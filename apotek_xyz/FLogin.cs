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
    public partial class FLogin : Form
    {
        SqlCommand cmd;
        public FLogin()
        {
            InitializeComponent();
        }

        public static string id;

        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if(conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                if(txtUsername.Text != "" && txtPassword.Text != "")
                {
                    var sql = $"usp_login '{txtUsername.Text}', '{txtPassword.Text}'";
                    cmd = new SqlCommand(sql, conn);
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    if(dt.Rows.Count > 0)
                    {
                        switch (dt.Rows[0]["Tipe_User"] as string)
                        {
                            case "Admin":
                                MessageBox.Show("Berhasil Login Sebagai Admin!");
                                this.Hide();
                                FAdmin_Home newpage_admin = new FAdmin_Home();
                                newpage_admin.Show();
                                break;
                            case "Apoteker":
                                MessageBox.Show("Berhasil Login Sebagai Apoteker!");
                                this.Hide();
                                FApoteker newpage_apoteker = new FApoteker();
                                newpage_apoteker.Show();
                                break;
                            case "Kasir":
                                id = dt.Rows[0]["Id_User"].ToString();
                                MessageBox.Show("Berhasil Login Sebagai Kasir!");
                                this.Hide();
                                FKasir newpage_kasir = new FKasir();
                                newpage_kasir.Show();
                                break;
                            case "Costumer":
                                id = dt.Rows[0]["Id_User"].ToString();
                                MessageBox.Show("Berhasil Login Sebagai Costumer!");
                                this.Hide();
                                FCostumer_Transaksi newpage_costumer = new FCostumer_Transaksi();
                                newpage_costumer.Show();
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username atau Password salah!");
                    }
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
                if(conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
     
        }

        private void btnDaftar_Click(object sender, EventArgs e)
        {
            this.Hide();
            FDaftar newpage = new FDaftar();
            newpage.Show();
        }

        private void FLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
