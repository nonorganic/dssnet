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

//using System;
//using System.Collections.Generic;
//using System.IO;
//using EU.Europa.EC.Markt.Dss;
//using EU.Europa.EC.Markt.Dss.Signature;
//using EU.Europa.EC.Markt.Dss.Signature.Provider;
//using EU.Europa.EC.Markt.Dss.Signature.Xades;
//using EU.Europa.EC.Markt.Dss.Validation;
//using EU.Europa.EC.Markt.Dss.Validation.Certificate;
//using EU.Europa.EC.Markt.Dss.Validation.X509;
//using EU.Europa.EC.Markt.Dss.Validation.Xades;
//using Org.BouncyCastle.Asn1.X500;
//using Org.BouncyCastle.Ocsp;
//using Org.BouncyCastle.Tsp;
//using Org.BouncyCastle.Cms;
//using Sharpen;
//using iTextSharp.text.log;
//using Org.BouncyCastle.X509;

//namespace EU.Europa.EC.Markt.Dss.Validation.Xades
//{
//    /// <summary>Parse an XAdES structure</summary>
//    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
//    /// 	</version>
//    public class XAdESSignature : AdvancedSignature
//    {
//        private static readonly ILogger LOG = ILogger.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Xades.XAdESSignature
//            ).FullName);

//        public static readonly string XADES_NAMESPACE = "http://uri.etsi.org/01903/v1.3.2#";

//        private System.Xml.XmlElement signatureElement;

//        /// <returns>the signatureElement</returns>
//        public virtual System.Xml.XmlElement GetSignatureElement()
//        {
//            return signatureElement;
//        }

//        /// <summary>The default constructor for XAdESSignature.</summary>
//        /// <remarks>The default constructor for XAdESSignature.</remarks>
//        /// <param name="signatureElement"></param>
//        public XAdESSignature(System.Xml.XmlElement signatureElement)
//        {
//            Init.Init();
//            if (signatureElement == null)
//            {
//                throw new ArgumentNullException("Must provide a signatureElement");
//            }
//            this.signatureElement = signatureElement;
//        }

//        public virtual SignatureFormat GetSignatureFormat()
//        {
//            return SignatureFormat.XAdES;
//        }

//        public virtual string GetSignatureAlgorithm()
//        {
//            try
//            {
//                return XMLUtils.GetElement(signatureElement, "./ds:SignedInfo/ds:SignatureMethod"
//                    ).GetAttribute("Algorithm");
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.SIGNATURE_METHOD_ERROR);
//            }
//        }

//        public virtual XAdESCertificateSource GetCertificateSource()
//        {
//            return new XAdESCertificateSource(signatureElement, false);
//        }

//        public virtual CertificateSource GetExtendedCertificateSource()
//        {
//            return new XAdESCertificateSource(signatureElement, true);
//        }

//        public virtual XAdESCRLSource GetCRLSource()
//        {
//            return new XAdESCRLSource(signatureElement);
//        }

//        public virtual XAdESOCSPSource GetOCSPSource()
//        {
//            return new XAdESOCSPSource(signatureElement);
//        }

//        public virtual X509Certificate GetSigningCertificate()
//        {
//            try
//            {
//                NodeList list = XMLUtils.GetNodeList(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:SignedProperties/xades:SignedSignatureProperties/"
//                     + "xades:SigningCertificate/xades:Cert");
//                for (int i = 0; i < list.GetLength(); i++)
//                {
//                    Element el = (Element)list.Item(i);
//                    Element issuerSubjectNameEl = XMLUtils.GetElement(el, "./xades:IssuerSerial/ds:X509IssuerName"
//                        );
//                    X500Name issuerName = new X500Name(issuerSubjectNameEl.GetTextContent());
//                    foreach (X509Certificate c in GetCertificateSource().GetCertificates())
//                    {
//                        X500Name cIssuer = new X500Name(c.GetIssuerX500Principal().GetName());
//                        if (cIssuer.Equals(issuerName))
//                        {
//                            return c;
//                        }
//                    }
//                }
//                return null;
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.SIGNING_CERTIFICATE_ENCODING);
//            }
//        }

