using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using SendGrid;

using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;

namespace MovieRental_Notification.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        public readonly IConfiguration _config;
        private string _templateEmailConfirmingAccount { get; set; }
        private string _templateEmailRecoveryPassword { get; set; }
        public NotificationRepository(IConfiguration config)
        {
            _config = config;

            #region Template confirming account
            _templateEmailConfirmingAccount = "<h1>Welcome to Movie Rental!</h1>";
            _templateEmailConfirmingAccount += "<p>Please confirm your account with the next link:<br/> <a href='@baseUrl'>Click here!<a></p>";
            #endregion

            #region Template recovery password
            _templateEmailRecoveryPassword = "<h1>Recovery your account!</h1>";
            _templateEmailRecoveryPassword += "<p>Dear @Username, your password has been changed to <br/><b>@NewPassword</b></p>";
            #endregion
        }
        public async Task<dynamic> SendEmailConfirmingAccount(string emailTo, string emailSubject, string baseUrl, string content = null)
        {
            var apiKey = _config.GetSection("SendGrid:SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("admin@movierental.dev", "Movie Rental Admin");
            var to = new EmailAddress(emailTo);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject,null, content ?? _templateEmailConfirmingAccount.Replace("@baseUrl",baseUrl));
            var response = await client.SendEmailAsync(msg);

            return response;
        }

        public async Task<dynamic> SendEmailRecoveryPassword(string emailTo, string emailSubject, string newPassword)
        {
            var apiKey = _config.GetSection("SendGrid:SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("admin@movierental.dev", "Movie Rental Admin");
            var to = new EmailAddress(emailTo);

            var msg = MailHelper.CreateSingleEmail(from, to, emailSubject, null,  _templateEmailRecoveryPassword.Replace("@Username", emailTo).Replace("@NewPassword", newPassword));
            var response = await client.SendEmailAsync(msg);

            return response;
        }
    }
}
