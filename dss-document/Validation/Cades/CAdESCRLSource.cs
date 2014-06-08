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
using Org.BouncyCastle.Asn1.Esf;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
//using Org.BouncyCastle.Jce.Provider;
//using Org.BouncyCastle.Util;
using Sharpen;
using Org.BouncyCastle.X509;
using System.Collections;
using Org.BouncyCastle.Security.Certificates;

namespace EU.Europa.EC.Markt.Dss.Validation.Cades
{
	/// <summary>CRLSource that retrieve information from a CAdES signature</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CAdESCRLSource : SignatureCRLSource
	{
		private CmsSignedData cmsSignedData;

		private SignerID signerId;

		/// <summary>The default constructor for CAdESCRLSource.</summary>
		/// <remarks>The default constructor for CAdESCRLSource.</remarks>
		/// <param name="encodedCMS"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESCRLSource(byte[] encodedCMS) : this(new CmsSignedData(encodedCMS))
		{
		}

		/// <summary>The default constructor for CAdESCRLSource.</summary>
		/// <remarks>The default constructor for CAdESCRLSource.</remarks>
		/// <param name="cms"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESCRLSource(CmsSignedData cms)
		{
            IEnumerator signers = cms.GetSignerInfos().GetSigners().GetEnumerator();
            signers.MoveNext();

            this.cmsSignedData = cms;
            this.signerId = ((SignerInformation)signers.Current).SignerID;
		}

		/// <summary>The default constructor for CAdESCRLSource.</summary>
		/// <remarks>The default constructor for CAdESCRLSource.</remarks>
		/// <param name="cms"></param>
		/// <param name="id"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESCRLSource(CmsSignedData cms, SignerID id)
		{
			this.cmsSignedData = cms;
			this.signerId = id;
		}

		public override IList<X509Crl> GetCRLsFromSignature()
		{
			IList<X509Crl> list = new AList<X509Crl>();
			try
			{
				// Add certificates contained in SignedData
                foreach (X509Crl crl in cmsSignedData.GetCrls
					("Collection").GetMatches(null))
				{					
					list.AddItem(crl);
				}
				// Add certificates in CAdES-XL certificate-values inside SignerInfo attribute if present
				SignerInformation si = cmsSignedData.GetSignerInfos().GetFirstSigner(signerId);
				if (si != null && si.UnsignedAttributes != null && si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues] != null)
				{
					RevocationValues revValues = RevocationValues.GetInstance(si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues].AttrValues[0]);
					foreach (CertificateList crlObj in revValues.GetCrlVals())
					{
						X509Crl crl = new X509Crl(crlObj);
						list.AddItem(crl);
					}
				}
			}
			/*catch (StoreException e)
			{
				throw new RuntimeException(e);
			}*/
			catch (CrlException e)
			{
				throw new RuntimeException(e);
			}
			return list;
		}
	}
}
