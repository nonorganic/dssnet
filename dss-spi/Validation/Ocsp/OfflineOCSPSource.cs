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
using System.Collections.Generic;

namespace EU.Europa.EC.Markt.Dss.Validation.Ocsp
{
    /// <summary>Abstract class that helps to implements OCSPSource with a already loaded list of BasicOCSPResp
    /// 	</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public abstract class OfflineOCSPSource : IOcspSource
    {
        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(OfflineOCSPSource).FullName
            );

        /// <exception cref="System.IO.IOException"></exception>
        public BasicOcspResp GetOcspResponse(X509Certificate certificate, X509Certificate
             issuerCertificate)
        {
            LOG.Info("find OCSP response");
            try
            {
                foreach (BasicOcspResp basicOCSPResp in GetOCSPResponsesFromSignature())
                {
                    CertificateID certId = new CertificateID(CertificateID.HashSha1, issuerCertificate
                        , certificate.SerialNumber);
                    foreach (SingleResp singleResp in basicOCSPResp.Responses)
                    {
                        if (singleResp.GetCertID().Equals(certId))
                        {
                            LOG.Info("OCSP response found");
                            return basicOCSPResp;
                        }
                    }
                }
                OcspNotFound(certificate, issuerCertificate);
                return null;
            }
            catch (OcspException e)
            {
                LOG.Error("OcspException: " + e.Message);
                return null;
            }
        }

        /// <summary>Callback used when the OCSP is not found.</summary>
        /// <remarks>Callback used when the OCSP is not found.</remarks>
        /// <param name="certificate"></param>
        /// <param name="issuerCertificate"></param>
        /// <exception cref="System.IO.IOException"></exception>
        public virtual void OcspNotFound(X509Certificate certificate, X509Certificate issuerCertificate
            )
        {
        }

        /// <summary>Retrieve the list of BasicOCSPResp contained in the Signature.</summary>
        /// <remarks>Retrieve the list of BasicOCSPResp contained in the Signature.</remarks>
        /// <returns></returns>
        public abstract IList<BasicOcspResp> GetOCSPResponsesFromSignature();
    }
}
