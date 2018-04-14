using System.Net;

using Meerkat.Mailer.Services;

using SendGrid;

namespace Meerkat.Mailer.SendGrid.Services
{
    public class SendMailMessageDispatcher : MessageDispatcher
    {
        private readonly SendGridConfiguration configuration;

        public SendMailMessageDispatcher(SendGridConfiguration configuration, IMessageDispatchNotifier notifier)
            : base(notifier)
        {
            this.configuration = configuration;
        }

        protected override bool Dispatch(IMessage message)
        {
            var sgMessage = message.ToSendGridMessage();

            // Send the email
            var client = new SendGridClient(configuration.ApiKey);
            var response = client.SendEmailAsync(sgMessage).Result;

            // TODO: Track failure/bounce etc
            return response.StatusCode == HttpStatusCode.Accepted;
        }
    }
}