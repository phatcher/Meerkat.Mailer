namespace Meerkat.Mailer
{
    public interface IMessageBase
    {
        /// <summary>
        /// Gets or sets the Subject property.
        /// </summary>
        string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Text property.
        /// <para>
        /// This is the body of the email in text form.
        /// </para>
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the Html property,
        /// <para>
        /// This is the body of the email in HTML
        /// </para>
        /// </summary>
        string Html { get; set; }
    }
}