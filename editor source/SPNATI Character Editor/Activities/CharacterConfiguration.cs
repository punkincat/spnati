using Desktop;
using System;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace SPNATI_Character_Editor.Activities
{
	[Spacer]
	[Activity(typeof(Character), 1000, DelayRun = true, Caption = "Configuration")]
	public partial class CharacterConfiguration : Activity
	{
		private Character _character;
		private CharacterEditorData _editorData;

		public override string Caption
		{
			get { return "Configuration"; }
		}

		public CharacterConfiguration()
		{
			InitializeComponent();
		}

		protected override void OnInitialize()
		{
			_character = Record as Character;
			_editorData = CharacterDatabase.GetEditorData(_character);
		}

		protected override void OnFirstActivate()
		{
			chkOnlyCustomPoses.Checked = _editorData.OnlyCustomPoses;
			chkHidePrefixlessImages.Checked = _editorData.HidePrefixlessImages;
			foreach (string prefix in _editorData.IgnoredPrefixes)
			{
				gridPrefixes.Rows.Add(new object[] { prefix });
			}
			foreach (string marker in _editorData.PosePreviewMarkers)
			{
				gridMarkers.Rows.Add(new object[] { marker });
			}
		}

		public override void Save()
		{
			_editorData.OnlyCustomPoses = chkOnlyCustomPoses.Checked;
			_editorData.HidePrefixlessImages = chkHidePrefixlessImages.Checked;
			_editorData.IgnoredPrefixes.Clear();
			foreach (DataGridViewRow row in gridPrefixes.Rows)
			{
				string prefix = row.Cells[0].Value?.ToString();
				if (!string.IsNullOrEmpty(prefix))
				{
					_editorData.IgnoredPrefixes.Add(prefix);	
				}
			}
			_editorData.PosePreviewMarkers.Clear();
			foreach (DataGridViewRow row in gridMarkers.Rows)
			{
				string marker = row.Cells[0].Value?.ToString();
				if (!string.IsNullOrEmpty(marker))
				{
					_editorData.PosePreviewMarkers.Add(marker);
				}
			}
			Workspace.SendMessage(WorkspaceMessages.UpdateMarkers, Enumerable.Empty<string>());
		}

		private void tsAddGroup_Click(object sender, EventArgs e)
		{
			/*Pose pose = new Pose();
			pose.Id = "new_pose";
			lstPoses.Items.Add(pose);
			lstPoses.SelectedItem = pose;
			_character.CustomPoses.Add(pose);
			_character.CustomPoses.Sort();
			_character.Character.PoseLibrary.Add(pose);*/
		}

		private void tsRemoveGroup_Click(object sender, EventArgs e)
		{
			/*if (_pose == null ||
				MessageBox.Show($"Are you sure you want to permanently delete {_pose}? This operation cannot be undone.",
						"Remove Pose", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
			{
				return;
			}
			_character.CustomPoses.Remove(_sourcePose);
			_character.Character.PoseLibrary.Remove(_sourcePose);
			lstPoses.Items.Remove(_sourcePose);
			if (lstPoses.Items.Count > 0)
			{
				lstPoses.SelectedIndex = 0;
			}*/
		}

		private void tsDuplicateGroup_Click(object sender, EventArgs e)
		{
			/*if (_pose == null) { return; }
			SavePose();

			Pose copy = _sourcePose.Clone() as Pose;
			lstPoses.Items.Add(copy);
			lstPoses.SelectedItem = copy;
			_character.CustomPoses.Add(copy);*/
		}
	}
}
