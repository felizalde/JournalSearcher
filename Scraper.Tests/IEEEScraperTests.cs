using AngleSharp;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Linq;
using Common.Models;
using System.Text.Json;

namespace Scraper.Tests
{
    public class IEEEScraperTests
    {
        IEEEScraper scraper;

        [SetUp]
        public void Setup()
        {
            scraper = new IEEEScraper(BrowsingContext.New(
                                         AngleSharp.Configuration.Default
                                                    .WithDefaultLoader().WithDefaultCookies()));
        }

        [Test]
        public async Task ReadJournals()
        {
            var journals = await scraper.GetListOfJouranls();
            Assert.IsNotNull(journals);
            //Assert.AreEqual(61, journals.Count);

            //var filtered = journals.Where(j => FilterJournalsWithMissingValues(j));
            //Assert.AreEqual(0, filtered.Count());

            var json = JsonSerializer.Serialize(journals);

            Assert.Pass();
        }
    }
}