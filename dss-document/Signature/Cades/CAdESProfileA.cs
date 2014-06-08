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

using EU.Europa.EC.Markt.Dss.Validation.Cades;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Cms;
using System.Collections;

namespace EU.Europa.EC.Markt.Dss.Signature.Cades
{
	/// <summary>
	/// This class holds the CAdES-A signature profiles; it supports the later, over time _extension_ of a signature with
	/// id-aa-ets-archiveTimestampV2 attributes as defined in ETSI TS 101 733 V1.8.1, clause 6.4.1.
	/// </summary>
	/// <remarks>
	/// This class holds the CAdES-A signature profiles; it supports the later, over time _extension_ of a signature with
	/// id-aa-ets-archiveTimestampV2 attributes as defined in ETSI TS 101 733 V1.8.1, clause 6.4.1.
	/// "If the certificate-values and revocation-values attributes are not present in the CAdES-BES or CAdES-EPES, then they
	/// shall be added to the electronic signature prior to computing the archive time-stamp token." is the reason we extend
	/// from the XL profile.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CAdESProfileA : CAdESProfileXL
	{
		public static readonly DerObjectIdentifier id_aa_ets_archiveTimestampV2 = PkcsObjectIdentifiers.IdAAEtsArchiveTimestampV2;

		/// <exception cref="System.IO.IOException"></exception>
		protected internal override SignerInformation ExtendCMSSignature(CmsSignedData cmsSignedData
			, SignerInformation si, SignatureParameters parameters, Document originalDocument
			)
		{
			si = base.ExtendCMSSignature(cmsSignedData, si, parameters, originalDocument);
			CAdESSignature signature = new CAdESSignature(cmsSignedData, si);
			//IDictionary<DerObjectIdentifier, Attribute> unsignedAttrHash = si.UnsignedAttributes.ToDictionary();
            IDictionary unsignedAttrHash = si.UnsignedAttributes.ToDictionary();
			Attribute archiveTimeStamp = GetTimeStampAttribute(CAdESProfileA.id_aa_ets_archiveTimestampV2
				, GetSignatureTsa(), digestAlgorithm, signature.GetArchiveTimestampData(0, originalDocument
				));
			//unsignedAttrHash.Put(CAdESProfileA.id_aa_ets_archiveTimestampV2, archiveTimeStamp);
            unsignedAttrHash.Add(CAdESProfileA.id_aa_ets_archiveTimestampV2, archiveTimeStamp);
			SignerInformation newsi = SignerInformation.ReplaceUnsignedAttributes(si, new AttributeTable
				(unsignedAttrHash));
			return newsi;
		}
	}
}
