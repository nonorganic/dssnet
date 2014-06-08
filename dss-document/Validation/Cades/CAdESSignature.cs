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
using System.IO;
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Cades;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.X509;
//using Org.Apache.Commons.IO.Output;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using BcCms = Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Esf;
//using Org.BouncyCastle.Asn1.Ess;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using BcX509 = Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
//using Org.BouncyCastle.Cms.Jcajce;
using Org.BouncyCastle.Ocsp;
//using Org.BouncyCastle.Operator;
using Org.BouncyCastle.Tsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using EU.Europa.EC.Markt.Dss.Validation.Ocsp;
using EU.Europa.EC.Markt.Dss.Validation.Crl;
using System.Collections;
using Org.BouncyCastle.Utilities.IO;
using Org.BouncyCastle.Utilities.Date;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation.Cades
{
	/// <summary>CAdES Signature class helper</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CAdESSignature : AdvancedSignature
	{
		public static readonly DerObjectIdentifier id_aa_ets_archiveTimestampV2 = PkcsObjectIdentifiers.IdAAEtsArchiveTimestampV2;

		private static ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Cades.CAdESSignature
			).FullName);

		private CmsSignedData cmsSignedData;

		private SignerInformation signerInformation;

		/// <summary>The default constructor for CAdESSignature.</summary>
		/// <remarks>The default constructor for CAdESSignature.</remarks>
		/// <param name="data"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESSignature(byte[] data) : this(new CmsSignedData(data))
		{
		}

		/// <summary>The default constructor for CAdESSignature.</summary>
		/// <remarks>The default constructor for CAdESSignature.</remarks>
		/// <param name="cms"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESSignature(CmsSignedData cms)
		{
            IEnumerator signers = cms.GetSignerInfos().GetSigners().GetEnumerator();
            signers.MoveNext();

            this.cmsSignedData = cms;
            this.signerInformation = (SignerInformation)signers.Current;
		}

		/// <summary>The default constructor for CAdESSignature.</summary>
		/// <remarks>The default constructor for CAdESSignature.</remarks>
		/// <param name="data"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESSignature(CmsSignedData cms, SignerInformation signerInformation)
		{
			this.cmsSignedData = cms;
			this.signerInformation = signerInformation;
		}

		/// <summary>The default constructor for CAdESSignature.</summary>
		/// <remarks>The default constructor for CAdESSignature.</remarks>
		/// <param name="data"></param>
		/// <exception cref="Org.Bouncycastle.Cms.CmsException">Org.Bouncycastle.Cms.CmsException
		/// 	</exception>
		public CAdESSignature(CmsSignedData cms, SignerID id) : this(cms, cms.GetSignerInfos().GetFirstSigner(id))
		{
		}

		public virtual SignatureFormat GetSignatureFormat()
		{
			return SignatureFormat.CAdES;
		}

		public virtual CertificateSource GetCertificateSource()
		{
			return new CAdESCertificateSource(cmsSignedData, signerInformation.SignerID, false
				);
		}

		public virtual CertificateSource GetExtendedCertificateSource()
		{
			return new CAdESCertificateSource(cmsSignedData, signerInformation.SignerID, true
				);
		}

		public virtual ICrlSource GetCRLSource()
		{
			return new CAdESCRLSource(cmsSignedData, signerInformation.SignerID);
		}

		public virtual IOcspSource GetOCSPSource()
		{
			return new CAdESOCSPSource(cmsSignedData, signerInformation.SignerID);
		}

		public virtual X509Certificate GetSigningCertificate()
		{
			LOG.Info("SignerInformation " + signerInformation.SignerID);
			ICollection<X509Certificate> certs = GetCertificates();
			foreach (X509Certificate cert in certs)
			{
				LOG.Info("Test match for certificate " + cert.SubjectDN.ToString());
				if (signerInformation.SignerID.Match(cert))
				{
					return cert;
				}
			}
			return null;
		}

		public virtual IList<X509Certificate> GetCertificates()
		{
			return ((CAdESCertificateSource)GetCertificateSource()).GetCertificates();
		}

		public virtual PolicyValue GetPolicyId()
		{
			if (signerInformation.SignedAttributes == null)
			{
				return null;
			}
			BcCms.Attribute sigPolicytAttr = signerInformation.SignedAttributes[PkcsObjectIdentifiers.IdAAEtsSigPolicyID];
			if (sigPolicytAttr == null)
			{
				return null;
			}
			if (sigPolicytAttr.AttrValues[0] is DerNull)
			{
				return new PolicyValue();
			}
			SignaturePolicyId sigPolicy = null;
			sigPolicy = SignaturePolicyId.GetInstance(sigPolicytAttr.AttrValues[0]);
			if (sigPolicy == null)
			{
				return new PolicyValue();
			}
			return new PolicyValue(sigPolicy.SigPolicyIdentifier.Id);
		}

		public virtual DateTimeObject GetSigningTime()
		{
			if (signerInformation.SignedAttributes != null && signerInformation.SignedAttributes
				[PkcsObjectIdentifiers.Pkcs9AtSigningTime] != null)
			{
				Asn1Set set = signerInformation.SignedAttributes[PkcsObjectIdentifiers.Pkcs9AtSigningTime]
					.AttrValues;
				try
				{
					object o = set[0];
					//if (o is ASN1UTCTime)
                    if (o is DerUtcTime)
					{
						//return ((ASN1UTCTime)o).GetDate();
                        return new DateTimeObject(
                            ((DerUtcTime)o).ToDateTime());
					}
                    if (o is BcX509.Time)
					{
                        return new DateTimeObject(
                            ((BcX509.Time)o).ToDateTime());
					}
					LOG.Error("Error when reading signing time. Unrecognized " + o.GetType());
				}
				catch (Exception ex)
				{
					LOG.Error("Error when reading signing time " + ex.Message);
					return null;
				}
			}
			return null;
		}

		/// <returns>the cmsSignedData</returns>
		public virtual CmsSignedData GetCmsSignedData()
		{
			return cmsSignedData;
		}

		public virtual string GetLocation()
		{
			return null;
		}

		public virtual string[] GetClaimedSignerRoles()
		{
			if (signerInformation.SignedAttributes == null)
			{
				return null;
			}
			BcCms.Attribute signerAttrAttr = signerInformation.SignedAttributes[PkcsObjectIdentifiers.IdAAEtsSignerAttr];
			if (signerAttrAttr == null)
			{
				return null;
			}
			SignerAttribute signerAttr = null;
			signerAttr = SignerAttribute.GetInstance(signerAttrAttr.AttrValues[0]);
			if (signerAttr == null)
			{
				return null;
			}
			string[] ret = new string[signerAttr.ClaimedAttributes.Count];
			for (int i = 0; i < signerAttr.ClaimedAttributes.Count; i++)
			{
				if (signerAttr.ClaimedAttributes[i] is DerOctetString)
				{
					ret[i] = Sharpen.Runtime.GetStringForBytes(((DerOctetString)signerAttr.ClaimedAttributes[i])
                        .GetOctets());
				}
				else
				{
					ret[i] = signerAttr.ClaimedAttributes[i].ToString();
				}
			}
			return ret;
		}

		private IList<TimestampToken> GetTimestampList(DerObjectIdentifier attrType, TimestampToken.TimestampType
			 timestampType)
		{
			if (signerInformation.UnsignedAttributes != null)
			{
				BcCms.Attribute timeStampAttr = signerInformation.UnsignedAttributes[attrType];
				if (timeStampAttr == null)
				{
					return null;
				}
				IList<TimestampToken> tstokens = new AList<TimestampToken>();
				foreach (Asn1Encodable value in timeStampAttr.AttrValues.ToArray())
				{
					try
					{
						TimeStampToken token = new TimeStampToken(new CmsSignedData(value.GetDerEncoded()
							));
						tstokens.AddItem(new TimestampToken(token, timestampType));
					}
					catch (Exception e)
					{
						throw new RuntimeException("Parsing error", e);
					}
				}
				return tstokens;
			}
			else
			{
				return null;
			}
		}

		protected internal virtual IList<TimestampToken> GetContentTimestamps()
		{
			return GetTimestampList(PkcsObjectIdentifiers.IdAAEtsContentTimestamp, TimestampToken.TimestampType
				.CONTENT_TIMESTAMP);
		}

		/// <exception cref="Sharpen.RuntimeException"></exception>
		public virtual IList<TimestampToken> GetSignatureTimestamps()
		{
			return GetTimestampList(PkcsObjectIdentifiers.IdAASignatureTimeStampToken, TimestampToken.TimestampType
				.SIGNATURE_TIMESTAMP);
		}

		public virtual IList<TimestampToken> GetTimestampsX1()
		{
			return GetTimestampList(PkcsObjectIdentifiers.IdAAEtsEscTimeStamp, TimestampToken.TimestampType
				.VALIDATION_DATA_TIMESTAMP);
		}

		public virtual IList<TimestampToken> GetTimestampsX2()
		{
			return GetTimestampList(PkcsObjectIdentifiers.IdAAEtsCertCrlTimestamp, TimestampToken.TimestampType
				.VALIDATION_DATA_REFSONLY_TIMESTAMP);
		}

		public virtual IList<TimestampToken> GetArchiveTimestamps()
		{
			return GetTimestampList(id_aa_ets_archiveTimestampV2, TimestampToken.TimestampType
				.ARCHIVE_TIMESTAMP);
		}

		public virtual string GetSignatureAlgorithm()
		{
			return signerInformation.EncryptionAlgOid;
		}

		public virtual bool CheckIntegrity(Document detachedDocument)
		{
            //TODO jbonilla Verifier?
            //JcaSimpleSignerInfoVerifierBuilder verifier = new JcaSimpleSignerInfoVerifierBuilder
            //    ();
			try
			{
				bool ret = false;
				SignerInformation si = null;
				if (detachedDocument != null)
				{
					// Recreate a SignerInformation with the content using a CMSSignedDataParser
					CmsSignedDataParser sp = new CmsSignedDataParser(new CmsTypedStream(detachedDocument
						.OpenStream()), cmsSignedData.GetEncoded());
					sp.GetSignedContent().Drain();
					si = sp.GetSignerInfos().GetFirstSigner(signerInformation.SignerID);
				}
				else
				{
					si = this.signerInformation;
				}
				//ret = si.Verify(verifier.Build(GetSigningCertificate()));
                ret = si.Verify(GetSigningCertificate());
				return ret;
			}
			/*catch (OperatorCreationException)
			{
				return false;
			}*/
			catch (CmsException)
			{
				return false;
			}
			catch (IOException)
			{
				return false;
			}
		}

		public virtual string GetContentType()
		{
			return signerInformation.ContentType.ToString();
		}

		/// <returns>the signerInformation</returns>
		public virtual SignerInformation GetSignerInformation()
		{
			return signerInformation;
		}

		public virtual IList<AdvancedSignature> GetCounterSignatures()
		{
			IList<AdvancedSignature> counterSigs = new AList<AdvancedSignature>();
			foreach (object o in this.signerInformation.GetCounterSignatures().GetSigners())
			{
				SignerInformation i = (SignerInformation)o;
				EU.Europa.EC.Markt.Dss.Validation.Cades.CAdESSignature info = new EU.Europa.EC.Markt.Dss.Validation.Cades.CAdESSignature
					(this.cmsSignedData, i.SignerID);
				counterSigs.AddItem(info);
			}
			return counterSigs;
		}

		public virtual IList<CertificateRef> GetCertificateRefs()
		{
			IList<CertificateRef> list = new AList<CertificateRef>();
			if (signerInformation.UnsignedAttributes != null)
			{
				BcCms.Attribute completeCertRefsAttr = signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertificateRefs];
				if (completeCertRefsAttr != null && completeCertRefsAttr.AttrValues.Count >
					 0)
				{
					DerSequence completeCertificateRefs = (DerSequence)completeCertRefsAttr.AttrValues[0];						
					for (int i1 = 0; i1 < completeCertificateRefs.Count; i1++)
					{                        
						OtherCertID otherCertId = OtherCertID.GetInstance(completeCertificateRefs[i1]);
						CertificateRef certId = new CertificateRef();
                        certId.SetDigestAlgorithm(otherCertId.OtherCertHash.HashAlgorithm.ObjectID.Id);

                        otherCertId.OtherCertHash.GetHashValue();

						certId.SetDigestValue(otherCertId.OtherCertHash.GetHashValue());
						if (otherCertId.IssuerSerial != null)
						{
							if (otherCertId.IssuerSerial.Issuer != null)
							{
								certId.SetIssuerName(otherCertId.IssuerSerial.Issuer.ToString());
							}
							if (otherCertId.IssuerSerial.Serial != null)
							{
								certId.SetIssuerSerial(otherCertId.IssuerSerial.Serial.ToString());
							}
						}
						list.AddItem(certId);
					}
				}
			}
			return list;
		}

		public virtual IList<CRLRef> GetCRLRefs()
		{
			IList<CRLRef> list = new AList<CRLRef>();
			if (signerInformation.UnsignedAttributes != null)
			{
				BcCms.Attribute completeRevocationRefsAttr = signerInformation.UnsignedAttributes
					[PkcsObjectIdentifiers.IdAAEtsRevocationRefs];
				if (completeRevocationRefsAttr != null && completeRevocationRefsAttr.AttrValues
					.Count > 0)
				{
					DerSequence completeCertificateRefs = (DerSequence)completeRevocationRefsAttr.AttrValues[0];						
					for (int i1 = 0; i1 < completeCertificateRefs.Count; i1++)
					{
						CrlOcspRef otherCertId = CrlOcspRef.GetInstance(completeCertificateRefs[i1]);
						foreach (CrlValidatedID id in otherCertId.CrlIDs.GetCrls())
						{
							list.AddItem(new CRLRef(id));
						}
					}
				}
			}
			return list;
		}

		public virtual IList<OCSPRef> GetOCSPRefs()
		{
			IList<OCSPRef> list = new AList<OCSPRef>();
			if (signerInformation.UnsignedAttributes != null)
			{
                BcCms.Attribute completeRevocationRefsAttr = signerInformation.UnsignedAttributes
                    [PkcsObjectIdentifiers.IdAAEtsRevocationRefs];
				if (completeRevocationRefsAttr != null && completeRevocationRefsAttr.AttrValues
					.Count > 0)
				{
					DerSequence completeRevocationRefs = (DerSequence)completeRevocationRefsAttr.AttrValues[0];
					for (int i1 = 0; i1 < completeRevocationRefs.Count; i1++)
					{
						CrlOcspRef otherCertId = CrlOcspRef.GetInstance(completeRevocationRefs[i1]);
						foreach (OcspResponsesID id in otherCertId.OcspIDs.GetOcspResponses())
						{
							list.AddItem(new OCSPRef(id, true));
						}
					}
				}
			}
			return list;
		}

		public virtual IList<X509Crl> GetCRLs()
		{
            return ((CAdESCRLSource)GetCRLSource()).GetCRLsFromSignature();
		}

		public virtual IList<BasicOcspResp> GetOCSPs()
		{
            return ((CAdESOCSPSource)GetOCSPSource()).GetOCSPResponsesFromSignature();
		}

		public virtual byte[] GetSignatureTimestampData()
		{
			return signerInformation.GetSignature();
		}

		public virtual byte[] GetTimestampX1Data()
		{
			try
			{
				ByteArrayOutputStream toTimestamp = new ByteArrayOutputStream();
				toTimestamp.Write(signerInformation.GetSignature());
				if (signerInformation.UnsignedAttributes != null)
				{
					toTimestamp.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAASignatureTimeStampToken].AttrType.GetDerEncoded());
					toTimestamp.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAASignatureTimeStampToken].AttrValues.GetDerEncoded());
				}
				toTimestamp.Write(GetTimestampX2Data());
				return toTimestamp.ToByteArray();
			}
			catch (IOException ex)
			{
				throw new RuntimeException(ex);
			}
		}

		public virtual byte[] GetTimestampX2Data()
		{
			try
			{
				ByteArrayOutputStream toTimestamp = new ByteArrayOutputStream();
				if (signerInformation.UnsignedAttributes != null)
				{
					toTimestamp.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertificateRefs].AttrType.GetDerEncoded());
					toTimestamp.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsCertificateRefs].AttrValues.GetDerEncoded());
					toTimestamp.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationRefs].AttrType.GetDerEncoded());
					toTimestamp.Write(signerInformation.UnsignedAttributes[PkcsObjectIdentifiers.IdAAEtsRevocationRefs].AttrValues.GetDerEncoded());
				}
				return toTimestamp.ToByteArray();
			}
			catch (IOException ex)
			{
				throw new RuntimeException(ex);
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual byte[] GetArchiveTimestampData(int index, Document originalDocument
			)
		{
			ByteArrayOutputStream toTimestamp = new ByteArrayOutputStream();
			BcCms.ContentInfo contentInfo = cmsSignedData.ContentInfo;
			BcCms.SignedData signedData = BcCms.SignedData.GetInstance(contentInfo.Content);
			// 5.4.1
			if (signedData.EncapContentInfo == null || signedData.EncapContentInfo.
				Content == null)
			{
				if (originalDocument != null)
				{
                    //jbonilla Hack para leer un InputStream en su totalidad.					
                    toTimestamp.Write(Streams.ReadAll(
                        originalDocument.OpenStream()));
				}
				else
				{
					throw new RuntimeException("Signature is detached and no original data provided."
						);
				}
			}
			else
			{
				BcCms.ContentInfo content = signedData.EncapContentInfo;
				DerOctetString octet = (DerOctetString)content.Content;
                BcCms.ContentInfo info2 = new BcCms.ContentInfo(new DerObjectIdentifier("1.2.840.113549.1.7.1"
					), new BerOctetString(octet.GetOctets()));
				toTimestamp.Write(info2.GetEncoded());
			}
			if (signedData.Certificates != null)
			{
				DerOutputStream output = new DerOutputStream(toTimestamp);
                output.WriteObject(signedData.Certificates);
				output.Close();
			}
			if (signedData.CRLs != null)
			{
                toTimestamp.Write(signedData.CRLs.GetEncoded());
			}
			if (signerInformation.UnsignedAttributes != null)
			{
				Asn1EncodableVector original = signerInformation.UnsignedAttributes.ToAsn1EncodableVector
					();
				IList<BcCms.Attribute> timeStampToRemove = GetTimeStampToRemove(index);
				Asn1EncodableVector filtered = new Asn1EncodableVector();
				for (int i = 0; i < original.Count; i++)
				{
					Asn1Encodable enc = original[i];
					if (!timeStampToRemove.Contains(enc))
					{
						filtered.Add(original[i]);
					}
				}
				SignerInformation filteredInfo = SignerInformation.ReplaceUnsignedAttributes(signerInformation
					, new BcCms.AttributeTable(filtered));
				toTimestamp.Write(filteredInfo.ToSignerInfo().GetEncoded());
			}
			return toTimestamp.ToByteArray();
		}

		private class AttributeTimeStampComparator : IComparer<BcCms.Attribute>
		{
            public virtual int Compare(BcCms.Attribute o1, BcCms.Attribute o2)
			{
				try
				{
					TimeStampToken t1 = new TimeStampToken(new CmsSignedData(o1.AttrValues
                        [0].ToAsn1Object().GetDerEncoded()));
					TimeStampToken t2 = new TimeStampToken(new CmsSignedData(o2.AttrValues
						[0].ToAsn1Object().GetDerEncoded()));
					return -t1.TimeStampInfo.GenTime.CompareTo(t2.TimeStampInfo.GenTime);
				}
				catch (Exception e)
				{
					throw new RuntimeException("Cannot read original ArchiveTimeStamp", e);
				}
			}

			internal AttributeTimeStampComparator(CAdESSignature _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly CAdESSignature _enclosing;
		}

        private IList<BcCms.Attribute> GetTimeStampToRemove(int archiveTimeStampToKeep)
		{
			IList<BcCms.Attribute> ts = new AList<BcCms.Attribute>();
			if (signerInformation.UnsignedAttributes != null)
			{
				Asn1EncodableVector v = signerInformation.UnsignedAttributes.GetAll(id_aa_ets_archiveTimestampV2
					);
				for (int i = 0; i < v.Count; i++)
				{
					Asn1Encodable enc = v[i];
					ts.AddItem((BcCms.Attribute)enc);
				}
				ts.Sort(new CAdESSignature.AttributeTimeStampComparator(this));
				for (int i_1 = 0; i_1 < archiveTimeStampToKeep; i_1++)
				{
					ts.Remove(0);
				}
			}
			return ts;
		}
	}
}
