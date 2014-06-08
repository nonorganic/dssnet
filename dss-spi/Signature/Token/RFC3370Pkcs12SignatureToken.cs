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
using Org.BouncyCastle.Security.Certificates;
using Sharpen;
using System.Collections.Generic;
using System.IO;

namespace EU.Europa.EC.Markt.Dss.Signature.Token
{
    /// <summary>Class holding all PKCS#12 file access logic.</summary>
    /// <remarks>Class holding all PKCS#12 file access logic.</remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class RFC3370Pkcs12SignatureToken : AsyncSignatureTokenConnection
    {
        private char[] password;

        private FilePath pkcs12File;

        /// <summary>Create a SignatureTokenConnection with the provided password and path to PKCS#12 file.
        /// 	</summary>
        /// <remarks>
        /// Create a SignatureTokenConnection with the provided password and path to PKCS#12 file. The default constructor
        /// for Pkcs12SignatureToken.
        /// </remarks>
        /// <param name="password"></param>
        /// <param name="pkcs12FilePath"></param>
        public RFC3370Pkcs12SignatureToken(string password, string pkcs12FilePath)
            : this
                (password.ToCharArray(), new FilePath(pkcs12FilePath))
        {
        }

        /// <summary>Create a SignatureTokenConnection with the provided password and path to PKCS#12 file.
        /// 	</summary>
        /// <remarks>
        /// Create a SignatureTokenConnection with the provided password and path to PKCS#12 file. The default constructor
        /// for Pkcs12SignatureToken.
        /// </remarks>
        /// <param name="password"></param>
        /// <param name="pkcs12FilePath"></param>
        public RFC3370Pkcs12SignatureToken(char[] password, string pkcs12FilePath)
            : this
                (password, new FilePath(pkcs12FilePath))
        {
        }

        /// <summary>Create a SignatureTokenConnection with the provided password and path to PKCS#12 file.
        /// 	</summary>
        /// <remarks>
        /// Create a SignatureTokenConnection with the provided password and path to PKCS#12 file. The default constructor
        /// for Pkcs12SignatureToken.
        /// </remarks>
        /// <param name="password"></param>
        /// <param name="pkcs12FilePath"></param>
        public RFC3370Pkcs12SignatureToken(string password, FilePath pkcs12File)
            : this(password
                .ToCharArray(), pkcs12File)
        {
        }

        /// <summary>Create a SignatureTokenConnection with the provided password and path to PKCS#12 file.
        /// 	</summary>
        /// <remarks>
        /// Create a SignatureTokenConnection with the provided password and path to PKCS#12 file. The default constructor
        /// for Pkcs12SignatureToken.
        /// </remarks>
        /// <param name="password"></param>
        /// <param name="pkcs12FilePath"></param>
        public RFC3370Pkcs12SignatureToken(char[] password, FilePath pkcs12File)
        {
            this.password = password;
            if (!pkcs12File.Exists())
            {
                throw new RuntimeException("File Not Found " + pkcs12File.GetAbsolutePath());
            }
            this.pkcs12File = pkcs12File;
        }

        public override void Close()
        {
        }

        /// <exception cref="Sharpen.NoSuchAlgorithmException"></exception>
        public override byte[] EncryptDigest(byte[] digestValue, DigestAlgorithm digestAlgo
            , IDssPrivateKeyEntry keyEntry)
        {
            try
            {
                ByteArrayOutputStream digestInfo = new ByteArrayOutputStream();
                //jbonilla: cambio de enum a clase.
                if (digestAlgo.Equals(DigestAlgorithm.SHA1))
                {
                    digestInfo.Write(Constants.SHA1_DIGEST_INFO_PREFIX);
                }
                else
                {
                    if (digestAlgo.Equals(DigestAlgorithm.SHA256))
                    {
                        digestInfo.Write(Constants.SHA256_DIGEST_INFO_PREFIX);
                    }
                    else
                    {
                        if (digestAlgo.Equals(DigestAlgorithm.SHA256))
                        {
                            digestInfo.Write(Constants.SHA512_DIGEST_INFO_PREFIX);
                        }
                    }
                }
                digestInfo.Write(digestValue);
                //Sharpen.Cipher cipher = Sharpen.Cipher.GetInstance(keyEntry.GetSignatureAlgorithm
                //    ().GetPadding());
                IBufferedCipher cipher = CipherUtilities.GetCipher(keyEntry.GetSignatureAlgorithm
                    ().GetPadding());

                //cipher.Init(Sharpen.Cipher.ENCRYPT_MODE, ((KSPrivateKeyEntry)keyEntry).GetPrivateKey
                //    ());
                cipher.Init(true, ((KSPrivateKeyEntry)keyEntry).PrivateKey);
                return cipher.DoFinal(digestInfo.ToByteArray());
            }
            catch (IOException e)
            {
                // Writing in a ByteArrayOutputStream. Should never happens.
                throw new RuntimeException(e);
            }
            /*catch (NoSuchPaddingException e)
            {
                throw new RuntimeException(e);
            }*/
            catch (InvalidKeyException e)
            {
                throw new RuntimeException(e);
            }
            /*catch (IllegalBlockSizeException e)
            {
                throw new RuntimeException(e);
            }
            catch (BadPaddingException)
            {
                // More likely the password is not good.
                throw new BadPasswordException(BadPasswordException.MSG.PKCS12_BAD_PASSWORD);
            }*/
        }

        /// <exception cref="Sharpen.KeyStoreException"></exception>
        public override IList<IDssPrivateKeyEntry> GetKeys()
        {
            IList<IDssPrivateKeyEntry> list = new AList<IDssPrivateKeyEntry>();
            try
            {
                //TODO 
                //KeyStore keyStore = KeyStore.GetInstance("PKCS12");
                //FileInputStream input = new FileInputStream(pkcs12File);
                //keyStore.Load(input, password);
                //input.Close();
                //Enumeration<string> aliases = keyStore.Aliases();
                //while (aliases.MoveNext())
                //{
                //    string alias = aliases.Current;
                //    if (keyStore.IsKeyEntry(alias))
                //    {
                //        KeyStore.PrivateKeyEntry entry = (KeyStore.PrivateKeyEntry)keyStore.GetEntry(alias
                //            , new KeyStore.PasswordProtection(password));
                //        list.AddItem(new KSPrivateKeyEntry(entry));
                //    }
                //}
            }
            catch (IOException e)
            {
                //if (e.InnerException is BadPaddingException)
                //{
                //    throw new BadPasswordException(BadPasswordException.MSG.PKCS12_BAD_PASSWORD);
                //}
                //throw new KeyStoreException("Can't initialize Sun PKCS#12 security " + "provider. Reason: "
                //     + e.InnerException.Message, e);
                throw;
            }
            catch (NoSuchAlgorithmException e)
            {
                /*throw new KeyStoreException("Can't initialize Sun PKCS#12 security " + "provider. Reason: "
                     + e.InnerException.Message, e);*/
                throw;
            }
            catch (CertificateException e)
            {
                /*throw new KeyStoreException("Can't initialize Sun PKCS#12 security " + "provider. Reason: "
                     + e.InnerException.Message, e);*/
                throw;
            }
            /*catch (UnrecoverableEntryException e)
            {
                throw new KeyStoreException("Can't initialize Sun PKCS#12 security " + "provider. Reason: "
                     + e.InnerException.Message, e);
            }*/
            return list;
        }
    }
}
