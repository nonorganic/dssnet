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
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Signature
{
	/// <summary>Interface for DocumentSignatureService.</summary>
	/// <remarks>Interface for DocumentSignatureService. Provides operations for sign/verify a document.
	/// 	</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public interface DocumentSignatureService
	{
		/// <summary>Digest content of the Document.</summary>
		/// <remarks>Digest content of the Document.</remarks>
		/// <param name="document"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		/// <exception cref="System.IO.IOException">System.IO.IOException</exception>
		/// <Deprecated>Should use the SignatureTokenConnection ability to digest instead.</Deprecated>
		EU.Europa.EC.Markt.Dss.Digest Digest(Document document, SignatureParameters parameters
			);

		/// <summary>Retrieve the stream of data that need to be signed.</summary>
		/// <remarks>Retrieve the stream of data that need to be signed.</remarks>
		/// <param name="document"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		/// <exception cref="System.IO.IOException">System.IO.IOException</exception>
		Stream ToBeSigned(Document document, SignatureParameters parameters);

		/// <summary>Sign the document with the provided signatureValue.</summary>
		/// <remarks>Sign the document with the provided signatureValue.</remarks>
		/// <param name="document"></param>
		/// <param name="parameters"></param>
		/// <param name="signatureValue"></param>
		/// <returns></returns>
		/// <exception cref="System.IO.IOException">System.IO.IOException</exception>
		Document SignDocument(Document document, SignatureParameters parameters, byte[] signatureValue
			);

		/// <summary>Extend the level of the signatures in the document</summary>
		/// <param name="document"></param>
		/// <param name="originalDocument">
		/// In case of extending a detached signature up to level -A, the original document is
		/// needed.
		/// </param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		/// <exception cref="System.IO.IOException">System.IO.IOException</exception>
		Document ExtendDocument(Document document, Document originalDocument, SignatureParameters
			 parameters);
	}
}
