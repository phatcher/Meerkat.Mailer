namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Accepts messages for delivery.
    /// </summary>
    public interface IMessageDispatcher
    {
        /// <summary>
        /// Gets or sets the delivery location.
        /// <para>
        /// Set a directory here if the message should be written to a directory.
        /// </para>
        /// </summary>
        string DeliveryLocation { get; set; }

        /// <summary>
        /// Gets or sets the server property.
        /// <para>
        /// Set a valid SMTP server here if the message should be sent directly.
        /// </para>
        /// </summary>
        string Server { get; set; }

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="message">The message to send</param>
        void Send(IMessage message);
    }
}