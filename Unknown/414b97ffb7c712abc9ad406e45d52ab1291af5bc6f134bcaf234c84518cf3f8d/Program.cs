using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

internal class Program
{
	private static readonly int SPIF_SENDCHANGE;

	private static int SPIF_UPDATEINIFILE;

	public static int SPI_SETDESKWALLPAPER { get; private set; }

	private static void Main()
	{
		string s = "/9j/4AAQSkZJRgABAQEASABIAAD/2wBDAAICAgICAQICAgIDAgIDAwYEAwMDAwcFBQQGCAcJCAgHC";
		byte[] bytes = Convert.FromBase64String(s);
		string text = Path.Combine(Path.GetTempPath(), "tempImage.jpg");
		File.WriteAllBytes(text, bytes);
		SetWallpaper(text);
	}

	private static void SetWallpaper(string imagePath)
	{
		if (File.Exists(imagePath))
		{
			Image image = Image.FromFile(imagePath);
			image.Dispose();
			string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Wallpaper.jpg");
			image.Save(text, Imaging.ImageFormat);
			SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, text, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
			image.Dispose();
			SystemParametersInfo(20, 0, text, 3);
		}
	}

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
}
