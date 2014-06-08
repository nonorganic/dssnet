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

using System;
using EU.Europa.EC.Markt.Dss.Validation.Report;
using Sharpen;
using iTextSharp.text.log;
//using Sharpen.Logging;

namespace EU.Europa.EC.Markt.Dss.Validation.Report
{
	/// <summary>Validation information about a Signature.</summary>
	/// <remarks>Validation information about a Signature.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class SignatureInformation
	{
		private static readonly ILogger LOG = LoggerFactory.GetLogger(typeof(EU.Europa.EC.Markt.Dss.Validation.Report.SignatureInformation
			).FullName);

		public enum FinalConclusions
		{
			QES,
			AdES_QC,
			AdES,
			UNDETERMINED
		}
        
        /// <returns>the signatureVerification</returns>
        public SignatureVerification SignatureVerification { get; private set; }

        /// <returns>the certPathRevocationAnalysis</returns>
        public CertPathRevocationAnalysis CertPathRevocationAnalysis { get; private set; }

        /// <returns>the signatureLevelAnalysis</returns>
        public SignatureLevelAnalysis SignatureLevelAnalysis { get; private set; }

        /// <returns>the qualificationsVerification</returns>
        public QualificationsVerification QualificationsVerification { get; private set; }

        /// <returns>the qcStatementInformation</returns>
        public QCStatementInformation QcStatementInformation { get; private set; }

        /// <returns>the finalConclusion</returns>
        public SignatureInformation.FinalConclusions FinalConclusion { get; private set; }

        /// <returns>the finalConclusionComment</returns>
        public string FinalConclusionComment { get; private set; }

		/// <summary>The default constructor for SignatureInformation.</summary>
		/// <remarks>The default constructor for SignatureInformation.</remarks>
		/// <param name="name"></param>
		/// <param name="signatureStructureVerification"></param>
		/// <param name="signatureVerification"></param>
		/// <param name="certPathRevocationAnalysis"></param>
		/// <param name="signatureLevelAnalysis"></param>
		/// <param name="qualificationsVerification"></param>
		/// <param name="qcStatementInformation"></param>
		/// <param name="finalConclusion"></param>
		public SignatureInformation(SignatureVerification signatureVerification, CertPathRevocationAnalysis
			 certPathRevocationAnalysis, SignatureLevelAnalysis signatureLevelAnalysis, QualificationsVerification
			 qualificationsVerification, QCStatementInformation qcStatementInformation)
		{
			this.SignatureVerification = signatureVerification;
			this.CertPathRevocationAnalysis = certPathRevocationAnalysis;
			this.SignatureLevelAnalysis = signatureLevelAnalysis;
			this.QualificationsVerification = qualificationsVerification;
			this.QcStatementInformation = qcStatementInformation;
			int tlContentCase = -1;
			if (certPathRevocationAnalysis.GetTrustedListInformation().IsServiceWasFound())
			{
				tlContentCase = 0;
			}
			if (certPathRevocationAnalysis.GetTrustedListInformation().IsServiceWasFound() &&
				 qualificationsVerification != null && qualificationsVerification.GetQCWithSSCD(
				).IsValid())
			{
				tlContentCase = 1;
			}
			if (certPathRevocationAnalysis.GetTrustedListInformation().IsServiceWasFound() &&
				 qualificationsVerification != null && qualificationsVerification.GetQCNoSSCD().
				IsValid())
			{
				tlContentCase = 2;
			}
			if (certPathRevocationAnalysis.GetTrustedListInformation().IsServiceWasFound() &&
				 qualificationsVerification != null && qualificationsVerification.GetQCSSCDStatusAsInCert
				().IsValid())
			{
				tlContentCase = 3;
			}
			if (certPathRevocationAnalysis.GetTrustedListInformation().IsServiceWasFound() &&
				 qualificationsVerification != null && qualificationsVerification.GetQCForLegalPerson
				().IsValid())
			{
				tlContentCase = 4;
			}
			if (!certPathRevocationAnalysis.GetTrustedListInformation().IsServiceWasFound())
			{
				// Case 5 and 6 are not discriminable */
				tlContentCase = 5;
				FinalConclusionComment = "no.tl.confirmation";
			}
			if (certPathRevocationAnalysis.GetTrustedListInformation().IsServiceWasFound() &&
				 !certPathRevocationAnalysis.GetTrustedListInformation().IsWellSigned())
			{
				tlContentCase = 7;
				FinalConclusionComment = "unsigned.tl.confirmation";
			}
			int certContentCase = -1;
			if (qcStatementInformation != null && !qcStatementInformation.GetQcCompliancePresent
				().IsValid() && !qcStatementInformation.GetQCPPlusPresent().IsValid() && qcStatementInformation
				.GetQCPPresent().IsValid() && !qcStatementInformation.GetQcSCCDPresent().IsValid
				())
			{
				certContentCase = 0;
			}
			if (qcStatementInformation != null && qcStatementInformation.GetQcCompliancePresent
				().IsValid() && !qcStatementInformation.GetQCPPlusPresent().IsValid() && qcStatementInformation
				.GetQCPPresent().IsValid() && !qcStatementInformation.GetQcSCCDPresent().IsValid
				())
			{
				certContentCase = 1;
			}
			if (qcStatementInformation != null && qcStatementInformation.GetQcCompliancePresent
				().IsValid() && !qcStatementInformation.GetQCPPlusPresent().IsValid() && qcStatementInformation
				.GetQCPPresent().IsValid() && qcStatementInformation.GetQcSCCDPresent().IsValid(
				))
			{
				certContentCase = 2;
			}
			if (qcStatementInformation != null && !qcStatementInformation.GetQcCompliancePresent
				().IsValid() && qcStatementInformation.GetQCPPlusPresent().IsValid() && !qcStatementInformation
				.GetQCPPresent().IsValid() && !qcStatementInformation.GetQcSCCDPresent().IsValid
				())
			{
				certContentCase = 3;
			}
			if (qcStatementInformation != null && qcStatementInformation.GetQcCompliancePresent
				().IsValid() && qcStatementInformation.GetQCPPlusPresent().IsValid() && !qcStatementInformation
				.GetQCPPresent().IsValid() && !qcStatementInformation.GetQcSCCDPresent().IsValid
				())
			{
				certContentCase = 4;
			}
			if (qcStatementInformation != null && qcStatementInformation.GetQcCompliancePresent
				().IsValid() && qcStatementInformation.GetQCPPlusPresent().IsValid() && qcStatementInformation
				.GetQcSCCDPresent().IsValid())
			{
				// QCPPlus stronger than QCP. If QCP is present, then it's ok.
				// && !qcStatementInformation.getQCPPresent().isValid()
				certContentCase = 5;
			}
			if (qcStatementInformation != null && qcStatementInformation.GetQcCompliancePresent
				().IsValid() && !qcStatementInformation.GetQCPPlusPresent().IsValid() && !qcStatementInformation
				.GetQCPPresent().IsValid() && !qcStatementInformation.GetQcSCCDPresent().IsValid
				())
			{
				certContentCase = 6;
			}
			if (qcStatementInformation != null && !qcStatementInformation.GetQcCompliancePresent
				().IsValid() && !qcStatementInformation.GetQCPPlusPresent().IsValid() && !qcStatementInformation
				.GetQCPPresent().IsValid() && qcStatementInformation.GetQcSCCDPresent().IsValid(
				))
			{
				certContentCase = 7;
			}
			if (qcStatementInformation != null && qcStatementInformation.GetQcCompliancePresent
				().IsValid() && !qcStatementInformation.GetQCPPlusPresent().IsValid() && !qcStatementInformation
				.GetQCPPresent().IsValid() && qcStatementInformation.GetQcSCCDPresent().IsValid(
				))
			{
				certContentCase = 8;
			}
			if (qcStatementInformation == null || (!qcStatementInformation.GetQcCompliancePresent
				().IsValid() && !qcStatementInformation.GetQCPPlusPresent().IsValid() && !qcStatementInformation
				.GetQCPPresent().IsValid() && !qcStatementInformation.GetQcSCCDPresent().IsValid
				()))
			{
				certContentCase = 9;
			}
			LOG.Info("TLCase : " + (tlContentCase + 1) + " - CertCase : " + (certContentCase 
				+ 1));
			try
			{
				SignatureInformation.FinalConclusions[][] matrix = new SignatureInformation.FinalConclusions
					[][] { new SignatureInformation.FinalConclusions[] { SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.AdES }, new SignatureInformation.FinalConclusions
					[] { SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.AdES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.AdES }, new SignatureInformation.FinalConclusions[] { SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES }, new SignatureInformation.FinalConclusions
					[] { SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.AdES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.AdES }, new SignatureInformation.FinalConclusions[] { SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.AdES }, new SignatureInformation.FinalConclusions
					[] { SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.AdES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.AdES }, new SignatureInformation.FinalConclusions[] { SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.AdES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.AdES }, new SignatureInformation.FinalConclusions
					[] { SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.AdES_QC, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.QES, SignatureInformation.FinalConclusions.AdES_QC, SignatureInformation.FinalConclusions
					.AdES, SignatureInformation.FinalConclusions.QES, SignatureInformation.FinalConclusions
					.AdES } };
				FinalConclusion = matrix[tlContentCase][certContentCase];
			}
			catch (IndexOutOfRangeException)
			{
				FinalConclusion = SignatureInformation.FinalConclusions.UNDETERMINED;
			}
		}	
	}
}
