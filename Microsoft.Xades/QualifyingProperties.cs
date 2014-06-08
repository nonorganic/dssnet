// QualifiyngProperties.cs
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
	/// The QualifyingProperties element acts as a container element for
	/// all the qualifying information that should be added to an XML
	/// signature
	/// </summary>
	public class QualifyingProperties
	{
		#region Private variables
		private string id;
		private string target;
		private SignedProperties signedProperties;
		private UnsignedProperties unsignedProperties;
		#endregion

		#region Public properties
		/// <summary>
		/// The optional Id attribute can be used to make a reference to the
		/// QualifyingProperties container.
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
		/// The mandatory Target attribute refers to the XML signature with which the
		/// qualifying properties are associated.
		/// </summary>
		public string Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
			}
		}

		/// <summary>
		/// The SignedProperties element contains a number of properties that are
		/// collectively signed by the XMLDSIG signature
		/// </summary>
		public SignedProperties SignedProperties
		{
			get
			{
				return this.signedProperties;
			}
			set
			{
				this.signedProperties = value;
			}
		}

		/// <summary>
		/// The UnsignedProperties element contains a number of properties that are
		/// not signed by the XMLDSIG signature
		/// </summary>
		public UnsignedProperties UnsignedProperties
		{
			get
			{
				return this.unsignedProperties;
			}
			set
			{
				this.unsignedProperties = value;
			}
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public QualifyingProperties()
		{
			this.signedProperties = new SignedProperties();
			this.unsignedProperties = new UnsignedProperties();
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

            if (!String.IsNullOrEmpty(this.id))
			{
				retVal = true;
			}

            if (!String.IsNullOrEmpty(this.target))
			{
				retVal = true;
			}

			if (this.signedProperties != null && this.signedProperties.HasChanged())
			{
				retVal = true;
			}

			if (this.unsignedProperties != null && this.unsignedProperties.HasChanged())
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

			if (xmlElement.HasAttribute("Target"))
			{
				this.target = xmlElement.GetAttribute("Target");
			}
			else
			{
				this.target = "";
				throw new CryptographicException("Target attribute missing");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			xmlNodeList = xmlElement.SelectNodes("xsd:SignedProperties", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("SignedProperties missing");
			}
			this.signedProperties = new SignedProperties();
			this.signedProperties.LoadXml((XmlElement)xmlNodeList.Item(0));

			xmlNodeList = xmlElement.SelectNodes("xsd:UnsignedProperties", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.unsignedProperties = new UnsignedProperties();
				this.unsignedProperties.LoadXml((XmlElement)xmlNodeList.Item(0), counterSignedXmlElement);
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
			retVal = creationXmlDocument.CreateElement("xsd","QualifyingProperties", XadesSignedXml.XadesNamespaceUri);
            if (!String.IsNullOrEmpty(this.id))
			{
				retVal.SetAttribute("Id", this.id);
			}

            if (!String.IsNullOrEmpty(this.target))
			{
				retVal.SetAttribute("Target", this.target);
			}
			else
			{
				throw new CryptographicException("QualifyingProperties Target attribute has no value");
			}

			if (this.signedProperties != null && this.signedProperties.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.signedProperties.GetXml(), true));
			}
			if (this.unsignedProperties != null && this.unsignedProperties.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.unsignedProperties.GetXml(), true));
			}           

			return retVal;
		}        

        private void SetPrefix(String prefix, XmlNode node)
        {
            if (node.NamespaceURI == XadesSignedXml.XadesNamespaceUri)
            {
                node.Prefix = prefix;
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                SetPrefix(prefix, child);
            }

            return;
        }
		#endregion
	}
}
