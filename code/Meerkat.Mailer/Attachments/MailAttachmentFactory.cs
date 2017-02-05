using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;

using Meerkat.Logging;

namespace Meerkat.Mailer.Attachments
{
     public class MailAttachmentFactory : IMailAttachmentFactory
    {
        private static readonly ILog Logger = LogProvider.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Attachment CreateAttachement(IAttachment source)
        {
            Attachment attachment = null;
            var textAttachment = source as TextAttachment;
            if (textAttachment != null)
            {
                attachment = new Attachment(new MemoryStream(Encoding.ASCII.GetBytes(textAttachment.Content)), textAttachment.Name);
            }

            var streamAttachment = source as StreamAttachment;
            if (streamAttachment != null)
            {
                attachment = string.IsNullOrEmpty(streamAttachment.MediaType) 
                    ? new Attachment(streamAttachment.Stream, streamAttachment.Name) 
                    : new Attachment(streamAttachment.Stream, streamAttachment.Name, streamAttachment.MediaType);
            }

            var contentStreamAttachment = source as ContentStreamAttachment;
            if (contentStreamAttachment != null)
            {
                attachment = new Attachment(contentStreamAttachment.Stream, contentStreamAttachment.ContentType);
            }

            if (attachment != null)
            {
                attachment.Name = source.Name;
            }
            else
            {
                Logger.Warn("Unable to create attachment for unknown type");
            }

            return attachment;
        }
    }
}