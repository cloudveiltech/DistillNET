using System;
using System.IO;
using System.Threading.Tasks;
using DistillNET;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DistillNETUnitTests
{
    [TestClass]
    public class FilterDbCollectionTests
    {
        private string[] getLineList()
        {
            return
            new string[] {
                "||testsite.com",
                "||pornsite.net",
                "||bad-subdomain.goodsite.net",
                "||goodsite.org^badurl",
                "(@$*)#()oboy-badpattern"
            };
        }

        private Tuple<int, int> getTestCollection(out FilterDbCollection collection)
        {
            string[] rawRuleStrings = getLineList();
            collection = new FilterDbCollection();

            Tuple<int, int> result = collection.ParseStoreRules(rawRuleStrings, 1).Result;

            return result;
        }

        [TestMethod]
        public void TestLoad_FromLines()
        {
            FilterDbCollection collection = null;
            Tuple<int, int> result = getTestCollection(out collection);

            Assert.AreEqual(result.Item1, 4);
            Assert.AreEqual(result.Item2, 1);
        }

        [TestMethod]
        public async void TestLoad_FromStream()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    var streamContent = string.Join("\r\n", getLineList());
                    writer.WriteLine(streamContent);
                }

                stream.Seek(0, SeekOrigin.Begin);

                FilterDbCollection memoryCollection = new FilterDbCollection();

                var result = await memoryCollection.ParseStoreRulesFromStream(stream, 1);
                Assert.AreEqual(result.Item1, 4);
                Assert.AreEqual(result.Item2, 1);
            }

            // TODO: Devise tests for testing bloom filters and GetFiltersFromDomain
        }
    }
}
