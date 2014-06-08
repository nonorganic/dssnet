// SigpolicyQualifier.cs
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
	/// This class can contain additional information qualifying the signature
	/// policy identifier
	/// </summary>
	public class SigPolicyQualifier
	{
		#region Private variables
		private XmlElement anyXmlElement;
		#endregion

		#region Public properties
		/// <summary>
		/// The generic XML element that represents a sig policy qualifier
		/// </summary>
		public virtual XmlElement AnyXmlElement
		{
			get
			{
				return this.anyXmlElement;
			}
			set
			{
				this.anyXmlElement = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SigPolicyQualifier()
		{
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Check to see if something has changed in this instance and needs to be serialized
		/// </summary>
		/// <returns>Flag indicating if a member needs serialization</returns>
		public virtual bool HasChanged()
		{
			bool retVal = false;

			if (this.anyXmlElement != null)
			{
				retVal = true;
			}

			return retVal;
		}

		/// <summary>
		/// Load state from an XML element
		/// </summary>
		/// <param name="xmlElement">XML element containing new state</param>
		public virtual void LoadXml(System.Xml.XmlElement xmlElement)
		{
			this.anyXmlElement = xmlElement;
		}

		/// <summary>
		/// Returns the XML representation of the this object
		/// </summary>
		/// <returns>XML element containing the state of this object</returns>
		public virtual XmlElement GetXml()
		{
			XmlDocument creationXmlDocument;
			XmlElement retVal;

			creationXmlDocument = new XmlDocument();
			retVal = creationXmlDocument.CreateElement("SigPolicyQualifier", XadesSignedXml.XadesNamespaceUri);

			if (this.anyXmlElement != null)
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.anyXmlElement, true));
			}

			return retVal;
		}
		#endregion
	}
}
