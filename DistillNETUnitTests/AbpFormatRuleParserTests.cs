using System;
using DistillNET;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DistillNETUnitTests
{
    [TestClass]
    public class AbpFormatRuleParserTests
    {
        [TestMethod]
        public void TestRegularDomainRule_WithoutException()
        {
            var parser = new AbpFormatRuleParser();

            Filter filter = parser.ParseAbpFormattedRule("||testsite.com", 1);

            // Needs to be a URL filter.
            if(!(filter is UrlFilter))
            {
                Assert.Fail();
            }

            UrlFilter urlFilter = filter as UrlFilter;

            // is not exception to rule.
            Assert.IsFalse(urlFilter.IsException);

            // all subdomains should match as well.
            Assert.IsTrue(urlFilter.IsMatch(new Uri("https://subdomain.testsite.com"), null));

            Assert.IsTrue(urlFilter.IsMatch(new Uri("https://testsite.com"), null));

            Assert.IsTrue(urlFilter.IsMatch(new Uri("http://testsite.com"), null));

            Assert.IsFalse(urlFilter.IsMatch(new Uri("http://does-not-match.com"), null));
        }

        [TestMethod]
        public void TestRegularDomainRule_WithException()
        {
            var parser = new AbpFormatRuleParser();

            Filter filter = parser.ParseAbpFormattedRule("@@||testsite.com", 1);

            // Needs to be a URL filter.
            if (!(filter is UrlFilter))
            {
                Assert.Fail();
            }

            UrlFilter urlFilter = filter as UrlFilter;

            // is exception to rule.
            Assert.IsTrue(urlFilter.IsException);

            // no need to test matching again for this one.
        }

        [TestMethod]
        public void TestUrlRule()
        {
            var parser = new AbpFormatRuleParser();

            Filter filter = parser.ParseAbpFormattedRule("||reddit.com^r^nsfw", 1);

            // Needs to be a URL filter.
            if (!(filter is UrlFilter))
            {
                Assert.Fail();
            }

            UrlFilter urlFilter = filter as UrlFilter;

            // is exception to rule.
            Assert.IsFalse(urlFilter.IsException);

            Assert.IsTrue(urlFilter.IsMatch(new Uri("https://reddit.com/r/nsfw/posthere"), null));

            Assert.IsFalse(urlFilter.IsMatch(new Uri("https://reddit.com/r/good"), null));
        }
    }
}
