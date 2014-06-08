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

namespace EU.Europa.EC.Markt.Dss.Validation.Tsl
{
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public class QualificationElement
    {
        private string qualification;

        private Condition condition;

        /// <summary>The default constructor for QualificationElement.</summary>
        /// <remarks>The default constructor for QualificationElement.</remarks>
        public QualificationElement(string qualification, Condition condition)
        {
            this.qualification = qualification;
            this.condition = condition;
        }

        /// <returns>the qualification</returns>
        public virtual string GetQualification()
        {
            return qualification;
        }

        /// <param name="qualification">the qualification to set</param>
        public virtual void SetQualification(string qualification)
        {
            this.qualification = qualification;
        }

        /// <returns>the condition</returns>
        public virtual Condition GetCondition()
        {
            return condition;
        }

        /// <param name="condition">the condition to set</param>
        public virtual void SetCondition(Condition condition)
        {
            this.condition = condition;
        }
    }
}
