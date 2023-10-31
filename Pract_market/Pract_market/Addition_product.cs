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
    public partial class Addition_product : Form
    {
        private bool add;
        public int id; // id изменяемой строки
        private static string connectionString = "Data Source = LAPTOP-B3GKVM66;Initial Catalog = prakt_market;Integrated Security = true;";
        public string[] row; // строка, которая будет изменятся
        public Addition_product(bool flag)
        {
            InitializeComponent();
            this.add = flag;
            if (flag)
            {
                button1.Text = "ADD";
                this.Text = "Addition product";
            }
            else if (!flag)
            {
                button1.Text = "EDIT";
                this.Text = "Editing product";
            }
        }

        private void AddRow()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO PRODUCT values ('{textBox1.Text}', '{int.Parse(comboBox1.Text)}', {int.Parse(textBox2.Text)})");          
                command.Connection = conn;
                command.ExecuteNonQuery();
            }
        }

        private void ChangeRow()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand($"UPDATE PRODUCT SET Name_product = {textBox1.Text}, Unit = '{int.Parse(comboBox1.Text)}', [Price ($)] = {int.Parse(textBox2.Text)} WHERE [Код товара] = {id}");
                command.Connection = conn;
                command.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (add)
            {
                AddRow();
            }
            else if (!add)
            {              
                ChangeRow();
            }
        }

        public int setId
        {
            set { id = value; }
        }
        public string[] Row
        {
            set { row = value; }
        }

        private void Addition_product_Load(object sender, EventArgs e)
        {
            if (add)
            {
                button1.Text = "ADD";
                this.Text = "Addition product";
            }
            else if (!add) // СЮДА НАДО!!!!
            {
                textBox1.Text = Convert.ToString(row[1]);
                comboBox1.Text = Convert.ToString(row[2]);
                textBox2.Text = Convert.ToString(row[3]);
                
            }
        }
    }
}
