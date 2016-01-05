using System.Collections.Generic;
using System.Globalization;

using Meerkat.Mailer.Services;

using Moq;

using NUnit.Framework;

namespace Meerkat.Mailer.Test.Services
{
    [TestFixture]
    public class MessageTemplateDispatcherFixture
    {
        [Test]
        public void SendMessageTemplate()
        {
            var american = new CultureInfo("en-US");
            var template = new MessageTemplate { Subject = "Subject {{A}}", Text = "Body {{A}}" };
            var properties = new Dictionary<string, object> { { "A", "B" } };

            // Arrange
            var repository = new Mock<IMessageTemplateRepository>();
            var notifier = new StubMessageNotifier();
            IMessageDispatcher messageDispatcher = new DiscardingMessageDispatcher(notifier);
            var dispatcher = new MessageTemplateDispatcher(repository.Object, messageDispatcher);

            repository.Setup(x => x.Find("Test", american)).Returns(template);

            // Act
            dispatcher.Send("Test", american, properties, new[] { "noreply@test.com" });

            // Assert
            Assert.IsNotNull(notifier.Message);
            Assert.AreEqual("Subject B", notifier.Message.Subject, "Subject differs");
            Assert.AreEqual("Body B", notifier.Message.Text, "Body differs");
            repository.VerifyAll();
        }
    }
}