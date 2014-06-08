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
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.Report;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Validation.Report
{
	/// <summary>Validation information for a Certificate Path (from a end user certificate to the Trusted List)
	/// 	</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CertPathRevocationAnalysis
	{
		private Result summary;

		private IList<CertificateVerification> certificatePathVerification = new AList<CertificateVerification
			>();

		private TrustedListInformation trustedListInformation;

		/// <summary>The default constructor for CertPathRevocationAnalysis.</summary>
		/// <remarks>The default constructor for CertPathRevocationAnalysis.</remarks>
		/// <param name="ctx"></param>
		/// <param name="info"></param>
		public CertPathRevocationAnalysis(ValidationContext ctx, TrustedListInformation info
			)
		{
			summary = new Result();
			this.trustedListInformation = info;
			if (ctx != null && ctx.GetNeededCertificates() != null)
			{
				foreach (CertificateAndContext cert in ctx.GetNeededCertificates())
				{
					CertificateVerification verif = new CertificateVerification(cert, ctx);
					certificatePathVerification.AddItem(verif);
				}
			}
			summary.SetStatus(Result.ResultStatus.VALID, null);
			if (certificatePathVerification != null)
			{
				foreach (CertificateVerification verif in certificatePathVerification)
				{
					if (verif.GetValidityPeriodVerification().IsInvalid())
					{
						summary.SetStatus(Result.ResultStatus.INVALID, "certificate.not.valid");
						break;
					}
					if (verif.GetCertificateStatus() != null)
					{
						if (verif.GetCertificateStatus().GetStatus() == CertificateValidity.REVOKED)
						{
							summary.SetStatus(Result.ResultStatus.INVALID, "certificate.revoked");
							break;
						}
						else
						{
							if (verif.GetCertificateStatus().GetStatus() == CertificateValidity.UNKNOWN || verif
								.GetCertificateStatus().GetStatus() == null)
							{
								summary.SetStatus(Result.ResultStatus.UNDETERMINED, "revocation.unknown");
							}
						}
					}
					else
					{
						summary.SetStatus(Result.ResultStatus.UNDETERMINED, "no.revocation.data");
					}
				}
			}
			if (trustedListInformation != null)
			{
				if (!trustedListInformation.IsServiceWasFound())
				{
					summary.SetStatus(Result.ResultStatus.INVALID, "no.trustedlist.service.was.found"
						);
				}
			}
			else
			{
				summary.SetStatus(Result.ResultStatus.INVALID, "no.trustedlist.service.was.found"
					);
			}
		}

		/// <returns>the summary</returns>
		public virtual Result GetSummary()
		{
			return summary;
		}

		/// <returns>the certificatePathVerification</returns>
		public virtual IList<CertificateVerification> GetCertificatePathVerification()
		{
			return certificatePathVerification;
		}

		/// <returns>the trustedListInformation</returns>
		public virtual TrustedListInformation GetTrustedListInformation()
		{
			return trustedListInformation;
		}

		/// <param name="summary">the summary to set</param>
		public virtual void SetSummary(Result summary)
		{
			this.summary = summary;
		}

		/// <param name="certificatePathVerification">the certificatePathVerification to set</param>
		public virtual void SetCertificatePathVerification(IList<CertificateVerification>
			 certificatePathVerification)
		{
			this.certificatePathVerification = certificatePathVerification;
		}

		/// <param name="trustedListInformation">the trustedListInformation to set</param>
		public virtual void SetTrustedListInformation(TrustedListInformation trustedListInformation
			)
		{
			this.trustedListInformation = trustedListInformation;
		}
	}
}
