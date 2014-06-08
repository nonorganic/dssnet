// SigpolicyQualifiers.cs
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
using System.Collections;

namespace Microsoft.Xades
{
	/// <summary>
	/// This class contains a collection of SigPolicyQualifiers
	/// </summary>
	public class SigPolicyQualifiers
	{
		#region Private variables
		private SigPolicyQualifierCollection sigPolicyQualifierCollection;
		#endregion

		#region Public properties
		/// <summary>
		/// A collection of sig policy qualifiers
		/// </summary>
		public SigPolicyQualifierCollection SigPolicyQualifierCollection
		{
			get
			{
				return this.sigPolicyQualifierCollection;
			}
			set
			{
				this.sigPolicyQualifierCollection = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SigPolicyQualifiers()
		{
			this.sigPolicyQualifierCollection = new SigPolicyQualifierCollection();
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

			if (this.sigPolicyQualifierCollection.Count > 0)
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
			SPUri newSPUri;
			SPUserNotice newSPUserNotice;
			SigPolicyQualifier newSigPolicyQualifier;
			IEnumerator enumerator;
			XmlElement iterationXmlElement;
			XmlElement subElement;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			this.sigPolicyQualifierCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:SigPolicyQualifier", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						subElement = (XmlElement)iterationXmlElement.SelectSingleNode("xsd:SPURI", xmlNamespaceManager);
						if (subElement != null)
						{
							newSPUri = new SPUri();
							newSPUri.LoadXml(iterationXmlElement);
							this.sigPolicyQualifierCollection.Add(newSPUri);
						}
						else
						{
							subElement = (XmlElement)iterationXmlElement.SelectSingleNode("xsd:SPUserNotice", xmlNamespaceManager);
							if (subElement != null)
							{
								newSPUserNotice = new SPUserNotice();
								newSPUserNotice.LoadXml(iterationXmlElement);
								this.sigPolicyQualifierCollection.Add(newSPUserNotice);
							}
							else
							{
								newSigPolicyQualifier = new SigPolicyQualifier();
								newSigPolicyQualifier.LoadXml(iterationXmlElement);
								this.sigPolicyQualifierCollection.Add(newSigPolicyQualifier);
							}
						}
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
			retVal = creationXmlDocument.CreateElement("SigPolicyQualifiers", XadesSignedXml.XadesNamespaceUri);

			if (this.sigPolicyQualifierCollection.Count > 0)
			{
				foreach (SigPolicyQualifier sigPolicyQualifier in this.sigPolicyQualifierCollection)
				{
					if (sigPolicyQualifier.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(sigPolicyQualifier.GetXml(), true));
					}
				}
			}

			return retVal;
		}
		#endregion
	}
}
