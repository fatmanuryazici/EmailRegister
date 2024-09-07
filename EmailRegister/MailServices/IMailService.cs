namespace EmailRegister.MailServices
{
    public interface IMailService
    {
        Task SendMailAsync(string email, string subject, string message);
    }
}
