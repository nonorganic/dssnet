// CertifiedRoles.cs
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
using System.Collections;

namespace Microsoft.Xades
{
	/// <summary>
	/// The CertifiedRoles element contains one or more wrapped attribute
	/// certificates for the signer
	/// </summary>
	public class CertifiedRoles
	{
		#region Private variables
		private CertifiedRoleCollection certifiedRoleCollection;
		#endregion

		#region Public properties
		/// <summary>
		/// Collection of certified roles
		/// </summary>
		public CertifiedRoleCollection CertifiedRoleCollection
		{
			get
			{
				return this.certifiedRoleCollection;
			}
			set
			{
				this.certifiedRoleCollection = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public CertifiedRoles()
		{
			this.certifiedRoleCollection = new CertifiedRoleCollection();
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

			if (this.certifiedRoleCollection.Count > 0)
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
			EncapsulatedPKIData newCertifiedRole;
			IEnumerator enumerator;
			XmlElement iterationXmlElement;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			this.certifiedRoleCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:CertifiedRole", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newCertifiedRole = new EncapsulatedPKIData("CertifiedRole");
						newCertifiedRole.LoadXml(iterationXmlElement);
						this.certifiedRoleCollection.Add(newCertifiedRole);
					}
				}
			}
			finally 
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
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
			retVal = creationXmlDocument.CreateElement("CertifiedRoles", XadesSignedXml.XadesNamespaceUri);

			if (this.certifiedRoleCollection.Count > 0)
			{
				foreach (EncapsulatedPKIData certifiedRole in this.certifiedRoleCollection)
				{
					if (certifiedRole.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(certifiedRole.GetXml(), true));
					}
				}
			}

			return retVal;
		}
		#endregion
	}
}
