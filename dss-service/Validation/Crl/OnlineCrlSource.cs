/*
 * DSS - Digital Signature Services
 *
 * Copyright (C) 2011 European Commission, Directorate-General Internal Market and Services (DG MARKT), B-1049 Bruxelles/Brussel
 *
 * Developed by: 2011 ARHS Developments S.A. (rue Nicolas BovÃ© 2B, L-1253 Luxembourg) http://www.arhs-developments.com
 *
 * This file is part of the "DSS - Digital Signature Services" project.
 *
 * "DSS - Digital Signature Services" is free software: you can redistribute it and/or modify it under the terms of
 * the GNU Lesser General Public License as published by the Free Software Foundation, either version 2.1 of the
 * License, or (at your option) any later version.
 *
 * DSS is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License along with
 * "DSS - Digital Signature Services".  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Validation.Crl;
using EU.Europa.EC.Markt.Dss.Validation.Https;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.Security.Certificates;

namespace EU.Europa.EC.Markt.Dss.Validation.Crl
{
	/// <summary>Online CRL repository.</summary>
	/// <remarks>Online CRL repository. This CRL repository implementation will download the CRLs from the given CRL URIs.
	/// 	</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class OnlineCrlSource : ICrlSource
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(OnlineCrlSource).FullName
			);

        /// <summary>Set the HTTPDataLoader to use for query the CRL server</summary>
        /// <param name="urlDataLoader"></param>
        public HTTPDataLoader UrlDataLoader { get; set; }

        //jbonilla
        public string IntermediateAcUrl { get; set; }

        public OnlineCrlSource()
        {
            this.UrlDataLoader = new NetHttpDataLoader();
        }
        
        public virtual X509Crl FindCrl(X509Certificate certificate, X509Certificate issuerCertificate)
		{
			try
			{
				string crlURL = GetCrlUri(certificate);
				LOG.Info("CRL's URL for " + certificate.SubjectDN + " : " + crlURL);
				if (crlURL == null)
				{
					return null;
				}
				if (crlURL.StartsWith("http://") || crlURL.StartsWith("https://"))
				{
					return GetCrl(crlURL);
				}
				else
				{
					LOG.Info("We support only HTTP and HTTPS CRL's url, this url is " + crlURL);
					return null;
				}
			}
			catch (CrlException e)
			{
				LOG.Error("error parsing CRL: " + e.Message);
				throw new RuntimeException(e);
			}
			catch (UriFormatException e)
			{
				LOG.Error("error parsing CRL: " + e.Message);
				throw new RuntimeException(e);
			}
			catch (CertificateException e)
			{
				LOG.Error("error parsing CRL: " + e.Message);
				throw new RuntimeException(e);
			}
			/*catch (NoSuchProviderException e)
			{
				LOG.Error("error parsing CRL: " + e.Message);
				throw new RuntimeException(e);
			}
			catch (NoSuchParserException e)
			{
				LOG.Error("error parsing CRL: " + e.Message);
				throw new RuntimeException(e);
			}
			catch (StreamParsingException e)
			{
				LOG.Error("error parsing CRL: " + e.Message);
				throw new RuntimeException(e);
			}*/
		}

		/// <exception cref="Sharpen.CertificateException"></exception>
		/// <exception cref="Sharpen.CRLException"></exception>
		/// <exception cref="Sharpen.NoSuchProviderException"></exception>
		/// <exception cref="Org.BouncyCastle.X509.NoSuchParserException"></exception>
		/// <exception cref="Org.BouncyCastle.X509.Util.StreamParsingException"></exception>
		private X509Crl GetCrl(string downloadUrl)
		{
			if (downloadUrl != null)
			{
				try
				{
					InputStream input = UrlDataLoader.Get(downloadUrl);

                    X509CrlParser parser = new X509CrlParser();
                    X509Crl crl = parser.ReadCrl(input);
					LOG.Info("CRL size: " + crl.GetEncoded().Length + " bytes");
					return crl;
				}
				catch (CannotFetchDataException)
				{
					return null;
				}
			}
			else
			{
				return null;
			}
		}

		/// <summary>Gives back the CRL URI meta-data found within the given X509 certificate.
		/// 	</summary>
		/// <remarks>Gives back the CRL URI meta-data found within the given X509 certificate.
		/// 	</remarks>
		/// <param name="certificate">the X509 certificate.</param>
		/// <returns>the CRL URI, or <code>null</code> if the extension is not present.</returns>
		/// <exception cref="System.UriFormatException">System.UriFormatException</exception>
		public virtual string GetCrlUri(X509Certificate certificate)
		{
            //byte[] crlDistributionPointsValue = certificate.GetExtensionValue(X509Extensions.
            //    CrlDistributionPoints);
            Asn1OctetString crlDistributionPointsValue = certificate.GetExtensionValue(X509Extensions.
                CrlDistributionPoints);
			if (null == crlDistributionPointsValue)
			{
				return null;
			}
			Asn1Sequence seq;
			try
			{
				DerOctetString oct;
                //oct = (DEROctetString)(new ASN1InputStream(new ByteArrayInputStream(crlDistributionPointsValue
                //    )).ReadObject());
                oct = (DerOctetString)crlDistributionPointsValue;                
                seq = (Asn1Sequence)new Asn1InputStream(oct.GetOctets()).ReadObject();
			}
			catch (IOException e)
			{
				throw new RuntimeException("IO error: " + e.Message, e);
			}
			CrlDistPoint distPoint = CrlDistPoint.GetInstance(seq);
			DistributionPoint[] distributionPoints = distPoint.GetDistributionPoints();
			foreach (DistributionPoint distributionPoint in distributionPoints)
			{
                DistributionPointName distributionPointName = distributionPoint.DistributionPointName;
				if (DistributionPointName.FullName != distributionPointName.PointType)
				{
					continue;
				}
				GeneralNames generalNames = (GeneralNames)distributionPointName.Name;
				GeneralName[] names = generalNames.GetNames();
				foreach (GeneralName name in names)
				{
					if (name.TagNo != GeneralName.UniformResourceIdentifier)
					{
						LOG.Info("not a uniform resource identifier");
						continue;
					}
					string str = null;
					if (name.ToAsn1Object() is DerTaggedObject)
					{
						DerTaggedObject taggedObject = (DerTaggedObject)name.ToAsn1Object();
						DerIA5String derStr = DerIA5String.GetInstance(taggedObject.GetObject());
						str = derStr.GetString();
					}
					else
					{
						DerIA5String derStr = DerIA5String.GetInstance(name.ToAsn1Object());
						str = derStr.GetString();
					}
					if (str != null && (str.StartsWith("http://") || str.StartsWith("https://"))
                        && str.ToUpperInvariant().Contains("CRL")) //jbonilla - El URL del CRL para el BCE está en la tercera posición y solo se puede acceder desde HTTP.
					{
						return str;
					}
					else
					{
						LOG.Info("Supports only http:// and https:// protocol for CRL");
					}            
				}
			}

            //jbonilla
            #region BCE
            if (certificate.SubjectDN.ToString()
                .Contains("AC BANCO CENTRAL DEL ECUADOR"))
            {
                return this.IntermediateAcUrl;
            }
            #endregion

			return null;
		}
	}
}
