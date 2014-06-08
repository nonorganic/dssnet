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

using EU.Europa.EC.Markt.Dss.Validation.Certificate;
using Sharpen;
using System;
using System.Collections.Generic;

namespace EU.Europa.EC.Markt.Dss.Validation.Tsl
{
    /// <summary>From a validation point of view, a Service is a set of pair ("Qualification Statement", "Condition").
    /// 	</summary>
    /// <remarks>From a validation point of view, a Service is a set of pair ("Qualification Statement", "Condition").
    /// 	</remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    [System.Serializable]
    public class ServiceInfo
    {
        private const long serialVersionUID = 4903410679096343832L;

        //jbonilla
        //private string type;
        public virtual string Type { get; set; }

        private IDictionary<string, Condition> qualifiersAndConditions = new Dictionary<string
            , Condition>();

        private string tspName;

        private string tspTradeName;

        private string tspPostalAddress;

        private string tspElectronicAddress;

        private string serviceName;

        private string currentStatus;

        private DateTime currentStatusStartingDate;

        private string statusAtReferenceTime;

        private DateTime statusStartingDateAtReferenceTime;

        private DateTime statusEndingDateAtReferenceTime;

        private bool tlWellSigned;

        /// <summary>Add a qualifier and the corresponding condition</summary>
        /// <param name="qualifier"></param>
        /// <param name="condition"></param>
        public virtual void AddQualifier(string qualifier, Condition condition)
        {
            qualifiersAndConditions.Put(qualifier, condition);
        }

        /// <returns>the qualifiersAndConditions</returns>
        public virtual IDictionary<string, Condition> GetQualifiersAndConditions()
        {
            return qualifiersAndConditions;
        }

        /// <summary>Retrieve all the qualifiers for which the corresponding condition evaluate to true.
        /// 	</summary>
        /// <remarks>Retrieve all the qualifiers for which the corresponding condition evaluate to true.
        /// 	</remarks>
        /// <param name="cert"></param>
        /// <returns></returns>
        public virtual IList<string> GetQualifiers(CertificateAndContext cert)
        {
            IList<string> list = new AList<string>();
            foreach (KeyValuePair<string, Condition> e in qualifiersAndConditions.EntrySet())
            {
                if (e.Value.Check(cert))
                {
                    list.AddItem(e.Key);
                }
            }
            return list;
        }

        //jbonilla
        ///// <summary>Return the type of the service</summary>
        ///// <returns></returns>
        //public virtual string GetType()
        //{
        //    return type;
        //}

        ///// <summary>Define the type of the service</summary>
        ///// <param name="type"></param>
        //public virtual void SetType(string type)
        //{
        //    this.type = type;
        //}

        /// <returns></returns>
        public virtual string GetTspName()
        {
            return tspName;
        }

        /// <param name="tspName"></param>
        public virtual void SetTspName(string tspName)
        {
            this.tspName = tspName;
        }

        /// <returns></returns>
        public virtual string GetTspTradeName()
        {
            return tspTradeName;
        }

        /// <param name="tspTradeName"></param>
        public virtual void SetTspTradeName(string tspTradeName)
        {
            this.tspTradeName = tspTradeName;
        }

        /// <returns></returns>
        public virtual string GetTspPostalAddress()
        {
            return tspPostalAddress;
        }

        /// <param name="tspPostalAddress"></param>
        public virtual void SetTspPostalAddress(string tspPostalAddress)
        {
            this.tspPostalAddress = tspPostalAddress;
        }

        /// <returns></returns>
        public virtual string GetTspElectronicAddress()
        {
            return tspElectronicAddress;
        }

        /// <param name="tspElectronicAddress"></param>
        public virtual void SetTspElectronicAddress(string tspElectronicAddress)
        {
            this.tspElectronicAddress = tspElectronicAddress;
        }

        /// <returns></returns>
        public virtual string GetServiceName()
        {
            return serviceName;
        }

        /// <param name="serviceName"></param>
        public virtual void SetServiceName(string serviceName)
        {
            this.serviceName = serviceName;
        }

        /// <returns></returns>
        public virtual string GetCurrentStatus()
        {
            return currentStatus;
        }

        /// <param name="currentStatus"></param>
        public virtual void SetCurrentStatus(string currentStatus)
        {
            this.currentStatus = currentStatus;
        }

        /// <returns></returns>
        public virtual DateTime GetCurrentStatusStartingDate()
        {
            return currentStatusStartingDate;
        }

        /// <param name="currentStatusStartingDate"></param>
        public virtual void SetCurrentStatusStartingDate(DateTime currentStatusStartingDate
            )
        {
            this.currentStatusStartingDate = currentStatusStartingDate;
        }

        /// <returns></returns>
        public virtual string GetStatusAtReferenceTime()
        {
            return statusAtReferenceTime;
        }

        /// <param name="statusAtReferenceTime"></param>
        public virtual void SetStatusAtReferenceTime(string statusAtReferenceTime)
        {
            this.statusAtReferenceTime = statusAtReferenceTime;
        }

        /// <returns></returns>
        public virtual DateTime GetStatusStartingDateAtReferenceTime()
        {
            return statusStartingDateAtReferenceTime;
        }

        /// <param name="statusStartingDateAtReferenceTime"></param>
        public virtual void SetStatusStartingDateAtReferenceTime(DateTime statusStartingDateAtReferenceTime
            )
        {
            this.statusStartingDateAtReferenceTime = statusStartingDateAtReferenceTime;
        }

        /// <returns></returns>
        public virtual DateTime GetStatusEndingDateAtReferenceTime()
        {
            return statusEndingDateAtReferenceTime;
        }

        /// <param name="statusEndingDateAtReferenceTime"></param>
        public virtual void SetStatusEndingDateAtReferenceTime(DateTime statusEndingDateAtReferenceTime
            )
        {
            this.statusEndingDateAtReferenceTime = statusEndingDateAtReferenceTime;
        }

        /// <returns>the tlWellSigned</returns>
        public virtual bool IsTlWellSigned()
        {
            return tlWellSigned;
        }

        /// <param name="tlWellSigned">the tlWellSigned to set</param>
        public virtual void SetTlWellSigned(bool tlWellSigned)
        {
            this.tlWellSigned = tlWellSigned;
        }
    }
}
