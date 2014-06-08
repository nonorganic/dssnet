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
using Org.BouncyCastle.Asn1.X509.Qualified;
using Sharpen;
using System.IO;

namespace EU.Europa.EC.Markt.Dss.Validation.Tsl
{
    /// <summary>Condition that check a specific QCStatement</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    [System.Serializable]
    public class QcStatementCondition : Condition
    {
        private const long serialVersionUID = -5504958938057542907L;

        private string qcStatementId = null;

        /// <summary>Mandatory for serializable</summary>
        public QcStatementCondition()
        {
        }

        /// <summary>The default constructor for QcStatementCondition.</summary>
        /// <remarks>The default constructor for QcStatementCondition.</remarks>
        /// <param name="qcStatementId"></param>
        public QcStatementCondition(string qcStatementId)
        {
            this.qcStatementId = qcStatementId;
        }

        /// <summary>The default constructor for QcStatementCondition.</summary>
        /// <remarks>The default constructor for QcStatementCondition.</remarks>
        /// <param name="qcStatementId"></param>
        public QcStatementCondition(DerObjectIdentifier qcStatementId)
            : this(qcStatementId.Id)
        {
        }

        public virtual bool Check(CertificateAndContext cert)
        {
            //TODO jbonilla - Validar
            //byte[] qcStatement = cert.GetCertificate().GetExtensionValue(X509Extensions.QCStatements);
            Asn1OctetString qcStatement = cert.GetCertificate().GetExtensionValue(X509Extensions.QCStatements);
            if (qcStatement != null)
            {
                try
                {
                    //Asn1InputStream input = new Asn1InputStream(qcStatement);                    
                    //DerOctetString s = (DerOctetString)input.ReadObject();
                    DerOctetString s = (DerOctetString)qcStatement;
                    byte[] content = s.GetOctets();
                    Asn1InputStream input = new Asn1InputStream(content);
                    DerSequence seq = (DerSequence)input.ReadObject();
                    for (int i = 0; i < seq.Count; i++)
                    {
                        QCStatement statement = QCStatement.GetInstance(seq[i]);
                        if (statement.StatementId.Id.Equals(qcStatementId))
                        {
                            return true;
                        }
                    }
                    return false;
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
