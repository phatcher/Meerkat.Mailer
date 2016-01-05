namespace Meerkat.Mailer.Services
{
    using System;

    /// <summary>
    /// Listener for message dispatch
    /// </summary>
    public interface IMessageDispatchNotifier
    {
        /// <summary>
        /// Notify that a message was sent and the result of the dispatch
        /// </summary>
        /// <param name="message">Message that was sent</param>
        /// <param name="ok">Whether we sucessfully sent it</param>
        /// <param name="exception">Any exception that was raised during sending</param>
        void Notify(IMessage message, bool ok, Exception exception = null);
    }
}