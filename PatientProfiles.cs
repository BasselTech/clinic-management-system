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

namespace Clinic_Management_System
{
    public partial class PatientProfiles : Form
    {
        public PatientProfiles()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
        SqlCommand command;
        private void updateList(string query)
        {
            command = con.CreateCommand();
            command.CommandText = "SELECT account_id, account_name, account_type FROM account WHERE account_type=2 AND (account_name LIKE @query OR account_phone LIKE @query)";
            command.Parameters.AddWithValue("@query", query + "%");
            con.Open();

            SqlDataReader reader = command.ExecuteReader();
            listBox1.Items.Clear();

            while(reader.Read())
            {
                listBox1.Items.Add(new account(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
            }

            con.Close();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            updateList(textBox4.Text);
        }

        private void PatientProfiles_Load(object sender, EventArgs e)
        {
            updateList("");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Inputs Validation
            if(textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please check the inputs!");
                return;
            }

            //Account Creation
            command = con.CreateCommand();
            command.CommandText = "INSERT INTO account (account_name, account_phone, account_notes, account_type, account_creation_date) VALUES (@name, @phone, @notes, 2, @date)";
            command.Parameters.AddWithValue("@name", textBox1.Text);
            command.Parameters.AddWithValue("@phone", textBox2.Text);
            command.Parameters.AddWithValue("@notes", textBox3.Text);
            command.Parameters.AddWithValue("@date", DateTime.Now);
            con.Open();

            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Account was created!");
            else
                MessageBox.Show("Failed to create the account!");
            con.Close();
            updateList("");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0 || listBox1.SelectedIndex >= listBox1.Items.Count)
                return;
            int account_id = ((account)listBox1.SelectedItem).getID();
            command = con.CreateCommand();
            command.CommandText = "SELECT account_name, account_dob, account_phone, account_notes, account_creation_date FROM account WHERE account_id=@id";
            command.Parameters.AddWithValue("@id", account_id);

            con.Open();

            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
            {
                textBox5.Text = account_id.ToString();
                textBox6.Text = reader.GetString(0);

                DateTime dob = new DateTime();
                if (DateTime.TryParse(reader.GetValue(1).ToString(), out dob))
                    dateTimePicker1.Value = dob;
                textBox7.Text = reader.GetString(2);
                textBox8.Text = reader.GetString(3);
                textBox9.Text = reader.GetValue(4).ToString();
            }

            con.Close();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Inputs Validation
            if(textBox6.Text == "" || textBox7.Text == "")
            {
                MessageBox.Show("Please check the inputs!");
                return;
            }

            //Editing the account
            command = con.CreateCommand();
            command.CommandText = "UPDATE account SET account_name = @name, account_phone = @phone, account_dob = @dob, account_notes = @notes WHERE account_id = @id";
            command.Parameters.AddWithValue("@name", textBox6.Text);
            command.Parameters.AddWithValue("@phone", textBox7.Text);
            command.Parameters.AddWithValue("@dob", dateTimePicker1.Value.ToString());
            command.Parameters.AddWithValue("@notes", textBox8.Text);
            command.Parameters.AddWithValue("@id", textBox5.Text);

            con.Open();

            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Account was updated!");
            else
                MessageBox.Show("Failed to updated the account!");
        
            con.Close();
        }
    }
}
