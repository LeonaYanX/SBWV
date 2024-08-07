
using MimeKit;
/*using System.Net.Mail;*/
using MailKit.Net.Smtp;
using MailKit;
using System.Threading;


namespace SBWV.Service
{
    public class MailSender
    {
        internal void SendEmailConfirmation(string email , string token)
        {

            // сгенерировать токен для подтверждения почты
            // сохранить токен в базу
            // сформировать ссылку для подтверждения
            // отправить письмо на почту

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Swap Book", "swapbook@yandex.com"));
            message.To.Add(new MailboxAddress("Дорогой пользователь", email));
            message.Subject = "Подтверждение почты";
            //так как нету сертификата и домена
            var url = $"https://localhost:7263/Account/ComfirmEmail/?email={email}&token={token}";
            message.Body = new TextPart("plain")
            {
                Text = @"Для подтверждения почты перейдите по ссылке: " + Environment.NewLine  + url+ Environment.NewLine+ "С уважением, SwapBook" 
                + Environment.NewLine + "При проблемах обращайтесь по адресу: swapbook@yandex.com и если это не вы, то обратитесь к администратору."
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.yandex.com", 465, true);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("swapbook@yandex.com", "yowcxykyqfzmmzrv");

                var x = client.Send(message); // MailKit.Net.Smtp.SmtpCommandException: '5.7.1 Message rejected under suspicion of SPAM;
                                              // https://ya.cc/1IrBc 1722791520-xBeMHoEXta60-1LmsZq1g' так как нет подписи и настроек связанных
                                              // с Доменом 
                client.Disconnect(true);
            }
        }
    }
}
