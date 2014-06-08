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
using System.Collections.Generic;

namespace EU.Europa.EC.Markt.Dss.Validation.Certificate
{
    /// <summary>
    /// The validation of a certificate require to access some other certificate from multiple source (Trusted List, trust
    /// store, the signature itself).
    /// </summary>
    /// <remarks>
    /// The validation of a certificate require to access some other certificate from multiple source (Trusted List, trust
    /// store, the signature itself). This interface provides abstraction for accessing a certificate, regardless of the
    /// source.
    /// </remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public interface CertificateSource
    {
        /// <summary>Give all certificate corresponding to a subject name.</summary>
        /// <remarks>Give all certificate corresponding to a subject name. Regardless of other criteria, like validity.
        /// 	</remarks>
        /// <param name="subjectName"></param>
        /// <returns>
        /// A list of certificates (and their respective context) corresponding to the subjectName. Never return
        /// null.
        /// </returns>
        /// <exception cref="System.IO.IOException">System.IO.IOException</exception>
        IList<CertificateAndContext> GetCertificateBySubjectName(X509Name subjectName
            );
    }
}
