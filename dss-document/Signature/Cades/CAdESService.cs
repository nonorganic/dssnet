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
using EU.Europa.EC.Markt.Dss.Validation.Tsp;
using iTextSharp.text.log;
using Org.BouncyCastle.Asn1.Cms;
//using Org.Apache.Commons.IO;
//using Org.BouncyCastle.Cert;
//using Org.BouncyCastle.Cert.Jcajce;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities.IO;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;
//using Org.BouncyCastle.Jce.Provider;
//using Org.BouncyCastle.Operator;
//using Org.BouncyCastle.Operator.BC;
using Sharpen;
using System;
using System.Collections;
using System.IO;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Signature.Cades
{
    /// <summary>CAdES implementation of DocumentSignatureService</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class CAdESService : DocumentSignatureService
    {
        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Signature.Cades.CAdESService
            ).FullName);

        /// <param name="tspSource">the tspSource to set</param>
        public ITspSource TspSource { get; set; }

        /// <param name="verifier">the verifier to set</param>
        public CertificateVerifier Verifier { get; set; }

        /// <summary>The default constructor for CAdESService.</summary>
        /// <remarks>The default constructor for CAdESService.</remarks>
        public CAdESService()
        {
            //jbonilla No se puede implementar en C#
            //Security.AddProvider(new BouncyCastleProvider());
        }

        /// <summary>Because some information are stored in the profile, a profile is not Thread-safe.
        /// 	</summary>
        /// <remarks>
        /// Because some information are stored in the profile, a profile is not Thread-safe. The software must create one
        /// for each request.
        /// </remarks>
        /// <returns>A new instance of signatureProfile corresponding to the parameters.</returns>
        private CAdESProfileBES GetSigningProfile(SignatureParameters parameters)
        {
            //jbonilla    	
            SignatureFormat signFormat = parameters.SignatureFormat;
            if (signFormat.Equals(SignatureFormat.CAdES_BES))
            {
                return new CAdESProfileBES();
            }
            else
            {
                if (signFormat.Equals(SignatureFormat.CAdES_EPES))
                {
                    return new CAdESProfileEPES();
                }
            }
            return new CAdESProfileEPES();
        }

        private CAdESSignatureExtension GetExtensionProfile(SignatureParameters parameters
            )
        {
            //jbonilla    	
            SignatureFormat signFormat = parameters.SignatureFormat;
            if (signFormat.Equals(SignatureFormat.CAdES_BES) || signFormat.Equals(SignatureFormat
                .CAdES_EPES))
            {
                return null;
            }
            else if (signFormat.Equals(SignatureFormat.CAdES_T))
            {
                CAdESProfileT extensionT = new CAdESProfileT();
                extensionT.SetSignatureTsa(TspSource);
                return extensionT;
            }
            else if (signFormat.Equals(SignatureFormat.CAdES_C))
            {
                CAdESProfileC extensionC = new CAdESProfileC();
                extensionC.SetSignatureTsa(TspSource);
                extensionC.SetCertificateVerifier(Verifier);
                return extensionC;
            }
            else if (signFormat.Equals(SignatureFormat.CAdES_X))
            {
                CAdESProfileX extensionX = new CAdESProfileX();
                extensionX.SetSignatureTsa(TspSource);
                extensionX.SetExtendedValidationType(1);
                extensionX.SetCertificateVerifier(Verifier);
                return extensionX;
            }
            else if (signFormat.Equals(SignatureFormat.CAdES_XL))
            {
                CAdESProfileXL extensionXL = new CAdESProfileXL();
                extensionXL.SetSignatureTsa(TspSource);
                extensionXL.SetExtendedValidationType(1);
                extensionXL.SetCertificateVerifier(Verifier);
                return extensionXL;
            }
            else if (signFormat.Equals(SignatureFormat.CAdES_A))
            {
                CAdESProfileA extensionA = new CAdESProfileA();
                extensionA.SetSignatureTsa(TspSource);
                extensionA.SetCertificateVerifier(Verifier);
                extensionA.SetExtendedValidationType(1);
                return extensionA;
            }

            throw new ArgumentException("Unsupported signature format " + parameters.SignatureFormat);
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual EU.Europa.EC.Markt.Dss.Digest Digest(Document document, SignatureParameters
             parameters)
        {
            byte[] digestValue = null;
            try
            {
                digestValue = DigestUtilities.CalculateDigest
                    (parameters.DigestAlgorithm.GetName(),
                    Streams.ReadAll(ToBeSigned(document, parameters)));
                return new EU.Europa.EC.Markt.Dss.Digest(parameters.DigestAlgorithm, digestValue
                    );
            }
            catch (NoSuchAlgorithmException e)
            {
                throw new RuntimeException(e);
            }
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual Stream ToBeSigned(Document document, SignatureParameters parameters
            )
        {
            if (parameters.SignaturePackaging != SignaturePackaging.ENVELOPING && parameters
                .SignaturePackaging != SignaturePackaging.DETACHED)
            {
                throw new ArgumentException("Unsupported signature packaging " + parameters.SignaturePackaging);
            }
            //jbonilla - No aplica para C#
            //SignatureInterceptorProvider provider = new SignatureInterceptorProvider();
            //Security.AddProvider(provider);
            //string jsAlgorithm = parameters.GetSignatureAlgorithm().GetJavaSignatureAlgorithm
            //    (parameters.GetDigestAlgorithm());
            //PreComputedContentSigner contentSigner = new PreComputedContentSigner(jsAlgorithm
            //    );
            PreComputedSigner signer = new PreComputedSigner();
            //CmsSignedDataGenerator generator = CreateCMSSignedDataGenerator(contentSigner, digestCalculatorProvider
            //    , parameters, GetSigningProfile(parameters), false, null);
            CmsSignedDataGenerator generator = CreateCMSSignedDataGenerator
                (signer, parameters, GetSigningProfile(parameters), false, null);

            byte[] toBeSigned = Streams.ReadAll(document.OpenStream());
            CmsProcessableByteArray content = new CmsProcessableByteArray(toBeSigned);
            try
            {
                bool includeContent = true;
                if (parameters.SignaturePackaging == SignaturePackaging.DETACHED)
                {
                    includeContent = false;
                }
                CmsSignedData signed = generator.Generate(content, includeContent);

                //jbonilla - El ISigner devuelve el mismo hash sin firmar para permitir
                //la generación de la firma por un medio externo, como un token.
                /*return new ByteArrayInputStream(contentSigner.GetByteOutputStream().ToByteArray());*/
                return new MemoryStream(signer.CurrentSignature());
            }
            catch (CmsException e)
            {
                throw new IOException("CmsException", e);
            }
        }

        /// <summary><inheritDoc></inheritDoc></summary>
        /// <exception cref="System.IO.IOException"></exception>
        public virtual Document SignDocument(Document document, SignatureParameters parameters
            , byte[] signatureValue)
        {
            if (parameters.SignaturePackaging != SignaturePackaging.ENVELOPING && parameters
                .SignaturePackaging != SignaturePackaging.DETACHED)
            {
                throw new ArgumentException("Unsupported signature packaging " + parameters.SignaturePackaging);
            }
            try
            {
                //jbonilla - No aplica para C#
                //string jsAlgorithm = parameters.GetSignatureAlgorithm().GetJavaSignatureAlgorithm
                //    (parameters.GetDigestAlgorithm());
                //PreComputedContentSigner cs = new PreComputedContentSigner(jsAlgorithm, signatureValue
                //    );
                PreComputedSigner s = new PreComputedSigner(signatureValue);

                //DigestCalculatorProvider digestCalculatorProvider = new BcDigestCalculatorProvider
                //    ();
                //CMSSignedDataGenerator generator = CreateCMSSignedDataGenerator(cs, digestCalculatorProvider
                //    , parameters, GetSigningProfile(parameters), true, null);
                CmsSignedDataGenerator generator = CreateCMSSignedDataGenerator(s, parameters
                    , GetSigningProfile(parameters), true, null);
                byte[] toBeSigned = Streams.ReadAll(document.OpenStream());
                CmsProcessableByteArray content = new CmsProcessableByteArray(toBeSigned);
                bool includeContent = true;
                if (parameters.SignaturePackaging == SignaturePackaging.DETACHED)
                {
                    includeContent = false;
                }
                CmsSignedData data = generator.Generate(content, includeContent);
                Document signedDocument = new CMSSignedDocument(data);
                CAdESSignatureExtension extension = GetExtensionProfile(parameters);
                if (extension != null)
                {
                    signedDocument = extension.ExtendSignatures(new CMSSignedDocument(data), document
                        , parameters);
                }
                return signedDocument;
            }
            catch (CmsException e)
            {
                throw new RuntimeException(e);
            }
        }

        /// <summary>Add a signature to the already CMS signed data document.</summary>
        /// <remarks>Add a signature to the already CMS signed data document.</remarks>
        /// <param name="_signedDocument"></param>
        /// <param name="parameters"></param>
        /// <param name="signatureValue"></param>
        /// <returns></returns>
        /// <exception cref="System.IO.IOException">System.IO.IOException</exception>
        public virtual Document AddASignatureToDocument(Document _signedDocument, SignatureParameters
             parameters, byte[] signatureValue)
        {
            if (parameters.SignaturePackaging != SignaturePackaging.ENVELOPING)
            {
                throw new ArgumentException("Unsupported signature packaging " + parameters.SignaturePackaging);
            }
            try
            {
                CmsSignedData originalSignedData = null;
                using (var stream = _signedDocument.OpenStream())
                {
                    originalSignedData = new CmsSignedData(stream);
                }

                //jbonilla - No aplica para C#
                //string jsAlgorithm = parameters.GetSignatureAlgorithm().GetJavaSignatureAlgorithm
                //    (parameters.GetDigestAlgorithm());
                //PreComputedContentSigner cs = new PreComputedContentSigner(jsAlgorithm, signatureValue
                //    );
                PreComputedSigner s = new PreComputedSigner(signatureValue);
                //DigestCalculatorProvider digestCalculatorProvider = new BcDigestCalculatorProvider
                //    ();
                //CMSSignedDataGenerator generator = CreateCMSSignedDataGenerator(cs, digestCalculatorProvider
                //    , parameters, GetSigningProfile(parameters), true, originalSignedData);
                CmsSignedDataGenerator generator = CreateCMSSignedDataGenerator(s, parameters
                    , GetSigningProfile(parameters), true, originalSignedData);

                //if (originalSignedData == null || originalSignedData.SignedContent.GetContent
                //    () == null)                
                if (originalSignedData == null || originalSignedData.SignedContent == null)
                {
                    throw new RuntimeException("Cannot retrieve orignal content");
                }
                //byte[] octetString = (byte[])originalSignedData.SignedContent.GetContent();
                //CmsProcessableByteArray content = new CmsProcessableByteArray(octetString);
                CmsProcessable content = originalSignedData.SignedContent;
                CmsSignedData data = generator.Generate(content, true);
                Document signedDocument = new CMSSignedDocument(data);
                CAdESSignatureExtension extension = GetExtensionProfile(parameters);
                if (extension != null)
                {
                    signedDocument = extension.ExtendSignatures(new CMSSignedDocument(data), null, parameters);
                }
                return signedDocument;
            }
            catch (CmsException e)
            {
                throw new RuntimeException(e);
            }
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual Document ExtendDocument(Document document, Document originalDocument
            , SignatureParameters parameters)
        {
            CAdESSignatureExtension extension = GetExtensionProfile(parameters);
            if (extension != null)
            {
                return extension.ExtendSignatures(document, originalDocument, parameters);
            }
            else
            {
                LOG.Info("No extension for " + parameters.SignatureFormat);
            }
            return document;
        }

        /// <exception cref="System.IO.IOException"></exception>
        //private CmsSignedDataGenerator CreateCMSSignedDataGenerator(ContentSigner contentSigner
        //    , DigestCalculatorProvider digestCalculatorProvider, SignatureParameters parameters
        //    , CAdESProfileBES cadesProfile, bool includeUnsignedAttributes, CmsSignedData originalSignedData
        //    )
        private CmsSignedDataGenerator CreateCMSSignedDataGenerator(ISigner signer
            , SignatureParameters parameters, CAdESProfileBES cadesProfile
            , bool includeUnsignedAttributes, CmsSignedData originalSignedData
            )
        {
            try
            {
                CmsSignedDataGenerator generator = new CmsSignedDataGenerator();
                X509Certificate signerCertificate = parameters.SigningCertificate;

                //X509CertificateHolder certHolder = new X509CertificateHolder(signerCertificate.GetEncoded());
                ArrayList certList = new ArrayList();
                certList.Add(signerCertificate);
                IX509Store certHolder = X509StoreFactory.Create("CERTIFICATE/COLLECTION",
                    new X509CollectionStoreParameters(certList));

                //jbonilla - El provider siempre es BC C#
                //SignerInfoGeneratorBuilder sigInfoGeneratorBuilder = new SignerInfoGeneratorBuilder
                //    (digestCalculatorProvider);

                CmsAttributeTableGenerator signedAttrGen = new DefaultSignedAttributeTableGenerator
                    (new AttributeTable(cadesProfile.GetSignedAttributes(parameters)));

                CmsAttributeTableGenerator unsignedAttrGen = new SimpleAttributeTableGenerator
                    ((includeUnsignedAttributes) ? new AttributeTable(cadesProfile.GetUnsignedAttributes
                    (parameters)) : null);

                //jbonilla - No existe ContentSigner en BC C#
                //SignerInfoGenerator sigInfoGen = sigInfoGeneratorBuilder.Build(contentSigner, certHolder);                

                //generator.AddSignerInfoGenerator(sigInfoGen);
                generator.SignerProvider = signer;
                generator.AddSigner(new NullPrivateKey(), signerCertificate, parameters.SignatureAlgorithm.GetOid()
                    , parameters.DigestAlgorithm.GetOid(), signedAttrGen, unsignedAttrGen);

                if (originalSignedData != null)
                {
                    generator.AddSigners(originalSignedData.GetSignerInfos());
                }
                //ICollection<X509Certificate> certs = new AList<X509Certificate>();
                IList certs = new ArrayList();
                //certs.AddItem(parameters.SigningCertificate);
                certs.Add(parameters.SigningCertificate);
                if (parameters.CertificateChain != null)
                {
                    foreach (X509Certificate c in parameters.CertificateChain)
                    {
                        if (!c.SubjectDN.Equals(parameters.SigningCertificate.SubjectDN))
                        {
                            //certs.AddItem(c);
                            certs.Add(c);
                        }
                    }
                }
                //JcaCertStore certStore = new JcaCertStore(certs);
                IX509Store certStore = X509StoreFactory.Create("Certificate/Collection",
                    new X509CollectionStoreParameters(certs));
                generator.AddCertificates(certStore);
                if (originalSignedData != null)
                {
                    generator.AddCertificates(originalSignedData.GetCertificates("Collection"));
                }
                return generator;
            }
            catch (CmsException e)
            {
                throw new IOException("CmsException", e);
            }
            catch (CertificateEncodingException e)
            {
                throw new IOException("CertificateEncodingException", e);
            }
            /*catch (OperatorCreationException e)
			{
				throw new IOException(e);
			}*/
        }
    }
}
