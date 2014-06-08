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

using Sharpen;
using System;

namespace EU.Europa.EC.Markt.Dss
{
    /// <summary>Occurs when object (X509, CRL, OCSP, ...) is not encoded correctly</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    [System.Serializable]
    public class EncodingException : RuntimeException
    {
        private ResourceBundle bundle = ResourceBundle.GetBundle("eu/europa/ec/markt/dss/i18n"
            );

        private EncodingException.MSG key;

        /// <summary>Supported messages</summary>
        public enum MSG
        {
            CERTIFICATE_CANNOT_BE_READ,
            OCSP_CANNOT_BE_READ,
            SIGNATURE_METHOD_ERROR,
            SIGNING_CERTIFICATE_ENCODING,
            SIGNING_TIME_ENCODING,
            SIGNATURE_POLICY_ENCODING,
            COUNTERSIGNATURE_ENCODING,
            SIGNATURE_TIMESTAMP_ENCODING,
            TIMESTAMP_X1_ENCODING,
            TIMESTAMP_X2_ENCODING,
            ARCHIVE_TIMESTAMP_ENCODING,
            CERTIFICATE_REF_ENCODING,
            CRL_REF_ENCODING,
            OCSP_REF_ENCODING,
            SIGNATURE_TIMESTAMP_DATA_ENCODING,
            TIMESTAMP_X1_DATA_ENCODING,
            TIMESTAMP_X2_DATA_ENCODING,
            ARCHIVE_TIMESTAMP_DATA_ENCODING,
            CRL_CANNOT_BE_WRITTEN
        }

        /// <summary>The default constructor for EncodingException.</summary>
        /// <remarks>The default constructor for EncodingException.</remarks>
        /// <param name="message"></param>
        public EncodingException(EncodingException.MSG key)
        {
            if (key == null)
            {
                throw new ArgumentException("Cannot build Exception without a message");
            }
            this.key = key;
        }

        //public override string GetLocalizedMessage()
        public string GetLocalizedMessage()
        {
            return bundle.GetString(key.ToString());
        }
    }
}
