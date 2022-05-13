using Microsoft.VisualStudio.TestTools.UnitTesting;

using StratusCube.Extensions.DependencyInjection;
using System;
using System.Linq;
using static Tests.TestConstants;

namespace Tests {
    [TestClass]
    public class SettingsBuilderTests {

        IDocumentStoreSettingsBuilder _settingsBuilder =
            new DocumentStoreSettingsBuilder();

        private void AssertAllUrls(string[] urls) =>
            Assert.IsTrue(
                urls.All(u => new Uri(SAMPLE_URL) == new Uri(u))
            );

        [TestInitialize]
        public void InitializeTest() =>
            _settingsBuilder = new DocumentStoreSettingsBuilder();

        [TestMethod]
        public void AddUrlTest() {
            _settingsBuilder.AddUrl(SAMPLE_URL);
            var settings = _settingsBuilder.Build();
            Assert.AreEqual(1 , settings.Urls.Length);
            _settingsBuilder.AddUrl(new Uri(SAMPLE_URL));
            settings = _settingsBuilder.Build();
            Assert.AreEqual(2 , settings.Urls.Length);
            AssertAllUrls(settings.Urls);
        }

        [TestMethod]
        public void AddUrlsTest() {
            var expected = Enumerable.Range(0 , 2).Select(_ => SAMPLE_URL);
            var settings = _settingsBuilder
                .AddUrls(expected)
                .Build();
            Assert.AreEqual(expected.Count() , settings.Urls.Length);
            settings = _settingsBuilder
                .AddUrls(expected.Select(u => new Uri(u)))
                .Build();
            
            Assert.AreEqual(expected.Count() * 2, settings.Urls.Length);

            AssertAllUrls(settings.Urls);
        }

        [TestMethod]
        public void ConfigureDatabaseNameTest() {
            var settings = _settingsBuilder
                .ConfigureDatabaseName(SAMPLE_DB_NAME)
                .Build();

            Assert.AreEqual(SAMPLE_DB_NAME , settings.DatabaseName);
        }

        [TestMethod]
        public void ConfigureSettings() {
            var expected = new DocumentStoreSettings {
                DatabaseName = SAMPLE_DB_NAME ,
                Urls = new[] { SAMPLE_URL } ,
                Identifier = SAMPLE_ID
            };

            var actual = _settingsBuilder.ConfigureSettings(s => {
                s.DatabaseName = SAMPLE_DB_NAME;
                s.Urls = expected.Urls;
                s.Identifier = SAMPLE_ID;
            }).Build();

            Assert.AreEqual(SAMPLE_ID , actual.Identifier);

            Assert.IsTrue(expected.Equals(actual));
        }
    }
}