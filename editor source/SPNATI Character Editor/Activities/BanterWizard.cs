using Desktop;
using Desktop.Skinning;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Activities
{
	[Spacer]
	[Activity(typeof(Character), 300, DelayRun = true, Caption = "Banter Wizard")]
	[Tutorial("https://youtu.be/wyKiC3bMbeY?t=19")]
	public partial class BanterWizard : Activity
	{
		private Character _character;
		private Case _workingResponse;
		private Case _selectedCase;
		private Character _selectedCharacter;
		public bool Modified { get; private set; }
		private int _oldSplitter;
		private bool _editing;
		private bool _loading;
		private string _path;
		private InboundLine _currentInbound;

		public BanterWizard()
		{
			InitializeComponent();
		}

		public override string Caption
		{
			get
			{
				return "Banter Wizard";
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
            ColJump.Flat = true;
            _path = Path.Combine(Config.GetString(Settings.GameDirectory), "opponents/"+_character.FolderName+"/banter.xml");
            if (!File.Exists(_path))
            {
                cmdUpdateBanter.Text = "Generate";
				cmdSaveBanter.Enabled = false;
				cmdLoadBanter.Enabled = false;
            }
			else
			{
				cmdUpdateBanter.Text = "Update";
                cmdSaveBanter.Enabled = true;
				cmdLoadBanter.Enabled = true;
			}
			cmdCreateResponse.Visible = false;
        }

		protected override void OnFirstActivate()
		{
			ctlResponse.SetCharacter(_character);
			HideResponses();
			lblCharacters.Text = string.Format(lblCharacters.Text, _character);
			lstCharacters.Sorted = true;
			if (File.Exists(_path))
			{ 
                toolTip1.SetToolTip(this.cmdUpdateBanter, "Update banter data. Use this after pulling other characters' updates from Git.");
            }
        }

		protected override void OnActivate()
		{
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

		private void lstCharacters_SelectedIndexChanged(object sender, EventArgs e)
		{
			Character character = lstCharacters.SelectedItem as Character;
			if (character == null)
			{
				cmdCreateResponse.Visible = false;
			}
			else 
			{
				BanterTargetedLines(lstCharacters.SelectedItem as Character);
				Workspace.SendMessage(WorkspaceMessages.PreviewCharacterChanged, lstCharacters.SelectedItem as Character);
				SelectLine(0);
				cmdCreateResponse.Visible = true;
			}
        }

		private void BanterTargetedLines(Character loaded)
		{
			if (loaded == null)
				return;
			grpLines.Text = "Lines spoken by " + loaded;
			HideResponses();
			gridLines.Rows.Clear();
			TargetingCharacter targeter = _character.BanterData.TargetingCharacters.Find(x => x.Id == loaded.FolderName);
			foreach (InboundLine line in targeter.Inbounds)
			{
				DataGridViewRow row = gridLines.Rows[gridLines.Rows.Add()];
				row.Tag = loaded;
                row.Cells["ColNewness"].Value = line.Newness;
                row.Cells["ColText"].Value = line.Text;
				row.Cells["ColText"].Tag = line;
				row.Cells["ColStage"].Value = line.StageRange;
				row.Cells["ColCase"].Value = line.CaseTag;
			}
            gridLines.Visible = (targeter.InboundCount > 0);
            lblNoMatches.Visible = (targeter.InboundCount == 0);
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

        private void CheckForResponses(Character character, string text)
		{
			foreach (Case workingCase in character.Behavior.GetWorkingCases())
			{
				foreach (DialogueLine dialogueLine in workingCase.Lines)
				{
					if (dialogueLine.Text == text)
					{
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
							_selectedCharacter = character;
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

				if (inbound != null)
				{
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

				CheckForResponses(c, inbound.Text);
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
                _selectedCharacter = null;
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
			HideResponses();
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
				string text = gridLines.Rows[e.RowIndex]?.Cells["ColText"].Value as string;
				if (c == null || string.IsNullOrEmpty(text))
                {
                    return;
                }

				c.PrepareForEdit();

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
            if (code == null)
            {
                cmdColorCode.BackColor = SkinManager.Instance.CurrentSkin.Background.Normal;
                cmdColorCode.Tag = null;
            }
            else
            {
                cmdColorCode.BackColor = code.GetColor();
                cmdColorCode.Tag = colorCode;
				if (_currentInbound != null)
				{
					_currentInbound.ColorCode = cmdColorCode.Tag?.ToString();
				//	_currentInbound.NotifyPropertyChanged;
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

        private void ListCharacters()
        {
            List<string> folderNames = new List<string>();
            foreach (Character character in CharacterDatabase.Characters)
            {
                if (character.FolderName == "human")
                {
                    continue;
                }
                folderNames.Add(character.FolderName);
            }

            _loading = true;
            splitContainer1.Panel1.Enabled = false;
            panelLoad.Visible = true;
            panelLoad.BringToFront();
            Cursor.Current = Cursors.WaitCursor;
            lstCharacters.Items.Clear();
            progress.Maximum = folderNames.Count;
			int count = 0;
            foreach (string folderName in folderNames)
            {
                progress.Value = count++;
                lblProgress.Text = "Scanning "+folderName+"...";
				Character loaded = CharacterDatabase.Load(folderName);
                TargetingCharacter ch = new TargetingCharacter(loaded, _character);
                _character.BanterData.TargetingCharacters.Add(ch);
				if (_character.BanterData.TargetingCharacters.Last().InboundCount > 0)
				{
					lstCharacters.Items.Add(loaded);
				}
            }
            panelLoad.Visible = false;
            splitContainer1.Panel1.Enabled = true;
            Cursor.Current = Cursors.Default;
            _loading = false;
        }

        private void ListCharactersFromFile()
        {
			_character.BanterData = Serialization.ImportBanter(_character.FolderName);
            _character.BanterData.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            _loading = true;
            splitContainer1.Panel1.Enabled = false;
            panelLoad.Visible = true;
            panelLoad.BringToFront();
            Cursor.Current = Cursors.WaitCursor;
            lstCharacters.Items.Clear();
			progress.Maximum = _character.BanterData.TargetingCharacters.Count;
            int count = 0;
            foreach (TargetingCharacter ch in _character.BanterData.TargetingCharacters)
            {
                progress.Value = count++;
                Character loaded = CharacterDatabase.Load(ch.Id);
                if (ch.InboundCount > 0)
                {
                    lstCharacters.Items.Add(loaded);
                }
            }
            panelLoad.Visible = false;
            splitContainer1.Panel1.Enabled = true;
            Cursor.Current = Cursors.Default;
            _loading = false;
        }

		private void UpdateBanter()
		{
			if (_character.BanterData.Timestamp == 0)
			{
                _character.BanterData = Serialization.ImportBanter(_character.FolderName);
            }

            _loading = true;
			splitContainer1.Panel1.Enabled = false;
			panelLoad.Visible = true;
			panelLoad.BringToFront();
			Cursor.Current = Cursors.WaitCursor;
			lstCharacters.Items.Clear();
			progress.Maximum = CharacterDatabase.Characters.Count() - 1;
			int count = 0;

			List<string> folderNames = new List<string>();
			foreach (Character character in CharacterDatabase.Characters)
			{
				if (character.FolderName == "human")
				{
					continue;
				}
				folderNames.Add(character.FolderName);
			}
			foreach (string folderName in folderNames)
			{ 
				progress.Value = count++;
				
				int index = _character.BanterData.TargetingCharacters.FindIndex(x => x.Id == folderName);
                Character loaded = CharacterDatabase.Load(folderName);
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
                    foreach (Case stageCase in loaded.GetWorkingCasesTargetedAtCharacter(_character, TargetType.DirectTarget))
                    {
						foreach (DialogueLine dialogueLine in stageCase.Lines)
						{
							InboundLine inbound = _character.BanterData.TargetingCharacters[index].Inbounds.Find(x => x.Text == dialogueLine.Text);
							if (inbound == null)
							{
								InboundLine inboundLine = new InboundLine(stageCase, dialogueLine);
                                _character.BanterData.TargetingCharacters[index].Inbounds.Add(inboundLine);
							}
							else
							{
								inbound.Newness = "";
							}
                        }
                    }

					List<InboundLine> linesToRemove = new List<InboundLine>();
                    foreach (InboundLine line in _character.BanterData.TargetingCharacters[index].Inbounds)
                    {
                        if (line.Newness == "X")
						{
							linesToRemove.Add(line);					}
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

            _character.BanterData.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _character.BanterData.WriteCEVersion();
            panelLoad.Visible = false;
			splitContainer1.Panel1.Enabled = true;
			Cursor.Current = Cursors.Default;
			_loading = false;
		}

		private void cmdUpdateBanter_Click(object sender, EventArgs e)
		{
            string path = Path.Combine(Config.GetString(Settings.GameDirectory), "opponents/" + _character.FolderName + "/banter.xml");
            if (!File.Exists(path))
            {
				GenerateBanterXML();
                cmdUpdateBanter.Text = "Update";
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
    }
}
