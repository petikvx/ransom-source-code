# Malware Source Code for Research Purposes Only

## Warning

I am offering the source code of this malware strictly for research and educational purposes. It is intended to be used by security professionals, researchers, and students to study and understand its mechanisms in order to improve cybersecurity defenses. The use of this code for malicious purposes is explicitly prohibited. Any use of this code for malicious intent or illegal activities is strongly discouraged, not condoned, and forbidden. Please ensure compliance with all applicable laws and ethical guidelines when handling this material.

## Articles

[Analyze of Fabian Ransomware v3 (2025-03-15)](UDS-Trojan/Win32/UDS-Trojan.Win32.Agent.gen-7f3bfd0cc61218f8b5bff0850eb3cc9d5eadd7e735f9c0faf1224972c99e253b/README.md)

## Download samples with MWDB

```bash
ptkmwdb() {
    # Prompt the user to enter the search pattern
    echo "Please enter the search pattern (e.g., tag:yara:malware_win_chaos or tag:\"yara:malware_win_chaos\"):"
    read search_pattern

    # If the input doesn't start with "tag:", prepend it
    if [[ ! "$search_pattern" =~ ^tag: ]]; then
        search_pattern="tag:$search_pattern"
    fi

    # Ensure the pattern is properly quoted if it contains a colon after tag:
    if [[ "$search_pattern" =~ ^tag:.*:.* ]] && [[ ! "$search_pattern" =~ ^tag:\".*\"$ ]]; then
        formatted_pattern="\"$search_pattern\""
    else
        formatted_pattern="$search_pattern"
    fi

    # Debugging: Show the formatted pattern
    echo "Using pattern: $formatted_pattern"

    # Execute the mwdb search with the formatted pattern and process all results
    mwdb search "$formatted_pattern" -o short | while read t; do
        # Check if a file or directory containing the hash exists in the current directory and subdirectories
        if ! find . -type f -name "*$t*" -o -type d -name "*$t*" | grep -q .; then
            echo "Downloading $t..."
            mwdb fetch "$t"
        else
            echo "A file or directory containing $t already exists, skipping download."
        fi
    done
}
```

1.  Prompts the user for a search pattern (e.g., tag:yara:malware_win_chaos).
2.  Prepends tag: to the pattern if missing.
3.  Adds quotes around the pattern (e.g., "tag:yara:malware_win_chaos") if it contains a colon after tag: and isn’t already quoted.
4.  Displays the formatted pattern for debugging.
5.  Runs mwdb search with the pattern, retrieving results in short format.
6.  For each result (assumed to be a hash), checks if a file or directory containing that hash exists in the current directory and its subdirectories using find.
7.  Downloads the file with mwdb fetch if no match is found; otherwise, skips it with a message.

## Yara search from Blocks

Download block from VirusShare or vx-underground and search specific Yara Rules.

