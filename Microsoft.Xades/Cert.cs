// Cert.cs
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
	/// This class contains certificate identification information
	/// </summary>
	public class Cert
	{
		#region Private variables
		private DigestAlgAndValueType certDigest;
		private IssuerSerial issuerSerial;
		#endregion

		#region Public properties
		/// <summary>
		/// The element CertDigest contains the digest of one of the
		/// certificates referenced in the sequence
		/// </summary>
		public DigestAlgAndValueType CertDigest
		{
			get
			{
				return this.certDigest;
			}
			set
			{
				this.certDigest = value;
			}
		}

		/// <summary>
		/// The element IssuerSerial contains the identifier of one of the
		/// certificates referenced in the sequence. Should the
		/// X509IssuerSerial element appear in the signature to denote the same
		/// certificate, its value MUST be consistent with the corresponding
		/// IssuerSerial element.
		/// </summary>
		public IssuerSerial IssuerSerial
		{
			get
			{
				return this.issuerSerial;
			}
			set
			{
				this.issuerSerial = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Cert()
		{
			this.certDigest = new DigestAlgAndValueType("CertDigest");
			this.issuerSerial = new IssuerSerial();
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

			if (this.certDigest != null && this.certDigest.HasChanged())
			{
				retVal = true;
			}

			if (this.issuerSerial != null && this.issuerSerial.HasChanged())
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
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			xmlNodeList = xmlElement.SelectNodes("xsd:CertDigest", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("CertDigest missing");
			}
			this.certDigest = new DigestAlgAndValueType("CertDigest");
			this.certDigest.LoadXml((XmlElement)xmlNodeList.Item(0));

			xmlNodeList = xmlElement.SelectNodes("xsd:IssuerSerial", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("IssuerSerial missing");
			}
			this.issuerSerial = new IssuerSerial();
			this.issuerSerial.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			retVal = creationXmlDocument.CreateElement("Cert", XadesSignedXml.XadesNamespaceUri);

			if (this.certDigest != null && this.certDigest.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.certDigest.GetXml(), true));
			}

			if (this.issuerSerial != null && this.issuerSerial.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.issuerSerial.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}