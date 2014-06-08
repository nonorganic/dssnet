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

using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using System.Collections.Generic;

namespace EU.Europa.EC.Markt.Dss.Validation.Crl
{
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public abstract class OfflineCRLSource : ICrlSource
    {
        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(OfflineCRLSource).FullName
            );

        public X509Crl FindCrl(X509Certificate certificate, X509Certificate issuerCertificate
            )
        {
            foreach (X509Crl crl in GetCRLsFromSignature())
            {
                if (crl.IssuerDN.Equals(issuerCertificate.SubjectDN))
                {
                    LOG.Info("CRL found for issuer " + issuerCertificate.SubjectDN.ToString
                        ());
                    return crl;
                }
            }
            LOG.Info("CRL not found for issuer " + issuerCertificate.SubjectDN.ToString());
            return null;
        }

        /// <summary>Retrieve the list of CRL contained in the Signature</summary>
        /// <returns></returns>
        public abstract IList<X509Crl> GetCRLsFromSignature();
    }
}
