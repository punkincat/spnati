using Desktop;
using Desktop.CommonControls;
using Desktop.Skinning;
using SPNATI_Character_Editor.Forms;
using System;
using System.Collections.Generic;
using System.IO;
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
//			tablePoseSets.Context = character;
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
			}
			_selectedEntry = _selectedSet.Entries[0];
			PopulateImageDropdown(_selectedEntry.Stage);
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
			//HashSet<int> imageStages = new HashSet<int>(selectedStages, selectedStages.Comparer);

			List<PoseMapping> poses = new List<PoseMapping>();
			for (int i = 0; i < selectedStages.Count; i++)
			{
				foreach (PoseMapping pose in _character.Character.PoseLibrary.GetPoses(selectedStages[i]))
				{
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
					cboPose.Items.Add(image);
				}
			}
		}


		/*
		public void UpdateAvailableImages(string stages, bool retainValue)
		{
			if (_character == null || _selectedEntry == null) { return; }
			HashSet<int> selectedStages = new HashSet<int>();
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
			HashSet<int> imageStages = new HashSet<int>(selectedStages, selectedStages.Comparer);

			/*
			List<object> values = new List<object>();
			if (retainValue)
			{
				//save off values
				for (int i = 0; i < gridDialogue.Rows.Count; i++)
				{
					DataGridViewRow row = gridDialogue.Rows[i];
					values.Add(row.Cells["ColImage"].Value);
				}
			}
			*/

		//			bool hasStageImages = _selectedCase.Lines.Find(l => l.Images.Count > 0) != null;
		//			int stageId = _selectedStage == null ? 0 : _selectedStage.Id;
		/*
					cboPose.Items.Clear();
					HashSet<PoseMapping> images = new HashSet<PoseMapping>();
					if (selectedStages == null)
					{
						images.AddRange(_character.Character.PoseLibrary.GetPoses(0));
						foreach (var image in images)
						{
							col.Items.Add(image);
						}
					}
					else
					{
						if (hasStageImages)
						{
							foreach (int selectedStage in _selectedEntry.Stage)
							{
								images.AddRange(_character.Character.PoseLibrary.GetPoses(selectedStage));
							}
						}
						else
						{
							images.AddRange(_character.Character.PoseLibrary.GetPoses(stageId));
						}

						foreach (PoseMapping image in images)
						{
							bool isGeneric = image.IsGeneric;
							bool allExist = true;
							if (!isGeneric && !hasStageImages)
							{
								foreach (int stage in imageStages)
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
								col.Items.Add(image);
							}
						}
					}

					foreach (DataGridViewRow row in gridDialogue.Rows)
					{
						SkinnedDataGridViewComboBoxCell cellCol = row.Cells[ColImage.Index] as SkinnedDataGridViewComboBoxCell;
						if (cellCol != null)
						{
							object old = cellCol.Value;
							cellCol.Items.Clear();
							foreach (object item in col.Items)
							{
								cellCol.Items.Add(item);
							}
							cellCol.Value = old;
						}
					}
					col.DisplayMember = "DefaultName";

					if (retainValue)
					{
						//restore values
						for (int i = 0; i < gridDialogue.Rows.Count; i++)
						{
							DataGridViewRow row = gridDialogue.Rows[i];

							//Make sure the value is still valid
							bool found = false;
							foreach (var item in col.Items)
							{
								PoseMapping img = item as PoseMapping;
								PoseMapping oldImg = values[i] as PoseMapping;
								if (oldImg == img)
								{
									row.Cells["ColImage"].Value = item;
									found = true;
									break;
								}
							}
							if (!found)
							{
								row.Cells["ColImage"].Value = null;
							}
						}
					}
				}
		*/

		private void tabsPoseSetEntries_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_selectedSet == null) { return; }
			int index = tabsPoseSetEntries.SelectedIndex;
			
			_selectedEntry = _selectedSet.Entries[index];
			PopulatePoseEntryTable();
		}

		private void AddSetEntryTab()
		{
			tabsPoseSetEntries.TabPages.Add($"Pose {tabsPoseSetEntries.TabPages.Count + 1}");
		}

		private void PopulatePoseEntryTable()
		{
			_selectedEntry.Character = _character.Character.ToString();
			if (tablePoseSetEntry.Data != _selectedEntry)
			{
				tablePoseSetEntry.Data = _selectedEntry;
				AddSpeedButtonsSet(tablePoseSetEntry);
			}
		}

		public bool AutoOpenConditions
		{
			set { tablePoseSetEntry.RunInitialAddEvents = value; }
		}



		public void Save()
		{
			SavePoseSet();
		}

		public void SavePoseSetEntry()
		{

		}


		public static void AddSpeedButtonsSet(PropertyTable table)
		{
			//table.AddSpeedButton("Property", "Img", (data) => { return AddFilter("property", data, "Img"); });
			table.AddSpeedButton("Property", "Stage", (data) => { return AddFilter("self", data, "Stage"); });
			//table.AddSpeedButton("Property", "Location", (data) => { return AddFilter("property", data, "Location"); });
			//table.AddSpeedButton("Property", "Direction", (data) => { return AddFilter("property", data, "Direction"); });
			//table.AddSpeedButton("Property", "Dialogue-Layer", (data) => { return AddFilter("property", data, "Dialogue-Layer"); });

			//table.AddSpeedButton("Conditions", "Variable Test", (data) => { return AddVariableTest("~_~", data); });
			//table.AddSpeedButton("Conditions", "Marker", (data) => { return AddVariableTest("~_.marker.*~", data); });
			//table.AddSpeedButton("Conditions", "Priority", (data) => { return AddFilter("condition", data, "Priority"); });
			//table.AddSpeedButton("Conditions", "Weight", (data) => { return AddFilter("condition", data, "Weight"); });
		}

		private static SpeedButtonData AddVariableTest(string variable, object data)
		{
			PoseSetEntry theEntry = data as PoseSetEntry;
			theEntry.Tests.Add(new ExpressionTest(variable, ""));
			return new SpeedButtonData("Expressions");
		}

		private static SpeedButtonData AddFilter(string role, object data, string subproperty = null, Character character = null)
		{
			PoseSetEntry entry = data as PoseSetEntry;

			//Case theCase = new Case();
			//TargetCondition condition = theCase.Conditions.Find(c => c.Role == role && (character == null || c.Character == character.FolderName));
			//if (subproperty == null)
			//{
			//	condition = null; //always create new filters for roles
			//}
			//if (entry.HasProperty(subproperty)
			//TargetCondition condition = new TargetCondition();
			/*if (condition == null || condition.HasProperty(subproperty))
			{
				condition = new TargetCondition();
				condition.Role = role;
				theCase.Conditions.Add(condition);
				if (character != null)
				{
					condition.Character = character.FolderName;
				}
			}
			*/


			return new SpeedButtonData("Property", subproperty) { ListItem = entry };
		}

		public Func<PropertyRecord, bool> GetRecordFilter(object data)
		{
			Case tag = data as Case;
			TriggerDefinition trigger = TriggerDatabase.GetTrigger(tag.Tag);
			return null;
		}


		private void SavePoseSet()
		{
			if (_selectedSet == null)
			{
				return;
			}

			var s = _selectedSet;

			tablePoseSetEntry.Save();
		}




		public SkinnedBackgroundType PanelType
		{
			get { return SkinnedBackgroundType.Background; }
		}


		public object CreateData()
		{
			PoseSetEntry poseEntry = new PoseSetEntry();
			//tag.AddStages(_selectedSet.Stages);
			return poseEntry;
		}

		public object GetRecordContext()
		{
			return _character;
		}



		private void stripPoseSetEntries_AddButtonClicked(object sender, EventArgs e)
		{
			if (_selectedSet == null) { return; }
			PoseSetEntry entry = new PoseSetEntry();
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
			_selectedEntry = _selectedSet.Entries[tabsPoseSetEntries.SelectedIndex];
			PopulatePoseEntryTable();

		}
	}
}
