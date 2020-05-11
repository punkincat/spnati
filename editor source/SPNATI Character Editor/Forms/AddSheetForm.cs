﻿using Desktop.Skinning;
using System;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Forms
{
	public partial class AddSheetForm : SkinnedForm
	{
		public AddSheetForm(string name)
		{
			InitializeComponent();
			txtName.Text = name;
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void cmdCreate_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		public string SheetName
		{
			get { return txtName.Text; }
		}
	}
}
