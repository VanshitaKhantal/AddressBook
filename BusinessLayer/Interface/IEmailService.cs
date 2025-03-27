using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    /// <summary>
    /// Interface for email-related services.
    /// Provides a method to send emails with a specified recipient, subject, and body.
    /// </summary>
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
}
