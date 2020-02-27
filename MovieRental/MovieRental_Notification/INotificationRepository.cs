using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieRental_Notification
{
    public interface INotificationRepository
    {
        Task<dynamic> SendEmailConfirmingAccount(string emailTo, string subject, string baseUrl, string content = null);
        Task<dynamic> SendEmailRecoveryPassword(string emailTo, string subject, string newPassword);
    }
}
