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

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.Collections.Generic;
using System.IO;

namespace EU.Europa.EC.Markt.Dss.Signature.Token
{
    /// <summary>Sometimes, the signature process has to be split in two phases : the digest phase and the encryption phase.
    /// 	</summary>
    /// <remarks>
    /// Sometimes, the signature process has to be split in two phases : the digest phase and the encryption phase. This
    /// separation is useful when the file and the SSCD are not on the same hardware. Two implementation of
    /// AsyncSignatureTokenConnection are provided. Only MSCAPI requires the signature to be done in one step (MS CAPI don't
    /// provide any RSA encryption operations).
    /// </remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public abstract class AsyncSignatureTokenConnection : ISignatureTokenConnection
    {
        /// <summary>The encryption of a digest it the atomic operation done by the SSCD.</summary>
        /// <remarks>
        /// The encryption of a digest it the atomic operation done by the SSCD. This encryption (RSA, DSA, ...) create the
        /// signature value.
        /// </remarks>
        /// <param name="digestValue"></param>
        /// <param name="digestAlgo"></param>
        /// <param name="keyEntry"></param>
        /// <returns></returns>
        /// <exception cref="Sharpen.NoSuchAlgorithmException"></exception>
        public abstract byte[] EncryptDigest(byte[] digestValue, DigestAlgorithm digestAlgo
            , IDssPrivateKeyEntry keyEntry);

        /// <summary>The encryption of a digest it the atomic operation done by the SSCD.</summary>
        /// <remarks>
        /// The encryption of a digest it the atomic operation done by the SSCD. This encryption (RSA, DSA, ...) create the
        /// signature value.
        /// </remarks>
        /// <param name="digest"></param>
        /// <param name="keyEntry"></param>
        /// <returns></returns>
        /// <exception cref="Sharpen.NoSuchAlgorithmException">Sharpen.NoSuchAlgorithmException
        /// 	</exception>
        public virtual byte[] EncryptDigest(Digest digest, IDssPrivateKeyEntry keyEntry)
        {
            return this.EncryptDigest(digest.GetValue(), digest.GetAlgorithm(), keyEntry);
        }

        /// <exception cref="Sharpen.NoSuchAlgorithmException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        public virtual byte[] Sign(Stream stream, DigestAlgorithm digestAlgo, IDssPrivateKeyEntry
             keyEntry)
        {
            if (SignatureAlgorithm.RSA == keyEntry.GetSignatureAlgorithm())
            {
                IDigest digester = DigestUtilities.GetDigest(digestAlgo.GetName());
                byte[] buffer = new byte[4096];
                int count = 0;
                while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    digester.BlockUpdate(buffer, 0, count);
                }
                byte[] digestValue = DigestUtilities.DoFinal(digester);
                return EncryptDigest(digestValue, digestAlgo, keyEntry);
            }
            else
            {
                //jbonilla
                throw new System.NotImplementedException("Implementar cuando no es RSA");
                //Sharpen.Signature signature = Sharpen.Signature.GetInstance(keyEntry.GetSignatureAlgorithm
                //    ().GetJavaSignatureAlgorithm(digestAlgo));
                //try
                //{
                //    signature.InitSign(((KSPrivateKeyEntry)keyEntry).GetPrivateKey());
                //    byte[] buffer = new byte[4096];
                //    int count = 0;
                //    while ((count = stream.Read(buffer)) > 0)
                //    {
                //        signature.Update(buffer, 0, count);
                //    }
                //    byte[] signValue = signature.Sign();
                //    return signValue;
                //}
                //catch (SignatureException e)
                //{
                //    throw new RuntimeException(e);
                //}
                //catch (InvalidKeyException e)
                //{
                //    throw new RuntimeException(e);
                //}
            }
        }

        public abstract void Close();

        public abstract IList<IDssPrivateKeyEntry> GetKeys();
    }
}
