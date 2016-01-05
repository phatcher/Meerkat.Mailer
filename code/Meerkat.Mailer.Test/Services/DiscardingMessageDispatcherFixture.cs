using Meerkat.Mailer.Services;

using NUnit.Framework;

namespace Meerkat.Mailer.Test.Services
{
    [TestFixture]
    public class DiscardingMessageDispatcherFixture
    {
        [Test]
        public void ReadWriteDeliveryLocation()
        {
            var dispatcher = new DiscardingMessageDispatcher { DeliveryLocation = "Fred" };

            Assert.AreEqual("Fred", dispatcher.DeliveryLocation);
        }

        [Test]
        public void ReadWriteServer()
        {
            var dispatcher = new DiscardingMessageDispatcher { Server = "Fred" };

            Assert.AreEqual("Fred", dispatcher.Server);
        }

        [Test]
        public void DispatchCallsNotify()
        {
            var notifier = new StubMessageNotifier();
            IMessageDispatcher dispatcher = new DiscardingMessageDispatcher(notifier);

            var m = new Message();
            dispatcher.Send(m);

            Assert.AreEqual(m, notifier.Message, "Messages differ");
        }
    }
}