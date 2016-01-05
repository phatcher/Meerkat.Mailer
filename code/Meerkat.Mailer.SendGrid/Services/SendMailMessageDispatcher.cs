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

            sgMessage.EnableClickTracking(configuration.EnableClickTracking);

            // Send the email
            var transportWeb = new Web(configuration.ApiKey);
            transportWeb.DeliverAsync(sgMessage);

            return true;
        }
    }
}