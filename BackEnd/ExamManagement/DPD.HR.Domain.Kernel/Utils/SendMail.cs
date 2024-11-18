using MimeKit;
using MailKit.Net.Smtp;

namespace DPD.HR.Kernel.Utils;

public static class SendMail
{
    #region FUNCTION
    //FUNCTION SEND MAIL AUTOMATION COMMON
    public static void SendMailAuto(string fromMail, string toMail, string password, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("QuanLyGacThi", fromMail));
        message.To.Add(new MailboxAddress("QLGC", toMail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };

        message.Body = bodyBuilder.ToMessageBody();

        // send the email
        using var client = new SmtpClient();
        client.Connect("smtp.gmail.com", 587, false);
        client.Authenticate(fromMail, password);
        client.Send(message);
        client.Disconnect(true);
    }
    #endregion
}