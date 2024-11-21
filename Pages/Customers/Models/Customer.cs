using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace NETProject.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [BindProperty, Required(ErrorMessage = "The First Name is Required")]
        public string Firstname { get; set; }
        [BindProperty, Required(ErrorMessage = "The Last Name is Required")]
        public string Lastname { get; set; }
        [BindProperty, Required]
        public string Email { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string Company { get; set; }
        [BindProperty]
        public string Notes { get; set; }
    }
}
