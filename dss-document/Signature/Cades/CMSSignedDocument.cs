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
using EU.Europa.EC.Markt.Dss.Signature;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Cms;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Signature.Cades
{
	/// <summary>A document composed by a CmsSignedData</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class CMSSignedDocument : Document
	{
		protected internal CmsSignedData signedData;

		/// <summary>The default constructor for CMSSignedDocument.</summary>
		/// <remarks>The default constructor for CMSSignedDocument.</remarks>
		/// <param name="data"></param>
		/// <exception cref="System.IO.IOException">System.IO.IOException</exception>
		public CMSSignedDocument(CmsSignedData data)
		{
			this.signedData = data;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual Stream OpenStream()
		{
            Stream output = new MemoryStream();
			DerOutputStream derOuput = new DerOutputStream(output);
			derOuput.WriteObject(Asn1Object.FromByteArray(signedData.GetEncoded()));
            output.Seek(0, SeekOrigin.Begin);
			return output;
		}

		/// <returns>the signedData</returns>
		public virtual CmsSignedData GetCMSSignedData()
		{
			return signedData;
		}

		public virtual string GetName()
		{
			return null;
		}

		public virtual MimeType GetMimeType()
		{
			return MimeType.Pkcs7;
		}
	}
}
