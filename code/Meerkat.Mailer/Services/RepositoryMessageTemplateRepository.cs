namespace Meerkat.Mailer.Services
{
    using System.Globalization;

    /// <summary>
    /// Implementation of IMessageTemplateRepository that uses a repository.
    /// </summary>
    public class RepositoryMessageTemplateRepository : MessageTemplateRepository
    {
        private readonly IRepository repository;

        /// <summary>
        /// Initialize a new instance of the MessageTemplateRepository class.
        /// </summary>
        /// <param name="repository"></param>
        public RepositoryMessageTemplateRepository(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Saves a message template.
        /// </summary>
        /// <param name="template"></param>
        public override void Save(MessageTemplate template)
        {
            repository.Save(template);
        }

        /// <summary>
        /// Find a message template.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        protected override MessageTemplate OnFind(string name, CultureInfo cultureInfo)
        {
            return (from t in repository.Queryable<MessageTemplate>()
                    where t.Name == name
                       && t.CultureCode == cultureInfo.Name
                    select t).FirstOrDefault();
        }
    }
}