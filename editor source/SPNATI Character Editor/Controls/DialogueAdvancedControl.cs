using Desktop.Skinning;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Controls
{
	public partial class DialogueAdvancedControl : UserControl, IDialogueDropDownControl
	{
		public int RowIndex { get; private set; }

		public SkinnedBackgroundType PanelType
		{
			get { return SkinnedBackgroundType.Background; }
		}

		private Character _character;
		private DialogueLine _line;
		private bool _settingData;
		private NicknameOperation _selectedNickOp;
		private Dictionary<string, string> _nickOps = new Dictionary<string, string>();

		public event EventHandler DataUpdated;

		public DialogueAdvancedControl()
		{
			InitializeComponent();
			cboGender.Items.AddRange(new string[] { "", "female", "male" });
			cboSize.Items.AddRange(new string[] { "", "small", "medium", "large" });
			cboAttr.Items.AddRange(new string[] { "", "timer", "stamina", "redirect-finish" });
			cboHeavy.Items.AddRange(new string[] { "", "true", "false" });
			cboOp.Items.AddRange(new string[] { "=", "+", "-", "*", "/", "%" });
			_nickOps.Add("=", "Reset To");
			_nickOps.Add("+", "Add/Increase Weight");
			_nickOps.Add("-", "Decrease Weight");
			_nickOps.Add(":", "Set Weight");
			cboNickOp.Items.AddRange(_nickOps.Values);
			recNickChar.RecordType = typeof(Character);
			cboDirection.DataSource = DialogueLine.ArrowDirections;
			cboFontSize.DataSource = DialogueLine.FontSizes;
			cboAI.DataSource = DialogueLine.AILevels;
			_settingData = false;
			OnUpdateSkin(SkinManager.Instance.CurrentSkin);
		}

		public void OnUpdateSkin(Skin skin)
		{
			BackColor = skin.Background.Normal;
			foreach (Control child in Controls)
			{
				SkinManager.Instance.ReskinControl(child, skin);
			}
			Invalidate(true);
		}

		public void SetData(int row, DialogueLine line, Character character)
		{
			_settingData = true;

			OnUpdateSkin(SkinManager.Instance.CurrentSkin);
			RowIndex = row;
			_line = line;
			_character = character;
			string fontSize = line.FontSize == "" ? character.Metadata.TextSize.ToString() : line.FontSize;
			cboDirection.Text = line.Direction ?? "";

			cboSize.Text = line.Size ?? "";
			cboAI.Text = line.Intelligence ?? "";
			cboGender.Text = line.Gender ?? "";
			txtLabel.Text = line.Label;
			chkResetAI.Checked = line.Intelligence == "";
			chkResetLabel.Checked = line.Label == "";
			chkLayer.Checked = line.Layer == "over" || (character.Metadata.BubblePosition.ToString() == "over" && line.Layer != "under");
			valWeight.Value = Math.Max(valWeight.Minimum, Math.Min(valWeight.Maximum, (decimal)line.Weight));

			float location = 0;
			string loc = line.Location;
			if (loc != null && loc.EndsWith("%"))
			{
				loc = loc.Substring(0, loc.Length - 1);
			}
			if (!float.TryParse(loc, NumberStyles.Number, CultureInfo.InvariantCulture, out location))
			{
				location = 50;
			}
			valLocation.Value = Math.Max(valLocation.Minimum, Math.Min(valLocation.Maximum, (decimal)location));

			cboHeavy.Text = "";
			cboAttr.Text = "";
			txtValue.Text = "";
			cboOp.Text = "";
			chkResetHeavy.Checked = false;
			if (line.DialogueOperations != null && line.DialogueOperations.ForfeitOps != null)
			{
				foreach (ForfeitOperation op in line.DialogueOperations.ForfeitOps.ToArray())
				{
					if (op.Attribute == "heavy")
					{
						if (op.Value == "reset")
						{
							cboHeavy.Text = "";
							chkResetHeavy.Checked = true;
						}
						else
						{
							cboHeavy.Text = op.Value ?? "";
							chkResetHeavy.Checked = false;
						}
					}
					else
					{
						cboAttr.Text = op.Attribute;
						txtValue.Text = op.Value;
						cboOp.Text = op.Operator;
					}
				}
			}

			recNickChar.RecordKey = "";
			cboNickOp.Text = "";
			txtNickname.Text = "";
			valNickWeight.Value = 1;
			lstNick.Items.Clear();
			if (line.DialogueOperations != null && line.DialogueOperations.NicknameOps != null)
			{
				foreach (NicknameOperation op in line.DialogueOperations.NicknameOps.ToArray())
				{
					lstNick.Items.Add(op);
					if (lstNick.Items.Count > 0) { lstNick.SelectedIndex = 0; }
				}
			}

			// has to be at end or else weight and location are set incorrectly
			cboFontSize.Text = fontSize ?? "";

			_settingData = false;
		}

		public DialogueLine GetLine()
		{
			float location = (float)valLocation.Value;
			if (location == 50)
			{
				_line.Location = null;
			}
			else
			{
				_line.Location = location.ToString(CultureInfo.InvariantCulture) + "%";
			}
			string direction = cboDirection.Text;
			if (string.IsNullOrEmpty(direction))
			{
				direction = null;
			}
			_line.Direction = direction;
			string fontSize = cboFontSize.Text;
			if (string.IsNullOrEmpty(fontSize) || fontSize == _character.Metadata.TextSize.ToString())
			{
				fontSize = null;
			}
			_line.FontSize = fontSize;
			_line.Weight = (float)valWeight.Value;

			string size = cboSize.Text;
			if (string.IsNullOrEmpty(size))
			{
				size = null;
			}
			_line.Size = size;

			if (chkResetAI.Checked)
			{
				_line.Intelligence = "";
			}
			else
			{
				string ai = cboAI.Text;
				if (string.IsNullOrEmpty(ai))
				{
					ai = null;
				}
				_line.Intelligence = ai;
			}

			string gender = cboGender.Text;
			if (string.IsNullOrEmpty(gender))
			{
				gender = null;
			}
			_line.Gender = gender;

			if (chkResetLabel.Checked)
			{
				_line.Label = "";
			}
			else
			{
				string label = txtLabel.Text;
				if (label == "")
				{
					label = null;
				}
				_line.Label = label;
			}

			if (_character.Metadata.BubblePosition == DialogueLayer.over)
			{
				_line.Layer = chkLayer.Checked ? "" : "under";
			}
			else
			{
				_line.Layer = chkLayer.Checked ? "over" : "";
			}

			ForfeitOperation mainForfeitOp = null;
			ForfeitOperation heavyOp = null;
			if (!string.IsNullOrEmpty(cboAttr.Text))
			{
				mainForfeitOp = new ForfeitOperation();
				mainForfeitOp.Attribute = cboAttr.Text;
				mainForfeitOp.Operator = cboOp.Text;
				mainForfeitOp.Value = txtValue.Text;
			}

			if (chkResetHeavy.Checked)
			{
				heavyOp = new ForfeitOperation();
				heavyOp.Attribute = "heavy";
				heavyOp.Value = "reset";
			}
			else if (!string.IsNullOrEmpty(cboHeavy.Text))
			{
				heavyOp = new ForfeitOperation();
				heavyOp.Attribute = "heavy";
				heavyOp.Value = cboHeavy.Text;
			}

			if (mainForfeitOp != null || heavyOp != null)
			{
				if (_line.DialogueOperations == null)
				{
					_line.DialogueOperations = new DialogueOperations();
				}
				_line.DialogueOperations.ForfeitOps.Clear();

				if (mainForfeitOp != null)
				{
					_line.DialogueOperations.ForfeitOps.Add(mainForfeitOp);
				}
				if (heavyOp != null)
				{
					_line.DialogueOperations.ForfeitOps.Add(heavyOp);
				}
			} else if (_line.DialogueOperations != null && _line.DialogueOperations.ForfeitOps != null)
			{
				_line.DialogueOperations.ForfeitOps.Clear();
				if (_line.DialogueOperations.IsEmpty())
				{
					_line.DialogueOperations = null;
				}
			}

			if (lstNick.Items.Count > 0)
			{
				if (_line.DialogueOperations == null)
				{
					_line.DialogueOperations = new DialogueOperations();
				}
				_line.DialogueOperations.NicknameOps.Clear();

				foreach (NicknameOperation op in lstNick.Items)
				{
					_line.DialogueOperations.NicknameOps.Add(op);
				}
			}
			else if (_line.DialogueOperations != null && _line.DialogueOperations.NicknameOps != null)
			{
				_line.DialogueOperations.NicknameOps.Clear();
				if (_line.DialogueOperations.IsEmpty())
				{
					_line.DialogueOperations = null;
				}
			}

			return _line;
		}

		private void valLocation_ValueChanged(object sender, EventArgs e)
		{
			if (!_settingData) DataUpdated?.Invoke(this, e);
		}

		private void chkResetAI_CheckedChanged(object sender, EventArgs e)
		{
			cboAI.Enabled = !chkResetAI.Checked;
		}

		private void chkResetLabel_CheckedChanged(object sender, EventArgs e)
		{
			txtLabel.Enabled = !chkResetLabel.Checked;
		}

		private void chkResetHeavy_CheckedChanged(object sender, EventArgs e)
		{
			cboHeavy.Enabled = !chkResetHeavy.Checked;
		}

		private void cboFontSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_settingData) DataUpdated?.Invoke(this, e);
		}

		private void chkLayer_CheckedChanged(object sender, EventArgs e)
		{
			if (!_settingData) DataUpdated?.Invoke(this, e);
		}

		private void cboAttr_SelectedIndexChanged(object sender, EventArgs e)
		{
			string attr = cboAttr.Text;
			if (attr == "redirect-finish")
			{
				cboOp.Items.Clear();
				cboOp.Items.AddRange(new string[] { "="});
				cboOp.SelectedItem = "=";
			}
			else
			{
				cboOp.Items.Clear();
				cboOp.Items.AddRange(new string[] { "=", "+", "-", "*", "/", "%" });
				cboOp.SelectedItem = "=";
			}

			if (!_settingData) DataUpdated?.Invoke(this, e);
		}

		private void cboOp_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_settingData) DataUpdated?.Invoke(this, e);
		}

		private void lstNick_SelectedIndexChanged(object sender, EventArgs e)
		{
			NicknameOperation nickOp = lstNick.SelectedItem as NicknameOperation;
			if (nickOp != null)
			{
				if (!_settingData)
				{
					saveNickOp();
					if (_selectedNickOp != null && string.IsNullOrEmpty(_selectedNickOp.Character))
					{
						_line.DialogueOperations.NicknameOps.Remove(_selectedNickOp);
						lstNick.Items.Remove(_selectedNickOp);
					}
				}
				_selectedNickOp = nickOp;
				_settingData = true;
				recNickChar.RecordKey = nickOp.Character;
				cboNickOp.Text = string.IsNullOrEmpty(nickOp.Operator) ? _nickOps["="] : _nickOps[nickOp.Operator];
				txtNickname.Text = nickOp.Name;
				valNickWeight.Value = cboNickOp.Text == _nickOps["="] ? 1 : Math.Max(valNickWeight.Minimum, Math.Min(valNickWeight.Maximum, nickOp.Weight));
				_settingData = false;
			}
		}

		private void saveNickOp()
		{
			if (_selectedNickOp == null) return;

			_selectedNickOp.Character = recNickChar.RecordKey;
			_selectedNickOp.Name = txtNickname.Text;
			_selectedNickOp.Operator = string.IsNullOrEmpty(cboNickOp.Text) ? "=" : _nickOps.FirstOrDefault(x => x.Value == cboNickOp.Text).Key;
			_selectedNickOp.Weight = _selectedNickOp.Operator == "=" ? 1 : (int)valNickWeight.Value;
		}

		private void tsNickAdd_Click(object sender, EventArgs e)
		{
			NicknameOperation op = new NicknameOperation();
			lstNick.Items.Add(op);
			if (_line.DialogueOperations == null)
			{
				_line.DialogueOperations = new DialogueOperations();
			}
			_line.DialogueOperations.NicknameOps.Add(op);
		}

		private void tsNickRemove_Click(object sender, EventArgs e)
		{
			if (_selectedNickOp == null) return;
			_line.DialogueOperations.NicknameOps.Remove(_selectedNickOp);
			lstNick.Items.Remove(_selectedNickOp);
			if (lstNick.Items.Count > 0)
			{
				lstNick.SelectedIndex = 0;
			}
			else
			{
				recNickChar.RecordKey = "";
				cboNickOp.Text = "";
				txtNickname.Text = "";
				valNickWeight.Value = 0;
			}
		}

		private void tsNickMoveDown_Click(object sender, EventArgs e)
		{
			if (lstNick.Items.Count < 2) return;
			int i = lstNick.SelectedIndex;
			if (i == lstNick.Items.Count - 1) return;
			_line.DialogueOperations.NicknameOps.Reverse(i, 2);
			NicknameOperation op = lstNick.Items[i + 1] as NicknameOperation;
			lstNick.Items[i + 1] = _selectedNickOp;
			lstNick.SelectedIndex = i + 1;
			lstNick.Items[i] = op;
			lstNick.RefreshListItems();
		}

		private void tsNickMoveUp_Click(object sender, EventArgs e)
		{
			if (lstNick.Items.Count < 2) return;
			int i = lstNick.SelectedIndex;
			if (i == 0) return;
			_line.DialogueOperations.NicknameOps.Reverse(i - 1, 2);
			NicknameOperation op = lstNick.Items[i - 1] as NicknameOperation;
			lstNick.Items[i - 1] = _selectedNickOp;
			lstNick.SelectedIndex = i - 1;
			lstNick.Items[i] = op;
			lstNick.RefreshListItems();
		}

		private void recNickChar_RecordChanged(object sender, Desktop.CommonControls.RecordEventArgs e)
		{
			if (_selectedNickOp == null || _settingData) return; 
			_selectedNickOp.Character = recNickChar.RecordKey;
			if (!string.IsNullOrEmpty(_selectedNickOp.Character)) lstNick.RefreshListItems();
		}

		private void txtNickname_TextChanged(object sender, EventArgs e)
		{
			if (_selectedNickOp == null || _settingData) return;
			_selectedNickOp.Name = txtNickname.Text;
			if (!string.IsNullOrEmpty(_selectedNickOp.Character)) lstNick.RefreshListItems();
		}

		private void cboNickOp_TextChanged(object sender, EventArgs e)
		{
			if (_selectedNickOp == null || _settingData) return;
			_selectedNickOp.Operator = string.IsNullOrEmpty(cboNickOp.Text) ? "=" : _nickOps.FirstOrDefault(x => x.Value == cboNickOp.Text).Key;
			if (_selectedNickOp.Operator == "=" && valNickWeight.Value != 1)
			{
				valNickWeight.Value = 1;
			}
		}

		private void valNickWeight_ValueChanged(object sender, EventArgs e)
		{
			if (_selectedNickOp == null || _settingData) return;
			_selectedNickOp.Weight = cboNickOp.Text == _nickOps["="] ? 1 : (int)valNickWeight.Value;
		}
	}

	public interface IDialogueDropDownControl : ISkinnedPanel, ISkinControl
	{
		event EventHandler DataUpdated;
		int RowIndex { get; }
		void SetData(int rowIndex, DialogueLine line, Character character);
		DialogueLine GetLine();
	}
}
