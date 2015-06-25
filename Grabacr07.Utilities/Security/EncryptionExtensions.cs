using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Grabacr07.Utilities.Security
{
	/// <summary>
	/// データの暗号化を行う静的メソッドを提供します。
	/// </summary>
	public static class EncryptionExtensions
	{
		/// <summary>
		/// AES アルゴリズムを使用して、現在の文字列を暗号化します。
		/// </summary>
		/// <param name="source">暗号化する文字列。</param>
		/// <param name="password">暗号化に使用するパスワード。</param>
		/// <returns>暗号化された文字列。</returns>
		public static string Encrypt(this string source, string password)
		{
			var result = "";

			using (var provider = new AesCryptoServiceProvider())
			{
				provider.SetPassword(password + CommonDefinitions.EncryptKey);

				using (var encryptor = provider.CreateEncryptor())
				{
					var target = Encoding.UTF8.GetBytes(source);
					result = Convert.ToBase64String(encryptor.TransformFinalBlock(target, 0, target.Length));
				}
			}

			return result;
		}

		/// <summary>
		/// AES アルゴリズムを使用して、暗号化された現在の文字列を複合化します。
		/// </summary>
		/// <param name="source">複合化する文字列。</param>
		/// <param name="password">複合化使用するパスワード。</param>
		/// <returns>複合化された文字列。</returns>
		public static string Decrypt(this string source, string password)
		{
			var result = "";

			using (var provider = new AesCryptoServiceProvider())
			{
				provider.SetPassword(password + CommonDefinitions.EncryptKey);

				using (var decryptor = provider.CreateDecryptor())
				{
					var target = Convert.FromBase64String(source);
					result = Encoding.UTF8.GetString(decryptor.TransformFinalBlock(target, 0, target.Length));
				}
			}

			return result;
		}


		/// <summary>
		/// AES アルゴリズム プロバイダーの現在のインスタンスに対し、対称キーおよび初期化ベクターを設定します。
		/// </summary>
		/// <param name="provider">AES アルゴリズム プロバイダー。</param>
		/// <param name="password">対称キーおよび初期ベクターの派生に使用するパスワード。</param>
		private static void SetPassword(this AesCryptoServiceProvider provider, string password)
		{
			provider.SetPassword(password, Encoding.UTF8.GetBytes("Patchouli Knowledge"), 1730);
		}
		private static void SetPassword(this AesCryptoServiceProvider provider, string password, byte[] salt, int iterations)
		{
			var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations);

			provider.Key = deriveBytes.GetBytes(provider.KeySize / 8);
			provider.IV = deriveBytes.GetBytes(provider.BlockSize / 8);
		}
	}
}
