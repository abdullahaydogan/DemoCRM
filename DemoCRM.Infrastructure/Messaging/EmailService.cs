using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DemoCRM.Infrastructure.Messaging
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendStudentCreatedEmail(string email, string name)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                    _emailSettings.SenderName,
                    _emailSettings.SenderEmail
                ));

            message.To.Add(MailboxAddress.Parse(email));

            message.Subject = "DemoCRM - Student Kaydı";

            message.Body = new TextPart("html")
            {
                Text = $@"
                        <html>
                            <body style='font-family: Arial; background-color:#f5f5f5; padding:20px;'>

                                <div style='max-width:600px; margin:auto; background:white; border-radius:10px; padding:30px;'>

                                    <h2 style='color:#2c3e50;'>🎉 DemoCRM'e Hoşgeldin {name}!</h2>

                                    <p style='font-size:16px; color:#333;'>
                                        DemoCRM sistemine kaydın başarıyla oluşturuldu.
                                    </p>

                                    <p style='font-size:15px; color:#555;'>
                                        Artık kurslara katılabilir ve eğitimlerini yönetebilirsin.
                                    </p>

                                    <div style='margin-top:25px'>
                                        <a href='http://localhost:5000'
                                           style='background:#3498db;
                                                  color:white;
                                                  padding:12px 20px;
                                                  text-decoration:none;
                                                  border-radius:6px;
                                                  font-weight:bold;'>
                                            DemoCRM'e Git
                                        </a>
                                    </div>

                                    <hr style='margin-top:30px'/>

                                    <p style='font-size:12px; color:#999'>
                                        Bu mail DemoCRM sistemi tarafından otomatik gönderilmiştir.
                                    </p>

                                </div>

                            </body>
                        </html>
                        "
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
            Console.WriteLine("EMAIL SERVICE ÇALIŞTI");
        }
    }
}
