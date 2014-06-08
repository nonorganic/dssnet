// Transform.cs
//
// XAdES Starter Kit for Microsoft .NET 3.5 (and above)
// 2010 Microsoft France
// Published under the CECILL-B Free Software license agreement.
// (http://www.cecill.info/licences/Licence_CeCILL-B_V1-en.txt)
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// WHETHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE. 
// THE ENTIRE RISK OF USE OR RESULTS IN CONNECTION WITH THE USE OF THIS CODE 
// AND INFORMATION REMAINS WITH THE USER. 
//

using System;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;

namespace Microsoft.Xades
{
	/// <summary>
	/// The Transform element contains a single transformation
	/// </summary>
	public class Transform
	{
		#region Private variables
		private string algorithm;
		private string xpath;
		#endregion

		#region Public properties
		/// <summary>
		/// Algorithm of the transformation
		/// </summary>
		public string Algorithm
		{
			get
			{
				return this.algorithm;
			}
			set
			{
				this.algorithm = value;
			}
		}

		/// <summary>
		/// XPath of the transformation
		/// </summary>
		public string XPath
		{
			get
			{
				return this.xpath;
			}
			set
			{
				this.xpath = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Transform()
		{
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Check to see if something has changed in this instance and needs to be serialized
		/// </summary>
		/// <returns>Flag indicating if a member needs serialization</returns>
		public bool HasChanged()
		{
			bool retVal = false;

            if (!String.IsNullOrEmpty(this.algorithm))
			{
				retVal = true;
			}

			if (!String.IsNullOrEmpty(this.xpath))
			{
				retVal = true;
			}

			return retVal;
		}

		/// <summary>
		/// Load state from an XML element
		/// </summary>
		/// <param name="xmlElement">XML element containing new state</param>
		public void LoadXml(System.Xml.XmlElement xmlElement)
		{
			XmlNodeList xmlNodeList;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}
			if (xmlElement.HasAttribute("Algorithm"))
			{
				this.algorithm = xmlElement.GetAttribute("Algorithm");
			}
			else
			{
				this.algorithm = "";
			}

			xmlNodeList = xmlElement.SelectNodes("XPath");
			if (xmlNodeList.Count != 0)
			{
				this.xpath = xmlNodeList.Item(0).InnerText;
			}
			else
			{
				this.xpath = "";
			}
		}

		/// <summary>
		/// Returns the XML representation of the this object
		/// </summary>
		/// <returns>XML element containing the state of this object</returns>
		public XmlElement GetXml()
		{
			XmlDocument creationXmlDocument;
			XmlElement retVal;
			XmlElement bufferXmlElement;

			creationXmlDocument = new XmlDocument();
			retVal = creationXmlDocument.CreateElement("ds", "Transform", SignedXml.XmlDsigNamespaceUrl);

			if (this.algorithm != null)
			{
				retVal.SetAttribute("Algorithm", this.algorithm);
			}
			else
			{
				retVal.SetAttribute("Algorithm", "");
			}

			if (this.xpath != null && this.xpath != "")
			{
				bufferXmlElement = creationXmlDocument.CreateElement("ds", "XPath", SignedXml.XmlDsigNamespaceUrl);
				bufferXmlElement.InnerText = this.xpath;
				retVal.AppendChild(bufferXmlElement);
			}

			return retVal;
		}
		#endregion
	}
}