//        public virtual DateTime GetSigningTime()
//        {
//            try
//            {
//                Element signingTimeEl = XMLUtils.GetElement(signatureElement, "ds:Object/xades:QualifyingProperties/xades:SignedProperties/xades:SignedSignatureProperties/"
//                     + "./xades:SigningTime");
//                if (signingTimeEl == null)
//                {
//                    return null;
//                }
//                string text = signingTimeEl.GetTextContent();
//                DatatypeFactory factory = DatatypeFactory.NewInstance();
//                XMLGregorianCalendar cal = factory.NewXMLGregorianCalendar(text);
//                return cal.ToGregorianCalendar().GetTime();
//            }
//            catch (DOMException e)
//            {
//                throw new RuntimeException(e);
//            }
//            catch (DatatypeConfigurationException e)
//            {
//                throw new RuntimeException(e);
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.SIGNING_TIME_ENCODING);
//            }
//        }

//        public virtual PolicyValue GetPolicyId()
//        {
//            try
//            {
//                Element policyId = XMLUtils.GetElement(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:SignedProperties/xades:SignedSignatureProperties/"
//                     + "xades:SignaturePolicyIdentifier");
//                if (policyId != null)
//                {
//                    Element el = XMLUtils.GetElement(policyId, "./xades:SignaturePolicyId/xades:SigPolicyId/xades:Identifier"
//                        );
//                    if (el != null)
//                    {
//                        return new PolicyValue(el.GetTextContent());
//                    }
//                    else
//                    {
//                        return new PolicyValue();
//                    }
//                }
//                else
//                {
//                    return null;
//                }
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.SIGNATURE_POLICY_ENCODING);
//            }
//        }

//        public virtual string GetLocation()
//        {
//            return null;
//        }

//        public virtual string[] GetClaimedSignerRoles()
//        {
//            NodeList list = XMLUtils.GetNodeList(signatureElement, "ds:Object/xades:QualifyingProperties/xades:SignedProperties/xades:SignedSignatureProperties/"
//                 + "xades:SignerRole/xades:ClaimedRoles/xades:ClaimedRole");
//            if (list.GetLength() == 0)
//            {
//                return null;
//            }
//            string[] roles = new string[list.GetLength()];
//            for (int i = 0; i < list.GetLength(); i++)
//            {
//                roles[i] = ((Element)list.Item(i)).GetTextContent();
//            }
//            return roles;
//        }

//        public virtual string GetContentType()
//        {
//            return "text/xml";
//        }

//        /// <exception cref="Javax.Xml.Xpath.XPathExpressionException"></exception>
//        private TimestampToken MakeTimestampToken(Element el, TimestampToken.TimestampType
//             timestampType)
//        {
//            Element timestampTokenNode = XMLUtils.GetElement(el, "./xades:EncapsulatedTimeStamp"
//                );
//            try
//            {
//                byte[] tokenbytes = Base64.DecodeBase64(timestampTokenNode.GetTextContent());
//                TimeStampToken tstoken = new TimeStampToken(new CMSSignedData(tokenbytes));
//                return new TimestampToken(tstoken, timestampType);
//            }
//            catch (Exception e)
//            {
//                throw new RuntimeException(e);
//            }
//        }

//        /// <exception cref="Javax.Xml.Xpath.XPathExpressionException"></exception>
//        private IList<TimestampToken> FindTimestampTokens(string elementName, TimestampToken.TimestampType
//             timestampType)
//        {
//            NodeList timestampsNodes = this.signatureElement.GetElementsByTagName(elementName
//                );
//            IList<TimestampToken> signatureTimestamps = new AList<TimestampToken>();
//            for (int i = 0; i < timestampsNodes.GetLength(); i++)
//            {
//                TimestampToken tstoken = MakeTimestampToken((Element)timestampsNodes.Item(i), timestampType
//                    );
//                if (tstoken != null)
//                {
//                    signatureTimestamps.AddItem(tstoken);
//                }
//            }
//            return signatureTimestamps;
//        }

