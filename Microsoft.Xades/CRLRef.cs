// CRLRef.cs
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
	/// This class contains information about a Certificate Revocation List (CRL)
	/// </summary>
	public class CRLRef
	{
		#region Private variables
		private DigestAlgAndValueType digestAlgAndValue;
		private CRLIdentifier crlIdentifier;
		#endregion

		#region Public properties
		/// <summary>
		/// The digest of the entire DER encoded
		/// </summary>
		public DigestAlgAndValueType CertDigest
		{
			get
			{
				return this.digestAlgAndValue;
			}
			set
			{
				this.digestAlgAndValue = value;
			}
		}

		/// <summary>
		/// CRLIdentifier is a set of data including the issuer, the time when
		/// the CRL was issued and optionally the number of the CRL.
		/// The Identifier element can be dropped if the CRL could be inferred
		/// from other information.
		/// </summary>
		public CRLIdentifier CRLIdentifier
		{
			get
			{
				return this.crlIdentifier;
			}
			set
			{
				this.crlIdentifier = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public CRLRef()
		{
			this.digestAlgAndValue = new DigestAlgAndValueType("DigestAlgAndValue");
			this.crlIdentifier = new CRLIdentifier();
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

			if (this.digestAlgAndValue != null && this.digestAlgAndValue.HasChanged())
			{
				retVal = true;
			}

			if (this.crlIdentifier != null && this.crlIdentifier.HasChanged())
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

			xmlNodeList = xmlElement.SelectNodes("xsd:DigestAlgAndValue", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("DigestAlgAndValue missing");
			}
			this.digestAlgAndValue = new DigestAlgAndValueType("DigestAlgAndValue");
			this.digestAlgAndValue.LoadXml((XmlElement)xmlNodeList.Item(0));

			xmlNodeList = xmlElement.SelectNodes("xsd:CRLIdentifier", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				this.crlIdentifier = null;
			}
			else
			{
				this.crlIdentifier = new CRLIdentifier();
				this.crlIdentifier.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			retVal = creationXmlDocument.CreateElement("CRLRef", XadesSignedXml.XadesNamespaceUri);

			if (this.digestAlgAndValue != null && this.digestAlgAndValue.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.digestAlgAndValue.GetXml(), true));
			}
			else
			{
				throw new CryptographicException("DigestAlgAndValue element missing in CRLRef");
			}

			if (this.crlIdentifier != null && this.crlIdentifier.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.crlIdentifier.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}
