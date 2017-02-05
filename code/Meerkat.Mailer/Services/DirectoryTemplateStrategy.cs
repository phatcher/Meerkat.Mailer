using System.IO;
using System.Reflection;

using Meerkat.Logging;

namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Strategy for template file names that partitions the culture as directories
    /// </summary>
    public class DirectoryTemplateStrategy : MessageTemplateFileStrategy
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DirectoryTemplateStrategy(string path) : base(path)
        {
        }

        public override string FileName(string templateName, string culture)
        {            
            var path = string.IsNullOrEmpty(culture)
                ? BasePath
                : Path.Combine(BasePath, culture);

            var result = Path.Combine(path, string.Format("{0}.xml", templateName));

            Logger.DebugFormat("{0} - {1}", templateName, result);

            return result;
        }
    }
}
