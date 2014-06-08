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

//jbonilla - No aplica para C#
/*
using System;
using EU.Europa.EC.Markt.Dss.Signature.Provider;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Signature.Provider
{
	/// <summary>Custom Signature implementation that intercept the digest generation.</summary>
	/// <remarks>Custom Signature implementation that intercept the digest generation.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class SignatureInterceptor : Sharpen.Signature
	{
		private MessageDigest digester;

		private SpecialPrivateKey specialPrivateKey;

		/// <summary>The default constructor for SignatureInterceptor.</summary>
		/// <remarks>The default constructor for SignatureInterceptor.</remarks>
		/// <exception cref="Sharpen.NoSuchAlgorithmException"></exception>
		public SignatureInterceptor() : base("SHA1withRSA")
		{
			digester = MessageDigest.GetInstance("SHA1");
		}

		/// <exception cref="Sharpen.InvalidKeyException"></exception>
		protected override void EngineInitVerify(PublicKey publicKey)
		{
			throw new NotSupportedException();
		}

		/// <exception cref="Sharpen.InvalidKeyException"></exception>
		protected override void EngineInitSign(PrivateKey privateKey)
		{
			if (privateKey is SpecialPrivateKey)
			{
				specialPrivateKey = (SpecialPrivateKey)privateKey;
			}
			else
			{
				throw new ArgumentException("Can only use instance of SpecialPrivateKey");
			}
		}

		/// <exception cref="Sharpen.SignatureException"></exception>
		protected override void EngineUpdate(byte b)
		{
			digester.Update(b);
		}

		/// <exception cref="Sharpen.SignatureException"></exception>
		protected override void EngineUpdate(byte[] b, int off, int len)
		{
			digester.Update(b, off, len);
		}

		/// <exception cref="Sharpen.SignatureException"></exception>
		protected override byte[] EngineSign()
		{
			specialPrivateKey.SetMessageDigest(digester.Digest());
			byte[] signature = specialPrivateKey.GetPreviouslyComputedSignature();
			if (signature == null)
			{
				return new byte[0];
			}
			else
			{
				return signature;
			}
		}

		/// <exception cref="Sharpen.SignatureException"></exception>
		protected override bool EngineVerify(byte[] sigBytes)
		{
			throw new NotSupportedException();
		}

		/// <exception cref="Sharpen.InvalidParameterException"></exception>
		protected override void EngineSetParameter(string param, object value)
		{
			throw new NotSupportedException();
		}

		/// <exception cref="Sharpen.InvalidParameterException"></exception>
		protected override object EngineGetParameter(string param)
		{
			throw new NotSupportedException();
		}
	}
}
*/