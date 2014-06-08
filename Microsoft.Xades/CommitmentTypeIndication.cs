// CommitmentTypeIndication.cs
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
	/// The commitment type can be indicated in the electronic signature
	/// by either explicitly using a commitment type indication in the
	/// electronic signature or implicitly or explicitly from the semantics
	/// of the signed data object.
	/// If the indicated commitment type is explicit by means of a commitment
	/// type indication in the electronic signature, acceptance of a verified
	/// signature implies acceptance of the semantics of that commitment type.
	/// The semantics of explicit commitment types indications shall be
	/// specified either as part of the signature policy or may be registered
	/// for	generic use across multiple policies.
	/// </summary>
	public class CommitmentTypeIndication
	{
		#region Private variables
		private ObjectIdentifier commitmentTypeId;
		private ObjectReferenceCollection objectReferenceCollection;
		private bool allSignedDataObjects;
		private CommitmentTypeQualifiers commitmentTypeQualifiers;
		#endregion

		#region Public properties
		/// <summary>
		/// The CommitmentTypeId element univocally identifies the type of commitment made by the signer.
		/// A number of commitments have been already identified and assigned corresponding OIDs.
		/// </summary>
		public ObjectIdentifier CommitmentTypeId
		{
			get
			{
				return this.commitmentTypeId;
			}
			set
			{
				this.commitmentTypeId = value;
			}
		}

		/// <summary>
		/// Collection of object references
		/// </summary>
		public ObjectReferenceCollection ObjectReferenceCollection
		{
			get
			{
				return this.objectReferenceCollection;
			}
			set
			{
				this.objectReferenceCollection = value;
				if (this.objectReferenceCollection != null)
				{
					if (this.objectReferenceCollection.Count > 0)
					{
						this.allSignedDataObjects = false;
					}
				}
			}
		}

		/// <summary>
		/// If all the signed data objects share the same commitment, the
		/// AllSignedDataObjects empty element MUST be present.
		/// </summary>
		public bool AllSignedDataObjects
		{
			get
			{
				return this.allSignedDataObjects;
			}
			set
			{
				this.allSignedDataObjects = value;
				if (this.allSignedDataObjects)
				{
					this.objectReferenceCollection.Clear();
				}
			}
		}

		/// <summary>
		/// The CommitmentTypeQualifiers element provides means to include additional
		/// qualifying information on the commitment made by the signer.
		/// </summary>
		public CommitmentTypeQualifiers CommitmentTypeQualifiers
		{
			get
			{
				return this.commitmentTypeQualifiers;
			}
			set
			{
				this.commitmentTypeQualifiers = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public CommitmentTypeIndication()
		{
			this.commitmentTypeId = new ObjectIdentifier("CommitmentTypeId");
			this.objectReferenceCollection = new ObjectReferenceCollection();
			this.allSignedDataObjects = true;
			this.commitmentTypeQualifiers = new CommitmentTypeQualifiers();
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

			if (this.commitmentTypeId != null && this.commitmentTypeId.HasChanged())
			{
				retVal = true;
			}

			if (this.objectReferenceCollection.Count > 0)
			{
				retVal = true;
			}

			if (this.commitmentTypeQualifiers != null && this.commitmentTypeQualifiers.HasChanged())
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
			IEnumerator enumerator;
			XmlElement iterationXmlElement;
			ObjectReference newObjectReference;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			xmlNodeList = xmlElement.SelectNodes("xsd:CommitmentTypeId", xmlNamespaceManager);
			if (xmlNodeList.Count == 0)
			{
				this.commitmentTypeId = null;
				throw new CryptographicException("CommitmentTypeId missing");
			}
			else
			{
				this.commitmentTypeId = new ObjectIdentifier("CommitmentTypeId");
				this.commitmentTypeId.LoadXml((XmlElement)xmlNodeList.Item(0));
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:ObjectReference", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.objectReferenceCollection.Clear();
				this.allSignedDataObjects = false;
				enumerator = xmlNodeList.GetEnumerator();
				try 
				{
					while (enumerator.MoveNext()) 
					{
						iterationXmlElement = enumerator.Current as XmlElement;
						if (iterationXmlElement != null)
						{
							newObjectReference = new ObjectReference();
							newObjectReference.LoadXml(iterationXmlElement);
							this.objectReferenceCollection.Add(newObjectReference);
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
			else
			{
				this.objectReferenceCollection.Clear();
				this.allSignedDataObjects = true;
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:CommitmentTypeQualifiers", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.commitmentTypeQualifiers = new CommitmentTypeQualifiers();
				this.commitmentTypeQualifiers.LoadXml((XmlElement)xmlNodeList.Item(0));
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
			XmlElement bufferXmlElement;

			creationXmlDocument = new XmlDocument();
			retVal = creationXmlDocument.CreateElement("CommitmentTypeIndication", XadesSignedXml.XadesNamespaceUri);

			if (this.commitmentTypeId != null && this.commitmentTypeId.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.commitmentTypeId.GetXml(), true));
			}
			else
			{
				throw new CryptographicException("CommitmentTypeId element missing");
			}

			if (this.allSignedDataObjects)
			{ //Add emty element as required
				bufferXmlElement = creationXmlDocument.CreateElement("AllSignedDataObjects", XadesSignedXml.XadesNamespaceUri);
				retVal.AppendChild(bufferXmlElement);
			}
			else
			{
				if (this.objectReferenceCollection.Count > 0)
				{
					foreach (ObjectReference objectReference in this.objectReferenceCollection)
					{
						if (objectReference.HasChanged())
						{
							retVal.AppendChild(creationXmlDocument.ImportNode(objectReference.GetXml(), true));
						}
					}
				}
			}

			if (this.commitmentTypeQualifiers != null && this.commitmentTypeQualifiers.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.commitmentTypeQualifiers.GetXml(), true));
			}

			return retVal;
		}
		#endregion
	}
}
