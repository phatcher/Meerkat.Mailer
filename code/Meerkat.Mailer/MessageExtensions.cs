using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Meerkat.Mailer
{
    public static class MessageExtensions
    {        
        /// <summary>
        /// Copy the key properties of a messaage to another message.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        public static void CopyTo(this IMessage source, IMessage message)
        {
            message.FromName = source.FromName;
            message.From = source.From;
            message.ReplyTo = source.ReplyTo;
            message.Subject = source.Subject;
            message.Text = source.Text;
            message.Html = source.Html;
            message.ToAddress.AddRange(source.ToAddress);
            message.Cc.AddRange(source.Cc);
            message.Bcc.AddRange(source.Bcc);
            message.Attachments.AddRange(source.Attachments);
        }

        /// <summary>
        /// Flatten an entity into a <see cref="IDictionary{T,U}"/> of properties so it can be used 
        /// by the template merger.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="useTypePrefix"></param>
        public static IDictionary<string, object> Flatten<T>(this T entity, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance, bool useTypePrefix = true)
        {
            if (entity == null)
            {
                return new Dictionary<string, object>();
            }

            // NB Use the type supplied rather than entity.GetType due to proxies from EF etc
            var type = typeof(T);

            // Based on http://stackoverflow.com/questions/4943817/mapping-object-to-dictionary-and-vice-versa/4944547#4944547
            var values = type.GetProperties(bindingAttr).ToDictionary
            (
                propInfo => useTypePrefix ? type.Name + "." + propInfo.Name : propInfo.Name,
                propInfo =>
                {
                    var x = propInfo.GetValue(entity, null);
                    return x;
                }
            );

            return values;
        }

        /// <summary>
        /// Add multiple values to a dictionary
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="values"></param>
        public static void AddRange(this IDictionary<string, object> properties, IDictionary<string, object> values)
        {
            foreach (var prop in values)
            {
                properties.Add(prop);
            }
        }

        /// <summary>
        /// Return the properties defined in the subject and body.
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetProperties(this IMessage message)
        {
            if (message == null)
            {
                return new Dictionary<string, string>();
            }

            var merger = new Merger();
            return merger.GetProperties(message.Subject + message.Text);
        }

        /// <summary>
        /// Merges the properties into the subject and html/text body.
        /// </summary>
        /// <param name="message">Message to merge</param>
        /// <param name="properties">Properties to use</param>
        public static void Merge(this IMessage message, IDictionary<string, object> properties)
        {
            if (properties == null || properties.Count == 0)
            {
                return;
            }

            var merger = new Merger(properties);
            if (!string.IsNullOrWhiteSpace(message.Subject))
            {
                message.Subject = merger.Merge(message.Subject);
            }

            if (!string.IsNullOrWhiteSpace(message.Text))
            {
                message.Text = merger.Merge(message.Text);
            }
        }

        /// <summary>
        /// Merges standard details and properties into the template producing a message
        /// </summary>
        /// <param name="template"></param>
        /// <param name="toAddress"></param>
        /// <param name="properties"></param>
        /// <param name="ccAddress"></param>
        /// <param name="bccAddress"></param>
        /// <returns></returns>
        public static IMessage Merge(this IMessageTemplate template, IDictionary<string, object> properties, IEnumerable<string> toAddress, IEnumerable<string> ccAddress = null, IEnumerable<string> bccAddress = null)
        {
            var to = toAddress ?? Enumerable.Empty<string>();
            var cc = ccAddress ?? Enumerable.Empty<string>();
            var bcc = bccAddress ?? Enumerable.Empty<string>();
            var props = properties ?? new Dictionary<string, object>();

            // Create a new target message and copy the base properties over
            var message = new Message();

            // Put a copy of our stuff in there
            template.CopyTo(message);

            // Set up who we are sending to
            foreach (var s in to)
            {
                message.ToAddress.Add(s);
            }

            foreach (var s in bcc)
            {
                message.Bcc.Add(s);
            }

            foreach (var s in cc)
            {
                message.Cc.Add(s);
            }

            // Ok, do the merging
            // NOTE: Do the merge twice as we might be pushing values over that contain variables
            message.Merge(props);
            message.Merge(props);

            return message;
        }

        /// <summary>
        /// Formats an email address with a display name
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string PrettyEmail(this string email, string name = null)
        {
            return string.IsNullOrEmpty(name) ? email : string.Format("{0} <{1}>", name, email);
        }
    }
}