using Desktop.Skinning;
using SPNATI_Character_Editor.DataStructures;
using System;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Controls
{
	public partial class CharacterSettingControl : UserControl, ISkinnedPanel, ISkinControl
	{
		private Character _character;
		private CharacterSettingsGroup _selectedGroup;
		private CharacterSetting _selectedCharacterSetting;

		public CharacterSettingControl()
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

		public void SetCharacter(Character character)
		{
			_character = character;
		}
		public void Clear()
		{
			tableCharacterSetting.Data = null;
			txtDescription.Text = "";
			txtValue.Text = "";
			chkDefault.Checked = false;
		}

		public void SetGroup(CharacterSettingsGroup workingGroup)
		{
			if (_selectedGroup != null)
			{
				tabsCharacterSettings.SelectedIndexChanged -= tabsCharacterSettings_SelectedIndexChanged;
				tabsCharacterSettings.SelectedIndex = 0;
				for (int i = tabsCharacterSettings.TabPages.Count - 1; i > 0; i--)
				{
					tabsCharacterSettings.TabPages.RemoveAt(i);
				}
			}
			_selectedGroup = workingGroup;
			if (_selectedGroup != null)
			{
				for (int i = 0; i < _selectedGroup.CharacterSettings.Count - 1; i++)
				{
					AddCharacterSettingTab();
				}
				tabsCharacterSettings.SelectedIndexChanged += tabsCharacterSettings_SelectedIndexChanged;
				_selectedCharacterSetting = _selectedGroup.CharacterSettings[0];
				SetCharacterSetting();
			}
		}

		private void SetCharacterSetting()
		{
			chkDefault.Checked = _selectedCharacterSetting.Default;
			txtDescription.Text = _selectedCharacterSetting.Name;
			txtValue.Text = _selectedCharacterSetting.Value;
			PopulateCSTable();
		}

		private void tabsCharacterSettings_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_selectedGroup == null) { return; }
			SaveCharacterSetting();
			int index = tabsCharacterSettings.SelectedIndex;
			_selectedCharacterSetting = _selectedGroup.CharacterSettings[index];
			SetCharacterSetting();
		}

		private void AddCharacterSettingTab()
		{
			tabsCharacterSettings.TabPages.Add($"Setting {tabsCharacterSettings.TabPages.Count + 1}");
		}

		private void PopulateCSTable()
		{
			if (tableCharacterSetting.Data != _selectedCharacterSetting)
			{
				tableCharacterSetting.Data = _selectedCharacterSetting;
			}
		}

		public void SaveCharacterSetting()
		{
			if (_selectedCharacterSetting == null) { return; }
			_character.IsDirty = true;
			_selectedCharacterSetting.Value = txtValue.Text;
			_selectedCharacterSetting.Name = txtDescription.Text;
			_selectedCharacterSetting.Default = chkDefault.Checked;
			tableCharacterSetting.Save();
		}


		public SkinnedBackgroundType PanelType
		{
			get { return SkinnedBackgroundType.Background; }
		}

		private void chkDefault_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void stripCharacterSettings_AddButtonClicked(object sender, EventArgs e)
		{
			if (_selectedGroup == null) { return; }
			CharacterSetting cs = new CharacterSetting();
			cs.Default = false;
			_selectedGroup.CharacterSettings.Add(cs);
			AddCharacterSettingTab();
			tabsCharacterSettings.SelectedIndex = tabsCharacterSettings.TabPages.Count;
		}

		private void stripCharacterSettings_CloseButtonClicked(object sender, EventArgs e)
		{
			if (_selectedGroup == null) { return; }
			int index = tabsCharacterSettings.SelectedIndex - 1;
			if (index >= 0)
			{
				_selectedGroup.CharacterSettings.RemoveAt(index + 1);
				tabsCharacterSettings.TabPages.RemoveAt(index + 1);
				for (int i = index; i < tabsCharacterSettings.TabPages.Count; i++)
				{
					tabsCharacterSettings.TabPages[i].Text = "Setting " + (i + 1);
				}
				tabsCharacterSettings.SelectedIndex = index < tabsCharacterSettings.TabPages.Count - 1 ? index + 1 : index;
			}
		}

		private void stripCharacterSettings_TabIndexChanged(object sender, EventArgs e)
		{
			SaveCharacterSetting();
			_selectedCharacterSetting = _selectedGroup.CharacterSettings[tabsCharacterSettings.SelectedIndex];
			SetCharacterSetting();
		}
	}
}