using System;
using System.Linq;

namespace DualShot;

internal class DSEncryption
{
	private static Random r = new Random();

	public static byte[] Encrypt(byte[] array, byte[] pbkey)
	{
		byte[] array2 = (byte[])array.Clone();
		int num = 0;
		for (int i = 0; i < array2.Length; i++)
		{
			try
			{
				array2[i] += pbkey[num];
			}
			catch
			{
			}
			num++;
			if (num > pbkey.Length)
			{
				num = 0;
			}
		}
		return array2;
	}

	public static byte[] Decrypt(byte[] array, byte[] pvkey)
	{
		byte[] array2 = (byte[])array.Clone();
		pvkey.Reverse();
		bool flag = false;
		byte[] array3 = new byte[0];
		foreach (byte b in pvkey)
		{
			Array.Resize(ref array3, array3.Length + 1);
			if (flag)
			{
				flag = false;
				array3[array3.GetUpperBound(0)] = b;
			}
			else
			{
				flag = true;
				array3[array3.GetUpperBound(0)] = (byte)(b - 1);
			}
		}
		int num = 0;
		for (int j = 0; j < array2.Length; j++)
		{
			try
			{
				array2[j] -= array3[num];
			}
			catch
			{
			}
			num++;
			if (num > array3.Length)
			{
				num = 0;
			}
		}
		return array2;
	}

	public static Tuple<byte[], byte[]> GenerateKeys(int length, int vlength)
	{
		byte[] array = new byte[0];
		for (int i = 0; i < length; i++)
		{
			Array.Resize(ref array, array.Length + 1);
			array[array.GetUpperBound(0)] = (byte)r.Next(1, vlength);
		}
		byte[] array2 = new byte[0];
		bool flag = false;
		byte[] array3 = array;
		foreach (byte b in array3)
		{
			Array.Resize(ref array2, array2.Length + 1);
			if (flag)
			{
				flag = false;
				array2[array2.GetUpperBound(0)] = b;
			}
			else
			{
				flag = true;
				array2[array2.GetUpperBound(0)] = (byte)(b + 1);
			}
		}
		return Tuple.Create(array, array2);
	}
}
