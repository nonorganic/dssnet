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
//using Mono.Math;
using Org.BouncyCastle.Asn1.Esf;
using Org.BouncyCastle.Asn1.X500;
using Sharpen;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <summary>Reference to a X509Crl</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CRLRef
	{
		private X509Name crlIssuer;

		private DateTime crlIssuedTime;

		private BigInteger crlNumber;

		private string algorithm;

		private byte[] digestValue;

		/// <summary>The default constructor for CRLRef.</summary>
		/// <remarks>The default constructor for CRLRef.</remarks>
		public CRLRef()
		{
		}

		/// <summary>The default constructor for CRLRef.</summary>
		/// <remarks>The default constructor for CRLRef.</remarks>
		/// <param name="cmsRef"></param>
		/// <exception cref="Sharpen.ParseException">Sharpen.ParseException</exception>
		public CRLRef(CrlValidatedID cmsRef)
		{
			try
			{
				crlIssuer = cmsRef.CrlIdentifier.CrlIssuer;
				crlIssuedTime = cmsRef.CrlIdentifier.CrlIssuedTime;
				crlNumber = cmsRef.CrlIdentifier.CrlNumber;
				algorithm = cmsRef.CrlHash.HashAlgorithm.ObjectID.Id;
				digestValue = cmsRef.CrlHash.GetHashValue();
			}
			catch (ParseException ex)
			{
				throw new RuntimeException(ex);
			}
		}

		/// <param name="crl"></param>
		/// <returns></returns>
		public virtual bool Match(X509Crl crl)
		{
			try
			{				
                byte[] computedValue = DigestUtilities.CalculateDigest
                    (algorithm, crl.GetEncoded());             
				return Arrays.Equals(digestValue, computedValue);
			}
			catch (NoSuchAlgorithmException ex)
			{
				throw new RuntimeException("Maybe BouncyCastle provider is not installed ?", ex);
			}
			catch (CrlException ex)
			{
				throw new RuntimeException(ex);
			}
		}

		/// <returns></returns>
		public virtual X509Name GetCrlIssuer()
		{
			return crlIssuer;
		}

		/// <param name="crlIssuer"></param>
		public virtual void SetCrlIssuer(X509Name crlIssuer)
		{
			this.crlIssuer = crlIssuer;
		}

		/// <returns></returns>
		public virtual DateTime GetCrlIssuedTime()
		{
			return crlIssuedTime;
		}

		/// <param name="crlIssuedTime"></param>
		public virtual void SetCrlIssuedTime(DateTime crlIssuedTime)
		{
			this.crlIssuedTime = crlIssuedTime;
		}

		/// <returns></returns>
		public virtual BigInteger GetCrlNumber()
		{
			return crlNumber;
		}

		/// <param name="crlNumber"></param>
		public virtual void SetCrlNumber(BigInteger crlNumber)
		{
			this.crlNumber = crlNumber;
		}

		/// <returns></returns>
		public virtual string GetAlgorithm()
		{
			return algorithm;
		}

		/// <param name="algorithm"></param>
		public virtual void SetAlgorithm(string algorithm)
		{
			this.algorithm = algorithm;
		}

		/// <returns></returns>
		public virtual byte[] GetDigestValue()
		{
			return digestValue;
		}

		/// <param name="digestValue"></param>
		public virtual void SetDigestValue(byte[] digestValue)
		{
			this.digestValue = digestValue;
		}
	}
}
