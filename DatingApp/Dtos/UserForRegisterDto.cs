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
        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastOnline { get; set; }
        public UserForRegisterDto()
        {
            Created = DateTime.Now;
            LastOnline = DateTime.Now;
        }
    }
}
