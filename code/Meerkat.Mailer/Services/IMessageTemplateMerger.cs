using System.Collections.Generic;
using System.Globalization;

namespace Meerkat.Mailer.Services
{
    public interface IMessageTemplateMerger
    {
        /// <summary>
        /// Merges an <see cref="IMessageTemplate" /> and return it.
        /// </summary>
        /// <param name="toAddress">Destination addresses for the message</param>
        /// <param name="templateName">Name of the template to use</param>
        /// <param name="cultureInfo">Culture of the template to use</param>
        /// <param name="properties">Properties to substitute in the template</param>
        /// <param name="ccAddress">The Cc addresses for the message</param>
        /// <param name="bccAddress">The Bcc addresses for the message</param>
        IMessage Merge(string templateName, CultureInfo cultureInfo, IDictionary<string, object> properties, IEnumerable<string> toAddress, IEnumerable<string> ccAddress = null, IEnumerable<string> bccAddress = null);
    }
}