using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pract_market
{
    public partial class Sales : Form
    {
        private static string connectionString = "Data Source = LAPTOP-B3GKVM66;Initial Catalog = prakt_market;Integrated Security = true;";
        private SqlConnection myConnection; // подключение к базе

        public Sales()
        {
            InitializeComponent();

        }

        private void Sales_Load(object sender, EventArgs e)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlCommand cmd = sqlcon.CreateCommand();
                cmd.CommandText = "select Id_sale, Name_product as [Product], Date_sale, Name_departmant as [Department], Quantity from SALE, PRODUCT, DEPARTMENT where Id_product = Product and Id_department = Department";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet data = new DataSet();
                dataAdapter.Fill(data);
                sqlcon.Close();
                dataGridView1.DataSource = data.Tables[0];
                comboBox1.Items.Add("<Select fild>");
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    comboBox1.Items.Add(dataGridView1.Columns[i].HeaderText);
                }

                sqlcon.Open();
                SqlCommand cmd1 = sqlcon.CreateCommand();
                cmd1.CommandText = "select Name_product from PRODUCT";
                SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                DataSet data1 = new DataSet();
                dataAdapter1.Fill(data1);
                sqlcon.Close();
                comboBox3.Items.Add("<Select product>");
                for (int i = 0; i < data1.Tables[0].Columns[0].Table.Rows.Count; i++)
                {
                    comboBox3.Items.Add(data1.Tables[0].Columns[0].Table.Rows[i].ItemArray[0].ToString().Trim(' '));
                }

                sqlcon.Open();
                SqlCommand cmd2 = sqlcon.CreateCommand();
                cmd2.CommandText = "select Name_departmant from DEPARTMENT";
                SqlDataAdapter dataAdapter2 = new SqlDataAdapter(cmd2);
                DataSet data2 = new DataSet();
                dataAdapter2.Fill(data2);
                sqlcon.Close();
                comboBox4.Items.Add("<Select department>");
                for (int i = 0; i < data2.Tables[0].Columns[0].Table.Rows.Count; i++)
                {
                    comboBox4.Items.Add(data2.Tables[0].Columns[0].Table.Rows[i].ItemArray[0].ToString().Trim(' '));
                }
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
        }

        // Метод для обновления dataGridView1
        private void Update1(string sql)
        {
            dataGridView1.Columns.Clear();
            myConnection = new SqlConnection(connectionString);
            myConnection.Open();
            SqlCommand cmd = myConnection.CreateCommand(); // создается команда
            cmd.CommandText = sql; // добавление текста запроса 
            SqlDataAdapter adapter = new SqlDataAdapter(cmd); // мост между DataSet и SQL Server
            DataSet m_set = new DataSet(); // хранилище таблицы
            adapter.Fill(m_set); // заполнение DataSet
            dataGridView1.DataSource = m_set.Tables[0]; // заполнение dataGridView1 из таблицы
            myConnection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sender == button1) // добавить продажу
            {
                this.Hide();
                Addition_sale sale = new Addition_sale();
                sale.ShowDialog();
                this.Show();
                Update1("select Id_sale, Name_product, Date_sale, Name_departmant, Quantity from SALE, PRODUCT, DEPARTMENT where Id_product = Product and Id_department = Department");
            }
            else if (sender == button2) // назад
            {
                Close();
            }
            else if (sender == button3) // сортировка
            {
                if (comboBox2.SelectedIndex == 1) // по возрастанию 
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_sale, Name_product as [Product], Date_sale, Name_departmant as [Department], Quantity from SALE, PRODUCT, DEPARTMENT where Id_product = Product and Id_department = Department order by {comboBox1.Text} asc";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
                else if (comboBox2.SelectedIndex == 2) // по убыванию 
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_sale, Name_product as [Product], Date_sale, Name_departmant as [Department], Quantity from SALE, PRODUCT, DEPARTMENT where Id_product = Product and Id_department = Department order by {comboBox1.Text} desc";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
            }
            else if (sender == button4) // фильтр
            {
                if (comboBox3.SelectedIndex > 0 && comboBox4.SelectedIndex > 0) // оба фильтра 
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_sale, Name_product, Date_sale, Name_departmant, Quantity from SALE, PRODUCT, DEPARTMENT where Id_product = Product and Name_departmant = '{comboBox4.Text}' and Id_department = Department and Name_product = '{comboBox3.Text}'";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
                else if (comboBox3.SelectedIndex > 0) // фильтр для продуктов
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_sale, Name_product, Date_sale, Name_departmant, Quantity from SALE, PRODUCT, DEPARTMENT where Id_product = Product and Name_product = '{comboBox3.Text}' and Id_department = Department";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
                else if (comboBox4.SelectedIndex > 0) // фильтр для отделов
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_sale, Name_product, Date_sale, Name_departmant, Quantity from SALE, PRODUCT, DEPARTMENT where Id_product = Product and Name_departmant = '{comboBox4.Text}' and Id_department = Department";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }                
                else if (comboBox3.SelectedIndex == 0 || comboBox4.SelectedIndex == 0)
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_sale, Name_product, Date_sale, Name_departmant, Quantity from SALE, PRODUCT, DEPARTMENT where Id_product = Product and Name_departmant = '{comboBox4.Text}' and Id_department = Department";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
                label5.Visible = true;
                label5.Text = $"Records: {dataGridView1.RowCount - 1}";
            }
        }
    }
}
