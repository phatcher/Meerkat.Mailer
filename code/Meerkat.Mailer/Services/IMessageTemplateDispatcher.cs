namespace Meerkat.Mailer.Services
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Sends a message based on a template
    /// </summary>
    public interface IMessageTemplateDispatcher
    {
        /// <summary>
        /// Fills a <see cref="MessageTemplate" /> and dispatches it.
        /// </summary>
        /// <param name="toAddress">Destination addresses for the message</param>
        /// <param name="templateName">Name of the template to use</param>
        /// <param name="cultureInfo">Culture of the template to use</param>
        /// <param name="properties">Properties to substitute in the template</param>
        /// <param name="ccAddress">The Cc addresses for the message</param>
        /// <param name="bccAddress">The Bcc addresses for the message</param>
        void Send(string templateName, CultureInfo cultureInfo, IDictionary<string, object> properties, IEnumerable<string> toAddress, IEnumerable<string> ccAddress = null, IEnumerable<string> bccAddress = null);
    }
}