// SignatureProductionPlace.cs
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
	/// In some transactions the purported place where the signer was at the time
	/// of signature creation may need to be indicated. In order to provide this
	/// information a new property may be included in the signature.
	/// This property specifies an address associated with the signer at a
	/// particular geographical (e.g. city) location.
	/// This is a signed property that qualifies the signer.
	/// An XML electronic signature aligned with the present document MAY contain
	/// at most one SignatureProductionPlace element.
	/// </summary>
	public class SignatureProductionPlace
	{
		#region Private variables
		private string city;
		private string stateOrProvince;
		private string postalCode;
		private string countryName;
		#endregion

		#region Public properties
		/// <summary>
		/// City where signature was produced
		/// </summary>
		public string City
		{
			get
			{
				return this.city;
			}
			set
			{
				this.city = value;
			}
		}

		/// <summary>
		/// State or province where signature was produced
		/// </summary>
		public string StateOrProvince
		{
			get
			{
				return this.stateOrProvince;
			}
			set
			{
				this.stateOrProvince = value;
			}
		}

		/// <summary>
		/// Postal code of place where signature was produced
		/// </summary>
		public string PostalCode
		{
			get
			{
				return this.postalCode;
			}
			set
			{
				this.postalCode = value;
			}
		}

		/// <summary>
		/// Country where signature was produced
		/// </summary>
		public string CountryName
		{
			get
			{
				return this.countryName;
			}
			set
			{
				this.countryName = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SignatureProductionPlace()
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

            if (!String.IsNullOrEmpty(this.city))
			{
				retVal = true;
			}

			if (!String.IsNullOrEmpty(this.stateOrProvince))
			{
				retVal = true;
			}

			if (!String.IsNullOrEmpty(this.postalCode))
			{
				retVal = true;
			}

			if (!String.IsNullOrEmpty(this.countryName))
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

			xmlNodeList = xmlElement.SelectNodes("xsd:City", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.city = xmlNodeList.Item(0).InnerText;
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:PostalCode", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.postalCode = xmlNodeList.Item(0).InnerText;
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:StateOrProvince", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.stateOrProvince = xmlNodeList.Item(0).InnerText;
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:CountryName", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.countryName = xmlNodeList.Item(0).InnerText;
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
			retVal = creationXmlDocument.CreateElement("SignatureProductionPlace", XadesSignedXml.XadesNamespaceUri);

			if (!String.IsNullOrEmpty(this.city))
			{
				bufferXmlElement = creationXmlDocument.CreateElement("City", XadesSignedXml.XadesNamespaceUri);
				bufferXmlElement.InnerText = this.city;
				retVal.AppendChild(bufferXmlElement);
			}

			if (!String.IsNullOrEmpty(this.stateOrProvince))
			{
				bufferXmlElement = creationXmlDocument.CreateElement("StateOrProvince", XadesSignedXml.XadesNamespaceUri);
				bufferXmlElement.InnerText = this.stateOrProvince;
				retVal.AppendChild(bufferXmlElement);
			}

			if (!String.IsNullOrEmpty(this.postalCode))
			{
				bufferXmlElement = creationXmlDocument.CreateElement("PostalCode", XadesSignedXml.XadesNamespaceUri);
				bufferXmlElement.InnerText = this.postalCode;
				retVal.AppendChild(bufferXmlElement);
			}

			if (this.countryName != null && this.countryName != "")
			{
				bufferXmlElement = creationXmlDocument.CreateElement("CountryName", XadesSignedXml.XadesNamespaceUri);
				bufferXmlElement.InnerText = this.countryName;
				retVal.AppendChild(bufferXmlElement);
			}

			return retVal;
		}
		#endregion
	}
}
