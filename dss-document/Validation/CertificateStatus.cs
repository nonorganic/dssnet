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
using Sharpen;
using Org.BouncyCastle.X509;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CertificateStatus
	{
        /// <summary>Get or Set the certificate for which the status is relevant</summary>         
		public X509Certificate Certificate { get; set; }

        /// <summary>Get or Set the issuer certificate</summary>        
        public X509Certificate IssuerCertificate { get; set; }

        /// <summary>Result of the validity check</summary>
		public CertificateValidity Validity { get; set; }

        /// <summary>Data from which the status is coming</summary>        
        public object StatusSource { get; set; }
        
        /// <summary>Type of source from which the status is coming</summary>        
		public ValidatorSourceType StatusSourceType { get; set; }

        /// <summary>The revocationObjectIssuingTime</summary>
		public DateTime RevocationObjectIssuingTime { get; set; }
                
        /// <summary>The revocationDate</summary>
		public DateTime RevocationDate { get; set; }

        /// <summary>Date when the validation was performed</summary>
        public DateTime ValidationDate { get; set; }

		public override string ToString()
		{
			return "CertificateStatus[The certificate of '" + (Certificate != null ? Certificate
				.SubjectDN.ToString() : "<<!!null!!>>") + "' is " + (Validity != null ? Validity.ToString
				() : "<<!!null!!>>") + " at the date " + ValidationDate + " according to " + (StatusSourceType
				 != null ? StatusSourceType.ToString() : "<<!!null!!>>") + "]";
		}
	}
}
