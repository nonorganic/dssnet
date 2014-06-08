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
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using System.Collections;

namespace EU.Europa.EC.Markt.Dss.Validation.Cades
{
	/// <summary>OCSPSource that retrieve information from a CAdESSignature.</summary>
	/// <remarks>OCSPSource that retrieve information from a CAdESSignature.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CAdESOCSPSource : SignatureOCSPSource
	{
		private CmsSignedData cmsSignedData;

		private SignerID signerId;

		/// <summary>The default constructor for CAdESOCSPSource.</summary>
		/// <remarks>The default constructor for CAdESOCSPSource.</remarks>
		/// <param name="encodedCMS"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESOCSPSource(byte[] encodedCMS) : this(new CmsSignedData(encodedCMS))
		{
		}

		/// <summary>The default constructor for CAdESOCSPSource.</summary>
		/// <remarks>The default constructor for CAdESOCSPSource.</remarks>
		/// <param name="encodedCMS"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESOCSPSource(CmsSignedData cms)
		{
            IEnumerator signers = cms.GetSignerInfos().GetSigners().GetEnumerator();
            signers.MoveNext();

            this.cmsSignedData = cms;
            this.signerId = ((SignerInformation)signers.Current).SignerID;
		}

		/// <summary>The default constructor for CAdESOCSPSource.</summary>
		/// <remarks>The default constructor for CAdESOCSPSource.</remarks>
		/// <param name="encodedCMS"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESOCSPSource(CmsSignedData cms, SignerID id)
		{
			this.cmsSignedData = cms;
			this.signerId = id;
		}

		public override IList<BasicOcspResp> GetOCSPResponsesFromSignature()
		{
			IList<BasicOcspResp> list = new AList<BasicOcspResp>();
			// Add certificates in CAdES-XL certificate-values inside SignerInfo attribute if present
			SignerInformation si = cmsSignedData.GetSignerInfos().GetFirstSigner(signerId);
			if (si != null && si.UnsignedAttributes != null && si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues] != null)
			{
				RevocationValues revValues = RevocationValues.GetInstance(si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationValues].AttrValues[0]);
				foreach (BasicOcspResponse ocspObj in revValues.GetOcspVals())
				{
					BasicOcspResp bOcspObj = new BasicOcspResp(ocspObj);
					list.AddItem(bOcspObj);
				}
			}
			return list;
		}
	}
}
