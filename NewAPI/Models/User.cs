﻿using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;

namespace NewAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt{ get; set; } = new byte[0];
        public List<Character>? Characters { get; set; }
    }
}
