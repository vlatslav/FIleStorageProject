using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogicLayer.Models
{
    public class UserModel
    {
        [Key]
        public string UserId { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public ICollection<int> FilesId { get; set; } //mod
        public ICollection<string> Roles { get; set; }
    }
}
