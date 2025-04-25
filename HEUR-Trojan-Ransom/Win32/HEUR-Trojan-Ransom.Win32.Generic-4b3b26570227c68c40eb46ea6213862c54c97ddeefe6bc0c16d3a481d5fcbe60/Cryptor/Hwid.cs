using System;
using System.Security.Cryptography;
using System.Text;

namespace Cryptor;

internal class Hwid
{
	public static string HWID()
	{
		try
		{
			return GetHash(Environment.CurrentManagedThreadId + Environment.UserName + Environment.MachineName + Environment.OSVersion.VersionString + Environment.SystemPageSize);
		}
		catch
		{
			return "Error HWID";
		}
	}

	public static string GetHash(string strToHash)
	{
		using MD5 mD = MD5.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(strToHash);
		return BitConverter.ToString(mD.ComputeHash(bytes), 0, 10).Replace("-", "").ToUpper();
	}
}
