using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Meerkat.Mailer.Test
{

    [TestFixture]
    public class MergerFixture
    {
        [Test]
        public void NullPropertiesSupplied()
        {
            Assert.Throws<ArgumentNullException>(() => new Merger(null));
        }

        [Test]
        public void SimpleMerge()
        {
            var candidate = "Test {{a}}";
            var expected = "Test B";

            var m = new Merger();
            m.AddProperty("a", "B");

            Assert.AreEqual(expected, m.Merge(candidate));
        }

        [Test]
        public void MergeInt()
        {
            var candidate = "Test {{a}}";
            var expected = "Test 1";

            var m = new Merger();
            m.AddProperty("a", 1);

            Assert.AreEqual(expected, m.Merge(candidate));
        }

        [Test]
        public void MergeDecimal()
        {
            var candidate = "Test {{a}}";
            var expected = "Test 1.2";

            var m = new Merger();
            m.AddProperty("a", 1.2M);

            Assert.AreEqual(expected, m.Merge(candidate));
        }

        [Test]
        public void MergeDouble()
        {
            var candidate = "Test {{a}}";
            var expected = "Test 1.2";

            var m = new Merger();
            m.AddProperty("a", 1.2);

            Assert.AreEqual(expected, m.Merge(candidate));
        }

        [Test]
        public void MergeDate()
        {
            var candidate = "Test {{a}}";
            var expected = "Test 09/10/2015 12:34:00";

            var value = new DateTime(2015, 10, 9, 12, 34, 0);
            var m = new Merger();
            m.AddProperty("a", value);

            Assert.AreEqual(expected, m.Merge(candidate));
        }

        [Test]
        public void MergeDateFormatted()
        {
            var candidate = "Test {{a:d MMM yyyy}}";
            var expected = "Test 9 Oct 2015";

            var value = new DateTime(2015, 10, 9, 12, 34, 0);
            var m = new Merger();
            m.AddProperty("a", value);

            Assert.AreEqual(expected, m.Merge(candidate));
        }

        [Test]
        public void MergeTime()
        {
            var candidate = "Test {{a:HH:mm}}";
            var expected = "Test 12:34";

            var value = new DateTime(2015, 10, 9, 12, 34, 0);
            var m = new Merger();
            m.AddProperty("a", value);

            Assert.AreEqual(expected, m.Merge(candidate));
        }

        [Test]
        public void MergeCaseInsensitive()
        {
            var candidate = "Test {{A}}";
            var expected = "Test B";

            var m = new Merger();
            m.AddProperty("a", "B");

            Assert.AreEqual(expected, m.Merge(candidate));
        }

        [Test]
        public void TwoPassMerge()
        {
            var candidate = "Test {{A}}";
            var expected = "Test C";

            var d = new Dictionary<string, object>();
            d.Add("A", "{{B}}");
            d.Add("B", "C");

            // Test the other constructor
            var m = new Merger(d);

            Assert.AreEqual(expected, m.Merge(m.Merge(candidate)));
        }

        [Test]
        public void FindSingleProperty()
        {
            var candidate = "Test {{A}}";

            var m = new Merger();

            var props = m.GetProperties(candidate);
            Assert.AreEqual(1, props.Count);
            Assert.IsTrue(props.ContainsKey("A"));
        }

        [Test]
        public void FindTwoProperty()
        {
            var candidate = "Test {{A}} {{B}}";

            var m = new Merger();

            var props = m.GetProperties(candidate);
            Assert.AreEqual(2, props.Count);
            Assert.IsTrue(props.ContainsKey("A"));
            Assert.IsTrue(props.ContainsKey("B"));
        }

        [Test]
        public void FindSinglePropertyMultipleInstance()
        {
            var candidate = "Test {{A}} {{A}}";

            var m = new Merger();

            var props = m.GetProperties(candidate);
            Assert.AreEqual(1, props.Count);
            Assert.IsTrue(props.ContainsKey("a"));
        }

        [Test]
        public void FindSinglePropertyDifferentCaseMultipleInstance()
        {
            var candidate = "Test {{A}} {{a}}";

            var m = new Merger();

            var props = m.GetProperties(candidate);
            Assert.AreEqual(1, props.Count);
            Assert.IsTrue(props.ContainsKey("a"));
        }
    }
}