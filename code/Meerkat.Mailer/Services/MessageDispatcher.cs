using System;
using System.Reflection;

using Meerkat.Logging;

namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Base implementation of the message dispatcher, handles notification of result leaving just the actual dispatch mechanism to worry about
    /// </summary>
    public abstract class MessageDispatcher : IMessageDispatcher
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMessageDispatchNotifier notifier;
        private string deliveryLocation;
        private string server;

        /// <summary>
        /// Initializes a new instance of the MessageDispatcher class.
        /// </summary>
        /// <param name="notifier"></param>
        protected MessageDispatcher(IMessageDispatchNotifier notifier)
        {
            this.notifier = notifier;
        }

        /// <inheritdoc />
        public string DeliveryLocation
        {
            get
            {
                return deliveryLocation;
            }
            set
            {
                deliveryLocation = value;
                OnMailOptionsChanged();
            }
        }

        /// <inheritdoc />
        public string Server
        {
            get
            {
                return server;
            }
            set
            {
                server = value;
                OnMailOptionsChanged();
            }
        }

        /// <inheritdoc />
        public void Send(IMessage message)
        {
            try
            {
                var ok = Dispatch(message);
                Notify(message, ok);
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Failed sending message {0} - {1}.", message.Subject, ex.Message);
                Notify(message, false, ex);
            }
        }

        /// <summary>
        /// Actually dispatch the message.
        /// </summary>
        /// <param name="message">Message to dispatch</param>
        /// <returns>true if the message was dispatched, otherwise false</returns>
        protected abstract bool Dispatch(IMessage message);

        /// <summary>
        /// Tell someone we send the message
        /// </summary>
        /// <param name="message">The message we sent</param>
        /// <param name="ok"></param>
        /// <param name="exception"></param>
        protected void Notify(IMessage message, bool ok, Exception exception = null)
        {
            notifier?.Notify(message, ok, exception);
        }

        /// <summary>
        /// Actions to perform if the mailing options change.
        /// </summary>
        protected virtual void OnMailOptionsChanged()
        {
        }
    }
}