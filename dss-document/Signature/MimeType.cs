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

namespace EU.Europa.EC.Markt.Dss.Signature
{
	/// <summary>
	/// TODO
	/// <p>
	/// DISCLAIMER: Project owner DG-MARKT.
	/// </summary>
	/// <remarks>
	/// TODO
	/// <p>
	/// DISCLAIMER: Project owner DG-MARKT.
	/// </remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	/// <author><a href="mailto:dgmarkt.Project-DSS@arhs-developments.com">ARHS Developments</a>
	/// 	</author>
	public sealed class MimeType
	{
		public static readonly MimeType Binary = new MimeType
			("application/octet-stream");

		public static readonly MimeType Xml = new MimeType
			("text/xml");

		public static readonly MimeType Pdf = new MimeType
			("application/pdf");

		public static readonly MimeType Pkcs7 = new MimeType
			("application/pkcs7-signature");

		private string code;

		/// <summary>The default constructor for MimeTypes.</summary>
		/// <remarks>The default constructor for MimeTypes.</remarks>
		private MimeType(string code)
		{
			//jbonilla
			this.code = code;
		}

		/// <returns>the code</returns>
		public string GetCode()
		{
			return code;
		}

		public static EU.Europa.EC.Markt.Dss.Signature.MimeType FromFileName(string name)
		{
			if (name.ToLower().EndsWith(".xml"))
			{
				return Xml;
			}
			else
			{
				if (name.ToLower().EndsWith(".pdf"))
				{
					return Pdf;
				}
				else
				{
					return Binary;
				}
			}
		}

		public override int GetHashCode()
		{
			int prime = 31;
			int result = 1;
			result = prime * result + ((code == null) ? 0 : code.GetHashCode());
			return result;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null)
			{
				return false;
			}
			if (!(obj is EU.Europa.EC.Markt.Dss.Signature.MimeType))
			{
				return false;
			}
			EU.Europa.EC.Markt.Dss.Signature.MimeType other = (EU.Europa.EC.Markt.Dss.Signature.MimeType
				)obj;
			if (code == null)
			{
				if (other.code != null)
				{
					return false;
				}
			}
			else
			{
				if (!code.Equals(other.code))
				{
					return false;
				}
			}
			return true;
		}
	}
}
