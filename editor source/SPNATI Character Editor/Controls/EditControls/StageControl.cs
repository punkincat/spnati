using Desktop;
using Desktop.CommonControls;
using Desktop.Skinning;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SPNATI_Character_Editor
{
	public partial class StageControl : PropertyEditControl
	{
		private bool _filterStagesToTarget;
		private string _sourceProperty;
		private MemberInfo _sourceMember;
		private string _alternateProperty;
		private MemberInfo _alternateMember;
		private string _skinVariable;
		private Costume _costumeSelected;

		public StageControl()
		{
			InitializeComponent();
			cboFrom.KeyMember = "Id";
			cboTo.KeyMember = "Id";
			recRefCostume.RecordType = typeof(Costume);
			recRefCostume.RecordFilter = FilterRefCostume;
		}

		private bool FilterRefCostume(IRecord record)
		{

			string key = _sourceMember.GetValue(Data)?.ToString();
			Character character = CharacterDatabase.Get(key);

			if (character == null)
			{
				return false;
			}
			Costume costume = record as Costume;
			return costume.Character == character || costume.Key == "default";
		}

		protected override void OnSetParameters(EditControlAttribute parameters)
		{
			StageSelectAttribute attr = parameters as StageSelectAttribute;
			_filterStagesToTarget = attr.FilterStagesToTarget;
			if (parameters.BoundProperties != null)
			{
				_sourceProperty = parameters.BoundProperties[0];
				if (parameters.BoundProperties.Length > 1)
				{
					_alternateProperty = parameters.BoundProperties[1];
				}
			}
			_skinVariable = attr.SkinVariable;
		}

		public override void ApplyMacro(List<string> values)
		{
			if (values.Count > 0)
			{
				ApplyValue(values[0]);
			}
		}

		public override void BuildMacro(List<string> values)
		{
			string value = BuildValue() ?? "";
			values.Add(value);
		}

		protected override void OnBoundData()
		{
			if (_sourceProperty != null)
			{
				_sourceMember = Data.GetType().GetMember(_sourceProperty)[0];
			}
			if (_alternateProperty != null)
			{
				_alternateMember = Data.GetType().GetMember(_alternateProperty)[0];
			}

			FillItems();

			string value = GetValue()?.ToString();
			ApplyValue(value);

			cboFrom.SelectedIndexChanged += cboFrom_SelectedIndexChanged;
			cboTo.SelectedIndexChanged += cboTo_SelectedIndexChanged;
		}

		private void ApplyValue(string value)
		{
			string min = value?.ToString();
			string max = null;
			if (!string.IsNullOrEmpty(min))
			{
				string[] pieces = min.Split('-');
				min = pieces[0];
				if (pieces.Length > 1)
					max = pieces[1];
			}

			SetCombo(cboFrom, min);
			SetCombo(cboTo, max);
		}

		private void SetCombo(SkinnedComboBox box, string stage)
		{
			for (int i = 0; i < box.Items.Count; i++)
			{
				StageName stageName = box.Items[i] as StageName;
				if (stageName != null && stageName.Id == stage)
				{
					box.SelectedIndex = i;
					return;
				}
			}
			box.Text = stage; //If couldn't set an object, just set the text
		}

		protected override void OnBindingUpdated(string property)
		{
			FillItems();
		}

		private void FillItems()
		{
			Case selectedCase = Data as Case;
			if (selectedCase == null)
			{
				selectedCase = Context as Case;
			}

			string key = _sourceMember.GetValue(Data)?.ToString();
			Character character = CharacterDatabase.Get(key);
			if (character == null)
			{
				if (Context is Character)
				{
					character = Context as Character;
				}
				//else if (SecondaryContext is Character)
				//{
				//	character = SecondaryContext as Character;
				//}

				if (character == null && _alternateMember != null)
				{
					string role = _alternateMember.GetValue(Data)?.ToString();
					if (role == "self")
					{
						character = SecondaryContext as Character;
					}
				}
			}

			string tag = selectedCase?.Tag;
			string filterType = null;
			string filterPosition = null;
			bool removing = false;
			bool removed = false;
			bool lookForward = false;
			bool filterStages = _filterStagesToTarget;
			if (Data is TargetCondition && ((TargetCondition)Data).Role == "target")
			{
				filterStages = true;
			}
			if (tag != null && filterStages)
			{
				removing = tag.Contains("removing_");
				removed = tag.Contains("removed_");
				lookForward = removing || tag == "opponent_stripping";
				if (removing || removed)
				{
					int index = tag.LastIndexOf('_');
					if (index >= 0 && index < tag.Length - 1)
					{
						filterType = tag.Substring(index + 1);
						if (filterType == "accessory")
							filterType = "extra";
					}
				}

				if (string.IsNullOrEmpty(filterType))
				{
					removing = tag.Contains("will_be_visible");
					removed = tag.Contains("is_visible");
					lookForward = removing || tag == "opponent_stripping";
					if (removing || removed)
					{
						filterType = "important";
						if (tag.Contains("crotch"))
						{
							filterPosition = "lower";
						}
						else if (tag.Contains("chest"))
						{
							filterPosition = "upper";
						}
					}
				}
			}

			List<object> data = new List<object>();
			cboFrom.BindingContext = new BindingContext();
			cboTo.BindingContext = new BindingContext();
			if (character == null || character.Layers == 0)
			{
				//If the character is not valid, still allow something but there's no way to give a useful name to it
				for (int i = 0; i < 8 + Clothing.ExtraStages; i++)
				{
					data.Add(i);
				}
				cboFrom.DataSource = data;
				cboTo.DataSource = data;
			}
			else
			{
				IWardrobe skin = GetSkin();
				for (int i = 0; i < character.Layers + Clothing.ExtraStages; i++)
				{
					if (filterStages)
					{
						if (filterType != null)
						{
							//Filter out stages that will never be valid
							if (i >= 0 && i <= character.Layers)
							{
								int layer = removed ? i - 1 : i;
								if (layer < 0 || layer >= character.Layers)
									continue;

								Clothing clothing = character.GetConvertedWardrobe()[character.Layers - layer - 1];
								string realType = clothing.Type;
								if (filterType != realType.ToLower() || (filterPosition != null && clothing.Position != filterPosition))
									continue;
							}
							else continue;
						}
					}
					data.Add(character.LayerToStageName(i, lookForward, skin));
				}
				cboFrom.DataSource = data;
				cboTo.DataSource = data;
			}
		}

		private IWardrobe GetSkin()
		{
			string key = _sourceMember.GetValue(Data)?.ToString();
			Character character = CharacterDatabase.Get(key);
			if (character == null)
			{
				if (Context is Character)
				{
					character = Context as Character;
				}
				else if (SecondaryContext is Character)
				{
					character = SecondaryContext as Character;
				}
			}

			if (_costumeSelected != null)
			{
				key = _costumeSelected.Id;
				if (key == "default")
				{
					return character;
				}

				Costume costume = CharacterDatabase.GetSkin("opponents/reskins/" + key + "/");
				if (costume != null)
				{
					return costume;
				}
			}

			Case theCase = Data as Case;
			if (theCase == null)
			{
				return character;
			}

			if (_skinVariable == "~target.costume~")
			{
				foreach (ExpressionTest expression in theCase.Expressions)
				{
					if (expression.Expression == _skinVariable)
					{
						key = expression.Value;
						if (!string.IsNullOrEmpty(key))
						{
							Costume costume = CharacterDatabase.GetSkin("opponents/reskins/" + key + "/");
							if (costume != null)
							{
								return costume;
							}
						}
						break;
					}
				}
			}
			else if (_skinVariable == "~_.costume~")
			{
				string other = "";
				
				foreach (TargetCondition cond in theCase.Conditions)
				{
					if (cond.Role != "self" && cond.Role != "target" && cond.Character != null)
					{
						other = cond.Character;
					}
				}

				if (!string.IsNullOrEmpty(other))
				{
					key = CharacterDatabase.GetId(other);
					string variable = $"~{key}.costume~";
					foreach (ExpressionTest expression in theCase.Expressions)
					{
						if (expression.Expression == variable)
						{
							key = expression.Value;
							if (!string.IsNullOrEmpty(key))
							{
								Costume costume = CharacterDatabase.GetSkin("opponents/reskins/" + key + "/");
								if (costume != null)
								{
									return costume;
								}
							}
							break;
						}
					}
				}
			}
			return character;
		}

		protected override void OnClear()
		{
			cboFrom.Text = "";
			cboTo.Text = "";
			Save();
		}

		private string BuildValue()
		{
			string min = ReadStage(cboFrom);
			string max = ReadStage(cboTo);

			if (string.IsNullOrEmpty(min))
			{
				return null;
			}
			string value = min;
			if (!string.IsNullOrEmpty(max) && min != max)
			{
				value += "-" + max;
			}
			return value;
		}

		protected override void OnSave()
		{
			string value = BuildValue();
			SetValue(value);
		}

		private string ReadStage(SkinnedComboBox box)
		{
			if (string.IsNullOrEmpty(box.Text))
			{
				return null;
			}
			StageName stage = box.SelectedItem as StageName;
			if (stage == null)
			{
				//Must be a generic stage
				return box.Text;
			}

			return stage.Id;
		}

		private void cboFrom_SelectedIndexChanged(object sender, EventArgs e)
		{
			Save();
		}

		private void cboTo_SelectedIndexChanged(object sender, EventArgs e)
		{
			Save();
		}

		private void recRefCostume_RecordChanged(object sender, RecordEventArgs e)
		{
			_costumeSelected = recRefCostume.Record as Costume;
			FillItems();
		}
	}

	public class StageSelectAttribute : EditControlAttribute
	{
		public override Type EditControlType
		{
			get { return typeof(StageControl); }
		}

		public bool FilterStagesToTarget { get; set; }

		public string SkinVariable { get; set; }
	}
}
