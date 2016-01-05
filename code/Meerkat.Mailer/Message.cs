using System.Collections.Generic;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using Meerkat.Mailer.Attachments;

namespace Meerkat.Mailer
{
    /// <summary>
    /// A message we want to send.
    /// </summary>
    [DataContract]
    public class Message : IMessage
    {
        private string replyTo;
        private List<IAttachment> attachments;

        public Message()
        {
            ToAddress = new List<string>();
            Bcc = new List<string>();
            Cc = new List<string>();
            Headers = new Dictionary<string, string>();
        }

        [DataMember(Order = 1)]
        public List<string> ToAddress { get; set; }

        [DataMember(Order = 2)]
        public List<string> Bcc { get; set; }

        [DataMember(Order = 3)]
        public List<string> Cc { get; set; }

        [DataMember(Order = 4)]
        public Dictionary<string, string> Headers { get; set; }

        [XmlIgnore]
        public List<IAttachment> Attachments 
        {
            get { return attachments ?? (attachments = new List<IAttachment>()); }
            set { attachments = value; }
        }

        /// <summary>
        /// Gets or sets the From property.
        /// <para>
        /// This is the email we are sending from.
        /// </para>
        /// </summary>
        [DataMember(Name = "fromEmail", Order = 5)]
        [XmlAttribute("fromEmail")]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the FromName property.
        /// <para>
        /// The display name of the email we are sending from.
        /// </para>
        /// </summary>
        [DataMember(Name = "fromName", Order = 6)]
        [XmlAttribute("fromName")]
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the ReplyTo property.
        /// </summary>
        [DataMember(Name = "replyToEmail", Order = 7)]
        [XmlAttribute("replyToEmail")]
        public string ReplyTo
        {
            get { return string.IsNullOrEmpty(replyTo) ? From : replyTo; }
            set { replyTo = value; }
        }

        /// <summary>
        /// Gets or sets the address to handle bounces
        /// </summary>
        [DataMember(Name = "bounceEmail", Order = 8)]
        [XmlAttribute("bounceEmail")]
        public string BounceAddress { get; set; }

        /// <summary>
        /// Gets or sets the Subject property.
        /// </summary>
        [DataMember(Name = "subjectEmail", Order = 9)]
        [XmlElement("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the text property.
        /// </summary>
        [DataMember(Name = "text", Order = 10)]
        [XmlElement("text")]
        public string Text { get; set; }

        /// <copydoc cref="IMessageBase.Html" />
        [DataMember(Name = "html", Order = 11)]
        [XmlElement("html")]
        public string Html { get; set; }
    }
}