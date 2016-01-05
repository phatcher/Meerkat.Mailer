using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace Meerkat.Mailer.Test
{
    [TestFixture]
    public class MergerPerformanceFixture
    {
        private const string LoreiIpsum = "Lorem ipsum dolor sit amet, {0} consectetuer {1} adipiscing elit. {2} In non urna {3} sit amet {4} felis {5} pretium {6} tempor. {7} Etiam {8} arcu {9} libero, vestibulum";

        private IDictionary<string, object> properties;

        [Test]
        public void NoVariables()
        {
            Check("**", "**");
        }

        [Test]
        public void OneVariableNoProperties()
        {
            Check("**", "*{{Name}}*");
        }

        [Test]
        public void OneVariableMatchingProperty()
        {
            properties.Add("name", "John");
            Check("*John*", "*{{name}}*", "Lower case variable");
            Check("*John*", "*{{Name}}*", "Upper case property");
            Check("*John*", "*{{NaMe}}*", "Mixed case property");
        }

        [Test]
        public void OneVariableMultipleMatchesSingleLine()
        {
            properties.Add("name", "John");
            var source = @" {{Name}}  {{Name}}";
            var expected = source.Replace("{{Name}}", "John");

            Check(expected, source);
        }

        [Test]
        public void OneVariableMultipleMatchesMultiLine()
        {
            properties.Add("name", "John");
            var source = @"
            
            {{Name}}

            {{Name}}";
            var expected = source.Replace("{{Name}}", "John");

            Check(expected, source);
        }

        [Test]
        public void TwoVariablesSingleLine()
        {
            properties.Add("name", "John");
            properties.Add("surname", "Smith");
            var source = @" {{Name}}  {{Surname}}";
            var expected = @" John  Smith";

            Check(expected, source);
        }

        [Test]
        public void TwoVariablesMultiLine()
        {
            properties.Add("name", "John");
            properties.Add("surname", "Smith");
            var source = @" {{Name}}
                            {{Surname}}";
            var expected = source.Replace("{{Name}}", "John").Replace("{{Surname}}", "Smith");

            Check(expected, source);
        }

        [Test]
        public void EntityMerge()
        {
            var person = new Person { Forename = "John", Surname = "Smith" };
            var source = @" {{Forename}}
                            {{Surname}}";
            var expected = source.Replace("{{Forename}}", "John").Replace("{{Surname}}", "Smith");
            properties.AddRange(person.Flatten(useTypePrefix: false));

            Check(expected, source);
        }

        [Test]
        public void EntityPrefixMerge()
        {
            var person = new Person { Forename = "John", Surname = "Smith" };
            var source = @" {{Person.Forename}}
                            {{Person.Surname}}";
            var expected = source.Replace("{{Person.Forename}}", "John").Replace("{{Person.Surname}}", "Smith");
            properties.AddRange(person.Flatten());

            Check(expected, source);
        }

        [Test]
        [Category("Performance")]
        public void SingleProperty1()
        {
            properties.Add("name", "John");
            MultiMerge(BuildLoreiIpsum("{{Name}}"), 1);
        }

        [Test]
        [Category("Performance")]
        public void SinglePropertyMultiMatch1()
        {
            properties.Add("name", "John");
            MultiMerge(BuildLoreiIpsum("{{Name}}", "{{Name}}"), 1);
        }

        [Test]
        [Category("Performance")]
        public void TwoProperty1()
        {
            properties.Add("name", "John");
            properties.Add("surname", "Smith");
            MultiMerge(BuildLoreiIpsum("{{Name}}", "{{Surname}}"), 1);
        }

        [Test]
        [Category("Performance")]
        public void TwoPropertyMultiMatch1()
        {
            properties.Add("name", "John");
            properties.Add("surname", "Smith");
            MultiMerge(BuildLoreiIpsum("{{Name}}", "{{Surname}}", "{{Name}}", "{{Surname}}"), 1);
        }

        [Test]
        [Category("Performance")]
        public void SingleProperty10K()
        {
            properties.Add("name", "John");
            MultiMerge(BuildLoreiIpsum("{{Name}}"), 10000);
        }

        [Test]
        [Category("Performance")]
        public void SinglePropertyMultiMatch10K()
        {
            properties.Add("name", "John");
            MultiMerge(BuildLoreiIpsum("{{Name}}", "{{Name}}"), 10000);
        }

        [Test]
        [Category("Performance")]
        public void TwoProperty10K()
        {
            properties.Add("name", "John");
            properties.Add("surname", "Smith");
            MultiMerge(BuildLoreiIpsum("{{Name}}", "{{Surname}}"), 10000);
        }

        [Test]
        //[Explicit]
        [Category("Performance")]
        public void TwoPropertyMultiMatch10K()
        {
            properties.Add("name", "John");
            properties.Add("surname", "Smith");
            MultiMerge(BuildLoreiIpsum("{{Name}}", "{{Surname}}", "{{Name}}", "{{Surname}}"), 10000);
        }

        [Test]
        //[Explicit]
        [Category("Performance")]
        public void TwoPropertyMultiMatch100K()
        {
            properties.Add("name", "John");
            properties.Add("surname", "Smith");
            MultiMerge(BuildLoreiIpsum("{{Name}}", "{{Surname}}", "{{Name}}", "{{Surname}}"), 100000);
        }

        [Test]
        //[Explicit]
        [Category("Performance")]
        public void TwoPropertyMultiMatch1M()
        {
            properties.Add("name", "John");
            properties.Add("surname", "Smith");
            MultiMerge(BuildLoreiIpsum("{{Name}}", "{{Surname}}", "{{Name}}", "{{Surname}}"), 1000000);
        }

        [SetUp]
        public void Setup()
        {
            properties = new Dictionary<string, object>();
        }

        private static void ReDim(ref string[] arr, int length)
        {
            var temp = new string[length];
            if (length > arr.Length)
            {
                Array.Copy(arr, 0, temp, 0, arr.Length);
            }
            else
            {
                Array.Copy(arr, 0, temp, 0, length);
            }

            arr = temp;
        }

        private static string BuildLoreiIpsum(params string[] properties)
        {
            ReDim(ref properties, 10);
            return string.Format(LoreiIpsum, properties);
        }

        private void Check(string expected, string value)
        {
            Check(expected, value, "Merge results differ");
        }

        private void Check(string expected, string value, string message)
        {
            var merger = new Merger(properties);
            var result = merger.Merge(value);
            Assert.AreEqual(expected, result, message);
        }

        private void MultiMerge(string value, int iterations)
        {
            var merger = new Merger(properties);

            for (var i = 0; i < iterations; i++)
            {
                merger.Merge(value);
            }
        }
    }
}