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
using System.IO;
using EU.Europa.EC.Markt.Dss.Signature.Xades;
using System.Security.Cryptography.Xml;
using System.Xml;
using Microsoft.Xades;

namespace EU.Europa.EC.Markt.Dss.Signature.Xades
{
    /// <summary>Utility class that contains some XML related method.</summary>
    /// <remarks>Utility class that contains some XML related method.</remarks>
    /// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
    /// 	</version>
    public static class XmlUtils
    {
        public static string GetNamespaceURI(string prefix)
        {
            if ("ds".Equals(prefix))
            {
                return SignedXml.XmlDsigNamespaceUrl;
            }
            else
            {
                if ("xades".Equals(prefix))
                {
                    return "http://uri.etsi.org/01903/v1.3.2#";
                }
                else
                {
                    if ("xades141".Equals(prefix))
                    {
                        return "http://uri.etsi.org/01903/v1.4.1#";
                    }
                    else
                    {
                        if ("xades111".Equals(prefix))
                        {
                            return "http://uri.etsi.org/01903/v1.1.1#";
                        }
                    }
                }
            }

            throw new ArgumentException("Prefix not recognized", prefix);
        }

        /// <summary>Return the Element corresponding the the XPath</summary>
        /// <param name="xmlElement"></param>
        /// <param name="xpathString"></param>
        /// <returns></returns>
        /// <exception cref="Javax.Xml.Xpath.XPathExpressionException">Javax.Xml.Xpath.XPathExpressionException
        /// 	</exception>
        public static XmlNodeList GetNodeList(XmlElement xmlElement, string xpathString)
        {
            NameTable table = new NameTable();
            XmlNamespaceManager manager = new XmlNamespaceManager(table);
            manager.AddNamespace("xades", GetNamespaceURI("xades"));
            manager.AddNamespace("ds", GetNamespaceURI("ds"));

            return xmlElement.SelectNodes(xpathString, manager);
        }


        public static Document ToDocument(XmlDocument originalDocument, XadesSignedXml xadesSignedXml)
        {
            XmlElement xmlElementToSave;

            originalDocument.DocumentElement.AppendChild(originalDocument.ImportNode(xadesSignedXml.GetXml(), true));
            xmlElementToSave = originalDocument.DocumentElement;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true; //Needed
            xmlDocument.LoadXml(xmlElementToSave.OuterXml);

            MemoryStream buf = new MemoryStream();
            xmlDocument.Save(buf);
            buf.Seek(0, SeekOrigin.Begin);

            return new InMemoryDocument(buf.ToArray());
        }

        public static XmlDocument ToXmlDocument(Document document)
        {
            XmlDocument xmlDocument;
            xmlDocument = new XmlDocument();
            xmlDocument.PreserveWhitespace = true;
            xmlDocument.Load(document.OpenStream());

            return xmlDocument;
        }
    }
}
