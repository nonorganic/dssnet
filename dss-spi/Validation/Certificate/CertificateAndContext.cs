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

using Org.BouncyCastle.X509;
using System.Runtime.Serialization;

namespace EU.Europa.EC.Markt.Dss.Validation.Certificate
{
    /// <summary>A certificate comes from a certain context (Trusted List, CertStore, Signature) and has somes properties
    /// 	</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class CertificateAndContext
    {
        private X509Certificate certificate;

        private CertificateSourceType certificateSource;

        private ISerializable context;

        /// <summary>The default constructor for CertificateAndContext.</summary>
        /// <remarks>The default constructor for CertificateAndContext.</remarks>
        public CertificateAndContext()
        {
        }

        /// <summary>
        /// Create a CertificateAndContext wrapping the provided X509Certificate The default constructor for
        /// CertificateAndContext.
        /// </summary>
        /// <remarks>
        /// Create a CertificateAndContext wrapping the provided X509Certificate The default constructor for
        /// CertificateAndContext.
        /// </remarks>
        /// <param name="cert"></param>
        public CertificateAndContext(X509Certificate cert)
            : this(cert, null)
        {
        }

        /// <summary>The default constructor for CertificateAndContext.</summary>
        /// <remarks>The default constructor for CertificateAndContext.</remarks>
        /// <param name="cert"></param>
        /// <param name="context"></param>
        public CertificateAndContext(X509Certificate cert, ISerializable context)
        {
            this.certificate = cert;
            this.context = context;
        }

        /// <summary>Get the X509 Certificate</summary>
        /// <returns></returns>
        public virtual X509Certificate GetCertificate()
        {
            return certificate;
        }

        /// <summary>Set the X509 Certificate</summary>
        /// <param name="certificate"></param>
        public virtual void SetCertificate(X509Certificate certificate)
        {
            this.certificate = certificate;
        }

        /// <summary>Get information about the source of the Certificate (TRUSTED_LIST, TRUST_STORE, ...)
        /// 	</summary>
        /// <returns></returns>
        public virtual CertificateSourceType GetCertificateSource()
        {
            return certificateSource;
        }

        /// <summary>Set information bout the source of the Certificate (TRUSTED_LIST, TRUST_STORE, ...)
        /// 	</summary>
        /// <param name="certificateSource"></param>
        public virtual void SetCertificateSource(CertificateSourceType certificateSource)
        {
            this.certificateSource = certificateSource;
        }

        /// <summary>Get information about the context from which the certificate is fetched</summary>
        /// <returns></returns>
        public virtual ISerializable GetContext()
        {
            return context;
        }

        /// <summary>Set information about the context from which the certificate if fetched</summary>
        /// <param name="context"></param>
        public virtual void SetContext(ISerializable context)
        {
            this.context = context;
        }

        public override string ToString()
        {
            return "Certificate[for=" + certificate.SubjectDN.ToString() + ",source=" + certificateSource
                 + ",issuedBy=" + certificate.IssuerDN + ",serial=" + certificate
                .SerialNumber + "]";
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((certificate == null) ? 0 : certificate.GetHashCode());
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
            if (GetType() != obj.GetType())
            {
                return false;
            }
            EU.Europa.EC.Markt.Dss.Validation.Certificate.CertificateAndContext other = (EU.Europa.EC.Markt.Dss.Validation.Certificate.CertificateAndContext
                )obj;
            if (certificate == null)
            {
                if (other.certificate != null)
                {
                    return false;
                }
            }
            else
            {
                if (!certificate.Equals(other.certificate))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
