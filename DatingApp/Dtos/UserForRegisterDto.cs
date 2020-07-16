using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        [MinLength(4, ErrorMessage = "Your username must be longer than 4 characters.")]
        public string Username { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 7, ErrorMessage = "Your password must be longer than 7 characters.")]
        public string Password { get; set; }
    }
}
