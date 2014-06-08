// SignerRole.cs
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
	/// According to what has been stated in the Introduction clause, an
	/// electronic signature produced in accordance with the present document
	/// incorporates: "a commitment that has been explicitly endorsed under a
	/// signature policy, at a given time, by a signer under an identifier,
	/// e.g. a name or a pseudonym, and optionally a role".
	/// While the name of the signer is important, the position of the signer
	/// within a company or an organization can be even more important. Some
	/// contracts may only be valid if signed by a user in a particular role,
	/// e.g. a Sales Director. In many cases who the sales Director really is,
	/// is not that important but being sure that the signer is empowered by his
	/// company to be the Sales Director is fundamental.
	/// </summary>
	public class SignerRole
	{
		#region Private variables
		private ClaimedRoles claimedRoles;
		private CertifiedRoles certifiedRoles;
		#endregion

		#region Public properties
		/// <summary>
		/// The ClaimedRoles element contains a sequence of roles claimed by
		/// the signer but not certified. Additional contents types may be
		/// defined on a domain application basis and be part of this element.
		/// The namespaces given to the corresponding XML schemas will allow
		/// their unambiguous identification in the case these roles use XML.
		/// </summary>
		public ClaimedRoles ClaimedRoles
		{
			get
			{
				return this.claimedRoles;
			}
			set
			{
				this.claimedRoles = value;
			}
		}

		/// <summary>
		/// The CertifiedRoles element contains one or more wrapped attribute
		/// certificates for the signer
		/// </summary>
		public CertifiedRoles CertifiedRoles
		{
			get
			{
				return this.certifiedRoles;
			}
			set
			{
				this.certifiedRoles = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SignerRole()
		{
			this.claimedRoles = new ClaimedRoles();
			this.certifiedRoles = new CertifiedRoles();
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

			if (this.claimedRoles != null && this.claimedRoles.HasChanged())
			{
				retVal = true;
			}

			if (this.certifiedRoles != null && this.certifiedRoles.HasChanged())
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

			xmlNodeList = xmlElement.SelectNodes("xsd:ClaimedRoles", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.claimedRoles = new ClaimedRoles();
				this.claimedRoles.LoadXml((XmlElement)xmlNodeList.Item(0));
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:CertifiedRoles", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.certifiedRoles = new CertifiedRoles();
				this.certifiedRoles.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			retVal = creationXmlDocument.CreateElement("SignerRole", XadesSignedXml.XadesNamespaceUri);

			if (this.claimedRoles != null && this.claimedRoles.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.claimedRoles.GetXml(), true));
			}

			if (this.certifiedRoles != null && this.certifiedRoles.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.certifiedRoles.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}
