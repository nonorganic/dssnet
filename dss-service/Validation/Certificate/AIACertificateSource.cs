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

using System.Collections.Generic;
using System.IO;
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.Https;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security.Certificates;
using System.Collections;

namespace EU.Europa.EC.Markt.Dss.Validation.Certificate
{
	/// <summary>Use the AIA attribute of a certificate to retrieve the issuer</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class AIACertificateSource : CertificateSource
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Certificate.AIACertificateSource
			).FullName);

		private X509Certificate certificate;

		private HTTPDataLoader httpDataLoader;

		/// <summary>The default constructor for AIACertificateSource.</summary>
		/// <remarks>The default constructor for AIACertificateSource.</remarks>
		public AIACertificateSource(X509Certificate certificate, HTTPDataLoader httpDataLoader
			)
		{
			this.certificate = certificate;
			this.httpDataLoader = httpDataLoader;
		}

        public virtual IList<CertificateAndContext> GetCertificateBySubjectName(X509Name
			 subjectName)
		{
			IList<CertificateAndContext> list = new AList<CertificateAndContext>();
			try
			{
				string url = GetAccessLocation(certificate, X509ObjectIdentifiers.IdADCAIssuers);
				if (url != null)
				{
                    X509CertificateParser parser = new X509CertificateParser();
                    X509Certificate cert = parser.ReadCertificate(httpDataLoader.Get(url));

					if (cert.SubjectDN.Equals(subjectName))
					{
						list.Add(new CertificateAndContext());
					}
				}
			}
			catch (CannotFetchDataException)
			{
                return new List<CertificateAndContext>();
			}
			catch (CertificateException)
			{
                return new List<CertificateAndContext>();
			}
			return list;
		}

		private string GetAccessLocation(X509Certificate certificate, DerObjectIdentifier
			 accessMethod)
		{
			try
			{
                //byte[] authInfoAccessExtensionValue = certificate.GetExtensionValue(X509Extensions
                //    .AuthorityInfoAccess);
                Asn1OctetString authInfoAccessExtensionValue = certificate.GetExtensionValue(X509Extensions
                    .AuthorityInfoAccess);
				if (null == authInfoAccessExtensionValue)
				{
					return null;
				}
				AuthorityInformationAccess authorityInformationAccess;
                //DerOctetString oct = (DerOctetString)(new Asn1InputStream(new MemoryStream
                //    (authInfoAccessExtensionValue)).ReadObject());
                DerOctetString oct = (DerOctetString)authInfoAccessExtensionValue;
                //authorityInformationAccess = new AuthorityInformationAccess((Asn1Sequence)new Asn1InputStream
                //    (oct.GetOctets()).ReadObject());
                authorityInformationAccess = AuthorityInformationAccess.GetInstance(oct);
				AccessDescription[] accessDescriptions = authorityInformationAccess.GetAccessDescriptions
					();
				foreach (AccessDescription accessDescription in accessDescriptions)
				{
					LOG.Info("access method: " + accessDescription.AccessMethod);
					bool correctAccessMethod = accessDescription.AccessMethod.Equals(accessMethod
						);
					if (!correctAccessMethod)
					{
						continue;
					}
					GeneralName gn = accessDescription.AccessLocation;
					if (gn.TagNo != GeneralName.UniformResourceIdentifier)
					{
						LOG.Info("not a uniform resource identifier");
						continue;
					}
					DerIA5String str = (DerIA5String)((DerTaggedObject)gn.ToAsn1Object()).GetObject();
					string accessLocation = str.GetString();
					LOG.Info("access location: " + accessLocation);
					return accessLocation;
				}
				return null;
			}
			catch (IOException e)
			{
				throw new RuntimeException("IO error: " + e.Message, e);
			}
		}
	}
}
