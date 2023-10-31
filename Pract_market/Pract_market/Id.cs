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
    public partial class Id : Form
    {
        private static string connectionString = "Data Source = LAPTOP-B3GKVM66;Initial Catalog = prakt_market;Integrated Security = true;";
        private SqlConnection myConnection; // подключение к базе
        private bool edit;
        private int id;
        private string name_table;
        public Id(bool flag, string name_table)
        {
            InitializeComponent();
            this.edit = flag;
            this.name_table = name_table;           
        }
        public int getId
        {
            get { return id; } 

        }

        public string getName
        {
            get { return name_table; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (edit)
            {
                string message = "Вы действительно хотите редактировать выбранную запись?";
                if (MessageBox.Show(message, "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
                id = Convert.ToInt32(textBox1.Text);
                Close();
            }
            else if (!edit)
            {
                string message = "Вы действительно хотите удалить выбранную запись?";
                if (MessageBox.Show(message, "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
                if (name_table == "PRODUCT")
                {
                    myConnection = new SqlConnection(connectionString);
                    myConnection.Open();
                    string cmdDelFromTovari = "Delete from PRODUCT where [Id_product] = @code";
                    SqlCommand cmd1 = new SqlCommand(cmdDelFromTovari, myConnection);
                    SqlParameter pr1 = new SqlParameter("@code", textBox1.Text);
                    cmd1.Parameters.Add(pr1); // добавление параметра в команду
                    cmd1.ExecuteNonQuery(); // выполнение запроса 
                    myConnection.Close();
                }
                else if (name_table == "STAFF")
                {
                    myConnection = new SqlConnection(connectionString);
                    myConnection.Open();
                    string cmdDelFromTovari = "Delete from STAFF where [Id_staff] = @code";
                    SqlCommand cmd1 = new SqlCommand(cmdDelFromTovari, myConnection);
                    SqlParameter pr1 = new SqlParameter("@code", textBox1.Text);
                    cmd1.Parameters.Add(pr1); // добавление параметра в команду
                    cmd1.ExecuteNonQuery(); // выполнение запроса 
                    myConnection.Close();
                }                
                Close();
            }
        }

        private void Id_Load(object sender, EventArgs e)
        {
            if (name_table == "STAFF")
            {
                label1.Text = "Enter the employee code";
            }
            else if (name_table == "PRODUCT")
            {
                label1.Text = "Enter the product code";
            }
        }
    }
}
