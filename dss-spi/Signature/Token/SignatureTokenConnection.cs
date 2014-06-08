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

using System.Collections.Generic;
using System.IO;

namespace EU.Europa.EC.Markt.Dss.Signature.Token
{
    /// <summary>Connection through available API to the SSCD (SmartCard, MSCAPI, PKCS#12)
    /// 	</summary>
    /// <version>$Revision: 2742 $ - $Date: 2013-10-17 12:32:08 +0200 (jeu., 17 oct. 2013) $
    /// 	</version>
    public interface ISignatureTokenConnection
    {
        /// <summary>Close the connection to the SSCD.</summary>
        /// <remarks>Close the connection to the SSCD.</remarks>
        void Close();

        /// <summary>Retrieve all the available keys (private keys entries) of the SSCD.</summary>
        /// <remarks>Retrieve all the available keys (private keys entries) of the SSCD.</remarks>
        /// <returns></returns>
        /// <exception cref="Sharpen.KeyStoreException">Sharpen.KeyStoreException</exception>
        IList<IDssPrivateKeyEntry> GetKeys();

        /// <summary>Sign the stream with the private key.</summary>
        /// <remarks>Sign the stream with the private key.</remarks>
        /// <param name="stream">The stream that need to be signed</param>
        /// <param name="digestAlgo"></param>
        /// <param name="keyEntry"></param>
        /// <returns></returns>
        /// <exception cref="Sharpen.NoSuchAlgorithmException">If the algorithm is not supported
        /// 	</exception>
        /// <exception cref="System.IO.IOException">the token cannot produce the signature</exception>
        byte[] Sign(Stream stream, DigestAlgorithm digestAlgo, IDssPrivateKeyEntry keyEntry);
    }
}
