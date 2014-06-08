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
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Crl;
using EU.Europa.EC.Markt.Dss.Validation.Ocsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <summary>
	/// Fetch revocation data from a certificate by querying a OCSP server first and then an CRL server if no OCSP response
	/// could be retrieved.
	/// </summary>
	/// <remarks>
	/// Fetch revocation data from a certificate by querying a OCSP server first and then an CRL server if no OCSP response
	/// could be retrieved.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class OCSPAndCRLCertificateVerifier : CertificateStatusVerifier
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.OCSPAndCRLCertificateVerifier
			).FullName);

		private IOcspSource ocspSource;

		private ICrlSource crlSource;

		/// <summary>The default constructor for OCSPAndCRLCertificateVerifier.</summary>
		/// <remarks>The default constructor for OCSPAndCRLCertificateVerifier.</remarks>
		public OCSPAndCRLCertificateVerifier()
		{
		}

		/// <summary>Build a OCSPAndCRLCertificateVerifier that will use the provided CRLSource and OCSPSource
		/// 	</summary>
		public OCSPAndCRLCertificateVerifier(ICrlSource crlSource, IOcspSource ocspSource)
		{
			this.crlSource = crlSource;
			this.ocspSource = ocspSource;
		}

		/// <summary>Get the OCSP Source from this verifier</summary>
		/// <returns></returns>
		public virtual IOcspSource GetOcspSource()
		{
			return ocspSource;
		}

		/// <summary>Set the OCSP source for this verifier</summary>
		/// <param name="ocspSource"></param>
		public virtual void SetOcspSource(IOcspSource ocspSource)
		{
			this.ocspSource = ocspSource;
		}

		/// <summary>Get the CRL source from this verifier</summary>
		/// <returns></returns>
		public virtual ICrlSource GetCrlSource()
		{
			return crlSource;
		}

		/// <summary>Set the CRL source for this verifier</summary>
		/// <param name="crlSource"></param>
		public virtual void SetCrlSource(ICrlSource crlSource)
		{
			this.crlSource = crlSource;
		}

		public virtual CertificateStatus Check(X509Certificate cert, X509Certificate potentialIssuer
			, DateTime validationDate)
		{
			CertificateStatusVerifier ocspVerifier = new OCSPCertificateVerifier(GetOcspSource
				());
			LOG.Info("OCSP request for " + cert.SubjectDN);
			CertificateStatus result = ocspVerifier.Check(cert, potentialIssuer, validationDate
				);
			if (result != null && result.Validity != CertificateValidity.UNKNOWN)
			{
				LOG.Info("OCSP validation done, don't need for CRL");
				return result;
			}
			else
			{
				LOG.Info("No OCSP check performed, looking for a CRL for " + cert.SubjectDN);
				CRLCertificateVerifier crlVerifier = new CRLCertificateVerifier(GetCrlSource());
				result = crlVerifier.Check(cert, potentialIssuer, validationDate);
				if (result != null && result.Validity != CertificateValidity.UNKNOWN)
				{
					LOG.Info("CRL check has been performed. Valid or not, the verification is done");
					return result;
				}
				else
				{
					LOG.Info("We had no response from OCSP nor CRL");					
                    return null;
				}
			}
		}
	}
}