//        public virtual IList<TimestampToken> GetSignatureTimestamps()
//        {
//            try
//            {
//                IList<TimestampToken> signatureTimestamps = new AList<TimestampToken>();
//                NodeList timestampsNodes = XMLUtils.GetNodeList(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:SignatureTimeStamp");
//                for (int i = 0; i < timestampsNodes.GetLength(); i++)
//                {
//                    TimestampToken tstoken = MakeTimestampToken((Element)timestampsNodes.Item(i), TimestampToken.TimestampType
//                        .SIGNATURE_TIMESTAMP);
//                    if (tstoken != null)
//                    {
//                        signatureTimestamps.AddItem(tstoken);
//                    }
//                }
//                return signatureTimestamps;
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.SIGNATURE_TIMESTAMP_ENCODING);
//            }
//        }

//        public virtual IList<TimestampToken> GetTimestampsX1()
//        {
//            try
//            {
//                IList<TimestampToken> signatureTimestamps = new AList<TimestampToken>();
//                NodeList timestampsNodes = XMLUtils.GetNodeList(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:SigAndRefsTimeStamp");
//                for (int i = 0; i < timestampsNodes.GetLength(); i++)
//                {
//                    TimestampToken tstoken = MakeTimestampToken((Element)timestampsNodes.Item(i), TimestampToken.TimestampType
//                        .VALIDATION_DATA_TIMESTAMP);
//                    if (tstoken != null)
//                    {
//                        signatureTimestamps.AddItem(tstoken);
//                    }
//                }
//                return signatureTimestamps;
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.TIMESTAMP_X1_ENCODING);
//            }
//        }

//        public virtual IList<TimestampToken> GetTimestampsX2()
//        {
//            try
//            {
//                IList<TimestampToken> signatureTimestamps = new AList<TimestampToken>();
//                NodeList timestampsNodes = XMLUtils.GetNodeList(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:RefsOnlyTimeStamp");
//                for (int i = 0; i < timestampsNodes.GetLength(); i++)
//                {
//                    TimestampToken tstoken = MakeTimestampToken((Element)timestampsNodes.Item(i), TimestampToken.TimestampType
//                        .VALIDATION_DATA_REFSONLY_TIMESTAMP);
//                    if (tstoken != null)
//                    {
//                        signatureTimestamps.AddItem(tstoken);
//                    }
//                }
//                return signatureTimestamps;
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.TIMESTAMP_X2_ENCODING);
//            }
//        }

//        public virtual IList<TimestampToken> GetArchiveTimestamps()
//        {
//            try
//            {
//                IList<TimestampToken> signatureTimestamps = new AList<TimestampToken>();
//                NodeList timestampsNodes = XMLUtils.GetNodeList(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades141:ArchiveTimeStamp");
//                for (int i = 0; i < timestampsNodes.GetLength(); i++)
//                {
//                    TimestampToken tstoken = MakeTimestampToken((Element)timestampsNodes.Item(i), TimestampToken.TimestampType
//                        .ARCHIVE_TIMESTAMP);
//                    if (tstoken != null)
//                    {
//                        signatureTimestamps.AddItem(tstoken);
//                    }
//                }
//                return signatureTimestamps;
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.ARCHIVE_TIMESTAMP_ENCODING);
//            }
//        }

//        public virtual IList<X509Certificate> GetCertificates()
//        {
//            return GetCertificateSource().GetCertificates();
//        }

