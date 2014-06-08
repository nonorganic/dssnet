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
using Sharpen;
using System.Collections.Generic;

namespace EU.Europa.EC.Markt.Dss.Validation.Certificate
{
    /// <summary>CertificateSource that wrap multiple CertificateSource</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class CompositeCertificateSource : CertificateSource
    {
        private CertificateSource[] sources;

        /// <summary>The default constructor for CompositeCertificateSource.</summary>
        /// <remarks>The default constructor for CompositeCertificateSource.</remarks>
        /// <param name="sources"></param>
        public CompositeCertificateSource(params CertificateSource[] sources)
        {
            this.sources = sources;
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual IList<CertificateAndContext> GetCertificateBySubjectName(X509Name
             subjectName)
        {
            IList<CertificateAndContext> list = new AList<CertificateAndContext>();
            foreach (CertificateSource source in sources)
            {
                if (source != null)
                {
                    IList<CertificateAndContext> @internal = source.GetCertificateBySubjectName(subjectName
                        );
                    if (@internal != null)
                    {
                        Sharpen.Collections.AddAll(list, @internal);
                    }
                }
            }
            return list;
        }
    }
}
