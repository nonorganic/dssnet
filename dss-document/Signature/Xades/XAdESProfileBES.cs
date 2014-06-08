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

using iTextSharp.text.log;
using Microsoft.Xades;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using SystemX509 = System.Security.Cryptography.X509Certificates;

namespace EU.Europa.EC.Markt.Dss.Signature.Xades
{
    /// <summary>Contains BES aspects of XAdES</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class XAdESProfileBES
    {
        private static readonly string XADES_TYPE = "http://uri.etsi.org/01903#SignedProperties";

        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Signature.Xades.XAdESProfileBES
            ).FullName);        

        /// <summary>The default constructor for XAdESProfileBES.</summary>
        /// <remarks>The default constructor for XAdESProfileBES.</remarks>
        public XAdESProfileBES()
        {
        }

        /// <summary>The ID of xades:SignedProperties is contained in the signed content of the xades Signature.
        /// 	</summary>
        /// <remarks>
        /// The ID of xades:SignedProperties is contained in the signed content of the xades Signature. We must create this
        /// ID in a deterministic way. The signingDate and signingCertificate are mandatory in the more basic level of
        /// signature, we use them as "seed" for generating the ID.
        /// </remarks>
        /// <param name="params"></param>
        /// <returns></returns>
        internal virtual string ComputeDeterministicId(SignatureParameters parameters)
        {
            try
            {
                IDigest digest = DigestUtilities.GetDigest("MD5");

                byte[] block;
                block = Encoding.ASCII.GetBytes(parameters.SigningDate.ToString());
                digest.BlockUpdate(block, 0, block.Length);
                block = parameters.SigningCertificate.GetEncoded();
                digest.BlockUpdate(block, 0, block.Length);

                string md5id = "id" + Hex.ToHexString(DigestUtilities.DoFinal(digest));
                return md5id;
            }
            catch (NoSuchAlgorithmException ex)
            {
                LOG.Error(ex.Message);
                throw new Exception("MD5 Algorithm not found !");
            }
            catch (CertificateEncodingException)
            {
                throw new Exception("Certificate encoding exception");
            }
        }

        protected internal virtual Stream GetToBeSignedStream(Document document, SignatureParameters
             parameters)
        {
            XmlDocument envelopedSignatureXmlDocument;
            XadesSignedXml xadesSignedXml;

            envelopedSignatureXmlDocument = XmlUtils.ToXmlDocument(document);
            xadesSignedXml = CreateFromXmlDocument(envelopedSignatureXmlDocument);

            string uid = ComputeDeterministicId(parameters);
            string signatureId = "sigId-" + uid;
            string signedId = "xades-" + uid;

            xadesSignedXml.Signature.Id = signatureId;

            XadesObject xadesObject = new XadesObject();
            //xadesObject.Id = "XadesObject";
            xadesObject.QualifyingProperties.SignedProperties.Id = signedId;
            xadesObject.QualifyingProperties.Target = "#" + signatureId;

            this.AddSignedSignatureProperties(
                xadesObject.QualifyingProperties.SignedProperties.SignedSignatureProperties,
                xadesObject.QualifyingProperties.SignedProperties.SignedDataObjectProperties,
                xadesObject.QualifyingProperties.UnsignedProperties.UnsignedSignatureProperties,
                parameters);

            xadesSignedXml.AddXadesObject(xadesObject);

            return xadesSignedXml.PreComputeSignature();
        }

        internal virtual Document SignDocument(Document document, SignatureParameters parameters
            , byte[] signatureValue)
        {
            XmlDocument envelopedSignatureXmlDocument;
            XadesSignedXml xadesSignedXml;

            envelopedSignatureXmlDocument = XmlUtils.ToXmlDocument(document);
            xadesSignedXml = CreateFromXmlDocument(envelopedSignatureXmlDocument);

            AddCertificateInfoToSignature(xadesSignedXml, parameters);

            string uid = ComputeDeterministicId(parameters);
            string signatureId = "sigId-" + uid;
            string signedId = "xades-" + uid;
            string signatureValueId = "sigValueId-" + uid;

            xadesSignedXml.Signature.Id = signatureId;

            XadesObject xadesObject = new XadesObject();
            //xadesObject.Id = "XadesObject";
            xadesObject.QualifyingProperties.SignedProperties.Id = signedId;
            xadesObject.QualifyingProperties.Target = "#" + signatureId;

            this.AddSignedSignatureProperties(
                xadesObject.QualifyingProperties.SignedProperties.SignedSignatureProperties,
                xadesObject.QualifyingProperties.SignedProperties.SignedDataObjectProperties,
                xadesObject.QualifyingProperties.UnsignedProperties.UnsignedSignatureProperties,
                parameters);

            xadesSignedXml.AddXadesObject(xadesObject);

            xadesSignedXml.ComputeExternalSignature(signatureValue);

            xadesSignedXml.SignatureValueId = signatureValueId;

            return XmlUtils.ToDocument(envelopedSignatureXmlDocument, xadesSignedXml);
        }

        // Create the XML that represents the transform. 
        private static XmlDsigXPathTransform CreateXPathTransform(string xpath, IDictionary<string, string> namespaces)
        {
            // create the XML that represents the transform
            XmlDocument doc = new XmlDocument();

            XmlElement xpathElem = doc.CreateElement("XPath");
            xpathElem.InnerText = xpath;

            // Add the namespaces that should be in scope for the XPath expression
            if (namespaces != null)
            {
                foreach (string ns in namespaces.Keys)
                {
                    XmlAttribute nsAttr = doc.CreateAttribute("xmlns", ns, "http://www.w3.org/2000/xmlns/");
                    nsAttr.Value = namespaces[ns];
                    xpathElem.Attributes.Append(nsAttr);
                }
            }

            // Build a transform from the inputs
            XmlDsigXPathTransform xpathTransform = new XmlDsigXPathTransform();
            xpathTransform.LoadInnerXml(xpathElem.SelectNodes("."));

            return xpathTransform;
        }

        #region Microsoft.Xades.TestClient

        private static XadesSignedXml CreateFromXmlDocument(XmlDocument envelopedSignatureXmlDocument)
        {
            XmlDsigEnvelopedSignatureTransform xmlDsigEnvelopedSignatureTransform;
            Reference reference;

            reference = new Reference();

            var xadesSignedXml = new XadesSignedXml(envelopedSignatureXmlDocument);

            reference.Uri = "";
            reference.Id = "xml_ref_id";

            //TODO jbonilla - Parameter?
            //reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
            reference.DigestMethod = SignedXml.XmlDsigSHA1Url;

            // ETSI TS 103 171 V2.1.1
            // 6.2.4 Transforms within ds:Reference element 
            {
                //XmlDsigC14NTransform xmlDsigC14NTransform = new XmlDsigC14NTransform();
                //reference.AddTransform(xmlDsigC14NTransform);
                xmlDsigEnvelopedSignatureTransform = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(xmlDsigEnvelopedSignatureTransform);

                //jbonilla - Para permitir multifirmas (co-firmas)            
                Dictionary<string, string> namespaces = new Dictionary<string, string>();
                namespaces.Add("ds", "http://www.w3.org/2000/09/xmldsig#");
                var xmlDsigXPathTransform = CreateXPathTransform("not(ancestor-or-self::ds:Signature)", namespaces);
                reference.AddTransform(xmlDsigXPathTransform);
            }
            xadesSignedXml.AddReference(reference);

            // ETSI TS 103 171 V2.1.1
            // 6.2.2 Canonicalization of ds:SignedInfo element            
            xadesSignedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigC14NTransformUrl;//"http://www.w3.org/2001/10/xml-exc-c14n#";

            //xadesSignedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
            xadesSignedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

            return xadesSignedXml;
        }

        private void AddSignedSignatureProperties(SignedSignatureProperties signedSignatureProperties,
            SignedDataObjectProperties signedDataObjectProperties,
            UnsignedSignatureProperties unsignedSignatureProperties,
            SignatureParameters parameters
            )
        {
            XmlDocument xmlDocument;
            Cert cert;
            SystemX509.X509Certificate2 x509Cert;

            x509Cert = DotNetUtilities.ToX509Certificate2(parameters.SigningCertificate);

            xmlDocument = new XmlDocument();

            cert = new Cert();
            cert.IssuerSerial.X509IssuerName = x509Cert.IssuerName.Name;
            cert.IssuerSerial.X509SerialNumber = x509Cert.SerialNumber;
            cert.CertDigest.DigestMethod.Algorithm = SignedXml.XmlDsigSHA1Url;
            cert.CertDigest.DigestValue = x509Cert.GetCertHash();
            signedSignatureProperties.SigningCertificate.CertCollection.Add(cert);

            signedSignatureProperties.SigningTime = parameters.SigningDate;

            signedSignatureProperties.SignaturePolicyIdentifier.SignaturePolicyImplied = true;

            DataObjectFormat newDataObjectFormat = new DataObjectFormat();

            //TODO jbonilla - Replace Description with text parameter
            newDataObjectFormat.Description = "Generado con 'intisign'";
            newDataObjectFormat.MimeType = "text/xml";
            newDataObjectFormat.ObjectReferenceAttribute = "#xml_ref_id";
            signedDataObjectProperties.DataObjectFormatCollection.Add(newDataObjectFormat);
        }

        private void AddCertificateInfoToSignature(XadesSignedXml xadesSignedXml, SignatureParameters parameters)
        {
            var key = parameters.SigningCertificate.GetPublicKey();

            if (key is RsaKeyParameters)
            {
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider();
                RSAParameters RSAKeyInfo = new RSAParameters();

                //Set RSAKeyInfo to the public key values. 
                RSAKeyInfo.Modulus = ((RsaKeyParameters)key).Modulus.ToByteArray();
                RSAKeyInfo.Exponent = ((RsaKeyParameters)key).Exponent.ToByteArray();

                rsaKey.ImportParameters(RSAKeyInfo);

                xadesSignedXml.SigningKey = rsaKey;

                KeyInfo keyInfo = new KeyInfo();

                // ETSI TS 103 171 V2.1.1
                // 6.2.1 Placement of the signing certificate
                // "b) In order to facilitate path-building, generators should include in the same ds:KeyInfo/X509Data element as 
                // in note a) all certificates not available to verifiers that can be used during path building."
                //keyInfo.AddClause(new KeyInfoX509Data(this.certificate));
                {
                    KeyInfoX509Data x509Data = new KeyInfoX509Data();
                    foreach (X509Certificate cert in parameters.CertificateChain)
                    {
                        x509Data.AddCertificate(DotNetUtilities.ToX509Certificate2(cert));
                        //TODO jbonilla validar más de uno?
                        //break;
                    }
                    keyInfo.AddClause(x509Data);
                }

                keyInfo.AddClause(new RSAKeyValue(rsaKey));

                xadesSignedXml.KeyInfo = keyInfo;
            }
            else
            {
                throw new ArgumentException("Only allowed RsaKeyParameters", "key");
            }
        }

        #endregion Microsoft.Xades.TestClient
    }
}
