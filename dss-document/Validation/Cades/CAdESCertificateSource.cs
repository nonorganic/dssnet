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
using EU.Europa.EC.Markt.Dss.Validation.Ades;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
//using Org.BouncyCastle.Cert;
using Org.BouncyCastle.Cms;
//using Org.BouncyCastle.Jce.Provider;
//using Org.BouncyCastle.Util;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using System.Collections;
using Org.BouncyCastle.Security.Certificates;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation.Cades
{
	/// <summary>CertificateSource that retrieve items from a CAdES Signature</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CAdESCertificateSource : SignatureCertificateSource
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Cades.CAdESCertificateSource
			).FullName);

		private CmsSignedData cmsSignedData;

		private SignerID signerId;

		private bool onlyExtended = true;

		/// <summary>The default constructor for CAdESCertificateSource.</summary>
		/// <remarks>The default constructor for CAdESCertificateSource.</remarks>
		/// <param name="encodedCMS"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESCertificateSource(CmsSignedData cms)
		{
            IEnumerator signers = cms.GetSignerInfos().GetSigners().GetEnumerator();
            signers.MoveNext();

            this.cmsSignedData = cms;
            this.signerId = ((SignerInformation)signers.Current).SignerID;
            this.onlyExtended = false;
		}

		/// <summary>The default constructor for CAdESCertificateSource.</summary>
		/// <remarks>The default constructor for CAdESCertificateSource.</remarks>
		/// <param name="encodedCMS"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESCertificateSource(CmsSignedData cms, SignerID id, bool onlyExtended)
		{
			this.cmsSignedData = cms;
			this.signerId = id;
			this.onlyExtended = onlyExtended;
		}

		public override IList<X509Certificate> GetCertificates()
		{
			IList<X509Certificate> list = new AList<X509Certificate>();
			try
			{
				if (!onlyExtended)
				{
					LOG.Info(cmsSignedData.GetCertificates("Collection").GetMatches(null).Count + " certificate in collection"
						);
					//foreach (X509CertificateHolder ch in (ICollection<X509CertificateHolder>)cmsSignedData
                    foreach (X509Certificate ch in cmsSignedData
						.GetCertificates("Collection").GetMatches(null))
					{
						//X509Certificate c = new X509CertificateObject(ch.ToASN1Structure());
                        X509Certificate c = ch;
						LOG.Info("Certificate for subject " + c.SubjectDN);
						if (!list.Contains(c))
						{
							list.AddItem(c);
						}
					}
				}
				// Add certificates in CAdES-XL certificate-values inside SignerInfo attribute if present
				SignerInformation si = cmsSignedData.GetSignerInfos().GetFirstSigner(signerId);
				if (si != null && si.UnsignedAttributes != null && si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertValues] != null)
				{
					DerSequence seq = (DerSequence)si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertValues].AttrValues[0];
					for (int i = 0; i < seq.Count; i++)
					{
						X509CertificateStructure cs = X509CertificateStructure.GetInstance(seq[i]);
						//X509Certificate c = new X509CertificateObject(cs);
                        X509Certificate c = new X509Certificate(cs);
						if (!list.Contains(c))
						{
							list.AddItem(c);
						}
					}
				}
			}
			catch (CertificateParsingException e)
			{
				throw new RuntimeException(e);
			}
			/*catch (StoreException e)
			{
				throw new RuntimeException(e);
			}*/
			return list;
		}
	}
}
