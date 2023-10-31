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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Pract_market
{
    public partial class Addition_sale : Form
    {
        private static string connectionString = "Data Source = LAPTOP-B3GKVM66;Initial Catalog = prakt_market;Integrated Security = true;";
        public Addition_sale()
        {
            InitializeComponent();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select Name_departmant from DEPARTMENT";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet data = new DataSet();
                dataAdapter.Fill(data);
                conn.Close();
                comboBox2.Items.Add("<Select department>");
                for (int i = 0; i < data.Tables[0].Columns[0].Table.Rows.Count; i++)
                {
                    comboBox2.Items.Add(data.Tables[0].Columns[0].Table.Rows[i].ItemArray[0].ToString());
                }
                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandText = "select Name_product from PRODUCT";
                SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                DataSet data1 = new DataSet();
                dataAdapter1.Fill(data1);
                conn.Close();
                comboBox1.Items.Add("<Select product>");
                for (int i = 0; i < data1.Tables[0].Columns[0].Table.Rows.Count; i++)
                {
                    comboBox1.Items.Add(data1.Tables[0].Columns[0].Table.Rows[i].ItemArray[0].ToString());
                }
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO SALE(Product, Date_sale, Department, Quantity) values ({comboBox1.SelectedIndex}, '{dateTimePicker1.Text}',{comboBox2.SelectedIndex}, {int.Parse(textBox1.Text)})");
                command.Connection = conn;
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Successfully", " ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Close();
            }         
        }
    }
}
