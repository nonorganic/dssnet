using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;

//jbonilla
namespace Org.BouncyCastle.Crypto.Signers
{
	/// <summary>Signer using a provided pre-computed signature, used by DSS</summary>	
    public class PreComputedSigner : ISigner
	{
		private byte[] PreComputedSignature { get; set; }
        private IDigest digest;
        //jbonilla
        private byte[] currentSignature;

		/// <param name="preComputedSignature">the preComputedSignature to set</param>
		public PreComputedSigner(byte[] preComputedSignature)
		{
			this.PreComputedSignature = preComputedSignature;            
            this.digest = new NullDigest();
		}

		/// <summary>The default constructor for PreComputedSigner.</summary>
		/// <remarks>The default constructor for PreComputedSigner.</remarks>
		/// <param name="algorithmName"></param>
        public PreComputedSigner()
            : this(new byte[0])
        {
        }

        public string AlgorithmName
        {
            get { return "NONE"; }
        }        

        public void Init(bool forSigning, ICipherParameters parameters)
        {
            Reset();            
        }

        public void Update(byte input)
        {
            digest.Update(input);
        }

        public void BlockUpdate(byte[] input, int inOff, int length)
        {
            digest.BlockUpdate(input, inOff, length);
        }

        public byte[] GenerateSignature()
        {
            if (PreComputedSignature.Length > 0)
            {
                currentSignature = PreComputedSignature;
                return PreComputedSignature;
            }
            else
            {
                byte[] hash = new byte[digest.GetDigestSize()];
                digest.DoFinal(hash, 0);
                //jbonilla
                currentSignature = hash;
                return currentSignature;
            }
        }

        //jbonilla
        public byte[] CurrentSignature()
        {
            return currentSignature;
        }

        public bool VerifySignature(byte[] signature)
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            //jbonilla
            currentSignature = null;
            digest.Reset();
        }        
    }
}
