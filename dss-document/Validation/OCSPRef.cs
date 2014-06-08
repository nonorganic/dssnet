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

using System.IO;
using EU.Europa.EC.Markt.Dss.Validation.Ocsp;
//using Org.Apache.Commons.Codec.Binary;
using Org.BouncyCastle.Asn1.Esf;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <summary>Reference an OcspResponse</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class OCSPRef
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.OCSPRef
			).FullName);

		private string algorithm;

		private byte[] digestValue;

		private bool matchOnlyBasicOCSPResponse;

		/// <summary>The default constructor for OCSPRef.</summary>
		/// <remarks>The default constructor for OCSPRef.</remarks>
		public OCSPRef(OcspResponsesID ocsp, bool matchOnlyBasicOCSPResponse) : this(ocsp
			.OcspRepHash.HashAlgorithm.ObjectID.Id, ocsp.OcspRepHash
			.GetHashValue(), matchOnlyBasicOCSPResponse)
		{
		}

		/// <summary>The default constructor for OCSPRef.</summary>
		/// <remarks>The default constructor for OCSPRef.</remarks>
		public OCSPRef(string algorithm, byte[] digestValue, bool matchOnlyBasicOCSPResponse
			)
		{
			this.algorithm = algorithm;
			this.digestValue = digestValue;
			this.matchOnlyBasicOCSPResponse = matchOnlyBasicOCSPResponse;
		}

		/// <param name="ocspResp"></param>
		/// <returns></returns>
		public virtual bool Match(BasicOcspResp ocspResp)
		{
			try
			{
				IDigest digest = DigestUtilities.GetDigest(algorithm);
                byte[] oscpBytes;
				if (matchOnlyBasicOCSPResponse)
				{
                    oscpBytes = ocspResp.GetEncoded();					
				}
				else
				{
                    oscpBytes = OCSPUtils.FromBasicToResp(ocspResp).GetEncoded();					
				}
                digest.BlockUpdate(oscpBytes, 0, oscpBytes.Length);
				byte[] computedValue = DigestUtilities.DoFinal(digest);
				LOG.Info("Compare " + Hex.ToHexString(digestValue) + " to computed value " + 
					Hex.ToHexString(computedValue) + " of BasicOcspResp produced at " + ocspResp
					.ProducedAt);
				return Arrays.Equals(digestValue, computedValue);
			}
			catch (NoSuchAlgorithmException ex)
			{
				throw new RuntimeException("Maybe BouncyCastle provider is not installed ?", ex);
			}
			catch (IOException ex)
			{
				throw new RuntimeException(ex);
			}
		}
	}
}
