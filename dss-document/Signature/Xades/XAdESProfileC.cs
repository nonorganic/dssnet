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

using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.Xades;
using iTextSharp.text.log;
using Microsoft.Xades;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Sharpen;
using System;
using System.Security.Cryptography.Xml;
using MSXades = Microsoft.Xades;

namespace EU.Europa.EC.Markt.Dss.Signature.Xades
{
    /// <summary>Contains XAdES-C profile aspects</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class XAdESProfileC : XAdESProfileT
    {
        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Signature.Xades.XAdESProfileC
            ).FullName);

        public static readonly string XADES_NAMESPACE = "http://uri.etsi.org/01903/v1.3.2#";

        public static readonly string XADES141_NAMESPACE = "http://uri.etsi.org/01903/v1.4.1#";

        protected internal CertificateVerifier certificateVerifier;

        /// <summary>The default constructor for XAdESProfileT.</summary>
        /// <remarks>The default constructor for XAdESProfileT.</remarks>
        /// <exception cref="Javax.Xml.Datatype.DatatypeConfigurationException">Javax.Xml.Datatype.DatatypeConfigurationException
        /// 	</exception>
        public XAdESProfileC()
            : base()
        {
        }

        /// <param name="certificateVerifier">the certificateVerifier to set</param>
        public virtual void SetCertificateVerifier(CertificateVerifier certificateVerifier
            )
        {
            this.certificateVerifier = certificateVerifier;
        }

        private void IncorporateCertificateRefs(CompleteCertificateRefs completeCertificateRefs
            , ValidationContext ctx)
        {
            if (ctx.GetNeededCertificates().Count > 1)
            {
                foreach (CertificateAndContext certificate in ctx.GetNeededCertificates())
                {
                    X509Certificate x509Cert = certificate.GetCertificate();

                    //jbonilla Don't include signing certificate
                    if (!x509Cert.Equals(ctx.GetCertificate()))
                    {
                        Cert chainCert = new Cert();
                        chainCert.IssuerSerial.X509IssuerName = x509Cert.IssuerDN.ToString();
                        chainCert.IssuerSerial.X509SerialNumber = x509Cert.SerialNumber.ToString();
                        //TODO jbonilla DigestMethod parameter?
                        chainCert.CertDigest.DigestMethod.Algorithm = SignedXml.XmlDsigSHA1Url;
                        chainCert.CertDigest.DigestValue = DotNetUtilities.ToX509Certificate2(x509Cert).GetCertHash();
                        //unsignedProperties.UnsignedSignatureProperties.CompleteCertificateRefs.Id = "CompleteCertificateRefsId-" + this.uid;
                        completeCertificateRefs.CertRefs.CertCollection.Add(chainCert);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Needed certificates empty", "chain");
            }
        }

        private void IncorporateCRLRefs(CompleteRevocationRefs completeRevocationRefs
            , ValidationContext ctx)
        {
            if (!ctx.GetNeededCRL().IsEmpty())
            {
                var crl = ctx.GetNeededCRL()[0];
                
                //TODO jbonilla Digest parameter?
                byte[] crlDigest = DigestUtilities.CalculateDigest("SHA-1", crl.GetEncoded());

                MSXades.CRLRef incCRLRef = new MSXades.CRLRef();

                incCRLRef.CertDigest.DigestMethod.Algorithm = SignedXml.XmlDsigSHA1Url;
                incCRLRef.CertDigest.DigestValue = crlDigest;

                //incCRLRef.CRLIdentifier.UriAttribute = "";
                incCRLRef.CRLIdentifier.Issuer = crl.IssuerDN.ToString();
                incCRLRef.CRLIdentifier.IssueTime = crl.ThisUpdate;

                completeRevocationRefs.CRLRefs.CRLRefCollection.Add(incCRLRef);
            }
        }

        private void IncorporateOCSPRefs(CompleteRevocationRefs completeRevocationRefs
            , ValidationContext ctx)
        {
            if (!ctx.GetNeededOCSPResp().IsEmpty())
            {
                var ocsp = ctx.GetNeededOCSPResp()[0];                

                //TODO jbonill Digest parameter?
                byte[] ocspDigest = DigestUtilities.CalculateDigest("SHA-1", ocsp.GetEncoded());

                MSXades.OCSPRef incOCSPRef = new MSXades.OCSPRef();

                //TODO jbonilla Digest parameter?
                incOCSPRef.CertDigest.DigestMethod.Algorithm = SignedXml.XmlDsigSHA1Url;
                incOCSPRef.CertDigest.DigestValue = ocspDigest;

                //TODO jbonilla 
                //incOCSPRef.OCSPIdentifier.UriAttribute = "";
                incOCSPRef.OCSPIdentifier.ProducedAt = ocsp.ProducedAt;

                string responderIdText = "";

                RespID respId = ocsp.ResponderId;
                ResponderID ocspResponderId = respId.ToAsn1Object();

                DerTaggedObject derTaggedObject = (DerTaggedObject)ocspResponderId.ToAsn1Object();

                if (2 == derTaggedObject.TagNo)
                {
                    responderIdText = Convert.ToBase64String(ocspResponderId.GetKeyHash());
                }
                else
                {
                    responderIdText = ocspResponderId.Name.ToString();
                }

                incOCSPRef.OCSPIdentifier.ResponderID = responderIdText;

                completeRevocationRefs.OCSPRefs.OCSPRefCollection.Add(incOCSPRef);
            }
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

            UnsignedProperties unsignedProperties = xadesSignedXml.UnsignedProperties;

            var completeCertificateRefs = new CompleteCertificateRefs();
            IncorporateCertificateRefs(completeCertificateRefs, ctx);
            unsignedProperties.UnsignedSignatureProperties.CompleteCertificateRefs = completeCertificateRefs;

            var completeRevocationRefs = new CompleteRevocationRefs();
            IncorporateOCSPRefs(completeRevocationRefs, ctx);           
            IncorporateCRLRefs(completeRevocationRefs, ctx);
            unsignedProperties.UnsignedSignatureProperties.CompleteRevocationRefs = completeRevocationRefs;

            xadesSignedXml.UnsignedProperties = unsignedProperties;   
        }
    }
}
