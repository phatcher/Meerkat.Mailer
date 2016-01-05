using System;

using Meerkat.Mailer.Services;

namespace Meerkat.Mailer.Test.Services
{
    internal class StubMessageNotifier : IMessageDispatchNotifier
    {
        /// <summary>
        /// Gets or sets the Message property.
        /// </summary>
        public IMessage Message { get; set; }

        public Exception Exception { get; set; }
   
        public void Notify(IMessage message, bool ok, Exception exception = null)
        {
            Message = message;
            Exception = exception;
        }
    }
}