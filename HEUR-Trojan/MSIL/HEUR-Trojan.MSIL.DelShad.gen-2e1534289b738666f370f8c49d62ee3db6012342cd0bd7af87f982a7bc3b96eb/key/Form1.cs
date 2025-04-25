using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace key;

public class Form1 : Form
{
	private IContainer components = null;

	public Form1()
	{
		InitializeComponent();
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
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Form1));
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(6f, 13f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackgroundImage = (Image)componentResourceManager.GetObject("$this.BackgroundImage");
		((Form)this).ClientSize = new Size(685, 637);
		((Control)this).Name = "Form1";
		((Control)this).Text = "Form1";
		((Control)this).ResumeLayout(false);
	}
}
