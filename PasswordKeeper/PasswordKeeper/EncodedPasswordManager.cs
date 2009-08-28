using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PasswordKeeper
{
	public class EncodedPasswordManager
	{
		private byte[] _masterPassword;

		

		public bool Authenticate(string masterPassword)
		{
			if (_masterPassword == Encoding.UTF8.GetBytes(masterPassword))
			{
				return true;
			}

			return false;
		}
	}

}
