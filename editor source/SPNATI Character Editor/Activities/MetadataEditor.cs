using Desktop;
using System;
using System.Collections.Generic;

namespace SPNATI_Character_Editor.Activities
{
	[Activity(typeof(Character), 0)]
	[Tutorial("https://www.youtube.com/watch?v=48ak4Ao6p5I")]
	public partial class MetadataEditor : Activity
	{
		private bool _populatingImages;
		private Character _character;

		public MetadataEditor()
		{
			InitializeComponent();

			cboGender.Items.AddRange(new string[] { "female", "male" });
			cboTitleGender.Items.AddRange(new string[] { "", "female", "male" });
			cboSize.Items.AddRange(new string[] { "small", "medium", "large" });
			ColDifficulty.Items.AddRange(DialogueLine.AILevels);
		}

		public override string Caption
		{
			get { return "Metadata"; }
		}

		protected override void OnInitialize()
		{
			_character = Record as Character;
		}

		protected override void OnFirstActivate()
		{
			txtFirstName.Text = _character.FirstName;
			txtLastName.Text = _character.LastName;
			txtDefaultCostumeName.Text = _character.Metadata.DefaultCostumeName;
			cboSize.SelectedItem = _character.Size;
			cboGender.SelectedItem = _character.Gender;
			if (_character.Metadata.Gender != _character.Gender && !string.IsNullOrEmpty(_character.Metadata.Gender))
			{
				cboTitleGender.SelectedItem = _character.Metadata.Gender;
				ExpandGender();
			}
			valRounds.Value = _character.Stamina;

			if (_character.Metadata.Description != null)
			{
				txtDescription.Text = _character.Metadata.Description.Replace("<br>", Environment.NewLine);
			}

			if(_character.Metadata.Height != null)
			{
				txtHeight.Text = _character.Metadata.Height;
			}
			else
			{
				txtHeight.Text = CharacterDatabase.GetEditorData(_character).Height;
			}

			txtAge.Text = CharacterDatabase.GetEditorData(_character).Age;
			txtpronunciationGuide.Text = CharacterDatabase.GetEditorData(_character).pronunciationGuide;
			txtSource.Text = _character.Metadata.Source;
			txtWriter.Text = _character.Metadata.Writer;
			txtArtist.Text = _character.Metadata.Artist;
			PopulatePortraitDropdown();
			if (_character.Metadata.Portrait != null)
			{
				if (_character.Metadata.Portrait.Image != null)
				{
					string portrait = _character.Metadata.Portrait.Image;
					PoseMapping pose = _character.PoseLibrary.GetPose(portrait);
					cboDefaultPic.SelectedItem = pose;
				}
				valPicX.Value = Math.Max(valPicX.Minimum, Math.Min((decimal)_character.Metadata.Portrait.X, valPicX.Maximum));
				valPicY.Value = Math.Max(valPicY.Minimum, Math.Min((decimal)_character.Metadata.Portrait.Y, valPicY.Maximum));
				valPicScale.Value = Math.Max(valPicScale.Minimum, Math.Min((decimal)_character.Metadata.Portrait.Scale, valPicScale.Maximum));
			}
			gridAI.Data = _character.Intelligence;

			string othernotes = CharacterDatabase.GetEditorData(_character).OtherNotes;
			if (othernotes != null)
			{
				txtOtherNotes.Text = othernotes.Replace("<br>", Environment.NewLine);
			}
		}

		/// <summary>
		/// Populates the default portrait dropdown menu
		/// </summary>
		private void PopulatePortraitDropdown()
		{
			_populatingImages = true;
			List<PoseMapping> poses = _character.PoseLibrary.GetPortraitPoses();
			List<PoseMapping> normalPoses = new List<PoseMapping>();

			foreach (PoseMapping pose in poses)
			{
				if (!pose.DisplayName.Contains("custom:"))
				{
					normalPoses.Add(pose);
				}
			}

			cboDefaultPic.DisplayMember = "DisplayName";
			cboDefaultPic.DataSource = normalPoses;
			_populatingImages = false;
		}

		protected override void OnActivate()
		{
			txtLabel.Text = _character.Label;

			if (_character.Metadata.Label != _character.Label && !string.IsNullOrEmpty(_character.Metadata.Label))
			{
				txtTitleLabel.Text = _character.Metadata.Label;
				ExpandLabel();
			}

			if (_character.Metadata.Portrait == null)
				return;

			PoseMapping image = _character.PoseLibrary.GetPose(_character.Metadata.Portrait.Image);
			if (image == null)
				return;
			_character.Metadata.Portrait.Image = image.Key.Replace("#-", "0-");
			Workspace.SendMessage(WorkspaceMessages.UpdatePreviewImage, new UpdateImageArgs(_character, image, 0));
			Workspace.SendMessage(WorkspaceMessages.PreviewLine, "");
		}

