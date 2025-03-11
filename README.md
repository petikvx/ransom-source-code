# Malware Source Code for Research Purposes Only

## Warning

I am offering the source code of this malware strictly for research and educational purposes. It is intended to be used by security professionals, researchers, and students to study and understand its mechanisms in order to improve cybersecurity defenses. The use of this code for malicious purposes is explicitly prohibited. Any use of this code for malicious intent or illegal activities is strongly discouraged, not condoned, and forbidden. Please ensure compliance with all applicable laws and ethical guidelines when handling this material.

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

- `yara detection/indicator_suspicious.yar -i INDICATOR_SUSPICIOUS_GENRansomware`
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
- `yara signature-base/blob/master/yara/crime_ransom_generic.yar -i Destructive_Ransomware_Gen1`
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
