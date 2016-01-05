using System.Globalization;

namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Finds message templates
    /// </summary>
    public interface IMessageTemplateRepository
    {
        /// <summary>
        /// Find a template for the specified culture
        /// </summary>
        /// <param name="name">Name of the template</param>
        /// <param name="cultureInfo">Culture to use, default to invariant culture</param>
        /// <returns>The message template</returns>
        IMessageTemplate Find(string name, CultureInfo cultureInfo = null);

        /// <summary>
        /// Save a message template.
        /// </summary>
        /// <param name="template"></param>
        void Save(IMessageTemplate template);
    }
}