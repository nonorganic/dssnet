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
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Signature.Cades;
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Cades;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
//using Mono.Math;
//using Org.Apache.Commons.Codec.Binary;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using BcCms = Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Esf;
//using Org.BouncyCastle.Asn1.Ess;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
//using Org.BouncyCastle.Jce.Provider;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using System.Collections.Generic;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;
using System.Collections;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Crypto;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Signature.Cades
{
	/// <summary>
	/// This class holds the CAdES-C signature profile; it supports the inclusion of the mandatory unsigned
	/// id-aa-ets-certificateRefs and id-aa-ets-revocationRefs attributes as specified in ETSI TS 101 733 V1.8.1, clauses
	/// 6.2.1 & 6.2.2.
	/// </summary>
	/// <remarks>
	/// This class holds the CAdES-C signature profile; it supports the inclusion of the mandatory unsigned
	/// id-aa-ets-certificateRefs and id-aa-ets-revocationRefs attributes as specified in ETSI TS 101 733 V1.8.1, clauses
	/// 6.2.1 & 6.2.2.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CAdESProfileC : CAdESProfileT
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(CAdESProfileC).FullName
			);

		protected internal CertificateVerifier certificateVerifier;

		/// <param name="certificateVerifier">the certificateVerifier to set</param>
		public virtual void SetCertificateVerifier(CertificateVerifier certificateVerifier
			)
		{
			this.certificateVerifier = certificateVerifier;
		}

		/// <summary>Create a reference to a X509Certificate</summary>
		/// <param name="cert"></param>
		/// <returns></returns>
		/// <exception cref="Sharpen.NoSuchAlgorithmException">Sharpen.NoSuchAlgorithmException
		/// 	</exception>
		/// <exception cref="Sharpen.CertificateEncodingException">Sharpen.CertificateEncodingException
		/// 	</exception>
		private OtherCertID MakeOtherCertID(X509Certificate cert)
		{
            byte[] d = DigestUtilities.CalculateDigest
                (X509ObjectIdentifiers.IdSha1, cert.GetEncoded());
			LOG.Info(new DerOctetString(d).ToString());
			OtherHash hash = new OtherHash(d);
			//OtherCertID othercertid = new OtherCertID(new DerSequence(hash.ToAsn1Object()));
            OtherCertID othercertid = new OtherCertID(hash);
            return othercertid;
		}

		/// <summary>Create a reference to a X509Crl</summary>
		/// <param name="crl"></param>
		/// <returns></returns>
		/// <exception cref="Sharpen.NoSuchAlgorithmException">Sharpen.NoSuchAlgorithmException
		/// 	</exception>
		/// <exception cref="Sharpen.CrlException">Sharpen.CrlException</exception>
		private CrlValidatedID MakeCrlValidatedID(X509Crl crl)
		{
            OtherHash hash = new OtherHash(DigestUtilities.CalculateDigest
                (X509ObjectIdentifiers.IdSha1, crl.GetEncoded()));                
			BigInteger crlnumber;
			CrlIdentifier crlid;
            DerObjectIdentifier crlExt = new DerObjectIdentifier("2.5.29.20");
            if (crl.GetExtensionValue(crlExt) != null)
			{
                //crlnumber = new DerInteger(crl.GetExtensionValue(crlExt)).GetPositiveValue();
                crlnumber = new DerInteger(crl.GetExtensionValue(crlExt).GetDerEncoded()).PositiveValue;
                //crlid = new CrlIdentifier(new X509Name(crl.IssuerDN.GetName()), new 
				crlid = new CrlIdentifier(crl.IssuerDN,
                    //new DerUtcTime(crl.ThisUpdate), crlnumber);
                    crl.ThisUpdate, crlnumber);
			}
			else
			{
				//crlid = new CrlIdentifier(new X509Name(crl.IssuerDN.GetName()), 
                crlid = new CrlIdentifier(crl.IssuerDN, 
                    //new DerUtcTime(crl.ThisUpdate));
                    crl.ThisUpdate);
			}
			CrlValidatedID crlvid = new CrlValidatedID(hash, crlid);
			return crlvid;
		}

		/// <summary>Create a reference on a OcspResp</summary>
		/// <param name="ocspResp"></param>
		/// <returns></returns>
		/// <exception cref="Sharpen.NoSuchAlgorithmException">Sharpen.NoSuchAlgorithmException
		/// 	</exception>
		/// <exception cref="Org.Bouncycastle.Ocsp.OcspException">Org.Bouncycastle.Ocsp.OcspException
		/// 	</exception>
		/// <exception cref="System.IO.IOException">System.IO.IOException</exception>
		private OcspResponsesID MakeOcspResponsesID(BasicOcspResp ocspResp)
		{			
			byte[] digestValue = DigestUtilities.CalculateDigest
                (X509ObjectIdentifiers.IdSha1, ocspResp.GetEncoded());                
			OtherHash hash = new OtherHash(digestValue);
			OcspResponsesID ocsprespid = new OcspResponsesID(new OcspIdentifier(ocspResp.ResponderId
				//.ToAsn1Object(), new DerGeneralizedTime(ocspResp.ProducedAt)), hash);
                .ToAsn1Object(), ocspResp.ProducedAt), hash);
			LOG.Info("Incorporate OcspResponseId[hash=" + Hex.ToHexString(digestValue) + 
				",producedAt=" + ocspResp.ProducedAt);
			return ocsprespid;
		}

		/// <exception cref="System.IO.IOException"></exception>
        //private IDictionary<DerObjectIdentifier, Asn1Encodable> ExtendUnsignedAttributes(IDictionary
        //    <DerObjectIdentifier, Asn1Encodable> unsignedAttrs, X509Certificate signingCertificate
        //    , SignatureParameters parameters, DateTime signingTime, CertificateSource optionalCertificateSource
        //    )
        private IDictionary ExtendUnsignedAttributes(IDictionary unsignedAttrs, X509Certificate signingCertificate
			, SignatureParameters parameters, DateTime signingTime, CertificateSource optionalCertificateSource
			)
		{
			ValidationContext validationContext = certificateVerifier.ValidateCertificate(signingCertificate
				, signingTime, new CompositeCertificateSource(new ListCertificateSource(parameters
				.CertificateChain), optionalCertificateSource), null, null);
			try
			{
				AList<OtherCertID> completeCertificateRefs = new AList<OtherCertID>();
				AList<CrlOcspRef> completeRevocationRefs = new AList<CrlOcspRef>();
				foreach (CertificateAndContext c in validationContext.GetNeededCertificates())
				{
					if (!c.Equals(signingCertificate))
					{
						completeCertificateRefs.AddItem(MakeOtherCertID(c.GetCertificate()));
					}
					// certificateValues.add(new X509CertificateStructure((Asn1Sequence) Asn1Object.fromByteArray(c
					// .getCertificate().getEncoded())));
					AList<CrlValidatedID> crlListIdValues = new AList<CrlValidatedID>();
					AList<OcspResponsesID> ocspListIDValues = new AList<OcspResponsesID>();
					foreach (X509Crl relatedcrl in validationContext.GetRelatedCRLs(c))
					{
						crlListIdValues.AddItem(MakeCrlValidatedID((X509Crl)relatedcrl));
					}
					foreach (BasicOcspResp relatedocspresp in validationContext.GetRelatedOCSPResp(c))
					{
						ocspListIDValues.AddItem(MakeOcspResponsesID(relatedocspresp));
					}
					CrlValidatedID[] crlListIdArray = new CrlValidatedID[crlListIdValues.Count];
					OcspResponsesID[] ocspListIDArray = new OcspResponsesID[ocspListIDValues.Count];
					completeRevocationRefs.AddItem(new CrlOcspRef(new CrlListID(Sharpen.Collections.ToArray
						(crlListIdValues, crlListIdArray)), new OcspListID(Sharpen.Collections.ToArray(ocspListIDValues
						, ocspListIDArray)), null));
				}
				OtherCertID[] otherCertIDArray = new OtherCertID[completeCertificateRefs.Count];
				CrlOcspRef[] crlOcspRefArray = new CrlOcspRef[completeRevocationRefs.Count];
				//unsignedAttrs.Put(PkcsObjectIdentifiers.IdAAEtsCertificateRefs, new Attribute(
                unsignedAttrs.Add(PkcsObjectIdentifiers.IdAAEtsCertificateRefs, new BcCms.Attribute(
					PkcsObjectIdentifiers.IdAAEtsCertificateRefs, new DerSet(new DerSequence(Sharpen.Collections.ToArray
					(completeCertificateRefs, otherCertIDArray)))));
                //unsignedAttrs.Put(PkcsObjectIdentifiers.IdAAEtsRevocationRefs, new Attribute(PkcsObjectIdentifiers.IdAAEtsRevocationRefs, new DerSet(new DerSequence(Sharpen.Collections.ToArray
				unsignedAttrs.Add(PkcsObjectIdentifiers.IdAAEtsRevocationRefs, new BcCms.Attribute(PkcsObjectIdentifiers.IdAAEtsRevocationRefs, new DerSet(new DerSequence(Sharpen.Collections.ToArray
					(completeRevocationRefs, crlOcspRefArray)))));
			}
			catch (NoSuchAlgorithmException e)
			{
				throw new RuntimeException(e);
			}
			catch (CertificateEncodingException e)
			{
				throw new RuntimeException(e);
			}
			catch (OcspException e)
			{
				throw new RuntimeException(e);
			}
			catch (IOException e)
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
			SignerInformation newSi = base.ExtendCMSSignature(signedData, si, parameters, originalData
				);
			//IDictionary<DerObjectIdentifier, Asn1Encodable> unsignedAttrs = newSi.UnsignedAttributes.ToDictionary();
            IDictionary unsignedAttrs = newSi.UnsignedAttributes.ToDictionary();
			CAdESSignature signature = new CAdESSignature(signedData, si.SignerID);
			unsignedAttrs = ExtendUnsignedAttributes(unsignedAttrs, signature.GetSigningCertificate
				(), parameters, signature.GetSigningTime().Value, signature.GetCertificateSource());
			return SignerInformation.ReplaceUnsignedAttributes(newSi, new BcCms.AttributeTable(unsignedAttrs
				));
		}
	}
}
