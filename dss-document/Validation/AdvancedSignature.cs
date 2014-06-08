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
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.Crl;
using EU.Europa.EC.Markt.Dss.Validation.Ocsp;
using EU.Europa.EC.Markt.Dss.Validation.X509;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Utilities.Date;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <summary>Provides an abstraction for an Advanced Electronic Signature.</summary>
	/// <remarks>
	/// Provides an abstraction for an Advanced Electronic Signature. This ease the validation process. Every signature
	/// format : XAdES, CAdES and PAdES are treated the same.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public interface AdvancedSignature
	{
		/// <summary>Specifies the format of the signature</summary>
		SignatureFormat GetSignatureFormat();

		/// <summary>Retrieves the signature algorithm (or cipher) used for generating the signature
		/// 	</summary>
		/// <returns></returns>
		string GetSignatureAlgorithm();

		/// <summary>Gets a certificate source for the ALL certificates embedded in the signature
		/// 	</summary>
		/// <returns></returns>
		/// <exception cref="System.Exception">System.Exception</exception>
		CertificateSource GetCertificateSource();

		/// <summary>Return only the certificates that are in the -XL/-LTV structure.</summary>
		/// <remarks>Return only the certificates that are in the -XL/-LTV structure.</remarks>
		/// <returns></returns>
		CertificateSource GetExtendedCertificateSource();

		/// <summary>Get certificates embedded in the signature</summary>
		/// <reutrn>a list of certificate contained in the signature</reutrn>
		IList<X509Certificate> GetCertificates();

		/// <summary>Gets a CRL source for the CRLs embedded in the signature</summary>
		/// <returns></returns>
		/// <exception cref="System.Exception">System.Exception</exception>
		ICrlSource GetCRLSource();

		/// <summary>Gets an OCSP source for the OCSP responses embedded in the signature</summary>
		/// <returns></returns>
		/// <exception cref="System.Exception">System.Exception</exception>
		IOcspSource GetOCSPSource();

		/// <summary>Get the signing certificate</summary>
		/// <returns></returns>
		X509Certificate GetSigningCertificate();

		/// <summary>Returns the signing time information</summary>
		/// <returns></returns>
		DateTimeObject GetSigningTime();

		/// <summary>Returns the Signature Policy OID from the signature</summary>
		/// <returns></returns>
		PolicyValue GetPolicyId();

		/// <summary>Return information about the place where the signature was generated</summary>
		/// <returns></returns>
		string GetLocation();

		/// <summary>Returns the content type of the signed data</summary>
		/// <returns></returns>
		string GetContentType();

		/// <summary>Returns the claimed role of the signer.</summary>
		/// <remarks>Returns the claimed role of the signer.</remarks>
		/// <returns></returns>
		string[] GetClaimedSignerRoles();

		/// <summary>Returns the signature timestamps</summary>
		/// <returns></returns>
		IList<TimestampToken> GetSignatureTimestamps();

		/// <summary>Returns the data that is timestamped in the SignatureTimeStamp</summary>
		/// <returns></returns>
		byte[] GetSignatureTimestampData();

		/// <summary>Archive timestamp seals the data of the signature in a specific order.</summary>
		/// <remarks>
		/// Archive timestamp seals the data of the signature in a specific order. We need to retrieve the data for each
		/// timestamp.
		/// </remarks>
		/// <param name="index"></param>
		/// <param name="originalData">For a detached signature, the original data are needed
		/// 	</param>
		/// <returns></returns>
		/// <exception cref="System.IO.IOException"></exception>
		byte[] GetArchiveTimestampData(int index, Document originalData);

		/// <summary>
		/// Returns the timestamp over the certificate/revocation references (and optionally other fields), used in -X
		/// profiles
		/// </summary>
		IList<TimestampToken> GetTimestampsX1();

		/// <returns></returns>
		IList<TimestampToken> GetTimestampsX2();

		/// <summary>Returns the archive TimeStamps</summary>
		/// <returns></returns>
		IList<TimestampToken> GetArchiveTimestamps();

		/// <summary>Verify the signature integrity; checks if the signed content has not been tampered with
		/// 	</summary>
		/// <param name="detachedDocument">the original document concerned by the signature if not part of the actual object
		/// 	</param>
		/// <returns>true if the signature is valid</returns>
		bool CheckIntegrity(Document detachedDocument);

		/// <summary>Returns a list of counter signatures applied to this signature</summary>
		/// <returns>a list of AdvancedSignatures representing the counter signatures</returns>
		IList<AdvancedSignature> GetCounterSignatures();

		/// <summary>Retrieve list of certificate ref</summary>
		/// <returns></returns>
		IList<CertificateRef> GetCertificateRefs();

		/// <returns>The list of CRLRefs contained in the Signature</returns>
		IList<CRLRef> GetCRLRefs();

		/// <returns>The list of OCSPRef contained in the Signature</returns>
		IList<OCSPRef> GetOCSPRefs();

		/// <returns>The list of X509Crl contained in the Signature</returns>
		IList<X509Crl> GetCRLs();

		/// <returns>The list of BasicOCSResp contained in the Signature</returns>
		IList<BasicOcspResp> GetOCSPs();

		/// <returns>The byte array digested to create a TimeStamp X1</returns>
		byte[] GetTimestampX1Data();

		/// <returns>The byte array digested to create a TimeStamp X2</returns>
		byte[] GetTimestampX2Data();
	}
}
