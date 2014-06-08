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
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
//using Javax.Security.Auth.X500;
//using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Validation.X509
{
	/// <summary>SignedToken containing a X509Certificate</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CertificateToken : SignedToken
	{
		private CertificateSourceFactory sourceFactory;

		private CertificateAndContext cert;

		private CertificateStatus status;

		/// <summary>Create a CertificateToken</summary>
		/// <param name="cert"></param>
		public CertificateToken(CertificateAndContext cert) : this(cert, null)
		{
		}

		/// <summary>Create a CertificateToken</summary>
		/// <param name="cert"></param>
		/// <param name="sourceFactory"></param>
		public CertificateToken(CertificateAndContext cert, CertificateSourceFactory sourceFactory
			)
		{
			this.cert = cert;
			this.sourceFactory = sourceFactory;
		}

		public virtual X509Name GetSignerSubjectName()
		{
			return cert.GetCertificate().IssuerDN;
		}

		/// <returns>the cert</returns>
		public virtual CertificateAndContext GetCertificateAndContext()
		{
			return cert;
		}

		/// <returns>the cert</returns>
		public virtual X509Certificate GetCertificate()
		{
			return cert.GetCertificate();
		}

		public virtual bool IsSignedBy(X509Certificate potentialIssuer)
		{
			try
			{
				GetCertificate().Verify(potentialIssuer.GetPublicKey());
				return true;
			}
			catch (InvalidKeyException)
			{
				return false;
			}
			catch (CertificateException)
			{
				return false;
			}
			catch (NoSuchAlgorithmException)
			{
				return false;
			}
			/*catch (NoSuchProviderException e)
			{
				throw new RuntimeException(e);
			}*/
			catch (SignatureException)
			{
				return false;
			}
		}

		/// <param name="status">the status to set</param>
		public virtual void SetStatus(CertificateStatus status)
		{
			this.status = status;
		}

		/// <returns>the status</returns>
		public virtual CertificateStatus GetStatus()
		{
			return status;
		}

		/// <summary>An X509Certificate may contain information about his issuer in the AIA attribute.
		/// 	</summary>
		/// <remarks>An X509Certificate may contain information about his issuer in the AIA attribute.
		/// 	</remarks>
		public virtual CertificateSource GetWrappedCertificateSource()
		{
			if (sourceFactory != null)
			{
				CertificateSource source = sourceFactory.CreateAIACertificateSource(GetCertificate
					());
				return source;
			}
			else
			{
				return null;
			}
		}

		public override int GetHashCode()
		{
			int prime = 31;
			int result = 1;
			try
			{
				result = prime * result + ((cert == null) ? 0 : Arrays.GetHashCode(GetCertificate().
					GetEncoded()));
			}
			catch (CertificateException)
			{
				return prime;
			}
			return result;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (GetType() != obj.GetType())
			{
				return false;
			}
			EU.Europa.EC.Markt.Dss.Validation.X509.CertificateToken other = (EU.Europa.EC.Markt.Dss.Validation.X509.CertificateToken
				)obj;
			if (cert == null)
			{
				if (other.cert != null)
				{
					return false;
				}
			}
			else
			{
				if (!cert.Equals(other.cert))
				{
					return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
			return "Certificate[subjectName=\"" + GetCertificate().SubjectDN + "\",issuedBy=\""
				 + GetCertificate().IssuerDN + "\"]";
		}
	}
}
