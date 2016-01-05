using System.Linq;

using NUnit.Framework;

namespace Meerkat.Mailer.Test
{
    [TestFixture]
    public class MessageFixture
    {
        [Test]
        public void ReadWriteProperties()
        {
            var m = new Message
            {
                From = "from@test.com",
                FromName = "From",
                ReplyTo = "reply@test.com",
                Subject = "Subject",
                Text = "Body",
                Html = "<p>Html</p>",
                BounceAddress = "BounceAddress"
            };
            m.ToAddress.Add("to@test.com");

            Assert.AreEqual("from@test.com", m.From, "From differs");
            Assert.AreEqual("From", m.FromName, "FromName differs");
            Assert.AreEqual("reply@test.com", m.ReplyTo, "ReplyTo differs");
            Assert.AreEqual("to@test.com", m.ToAddress.First(), "To differs");
            Assert.AreEqual("Subject", m.Subject, "Subject differs");
            Assert.AreEqual("Body", m.Text, "Text differs");
            Assert.AreEqual("<p>Html</p>", m.Html, "HTML differs");
            Assert.AreEqual("BounceAddress", m.BounceAddress, "BounceAddress differs");
        }
    }
}