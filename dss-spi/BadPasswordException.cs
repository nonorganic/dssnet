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
    /// <summary>Thrown when using bad password</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    [System.Serializable]
    public class BadPasswordException : RuntimeException
    {
        private ResourceBundle bundle = ResourceBundle.GetBundle("eu/europa/ec/markt/dss/i18n"
            );

        private BadPasswordException.MSG key;

        /// <summary>Supported messages</summary>
        public enum MSG
        {
            PKCS11_BAD_PASSWORD,
            PKCS12_BAD_PASSWORD
        }

        /// <summary>The default constructor for BadPasswordException.</summary>
        /// <remarks>The default constructor for BadPasswordException.</remarks>
        /// <param name="message"></param>
        public BadPasswordException(BadPasswordException.MSG message)
        {
            if (message == null)
            {
                throw new ArgumentException("Cannot build Exception without a message");
            }
            this.key = message;
        }

        //public override string GetLocalizedMessage()
        public string GetLocalizedMessage()
        {
            return bundle.GetString(key.ToString());
        }
    }
}
