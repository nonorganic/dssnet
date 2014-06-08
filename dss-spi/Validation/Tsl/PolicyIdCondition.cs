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
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Sharpen;
using System.IO;

namespace EU.Europa.EC.Markt.Dss.Validation.Tsl
{
    /// <summary>Check if a certificate has a specific policy id</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    [System.Serializable]
    public class PolicyIdCondition : Condition
    {
        private const long serialVersionUID = 7590885101177874819L;

        private string policyOid;

        /// <summary>The default constructor for PolicyIdCondition.</summary>
        /// <remarks>The default constructor for PolicyIdCondition.</remarks>
        public PolicyIdCondition()
        {
        }

        /// <summary>The default constructor for PolicyIdCondition.</summary>
        /// <remarks>The default constructor for PolicyIdCondition.</remarks>
        /// <param name="policyId"></param>
        public PolicyIdCondition(string policyId)
        {
            this.policyOid = policyId;
        }

        /// <returns>the policyOid</returns>
        public virtual string GetPolicyOid()
        {
            return policyOid;
        }

        public virtual bool Check(CertificateAndContext cert)
        {
            //TODO jbonilla - validar.
            //byte[] certificatePolicies = cert.GetCertificate().GetExtensionValue(X509Extensions.CertificatePolicies);
            Asn1OctetString certificatePolicies = cert.GetCertificate().GetExtensionValue(X509Extensions.CertificatePolicies);
            if (certificatePolicies != null)
            {
                try
                {
                    //Asn1InputStream input = new Asn1InputStream(certificatePolicies);
                    //DerOctetString s = (DerOctetString)input.ReadObject();
                    DerOctetString s = (DerOctetString)certificatePolicies;
                    byte[] content = s.GetOctets();
                    Asn1InputStream input = new Asn1InputStream(content);
                    DerSequence seq = (DerSequence)input.ReadObject();
                    for (int i = 0; i < seq.Count; i++)
                    {
                        PolicyInformation policyInfo = PolicyInformation.GetInstance(seq[i]);
                        if (policyInfo.PolicyIdentifier.Id.Equals(policyOid))
                        {
                            return true;
                        }
                    }
                }
                catch (IOException e)
                {
                    throw new RuntimeException(e);
                }
            }
            return false;
        }
    }
}
