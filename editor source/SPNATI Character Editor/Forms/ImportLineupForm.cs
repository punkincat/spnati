using Desktop.Skinning;
using KisekaeImporter;
using SPNATI_Character_Editor.Properties;
using System;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Forms
{
	public partial class ImportLineupForm : SkinnedForm
	{
		public KisekaeCode Code { get; private set; }

		public ImportLineupForm(ImportLineupMode mode)
		{
			InitializeComponent();

			switch (mode)
			{
				case ImportLineupMode.Pose:
					picHelp.Image = Resources.LineupPose;
					break;
				case ImportLineupMode.Wardrobe:
					picHelp.Image = Resources.LineupClothing;
					break;
				default:
					picHelp.Image = Resources.LineupAll;
					break;
			}
		}

		private void cmdOK_Click(object sender, EventArgs e)
		{
			string text = txtCode.Text;
			if (!text.Contains("***"))
			{
				MessageBox.Show("This does not appear to be a character lineup. Do you have ALL selected in Kisekae's Export window?");
				return;
			}

			Code = new KisekaeCode(txtCode.Text, false);

			DialogResult = DialogResult.OK;
			Close();
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}

	public enum ImportLineupMode
	{
		All,
		Wardrobe,
		Pose
	}
}
