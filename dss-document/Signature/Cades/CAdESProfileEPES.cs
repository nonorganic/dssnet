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

using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Signature.Cades;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Esf;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;
using Sharpen;
using System.Collections;
using System.Collections.Generic;

namespace EU.Europa.EC.Markt.Dss.Signature.Cades
{
	/// <summary>
	/// This class holds the CAdES-EPES signature profile; it supports the inclusion of the mandatory signed
	/// id_aa_ets_sigPolicyId attribute as specified in ETSI TS 101 733 V1.8.1, clause 5.8.1.
	/// </summary>
	/// <remarks>
	/// This class holds the CAdES-EPES signature profile; it supports the inclusion of the mandatory signed
	/// id_aa_ets_sigPolicyId attribute as specified in ETSI TS 101 733 V1.8.1, clause 5.8.1.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CAdESProfileEPES : CAdESProfileBES
	{
		/// <summary>The default constructor for CAdESProfileEPES.</summary>
		/// <remarks>The default constructor for CAdESProfileEPES.</remarks>
		public CAdESProfileEPES()
		{
		}

		/// <summary>The default constructor for CAdESProfileEPES.</summary>
		/// <remarks>The default constructor for CAdESProfileEPES.</remarks>
		public CAdESProfileEPES(bool padesUsage) : base(padesUsage)
		{
		}

		//internal override IDictionary<DerObjectIdentifier, Asn1Encodable> GetSignedAttributes
        internal override IDictionary GetSignedAttributes
			(SignatureParameters parameters)
		{
			try
			{
				//IDictionary<DerObjectIdentifier, Asn1Encodable> signedAttrs = base.GetSignedAttributes(parameters);
                IDictionary signedAttrs = base.GetSignedAttributes(parameters);
				Attribute policy = null;
				SignaturePolicyIdentifier sigPolicy = null;
				switch (parameters.SignaturePolicy)
				{
					case SignaturePolicy.EXPLICIT:
					{
						sigPolicy = new SignaturePolicyIdentifier(new SignaturePolicyId(new DerObjectIdentifier
							(parameters.SignaturePolicyID), new OtherHashAlgAndValue(new AlgorithmIdentifier
							(DigestAlgorithm.GetByName(parameters.SignaturePolicyHashAlgo).GetOid()), new 
							DerOctetString(parameters.SignaturePolicyHashValue))));
						policy = new Attribute(PkcsObjectIdentifiers.IdAAEtsSigPolicyID, new DerSet(sigPolicy
							));
						signedAttrs.Add(PkcsObjectIdentifiers.IdAAEtsSigPolicyID, policy);
						break;
					}

					case SignaturePolicy.IMPLICIT:
					{
						sigPolicy = new SignaturePolicyIdentifier();
						//sigPolicy.IsSignaturePolicyImplied(); TODO jbonilla - validar
						policy = new Attribute(PkcsObjectIdentifiers.IdAAEtsSigPolicyID, new DerSet(sigPolicy
							));
						signedAttrs.Add(PkcsObjectIdentifiers.IdAAEtsSigPolicyID, policy);
						break;
					}

					case SignaturePolicy.NO_POLICY:
					{
						break;
					}
				}
				return signedAttrs;
			}
			catch (NoSuchAlgorithmException ex)
			{
				throw new ProfileException(ex.Message);
			}
		}
	}
}
