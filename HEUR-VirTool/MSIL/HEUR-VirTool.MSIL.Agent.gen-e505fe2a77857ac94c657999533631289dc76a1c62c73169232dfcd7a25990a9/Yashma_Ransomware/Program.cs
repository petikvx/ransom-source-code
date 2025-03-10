using System;
using System.Windows.Forms;
using CustomWindowsForm;

namespace Yashma_Ransomware;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run((Form)(object)new BlackForm());
	}
}
