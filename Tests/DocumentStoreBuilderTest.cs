using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Client.Documents.Conventions;
using StratusCube.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static Tests.CertificateUtil;
using static Tests.TestConstants;

namespace Tests {

    [TestClass]
    public class DocumentStoreBuilderTest {
        DocumentStoreBuilder _optionsBuilder =
            new DocumentStoreBuilder();

        static TestContext? _context;

        static string TempDir =>
            $"{_context?.TestRunDirectory}/temp";

        [ClassInitialize]
        public static void Initialize(TestContext context) {
            _context = context;
            Directory.CreateDirectory(TempDir);
            WriteCert(TempDir);
        }

        [TestInitialize]
        public void InitializeTest() {
            _optionsBuilder =
                new DocumentStoreBuilder();
        }

        [TestMethod]
        public void AddCertificateTest() {
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.ThrowsException<ArgumentNullException>(() =>
                _optionsBuilder.AddCertificate(cert: null as X509Certificate2)
            );
#pragma warning restore CS8604 // Possible null reference argument.

            _optionsBuilder.AddCertificate(Cert);
            Assert.AreEqual(
                Cert?.Thumbprint ,
                _optionsBuilder?.Options?.Certificate?.Thumbprint
            );
        }

        [TestMethod]
        public void AddCertificateByteTest() {
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.ThrowsException<ArgumentNullException>(() =>
                _optionsBuilder.AddCertificate(
                    certBytes: null as byte[] ,
                    password: CERT_PASSWORD
                )
            );
            Assert.ThrowsException<ArgumentNullException>(() =>
                _optionsBuilder.AddCertificate(
                    certBytes: Cert.RawData ,
                    password: null as string
                )
            );

#pragma warning restore CS8604 // Possible null reference argument.

            var copyBytes = new byte[Cert.RawData.Length];
            Cert.RawData.CopyTo(copyBytes , 0);

            _optionsBuilder.AddCertificate(
                copyBytes ,
                CERT_PASSWORD
            );

            Assert.AreEqual(
                Cert?.Thumbprint ,
                _optionsBuilder?.Options?.Certificate?.Thumbprint
            );
        }

        [TestMethod]
        public void AddCertificateFuncTest() {
#pragma warning disable CS8604 // Possible null reference argument.
            Assert.ThrowsException<ArgumentNullException>(() =>
                _optionsBuilder.AddCertificate(cert: null as Func<X509Certificate2>)
            );
#pragma warning restore CS8604 // Possible null reference argument.

            _optionsBuilder.AddCertificate(() => Cert);
            Assert.AreEqual(
                Cert?.Thumbprint ,
                _optionsBuilder?.Options?.Certificate?.Thumbprint
            );
        }

        [TestMethod]
        public void AddCertificateFileTest() {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.ThrowsException<ArgumentNullException>(
                () => _optionsBuilder.AddCertificate(
                    filePath: $"{TempDir}/test.pfx" ,
                    password: null
                )
            );
            Assert.ThrowsException<ArgumentNullException>(
                () => _optionsBuilder.AddCertificate(
                    filePath: null ,
                    password: CERT_PASSWORD
                )
            );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            _optionsBuilder.AddCertificate(
                $"{TempDir}/test.pfx" , CERT_PASSWORD
            );
            Assert.AreEqual(
                Cert.Thumbprint ,
                _optionsBuilder?.Options?.Certificate?.Thumbprint
            );
        }

        private void WithSetup(Action<
            IDocumentStoreOptions ,
            DocumentConventions ,
            DocumentStoreSettings
        > setup) {
            var settings = new DocumentStoreSettings {
                DatabaseName = SAMPLE_DB_NAME ,
                Urls = new[] { SAMPLE_URL } ,
                Identifier = SAMPLE_ID
            };

            var conventions = new DocumentConventions {
                FindCollectionNameForDynamic = type =>
                    "@empty"
            }; ;

            var options = new DocumentStoreOptions {
                Certificate = Cert ,
                Settings = settings ,
                Conventions = conventions
            };

            setup(options , conventions , settings);
        }



        [TestMethod]
        public void ConfigureTest() =>
            WithSetup((expected , _ , _) => {
                var actual = (DocumentStoreBuilder) _optionsBuilder
                    .Configure(expected);

                Assert.IsTrue(expected.Equals(actual.Options));
            });

