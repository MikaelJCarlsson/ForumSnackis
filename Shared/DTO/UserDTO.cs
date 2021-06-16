using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumSnackis.Shared.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public int Posts { get; set; }
        public DateTime JoinDate { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public string UserBio { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("OldPassword", ErrorMessage =
    "The password and confirmation password do not match.")]
        public string CompareOldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
    }
}
