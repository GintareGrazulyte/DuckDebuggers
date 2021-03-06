﻿using BLL_API;
using BOL;
using System.Net;
using System.Net.Mail;

namespace BLL
{
    public class EmailService : IEmailService
    {
        public void SendEmail(Email email)
        {
            var toAddress = new MailAddress(email.ToAddress, email.ToName);
            var fromAddress = new MailAddress("duckdebuggers@gmail.com", "Duck debuggers");
            const string fromPassword = "Debuggers123@";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

           

            var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = email.Subject,
                Body = email.Body        
            };

            if (!string.IsNullOrEmpty(email.AttachmentPath))
            {
                var attachment = new Attachment(email.AttachmentPath);
                message.Attachments.Add(attachment);
            }
           
            smtp.Send(message);
        }
    }
}
