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
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected O, but got Unknown
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		string[] array = new string[6] { "System.dll", "System.Linq.dll", "System.Windows.Forms.dll", "System.Text.RegularExpressions.dll", "System.Runtime.InteropServices.dll", "System.Security.dll" };
		Dictionary<string, string> dictionary = new Dictionary<string, string> { { "CompilerVersion", "v4.0" } };
		string text = "/target:winexe /platform:anycpu /optimize+ ";
		if (iconLocation != "")
		{
			text = text + "/win32icon:" + iconLocation;
		}
		CSharpCodeProvider val = new CSharpCodeProvider((IDictionary<string, string>)dictionary);
		try
		{
			CompilerParameters val2 = new CompilerParameters(array);
			val2.GenerateExecutable = true;
			val2.GenerateInMemory = false;
			val2.OutputAssembly = savePath;
			val2.CompilerOptions = text;
			val2.TreatWarningsAsErrors = false;
			val2.IncludeDebugInformation = false;
			CompilerParameters val3 = val2;
			CompilerResults val4 = ((CodeDomProvider)val).CompileAssemblyFromSource(val3, new string[1] { sourceCode });
			if (((CollectionBase)(object)val4.Errors).Count > 0)
			{
				foreach (CompilerError item in (CollectionBase)(object)val4.Errors)
				{
					CompilerError val5 = item;
					MessageBox.Show($"{val5.ErrorText}\nLine: {val5.Line} - Column: {val5.Column}\nFile: {val5.FileName}");
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
