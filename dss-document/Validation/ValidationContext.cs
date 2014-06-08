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
using System.Text;
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.Crl;
using EU.Europa.EC.Markt.Dss.Validation.Ocsp;
using EU.Europa.EC.Markt.Dss.Validation.Tsl;
using EU.Europa.EC.Markt.Dss.Validation.X509;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security.Certificates;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <summary>
	/// During the validation of a certificate, the software retrieve differents X509 artifact like Certificate, CRL and OCSP
	/// Response.
	/// </summary>
	/// <remarks>
	/// During the validation of a certificate, the software retrieve differents X509 artifact like Certificate, CRL and OCSP
	/// Response. The ValidationContext is a "cache" for one validation request that contains every object retrieved so far.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class ValidationContext
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.ValidationContext
			).FullName);

		private IList<BasicOcspResp> neededOCSPResp = new AList<BasicOcspResp>();

		private IList<X509Crl> neededCRL = new AList<X509Crl>();

		private IList<CertificateAndContext> neededCertificates = new AList<CertificateAndContext
			>();

		private X509Certificate certificate;

		private IDictionary<SignedToken, RevocationData> revocationInfo = new Dictionary<
			SignedToken, RevocationData>();

		private CertificateSource trustedListCertificatesSource;

		private IOcspSource ocspSource;

		private ICrlSource crlSource;

		private DateTime validationDate;

		private SignatureEventDelegate signatureEventDelegate = new SignatureEventDelegate
			();

		/// <summary>The default constructor for ValidationContextV2.</summary>
		/// <remarks>The default constructor for ValidationContextV2.</remarks>
		/// <param name="certificate">The certificate that will be validated.</param>
		public ValidationContext(X509Certificate certificate, DateTime validationDate)
		{
			if (certificate != null)
			{
				LOG.Info("New context for " + certificate.SubjectDN);
				this.certificate = certificate;
				AddNotYetVerifiedToken(new CertificateToken(new CertificateAndContext(certificate
					)));
			}
			this.validationDate = validationDate;
		}

		/// <summary>Return the certificate for which this ValidationContext has been created
		/// 	</summary>
		/// <returns>the certificate</returns>
		public virtual X509Certificate GetCertificate()
		{
			return certificate;
		}

		/// <returns>the validationDate</returns>
		public virtual DateTime GetValidationDate()
		{
			return validationDate;
		}

		/// <param name="trustedListCertificatesSource">the trustedListCertificatesSource to set
		/// 	</param>
		public virtual void SetTrustedListCertificatesSource(CertificateSource trustedListCertificatesSource
			)
		{
			this.trustedListCertificatesSource = trustedListCertificatesSource;
		}

		/// <param name="crlSource">the crlSource to set</param>
		public virtual void SetCrlSource(ICrlSource crlSource)
		{
			this.crlSource = crlSource;
		}

		/// <param name="ocspSource">the ocspSource to set</param>
		public virtual void SetOcspSource(IOcspSource ocspSource)
		{
			this.ocspSource = ocspSource;
		}

		internal virtual SignedToken GetOneNotYetVerifiedToken()
		{
			foreach (KeyValuePair<SignedToken, RevocationData> e in revocationInfo.EntrySet())
			{
				if (e.Value == null)
				{
					LOG.Info("=== Get token to validate " + e.Key);
					return e.Key;
				}
			}
			return null;
		}

		/// <param name="signedToken"></param>
		/// <param name="optionalSource"></param>
		/// <param name="validationDate"></param>
		/// <returns></returns>
		/// <exception cref="System.IO.IOException">An error occurs when accessing the CertificateSource
		/// 	</exception>
		internal virtual CertificateAndContext GetIssuerCertificate(SignedToken signedToken
			, CertificateSource optionalSource, DateTime validationDate)
		{
			if (signedToken.GetSignerSubjectName() == null)
			{
				return null;
			}
			IList<CertificateAndContext> list = new CompositeCertificateSource(trustedListCertificatesSource
				, optionalSource).GetCertificateBySubjectName(signedToken.GetSignerSubjectName()
				);
			if (list != null)
			{
				foreach (CertificateAndContext cert in list)
				{
					LOG.Info(cert.ToString());
					if (validationDate != null)
					{
						try
						{
							cert.GetCertificate().CheckValidity(validationDate);
						}
						catch (CertificateExpiredException)
						{
							LOG.Info("Was expired");
							continue;
						}
						catch (CertificateNotYetValidException)
						{
							LOG.Info("Was not yet valid");
							continue;
						}
						if (cert.GetCertificateSource() == CertificateSourceType.TRUSTED_LIST && cert.GetContext
							() != null)
						{
							ServiceInfo info = (ServiceInfo)cert.GetContext();
							if (info.GetStatusStartingDateAtReferenceTime() != null && validationDate.CompareTo( //jbonilla Before
								info.GetStatusStartingDateAtReferenceTime()) < 0)
							{
								LOG.Info("Was not valid in the TSL");
								continue;
							}
							else
							{
								if (info.GetStatusEndingDateAtReferenceTime() != null && validationDate.CompareTo(info //jbonilla After
									.GetStatusEndingDateAtReferenceTime()) > 0)
								{
									LOG.Info("Was not valid in the TSL");
									continue;
								}
							}
						}
					}
					if (signedToken.IsSignedBy(cert.GetCertificate()))
					{
						return cert;
					}
				}
			}
			return null;
		}

		internal virtual void AddNotYetVerifiedToken(SignedToken signedToken)
		{
			if (!revocationInfo.ContainsKey(signedToken))
			{
				LOG.Info("New token to validate " + signedToken + " hashCode " + signedToken.GetHashCode
					());
				revocationInfo.Put(signedToken, null);
				if (signedToken is CRLToken)
				{
					neededCRL.AddItem(((CRLToken)signedToken).GetX509crl());
				}
				else
				{
					if (signedToken is OCSPRespToken)
					{
						neededOCSPResp.AddItem(((OCSPRespToken)signedToken).GetOcspResp());
					}
					else
					{
						if (signedToken is CertificateToken)
						{
							bool found = false;
							CertificateAndContext newCert = ((CertificateToken)signedToken).GetCertificateAndContext
								();
							foreach (CertificateAndContext c in neededCertificates)
							{
								if (c.GetCertificate().Equals(newCert.GetCertificate()))
								{
									found = true;
									break;
								}
							}
							if (!found)
							{
								neededCertificates.AddItem(newCert);
							}
						}
					}
				}
			}
			else
			{
				LOG.Info("Token was already in list " + signedToken);
			}
		}

		internal virtual void Validate(SignedToken signedToken, RevocationData data)
		{
			if (data == null)
			{
				throw new ArgumentException("data cannot be null");
			}
			if (!revocationInfo.ContainsKey(signedToken))
			{
				throw new ArgumentException(signedToken + " must be a key of revocationInfo");
			}
			revocationInfo.Put(signedToken, data);
		}

		/// <summary>Validate the timestamp</summary>
		/// <param name="timestamp"></param>
		/// <param name="optionalSource"></param>
		/// <param name="optionalCRLSource"></param>
		/// <param name="optionalOCPSSource"></param>
		/// <exception cref="System.IO.IOException"></exception>
		public virtual void ValidateTimestamp(TimestampToken timestamp, CertificateSource
			 optionalSource, ICrlSource optionalCRLSource, IOcspSource optionalOCPSSource)
		{
			AddNotYetVerifiedToken(timestamp);
			Validate(timestamp.GetTimeStamp().TimeStampInfo.GenTime, new CompositeCertificateSource
				(timestamp.GetWrappedCertificateSource(), optionalSource), optionalCRLSource, optionalOCPSSource
				);
		}

		/// <summary>Build the validation context for the specific date</summary>
		/// <param name="validationDate"></param>
		/// <param name="optionalSource"></param>
		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Validate(DateTime validationDate, CertificateSource optionalSource
			, ICrlSource optionalCRLSource, IOcspSource optionalOCPSSource)
		{
			int previousSize = revocationInfo.Count;
			int previousVerified = VerifiedTokenCount();
			SignedToken signedToken = GetOneNotYetVerifiedToken();
			if (signedToken != null)
			{
				CertificateSource otherSource = optionalSource;
				if (signedToken != null)
				{
					otherSource = new CompositeCertificateSource(signedToken.GetWrappedCertificateSource
						(), optionalSource);
				}
				CertificateAndContext issuer = GetIssuerCertificate(signedToken, otherSource, validationDate
					);
				RevocationData data = null;
				if (issuer == null)
				{
					LOG.Warn("Don't found any issuer for token " + signedToken);
					data = new RevocationData(signedToken);
				}
				else
				{
					AddNotYetVerifiedToken(new CertificateToken(issuer));
					if (issuer.GetCertificate().SubjectDN.Equals(issuer.GetCertificate
						().IssuerDN))
					{
						SignedToken trustedToken = new CertificateToken(issuer);
						RevocationData noNeedToValidate = new RevocationData();
						// noNeedToValidate.setRevocationData(CertificateSourceType.TRUSTED_LIST);
						Validate(trustedToken, noNeedToValidate);
					}
					if (issuer.GetCertificateSource() == CertificateSourceType.TRUSTED_LIST)
					{
						SignedToken trustedToken = new CertificateToken(issuer);
						RevocationData noNeedToValidate = new RevocationData();
						noNeedToValidate.SetRevocationData(CertificateSourceType.TRUSTED_LIST);
						Validate(trustedToken, noNeedToValidate);
					}
					if (signedToken is CertificateToken)
					{
						CertificateToken ct = (CertificateToken)signedToken;
						CertificateStatus status = GetCertificateValidity(ct.GetCertificateAndContext(), 
							issuer, validationDate, optionalCRLSource, optionalOCPSSource);
						data = new RevocationData(signedToken);
						if (status != null)
						{
							data.SetRevocationData(status.StatusSource);
							if (status.StatusSource is X509Crl)
							{
								AddNotYetVerifiedToken(new CRLToken((X509Crl)status.StatusSource));
							}
							else
							{
								if (status.StatusSource is BasicOcspResp)
								{
									AddNotYetVerifiedToken(new OCSPRespToken((BasicOcspResp)status.StatusSource));
								}
							}
						}
						else
						{
							LOG.Warn("No status for " + signedToken);
						}
					}
					else
					{
						if (signedToken is CRLToken || signedToken is OCSPRespToken || signedToken is TimestampToken)
						{
							data = new RevocationData(signedToken);
							data.SetRevocationData(issuer);
						}
						else
						{
							throw new RuntimeException("Not supported token type " + signedToken.GetType().Name
								);
						}
					}
				}
				Validate(signedToken, data);
				LOG.Info(this.ToString());
				int newSize = revocationInfo.Count;
				int newVerified = VerifiedTokenCount();
				if (newSize != previousSize || newVerified != previousVerified)
				{
					Validate(validationDate, otherSource, optionalCRLSource, optionalOCPSSource);
				}
			}
		}

		internal virtual int VerifiedTokenCount()
		{
			int count = 0;
			foreach (KeyValuePair<SignedToken, RevocationData> e in revocationInfo.EntrySet())
			{
				if (e.Value != null)
				{
					count++;
				}
			}
			return count;
		}

		public override string ToString()
		{
			int count = 0;
			StringBuilder builder = new StringBuilder();
			foreach (KeyValuePair<SignedToken, RevocationData> e in revocationInfo.EntrySet())
			{
				if (e.Value != null)
				{
					builder.Append(e.Value);
					count++;
				}
				else
				{
					builder.Append(e.Key);
				}
				builder.Append(" ");
			}
			return "ValidationContext contains " + revocationInfo.Count + " SignedToken and "
				 + count + " of them have been verified. List : " + builder.ToString();
		}

		private CertificateStatus GetCertificateValidity(CertificateAndContext cert, CertificateAndContext
			 potentialIssuer, DateTime validationDate, ICrlSource optionalCRLSource, IOcspSource
			 optionalOCSPSource)
		{
			if (optionalCRLSource != null || optionalOCSPSource != null)
			{
				LOG.Info("Verify with offline services");
				OCSPAndCRLCertificateVerifier verifier = new OCSPAndCRLCertificateVerifier();
				verifier.SetCrlSource(optionalCRLSource);
				verifier.SetOcspSource(optionalOCSPSource);
				CertificateStatus status = verifier.Check(cert.GetCertificate(), potentialIssuer.
					GetCertificate(), validationDate);
				if (status != null)
				{
					return status;
				}
			}
			LOG.Info("Verify with online services");
			OCSPAndCRLCertificateVerifier onlineVerifier = new OCSPAndCRLCertificateVerifier(
				);
			onlineVerifier.SetCrlSource(crlSource);
			onlineVerifier.SetOcspSource(ocspSource);
			return onlineVerifier.Check(cert.GetCertificate(), potentialIssuer.GetCertificate
				(), validationDate);
		}

		/// <returns>the neededCRL</returns>
		public virtual IList<X509Crl> GetNeededCRL()
		{
			return neededCRL;
		}

		/// <returns>the neededOCSPResp</returns>
		public virtual IList<BasicOcspResp> GetNeededOCSPResp()
		{
			return neededOCSPResp;
		}

		/// <returns>the neededCertificates</returns>
		public virtual IList<CertificateAndContext> GetNeededCertificates()
		{
			return neededCertificates;
		}

		/// <summary>Finds the provided certificate's issuer in the context</summary>
		/// <param name="cert">The certificate whose issuer to find</param>
		/// <returns>the issuer's X509Certificate</returns>
		public virtual CertificateAndContext GetIssuerCertificateFromThisContext(CertificateAndContext
			 cert)
		{
			if (cert.GetCertificate().SubjectDN.Equals(cert.GetCertificate().IssuerDN))
			{
				return null;
			}
			foreach (CertificateAndContext c in neededCertificates)
			{
				if (c.GetCertificate().SubjectDN.Equals(cert.GetCertificate().IssuerDN))
				{
					return c;
				}
			}
			return null;
		}

		private bool ConcernsCertificate(X509Crl x509crl, CertificateAndContext cert)
		{
			return (x509crl.IssuerDN.Equals(cert.GetCertificate().IssuerDN));
		}

		private bool ConcernsCertificate(BasicOcspResp basicOcspResp, CertificateAndContext
			 cert)
		{
			CertificateAndContext issuerCertificate = GetIssuerCertificateFromThisContext(cert
				);
			if (issuerCertificate == null)
			{
				return false;
			}
			else
			{
				try
				{
					CertificateID matchingCertID = new CertificateID(CertificateID.HashSha1, issuerCertificate
						.GetCertificate(), cert.GetCertificate().SerialNumber);
					foreach (SingleResp resp in basicOcspResp.Responses)
					{
						if (resp.GetCertID().Equals(matchingCertID))
						{
							return true;
						}
					}
					return false;
				}
				catch (OcspException ex)
				{
					throw new RuntimeException(ex);
				}
			}
		}

		/// <summary>Returns the CRLs in the context which concern the provided certificate.</summary>
		/// <remarks>
		/// Returns the CRLs in the context which concern the provided certificate. It can happen there are more than one,
		/// even though this is unlikely.
		/// </remarks>
		/// <param name="cert">the X509 certificate</param>
		/// <returns>the list of CRLs related to the certificate</returns>
		public virtual IList<X509Crl> GetRelatedCRLs(CertificateAndContext cert)
		{
			IList<X509Crl> crls = new AList<X509Crl>();
			foreach (X509Crl crl in this.neededCRL)
			{
				if (ConcernsCertificate(crl, cert))
				{
					crls.AddItem(crl);
				}
			}
			return crls;
		}

		/// <summary>Returns the OCSP responses in the context which concern the provided certificate.
		/// 	</summary>
		/// <remarks>
		/// Returns the OCSP responses in the context which concern the provided certificate. It can happen there are more
		/// than one, even though this is unlikely.
		/// </remarks>
		/// <param name="cert">the X509 certificate</param>
		/// <returns>the list of OCSP responses related to the certificate</returns>
		/// <exception cref="Org.Bouncycastle.Ocsp.OcspException">Org.Bouncycastle.Ocsp.OcspException
		/// 	</exception>
		public virtual IList<BasicOcspResp> GetRelatedOCSPResp(CertificateAndContext cert
			)
		{
			IList<BasicOcspResp> ocspresps = new AList<BasicOcspResp>();
			foreach (BasicOcspResp ocspresp in this.neededOCSPResp)
			{
				if (this.ConcernsCertificate(ocspresp, cert))
				{
					ocspresps.AddItem(ocspresp);
				}
			}
			return ocspresps;
		}

		/// <param name="cert"></param>
		/// <returns></returns>
		public virtual CertificateStatus GetCertificateStatusFromContext(CertificateAndContext
			 cert)
		{
			if (cert.GetCertificateSource() == CertificateSourceType.TRUSTED_LIST)
			{
				CertificateStatus status = new CertificateStatus();
				status.Validity = CertificateValidity.VALID;
				status.StatusSourceType = ValidatorSourceType.TRUSTED_LIST;
				status.Certificate = cert.GetCertificate();
				return status;
			}
			CertificateAndContext issuer = GetIssuerCertificateFromThisContext(cert);
			if (issuer == null)
			{
				return null;
			}
			IOcspSource ocspSource = new ListOCSPSource(neededOCSPResp);
			ICrlSource crlSource = new ListCRLSource(neededCRL);
			OCSPAndCRLCertificateVerifier verifier = new OCSPAndCRLCertificateVerifier();
			verifier.SetCrlSource(crlSource);
			verifier.SetOcspSource(ocspSource);
			return verifier.Check(cert.GetCertificate(), issuer.GetCertificate(), GetValidationDate
				());
		}

		/// <summary>Retrieve the parent from the trusted list</summary>
		/// <param name="ctx"></param>
		/// <returns></returns>
		public virtual CertificateAndContext GetParentFromTrustedList(CertificateAndContext
			 ctx)
		{
			CertificateAndContext parent = ctx;
			while (GetIssuerCertificateFromThisContext(parent) != null)
			{
				parent = GetIssuerCertificateFromThisContext(parent);
				if (parent.GetCertificateSource() == CertificateSourceType.TRUSTED_LIST)
				{
					LOG.Info("Parent from TrustedList found " + parent);
					return parent;
				}
			}
			LOG.Warn("No issuer in the TrustedList for this certificate. The parent found is "
				 + parent);
			return null;
		}

		/// <summary>Return the ServiceInfo of the parent (in the Trusted List) of the certificate
		/// 	</summary>
		/// <returns></returns>
		public virtual ServiceInfo GetRelevantServiceInfo()
		{
			CertificateAndContext cert = new CertificateAndContext(GetCertificate());
			CertificateAndContext parent = GetParentFromTrustedList(cert);
			if (parent == null)
			{
				return null;
			}
			else
			{
				ServiceInfo info = (ServiceInfo)parent.GetContext();
				return info;
			}
		}

		/// <summary>Return the qualifications statement for the signing certificate</summary>
		/// <returns></returns>
		public virtual IList<string> GetQualificationStatement()
		{
			ServiceInfo info = GetRelevantServiceInfo();
			LOG.Info("Service Information " + info);
			if (info == null)
			{
				return null;
			}
			else
			{
				return info.GetQualifiers(new CertificateAndContext(GetCertificate()));
			}
		}

		/// <param name="listener"></param>
		public virtual void AddListener(SignatureEventListener listener)
		{
			signatureEventDelegate.AddListener(listener);
		}

		/// <param name="listener"></param>
		public virtual void RemoveListener(SignatureEventListener listener)
		{
			signatureEventDelegate.RemoveListener(listener);
		}
	}
}
