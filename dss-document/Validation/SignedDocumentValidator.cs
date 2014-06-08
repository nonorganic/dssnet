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
using EU.Europa.EC.Markt.Dss.Validation.Report;
using EU.Europa.EC.Markt.Dss.Validation.Tsl;
using EU.Europa.EC.Markt.Dss.Validation.X509;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <summary>Validate the signed document</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public abstract class SignedDocumentValidator
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(SignedDocumentValidator
			).FullName);

		private static readonly string SVC_INFO = "http://uri.etsi.org/TrstSvc/eSigDir-1999-93-EC-TrustedList/SvcInfoExt/";

        /// <returns>the document</returns>
        public Document Document { get; internal set; }

        /// <summary>Sets the Document containing the original content to sign, for detached signature scenarios
        /// 	</summary>
        /// <param name="externalContent">the externalContent to set</param>
        /// <returns>the externalContent</returns>
        public Document ExternalContent { get; set; }

        /// <param name="certificateVerifier">the certificateVerifier to set</param>
        public CertificateVerifier CertificateVerifier { internal get; set; }

		private Condition qcp = new PolicyIdCondition("0.4.0.1456.1.2");

		private Condition qcpplus = new PolicyIdCondition("0.4.0.1456.1.1");

		private Condition qccompliance = new QcStatementCondition(EtsiQCObjectIdentifiers
			.IdEtsiQcsQcCompliance);

		private Condition qcsscd = new QcStatementCondition(EtsiQCObjectIdentifiers.IdEtsiQcsQcSscd);

		private static readonly string MIMETYPE = "mimetype";

		private static readonly string MIMETYPE_ASIC_S = "application/vnd.etsi.asic-s+zip";

		private static readonly string SIGNATURES_XML = "META-INF/signatures.xml";

		private static readonly string SIGNATURES_P7S = "META-INF/signatures.p7s";

		//import eu.europa.ec.markt.dss.validation.asic.ASiCXMLDocumentValidator;
		//import eu.europa.ec.markt.dss.validation.pades.PDFDocumentValidator;
		//import eu.europa.ec.markt.dss.validation.xades.XMLDocumentValidator;
		/// <summary>Guess the document format and return an appropriate document</summary>
		/// <param name="document"></param>
		/// <returns></returns>
		/// <exception cref="System.IO.IOException"></exception>
		public static SignedDocumentValidator FromDocument(Document document)
		{
			InputStream input = null;
			try
			{
				input = new BufferedInputStream(document.OpenStream());
				input.Mark(5);
				byte[] preamble = new byte[5];
				int read = input.Read(preamble);
				input.Reset();
				if (read < 5)
				{
					throw new RuntimeException("Not a signed document");
				}
				string preambleString = Sharpen.Runtime.GetStringForBytes(preamble);
				if (Sharpen.Runtime.GetBytesForString(preambleString)[0] == unchecked((int)(0x30)
					))
				{
					try
					{
						return new CMSDocumentValidator(document);
					}
					catch (CmsException)
					{
						throw new IOException("Not a valid CAdES file");
					}
				}
				else
				{
					throw new RuntimeException("Document format not recognized/handled");
				}
			}
			finally
			{
				if (input != null)
				{
					try
					{
						input.Close();
					}
					catch (IOException)
					{
					}
				}
			}
		}

		/// <summary>Retrieves the signatures found in the document</summary>
		/// <returns>a list of AdvancedSignatures for validation purposes</returns>
		public abstract IList<AdvancedSignature> GetSignatures();

		/// <summary>Retrieves the number of signatures found in the document</summary>
		/// <returns>number of signatures</returns>
		public virtual int NumberOfSignatures()
		{
			IList<AdvancedSignature> signatures = this.GetSignatures();
			if (signatures == null)
			{
				return 0;
			}
			return signatures.Count;
		}

		protected internal virtual SignatureVerification[] VerifyCounterSignatures(AdvancedSignature
			 signature, ValidationContext ctx)
		{
			IList<AdvancedSignature> counterSignatures = signature.GetCounterSignatures();
			if (counterSignatures == null)
			{
				return null;
			}
			IList<SignatureVerification> counterSigVerifs = new AList<SignatureVerification>(
				);
			foreach (AdvancedSignature counterSig in counterSignatures)
			{
				Result counterSigResult = new Result(counterSig.CheckIntegrity(ExternalContent));
				string counterSigAlg = counterSig.GetSignatureAlgorithm();
				counterSigVerifs.AddItem(new SignatureVerification(counterSigResult, counterSigAlg
					));
			}
			SignatureVerification[] ret = new SignatureVerification[counterSigVerifs.Count];
			return Sharpen.Collections.ToArray(counterSigVerifs, ret);
		}

		/// <summary>Check the list of Timestamptoken.</summary>
		/// <remarks>Check the list of Timestamptoken. For each one a TimestampVerificationResult is produced
		/// 	</remarks>
		/// <param name="signature"></param>
		/// <param name="referenceTime"></param>
		/// <param name="ctx"></param>
		/// <param name="tstokens"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		protected internal virtual IList<TimestampVerificationResult> VerifyTimestamps(AdvancedSignature
			 signature, DateTime referenceTime, ValidationContext ctx, IList<TimestampToken>
			 tstokens, byte[] data)
		{
			IList<TimestampVerificationResult> tstokenVerifs = new AList<TimestampVerificationResult
				>();
			if (tstokens != null)
			{
				foreach (TimestampToken t in tstokens)
				{
					TimestampVerificationResult verif = new TimestampVerificationResult(t);
					try
					{
						if (t.MatchData(data))
						{
							verif.SetSameDigest(new Result(Result.ResultStatus.VALID, null));
						}
						else
						{
							verif.SetSameDigest(new Result(Result.ResultStatus.INVALID, "timestamp.dont.sign.data"
								));
						}
					}
					catch (NoSuchAlgorithmException)
					{
						verif.SetSameDigest(new Result(Result.ResultStatus.UNDETERMINED, "no.such.algoritm"
							));
					}
					CheckTimeStampCertPath(t, verif, ctx, signature);
					tstokenVerifs.AddItem(verif);
				}
			}
			return tstokenVerifs;
		}

		protected internal virtual SignatureLevelBES VerifyLevelBES(AdvancedSignature signature
			, DateTime referenceTime, ValidationContext ctx)
		{
			try
			{
				Result signingCertRefVerification = new Result();
				if (signature.GetSigningCertificate() != null)
				{
					signingCertRefVerification.SetStatus(Result.ResultStatus.VALID, null);
				}
				else
				{
					signingCertRefVerification.SetStatus(Result.ResultStatus.INVALID, "no.signing.certificate"
						);
				}
				SignatureVerification[] counterSigsVerif = VerifyCounterSignatures(signature, ctx
					);
				Result levelReached = new Result(signingCertRefVerification.IsValid());
				return new SignatureLevelBES(levelReached, signature, signingCertRefVerification, 
					counterSigsVerif, null);
			}
			catch (Exception)
			{
				return new SignatureLevelBES(new Result(Result.ResultStatus.INVALID, "exception.while.verifying"
					), null, new Result(Result.ResultStatus.INVALID, "exception.while.verifying"), null
					, null);
			}
		}

		protected internal virtual SignatureLevelEPES VerifyLevelEPES(AdvancedSignature signature
			, DateTime referenceTime, ValidationContext ctx)
		{
			try
			{
				PolicyValue policyValue = signature.GetPolicyId();
				Result levelReached = new Result(policyValue != null);
				return new SignatureLevelEPES(signature, levelReached);
			}
			catch (Exception)
			{
				return new SignatureLevelEPES(signature, new Result(Result.ResultStatus.INVALID, 
					"exception.while.verifying"));
			}
		}

		private Result ResultForTimestamps(IList<TimestampVerificationResult> signatureTimestampsVerification
			, Result levelReached)
		{
			if (signatureTimestampsVerification == null || signatureTimestampsVerification.IsEmpty
				())
			{
				levelReached.SetStatus(Result.ResultStatus.INVALID, "no.timestamp");
			}
			else
			{
				levelReached.SetStatus(Result.ResultStatus.VALID, null);
				foreach (TimestampVerificationResult result in signatureTimestampsVerification)
				{
					if (result.GetSameDigest().IsUndetermined())
					{
						levelReached.SetStatus(Result.ResultStatus.UNDETERMINED, "one.of.timestamp.digest.undetermined"
							);
					}
					else
					{
						if (result.GetSameDigest().IsInvalid())
						{
							levelReached.SetStatus(Result.ResultStatus.INVALID, "timestamp.dont.sign.data");
							break;
						}
					}
				}
			}
			return levelReached;
		}

		protected internal virtual SignatureLevelT VerifyLevelT(AdvancedSignature signature
			, DateTime referenceTime, ValidationContext ctx)
		{
			IList<TimestampToken> sigTimestamps = signature.GetSignatureTimestamps();
			IList<TimestampVerificationResult> results = VerifyTimestamps(signature, referenceTime
				, ctx, sigTimestamps, signature.GetSignatureTimestampData());
			return new SignatureLevelT(ResultForTimestamps(results, new Result()), results);
		}

		private bool EveryCertificateRefAreThere(ValidationContext ctx, IList<CertificateRef
			> refs, X509Certificate signingCert)
		{
			try
			{
				foreach (CertificateAndContext neededCert in ctx.GetNeededCertificates())
				{
					if (neededCert.GetCertificate().Equals(ctx.GetCertificate()))
					{
						LOG.Info("Don't check for the signing certificate");
						continue;
					}
					LOG.Info("Looking for the CertificateRef of " + neededCert);
					bool found = false;
					foreach (CertificateRef referencedCert in refs)
					{
						LOG.Info("Compare to " + referencedCert);						
                        byte[] hash = DigestUtilities.CalculateDigest
                            (referencedCert.GetDigestAlgorithm(),
                            neededCert.GetCertificate().GetEncoded());                            
						if (Arrays.Equals(hash, referencedCert.GetDigestValue()))
						{
							found = true;
							break;
						}
					}
					LOG.Info("Ref " + (found ? " found" : " not found"));
					if (!found)
					{
						return false;
					}
				}
				return true;
			}
			/*catch (NoSuchProviderException e)
			{
				throw new RuntimeException(e);
			}*/
			catch (CertificateEncodingException ex)
			{
				throw new RuntimeException(ex);
			}
			catch (NoSuchAlgorithmException e)
			{
				throw new RuntimeException(e);
			}
		}

		protected internal virtual SignatureLevelC VerifyLevelC(AdvancedSignature signature
			, DateTime referenceTime, ValidationContext ctx, bool rehashValues)
		{
			try
			{
				IList<CertificateRef> refs = signature.GetCertificateRefs();
				Result everyNeededCertAreInSignature = new Result();
				if (refs == null || refs.IsEmpty())
				{
					everyNeededCertAreInSignature.SetStatus(Result.ResultStatus.INVALID, "no.certificate.ref"
						);
				}
				else
				{
					if (EveryCertificateRefAreThere(ctx, refs, signature.GetSigningCertificate()))
					{
						everyNeededCertAreInSignature.SetStatus(Result.ResultStatus.VALID, null);
					}
					else
					{
						everyNeededCertAreInSignature.SetStatus(Result.ResultStatus.INVALID, "not.all.needed.certificate.ref"
							);
					}
				}
				LOG.Info("Every CertificateRef found " + everyNeededCertAreInSignature);
				IList<OCSPRef> ocspRefs = signature.GetOCSPRefs();
				IList<CRLRef> crlRefs = signature.GetCRLRefs();
				int refCount = 0;
				Result everyNeededRevocationData = new Result(Result.ResultStatus.VALID, null);
				refCount += ocspRefs.Count;
				refCount += crlRefs.Count;
				Result thereIsRevocationData = null;
				Result levelCReached = null;
				if (rehashValues)
				{
					if (!EveryOCSPValueOrRefAreThere(ctx, ocspRefs))
					{
						everyNeededRevocationData.SetStatus(Result.ResultStatus.INVALID, "not.all.needed.ocsp.ref"
							);
					}
					if (!EveryCRLValueOrRefAreThere(ctx, crlRefs))
					{
						everyNeededRevocationData.SetStatus(Result.ResultStatus.INVALID, "not.all.needed.crl.ref"
							);
					}
					levelCReached = new Result(everyNeededCertAreInSignature.GetStatus() == Result.ResultStatus
						.VALID && everyNeededRevocationData.GetStatus() == Result.ResultStatus.VALID);
					return new SignatureLevelC(levelCReached, everyNeededCertAreInSignature, everyNeededRevocationData
						);
				}
				else
				{
					thereIsRevocationData = new Result();
					if (refCount == 0)
					{
						thereIsRevocationData.SetStatus(Result.ResultStatus.INVALID, "no.revocation.data.reference"
							);
					}
					else
					{
						thereIsRevocationData.SetStatus(Result.ResultStatus.VALID, "at.least.one.reference"
							);
					}
					levelCReached = new Result(everyNeededCertAreInSignature.GetStatus() == Result.ResultStatus
						.VALID && thereIsRevocationData.GetStatus() == Result.ResultStatus.VALID);
					return new SignatureLevelC(levelCReached, everyNeededCertAreInSignature, thereIsRevocationData
						);
				}
			}
			catch (Exception)
			{
				return new SignatureLevelC(new Result(Result.ResultStatus.INVALID, "exception.while.verifying"
					), new Result(Result.ResultStatus.INVALID, "exception.while.verifying"), new Result
					(Result.ResultStatus.INVALID, "exception.while.verifying"));
			}
		}

		private void CheckTimeStampCertPath(TimestampToken t, TimestampVerificationResult
			 result, ValidationContext ctx, AdvancedSignature signature)
		{
			try
			{
				result.GetCertPathUpToTrustedList().SetStatus(Result.ResultStatus.INVALID, "cannot.reached.tsl"
					);
				ctx.ValidateTimestamp(t, signature.GetCertificateSource(), signature.GetCRLSource
					(), signature.GetOCSPSource());
				foreach (CertificateAndContext c in ctx.GetNeededCertificates())
				{
					if (c.GetCertificate().SubjectDN.Equals(t.GetSignerSubjectName()))
					{
						if (ctx.GetParentFromTrustedList(c) != null)
						{
							result.GetCertPathUpToTrustedList().SetStatus(Result.ResultStatus.VALID, null);
							break;
						}
					}
				}
			}
			catch (IOException)
			{
				result.GetCertPathUpToTrustedList().SetStatus(Result.ResultStatus.UNDETERMINED, "exception.while.verifying"
					);
			}
		}

		protected internal virtual SignatureLevelX VerifyLevelX(AdvancedSignature signature
			, DateTime referenceTime, ValidationContext ctx)
		{
			try
			{
				Result levelReached = new Result();
				levelReached.SetStatus(Result.ResultStatus.VALID, null);
				TimestampVerificationResult[] x1Results = null;
				TimestampVerificationResult[] x2Results = null;
				IList<TimestampToken> timestampX1 = signature.GetTimestampsX1();
				if (timestampX1 != null && !timestampX1.IsEmpty())
				{
					byte[] data = signature.GetTimestampX1Data();
					x1Results = new TimestampVerificationResult[timestampX1.Count];
					for (int i = 0; i < timestampX1.Count; i++)
					{
						try
						{
							TimestampToken t = timestampX1[i];
							x1Results[i] = new TimestampVerificationResult(t);
							if (!t.MatchData(data))
							{
								levelReached.SetStatus(Result.ResultStatus.INVALID, "timestamp.dont.sign.data");
								x1Results[i].SetSameDigest(new Result(Result.ResultStatus.INVALID, "timestamp.dont.sign.data"
									));
							}
							else
							{
								x1Results[i].SetSameDigest(new Result(Result.ResultStatus.VALID, null));
							}
							CheckTimeStampCertPath(t, x1Results[i], ctx, signature);
						}
						catch (NoSuchAlgorithmException)
						{
							levelReached.SetStatus(Result.ResultStatus.UNDETERMINED, "no.such.algoritm");
						}
					}
				}
				IList<TimestampToken> timestampX2 = signature.GetTimestampsX2();
				if (timestampX2 != null && !timestampX2.IsEmpty())
				{
					byte[] data = signature.GetTimestampX2Data();
					x2Results = new TimestampVerificationResult[timestampX2.Count];
					int i = 0;
					foreach (TimestampToken t in timestampX2)
					{
						try
						{
							x2Results[i] = new TimestampVerificationResult(t);
							if (!t.MatchData(data))
							{
								levelReached.SetStatus(Result.ResultStatus.INVALID, "timestamp.dont.sign.data");
								x2Results[i].SetSameDigest(new Result(Result.ResultStatus.INVALID, "timestamp.dont.sign.data"
									));
							}
							else
							{
								x2Results[i].SetSameDigest(new Result(Result.ResultStatus.VALID, null));
							}
							CheckTimeStampCertPath(t, x2Results[i], ctx, signature);
						}
						catch (NoSuchAlgorithmException)
						{
							levelReached.SetStatus(Result.ResultStatus.UNDETERMINED, "no.such.algoritm");
						}
					}
				}
				if ((timestampX1 == null || timestampX1.IsEmpty()) && (timestampX2 == null || timestampX2
					.IsEmpty()))
				{
					levelReached.SetStatus(Result.ResultStatus.INVALID, "no.timestamp");
				}
				return new SignatureLevelX(signature, levelReached, x1Results, x2Results);
			}
			catch (Exception)
			{
				return new SignatureLevelX(signature, new Result(Result.ResultStatus.INVALID, "exception.while.verifying"
					));
			}
		}

		/// <summary>
		/// For level -XL, every certificates values contained in the ValidationContext (except the SigningCertificate) must
		/// be in the CertificatesValues of the signature
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="certificates"></param>
		/// <param name="signingCert"></param>
		/// <returns></returns>
		protected internal virtual bool EveryCertificateValueAreThere(ValidationContext ctx
			, IList<X509Certificate> certificates, X509Certificate signingCert)
		{
			foreach (CertificateAndContext neededCert in ctx.GetNeededCertificates())
			{
				if (neededCert.GetCertificate().Equals(signingCert))
				{
					continue;
				}
				LOG.Info("Looking for the certificate ref of " + neededCert);
				bool found = false;
				foreach (X509Certificate referencedCert in certificates)
				{
					LOG.Info("Compare to " + referencedCert.SubjectDN);
					if (referencedCert.Equals(neededCert.GetCertificate()))
					{
						found = true;
						break;
					}
				}
				LOG.Info("Cert " + (found ? " found" : " not found"));
				if (!found)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// For level -XL or C, every BasicOcspResponse values contained in the ValidationContext must be in the
		/// RevocationValues or the RevocationRef of the signature
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="refs"></param>
		/// <param name="signingCert"></param>
		/// <returns></returns>
		protected internal virtual bool EveryOCSPValueOrRefAreThere<_T0>(ValidationContext
			 ctx, IList<_T0> ocspValuesOrRef)
		{
			foreach (BasicOcspResp ocspResp in ctx.GetNeededOCSPResp())
			{
				LOG.Info("Looking for the OcspResp produced at " + ocspResp.ProducedAt);
				bool found = false;
				foreach (object valueOrRef in ocspValuesOrRef)
				{
					if (valueOrRef is BasicOcspResp)
					{
						BasicOcspResp sigResp = (BasicOcspResp)valueOrRef;
						if (sigResp.Equals(ocspResp))
						{
							found = true;
							break;
						}
					}
					if (valueOrRef is OCSPRef)
					{
						OCSPRef @ref = (OCSPRef)valueOrRef;
						if (@ref.Match(ocspResp))
						{
							found = true;
							break;
						}
					}
				}
				LOG.Info("Ref " + (found ? " found" : " not found"));
				if (!found)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// For level -XL, every X509Crl values contained in the ValidationContext must be in the RevocationValues of the
		/// signature
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="refs"></param>
		/// <param name="signingCert"></param>
		/// <returns></returns>
		protected internal virtual bool EveryCRLValueOrRefAreThere<_T0>(ValidationContext
			 ctx, IList<_T0> crlValuesOrRef)
		{
			foreach (X509Crl crl in ctx.GetNeededCRL())
			{
				LOG.Info("Looking for CRL ref issued by " + crl.IssuerDN);
				bool found = false;
				foreach (object valueOrRef in crlValuesOrRef)
				{
					if (valueOrRef is X509Crl)
					{
						X509Crl sigCRL = (X509Crl)valueOrRef;
						if (sigCRL.Equals(crl))
						{
							found = true;
							break;
						}
					}
					if (valueOrRef is CRLRef)
					{
						CRLRef @ref = (CRLRef)valueOrRef;
						if (@ref.Match(crl))
						{
							found = true;
							break;
						}
					}
				}
				LOG.Info("Ref " + (found ? " found" : " not found"));
				if (!found)
				{
					return false;
				}
			}
			return true;
		}

		protected internal virtual SignatureLevelXL VerifyLevelXL(AdvancedSignature signature
			, DateTime referenceTime, ValidationContext ctx)
		{
			try
			{
				Result levelReached = new Result();
				Result everyNeededCertAreInSignature = new Result();
				everyNeededCertAreInSignature.SetStatus(Result.ResultStatus.VALID, null);
				Result everyNeededRevocationData = new Result();
				everyNeededRevocationData.SetStatus(Result.ResultStatus.VALID, null);
				IList<X509Certificate> refs = signature.GetCertificates();
				if (refs.IsEmpty())
				{
					LOG.Info("There is no certificate refs in the signature");
					everyNeededCertAreInSignature.SetStatus(Result.ResultStatus.INVALID, "no.certificate.value"
						);
				}
				else
				{
					if (!EveryCertificateValueAreThere(ctx, refs, signature.GetSigningCertificate()))
					{
						everyNeededCertAreInSignature.SetStatus(Result.ResultStatus.INVALID, "not.all.needed.certificate.value"
							);
					}
				}
				LOG.Info("Every certificate found " + everyNeededCertAreInSignature);
				int valueCount = 0;
				IList<BasicOcspResp> ocspValues = signature.GetOCSPs();
				if (ocspValues != null)
				{
					valueCount += ocspValues.Count;
					if (!EveryOCSPValueOrRefAreThere(ctx, ocspValues))
					{
						everyNeededRevocationData.SetStatus(Result.ResultStatus.INVALID, "not.all.needed.ocsp.value"
							);
					}
				}
				IList<X509Crl> crlValues = signature.GetCRLs();
				if (crlValues != null)
				{
					valueCount += crlValues.Count;
					if (!EveryCRLValueOrRefAreThere(ctx, crlValues))
					{
						everyNeededRevocationData.SetStatus(Result.ResultStatus.INVALID, "not.all.needed.crl.value"
							);
					}
				}
				if (valueCount == 0)
				{
					everyNeededRevocationData.SetStatus(Result.ResultStatus.INVALID, "no.revocation.data.value"
						);
				}
				levelReached.SetStatus((everyNeededCertAreInSignature.GetStatus() == Result.ResultStatus
					.VALID && everyNeededRevocationData.GetStatus() == Result.ResultStatus.VALID) ? 
					Result.ResultStatus.VALID : Result.ResultStatus.INVALID, null);
				return new SignatureLevelXL(levelReached, everyNeededCertAreInSignature, everyNeededRevocationData
					);
			}
			catch (Exception)
			{
				return new SignatureLevelXL(new Result(Result.ResultStatus.INVALID, "exception.while.verifying"
					), new Result(Result.ResultStatus.INVALID, "exception.while.verifying"), new Result
					(Result.ResultStatus.INVALID, "exception.while.verifying"));
			}
		}

		protected internal virtual SignatureLevelA VerifyLevelA(AdvancedSignature signature
			, DateTime referenceTime, ValidationContext ctx)
		{
			try
			{
				Result levelReached = new Result();
				IList<TimestampVerificationResult> verifs = null;
				try
				{
					IList<TimestampToken> timestamps = signature.GetArchiveTimestamps();
					verifs = VerifyTimestamps(signature, referenceTime, ctx, timestamps, signature.GetArchiveTimestampData
						(0, ExternalContent));
				}
				catch (IOException e)
				{
					LOG.Error("Error verifyind level A " + e.Message);
					levelReached.SetStatus(Result.ResultStatus.UNDETERMINED, "exception.while.verifying"
						);
				}
				return new SignatureLevelA(ResultForTimestamps(verifs, levelReached), verifs);
			}
			catch (Exception)
			{
				return new SignatureLevelA(new Result(Result.ResultStatus.INVALID, "exception.while.verifying"
					), null);
			}
		}

		protected internal virtual SignatureLevelLTV VerifyLevelLTV(AdvancedSignature signature
			, DateTime referenceTime, ValidationContext ctx)
		{
			return null;
		}

		protected internal virtual QualificationsVerification VerifyQualificationsElement
			(AdvancedSignature signature, DateTime referenceTime, ValidationContext ctx)
		{
			Result qCWithSSCD = new Result();
			Result qCNoSSCD = new Result();
			Result qCSSCDStatusAsInCert = new Result();
			Result qCForLegalPerson = new Result();
			IList<string> qualifiers = ctx.GetQualificationStatement();
			if (qualifiers != null)
			{
				qCWithSSCD = new Result(qualifiers.Contains(SVC_INFO + "QCWithSSCD"));
				qCNoSSCD = new Result(qualifiers.Contains(SVC_INFO + "QCNoSSCD"));
				qCSSCDStatusAsInCert = new Result(qualifiers.Contains(SVC_INFO + "QCSSCDStatusAsInCert"
					));
				qCForLegalPerson = new Result(qualifiers.Contains(SVC_INFO + "QCForLegalPerson"));
			}
			return new QualificationsVerification(qCWithSSCD, qCNoSSCD, qCSSCDStatusAsInCert, 
				qCForLegalPerson);
		}

		protected internal virtual QCStatementInformation VerifyQStatement(X509Certificate
			 certificate)
		{
			if (certificate != null)
			{
				Result qCPPresent = new Result(qcp.Check(new CertificateAndContext(certificate)));
				Result qCPPlusPresent = new Result(qcpplus.Check(new CertificateAndContext(certificate
					)));
				Result qcCompliancePresent = new Result(qccompliance.Check(new CertificateAndContext
					(certificate)));
				Result qcSCCDPresent = new Result(qcsscd.Check(new CertificateAndContext(certificate
					)));
				return new QCStatementInformation(qCPPresent, qCPPlusPresent, qcCompliancePresent
					, qcSCCDPresent);
			}
			else
			{
				return new QCStatementInformation(null, null, null, null);
			}
		}

		/// <summary>Main method for validating a signature</summary>
		/// <param name="signature"></param>
		/// <param name="referenceTime"></param>
		/// <returns>the report part pertaining to the signature</returns>
		protected internal virtual SignatureInformation ValidateSignature(AdvancedSignature
			 signature, DateTime referenceTime)
		{
			if (signature.GetSigningCertificate() == null)
			{
				LOG.Error("There is no signing certificate");
				return null;
			}
			QCStatementInformation qcStatementInformation = VerifyQStatement(signature.GetSigningCertificate
				());
			SignatureVerification signatureVerification = new SignatureVerification(new Result
				(signature.CheckIntegrity(this.ExternalContent)), signature.GetSignatureAlgorithm
				());
			try
			{
				ValidationContext ctx = CertificateVerifier.ValidateCertificate(signature.GetSigningCertificate
					(), referenceTime, signature.GetCertificateSource(), signature.GetCRLSource(), signature
					.GetOCSPSource());
				TrustedListInformation info = new TrustedListInformation(ctx.GetRelevantServiceInfo
					());
				CertPathRevocationAnalysis path = new CertPathRevocationAnalysis(ctx, info);
				SignatureLevelXL signatureLevelXL = VerifyLevelXL(signature, referenceTime, ctx);
				SignatureLevelC signatureLevelC = VerifyLevelC(signature, referenceTime, ctx, signatureLevelXL
					 != null ? signatureLevelXL.GetLevelReached().IsValid() : false);
				SignatureLevelAnalysis signatureLevelAnalysis = new SignatureLevelAnalysis(signature
					, VerifyLevelBES(signature, referenceTime, ctx), VerifyLevelEPES(signature, referenceTime
					, ctx), VerifyLevelT(signature, referenceTime, ctx), signatureLevelC, VerifyLevelX
					(signature, referenceTime, ctx), signatureLevelXL, VerifyLevelA(signature, referenceTime
					, ctx), VerifyLevelLTV(signature, referenceTime, ctx));
				QualificationsVerification qualificationsVerification = VerifyQualificationsElement
					(signature, referenceTime, ctx);
				SignatureInformation signatureInformation = new SignatureInformation(signatureVerification
					, path, signatureLevelAnalysis, qualificationsVerification, qcStatementInformation
					);
				return signatureInformation;
			}
			catch (IOException e)
			{
				throw new RuntimeException("Cannot read signature file", e);
			}
		}

		/// <summary>Validate the document and all its signatures</summary>
		/// <returns>the validation report</returns>
		public virtual ValidationReport ValidateDocument()
		{
			DateTime verificationTime = DateTime.Now;
			TimeInformation timeInformation = new TimeInformation(verificationTime);
			IList<SignatureInformation> signatureInformationList = new AList<SignatureInformation
				>();
			foreach (AdvancedSignature signature in GetSignatures())
			{
				signatureInformationList.AddItem(ValidateSignature(signature, signature.GetSigningTime
                    () == null ? DateTime.Now : signature.GetSigningTime().Value));
			}
			return new ValidationReport(timeInformation, signatureInformationList);
		}
	}
}
