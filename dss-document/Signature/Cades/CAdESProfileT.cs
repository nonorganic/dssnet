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
using EU.Europa.EC.Markt.Dss.Validation.Tsp;
using iTextSharp.text.log;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using BcCms = Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
//using Org.BouncyCastle.Operator;
using Sharpen;
using System.Collections.Generic;
using System.Collections;
using Org.BouncyCastle.Asn1.Oiw;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Signature.Cades
{
	/// <summary>
	/// This class holds the CAdES-T signature profile; it supports the inclusion of the mandatory unsigned
	/// id-aa-signatureTimeStampToken attribute as specified in ETSI TS 101 733 V1.8.1, clause 6.1.1.
	/// </summary>
	/// <remarks>
	/// This class holds the CAdES-T signature profile; it supports the inclusion of the mandatory unsigned
	/// id-aa-signatureTimeStampToken attribute as specified in ETSI TS 101 733 V1.8.1, clause 6.1.1.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CAdESProfileT : CAdESSignatureExtension
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(CAdESProfileT).FullName
			);

		/*internal AlgorithmIdentifier digestAlgorithm = new DefaultDigestAlgorithmIdentifierFinder
			().Find(new DefaultSignatureAlgorithmIdentifierFinder().Find("SHA1withRSA"));*/

        internal AlgorithmIdentifier digestAlgorithm = new AlgorithmIdentifier(OiwObjectIdentifiers.IdSha1);

		/// <param name="signatureTsa">the TSA used for the signature-time-stamp attribute</param>
		public override void SetSignatureTsa(ITspSource signatureTsa)
		{
			this.signatureTsa = signatureTsa;
		}

		/// <exception cref="System.IO.IOException"></exception>
		protected internal override SignerInformation ExtendCMSSignature(CmsSignedData signedData
			, SignerInformation si, SignatureParameters parameters, Document originalData)
		{
			if (this.signatureTsa == null)
			{
				throw new ConfigurationException(ConfigurationException.MSG.CONFIGURE_TSP_SERVER);
			}
			LOG.Info("Extend signature with id " + si.SignerID);
			BcCms.AttributeTable unsigned = si.UnsignedAttributes;
			//IDictionary<DerObjectIdentifier, Attribute> unsignedAttrHash = null;
            IDictionary unsignedAttrHash = null;
			if (unsigned == null)
			{
				unsignedAttrHash = new Dictionary<DerObjectIdentifier, Attribute>();
			}
			else
			{
				unsignedAttrHash = si.UnsignedAttributes.ToDictionary();
			}
            
            //TODO jbonilla - ¿Qué ocurre si ya es CAdES-T? No se debería volver a extender.
			Attribute signatureTimeStamp = GetTimeStampAttribute(PkcsObjectIdentifiers.IdAASignatureTimeStampToken
				, this.signatureTsa, digestAlgorithm, si.GetSignature());
			//unsignedAttrHash.Put(PkcsObjectIdentifiers.IdAASignatureTimeStampToken, signatureTimeStamp);
            unsignedAttrHash.Add(PkcsObjectIdentifiers.IdAASignatureTimeStampToken, signatureTimeStamp);
			SignerInformation newsi = SignerInformation.ReplaceUnsignedAttributes(si, new BcCms.AttributeTable
				(unsignedAttrHash));
			return newsi;
		}
		// Attribute signatureTimeStamp = getTimeStampAttribute(PkcsObjectIdentifiers.IdAASignatureTimeStampToken,
		// this.signatureTsa, digestAlgorithm, si.getSignature());
		//
		// AttributeTable table2 = si.getUnsignedAttributes().add(PkcsObjectIdentifiers.IdAASignatureTimeStampToken,
		// signatureTimeStamp);
		// /* If we add a timestamp, then we must remove every reference to timestamp -X and archive timestamp */
		// table2 = table2.remove(CAdESProfileA.id_aa_ets_archiveTimestampV2);
		// table2 = table2.remove(PkcsObjectIdentifiers.IdAAEtsEscTimeStamp);
		//
		// SignerInformation newsi = SignerInformation.replaceUnsignedAttributes(si, table2);
		// return newsi;
		//
	}
}
