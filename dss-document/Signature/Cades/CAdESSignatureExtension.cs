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
using EU.Europa.EC.Markt.Dss.Signature;
using EU.Europa.EC.Markt.Dss.Signature.Cades;
using EU.Europa.EC.Markt.Dss.Validation.Tsp;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using BcCms = Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
//using Org.BouncyCastle.Jce.Provider;
using Org.BouncyCastle.Tsp;
using Sharpen;
using iTextSharp.text.log;
using System.Collections;
using Org.BouncyCastle.Security;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Signature.Cades
{
	/// <summary>Base class for extending a CAdESSignature.</summary>
	/// <remarks>Base class for extending a CAdESSignature.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public abstract class CAdESSignatureExtension : SignatureExtension
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(CAdESSignatureExtension
			).FullName);

		protected internal ITspSource signatureTsa;

		/// <returns>the TSA used for the signature-time-stamp attribute</returns>
		public virtual ITspSource GetSignatureTsa()
		{
			return signatureTsa;
		}

		/// <param name="signatureTsa">the signatureTsa to set</param>
		public virtual void SetSignatureTsa(ITspSource signatureTsa)
		{
			this.signatureTsa = signatureTsa;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual Document ExtendSignatures(Document document, Document originalData
			, SignatureParameters parameters)
		{
			try
			{
				CmsSignedData signedData = new CmsSignedData(document.OpenStream());
				SignerInformationStore signerStore = signedData.GetSignerInfos();
				AList<SignerInformation> siArray = new AList<SignerInformation>();				

                foreach (SignerInformation si in signerStore.GetSigners())
                {                    
                    try
                    {
                        //jbonilla - Hack para evitar errores cuando una firma ya ha sido extendida.
                        //Se asume que sólo se extiende las firmas desde BES.
                        //TODO jbonilla - Se debería validar hasta qué punto se extendió (BES, T, C, X, XL).
                        if(si.UnsignedAttributes.Count == 0)
                        {
                            siArray.AddItem(ExtendCMSSignature(signedData, si, parameters, originalData));
                        }
                        else
                        {
                            LOG.Error("Already extended?");
                            siArray.AddItem(si);
                        }                        
                    }
                    catch (IOException)
                    {
                        LOG.Error("Exception when extending signature");
                        siArray.AddItem(si);
                    }
                }
				
				SignerInformationStore newSignerStore = new SignerInformationStore(siArray);
				CmsSignedData extended = CmsSignedData.ReplaceSigners(signedData, newSignerStore);
				return new InMemoryDocument(extended.GetEncoded());
			}
			catch (CmsException)
			{
				throw new IOException("Cannot parse CMS data");
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual Document ExtendSignature(object signatureId, Document document, Document
			 originalData, SignatureParameters parameters)
		{
			SignerID toExtendId = (SignerID)signatureId;
			try
			{
				CmsSignedData signedData = new CmsSignedData(document.OpenStream());
				SignerInformationStore signerStore = signedData.GetSignerInfos();
				AList<SignerInformation> siArray = new AList<SignerInformation>();
				//Iterator<object> infos = signerStore.GetSigners().Iterator();
                IEnumerator infos = signerStore.GetSigners().GetEnumerator();
				while (infos.MoveNext())
				{
					SignerInformation si = (SignerInformation)infos.Current;
					if (si.SignerID.Equals(toExtendId))
					{
						try
						{
							siArray.AddItem(ExtendCMSSignature(signedData, si, parameters, originalData));
						}
						catch (IOException)
						{
							LOG.Error("Exception when extending signature");
							siArray.AddItem(si);
						}
					}
				}
				SignerInformationStore newSignerStore = new SignerInformationStore(siArray);
				CmsSignedData extended = CmsSignedData.ReplaceSigners(signedData, newSignerStore);
				return new InMemoryDocument(extended.GetEncoded());
			}
			catch (CmsException)
			{
				throw new IOException("Cannot parse CMS data");
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		protected internal abstract SignerInformation ExtendCMSSignature(CmsSignedData signedData
			, SignerInformation si, SignatureParameters parameters, Document originalData);

		/// <summary>
		/// Computes an attribute containing a time-stamp token of the provided data, from the provided TSA using the
		/// provided.
		/// </summary>
		/// <remarks>
		/// Computes an attribute containing a time-stamp token of the provided data, from the provided TSA using the
		/// provided. The hashing is performed by the method using the specified algorithm and a BouncyCastle provider.
		/// </remarks>
		/// <param name="signedData"></param>
		/// <exception cref="System.Exception">System.Exception</exception>
		protected internal virtual BcCms.Attribute GetTimeStampAttribute(DerObjectIdentifier oid
			, ITspSource tsa, AlgorithmIdentifier digestAlgorithm, byte[] messageImprint)
		{
			try
			{
                //jbonilla Hack para obtener el digest del TSA
                IDigest digest = null;
                string algorithmName = null;
                if (tsa is ITSAClient)
                {
                    //TODO jbonilla - ¿AlgorithmIdentifier?
                    digest = ((ITSAClient)tsa).GetMessageDigest();
                    algorithmName = digest.AlgorithmName;                    
                }
                else
                {
                    digest = DigestUtilities.GetDigest(DigestAlgorithm.SHA1.GetName());
                    algorithmName = DigestAlgorithm.SHA1.GetName();
                }
                byte[] toTimeStamp = DigestAlgorithms.Digest(digest, messageImprint);

                TimeStampResponse tsresp = tsa.GetTimeStampResponse(DigestAlgorithm.GetByName(algorithmName)
                    , toTimeStamp);
				TimeStampToken tstoken = tsresp.TimeStampToken;
				if (tstoken == null)
				{
					throw new ArgumentNullException("The TimeStampToken returned for the signature time stamp was empty."
						);
				}
				BcCms.Attribute signatureTimeStamp = new BcCms.Attribute(oid, new DerSet(Asn1Object.FromByteArray
					(tstoken.GetEncoded())));
				return signatureTimeStamp;
			}
			catch (IOException e)
			{
				throw new RuntimeException(e);
			}
			catch (NoSuchAlgorithmException e)
			{
				throw new RuntimeException(e);
			}
		}

		/// <param name="signedData"></param>
		/// <returns></returns>
		public virtual CmsSignedData ExtendCMSSignedData(CmsSignedData signedData, Document
			 originalData, SignatureParameters parameters)
		{
			SignerInformationStore signerStore = signedData.GetSignerInfos();
			AList<SignerInformation> siArray = new AList<SignerInformation>();
			//Iterator<SignerInformation> infos = signerStore.GetSigners().Iterator();
            IEnumerator infos = signerStore.GetSigners().GetEnumerator();
			while (infos.MoveNext())
			{
                SignerInformation si = (SignerInformation)infos.Current;
				try
				{
					siArray.AddItem(ExtendCMSSignature(signedData, si, parameters, originalData));
				}
				catch (IOException)
				{
					LOG.Error("Exception when extending signature");
					siArray.AddItem(si);
				}
			}
			SignerInformationStore newSignerStore = new SignerInformationStore(siArray);
			return CmsSignedData.ReplaceSigners(signedData, newSignerStore);
		}
	}
}
