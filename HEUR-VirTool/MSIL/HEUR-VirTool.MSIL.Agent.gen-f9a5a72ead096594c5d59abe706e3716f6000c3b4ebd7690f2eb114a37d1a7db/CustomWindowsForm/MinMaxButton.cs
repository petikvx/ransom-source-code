using System;
using System.Drawing;
using System.Windows.Forms;

namespace CustomWindowsForm;

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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		((ButtonBase)this).OnPaint(pe);
		switch (_customFormState)
		{
		case CustomFormState.Normal:
		{
			pe.Graphics.FillRectangle((Brush)new SolidBrush(color), ((Control)this).ClientRectangle);
			for (int i = 0; i < 2; i++)
			{
				pe.Graphics.DrawRectangle(new Pen(((Control)this).ForeColor), textX + i + 1, textY, 10, 10);
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
