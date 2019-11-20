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
    public partial class Visits : Form
    {
        int account_id, patient_id, reservation_id;

        public Visits(int account_id, int patient_id, int reservation_id)
        {
            InitializeComponent();
            this.account_id = account_id;
            this.patient_id = patient_id;
            this.reservation_id = reservation_id;

            command = con.CreateCommand();

            command.CommandText = "SELECT visit_id FROM visit WHERE visit_reservation_id=@reservation_id";
            command.Parameters.AddWithValue("@reservation_id", reservation_id);

            con.Open();

            var result = command.ExecuteScalar();

            if (result == null)
                groupBox1.Enabled = true;
            else
                groupBox1.Enabled = false;

            con.Close();

            updateList();

        }

        SqlConnection con = new SqlConnection(Properties.Resources.connectionString);

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex < 0 || listBox1.SelectedIndex >= listBox1.Items.Count)
            {
                MessageBox.Show("Please select a visit!");
                return;
            }

            visit v = (visit)listBox1.SelectedItem;
            textBox4.Text = v.visit_id.ToString();
            textBox5.Text = v.patient.ToString();
            textBox6.Text = v.secretary.ToString();
            textBox7.Text = v.doctor.ToString();
            textBox8.Text = v.date.ToString();
            textBox9.Text = v.reasons;
            textBox10.Text = v.diagnosis;
            textBox11.Text = v.notes;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Please enter reasons and diagnosis!");
                return;
            }

            command = con.CreateCommand();

            con.Open();

            command.CommandText = "INSERT INTO visit (visit_reservation_id, visit_reasons, visit_diagnosis, visit_notes, visit_doctor_id, visit_date) VALUES (@id, @reasons, @diagnosis, @notes, @doctor_id, @date)";
            command.Parameters.AddWithValue("@id", reservation_id);
            command.Parameters.AddWithValue("@reasons", textBox1.Text);
            command.Parameters.AddWithValue("@diagnosis", textBox2.Text);
            command.Parameters.AddWithValue("@notes", textBox3.Text);
            command.Parameters.AddWithValue("@doctor_id", account_id);
            command.Parameters.AddWithValue("@date", DateTime.Now);

            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Visit was added!");
            else
                MessageBox.Show("Failed to add the visit!");

            con.Close();

            updateList();
            groupBox1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox9.Text == "" || textBox10.Text == "")
            {
                MessageBox.Show("Please enter reasons and diagnosis!");
                return;
            }

            command = con.CreateCommand();
            command.CommandText = "UPDATE visit SET visit_reasons = @reasons, visit_diagnosis = @diagnosis, visit_notes = @notes WHERE visit_id = @id";
            command.Parameters.AddWithValue("@reasons", textBox9.Text);
            command.Parameters.AddWithValue("@diagnosis", textBox10.Text);
            command.Parameters.AddWithValue("@notes", textBox11.Text);
            command.Parameters.AddWithValue("@id", textBox4.Text);

            con.Open();

            if (command.ExecuteNonQuery() > 0)
                MessageBox.Show("Visit was updated!");
            else
                MessageBox.Show("Failed to update the visit!");

            con.Close();
            updateList();
        }

        SqlCommand command;

        private void updateList()
        {
            command = con.CreateCommand();
            command.CommandText = "SELECT visit_id, patient.account_id, patient.account_name, secretary.account_id, secretary.account_name, doctor.account_id, doctor.account_name, visit_date, visit_reasons, visit_diagnosis, visit_notes FROM visit, reservation, account as patient, account as secretary, account as doctor WHERE visit_reservation_id = reservation_id AND reservation_patient_id = patient.account_id AND reservation_secretary_id = secretary.account_id AND visit_doctor_id = doctor.account_id AND patient.account_id = @patient_id";
            command.Parameters.AddWithValue("@patient_id", patient_id);

            con.Open();

            SqlDataReader reader = command.ExecuteReader();
            listBox1.Items.Clear();

            while(reader.Read())
            {
                int visit_id = reader.GetInt32(0);
                int patient_id = reader.GetInt32(1);
                string patient_name = reader.GetString(2);
                int secretary_id = reader.GetInt32(3);
                string secretary_name = reader.GetString(4);
                int doctor_id = reader.GetInt32(5);
                string doctor_name = reader.GetString(6);
                DateTime date = new DateTime();
                DateTime.TryParse(reader.GetValue(7).ToString(), out date);
                string reasons = reader.GetString(8);
                string diagnosis = reader.GetString(9);
                string notes = reader.GetString(10);

                listBox1.Items.Add(new visit(visit_id, patient_id, patient_name, secretary_id, secretary_name, doctor_id, doctor_name, date, reasons, diagnosis, notes));
            }

            con.Close();
        }
    }
}
