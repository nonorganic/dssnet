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

namespace EU.Europa.EC.Markt.Dss.Signature
{
    /// <summary>Signature format handled by the application</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public sealed class SignatureFormat
    {
        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat CAdES_BES
             = new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("CAdES_BES");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat CAdES_EPES
             = new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("CAdES_EPES");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat CAdES_T =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("CAdES_T");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat CAdES_C =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("CAdES_C");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat CAdES_X =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("CAdES_X");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat CAdES_XL =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("CAdES_XL");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat CAdES_A =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("CAdES_A");


        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat XAdES_BES
             = new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("XAdES_BES");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat XAdES_EPES
             = new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("XAdES_EPES");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat XAdES_T =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("XAdES_T");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat XAdES_C =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("XAdES_C");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat XAdES_X =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("XAdES_X");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat XAdES_XL =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("XAdES_XL");

        public static readonly EU.Europa.EC.Markt.Dss.Signature.SignatureFormat XAdES_A =
            new EU.Europa.EC.Markt.Dss.Signature.SignatureFormat("XAdES_A");

        private string name;

        /// <summary>The default constructor for MimeTypes.</summary>
        /// <remarks>The default constructor for MimeTypes.</remarks>
        private SignatureFormat(string name)
        {
            //jbonilla
            this.name = name;
        }

        /// <returns>the code</returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>Return the SignatureFormat based on the name (String)</summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static EU.Europa.EC.Markt.Dss.Signature.SignatureFormat ValueByName(string
             name)
        {
            name = name.ToLower().Replace("-", "_");

            switch (name)
            {
                case "cadesbes":
                    return CAdES_BES;
                case "cadesepes":
                    return CAdES_EPES;
                case "cadest":
                    return CAdES_T;
                case "cadesc":
                    return CAdES_C;
                case "cadesx":
                    return CAdES_X;
                case "cadesxl":
                    return CAdES_XL;
                case "cadesa":
                    return CAdES_A;
                case "xadesbes":
                    return XAdES_BES;
                case "xadesepes":
                    return XAdES_EPES;
                case "xadest":
                    return XAdES_T;
                case "xadesc":
                    return XAdES_C;
                case "xadesx":
                    return XAdES_X;
                case "xadesxl":
                    return XAdES_XL;
                case "xadesa":
                    return XAdES_A;
            }

            throw new ArgumentException(name);
        }

        public override string ToString()
        {
            return name.Replace("_", "-");
        }

        public override int GetHashCode()
        {
            int prime = 31;
            int result = 1;
            result = prime * result + ((name == null) ? 0 : name.GetHashCode());
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj is EU.Europa.EC.Markt.Dss.Signature.SignatureFormat))
            {
                return false;
            }
            EU.Europa.EC.Markt.Dss.Signature.SignatureFormat other = (EU.Europa.EC.Markt.Dss.Signature.SignatureFormat
                )obj;
            if (name == null)
            {
                if (other.name != null)
                {
                    return false;
                }
            }
            else
            {
                if (!name.Equals(other.name))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
