// ClaimedRoles.cs
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
	/// The ClaimedRoles element contains a sequence of roles claimed by
	/// the signer but not certified. Additional contents types may be
	/// defined on a domain application basis and be part of this element.
	/// The namespaces given to the corresponding XML schemas will allow
	/// their unambiguous identification in the case these roles use XML.
	/// </summary>
	public class ClaimedRoles
	{
		#region Private variables
		private ClaimedRoleCollection claimedRoleCollection;
		#endregion

		#region Public properties
		/// <summary>
		/// Collection of claimed roles
		/// </summary>
		public ClaimedRoleCollection ClaimedRoleCollection
		{
			get
			{
				return this.claimedRoleCollection;
			}
			set
			{
				this.claimedRoleCollection = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public ClaimedRoles()
		{
			this.claimedRoleCollection = new ClaimedRoleCollection();
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

			if (this.claimedRoleCollection.Count > 0)
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
			ClaimedRole newClaimedRole;
			IEnumerator enumerator;
			XmlElement iterationXmlElement;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			this.claimedRoleCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:ClaimedRole", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newClaimedRole = new ClaimedRole();
						newClaimedRole.LoadXml(iterationXmlElement);
						this.claimedRoleCollection.Add(newClaimedRole);
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
			retVal = creationXmlDocument.CreateElement("ClaimedRoles", XadesSignedXml.XadesNamespaceUri);

			if (this.claimedRoleCollection.Count > 0)
			{
				foreach (ClaimedRole claimedRole in this.claimedRoleCollection)
				{
					if (claimedRole.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(claimedRole.GetXml(), true));
					}
				}
			}

			return retVal;
		}
		#endregion
	}
}
