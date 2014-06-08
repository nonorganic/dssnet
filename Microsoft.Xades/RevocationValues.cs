// RevocationValues.cs
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

namespace Microsoft.Xades
{
	/// <summary>
	/// The RevocationValues element is used to hold the values of the
	/// revocation information which are to be shipped with the XML signature
	/// in case of an XML Advanced Electronic Signature with Extended
	/// Validation Data (XAdES-X-Long). This is a unsigned property that
	/// qualifies the signature. An XML electronic signature aligned with the
	/// present document MAY contain at most one RevocationValues element.
	/// </summary>
	public class RevocationValues
	{
		#region Private variables
		private string id;
		private CRLValues crlValues;
		private OCSPValues ocspValues;
		private OtherValues otherValues;
		#endregion

		#region Public properties
		/// <summary>
		/// Optional Id for the XML element
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
		/// Certificate Revocation Lists
		/// </summary>
		public CRLValues CRLValues
		{
			get
			{
				return this.crlValues;
			}
			set
			{
				this.crlValues = value;
			}
		}

		/// <summary>
		/// Responses from an online certificate status server
		/// </summary>
		public OCSPValues OCSPValues
		{
			get
			{
				return this.ocspValues;
			}
			set
			{
				this.ocspValues = value;
			}
		}

		/// <summary>
		/// Placeholder for other revocation information is provided for future
		/// use
		/// </summary>
		public OtherValues OtherValues
		{
			get
			{
				return this.otherValues;
			}
			set
			{
				this.otherValues = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public RevocationValues()
		{
			this.crlValues = new CRLValues();
			this.ocspValues = new OCSPValues();
			this.otherValues = new OtherValues();
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
			if (this.crlValues != null && this.crlValues.HasChanged())
			{
				retVal = true;
			}
			if (this.ocspValues != null && this.ocspValues.HasChanged())
			{
				retVal = true;
			}
			if (this.otherValues != null && this.otherValues.HasChanged())
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

			xmlNodeList = xmlElement.SelectNodes("xsd:CRLValues", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.crlValues = new CRLValues();
				this.crlValues.LoadXml((XmlElement)xmlNodeList.Item(0));
			}
			xmlNodeList = xmlElement.SelectNodes("xsd:OCSPValues", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.ocspValues = new OCSPValues();
				this.ocspValues.LoadXml((XmlElement)xmlNodeList.Item(0));
			}
			xmlNodeList = xmlElement.SelectNodes("xsd:OtherValues", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.otherValues = new OtherValues();
				this.otherValues.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			retVal = creationXmlDocument.CreateElement("RevocationValues", XadesSignedXml.XadesNamespaceUri);
			if (this.id != null && this.id != "")
			{
				retVal.SetAttribute("Id", this.id);
			}
			if (this.crlValues != null && this.crlValues.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.crlValues.GetXml(), true));
			}
			if (this.ocspValues != null && this.ocspValues.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.ocspValues.GetXml(), true));
			}
			if (this.otherValues != null && this.otherValues.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.otherValues.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}
