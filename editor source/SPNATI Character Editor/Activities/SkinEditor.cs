using Desktop;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Activities
{
	[Activity(typeof(Costume), 0)]
	[Tutorial("https://www.youtube.com/watch?v=35jIow_jMHE")]
	public partial class SkinEditor : Activity
	{
		private bool _linkDataChanged = false;
		private Costume _costume;
		private bool _populatingImages;
		private bool _exportOnQuit;

		public SkinEditor()
		{
			InitializeComponent();

			cboStatus.Items.Add("");
			cboStatus.Items.Add("online");
			cboStatus.Items.Add("offline");
			cboStatus.Items.Add("unlisted");
			cboGender.Items.AddRange(new string[] { "female", "male" });
			cboEvent.Items.AddRange(new string[] { "", "none", "valentines", "april_fools", "easter", "summer", "halloween", "xmas", "sleepover" });
		}

		public override string Caption
		{
			get { return "General"; }
		}

		protected override void OnInitialize()
		{
			_costume = Record as Costume;
			SubscribeWorkspace<bool>(WorkspaceMessages.Save, OnSaveWorkspace);
		}

		private void OnSaveWorkspace(bool auto)
		{
			if (!auto)
			{
				Save();
				if (Serialization.ExportSkin(_costume))
				{
					Shell.Instance.SetStatus(string.Format("{0} exported successfully at {1}.", _costume, DateTime.Now.ToShortTimeString()));
				}
				else
				{
					Shell.Instance.SetStatus(string.Format("{0} failed to export.", _costume));
				}
			}
		}

		private void LinkCharacter()
		{
			Character character = RecordLookup.DoLookup(typeof(Character), "", false, _costume) as Character;
			if (character != null)
			{
				_costume.LinkCharacter(character);
			}
		}

		protected override void OnFirstActivate()
		{
			if (_costume.Character == null)
			{
				LinkCharacter();
			}

			SkinLink link = _costume.Link;
			if (link != null)
			{
				txtName.Text = link.Name;
				cboStatus.Text = link.Status;
				cboEvent.Text = link.Set;
				string gender = link.Gender ?? _costume.Character.Gender;
				cboGender.SelectedItem = gender;
			}

			cboBaseStage.Items.Add("- None -");
			for (int i = 0; i < _costume.Layers + Clothing.ExtraStages; i++)
			{
				cboBaseStage.Items.Add(_costume.Character.LayerToStageName(i, _costume));
			}

			//if anyone tries to get fancy by linking to multiple folders instead of just the reskin and the base, sorry, but we're not handling it for now
			string baseFolder = $"opponents/{_costume.Character.FolderName}/";
			StageSpecificValue baseStage = _costume.Folders.Find(f => f.Value == baseFolder);
			if (baseStage != null)
			{
				cboBaseStage.SelectedIndex = baseStage.Stage + 1;
			}
			else
			{
				cboBaseStage.SelectedIndex = -1;
			}

			gridLabels.Set(_costume.Labels);

			PopulatePortraitDropdown();
			if (_costume.Link?.PreviewImage != null)
			{
				string portrait = _costume.Link.PreviewImage;
				PoseMapping pose = _costume.Character.PoseLibrary.GetPose(portrait);
				cboDefaultPic.SelectedItem = pose;
			}
		}

		/// <summary>
		/// Populates the default portrait dropdown menu
		/// </summary>
		private void PopulatePortraitDropdown()
		{
			_populatingImages = true;
			List<PoseMapping> poses = _costume.Character.PoseLibrary.GetPortraitPoses();
			cboDefaultPic.DisplayMember = "DisplayName";
			cboDefaultPic.DataSource = poses;
			_populatingImages = false;
		}

		private bool PromptToSave()
		{
			if (_costume == null || !_costume.IsDirty)
				return true;
			DialogResult result = MessageBox.Show(string.Format("Do you wish to save {0} first?", _costume.Link.Name), "Save changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
			if (result == DialogResult.Yes)
			{
				_exportOnQuit = true;
				return true;
			}
			else if (result == DialogResult.No)
			{
				return true;
			}
			return false;
		}

		public override bool CanQuit(CloseArgs args)
		{
			return PromptToSave();
		}

		public override void Quit()
		{
			if (_exportOnQuit)
			{
				OnSaveWorkspace(false);
			}
		}

		public override void Save()
		{
			_costume.Labels = gridLabels.Values;

			if (_costume.Link != null)
			{
				string status = cboStatus.Text;
				if (string.IsNullOrEmpty(status))
				{
					status = null;
				}

				string set = cboEvent.Text;
				if (string.IsNullOrEmpty(set) || set == "none")
				{
					set = null;
				}

				string gender = cboGender.SelectedItem?.ToString();

				string label = _costume.Labels.Count > 0 ? _costume.Labels[0].Value : null;

				int countUnskipped = 0;
				foreach (Clothing c in _costume.Wardrobe)
				{
					if (c != null && c.Type != "skip")
					{
						countUnskipped++;
					}
				}

				if (txtName.Text != _costume.Link.Name || status != _costume.Link.Status || set != _costume.Link.Set || _costume.Link.IsDirty
					|| gender != _costume.Link.Gender || label != _costume.Link.Label || countUnskipped != _costume.Link.LayersNonSkip)
				{
					_linkDataChanged = true;
				}
				if (_linkDataChanged)
				{
					_linkDataChanged = false;
					_costume.Link.IsDirty = false;
					_costume.Link.Name = txtName.Text;
					_costume.Link.Status = status;
					_costume.Link.Set = set;
					_costume.Link.Label = label;
					_costume.Link.LayersNonSkip = countUnskipped;

					if (gender != _costume.Character.Gender)
					{
						_costume.Link.Gender = gender;
					}
					else
					{
						_costume.Link.Gender = null;
					}

					Serialization.ExportCharacter(_costume.Character);
				}
			}

			//Here's where any unexpected folders are thrown out
			string folder = _costume.Folder;
			_costume.Folders.Clear();
			_costume.Folders.Add(new StageSpecificValue(0, folder));
			int baseIndex = cboBaseStage.SelectedIndex - 1;
			if (baseIndex >= 0)
			{
				_costume.Folders.Add(new StageSpecificValue(baseIndex, $"opponents/{_costume.Character.FolderName}/"));
			}
		}

		private void cboDefaultPic_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (_populatingImages)
				return;

			PoseMapping image = cboDefaultPic.SelectedItem as PoseMapping;
			if (image == null)
				return;
			string newKey = image.Key.Replace("#-", "0-");
			if (_costume.Link.PreviewImage != newKey)
			{
				_costume.Link.PreviewImage = newKey;
				_costume.Link.IsDirty = true;
			}
			Workspace.SendMessage(WorkspaceMessages.UpdatePreviewImage, new UpdateImageArgs(_costume, image, 0));
		}
	}
}
