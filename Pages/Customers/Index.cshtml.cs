using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using NETProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NETProject.Pages.Customers
{
    public class Index : PageModel
    {
        public List<Customer> Customers { get; set; } = new List<Customer>();

        public void OnGet()
        {
            LoadCustomers(); // Memuat daftar customer saat halaman pertama kali dimuat
        }

        public IActionResult OnPostDelete(int id)
        {
            string connectionString = "Server=localhost;Database=customer;User ID=root;Password=;SslMode=none;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM customer WHERE id = @Id"; // Query untuk menghapus customer berdasarkan ID
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id); // Menambahkan ID customer yang akan dihapus

                connection.Open();
                command.ExecuteNonQuery(); // Eksekusi query penghapusan
                connection.Close();
            }

            // Setelah penghapusan berhasil, refresh daftar customer
            LoadCustomers();

            // Redirect untuk memuat ulang halaman dan menampilkan daftar customer terbaru
            return RedirectToPage();
        }

        private void LoadCustomers()
        {
            string connectionString = "Server=localhost;Database=customer;User ID=root;Password=;SslMode=none;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM customer"; // Query untuk mengambil semua data customer
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                Customers.Clear(); // Pastikan untuk membersihkan list sebelum menambah data baru
                while (reader.Read())
                {
                    Customers.Add(new Customer
                    {
                        Id = reader.GetInt32("id"),
                        Firstname = reader.GetString("firstname"),
                        Lastname = reader.GetString("lastname"),
                        Email = reader.GetString("email"),
                        Phone = reader.GetString("phone"),
                        Address = reader.GetString("address"),
                        Company = reader.GetString("company"),
                        Notes = reader.GetString("notes")
                    });
                }
                connection.Close();
            }
        }
    }
}
