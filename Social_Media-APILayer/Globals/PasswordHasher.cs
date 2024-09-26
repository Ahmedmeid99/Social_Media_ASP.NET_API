using System.Security.Cryptography;
using System.Text;

namespace Social_Media_APILayer.Globals
{
	public class PasswordHasher
	{
		public static string HashingPassword(string Password)
		{
			using (SHA256 sHA256 = SHA256.Create())
			{

				byte[] hashBytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(Password));
				return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}
		public bool VerifyPassword(string hashedPassword, string providedPassword)
		{
			return hashedPassword == HashingPassword(providedPassword);
		}

	}
}
