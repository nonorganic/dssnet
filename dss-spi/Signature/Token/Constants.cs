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

namespace EU.Europa.EC.Markt.Dss.Signature.Token
{
    /// <summary>Constants shared between PKCS#11 and PC/SC.</summary>
    /// <remarks>
    /// Constants shared between PKCS#11 and PC/SC. Needed to isolate these since SunPKCS11 is not always available. Windows
    /// 64 bit has no sunpkcs11.jar yet (JRE 1.6.0_20).
    /// </remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public abstract class Constants
    {
        public static readonly byte[] SHA1_DIGEST_INFO_PREFIX = new byte[] { unchecked((int)(0x30))
			, unchecked((int)(0x1f)), unchecked((int)(0x30)), unchecked((int)(0x07)), unchecked(
			(int)(0x06)), unchecked((int)(0x05)), unchecked((int)(0x2b)), unchecked((int)(0x0e
			)), unchecked((int)(0x03)), unchecked((int)(0x02)), unchecked((int)(0x1a)), unchecked(
			(int)(0x04)), unchecked((int)(0x14)) };

        public static readonly byte[] SHA224_DIGEST_INFO_PREFIX = new byte[] { unchecked((int)(0x30
			)), unchecked((int)(0x2b)), unchecked((int)(0x30)), unchecked((int)(0x0b)), unchecked(
			(int)(0x06)), unchecked((int)(0x09)), unchecked((int)(0x60)), unchecked((byte)unchecked(
			(int)(0x86))), unchecked((int)(0x48)), unchecked((int)(0x01)), unchecked((int)(0x65
			)), unchecked((int)(0x03)), unchecked((int)(0x04)), unchecked((int)(0x02)), unchecked(
			(int)(0x04)), unchecked((int)(0x04)), unchecked((int)(0x1c)) };

        public static readonly byte[] SHA256_DIGEST_INFO_PREFIX = new byte[] { unchecked((int)(0x30
			)), unchecked((int)(0x2f)), unchecked((int)(0x30)), unchecked((int)(0x0b)), unchecked(
			(int)(0x06)), unchecked((int)(0x09)), unchecked((int)(0x60)), unchecked((byte)unchecked(
			(int)(0x86))), unchecked((int)(0x48)), unchecked((int)(0x01)), unchecked((int)(0x65
			)), unchecked((int)(0x03)), unchecked((int)(0x04)), unchecked((int)(0x02)), unchecked(
			(int)(0x01)), unchecked((int)(0x04)), unchecked((int)(0x20)) };

        public static readonly byte[] SHA384_DIGEST_INFO_PREFIX = new byte[] { unchecked((int)(0x30
			)), unchecked((int)(0x3f)), unchecked((int)(0x30)), unchecked((int)(0x0b)), unchecked(
			(int)(0x06)), unchecked((int)(0x09)), unchecked((int)(0x60)), unchecked((byte)unchecked(
			(int)(0x86))), unchecked((int)(0x48)), unchecked((int)(0x01)), unchecked((int)(0x65
			)), unchecked((int)(0x03)), unchecked((int)(0x04)), unchecked((int)(0x02)), unchecked(
			(int)(0x02)), unchecked((int)(0x04)), unchecked((int)(0x30)) };

        public static readonly byte[] SHA512_DIGEST_INFO_PREFIX = new byte[] { unchecked((int)(0x30
			)), unchecked((int)(0x4f)), unchecked((int)(0x30)), unchecked((int)(0x0b)), unchecked(
			(int)(0x06)), unchecked((int)(0x09)), unchecked((int)(0x60)), unchecked((byte)unchecked(
			(int)(0x86))), unchecked((int)(0x48)), unchecked((int)(0x01)), unchecked((int)(0x65
			)), unchecked((int)(0x03)), unchecked((int)(0x04)), unchecked((int)(0x02)), unchecked(
			(int)(0x03)), unchecked((int)(0x04)), unchecked((int)(0x40)) };

        public static readonly byte[] RIPEMD160_DIGEST_INFO_PREFIX = new byte[] { unchecked((int)(0x30
			)), unchecked((int)(0x1f)), unchecked((int)(0x30)), unchecked((int)(0x07)), unchecked(
			(int)(0x06)), unchecked((int)(0x05)), unchecked((int)(0x2b)), unchecked((int)(0x24
			)), unchecked((int)(0x03)), unchecked((int)(0x02)), unchecked((int)(0x01)), unchecked(
			(int)(0x04)), unchecked((int)(0x14)) };

        public static readonly byte[] RIPEMD128_DIGEST_INFO_PREFIX = new byte[] { unchecked((int)(0x30
			)), unchecked((int)(0x1b)), unchecked((int)(0x30)), unchecked((int)(0x07)), unchecked(
			(int)(0x06)), unchecked((int)(0x05)), unchecked((int)(0x2b)), unchecked((int)(0x24
			)), unchecked((int)(0x03)), unchecked((int)(0x02)), unchecked((int)(0x02)), unchecked(
			(int)(0x04)), unchecked((int)(0x10)) };

        public static readonly byte[] RIPEMD256_DIGEST_INFO_PREFIX = new byte[] { unchecked((int)(0x30
			)), unchecked((int)(0x2b)), unchecked((int)(0x30)), unchecked((int)(0x07)), unchecked(
			(int)(0x06)), unchecked((int)(0x05)), unchecked((int)(0x2b)), unchecked((int)(0x24
			)), unchecked((int)(0x03)), unchecked((int)(0x02)), unchecked((int)(0x03)), unchecked(
			(int)(0x04)), unchecked((int)(0x20)) };
    }
}
