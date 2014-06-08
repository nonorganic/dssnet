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
	/// <summary>Information about the QCStatement in the certificate</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class QCStatementInformation
	{
		private Result QCPPresent;

		private Result QCPPlusPresent;

		private Result QcCompliancePresent;

		private Result QcSCCDPresent;

		/// <returns></returns>
		public virtual Result GetQCPPresent()
		{
			return QCPPresent;
		}

		/// <param name="qCPPresent"></param>
		public virtual void SetQCPPresent(Result qCPPresent)
		{
			QCPPresent = qCPPresent;
		}

		/// <returns></returns>
		public virtual Result GetQCPPlusPresent()
		{
			return QCPPlusPresent;
		}

		/// <param name="qCPPlusPresent"></param>
		public virtual void SetQCPPlusPresent(Result qCPPlusPresent)
		{
			QCPPlusPresent = qCPPlusPresent;
		}

		/// <returns></returns>
		public virtual Result GetQcCompliancePresent()
		{
			return QcCompliancePresent;
		}

		/// <param name="qcCompliancePresent"></param>
		public virtual void SetQcCompliancePresent(Result qcCompliancePresent)
		{
			QcCompliancePresent = qcCompliancePresent;
		}

		/// <returns></returns>
		public virtual Result GetQcSCCDPresent()
		{
			return QcSCCDPresent;
		}

		/// <param name="qcSCCDPresent"></param>
		public virtual void SetQcSCCDPresent(Result qcSCCDPresent)
		{
			QcSCCDPresent = qcSCCDPresent;
		}

		/// <summary>The default constructor for QCStatementInformation.</summary>
		/// <remarks>The default constructor for QCStatementInformation.</remarks>
		/// <param name="name"></param>
		/// <param name="qCPPresent"></param>
		/// <param name="qCPPlusPresent"></param>
		/// <param name="qcCompliancePresent"></param>
		/// <param name="qcSCCDPresent"></param>
		public QCStatementInformation(Result qCPPresent, Result qCPPlusPresent, Result qcCompliancePresent
			, Result qcSCCDPresent)
		{
			this.QCPPresent = qCPPresent;
			this.QCPPlusPresent = qCPPlusPresent;
			this.QcCompliancePresent = qcCompliancePresent;
			this.QcSCCDPresent = qcSCCDPresent;
		}
	}
}
