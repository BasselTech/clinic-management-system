using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace Clinic_Management_System
{
    public class utils
    {
        public static string hashPassword(string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }

        public static Dictionary<int, string> getSlots()
        {
            Dictionary<int, string> slots = new Dictionary<int, string>();
            slots.Add(1, "Slot 1: From 6:00 PM to 6:30 PM");
            slots.Add(2, "Slot 2: From 6:30 PM to 7:00 PM");
            slots.Add(3, "Slot 3: From 7:00 PM to 7:30 PM");
            slots.Add(4, "Slot 4: From 7:30 PM to 8:00 PM");
            slots.Add(5, "Slot 5: From 8:00 PM to 8:30 PM");
            slots.Add(6, "Slot 6: From 8:30 PM to 9:00 PM");
            slots.Add(7, "Slot 7: From 9:00 PM to 9:30 PM");
            slots.Add(8, "Slot 8: From 9:30 PM to 10:00 PM");
            slots.Add(9, "Slot 9: From 10:00 PM to 10:30 PM");
            slots.Add(10, "Slot 10: From 10:30 PM to 11:00 PM");
            return slots;
        }

        public static void createAdmin(string password)
        {
            SqlConnection con = new SqlConnection(Properties.Resources.connectionString);
            SqlCommand command = con.CreateCommand();

            command.CommandText = "INSERT INTO [user] (user_username, user_password) VALUES (@username, @password)";
            command.Parameters.AddWithValue("@username", "admin");
            command.Parameters.AddWithValue("@password", hashPassword(password));

            con.Open();

            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception)
            {

            }

            con.Close();
        }
    }
}
