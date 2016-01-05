using System.IO;

namespace Meerkat.Mailer.Attachments
{
    public class StreamAttachment : IAttachment
    {
        public string Name { get; set; }

        public Stream Stream { get; set; }

        public string MediaType { get; set; }
    }
}