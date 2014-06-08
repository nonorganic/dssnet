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

namespace EU.Europa.EC.Markt.Dss.Signature
{
	/// <summary>In memory representation of a document</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class InMemoryDocument : Document
	{
		private string name;

		private MimeType mimeType;

		private byte[] document;

		/// <summary>Create document that retains the data in memory</summary>
		/// <param name="document"></param>
		public InMemoryDocument(byte[] document) : this(document, null, null)
		{
		}

		public InMemoryDocument(byte[] document, string name, MimeType mimeType)
		{
			this.document = document;
			this.name = name;
			this.mimeType = mimeType;
		}

		public InMemoryDocument(byte[] document, string name)
		{
			this.document = document;
			this.name = name;
			this.mimeType = MimeType.FromFileName(name);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual Stream OpenStream()
		{
			return new MemoryStream(document);
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual MimeType GetMimeType()
		{
			return mimeType;
		}
	}
}
