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

//namespace EU.Europa.EC.Markt.Dss.Signature.Xades
//{
//    /// <summary>EPES profile for XAdES</summary>
//    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
//    /// 	</version>
//    public class XAdESProfileEPES : XAdESProfileBES
//    {
//        /// <summary>The default constructor for XAdESProfileEPES.</summary>
//        /// <remarks>The default constructor for XAdESProfileEPES.</remarks>
//        public XAdESProfileEPES() : base()
//        {
//        }

//        protected internal override QualifyingPropertiesType CreateXAdESQualifyingProperties
//            (SignatureParameters @params, string signedInfoId, string dataFormatRef, string 
//            dataFormatMimetype)
//        {
//            QualifyingPropertiesType qualifyingProperties = base.CreateXAdESQualifyingProperties
//                (@params, signedInfoId, dataFormatRef, dataFormatMimetype);
//            SignaturePolicyIdType policyId = GetXades13ObjectFactory().CreateSignaturePolicyIdType
//                ();
//            SignaturePolicyIdentifierType policyIdentifier = GetXades13ObjectFactory().CreateSignaturePolicyIdentifierType
//                ();
//            switch (@params.GetSignaturePolicy())
//            {
//                case SignaturePolicy.IMPLICIT:
//                {
//                    policyIdentifier.SetSignaturePolicyImplied(string.Empty);
//                    qualifyingProperties.GetSignedProperties().GetSignedSignatureProperties().SetSignaturePolicyIdentifier
//                        (policyIdentifier);
//                    break;
//                }

//                case SignaturePolicy.EXPLICIT:
//                {
//                    ObjectIdentifierType objectId = GetXades13ObjectFactory().CreateObjectIdentifierType
//                        ();
//                    IdentifierType id = GetXades13ObjectFactory().CreateIdentifierType();
//                    id.SetValue(@params.GetSignaturePolicyId());
//                    objectId.SetIdentifier(id);
//                    policyId.SetSigPolicyId(objectId);
//                    if (@params.GetSignaturePolicyHashAlgo() != null && @params.GetSignaturePolicyHashValue
//                        () != null)
//                    {
//                        DigestAlgAndValueType hash = GetXades13ObjectFactory().CreateDigestAlgAndValueType
//                            ();
//                        DigestMethodType digestAlgo = GetDsObjectFactory().CreateDigestMethodType();
//                        digestAlgo.SetAlgorithm(@params.GetSignaturePolicyHashAlgo());
//                        hash.SetDigestMethod(digestAlgo);
//                        hash.SetDigestValue(@params.GetSignaturePolicyHashValue());
//                        policyId.SetSigPolicyHash(hash);
//                    }
//                    policyIdentifier.SetSignaturePolicyId(policyId);
//                    qualifyingProperties.GetSignedProperties().GetSignedSignatureProperties().SetSignaturePolicyIdentifier
//                        (policyIdentifier);
//                    break;
//                }

//                case SignaturePolicy.NO_POLICY:
//                {
//                    break;
//                }
//            }
//            return qualifyingProperties;
//        }
//    }
//}
