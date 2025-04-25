
### Introduction to Malware Analysis

The file identified by the hash SHA-256 (67c7396ddf3846b39194bbf183ee5ab683464d318f0b7e5e72b75f81355f96af) has been flagged as a potential malware sample, as indicated by multiple crowdsourced YARA rules analyzed on VirusTotal. This 81 KB Windows executable, created on March 6, 2025, and first submitted for analysis on March 7, 2025, exhibits characteristics suggestive of ransomware, with detections linking it to known strains such as Babuk and destructive malware variants. Compiled using Microsoft Visual C++ and submitted within a five-day window, the file has triggered alerts for suspicious command variations, references to VEEAM, and potential locker behavior. This analysis aims to explore the file’s properties, assess its malicious intent based on detection patterns, and provide insights into its potential impact and mitigation strategies.

### Basic Properties

- **MD5**: 480315a9112fed60ac9d8c8d7b6c2d7e
- **SHA-1**: ac5cee822b7c5ba7449c42448bcfc11a14a7
- **SHA-256**: 67c7396ddf3846b39194bbf183ee5ab683464d318f0b7e5e72b75f81355f96af
- **SHA-512**: 67c7396ddf3846b39194bbf183ee5ab683464d318f0b7e5e72b75f81355f96af
- **Authentihash**: 06f955e1388f2fce10bd56f77a2b762d2ca39c5210fb097c428e9971d
- **Imphash**: 61c9bebeda0fcf46a412381cb1fd8
- **Rich PE header hash**: 67effa48956f6b7f9a2cb8e5bf1c
- **SSDEEP**: 1536:7z6MH9XJ3J5rQL0jgY8zP8LHD4XwANH71dLgdIiFM2iG2k4Xw:5MHVsJrQL0jgY8zP8LHD4XwANH71dP
- **TLSH**: T18883BE11954D28D512231C13A1FAC13A1F0B0358B863C017AEFBOAD8F6FF27
- **File type**: Win32 EXE executable windows win32 pe
- **Magic**: PE32 executable (GUI) Intel 80386, for MS Windows
- **TrID**: Win32 Executable MS Visual C++ (56.3%) | Win32 Dynamic Link Library (generic) (11.8%) | Win16 NE executable (generic) (9%) | Win32 Executable (generic) (8.1%) | PE32 (Compiler: Microsoft Visual C++/C++ (19.36.35323) [C++]) | Linker: Microsoft Linker (14.36.35323) | Tool: Visual Studio (2022 version 17.6)
- **Magika**: PEBIN
- **File size**: 81.00 KB (82944 bytes)

### File Info

- **Creation Time**: 2025-03-06 01:55:27 UTC
- **First Submission**: 2025-03-07 09:12:59 UTC
- **Last Submission**: 2025-03-09 06:32:03 UTC
- **Last Analysis**: 2025-03-11 16:54:01 UTC

### Crowdsourced YARA Rules

- **Matches rule INDICATOR_SUSPICIOUS_GenRansomware from ruleset indicator_suspicious at https://github.com/ditekshen/detection by ditekShen**

- **Matches rule INDICATOR_SUSPICIOUS_EXE_References_VEEAM from ruleset indicator_suspicious at https://github.com/ditekshen/detection by ditekShen**

- **Matches rule MALWARE_Win_Babuk from ruleset malware at https://github.com/ditekshen/detection by ditekShen**

- **Matches rule Destructive_Ransomware_Gen1 from ruleset apt_olympic_destroyer at https://github.com/Neo23x0/signature-base by Florian Roth (Nextron Systems)**

- **Matches rule Ransom_Babuk from ruleset RANSOM_BabukLocker_Jan2021 at https://github.com/advanced-threat-research/Yara-Rules by TS @ McAfee ATR**

- **Matches rule win_astrolocker_auto from ruleset win_astrolocker_auto at https://malpedia.caad.fkie.fraunhofer.de/ by Felix Bilstein - yara-signator (https://github.com/fxb-cocacoding/dot.com)**
