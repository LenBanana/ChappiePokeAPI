using HelperMethods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HelperMethods.Helper
{
    public class EmailSender
    {
        public static SmtpSettings _smtpSettings;

        public static async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new System.Net.NetworkCredential(_smtpSettings.Address, _smtpSettings.Password);
                    using (MailMessage mail = new MailMessage(_smtpSettings.Address, email, subject, message))
                    {
                        mail.IsBodyHtml = true;
                        await client.SendMailAsync(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
