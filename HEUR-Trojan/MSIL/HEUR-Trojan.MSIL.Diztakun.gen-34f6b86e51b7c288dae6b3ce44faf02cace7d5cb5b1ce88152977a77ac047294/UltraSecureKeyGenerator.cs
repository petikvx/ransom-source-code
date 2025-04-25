using System;
using System.Security.Cryptography;
using System.Text;

public class UltraSecureKeyGenerator
{
	private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

	private static readonly (int Start, int End)[] unicodeRanges = new(int, int)[9]
	{
		(33, 126),
		(913, 969),
		(1024, 1279),
		(1536, 1791),
		(2304, 2431),
		(12352, 12447),
		(12448, 12543),
		(19968, 40959),
		(65280, 65519)
	};

	public static string CreatePassword(int length)
	{
		if (length < 4094)
		{
			length = 4094;
		}
		StringBuilder stringBuilder = new StringBuilder(length);
		byte[] array = new byte[8];
		for (int i = 0; i < length - 128; i++)
		{
			rng.GetBytes(array);
			ulong num = BitConverter.ToUInt64(array, 0);
			int num2 = (int)(num % (ulong)unicodeRanges.Length);
			(int, int) tuple = unicodeRanges[num2];
			rng.GetBytes(array);
			num = BitConverter.ToUInt64(array, 0);
			int utf = tuple.Item1 + (int)(num % (ulong)(tuple.Item2 - tuple.Item1 + 1));
			stringBuilder.Append(char.ConvertFromUtf32(utf));
		}
		string value = GenerateComplexSalt();
		stringBuilder.Append(value);
		return MultiLayerShuffle(stringBuilder.ToString());
	}

	private static string GenerateComplexSalt()
	{
		byte[] array = new byte[128];
		rng.GetBytes(array);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Convert.ToBase64String(array));
		stringBuilder.Append(DateTime.UtcNow.Ticks);
		stringBuilder.Append(Environment.TickCount);
		stringBuilder.Append(DateTime.Now.Millisecond);
		byte[] array2 = new byte[32];
		rng.GetBytes(array2);
		stringBuilder.Append(Convert.ToBase64String(array2));
		stringBuilder.Append(Environment.ProcessorCount);
		stringBuilder.Append(Environment.WorkingSet);
		return stringBuilder.ToString();
	}

	private static string MultiLayerShuffle(string input)
	{
		char[] array = input.ToCharArray();
		byte[] array2 = new byte[8];
		for (int i = 0; i < 7; i++)
		{
			for (int num = array.Length - 1; num > 0; num--)
			{
				rng.GetBytes(array2);
				ulong num2 = BitConverter.ToUInt64(array2, 0);
				int num3 = (int)(num2 % (ulong)(num + 1));
				char c = array[num];
				array[num] = array[num3];
				array[num3] = c;
				if (i % 2 == 0)
				{
					array[num] = (char)(array[num] ^ (ushort)(num2 & 0x1F));
				}
				if (i % 3 == 0)
				{
					array[num3] = (char)(array[num3] ^ (ushort)((num2 >> 32) & 0x1F));
				}
			}
			if (i % 2 == 0)
			{
				Array.Reverse((Array)array, 0, array.Length);
			}
		}
		return new string(array);
	}
}
