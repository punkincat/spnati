using Desktop;
using Desktop.CommonControls;
using Desktop.Skinning;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SPNATI_Character_Editor.Activities
{
	[Spacer]
	[Activity(typeof(Character), 300, DelayRun = true, Caption = "Banter Wizard")]
	[Tutorial("https://youtu.be/wyKiC3bMbeY?t=19")]
	public partial class BanterWizard : Activity
	{
		private Character _character;
		private IWardrobe _costume;
		private Case _workingResponse;
		private Case _selectedCase;
		private Character _selectedCharacter;
		public bool Modified { get; private set; }
		private int _oldSplitter;
		private bool _editing;
		private bool _loading;
		private InboundLine _currentInbound;
		private string _path;

		private bool _filterToColor;
		private string _colorFilter;
		private bool _filterNew;
		private bool _filterModText;
		private bool _filterModCond;
		private bool _filterOld;

		private bool _filterMain;
		private bool _filterTesting;
		private bool _filterOffline;
		private bool _filterIncomplete;
		private bool _filterEvent;
		private bool _filterDuplicate;
		private bool _filterUnlisted;

		private bool _filterToOne;
		private Character _oneCharacter;

		public BanterWizard()
		{
			InitializeComponent();
			recOneCharacter.RecordType = typeof(Character);
			recReferenceCostume.RecordType = typeof(Costume);
		}

		public override string Caption
		{
			get
			{
				return "Banter Wizard";
			}
		}
		private bool FilterRefCostume(IRecord record)
		{
			Costume costume = record as Costume;
			return costume.Character == _character || costume.Key == "default";
		}

		private void recReferenceCostume_RecordChanged(object sender, RecordEventArgs e)
		{
			Costume costume = recReferenceCostume.Record as Costume;
			if (costume.Key == "default")
			{
				_costume = _character;
			}
			else
			{
				_costume = costume;
			}
		}

		public override bool CanRun()
		{
			return !Config.SafeMode;
		}

		protected override void OnInitialize()
		{
			_character = Record as Character;
			_character.IsDirty = true;
			_path = Path.Combine(Config.GetString(Settings.GameDirectory), "opponents/" + _character.FolderName + "/banter.xml");
			recReferenceCostume.RecordFilter = FilterRefCostume;
			recReferenceCostume.Record = CharacterDatabase.GetSkin("default");
			ColJump.Flat = true;
			if(!File.Exists(_path))
			{
				cmdUpdateBanter.Text = "Generate";
				cmdSaveBanter.Enabled = false;
				cmdLoadBanter.Enabled = false;
			}
			else
			{
				cmdUpdateBanter.Text = "Generate/Update";
				toolTip1.SetToolTip(this.cmdUpdateBanter, "Update banter data. Use this after pulling other characters' updates from Git.\nYour unsaved changes will be discarded.");
				cmdSaveBanter.Enabled = true;
				cmdLoadBanter.Enabled = true;
			}
			cmdCreateResponse.Visible = false;
			panelLoad.BringToFront();
			progressBar.Visible = false;
		}

		protected override void OnFirstActivate()
		{
			ctlResponse.SetCharacter(_character);
			HideResponses();
			lblCharacters.Text = string.Format(lblCharacters.Text, _character);
			lstCharacters.Sorted = true;
			_filterToColor = false;
			chkColorFilter.Checked = false;
			_filterToOne = false;
			chkOneCharacter.Checked = false;
			_filterNew = true;
			_filterModText = true;
			_filterModCond = true;
			_filterOld = true;
			chkLineFiltering.SetItemChecked(0, true);
			chkLineFiltering.SetItemChecked(1, true);
			chkLineFiltering.SetItemChecked(2, true);
			chkLineFiltering.SetItemChecked(3, true);

			_filterMain = true;
			_filterTesting = true;
			_filterOffline = false;
			_filterIncomplete = false;
			_filterEvent = false;
			_filterDuplicate = false;
			_filterUnlisted = false;
			chkCharacterFiltering.SetItemChecked(0, true);
			chkCharacterFiltering.SetItemChecked(1, true);
			chkCharacterFiltering.SetItemChecked(2, false);
			chkCharacterFiltering.SetItemChecked(3, false);
			chkCharacterFiltering.SetItemChecked(4, false);
			chkCharacterFiltering.SetItemChecked(5, false);
			chkCharacterFiltering.SetItemChecked(6, false);
		}

		protected override void OnActivate()
		{
			if (!File.Exists(_path))
			{
				cmdUpdateBanter.Text = "Generate";
				cmdSaveBanter.Enabled = false;
				cmdLoadBanter.Enabled = false;
			}
			else
			{
				cmdUpdateBanter.Text = "Generate/Update";
				cmdSaveBanter.Enabled = true;
				cmdLoadBanter.Enabled = true;
			}
			if (_selectedCharacter != null && _currentInbound != null)
			{
				int stage;
				if (_currentInbound.StageRange == "10")
				{
					stage = 10;
				}
				else
				{
					stage = int.Parse(_currentInbound.StageRange[0].ToString());
				}

				PoseMapping pose = _selectedCharacter.PoseLibrary.GetPose(_currentInbound.Img);
				if (pose != null)
				{
					Workspace.SendMessage(WorkspaceMessages.UpdatePreviewImage, new UpdateImageArgs(_selectedCharacter, pose, stage));
				}

				DialogueLine dialogueLine = new DialogueLine();
				dialogueLine.Text = _currentInbound.Text;
				Workspace.SendMessage(WorkspaceMessages.PreviewLine, dialogueLine);
			}
		}

		protected override void OnParametersUpdated(params object[] parameters)
		{
		}

		public override bool CanQuit(CloseArgs args)
		{
			if (_loading)
			{
				return false;
			}
			return base.CanQuit(args);
		}

		public override bool CanDeactivate(DeactivateArgs args)
		{
			if (_loading)
			{
				return false;
			}
			return base.CanDeactivate(args);
		}

		protected override void OnDeactivate()
		{
			if (_character != null)
			{
				Workspace.SendMessage(WorkspaceMessages.PreviewCharacterChanged, _character);
			}
		}

		private void lstCharacters_SelectedIndexChanged(object sender, EventArgs e)
		{
			Character character = lstCharacters.SelectedItem as Character;
			if (character == null)
			{
				cmdCreateResponse.Visible = false;
			}
			else 
			{
				_selectedCharacter = lstCharacters.SelectedItem as Character;
				BanterTargetedLines(_selectedCharacter);
				Workspace.SendMessage(WorkspaceMessages.PreviewCharacterChanged, _selectedCharacter);
				SelectLine(0);
				cmdCreateResponse.Visible = true;
			}
		}
		private bool CharacterFiltering(string folderName)
		{
			string status = Listing.Instance.GetCharacterStatus(folderName);

			if (status == OpponentStatus.Main)
				return _filterMain;
			if (status == OpponentStatus.Testing)
				return _filterTesting;
			if (status == OpponentStatus.Offline)
				return _filterOffline;
			if (status == OpponentStatus.Incomplete)
				return _filterIncomplete;
			if (status == OpponentStatus.Event)
				return _filterEvent;
			if (status == OpponentStatus.Duplicate)
				return _filterDuplicate;
			if (status == OpponentStatus.Unlisted)
				return _filterUnlisted;
			return true;
		}

		private bool LineFiltering(InboundLine line)
		{
			if (_filterToColor)
			{
				if (line.ColorCode != _colorFilter)
				{
					return false;
				}
			}
			if (string.IsNullOrEmpty(line.Newness))
				return _filterOld;
			if (line.Newness == "N")
				return _filterNew;
			if (line.Newness == "T")
				return _filterModText;
			if (line.Newness == "C")
				return _filterModCond;
			if (line.Newness == "B")
				return _filterModText || _filterModCond;
			return true;
		}

		private void BanterTargetedLines(Character loaded)
		{
			if (loaded == null)
				return;
			grpLines.Text = "Lines spoken by " + loaded;
			HideResponses();
			gridLines.Rows.Clear();
			TargetingCharacter targeter = _character.BanterData.TargetingCharacters.Find(x => x.Id == loaded.FolderName);
			int count = 0;
			foreach (InboundLine line in targeter.Inbounds)
			{
				if (LineFiltering(line))
				{
					DataGridViewRow row = gridLines.Rows[gridLines.Rows.Add()];
					row.Tag = loaded;
					row.Cells["ColNewness"].Value = line.Newness;
					row.Cells["ColText"].Value = line.Text;
					row.Cells["ColText"].Tag = line;
					row.Cells["ColStage"].Value = line.StageRange;
					row.Cells["ColCase"].Value = line.CaseTag;
					count++;
				}
			}
			gridLines.Visible = (count > 0);
			lblNoMatches.Visible = (count == 0);
		}


		private void gridLines_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			SelectLine(e.RowIndex);
		}

		private void ShowBasicLines(Character character, Case workingCase)
		{

			//show the default lines that the character is probably reacting to
			lstBasicLines.Items.Clear();

			List<Case> possibleCases = Case.GetMatchingCases(workingCase, character, _character);
			if (possibleCases.Count == 0) { return; }

			int index = -1;

			for (int i = 0; i < possibleCases.Count; i++)
			{
				if (Character.IsCaseTargetedAtCharacter(possibleCases[i], character, TargetType.DirectTarget))
				{
					index = i;
					break;
				}

				foreach (DialogueLine ln in possibleCases[i].Lines)
				{
					if (!string.IsNullOrEmpty(ln.Marker) && workingCase.Conditions.Find((c => c.Character == _character.FolderName && c.SayingMarker == ln.Marker)) != null)
					{
						index = i;
						break;
					}

					foreach (MarkerOperation marker in ln.Markers)
					{
						if (workingCase.Conditions.Find((c => c.Character == _character.FolderName && c.SayingMarker == marker.Name)) != null)
						{
							index = i;
							break;
						}
					}

					if (index != -1) { break; }

					if (workingCase.Conditions.Find((c => c.Character == _character.FolderName && c.Saying == ln.Text)) != null)
					{
						index = i;
						break;
					}
				}

				if (index != -1) { break; }
			}

			if (index != -1)
			{
				lblBasicText.Text = possibleCases[index].ToString();
				foreach (var line in possibleCases[index].Lines)
				{
					lstBasicLines.Items.Add(line);
				}
			}
			return;
		}

		private void RenameCaseTag(Case workingCase, DataGridViewRow row)
		{
			bool removing;
			bool lookForward;
			removing = workingCase.Tag.Contains("removing_");
			lookForward = removing || workingCase.Tag == "opponent_stripping";
			string stages = "";
			int stage;
			foreach (TargetCondition cond in workingCase.Conditions)
			{
				if (cond.Character == _character.FolderName && !string.IsNullOrEmpty(cond.Stage))
				{
					stages = cond.Stage;
					break;
				}
			}
			if (!string.IsNullOrEmpty(stages))
			{
				if (stages == "10")
				{
					stage = 10;
				}
				else
				{
					stage = int.Parse(stages[0].ToString());
				}
				row.Cells["ColCase"].Value = _character.LayerToStageName(stage, lookForward, _costume) + " (" + stages + ")";
			}
		}

		private void CheckForResponses(Character character, string text, DataGridViewRow row)
		{
			foreach (Case workingCase in character.Behavior.GetWorkingCases())
			{
				foreach (DialogueLine dialogueLine in workingCase.Lines)
				{
					if (dialogueLine.Text == text)
					{
						if (Character.IsCaseTargetedAtCharacter(workingCase, _character, TargetType.DirectTarget))
						{
							RenameCaseTag(workingCase, row);
						}
						Case sampleResponse = workingCase.CreateResponse(character, _character);
						if (sampleResponse == null)
						{
							cmdCreateResponse.Enabled = false; //can't respond to this line
							lstBasicLines.Items.Clear();
						}
						else
						{
							cmdCreateResponse.Enabled = true;
							bool hasResponses = false;
							foreach (Case workingCase1 in _character.Behavior.GetWorkingCases())
							{
								if (workingCase1.MatchesConditions(sampleResponse))
								{
									//Found one
									ShowResponse(workingCase1, false);
									hasResponses = true;
									break;
								}
							}

							if (!hasResponses)
							{
								ShowBasicLines(character, workingCase);
							}

							_selectedCase = workingCase;
						}

						return;
					}
				}
			}
		}

		private void SelectLine(int rowIndex)
		{
			DataGridViewRow row = gridLines.Rows[rowIndex];
			Character c = row.Tag as Character;
			InboundLine inbound = row.Cells["ColText"].Tag as InboundLine;
			HideResponses();
			if (inbound != null)
			{
				_currentInbound = inbound;
				SetColorButton(inbound.ColorCode);
				grpBaseLine.Text = string.Format("{0} may be reacting to these lines from {1}:", c, _character);
				CheckForResponses(c, inbound.Text, row);

				int stage;
				if (inbound.StageRange == "10")
				{
					stage = 10;
				}
				else
				{
					stage = int.Parse(inbound.StageRange[0].ToString());
				}

				PoseMapping pose = c.PoseLibrary.GetPose(inbound.Img);
				if (pose != null)
				{
					Workspace.SendMessage(WorkspaceMessages.UpdatePreviewImage, new UpdateImageArgs(c, pose, stage));
				}
				DialogueLine dialogueLine = new DialogueLine();
				dialogueLine.Text = inbound.Text;
				Workspace.SendMessage(WorkspaceMessages.PreviewLine, dialogueLine);
			}
			else
			{
				lstBasicLines.Items.Clear();
			}
		}

		private void gridResponse_HighlightRow(object sender, int index)
		{
			if (index == -1)
				return;
			PoseMapping image;
			if (ctlResponse.Visible)
			{
				image = ctlResponse.GetImage(index);
				Workspace.SendMessage(WorkspaceMessages.PreviewLine, ctlResponse.GetLine(index));
			}
			else
			{
				image = gridResponse.GetImage(index);
				Workspace.SendMessage(WorkspaceMessages.PreviewLine, gridResponse.GetLine(index));
			}
			if (image != null)
			{
				Workspace.SendMessage(WorkspaceMessages.UpdatePreviewImage, new UpdateImageArgs(_character, image, _workingResponse.Stages[0]));
			}
		}

		private void cmdCreateResponse_Click(object sender, EventArgs e)
		{
			if (_workingResponse == null)
			{
				CreateResponse();
			}
		}
		
		private void CreateResponse()
		{
			if (_selectedCase == null)
				return;
			Modified = true;

			Case caseToRespondTo = _selectedCase;

			bool hasTarget = false;
			bool hasFilter = false;

			foreach (TargetCondition condition in _selectedCase.Conditions)
			{
				if (condition.Role == "target" && condition.Character == _character.FolderName) { hasTarget = true; }
				if (condition.Role == "target" && condition.FilterTag == _character.FolderName) { hasFilter = true; }
			}

			if (hasFilter && !hasTarget)
			{
				//If making a response to a line that has a filter but no target, assume they're targeting you directly
				caseToRespondTo = _selectedCase.CopyConditions();
				TargetCondition theCondition = new TargetCondition();
				theCondition.Role = "target";
				theCondition.Character = _character.FolderName;
				caseToRespondTo.Conditions.Add(theCondition);
				caseToRespondTo.StageRange = _selectedCase.StageRange;
			}

			Case response = caseToRespondTo.CreateResponse(_selectedCharacter, _character);
			if (response == null)
				return;
			ShowResponse(response, true);
		}

	
		private void HideResponses()
		{
			cmdCreateResponse.Text = "Create Response";
			splitContainer3.Panel2Collapsed = true;
			splitContainer3.Panel1Collapsed = false;
			if (_workingResponse != null)
			{
				if (gridResponse.Visible)
				{
					gridResponse.Save();
					gridResponse.Clear();
				}
				else if (_editing)
				{
					ctlResponse.SetCase(null, null);
					_editing = false;
					gridLines.Enabled = true;
					splitContainer2.SplitterDistance = _oldSplitter;
				}
				_workingResponse = null;
				_selectedCase = null;
			}
		}
		
		private void ShowResponse(Case response, bool editing)
		{
			HashSet<int> selectedStages = new HashSet<int>();
			foreach (int stage in response.Stages)
			{
				selectedStages.Add(stage);
			}
			_workingResponse = response;
			if (editing)
			{
				gridResponse.Visible = false;
				ctlResponse.Visible = true;
				ctlResponse.SetCase(new Stage(response.Stages[0]), response);
			}
			else
			{
				gridResponse.Visible = true;
				ctlResponse.Visible = false;
				Stage stage = new Stage(response.Stages[0]);
				gridResponse.SetData(_character, stage, response, selectedStages);
			}			
			
			grpResponse.Text = $"Response from {_character}";
			splitContainer3.Panel2Collapsed = false;
			splitContainer3.Panel1Collapsed = true;
			cmdAccept.Enabled = editing;
			cmdDiscard.Enabled = editing;

			cmdCreateResponse.Enabled = false;
			if (editing)
			{
				_editing = true;
				_oldSplitter = splitContainer2.SplitterDistance;
				splitContainer2.SplitterDistance = 110;
				if (gridLines.SelectedCells.Count > 0)
				{
					gridLines.FirstDisplayedScrollingRowIndex = gridLines.SelectedCells[0].RowIndex;
				}
				gridLines.Enabled = false;
			}
		}

		public override void Save()
		{
			if (_workingResponse != null)
			{
				ctlResponse.Save();
			}
		}

		private void cmdJump_Click(object sender, EventArgs e)
		{
			if (_editing)
			{
				ctlResponse.Save();
				_character.Behavior.AddWorkingCase(_workingResponse);
			}
			else
			{
				gridResponse.Save();
			}
			Shell.Instance.Launch<Character, DialogueEditor>(_character, _workingResponse);
		}

		private void cmdAccept_Click(object sender, EventArgs e)
		{
			ctlResponse.Save();
			_character.Behavior.AddWorkingCase(_workingResponse);
			BanterTargetedLines(_selectedCharacter);
			SelectLine(0);
		}

		private void cmdDiscard_Click(object sender, EventArgs e)
		{
			HideResponses();
		}

		private void gridLines_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				InboundLine line = gridLines.Rows[e.RowIndex]?.Cells["ColText"].Tag as InboundLine;
				if (line == null)
				{
					return;
				}
				if (e.ColumnIndex == ColJump.Index)
				{
					Image img = Properties.Resources.GoToLine;
					e.Paint(e.CellBounds, DataGridViewPaintParts.All);
					var w = img.Width;
					var h = img.Height;
					var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
					var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

					e.Graphics.DrawImage(img, new Rectangle(x, y, w, h));
					e.Handled = true;
				}
				else if (e.ColumnIndex == ColColor.Index)
				{
					if (line.ColorCode == null)
					{
						e.CellStyle.BackColor = SkinManager.Instance.CurrentSkin.Background.Normal;
					}
					else
					{
						ColorCode code = Definitions.Instance.Get<ColorCode>(line.ColorCode);
						if (code == null)
						{
							e.CellStyle.BackColor = SkinManager.Instance.CurrentSkin.Background.Normal;
						}
						else
						{
							e.CellStyle.BackColor = code.GetColor();
						}
					}
				}
			}
		}

		private void gridLines_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0 && e.ColumnIndex == ColJump.Index)
			{
				Character c = gridLines.Rows[e.RowIndex]?.Tag as Character;
				if (!c.IsFullyLoaded)
				{
					c = CharacterDatabase.Load(c.FolderName);
				}
				string text = gridLines.Rows[e.RowIndex]?.Cells["ColText"].Value as string;
				if (c == null || string.IsNullOrEmpty(text))
				{
					return;
				}

				foreach (Case workingCase in c.Behavior.GetWorkingCases())
				{
					foreach(DialogueLine dialogueLine in workingCase.Lines)
					{
						if (dialogueLine.Text == text)
						{
							Shell.Instance.Launch<Character, DialogueEditor>(c, new ValidationContext(new Stage(workingCase.Stages[0]), workingCase, dialogueLine));
							return;
						}
					}
				}
			}
		}

		private void cmdSaveBanter_Click(object sender, EventArgs e)
		{
			if (_character.BanterData.Timestamp == 0)
			{
				MessageBox.Show("No banter data to save.");
			}
			else
			{
				_character.BanterData.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				Serialization.BackupBanter(_character, Convert.ToString(_character.BanterData.Timestamp));
				cmdLoadBanter.Enabled = true;
			}
		}

		private void GenerateBanterXML()
		{
			_character.BanterData.LinkOwner(_character);
			_character.BanterData.WriteCEVersion();
			_character.BanterData.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			ListCharacters();
			cmdSaveBanter.Enabled = true;
		}

		private void BeginLoading()
		{
			_loading = true;
			Cursor.Current = Cursors.WaitCursor;
			splitContainer1.Panel1.Enabled = false;
			splitContainer2.Visible = false;
			panelLoad.Visible = true;
			progressBar.Visible = true;
			progressBar.Value = 0;
			lblProgress.Visible = true;
			lstCharacters.Items.Clear();
			gridLines.Rows.Clear();
		}

		private void EndLoading()
		{
			panelLoad.Visible = false;
			progressBar.Visible = false;
			lblProgress.Visible = false;
			splitContainer2.Visible = true;
			splitContainer1.Panel1.Enabled = true;
			Cursor.Current = Cursors.Default;
			_loading = false;
		}


		private void ListCharacters()
		{
			BeginLoading();

			List<string> folderNames = new List<string>();

			if (_filterToOne && _oneCharacter != null)
			{
				if (_oneCharacter.FolderName != "human" && CharacterFiltering(_oneCharacter.FolderName))
				{
					folderNames.Add(_oneCharacter.FolderName);
				}
			}
			else
			{
				foreach (Opponent opponent in Listing.Instance.Characters)
				{
					if (opponent.Name == "human" || !CharacterFiltering(opponent.Name))
					{
						continue;
					}
					folderNames.Add(opponent.Name);
				}
				folderNames = folderNames.Distinct().ToList();
			}

			progressBar.Maximum = Math.Max(1,folderNames.Count);
			int count = 0;
			foreach (string folderName in folderNames)
			{
				progressBar.Value = count++;
				lblProgress.Text = "Scanning " + folderName + "...";
				lblProgress.Refresh();
				if (!CharacterDatabase.Exists(folderName))
				{
					continue;
				}
				Character loaded = CharacterDatabase.Get(folderName);
				if (!loaded.IsFullyLoaded)
				{ 
					loaded = CharacterDatabase.Load(folderName);
				}
				TargetingCharacter ch = new TargetingCharacter(loaded, _character);
				_character.BanterData.TargetingCharacters.Add(ch);
				if (_character.BanterData.TargetingCharacters.Last().InboundCount > 0)
				{
					lstCharacters.Items.Add(loaded);
				}	
			}

			EndLoading();
		}

		private void ListCharactersFromFile()
		{
			BeginLoading();

			_character.BanterData = Serialization.ImportBanter(_character.FolderName);
			_character.BanterData.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

			progressBar.Maximum = _character.BanterData.TargetingCharacters.Count;
			int count = 0;
			foreach (TargetingCharacter ch in _character.BanterData.TargetingCharacters)
			{
				if (_filterToOne && _oneCharacter != null)
				{
					if (_oneCharacter.FolderName != ch.Id)
					{
						progressBar.Value = count++;
						continue;
					}
				}
				if (!CharacterFiltering(ch.Id))
				{
					progressBar.Value = count++;
					continue;
				}
				progressBar.Value = count++;
				lblProgress.Text = "Loading " + ch.Id + "...";
				lblProgress.Refresh();
				if (ch.InboundCount > 0)
				{
					Character loaded = CharacterDatabase.Get(ch.Id);
					if (!loaded.IsFullyLoaded)
					{
						loaded = CharacterDatabase.Load(ch.Id);
					}
					lstCharacters.Items.Add(loaded);
				}
			}

			EndLoading();
		}

		private void ReListCharacters()
		{
			BeginLoading();

			progressBar.Maximum = _character.BanterData.TargetingCharacters.Count;
			int count = 0;
			foreach (TargetingCharacter ch in _character.BanterData.TargetingCharacters)
			{
				if (_filterToOne && _oneCharacter != null)
				{
					if (_oneCharacter.FolderName != ch.Id)
					{
						progressBar.Value = count++;
						continue;
					}
				}
				if (!CharacterFiltering(ch.Id))
				{
					progressBar.Value = count++;
					continue;
				}
				progressBar.Value = count++;
				lblProgress.Text = "Loading " + ch.Id + "...";
				lblProgress.Refresh();
				int matchingLineCount = 0;

				foreach (InboundLine inbound in ch.Inbounds)
				{
					if (LineFiltering(inbound))
					{
						matchingLineCount++;
					}
				}

				if (matchingLineCount > 0)
				{
					Character loaded = CharacterDatabase.Get(ch.Id);
					if (!loaded.IsFullyLoaded)
					{
						loaded = CharacterDatabase.Load(ch.Id);
					}
					lstCharacters.Items.Add(loaded);
				}
			}

			EndLoading();

		}

		private void UpdateBanter()
		{
			BeginLoading();

			if (_character.BanterData.Timestamp == 0)
			{
				_character.BanterData = Serialization.ImportBanter(_character.FolderName);
			}

			int count = 0;

			List<string> folderNames = new List<string>();

			if (_filterToOne && _oneCharacter != null)
			{
				if (_oneCharacter.FolderName != "human" && CharacterFiltering(_oneCharacter.FolderName))
				{
					folderNames.Add(_oneCharacter.FolderName);
				}
			}
			else
			{
				foreach (Opponent opponent in Listing.Instance.Characters)
				{
					if (opponent.Name == "human" || !CharacterFiltering(opponent.Name))
					{
						continue;
					}
					folderNames.Add(opponent.Name);
				}
				folderNames = folderNames.Distinct().ToList();
			}
			progressBar.Maximum = Math.Max(1,folderNames.Count());

			foreach (string folderName in folderNames)
			{
				progressBar.Value = count++;
				lblProgress.Text = "Scanning " + folderName + "...";
				lblProgress.Refresh();

				if (!CharacterDatabase.Exists(folderName))
				{
					continue;
				}
				Character loaded = CharacterDatabase.Get(folderName);
				if (!loaded.IsFullyLoaded)
				{
					loaded = CharacterDatabase.Load(folderName);
				}
				int index = _character.BanterData.TargetingCharacters.FindIndex(x => x.Id == folderName);

				if (index == -1)
				{
					TargetingCharacter ch = new TargetingCharacter(loaded, _character);
					_character.BanterData.TargetingCharacters.Add(ch);
					if (ch.InboundCount > 0)
					{
						lstCharacters.Items.Add(loaded);
					}
					continue;
				}
				else if (loaded.Metadata.LastUpdate == _character.BanterData.TargetingCharacters[index].Timestamp)
				{
					foreach(InboundLine line in _character.BanterData.TargetingCharacters[index].Inbounds)
					{
						line.Newness = "";
					}
					if (_character.BanterData.TargetingCharacters[index].InboundCount > 0)
					{
						lstCharacters.Items.Add(loaded);
					}
					continue;
				}
				else
				{
					foreach (InboundLine line in _character.BanterData.TargetingCharacters[index].Inbounds)
					{
						line.Newness = "X";
					}

					Dictionary<DialogueLine, Case> behaviourLines = new Dictionary<DialogueLine, Case>();

					foreach (Case stageCase in loaded.GetWorkingCasesTargetedAtCharacter(_character, TargetType.DirectTarget))
					{
						foreach (DialogueLine dialogueLine in stageCase.Lines)
						{
							InboundLine inbound = _character.BanterData.TargetingCharacters[index].Inbounds.Find(x => x.Text == dialogueLine.Text);
							if (inbound == null)
							{
								behaviourLines.Add(dialogueLine, stageCase);
							}
							else
							{
								int stageCaseHash = 1;
								foreach (TargetCondition condition in stageCase.Conditions)
								{
									stageCaseHash *= condition.GetHashCode();
								}

								if (stageCaseHash != inbound.Condition)
								{
									inbound.Newness = "C";
									inbound.Condition = stageCaseHash;
								}
								else
								{
									inbound.Newness = "";
								}
							}
						}
					}

					List<InboundLine> banterLines = new List<InboundLine>();
					List<InboundLine> linesToRemove = new List<InboundLine>();

					if (behaviourLines.Count > 0) 
					{ 
						foreach (InboundLine line in _character.BanterData.TargetingCharacters[index].Inbounds)
						{
							if (line.Newness == "X")
							{
								banterLines.Add(line);
							}
						}

						if (banterLines.Count > 0)
						{ 
							foreach (KeyValuePair<DialogueLine, Case> kvp in behaviourLines)
							{
								float minRatio = 1.0F;
								int minLineIndex = -1;
								Levenshtein levenshtein = new Levenshtein(kvp.Key.Text);
								foreach (InboundLine item in banterLines)
								{
									float ratio = Convert.ToSingle(levenshtein.DistanceFrom(item.Text)) / Convert.ToSingle(Math.Max(item.Text.Length, kvp.Key.Text.Length));
									if (ratio < minRatio && ratio < 0.35F)
									{
										minRatio = ratio;
										minLineIndex = banterLines.IndexOf(item);
									}
								}
								if (minRatio < 0.35F)
								{
									InboundLine inbound = _character.BanterData.TargetingCharacters[index].Inbounds.Find(x => x.Text == banterLines[minLineIndex].Text);
									int stageCaseHash = 1;
									foreach (TargetCondition condition in kvp.Value.Conditions)
									{
										stageCaseHash *= condition.GetHashCode();
									}

									if (stageCaseHash != inbound.Condition)
									{
										inbound.Newness = "B";
										inbound.Condition = stageCaseHash;
										inbound.Text = kvp.Key.Text;
									}
									else
									{
										inbound.Newness = "T";
										inbound.Text = kvp.Key.Text;
									}
									banterLines.RemoveAt(minLineIndex);
								}
								else
								{
									InboundLine inboundLine = new InboundLine(kvp.Value, kvp.Key);
									_character.BanterData.TargetingCharacters[index].Inbounds.Add(inboundLine);
								}
							}
						}
						else
						{
							foreach (KeyValuePair< DialogueLine, Case> kvp in behaviourLines)
							{
								InboundLine inboundLine = new InboundLine(kvp.Value, kvp.Key);
								_character.BanterData.TargetingCharacters[index].Inbounds.Add(inboundLine);
							}
						}
					}

					foreach (InboundLine line in _character.BanterData.TargetingCharacters[index].Inbounds)
					{
						if (line.Newness == "X")
						{
								linesToRemove.Add(line);
						}
					}
					
					foreach (InboundLine line in linesToRemove)
					{
						_character.BanterData.TargetingCharacters[index].Inbounds.Remove(line);
					}

					_character.BanterData.TargetingCharacters[index].InboundCount = _character.BanterData.TargetingCharacters[index].Inbounds.Count;

					if (_character.BanterData.TargetingCharacters[index].InboundCount > 0)
					{
						lstCharacters.Items.Add(loaded);
					}
				}
			}

			_character.BanterData.TargetingCharacters = _character.BanterData.TargetingCharacters.OrderBy(x => x?.Id).ToList();

			_character.BanterData.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			_character.BanterData.WriteCEVersion();

			EndLoading();
		}

		private void cmdUpdateBanter_Click(object sender, EventArgs e)
		{
			if (!File.Exists(_path))
			{
				List<string> folderNames = new List<string>();
				if (_filterToOne && _oneCharacter != null)
				{
					if (_oneCharacter.FolderName != "human" && CharacterFiltering(_oneCharacter.FolderName))
					{
						folderNames.Add(_oneCharacter.FolderName);
					}
				}
				else
				{
					foreach (Opponent opponent in Listing.Instance.Characters)
					{
						if (opponent.Name == "human" || !CharacterFiltering(opponent.Name))
						{
							continue;
						}
						folderNames.Add(opponent.Name);
					}
				}
				if (folderNames.Count == 0)
				{
					MessageBox.Show("No characters matching the filters.");
					return;
				}
				GenerateBanterXML();
				cmdUpdateBanter.Text = "Generate/Update";
				_character.BanterData.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				Serialization.BackupBanter(_character, Convert.ToString(_character.BanterData.Timestamp));
				cmdLoadBanter.Enabled = true;
			}
			else
			{
				UpdateBanter();
			}
		}

		private void cmdLoadBanter_Click(object sender, EventArgs e)
		{
			ListCharactersFromFile();
		}


		private void cmdColorCode_Click(object sender, EventArgs e)
		{
			ColorCode color = RecordLookup.DoLookup(typeof(ColorCode), "", false, null) as ColorCode;
			if (color != null)
			{
				SetColorButton(color.Key);
			}
		}

		private void SetColorButton(string colorCode)
		{
			ColorCode code = Definitions.Instance.Get<ColorCode>(colorCode);
			if (code == null || colorCode == "0")
			{
				cmdColorCode.BackColor = SkinManager.Instance.CurrentSkin.Background.Normal;
				cmdColorCode.Tag = null;
				if (_currentInbound != null)
				{
					_currentInbound.ColorCode = null;
				}
			}
			else
			{
				cmdColorCode.BackColor = code.GetColor();
				cmdColorCode.Tag = colorCode;
				if (_currentInbound != null)
				{
					_currentInbound.ColorCode = cmdColorCode.Tag?.ToString();
				}
			}
		}

		private void cmdColorFilter_Click(object sender, EventArgs e)
		{
			ColorCode color = RecordLookup.DoLookup(typeof(ColorCode), "", false, null) as ColorCode;
			if (color != null)
			{
				SetColorFilterButton(color.Key);
			}
		}

		private void SetColorFilterButton(string colorCode)
		{
			ColorCode code = Definitions.Instance.Get<ColorCode>(colorCode);
			if (code == null || colorCode == "0")
			{
				cmdColorFilter.BackColor = SkinManager.Instance.CurrentSkin.Background.Normal;
				cmdColorFilter.Tag = null;
				_colorFilter = null;
			}
			else
			{
				cmdColorFilter.BackColor = code.GetColor();
				cmdColorFilter.Tag = colorCode;
				_colorFilter = colorCode;
			}
		}

		private void chkColorFilter_CheckedChanged(object sender, EventArgs e)
		{
			_filterToColor = chkColorFilter.Checked;
		}
		
		private void chkOneCharacter_CheckedChanged(object sender, EventArgs e)
		{
			_filterToOne = chkOneCharacter.Checked;
		}

		private void chkLineFiltering_SelectedIndexChanged(object sender, EventArgs e)
		{
			
			_filterNew = chkLineFiltering.GetItemChecked(0);
			_filterModText = chkLineFiltering.GetItemChecked(1);
			_filterModCond = chkLineFiltering.GetItemChecked(2);
			_filterOld = chkLineFiltering.GetItemChecked(3);		
		}

		private void cmdApplyFilters_Click(object sender, EventArgs e)
		{
			if (_character.BanterData.Timestamp != 0)
				ReListCharacters();
		}

		private void chkCharacterFiltering_SelectedIndexChanged(object sender, EventArgs e)
		{
			_filterMain = chkCharacterFiltering.GetItemChecked(0);
			_filterTesting = chkCharacterFiltering.GetItemChecked(1);
			_filterOffline = chkCharacterFiltering.GetItemChecked(2);
			_filterIncomplete = chkCharacterFiltering.GetItemChecked(3);
			_filterEvent = chkCharacterFiltering.GetItemChecked(4);
			_filterDuplicate = chkCharacterFiltering.GetItemChecked(5);
			_filterUnlisted = chkCharacterFiltering.GetItemChecked(6);
		}

		private void recOneCharacter_RecordChanged(object sender, RecordEventArgs e)
		{
			_oneCharacter = recOneCharacter.Record as Character;
		}
	}


	public partial class Levenshtein
	{
		private readonly string storedValue;
		private readonly int[] costs;

		/// <summary>
		/// Creates a new instance with a value to test other values against
		/// </summary>
		/// <param Name="value">Value to compare other values to.</param>
		public Levenshtein(string value)
		{
			this.storedValue = value;
			// Create matrix row
			this.costs = new int[this.storedValue.Length];
		}

		/// <summary>
		/// gets the length of the stored value that is tested against
		/// </summary>
		public int StoredLength => this.storedValue.Length;

		/// <summary>
		/// Compares a value to the stored value. 
		/// Not thread safe.
		/// </summary>
		/// <returns>Difference. 0 complete match.</returns>
		public int DistanceFrom(string value)
		{
			if (costs.Length == 0)
			{
				return value.Length;
			}

			// Add indexing for insertion to first row
			for (int i = 0; i < this.costs.Length;)
			{
				this.costs[i] = ++i;
			}

			for (int i = 0; i < value.Length; i++)
			{
				// cost of the first index
				int cost = i;
				int previousCost = i;

				// cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
				char value1Char = value[i];

				for (int j = 0; j < this.storedValue.Length; j++)
				{
					int currentCost = cost;

					// assigning this here reduces the array reads we do, improvement of the old version
					cost = costs[j];

					if (value1Char != this.storedValue[j])
					{
						if (previousCost < currentCost)
						{
							currentCost = previousCost;
						}

						if (cost < currentCost)
						{
							currentCost = cost;
						}

						++currentCost;
					}

					/* 
					 * Improvement on the older versions.
					 * Swapping the variables here results in a performance improvement for modern intel CPU’s, but I have no idea why?
					 */
					costs[j] = currentCost;
					previousCost = currentCost;
				}
			}

			return this.costs[this.costs.Length - 1];
		}
	}

}
