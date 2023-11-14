using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MLunarCoffee.Models
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string PasswordEnCrypt { get; set; }
        public string IP { get; set; }
        public string TokenFCM { get; set; }
        public string From { get; set; }
    }
    public class UserModelType
    {
        public string UserName { get; set; }
        public string PasswordEnCrypt { get; set; }
        public string IP { get; set; }
    }
    public class AuthorResultModel
    {
        public string RESULT { get; set; }
        public string ? ErrTime { get; set; }
        public DateTime ? BlockTime { get; set; }
        [Required]
        public string Session { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordEnCrypt { get; set; }

        public int ID { get; set; }
        public string Lang { get; set; }
        public int Minify { get; set; }
    }
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
        public string Role { get; set; }
    }
    public class UserGroup
    {
        public int UserID { get; set; }
        public int GroupID { get; set; }
    }
    public class UserLogout
    {
        public string? Islogout { get; set; }
    }

    public class AuthAdmin
    {
        public string RESULT { get; set; }
        public string? ErrTime { get; set; }
        public DateTime? BlockTime { get; set; }

    }
}
