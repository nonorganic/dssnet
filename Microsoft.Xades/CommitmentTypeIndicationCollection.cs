// ACommitmentTypeIndicationCollection.cs
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
using System.Collections;

namespace Microsoft.Xades
{
	/// <summary>
	/// Collection class that derives from ArrayList.  It provides the minimally
	/// required functionality to add instances of typed classes and obtain typed
	/// elements through a custom indexer.
	/// </summary>
	public class CommitmentTypeIndicationCollection : ArrayList
	{
		/// <summary>
		/// New typed indexer for the collection
		/// </summary>
		/// <param name="index">Index of the object to retrieve from collection</param>
		public new CommitmentTypeIndication this[int index]
		{
			get
			{
				return (CommitmentTypeIndication)base[index];
			}
			set
			{
				base[index] = value;
			}
		}

		/// <summary>
		/// Add typed object to the collection
		/// </summary>
		/// <param name="objectToAdd">Typed object to be added to collection</param>
		/// <returns>The object that has been added to collection</returns>
		public CommitmentTypeIndication Add(CommitmentTypeIndication objectToAdd)
		{
			base.Add(objectToAdd);

			return objectToAdd;
		}

		/// <summary>
		/// Add new typed object to the collection
		/// </summary>
		/// <returns>The newly created object that has been added to collection</returns>
		public CommitmentTypeIndication Add()
		{
			return this.Add(new CommitmentTypeIndication());
		}
	}
}