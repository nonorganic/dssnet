using Org.BouncyCastle.Crypto;

//jbonilla
namespace EU.Europa.EC.Markt.Dss
{
    /// <summary>
    /// AsymmetricKeyParameter for Pre-computed signature
    /// </summary>
    public class NullPrivateKey : AsymmetricKeyParameter
    {
        public NullPrivateKey()
            : base(true)
        {
        }

        public override string ToString()
        {
            return "NULL";
        }
    }
}
