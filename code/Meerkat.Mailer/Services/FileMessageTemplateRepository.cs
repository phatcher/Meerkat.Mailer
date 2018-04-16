using System;
using System.Globalization;
using System.IO;
using System.Reflection;

using Meerkat.Logging;

namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Retrieve the message templates from the file system
    /// </summary>
    public class FileMessageTemplateRepository : IMessageTemplateRepository
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMessageTemplateFileStrategy strategy;

        /// <summary>
        /// Initializes a new instance of the FileMessageTemplateRepository class.
        /// </summary>
        /// <param name="strategy"></param>
        public FileMessageTemplateRepository(IMessageTemplateFileStrategy strategy)
        {
            this.strategy = strategy;
        }

        /// <summary>
        /// Save a message template.
        /// </summary>
        /// <param name="entity"></param>
        public void Save(IMessageTemplate entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (!(entity is MessageTemplate template))
            {
                throw new NotSupportedException("Cannot persist unless entity is a MessageTemplate");
            }

            Save(template, FileName(template.Name, template.CultureCode));
        }

        public IMessageTemplate Find(string name, CultureInfo cultureInfo = null)
        {
            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.InvariantCulture;
            }

            var fileName = FileName(name, cultureInfo.Name);

            Logger.Debug("Looking for " + fileName);

            // Keep going while we haven't found it, and we haven't reached the top-most culture.
            while (!File.Exists(fileName) && cultureInfo != CultureInfo.InvariantCulture)
            {
                // Walk the resource hierarchy
                cultureInfo = cultureInfo.Parent;
                fileName = FileName(name, cultureInfo.Name);
                Logger.Debug("Looking for " + fileName);
            }

            if (File.Exists(fileName))
            {
                return Load(fileName);
            }

            Logger.WarnFormat("{0}: Template not found in '{1}'", name, fileName);
            return null;
        }

        private string FileName(string name, string culture)
        {
            return strategy.FileName(name, culture);
        }

        private MessageTemplate Load(string fileName)
        {
            return fileName.LoadDataContract<MessageTemplate>();
            //return fileName.LoadXmlDocument<MessageTemplate>();
        }

        private void Save(MessageTemplate template, string fileName)
        {
            template.DataContractSerialize(fileName);
            //template.XmlSerialize(fileName);
        }
    }
}