//        public virtual bool CheckIntegrity(Document detachedDocument)
//        {
//            DOMValidateContext valContext = new DOMValidateContext(KeySelector.SingletonKeySelector
//                (GetSigningCertificate().GetPublicKey()), this.signatureElement);
//            if (detachedDocument != null)
//            {
//                valContext.SetURIDereferencer(new OneExternalFileURIDereferencer("detached-file", 
//                    detachedDocument));
//            }
//            XMLSignatureFactory factory = XMLSignatureFactory.GetInstance("DOM", new XMLDSigRI
//                ());
//            try
//            {
//                XMLSignature signature = factory.UnmarshalXMLSignature(valContext);
//                RecursiveIdBrowse(valContext, signatureElement);
//                bool r = signature.Validate(valContext);
//                return r;
//            }
//            catch (MarshalException e)
//            {
//                throw new RuntimeException(e);
//            }
//            catch (XMLSignatureException e)
//            {
//                throw new RuntimeException(e);
//            }
//        }

//        private void RecursiveIdBrowse(DOMValidateContext context, Element element)
//        {
//            for (int i = 0; i < element.GetChildNodes().GetLength(); i++)
//            {
//                Node node = element.GetChildNodes().Item(i);
//                if (node.GetNodeType() == Node.ELEMENT_NODE)
//                {
//                    Element childEl = (Element)node;
//                    if (childEl.HasAttribute("Id"))
//                    {
//                        context.SetIdAttributeNS(childEl, null, "Id");
//                    }
//                    RecursiveIdBrowse(context, childEl);
//                }
//            }
//        }

//        public virtual IList<AdvancedSignature> GetCounterSignatures()
//        {
//            // see ETSI TS 101 903 V1.4.2 (2010-12) pp. 38/39/40
//            try
//            {
//                NodeList counterSigs = XMLUtils.GetNodeList(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:CounterSignature");
//                if (counterSigs == null)
//                {
//                    return null;
//                }
//                IList<AdvancedSignature> xadesList = new AList<AdvancedSignature>();
//                for (int i = 0; i < counterSigs.GetLength(); i++)
//                {
//                    Element counterSigEl = (Element)counterSigs.Item(i);
//                    Element signatureEl = XMLUtils.GetElement(counterSigEl, "./ds:Signature");
//                    // Verify that the element is a proper signature by trying to build a XAdESSignature out of it
//                    EU.Europa.EC.Markt.Dss.Validation.Xades.XAdESSignature xCounterSig = new EU.Europa.EC.Markt.Dss.Validation.Xades.XAdESSignature
//                        (signatureEl);
//                    // Verify that there is a ds:Reference element with a Type set to: http://uri.etsi.org/01903#CountersignedSignature
//                    // (as per the XAdES spec)
//                    XMLSignatureFactory factory = XMLSignatureFactory.GetInstance("DOM");
//                    XMLSignature signature = factory.UnmarshalXMLSignature(new DOMStructure(signatureEl
//                        ));
//                    LOG.Info("Verifying countersignature References");
//                    foreach (object refobj in signature.GetSignedInfo().GetReferences())
//                    {
//                        Reference @ref = (Reference)refobj;
//                        if (@ref.GetType() != null && @ref.GetType().Equals("http://uri.etsi.org/01903#CountersignedSignature"
//                            ))
//                        {
//                            // Ok, this seems to be a countersignature
//                            // Verify that the digest is that of the signature value
//                            if (@ref.Validate(new DOMValidateContext(xCounterSig.GetSigningCertificate().GetPublicKey
//                                (), XMLUtils.GetElement(signatureElement, "./ds:SignatureValue"))))
//                            {
//                                LOG.Info("Reference verification succeeded, adding countersignature");
//                                xadesList.AddItem(xCounterSig);
//                            }
//                            else
//                            {
//                                LOG.Warning("Skipping countersignature because the Reference doesn't contain a hash of the embedding SignatureValue"
//                                    );
//                            }
//                            break;
//                        }
//                    }
//                }
//                return xadesList;
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.COUNTERSIGNATURE_ENCODING);
//            }
//            catch (MarshalException)
//            {
//                throw new EncodingException(EncodingException.MSG.COUNTERSIGNATURE_ENCODING);
//            }
//            catch (XMLSignatureException)
//            {
//                throw new EncodingException(EncodingException.MSG.COUNTERSIGNATURE_ENCODING);
//            }
//        }

