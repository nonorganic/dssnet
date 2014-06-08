// IssuerSerial.cs
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
	/// The element IssuerSerial contains the identifier of one of the
	/// certificates referenced in the sequence
	/// </summary>
	public class IssuerSerial
	{
		#region Private variables
		private string x509IssuerName;
		private string x509SerialNumber;
		#endregion

		#region Public properties
		/// <summary>
		/// Name of the X509 certificate issuer
		/// </summary>
		public string X509IssuerName
		{
			get
			{
				return this.x509IssuerName;
			}
			set
			{
				this.x509IssuerName = value;
			}
		}

		/// <summary>
		/// Serial number of the X509 certificate
		/// </summary>
		public string X509SerialNumber
		{
			get
			{
				return this.x509SerialNumber;
			}
			set
			{
				this.x509SerialNumber = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public IssuerSerial()
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

			if (!String.IsNullOrEmpty(this.x509IssuerName))
			{
				retVal = true;
			}

			if (!String.IsNullOrEmpty(this.x509SerialNumber))
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
			xmlNamespaceManager.AddNamespace("ds", SignedXml.XmlDsigNamespaceUrl);

			xmlNodeList = xmlElement.SelectNodes("ds:X509IssuerName", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("X509IssuerName missing");
			}
			this.x509IssuerName = xmlNodeList.Item(0).InnerText;

			xmlNodeList = xmlElement.SelectNodes("ds:X509SerialNumber", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("X509SerialNumber missing");
			}
			this.x509SerialNumber = xmlNodeList.Item(0).InnerText;
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
			retVal = creationXmlDocument.CreateElement("IssuerSerial", XadesSignedXml.XadesNamespaceUri);

			bufferXmlElement = creationXmlDocument.CreateElement("X509IssuerName", SignedXml.XmlDsigNamespaceUrl);
			bufferXmlElement.InnerText = this.x509IssuerName;
			retVal.AppendChild(bufferXmlElement);

			bufferXmlElement = creationXmlDocument.CreateElement("X509SerialNumber", SignedXml.XmlDsigNamespaceUrl);
			bufferXmlElement.InnerText = this.x509SerialNumber;
			retVal.AppendChild(bufferXmlElement);

			return retVal;
		}
		#endregion
	}
}