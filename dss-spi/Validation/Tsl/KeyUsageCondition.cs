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

using EU.Europa.EC.Markt.Dss.Validation.Certificate;

namespace EU.Europa.EC.Markt.Dss.Validation.Tsl
{
    /// <summary>Test if the certificate has a Key usage</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    [System.Serializable]
    public class KeyUsageCondition : Condition
    {
        private const long serialVersionUID = -7931767601112389304L;

        /// <summary>
        /// KeyUsage bit values
        /// <p>
        /// DISCLAIMER: Project owner DG-MARKT.
        /// </summary>
        /// <remarks>
        /// KeyUsage bit values
        /// <p>
        /// DISCLAIMER: Project owner DG-MARKT.
        /// </remarks>
        /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
        /// 	</version>
        /// <author><a href="mailto:dgmarkt.Project-DSS@arhs-developments.com">ARHS Developments</a>
        /// 	</author>
        public enum KeyUsageBit
        {
            digitalSignature,
            nonRepudiation,
            keyEncipherment,
            dataEncipherment,
            keyAgreement,
            keyCertSign,
            crlSign,
            encipherOnly,
            decipherOnly
        }

        private KeyUsageCondition.KeyUsageBit bit;

        /// <summary>The default constructor for KeyUsageCondition.</summary>
        /// <remarks>The default constructor for KeyUsageCondition.</remarks>
        public KeyUsageCondition()
        {
        }

        /// <summary>The default constructor for KeyUsageCondition.</summary>
        /// <remarks>The default constructor for KeyUsageCondition.</remarks>
        /// <param name="bit"></param>
        public KeyUsageCondition(KeyUsageCondition.KeyUsageBit bit)
        {
            this.bit = bit;
        }

        /// <summary>The default constructor for KeyUsageCondition.</summary>
        /// <remarks>The default constructor for KeyUsageCondition.</remarks>
        /// <param name="value"></param>
        public KeyUsageCondition(string value)
            : this((KeyUsageBit)System.Enum.Parse(typeof(KeyUsageBit), value))
        {
        }

        /// <returns>the bit</returns>
        public virtual KeyUsageCondition.KeyUsageBit GetBit()
        {
            return bit;
        }

        public virtual bool Check(CertificateAndContext cert)
        {
            return cert.GetCertificate().GetKeyUsage()[(int)bit];
        }
    }
}
