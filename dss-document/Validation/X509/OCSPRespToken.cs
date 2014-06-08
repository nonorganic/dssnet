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

using System.Collections.Generic;
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.X509;
//using Javax.Security.Auth.X500;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation.X509
{
	/// <summary>OCSP Signed Token</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class OCSPRespToken : SignedToken
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.X509.OCSPRespToken
			).FullName);

		private BasicOcspResp ocspResp;

		/// <summary>The default constructor for OCSPRespToken.</summary>
		/// <remarks>The default constructor for OCSPRespToken.</remarks>
		/// <param name="ocspResp"></param>
		public OCSPRespToken(BasicOcspResp ocspResp)
		{
			this.ocspResp = ocspResp;
		}

		/// <returns>the ocspResp</returns>
		public virtual BasicOcspResp GetOcspResp()
		{
			return ocspResp;
		}

		public virtual X509Name GetSignerSubjectName()
		{
			if (ocspResp.ResponderId.ToAsn1Object().Name != null)
			{
				//return new X509Name(ocspResp.ResponderId.ToAsn1Object().Name.GetDerEncoded());
                return ocspResp.ResponderId.ToAsn1Object().Name;
			}
			else
			{
				IList<X509Certificate> certs = ((OCSPRespCertificateSource)GetWrappedCertificateSource()).GetCertificates();
				foreach (X509Certificate c in certs)
				{
					if (IsSignedBy(c))
					{
						return c.SubjectDN;
					}
				}
				LOG.Warn("Don't found an signer for OCSPToken in the " + certs.Count + " certificates "
					 + certs);
				return null;
			}
		}

		public virtual bool IsSignedBy(X509Certificate potentialIssuer)
		{
			try
			{
				//return ocspResp.Verify(potentialIssuer.GetPublicKey(), "BC");
                return ocspResp.Verify(potentialIssuer.GetPublicKey());
			}
			/*catch (NoSuchProviderException e)
			{
				throw new RuntimeException(e);
			}*/
			catch (OcspException)
			{
				return false;
			}
		}

		public virtual CertificateSource GetWrappedCertificateSource()
		{
			return new OCSPRespCertificateSource(ocspResp);
		}

		public override int GetHashCode()
		{
			int prime = 31;
			int result = 1;
			result = prime * result + ((ocspResp == null) ? 0 : ocspResp.GetHashCode());
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
			EU.Europa.EC.Markt.Dss.Validation.X509.OCSPRespToken other = (EU.Europa.EC.Markt.Dss.Validation.X509.OCSPRespToken
				)obj;
			if (ocspResp == null)
			{
				if (other.ocspResp != null)
				{
					return false;
				}
			}
			else
			{
				if (!ocspResp.Equals(other.ocspResp))
				{
					return false;
				}
			}
			return true;
		}

		public override string ToString()
		{
			return "OcspResp[signedBy=" + GetSignerSubjectName() + "]";
		}
	}
}
