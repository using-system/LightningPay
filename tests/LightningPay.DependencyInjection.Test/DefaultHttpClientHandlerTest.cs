using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace LightningPay.DependencyInjection.Test
{
    public class DefaultHttpClientHandlerTest
    {
        [Fact]
        public void Ctor_Should_Add_CerValCb_If_AllowInsecure_Setted()
        {
            //Arrange
            var httpHandler = new DefaultHttpClientHandler(new HttpClientHandlerOptions()
            {
                AllowInsecure = true
            });

            //Act
            var actual = 
                httpHandler.ServerCertificateCustomValidationCallback(null, null, null, System.Net.Security.SslPolicyErrors.None);

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void Ctor_Should_Add_CerValCb_If_CertificateThumbprint_Setted()
        {
            //Arrange
            var certificate = GenerateSelfSignedCertificate();
            var httpHandler = new DefaultHttpClientHandler(new HttpClientHandlerOptions()
            {
                CertificateThumbprint = certificate.Thumbprint.HexStringToByteArray()
            });
            var certChain = new X509Chain();
            certChain.Build(certificate);

            //Act
            var actual =
                httpHandler.ServerCertificateCustomValidationCallback(null, null, certChain, System.Net.Security.SslPolicyErrors.None);

            //Assert
            Assert.True(actual);
        }

        private X509Certificate2 GenerateSelfSignedCertificate()
        {
            string secp256r1Oid = "1.2.840.10045.3.1.7";  //oid for prime256v1(7)  other identifier: secp256r1

            string subjectName = "Self-Signed-Cert-Example";

            var ecdsa = ECDsa.Create(ECCurve.CreateFromValue(secp256r1Oid));

            var certRequest = new CertificateRequest($"CN={subjectName}", ecdsa, HashAlgorithmName.SHA256);

            //add extensions to the request (just as an example)
            //add keyUsage
            certRequest.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, true));

            X509Certificate2 generatedCert = certRequest.CreateSelfSigned(DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddYears(10)); // generate the cert and sign!

            X509Certificate2 pfxGeneratedCert = new X509Certificate2(generatedCert.Export(X509ContentType.Pfx)); //has to be turned into pfx or Windows at least throws a security credentials not found during sslStream.connectAsClient or HttpClient request...

            return pfxGeneratedCert;
        }
    }
}
