// XadesObject.cs
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
	/// This class represents the unique object of a XAdES signature that
	/// contains all XAdES information
	/// </summary>
	public class XadesObject
	{
		#region Private variable
		private string id;
		private QualifyingProperties qualifyingProperties;
		#endregion

		#region Public properties
		/// <summary>
		/// Id attribute of the XAdES object
		/// </summary>
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		/// <summary>
		/// The QualifyingProperties element acts as a container element for
		/// all the qualifying information that should be added to an XML
		/// signature.
		/// </summary>
		public QualifyingProperties QualifyingProperties
		{
			get
			{
				return this.qualifyingProperties;
			}
			set
			{
				this.qualifyingProperties = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public XadesObject()
		{
			this.qualifyingProperties = new QualifyingProperties();
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

			if (this.id != null && this.id != "")
			{
				retVal = true;
			}

			if (this.qualifyingProperties != null && this.qualifyingProperties.HasChanged())
			{
				retVal = true;
			}

			return retVal;
		}

		/// <summary>
		/// Load state from an XML element
		/// </summary>
		/// <param name="xmlElement">XML element containing new state</param>
		/// <param name="counterSignedXmlElement">Element containing parent signature (needed if there are counter signatures)</param>
		public void LoadXml(XmlElement xmlElement, XmlElement counterSignedXmlElement)
		{
			XmlNamespaceManager xmlNamespaceManager;
			XmlNodeList xmlNodeList;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}
			if (xmlElement.HasAttribute("Id"))
			{
				this.id = xmlElement.GetAttribute("Id");
			}
			else
			{
				this.id = "";
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
            xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			xmlNodeList = xmlElement.SelectNodes("xsd:QualifyingProperties", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("QualifyingProperties missing");
			}
			this.qualifyingProperties = new QualifyingProperties();
            this.qualifyingProperties.LoadXml((XmlElement)xmlNodeList.Item(0), counterSignedXmlElement);

			xmlNodeList = xmlElement.SelectNodes("xsd:QualifyingPropertiesReference", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				throw new CryptographicException("Current implementation can't handle QualifyingPropertiesReference element");
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

			creationXmlDocument = new XmlDocument();
			retVal = creationXmlDocument.CreateElement("ds", "Object", SignedXml.XmlDsigNamespaceUrl);
			if (this.id != null && this.id != "")
			{
				retVal.SetAttribute("Id", this.id);
			}

			if (this.qualifyingProperties != null && this.qualifyingProperties.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.qualifyingProperties.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}
