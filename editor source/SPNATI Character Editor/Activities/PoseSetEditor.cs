using Desktop;
using System;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Activities
{
	[Activity(typeof(Character), 212)]
	[Activity(typeof(Costume), 212)]
	public partial class PoseSetEditor : Activity
	{
		private ISkin _character;
		bool _initialized = false;
		PoseSet _selectedSet;
		bool _selecting = true;

		public override string Caption
		{
			get { return "Pose Sets"; }
		}

		public PoseSetEditor()
		{
			InitializeComponent();
		}

		protected override void OnInitialize()
		{
			_character = Record as ISkin;
			poseSetControl1.SetCharacter(_character);
		}

		protected override void OnFirstActivate()
		{
			poseSetControl1.Activate();
			RebuildPoseSetList();
		}

		private void RebuildPoseSetList()
		{
			lstPoseSets.Items.Clear();
			foreach (PoseSet poseSet in _character.CustomPoseSets)
			{
				lstPoseSets.Items.Add(poseSet);
			}
			lstPoseSets.Sorted = true;

			if (lstPoseSets.Items.Count > 0)
			{
				lstPoseSets.SelectedIndex = 0;
			}
			_initialized = true;
		}

		private void lstPoseSets_SelectedIndexChanged(object sender, EventArgs e)
		{
			PoseSet poseSet = lstPoseSets.SelectedItem as PoseSet;
			if (poseSet != null)
			{
				if (_initialized)
				{
					poseSetControl1.SavePoseSetEntry();
				}
				_selectedSet = poseSet;
				_selecting = true;
				txtPoseSetRename.Text = _selectedSet.Id;
				poseSetControl1.SetPoseSet(_selectedSet);
				_selecting = false;
			}
		}

		private void tsRemovePoseSet_Click(object sender, EventArgs e)
		{
			if (_selectedSet == null ||
	MessageBox.Show($"Are you sure you want to permanently delete {_selectedSet}? This operation cannot be undone.",
			"Remove Pose Set", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
			{
				return;
			}
			_character.IsDirty = true;
			_character.CustomPoseSets.Remove(_selectedSet);
			_character.Character.PoseLibrary.Remove(_selectedSet);
			lstPoseSets.Items.Remove(_selectedSet);
			if (lstPoseSets.Items.Count > 0)
			{
				lstPoseSets.SelectedIndex = 0;
			}
			else
			{
				poseSetControl1.Clear();
			}
		}

		private void tsAddPoseSet_Click(object sender, EventArgs e)
		{
			_character.IsDirty = true;
			PoseSet poseSet = new PoseSet();
			PoseSetEntry entry = new PoseSetEntry();
			entry.Stage = "0";
			entry.Character = _character.Character.FolderName;
			poseSet.Entries.Add(entry);
			lstPoseSets.Items.Add(poseSet);
			lstPoseSets.SelectedItem = poseSet;
			_character.CustomPoseSets.Add(poseSet);
			_character.CustomPoseSets.Sort();
			_character.Character.PoseLibrary.Add(poseSet);
			poseSetControl1.ShowLblStage();
		}

		private void tsDuplicatePoseSet_Click(object sender, EventArgs e)
		{
			if (_selectedSet == null) { return; }
			poseSetControl1.SavePoseSetEntry();

			PoseSet copy = _selectedSet.Clone() as PoseSet;
			lstPoseSets.Items.Add(copy);
			lstPoseSets.SelectedItem = copy;
			_character.CustomPoseSets.Add(copy);
		}

		public override void Save()
		{
			poseSetControl1.SavePoseSetEntry();
		}

		private void txtPoseSetRename_TextChanged(object sender, EventArgs e)
		{
			if (_selectedSet != null && !_selecting)
			{
				_character.IsDirty = true;
				_selectedSet.Id = txtPoseSetRename.Text;
				_character.Character.PoseLibrary.Rename(_selectedSet);
				lstPoseSets.RefreshListItems();
			}
		}
	}
}
