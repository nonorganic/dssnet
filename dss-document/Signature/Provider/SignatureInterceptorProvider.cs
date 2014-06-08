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

//jbonilla - No aplica para C#
/*
using EU.Europa.EC.Markt.Dss.Signature.Provider;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Signature.Provider
{
	/// <summary>Provider for the SignatureInterceptor</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	[System.Serializable]
	public class SignatureInterceptorProvider : Sharpen.Provider
	{
		public static string NAME = "SignatureInterceptor";

		private Sharpen.Provider legacy = null;

		/// <summary>The default constructor for SignatureInterceptorProvider.</summary>
		/// <remarks>The default constructor for SignatureInterceptorProvider.</remarks>
		public SignatureInterceptorProvider() : base(NAME, 1.0, "Signature Interceptor Provider"
			)
		{
			Put("Signature.SHA1withRSA", typeof(SignatureInterceptor).FullName);
		}

		/// <summary>The default constructor for SignatureInterceptorProvider.</summary>
		/// <remarks>The default constructor for SignatureInterceptorProvider.</remarks>
		/// <param name="legacy"></param>
		public SignatureInterceptorProvider(Sharpen.Provider legacy) : base(NAME, 1.0, "Signature Interceptor Provider"
			)
		{
			this.legacy = legacy;
		}

		public override Provider.Service GetService(string type, string algorithm)
		{
			lock (this)
			{
				if (legacy != null)
				{
					return legacy.GetService(type, algorithm);
				}
				return base.GetService(type, algorithm);
			}
		}
	}
}
*/