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
using System.Collections.Generic;
using EU.Europa.EC.Markt.Dss.Validation.Tsl;
using Sharpen;
using Org.BouncyCastle.Utilities.Date;

namespace EU.Europa.EC.Markt.Dss.Validation.Report
{
	/// <summary>Contains trusted list information relevant to a certificate</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class TrustedListInformation
	{
		private ServiceInfo trustService;

		/// <summary>The default constructor for TrustedListInformation.</summary>
		/// <remarks>The default constructor for TrustedListInformation.</remarks>
		/// <param name="ts"></param>
		public TrustedListInformation(ServiceInfo ts)
		{
			this.trustService = ts;
		}

		/// <returns>the serviceWasFound</returns>
		public virtual bool IsServiceWasFound()
		{
			return trustService != null;
		}

		/// <returns></returns>
		public virtual string GetTSPName()
		{
			if (trustService == null)
			{
				return null;
			}
			return trustService.GetTspName();
		}

		/// <returns></returns>
		public virtual string GetTSPTradeName()
		{
			if (trustService == null)
			{
				return null;
			}
			return trustService.GetTspTradeName();
		}

		/// <returns></returns>
		public virtual string GetTSPPostalAddress()
		{
			if (trustService == null)
			{
				return null;
			}
			return trustService.GetTspPostalAddress();
		}

		/// <returns></returns>
		public virtual string GetTSPElectronicAddress()
		{
			if (trustService == null)
			{
				return null;
			}
			return trustService.GetTspElectronicAddress();
		}

		/// <returns></returns>
		public virtual string GetServiceType()
		{
			if (trustService == null)
			{
				return null;
			}
			return trustService.Type;
		}

		/// <returns></returns>
		public virtual string GetServiceName()
		{
			if (trustService == null)
			{
				return null;
			}
			return trustService.GetServiceName();
		}

		/// <returns></returns>
		public virtual string GetCurrentStatus()
		{
			if (trustService == null)
			{
				return null;
			}
			string status = trustService.GetCurrentStatus();
			int slashIndex = status.LastIndexOf('/');
			if (slashIndex > 0 && slashIndex < status.Length - 1)
			{
				status = Sharpen.Runtime.Substring(status, slashIndex + 1);
			}
			return status;
		}

		/// <returns></returns>
		public virtual DateTimeObject GetCurrentStatusStartingDate()
		{
			if (trustService == null)
			{
				return null;
			}
			return new DateTimeObject(
                trustService.GetCurrentStatusStartingDate());
		}

		/// <returns></returns>
		public virtual string GetStatusAtReferenceTime()
		{
			if (trustService == null)
			{
				return null;
			}
			string status = trustService.GetStatusAtReferenceTime();
			int slashIndex = status.LastIndexOf('/');
			if (slashIndex > 0 && slashIndex < status.Length - 1)
			{
				status = Sharpen.Runtime.Substring(status, slashIndex + 1);
			}
			return status;
		}

		public virtual DateTimeObject GetStatusStartingDateAtReferenceTime()
		{
			if (trustService == null)
			{
				return null;
			}
			return new DateTimeObject(
                trustService.GetStatusStartingDateAtReferenceTime());
		}

		/// <summary>Is the Trusted List well signed</summary>
		/// <returns></returns>
		public virtual bool IsWellSigned()
		{
			if (trustService == null)
			{
				return false;
			}
			return trustService.IsTlWellSigned();
		}

		/// <summary>Return the list of condition associated to this service</summary>
		/// <returns></returns>
		public virtual IList<QualificationElement> GetQualitificationElements()
		{
			if (trustService == null)
			{
				return null;
			}
			IList<QualificationElement> elements = new AList<QualificationElement>();
			foreach (KeyValuePair<string, Condition> e in trustService.GetQualifiersAndConditions
				().EntrySet())
			{
				elements.AddItem(new QualificationElement(e.Key, e.Value));
			}
			return elements;
		}
	}
}
