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
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Report;
using Sharpen;
using Org.BouncyCastle.X509;

namespace EU.Europa.EC.Markt.Dss.Validation.Report
{
	/// <summary>Validation information for level BES</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class SignatureLevelBES : SignatureLevel
	{
		private X509Certificate signingCertificate;

		private Result signingCertRefVerification;

		private SignatureVerification[] counterSignaturesVerification;

		private IList<TimestampVerificationResult> timestampsVerification;

		private IList<X509Certificate> certificates;

		private DateTime signingTime;

		private string location;

		private string[] claimedSignerRole;

		private string contentType;

		/// <summary>The default constructor for SignatureLevelBES.</summary>
		/// <remarks>The default constructor for SignatureLevelBES.</remarks>
		/// <param name="name"></param>
		/// <param name="signature"></param>
		/// <param name="levelReached"></param>
		public SignatureLevelBES(Result levelReached, AdvancedSignature signature, Result
			 signingCertificateVerification, SignatureVerification[] counterSignatureVerification
			, IList<TimestampVerificationResult> timestampsVerification) : base(levelReached
			)
		{
			this.signingCertRefVerification = signingCertificateVerification;
			this.counterSignaturesVerification = counterSignatureVerification;
			this.timestampsVerification = timestampsVerification;
			if (signature != null)
			{
				certificates = signature.GetCertificates();
				signingCertificate = signature.GetSigningCertificate();
				signingTime = signature.GetSigningTime().Value;
				location = signature.GetLocation();
				claimedSignerRole = signature.GetClaimedSignerRoles();
				contentType = signature.GetContentType();
			}
		}

		/// <returns>the signingCertificateVerification</returns>
		public virtual Result GetSigningCertRefVerification()
		{
			return signingCertRefVerification;
		}

		/// <returns>the counterSignaturesVerification</returns>
		public virtual SignatureVerification[] GetCounterSignaturesVerification()
		{
			return counterSignaturesVerification;
		}

		/// <returns>the timestampsVerification</returns>
		public virtual IList<TimestampVerificationResult> GetTimestampsVerification()
		{
			return timestampsVerification;
		}

		/// <returns></returns>
		/// <seealso cref="EU.Europa.EC.Markt.Dss.Validation.AdvancedSignature.GetCertificates()
		/// 	">EU.Europa.EC.Markt.Dss.Validation.AdvancedSignature.GetCertificates()</seealso>
		public virtual IList<X509Certificate> GetCertificates()
		{
			return certificates;
		}

		/// <returns></returns>
		/// <seealso cref="EU.Europa.EC.Markt.Dss.Validation.AdvancedSignature.GetLocation()"
		/// 	>EU.Europa.EC.Markt.Dss.Validation.AdvancedSignature.GetLocation()</seealso>
		public virtual string GetLocation()
		{
			return location;
		}

		/// <returns></returns>
		/// <seealso cref="EU.Europa.EC.Markt.Dss.Validation.AdvancedSignature.GetContentType()
		/// 	">EU.Europa.EC.Markt.Dss.Validation.AdvancedSignature.GetContentType()</seealso>
		public virtual string GetContentType()
		{
			return contentType;
		}

		/// <returns></returns>
		/// <seealso cref="EU.Europa.EC.Markt.Dss.Validation.AdvancedSignature.GetClaimedSignerRoles()
		/// 	">EU.Europa.EC.Markt.Dss.Validation.AdvancedSignature.GetClaimedSignerRoles()</seealso>
		public virtual string[] GetClaimedSignerRoles()
		{
			return claimedSignerRole;
		}

		/// <returns></returns>
		public virtual X509Certificate GetSigningCertificate()
		{
			return signingCertificate;
		}

		/// <summary>The signing time of this signature</summary>
		/// <returns></returns>
		public virtual DateTime GetSigningTime()
		{
			return signingTime;
		}
	}
}
