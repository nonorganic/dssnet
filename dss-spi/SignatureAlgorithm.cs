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

using Org.BouncyCastle.Cms;
using System;

namespace EU.Europa.EC.Markt.Dss
{
    /// <summary>Supported signature algorithm</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public sealed class SignatureAlgorithm
    {
        public static readonly EU.Europa.EC.Markt.Dss.SignatureAlgorithm RSA = new EU.Europa.EC.Markt.Dss.SignatureAlgorithm
            ("RSA", CmsSignedGenerator.EncryptionRsa, "RSA/ECB/PKCS1Padding");

        public static readonly EU.Europa.EC.Markt.Dss.SignatureAlgorithm DSA = new EU.Europa.EC.Markt.Dss.SignatureAlgorithm
            ("DSA", CmsSignedGenerator.EncryptionDsa, string.Empty);

        public static readonly EU.Europa.EC.Markt.Dss.SignatureAlgorithm ECDSA = new EU.Europa.EC.Markt.Dss.SignatureAlgorithm
            ("ECDSA", CmsSignedGenerator.EncryptionECDsa, "ECDSA");

        private string name;

        private string oid;

        private string padding;

        private SignatureAlgorithm(string name, string oid, string padding)
        {
            this.name = name;
            this.oid = oid;
            this.padding = padding;
        }

        //public string GetJavaSignatureAlgorithm(DigestAlgorithm algorithm)
        public string GetSignatureAlgorithm(DigestAlgorithm algorithm)
        {
            //jbonilla: cambio de enum a clase.
            if (this.Equals(SignatureAlgorithm.RSA))
            {
                if (algorithm.Equals(DigestAlgorithm.SHA1))
                {
                    return "SHA1withRSA";
                }
                else
                {
                    if (algorithm.Equals(DigestAlgorithm.SHA256))
                    {
                        return "SHA256withRSA";
                    }
                    else
                    {
                        if (algorithm.Equals(DigestAlgorithm.SHA256))
                        {
                            return "SHA512withRSA";
                        }
                    }
                }
            }
            else
            {
                if (this.Equals(SignatureAlgorithm.ECDSA))
                {
                    if (algorithm.Equals(DigestAlgorithm.SHA1))
                    {
                        return "SHA1withECDSA";
                    }
                    else
                    {
                        if (algorithm.Equals(DigestAlgorithm.SHA256))
                        {
                            return "SHA256withECDSA";
                        }
                        else
                        {
                            if (algorithm.Equals(DigestAlgorithm.SHA256))
                            {
                                return "SHA512withECDSA";
                            }
                        }
                    }
                }
            }
            throw new NotSupportedException();
        }

        public string GetXMLSignatureAlgorithm(DigestAlgorithm digestAlgo)
        {
            if (this.Equals(EU.Europa.EC.Markt.Dss.SignatureAlgorithm.RSA))
            {
                if (digestAlgo.Equals(DigestAlgorithm.SHA1))
                {
                    return "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
                }
                else
                {
                    if (digestAlgo.Equals(DigestAlgorithm.SHA256))
                    {
                        return "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                    }
                    else
                    {
                        if (digestAlgo.Equals(DigestAlgorithm.SHA256))
                        {
                            return "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512";
                        }
                    }
                }
            }
            else
            {
                if (this.Equals(EU.Europa.EC.Markt.Dss.SignatureAlgorithm.ECDSA))
                {
                    if (digestAlgo.Equals(DigestAlgorithm.SHA1))
                    {
                        return "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha1";
                    }
                    else
                    {
                        if (digestAlgo.Equals(DigestAlgorithm.SHA256))
                        {
                            return "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256";
                        }
                        else
                        {
                            if (digestAlgo.Equals(DigestAlgorithm.SHA256))
                            {
                                return "http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha512";
                            }
                        }
                    }
                }
            }
            throw new NotSupportedException();
        }

        /// <returns>the name</returns>
        public string GetName()
        {
            return name;
        }

        /// <param name="name">the name to set</param>
        public void SetName(string name)
        {
            this.name = name;
        }

        /// <returns>the oid</returns>
        public string GetOid()
        {
            return oid;
        }

        /// <param name="oid">the oid to set</param>
        public void SetOid(string oid)
        {
            this.oid = oid;
        }

        /// <returns>the padding</returns>
        public string GetPadding()
        {
            return padding;
        }

        /// <param name="padding">the padding to set</param>
        public void SetPadding(string padding)
        {
            this.padding = padding;
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
            if (!(obj is EU.Europa.EC.Markt.Dss.SignatureAlgorithm))
            {
                return false;
            }
            EU.Europa.EC.Markt.Dss.SignatureAlgorithm other = (EU.Europa.EC.Markt.Dss.SignatureAlgorithm
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
