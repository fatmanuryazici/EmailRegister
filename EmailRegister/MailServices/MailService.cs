
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EmailRegister.MailServices
{
    public class MailService : IMailService

    {
        public async Task SendMailAsync(string email, string subject, string message)
        {
            try
            {   //MimeKit,MailKit register olabilmek için gerekli paketler.
                var newEmail = new MimeMessage();
                newEmail.From.Add(MailboxAddress.Parse("fatmanur@gmail.com")); //hangi mail adresini kullanacagımı gösteriyorum
                newEmail.To.Add(MailboxAddress.Parse(email));
                newEmail.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = message; //mail kutucugundaki mesajı BodyBuilder ile HtmlBody e entegre ettik
                newEmail.Body = builder.ToMessageBody(); //html dilinden normal görünüme geçti

                var smtp = new SmtpClient(); //smtp mail gönderme sunucusu
                await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("fatmanur@gmail.com", "soubzozkdbkgyxpb");
                await smtp.SendAsync(newEmail);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"E-posta gonderilirken bir hata oluştur :" + ex.Message);
            }
        }


    }
}
