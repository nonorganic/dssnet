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

//jbonilla Serviría para conectarse a una BDD, quizá en un servidor web. 
//De momento, para la aplicación de escritorio está bien un File
//Requiere: Spring Data
/*
using System;
using System.Collections.Generic;
using System.IO;
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Validation.Crl;
using Sharpen;
using Spring.Dao.Support;
using iTextSharp.text.log;
using Org.BouncyCastle.X509;
using Spring.Data.Generic;
using Spring.Data;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Security.Certificates;
using System.Data;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.Encoders;
using System.Data.Common;
using Spring.Data.Common;


namespace EU.Europa.EC.Markt.Dss.Validation.Crl
{
	/// <summary>CRLSource that retrieve information from a JDBC datasource</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class AdoCacheCRLSource : AdoDaoSupport, ICrlSource
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Crl.AdoCacheCRLSource
			).FullName);

		private OnlineCRLSource cachedSource;

		/// <summary>The default constructor for JdbcCRLSource.</summary>
		/// <remarks>The default constructor for JdbcCRLSource.</remarks>
		public AdoCacheCRLSource()
		{     
		}

		/// <param name="cachedSource">the cachedSource to set</param>
		public virtual void SetCachedSource(OnlineCRLSource cachedSource)
		{
			this.cachedSource = cachedSource;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void InitDao()
		{
			base.InitDao();
			try
			{
                this.AdoTemplate.ExecuteScalar(CommandType.Text
                    , "SELECT COUNT(*) FROM CACHED_CRL");
			}
			catch (BadSqlGrammarException)
			{
                this.AdoTemplate.ExecuteNonQuery(CommandType.Text
                    , "CREATE TABLE CACHED_CRL ( ID CHAR(20), DATA LONGVARBINARY)");
			}
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual X509Crl FindCrl(X509Certificate certificate, X509Certificate issuerCertificate
			)
		{
			OnlineCRLSource source = new OnlineCRLSource();
			string crlUrl = source.GetCrlUri(certificate);
			if (crlUrl != null)
			{
				try
				{					
					string key = Hex.ToHexString(
                        DigestUtilities.CalculateDigest(DigestAlgorithm.SHA1.GetOid()
                        , Sharpen.Runtime.GetBytesForString(crlUrl)));

                    IList<CachedCRL> crls = AdoTemplate.QueryWithRowMapperDelegate<CachedCRL>(CommandType.Text
                        , "SELECT * FROM CACHED_CRL WHERE ID = :ID"
                        , delegate(IDataReader dataReader, int rowNum)
                        {
                            CachedCRL cached = new CachedCRL();
                            cached.SetKey((string)dataReader["ID"]);
                            cached.SetCrl((byte[])dataReader["DATA"]);
                            return cached;
                        }
                        , "ID", DbType.String, key.Length, key
                        );                        

					if (crls.Count == 0)
					{
						LOG.Info("CRL not in cache");
						X509Crl originalCRL = cachedSource.FindCrl(certificate, issuerCertificate);
						if (originalCRL != null)
						{
                             
                            IDbParameters insertParams = CreateDbParameters();
                            insertParams.AddWithValue("ID", key);
                            insertParams.AddWithValue("DATA", originalCRL.GetEncoded());

                            AdoTemplate.ExecuteNonQuery(CommandType.Text
                                , "INSERT INTO CACHED_CRL (ID, DATA) VALUES (:ID,:DATA)"
                                , insertParams);
                            
							return originalCRL;
						}
						else
						{
							return null;
						}
					}
					CachedCRL crl = crls[0];

                    X509CrlParser parser = new X509CrlParser();
                    X509Crl x509crl = parser.ReadCrl(crl.GetCrl());
					
					if (x509crl.NextUpdate.Value.CompareTo(DateTime.Now) > 0)
					{
						LOG.Info("CRL in cache");
						return x509crl;
					}
					else
					{
						LOG.Info("CRL expired");
						X509Crl originalCRL = cachedSource.FindCrl(certificate, issuerCertificate);

                        IDbParameters updateParams = CreateDbParameters();
                        updateParams.AddWithValue("ID", key);
                        updateParams.AddWithValue("DATA", originalCRL.GetEncoded());

                        AdoTemplate.ExecuteNonQuery(CommandType.Text
                               , "UPDATE CACHED_CRL SET DATA = :DATA  WHERE ID = :ID "
                               , updateParams);
						
						return originalCRL;
					}
				}
				catch (NoSuchAlgorithmException)
				{
					LOG.Info("Cannot instantiate digest for algorithm SHA1 !?");
				}
				catch (CrlException)
				{
					LOG.Info("Cannot serialize CRL");
				}
				catch (CertificateException)
				{
					LOG.Info("Cannot instanciate X509 Factory");
				}
			}
			return null;
		}		
    }
}
*/