using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETprojektWEB.Models
{
    public class LoginForm
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email field is required.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage ="Passoword field is required.")]
        [MinLength(3, ErrorMessage = "Password must consist of at least 3 characters")]
        public string Password { get; set; }
    }
}