//        public virtual IList<CertificateRef> GetCertificateRefs()
//        {
//            try
//            {
//                Element signingCertEl = XMLUtils.GetElement(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:CompleteCertificateRefs/xades:CertRefs");
//                if (signingCertEl == null)
//                {
//                    return null;
//                }
//                IList<CertificateRef> certIds = new AList<CertificateRef>();
//                NodeList certIdnodes = XMLUtils.GetNodeList(signingCertEl, "./xades:Cert");
//                for (int i = 0; i < certIdnodes.GetLength(); i++)
//                {
//                    Element certId = (Element)certIdnodes.Item(i);
//                    Element issuerNameEl = XMLUtils.GetElement(certId, "./xades:IssuerSerial/ds:X509IssuerName"
//                        );
//                    Element issuerSerialEl = XMLUtils.GetElement(certId, "./xades:IssuerSerial/ds:X509SerialNumber"
//                        );
//                    Element digestAlgorithmEl = XMLUtils.GetElement(certId, "./xades:CertDigest/ds:DigestMethod"
//                        );
//                    Element digestValueEl = XMLUtils.GetElement(certId, "./xades:CertDigest/ds:DigestValue"
//                        );
//                    CertificateRef genericCertId = new CertificateRef();
//                    if (issuerNameEl != null && issuerSerialEl != null)
//                    {
//                        genericCertId.SetIssuerName(issuerNameEl.GetTextContent());
//                        genericCertId.SetIssuerSerial(issuerSerialEl.GetTextContent());
//                    }
//                    string algorithm = digestAlgorithmEl.GetAttribute("Algorithm");
//                    genericCertId.SetDigestAlgorithm(GetShortAlgoName(algorithm));
//                    genericCertId.SetDigestValue(Base64.DecodeBase64(digestValueEl.GetTextContent()));
//                    certIds.AddItem(genericCertId);
//                }
//                return certIds;
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.CERTIFICATE_REF_ENCODING);
//            }
//        }

//        private string GetShortAlgoName(string longAlgoName)
//        {
//            if (DigestMethod.SHA1.Equals(longAlgoName))
//            {
//                return "SHA1";
//            }
//            else
//            {
//                if (DigestMethod.SHA256.Equals(longAlgoName))
//                {
//                    return "SHA256";
//                }
//                else
//                {
//                    if (DigestMethod.SHA512.Equals(longAlgoName))
//                    {
//                        return "SHA512";
//                    }
//                    else
//                    {
//                        throw new RuntimeException("Algorithm " + longAlgoName + " not supported");
//                    }
//                }
//            }
//        }

//        public virtual IList<CRLRef> GetCRLRefs()
//        {
//            try
//            {
//                IList<CRLRef> certIds = new AList<CRLRef>();
//                Element signingCertEl = XMLUtils.GetElement(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:CompleteRevocationRefs/xades:CRLRefs");
//                if (signingCertEl != null)
//                {
//                    NodeList certIdnodes = XMLUtils.GetNodeList(signingCertEl, "./xades:CRLRef");
//                    for (int i = 0; i < certIdnodes.GetLength(); i++)
//                    {
//                        Element certId = (Element)certIdnodes.Item(i);
//                        Element digestAlgorithmEl = XMLUtils.GetElement(certId, "./xades:DigestAlgAndValue/ds:DigestMethod"
//                            );
//                        Element digestValueEl = XMLUtils.GetElement(certId, "./xades:DigestAlgAndValue/ds:DigestValue"
//                            );
//                        string algorithm = digestAlgorithmEl.GetAttribute("Algorithm");
//                        string digestAlgo = GetShortAlgoName(algorithm);
//                        CRLRef @ref = new CRLRef();
//                        @ref.SetAlgorithm(digestAlgo);
//                        @ref.SetDigestValue(Base64.DecodeBase64(digestValueEl.GetTextContent()));
//                        certIds.AddItem(@ref);
//                    }
//                }
//                return certIds;
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.CRL_REF_ENCODING);
//            }
//        }

