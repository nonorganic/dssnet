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
using Org.BouncyCastle.Utilities.Encoders;
//using Org.Apache.Commons.Codec.Binary;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <summary>Reference a Certificate</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CertificateRef
	{
		private string digestAlgorithm;

		private byte[] digestValue;

		private string issuerName;

		private string issuerSerial;

		public override string ToString()
		{
			return "CertificateRef[issuerName=" + issuerName + ",issuerSerial=" + issuerSerial
				 + ",digest=" + Hex.ToHexString(digestValue) + "]";
		}

		/// <returns></returns>
		public virtual string GetDigestAlgorithm()
		{
			return digestAlgorithm;
		}

		/// <param name="digestAlgorithm"></param>
		public virtual void SetDigestAlgorithm(string digestAlgorithm)
		{
			this.digestAlgorithm = digestAlgorithm;
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

		/// <returns></returns>
		public virtual string GetIssuerName()
		{
			return issuerName;
		}

		/// <param name="issuerName"></param>
		public virtual void SetIssuerName(string issuerName)
		{
			this.issuerName = issuerName;
		}

		/// <returns></returns>
		public virtual string GetIssuerSerial()
		{
			return issuerSerial;
		}

		/// <param name="issuerSerial"></param>
		public virtual void SetIssuerSerial(string issuerSerial)
		{
			this.issuerSerial = issuerSerial;
		}
	}
}
