﻿using System.ComponentModel.DataAnnotations;

namespace TaramaMVC.Models
{
        public class Login
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        public bool Remember { get; set; }
    }
    }

