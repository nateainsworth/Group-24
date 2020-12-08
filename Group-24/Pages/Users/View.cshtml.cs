using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Group_24.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Group_24.Pages.Users
{
    public class ViewModel : PageModel
    {

        public List<User> UserRecords { get; set; }

        [BindProperty(SupportsGet =true)]
        public string Type { get; set; }

        public List<int> AccountType { get; set; } = new List<int> { 0, 1, 2 };



        public IActionResult OnGet()
        {
            string G24database_connection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Nate\source\repos\2\Group-24\Group-24\Data\G24Database.mdf;Integrated Security=True";
            SqlConnection connect = new SqlConnection(G24database_connection);
            connect.Open();

            using (SqlCommand command = new SqlCommand())
            {

                command.Connection = connect;
                //sets all new users to a modlevel of 0
                command.CommandText = @"SELECT * FROM Users";

                if (!(string.IsNullOrEmpty(Type) || Type == "ALL"))
                {
                    command.CommandText += " WHERE ModLevel = @accType";
                    command.Parameters.AddWithValue("@accType", Convert.ToInt32(Type));
                }

                SqlDataReader reader = command.ExecuteReader();

                UserRecords = new List<User>();

                while (reader.Read())
                {
                    User record = new User();
                    record.UserID = reader.GetInt32(0);
                    record.FirstName = reader.GetString(1);
                    record.LastName = reader.GetString(2);
                    record.EmailAddress = reader.GetString(3);
                    record.Password = reader.GetString(4);
                    record.ModLevel = reader.GetInt32(5);


                    UserRecords.Add(record);
                }
                reader.Close();

            }

            return Page();
        }

         
    }
}
