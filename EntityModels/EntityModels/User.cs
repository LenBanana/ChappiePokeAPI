using HelperMethods.Helper;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityModels.EntityModels
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            UserID = 0;
            LastLogin = DateTime.MinValue;
            UserPrivileges = 0;
            Avatar = "";
            SessionKey = "";
        }
        public int UserID { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Lastname { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Password { get; set; }
        public UserPrivileges UserPrivileges { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLogin { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Avatar { get; set; }
        public string SessionKey { get; set; }
        public string PasswordSalt { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public int RegisterCodeID { get; set; }
        [ForeignKey("RegisterCodeID")]
        public virtual RegisterCode RegisterCode { get; set; }
        public User ShallowCopy()
        {
            return (User)this.MemberwiseClone();
        }

        public async Task<bool> SendRegisterMail()
        {
            string subject = "Chappie Shop - Account Verification for " + Username;
            string path = System.IO.Directory.GetCurrentDirectory() + "/Files/VerificationMail.html";
            string message = System.IO.File.ReadAllText(path);
            message = message.Replace("{{Username}}", Username);
            message = message.Replace("{{VerificationCode}}", RegisterCode.Code.ToString());
            return await EmailSender.SendEmailAsync(Email, subject, message);
        }

        public bool IsValid()
        {
            if (Username.Length > 2 && Password.Length > 0 && Email.Length > 0)
                return true;
            else
                return false;
        }
    }
}
