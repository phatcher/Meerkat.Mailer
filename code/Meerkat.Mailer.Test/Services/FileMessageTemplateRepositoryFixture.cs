using System.Globalization;

using Meerkat.Mailer.Services;

using NUnit.Framework;

namespace Meerkat.Mailer.Test.Services
{
    [TestFixture]
    public class FileMessageTemplateRepositoryFixture : Fixture
    {
        [Test]
        public void SaveTemplate()
        {
            var template = new MessageTemplate
            {
                Name = "fred",
                CultureCode = "en-GB",
                From = "test@test.com",
                FromName = "Test Name",
                Subject = "Subject",
                Text = "Hello world"
            };

            var strategy = new FilenameTemplateStrategy("App_Data/MailTemplates");
            var repository = new FileMessageTemplateRepository(strategy);

            repository.Save(template);

            var candidate = repository.Find("fred", new CultureInfo("en-GB")) as MessageTemplate;

            Check(template, candidate);
        }

        [Test]
        public void DirectoryStrategyFinder()
        {
            var template = new MessageTemplate
            {
                Name = "fred",
                CultureCode = "en",
                From = "test@test.com",
                FromName = "Test Name",
                Subject = "Subject",
                Text = "Hello world"
            };

            var strategy = new DirectoryTemplateStrategy("App_Data/MailTemplates");
            var repository = new FileMessageTemplateRepository(strategy);

            var candidate = repository.Find("fred", new CultureInfo("en-US")) as MessageTemplate;

            Check(template, candidate);
        }

        [Test]
        public void FileStrategyFinder()
        {
            var template = new MessageTemplate
            {
                Name = "fred",
                CultureCode = "en",
                From = "test@test.com",
                FromName = "Test Name",
                Subject = "Subject",
                Text = "Hello world"
            };

            var strategy = new FilenameTemplateStrategy("App_Data/MailTemplates");
            var repository = new FileMessageTemplateRepository(strategy);

            var candidate = repository.Find("fred", new CultureInfo("en-US")) as MessageTemplate;

            Check(template, candidate);
        }

    }
}