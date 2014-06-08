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

using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
//using Javax.Security.Auth.X500;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Validation.X509
{
	/// <summary>CRL Signed Token</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CRLToken : SignedToken
	{
		private X509Crl x509crl;

		/// <summary>The default constructor for CRLToken.</summary>
		/// <remarks>The default constructor for CRLToken.</remarks>
		/// <param name="crl"></param>
		public CRLToken(X509Crl crl)
		{
			this.x509crl = crl;
		}

		/// <returns>the x509crl</returns>
		public virtual X509Crl GetX509crl()
		{
			return x509crl;
		}

		public virtual X509Name GetSignerSubjectName()
		{
			return x509crl.IssuerDN;
		}

		public virtual bool IsSignedBy(X509Certificate potentialIssuer)
		{
			try
			{
				x509crl.Verify(potentialIssuer.GetPublicKey());
				return true;
			}
			catch (InvalidKeyException)
			{
				return false;
			}
			/*catch (CrlException)
			{
				return false;
			}*/
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

		public virtual CertificateSource GetWrappedCertificateSource()
		{
			return null;
		}

		public override int GetHashCode()
		{
			int prime = 31;
			int result = 1;
			result = prime * result + ((x509crl == null) ? 0 : x509crl.GetHashCode());
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
			EU.Europa.EC.Markt.Dss.Validation.X509.CRLToken other = (EU.Europa.EC.Markt.Dss.Validation.X509.CRLToken
				)obj;
			if (x509crl == null)
			{
				if (other.x509crl != null)
				{
					return false;
				}
			}
			else
			{
				if (!x509crl.Equals(other.x509crl))
				{
					return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
			return "CRL[signedBy=" + GetSignerSubjectName() + "]";
		}
	}
}
