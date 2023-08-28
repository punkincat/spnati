using Desktop;
using SPNATI_Character_Editor.DataStructures;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Activities
{
	[Spacer]
	[Activity(typeof(Character), 1000, DelayRun = true, Caption = "Configuration")]
	public partial class CharacterConfiguration : Activity
	{
		private Character _character;
		private CharacterEditorData _editorData;
		bool _initialized = false;
		bool _selecting = true;
		private CharacterSettingsGroup _selectedGroup;

		public override string Caption
		{
			get { return "Configuration"; }
		}

		public CharacterConfiguration()
		{
			InitializeComponent();
			recMarker.RecordType = typeof(Marker);
		}

		protected override void OnInitialize()
		{
			_character = Record as Character;
			_editorData = CharacterDatabase.GetEditorData(_character);
			characterSettingControl1.SetCharacter(_character);
			recMarker.RecordContext = _character;
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
			characterSettingControl1.Activate();
			RebuildSettingsList();
		}

		private void RebuildSettingsList()
		{
			lstGroups.Items.Clear();
			foreach (CharacterSettingsGroup group in _character.Behavior.CharacterSettingsGroups)
			{
				lstGroups.Items.Add(group);
			}
			lstGroups.Sorted = true;

			if (lstGroups.Items.Count > 0)
			{
				lstGroups.SelectedIndex = 0;
			}
			_initialized = true;

		}

		private void lstGroups_SelectedIndexChanged(object sender, EventArgs e)
		{
			CharacterSettingsGroup group = lstGroups.SelectedItem as CharacterSettingsGroup;
			if (group != null)
			{
				if (_initialized)
				{
					characterSettingControl1.SaveCharacterSetting();
				}
				_selectedGroup = group;
				_selecting = true;
				txtRename.Text = _selectedGroup.Id;
				recMarker.RecordKey = _selectedGroup.Marker;
				characterSettingControl1.SetGroup(_selectedGroup);
				_selecting = false;
			}
		}

		public override void Save()
		{
			characterSettingControl1.SaveCharacterSetting();
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


		private void tsRemoveGroup_Click(object sender, EventArgs e)
		{
			if (_selectedGroup == null ||
	MessageBox.Show($"Are you sure you want to permanently delete {_selectedGroup}? This operation cannot be undone.",
			"Remove Character Settings Group", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
			{
				return;
			}
			_character.IsDirty = true;
			_character.Behavior.CharacterSettingsGroups.Remove(_selectedGroup);
			lstGroups.Items.Remove(_selectedGroup);
			if (lstGroups.Items.Count > 0)
			{
				lstGroups.SelectedIndex = 0;
			}
			else
			{
				characterSettingControl1.Clear();
			}
		}

		private void tsAddGroup_Click(object sender, EventArgs e)
		{
			_character.IsDirty = true;
			CharacterSettingsGroup group = new CharacterSettingsGroup();
			CharacterSetting setting = new CharacterSetting();
			group.CharacterSettings.Add(setting);
			lstGroups.Items.Add(group);
			lstGroups.SelectedItem = group;
			_character.Behavior.CharacterSettingsGroups.Add(group);
			_character.Behavior.CharacterSettingsGroups.Sort();
		}


		private void tsDuplicateGroup_Click(object sender, EventArgs e)
		{
			if (_selectedGroup == null) { return; }
			characterSettingControl1.SaveCharacterSetting();

			CharacterSettingsGroup copy = _selectedGroup.Clone() as CharacterSettingsGroup;
			lstGroups.Items.Add(copy);
			lstGroups.SelectedItem = copy;
			_character.Behavior.CharacterSettingsGroups.Add(copy);
		}
		private void txtRename_TextChanged(object sender, EventArgs e)
		{
			if (_selectedGroup != null && !_selecting)
			{
				_character.IsDirty = true;
				_selectedGroup.Id = txtRename.Text;
				lstGroups.RefreshListItems();
			}
		}

		private void recMarker_RecordChanged(object sender, Desktop.CommonControls.RecordEventArgs e)
		{
			_selectedGroup.Marker = recMarker.RecordKey;
		}
	}
}
