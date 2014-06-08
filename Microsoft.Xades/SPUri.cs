// SPUri.cs
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
	/// SPUri represents the URL where the copy of the Signature Policy may be
	/// obtained.  The class derives from SigPolicyQualifier.
	/// </summary>
	public class SPUri : SigPolicyQualifier
	{
		#region Private variables
		private string uri;
		#endregion

		#region Public properties
		/// <summary>
		/// Uri for the sig policy qualifier
		/// </summary>
		public string Uri
		{
			get
			{
				return this.uri;
			}
			set
			{
				this.uri = value;
			}
		}

		/// <summary>
		/// Inherited generic element, not used in the SPUri class
		/// </summary>
		public override XmlElement AnyXmlElement
		{
			get
			{
				return null; //This does not make sense for SPUri
			}
			set
			{
				throw new CryptographicException("Setting AnyXmlElement on a SPUri is not supported");
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SPUri()
		{
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Check to see if something has changed in this instance and needs to be serialized
		/// </summary>
		/// <returns>Flag indicating if a member needs serialization</returns>
		public override bool HasChanged()
		{
			bool retVal = false;

			if (this.uri != null && this.uri != "")
			{
				retVal = true;
			}

			return retVal;
		}

		/// <summary>
		/// Load state from an XML element
		/// </summary>
		/// <param name="xmlElement">XML element containing new state</param>
		public override void LoadXml(System.Xml.XmlElement xmlElement)
		{
			XmlNamespaceManager xmlNamespaceManager;
			XmlNodeList xmlNodeList;

			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			xmlNodeList = xmlElement.SelectNodes("xsd:SPURI", xmlNamespaceManager);

			this.uri = ((XmlElement)xmlNodeList.Item(0)).InnerText;
		}

		/// <summary>
		/// Returns the XML representation of the this object
		/// </summary>
		/// <returns>XML element containing the state of this object</returns>
		public override XmlElement GetXml()
		{
			XmlDocument creationXmlDocument;
			XmlElement bufferXmlElement;
			XmlElement retVal;

			creationXmlDocument = new XmlDocument();
			retVal = creationXmlDocument.CreateElement("SigPolicyQualifier", XadesSignedXml.XadesNamespaceUri);

			bufferXmlElement = creationXmlDocument.CreateElement("SPURI", XadesSignedXml.XadesNamespaceUri);
			bufferXmlElement.InnerText = this.uri;
			retVal.AppendChild(creationXmlDocument.ImportNode(bufferXmlElement, true));

			return retVal;
		}
		#endregion
	}
}
