using Desktop.Skinning;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Controls
{
	public partial class PoseSetControl : UserControl, ISkinnedPanel, ISkinControl
	{
		private ISkin _character;
		private PoseSet _selectedSet;
		private PoseSetEntry _selectedEntry;

		public PoseSetControl()
		{
			InitializeComponent();
			cboDirection.DataSource = DialogueLine.ArrowDirections;
		}

		public void OnUpdateSkin(Skin skin)
		{
			BackColor = skin.Background.GetColor(VisualState.Normal, false, Enabled);
		}

		public void Activate()
		{
		}

		public void SetCharacter(ISkin character)
		{
			_character = character;
			lblStage.Visible = _character.CustomPoseSets.Count > 0;
//			tablePoseSets.Context = character;
		}
		public void Clear()
		{
			lblStage.Visible = false;
			tablePoseSetEntry.Data = null;
			cboPose.DataSource = null;
			chkLayer.Checked = false;
			valWeight.Value = (decimal) 0.001;
			valPriority.Value = 0;
			valLocation.Value = 50;
			cboDirection.Text = "";
		}

		public void ShowLblStage()
		{
			if (!lblStage.Visible)
			{
				lblStage.Visible = true;
			}
		}

		public void SetPoseSet(PoseSet workingSet)
		{
			if (_selectedSet != null)
			{
				tabsPoseSetEntries.SelectedIndexChanged -= tabsPoseSetEntries_SelectedIndexChanged;
				tabsPoseSetEntries.SelectedIndex = 0;
				for (int i = tabsPoseSetEntries.TabPages.Count - 1; i > 0; i--)
				{
					tabsPoseSetEntries.TabPages.RemoveAt(i);
				}
			}
			_selectedSet = workingSet;
			if (_selectedSet != null)
			{
				for (int i = 0; i < _selectedSet.Entries.Count - 1; i++)
				{
					AddSetEntryTab();
				}
				tabsPoseSetEntries.SelectedIndexChanged += tabsPoseSetEntries_SelectedIndexChanged;
				_selectedEntry = _selectedSet.Entries[0];
				SetPoseSetEntry();
			}
		}

		private void SetPoseSetEntry()
		{
			PopulateImageDropdown(_selectedEntry.Stage);
			if (!string.IsNullOrEmpty(_selectedEntry.Img))
			{
				PoseMapping pose = _character.Character.PoseLibrary.GetPose(_selectedEntry.Img);
				cboPose.SelectedItem = pose;
			}
			chkLayer.Checked = _selectedEntry.DialogueLayer == "over" || (_character.Character.Metadata.BubblePosition.ToString() == "over" && _selectedEntry.DialogueLayer != "under");
			valWeight.Value = Math.Max(valWeight.Minimum, Math.Min(valWeight.Maximum, (decimal)_selectedEntry.Weight));
			float location;
			string loc = _selectedEntry.Location;
			if (loc != null && loc.EndsWith("%"))
			{
				loc = loc.Substring(0, loc.Length - 1);
			}
			if (!float.TryParse(loc, NumberStyles.Number, CultureInfo.InvariantCulture, out location))
			{
				location = 50;
			}
			valLocation.Value = Math.Max(valLocation.Minimum, Math.Min(valLocation.Maximum, (decimal)location));
			cboDirection.Text = _selectedEntry.Direction ?? "";
			valPriority.Value = Math.Max(valPriority.Minimum, Math.Min(valPriority.Maximum, _selectedEntry.Priority));
			PopulatePoseEntryTable();
		}

		private void PopulateImageDropdown(string stages)
		{
			if (_character == null || _selectedEntry == null) { return; }
			List<int> selectedStages = new List<int>();
			if (int.TryParse(stages, out int x))
			{
				selectedStages.Add(x);
			}
			else
			{
				string[] strings = stages.Split('-');
				if (strings.Length != 2)
				{
					return;
				}
				if (int.TryParse(strings[0], out int y) && int.TryParse(strings[1], out int z))
				{
					for (int i = y; i <= z; i++)
					{
						selectedStages.Add(i);
					}
				}
				else
				{
					return;
				}
			}

			List<PoseMapping> poses = new List<PoseMapping>();
			List<PoseMapping> existingPoses = new List<PoseMapping>();
			for (int i = 0; i < selectedStages.Count; i++)
			{
				foreach (PoseMapping pose in _character.Character.PoseLibrary.GetPoses(selectedStages[i], true))
				{
					if (!poses.Contains(pose))
						poses.Add(pose);
				}
			}
			foreach (PoseMapping image in poses)
			{
				bool isGeneric = image.IsGeneric;
				bool allExist = true;
				if (!isGeneric)
				{
					foreach (int stage in selectedStages)
					{
						if (!image.ContainsStage(stage))
						{
							allExist = false;
							break;
						}
					}
				}
				if (allExist)
				{
					existingPoses.Add(image);
				}
			}
			cboPose.DataSource = existingPoses;
		}

		private void tabsPoseSetEntries_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_selectedSet == null) { return; }
			SavePoseSetEntry();
			int index = tabsPoseSetEntries.SelectedIndex;
			
			_selectedEntry = _selectedSet.Entries[index];
			SetPoseSetEntry();
		}

		private void AddSetEntryTab()
		{
			tabsPoseSetEntries.TabPages.Add($"Pose {tabsPoseSetEntries.TabPages.Count + 1}");
		}

		private void PopulatePoseEntryTable()
		{
			_selectedEntry.Character = _character.Character.FolderName;
			if (tablePoseSetEntry.Data != _selectedEntry)
			{
				tablePoseSetEntry.Data = _selectedEntry;
				//AddSpeedButtonsSet(tablePoseSetEntry);
			}
		}

		//public void Save()
		//{
		//}

		public void SavePoseSetEntry()
		{
			if (_selectedEntry == null) { return; }
			_character.IsDirty = true;
			_selectedEntry.Weight = (float)valWeight.Value == 0.001f ? 0f : (float)valWeight.Value;
			_selectedEntry.Priority = (int) valPriority.Value;
			_selectedEntry.Direction = cboDirection.Text;
			PoseMapping image = cboPose.SelectedItem as PoseMapping;
			if (image == null)
				return;
			_selectedEntry.Img = image.Key;
			tablePoseSetEntry.Save();
		}


		public SkinnedBackgroundType PanelType
		{
			get { return SkinnedBackgroundType.Background; }
		}


		/*public object CreateData()
		{
			PoseSetEntry poseEntry = new PoseSetEntry();
			return poseEntry;
		}*/

		private void stripPoseSetEntries_AddButtonClicked(object sender, EventArgs e)
		{
			if (_selectedSet == null) { return; }
			PoseSetEntry entry = new PoseSetEntry();
			entry.Stage = "0";
			entry.Character = _character.FolderName;
			_selectedSet.Entries.Add(entry);
			AddSetEntryTab();
			tabsPoseSetEntries.SelectedIndex = tabsPoseSetEntries.TabPages.Count;
		}

		private void stripPoseSetEntries_CloseButtonClicked(object sender, EventArgs e)
		{
			if (_selectedSet == null) { return; }
			int index = tabsPoseSetEntries.SelectedIndex - 1;
			if (index >= 0)
			{
				_selectedSet.Entries.RemoveAt(index + 1);
				tabsPoseSetEntries.TabPages.RemoveAt(index + 1);
				for (int i = index; i < tabsPoseSetEntries.TabPages.Count; i++)
				{
					tabsPoseSetEntries.TabPages[i].Text = "Pose " + (i + 1);
				}
				tabsPoseSetEntries.SelectedIndex = index < tabsPoseSetEntries.TabPages.Count - 1 ? index + 1 : index;
			}
		}

		private void stripPoseSetEntries_TabIndexChanged(object sender, EventArgs e)
		{
			SavePoseSetEntry();
			_selectedEntry = _selectedSet.Entries[tabsPoseSetEntries.SelectedIndex];
			SetPoseSetEntry();
		}
		private void chkLayer_CheckedChanged(object sender, EventArgs e)
		{
			if (_character.Character.Metadata.BubblePosition == DialogueLayer.over)
			{
				_selectedEntry.DialogueLayer = chkLayer.Checked ? "" : "under";
			}
			else
			{
				_selectedEntry.DialogueLayer = chkLayer.Checked ? "over" : "";
			}
		}
	}
}
