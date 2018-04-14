using SendGrid.Helpers.Mail;

namespace Meerkat.Mailer.SendGrid
{
    public static class MessageExtensions
    {
        public static SendGridMessage ToSendGridMessage(this IMessage message)
        {
            var sgMessage = new SendGridMessage
            {
                From = new EmailAddress(message.From.PrettyEmail(message.FromName)),
                Subject = message.Subject,
            };

            foreach (var address in message.ToAddress)
            {
                sgMessage.AddTo(address);
            }

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
                sgMessage.HtmlContent = message.Html;
            }

            if (!string.IsNullOrEmpty(message.Text))
            {
                sgMessage.PlainTextContent = message.Text;
            }

            return sgMessage;
        }
    }
}