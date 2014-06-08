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
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Signature.Xades;
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.Ocsp;
using EU.Europa.EC.Markt.Dss.Validation.Xades;
using Org.BouncyCastle.Ocsp;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using Microsoft.Xades;
using Org.BouncyCastle.Security;

namespace EU.Europa.EC.Markt.Dss.Signature.Xades
{
    /// <summary>XL profile of XAdES signature</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class XAdESProfileXL : XAdESProfileX
    {
        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Signature.Xades.XAdESProfileXL
            ).FullName);

        /// <summary>The default constructor for XAdESProfileT.</summary>
        /// <remarks>The default constructor for XAdESProfileT.</remarks>
        public XAdESProfileXL()
            : base()
        {
        }

        protected internal override void ExtendSignatureTag(XadesSignedXml xadesSignedXml)
        {
            base.ExtendSignatureTag(xadesSignedXml);

            X509Certificate signingCertificate = DotNetUtilities.FromX509Certificate(
                xadesSignedXml.GetSigningCertificate());

            DateTime signingTime = xadesSignedXml.XadesObject.QualifyingProperties
                .SignedProperties.SignedSignatureProperties.SigningTime;

            ValidationContext ctx = certificateVerifier.ValidateCertificate(signingCertificate
                , signingTime, new XAdESCertificateSource(xadesSignedXml.GetXml(), false), null, null);

            UnsignedProperties unsignedProperties = null;
            //int certificateValuesCounter;
            CertificateValues certificateValues;
            EncapsulatedX509Certificate encapsulatedX509Certificate;
            RevocationValues revocationValues;
            CRLValue newCRLValue;
            OCSPValue newOCSPValue;

            unsignedProperties = xadesSignedXml.UnsignedProperties;

            //TODO jbonilla Validate certificate refs.
            {                
                unsignedProperties.UnsignedSignatureProperties.CertificateValues = new CertificateValues();
                certificateValues = unsignedProperties.UnsignedSignatureProperties.CertificateValues;
                //certificateValues.Id = this.certificateValuesIdTextBox.Text;
                //certificateValuesCounter = 0;

                foreach (CertificateAndContext certificate in ctx.GetNeededCertificates())
                {
                    encapsulatedX509Certificate = new EncapsulatedX509Certificate();
                    //encapsulatedX509Certificate.Id = this.certificateValuesIdTextBox.Text + certificateValuesCounter.ToString();
                    encapsulatedX509Certificate.PkiData = certificate.GetCertificate().GetEncoded();
                    //certificateValuesCounter++;
                    certificateValues.EncapsulatedX509CertificateCollection.Add(encapsulatedX509Certificate);
                }             
            }
            
            unsignedProperties = xadesSignedXml.UnsignedProperties;
            unsignedProperties.UnsignedSignatureProperties.RevocationValues = new RevocationValues();
            revocationValues = unsignedProperties.UnsignedSignatureProperties.RevocationValues;
            //revocationValues.Id = this.revocationValuesIdTextBox.Text;           

            if (ctx.GetNeededOCSPResp().Count > 0)
            {
                foreach(BasicOcspResp ocsp in ctx.GetNeededOCSPResp())
                {
                    newOCSPValue = new OCSPValue();
                    newOCSPValue.PkiData = OCSPUtils.FromBasicToResp(ocsp).GetEncoded();
                    revocationValues.OCSPValues.OCSPValueCollection.Add(newOCSPValue);
                }               
            }

            if (ctx.GetNeededCRL().Count > 0)
            {
                foreach (X509Crl crl in ctx.GetNeededCRL())
                {
                    newCRLValue = new CRLValue();
                    newCRLValue.PkiData = crl.GetEncoded();
                    revocationValues.CRLValues.CRLValueCollection.Add(newCRLValue);
                }                
            }           

            xadesSignedXml.UnsignedProperties = unsignedProperties;
        }
    }
}
