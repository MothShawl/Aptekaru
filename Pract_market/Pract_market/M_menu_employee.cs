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

namespace Pract_market
{
    public partial class M_menu_employee : Form
    {
        private static string connectionString = "Data Source = LAPTOP-B3GKVM66;Initial Catalog = prakt_market;Integrated Security = true;";
        private int id;
        public M_menu_employee(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sender == button1) // продажи
            {
                this.Hide();
                Sales sale = new Sales();
                sale.ShowDialog();
                this.Show();
            }
            else if (sender == button2) // товары
            {
                this.Hide();
                Products products = new Products();
                products.ShowDialog();
                this.Show();
            }
            else if (sender == button3) // выход
            {
                Close();
            }
        }

        private void M_menu_employee_Load(object sender, EventArgs e)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlCommand cmd = sqlcon.CreateCommand();
                cmd.CommandText = $"select Surname_staff, Name_staff, [Login], [Password], Name_position, Id_staff from STAFF, POSITION where Id_position = Position and Id_staff = {id}";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet data = new DataSet();
                dataAdapter.Fill(data);
                sqlcon.Close();
                label1.Text = data.Tables[0].Columns[0].Table.Rows[0].ItemArray[0].ToString();
                label2.Text = data.Tables[0].Columns[0].Table.Rows[0].ItemArray[1].ToString();
                label3.Text = data.Tables[0].Columns[0].Table.Rows[0].ItemArray[4].ToString();
            }
        }
    }
}
