using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using Common.Logging;

namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Sends a message based on a template
    /// </summary>
    public class MessageTemplateDispatcher : IMessageTemplateDispatcher
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMessageTemplateRepository repository;
        private readonly IMessageDispatcher dispatcher;

        /// <summary>
        /// Create a new instance of the <see cref="MessageTemplateDispatcher"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="dispatcher"></param>
        public MessageTemplateDispatcher(IMessageTemplateRepository repository, IMessageDispatcher dispatcher)
        {
            this.repository = repository;
            this.dispatcher = dispatcher;
        }

        /// <copydoc cref="IMessageTemplateDispatcher.Send" />
        public void Send(string templateName, CultureInfo cultureInfo, IDictionary<string, object> properties, IEnumerable<string> toAddress, IEnumerable<string> ccAddress = null, IEnumerable<string> bccAddress = null)
        {
            var template = repository.Find(templateName, cultureInfo);
            if (template == null)
            {
                Logger.ErrorFormat("{0}: Cannot send, template not found for culture '{1}'", templateName, cultureInfo);
                return;
            }

            var message = template.Merge(properties, toAddress, ccAddress, bccAddress);
            OnSend(message);

            dispatcher.Send(message);
        }

        /// <summary>
        /// Perform any customizations to the message before sending, e.g. adding headers
        /// </summary>
        /// <param name="message"></param>
        protected virtual void OnSend(IMessage message)
        {
            // TODO: Should be set up by the caller
            ////mail.Headers.Add("X-Client", message.Client);
            ////mail.Headers.Add("X-Transmission", message.TransmissionId.ToString());
            message.Headers["X-UserEmail"] = message.ToAddress.First();
        }
    }
}