//        public virtual IList<OCSPRef> GetOCSPRefs()
//        {
//            try
//            {
//                IList<OCSPRef> certIds = new AList<OCSPRef>();
//                Element signingCertEl = XMLUtils.GetElement(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:CompleteRevocationRefs/xades:OCSPRefs");
//                if (signingCertEl != null)
//                {
//                    NodeList certIdnodes = XMLUtils.GetNodeList(signingCertEl, "./xades:OCSPRef");
//                    for (int i = 0; i < certIdnodes.GetLength(); i++)
//                    {
//                        Element certId = (Element)certIdnodes.Item(i);
//                        Element digestAlgorithmEl = XMLUtils.GetElement(certId, "./xades:DigestAlgAndValue/ds:DigestMethod"
//                            );
//                        Element digestValueEl = XMLUtils.GetElement(certId, "./xades:DigestAlgAndValue/ds:DigestValue"
//                            );
//                        if (digestAlgorithmEl == null || digestValueEl == null)
//                        {
//                            throw new NotETSICompliantException(NotETSICompliantException.MSG.XADES_DIGEST_ALG_AND_VALUE_ENCODING
//                                );
//                        }
//                        string algorithm = digestAlgorithmEl.GetAttribute("Algorithm");
//                        string digestAlgo = GetShortAlgoName(algorithm);
//                        certIds.AddItem(new OCSPRef(digestAlgo, Base64.DecodeBase64(digestValueEl.GetTextContent
//                            ()), false));
//                    }
//                }
//                return certIds;
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.OCSP_REF_ENCODING);
//            }
//        }

//        public virtual IList<X509Crl> GetCRLs()
//        {
//            return GetCRLSource().GetCRLsFromSignature();
//        }

//        public virtual IList<BasicOcspResp> GetOCSPs()
//        {
//            return GetOCSPSource().GetOCSPResponsesFromSignature();
//        }

//        private byte[] GetC14nValue(Node node)
//        {
//            try
//            {
//                Canonicalizer c14n = Canonicalizer.GetInstance(CanonicalizationMethod.EXCLUSIVE);
//                return c14n.CanonicalizeSubtree(node);
//            }
//            catch (InvalidCanonicalizerException e)
//            {
//                throw new RuntimeException("c14n algo error: " + e.Message, e);
//            }
//            catch (CanonicalizationException e)
//            {
//                throw new RuntimeException("c14n error: " + e.Message, e);
//            }
//        }

//        private byte[] GetC14nValue(IList<Node> nodeList)
//        {
//            try
//            {
//                ByteArrayOutputStream buffer = new ByteArrayOutputStream();
//                foreach (Node node in nodeList)
//                {
//                    Canonicalizer c14n = Canonicalizer.GetInstance(CanonicalizationMethod.EXCLUSIVE);
//                    buffer.Write(c14n.CanonicalizeSubtree(node));
//                }
//                return buffer.ToByteArray();
//            }
//            catch (IOException e)
//            {
//                throw new RuntimeException(e);
//            }
//            catch (InvalidCanonicalizerException e)
//            {
//                throw new RuntimeException("c14n algo error: " + e.Message, e);
//            }
//            catch (CanonicalizationException e)
//            {
//                throw new RuntimeException("c14n error: " + e.Message, e);
//            }
//        }

