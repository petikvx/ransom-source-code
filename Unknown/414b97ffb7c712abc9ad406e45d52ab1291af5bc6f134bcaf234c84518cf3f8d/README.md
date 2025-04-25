
# 🔒 Analysis of a C# Malware Disabling User Tools and Encrypting Files (Ransomware)

## 🧩 Introduction

This malware analysis compiles three components written in **C#**, each contributing to a typical ransomware attack lifecycle:

1. Disables essential system tools to hinder user intervention.
2. Encrypts user files and sabotages backup mechanisms.
3. Alters the desktop wallpaper to visually notify the user of the compromise.

The malware is built using the .NET framework and exhibits behavior consistent with ransomware seen in the wild, though lacking obfuscation or advanced stealth features.

---

## 🚨 General Behavior

This malware performs the following actions:

- Disables Task Manager and Registry Editor.
- Encrypts a wide range of file types in key user directories.
- Deletes system backups and disables recovery mechanisms.
- Drops a file to the desktop as a signal.
- Modifies the user's wallpaper, likely to display a ransom note or a visual marker.

---

## 🔍 Technical Analysis

### 🛑 Function: `Main2` – Disabling Task Manager & Registry Editor

```csharp
RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
registryKey.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);
registryKey.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
```

- Modifies:
  - `HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableTaskMgr`
  - `HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableRegistryTools`

---

### 🧨 Function: `Main1` – Ransomware Payload

- AES encryption with hardcoded key.
- Recursively encrypts files with these **84 file extensions**:

```
.7z, .7-zip, .accdb, .ace, .apk, .arj, .asp, .aspx, .avi, .backup,
.bak, .bay, .bmp, .bz2, .cab, .cer, .contact, .core, .cpp, .crt,
.cs, .css, .csv, .dat, .db, .dll, .doc, .docx, .dwg, .exif,
.flv, .gzip, .htm, .html, .ibank, .ico, .ini, .iso, .jar, .java,
.jpe, .jpeg, .jpg, .js, .lnk, .lzh, .m4a, .mdb, .mkv, .mov,
.mp3, .mp4, .mpeg, .mpg, .odt, .p7c, .pas, .pdb, .pdf, .php,
.png, .ppt, .pptx, .psd, .py, .rar, .rb, .rtf, .settings, .sie,
.sql, .sum, .tar, .txt, .wallet, .wma, .wmv, .xls, .xlsb, .xlsm,
.xlsx, .xml, .xz, .zip
```

- Targeted Directories include:
  - Desktop, Documents, Pictures, Downloads, Music, Videos, OneDrive, etc.
  - Both user-specific and shared/public folders.

---

### 🧹 Function: `ExecuteDestructiveCommands`

- Executes:
  - `vssadmin delete shadows /all /quiet`
  - `wmic shadowcopy delete`
  - `bcdedit /set {default} recoveryenabled no`
  - `bcdedit /set {default} bootstatuspolicy ignoreallfailures`
  - `wbadmin delete catalog -quiet`

These commands disable Windows recovery and remove backup copies, ensuring files can't be restored easily.

---

### 🧾 Function: `CreateFileOnDesktop`

- Drops a file `dead.txt` on the user's Desktop with content `"прив"`.

---

### 🖼️ Function: `Main` – Changing the Desktop Wallpaper

- Decodes a base64 image and saves it as `tempImage.jpg`.
- Uses `SystemParametersInfo` API to set the wallpaper:
  - Flags: `SPIF_UPDATEINIFILE | SPIF_SENDCHANGE`
  - Action: `SPI_SETDESKWALLPAPER`

This is often used to display ransom notes visually on the user's desktop.

---

## 🛠️ Techniques Used

- **Defensive Evasion**: Disabling Task Manager and Regedit.
- **Ransomware Behavior**: In-place AES encryption on sensitive files.
- **Anti-Recovery**: Removes shadow copies and disables recovery tools.
- **Psychological Warfare**: Custom wallpaper and dropped file.
- **No Network Communication**: No C2, no ransom mechanism in current version.

---

## ✅ Conclusion

This malware represents a complete ransomware prototype, simulating real-world attack behaviors with effective disruption capabilities. It can cause significant data loss and user stress.

---

## 🧾 Indicators (IOCs)

### 🔑 Registry Keys Modified

- `HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableTaskMgr`
- `HKCU\Software\Microsoft\Windows\CurrentVersion\Policies\System\DisableRegistryTools`

### 🧬 File Extensions Targeted (84 total)

- `.7z`, `.7-zip`, `.accdb`, `.ace`, `.apk`, `.arj`, `.asp`, `.aspx`, `.avi`, `.backup`
- `.bak`, `.bay`, `.bmp`, `.bz2`, `.cab`, `.cer`, `.contact`, `.core`, `.cpp`, `.crt`
- `.cs`, `.css`, `.csv`, `.dat`, `.db`, `.dll`, `.doc`, `.docx`, `.dwg`, `.exif`
- `.flv`, `.gzip`, `.htm`, `.html`, `.ibank`, `.ico`, `.ini`, `.iso`, `.jar`, `.java`
- `.jpe`, `.jpeg`, `.jpg`, `.js`, `.lnk`, `.lzh`, `.m4a`, `.mdb`, `.mkv`, `.mov`
- `.mp3`, `.mp4`, `.mpeg`, `.mpg`, `.odt`, `.p7c`, `.pas`, `.pdb`, `.pdf`, `.php`
- `.png`, `.ppt`, `.pptx`, `.psd`, `.py`, `.rar`, `.rb`, `.rtf`, `.settings`, `.sie`
- `.sql`, `.sum`, `.tar`, `.txt`, `.wallet`, `.wma`, `.wmv`, `.xls`, `.xlsb`, `.xlsm`
- `.xlsx`, `.xml`, `.xz`, `.zip`

### 📁 Files Dropped

- `dead.txt` with content `"прив"`
- Temporary wallpaper image (`tempImage.jpg` or `Wallpaper.jpg`)

### 🧨 Commands Executed

- `vssadmin delete shadows /all /quiet`
- `wmic shadowcopy delete`
- `bcdedit /set recoveryenabled no`
- `bcdedit /set bootstatuspolicy ignoreallfailures`
- `wbadmin delete catalog -quiet`

---

