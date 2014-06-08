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

using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.X509;

namespace EU.Europa.EC.Markt.Dss.Validation.Ocsp
{
    /// <summary>The validation of a certificate may requires the use of OCSP information.
    /// 	</summary>
    /// <remarks>
    /// The validation of a certificate may requires the use of OCSP information. Theses information can provide from
    /// multiple source (the signature itself, online OCSP server, ...). This interface provide an abstraction for a source
    /// of OCSPResp
    /// </remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public interface IOcspSource
    {
        /// <summary>Get and OCSPResp for the given certificate/issuerCertificate couple.</summary>
        /// <remarks>Get and OCSPResp for the given certificate/issuerCertificate couple.</remarks>
        /// <param name="certificate">The certificate for which the request is made</param>
        /// <param name="issuerCertificate">The issuerCertificate of the certificate</param>
        /// <returns>An OCSPResp containing information about the validity of the certificate
        /// 	</returns>
        /// <exception cref="System.IO.IOException"></exception>
        BasicOcspResp GetOcspResponse(X509Certificate certificate, X509Certificate issuerCertificate
            );
    }
}