        [TestMethod]
        public void ConfigureActionTest() {

            //. For some reason the with function does not
            //. works as expected even though the object we're
            //. manually verified to be the same.
            var settings = new DocumentStoreSettings {
                DatabaseName = SAMPLE_DB_NAME ,
                Urls = new[] { SAMPLE_URL } ,
                Identifier = SAMPLE_ID
            };

            var conventions = new DocumentConventions {
                FindCollectionNameForDynamic = type =>
                    "@empty"
            };

            var expected = new DocumentStoreOptions {
                Certificate = Cert ,
                Conventions = conventions ,
                Settings = settings
            };

            var actual = (DocumentStoreBuilder) _optionsBuilder.Configure(options => {
                options.Certificate = Cert;
                options.Settings = settings;
                options.Conventions = conventions;

            });

            Assert.IsTrue(expected.Equals(actual.Options));
        }

        [TestMethod]
        public void ConfigureSettingsConfigurationTest() {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string , string> {
                        [nameof(IDocumentStoreSettings.DatabaseName)] = SAMPLE_DB_NAME ,
                        [$"{nameof(IDocumentStoreSettings.Urls)}:0"] = SAMPLE_URL ,
                        [nameof(IDocumentStoreSettings.Identifier)] = SAMPLE_ID
                    }.AsEnumerable()
                ).Build();

            var options = (DocumentStoreBuilder) _optionsBuilder.ConfigureSettings(config);

            var expected = new DocumentStoreSettings {
                DatabaseName = SAMPLE_DB_NAME ,
                Urls = new[] { SAMPLE_URL } ,
                Identifier = SAMPLE_ID
            };

            Assert.IsTrue(expected.Equals(options?.Options?.Settings));
        }

        [TestMethod]
        public void ConfigureSettingsTest() {
            var expected = new DocumentStoreSettings {
                DatabaseName = SAMPLE_DB_NAME,
                Urls = new [] { SAMPLE_URL } ,
                Identifier= SAMPLE_ID
            };

            var actual = (DocumentStoreBuilder) _optionsBuilder
                .ConfigureSettings(expected);

            Assert.IsTrue(expected.Equals(actual.Options.Settings));
        }

        [TestMethod]
        public void ConfigureSettingsActionTest() {
            var expected = new DocumentStoreSettings {
                DatabaseName = SAMPLE_DB_NAME ,
                Urls = new[] { SAMPLE_URL } ,
                Identifier = SAMPLE_ID
            };

            var actual = (DocumentStoreBuilder) _optionsBuilder
                .ConfigureSettings(settings => {
                    settings.DatabaseName = SAMPLE_DB_NAME;
                    settings.Urls = new[] { SAMPLE_URL };
                    settings.Identifier = SAMPLE_ID;
                });

            Assert.IsTrue(expected.Equals(actual.Options.Settings));
        }

        [TestMethod]
        public void ConfigureSettingsBuilderTest() {
            var expected = new DocumentStoreSettings {
                DatabaseName = SAMPLE_DB_NAME ,
                Urls = new[] { SAMPLE_URL } ,
                Identifier = SAMPLE_ID
            };

            var actual = (DocumentStoreBuilder) _optionsBuilder
                .ConfigureSettings(builder =>
                    builder.AddUrl(SAMPLE_URL)
                        .ConfigureDatabaseName(SAMPLE_DB_NAME)
                        .ConfigureSettings(_ => _.Identifier = SAMPLE_ID)
                );

            Assert.IsTrue(expected.Equals(actual.Options.Settings));
        }

        [TestMethod]
        public void ConfigureConventionsActionTest() {

            var actual = (DocumentStoreBuilder)_optionsBuilder
                .ConfigureConventions(conventions => {
                    conventions.FindCollectionNameForDynamic = type =>
                        "@empty";
                });

            Assert.IsNotNull(
                actual?.Options
                    ?.Conventions
                    ?.FindCollectionNameForDynamic
            );
        }

        [TestMethod]
        public void BuildTest() {

            _optionsBuilder.ConfigureSettings(builder => {
                builder.AddUrl("http:/foo");
                builder.ConfigureDatabaseName(SAMPLE_DB_NAME);
                builder.ConfigureSettings(_ => {
                    _.Identifier = SAMPLE_ID;
                });
            }).AddCertificate(Cert);

            Assert.ThrowsException<ArgumentException>(() =>
                _optionsBuilder.Build()
            );

            _optionsBuilder.ConfigureSettings(_ => _.Urls = new[] { SAMPLE_URL });

            var doc = _optionsBuilder.Build();

            Assert.AreEqual(
                _optionsBuilder.Options.Settings.DatabaseName , doc.Database
            );

            CollectionAssert.AreEquivalent(
                _optionsBuilder.Options.Settings.Urls , doc.Urls
            );

            Assert.AreEqual(Cert , doc.Certificate);
        }

        [ClassCleanup]
        public static void Cleanup() {
            Directory.Delete(TempDir , true);
        }
    }
}