//        public virtual byte[] GetSignatureTimestampData()
//        {
//            try
//            {
//                Element signatureValue = XMLUtils.GetElement(signatureElement, "./ds:SignatureValue"
//                    );
//                return GetC14nValue(signatureValue);
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.SIGNATURE_TIMESTAMP_DATA_ENCODING
//                    );
//            }
//        }

//        public virtual byte[] GetTimestampX1Data()
//        {
//            try
//            {
//                IList<Node> timeStampNodesXadesX1 = new AList<Node>();
//                Element signatureValue = XMLUtils.GetElement(signatureElement, "./ds:SignatureValue"
//                    );
//                timeStampNodesXadesX1.AddItem(signatureValue);
//                NodeList signatureTimeStampNode = XMLUtils.GetNodeList(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:SignatureTimeStamp");
//                if (signatureTimeStampNode != null)
//                {
//                    for (int i = 0; i < signatureTimeStampNode.GetLength(); i++)
//                    {
//                        timeStampNodesXadesX1.AddItem(signatureTimeStampNode.Item(i));
//                    }
//                }
//                Node completeCertificateRefsNode = XMLUtils.GetElement(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:CompleteCertificateRefs");
//                if (completeCertificateRefsNode != null)
//                {
//                    timeStampNodesXadesX1.AddItem(completeCertificateRefsNode);
//                }
//                Node completeRevocationRefsNode = XMLUtils.GetElement(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:CompleteRevocationRefs");
//                if (completeRevocationRefsNode != null)
//                {
//                    timeStampNodesXadesX1.AddItem(completeRevocationRefsNode);
//                }
//                return GetC14nValue(timeStampNodesXadesX1);
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.TIMESTAMP_X1_DATA_ENCODING);
//            }
//        }

//        public virtual byte[] GetTimestampX2Data()
//        {
//            try
//            {
//                IList<Node> timeStampNodesXadesX1 = new AList<Node>();
//                Node completeCertificateRefsNode = XMLUtils.GetElement(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "/xades:CompleteCertificateRefs");
//                if (completeCertificateRefsNode != null)
//                {
//                    timeStampNodesXadesX1.AddItem(completeCertificateRefsNode);
//                }
//                Node completeRevocationRefsNode = XMLUtils.GetElement(signatureElement, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                     + "xades:CompleteRevocationRefs");
//                if (completeRevocationRefsNode != null)
//                {
//                    timeStampNodesXadesX1.AddItem(completeRevocationRefsNode);
//                }
//                return GetC14nValue(timeStampNodesXadesX1);
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.TIMESTAMP_X2_DATA_ENCODING);
//            }
//        }

