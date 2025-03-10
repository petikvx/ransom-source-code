using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace CustomWindowsForm;

public class Form2 : Form
{
	private bool mouseDown;

	private Point lastLocation;

	private IContainer components;

	private Panel panel1;

	private Label label3;

	private Label label2;

	private Label label1;

	private ButtonZ _CloseButton;

	private ButtonZ buttonZ1;

	private TextBox textBox2;

	private Label label4;

	private TextBox textBox1;

	public Form2()
	{
		InitializeComponent();
	}

	private void buttonZ1_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void panel1_Paint(object sender, PaintEventArgs e)
	{
	}

	private void panel1_MouseDown(object sender, MouseEventArgs e)
	{
		mouseDown = true;
		lastLocation = e.Location;
	}

	private void panel1_MouseMove(object sender, MouseEventArgs e)
	{
		if (mouseDown)
		{
			int x = ((Form)this).Location.X - lastLocation.X + e.X;
			int y = ((Form)this).Location.Y - lastLocation.Y + e.Y;
			((Form)this).Location = new Point(x, y);
			((Control)this).Update();
		}
	}

	private void panel1_MouseUp(object sender, MouseEventArgs e)
	{
		mouseDown = false;
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
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Expected O, but got Unknown
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_077b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form2));
		panel1 = new Panel();
		textBox2 = new TextBox();
		label4 = new Label();
		textBox1 = new TextBox();
		buttonZ1 = new ButtonZ();
		label3 = new Label();
		label2 = new Label();
		label1 = new Label();
		_CloseButton = new ButtonZ();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).BackColor = Color.FromArgb(30, 30, 30);
		((Control)panel1).Controls.Add((Control)(object)textBox2);
		((Control)panel1).Controls.Add((Control)(object)label4);
		((Control)panel1).Controls.Add((Control)(object)textBox1);
		((Control)panel1).Controls.Add((Control)(object)buttonZ1);
		((Control)panel1).Controls.Add((Control)(object)label3);
		((Control)panel1).Controls.Add((Control)(object)label2);
		((Control)panel1).Controls.Add((Control)(object)label1);
		((Control)panel1).Controls.Add((Control)(object)_CloseButton);
		((Control)panel1).Location = new Point(1, 1);
		((Control)panel1).Margin = new Padding(4, 4, 4, 4);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(516, 479);
		((Control)panel1).TabIndex = 0;
		((Control)panel1).Paint += new PaintEventHandler(panel1_Paint);
		((Control)panel1).MouseDown += new MouseEventHandler(panel1_MouseDown);
		((Control)panel1).MouseMove += new MouseEventHandler(panel1_MouseMove);
		((Control)panel1).MouseUp += new MouseEventHandler(panel1_MouseUp);
		((Control)textBox2).Location = new Point(145, 393);
		((Control)textBox2).Margin = new Padding(4, 4, 4, 4);
		((Control)textBox2).Name = "textBox2";
		((Control)textBox2).Size = new Size(333, 22);
		((Control)textBox2).TabIndex = 12;
		((Control)textBox2).Text = "44wJKzwrzWY7dxLov4EjVia3wmwaj6ige6a8C6eHKXKtVy8PTU3SnCG6A6do3vL4Cu3kLUedKwjomDKe754QhshVJw52xFV";
		((Control)label4).AutoSize = true;
		((Control)label4).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label4).ForeColor = Color.White;
		((Control)label4).Location = new Point(49, 394);
		((Control)label4).Margin = new Padding(4, 0, 4, 0);
		((Control)label4).Name = "label4";
		((Control)label4).Size = new Size(90, 25);
		((Control)label4).TabIndex = 11;
		((Control)label4).Text = "Monero: ";
		label4.TextAlign = (ContentAlignment)2;
		((Control)textBox1).Location = new Point(145, 350);
		((Control)textBox1).Margin = new Padding(4, 4, 4, 4);
		((Control)textBox1).Name = "textBox1";
		((Control)textBox1).Size = new Size(333, 22);
		((Control)textBox1).TabIndex = 10;
		((Control)textBox1).Text = "bc1qlnzcep4l4ac0ttdrq7awxev9ehu465f2vpt9x0";
		((Control)buttonZ1).Anchor = (AnchorStyles)9;
		buttonZ1.BZBackColor = Color.FromArgb(30, 30, 30);
		buttonZ1.DisplayText = "X";
		((ButtonBase)buttonZ1).FlatStyle = (FlatStyle)0;
		((Control)buttonZ1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)buttonZ1).ForeColor = Color.White;
		((Control)buttonZ1).Location = new Point(463, 10);
		((Control)buttonZ1).Margin = new Padding(4, 4, 4, 4);
		buttonZ1.MouseClickColor1 = Color.FromArgb(60, 60, 160);
		buttonZ1.MouseHoverColor = Color.FromArgb(50, 50, 50);
		((Control)buttonZ1).Name = "buttonZ1";
		((Control)buttonZ1).Size = new Size(41, 30);
		((Control)buttonZ1).TabIndex = 9;
		((Control)buttonZ1).Text = "X";
		buttonZ1.TextLocation_X = 6;
		buttonZ1.TextLocation_Y = 1;
		((ButtonBase)buttonZ1).UseVisualStyleBackColor = true;
		((Control)buttonZ1).Click += buttonZ1_Click;
		((Control)label3).AutoSize = true;
		((Control)label3).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = Color.White;
		((Control)label3).Location = new Point(49, 351);
		((Control)label3).Margin = new Padding(4, 0, 4, 0);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(81, 25);
		((Control)label3).TabIndex = 8;
		((Control)label3).Text = "Bitcoin: ";
		label3.TextAlign = (ContentAlignment)2;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = Color.White;
		((Control)label2).Location = new Point(15, 10);
		((Control)label2).Margin = new Padding(4, 0, 4, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(64, 25);
		((Control)label2).TabIndex = 7;
		((Control)label2).Text = "About";
		label2.TextAlign = (ContentAlignment)2;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = Color.White;
		((Control)label1).Location = new Point(4, 57);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(485, 275);
		((Control)label1).TabIndex = 6;
		((Control)label1).Text = componentResourceManager.GetString("label1.Text");
		label1.TextAlign = (ContentAlignment)2;
		((Control)_CloseButton).Anchor = (AnchorStyles)9;
		_CloseButton.BZBackColor = Color.FromArgb(30, 30, 30);
		_CloseButton.DisplayText = "X";
		((ButtonBase)_CloseButton).FlatStyle = (FlatStyle)0;
		((Control)_CloseButton).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_CloseButton).ForeColor = Color.White;
		((Control)_CloseButton).Location = new Point(523, 30);
		((Control)_CloseButton).Margin = new Padding(4, 4, 4, 4);
		_CloseButton.MouseClickColor1 = Color.FromArgb(60, 60, 160);
		_CloseButton.MouseHoverColor = Color.FromArgb(50, 50, 50);
		((Control)_CloseButton).Name = "_CloseButton";
		((Control)_CloseButton).Size = new Size(41, 30);
		((Control)_CloseButton).TabIndex = 5;
		((Control)_CloseButton).Text = "X";
		_CloseButton.TextLocation_X = 6;
		_CloseButton.TextLocation_Y = 1;
		((ButtonBase)_CloseButton).UseVisualStyleBackColor = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(25, 25, 25);
		((Form)this).ClientSize = new Size(521, 485);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Margin = new Padding(4, 4, 4, 4);
		((Control)this).Name = "Form2";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Form2";
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
