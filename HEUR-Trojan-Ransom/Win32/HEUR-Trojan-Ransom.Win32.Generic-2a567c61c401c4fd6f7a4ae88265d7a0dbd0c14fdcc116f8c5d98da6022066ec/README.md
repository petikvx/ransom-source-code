
# 🔐 Malware Analysis Report

---

## 🧾 Introduction

This sample is a **.NET-based ransomware** named tentatively as **FBIRAS Locker**, referencing the extension `.FBIRAS` added to encrypted files. The code is heavily obfuscated by design and demonstrates full ransomware behavior including encryption, persistence, process name spoofing, and ransom note drop.  

- **Platform**: Windows (.NET/C#)
- **Sample Type**: Ransomware
- **File extension used**: `.FBIRAS`
- **Ransom note**: `Readme.txt`
- **Public RSA Key Detected**: Yes
- **Suggested VT link**:  
`https://www.virustotal.com/gui/file/<SHA256_HASH_HERE>`

---

## 🧠 General Behavior

Upon execution, the malware:
- Ensures only one instance via a `Mutex`
- Attempts to clone itself under the name `Runtime Broker.exe`
- Registers itself in Windows startup (`HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Run`)
- Scans drives and folders, encrypts files selectively
- Drops a ransom note in each affected directory
- Draws a threatening ransom message as the desktop wallpaper
- Deletes shadow copies and backup catalogs to prevent recovery

---

## 🛠️ Technical Analysis

### Function: `Main`
```csharp
new Mutex(initiallyOwned: true, Environment.MachineName, out var createdNew);
if (!createdNew) Environment.Exit(0);
```
Ensures only one instance runs using a mutex with machine-specific name.

```csharp
if (CHANGE_PROCESS_NAME != "") { COPY_FILE(CHANGE_PROCESS_NAME); }
```
Copies itself under a disguise name, e.g., `Runtime Broker.exe`.

```csharp
STARTUP();
```
Adds a registry run key for persistence.

```csharp
LOOK_FOR_EXTENSIONS or LOOK_FOR_EXCEPTIONS
```
Recursively encrypts files across drives, filtering extensions as configured.

---

## 🧪 Techniques Used

### 💾 **Persistence**
```csharp
Registry.CurrentUser.OpenSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run")
```
Sets itself to launch at Windows startup.

### 🎭 **Obfuscation / Process Masquerading**
```csharp
CHANGE_PROCESS_NAME = "Runtime Broker.exe";
```
Disguises as a legitimate Windows process name.

### 🔐 **Encryption / Ransomware Logic**
Uses AES-256 encryption (via `RijndaelManaged`) with CBC mode. Keys are:
- Randomly generated per file
- Appended RSA-encrypted at file end

#### File < 512KB → Full encryption
#### File ≥ 512KB → Triple partial encryption (start, middle, end)

```csharp
ENCRYPT_DATA → RijndaelManaged encryption
FULL_ENCRYPT / TRIPLE_ENCRYPT → file encryption logic
RSA_ENCRYPT → encrypts key+IV with RSA_PUBLIC_KEY
```

### 📄 **Ransom Note Drop**
```csharp
File.WriteAllText(path2, TEXT_MESSAGE);
```
Writes `Readme.txt` containing instructions and Telegram contact (`@Lawinfo19`).

### 🧹 **Backup Deletion**
```csharp
vssadmin delete shadows /all /quiet
wbadmin delete catalog -quiet
```
Deletes Windows shadow copies and backup history.

### 🧼 **Recycle Bin Wipe**
```csharp
SHEmptyRecycleBin(IntPtr.Zero, null, 7);
```
Empties recycle bin silently.

---

## 🔐 In-depth Cryptographic Analysis

### Encryption steps
1. Generate random 32-byte key and 16-byte IV
2. Encrypt data with AES-CBC (`RijndaelManaged`)
3. RSA encrypts key + IV string and appends to file

### Algorithm used
- **Symmetric**: AES-256 (CBC mode)
- **Asymmetric**: RSA (2048-bit modulus inferred)

### Key management
- **Per file**: key/IV are newly generated
- **Stored**: appended as RSA-encrypted blob at file end

### Initialization Vector
- 16 random ASCII characters
- Correctly used in CBC mode

### Libraries used
- `System.Security.Cryptography.RijndaelManaged`
- `System.Security.Cryptography.RSACryptoServiceProvider`

### Recoverability assessment
- **Private key missing**: decryption without ransom is impossible
- **Strong crypto**: AES-256 + RSA (no obvious weaknesses)

### Ransom note content (partial)
```
All Your files have been locked with ransomware by law enforcement...
Telegram = @Lawinfo19
You need us to pay a amount for your criminal activity...
```

### Encrypted file extension
`.FBIRAS`

---

## 🕒 Execution Timeline

```md
1. Ensures only one instance via mutex
2. Copies itself as "Runtime Broker.exe"
3. Registers itself for startup via registry
4. Recursively scans and encrypts files
5. Drops ransom note as Readme.txt
6. Changes desktop wallpaper with threat
7. Terminates various processes (e.g., Word, SQL)
8. Deletes shadow copies and backups
9. Empties recycle bin
```

---

## ✅ Conclusion

- **Objectives**: Encrypt files and extort ransom under guise of fake law enforcement
- **Danger level**: 🚨 **Critical** – destructive, data-encrypting, and persistent
- **Recommendations**:
  - Block Telegram handle `@Lawinfo19`
  - Implement file backup strategies
  - Monitor registry and unusual process names like `Runtime Broker.exe`
  - Create **YARA** rules for `.FBIRAS`, ransom note patterns, RSA blob endings

---

## 🧷 IOCs (Indicators of Compromise)

| Type        | Value                             |
|-------------|------------------------------------|
| File Name   | `Readme.txt`                      |
| Extension   | `.FBIRAS`                         |
| Registry Key| `HKCU\Software\Microsoft\...\Run` |
| Telegram    | `@Lawinfo19`                      |
| Process Name| `Runtime Broker.exe`              |
| RSA Key     | Present (public only)             |
