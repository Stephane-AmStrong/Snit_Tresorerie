using Contracts;
using Entities.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Repository
{
    public class EmailSenderRepository : IEmailSenderRepository
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailSenderRepository(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void Send(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task SendAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }


        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.Name, _emailConfig.EmailId));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder 
            { 
                //HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) 
                HtmlBody = message.Content
            };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    //client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    //client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.EmailId, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    //await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    //await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.EmailId, _emailConfig.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        /*
        public static class SslCertificateValidationExample
        {
            public static void SendMessage(MimeMessage message)
            {
                using (var client = new SmtpClient())
                {
                    // Set our custom SSL certificate validation callback.
                    client.ServerCertificateValidationCallback = MySslCertificateValidationCallback;

                    // Connect
                    smtp.Connect(smtpserver, 587, SecureSocketOptions.StartTls);

                    // Authenticate with our username and password.
                    smtp.Authenticate(loginmail, loginpassword);

                    // Send our message.
                    client.Send(message);

                    // Disconnect cleanly from the server.
                    client.Disconnect(true);
                }
            }

            static bool MySslCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                // If there are no errors, then everything went smoothly.
                if (sslPolicyErrors == SslPolicyErrors.None)
                    return true;

                // Note: MailKit will always pass the host name string as the `sender` argument.
                var host = (string)sender;

                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) != 0)
                {
                    // This means that the remote certificate is unavailable. Notify the user and return false.
                    Console.WriteLine("The SSL certificate was not available for {0}", host);
                    return false;
                }

                if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) != 0)
                {
                    // This means that the server's SSL certificate did not match the host name that we are trying to connect to.
                    var certificate2 = certificate as X509Certificate2;
                    var cn = certificate2 != null ? certificate2.GetNameInfo(X509NameType.SimpleName, false) : certificate.Subject;

                    Console.WriteLine("The Common Name for the SSL certificate did not match {0}. Instead, it was {1}.", host, cn);
                    return false;
                }

                // The only other errors left are chain errors.
                Console.WriteLine("The SSL certificate for the server could not be validated for the following reasons:");

                // The first element's certificate will be the server's SSL certificate (and will match the `certificate` argument)
                // while the last element in the chain will typically either be the Root Certificate Authority's certificate -or- it
                // will be a non-authoritative self-signed certificate that the server admin created. 
                foreach (var element in chain.ChainElements)
                {
                    // Each element in the chain will have its own status list. If the status list is empty, it means that the
                    // certificate itself did not contain any errors.
                    if (element.ChainElementStatus.Length == 0)
                        continue;

                    Console.WriteLine("\u2022 {0}", element.Certificate.Subject);
                    foreach (var error in element.ChainElementStatus)
                    {
                        // `error.StatusInformation` contains a human-readable error string while `error.Status` is the corresponding enum value.
                        Console.WriteLine("\t\u2022 {0}", error.StatusInformation);
                    }
                }

                return false;
            }
        }
        */
    }
}

