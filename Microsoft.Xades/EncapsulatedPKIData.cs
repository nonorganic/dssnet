// EncapsulatedPKIData.cs
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
	/// EncapsulatedPKIData is used to incorporate a piece of PKI data
	/// into an XML structure whereas the PKI data is encoded using an ASN.1
	/// encoding mechanism. Examples of such PKI data that are widely used at
	/// the time include X509 certificates and revocation lists, OCSP responses,
	/// attribute certificates and time-stamps.
	/// </summary>
	public class EncapsulatedPKIData
	{
		#region Private variables
		private string tagName;
		private string id;
		private byte[] pkiData;
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
		/// The optional ID attribute can be used to make a reference to an element
		/// of this data type.
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
		/// Base64 encoded content of this data type 
		/// </summary>
		public byte[] PkiData
		{
			get
			{
				return this.pkiData;
			}
			set
			{
				this.pkiData = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public EncapsulatedPKIData()
		{
		}

		/// <summary>
		/// Constructor with TagName
		/// </summary>
		/// <param name="tagName">Name of the tag when serializing with GetXml</param>
		public EncapsulatedPKIData(string tagName)
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

            if (!String.IsNullOrEmpty(this.id))
			{
				retVal = true;
			}

			if (this.pkiData != null && this.pkiData.Length > 0)
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

			this.pkiData = Convert.FromBase64String(xmlElement.InnerText);
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
			retVal = creationXmlDocument.CreateElement(this.tagName, XadesSignedXml.XadesNamespaceUri);

            if (!String.IsNullOrEmpty(this.id))
			{
				retVal.SetAttribute("Id", this.id);
			}

			if (this.pkiData != null && this.pkiData.Length > 0)
			{
				retVal.InnerText = Convert.ToBase64String(this.pkiData);
			}

			return retVal;
		}
		#endregion
	}
}
