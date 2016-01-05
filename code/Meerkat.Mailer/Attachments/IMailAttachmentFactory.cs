namespace Meerkat.Mailer.Attachments
{
    using System.Net.Mail;

    public interface IMailAttachmentFactory
    {
        Attachment CreateAttachement(IAttachment attachment);
    }
}