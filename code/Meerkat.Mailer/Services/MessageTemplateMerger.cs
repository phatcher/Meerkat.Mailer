using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using Meerkat.Logging;

namespace Meerkat.Mailer.Services
{
    public class MessageTemplateMerger : IMessageTemplateMerger
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IMessageTemplateRepository repository;

        public MessageTemplateMerger(IMessageTemplateRepository repository)
        {
            this.repository = repository;
        }

        public IMessage Merge(string templateName, CultureInfo cultureInfo, IDictionary<string, object> properties, IEnumerable<string> toAddress, IEnumerable<string> ccAddress = null, IEnumerable<string> bccAddress = null)
        {
            var template = repository.Find(templateName, cultureInfo);
            if (template == null)
            {
                Logger.ErrorFormat("{0}: Cannot merge, template not found for culture '{1}'", templateName, cultureInfo);
                return null;
            }

            return template.Merge(properties, toAddress, ccAddress, bccAddress);
        }
    }
}