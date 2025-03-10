using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.CSharp;

namespace CustomWindowsForm;

public class Compiler
{
	public Compiler(string sourceCode, string savePath, string iconLocation)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		string[] array = new string[7] { "System.dll", "System.Linq.dll", "System.Windows.Forms.dll", "System.Text.RegularExpressions.dll", "System.Runtime.InteropServices.dll", "System.ServiceProcess.dll", "System.Security.dll" };
		Dictionary<string, string> obj = new Dictionary<string, string> { { "CompilerVersion", "v4.0" } };
		string text = "/target:winexe /platform:anycpu /optimize+ ";
		if (iconLocation != "")
		{
			text = text + "/win32icon:" + iconLocation;
		}
		CSharpCodeProvider val = new CSharpCodeProvider((IDictionary<string, string>)obj);
		try
		{
			CompilerParameters val2 = new CompilerParameters(array)
			{
				GenerateExecutable = true,
				GenerateInMemory = false,
				OutputAssembly = savePath,
				CompilerOptions = text,
				TreatWarningsAsErrors = false,
				IncludeDebugInformation = false
			};
			CompilerResults val3 = ((CodeDomProvider)val).CompileAssemblyFromSource(val2, new string[1] { sourceCode });
			if (((CollectionBase)(object)val3.Errors).Count > 0)
			{
				foreach (CompilerError item in (CollectionBase)(object)val3.Errors)
				{
					CompilerError val4 = item;
					MessageBox.Show($"{val4.ErrorText}\nLine: {val4.Line} - Column: {val4.Column}\nFile: {val4.FileName}");
				}
				return;
			}
			MessageBox.Show("Done!");
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
