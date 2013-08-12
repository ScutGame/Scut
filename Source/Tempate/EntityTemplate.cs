using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.ComponentModel.Design;
using CodeSmith.Engine;
using SchemaExplorer;

public class EntityTemplate : CodeTemplate
{
	
	private string _outputDirectory = String.Empty;
	
	public EntityTemplate() : base()
	{
	}
	
	[Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(System.Drawing.Design.UITypeEditor))] 
	[Optional]
	[Category("Output")]
	[Description("The directory to output the results to.")]
	public string OutputDirectory 
	{
		get
		{
			// default to the directory that the template is located in
			if (_outputDirectory.Length == 0) return this.CodeTemplateInfo.DirectoryName + "output\\";
			
			return _outputDirectory;
		}
		set
		{
			if (!value.EndsWith("\\")) value += "\\";
			_outputDirectory = value;
		} 
	}
}