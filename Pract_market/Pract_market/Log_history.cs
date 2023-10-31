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
    public partial class Log_history : Form
    {
        private static string connectionString = "Data Source = LAPTOP-B3GKVM66;Initial Catalog = prakt_market;Integrated Security = true;";
        public Log_history()
        {
            InitializeComponent();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select Login from STAFF";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet data = new DataSet();
                dataAdapter.Fill(data);
                conn.Close();
                comboBox1.Items.Add("<Select login>");
                for (int i = 0; i < data.Tables[0].Columns[0].Table.Rows.Count; i++)
                {
                    comboBox1.Items.Add(data.Tables[0].Columns[0].Table.Rows[i].ItemArray[0].ToString().Trim(' '));
                }
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void Log_history_Load(object sender, EventArgs e)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                sqlcon.Open();
                SqlCommand cmd = sqlcon.CreateCommand();
                cmd.CommandText = $"select id_record, Data_time_entrance, [Login], Attempt from LOG_HISTORY, STAFF where Id_staff = Employee";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataSet data = new DataSet();
                dataAdapter.Fill(data);
                sqlcon.Close();
                dataGridView1.DataSource = data.Tables[0];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sender == button1) // выход
            {
                Close();
            }
            else if (sender == button2) // фильтр
            {
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    SqlCommand cmd1 = sqlcon.CreateCommand();
                    cmd1.CommandText = $"select id_record, Data_time_entrance, [Login], Attempt from LOG_HISTORY, STAFF where Id_staff = Employee and Login = '{comboBox1.Text}'";
                    SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                    DataSet data1 = new DataSet();
                    dataAdapter1.Fill(data1);
                    sqlcon.Close();
                    dataGridView1.DataSource = data1.Tables[0];
                }
            }
            else if (sender == button3) // сортировка
            {
                if (comboBox2.SelectedIndex == 1)
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = "select id_record, Data_time_entrance, [Login], Attempt from LOG_HISTORY, STAFF where Id_staff = Employee order by Data_time_entrance asc";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
                else if (comboBox2.SelectedIndex == 2)
                {
                    using (SqlConnection sqlcon = new SqlConnection(connectionString))
                    {
                        sqlcon.Open();
                        SqlCommand cmd1 = sqlcon.CreateCommand();
                        cmd1.CommandText = "select id_record, Data_time_entrance, [Login], Attempt from LOG_HISTORY, STAFF where Id_staff = Employee order by Data_time_entrance desc";
                        SqlDataAdapter dataAdapter1 = new SqlDataAdapter(cmd1);
                        DataSet data1 = new DataSet();
                        dataAdapter1.Fill(data1);
                        sqlcon.Close();
                        dataGridView1.DataSource = data1.Tables[0];
                    }
                }
            }
        }
    }
}
