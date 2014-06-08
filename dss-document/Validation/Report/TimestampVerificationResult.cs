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
using EU.Europa.EC.Markt.Dss.Validation.Report;
using EU.Europa.EC.Markt.Dss.Validation.X509;
using Org.BouncyCastle.Cms;
using Sharpen;
using System.Collections;

namespace EU.Europa.EC.Markt.Dss.Validation.Report
{
	/// <summary>Validation information of a timestamp.</summary>
	/// <remarks>Validation information of a timestamp.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class TimestampVerificationResult
	{
		private Result sameDigest;

		private Result certPathVerification = new Result();

		private string signatureAlgorithm;

		private string serialNumber;

		private DateTime creationTime;

		private string issuerName;

		/// <summary>The default constructor for TimestampVerificationResult.</summary>
		/// <remarks>The default constructor for TimestampVerificationResult.</remarks>
		public TimestampVerificationResult()
		{
		}

		/// <summary>The default constructor for TimestampVerificationResult.</summary>
		/// <remarks>The default constructor for TimestampVerificationResult.</remarks>
		public TimestampVerificationResult(TimestampToken token)
		{
			if (token != null && token.GetTimeStamp() != null)
			{
                IEnumerator signers = token.GetTimeStamp().ToCmsSignedData().GetSignerInfos
                    ().GetSigners().GetEnumerator();
                signers.MoveNext();
				signatureAlgorithm = ((SignerInformation)signers.Current).EncryptionAlgOid;
				serialNumber = token.GetTimeStamp().TimeStampInfo.SerialNumber.ToString();
				creationTime = token.GetTimeStamp().TimeStampInfo.GenTime;
				issuerName = token.GetSignerSubjectName().ToString();
			}
		}

		/// <param name="sameDigest">the sameDigest to set</param>
		public virtual void SetSameDigest(Result sameDigest)
		{
			this.sameDigest = sameDigest;
		}

		/// <returns>the sameDigest</returns>
		public virtual Result GetSameDigest()
		{
			return sameDigest;
		}

		/// <returns></returns>
		public virtual string GetSignatureAlgorithm()
		{
			return signatureAlgorithm;
		}

		/// <returns></returns>
		public virtual string GetSerialNumber()
		{
			return serialNumber;
		}

		/// <returns></returns>
		public virtual DateTime GetCreationTime()
		{
			return creationTime;
		}

		/// <returns></returns>
		public virtual string GetIssuerName()
		{
			return issuerName;
		}

		/// <returns></returns>
		public virtual Result GetCertPathUpToTrustedList()
		{
			return certPathVerification;
		}

		/// <returns>the certPathVerification</returns>
		public virtual Result GetCertPathVerification()
		{
			return certPathVerification;
		}

		/// <param name="certPathVerification">the certPathVerification to set</param>
		public virtual void SetCertPathVerification(Result certPathVerification)
		{
			this.certPathVerification = certPathVerification;
		}

		/// <param name="signatureAlgorithm">the signatureAlgorithm to set</param>
		public virtual void SetSignatureAlgorithm(string signatureAlgorithm)
		{
			this.signatureAlgorithm = signatureAlgorithm;
		}

		/// <param name="serialNumber">the serialNumber to set</param>
		public virtual void SetSerialNumber(string serialNumber)
		{
			this.serialNumber = serialNumber;
		}

		/// <param name="creationTime">the creationTime to set</param>
		public virtual void SetCreationTime(DateTime creationTime)
		{
			this.creationTime = creationTime;
		}

		/// <param name="issuerName">the issuerName to set</param>
		public virtual void SetIssuerName(string issuerName)
		{
			this.issuerName = issuerName;
		}
	}
}
