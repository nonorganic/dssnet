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

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using Sharpen;
using System.Collections.Generic;

namespace EU.Europa.EC.Markt.Dss.Validation.Certificate
{
    /// <summary>
    /// Some certificate source are "offline", that means that the set of certificate is availaible and the software only
    /// needs to find the certificate on base of the subjectName
    /// </summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public abstract class OfflineCertificateSource : CertificateSource
    {
        private CertificateSourceType sourceType;

        /// <param name="sourceType">the sourceType to set</param>
        public virtual void SetSourceType(CertificateSourceType sourceType)
        {
            this.sourceType = sourceType;
        }

        public IList<CertificateAndContext> GetCertificateBySubjectName(X509Name subjectName
            )
        {
            IList<CertificateAndContext> list = new AList<CertificateAndContext>();
            foreach (X509Certificate cert in GetCertificates())
            {
                if (subjectName.Equals(cert.SubjectDN))
                {
                    CertificateAndContext cc = new CertificateAndContext(cert);
                    cc.SetCertificateSource(sourceType);
                    list.AddItem(cc);
                }
            }
            return list;
        }

        /// <summary>Retrieve the list of certificate from this source.</summary>
        /// <remarks>Retrieve the list of certificate from this source.</remarks>
        /// <returns></returns>
        public abstract IList<X509Certificate> GetCertificates();
    }
}
