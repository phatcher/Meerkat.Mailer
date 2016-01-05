using System.Collections.Generic;

using NUnit.Framework;

namespace Meerkat.Mailer.Test
{
    [TestFixture]
    public class MessageTemplateFixture
    {
        [Test]
        public void SubjectAndBodyPreserved()
        {
            var t = new MessageTemplate { Subject = "Subject", Text = "Body" };

            var properties = new Dictionary<string, object>();

            var m = t.Merge(properties, new [] { "noreply@test.com" });
            Assert.AreEqual(m.Subject, t.Subject, "Subject differs");
            Assert.AreEqual(m.Text, t.Text, "Body differs");
        }

        [Test]
        public void SimpleMerge()
        {
            var t = new MessageTemplate { Subject = "Subject {{A}}", Text = "Body {{A}}" };

            var properties = new Dictionary<string, object> { { "A", "B" } };

            var m = t.Merge(properties, new[] { "noreply@test.com" });
            Assert.AreEqual("Subject B", m.Subject, "Subject differs");
            Assert.AreEqual("Body B", m.Text, "Body differs");
        }

        [Test]
        public void DoubleMerge()
        {
            var t = new MessageTemplate { Subject = "Subject {{A}}", Text = "Body {{B}}" };

            var properties = new Dictionary<string, object>
            {
                { "A", "{{B}}" }, 
                { "B", "C" }
            };

            var m = t.Merge(properties, new[] { "noreply@test.com" });
            Assert.AreEqual("Subject C", m.Subject, "Subject differs");
            Assert.AreEqual("Body C", m.Text, "Body differs");
        }

        [Test]
        public void PropertyInBody()
        {
            var t = new MessageTemplate { Subject = "Subject", Text = "Body {{B}}" };

            var properties = t.GetProperties();
            Assert.AreEqual(1, properties.Count);
            Assert.IsTrue(properties.ContainsKey("b"));
        }

        [Test]
        public void PropertyInSubject()
        {
            var t = new MessageTemplate { Subject = "Subject {{A}}", Text = "Body" };

            var properties = t.GetProperties();
            Assert.AreEqual(1, properties.Count);
            Assert.IsTrue(properties.ContainsKey("a"));
        }

        [Test]
        public void CanMergeWithAllNullParameters()
        {
            var t = new MessageTemplate();
            var m = t.Merge(null, null, null, null);
            Assert.IsNotNull(m);
        }
    }
}