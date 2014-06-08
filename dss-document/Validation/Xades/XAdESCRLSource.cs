///*
// * DSS - Digital Signature Services
// *
// * Copyright (C) 2011 European Commission, Directorate-General Internal Market and Services (DG MARKT), B-1049 Bruxelles/Brussel
// *
// * Developed by: 2011 ARHS Developments S.A. (rue Nicolas BovÃ© 2B, L-1253 Luxembourg) http://www.arhs-developments.com
// *
// * This file is part of the "DSS - Digital Signature Services" project.
// *
// * "DSS - Digital Signature Services" is free software: you can redistribute it and/or modify it under the terms of
// * the GNU Lesser General Public License as published by the Free Software Foundation, either version 2.1 of the
// * License, or (at your option) any later version.
// *
// * DSS is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
// * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public License along with
// * "DSS - Digital Signature Services".  If not, see <http://www.gnu.org/licenses/>.
// */

//using System.Collections.Generic;
//using System.IO;
//using EU.Europa.EC.Markt.Dss.Signature.Xades;
//using EU.Europa.EC.Markt.Dss.Validation.Ades;
//using Sharpen;
//using Org.BouncyCastle.X509;

//namespace EU.Europa.EC.Markt.Dss.Validation.Xades
//{
//    /// <summary>Retrieve CRL values from an XAdES (&gt;XL) signature.</summary>
//    /// <remarks>Retrieve CRL values from an XAdES (&gt;XL) signature.</remarks>
//    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
//    /// 	</version>
//    public class XAdESCRLSource : SignatureCRLSource
//    {
//        private static readonly string CRL_XPATH = "ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties/xades:RevocationValues/xades:CRLValues/xades:EncapsulatedCRLValue";

//        private System.Xml.XmlElement signatureElement;

//        /// <summary>The default constructor for XAdESCRLSource.</summary>
//        /// <remarks>The default constructor for XAdESCRLSource.</remarks>
//        /// <param name="signatureElement"></param>
//        public XAdESCRLSource(System.Xml.XmlElement signatureElement)
//        {
//            this.signatureElement = signatureElement;
//        }

//        public override IList<X509Crl> GetCRLsFromSignature()
//        {
//            IList<X509Crl> list = new AList<X509Crl>();
//            try
//            {
//                NodeList nodeList = (NodeList)XMLUtils.GetNodeList(signatureElement, CRL_XPATH);
//                for (int i = 0; i < nodeList.GetLength(); i++)
//                {
//                    Element certEl = (Element)nodeList.Item(i);
//                    CertificateFactory factory = CertificateFactory.GetInstance("X509");
//                    byte[] derEncoded = Base64.DecodeBase64(certEl.GetTextContent());
//                    X509Crl cert = (X509Crl)factory.GenerateCRL(new ByteArrayInputStream(derEncoded));
//                    list.AddItem(cert);
//                }
//            }
//            catch (CertificateException e)
//            {
//                throw new RuntimeException(e);
//            }
//            catch (CrlException e)
//            {
//                throw new RuntimeException(e);
//            }
//            return list;
//        }
//    }
//}
