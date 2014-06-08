/*
 * DSS - Digital Signature Services
 *
 * Copyright (C) 2011 European Commission, Directorate-General Internal Market and Services (DG MARKT), B-1049 Bruxelles/Brussel
 *
 * Developed by: 2011 ARHS Developments S.A. (rue Nicolas BovÃ© 2B, L-1253 Luxembourg) http://www.arhs-developments.com
 *
 * This file is part of the "DSS - Digital Signature Services" project.
 *
 * "DSS - Digital Signature Services" is free software: you can redistribute it and/or modify it under the terms of
 * the GNU Lesser General Public License as published by the Free Software Foundation, either version 2.1 of the
 * License, or (at your option) any later version.
 *
 * DSS is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License along with
 * "DSS - Digital Signature Services".  If not, see <http://www.gnu.org/licenses/>.
 */

//jbonilla Pkcs11 no se implementará
/*
using System;
using System.Collections.Generic;
using System.IO;
using EU.Europa.EC.Markt.Dss;
using EU.Europa.EC.Markt.Dss.Signature.Token;
//using Javax.Security.Auth.Callback;
using Org.BouncyCastle.Asn1.X509;
using Sharpen;
//using Sun.Security.Pkcs11;
//using Sun.Security.Pkcs11.Wrapper;

namespace EU.Europa.EC.Markt.Dss.Signature.Token
{
	/// <summary>PKCS11 token with callback</summary>
	/// <version>$Revision: 1887 $ - $Date: 2013-04-23 14:56:09 +0200 (mar., 23 avr. 2013) $
	/// 	</version>
	public class Pkcs11SignatureToken : AsyncSignatureTokenConnection
	{
		private Provider _pkcs11Provider;

		private string pkcs11Path;

		private KeyStore _keyStore;

		private readonly PasswordInputCallback callback;

		/// <summary>Create the SignatureTokenConnection, using the provided path for the library.
		/// 	</summary>
		/// <remarks>Create the SignatureTokenConnection, using the provided path for the library.
		/// 	</remarks>
		/// <param name="pkcs11Path"></param>
		public Pkcs11SignatureToken(string pkcs11Path) : this(pkcs11Path, (PasswordInputCallback
			)null)
		{
		}

		/// <summary>
		/// Create the SignatureTokenConnection, using the provided path for the library and a way of retrieving the password
		/// from the user.
		/// </summary>
		/// <remarks>
		/// Create the SignatureTokenConnection, using the provided path for the library and a way of retrieving the password
		/// from the user. The default constructor for CallbackPkcs11SignatureToken.
		/// </remarks>
		/// <param name="pkcs11Path"></param>
		/// <param name="callback"></param>
		public Pkcs11SignatureToken(string pkcs11Path, PasswordInputCallback callback)
		{
			this.pkcs11Path = pkcs11Path;
			this.callback = callback;
		}

		/// <summary>Sometimes, the password is known in advance.</summary>
		/// <remarks>
		/// Sometimes, the password is known in advance. This create a SignatureTokenConnection and the keys will be accessed
		/// using the provided password. The default constructor for CallbackPkcs11SignatureToken.
		/// </remarks>
		/// <param name="pkcs11Path"></param>
		/// <param name="password"></param>
		public Pkcs11SignatureToken(string pkcs11Path, char[] password) : this(pkcs11Path
			, new PrefilledPasswordCallback(password))
		{
		}

		private Provider GetProvider()
		{
			try
			{
				if (_pkcs11Provider == null)
				{
					string aPKCS11LibraryFileName = GetPkcs11Path();
					string pkcs11ConfigSettings = "name = SmartCard\n" + "library = " + aPKCS11LibraryFileName;
					byte[] pkcs11ConfigBytes = Sharpen.Runtime.GetBytesForString(pkcs11ConfigSettings
						);
					ByteArrayInputStream confStream = new ByteArrayInputStream(pkcs11ConfigBytes);
					SunPKCS11 pkcs11 = new SunPKCS11(confStream);
					_pkcs11Provider = (Provider)pkcs11;
					Sharpen.Security.AddProvider(_pkcs11Provider);
				}
				return _pkcs11Provider;
			}
			catch (ProviderException)
			{
				throw new ConfigurationException(ConfigurationException.MSG.NOT_PKCS11_LIB);
			}
		}

		/// <exception cref="Sharpen.KeyStoreException"></exception>
		/// <exception cref="EU.Europa.EC.Markt.Dss.ConfigurationException"></exception>
		private KeyStore GetKeyStore()
		{
			if (_keyStore == null)
			{
				_keyStore = KeyStore.GetInstance("PKCS11", GetProvider());
				try
				{
					_keyStore.Load(new _LoadStoreParameter_128(this));
				}
				catch (Exception e)
				{
					if (e is PKCS11Exception)
					{
						if ("CKR_PIN_INCORRECT".Equals(e.Message))
						{
							throw new BadPasswordException(BadPasswordException.MSG.PKCS11_BAD_PASSWORD);
						}
					}
					if (e is ConfigurationException)
					{
						throw (ConfigurationException)e;
					}
					throw new KeyStoreException("Can't initialize Sun PKCS#11 security " + "provider. Reason: "
						 + e.InnerException.Message, e);
				}
			}
			return _keyStore;
		}

		private sealed class _LoadStoreParameter_128 : KeyStore.LoadStoreParameter
		{
			public _LoadStoreParameter_128(Pkcs11SignatureToken _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public KeyStore.ProtectionParameter GetProtectionParameter()
			{
				return new KeyStore.CallbackHandlerProtection(new _CallbackHandler_132(this));
			}

			private sealed class _CallbackHandler_132 : CallbackHandler
			{
				public _CallbackHandler_132(_LoadStoreParameter_128 _enclosing)
				{
					this._enclosing = _enclosing;
				}

				/// <exception cref="System.IO.IOException"></exception>
				/// <exception cref="Javax.Security.Auth.Callback.UnsupportedCallbackException"></exception>
				public void Handle(Javax.Security.Auth.Callback.Callback[] callbacks)
				{
					foreach (Javax.Security.Auth.Callback.Callback c in callbacks)
					{
						if (c is PasswordCallback)
						{
							((PasswordCallback)c).SetPassword(this._enclosing._enclosing.callback.GetPassword
								());
							return;
						}
					}
					throw new RuntimeException("No password callback");
				}

				private readonly _LoadStoreParameter_128 _enclosing;
			}

			private readonly Pkcs11SignatureToken _enclosing;
		}

		private string GetPkcs11Path()
		{
			return pkcs11Path;
		}

		public override void Close()
		{
			if (_pkcs11Provider != null)
			{
				try
				{
					Sharpen.Security.RemoveProvider(_pkcs11Provider.GetName());
				}
				catch (Exception ex)
				{
					Sharpen.Runtime.PrintStackTrace(ex);
				}
			}
			this._pkcs11Provider = null;
			this._keyStore = null;
		}

		/// <exception cref="Sharpen.NoSuchAlgorithmException"></exception>
		public override byte[] EncryptDigest(byte[] digestValue, DigestAlgorithm digestAlgo
			, DSSPrivateKeyEntry keyEntry)
		{
			try
			{
				DigestInfo digestInfo = new DigestInfo(digestAlgo.GetAlgorithmIdentifier(), digestValue
					);
				Sharpen.Cipher cipher = Sharpen.Cipher.GetInstance(keyEntry.GetSignatureAlgorithm
					().GetPadding());
				cipher.Init(Sharpen.Cipher.ENCRYPT_MODE, ((KSPrivateKeyEntry)keyEntry).GetPrivateKey
					());
				return cipher.DoFinal(digestInfo.GetDerEncoded());
			}
			catch (NoSuchPaddingException e)
			{
				throw new RuntimeException(e);
			}
			catch (InvalidKeyException e)
			{
				throw new RuntimeException(e);
			}
			catch (IllegalBlockSizeException e)
			{
				throw new RuntimeException(e);
			}
			catch (BadPaddingException)
			{
				// More likely bad password
				throw new BadPasswordException(BadPasswordException.MSG.PKCS11_BAD_PASSWORD);
			}
		}

		/// <exception cref="Sharpen.KeyStoreException"></exception>
		/// <exception cref="EU.Europa.EC.Markt.Dss.ConfigurationException"></exception>
		public override IList<DSSPrivateKeyEntry> GetKeys()
		{
			IList<DSSPrivateKeyEntry> list = new AList<DSSPrivateKeyEntry>();
			try
			{
				KeyStore keyStore = GetKeyStore();
				Enumeration<string> aliases = keyStore.Aliases();
				while (aliases.MoveNext())
				{
					string alias = aliases.Current;
					if (keyStore.IsKeyEntry(alias))
					{
						KeyStore.PrivateKeyEntry entry = (KeyStore.PrivateKeyEntry)keyStore.GetEntry(alias
							, null);
						list.AddItem(new KSPrivateKeyEntry(entry));
					}
				}
			}
			catch (Exception e)
			{
				if (e is ConfigurationException)
				{
					throw (ConfigurationException)e;
				}
				throw new KeyStoreException("Can't initialize Sun PKCS#11 security " + "provider. Reason: "
					 + (e.InnerException != null ? e.InnerException.Message : e.Message), e);
			}
			return list;
		}
	}
}
*/