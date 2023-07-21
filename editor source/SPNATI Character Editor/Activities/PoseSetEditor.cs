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

		PoseSet _selectedSet;

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
		}

		private void lstPoseSets_SelectedIndexChanged(object sender, EventArgs e)
		{
			PoseSet poseSet = lstPoseSets.SelectedItem as PoseSet;
			if (poseSet != null)
			{
				_selectedSet = poseSet;
				txtPoseSetRename.Text = _selectedSet.Id;
				poseSetControl1.SetPoseSet(_selectedSet);
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
			_character.CustomPoseSets.Remove(_selectedSet);
			//_character.Character.PoseLibrary.Remove(_sourcePose);
			lstPoseSets.Items.Remove(_selectedSet);
			if (lstPoseSets.Items.Count > 0)
			{
				lstPoseSets.SelectedIndex = 0;
			}

		}

		private void tsAddPoseSet_Click(object sender, EventArgs e)
		{
			PoseSet poseSet = new PoseSet();
			lstPoseSets.Items.Add(poseSet);
			lstPoseSets.SelectedItem = poseSet;
			_character.CustomPoseSets.Add(poseSet);
			_character.CustomPoseSets.Sort();
			//_character.Character.PoseLibrary.Add(poseSet);
		}

		private void tsDuplicatePoseSet_Click(object sender, EventArgs e)
		{
			if (_selectedSet == null) { return; }
			SavePoseSet();

			PoseSet copy = _selectedSet.Clone() as PoseSet;
			lstPoseSets.Items.Add(_selectedSet);
			lstPoseSets.SelectedItem = copy;
			_character.CustomPoseSets.Add(copy);

		}

		private void SavePoseSet()
		{

		}

		public override void Save()
		{
	
		}

		private void txtPoseSetRename_TextChanged(object sender, EventArgs e)
		{
			if (_selectedSet != null)
			{
				_selectedSet.Id = txtPoseSetRename.Text;
			}
		}
	}
}
