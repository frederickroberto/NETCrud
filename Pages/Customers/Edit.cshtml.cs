using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using NETProject.Models; // Import Customer model dari Models folder

namespace NETProject.Pages.Customers
{
    public class Edit : PageModel
    {
        [BindProperty]
        public Customer Customer { get; set; } // Properti untuk data customer yang diedit

        public IActionResult OnGet(int id)
        {
            string connectionString = "Server=localhost;Database=customer;User ID=root;Password=;SslMode=none;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM customer WHERE id = @Id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Customer = new Customer
                    {
                        Id = reader.GetInt32("id"),
                        Firstname = reader.GetString("firstname"),
                        Lastname = reader.GetString("lastname"),
                        Email = reader.GetString("email"),
                        Phone = reader.GetString("phone"),
                        Address = reader.GetString("address"),
                        Company = reader.GetString("company"),
                        Notes = reader.GetString("notes")
                    };
                }
                else
                {
                    return NotFound();
                }
                connection.Close();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = "Server=localhost;Database=customer;User ID=root;Password=;SslMode=none;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                    UPDATE customer 
                    SET firstname = @Firstname, 
                        lastname = @Lastname, 
                        email = @Email, 
                        phone = @Phone, 
                        address = @Address, 
                        company = @Company, 
                        notes = @Notes 
                    WHERE id = @Id";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Firstname", Customer.Firstname);
                command.Parameters.AddWithValue("@Lastname", Customer.Lastname);
                command.Parameters.AddWithValue("@Email", Customer.Email);
                command.Parameters.AddWithValue("@Phone", Customer.Phone);
                command.Parameters.AddWithValue("@Address", Customer.Address);
                command.Parameters.AddWithValue("@Company", Customer.Company);
                command.Parameters.AddWithValue("@Notes", Customer.Notes);
                command.Parameters.AddWithValue("@Id", Customer.Id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            return RedirectToPage("/Customers/Index");
        }
    }
}
