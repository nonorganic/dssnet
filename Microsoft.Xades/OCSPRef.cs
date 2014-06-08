// OCSPRef.cs
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
	/// This class identifies one OCSP response
	/// </summary>
	public class OCSPRef
	{
		#region Private variables
		private OCSPIdentifier ocspIdentifier;
		private DigestAlgAndValueType digestAlgAndValue;
		#endregion

		#region Public properties
		/// <summary>
		/// Identification of one OCSP response
		/// </summary>
		public OCSPIdentifier OCSPIdentifier
		{
			get
			{
				return this.ocspIdentifier;
			}
			set
			{
				this.ocspIdentifier = value;
			}
		}

		/// <summary>
		/// The digest computed on the DER encoded OCSP response, since it may be
		/// needed to differentiate between two OCSP responses by the same server
		/// with their "ProducedAt" fields within the same second.
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
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public OCSPRef()
		{
			this.ocspIdentifier = new OCSPIdentifier();
			this.digestAlgAndValue = new DigestAlgAndValueType("DigestAlgAndValue");
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

			if (this.ocspIdentifier != null && this.ocspIdentifier.HasChanged())
			{
				retVal = true;
			}

			if (this.digestAlgAndValue != null && this.digestAlgAndValue.HasChanged())
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

			xmlNodeList = xmlElement.SelectNodes("xsd:OCSPIdentifier", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("OCSPIdentifier missing");
			}
			this.ocspIdentifier = new OCSPIdentifier();
			this.ocspIdentifier.LoadXml((XmlElement)xmlNodeList.Item(0));

			xmlNodeList = xmlElement.SelectNodes("xsd:DigestAlgAndValue", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				this.digestAlgAndValue = null;
			}
			else
			{
				this.digestAlgAndValue = new DigestAlgAndValueType("DigestAlgAndValue");
				this.digestAlgAndValue.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			retVal = creationXmlDocument.CreateElement("OCSPRef", XadesSignedXml.XadesNamespaceUri);

			if (this.ocspIdentifier != null && this.ocspIdentifier.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.ocspIdentifier.GetXml(), true));
			}
			else
			{
				throw new CryptographicException("OCSPIdentifier element missing in OCSPRef");
			}

			if (this.digestAlgAndValue != null && this.digestAlgAndValue.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.digestAlgAndValue.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}
