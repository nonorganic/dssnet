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
using EU.Europa.EC.Markt.Dss.Validation.Crl;
//using Mono.Math;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Math;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation.Crl
{
	/// <summary>Verifier based on CRL</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CRLCertificateVerifier : CertificateStatusVerifier
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Crl.CRLCertificateVerifier
			).FullName);

		private readonly ICrlSource crlSource;

		/// <summary>Main constructor.</summary>
		/// <remarks>Main constructor.</remarks>
		/// <param name="crlSource">the CRL repository used by this CRL trust linker.</param>
		public CRLCertificateVerifier(ICrlSource crlSource)
		{
			this.crlSource = crlSource;
		}

		public virtual CertificateStatus Check(X509Certificate childCertificate, X509Certificate
			 certificate, DateTime validationDate)
		{
			try
			{
				CertificateStatus report = new CertificateStatus();
				report.Certificate = childCertificate;
				report.ValidationDate = validationDate;
				report.IssuerCertificate = certificate;
				if (crlSource == null)
				{
					LOG.Warn("CRLSource null");
					return null;
				}
				X509Crl x509crl = crlSource.FindCrl(childCertificate, certificate);
				if (x509crl == null)
				{
					LOG.Info("No CRL found for certificate " + childCertificate.SubjectDN);
					return null;
				}
				if (!IsCRLValid(x509crl, certificate, validationDate))
				{
					LOG.Warn("The CRL is not valid !");
					return null;
				}
				report.StatusSource = x509crl;
				report.Validity = CertificateValidity.UNKNOWN;
				report.Certificate = childCertificate;
				report.StatusSourceType = ValidatorSourceType.CRL;
				report.ValidationDate = validationDate;
				X509CrlEntry crlEntry = x509crl.GetRevokedCertificate(childCertificate.SerialNumber);
				if (null == crlEntry)
				{
					LOG.Info("CRL OK for: " + childCertificate.SubjectDN);
					report.Validity = CertificateValidity.VALID;
				}
				else
				{
					if (crlEntry.RevocationDate.CompareTo(validationDate) > 0) //jbonilla - After
					{
						LOG.Info("CRL OK for: " + childCertificate.SubjectDN + " at " + validationDate
							);
						report.Validity = CertificateValidity.VALID;
						report.RevocationObjectIssuingTime = x509crl.ThisUpdate;
					}
					else
					{
						LOG.Info("CRL reports certificate: " + childCertificate.SubjectDN
							 + " as revoked since " + crlEntry.RevocationDate);
						report.Validity = CertificateValidity.REVOKED;
						report.RevocationObjectIssuingTime = x509crl.ThisUpdate;
						report.RevocationDate = crlEntry.RevocationDate;
					}
				}
				return report;
			}
			catch (IOException e)
			{
				LOG.Error("IOException when accessing CRL for " + childCertificate.SubjectDN.ToString() + " " + e.Message);
				return null;
			}
		}

		private bool IsCRLValid(X509Crl x509crl, X509Certificate issuerCertificate, DateTime
			 validationDate)
		{
			if (!IsCRLOK(x509crl, issuerCertificate, validationDate))
			{
				return false;
			}
			else
			{
				LOG.Info("CRL number: " + GetCrlNumber(x509crl));
				return true;
			}
		}

		private bool IsCRLOK(X509Crl x509crl, X509Certificate issuerCertificate, DateTime
			 validationDate)
		{
			if (issuerCertificate == null)
			{
				throw new ArgumentNullException("Must provide a issuer certificate to validate the signature"
					);
			}
			if (!x509crl.IssuerDN.Equals(issuerCertificate.SubjectDN))
			{
				LOG.Warn("The CRL must be signed by the issuer (" + issuerCertificate.SubjectDN
					+ " ) but instead is signed by " + x509crl.IssuerDN);
				return false;
			}
			try
			{
				x509crl.Verify(issuerCertificate.GetPublicKey());
			}
			catch (Exception e)
			{
				LOG.Warn("The signature verification for CRL cannot be performed : " + e.Message
					);
				return false;
			}
			DateTime thisUpdate = x509crl.ThisUpdate;
			LOG.Info("validation date: " + validationDate);
			LOG.Info("CRL this update: " + thisUpdate);
			//        if (thisUpdate.after(validationDate)) {
			//            LOG.warning("CRL too young");
			//            return false;
			//        }
			LOG.Info("CRL next update: " + x509crl.NextUpdate);
			if (x509crl.NextUpdate != null && validationDate.CompareTo(x509crl.NextUpdate.Value) > 0) //jbonilla After
			{
				LOG.Info("CRL too old");
				return false;
			}
			// assert cRLSign KeyUsage bit
			if (null == issuerCertificate.GetKeyUsage())
			{
				LOG.Warn("No KeyUsage extension for CRL issuing certificate");
				return false;
			}
			if (false == issuerCertificate.GetKeyUsage()[6])
			{
				LOG.Warn("cRLSign bit not set for CRL issuing certificate");
				return false;
			}
			return true;
		}

		private BigInteger GetCrlNumber(X509Crl crl)
		{
			//byte[] crlNumberExtensionValue = crl.GetExtensionValue(X509Extensions.CrlNumber);
            Asn1OctetString crlNumberExtensionValue = crl.GetExtensionValue(X509Extensions.CrlNumber);
			if (null == crlNumberExtensionValue)
			{
				return null;
			}
			try
			{
                //DerOctetString octetString = (DerOctetString)(new ASN1InputStream(new ByteArrayInputStream
                //    (crlNumberExtensionValue)).ReadObject());
                DerOctetString octetString = (DerOctetString)crlNumberExtensionValue;
				byte[] octets = octetString.GetOctets();
				DerInteger integer = (DerInteger)new Asn1InputStream(octets).ReadObject();
				BigInteger crlNumber = integer.PositiveValue;
				return crlNumber;
			}
			catch (IOException e)
			{
				throw new RuntimeException("IO error: " + e.Message, e);
			}
		}
	}
}