- `yara detection/yara/indicator_suspicious.yar -i INDICATOR_SUSPICIOUS_GENRansomware`
```yara
rule INDICATOR_SUSPICIOUS_GENRansomware {
    meta:
        description = "Detects command variations typically used by ransomware"
        author = "ditekSHen"
    strings:
        $cmd1 = "cmd /c \"WMIC.exe shadowcopy delet\"" ascii wide nocase
        $cmd2 = "vssadmin.exe Delete Shadows /all" ascii wide nocase
        $cmd3 = "Delete Shadows /all" ascii wide nocase
        $cmd4 = "} recoveryenabled no" ascii wide nocase
        $cmd5 = "} bootstatuspolicy ignoreallfailures" ascii wide nocase
        $cmd6 = "wmic SHADOWCOPY DELETE" ascii wide nocase
        $cmd7 = "\\Microsoft\\Windows\\SystemRestore\\SR\" /disable" ascii wide nocase
        $cmd8 = "resize shadowstorage /for=c: /on=c: /maxsize=" ascii wide nocase
        $cmd9 = "shadowcopy where \"ID='%s'\" delete" ascii wide nocase
        $cmd10 = "wmic.exe SHADOWCOPY /nointeractive" ascii wide nocase
        $cmd11 = "WMIC.exe shadowcopy delete" ascii wide nocase
        $cmd12 = "Win32_Shadowcopy | ForEach-Object {$_.Delete();}" ascii wide nocase
        $delr = /del \/s \/f \/q(( [A-Za-z]:\\(\*\.|[Bb]ackup))(VHD|bac|bak|wbcat|bkf)?)+/ ascii wide
        $wp1 = "delete catalog -quiet" ascii wide nocase
        $wp2 = "wbadmin delete backup" ascii wide nocase
        $wp3 = "delete systemstatebackup" ascii wide nocase
    condition:
        (uint16(0) == 0x5a4d and 2 of ($cmd*) or (1 of ($cmd*) and 1 of ($wp*)) or #delr > 4) or (4 of them)
}
```
- `yara signature-base/apt_olympic_destroyer.yar -i Destructive_Ransomware_Gen1`
```yara
rule Destructive_Ransomware_Gen1 {
   meta:
      description = "Detects destructive malware"
      license = "Detection Rule License 1.1 https://github.com/Neo23x0/signature-base/blob/master/LICENSE"
      author = "Florian Roth (Nextron Systems)"
      reference = "http://blog.talosintelligence.com/2018/02/olympic-destroyer.html"
      date = "2018-02-12"
      hash1 = "ae9a4e244a9b3c77d489dee8aeaf35a7c3ba31b210e76d81ef2e91790f052c85"
      id = "3a7ce55e-fb28-577b-91bb-fe02d7b3d73c"
   strings:
      $x1 = "/set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no" fullword wide
      $x2 = "delete shadows /all /quiet" fullword wide
      $x3 = "delete catalog -quiet" fullword wide
   condition:
      uint16(0) == 0x5a4d and filesize < 100KB and 1 of them
}
```
- `yara signature-base/yara/crime_ransom_generic.yar -i SUSP_RANSOMWARE_Indicator_Jul20`
```yara
rule SUSP_RANSOMWARE_Indicator_Jul20 {
   meta:
      description = "Detects ransomware indicator"
      author = "Florian Roth (Nextron Systems)"
      reference = "https://securelist.com/lazarus-on-the-hunt-for-big-game/97757/"
      date = "2020-07-28"
      score = 60
      hash1 = "52888b5f881f4941ae7a8f4d84de27fc502413861f96ee58ee560c09c11880d6"
      hash2 = "5e78475d10418c6938723f6cfefb89d5e9de61e45ecf374bb435c1c99dd4a473"
      hash3 = "6cb9afff8166976bd62bb29b12ed617784d6e74b110afcf8955477573594f306"
      id = "6036fdfd-8474-5d79-ac75-137ac2efdc77"
   strings:
      $ = "Decrypt.txt" ascii wide 
      $ = "DecryptFiles.txt" ascii wide
      $ = "Decrypt-Files.txt" ascii wide
      $ = "DecryptFilesHere.txt" ascii wide
      $ = "DECRYPT.txt" ascii wide 
      $ = "DecryptFiles.txt" ascii wide
      $ = "DECRYPT-FILES.txt" ascii wide
      $ = "DecryptFilesHere.txt" ascii wide
      $ = "DECRYPT_INSTRUCTION.TXT" ascii wide 
      $ = "FILES ENCRYPTED.txt" ascii wide
      $ = "DECRYPT MY FILES" ascii wide 
      $ = "DECRYPT-MY-FILES" ascii wide 
      $ = "DECRYPT_MY_FILES" ascii wide
      $ = "DECRYPT YOUR FILES" ascii wide  
      $ = "DECRYPT-YOUR-FILES" ascii wide 
      $ = "DECRYPT_YOUR_FILES" ascii wide 
      $ = "DECRYPT FILES.txt" ascii wide
   condition:
      uint16(0) == 0x5a4d and
      filesize < 1400KB and
      1 of them
}
```

```
petik@ams:signature-base$ find ./ -name "*ransom*yar"
./yara/crime_ransom_robinhood.yar
./yara/crime_ransom_ragna_locker.yar
./yara/crime_ryuk_ransomware.yar
./yara/crime_ransom_germanwiper.yar
./yara/apt_ransom_lockbit_citrixbleed_nov23.yar
./yara/crime_covid_ransom.yar
./yara/crime_ransom_darkside.yar
./yara/apt_ransom_vicesociety_dec22.yar
./yara/crime_mal_ransom_wadharma.yar
./yara/mal_ransom_lorenz.yar
./yara/crime_ransom_stealbit_lockbit.yar
./yara/crime_ransom_lockergoga.yar
./yara/crime_ransom_conti.yar
./yara/crime_ransom_venus.yar
./yara/crime_ransom_generic.yar
./yara/mal_inc_ransomware.yar
./yara/crime_ransom_prolock.yar
./yara/crime_ransom_revil.yar
./yara/crime_maze_ransomware.yar
./yara/crime_hermes_ransom.yar
./yara/crime_dearcry_ransom.yar
./yara/mal_ransom_esxi_attacks_feb23.yar
./yara/apt_lazarus_vhd_ransomware.yar
./yara/apt_ransom_darkbit_feb23.yar
```

