using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Tests {
    internal class CertificateUtil {
        public static string CERT_PASSWORD = 
            Guid.NewGuid().ToString();

        public static X509Certificate2 Cert { get; private set; } =
            MakeCert();

        public static X509Certificate2 MakeCert() {
            var ecdsa = ECDsa.Create(); // generate asymmetric key pair
            var req = new CertificateRequest("cn=foobar" , ecdsa , HashAlgorithmName.SHA256);
            return req.CreateSelfSigned(
                DateTimeOffset.Now , 
                DateTimeOffset.Now.AddMinutes(5)
            );
        }

        public static void WriteCert(string rootDirectory) {
            

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes(
                $"{rootDirectory}/test.pfx" , 
                Cert.Export(X509ContentType.Pfx , 
                CERT_PASSWORD)
            );

            // Create Base 64 encoded CER (public key only)
            File.WriteAllText($"{rootDirectory}/test.cer" ,
                "-----BEGIN CERTIFICATE-----\r\n"
                + Convert.ToBase64String(Cert.Export(X509ContentType.Cert) , Base64FormattingOptions.InsertLineBreaks)
                + "\r\n-----END CERTIFICATE-----");
        }
    }
}
