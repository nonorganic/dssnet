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

using iTextSharp.text.log;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using Org.BouncyCastle.X509;
using Sharpen;
using System.Collections.Generic;
using System.IO;

namespace EU.Europa.EC.Markt.Dss.Validation.Certificate
{
    /// <summary>Implements a CertificateSource using a JKS KeyStore.</summary>
    /// <remarks>Implements a CertificateSource using a JKS KeyStore.</remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class KeyStoreCertificateSource : OfflineCertificateSource
    {
        private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Certificate.KeyStoreCertificateSource
            ).FullName);

        private FilePath keyStoreFile;

        private string password;

        private string keyStoreType;

        /// <summary>The default constructor for KeyStoreCertificateSource.</summary>
        /// <remarks>The default constructor for KeyStoreCertificateSource.</remarks>
        public KeyStoreCertificateSource(string keyStoreFilename, string password)
            : this
                (new FilePath(keyStoreFilename), "JKS", password)
        {
        }

        /// <summary>The default constructor for KeyStoreCertificateSource.</summary>
        /// <remarks>The default constructor for KeyStoreCertificateSource.</remarks>
        public KeyStoreCertificateSource(FilePath keyStoreFile, string password)
            : this(keyStoreFile
                , "JKS", password)
        {
        }

        /// <summary>The default constructor for MockTSLCertificateSource.</summary>
        /// <remarks>The default constructor for MockTSLCertificateSource.</remarks>
        public KeyStoreCertificateSource(FilePath keyStoreFile, string keyStoreType, string
             password)
        {
            this.keyStoreFile = keyStoreFile;
            this.keyStoreType = keyStoreType;
            this.password = password;
        }

        public override IList<X509Certificate> GetCertificates()
        {
            IList<X509Certificate> certificates = new AList<X509Certificate>();
            try
            {
                throw new System.NotImplementedException();
                //TODO jbonilla - validar como le hicimos en Intisign
                //KeyStore keyStore = KeyStore.GetInstance(keyStoreType);
                //keyStore.Load(new FileInputStream(keyStoreFile), password.ToCharArray());
                //Enumeration<string> aliases = keyStore.Aliases();
                //while (aliases.MoveNext())
                //{
                //    string alias = aliases.Current;
                //    Sharpen.Certificate onecert = keyStore.GetCertificate(alias);
                //    LOG.Info("Alias " + alias + " Cert " + ((X509Certificate)onecert).SubjectDN);
                //    if (onecert != null)
                //    {
                //        certificates.AddItem((X509Certificate)onecert);
                //    }
                //    if (keyStore.GetCertificateChain(alias) != null)
                //    {
                //        foreach (Sharpen.Certificate cert in keyStore.GetCertificateChain(alias))
                //        {
                //            LOG.Info("Alias " + alias + " Cert " + ((X509Certificate)cert).SubjectDN);
                //            if (!certificates.Contains(cert))
                //            {
                //                certificates.AddItem((X509Certificate)cert);
                //            }
                //        }
                //    }
                //}
            }
            catch (CertificateException)
            {
                throw new EncodingException(EncodingException.MSG.CERTIFICATE_CANNOT_BE_READ);
            }
            /*catch (KeyStoreException)
            {
                throw new EncodingException(EncodingException.MSG.CERTIFICATE_CANNOT_BE_READ);
            }*/
            catch (NoSuchAlgorithmException)
            {
                throw new EncodingException(EncodingException.MSG.CERTIFICATE_CANNOT_BE_READ);
            }
            catch (FileNotFoundException)
            {
                throw new EncodingException(EncodingException.MSG.CERTIFICATE_CANNOT_BE_READ);
            }
            catch (IOException)
            {
                throw new EncodingException(EncodingException.MSG.CERTIFICATE_CANNOT_BE_READ);
            }
            return certificates;
        }
    }
}
