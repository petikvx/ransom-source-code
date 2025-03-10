using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CustomWindowsForm;
using Yashma_Ransomware.Properties;

namespace Ryuk.Net;

public class extensions : Form
{
	private bool mouseDown;

	private Point lastLocation;

	private IContainer components;

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
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
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
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
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
