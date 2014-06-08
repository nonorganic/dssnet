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
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Signature.Cades;
//using Org.Apache.Commons.IO.Output;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using BcCms = Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
//using Org.BouncyCastle.Operator;
using Sharpen;
using Org.BouncyCastle.Security;
using System.Collections;

namespace EU.Europa.EC.Markt.Dss.Signature.Cades
{
	/// <summary>
	/// This class holds the CAdES-X signature profiles; it supports the inclusion of a combination of the unsigned
	/// attributes id-aa-ets-escTimeStamp, id-aa-ets-certCRLTimestamp, id-aa-ets-certValues, id-aa-ets-revocationValues as
	/// defined in ETSI TS 101 733 V1.8.1, clause 6.3.
	/// </summary>
	/// <remarks>
	/// This class holds the CAdES-X signature profiles; it supports the inclusion of a combination of the unsigned
	/// attributes id-aa-ets-escTimeStamp, id-aa-ets-certCRLTimestamp, id-aa-ets-certValues, id-aa-ets-revocationValues as
	/// defined in ETSI TS 101 733 V1.8.1, clause 6.3.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CAdESProfileX : CAdESProfileC
	{
        //jbonilla Se hereda de CAdESProfileT a través de CAdESProfileC
        //internal AlgorithmIdentifier digestAlgorithm = new DefaultDigestAlgorithmIdentifierFinder
        //    ().Find(new DefaultSignatureAlgorithmIdentifierFinder().Find("SHA1withRSA"));

		protected internal int extendedValidationType = 1;

		/// <summary>
		/// Gets the type of the CAdES-X signature (Type 1 with id-aa-ets-escTimeStamp or Type 2 with
		/// id-aa-ets-certCRLTimestamp)
		/// </summary>
		/// <returns>the extendedValidationType</returns>
		public virtual int GetExtendedValidationType()
		{
			return extendedValidationType;
		}

		/// <summary>
		/// Sets the type of the CAdES-X signature (Type 1 with id-aa-ets-escTimeStamp or Type 2 with
		/// id-aa-ets-certCRLTimestamp)
		/// </summary>
		/// <param name="extendedValidationType">to type to set, 1 or 2</param>
		public virtual void SetExtendedValidationType(int extendedValidationType)
		{
			if (extendedValidationType != 1 && extendedValidationType != 2)
			{
				throw new ArgumentException("The extended validation data type (CAdES-X type) shall be either 1 or 2"
					);
			}
			this.extendedValidationType = extendedValidationType;
		}

		/// <exception cref="System.IO.IOException"></exception>
		protected internal override SignerInformation ExtendCMSSignature(CmsSignedData signedData
			, SignerInformation si, SignatureParameters parameters, Document originalData)
		{
			si = base.ExtendCMSSignature(signedData, si, parameters, originalData);
			DerObjectIdentifier attributeId = null;
			ByteArrayOutputStream toTimestamp = new ByteArrayOutputStream();
			switch (GetExtendedValidationType())
			{
				case 1:
				{
					attributeId = PkcsObjectIdentifiers.IdAAEtsEscTimeStamp;
					toTimestamp.Write(si.GetSignature());
					// We don't include the outer SEQUENCE, only the attrType and attrValues as stated by the TS Â§6.3.5,
					// NOTE 2)
					toTimestamp.Write(si.UnsignedAttributes[PkcsObjectIdentifiers.IdAASignatureTimeStampToken]
						.AttrType.GetDerEncoded());
					toTimestamp.Write(si.UnsignedAttributes[PkcsObjectIdentifiers.IdAASignatureTimeStampToken]
						.AttrValues.GetDerEncoded());
					break;
				}

				case 2:
				{
					attributeId = PkcsObjectIdentifiers.IdAAEtsCertCrlTimestamp;
					break;
				}

				default:
				{
					throw new InvalidOperationException("CAdES-X Profile: Extended validation is set but no valid type (1 or 2)"
						);
				}
			}
			toTimestamp.Write(si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertificateRefs]
				.AttrType.GetDerEncoded());
			toTimestamp.Write(si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertificateRefs]
				.AttrValues.GetDerEncoded());
			toTimestamp.Write(si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationRefs]
				.AttrType.GetDerEncoded());
			toTimestamp.Write(si.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationRefs]
				.AttrValues.GetDerEncoded());
			//IDictionary<DerObjectIdentifier, Attribute> unsignedAttrHash = si.UnsignedAttributes.ToDictionary();
            IDictionary unsignedAttrHash = si.UnsignedAttributes.ToDictionary();
			BcCms.Attribute extendedTimeStamp = GetTimeStampAttribute(attributeId, GetSignatureTsa(
				), digestAlgorithm, toTimestamp.ToByteArray());
			//unsignedAttrHash.Put(attributeId, extendedTimeStamp);
            unsignedAttrHash.Add(attributeId, extendedTimeStamp);
			return SignerInformation.ReplaceUnsignedAttributes(si, new BcCms.AttributeTable(unsignedAttrHash
				));
		}
	}
}
