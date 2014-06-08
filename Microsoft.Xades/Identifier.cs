// Identifier.cs
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
	/// Possible values for Qualifier
	/// </summary>
	public enum KnownQualifier
	{
		/// <summary>
		/// Value has not been set
		/// </summary>
		Uninitalized,
		/// <summary>
		/// OID encoded as Uniform Resource Identifier (URI).
		/// </summary>
		OIDAsURI,
		/// <summary>
		/// OID encoded as Uniform Resource Name (URN)
		/// </summary>
		OIDAsURN
	}

	/// <summary>
	/// The Identifier element contains a permanent identifier. Once assigned the
	/// identifier can never be re-assigned	again. It supports both the mechanism
	/// that is used to identify objects in ASN.1 and the mechanism that is
	/// usually used to identify objects in an XML environment.
	/// </summary>
	public class Identifier
	{
		#region Private variables
		private KnownQualifier qualifier;
		private string identifierUri;
		#endregion

		#region Public properties
		/// <summary>
		/// The optional Qualifier attribute can be used to provide a hint about the
		/// applied encoding (values OIDAsURN or OIDAsURI)
		/// </summary>
		public KnownQualifier Qualifier
		{
			get
			{
				return this.qualifier;
			}
			set
			{
				this.qualifier = value;
			}
		}

		/// <summary>
		/// Identification of the XML environment object
		/// </summary>
		public string IdentifierUri
		{
			get
			{
				return this.identifierUri;
			}
			set
			{
				this.identifierUri = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Identifier()
		{
			this.qualifier = KnownQualifier.Uninitalized;
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

			if (this.qualifier != KnownQualifier.Uninitalized)
			{
				retVal = true;
			}

			if (!String.IsNullOrEmpty(this.identifierUri))
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
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			if (xmlElement.HasAttribute("Qualifier"))
			{
				this.qualifier = (KnownQualifier)KnownQualifier.Parse(typeof(KnownQualifier), xmlElement.GetAttribute("Qualifier"), true);
			}
			else
			{
				this.qualifier = KnownQualifier.Uninitalized;
			}

			this.identifierUri = xmlElement.InnerText;
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
			retVal = creationXmlDocument.CreateElement("Identifier", XadesSignedXml.XadesNamespaceUri);

			if (this.qualifier != KnownQualifier.Uninitalized)
			{
				retVal.SetAttribute("Qualifier", this.qualifier.ToString());
			}

			retVal.InnerText = this.identifierUri;

			return retVal;
		}
		#endregion
	}
}
