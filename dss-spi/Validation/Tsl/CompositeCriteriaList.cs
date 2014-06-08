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
using System;
using System.Collections.Generic;

namespace EU.Europa.EC.Markt.Dss.Validation.Tsl
{
    /// <summary>Condition resulting of the composition of other Condition</summary>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    [System.Serializable]
    public class CompositeCriteriaList : Condition
    {
        private const long serialVersionUID = 904590921979120791L;

        /// <summary>How the conditions are aggregated.</summary>
        /// <remarks>
        /// How the conditions are aggregated.
        /// <p>
        /// DISCLAIMER: Project owner DG-MARKT.
        /// </remarks>
        /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
        /// 	</version>
        /// <author><a href="mailto:dgmarkt.Project-DSS@arhs-developments.com">ARHS Developments</a>
        /// 	</author>
        public enum Composition
        {
            atLeastOne,
            all,
            none
        }

        private Condition[] conditions;

        private CompositeCriteriaList.Composition composition;

        /// <summary>The default constructor for CompositeCriteriaList.</summary>
        /// <remarks>The default constructor for CompositeCriteriaList.</remarks>
        public CompositeCriteriaList()
        {
        }

        /// <summary>The default constructor for CompositeCriteriaList.</summary>
        /// <remarks>The default constructor for CompositeCriteriaList.</remarks>
        /// <param name="conditions"></param>
        public CompositeCriteriaList(CompositeCriteriaList.Composition composition, params 
			Condition[] conditions)
        {
            this.composition = composition;
            this.conditions = conditions;
        }

        /// <returns>the composition</returns>
        public virtual CompositeCriteriaList.Composition GetComposition()
        {
            return composition;
        }

        /// <returns>the conditions</returns>
        public virtual Condition[] GetConditions()
        {
            return conditions;
        }

        /// <summary>The default constructor for CompositeCriteriaList.</summary>
        /// <remarks>The default constructor for CompositeCriteriaList.</remarks>
        /// <param name="composition"></param>
        /// <param name="conditions"></param>
        public CompositeCriteriaList(CompositeCriteriaList.Composition composition, IList
            <Condition> conditions)
            : this(composition, Sharpen.Collections.ToArray(conditions
                , new Condition[conditions.Count]))
        {
        }

        public virtual bool Check(CertificateAndContext cert)
        {
            switch (composition)
            {
                case CompositeCriteriaList.Composition.all:
                    {
                        foreach (Condition c in conditions)
                        {
                            if (!c.Check(cert))
                            {
                                return false;
                            }
                        }
                        return true;
                    }

                case CompositeCriteriaList.Composition.atLeastOne:
                    {
                        foreach (Condition c_1 in conditions)
                        {
                            if (c_1.Check(cert))
                            {
                                return true;
                            }
                        }
                        return false;
                    }

                case CompositeCriteriaList.Composition.none:
                    {
                        foreach (Condition c_2 in conditions)
                        {
                            if (c_2.Check(cert))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
            }
            throw new InvalidOperationException("Unsupported Composition " + composition);
        }
    }
}
