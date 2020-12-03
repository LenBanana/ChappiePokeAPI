using ChappiePokeAPI.DataAccess;
using EntityModels.EntityModels;
using HelperMethods.Helper;
using HelperVariables.Globals;
using Models.DTOModels;
using Models.DTOResponseModels;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChappiePokeAPI.Extensions
{
    public static class PokeDBUserExtension
    {
        public static async Task<User> AddUser(this PokeDBContext context, RegisterRequest registerRequest)
        {
            try
            {
                RegisterCode registerCode = new RegisterCode();
                await context.RegisterCodes.AddAsync(registerCode);
                await context.SaveChangesAsync();
                User user = new User();
                user.Username = registerRequest.Username;
                user.Email = registerRequest.Email;
                user.Avatar = registerRequest.Avatar;
                user.Name = registerRequest.Firstname;
                user.Lastname = registerRequest.Lastname;
                user.DateCreated = DateTime.Now;
                user.RegisterCodeID = registerCode.RegisterCodeID;
                user.SessionKey = Cryptographie.GenerateUniqueToken();
                user.PasswordSalt = Cryptographie.GenerateUniqueToken();
                user.Password = registerRequest.Password + "$" + user.PasswordSalt;
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                await user.SendRegisterMail();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static async Task<List<Customer>> AddCustomer(this PokeDBContext context, Customer customer, GenericRequest request)
        {
            try
            {
                User user = context.Users.FirstOrDefault(x => x.SessionKey == request.SessionKey);
                if (user != null)
                {
                    user.Customers = context.Customers.Where(x => x.UserID == user.UserID).ToList();
                    if (!user.Customers.Any(x => x.Email == customer.Email || (x.BillingAddress.AddressLine1 == customer.BillingAddress.AddressLine1 && x.BillingAddress.AddressLine2 == customer.BillingAddress.AddressLine2)))
                    {
                        customer.ShippingAddress = customer.BillingAddress;
                        user.Customers.Add(customer);
                        await context.SaveChangesAsync();
                        List<Customer> result = new List<Customer>(user.Customers);
                        result.ForEach(x => { x.User = new User(); x.BillingAddress = context.Addresses.First(y => y.AddressID == x.BillingAddressAddressID); });
                        return result;
                    }
                    else
                        return null;
                } else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static async Task<List<Customer>> UpdateCustomer(this PokeDBContext context, Customer customer, GenericRequest request)
        {
            try
            {
                User user = context.Users.FirstOrDefault(x => x.SessionKey == request.SessionKey);
                if (user != null)
                {
                    user.Customers = context.Customers.Where(x => x.UserID == user.UserID).ToList();
                    int idx = user.Customers.ToList().FindIndex(x => x.CustomerID == customer.CustomerID);
                    if (idx != -1)
                    {
                        user.Customers.ElementAt(idx).BillingAddress = context.Addresses.First(x => x.AddressID == user.Customers.ElementAt(idx).BillingAddressAddressID);
                        user.Customers.ElementAt(idx).ShippingAddress = context.Addresses.First(x => x.AddressID == user.Customers.ElementAt(idx).ShippingAddressAddressID);
                        user.Customers.ElementAt(idx).Name = customer.Name;
                        user.Customers.ElementAt(idx).Lastname = customer.Lastname;
                        if (!user.Customers.Any(x => x.CustomerID != customer.CustomerID && x.Email == customer.Email))
                            user.Customers.ElementAt(idx).Email = customer.Email;
                        user.Customers.ElementAt(idx).CompanyName = customer.CompanyName;
                        user.Customers.ElementAt(idx).ContactPerson = customer.ContactPerson;
                        if (!user.Customers.Any(x => x.CustomerID != customer.CustomerID && (x.BillingAddress.AddressLine1 == customer.BillingAddress.AddressLine1 && x.BillingAddress.AddressLine2 == customer.BillingAddress.AddressLine2)))
                        {
                            user.Customers.ElementAt(idx).BillingAddress.AddressLine1 = customer.BillingAddress.AddressLine1;
                            user.Customers.ElementAt(idx).BillingAddress.AddressLine2 = customer.BillingAddress.AddressLine2;
                        }
                        user.Customers.ElementAt(idx).BillingAddress.Country = customer.BillingAddress.Country;
                        user.Customers.ElementAt(idx).BillingAddress.State = customer.BillingAddress.State;
                        user.Customers.ElementAt(idx).BillingAddress.City = customer.BillingAddress.City;
                        user.Customers.ElementAt(idx).BillingAddress.PostalCode = customer.BillingAddress.PostalCode;
                        await context.SaveChangesAsync();
                        List<Customer> result = new List<Customer>(user.Customers);
                        result.ForEach(x => { x.User = new User(); x.BillingAddress = context.Addresses.First(y => y.AddressID == x.BillingAddressAddressID); });
                        return result;
                    }
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static async Task<bool> Login(this PokeDBContext context, User user)
        {
            try
            {
                user.SessionKey = Cryptographie.GenerateUniqueToken();
                user.LastLogin = DateTime.Now;
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static async Task<User> Logout(this PokeDBContext context, string SessionKey)
        {
            try
            {
                User user = context.Users.FirstOrDefault(x => x.SessionKey == SessionKey);
                if (user != null)
                {
                    user.SessionKey = "";
                    await context.SaveChangesAsync();
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static async Task<bool> VerifyUser(this PokeDBContext context, User user, int VerificationCode)
        {
            if (context.RegisterCodes.First(x => x.RegisterCodeID == user.RegisterCodeID).Code == VerificationCode)
            {
                user.UserPrivileges = UserPrivileges.Verified;
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static User GetUserCopy(this PokeDBContext context, string SessionKey, bool GetOrders)
        {
            User dbUser = context.Users.FirstOrDefault(x => x.SessionKey == SessionKey);
            if (dbUser != null)
            {
                User user = dbUser.ShallowCopy();
                user.Password = "";
                user.PasswordSalt = "";
                user.SessionKey = "";
                if (GetOrders)
                {
                    //user.Orders = context.Orders.Where(x => x.UserID == user.UserID).ToList();
                    //user.Orders.ForEach(x => x.SaleOrderProducts = context.SaleOrderProducts.Where(y => y.OrderID == x.OrderID).ToList());
                    //user.Orders.ForEach(x => x.SaleOrderProducts.ForEach(y => y.Product = context.Products.First(z => z.ProductID == y.ProductID)));
                }
                return user;
            } else
                return dbUser;
        }
    }
}
