
# 🛡️ Analyse complète du ver Email-Worm.Win32.Repah.a (TASM32)

## Introduction

Le fichier analysé est un code assembleur Win32 généré par IDA, issu d’un binaire PE ciblant la plateforme Windows (80386). Il est classé comme un **ver de messagerie**, c’est-à-dire un malware auto-répliquant via les messageries ou clients email. Il s'agit d'une souche nommée `Email-Worm.Win32.Repah.a`.

Le ver semble avoir été compilé avec **Visual C++**, ce qui explique la structure typique avec appels à des fonctions API Windows comme `GetWindowsDirectoryA`, `lstrcat`, ou `GetModuleHandleA`.

---

## 📋 Comportement général

Ce ver est conçu pour :

1. **Se copier dans le dossier Windows** sous un nom déguisé (`weather.txt.exe`)
2. **Modifier les fichiers `script.ini` de mIRC**, outil souvent utilisé dans les vers pour propager des liens malveillants.
3. **Créer un script VBS** (`mail.vbs`) destiné à l’auto-exécution.
4. **Insérer des commandes d’exécution dans les fichiers cibles**, pour se relancer ou se propager.

---

## 🔍 Analyse technique

### 📁 Copie du ver dans le dossier Windows

```asm
push    32h                             ; Taille du buffer
push    (offset Buffer+77h)            ; Adresse de destination
call    GetWindowsDirectoryA           ; Remplit le buffer avec le chemin Windows
push    offset aWeatherTxtExe          ; "\weather.txt.exe"
push    (offset Buffer+77h)
call    lstrcat                        ; Concatène pour créer le chemin complet
```

➡️ Cette séquence construit le chemin complet `C:\Windows\weather.txt.exe`, où le malware va se copier.

---

### 🪝 Infection des fichiers mIRC

```asm
mov     ebx, offset aCMircScriptIni    ; "c:\mirc\script.ini"
call    sub_EcrireScriptIni
mov     ebx, offset aDMircScriptIni    ; "d:\mirc\script.ini"
call    sub_EcrireScriptIni
```

📌 La fonction `sub_EcrireScriptIni` (anciennement `sub_40108A`) écrit une commande dans `script.ini` pour exécuter le malware à l’ouverture d’une session IRC.

---

### 🧬 Création d’un script mail.vbs

```asm
mov     ebx, offset File               ; "c:\mail.vbs"
call    sub_CreerScriptVBS
```

➡️ Fonction `sub_CreerScriptVBS` (anciennement `sub_4010A3`) génère un fichier Visual Basic Script destiné à exécuter ou propager le malware.

---

### 📂 Fonctions internes renommées

- `sub_40108A` → `sub_EcrireScriptIni`
- `sub_4010A3` → `sub_CreerScriptVBS`

---

### 🧾 Fonction `sub_EcrireScriptIni` – Infection des scripts mIRC

```asm
push    0               ; iAttribute
push    ebx             ; lpPathName (ex: c:\mirc\script.ini)
call    _lcreat
push    0A9h            ; Longueur du contenu (169 octets)
push    offset Buffer   ; Contenu à écrire : commandes mIRC
push    eax             ; Handle du fichier
call    _lwrite
```

📌 Le buffer contient probablement un script mIRC qui exécute le fichier `weather.txt.exe`.

---

### 🖋️ Fonction `sub_CreerScriptVBS` – Génération de `mail.vbs`

```asm
push    0               ; iAttribute
push    ebx             ; lpPathName (ex: c:\mail.vbs)
call    _lcreat
push    45Eh            ; Longueur du script VBS
push    offset aOnErrorResumeN ; Contenu VBS
push    eax             ; Handle
call    _lwrite
```

🔍 Le contenu `aOnErrorResumeN` commence par :
```vbs
On Error Resume Next
Dim weather, Ma...
```

---

## ✅ Conclusion et détection

Le ver utilise des techniques classiques mais efficaces :

- Manipulation directe de fichiers (`_lcreat`, `_lwrite`)
- Infection de clients IRC
- Génération de scripts VBS pour persistance
- Déguisement de l'exécutable (`weather.txt.exe`)

### 🛡️ Recommandations

- Bloquer les extensions `.vbs`, `.exe` déguisées
- Surveiller `c:\mirc\script.ini`
- Éviter les clients IRC non surveillés
- Activer la détection heuristique

### 🧪 Signatures

- **SHA256** : `CBF73952DB8E199C18BF5076D048E9FDC68EF27B2216A856486495F8BA96EBBF`
- **MD5** : `769457A0F5B15B1590F90E07900E3579`

---

Analyse réalisée par MalwareArticlesGPT.
