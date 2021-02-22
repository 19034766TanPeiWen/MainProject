using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MainProject.Models
{
    public class UserDetails
    {

        [Required(ErrorMessage = "Please enter Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be 5-20 char")]
        public string Password { get; set; }
        
        public string Image { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

    }
}
