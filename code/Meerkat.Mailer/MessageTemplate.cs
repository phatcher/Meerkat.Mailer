using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Meerkat.Mailer
{
    /// <summary>
    /// A message template with values ready to be merged
    /// </summary>
    [DataContract(Name = "messageTemplate")]
    [XmlRoot("messageTemplate")]
    public class MessageTemplate : Message, IMessageTemplate
    {
        /// <summary>
        /// Gets or sets the Id property.
        /// </summary>
        [XmlIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name property.
        /// </summary>
        [DataMember(Name = "name", Order = 1)]
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the CultureCode property.
        /// </summary>
        [DataMember(Name = "cultureCode", Order = 2)]
        [XmlAttribute("cultureCode")]
        public string CultureCode { get; set; }
    }
}