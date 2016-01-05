using System.Collections.Generic;
using System.Globalization;

using Meerkat.Mailer.Services;

using Moq;

using NUnit.Framework;

namespace Meerkat.Mailer.Test.Services
{
    [TestFixture]
    public class MessageTemplateMergerFixture
    {
        [Test]
        public void Merge()
        {
            var american = new CultureInfo("en-US");
            var template = new MessageTemplate { Subject = "Subject {{A}}", Text = "Body {{A}}" };
            var properties = new Dictionary<string, object> { { "A", "B" } };

            // Arrange
            var repository = new Mock<IMessageTemplateRepository>();
            var dispatcher = new MessageTemplateMerger(repository.Object);

            repository.Setup(x => x.Find("Test", american)).Returns(template);

            // Act
            var message = dispatcher.Merge("Test", american, properties, new[] { "noreply@test.com" });

            // Assert
            Assert.IsNotNull(message);
            Assert.AreEqual("Subject B", message.Subject, "Subject differs");
            Assert.AreEqual("Body B", message.Text, "Body differs");
            repository.VerifyAll();
        }

        [Test]
        public void CantFindTemplate()
        {
            var american = new CultureInfo("en-US");
            MessageTemplate template = null;
            var properties = new Dictionary<string, object> { { "A", "B" } };

            // Arrange
            var repository = new Mock<IMessageTemplateRepository>();
            var dispatcher = new MessageTemplateMerger(repository.Object);

            repository.Setup(x => x.Find("Test", american)).Returns(template);

            // Act
            var message = dispatcher.Merge("Test", american, properties, new[] { "noreply@test.com" });

            // Assert
            Assert.IsNull(message);
        }
    }
}