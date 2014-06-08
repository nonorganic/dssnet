// ObjectIdentifier.cs
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

namespace Microsoft.Xades
{
	/// <summary>
	/// ObjectIdentifier allows the specification of an unique and permanent
	/// object of an object and some additional information about the nature of
	/// the	data object
	/// </summary>
	public class ObjectIdentifier
	{
		#region Private variables
		private string tagName;
		private Identifier identifier;
		private string description;
		private DocumentationReferences documentationReferences;
		#endregion

		#region Public properties
		/// <summary>
		/// The name of the element when serializing
		/// </summary>
		public string TagName
		{
			get
			{
				return this.tagName;
			}
			set
			{
				this.tagName = value;
			}
		}

		/// <summary>
		/// Specification of an unique and permanent identifier
		/// </summary>
		public Identifier Identifier
		{
			get
			{
				return this.identifier;
			}
			set
			{
				this.identifier = value;
			}
		}

		/// <summary>
		/// Textual description of the nature of the data object
		/// </summary>
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		/// <summary>
		/// References to documents where additional information about the
		/// nature of the data object can be found
		/// </summary>
		public DocumentationReferences DocumentationReferences
		{
			get
			{
				return this.documentationReferences;
			}
			set
			{
				this.documentationReferences = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public ObjectIdentifier()
		{
			this.identifier = new Identifier();
			this.documentationReferences = new DocumentationReferences();
		}

		/// <summary>
		/// Constructor with TagName
		/// </summary>
		/// <param name="tagName">Name of the tag when serializing with GetXml</param>
		public ObjectIdentifier(string tagName) : this()
		{
			this.tagName = tagName;
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

			if (this.identifier != null && this.identifier.HasChanged())
			{
				retVal = true;
			}

			if (!String.IsNullOrEmpty(this.description))
			{
				retVal = true;
			}

			if (this.documentationReferences != null && this.documentationReferences.HasChanged())
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
			XmlNamespaceManager xmlNamespaceManager;
			XmlNodeList xmlNodeList;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			xmlNodeList = xmlElement.SelectNodes("xsd:Identifier", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("Identifier missing");
			}
			this.identifier = new Identifier();
			this.identifier.LoadXml((XmlElement)xmlNodeList.Item(0));

			xmlNodeList = xmlElement.SelectNodes("xsd:Description", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.description = xmlNodeList.Item(0).InnerText;
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:DocumentationReferences", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.documentationReferences = new DocumentationReferences();
				this.documentationReferences.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			retVal = creationXmlDocument.CreateElement(this.tagName, XadesSignedXml.XadesNamespaceUri);

			if (this.identifier != null && this.identifier.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.identifier.GetXml(), true));
			}
			else
			{
				throw new CryptographicException("Identifier element missing in OjectIdentifier");
			}

			if (!String.IsNullOrEmpty(this.description))
			{
				bufferXmlElement = creationXmlDocument.CreateElement("Description", XadesSignedXml.XadesNamespaceUri);
				bufferXmlElement.InnerText = this.description;
				retVal.AppendChild(bufferXmlElement);
			}

			if (this.documentationReferences != null && this.documentationReferences.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.documentationReferences.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}
