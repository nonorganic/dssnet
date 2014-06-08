/*
 * DSS - Digital Signature Services
 *
 * Copyright (C) 2011 European Commission, Directorate-General Internal Market and Services (DG MARKT), B-1049 Bruxelles/Brussel
 *
 * Developed by: 2011 ARHS Developments S.A. (rue Nicolas Bové 2B, L-1253 Luxembourg) http://www.arhs-developments.com
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
using System.IO;

namespace EU.Europa.EC.Markt.Dss
{
    /// <summary>Exception when the data cannot be fetched</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    [System.Serializable]
    public class CannotFetchDataException : RuntimeException
    {
        private const long serialVersionUID = -1112490792269827445L;

        private ResourceBundle bundle = ResourceBundle.GetBundle("eu/europa/ec/markt/dss/i18n"
            );

        private CannotFetchDataException.MSG key;

        private Exception cause;

        private string serviceName;

        /// <summary>Supported messages</summary>
        public enum MSG
        {
            IO_EXCEPTION,
            TIMOUT_EXCEPTION,
            SIZE_LIMIT_EXCEPTION,
            UNKNOWN_HOST_EXCEPTION
        }

        /// <summary>The default constructor for BadPasswordException.</summary>
        /// <remarks>The default constructor for BadPasswordException.</remarks>
        /// <param name="message"></param>
        public CannotFetchDataException(CannotFetchDataException.MSG message, string serviceName
            )
        {
            if (message == null)
            {
                throw new ArgumentException("Cannot build Exception without a message");
            }
            this.key = message;
        }

        /// <summary>The default constructor for CannotFetchDataException.</summary>
        /// <remarks>The default constructor for CannotFetchDataException.</remarks>
        /// <param name="ex"></param>
        public CannotFetchDataException(IOException ex, string serviceName)
            : this(ex is
                UnknownHostException ? CannotFetchDataException.MSG.UNKNOWN_HOST_EXCEPTION : CannotFetchDataException.MSG
                .IO_EXCEPTION, serviceName)
        {
            cause = ex;
            this.serviceName = serviceName;
        }
     
        //public override string GetLocalizedMessage()
        public string GetLocalizedMessage()
        {
            MessageFormat format = new MessageFormat(bundle.GetString(key.ToString()));
            object[] args = new object[] { serviceName };
            return format.Format(args);
        }
    }
}
