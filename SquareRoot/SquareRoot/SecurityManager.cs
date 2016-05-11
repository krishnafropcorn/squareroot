using System;

namespace SquareRoot
{
	public class SecurityManager : ISecurityManager
	{
		private const string Acr35EncryptionKeyKey = "Acr35EncryptionKey";

		public string Acr35EncryptionKey {
			get {
				return "abcdefgh";
			}
		}
	}
}

