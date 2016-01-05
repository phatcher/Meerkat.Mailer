namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Determines the filename for a message template.
    /// </summary>
    public interface IMessageTemplateFileStrategy
    {
        /// <summary>
        /// Determine the template's file name from its name and culture.
        /// </summary>
        /// <param name="templateName">Name to use</param>
        /// <param name="culture">Culture to use</param>
        /// <returns>Fully-qualified path to the appropriate template</returns>
        string FileName(string templateName, string culture);
    }
}