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
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Signature.Xades;
using EU.Europa.EC.Markt.Dss.Validation.Xades;
using iTextSharp.text.log;
using Org.BouncyCastle.Security;
using Microsoft.Xades;
using System.Collections;
using System.Security.Cryptography.Xml;

namespace EU.Europa.EC.Markt.Dss.Signature.Xades
{
    /// <summary>X attributes of profile xades</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class XAdESProfileX : XAdESProfileC
    {
        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Signature.Xades.XAdESProfileX
            ).FullName);

        /// <summary>The default constructor for XAdESProfileT.</summary>
        /// <remarks>The default constructor for XAdESProfileT.</remarks>
        public XAdESProfileX()
            : base()
        {
        }

        protected internal override void ExtendSignatureTag(XadesSignedXml xadesSignedXml)
        {
            base.ExtendSignatureTag(xadesSignedXml);

            InjectXadesXInformation(xadesSignedXml);
        }

        private void InjectXadesXInformation(XadesSignedXml xadesSignedXml)
        {
            TimeStamp xadesXTimeStamp;
            ArrayList signatureValueElementXpaths;            
            byte[] signatureValueHash;            
            byte[] tsaTimeStamp;

            signatureValueElementXpaths = new ArrayList();

            signatureValueElementXpaths.Add("ds:Object/xsd:QualifyingProperties/xsd:UnsignedProperties/xsd:UnsignedSignatureProperties/xsd:CompleteCertificateRefs");
            signatureValueElementXpaths.Add("ds:Object/xsd:QualifyingProperties/xsd:UnsignedProperties/xsd:UnsignedSignatureProperties/xsd:CompleteRevocationRefs");
            
            signatureValueHash = ComputeHashValueOfElementList(xadesSignedXml.GetXml(), signatureValueElementXpaths);

            //jbonilla
            tsaTimeStamp = this.tspSource.GetTimeStampToken(signatureValueHash);
            xadesXTimeStamp = new TimeStamp("RefsOnlyTimeStamp");

            xadesXTimeStamp.EncapsulatedTimeStamp.PkiData = tsaTimeStamp;
            xadesXTimeStamp.CanonicalizationMethod.Algorithm = SignedXml.XmlDsigExcC14NTransformUrl;
            //xadesXTimeStamp.EncapsulatedTimeStamp.Id = "";

            //jbonilla Deprecated
            //foreach (string elementIdValue in elementIdValues)
            //{
            //    hashDataInfo = new HashDataInfo();
            //    hashDataInfo.UriAttribute = "#" + elementIdValue;
            //    xadesXTimeStamp.HashDataInfoCollection.Add(hashDataInfo);
            //}

            UnsignedProperties unsignedProperties = xadesSignedXml.UnsignedProperties;

            unsignedProperties.UnsignedSignatureProperties.RefsOnlyTimeStampFlag = true;
            unsignedProperties.UnsignedSignatureProperties.RefsOnlyTimeStampCollection.Add(xadesXTimeStamp);

            xadesSignedXml.UnsignedProperties = unsignedProperties;

        }
    }
}
