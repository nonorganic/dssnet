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
using System.IO;
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Validation.Tsp;
using Org.BouncyCastle.Tsp;
using Sharpen;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities.Encoders;
using EU.Europa.EC.Markt.Dss.Validation.Https;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto;

namespace EU.Europa.EC.Markt.Dss.Validation.Tsp
{
	/// <summary>Class encompassing a RFC 3161 TSA, accessed through HTTP(S) to a given URI
	/// 	</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class OnlineTspSource : TSAClientBouncyCastle, ITspSource
	{
    
        public OnlineTspSource(string tspServer)
            : base(tspServer)
        {            
        }

        public OnlineTspSource(string tspServer, string username, string password)
            : base(tspServer, username, password)
        {            
        }       

        public virtual TimeStampResponse GetTimeStampResponse(DigestAlgorithm algorithm, byte[] digest)
        {
            this.digestAlgorithm = algorithm.GetName();
            byte[] respBytes = null;

            TimeStampRequestGenerator tsqGenerator = new TimeStampRequestGenerator();
            tsqGenerator.SetCertReq(true);
            // tsqGenerator.setReqPolicy("1.3.6.1.4.1.601.10.3.1");
            BigInteger nonce = BigInteger.ValueOf(DateTime.Now.Ticks + Environment.TickCount);
            TimeStampRequest request = tsqGenerator.Generate(DigestAlgorithms.GetAllowedDigests(digestAlgorithm), digest, nonce);            
            byte[] requestBytes = request.GetEncoded();

            // Call the communications layer
            respBytes = GetTSAResponse(requestBytes);

            // Handle the TSA response
            return new TimeStampResponse(respBytes);
            
        }        
    }
}
