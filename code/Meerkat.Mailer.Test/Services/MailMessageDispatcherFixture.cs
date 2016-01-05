using Meerkat.Mailer.Attachments;
using Meerkat.Mailer.Services;

using Moq;

using nDumbster.Smtp;

using NUnit.Framework;

using System.Net.Mail;

namespace Meerkat.Mailer.Test.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    [TestFixture]
    public class MailMessageDispatcherFixture
    {
        private SimpleSmtpServer smtpServer;

        [SetUp]
        public void Setup()
        {
            // NB Use port configured in App.config
            var client = new SmtpClient();
            smtpServer = SimpleSmtpServer.Start(client.Port);
        }

        [TearDown]
        public void Teardown()
        {
            if (smtpServer != null)
            {
                smtpServer.Stop();
            }
        }

        [Test]
        public void NoReceipient()
        {
            var attachmentFactory = new Mock<IMailAttachmentFactory>();
            var notifier = new Mock<IMessageDispatchNotifier>();
            Exception ex = null;
            notifier.Setup(x => x.Notify(It.IsAny<IMessage>(), false, It.IsAny<Exception>()))
                    .Callback<IMessage, bool, Exception>((x, y, z) => ex = z);

            var dispatcher = new MailMessageDispatcher(attachmentFactory.Object, notifier.Object);

            var message = new Message();

            dispatcher.Send(message);

            Assert.AreEqual(0, smtpServer.ReceivedEmailCount, "Count differs");
            Assert.IsNotNull(ex);
            Assert.AreEqual("A recipient must be specified.", ex.Message);
        }

        [Test]
        public void Send()
        {
            var attachmentFactory = new Mock<IMailAttachmentFactory>();
            var notifier = new Mock<IMessageDispatchNotifier>();

            var dispatcher = new MailMessageDispatcher(attachmentFactory.Object, notifier.Object);

            var source = new TextAttachment
            {
                Name = "Text",
                Content = "Content"
            };

            var attachment = new Attachment(new MemoryStream(Encoding.ASCII.GetBytes("Content")), "Test");
            attachmentFactory.Setup(x => x.CreateAttachement(It.IsAny<IAttachment>())).Returns(attachment);

            var message = new Message
            {
                From = "from@test.com",
                FromName = "From",
                BounceAddress = "bounce@test.com",
                Subject = "Subject",
                Text = "Text",
                Html = "Html",
            };
            message.ToAddress.Add("to@test.com");
            message.Cc.Add("cc@test.com");
            message.Bcc.Add("bcc@test.com");
            message.Headers["X-Path"] = "xpath";
            message.Attachments.Add(source);
          
            dispatcher.Send(message);

            Assert.AreEqual(1, smtpServer.ReceivedEmailCount, "Count differs");
            notifier.Verify(x => x.Notify(It.IsAny<IMessage>(), true, null));

            var candidate = smtpServer.ReceivedEmail.First();
            Assert.AreEqual("Subject", candidate.Subject, "Subject differs");
        }
    }
}