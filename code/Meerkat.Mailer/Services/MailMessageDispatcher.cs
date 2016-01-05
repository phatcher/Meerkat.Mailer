using System;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;

using Common.Logging;

using Meerkat.Mailer.Attachments;

namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Uses the <see cref="SmtpClient" /> to send messages.
    /// </summary>
    public class MailMessageDispatcher : MessageDispatcher
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMailAttachmentFactory attachmentFactory;

        /// <summary>
        /// Initializes a new instance of the MailMessageDispatcher class.
        /// </summary>
        /// <param name="notifier"></param>
        /// <param name="attachmentFactory"></param>
        public MailMessageDispatcher(IMailAttachmentFactory attachmentFactory, IMessageDispatchNotifier notifier)
            : base(notifier)
        {
            this.attachmentFactory = attachmentFactory;
        }

        private void AddAttachment(MailMessage mail, IAttachment attachment)
        {
            if (attachmentFactory == null)
            {
                Logger.Warn("Unable to add attachments no factory was supplied");
                return;
            }

            var a = attachmentFactory.CreateAttachement(attachment);
            if (a != null)
            {
                mail.Attachments.Add(a);
            }
        }

        protected override bool Dispatch(IMessage message)
        {
            SmtpClient mailer = null;
            MailMessage mail = null;
            try
            {
                mailer = ConfigureMailClient();
                mail = new MailMessage();

                if (!string.IsNullOrWhiteSpace(message.From))
                {
                    mail.From = new MailAddress(message.From, string.IsNullOrWhiteSpace(message.FromName) ? string.Empty : message.FromName);
                }

                message.ToAddress.ForEach(s => mail.To.Add(new MailAddress(s)));
                message.Cc.ForEach(s => mail.CC.Add(new MailAddress(s)));
                message.Bcc.ForEach(s => mail.Bcc.Add(new MailAddress(s)));
                if (!string.IsNullOrWhiteSpace(message.ReplyTo))
                {
                    mail.ReplyToList.Add(new MailAddress(message.ReplyTo));
                }
                mail.Subject = message.Subject;

                if (!string.IsNullOrEmpty(message.Text))
                {
                    var plainView = AlternateView.CreateAlternateViewFromString(message.Text, null, MediaTypeNames.Text.Plain);
                    mail.AlternateViews.Add(plainView);
                }

                if (!string.IsNullOrEmpty(message.Html))
                {
                    var htmlView = AlternateView.CreateAlternateViewFromString(message.Html, null, MediaTypeNames.Text.Html);
                    mail.AlternateViews.Add(htmlView);

                    // TODO: Do we want to add a "Sorry, this email is formatted with HTML" text
                }

                message.Attachments.ForEach(att => AddAttachment(mail, att));

                // Custom headers
                foreach (var pair in message.Headers)
                {
                    mail.Headers.Add(pair.Key, pair.Value);
                }

                // Do not add header if no bounceAddress
                if (!string.IsNullOrWhiteSpace(message.BounceAddress))
                {
                    mail.Headers.Add("Return-Path", message.BounceAddress);
                }

                // Ok, send it
                TrySend(mail, mailer);
            }
            finally
            {
                // Get rid of the attachments and mailer explicitly - ensure we've sent
                if (mailer != null)
                {
                    mailer.Dispose();
                }

                if (mail != null)
                {
                    foreach (var attachment in mail.Attachments)
                    {
                        attachment.Dispose();
                    }
                    mail.Dispose();
                }
            }

            return true;
        }

        private static void TrySend(MailMessage mail, SmtpClient mailer)
        {
            var tries = 0;
            const int MaxTries = 3;
            Exception lastException;
            do
            {
                tries++;
                try
                {
                    mailer.Send(mail);
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    Logger.ErrorFormat("Failed sending message. [{0}] - {1}", mail.Subject, ex.Message);
                    throw;
                }
                catch (SmtpFailedRecipientsException failedRecipientsException)
                {
                    lastException = failedRecipientsException;

                    var failedAddresses = failedRecipientsException.InnerExceptions.Select(x => x.FailedRecipient).ToArray();
                    Logger.InfoFormat(
                        "Failed emailing {0} to {1}. Attempt {2}/{3}",
                        mail.Subject,
                        string.Join(",", failedAddresses),
                        tries,
                        MaxTries);

                    mail.To.Clear();
                    failedAddresses.ToList().ForEach(x => mail.To.Add(x));
                }
                catch (SmtpException smtpException)
                {
                    lastException = smtpException;
                    Logger.ErrorFormat("Failed sending message {0} - {1}. Attempt {2}/{3}", mail.Subject, smtpException.Message, tries, MaxTries);
                }
            }
            while (tries < MaxTries);

            throw lastException;
        }

        private SmtpClient ConfigureMailClient()
        {
            // Get a client configured by app.config (System.Net.MailSettings)
            var client = new SmtpClient();

            // Override if supplied
            if (!string.IsNullOrEmpty(Server))
            {
                // Use server delivery if we have it
                client.Host = Server;
            }
            else if (!string.IsNullOrEmpty(DeliveryLocation))
            {
                // Or an explicit delivery location
                client.PickupDirectoryLocation = DeliveryLocation;
                client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            }

            if (string.IsNullOrWhiteSpace(client.Host) && string.IsNullOrWhiteSpace(client.PickupDirectoryLocation))
            {
                throw new NotSupportedException("Must specify either SMTP Server or delivery location");
            }

            return client;
        }
    }
}