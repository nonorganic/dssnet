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

using System.Collections.Generic;
using System.IO;
using EU.Europa.EC.Markt.Dss.Signature.Xades;
using EU.Europa.EC.Markt.Dss.Validation.Ades;
using Sharpen;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security.Certificates;
using Microsoft.Xades;
using System.Xml;
using Org.BouncyCastle.Utilities.Encoders;

namespace EU.Europa.EC.Markt.Dss.Validation.Xades
{
    /// <summary>Retrieve Certificates contained in a XAdES structure</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class XAdESCertificateSource : SignatureCertificateSource
    {
        private System.Xml.XmlElement signatureElement;

        private bool onlyExtended;

        /// <summary>The default constructor for XAdESCertificateSource.</summary>
        /// <remarks>The default constructor for XAdESCertificateSource.</remarks>
        /// <param name="signatureElement"></param>
        public XAdESCertificateSource(XmlElement signatureElement, bool onlyExtended)
        {
            this.signatureElement = signatureElement;
            this.onlyExtended = onlyExtended;
        }

        public override IList<X509Certificate> GetCertificates()
        {
            IList<X509Certificate> list = new AList<X509Certificate>();

            XmlNodeList nodes;

            nodes = XmlUtils.GetNodeList(signatureElement,
                "//ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties/xades:CertificateValues/xades:EncapsulatedX509Certificate");

            foreach (XmlNode node in nodes)
            {
                byte[] derEncoded = Base64.Decode(
                    System.Text.Encoding.ASCII.GetBytes(node.InnerText));
                X509Certificate cert = new X509CertificateParser().ReadCertificate(derEncoded);
                if (!list.Contains(cert))
                {
                    list.AddItem(cert);
                }
            }

            if (!onlyExtended)
            {
                nodes = XmlUtils.GetNodeList(signatureElement,
                    "//ds:KeyInfo/ds:X509Data/ds:X509Certificate");

                foreach (XmlNode node in nodes)
                {
                    byte[] derEncoded = Base64.Decode(
                        System.Text.Encoding.ASCII.GetBytes(node.InnerText));
                    X509Certificate cert = new X509CertificateParser().ReadCertificate(derEncoded);
                    if (!list.Contains(cert))
                    {
                        list.AddItem(cert);
                    }
                }
            }

            return list;
        }
    }
}
