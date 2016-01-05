namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Discards the messages coming in, useful for test purposes
    /// </summary>
    public class DiscardingMessageDispatcher : MessageDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the DiscardingMessageDispatcher class.
        /// </summary>
        public DiscardingMessageDispatcher() : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DiscardingMessageDispatcher class.
        /// </summary>
        /// <param name="notifier"></param>
        public DiscardingMessageDispatcher(IMessageDispatchNotifier notifier) : base(notifier)
        {
        }

        /// <summary>
        /// Dispatch the message, this implementation discards all messages
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override bool Dispatch(IMessage message)
        {
            // Do nothing, and return true
            return true;
        }
    }
}