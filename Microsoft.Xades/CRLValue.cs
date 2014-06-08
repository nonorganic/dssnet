// CRLValue.cs
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
	/// This class consist of a sequence of at least one Certificate Revocation
	/// List. Each EncapsulatedCRLValue will contain the base64 encoding of a
	/// DER-encoded X509 CRL.
	/// </summary>
	public class CRLValue : EncapsulatedPKIData
	{
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public CRLValue()
		{
			this.TagName = "EncapsulatedCRLValue";
		}
		#endregion
	}
}
