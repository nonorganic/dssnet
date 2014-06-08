// NoticeRef.cs
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
	/// The NoticeRef element names an organization and identifies by
	/// numbers a group of textual statements prepared by that organization,
	/// so that the application could get the explicit notices from a notices file.
	/// </summary>
	public class NoticeRef
	{
		#region Private variables
		private string organization;
		private NoticeNumbers noticeNumbers;
		#endregion

		#region Public properties
		/// <summary>
		/// Organization issuing the signature policy
		/// </summary>
		public string Organization
		{
			get
			{
				return this.organization;
			}
			set
			{
				this.organization = value;
			}
		}

		/// <summary>
		/// Numerical identification of textual statements prepared by the organization,
		/// so that the application can get the explicit notices from a notices file.
		/// </summary>
		public NoticeNumbers NoticeNumbers
		{
			get
			{
				return this.noticeNumbers;
			}
			set
			{
				this.noticeNumbers = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public NoticeRef()
		{
			this.noticeNumbers = new NoticeNumbers();
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

			if (!String.IsNullOrEmpty(this.organization))
			{
				retVal = true;
			}

			if (this.noticeNumbers != null && this.noticeNumbers.HasChanged())
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

			xmlNodeList = xmlElement.SelectNodes("xsd:Organization", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("Organization missing");
			}
			this.organization = xmlNodeList.Item(0).InnerText;

			xmlNodeList = xmlElement.SelectNodes("xsd:NoticeNumbers", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				throw new CryptographicException("NoticeNumbers missing");
			}
			this.noticeNumbers = new NoticeNumbers();
			this.noticeNumbers.LoadXml((XmlElement)xmlNodeList.Item(0));
		}

		/// <summary>
		/// Returns the XML representation of the this object
		/// </summary>
		/// <returns>XML element containing the state of this object</returns>
		public XmlElement GetXml()
		{
			XmlDocument creationXmlDocument;
			XmlElement bufferXmlElement;
			XmlElement retVal;

			creationXmlDocument = new XmlDocument();
			retVal = creationXmlDocument.CreateElement("NoticeRef", XadesSignedXml.XadesNamespaceUri);

			if (this.organization == null)
			{
				throw new CryptographicException("Organization can't be null");
			}
			bufferXmlElement = creationXmlDocument.CreateElement("Organization", XadesSignedXml.XadesNamespaceUri);
			bufferXmlElement.InnerText = this.organization;
			retVal.AppendChild(bufferXmlElement);

			if (this.noticeNumbers == null)
			{
				throw new CryptographicException("NoticeNumbers can't be null");
			}
			retVal.AppendChild(creationXmlDocument.ImportNode(this.noticeNumbers.GetXml(), true));

			return retVal;
		}
		#endregion
	}
}