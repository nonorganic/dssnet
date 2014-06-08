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
using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using EU.Europa.EC.Markt.Dss.Validation.X509;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using Org.BouncyCastle.X509;

namespace EU.Europa.EC.Markt.Dss.Validation.X509
{
	/// <summary>RevocationData for a specific SignedToken</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class RevocationData
	{
		private SignedToken targetToken;

		private object revocationData;

		/// <summary>The default constructor for RevocationData.</summary>
		/// <remarks>The default constructor for RevocationData.</remarks>
		public RevocationData()
		{
		}

		/// <summary>The default constructor for RevocationData.</summary>
		/// <remarks>The default constructor for RevocationData.</remarks>
		/// <param name="signedToken"></param>
		public RevocationData(SignedToken signedToken)
		{
			this.targetToken = signedToken;
		}

		/// <summary>The target of this revocation data</summary>
		/// <returns></returns>
		public virtual SignedToken GetTargetToken()
		{
			return targetToken;
		}

		/// <summary>The value of the revocation data</summary>
		/// <returns></returns>
		public virtual object GetRevocationData()
		{
			return revocationData;
		}

		/// <summary>Set the value of the revocation data</summary>
		/// <param name="revocationData"></param>
		public virtual void SetRevocationData(object revocationData)
		{
			if (targetToken is CertificateToken)
			{
				if (!(revocationData is CertificateSourceType) && !(revocationData is BasicOcspResp
					) && !(revocationData is X509Crl))
				{
					throw new ArgumentException("For " + targetToken + " only OCSP, CRL or CertificateSourceType are valid. (Trying to add "
						 + revocationData.GetType().Name + ").");
				}
			}
			this.revocationData = revocationData;
		}

		public override string ToString()
		{
			string data = null;
			if (GetRevocationData() is X509Crl)
			{
				data = "CRL[from=" + ((X509Crl)GetRevocationData()).IssuerDN + "]";
			}
			else
			{
				if (GetRevocationData() is BasicOcspResp)
				{
					data = "OCSP[from" + ((BasicOcspResp)GetRevocationData()).ResponderId.ToAsn1Object
						().Name + "]";
				}
				else
				{
					if (GetRevocationData() is X509Certificate)
					{
						data = "Certificate[subjectName=" + ((X509Certificate)GetRevocationData()).SubjectDN + "]";
					}
					else
					{
						if (GetRevocationData() != null)
						{
							data = GetRevocationData().ToString();
						}
						else
						{
							data = "*** NO VALIDATION DATA AVAILABLE ***";
						}
					}
				}
			}
			return "RevocationData[token=" + targetToken + ",data=" + data + "]";
		}
	}
}
