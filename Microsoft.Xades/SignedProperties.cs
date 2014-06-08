// SignedProperties.cs
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
	/// The SignedProperties element contains a number of properties that are
	/// collectively signed by the XMLDSIG signature
	/// </summary>
	public class SignedProperties
	{
		#region Constants
		/// <summary>
		/// Default value for the SignedProperties Id attribute
		/// </summary>
		public const string DefaultSignedPropertiesId = "SignedPropertiesId";
		#endregion

		#region Private variables
		private string id;
		private SignedSignatureProperties signedSignatureProperties;
		private SignedDataObjectProperties signedDataObjectProperties;
		#endregion

		#region Public properties

		/// <summary>
		/// This Id is used to be able to point the signature reference to this
		/// element.  It is initialized by default.
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
		/// The properties that qualify the signature itself or the signer are
		/// included as content of the SignedSignatureProperties element
		/// </summary>
		public SignedSignatureProperties SignedSignatureProperties
		{
			get
			{
				return this.signedSignatureProperties;
			}
			set
			{
				this.signedSignatureProperties = value;
			}
		}

		/// <summary>
		/// The SignedDataObjectProperties element contains properties that qualify
		/// some of the signed data objects
		/// </summary>
		public SignedDataObjectProperties SignedDataObjectProperties
		{
			get
			{
				return this.signedDataObjectProperties;
			}
			set
			{
				this.signedDataObjectProperties = value;
			}
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SignedProperties()
		{
			this.id = DefaultSignedPropertiesId; //This is where signature reference points to
			this.signedSignatureProperties = new SignedSignatureProperties();
			this.signedDataObjectProperties = new SignedDataObjectProperties();
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

			if (this.signedSignatureProperties != null && this.signedSignatureProperties.HasChanged())
			{
				retVal = true;
			}

			if (this.signedDataObjectProperties != null && this.signedDataObjectProperties.HasChanged())
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

			xmlNodeList = xmlElement.SelectNodes("xsd:SignedSignatureProperties", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("SignedSignatureProperties missing");
			}
			this.signedSignatureProperties = new SignedSignatureProperties();
			this.signedSignatureProperties.LoadXml((XmlElement)xmlNodeList.Item(0));

			xmlNodeList = xmlElement.SelectNodes("xsd:SignedDataObjectProperties", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.signedDataObjectProperties = new SignedDataObjectProperties();
				this.signedDataObjectProperties.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			retVal = creationXmlDocument.CreateElement("SignedProperties", XadesSignedXml.XadesNamespaceUri);
            if (!String.IsNullOrEmpty(this.id))
			{
				retVal.SetAttribute("Id", this.id);
			}

			if (this.signedSignatureProperties != null)
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.signedSignatureProperties.GetXml(), true));
			}
			else
			{
				throw new CryptographicException("SignedSignatureProperties should not be null");
			}

			if (this.signedDataObjectProperties != null && this.signedDataObjectProperties.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.signedDataObjectProperties.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}
