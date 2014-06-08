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
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Signature;
using Sharpen;
using Org.BouncyCastle.X509;

namespace EU.Europa.EC.Markt.Dss.Signature
{
	/// <summary>Parameters for a Signature creation/extension</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class SignatureParameters
	{
        /// <summary>Get or Set the signing date</summary>        
		public virtual DateTime SigningDate { get; set; }

        /// <summary>Get or Set the signing certificate</summary>        
        public virtual X509Certificate SigningCertificate { get; set; }

        /// <summary>Get or Set the certificate chain</summary>		
        public virtual IList<X509Certificate> CertificateChain { get; set; }

        /// <summary>Return or Set the type of signature policy</summary>
        public virtual SignaturePolicy SignaturePolicy { get; set; }
        
        /// <summary>Get or Set the signature policy (EPES)</summary>
        public virtual string SignaturePolicyID { get; set; }

        /// <summary>Return or Set the hash algorithm for the signature policy 
        /// or Set the hash algorithm for the explicit signature policy</summary>
        public virtual string SignaturePolicyHashAlgo { get; set; }

        /// <summary>Get the hash value of the explicit signature policy 
        /// or Set the hash value of implicit signature policy</summary>        
        public virtual byte[] SignaturePolicyHashValue { get; set; }

        /// <summary>Get or Set claimed role</summary>
        public virtual string ClaimedSignerRole { get; set; }

        /// <summary>Get or Set signature format</summary>
        public virtual SignatureFormat SignatureFormat { get; set; }

        /// <summary>Get or Set Signature packaging</summary>
        public virtual SignaturePackaging SignaturePackaging { get; set; }
        
        /// <returns>the signatureAlgorithm</returns>
        public virtual SignatureAlgorithm SignatureAlgorithm { get; set; }

        /// <returns>the digestAlgorithm</returns>
        public virtual DigestAlgorithm DigestAlgorithm { get; set; }

        /// <returns>the reason</returns>
        public virtual string Reason { get; set; }

        /// <returns>the contactInfo</returns>
        public virtual string ContactInfo { get; set; }

        /// <returns>the location</returns>
        public virtual string Location { get; set; }

        /// <returns>the commitmentTypeIndication</returns>
        public virtual IList<string> CommitmentTypeIndication { get; set; }

        public SignatureParameters()
        {
            this.CertificateChain = new AList<X509Certificate>();
            this.SignaturePolicy = SignaturePolicy.NO_POLICY;
            this.DigestAlgorithm = DigestAlgorithm.SHA1;
            this.SignatureAlgorithm = SignatureAlgorithm.RSA;
        }

        /// <summary>Set the certificate chain</summary>
		/// <param name="certificateChain"></param>
		public virtual void SetCertificateChain(params X509Certificate[] certificateChain)
		{
			IList<X509Certificate> list = new AList<X509Certificate>();
			foreach (X509Certificate c in certificateChain)
			{
				list.AddItem((X509Certificate)c);
			}
			this.CertificateChain = list;
		}		

		/// <summary>Set signature format</summary>
		/// <param name="signatureFormat"></param>
		[System.ObsoleteAttribute(@"Use the SignatureFormat enumeration instead")]
		public virtual void SetSignatureFormat(string signatureFormat)
		{
			this.SignatureFormat = SignatureFormat.ValueByName(signatureFormat);
		}	
	}
}
