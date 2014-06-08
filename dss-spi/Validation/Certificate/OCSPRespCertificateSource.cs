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
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.X509;
using Sharpen;
using System.Collections.Generic;

namespace EU.Europa.EC.Markt.Dss.Validation.Certificate
{
    /// <summary>Implement a CertificateSource that retrieve the certificates from an OCSPResponse
    /// 	</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class OCSPRespCertificateSource : OfflineCertificateSource
    {
        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Certificate.OCSPRespCertificateSource
            ).FullName);

        private BasicOcspResp ocspResp;

        /// <summary>The default constructor for OCSPRespCertificateSource.</summary>
        /// <remarks>The default constructor for OCSPRespCertificateSource.</remarks>
        public OCSPRespCertificateSource(BasicOcspResp ocspResp)
        {
            this.ocspResp = ocspResp;
        }

        public override IList<X509Certificate> GetCertificates()
        {
            IList<X509Certificate> certs = new AList<X509Certificate>();
            try
            {
                //foreach (X509Certificate c in ocspResp.GetCerts(null))
                foreach (X509Certificate c in ocspResp.GetCerts())
                {
                    LOG.Info(c.SubjectDN + " issued by " + c.IssuerDN
                         + " serial number " + c.SerialNumber);
                    certs.AddItem(c);
                }
            }
            catch (OcspException)
            {
                throw new EncodingException(EncodingException.MSG.OCSP_CANNOT_BE_READ);
            }
            /*catch (NoSuchProviderException e)
            {
                // Provider (BouncyCastle) not found. Should never happens.
                throw new RuntimeException(e);
            }*/
            return certs;
        }
    }
}
