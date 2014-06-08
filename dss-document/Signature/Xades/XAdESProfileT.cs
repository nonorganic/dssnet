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

using System;
using System.Collections.Generic;
using System.IO;
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Signature.Xades;
using EU.Europa.EC.Markt.Dss.Validation.Tsp;
//using EU.Europa.EC.Markt.Dss.Validation.Xades;
using Org.BouncyCastle.Tsp;
using Sharpen;
using iTextSharp.text.log;
using Microsoft.Xades;
using System.Collections;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.IO;
using Org.BouncyCastle.Security;

namespace EU.Europa.EC.Markt.Dss.Signature.Xades
{
    /// <summary>-T profile of XAdES signature</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class XAdESProfileT : SignatureExtension
    {
        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Signature.Xades.XAdESProfileT
            ).FullName);

        internal ITspSource tspSource;        

        /// <summary>The default constructor for XAdESProfileT.</summary>
        /// <remarks>The default constructor for XAdESProfileT.</remarks>
        public XAdESProfileT()
            : base()
        {
        }

        /// <param name="tspSource">the tspSource to set</param>
        public virtual void SetTspSource(ITspSource tspSource)
        {
            this.tspSource = tspSource;
        }

        protected internal virtual void ExtendSignatureTag(XadesSignedXml xadesSignedXml)
        {
            RequestTimeStamp(xadesSignedXml);
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual Document ExtendSignatures(Document document, Document originalData
            , SignatureParameters parameters)
        {
            InputStream input = document.OpenStream();
            if (this.tspSource == null)
            {
                throw new ConfigurationException(ConfigurationException.MSG.CONFIGURE_TSP_SERVER);
            }
            try
            {
                throw new NotImplementedException();
            }
            finally
            {
                if (input != null)
                {
                    input.Close();
                }
            }
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual Document ExtendSignature(object signatureId, Document document, Document
             originalData, SignatureParameters parameters)
        {
            if (this.tspSource == null)
            {
                throw new ConfigurationException(ConfigurationException.MSG.CONFIGURE_TSP_SERVER);
            }

            XmlDocument envelopedSignatureXmlDocument;
            XmlDocument xadesDocument;
            XmlElement signature;            
            XadesSignedXml xadesSignedXml;

            xadesDocument = XmlUtils.ToXmlDocument(document);
            xadesDocument.PreserveWhitespace = true;
            xadesDocument.Load(document.OpenStream());

            xadesSignedXml = new XadesSignedXml(xadesDocument.DocumentElement); //Needed if it is a enveloped signature document

            signature = xadesSignedXml.GetIdElement(xadesDocument, (string)signatureId);
            
            xadesSignedXml.LoadXml(signature);

            ExtendSignatureTag(xadesSignedXml);

            envelopedSignatureXmlDocument = XmlUtils.ToXmlDocument(originalData);
            return XmlUtils.ToDocument(envelopedSignatureXmlDocument, xadesSignedXml);
        }

        private void RequestTimeStamp(XadesSignedXml xadesSignedXml)
        {
            TimeStamp signatureTimeStamp;
            ArrayList signatureValueElementXpaths;
            byte[] signatureValueHash;
            byte[] tsaTimeStamp;

            signatureValueElementXpaths = new ArrayList();
            signatureValueElementXpaths.Add("ds:SignatureValue");            
            signatureValueHash = ComputeHashValueOfElementList(xadesSignedXml.GetXml(), signatureValueElementXpaths);

            //jbonilla
            tsaTimeStamp = this.tspSource.GetTimeStampToken(signatureValueHash);

            signatureTimeStamp = new TimeStamp("SignatureTimeStamp");
            //signatureTimeStamp.EncapsulatedTimeStamp.Id = "SignatureTimeStamp" + this.uid;
            signatureTimeStamp.EncapsulatedTimeStamp.PkiData = tsaTimeStamp;
            signatureTimeStamp.CanonicalizationMethod.Algorithm = SignedXml.XmlDsigExcC14NTransformUrl;

            //jbonilla Deprecated
            //HashDataInfo hashDataInfo = new HashDataInfo();
            //hashDataInfo.UriAttribute = "#" + elementIdValues[0];
            //signatureTimeStamp.HashDataInfoCollection.Add(hashDataInfo);

            UnsignedProperties unsignedProperties = xadesSignedXml.UnsignedProperties;
            unsignedProperties.UnsignedSignatureProperties.SignatureTimeStampCollection.Add(signatureTimeStamp);
            xadesSignedXml.UnsignedProperties = unsignedProperties;

            //TODO jbonilla - Delete?
            XmlElement xml = xadesSignedXml.XadesObject.GetXml();
            XmlElement xml1 = xadesSignedXml.GetXml();
        }

        public byte[] ComputeHashValueOfElementList(XmlElement signatureXmlElement, ArrayList elementXpaths)
        {
            XmlDocument xmlDocument;
            XmlNamespaceManager xmlNamespaceManager;
            XmlNodeList searchXmlNodeList;
            XmlElement composedXmlElement;
            XmlDsigExcC14NTransform xmlTransform;

            Stream canonicalizedStream;
            //SHA1 sha1Managed;
            byte[] retVal;

            xmlDocument = signatureXmlElement.OwnerDocument;
            composedXmlElement = xmlDocument.CreateElement("ComposedElement", SignedXml.XmlDsigNamespaceUrl);
            xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            xmlNamespaceManager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);
            xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);
            foreach (string elementXpath in elementXpaths)
            {
                searchXmlNodeList = signatureXmlElement.SelectNodes(elementXpath, xmlNamespaceManager);
                if (searchXmlNodeList.Count == 0)
                {
                    throw new CryptographicException("Element " + elementXpath + " not found while calculating hash");
                }
                foreach (XmlNode xmlNode in searchXmlNodeList)
                {
                    //jbonilla Id attr deprecated
                    //if (((XmlElement)xmlNode).HasAttribute("Id"))
                    //{
                    //    elementIdValues.Add(((XmlElement)xmlNode).Attributes["Id"].Value);
                    //    composedXmlElement.AppendChild(xmlNode);
                    //}
                    //else
                    //{
                    //    throw new CryptographicException("Id attribute missing on " + xmlNode.LocalName + " element");
                    //}
                    composedXmlElement.AppendChild(xmlNode);
                }
            }

            //Initialise the stream to read the node list
            MemoryStream nodeStream = new MemoryStream();
            XmlWriter xw = XmlWriter.Create(nodeStream);
            composedXmlElement.ChildNodes[0].WriteTo(xw);
            xw.Flush();
            nodeStream.Position = 0;

            //modificado
            xmlTransform = new XmlDsigExcC14NTransform();
            xmlTransform.LoadInput(nodeStream);

            canonicalizedStream = (Stream)xmlTransform.GetOutput(typeof(Stream));

            //sha1Managed = new SHA1Managed();
            //retVal = sha1Managed.ComputeHash(canonicalizedStream);
            IDigest digest = this.tspSource.GetMessageDigest();
            byte[] canonicalizedBytes = Streams.ReadAll(canonicalizedStream);
            digest.BlockUpdate(canonicalizedBytes, 0, canonicalizedBytes.Length);
            retVal = DigestUtilities.DoFinal(digest);

            canonicalizedStream.Close();

            return retVal;
        }
    }
}
