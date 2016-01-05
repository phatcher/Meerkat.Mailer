using System.Net.Mail;

using SendGrid;

namespace Meerkat.Mailer.SendGrid
{
    public static class MessageExtensions
    {
        public static SendGridMessage ToSendGridMessage(this IMessage message)
        {
            var sgMessage = new SendGridMessage
            {
                From = new MailAddress(message.From.PrettyEmail(message.FromName)),
                Subject = message.Subject,
            };

            sgMessage.AddTo(message.ToAddress);
            foreach (var address in message.Cc)
            {
                sgMessage.AddCc(address);
            }
            foreach (var address in message.Bcc)
            {
                sgMessage.AddBcc(address);
            }

            if (!string.IsNullOrEmpty(message.Html))
            {
                sgMessage.Html = message.Html;
            }

            if (!string.IsNullOrEmpty(message.Text))
            {
                sgMessage.Text = message.Text;
            }

            return sgMessage;
        }
    }
}