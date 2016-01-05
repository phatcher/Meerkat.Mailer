using System.IO;
using System.Reflection;

using Common.Logging;

namespace Meerkat.Mailer.Services
{
    /// <summary>
    /// Strategy for template names that partitions the culture as part of the template file name.
    /// </summary>
    public class FilenameTemplateStrategy : MessageTemplateFileStrategy
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FilenameTemplateStrategy(string path) : base(path)
        {
        }

        public override string FileName(string name, string culture)
        {
            var result = string.IsNullOrEmpty(culture)
                ? Path.Combine(BasePath, string.Format("{0}.xml", name))
                : Path.Combine(BasePath, string.Format("{0}.{1}.xml", name, culture));

            Logger.DebugFormat("{0} - {1}", name, result);

            return result;
        }
    }
}