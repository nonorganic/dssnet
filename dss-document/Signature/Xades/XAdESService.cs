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
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.IO;

namespace EU.Europa.EC.Markt.Dss.Signature.Xades
{
    /// <summary>XAdES implementation of DocumentSignatureService</summary>
    /// <version>$Revision: 2742 $ - $Date: 2013-10-17 12:32:08 +0200 (jeu., 17 oct. 2013) $
    /// 	</version>
    public class XAdESService : DocumentSignatureService
    {

        /// <param name="tspSource">the tspSource to set</param>
        public ITspSource TspSource { get; set; }

        /// <param name="verifier">the verifier to set</param>
        public CertificateVerifier Verifier { get; set; }

        private XAdESProfileBES GetSigningProfile(SignatureParameters parameters)
        {
            //jbonilla
            SignatureFormat signFormat = parameters.SignatureFormat;
            if (signFormat.Equals(SignatureFormat.XAdES_BES))
            {
                return new XAdESProfileBES();
            }
            //else
            //{
            //    if (signFormat.Equals(SignatureFormat.XAdES_EPES))
            //    {
            //        return new XAdESProfileEPES();
            //    }
            //}
            //TODO jbonilla EPES?
            return new XAdESProfileBES();
        }

        private SignatureExtension GetExtensionProfile(SignatureParameters parameters)
        {
            //jbonilla
            SignatureFormat signFormat = parameters.SignatureFormat;
            if (signFormat.Equals(SignatureFormat.XAdES_BES) || signFormat.Equals(SignatureFormat
                .XAdES_EPES))
            {
                return null;
            }
            else if (signFormat.Equals(SignatureFormat.XAdES_T))
            {
                XAdESProfileT extensionT = new XAdESProfileT();
                extensionT.SetTspSource(TspSource);
                return extensionT;

            }
            else if (signFormat.Equals(SignatureFormat.XAdES_C))
            {
                XAdESProfileC extensionC = new XAdESProfileC();
                extensionC.SetTspSource(TspSource);
                extensionC.SetCertificateVerifier(Verifier);
                return extensionC;
            }
            else if (signFormat.Equals(SignatureFormat.XAdES_X))
            {
                XAdESProfileX extensionX = new XAdESProfileX();
                extensionX.SetTspSource(TspSource);
                extensionX.SetCertificateVerifier(Verifier);
                return extensionX;
            }
            else if (signFormat.Equals(SignatureFormat.XAdES_XL))
            {
                XAdESProfileXL extensionXL = new XAdESProfileXL();
                extensionXL.SetTspSource(TspSource);
                extensionXL.SetCertificateVerifier(Verifier);
                return extensionXL;
            }
            //                else
            //                {
            //                    if (signFormat.Equals(SignatureFormat.XAdES_A))
            //                    {
            //                        throw new NotImplementedException();
            //                        //XAdESProfileA extensionA = new XAdESProfileA();
            //                        //extensionA.SetTspSource(tspSource);
            //                        //extensionA.SetCertificateVerifier(certificateVerifier);
            //                        //return extensionA;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            throw new ArgumentException("Unsupported signature format " + parameters.SignatureFormat);
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual EU.Europa.EC.Markt.Dss.Digest Digest(Document document, SignatureParameters
             parameters)
        {
            Stream input = ToBeSigned(document, parameters);
            byte[] data = Streams.ReadAll(input);
            byte[] digestValue = DigestUtilities.CalculateDigest(
                parameters.DigestAlgorithm.GetName(), data);
            return new EU.Europa.EC.Markt.Dss.Digest(parameters.DigestAlgorithm, digestValue);
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual Stream ToBeSigned(Document document, SignatureParameters parameters
            )
        {
            return GetSigningProfile(parameters).GetToBeSignedStream(document, parameters);
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual Document SignDocument(Document document, SignatureParameters parameters
            , byte[] signatureValue)
        {
            XAdESProfileBES profile = GetSigningProfile(parameters);
            Document signedDoc = profile.SignDocument(document, parameters, signatureValue);
            SignatureExtension extension = GetExtensionProfile(parameters);
            if (extension != null)
            {
                if (parameters.SignaturePackaging == SignaturePackaging.ENVELOPED)
                {
                    string signatureId = "sigId-" + profile.ComputeDeterministicId(parameters);
                    return extension.ExtendSignature(signatureId, signedDoc, document, parameters);
                }
                else
                {
                    return extension.ExtendSignatures(signedDoc, document, parameters);
                }
            }
            else
            {
                return signedDoc;
            }
        }

        /// <exception cref="System.IO.IOException"></exception>
        public virtual Document ExtendDocument(Document document, Document originalDocument
            , SignatureParameters parameters)
        {
            throw new NotImplementedException();
            //SignatureExtension extension = GetExtensionProfile(parameters);
            //if (extension != null)
            //{
            //    return extension.ExtendSignatures(document, originalDocument, parameters);
            //}
            //else
            //{
            //    return document;
            //}
        }
    }
}