```yara
rule COMBINED_RANSOMWARE_Detector {
   meta:
      description = "Detecte les indicateurs de ransomware, comportements destructeurs et commandes typiques"
      author = "Florian Roth (Nextron Systems), ditekSHen, adapté par [votre nom]"
      reference = "https://securelist.com/lazarus-on-the-hunt-for-big-game/97757/, http://blog.talosintelligence.com/2018/02/olympic-destroyer.html"
      date = "2025-03-11"
      score = 70
      id = "e8f4c2d1-9b5a-4e7b-8f9c-3a2d5e7f1b9c"
   strings:
      // Indicateurs de fichiers ransomware (SUSP_RANSOMWARE_Indicator_Jul20)
      $rf1 = "Decrypt.txt" ascii wide
      $rf2 = "DECRYPT_INSTRUCTION.TXT" ascii wide
      $rf3 = "FILES ENCRYPTED.txt" ascii wide
      $rf4 = "DECRYPT YOUR FILES" ascii wide
      $rf5 = "DECRYPT-MY-FILES" ascii wide

      // Commandes destructrices (Destructive_Ransomware_Gen1)
      $dc1 = "delete shadows /all /quiet" fullword wide
      $dc2 = "/set {default} bootstatuspolicy ignoreallfailures" fullword wide
      $dc3 = "delete catalog -quiet" fullword wide

      // Commandes typiques de ransomware (INDICATOR_SUSPICIOUS_GENRansomware)
      $rc1 = "vssadmin.exe Delete Shadows /all" ascii wide nocase
      $rc2 = "wmic SHADOWCOPY DELETE" ascii wide nocase
      $rc3 = "} recoveryenabled no" ascii wide nocase
      $rc4 = "wbadmin delete backup" ascii wide nocase
      $rc5 = "resize shadowstorage /for=c: /on=c: /maxsize=" ascii wide nocase
      $rc6 = /del \/s \/f \/q(( [A-Za-z]:\\(\*\.|[Bb]ackup))(VHD|bac|bak|wbcat|bkf)?)+/ ascii wide

   condition:
      uint16(0) == 0x5a4d and // Signature PE (fichier exécutable Windows)
      filesize < 1400KB and   // Limite de taille pour inclure les cas les plus courants
      (
         1 of ($rf*) or       // Au moins un indicateur de fichier ransomware
         1 of ($dc*) or       // Ou une commande destructrice
         2 of ($rc*) or       // Ou deux commandes typiques de ransomware
         #rc6 > 4             // Ou plus de 4 suppressions de fichiers de sauvegarde
      )
}
```

