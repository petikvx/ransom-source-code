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
