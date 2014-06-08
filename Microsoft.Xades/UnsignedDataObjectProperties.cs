// UnsignedDataObjectProperties.cs
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
	/// The UnsignedDataObjectProperties element may contain properties that
	/// qualify some of the signed data objects.
	/// </summary>
	public class UnsignedDataObjectProperties
	{
		#region Private variables
		private UnsignedDataObjectPropertyCollection unsignedDataObjectPropertyCollection;
		#endregion

		#region Public properties
		/// <summary>
		/// A collection of unsigned data object properties
		/// </summary>
		public UnsignedDataObjectPropertyCollection UnsignedDataObjectPropertyCollection
		{
			get
			{
				return this.unsignedDataObjectPropertyCollection;
			}
			set
			{
				this.unsignedDataObjectPropertyCollection = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public UnsignedDataObjectProperties()
		{
			this.unsignedDataObjectPropertyCollection = new UnsignedDataObjectPropertyCollection();
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

			if (this.unsignedDataObjectPropertyCollection.Count > 0)
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
			UnsignedDataObjectProperty newUnsignedDataObjectProperty;
			IEnumerator enumerator;
			XmlElement iterationXmlElement;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			this.unsignedDataObjectPropertyCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:UnsignedDataObjectProperty", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newUnsignedDataObjectProperty = new UnsignedDataObjectProperty();
						newUnsignedDataObjectProperty.LoadXml(iterationXmlElement);
						this.unsignedDataObjectPropertyCollection.Add(newUnsignedDataObjectProperty);
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
			retVal = creationXmlDocument.CreateElement("UnsignedDataObjectProperties", XadesSignedXml.XadesNamespaceUri);

			if (this.unsignedDataObjectPropertyCollection.Count > 0)
			{
				foreach (UnsignedDataObjectProperty unsignedDataObjectProperty in this.unsignedDataObjectPropertyCollection)
				{
					if (unsignedDataObjectProperty.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(unsignedDataObjectProperty.GetXml(), true));
					}
				}
			}

			return retVal;
		}
		#endregion
	}
}
