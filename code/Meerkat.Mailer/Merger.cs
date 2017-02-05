namespace Meerkat.Mailer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Merges property values over a text template
    /// </summary>
    public class Merger
    {
        // NB Don't make this shared as I'm not sure that RegEx will work with multiple threads accessing the same object
        private const string LeftDelimiter = @"{";
        private const string RightDelimiter = @"}";
        private const string OpenReference = LeftDelimiter + LeftDelimiter;
        private const string CloseRefence = RightDelimiter + RightDelimiter;
        private const string Capture = "property";

        private readonly Regex propertyRegex =
                new Regex(
                        string.Format(CultureInfo.InvariantCulture, @"(?:{0})(?<{2}>.*?)(?:{1})", OpenReference, CloseRefence, Capture),
                        RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly Dictionary<string, object> properties;

        /// <summary>
        /// Creates a new instance of the <see cref="Merger"/> class.
        /// </summary>
        public Merger()
        {
            // Make the properties case-insensitive.
            properties = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Merger"/> class.
        /// </summary>
        /// <param name="properties">Dictionary of properties</param>
        /// <exception cref="ArgumentNullException">When properties is null</exception>
        public Merger(IEnumerable<KeyValuePair<string, object>> properties) : this()
        {
            if (properties == null)
            {
                throw new ArgumentNullException("properties");
            }

            foreach (var prop in properties)
            {
                AddProperty(prop.Key, prop.Value);
            }
        }

        /// <summary>
        /// Adds a named property ready for merging
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddProperty(string name, object value)
        {
            properties[name] = value;
        }

        /// <summary>
        /// Merges the properties into the string
        /// </summary>
        /// <param name="value">Original value to merge against</param>
        /// <returns>The merged value.</returns>
        public string Merge(string value)
        {
            return propertyRegex.Replace(value, PropertyReplacer);
        }

        /// <summary>
        /// Gets the properties defined 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetProperties(string value)
        {
            var finder = new PropertyFinder();
            propertyRegex.Replace(value, finder.Log);

            return finder.Properties;
        }

        private string PropertyReplacer(Match m)
        {
            var propertyName = m.Groups[Capture].Value;
            var formatter = string.Empty;
            var posn = propertyName.IndexOf(':');
            if (posn > -1)
            {
                // Split before and after the first colon
                formatter = propertyName.Substring(posn + 1);
                propertyName = propertyName.Substring(0, posn);
            }

            // NB Using TryGetValue is 50% cheaper than ContainsKey + indexer.
            object value;
            if (!properties.TryGetValue(propertyName, out value))
            {
                return string.Empty;
            }

            formatter = string.IsNullOrEmpty(formatter) 
                        ? "{0}" 
                        : "{0:" + formatter + "}";

            // TODO: Use the current templates culture code
            var result = string.Format(formatter, value);

            return result;
        }

        private sealed class PropertyFinder
        {
            private readonly Dictionary<string, string> props;

            public PropertyFinder()
            {
                // NB Need the comparer to get case-insensitive behaviour
                props = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            }

            public IDictionary<string, string> Properties
            {
                get { return props; }
            }

            public string Log(Match m)
            {
                var propertyName = m.Groups[Capture].Value;

                props[propertyName] = null;

                return string.Empty;
            }
        }
    }
}