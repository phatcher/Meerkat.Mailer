namespace Meerkat.Mailer
{
    public interface IMessageTemplate : IMessage
    {
        /// <summary>
        /// Gets or sets the Id property.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name property.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the CultureCode property.
        /// </summary>
        string CultureCode { get; set; }
    }
}