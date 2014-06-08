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

namespace EU.Europa.EC.Markt.Dss
{
    /// <summary>
    /// Container for a Digest and his algorithm
    /// <p>DISCLAIMER: Project owner DG-MARKT.
    /// </summary>
    /// <remarks>
    /// Container for a Digest and his algorithm
    /// <p>DISCLAIMER: Project owner DG-MARKT.
    /// </remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    /// <author><a href="mailto:dgmarkt.Project-DSS@arhs-developments.com">ARHS Developments</a>
    /// 	</author>
    public class Digest
    {
        private DigestAlgorithm algorithm;

        private byte[] value;

        /// <summary>The default constructor for Digest.</summary>
        /// <remarks>The default constructor for Digest.</remarks>
        public Digest()
        {
        }

        public Digest(DigestAlgorithm algorithm, byte[] value)
            : base()
        {
            this.algorithm = algorithm;
            this.value = value;
        }

        /// <returns>the algorithm</returns>
        public virtual DigestAlgorithm GetAlgorithm()
        {
            return algorithm;
        }

        /// <param name="algorithm">the algorithm to set</param>
        public virtual void SetAlgorithm(DigestAlgorithm algorithm)
        {
            this.algorithm = algorithm;
        }

        /// <returns>the value</returns>
        public virtual byte[] GetValue()
        {
            return value;
        }

        /// <param name="value">the value to set</param>
        public virtual void SetValue(byte[] value)
        {
            this.value = value;
        }
    }
}
