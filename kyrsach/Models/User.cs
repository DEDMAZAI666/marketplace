using System;

namespace kyrsach.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime RegDate { get; set; } = DateTime.Now;
        public string Role { get; set; } = "User"; // "User" или "Admin"

        public bool IsAdmin => Role == "Admin";
    }
}