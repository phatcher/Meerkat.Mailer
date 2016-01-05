namespace Meerkat.Mailer.Attachments
{
    public class TextAttachment : IAttachment
    {
        public string Name { get; set; }

        public string Content { get; set; }
    }
}