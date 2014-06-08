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

using EU.Europa.EC.Markt.Dss.Validation.Report;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Validation.Report
{
	/// <summary>Qualification of the certificate according to the QualificationElement of the Trusted List.
	/// 	</summary>
	/// <remarks>Qualification of the certificate according to the QualificationElement of the Trusted List.
	/// 	</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class QualificationsVerification
	{
		private Result QCWithSSCD;

		private Result QCNoSSCD;

		private Result QCSSCDStatusAsInCert;

		private Result QCForLegalPerson;

		/// <returns>the qCWithSSCD</returns>
		public virtual Result GetQCWithSSCD()
		{
			return QCWithSSCD;
		}

		/// <returns>the qCNoSSCD</returns>
		public virtual Result GetQCNoSSCD()
		{
			return QCNoSSCD;
		}

		/// <returns>the qCSSCDStatusAsInCert</returns>
		public virtual Result GetQCSSCDStatusAsInCert()
		{
			return QCSSCDStatusAsInCert;
		}

		/// <returns>the qCForLegalPerson</returns>
		public virtual Result GetQCForLegalPerson()
		{
			return QCForLegalPerson;
		}

		/// <summary>The default constructor for QualificationExtensionAnalysis.</summary>
		/// <remarks>The default constructor for QualificationExtensionAnalysis.</remarks>
		/// <param name="name"></param>
		/// <param name="qCWithSSCD"></param>
		/// <param name="qCNoSSCD"></param>
		/// <param name="qCSSCDStatusAsInCert"></param>
		/// <param name="qCForLegalPerson"></param>
		public QualificationsVerification(Result qCWithSSCD, Result qCNoSSCD, Result qCSSCDStatusAsInCert
			, Result qCForLegalPerson)
		{
			QCWithSSCD = qCWithSSCD;
			QCNoSSCD = qCNoSSCD;
			QCSSCDStatusAsInCert = qCSSCDStatusAsInCert;
			QCForLegalPerson = qCForLegalPerson;
		}
	}
}
