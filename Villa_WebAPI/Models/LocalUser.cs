﻿namespace MagicVilla_API.Models
{
    public class LocalUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;      
        public string Password { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
    }
}
