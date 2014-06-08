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

using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.Report;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.X509;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Validation.Report
{
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CertificateVerification
	{
		private CertificateAndContext certificate;

		private Result validityPeriodVerification;

		private SignatureVerification signatureVerification;

		private RevocationVerificationResult certificateStatus;

		/// <summary>The default constructor for CertificateVerification.</summary>
		/// <remarks>The default constructor for CertificateVerification.</remarks>
		public CertificateVerification()
		{
		}

		/// <summary>The default constructor for CertificateVerification.</summary>
		/// <remarks>The default constructor for CertificateVerification.</remarks>
		/// <param name="cert"></param>
		/// <param name="ctx"></param>
		public CertificateVerification(CertificateAndContext cert, ValidationContext ctx)
		{
			certificate = cert;
			if (cert != null)
			{
				try
				{
					cert.GetCertificate().CheckValidity(ctx.GetValidationDate());
					validityPeriodVerification = new Result(Result.ResultStatus.VALID, null);
				}
				catch (CertificateExpiredException)
				{
					validityPeriodVerification = new Result(Result.ResultStatus.INVALID, "certificate.expired"
						);
				}
				catch (CertificateNotYetValidException)
				{
					validityPeriodVerification = new Result(Result.ResultStatus.INVALID, "certificate.not.yet.valid"
						);
				}
				CertificateStatus status = ctx.GetCertificateStatusFromContext(cert);
				if (status != null)
				{
					certificateStatus = new RevocationVerificationResult(status);
				}
			}
		}

		/// <returns>the certificate</returns>
		public virtual X509Certificate GetCertificate()
		{
			return certificate.GetCertificate();
		}

		/// <returns>the validityPeriodVerification</returns>
		public virtual Result GetValidityPeriodVerification()
		{
			return validityPeriodVerification;
		}

		/// <returns>the signatureVerification</returns>
		public virtual SignatureVerification GetSignatureVerification()
		{
			return signatureVerification;
		}

		/// <returns>the certificateStatus</returns>
		public virtual RevocationVerificationResult GetCertificateStatus()
		{
			if (certificateStatus == null)
			{
				return new RevocationVerificationResult();
			}
			return certificateStatus;
		}

		/// <param name="certificate">the certificate to set</param>
		public virtual void SetCertificate(CertificateAndContext certificate)
		{
			this.certificate = certificate;
		}

		/// <param name="validityPeriodVerification">the validityPeriodVerification to set</param>
		public virtual void SetValidityPeriodVerification(Result validityPeriodVerification
			)
		{
			this.validityPeriodVerification = validityPeriodVerification;
		}

		/// <param name="signatureVerification">the signatureVerification to set</param>
		public virtual void SetSignatureVerification(SignatureVerification signatureVerification
			)
		{
			this.signatureVerification = signatureVerification;
		}

		/// <param name="certificateStatus">the certificateStatus to set</param>
		public virtual void SetCertificateStatus(RevocationVerificationResult certificateStatus
			)
		{
			this.certificateStatus = certificateStatus;
		}
	}
}
