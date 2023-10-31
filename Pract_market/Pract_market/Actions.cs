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
    public partial class Actions : Form
    {
        private bool add;
        public int id; // id изменяемой строки
        public string[] row; // строка, которая будет изменятся
        private static string connectionString = "Data Source = LAPTOP-B3GKVM66;Initial Catalog = prakt_market;Integrated Security = true;";
        public Actions(bool flag)
        {
            InitializeComponent();
            this.add = flag;
            if (flag)
            {
                button1.Text = "ADD";
                this.Text = "Addition staff";
            }
            else if (!flag)
            {
                button1.Text = "EDIT";
                this.Text = "Editing staff";
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select Name_departmant from DEPARTMENT";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet data = new DataSet();
                dataAdapter.Fill(data);
                conn.Close();
                comboBox1.Items.Add("<Select departmant>");
                for (int i = 0; i < data.Tables[0].Columns[0].Table.Rows.Count; i++)
                {
                    comboBox1.Items.Add(data.Tables[0].Columns[0].Table.Rows[i].ItemArray[0].ToString().Trim(' '));
                }
                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandText = "select Name_position from POSITION";
                SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                DataSet data1 = new DataSet();
                dataAdapter1.Fill(data1);
                conn.Close();
                comboBox2.Items.Add("<Select position>");
                for (int i = 0; i < data1.Tables[0].Columns[0].Table.Rows.Count; i++)
                {
                    comboBox2.Items.Add(data1.Tables[0].Columns[0].Table.Rows[i].ItemArray[0].ToString().Trim(' '));
                }
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void AddRow()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO STAFF values ('{textBox1.Text}', '{textBox2.Text}', '{textBox5.Text}', '{textBox6.Text}', '{textBox3.Text}', '{textBox4.Text}', {comboBox1.SelectedIndex},{comboBox2.SelectedIndex})");
                command.Connection = conn;
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void ChangeRow()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand($"UPDATE STAFF SET Surname_staff = '{textBox1.Text}', Name_staff = '{textBox2.Text}', Login = '{textBox5.Text}', Password = '{textBox6.Text}', Address_h = '{textBox3.Text}', STAFF.Phone = '{textBox4.Text}', Department = {comboBox1.SelectedIndex}, Position = {comboBox2.SelectedIndex} WHERE [Id_staff] = {id}");
                command.Connection = conn;
                command.ExecuteNonQuery();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (add)
            {
                AddRow();
                MessageBox.Show("Successfully", " ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                Close();
            }
            else if (!add)
            {
                ChangeRow();
                Close();
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

        private void Actions_Load(object sender, EventArgs e)
        {
            if (add)
            {
                button1.Text = "ADD";
                this.Text = "Addition product";
            }
            else if (!add)
            {
                textBox1.Text = Convert.ToString(row[1]);
                textBox2.Text = Convert.ToString(row[2]);
                textBox5.Text = Convert.ToString(row[3]);
                textBox6.Text = Convert.ToString(row[4]);
                comboBox1.Text = Convert.ToString(row[5]);
                comboBox2.Text = Convert.ToString(row[6]);
                textBox3.Text = Convert.ToString(row[7]);
                textBox4.Text = Convert.ToString(row[8]);
            }
        }
    }
}
