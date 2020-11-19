using EntityModels.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOExtensions
{
    public static class UserExtension
    {
        public static User Update(this User user, User requestUser, IEnumerable<User> Users)
        {
            if (requestUser.Username.Length >= 3 && Users.Any(x => x.Username.ToLower() == requestUser.Username.ToLower()))
            {
                user.Username = requestUser.Username;
            }
            if (requestUser.Password.Length >= 8)
            {
                user.Password = requestUser.Password + "$" + user.PasswordSalt;
            }
            if (requestUser.Email.Length >= 4 && requestUser.Email.Contains('@') && Users.Any(x => x.Email.ToLower() == requestUser.Email.ToLower()))
            {
                user.Email = requestUser.Email;
            }
            if (requestUser.Name.Length >= 2)
            {
                user.Name = requestUser.Name;
            }
            if (requestUser.Lastname.Length >= 2)
            {
                user.Lastname = requestUser.Lastname;
            }
            return user;
        }

        public static List<Customer> GetCustomers(this User user, IEnumerable<Customer> customers, IEnumerable<Address> addresses)
        {
            List<Customer> result = customers.Where(x => x.UserID == user.UserID).ToList();
            result.ForEach(x => { x.BillingAddress = addresses.First(y => y.AddressID == x.BillingAddressAddressID); x.ShippingAddress = addresses.First(y => y.AddressID == x.ShippingAddressAddressID ); });
            return result;
        }

    }
}
