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

using System.IO;
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Validation.Https;
using Sharpen;
using iTextSharp.text.log;
using System.Net;
using Org.BouncyCastle.Utilities.IO;

namespace EU.Europa.EC.Markt.Dss.Validation.Https
{
	/// <summary>Implementation of HTTPDataLoader using HttpClient.</summary>
	/// <remarks>
	/// Implementation of HTTPDataLoader using HttpClient. More flexible for HTTPS without having to add the certificate to
	/// the JVM TrustStore.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class NetHttpDataLoader : HTTPDataLoader
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Https.NetHttpDataLoader
			).FullName);
		
        public string ContentType { get; set; }
        public string Accept { get; set; }
        public int TimeOut { get; set; }

        public NetHttpDataLoader()
        {
            this.TimeOut = 500;
        }		

        /// <exception cref="EU.Europa.EC.Markt.Dss.CannotFetchDataException"></exception>
        public Stream Get(string URL)
        {
            try
            {
                LOG.Info("Fetching data from url " + URL);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream dataStream = response.GetResponseStream();
                    return dataStream;
                }
                else
                {
                    return new MemoryStream(new byte[0]);
                }
            }
            catch (IOException ex)
            {
                throw new CannotFetchDataException(ex, URL);
            }
        }

        /// <exception cref="EU.Europa.EC.Markt.Dss.CannotFetchDataException"></exception>
        public Stream Post(string URL, InputStream content)
        {
            try
            {
                LOG.Info("Post data to url " + URL);      

                byte[] data = Streams.ReadAll(content);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Timeout = TimeOut;
                request.Method = "POST";
                request.ContentLength = data.Length;

                if (ContentType != null)
                {
                    request.ContentType = ContentType;
                }

                if (Accept != null)
                {
                    request.Accept = Accept;
                }

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(data, 0, data.Length);
                dataStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                dataStream = response.GetResponseStream();

                return dataStream;
            }
            catch (IOException ex)
            {
                throw new CannotFetchDataException(ex, URL);
            }
        }
    }
}
