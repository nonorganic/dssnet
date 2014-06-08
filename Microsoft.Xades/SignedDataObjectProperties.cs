// SignedDataObjectProperties.cs
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
	/// The SignedDataObjectProperties element contains properties that qualify
	/// some of the signed data objects
	/// </summary>
	public class SignedDataObjectProperties
	{
		#region Private variables
		private DataObjectFormatCollection dataObjectFormatCollection;
		private CommitmentTypeIndicationCollection commitmentTypeIndicationCollection;
		private AllDataObjectsTimeStampCollection allDataObjectsTimeStampCollection;
		private IndividualDataObjectsTimeStampCollection individualDataObjectsTimeStampCollection;
		#endregion

		#region Public properties
		/// <summary>
		/// Collection of signed data object formats
		/// </summary>
		public DataObjectFormatCollection DataObjectFormatCollection
		{
			get
			{
				return this.dataObjectFormatCollection;
			}
			set
			{
				this.dataObjectFormatCollection = value;
			}
		}

		/// <summary>
		/// Collection of commitment type indications
		/// </summary>
		public CommitmentTypeIndicationCollection CommitmentTypeIndicationCollection
		{
			get
			{
				return this.commitmentTypeIndicationCollection;
			}
			set
			{
				this.commitmentTypeIndicationCollection = value;
			}
		}

		/// <summary>
		/// Collection of all data object timestamps
		/// </summary>
		public AllDataObjectsTimeStampCollection AllDataObjectsTimeStampCollection
		{
			get
			{
				return this.allDataObjectsTimeStampCollection;
			}
			set
			{
				this.allDataObjectsTimeStampCollection = value;
			}
		}

		/// <summary>
		/// Collection of individual data object timestamps
		/// </summary>
		public IndividualDataObjectsTimeStampCollection IndividualDataObjectsTimeStampCollection
		{
			get
			{
				return this.individualDataObjectsTimeStampCollection;
			}
			set
			{
				this.individualDataObjectsTimeStampCollection = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public SignedDataObjectProperties()
		{
			this.dataObjectFormatCollection = new DataObjectFormatCollection();
			this.commitmentTypeIndicationCollection = new CommitmentTypeIndicationCollection();
			this.allDataObjectsTimeStampCollection = new AllDataObjectsTimeStampCollection();
			this.individualDataObjectsTimeStampCollection = new IndividualDataObjectsTimeStampCollection();
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

			if (this.dataObjectFormatCollection.Count > 0)
			{
				retVal = true;
			}

			if (this.commitmentTypeIndicationCollection.Count > 0)
			{
				retVal = true;
			}

			if (this.allDataObjectsTimeStampCollection.Count > 0)
			{
				retVal = true;
			}

			if (this.individualDataObjectsTimeStampCollection.Count > 0)
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
			DataObjectFormat newDataObjectFormat;
			CommitmentTypeIndication newCommitmentTypeIndication;
			TimeStamp newTimeStamp;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			this.dataObjectFormatCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:DataObjectFormat", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newDataObjectFormat = new DataObjectFormat();
						newDataObjectFormat.LoadXml(iterationXmlElement);
						this.dataObjectFormatCollection.Add(newDataObjectFormat);
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

			this.dataObjectFormatCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:CommitmentTypeIndication", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newCommitmentTypeIndication = new CommitmentTypeIndication();
						newCommitmentTypeIndication.LoadXml(iterationXmlElement);
						this.commitmentTypeIndicationCollection.Add(newCommitmentTypeIndication);
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

			this.dataObjectFormatCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:AllDataObjectsTimeStamp", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newTimeStamp = new TimeStamp("AllDataObjectsTimeStamp");
						newTimeStamp.LoadXml(iterationXmlElement);
						this.allDataObjectsTimeStampCollection.Add(newTimeStamp);
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

			this.dataObjectFormatCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:IndividualDataObjectsTimeStamp", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newTimeStamp = new TimeStamp("IndividualDataObjectsTimeStamp");
						newTimeStamp.LoadXml(iterationXmlElement);
						this.individualDataObjectsTimeStampCollection.Add(newTimeStamp);
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
			retVal = creationXmlDocument.CreateElement("SignedDataObjectProperties", XadesSignedXml.XadesNamespaceUri);

			if (this.dataObjectFormatCollection.Count > 0)
			{
				foreach (DataObjectFormat dataObjectFormat in this.dataObjectFormatCollection)
				{
					if (dataObjectFormat.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(dataObjectFormat.GetXml(), true));
					}
				}
			}

			if (this.commitmentTypeIndicationCollection.Count > 0)
			{
				foreach (CommitmentTypeIndication commitmentTypeIndication in this.commitmentTypeIndicationCollection)
				{
					if (commitmentTypeIndication.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(commitmentTypeIndication.GetXml(), true));
					}
				}
			}

			if (this.allDataObjectsTimeStampCollection.Count > 0)
			{
				foreach (TimeStamp timeStamp in this.allDataObjectsTimeStampCollection)
				{
					if (timeStamp.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(timeStamp.GetXml(), true));
					}
				}
			}

			if (this.individualDataObjectsTimeStampCollection.Count > 0)
			{
				foreach (TimeStamp timeStamp in this.individualDataObjectsTimeStampCollection)
				{
					if (timeStamp.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(timeStamp.GetXml(), true));
					}
				}
			}

			return retVal;
		}
		#endregion
	}
}
