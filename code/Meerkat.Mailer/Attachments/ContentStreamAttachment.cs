using System.IO;
using System.Net.Mime;

namespace Meerkat.Mailer.Attachments
{
    public class ContentStreamAttachment : IAttachment
    {
        public string Name { get; set; }

        public Stream Stream { get; set; }

        public ContentType ContentType { get; set; }
    }
}