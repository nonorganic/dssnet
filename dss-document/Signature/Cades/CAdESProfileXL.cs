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
using System.Collections.Generic;
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Signature.Cades;
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Cades;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using BcCms = Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Esf;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security.Certificates;
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
	public class CAdESProfileXL : CAdESProfileX
	{
		/// <exception cref="System.IO.IOException"></exception>
        //private IDictionary<DerObjectIdentifier, Asn1Encodable> ExtendUnsignedAttributes(IDictionary
        //    <DerObjectIdentifier, Asn1Encodable> unsignedAttrs, X509Certificate signingCertificate
        //    , DateTime signingDate, CertificateSource optionalCertificateSource)
        private IDictionary ExtendUnsignedAttributes(IDictionary unsignedAttrs
            , X509Certificate signingCertificate, DateTime signingDate
            , CertificateSource optionalCertificateSource)
		{
			ValidationContext validationContext = certificateVerifier.ValidateCertificate(signingCertificate
				, signingDate, optionalCertificateSource, null, null);
			try
			{
				IList<X509CertificateStructure> certificateValues = new AList<X509CertificateStructure
					>();
				AList<CertificateList> crlValues = new AList<CertificateList>();
				AList<BasicOcspResponse> ocspValues = new AList<BasicOcspResponse>();
				foreach (CertificateAndContext c in validationContext.GetNeededCertificates())
				{
					if (!c.Equals(signingCertificate))
					{
                        certificateValues.AddItem(X509CertificateStructure.GetInstance(((Asn1Sequence)Asn1Object.FromByteArray
                            (c.GetCertificate().GetEncoded()))));
					}
				}
				foreach (X509Crl relatedcrl in validationContext.GetNeededCRL())
				{                    
					crlValues.AddItem(CertificateList.GetInstance((Asn1Sequence)Asn1Object.FromByteArray(((X509Crl
						)relatedcrl).GetEncoded())));
				}
				foreach (BasicOcspResp relatedocspresp in validationContext.GetNeededOCSPResp())
				{                    
					ocspValues.AddItem((BasicOcspResponse.GetInstance((Asn1Sequence)Asn1Object.FromByteArray(
						relatedocspresp.GetEncoded()))));
				}
				CertificateList[] crlValuesArray = new CertificateList[crlValues.Count];
				BasicOcspResponse[] ocspValuesArray = new BasicOcspResponse[ocspValues.Count];
				RevocationValues revocationValues = new RevocationValues(Sharpen.Collections.ToArray
					(crlValues, crlValuesArray), Sharpen.Collections.ToArray(ocspValues, ocspValuesArray
					), null);
				//unsignedAttrs.Put(PkcsObjectIdentifiers.IdAAEtsRevocationValues, new Attribute
                unsignedAttrs.Add(PkcsObjectIdentifiers.IdAAEtsRevocationValues, new BcCms.Attribute
					(PkcsObjectIdentifiers.IdAAEtsRevocationValues, new DerSet(revocationValues))
					);
				X509CertificateStructure[] certValuesArray = new X509CertificateStructure[certificateValues
					.Count];
				//unsignedAttrs.Put(PkcsObjectIdentifiers.IdAAEtsCertValues, new Attribute(PkcsObjectIdentifiers.IdAAEtsCertValues, new DerSet(new DerSequence(Sharpen.Collections.ToArray(certificateValues
                unsignedAttrs.Add(PkcsObjectIdentifiers.IdAAEtsCertValues, new BcCms.Attribute(PkcsObjectIdentifiers.IdAAEtsCertValues, new DerSet(new DerSequence(Sharpen.Collections.ToArray(certificateValues
					, certValuesArray)))));
			}
			catch (CertificateEncodingException e)
			{
				throw new RuntimeException(e);
			}
			catch (CrlException e)
			{
				throw new RuntimeException(e);
			}
			return unsignedAttrs;
		}

		/// <exception cref="System.IO.IOException"></exception>
		protected internal override SignerInformation ExtendCMSSignature(CmsSignedData signedData
			, SignerInformation si, SignatureParameters parameters, Document originalData)
		{
			si = base.ExtendCMSSignature(signedData, si, parameters, originalData);
			//IDictionary<DerObjectIdentifier, Asn1Encodable> unsignedAttrs = si.UnsignedAttributes.ToDictionary();
            IDictionary unsignedAttrs = si.UnsignedAttributes.ToDictionary();
			CAdESSignature signature = new CAdESSignature(signedData, si.SignerID);
			DateTime signingTime = signature.GetSigningTime().Value;
			if (signingTime == null)
			{
				signingTime = parameters.SigningDate;
			}
			if (signingTime == null)
			{
				signingTime = DateTime.Now;
			}
			unsignedAttrs = ExtendUnsignedAttributes(unsignedAttrs, signature.GetSigningCertificate
				(), signingTime, signature.GetCertificateSource());
			SignerInformation newsi = SignerInformation.ReplaceUnsignedAttributes(si, new BcCms.AttributeTable
				(unsignedAttrs));
			return newsi;
		}
	}
}
