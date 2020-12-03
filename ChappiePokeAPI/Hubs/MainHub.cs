using ChappiePokeAPI.DataAccess;
using EntityModels.EntityModels;
using ChappiePokeAPI.Extensions;
using HelperMethods.Helper;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models.DTOResponseModels;
using Models.Enums;
using Models.DTOModels;
using Models.DTOExtensions;
using Microsoft.EntityFrameworkCore;
using HelperVariables.Globals;

namespace ChappiePokeAPI.Hubs
{
    [Microsoft.AspNetCore.Cors.EnableCors("MyPolicy")]
    public class MainHub : Hub
    {
        private PokeDBContext PokeDB { get; set; }
        public MainHub(PokeDBContext dbContext)
        {
            PokeDB = dbContext;
        }


        // ******** UserLogin ********
        #region Userlogin

        public async Task Register(RegisterRequest registerRequest)
        {
            RegisterResponse response = new RegisterResponse();
            try
            {
                if (!PokeDB.Users.Any(x => x.Username.ToLower() == registerRequest.Username.ToLower() || x.Email.ToLower() == registerRequest.Email.ToLower()))
                {
                    User newUser = await PokeDB.AddUser(registerRequest);
                    response.SessionKey = newUser.SessionKey;
                    response.PasswordSalt = newUser.PasswordSalt;
                }
                else if (PokeDB.Users.Any(x => x.Username.ToLower() == registerRequest.Username.ToLower()))
                {
                    response.RegisterError = RegisterError.UsernameTaken;
                }
                else if (PokeDB.Users.Any(x => x.Email.ToLower() == registerRequest.Email.ToLower()))
                {
                    response.RegisterError = RegisterError.EmailTaken;
                }
            }
            catch
            {
                response.RegisterError = RegisterError.OtherError;
            }
            await Clients.Caller.SendAsync("RegisterRequest", response);
        }

        public async Task ConfirmPassword(LoginRequest request)
        {
            PasswordConfirmationError error = PasswordConfirmationError.NoError;
            User user = await PokeDB.Users.FirstOrDefaultAsync(
                x => ((x.Username.ToLower() == request.UsernameOrMail.ToLower() || x.Email.ToLower() == request.UsernameOrMail.ToLower()) && x.Password == request.Password + "$" + x.PasswordSalt)); //
            if (user == null)
            {
                error = PasswordConfirmationError.WrongPassword;
                await Clients.Caller.SendAsync("ConfirmPasswordRequest", error);
            }
            else
            {
                LoginResponse response = new LoginResponse(user);
                if (response.LoginError == LoginError.NotVerified)
                {
                    await Clients.Caller.SendAsync("LoginRequest", response);
                    return;
                }
                await Clients.Caller.SendAsync("ConfirmPasswordRequest", error);
            }
        }

        public async Task Login(LoginRequest loginRequest)
        {
            User user = await PokeDB.Users.FirstOrDefaultAsync(
                x => ((x.Username.ToLower() == loginRequest.UsernameOrMail.ToLower() || x.Email.ToLower() == loginRequest.UsernameOrMail.ToLower()) && x.Password == loginRequest.Password + "$" + x.PasswordSalt) //
                    || (loginRequest.SessionKey.Length > 0 && x.SessionKey == loginRequest.SessionKey));
            if (user != null)
                await PokeDB.Login(user);

            LoginResponse response = new LoginResponse(user);
            await Clients.Caller.SendAsync("LoginRequest", response);
        }

        public async Task Logout(GenericRequest request)
        {
            User user = await PokeDB.Logout(request.SessionKey);
            if (user != null)
                await Clients.Caller.SendAsync("LogoutRequest", true);
            else
                await Clients.Caller.SendAsync("LogoutRequest", false);
        }