//        /// <exception cref="System.IO.IOException"></exception>
//        public virtual byte[] GetArchiveTimestampData(int index, Document originalData)
//        {
//            try
//            {
//                ByteArrayOutputStream buffer = new ByteArrayOutputStream();
//                XMLStructure s = new DOMStructure(signatureElement);
//                XMLSignatureFactory factory = XMLSignatureFactory.GetInstance("DOM", new XMLDSigRI
//                    ());
//                DOMXMLSignature signature = (DOMXMLSignature)factory.UnmarshalXMLSignature(s);
//                DOMSignContext signContext = new DOMSignContext(new SpecialPrivateKey(), signatureElement
//                    );
//                signContext.PutNamespacePrefix(XMLSignature.XMLNS, "ds");
//                signContext.SetProperty("javax.xml.crypto.dsig.cacheReference", true);
//                signContext.SetURIDereferencer(new OneExternalFileURIDereferencer("detached-file"
//                    , originalData));
//                // TODO naramsda: check ! Don't let met publish that without further test !!
//                // DocumentBuilderFactory dbf = DocumentBuilderFactory.newInstance();
//                // dbf.setNamespaceAware(true);
//                // org.w3c.dom.Document xmlDoc = dbf.newDocumentBuilder().newDocument();
//                // signature.marshal(xmlDoc.createElement("test"), "ds", signContext);
//                foreach (object o in signature.GetSignedInfo().GetReferences())
//                {
//                    DOMReference r = (DOMReference)o;
//                    InputStream data = r.GetDigestInputStream();
//                    if (data != null)
//                    {
//                        IOUtils.Copy(data, buffer);
//                    }
//                }
//                IList<Node> timeStampNodesXadesA = new List<Node>();
//                Element signedInfo = XMLUtils.GetElement(signatureElement, "./ds:SignedInfo");
//                timeStampNodesXadesA.AddItem(signedInfo);
//                Element signatureValue = XMLUtils.GetElement(signatureElement, "./ds:SignatureValue"
//                    );
//                timeStampNodesXadesA.AddItem(signatureValue);
//                Element keyInfo = XMLUtils.GetElement(signatureElement, "./ds:KeyInfo");
//                timeStampNodesXadesA.AddItem(keyInfo);
//                Element unsignedSignaturePropertiesNode = GetUnsignedSignatureProperties(signatureElement
//                    );
//                NodeList unsignedProperties = unsignedSignaturePropertiesNode.GetChildNodes();
//                int count = 0;
//                for (int i = 0; i < unsignedProperties.GetLength(); i++)
//                {
//                    if (unsignedProperties.Item(i).GetNodeType() == Node.ELEMENT_NODE)
//                    {
//                        Element unsignedProperty = (Element)unsignedProperties.Item(i);
//                        if ("ArchiveTimeStamp".Equals(unsignedProperty.GetLocalName()))
//                        {
//                            if (count == index)
//                            {
//                                LOG.Info("We only need data up to ArchiveTimeStamp index " + index);
//                                break;
//                            }
//                            count++;
//                        }
//                        timeStampNodesXadesA.AddItem(unsignedProperty);
//                    }
//                }
//                buffer.Write(GetC14nValue(timeStampNodesXadesA));
//                return buffer.ToByteArray();
//            }
//            catch (MarshalException e)
//            {
//                //        } catch (ParserConfigurationException e) {
//                //            throw new IOException("Error when computing the archive data", e);
//                throw new IOException("Error when computing the archive data", e);
//            }
//            catch (XPathExpressionException)
//            {
//                throw new EncodingException(EncodingException.MSG.ARCHIVE_TIMESTAMP_DATA_ENCODING
//                    );
//            }
//        }

//        private Element GetUnsignedSignatureProperties(Element signatureEl)
//        {
//            try
//            {
//                Element unsignedSignaturePropertiesNode = XMLUtils.GetElement(signatureEl, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties/xades:UnsignedSignatureProperties"
//                    );
//                if (unsignedSignaturePropertiesNode == null)
//                {
//                    Element qualifyingProperties = XMLUtils.GetElement(signatureEl, "./ds:Object/xades:QualifyingProperties"
//                        );
//                    Element unsignedProperties = XMLUtils.GetElement(qualifyingProperties, "./ds:Object/xades:QualifyingProperties/xades:UnsignedProperties"
//                        );
//                    if (unsignedProperties == null)
//                    {
//                        unsignedProperties = qualifyingProperties.GetOwnerDocument().CreateElementNS(XADES_NAMESPACE
//                            , "UnsignedProperties");
//                        qualifyingProperties.AppendChild(unsignedProperties);
//                    }
//                    unsignedSignaturePropertiesNode = unsignedProperties.GetOwnerDocument().CreateElementNS
//                        (XADES_NAMESPACE, "UnsignedSignatureProperties");
//                    unsignedProperties.AppendChild(unsignedSignaturePropertiesNode);
//                }
//                return unsignedSignaturePropertiesNode;
//            }
//            catch (XPathExpressionException)
//            {
//                // Should never happens
//                throw new RuntimeException("Cannot build unsigned signature properties");
//            }
//        }
//    }
//}
