using System.IO;
using System.Reflection;

using Meerkat.Logging;

namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Abstract implementation of a <see cref="IMessageTemplateFileStrategy"/>.
    /// </summary>
    public abstract class MessageTemplateFileStrategy : IMessageTemplateFileStrategy
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected MessageTemplateFileStrategy(string path)
        {
            BasePath = path.MapPath();
            Logger.Debug("Checking BasePath: " + BasePath);

            if (Directory.Exists(BasePath))
            {
                return;
            }

            Logger.Info("Creating: " + BasePath);
            Directory.CreateDirectory(BasePath);            
        }

        /// <summary>
        /// Gets the base path for templates.
        /// </summary>
        public string BasePath { get; private set; }

        /// <copydoc cref="IMessageTemplateFileStrategy.FileName" />
        public abstract string FileName(string templateName, string culture);
    }
}