        public async Task Verify(VerificationRequest verificationRequest)
        {
            VerificationError response = VerificationError.NoError;
            try
            {
                User user = await PokeDB.Users.FirstOrDefaultAsync(x => x.SessionKey == verificationRequest.SessionKey);
                if (user != null)
                {
                    bool verified = await PokeDB.VerifyUser(user, verificationRequest.VerificationCode);
                    if (!verified)
                        response = VerificationError.WrongToken;
                }
                else
                    response = VerificationError.WrongSessionKey;
            }
            catch
            {
                response = VerificationError.OtherError;
            }
            await Clients.Caller.SendAsync("VerifyRequest", response);

        }

        #endregion

        // ******** GET ********

        #region GetRequests

        public async Task GetPrivileges(GenericRequest request)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, false);
            await Clients.Caller.SendAsync("UserPrivilegeRequest", new UserPrivilegeResponse(user));
        }

        public async Task GetUser(GenericRequest request)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, false);
            if (user != null)
                await Clients.Caller.SendAsync("UserRequest", user);
        }

        public async Task GetOrders(GenericRequest request)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, true);
            if (user != null)
                await Clients.Caller.SendAsync("OrderRequest", user.Orders);
            else
                await Clients.Caller.SendAsync("OrderRequest");
        }

        public async Task GetCustomer(GenericRequest request)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, false);
            if (user != null)
            {
                List<Customer> customers = user.GetCustomers(PokeDB.Customers, PokeDB.Addresses);
                await Clients.Caller.SendAsync("CustomerRequest", customers);
            }
        }

        public async Task GetShop()
        {
            List<Product> products = PokeDB.GetProducts();
            await Clients.Caller.SendAsync("ShopRequest", products);
        }

        public async Task GetPulls()
        {
            List<Pull> pulls = PokeDB.Pulls.ToList();
            await Clients.Caller.SendAsync("PullRequest", pulls);
        }

        public async Task GetShopAdministration(GenericRequest request)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, false);
            if (user != null && user.UserPrivileges == UserPrivileges.Administrator)
            {
                List<Product> products = PokeDB.GetProducts();
                await Clients.Caller.SendAsync("ShopAdminRequest", products);
            }
        }

        public async Task GetUserAdministration(GenericRequest request)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, false);
            if (user != null && user.UserPrivileges == UserPrivileges.Administrator)
            {
                List<User> users = new List<User>(PokeDB.Users);
                users.ForEach(x => { x.SessionKey = ""; x.PasswordSalt = ""; });
                await Clients.Caller.SendAsync("UserAdminRequest", users);
            }
        }

        #endregion

        // ******** UPDATE ********

        #region UpdateRequests

        public async Task UpdateUser(User requestUser)
        {
            User user = await PokeDB.Users.FirstOrDefaultAsync(x => x.SessionKey == requestUser.SessionKey);
            if (user != null)
            {
                LoginResponse response = new LoginResponse(user);
                if (response.LoginError == LoginError.NotVerified)
                {
                    await Clients.Caller.SendAsync("LoginRequest", response);
                    return;
                }
                user.Update(requestUser, PokeDB.Users);
                await PokeDB.SaveChangesAsync();
                User returnUser = PokeDB.GetUserCopy(requestUser.SessionKey, false);
                await Clients.Caller.SendAsync("UserRequest", returnUser);
            }
            else
                await Clients.Caller.SendAsync("LogoutRequest", false);

        }

        public async Task SetUserPrivilege(GenericRequest request, User requestUser)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, false);
            if (requestUser != null && user != null && user.UserPrivileges == UserPrivileges.Administrator)
            {
                User requestUserCopy = await PokeDB.Users.FirstOrDefaultAsync(x => x.UserID == requestUser.UserID);
                if (requestUserCopy != null && user.UserPrivileges > requestUserCopy.UserPrivileges)
                {
                    requestUserCopy.UserPrivileges = requestUser.UserPrivileges;
                    await PokeDB.SaveChangesAsync();
                    List<User> users = new List<User>(PokeDB.Users);
                    users.ForEach(x => { x.SessionKey = ""; x.PasswordSalt = ""; x.Password = ""; });
                    await Clients.Caller.SendAsync("UserAdminRequest", users);
                } else
                {
                    await Clients.Caller.SendAsync("UserChangeRequest", new UserChangeResponse(GenericError.Error));
                }
            }
            else
                await Clients.Caller.SendAsync("UserChangeRequest", new UserChangeResponse(GenericError.Error));
        }

        public async Task UpdateCustomer(GenericRequest request, Customer customer)
        {
            List<Customer> customers = await PokeDB.UpdateCustomer(customer, request);
            if (customers == null)
                await Clients.Caller.SendAsync("CustomerError", true);
            else
                await Clients.Caller.SendAsync("CustomerRequest", customers);

        }

        public async Task SetPassword(PasswordRequest request)
        {
            User user = await PokeDB.Users.FirstOrDefaultAsync(x => x.SessionKey == request.SessionKey);
            if (user != null)
            {
                user.Password = request.Password;
                await PokeDB.SaveChangesAsync();
            }
        }

        #endregion

        // ******** ADD ********

        #region AddRequests
        public async Task AddCustomer(GenericRequest request, Customer customer)
        {
            List<Customer> customers = await PokeDB.AddCustomer(customer, request);
            if (customers == null)
                await Clients.Caller.SendAsync("CustomerError", true);
            else
                await Clients.Caller.SendAsync("CustomerRequest", customers);

        }

        public async Task AddProduct(GenericRequest request, Product product)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, false);
            if (user != null && user.UserPrivileges == UserPrivileges.Administrator)
            {
                int ProductID = 0;
                ProductAddResponse response = new ProductAddResponse();
                if (product.ProductID > 0)
                {
                    ProductID = (await PokeDB.UpdateProduct(product)).ProductID;
                }
                else
                {
                    PokeDB.Products.Add(product);
                    await PokeDB.SaveChangesAsync();
                    ProductID = product.ProductID;
                }
                response.Success = true;
                if (ProductID != 0)
                    response.ProductID = ProductID;
                await Clients.Caller.SendAsync("AddProductSuccess", response);
                await GetShopAdministration(request);
            }
        }

        #endregion


        // ******** DELETE ********

        public async Task DeleteProduct(GenericRequest request, Product product)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, false);
            if (user != null && user.UserPrivileges == UserPrivileges.Administrator)
            {
                if (product.ProductID > 0)
                {
                    var prod = await PokeDB.Products.FirstAsync(x => x.ProductID == product.ProductID);
                    var images = new List<ImageGroup>(prod.ImageGroups);
                    PokeDB.Products.Remove(prod);
                    await PokeDB.SaveChangesAsync();
                    foreach (var img in images)
                    {
                        var imgPath = System.IO.Path.Combine(Paths.AssetUploadPath, img.ImagePath);
                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);

                    }
                    await Clients.Caller.SendAsync("AddProductSuccess", true);
                }
            }
        }

        public async Task DeleteUser(GenericRequest request, User requestUser)
        {
            User user = PokeDB.GetUserCopy(request.SessionKey, false);
            if (user != null && user.UserPrivileges == UserPrivileges.Administrator)
            {
                if (user.UserID == requestUser.UserID)
                {
                    await Clients.Caller.SendAsync("UserChangeRequest", new UserChangeResponse(GenericError.Error));
                    return;
                }
                User requestUserCopy = await PokeDB.Users.FirstAsync(x => x.UserID == requestUser.UserID);
                if (requestUserCopy != null && user.UserPrivileges > requestUserCopy.UserPrivileges)
                {
                    PokeDB.Users.Remove(requestUserCopy);
                    await PokeDB.SaveChangesAsync();
                    List<User> users = new List<User>(PokeDB.Users);
                    users.ForEach(x => { x.SessionKey = ""; x.PasswordSalt = ""; x.Password = ""; });
                    await Clients.Caller.SendAsync("UserAdminRequest", users);
                }
                else
                {
                    await Clients.Caller.SendAsync("UserChangeRequest", new UserChangeResponse(GenericError.Error));
                }
            }
        }
    }
}
