using System.Collections.Generic;

using Meerkat.Mailer.Attachments;

namespace Meerkat.Mailer
{
    public interface IMessage : IMessageBase
    {
        /// <summary>
        /// Gets or sets the From property.
        /// <para>
        /// This is the email we are sending from.
        /// </para>
        /// </summary>
        string From { get; set; }

        /// <summary>
        /// Gets or sets the FromName property.
        /// <para>
        /// The display name of the email we are sending from.
        /// </para>
        /// </summary>
        string FromName { get; set; }

        List<string> ToAddress { get; set; }

        /// <summary>
        /// Gets or sets the address to handle bounces
        /// </summary>
        string BounceAddress { get; set; }
        
        /// <summary>
        /// Gets or sets the ReplyTo property.
        /// </summary>
        string ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the Headers property.
        /// <para>
        /// Allows for arbitrary headers to be added to the target email
        /// </para>
        /// </summary>
        Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets the list of Bcc addresses.
        /// </summary>
        List<string> Bcc { get; set; }
        
        /// <summary>
        /// Gets or sets the list of Cc addresses
        /// </summary>
        List<string> Cc { get; set; }

        /// <summary>
        /// Gets or sets the list of attachments
        /// </summary>
        List<IAttachment> Attachments { get; set; }
    }
}