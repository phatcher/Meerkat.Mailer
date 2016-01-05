using System.IO;
using System.Net.Mime;
using System.Text;

using Meerkat.Mailer.Attachments;

using NUnit.Framework;

namespace Meerkat.Mailer.Test.Services
{
    [TestFixture]
    public class MailAttachmentFactoryFixture
    {
        [Test]
        public void TextAttachment()
        {
            var content = new TextAttachment
            {
                Name = "Test", 
                Content = "Content"
            };

            var factory = new MailAttachmentFactory();
            var candidate = factory.CreateAttachement(content);

            Assert.IsNotNull(candidate);
            Assert.AreEqual("Test", candidate.Name);
        }

        [Test]
        public void StreamAttachment()
        {
            var content = new StreamAttachment
            {
                Name = "Test",
                Stream = new MemoryStream(Encoding.ASCII.GetBytes("Content")),
                MediaType = "text/plain"
            };

            var factory = new MailAttachmentFactory();
            var candidate = factory.CreateAttachement(content);

            Assert.IsNotNull(candidate);
            Assert.AreEqual("Test", candidate.Name);
        }

        [Test]
        public void ContentAttachment()
        {
            var content = new ContentStreamAttachment
            {
                Name = "Test",
                Stream = new MemoryStream(Encoding.ASCII.GetBytes("Content")),
                ContentType = new ContentType("text/plain")
            };

            var factory = new MailAttachmentFactory();
            var candidate = factory.CreateAttachement(content);

            Assert.IsNotNull(candidate);
            Assert.AreEqual("Test", candidate.Name);
        }
    }
}