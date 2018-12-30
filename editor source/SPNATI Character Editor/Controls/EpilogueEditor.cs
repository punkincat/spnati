﻿using Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Controls
{
	[Activity(typeof(Character), 40)]
	public partial class EpilogueEditor : Activity
	{
		private Character _character;
		private Epilogue _ending;
		private bool _populatingEnding;

		public EpilogueEditor()
		{
			InitializeComponent();

			Enabled = false;
		}

		public override string Caption
		{
			get
			{
				return "Epilogues";
			}
		}

		protected override void OnInitialize()
		{
			SetCharacter(Record as Character);
		}

		protected override void OnFirstActivate()
		{
			tmrActivate.Enabled = true; //for lack of a good event for when controls are in place and visible, using a timer before launching the initial epilogue
		}

		protected override void OnActivate()
		{
			Workspace.ToggleSidebar(false);
		}

		protected override void OnDeactivate()
		{
			Workspace.ToggleSidebar(true);
		}

		private void tmrActivate_Tick(object sender, EventArgs e)
		{
			tmrActivate.Enabled = false;
			EnableForEdit();
		}

		private void EnableForEdit()
		{
			Enabled = true;
			PopulateEndingCombo();

			if (_character.Endings.Count > 0)
			{
				cboEnding.SelectedIndex = 0;
			}
			else
			{
			}
		}

		public void SetCharacter(Character character)
		{
			_character = character;
			_ending = null;
		}

		private void PopulateEndingCombo()
		{
			_populatingEnding = true;
			//Endings combo
			cboEnding.Items.Clear();
			foreach (Epilogue ending in _character.Endings)
			{
				cboEnding.Items.Add(ending);
			}
			_populatingEnding = false;
		}

		private void cboEnding_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (_populatingEnding)
				return;
			Epilogue epilogue = cboEnding.SelectedItem as Epilogue;
			LoadEnding(epilogue);
		}

		private void cmdAddEnding_Click(object sender, EventArgs e)
		{
			CreateNewEnding();
		}

		private void cmdDeleteEnding_Click(object sender, System.EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete this ending?", "Delete Ending", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				RemoveEnding();
			}
		}

		private void CreateNewEnding()
		{
			Epilogue ending = new Epilogue();
			_character.Endings.Add(ending);
			PopulateEndingCombo();
			cboEnding.SelectedItem = ending;
		}

		private void LoadEnding(Epilogue ending)
		{
			SaveEnding();
			_ending = ending;
			PopulateDataFields();
			cmdDeleteEnding.Enabled = tabs.Enabled = (ending != null);
			tableGeneral.Context = new EpilogueContext(_character, _ending, null);
			tableGeneral.Data = _ending;
			canvas.SetEpilogue(_ending, _character);
			tabs.SelectedTab = (string.IsNullOrEmpty(ending.Title) || ending.Title == "New Ending" ? pageGeneral : pageScenes);
		}

		private void PopulateDataFields()
		{
			if (_ending == null)
			{
				selAllMarkers.Clear();
				selNotMarkers.Clear();
				selAllMarkers.Clear();
				selAlsoPlayingAllMarkers.Clear();
				selAlsoPlayingAnyMarkers.Clear();
				selAlsoPlayingNotMarkers.Clear();
				return;
			}

			selAllMarkers.SelectableItems
				= selNotMarkers.SelectableItems
				= selAnyMarkers.SelectableItems
				= _character.Markers.Values.ToList().ConvertAll(m => m.Name).ToArray();
			selAllMarkers.SelectedItems = _ending.AllMarkers?.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			selNotMarkers.SelectedItems = _ending.NotMarkers?.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			selAnyMarkers.SelectedItems = _ending.AnyMarkers?.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

			HashSet<string> otherMarkers = new HashSet<string>();
			foreach (Character c in CharacterDatabase.Characters)
			{
				if (c != _character)
				{
					foreach (Marker m in c.Markers.Values)
					{
						if (m.Scope == MarkerScope.Public)
						{
							otherMarkers.Add(m.Name);
						}
					}
				}
			}
			selAlsoPlayingAllMarkers.SelectableItems
				= selAlsoPlayingNotMarkers.SelectableItems
				= selAlsoPlayingAnyMarkers.SelectableItems
				= otherMarkers.ToArray();
			selAlsoPlayingAllMarkers.SelectedItems = _ending.AlsoPlayingAllMarkers?.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			selAlsoPlayingNotMarkers.SelectedItems = _ending.AlsoPlayingNotMarkers?.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			selAlsoPlayingAnyMarkers.SelectedItems = _ending.AlsoPlayingAnyMarkers?.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
		}

		public override void Save()
		{
			SaveEnding();
		}

		private void SaveEnding()
		{
			if (_ending == null)
				return;

			_ending.AllMarkers = selAllMarkers.SelectedItems.Length > 0 ? string.Join(" ", selAllMarkers.SelectedItems) : null;
			_ending.NotMarkers = selNotMarkers.SelectedItems.Length > 0 ? string.Join(" ", selNotMarkers.SelectedItems) : null;
			_ending.AnyMarkers = selAnyMarkers.SelectedItems.Length > 0 ? string.Join(" ", selAnyMarkers.SelectedItems) : null;
			_ending.AlsoPlayingAllMarkers = selAlsoPlayingAllMarkers.SelectedItems.Length > 0 ? string.Join(" ", selAlsoPlayingAllMarkers.SelectedItems) : null;
			_ending.AlsoPlayingNotMarkers = selAlsoPlayingNotMarkers.SelectedItems.Length > 0 ? string.Join(" ", selAlsoPlayingNotMarkers.SelectedItems) : null;
			_ending.AlsoPlayingAnyMarkers = selAlsoPlayingAnyMarkers.SelectedItems.Length > 0 ? string.Join(" ", selAlsoPlayingAnyMarkers.SelectedItems) : null;
		}

		private void RemoveEnding()
		{
			if (_character == null || _ending == null)
				return;
			_character.Endings.Remove(_ending);
			_ending = null;
			LoadEnding(null);
			PopulateEndingCombo();
			if (_character.Endings.Count > 0)
			{
				cboEnding.SelectedIndex = 0;
			}
		}
	}
}
