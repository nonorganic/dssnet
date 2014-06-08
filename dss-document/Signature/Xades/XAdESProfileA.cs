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

//using System.IO;
//using EU.Europa.EC.Markt.Dss;
//using EU.Europa.EC.Markt.Dss.Signature;
//using EU.Europa.EC.Markt.Dss.Signature.Xades;
//using EU.Europa.EC.Markt.Dss.Validation.Xades;
//using Sharpen;
//using iTextSharp.text.log;

//namespace EU.Europa.EC.Markt.Dss.Signature.Xades
//{
//    /// <summary>Holds level A aspects of xades</summary>
//    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
//    /// 	</version>
//    public class XAdESProfileA : XAdESProfileXL
//    {
//        private static readonly ILogger LOG = ILogger.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Signature.Xades.XAdESProfileA
//            ).FullName);

//        private ObjectFactory GetXades14ObjectFactory()
//        {
//            return new ObjectFactory();
//        }

//        /// <summary>The default constructor for XAdESProfileT.</summary>
//        /// <remarks>The default constructor for XAdESProfileT.</remarks>
//        public XAdESProfileA() : base()
//        {
//        }

//        /// <exception cref="Javax.Xml.Xpath.XPathExpressionException"></exception>
//        private Element GetUnsignedSignatureProperties(Element signatureEl)
//        {
//            Element unsignedSignaturePropertiesNode = XMLUtils.GetElement(signatureEl, "//xades:UnsignedSignatureProperties"
//                );
//            if (unsignedSignaturePropertiesNode == null)
//            {
//                Element qualifyingProperties = XMLUtils.GetElement(signatureEl, "//xades:QualifyingProperties"
//                    );
//                Element unsignedProperties = XMLUtils.GetElement(qualifyingProperties, "//xades:UnsignedProperties"
//                    );
//                if (unsignedProperties == null)
//                {
//                    unsignedProperties = qualifyingProperties.GetOwnerDocument().CreateElementNS(XADES_NAMESPACE
//                        , "UnsignedProperties");
//                    qualifyingProperties.AppendChild(unsignedProperties);
//                }
//                unsignedSignaturePropertiesNode = unsignedProperties.GetOwnerDocument().CreateElementNS
//                    (XADES_NAMESPACE, "UnsignedSignatureProperties");
//                unsignedProperties.AppendChild(unsignedSignaturePropertiesNode);
//            }
//            return unsignedSignaturePropertiesNode;
//        }

//        protected internal override void ExtendSignatureTag(Element signatureEl, Document
//             originalData, SignatureFormat signatureFormat)
//        {
//            base.ExtendSignatureTag(signatureEl, originalData, signatureFormat);
//            try
//            {
//                XAdESSignature signature = new XAdESSignature(signatureEl);
//                MessageDigest digest = MessageDigest.GetInstance(DigestAlgorithm.SHA1.GetName());
//                digest.Update(signature.GetArchiveTimestampData(-1, originalData));
//                byte[] digestValue = digest.Digest();
//                XAdESTimeStampType timeStampXadesA = CreateXAdESTimeStamp(DigestAlgorithm.SHA1, digestValue
//                    );
//                Element unsignedSignaturePropertiesNode = GetUnsignedSignatureProperties(signatureEl
//                    );
//                marshaller.Marshal(GetXades14ObjectFactory().CreateArchiveTimeStamp(timeStampXadesA
//                    ), unsignedSignaturePropertiesNode);
//            }
//            catch (XPathExpressionException e)
//            {
//                throw new RuntimeException(e);
//            }
//            catch (JAXBException e)
//            {
//                throw new RuntimeException("JAXB error: " + e.Message, e);
//            }
//            catch (IOException e)
//            {
//                throw new RuntimeException(e);
//            }
//            catch (NoSuchAlgorithmException e)
//            {
//                throw new RuntimeException(e);
//            }
//        }
//    }
//}
