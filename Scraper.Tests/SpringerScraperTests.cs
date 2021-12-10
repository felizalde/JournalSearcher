using AngleSharp;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Common.Models;
using System.Text.Json;

namespace Scraper.Tests
{
    public class SpringerScraperTests
    {
        SpringerScraper scraper;

        [SetUp]
        public void Setup()
        {
            scraper = new SpringerScraper(BrowsingContext.New(
                                         AngleSharp.Configuration.Default
                                                    .WithDefaultLoader().WithDefaultCookies()));
        }

        [Test]
        public async Task ReadJournals()
        {
            var journals = await scraper.Run();
            Assert.IsNotNull(journals);
            //Assert.AreEqual(61, journals.Count);

            //var filtered = journals.Where(j => FilterJournalsWithMissingValues(j));
            //Assert.AreEqual(0, filtered.Count());

            var json = JsonSerializer.Serialize(journals);

            Assert.Pass();
        }


        private bool FilterJournalsWithMissingValues(Journal journal)
        {
            return string.IsNullOrEmpty(journal.OriginalID) ||
                   string.IsNullOrEmpty(journal.Title) ||
                   string.IsNullOrEmpty(journal.ImgUrl) ||
                   string.IsNullOrEmpty(journal.Url) ||
                   string.IsNullOrEmpty(journal.AimsAndScope) ||
                   string.IsNullOrEmpty(journal.About);
        }
    }
}