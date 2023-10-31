using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Pract_market
{
    public partial class Sign_In : Form
    {
        private static string connectionString = "Data Source = LAPTOP-B3GKVM66;Initial Catalog = prakt_market;Integrated Security = true;";
        private string text = String.Empty;
        private int attempts = 0;
        private int time;
        private int time_whole = 180;
        public Sign_In()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Not all fields are filled in", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            else
            {
                using (SqlConnection sqlcon = new SqlConnection(connectionString))
                {
                    sqlcon.Open();
                    SqlCommand cmd = sqlcon.CreateCommand();
                    cmd.CommandText = "select Surname_staff, Name_staff, [Login], [Password], Name_position, Id_staff from STAFF, POSITION where Id_position = Position";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataSet data = new DataSet();
                    dataAdapter.Fill(data);
                    sqlcon.Close();
                    bool flag = false;
                    textBox1.Text = textBox1.Text.Trim(' ');
                    textBox2.Text = textBox2.Text.Trim(' ');
                    for (int i = 0; i < data.Tables[0].Columns[0].Table.Rows.Count; i++)
                    {
                        if (textBox1.Text == data.Tables[0].Columns[2].Table.Rows[i].ItemArray[2].ToString().Trim(' ') && textBox2.Text == data.Tables[0].Columns[3].Table.Rows[i].ItemArray[3].ToString().Trim(' '))
                        {
                            sqlcon.Open();
                            SqlCommand cmd_in = sqlcon.CreateCommand();
                            cmd.CommandText = $"INSERT into LOG_HISTORY VALUES ('{DateTime.Now}', {Convert.ToInt32(data.Tables[0].Columns[2].Table.Rows[i].ItemArray[5])}, 'True' )";
                            cmd.ExecuteNonQuery();
                            sqlcon.Close();
                            flag = true;
                            if (data.Tables[0].Columns[2].Table.Rows[i].ItemArray[4].ToString() == "Director")
                            {
                                M_menu_admin mainMenu = new M_menu_admin(Convert.ToInt32(data.Tables[0].Columns[2].Table.Rows[i].ItemArray[5]));
                                this.Hide();
                                mainMenu.ShowDialog();
                                this.Show();

                            }
                            else
                            {
                                M_menu_employee mainMenu = new M_menu_employee(Convert.ToInt32(data.Tables[0].Columns[2].Table.Rows[i].ItemArray[5]));
                                this.Hide();
                                mainMenu.ShowDialog();
                                this.Show();
                            }

                        }
                    }
                    if (!flag)
                    {
                        MessageBox.Show("Invalid username or password", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        textBox2.Clear();
                        attempts++;
                        for (int i = 0; i < data.Tables[0].Columns[0].Table.Rows.Count; i++)
                        {
                            if (data.Tables[0].Columns[0].Table.Rows[i].ItemArray[2].ToString().Trim(' ') == textBox1.Text)
                            {
                                sqlcon.Open();
                                SqlCommand cmd_in = sqlcon.CreateCommand();
                                cmd.CommandText = $"INSERT into LOG_HISTORY VALUES ('{DateTime.Now}', {Convert.ToInt32(data.Tables[0].Columns[2].Table.Rows[i].ItemArray[5])}, 'false' )";
                                cmd.ExecuteNonQuery();
                                sqlcon.Close();
                            }
                        }

                    }
                    if (attempts == 1)
                    {
                        button2.Visible = true;
                        textBox3.Visible = true;
                        pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
                    }
                    if (attempts == 2)
                    {
                        textBox3.Clear();
                        button1.Enabled = false;
                        timer1.Start();
                        textBox3.Visible = false;
                        label4.Visible = true;
                        label4.Text = $"BLOCKING! Time left: {time_whole - time}";
                    }
                    if (attempts == 3)
                    {
                        System.Windows.Forms.Application.Restart();
                    }
                }          
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();
            Bitmap result = new Bitmap(Width, Height);
            int Xpos = rnd.Next(0, Width - 50);
            int Ypos = rnd.Next(15, Height - 15);
            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };
            Graphics g = Graphics.FromImage((Image)result);
            g.Clear(Color.Gray);
            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];
            g.DrawString(text,
                         new Font("Arial", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));
            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);
            return result;
        }

        private void Sign_In_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == ' ') e.Handled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {          
            time++;
            label4.Text = $"BLOCKING! Time left: {time_whole - time} sec";
            if (time == 180)
            {
                timer1.Stop();
                button1.Enabled = true;
                label4.Visible = false;
            }
        }
    }
}
