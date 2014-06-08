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
	/// <summary>Representation of the Result in the validation report.</summary>
	/// <remarks>Representation of the Result in the validation report.</remarks>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class Result
	{
		/// <summary>Supported values</summary>
		public enum ResultStatus
		{
			VALID,
			INVALID,
			UNDETERMINED,
			VALID_WITH_WARNINGS,
			INFORMATION
		}

		private Result.ResultStatus status;

		protected internal string description;

		/// <summary>The default constructor for Result.</summary>
		/// <remarks>The default constructor for Result.</remarks>
		/// <param name="name"></param>
		public Result(Result.ResultStatus status, string description)
		{
			// or PASS
			// or FAIL
			// or UNKNOWN
			this.status = status;
			this.description = description;
		}

		/// <summary>The default constructor for Result.</summary>
		/// <remarks>The default constructor for Result.</remarks>
		public Result() : this(Result.ResultStatus.UNDETERMINED, null)
		{
		}

		/// <summary>One-liner to create a Result by asserting something</summary>
		/// <param name="assertion"></param>
		/// <param name="statusIfFailed">the status to set if the test fails</param>
		private Result(bool assertion, Result.ResultStatus statusIfFailed) : this()
		{
			if (assertion)
			{
				this.SetStatus(Result.ResultStatus.VALID, null);
			}
			else
			{
				this.SetStatus(statusIfFailed, null);
			}
		}

		/// <summary>One-liner to create a Result by asserting something, set to invalid if the assertion fails
		/// 	</summary>
		/// <param name="assertion"></param>
		public Result(bool assertion) : this(assertion, Result.ResultStatus.INVALID)
		{
		}

		public override string ToString()
		{
			return "Result[" + status + "]";
		}

		/// <summary>returns whether the check was valid</summary>
		/// <returns>true if valid</returns>
		public virtual bool IsValid()
		{
			return (GetStatus() == Result.ResultStatus.VALID);
		}

		/// <summary>returns whether the check was invalid</summary>
		/// <returns>true if valid</returns>
		public virtual bool IsInvalid()
		{
			return (GetStatus() == Result.ResultStatus.INVALID);
		}

		/// <summary>returns whether the check was undetermined</summary>
		/// <returns>true if undetermined</returns>
		public virtual bool IsUndetermined()
		{
			return (GetStatus() == Result.ResultStatus.UNDETERMINED);
		}

		/// <param name="status"></param>
		public virtual void SetStatus(Result.ResultStatus status, string description)
		{
			this.status = status;
			this.description = description;
		}

		/// <summary>Set description of the result</summary>
		/// <param name="description"></param>
		public virtual void SetDescription(string description)
		{
			this.description = description;
		}

		/// <returns>the result</returns>
		public virtual Result.ResultStatus GetStatus()
		{
			return status;
		}

		/// <returns>the description</returns>
		public virtual string GetDescription()
		{
			return description;
		}
	}
}
