// UnsignedSignatureProperties.cs
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
	/// UnsignedSignatureProperties may contain properties that qualify XML
	/// signature itself or the signer
	/// </summary>
	public class UnsignedSignatureProperties
	{
		#region Private variables
		private CounterSignatureCollection counterSignatureCollection;
		private SignatureTimeStampCollection signatureTimeStampCollection;
		private CompleteCertificateRefs completeCertificateRefs;
		private CompleteRevocationRefs completeRevocationRefs;
		private bool refsOnlyTimeStampFlag;
		private SignatureTimeStampCollection sigAndRefsTimeStampCollection;
		private SignatureTimeStampCollection refsOnlyTimeStampCollection;
		private CertificateValues certificateValues;
		private RevocationValues revocationValues;
		private SignatureTimeStampCollection archiveTimeStampCollection;
		#endregion

		#region Public properties
		/// <summary>
		/// A collection of counter signatures
		/// </summary>
		public CounterSignatureCollection CounterSignatureCollection
		{
			get
			{
				return this.counterSignatureCollection;
			}
			set
			{
				this.counterSignatureCollection = value;
			}
		}

		/// <summary>
		/// A collection of signature timestamps
		/// </summary>
		public SignatureTimeStampCollection SignatureTimeStampCollection
		{
			get
			{
				return this.signatureTimeStampCollection;
			}
			set
			{
				this.signatureTimeStampCollection = value;
			}
		}

		/// <summary>
		/// This clause defines the XML element containing the sequence of
		/// references to the full set of CA certificates that have been used
		/// to validate the electronic signature up to (but not including) the
		/// signer's certificate. This is an unsigned property that qualifies
		/// the signature.
		/// An XML electronic signature aligned with the present document MAY
		/// contain at most one CompleteCertificateRefs element.
		/// </summary>
		public CompleteCertificateRefs CompleteCertificateRefs
		{
			get
			{
				return this.completeCertificateRefs;
			}
			set
			{
				this.completeCertificateRefs = value;
			}
		}

		/// <summary>
		/// This clause defines the XML element containing a full set of
		/// references to the revocation data that have been used in the
		/// validation of the signer and CA certificates.
		/// This is an unsigned property that qualifies the signature.
		/// The XML electronic signature aligned with the present document
		/// MAY contain at most one CompleteRevocationRefs element.
		/// </summary>
		public CompleteRevocationRefs CompleteRevocationRefs
		{
			get
			{
				return this.completeRevocationRefs;
			}
			set
			{
				this.completeRevocationRefs = value;
			}
		}

		/// <summary>
		/// Flag indicating if the RefsOnlyTimeStamp element (or several) is
		/// present (RefsOnlyTimeStampFlag = true).  If one or more
		/// sigAndRefsTimeStamps are present, RefsOnlyTimeStampFlag will be false.
		/// </summary>
		public bool RefsOnlyTimeStampFlag
		{
			get
			{
				return this.refsOnlyTimeStampFlag;
			}
			set
			{
				this.refsOnlyTimeStampFlag = value;
			}
		}

		/// <summary>
		/// A collection of sig and refs timestamps
		/// </summary>
		public SignatureTimeStampCollection SigAndRefsTimeStampCollection
		{
			get
			{
				return this.sigAndRefsTimeStampCollection;
			}
			set
			{
				this.sigAndRefsTimeStampCollection = value;
			}
		}

		/// <summary>
		/// A collection of refs only timestamps
		/// </summary>
		public SignatureTimeStampCollection RefsOnlyTimeStampCollection
		{
			get
			{
				return this.refsOnlyTimeStampCollection;
			}
			set
			{
				this.refsOnlyTimeStampCollection = value;
			}
		}

		/// <summary>
		/// Certificate values
		/// </summary>
		public CertificateValues CertificateValues
		{
			get
			{
				return this.certificateValues;
			}
			set
			{
				this.certificateValues = value;
			}
		}

		/// <summary>
		/// Revocation values
		/// </summary>
		public RevocationValues RevocationValues
		{
			get
			{
				return this.revocationValues;
			}
			set
			{
				this.revocationValues = value;
			}
		}

		/// <summary>
		/// A collection of signature timestamp
		/// </summary>
		public SignatureTimeStampCollection ArchiveTimeStampCollection
		{
			get
			{
				return this.archiveTimeStampCollection;
			}
			set
			{
				this.archiveTimeStampCollection = value;
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public UnsignedSignatureProperties()
		{
			this.counterSignatureCollection = new CounterSignatureCollection();
			this.signatureTimeStampCollection = new SignatureTimeStampCollection();
			this.completeCertificateRefs = new CompleteCertificateRefs();
			this.completeRevocationRefs = new CompleteRevocationRefs();
			this.refsOnlyTimeStampFlag = false;
			this.sigAndRefsTimeStampCollection = new SignatureTimeStampCollection();
			this.refsOnlyTimeStampCollection = new SignatureTimeStampCollection();
			this.certificateValues = new CertificateValues();
			this.revocationValues = new RevocationValues();
			this.archiveTimeStampCollection = new SignatureTimeStampCollection();
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

			if (this.counterSignatureCollection.Count > 0)
			{
				retVal = true;
			}

			if (this.signatureTimeStampCollection.Count > 0)
			{
				retVal = true;
			}

			if (this.completeCertificateRefs != null && this.completeCertificateRefs.HasChanged())
			{
				retVal = true;
			}

			if (this.completeRevocationRefs != null && this.completeRevocationRefs.HasChanged())
			{
				retVal = true;
			}

			if (this.sigAndRefsTimeStampCollection.Count > 0)
			{
				retVal = true;
			}

			if (this.refsOnlyTimeStampCollection.Count > 0)
			{
				retVal = true;
			}

			if (this.certificateValues != null && this.certificateValues.HasChanged())
			{
				retVal = true;
			}

			if (this.revocationValues != null && this.revocationValues.HasChanged())
			{
				retVal = true;
			}

			if (this.archiveTimeStampCollection.Count > 0)
			{
				retVal = true;
			}

			return retVal;
		}

		/// <summary>
		/// Load state from an XML element
		/// </summary>
		/// <param name="xmlElement">XML element containing new state</param>
		/// <param name="counterSignedXmlElement">Element containing parent signature (needed if there are counter signatures)</param>
		public void LoadXml(System.Xml.XmlElement xmlElement, XmlElement counterSignedXmlElement)
		{
			XmlNamespaceManager xmlNamespaceManager;
			XmlNodeList xmlNodeList;
			IEnumerator enumerator;
			XmlElement iterationXmlElement;
			XadesSignedXml newXadesSignedXml;
			TimeStamp newTimeStamp;
			XmlElement counterSignatureElement;
			
			if (xmlElement == null)
			{
				throw new ArgumentNullException("xmlElement");
			}

			xmlNamespaceManager = new XmlNamespaceManager(xmlElement.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("xsd", XadesSignedXml.XadesNamespaceUri);

			this.counterSignatureCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:CounterSignature", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						if (counterSignedXmlElement != null)
						{
							newXadesSignedXml = new XadesSignedXml(counterSignedXmlElement);
						}
						else
						{
							newXadesSignedXml = new XadesSignedXml();
						}
						//Skip any whitespace at start
						counterSignatureElement = null;
						for (int childNodeCounter = 0; (childNodeCounter < iterationXmlElement.ChildNodes.Count) && (counterSignatureElement == null); childNodeCounter++)
						{
							if (iterationXmlElement.ChildNodes[childNodeCounter] is XmlElement)
							{
								counterSignatureElement = (XmlElement)iterationXmlElement.ChildNodes[childNodeCounter];
							}
						}
						if (counterSignatureElement != null)
						{
							newXadesSignedXml.LoadXml(counterSignatureElement);
							this.counterSignatureCollection.Add(newXadesSignedXml);
						}
						else
						{
							throw new CryptographicException("CounterSignature element does not contain signature");
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

			this.signatureTimeStampCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:SignatureTimeStamp", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newTimeStamp = new TimeStamp("SignatureTimeStamp");
						newTimeStamp.LoadXml(iterationXmlElement);
						this.signatureTimeStampCollection.Add(newTimeStamp);
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

			xmlNodeList = xmlElement.SelectNodes("xsd:CompleteCertificateRefs", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.completeCertificateRefs = new CompleteCertificateRefs();
				this.completeCertificateRefs.LoadXml((XmlElement)xmlNodeList.Item(0));
			}
			else
			{
				this.completeCertificateRefs = null;
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:CompleteRevocationRefs", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.CompleteRevocationRefs = new CompleteRevocationRefs();
				this.CompleteRevocationRefs.LoadXml((XmlElement)xmlNodeList.Item(0));
			}
			else
			{
				this.completeRevocationRefs = null;
			}

			this.sigAndRefsTimeStampCollection.Clear();
			this.refsOnlyTimeStampCollection.Clear();

			xmlNodeList = xmlElement.SelectNodes("xsd:SigAndRefsTimeStamp", xmlNamespaceManager);
			if (xmlNodeList.Count > 0)
			{
				this.refsOnlyTimeStampFlag = false;
				enumerator = xmlNodeList.GetEnumerator();
				try 
				{
					while (enumerator.MoveNext()) 
					{
						iterationXmlElement = enumerator.Current as XmlElement;
						if (iterationXmlElement != null)
						{
							newTimeStamp = new TimeStamp("SigAndRefsTimeStamp");
							newTimeStamp.LoadXml(iterationXmlElement);
							this.sigAndRefsTimeStampCollection.Add(newTimeStamp);
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
				xmlNodeList = xmlElement.SelectNodes("xsd:RefsOnlyTimeStamp", xmlNamespaceManager);
				if (xmlNodeList.Count > 0)
				{
					this.refsOnlyTimeStampFlag = true;
					enumerator = xmlNodeList.GetEnumerator();
					try 
					{
						while (enumerator.MoveNext()) 
						{
							iterationXmlElement = enumerator.Current as XmlElement;
							if (iterationXmlElement != null)
							{
								newTimeStamp = new TimeStamp("RefsOnlyTimeStamp");
								newTimeStamp.LoadXml(iterationXmlElement);
								this.refsOnlyTimeStampCollection.Add(newTimeStamp);
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
					this.refsOnlyTimeStampFlag = false;
				}
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:CertificateValues", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.certificateValues = new CertificateValues();
				this.certificateValues.LoadXml((XmlElement)xmlNodeList.Item(0));
			}
			else
			{
				this.certificateValues = null;
			}

			xmlNodeList = xmlElement.SelectNodes("xsd:RevocationValues", xmlNamespaceManager);
			if (xmlNodeList.Count != 0)
			{
				this.revocationValues = new RevocationValues();
				this.revocationValues.LoadXml((XmlElement)xmlNodeList.Item(0));
			}
			else
			{
				this.revocationValues = null;
			}

			this.archiveTimeStampCollection.Clear();
			xmlNodeList = xmlElement.SelectNodes("xsd:ArchiveTimeStamp", xmlNamespaceManager);
			enumerator = xmlNodeList.GetEnumerator();
			try 
			{
				while (enumerator.MoveNext()) 
				{
					iterationXmlElement = enumerator.Current as XmlElement;
					if (iterationXmlElement != null)
					{
						newTimeStamp = new TimeStamp("ArchiveTimeStamp");
						newTimeStamp.LoadXml(iterationXmlElement);
						this.archiveTimeStampCollection.Add(newTimeStamp);
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
			XmlElement bufferXmlElement;

			creationXmlDocument = new XmlDocument();
			retVal = creationXmlDocument.CreateElement("UnsignedSignatureProperties", XadesSignedXml.XadesNamespaceUri);

			if (this.counterSignatureCollection.Count > 0)
			{
				foreach (XadesSignedXml xadesSignedXml in this.counterSignatureCollection)
				{
					bufferXmlElement = creationXmlDocument.CreateElement("CounterSignature", XadesSignedXml.XadesNamespaceUri);
					bufferXmlElement.AppendChild(creationXmlDocument.ImportNode(xadesSignedXml.GetXml(), true));
					retVal.AppendChild(creationXmlDocument.ImportNode(bufferXmlElement, true));
				}
			}

			if (this.signatureTimeStampCollection.Count > 0)
			{
				foreach (TimeStamp timeStamp in this.signatureTimeStampCollection)
				{
					if (timeStamp.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(timeStamp.GetXml(), true));
					}
				}
			}

			if (this.completeCertificateRefs != null && this.completeCertificateRefs.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.completeCertificateRefs.GetXml(), true));
			}

			if (this.completeRevocationRefs != null && this.completeRevocationRefs.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.completeRevocationRefs.GetXml(), true));
			}

			if (!this.refsOnlyTimeStampFlag)
			{
				foreach (TimeStamp timeStamp in this.sigAndRefsTimeStampCollection)
				{
					if (timeStamp.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(timeStamp.GetXml(), true));
					}
				}
			}
			else
			{
				foreach (TimeStamp timeStamp in this.refsOnlyTimeStampCollection)
				{
					if (timeStamp.HasChanged())
					{
						retVal.AppendChild(creationXmlDocument.ImportNode(timeStamp.GetXml(), true));
					}
				}
			}

			if (this.certificateValues != null && this.certificateValues.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.certificateValues.GetXml(), true));
			}

			if (this.revocationValues != null && this.revocationValues.HasChanged())
			{
				retVal.AppendChild(creationXmlDocument.ImportNode(this.revocationValues.GetXml(), true));
			}

			if (this.archiveTimeStampCollection.Count > 0)
			{
				foreach (TimeStamp timeStamp in this.archiveTimeStampCollection)
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
