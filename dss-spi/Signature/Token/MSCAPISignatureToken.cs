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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Signature.Token;
using Sharpen;
using SystemX509 = System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.X509;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Utilities.IO;

namespace EU.Europa.EC.Markt.Dss.Signature.Token
{
	/// <summary>Class holding all MS CAPI API access logic.</summary>
	/// <remarks>Class holding all MS CAPI API access logic.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class MSCAPISignatureToken : ISignatureTokenConnection
	{
        public SystemX509.X509Certificate2 Cert { get; set; }
        public ICollection<X509Certificate> Keystore { get; set; }

        public MSCAPISignatureToken()
        {            
        }

        public void Close()
        {            
        }

        public IList<IDssPrivateKeyEntry> GetKeys()
        {
            if (this.Cert == null)
                throw new ArgumentNullException("Cert");            

            IList<IDssPrivateKeyEntry> keys = new List<IDssPrivateKeyEntry>();

            keys.Add(new KSX509Certificate2Entry() { 
                Cert2 = this.Cert,
                Keystore = this.Keystore
            });
            return keys;
        }

        public byte[] Sign(Stream stream, DigestAlgorithm digestAlgo, IDssPrivateKeyEntry keyEntry)
        {
            byte[] signedBytes;

            if (keyEntry is KSX509Certificate2Entry)
            {
                var cert = ((KSX509Certificate2Entry)keyEntry).Cert2;

                X509Certificate2Signature signer = new X509Certificate2Signature(cert, digestAlgo.GetName());

                signedBytes = signer.Sign(Streams.ReadAll(stream));

                stream.Close();

                return signedBytes;
            }

            throw new ArgumentException("Only allowed KSX509Certificate2Entry", "keyEntry");
        }
    }
}
