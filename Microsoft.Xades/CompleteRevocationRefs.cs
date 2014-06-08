// CompleteRevocationRefs.cs
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
	/// This clause defines the XML element containing a full set of
	/// references to the revocation data that have been used in the
	/// validation of the signer and CA certificates.
	/// This is an unsigned property that qualifies the signature.
	/// The XML electronic signature aligned with the present document
	/// MAY contain at most one CompleteRevocationRefs element.
	/// </summary>
	public class CompleteRevocationRefs
	{
		#region Private variables
		private string id;
		private CRLRefs crlRefs;
		private OCSPRefs ocspRefs;
		private OtherRefs otherRefs;
		#endregion

		#region Public properties
		/// <summary>
		/// The optional Id attribute can be used to make a reference to the CompleteRevocationRefs element
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
		/// Sequences of references to CRLs
		/// </summary>
		public CRLRefs CRLRefs
		{
			get
			{
				return this.crlRefs;
			}
			set
			{
				this.crlRefs = value;
			}
		}

		/// <summary>
		/// Sequences of references to OCSP responses
		/// </summary>
		public OCSPRefs OCSPRefs
		{
			get
			{
				return this.ocspRefs;
			}
			set
			{
				this.ocspRefs = value;
			}
		}

		/// <summary>
		/// Other references to alternative forms of revocation data
		/// </summary>
		public OtherRefs OtherRefs
		{
			get
			{
				return this.otherRefs;
			}
			set
			{
				this.otherRefs = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public CompleteRevocationRefs()
		{
			this.crlRefs = new CRLRefs();
			this.ocspRefs = new OCSPRefs();
			this.otherRefs = new OtherRefs();
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
			if (this.crlRefs != null && this.crlRefs.HasChanged())
			{
				retVal = true;
			}
			if (this.ocspRefs != null && this.ocspRefs.HasChanged())
			{
				retVal = true;
			}
			if (this.otherRefs != null && this.otherRefs.HasChanged())
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

			xmlNodeList = xmlElement.SelectNodes("xsd:CRLRefs", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.crlRefs = new CRLRefs();
				this.crlRefs.LoadXml((XmlElement)xmlNodeList.Item(0));
			}
			xmlNodeList = xmlElement.SelectNodes("xsd:OCSPRefs", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.ocspRefs = new OCSPRefs();
				this.ocspRefs.LoadXml((XmlElement)xmlNodeList.Item(0));
			}
			xmlNodeList = xmlElement.SelectNodes("xsd:OtherRefs", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.otherRefs = new OtherRefs();
				this.otherRefs.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			retVal = creationXmlDocument.CreateElement("CompleteRevocationRefs", XadesSignedXml.XadesNamespaceUri);
			if (!String.IsNullOrEmpty(this.id))
			{
				retVal.SetAttribute("Id", this.id);
			}
			if (this.crlRefs != null && this.crlRefs.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.crlRefs.GetXml(), true));
			}
			if (this.ocspRefs != null && this.ocspRefs.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.ocspRefs.GetXml(), true));
			}
			if (this.otherRefs != null && this.otherRefs.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.otherRefs.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}