Code I use :
```bash
# Function to scan for ransomware and move detected files to a quarantine folder
ptk_yara() {
    # Check if YARA is installed
    if ! command -v yara &> /dev/null; then
        echo "Error: YARA is not installed. Please install it:"
        echo "  Debian/Ubuntu: sudo apt-get install yara"
        echo "  Red Hat/CentOS: sudo yum install yara"
        return 1
    fi

    # Check if a parameter (folder to scan) is provided
    if [ -z "$1" ]; then
        echo "Usage: $0 <folder>"
        echo "Example: $0 /path/to/folder"
        return 1
    fi

    local TARGET_DIR="$1"

    # Check if the target folder exists
    if [ ! -d "$TARGET_DIR" ]; then
        echo "Error: The folder '$TARGET_DIR' does not exist or is not a directory."
        return 1
    fi

    # Default paths for YARA directories
    local DETECTION_DIR="$HOME/detection"
    local SIGNATURE_BASE_DIR="$HOME/signature-base"

    # Verify existence of default directories
    for dir in "$DETECTION_DIR" "$SIGNATURE_BASE_DIR"; do
        if [ ! -d "$dir" ]; then
            echo "Error: The directory '$dir' does not exist."
            return 1
        fi
    done

    # Specific paths for targeted YARA files
    local INDICATOR_YARA="$DETECTION_DIR/yara/indicator_suspicious.yar"
    local OLYMPIC_YARA="$SIGNATURE_BASE_DIR/yara/apt_olympic_destroyer.yar"
    local GENERIC_YARA="$SIGNATURE_BASE_DIR/yara/crime_ransom_generic.yar"

    # Verify existence of specific YARA files
    for yara_file in "$INDICATOR_YARA" "$OLYMPIC_YARA" "$GENERIC_YARA"; do
        if [ ! -f "$yara_file" ]; then
            echo "Error: The YARA file '$yara_file' does not exist."
            return 1
        fi
    done

    # List of YARA files with "ransom" in their name
    local RANSOM_YARA_DIR="$SIGNATURE_BASE_DIR/yara"
    local RANSOM_YARA_FILES=$(find "$RANSOM_YARA_DIR" -name "*ransom*yar" 2>/dev/null)
    if [ -z "$RANSOM_YARA_FILES" ]; then
        echo "Warning: No '*ransom*yar' files found in '$RANSOM_YARA_DIR'."
    fi

    # Count total number of rules used
    local TOTAL_RULES=3  # The 3 specific files
    if [ -n "$RANSOM_YARA_FILES" ]; then
        local EXTRA_RULES=$(echo "$RANSOM_YARA_FILES" | wc -l)
        TOTAL_RULES=$((TOTAL_RULES + EXTRA_RULES))
    fi

    # Perform the scan
    echo "Scanning '$TARGET_DIR' with $TOTAL_RULES YARA rules..."
    local RESULT_FILE="scan_results.txt"
    > "$RESULT_FILE"  # Clear the file before starting

    # Scan with specific files and targeted rules
    echo " - $(basename "$INDICATOR_YARA") [INDICATOR_SUSPICIOUS_GENRansomware]"
    yara -r -i INDICATOR_SUSPICIOUS_GENRansomware "$INDICATOR_YARA" "$TARGET_DIR" >> "$RESULT_FILE" 2>/dev/null

    echo " - $(basename "$OLYMPIC_YARA") [Destructive_Ransomware_Gen1]"
    yara -r -i Destructive_Ransomware_Gen1 "$OLYMPIC_YARA" "$TARGET_DIR" >> "$RESULT_FILE" 2>/dev/null

    echo " - $(basename "$GENERIC_YARA") [SUSP_RANSOMWARE_Indicator_Jul20]"
    yara -r -i SUSP_RANSOMWARE_Indicator_Jul20 "$GENERIC_YARA" "$TARGET_DIR" >> "$RESULT_FILE" 2>/dev/null

    # Scan with all *ransom*yar files (all rules included)
    if [ -n "$RANSOM_YARA_FILES" ]; then
        for yara_file in $RANSOM_YARA_FILES; do
            echo " - $(basename "$yara_file") [all rules]"
            yara -r "$yara_file" "$TARGET_DIR" >> "$RESULT_FILE" 2>/dev/null
        done
    fi

    # Create the quarantine folder with "-ransomware" suffix
    local QUARANTINE_DIR="${TARGET_DIR}-ransomware"
    if [ ! -d "$QUARANTINE_DIR" ]; then
        mkdir -p "$QUARANTINE_DIR"
        if [ $? -ne 0 ]; then
            echo "Error: Unable to create the folder '$QUARANTINE_DIR'."
            rm -f "$RESULT_FILE"
            return 1
        fi
    fi

    # Process results and move detected files
    echo "Results:"
    if [ -s "$RESULT_FILE" ]; then
        cat "$RESULT_FILE"
        local DETECTED_COUNT=$(wc -l < "$RESULT_FILE")
        echo "Number of detections: $DETECTED_COUNT"

        # Extract file paths and move them
        echo "Moving detected files to '$QUARANTINE_DIR'..."
        while IFS= read -r line; do
            local FILE_PATH=$(echo "$line" | awk '{print $2}')
            if [ -f "$FILE_PATH" ]; then
                mv "$FILE_PATH" "$QUARANTINE_DIR/"
                if [ $? -eq 0 ]; then
                    echo " - Moved: $FILE_PATH -> $QUARANTINE_DIR/$(basename "$FILE_PATH")"
                else
                    echo " - Error moving '$FILE_PATH'."
                fi
            else
                echo " - File not found: $FILE_PATH"
            fi
        done < "$RESULT_FILE"
    else
        echo "No ransomware detected."
    fi

    # Cleanup
    rm -f "$RESULT_FILE"
}
```
## Categorize malware by compiler

The `ptkcompilers` Bash function organizes files in specified directories based on their compiler signatures and renames them according to specific rules. It:

