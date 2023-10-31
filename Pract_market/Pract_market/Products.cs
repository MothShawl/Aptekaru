using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Pract_market
{
    public partial class Products : Form
    {
        private static string connectionString = "Data Source = LAPTOP-B3GKVM66;Initial Catalog = prakt_market;Integrated Security = true;";
        private SqlConnection myConnection; // подключение к базе

        public Products()
        {
            InitializeComponent();
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (sender == button1) // добавление продукта
            {
                this.Hide();
                Addition_product add = new Addition_product(true);
                add.ShowDialog();
                this.Show();
                Update1("select Id_product,Name_product, Name_unit, [Price ($)] from PRODUCT,UNIT where Id_unit = Unit ORDER BY [Id_product]");
            }
            else if (sender == button2) // редактирование продукта
            {
                this.Hide();
                Id edit = new Id(true,"PRODUCT");
                edit.ShowDialog();
                this.Show();
                Update1("select Id_product,Name_product, Name_unit, [Price ($)] from PRODUCT,UNIT where Id_unit = Unit ORDER BY [Id_product]");

                this.Hide();
                Addition_product edit1 = new Addition_product(false);
                edit1.setId = edit.getId;
                int index = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++) // поиск нужной строки с id
                {
                    if (int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString()) == edit.getId)
                    {
                        index = i;
                        break;
                    }
                }
                string[] row = new string[dataGridView1.Rows[index].Cells.Count]; // массив строк, в котором хранится строка для изменений 
                for (int i = 0; i < dataGridView1.Rows[index].Cells.Count; i++) // заполняется значениями массив строк
                {
                    row[i] = dataGridView1.Rows[index].Cells[i].Value.ToString();
                }
                edit1.Row = row;              
                edit1.ShowDialog();
                this.Show();
                Update1("select Id_product,Name_product, Name_unit, [Price ($)] from PRODUCT,UNIT where Id_unit = Unit ORDER BY [Id_product]");
            }
            else if (sender == button3) // удаление продукта
            {
                this.Hide();
                Id del = new Id(false,"PRODUCT");
                del.ShowDialog();
                this.Show();
                Update1("select Id_product,Name_product, Name_unit, [Price ($)] from PRODUCT,UNIT where Id_unit = Unit ORDER BY [Id_product]");
            }
            else if (sender == button4) // выход в главное меню
            {
                Close();
            }
            else if (sender == button5) // сортировка
            {
                if (comboBox3.SelectedIndex == 1) // по возрастанию 
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_product,Name_product, Name_unit, [Price ($)] from PRODUCT,UNIT where Id_unit = Unit order by {comboBox2.Text} asc";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
                else if (comboBox3.SelectedIndex == 2) // по убыванию 
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_product,Name_product, Name_unit, [Price ($)] from PRODUCT,UNIT where Id_unit = Unit order by {comboBox2.Text} desc";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
            }
            else if (sender == button6) // фильтр
            {
                if (comboBox1.SelectedIndex > 0) // фильтр для продуктов
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_product,Name_product, Name_unit, [Price ($)] from PRODUCT,UNIT where Id_unit = Unit and Name_product = '{comboBox1.Text}'";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
                else if (comboBox1.SelectedIndex == 0)
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = $"select Id_product,Name_product, Name_unit, [Price ($)] from PRODUCT,UNIT where Id_unit = Unit ORDER BY [Id_product]";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
            }
            label4.Visible = true;
            label4.Text = $"Records: {dataGridView1.RowCount - 1}";
        }

        private void Products_Load(object sender, EventArgs e)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlCommand cmd = sqlcon.CreateCommand();
                cmd.CommandText = "select Id_product,Name_product, Name_unit, [Price ($)] from PRODUCT,UNIT where Id_unit = Unit";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet data = new DataSet();
                dataAdapter.Fill(data);
                sqlcon.Close();
                dataGridView1.DataSource = data.Tables[0];

                sqlcon.Open();
                SqlCommand cmd1 = sqlcon.CreateCommand();
                cmd1.CommandText = "select Name_product from PRODUCT";
                SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                DataSet data1 = new DataSet();
                dataAdapter1.Fill(data1);
                sqlcon.Close();
                comboBox1.Items.Add("<Select product>");
                for (int i = 0; i < data1.Tables[0].Columns[0].Table.Rows.Count; i++)
                {
                    comboBox1.Items.Add(data1.Tables[0].Columns[0].Table.Rows[i].ItemArray[0].ToString().Trim(' '));
                }

                sqlcon.Open();
                SqlCommand cmd2 = sqlcon.CreateCommand();
                cmd2.CommandText = "select Id_product,Name_product, Name_unit as [Unit], [Price ($)] from PRODUCT,UNIT where Id_unit = Unit";
                SqlDataAdapter dataAdapter2 = new SqlDataAdapter(cmd2);
                DataSet data2 = new DataSet();
                dataAdapter.Fill(data2);
                sqlcon.Close();
                dataGridView1.DataSource = data2.Tables[0];
                comboBox2.Items.Add("<Select fild>");
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    comboBox2.Items.Add(dataGridView1.Columns[i].HeaderText);
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
            }
        }
    }
}
