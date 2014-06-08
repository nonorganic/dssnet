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

using EU.Europa.EC.Markt.Dss.Signature;
using Sharpen;

namespace EU.Europa.EC.Markt.Dss.Validation
{
	/// <summary>Represent the value of a SignaturePolicy</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class PolicyValue
	{
		private string signaturePolicyId;

		private SignaturePolicy policy;

		/// <summary>The default constructor for PolicyValue.</summary>
		/// <remarks>The default constructor for PolicyValue.</remarks>
		/// <param name="signaturePolicyId"></param>
		public PolicyValue(string signaturePolicyId)
		{
			this.signaturePolicyId = signaturePolicyId;
			this.policy = SignaturePolicy.EXPLICIT;
		}

		/// <summary>The default constructor for PolicyValue.</summary>
		/// <remarks>The default constructor for PolicyValue.</remarks>
		public PolicyValue()
		{
			this.policy = SignaturePolicy.IMPLICIT;
		}

		/// <returns>the signaturePolicyId</returns>
		public virtual string GetSignaturePolicyId()
		{
			return signaturePolicyId;
		}

		/// <returns>the policy</returns>
		public virtual SignaturePolicy GetPolicy()
		{
			return policy;
		}

		public override string ToString()
		{
			switch (policy)
			{
				case SignaturePolicy.EXPLICIT:
				{
					return signaturePolicyId;
				}

				default:
				{
					return policy.ToString();
					break;
				}
			}
		}
	}
}
