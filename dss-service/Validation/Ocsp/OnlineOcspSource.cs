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
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Validation.Https;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Ocsp;
using Sharpen;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;

namespace EU.Europa.EC.Markt.Dss.Validation.Ocsp
{
	/// <summary>Online OCSP repository.</summary>
	/// <remarks>Online OCSP repository. This implementation will contact the OCSP Responder to retrieve the OCSP response.
	/// 	</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class OnlineOcspSource : IOcspSource
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Ocsp.OnlineOcspSource
			).FullName);

        /// <summary>Set the HTTPDataLoader to use for querying the OCSP server.</summary>
        /// <remarks>Set the HTTPDataLoader to use for querying the OCSP server.</remarks>
        /// <param name="httpDataLoader"></param>
        public HTTPDataLoader HttpDataLoader { get; set; }

        //jbonilla
        public string OcspUri { get; set; }

		/// <summary>Create an OCSP source The default constructor for OnlineOCSPSource.</summary>
		/// <remarks>Create an OCSP source The default constructor for OnlineOCSPSource.</remarks>
		public OnlineOcspSource()
		{			
            NetHttpDataLoader dataLoader = new NetHttpDataLoader();
            dataLoader.TimeOut = 5000;
            dataLoader.ContentType = "application/ocsp-request";
            dataLoader.Accept = "application/ocsp-response";
            this.HttpDataLoader = dataLoader;
		}	

		/// <exception cref="System.IO.IOException"></exception>
		public BasicOcspResp GetOcspResponse(X509Certificate certificate, X509Certificate issuerCertificate)		
		{
			try
			{
				this.OcspUri = GetAccessLocation(certificate, X509ObjectIdentifiers.OcspAccessMethod);
                LOG.Info("OCSP URI: " + this.OcspUri);
                if (this.OcspUri == null)
				{
					return null;
				}
				OcspReqGenerator ocspReqGenerator = new OcspReqGenerator();
				CertificateID certId = new CertificateID(CertificateID.HashSha1, issuerCertificate
					, certificate.SerialNumber);
				ocspReqGenerator.AddRequest(certId);
				OcspReq ocspReq = ocspReqGenerator.Generate();
				byte[] ocspReqData = ocspReq.GetEncoded();
                OcspResp ocspResp = new OcspResp(HttpDataLoader.Post(this.OcspUri, new MemoryStream
					(ocspReqData)));
				try
				{
					return (BasicOcspResp)ocspResp.GetResponseObject();
				}
				catch (ArgumentNullException)
				{
					// Encountered a case when the OCSPResp is initialized with a null OCSP response...
					// (and there are no nullity checks in the OCSPResp implementation)
					return null;
				}
			}
			catch (CannotFetchDataException)
			{
				return null;
			}
			catch (OcspException e)
			{
				LOG.Error("OCSP error: " + e.Message);
				return null;
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		private string GetAccessLocation(X509Certificate certificate, DerObjectIdentifier
			 accessMethod)
		{
            //byte[] authInfoAccessExtensionValue = certificate.GetExtensionValue(X509Extensions
            //    .AuthorityInfoAccess);
            Asn1OctetString authInfoAccessExtensionValue = certificate.GetExtensionValue(X509Extensions
                .AuthorityInfoAccess);
			if (null == authInfoAccessExtensionValue)
			{
				return null;
			}
			AuthorityInformationAccess authorityInformationAccess;
            //DerOctetString oct = (DerOctetString)(new Asn1InputStream(new MemoryStream
            //    (authInfoAccessExtensionValue)).ReadObject());
            DerOctetString oct = (DerOctetString)authInfoAccessExtensionValue;            
            //authorityInformationAccess = new AuthorityInformationAccess((Asn1Sequence)new Asn1InputStream
            //    (oct.GetOctets()).ReadObject());
            authorityInformationAccess = AuthorityInformationAccess.GetInstance((Asn1Sequence)new Asn1InputStream
                (oct.GetOctets()).ReadObject());            
			AccessDescription[] accessDescriptions = authorityInformationAccess.GetAccessDescriptions
				();
			foreach (AccessDescription accessDescription in accessDescriptions)
			{
				LOG.Info("access method: " + accessDescription.AccessMethod);
				bool correctAccessMethod = accessDescription.AccessMethod.Equals(accessMethod
					);
				if (!correctAccessMethod)
				{
					continue;
				}
				GeneralName gn = accessDescription.AccessLocation;
				if (gn.TagNo != GeneralName.UniformResourceIdentifier)
				{
					LOG.Info("not a uniform resource identifier");
					continue;
				}
				DerIA5String str = (DerIA5String)((DerTaggedObject)gn.ToAsn1Object()).GetObject();
				string accessLocation = str.GetString();
				LOG.Info("access location: " + accessLocation);
				return accessLocation;
			}
			return null;
		}
    }
}
