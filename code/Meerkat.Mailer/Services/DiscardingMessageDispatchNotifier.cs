namespace Meerkat.Mailer.Services
{
    using System;

    /// <summary>
    /// Discards notifications it receives, useful for testing
    /// </summary>
    public class DiscardingMessageDispatchNotifier : IMessageDispatchNotifier
    {
        public void Notify(IMessage message, bool ok, Exception exception)
        {
        }
    }
}