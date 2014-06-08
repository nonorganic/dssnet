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
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.Crl;
using EU.Europa.EC.Markt.Dss.Validation.Ocsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using System.Threading;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <summary>Verify the status of a certificate using the Trusted List model.</summary>
	/// <remarks>Verify the status of a certificate using the Trusted List model.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class TrustedListCertificateVerifier : CertificateVerifier
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(TrustedListCertificateVerifier
			).FullName);

        /// <summary>Define how the certificate from the Trusted Lists are retrived.</summary>
        /// <remarks>Define how the certificate from the Trusted Lists are retrived.</remarks>
        /// <param name="trustedListCertificatesSource">the trustedListCertificatesSource to set
        /// 	</param>
        public CertificateSource TrustedListCertificatesSource { get; set; }

        /// <summary>Define the AiaCertificateSourceFactory that permits the retrieval of the certificate linked in the AIA field.
        /// 	</summary>
        /// <remarks>Define the AiaCertificateSourceFactory that permits the retrieval of the certificate linked in the AIA field.
        /// 	</remarks>
        /// <param name="aiaCertificateSourceFactory">the aiaCertificateSourceFactory to set</param>
        public CertificateSourceFactory AiaCertificateSourceFactory { get; set; }

        /// <summary>Define the source of OCSP used bu this class</summary>
        /// <param name="ocspSource">the ocspSource to set</param>
        public IOcspSource OcspSource { get; set; }

        /// <summary>Define the source of CRL used by this class</summary>
        /// <param name="crlSource">the crlSource to set</param>
        public ICrlSource CrlSource { get; set; }        
        
		/// <exception cref="System.IO.IOException"></exception>
		public virtual ValidationContext ValidateCertificate(X509Certificate cert, DateTime
			 validationDate, CertificateSource optionalCertificateSource, ICrlSource optionalCRLSource
			, IOcspSource optionalOCSPSource)
		{
			if (cert == null || validationDate == null)
			{
				throw new ArgumentNullException("A validation context must contains a cert and a validation date"
					);
			}
			ValidationContext previous = validationContextThreadLocal.Value;
			if (previous != null && previous.GetCertificate().Equals(cert) && previous.GetValidationDate
				().Equals(validationDate))
			{
				LOG.Info("We don't need to check twice for the same");
				return previous;
			}
			ValidationContext context = new ValidationContext(cert, validationDate);
			context.SetCrlSource(CrlSource);
			context.SetOcspSource(OcspSource);
			context.SetTrustedListCertificatesSource(TrustedListCertificatesSource);
			context.Validate(validationDate, optionalCertificateSource, optionalCRLSource, optionalOCSPSource
				);
			validationContextThreadLocal.Value = context;
			return context;            
		}

		private ThreadLocal<ValidationContext> validationContextThreadLocal = new ThreadLocal
			<ValidationContext>();
	}
}
