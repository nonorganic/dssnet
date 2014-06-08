// HashDataInfo.cs
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
	/// The HashDataInfo class contains a uri attribute referencing a data object
	/// and a ds:Transforms element indicating the transformations to make to this
	/// data object.
	/// The sequence of HashDataInfo elements will be used to produce the input of
	/// the hash computation process whose result will be included in the
	/// timestamp request to be sent to the TSA.
	/// </summary>
	public class HashDataInfo
	{
		#region Private variables
		private string uriAttribute;
		private Transforms transforms;
		#endregion

		#region Public properties
		/// <summary>
		/// Uri referencing a data object
		/// </summary>
		public string UriAttribute
		{
			get
			{
				return this.uriAttribute;
			}
			set
			{
				this.uriAttribute = value;
			}
		}

		/// <summary>
		/// Transformations to make to this data object
		/// </summary>
		public Transforms Transforms
		{
			get
			{
				return this.transforms;
			}
			set
			{
				this.transforms = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public HashDataInfo()
		{
			this.transforms = new Transforms();
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

			if (!String.IsNullOrEmpty(this.uriAttribute))
			{
				retVal = true;
			}

			if (this.transforms != null && this.transforms.HasChanged())
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
			if (xmlElement.HasAttribute("uri"))
			{
				this.uriAttribute = xmlElement.GetAttribute("uri");
			}
			else
			{
				this.uriAttribute = "";
				throw new CryptographicException("uri attribute missing");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			xmlNodeList = xmlElement.SelectNodes("xsd:Transforms", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.transforms = new Transforms();
				this.transforms.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			retVal = creationXmlDocument.CreateElement("HashDataInfo", XadesSignedXml.XadesNamespaceUri);

			retVal.SetAttribute("uri", this.uriAttribute);

			if (this.transforms != null && this.transforms.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.transforms.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}
