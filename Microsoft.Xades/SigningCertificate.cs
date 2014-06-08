// SigningCertificate.cs
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
	/// This class has as purpose to provide the simple substitution of the
	/// certificate. It contains references to certificates and digest values
	/// computed on them
	/// </summary>
	public class SigningCertificate
	{
		#region Private variables
		private CertCollection certCollection;
		#endregion

		#region Public properties
		/// <summary>
		/// A collection of certs
		/// </summary>
		public CertCollection CertCollection
		{
			get
			{
				return this.certCollection;
			}
			set
			{
				this.certCollection = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SigningCertificate()
		{
			this.certCollection = new CertCollection();
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Check to see if something has changed in this instance and needs to be serialized
		/// </summary>
		/// <returns>Flag indicating if a member needs serialization</returns>
		public bool HasChanged()
		{
            return true; //Should always be considered dirty
		}

		/// <summary>
		/// Load state from an XML element
		/// </summary>
		/// <param name="xmlElement">XML element containing new state</param>
		public void LoadXml(System.Xml.XmlElement xmlElement)
		{
			XmlNamespaceManager xmlNamespaceManager;
			XmlNodeList xmlNodeList;
			IEnumerator enumerator;
			XmlElement iterationXmlElement;
			Cert newCert;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			this.certCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:Cert", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newCert = new Cert();
						newCert.LoadXml(iterationXmlElement);
						this.certCollection.Add(newCert);
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
			retVal = creationXmlDocument.CreateElement("SigningCertificate", XadesSignedXml.XadesNamespaceUri);

			if (this.certCollection.Count > 0)
			{
				foreach (Cert cert in this.certCollection)
				{
					if (cert.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(cert.GetXml(), true));
					}
				}
			}
			else
			{
				throw new CryptographicException("SigningCertificate.Certcollection should have count > 0");
			}

			return retVal;
		}
		#endregion
	}
}
