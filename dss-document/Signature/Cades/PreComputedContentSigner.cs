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

/*
using System.IO;
//using Org.Apache.Commons.IO.Output;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Operator;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Signature.Cades
{
	/// <summary>ContentSigner using a provided pre-computed signature</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
    public class PreComputedContentSigner : ContentSigner //TODO jbonilla BC no contiene ContentSigner en C#
	{
		private byte[] preComputedSignature;

		private AlgorithmIdentifier algorithmIdentifier;

		private AlgorithmIdentifier digestAlgorithmIdentifier;

		private ByteArrayOutputStream byteOutputStream = new ByteArrayOutputStream();

		private sealed class _OutputStream_47 : OutputStream
		{
			public _OutputStream_47()
			{
			}

			/// <exception cref="System.IO.IOException"></exception>
			public override void Write(int arg0)
			{
			}
		}

		internal OutputStream nullOutputStream = new _OutputStream_47();

		/// <param name="preComputedSignature">the preComputedSignature to set</param>
		public PreComputedContentSigner(string algorithmIdentifier, byte[] preComputedSignature
			)
		{
			this.preComputedSignature = preComputedSignature;
			this.algorithmIdentifier = new DefaultSignatureAlgorithmIdentifierFinder().Find(algorithmIdentifier
				);
			this.digestAlgorithmIdentifier = new DefaultDigestAlgorithmIdentifierFinder().Find
				(this.algorithmIdentifier);
		}

		/// <summary>The default constructor for PreComputedContentSigner.</summary>
		/// <remarks>The default constructor for PreComputedContentSigner.</remarks>
		/// <param name="algorithmIdentifier"></param>
		public PreComputedContentSigner(string algorithmIdentifier) : this(algorithmIdentifier
			, new byte[0])
		{
		}

		public virtual AlgorithmIdentifier GetAlgorithmIdentifier()
		{
			return algorithmIdentifier;
		}

		/// <returns>the digestAlgorithmIdentifier</returns>
		public virtual AlgorithmIdentifier GetDigestAlgorithmIdentifier()
		{
			return digestAlgorithmIdentifier;
		}

		public virtual OutputStream GetOutputStream()
		{
			return byteOutputStream;
		}

		// return nullOutputStream;
		public virtual byte[] GetSignature()
		{
			return preComputedSignature;
		}

		/// <returns>the preComputedSignature</returns>
		public virtual byte[] GetPreComputedSignature()
		{
			return preComputedSignature;
		}

		/// <returns>the byteOutputStream</returns>
		public virtual ByteArrayOutputStream GetByteOutputStream()
		{
			return byteOutputStream;
		}
	}
}
*/