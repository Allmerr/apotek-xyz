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
    public partial class FCostumer_Transaksi : Form
    {
        SqlCommand cmd;
        public FCostumer_Transaksi()
        {
            InitializeComponent();
        }

        public void getTotal()
        {
            int total = 0;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                //total += dgv.Rows[i].Cells[""]
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            var conn = new SqlConnection(connection.getKoneksi());
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cmd = new SqlCommand($"SELECT * FROM Tbl_Resep WHERE No_Resep = '{txtKodeResep.Text}' WHERE Is_Deleted = 0", conn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                cmd.ExecuteNonQuery();
                dgv.DataSource = dt;

                getTotal();
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