1.  **Checks Dependencies**: Ensures diec (Detect It Easy) and file commands are installed, returning an error if either is missing.
2.  **Validates Input**: Requires at least one directory as an argument, otherwise returns an error.
3.  **Categorizes Files**: Uses diec to analyze files and moves them into subdirectories (VisualCpp, DotNet, MinGW, GoLang, PureBasic, Delphi, VBNet, Unknown, Other) based on detected compilers, with special handling for Borland and Embarcadero Delphi files.
4.  **Renames Files**:
    -   Adds .exe to all files in DotNet.
    -   Uses file to detect PE32/PE32+ executables in other directories and renames them with .ex_.
5.  **Cleans Up**: Removes empty subdirectories after processing.
6.  **Provides Feedback**: Outputs debug messages and a summary of processed directories and renaming rules.

```bash
ptk_compilers() {

    # The sort_files function categorizes files by compiler using diec, renames .NET files to .exe and PE32/PE32+
    # executables to .ex_ using file, and organizes them into subdirectories.

    # Check if diec tool is available (for initial categorization)
    if ! command -v diec >/dev/null 2>&1; then
        echo "Error: 'diec' tool is not installed or not found in PATH."
        echo "Please install Detect It Easy (DIE) to use this script."
        echo "You can download it from: https://github.com/horsicq/Detect-It-Easy"
        return 1
    fi

    # Check if file command is available (for PE32/PE32+ detection)
    if ! command -v file >/dev/null 2>&1; then
        echo "Error: 'file' command is not installed or not found in PATH."
        echo "This is a standard Unix tool required for executable detection."
        return 1
    fi

    # Check if at least one directory is provided
    if [ $# -eq 0 ]; then
        echo "Error: Please provide at least one source directory as parameter"
        echo "Usage: $0 dir1 [dir2 dir3 ...]"
        return 1
    fi

    # List of directory names to create/check
    local DIR_NAMES="VisualCpp DotNet MinGW GoLang PureBasic Delphi VBNet Unknown Other"

    # Process each directory provided as parameter
    for SOURCE_DIR in "$@"; do
        # Check if source directory exists
        if [ ! -d "$SOURCE_DIR" ]; then
            echo "Warning: Directory '$SOURCE_DIR' does not exist, skipping..."
            continue
        fi

        echo "Processing directory: $SOURCE_DIR"
        
        # Create destination directories within the source directory
        for dir in $DIR_NAMES; do
            mkdir -p "$SOURCE_DIR/$dir"
        done

        # Loop through all files in the current source directory
        for file in "$SOURCE_DIR"/*; do
            # Skip if it's one of our destination directories or no files exist
            case "$(basename "$file")" in
                VisualCpp|DotNet|MinGW|GoLang|PureBasic|Delphi|VBNet|Unknown|Other)
                    continue ;;
            esac
            [ -e "$file" ] || continue
            
            # Run diec and capture the output for initial categorization
            local output
            output=$(diec -d "$file" 2>/dev/null)
            
            # Determine category based on keywords and move files
            if echo "$output" | grep -q "Microsoft Visual C/C++"; then
                mv "$file" "$SOURCE_DIR/VisualCpp/"
            elif echo "$output" | grep -q ".NET Framework\|VB.NET"; then
                mv "$file" "$SOURCE_DIR/DotNet/"
            elif echo "$output" | grep -q "MinGW"; then
                mv "$file" "$SOURCE_DIR/MinGW/"
            elif echo "$output" | grep -q "Go("; then
                mv "$file" "$SOURCE_DIR/GoLang/"
            elif echo "$output" | grep -q "PureBasic"; then
                mv "$file" "$SOURCE_DIR/PureBasic/"
            elif echo "$output" | grep -q "Borland Delphi"; then
                mv "$file" "$SOURCE_DIR/Delphi/"
            # New condition for Embarcadero Delphi or Turbo Linker/Delphi
            elif echo "$output" | grep -q "Embarcadero Delphi\|Turbo Linker.*Delphi"; then
                mv "$file" "$SOURCE_DIR/Delphi/"
            elif echo "$output" | grep -q "VB.NET"; then
                mv "$file" "$SOURCE_DIR/VBNet/"
            elif echo "$output" | grep -q "Unknown: Unknown"; then
                mv "$file" "$SOURCE_DIR/Unknown/"
            else
                mv "$file" "$SOURCE_DIR/Other/"
            fi
        done

        # Rename files according to specifications
        # 1. Rename all files in DotNet with .exe
        echo "Renaming files in DotNet with .exe extension..."
        for file in "$SOURCE_DIR/DotNet"/*; do
            if [ -f "$file" ]; then
                # Avoid re-adding .exe if already present
                if [[ "$file" != *.exe ]]; then
                    mv "$file" "${file}.exe"
                    echo "Renamed: $file -> ${file}.exe"
                fi
            fi
        done

        # 2. Rename PE32/PE32+ executables in other directories with .ex_
        echo "Checking for PE32/PE32+ executables in other directories using 'file'..."
        for dir in $DIR_NAMES; do
            # Skip DotNet directory as it has special treatment
            [ "$dir" = "DotNet" ] && continue
            
            for file in "$SOURCE_DIR/$dir"/*; do
                if [ -f "$file" ]; then
                    # Use 'file' command to detect PE32 or PE32+ executables
                    local output
                    output=$(file "$file")
                    echo "Debug: Checking $file"
                    echo "Debug: file output = $output"
                    
                    # Adjusted regex for 'file' output
                    if echo "$output" | grep -q "PE32 executable\|PE32+ executable"; then
                        # Avoid re-adding .ex_ if already present
                        if [[ "$file" != *.ex_ ]]; then
                            mv "$file" "${file}.ex_"
                            echo "Renamed: $file -> ${file}.ex_ (PE32/PE32+ detected)"
                        else
                            echo "Skipped: $file already has .ex_ extension"
                        fi
                    else
                        echo "Debug: $file is not a PE32/PE32+ executable"
                    fi
                fi
            done
        done

        # Remove empty directories among those we created
        for dir in $DIR_NAMES; do
            if [ -d "$SOURCE_DIR/$dir" ] && [ -z "$(ls -A "$SOURCE_DIR/$dir")" ]; then
                rmdir "$SOURCE_DIR/$dir"
                echo "Removed empty directory: $SOURCE_DIR/$dir"
            fi
        done
    done

    echo "Sorting and renaming completed!"
    echo "Processed directories: $@"
    echo "Created directories in each source directory (empty ones removed):"
    echo "- VisualCpp: Files compiled with Microsoft Visual C/C++ (*.ex_ for PE32/PE32+)"
    echo "- DotNet: .NET Framework files (all renamed with .exe)"
    echo "- MinGW: Files compiled with MinGW (*.ex_ for PE32/PE32+)"
    echo "- GoLang: Files compiled with Go (*.ex_ for PE32/PE32+)"
    echo "- PureBasic: Files compiled with PureBasic (*.ex_ for PE32/PE32+)"
    echo "- Delphi: Files compiled with Borland/Embarcadero Delphi (*.ex_ for PE32/PE32+)"
    echo "- VBNet: Specific VB.NET files (*.ex_ for PE32/PE32+)"
    echo "- Unknown: Files with unknown compiler (*.ex_ for PE32/PE32+)"
    echo "- Other: Other uncategorized compilers (*.ex_ for PE32/PE32+)"
}
```
```
petik@lab:~$ tree 2025-02-11-ransomware
2025-02-11-ransomware
├── Delphi
│   ├── HEUR-Trojan-Ransom.Win32.Generic-26a74ace0b160831901b912f173137326019e731530e4ec283cbe44e3ccda165.ex_
│   ├── HEUR-Trojan-Ransom.Win32.Generic-fe81c5caa0e269c1cbbd0aca9557677c4f57829d621f2f21768728c92e4f0498.ex_
│   ├── HEUR-Trojan-Ransom.Win32.Trigona.gen-be6dfedd91e1ebc81d58a61b05b0ef93322425b247636a0f7f5c3eeebbc52edd.ex_
│   ├── HEUR-Trojan-Ransom.Win32.Trigona.gen-ff82b9021651a1de046c80242be44b53fa23f88f850b9d536ce8be7d9b41afbb.ex_
│   ├── UDS-Trojan-Ransom.Win32.Trigona.ad-b4df356a0b8746b77456531f53cf8418b356b40cd3a9ea214bfe2adba8e8838b.ex_
│   └── UDS-Trojan-Ransom.Win32.Trigona.ae-807b27dd4ddf9f8be2493a9e9f9a1bbe69c06770039847425acc6458d72f29dd.ex_
└── VisualCpp
    └── HEUR-Trojan-Ransom.Win32.Generic-44d933ece41417b88fe4961b801c851d7b802d3d5c32bfeba187041b6f675faa.ex_

```
