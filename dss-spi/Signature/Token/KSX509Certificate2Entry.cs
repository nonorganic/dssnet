using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Pkix;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using SystemX509 = System.Security.Cryptography.X509Certificates;

namespace EU.Europa.EC.Markt.Dss.Signature.Token
{
    public class KSX509Certificate2Entry : IDssPrivateKeyEntry
    {        
        public SystemX509.X509Certificate2 Cert2 { get; set; }
        public ICollection<X509Certificate> Keystore { get; set; }

        public X509Certificate GetCertificate()
        {
            return DotNetUtilities.FromX509Certificate(Cert2);
        }

        public X509Certificate[] GetCertificateChain()
        {
            var list = new List<X509Certificate>();

            var chain = new SystemX509.X509Chain();

            chain.ChainPolicy.RevocationFlag = SystemX509.X509RevocationFlag.EntireChain;
            chain.ChainPolicy.RevocationMode = SystemX509.X509RevocationMode.Online;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 30);
            chain.ChainPolicy.VerificationFlags = SystemX509.X509VerificationFlags.NoFlag;

            if (chain.Build(this.Cert2) == true)
            {
                foreach (SystemX509.X509ChainElement element in chain.ChainElements)
                {
                    list.Add(DotNetUtilities.FromX509Certificate(element.Certificate));
                }
            }
            else
            {
                list.Add(DotNetUtilities.FromX509Certificate(this.Cert2));
            }
            
            return list.ToArray();
        }

        public SignatureAlgorithm GetSignatureAlgorithm()
        {
            if (Cert2.PrivateKey is RSACryptoServiceProvider)
            {
                return SignatureAlgorithm.RSA;
            }
            else if (Cert2.PrivateKey is DSACryptoServiceProvider)
            {
                return SignatureAlgorithm.DSA;
            }

            throw new ArgumentException("Unknown encryption algorithm " + Cert2.PrivateKey);
        }
    }
}
