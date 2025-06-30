using System.Text;
using System.Security.Cryptography;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace BEBase.Extension
{
    public static class Helper
    {
        public static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public static async Task<bool> SendNotificationEmail(string toEmail, string title, string messageContent)
        {
            var userName = "RideOnSystem";
            var emailFrom = "rideonvie@gmail.com";
            var password = "qzdm ixwd gjij fzzj";

            var subject = $"[Thông báo] {title}";

            var body = $@"
    <html>
        <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;'>
            <div style='max-width: 600px; margin: auto; background-color: #fff; padding: 20px; border-radius: 8px;'>
                <h2 style='color: #333;'>{title}</h2>
                <p style='color: #555; font-size: 14px;'>{messageContent}</p>
                <hr />
                <p style='font-size: 12px; color: #888;'>Email tự động từ hệ thống KoiCare. Vui lòng không trả lời.</p>
            </div>
        </body>
    </html>
    ";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(userName, emailFrom));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailFrom, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendNotificationEmail error: {ex.Message}");
                return false;
            }
        }

    }
}
