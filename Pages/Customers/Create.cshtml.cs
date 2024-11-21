using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using NETProject.Models; // Impor Customer dari Models
using Microsoft.Extensions.Logging;

namespace NETProject.Pages.Customers
{
    public class Create : PageModel
    {
        private readonly ILogger<Create> _logger; // Untuk logging error
        [BindProperty]
        public Customer Customer { get; set; }

        public Create(ILogger<Create> logger) // Dependency Injection untuk Logger
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = "Server=localhost;Database=customer;User ID=root;Password=;SslMode=none;";

            try
            {
                // Menggunakan MySqlConnection dalam blok using untuk memastikan koneksi ditutup setelah selesai
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO customer (firstname, lastname, email, phone, address, company, notes) " +
                                   "VALUES (@Firstname, @Lastname, @Email, @Phone, @Address, @Company, @Notes)";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Firstname", Customer.Firstname);
                    command.Parameters.AddWithValue("@Lastname", Customer.Lastname);
                    command.Parameters.AddWithValue("@Email", Customer.Email);
                    command.Parameters.AddWithValue("@Phone", Customer.Phone);
                    command.Parameters.AddWithValue("@Address", Customer.Address);
                    command.Parameters.AddWithValue("@Company", Customer.Company);
                    command.Parameters.AddWithValue("@Notes", Customer.Notes);

                    // Membuka koneksi dan menjalankan perintah SQL
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

                // Redirect ke halaman Index setelah berhasil menyimpan data
                return RedirectToPage("/Customers/Index");
            }
            catch (Exception ex)
            {
                // Jika terjadi error, tampilkan log error dan kembalikan ke halaman Create dengan pesan error
                _logger.LogError(ex, "Error occurred while saving customer data.");
                ModelState.AddModelError("", "There was an error while saving the data. Please try again.");
                return Page(); // Kembali ke halaman Create dengan pesan error
            }
        }
    }
}
