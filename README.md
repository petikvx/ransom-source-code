# Malware Source Code for Research Purposes Only

## Warning

I am offering the source code of this malware strictly for research and educational purposes. It is intended to be used by security professionals, researchers, and students to study and understand its mechanisms in order to improve cybersecurity defenses. The use of this code for malicious purposes is explicitly prohibited. Any use of this code for malicious intent or illegal activities is strongly discouraged, not condoned, and forbidden. Please ensure compliance with all applicable laws and ethical guidelines when handling this material.

## Download samples with MWDB

```bash
#!/bin/bash

# Prompt the user to enter the search pattern
echo "Please enter the search pattern (e.g., tag:\"yara:malware_win_chaos\"):"
read search_pattern

# Ensure the pattern is properly quoted for mwdb search
# If it doesn't already start and end with quotes, add them
if [[ ! "$search_pattern" =~ ^\".*\"$ ]]; then
    formatted_pattern="\"$search_pattern\""
else
    formatted_pattern="$search_pattern"
fi

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
```