		public override void Save()
		{
			_character.Label = txtLabel.Text;
			_character.Metadata.Label = txtTitleLabel.Text;
			_character.FirstName = txtFirstName.Text;
			_character.LastName = txtLastName.Text;
			_character.Metadata.DefaultCostumeName = txtDefaultCostumeName.Text;
			_character.Stamina = (int)valRounds.Value;
			_character.Gender = cboGender.SelectedItem.ToString();
			string titleGender = cboTitleGender.SelectedItem?.ToString();
			if (!string.IsNullOrEmpty(titleGender))
			{
				_character.Metadata.Gender = titleGender;
			}
			_character.Size = cboSize.SelectedItem.ToString();
			_character.Metadata.Description = txtDescription.Text.Replace(Environment.NewLine, "<br>");
			CharacterDatabase.GetEditorData(_character).Height = txtHeight.Text;
			CharacterDatabase.GetEditorData(_character).Age = txtAge.Text;
			CharacterDatabase.GetEditorData(_character).pronunciationGuide = txtpronunciationGuide.Text;
			_character.Metadata.Source = txtSource.Text;
			_character.Metadata.DefaultCostumeName = txtDefaultCostumeName.Text;
			_character.Metadata.Writer = txtWriter.Text;
			_character.Metadata.Artist = txtArtist.Text;
			gridAI.Save(ColAIStage);
			CharacterDatabase.GetEditorData(_character).OtherNotes = txtOtherNotes.Text.Replace(Environment.NewLine,"<br>");
			if (_character.Metadata.Portrait != null)
			{
				_character.Metadata.Portrait.X = (int)valPicX.Value;
				_character.Metadata.Portrait.Y = (int)valPicY.Value;
				_character.Metadata.Portrait.Scale = (float)valPicScale.Value;
			}

		}

		private void cboDefaultPic_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_populatingImages)
				return;
			PoseMapping image = cboDefaultPic.SelectedItem as PoseMapping;
			if (image == null)
				return;
			_character.Metadata.Portrait.Image = image.Key.Replace("#-", "0-");
			Workspace.SendMessage(WorkspaceMessages.UpdatePreviewImage, new UpdateImageArgs(_character, image, 0));
		}

		private void cboGender_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			string gender = cboGender.SelectedItem?.ToString();
			if (gender == "male")
			{
				lblSize.Text = "Crotch:";
			}
			else
			{
				lblSize.Text = "Chest:";
			}
		}

		private void cmdExpandGender_Click(object sender, EventArgs e)
		{
			ExpandGender();
		}

		private void ExpandGender()
		{
			cmdExpandGender.Visible = false;
			lblTitleGender.Visible = true;
			cboTitleGender.Visible = true;
		}

		private void cmdExpandLabel_Click(object sender, EventArgs e)
		{
			ExpandLabel();
		}

		private void ExpandLabel()
		{
			cmdExpandLabel.Visible = false;
			lblTitleLabel.Visible = true;
			txtTitleLabel.Visible = true;
		}

		private void cmdExpandPicOptions_Click(object sender, EventArgs e)
		{
			ExpandPic();
		}

		private void ExpandPic()
		{
			cmdExpandPicOptions.Visible = false;
			lblPicX.Visible = true;
			valPicX.Visible = true;
			lblPicY.Visible = true;
			valPicY.Visible = true;
			lblPicScale.Visible = true;
			valPicScale.Visible = true;
		}

		private void gridAISimplify()
		{
			int previousStage = 0;
			string previousLevel = DialogueLine.AILevels[4];
			for (int i = 0; i < gridAI.Rows.Count; i++)
			{
				int stage;
				bool success = gridAI.Rows[i].Cells[0].Value != null;
				if (!success)
				{
					gridAI.Rows[i].Cells[0].Value = previousStage;
				}
				success = int.TryParse(gridAI.Rows[i].Cells[0].Value.ToString(), out stage);
				if (!success)
				{
					gridAI.Rows[i].Cells[0].Value = previousStage;
				}
				else if (stage < previousStage)
				{
					gridAI.Rows[i].Cells[0].Value = previousStage;
				}
				else if (stage > _character.Layers)
				{
					gridAI.Rows[i].Cells[0].Value = _character.Layers;
				}
				previousStage = int.Parse(gridAI.Rows[i].Cells[0].Value.ToString());

				if (gridAI.Rows[i].Cells[1].Value == null || gridAI.Rows[i].Cells[1].Value.ToString() == "")
				{
					gridAI.Rows[i].Cells[1].Value = previousLevel;
				}
				else
				{
					previousLevel = gridAI.Rows[i].Cells[1].Value.ToString();
				}
			}
			for (int i = gridAI.Rows.Count - 1; i > 0; i--)
			{
				if (gridAI.Rows[i].Cells[0].Value.ToString() == gridAI.Rows[i - 1].Cells[0].Value.ToString() || gridAI.Rows[i].Cells[1].Value.ToString() == gridAI.Rows[i - 1].Cells[1].Value.ToString())
				{
					if (!gridAI.Rows[i].IsNewRow)
					{
						gridAI.Rows.RemoveAt(i);
					}
					else
					{
						gridAI.Rows[i].Cells[0].Value = null;
						gridAI.Rows[i].Cells[1].Value = null;
					}
				}
			}
			if (gridAI.Rows[0].Cells[0].Value != null || gridAI.Rows[0].Cells[0].Value.ToString() != "0")
			{
				gridAI.Rows[0].Cells[0].Value = 0;
			}
		}

		private void gridAI_Validated(object sender, EventArgs e)
		{
			gridAISimplify();
		}
	}
}
