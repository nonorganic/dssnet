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
using EU.Europa.EC.Markt.Dss.Validation.Cades;
using EU.Europa.EC.Markt.Dss.Validation.X509;
//using Javax.Security.Auth.X500;
using Org.BouncyCastle.Tsp;
using Sharpen;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Security;

namespace EU.Europa.EC.Markt.Dss.Validation.X509
{
	/// <summary>SignedToken containing a TimeStamp.</summary>
	/// <remarks>SignedToken containing a TimeStamp.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class TimestampToken : SignedToken
	{
		/// <summary>
		/// Source of the timestamp
		/// <p>
		/// DISCLAIMER: Project owner DG-MARKT.
		/// </summary>
		/// <remarks>
		/// Source of the timestamp
		/// <p>
		/// DISCLAIMER: Project owner DG-MARKT.
		/// </remarks>
		/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
		/// 	</version>
		/// <author><a href="mailto:dgmarkt.Project-DSS@arhs-developments.com">ARHS Developments</a>
		/// 	</author>
		public enum TimestampType
		{
			CONTENT_TIMESTAMP,
			INDIVIDUAL_CONTENT_TIMESTAMP,
			SIGNATURE_TIMESTAMP,
			VALIDATION_DATA_REFSONLY_TIMESTAMP,
			VALIDATION_DATA_TIMESTAMP,
			ARCHIVE_TIMESTAMP
		}

		private TimeStampToken timeStamp;

		private TimestampToken.TimestampType timeStampType;

		/// <summary>The default constructor for TimestampToken.</summary>
		/// <remarks>The default constructor for TimestampToken.</remarks>
		/// <param name="timeStamp"></param>
		public TimestampToken(TimeStampToken timeStamp)
		{
			// CAdES: id-aa-ets-contentTimestamp, XAdES: AllDataObjectsTimeStamp, PAdES standard
			// timestamp
			// XAdES: IndividualDataObjectsTimeStamp
			// CAdES: id-aa-signatureTimeStampToken, XAdES: SignatureTimeStamp
			// CAdES: id-aa-ets-certCRLTimestamp, XAdES: RefsOnlyTimeStamp
			// CAdES: id-aa-ets-escTimeStamp, XAdES: SigAndRefsTimeStamp
			// CAdES: id-aa-ets-archiveTimestamp, XAdES: ArchiveTimeStamp, PAdES-LTV "document timestamp"
			this.timeStamp = timeStamp;
		}

		/// <summary>Constructor with an indication of the time-stamp type The default constructor for TimestampToken.
		/// 	</summary>
		/// <remarks>Constructor with an indication of the time-stamp type The default constructor for TimestampToken.
		/// 	</remarks>
		public TimestampToken(TimeStampToken timeStamp, TimestampToken.TimestampType type
			)
		{
			this.timeStamp = timeStamp;
			this.timeStampType = type;
		}

		public virtual X509Name GetSignerSubjectName()
		{
            ICollection<X509Certificate> certs = ((CAdESCertificateSource)GetWrappedCertificateSource()).GetCertificates
				();
			foreach (X509Certificate cert in certs)
			{
				if (timeStamp.SignerID.Match(cert))
				{
					return cert.SubjectDN;
				}
			}
			return null;
		}

		public virtual bool IsSignedBy(X509Certificate potentialIssuer)
		{
			try
			{
				//timeStamp.Validate(potentialIssuer, "BC");
                timeStamp.Validate(potentialIssuer);
				return true;
			}
			catch (CertificateExpiredException)
			{
				return false;
			}
			catch (CertificateNotYetValidException)
			{
				return false;
			}
			catch (TspValidationException)
			{
				return false;
			}
			/*catch (NoSuchProviderException e)
			{
				throw new RuntimeException(e);
			}*/
			catch (TspException)
			{
				return false;
			}
		}

		public virtual CertificateSource GetWrappedCertificateSource()
		{
			return new CAdESCertificateSource(timeStamp.ToCmsSignedData());
		}

		/// <returns>the timeStampType</returns>
		public virtual TimestampToken.TimestampType GetTimeStampType()
		{
			return timeStampType;
		}

		/// <returns>the timeStamp token</returns>
		public virtual TimeStampToken GetTimeStamp()
		{
			return timeStamp;
		}

		/// <summary>Check if the TimeStampToken matches the data</summary>
		/// <param name="data"></param>
		/// <returns>true if the data are verified by the TimeStampToken</returns>
		/// <exception cref="Sharpen.NoSuchAlgorithmException">Sharpen.NoSuchAlgorithmException
		/// 	</exception>
		public virtual bool MatchData(byte[] data)
		{
			string hashAlgorithm = timeStamp.TimeStampInfo.HashAlgorithm.ObjectID.Id;			
            byte[] computedDigest = DigestUtilities.CalculateDigest
                (hashAlgorithm, data);                
			return Arrays.Equals(computedDigest, timeStamp.TimeStampInfo.GetMessageImprintDigest
				());
		}

		/// <summary>Retrieve the timestamp generation date</summary>
		/// <returns></returns>
		public virtual DateTime GetGenTimeDate()
		{
			return timeStamp.TimeStampInfo.GenTime;
		}
	}
}
