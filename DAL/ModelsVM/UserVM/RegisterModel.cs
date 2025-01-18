using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ModelsVM.UserVM
{

        public class RegisterModel
        {
            [StringLength(20)]
            [Required]
            public string FirstName { get; set; }
            [Required]
            [StringLength(20)]
            public string LastName { get; set; }
            [DataType(DataType.PhoneNumber)]
            [Required]
            public string Phone { get; set; }
            

            [StringLength(50)]
            [Required]
            public string Username { get; set; }

            [StringLength(255)]
            [Required]
            public string Email { get; set; }

            [StringLength(20)]
            [DataType(DataType.Password)]
            [Required]
             public string Password { get; set; }

        [Required]
        public TypeUser TypeUser { get; set; }
    }
        public enum TypeUser
        {
            
            [EnumMember(Value = "Admin")]
            Admin,
            [EnumMember(Value = "User")]
            User,
        }
    }
