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
using System.IO;
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Ocsp;
//using Org.BouncyCastle.Jce.Provider;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation.Ocsp
{
	/// <summary>Check the status of the certificate using an OCSPSource</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class OCSPCertificateVerifier : CertificateStatusVerifier
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Ocsp.OCSPCertificateVerifier
			).FullName);

		private readonly IOcspSource ocspSource;

		/// <summary>Create a CertificateVerifier that will use the OCSP Source for checking revocation data.
		/// 	</summary>
		/// <remarks>
		/// Create a CertificateVerifier that will use the OCSP Source for checking revocation data. The default constructor
		/// for OCSPCertificateVerifier.
		/// </remarks>
		/// <param name="ocspSource"></param>
		public OCSPCertificateVerifier(IOcspSource ocspSource)
		{
            //jbonilla
			//Security.AddProvider(new BouncyCastleProvider());
			this.ocspSource = ocspSource;
		}

		public virtual CertificateStatus Check(X509Certificate childCertificate, X509Certificate
			 certificate, DateTime validationDate)
		{
			CertificateStatus status = new CertificateStatus();
			status.Certificate = childCertificate;
			status.ValidationDate = validationDate;
			status.IssuerCertificate = certificate;
			if (ocspSource == null)
			{
				LOG.Warn("OCSPSource null");
				return null;
			}
			try
			{
				BasicOcspResp ocspResp = ocspSource.GetOcspResponse(childCertificate, certificate
					);
				if (null == ocspResp)
				{
					LOG.Info("OCSP response not found");
					return null;
				}
				BasicOcspResp basicOCSPResp = (BasicOcspResp)ocspResp;
				CertificateID certificateId = new CertificateID(CertificateID.HashSha1, certificate
					, childCertificate.SerialNumber);
				SingleResp[] singleResps = basicOCSPResp.Responses;
				foreach (SingleResp singleResp in singleResps)
				{
					CertificateID responseCertificateId = singleResp.GetCertID();
					if (false == certificateId.Equals(responseCertificateId))
					{
						continue;
					}
					DateTime thisUpdate = singleResp.ThisUpdate;
					LOG.Info("OCSP thisUpdate: " + thisUpdate);
					LOG.Info("OCSP nextUpdate: " + singleResp.NextUpdate);
					status.StatusSourceType = ValidatorSourceType.OCSP;
					status.StatusSource = ocspResp;
					status.RevocationObjectIssuingTime = ocspResp.ProducedAt;
					if (null == singleResp.GetCertStatus())
					{
						LOG.Info("OCSP OK for: " + childCertificate.SubjectDN);
						status.Validity = CertificateValidity.VALID;
					}
					else
					{
						LOG.Info("OCSP certificate status: " + singleResp.GetCertStatus().GetType().FullName
							);
						if (singleResp.GetCertStatus() is RevokedStatus)
						{
							LOG.Info("OCSP status revoked");
							if (validationDate.CompareTo(((RevokedStatus)singleResp.GetCertStatus()).RevocationTime) < 0) //jbonilla - Before
							{
								LOG.Info("OCSP revocation time after the validation date, the certificate was valid at "
									 + validationDate);
								status.Validity = CertificateValidity.VALID;
							}
							else
							{
								status.RevocationDate = ((RevokedStatus)singleResp.GetCertStatus()).RevocationTime;
								status.Validity = CertificateValidity.REVOKED;
							}
						}
						else
						{
							if (singleResp.GetCertStatus() is UnknownStatus)
							{
								LOG.Info("OCSP status unknown");
								status.Validity = CertificateValidity.UNKNOWN;
							}
						}
					}
					return status;
				}
				LOG.Info("no matching OCSP response entry");
				return null;
			}
			catch (IOException ex)
			{
				LOG.Error("OCSP exception: " + ex.Message);
				return null;
			}
			catch (OcspException ex)
			{
				LOG.Error("OCSP exception: " + ex.Message);
				throw new RuntimeException(ex);
			}
		}
	}
}
