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

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace EU.Europa.EC.Markt.Dss.Signature.Token
{
    /// <summary>Wrapper of a PrivateKeyEntry comming from a KeyStore.</summary>
    /// <remarks>Wrapper of a PrivateKeyEntry comming from a KeyStore.</remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class KSPrivateKeyEntry : IDssPrivateKeyEntry
    {
        private X509Certificate Certificate { get; set; }

        private X509Certificate[] CertificateChain { get; set; }

        public AsymmetricKeyParameter PrivateKey { get; set; }

        /// <summary>The default constructor for DSSPrivateKeyEntry.</summary>
        /// <remarks>The default constructor for DSSPrivateKeyEntry.</remarks>
        //public KSPrivateKeyEntry(KeyStore.PrivateKeyEntry privateKeyEntry)        
        public KSPrivateKeyEntry(X509Certificate certificate, X509Certificate[] certificateChain, AsymmetricKeyParameter privateKey)
        {
            Certificate = certificate;
            CertificateChain = certificateChain;
            PrivateKey = privateKey;
        }

        public X509Certificate GetCertificate()
        {
            return this.Certificate;
        }

        public X509Certificate[] GetCertificateChain()
        {
            return this.CertificateChain;
        }

        /// <exception cref="Sharpen.NoSuchAlgorithmException"></exception>
        public virtual SignatureAlgorithm GetSignatureAlgorithm()
        {
            if (PrivateKey is RsaKeyParameters)
            {
                return SignatureAlgorithm.RSA;
            }
            else
            {
                if (PrivateKey is DsaKeyParameters)
                {
                    return SignatureAlgorithm.DSA;
                }
                else
                {
                    throw new NoSuchAlgorithmException("Don't find algorithm for PrivateKey of type "
                        + PrivateKey.GetType());
                }
            }
        }
    }
}
