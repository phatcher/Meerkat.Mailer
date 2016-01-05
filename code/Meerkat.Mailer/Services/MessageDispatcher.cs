namespace Meerkat.Mailer.Services
{
    using System;

    /// <summary>
    /// Base implementation of the message dispatcher, handles notification of result leaving just the dispatch mechanism to worry about
    /// </summary>
    public abstract class MessageDispatcher : IMessageDispatcher
    {
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

        /// <summary>
        /// Gets or sets the delivery location.
        /// <para>
        /// Set a directory here if the message should be written to a directory.
        /// </para>
        /// </summary>
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

        /// <summary>
        /// Gets or sets the server property.
        /// <para>
        /// Set a valid SMTP server here if the message should be sent directly.
        /// </para>
        /// </summary>
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

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="message">The message to send</param>
        public void Send(IMessage message)
        {
            try
            {
                var ok = Dispatch(message);
                Notify(message, ok);
            }
            catch (Exception ex)
            {
                // TODO: Should we log the exception here?
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
            if (notifier != null)
            {
                notifier.Notify(message, ok, exception);
            }
        }

        /// <summary>
        /// Actions to perform if the mailing options change.
        /// </summary>
        protected virtual void OnMailOptionsChanged()
        {
        }
    }
}