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
using EU.Europa.EC.Markt.Dss.Validation;
using EU.Europa.EC.Markt.Dss.Validation.Cades;
using EU.Europa.EC.Markt.Dss.Validation.Report;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Validation.Report
{
	/// <summary>Information for all the levels of the signature.</summary>
	/// <remarks>Information for all the levels of the signature.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class SignatureLevelAnalysis
	{
		private AdvancedSignature signature;

		private SignatureLevelBES levelBES;

		private SignatureLevelEPES levelEPES;

		private SignatureLevelT levelT;

		private SignatureLevelC levelC;

		private SignatureLevelX levelX;

		private SignatureLevelXL levelXL;

		private SignatureLevelA levelA;

		private SignatureLevelLTV levelLTV;

		/// <summary>The default constructor for SignatureLevelAnalysis.</summary>
		/// <remarks>The default constructor for SignatureLevelAnalysis.</remarks>
		/// <param name="name"></param>
		/// <param name="signature"></param>
		public SignatureLevelAnalysis(AdvancedSignature signature, SignatureLevelBES levelBES
			, SignatureLevelEPES levelEPES, SignatureLevelT levelT, SignatureLevelC levelC, 
			SignatureLevelX levelX, SignatureLevelXL levelXL, SignatureLevelA levelA, SignatureLevelLTV
			 levelLTV)
		{
			//import eu.europa.ec.markt.dss.validation.pades.PAdESSignature;
			//import eu.europa.ec.markt.dss.validation.xades.XAdESSignature;
			bool levelReached = true;
			this.signature = signature;
			this.levelBES = levelBES;
			bool levelBESReached = LevelIsReached(levelBES, levelReached);
			levelReached = levelBESReached;
			this.levelEPES = levelEPES;
			LevelIsReached(levelEPES, levelReached);
			this.levelT = levelT;
			bool levelReachedT = LevelIsReached(levelT, levelReached);
			this.levelC = levelC;
			levelReached = LevelIsReached(levelC, levelReachedT);
			this.levelX = levelX;
			levelReached = LevelIsReached(levelX, levelReached);
			this.levelXL = levelXL;
			levelReached = LevelIsReached(levelXL, levelReached);
			this.levelA = levelA;
			levelReached = LevelIsReached(levelA, levelReached);
			this.levelLTV = levelLTV;
			levelReached = LevelIsReached(levelLTV, levelBESReached);
		}

		private bool LevelIsReached(SignatureLevel level, bool previousLevel)
		{
			if (level != null)
			{
				if (!previousLevel)
				{
					level.GetLevelReached().SetStatus(Result.ResultStatus.INVALID, "previous.level.has.errors"
						);
				}
				bool thisLevel = previousLevel && level.GetLevelReached().IsValid();
				return thisLevel;
			}
			else
			{
				return false;
			}
		}

		/// <returns>the signatureFormat</returns>
		public virtual string GetSignatureFormat()
		{
			string signatureFormat = null;
			if (signature is CAdESSignature)
			{
				signatureFormat = "PAdES";
			}
			else
			{
				throw new InvalidOperationException("Unsupported AdvancedSignature " + signature.
					GetType().FullName);
			}
			return signatureFormat;
		}

		/// <returns>the signature</returns>
		public virtual AdvancedSignature GetSignature()
		{
			return signature;
		}

		/// <summary>Get report for level BES</summary>
		/// <returns></returns>
		public virtual SignatureLevelBES GetLevelBES()
		{
			return levelBES;
		}

		/// <summary>Get report for level EPES</summary>
		/// <returns></returns>
		public virtual SignatureLevelEPES GetLevelEPES()
		{
			return levelEPES;
		}

		/// <summary>Get report for level T</summary>
		/// <returns></returns>
		public virtual SignatureLevelT GetLevelT()
		{
			return levelT;
		}

		/// <summary>Get report for level C</summary>
		/// <returns></returns>
		public virtual SignatureLevelC GetLevelC()
		{
			return levelC;
		}

		/// <summary>Get report for level X</summary>
		/// <returns></returns>
		public virtual SignatureLevelX GetLevelX()
		{
			return levelX;
		}

		/// <summary>Get report for level XL</summary>
		/// <returns></returns>
		public virtual SignatureLevelXL GetLevelXL()
		{
			return levelXL;
		}

		/// <summary>Get report for level A</summary>
		/// <returns></returns>
		public virtual SignatureLevelA GetLevelA()
		{
			return levelA;
		}

		/// <summary>Get report for level LTV</summary>
		/// <returns></returns>
		public virtual SignatureLevelLTV GetLevelLTV()
		{
			return levelLTV;
		}
	}
}
