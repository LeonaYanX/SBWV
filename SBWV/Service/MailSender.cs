
using System.Net.Mail;

namespace SBWV.Service
{
    public class MailSender
    {
        internal void SendEmailConfirmation(string email)
        {
            
            // сгенерировать токен для подтверждения почты
            // сохранить токен в базу
            // сформировать ссылку для подтверждения
            // отправить письмо на почту
            
            var mailClient = new SmtpClient();

            MailMessage message = new MailMessage();
            mailClient.SendMailAsync(message);
        }
    }
}
