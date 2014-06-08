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
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Validation.Ocsp
{
	/// <summary>Utility class used to convert OcspResp to BasicOcspResp</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public abstract class OCSPUtils
	{
		/// <summary>Convert a OcspResp in a BasicOcspResp</summary>
		/// <param name="ocspResp"></param>
		/// <returns></returns>
		public static BasicOcspResp FromRespToBasic(OcspResp ocspResp)
		{
			try
			{
				return (BasicOcspResp)ocspResp.GetResponseObject();
			}
			catch (OcspException e)
			{
				throw new RuntimeException(e);
			}
		}

		/// <summary>Convert a BasicOcspResp in OcspResp (connection status is set to SUCCESSFUL).
		/// 	</summary>
		/// <remarks>Convert a BasicOcspResp in OcspResp (connection status is set to SUCCESSFUL).
		/// 	</remarks>
		/// <param name="basicOCSPResp"></param>
		/// <returns></returns>
		public static OcspResp FromBasicToResp(BasicOcspResp basicOCSPResp)
		{
			try
			{
				return FromBasicToResp(basicOCSPResp.GetEncoded());
			}
			catch (IOException e)
			{
				throw new RuntimeException(e);
			}
		}

		/// <summary>Convert a BasicOcspResp in OcspResp (connection status is set to SUCCESSFUL).
		/// 	</summary>
		/// <remarks>Convert a BasicOcspResp in OcspResp (connection status is set to SUCCESSFUL).
		/// 	</remarks>
		/// <param name="basicOCSPResp"></param>
		/// <returns></returns>
		public static OcspResp FromBasicToResp(byte[] basicOCSPResp)
		{
			OcspResponse response = new OcspResponse(new OcspResponseStatus(OcspResponseStatus
				.Successful), new ResponseBytes(OcspObjectIdentifiers.PkixOcspBasic, new DerOctetString
				(basicOCSPResp)));
			OcspResp resp = new OcspResp(response);
			return resp;
		}
	}
}
