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

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;

namespace EU.Europa.EC.Markt.Dss
{
    /// <summary>Supported Algorithms</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public sealed class DigestAlgorithm
    {
        public static readonly EU.Europa.EC.Markt.Dss.DigestAlgorithm SHA1 = new EU.Europa.EC.Markt.Dss.DigestAlgorithm
            ("SHA-1", "1.3.14.3.2.26", /*DigestMethod.SHA1*/"SHA1");

        public static readonly EU.Europa.EC.Markt.Dss.DigestAlgorithm SHA256 = new EU.Europa.EC.Markt.Dss.DigestAlgorithm
            ("SHA-256", "2.16.840.1.101.3.4.2.1", /*DigestMethod.SHA256*/"SHA256");

        public static readonly EU.Europa.EC.Markt.Dss.DigestAlgorithm SHA512 = new EU.Europa.EC.Markt.Dss.DigestAlgorithm
            ("SHA-512", "2.16.840.1.101.3.4.2.3", /*DigestMethod.SHA512*/"SHA512");

        private string name;

        private string oid;

        private string xmlId;

        private DigestAlgorithm(string name, string oid, string xmlId)
        {
            this.name = name;
            this.oid = oid;
            this.xmlId = xmlId;
        }

        /// <summary>Return the algorithm corresponding to the name</summary>
        /// <param name="algoName"></param>
        /// <returns></returns>
        /// <exception cref="Sharpen.NoSuchAlgorithmException">Sharpen.NoSuchAlgorithmException
        /// 	</exception>
        public static EU.Europa.EC.Markt.Dss.DigestAlgorithm GetByName(string algoName)
        {
            if ("SHA-1".Equals(algoName) || "SHA1".Equals(algoName))
            {
                return SHA1;
            }
            if ("SHA-256".Equals(algoName))
            {
                return SHA256;
            }
            if ("SHA-512".Equals(algoName))
            {
                return SHA512;
            }
            throw new NoSuchAlgorithmException("unsupported algo: " + algoName);
        }

        /// <returns>the name</returns>
        public string GetName()
        {
            return name;
        }

        /// <returns>the oid</returns>
        public string GetOid()
        {
            return oid;
        }

        /// <returns>the xmlId</returns>
        public string GetXmlId()
        {
            return xmlId;
        }

        /// <summary>Gets the ASN.1 algorithm identifier structure corresponding to this digest algorithm
        /// 	</summary>
        /// <returns>the AlgorithmIdentifier</returns>
        public AlgorithmIdentifier GetAlgorithmIdentifier()
        {
            return new AlgorithmIdentifier(new DerObjectIdentifier(this.GetOid()), DerNull.Instance);
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((name == null) ? 0 : name.GetHashCode());
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj is EU.Europa.EC.Markt.Dss.DigestAlgorithm))
            {
                return false;
            }
            EU.Europa.EC.Markt.Dss.DigestAlgorithm other = (EU.Europa.EC.Markt.Dss.DigestAlgorithm
                )obj;
            if (name == null)
            {
                if (other.name != null)
                {
                    return false;
                }
            }
            else
            {
                if (!name.Equals(other.name))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
