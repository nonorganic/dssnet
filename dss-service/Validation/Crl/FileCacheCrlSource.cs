using System;
using System.Collections.Generic;
using System.IO;
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Validation.Crl;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using System.Net;

namespace EU.Europa.EC.Markt.Dss.Validation.Crl
{
	/// <summary>CRLSource that retrieve information from a JDBC datasource</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class FileCacheCrlSource : ICrlSource
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Crl.FileCacheCrlSource
			).FullName);        

		public OnlineCrlSource CachedSource { get; set; }
		
		public FileCacheCrlSource()
		{     
		}		

		/// <exception cref="System.IO.IOException"></exception>
		public virtual X509Crl FindCrl(X509Certificate certificate, X509Certificate issuerCertificate)
		{
			OnlineCrlSource source = this.CachedSource ?? new OnlineCrlSource();
			string crlUrl = source.GetCrlUri(certificate);            

            if (crlUrl != null)
			{
                try
                {
                    CachedCRL cachedCrl = null;

                    string key = Hex.ToHexString(
                        DigestUtilities.CalculateDigest(DigestAlgorithm.SHA1.GetOid()
                        , Sharpen.Runtime.GetBytesForString(crlUrl)));

                    string pathCrl = Path.Combine("CRL", key);

                    DirectoryInfo dirCrl = new DirectoryInfo("CRL");

                    if (dirCrl.Exists)
                    {
                        FileInfo[] archivosCrl = dirCrl.GetFiles();

                        foreach (FileInfo a in archivosCrl)
                        {
                            if (a.Extension.Equals(".txt"))
                                continue;

                            if (a.Name.Equals(key))
                            {
                                cachedCrl = new CachedCRL()
                                {
                                    Crl = File.ReadAllBytes(a.FullName),
                                    Key = key
                                };

                                break;
                            }
                        }
                    }
                    else
                    {
                        dirCrl.Create();
                    }

                    if (cachedCrl == null)
                    {
                        LOG.Info("CRL not in cache");
                        return FindAndCacheCrlOnline(certificate, issuerCertificate, pathCrl);
                    }

                    X509CrlParser parser = new X509CrlParser();
                    X509Crl x509crl = parser.ReadCrl(cachedCrl.Crl);

                    if (x509crl.NextUpdate.Value.CompareTo(DateTime.Now) > 0)
                    {
                        LOG.Info("CRL in cache");
                        return x509crl;
                    }
                    else
                    {
                        LOG.Info("CRL expired");
                        return FindAndCacheCrlOnline(certificate, issuerCertificate, pathCrl);
                    }
                }
                catch (NoSuchAlgorithmException)
                {
                    LOG.Info("Cannot instantiate digest for algorithm SHA1 !?");
                }
                catch (CrlException)
                {
                    LOG.Info("Cannot serialize CRL");
                }
                catch (CertificateException)
                {
                    LOG.Info("Cannot instanciate X509 Factory");
                }
                catch (WebException)
                {
                    LOG.Info("Cannot connect to CRL URL");
                }
			}
			return null;
		}

        private X509Crl FindAndCacheCrlOnline(X509Certificate certificate
            , X509Certificate issuerCertificate, string pathCrl)
        {
            X509Crl originalCRL = CachedSource.FindCrl(certificate, issuerCertificate);

            if (originalCRL != null)
            {                
                File.WriteAllBytes(pathCrl, originalCRL.GetEncoded());
                return originalCRL;
            }
            else
            {
                return null;
            }
        }
    }
}