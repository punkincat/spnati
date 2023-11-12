using Desktop.Skinning;
using System;
using Desktop;
using SPNATI_Character_Editor.Categories;
using SPNATI_Character_Editor.DataStructures;

namespace SPNATI_Character_Editor.Forms
{
	public partial class WardrobeAdvancedForm : SkinnedForm
	{
		private Clothing _clothing;
		public ClothingLayer layer = new ClothingLayer();
		public string item;
		public string reveal;
		public string fromStage;
		public bool fromDeal;
		private int _layers;
		public bool notFromStart = false;
		public bool revealBool = false;
		public bool select = false;
		public bool define = false;
		public bool differentItem = false;

		public WardrobeAdvancedForm(IWardrobe wardrobe, int row)
		{
			InitializeComponent();
			_layers = wardrobe.Layers;
			_clothing = wardrobe.GetClothing(_layers - row - 1);

			recGeneric.RecordType = typeof(ClothingCategory);
			recType.RecordType = typeof(ClothingTypeCategory);
			recType.RecordFilter = TypeFilter;
			recPosition.RecordType = typeof(ClothingPositionCategory);
			recPosition.RecordFilter = FilterPosition;
			cboFromDeal.Items.AddRange(new string[] { "", "Stripping", "Stripped" });
			cboFromStage.Items.Add("");
			for (int i = _layers - 1; i >= _layers - row ; i--)
			{
				Clothing clothing = wardrobe.GetClothing(i);
				if (clothing.Type != "skip")
				{
					cboFromStage.Items.Add(clothing.ToString());
				}
			}
			cboItem.Items.Add("");
			for (int i = 0; i < _layers - row - 1; i++)
			{
				Clothing clothing = wardrobe.GetClothing(i);
				if (clothing.Type != "skip")
				{
					cboItem.Items.Add(clothing.ToString());
				}
			}
			cboReveal.Items.AddRange(new string[] { "", "none", "upper", "lower", "both" });

			chkNotFromStart.Checked = !string.IsNullOrEmpty(_clothing.FromStage);
			if (!string.IsNullOrEmpty(_clothing.FromStage))
			{
				int index = _clothing.FromStage.ToInt();
				if (index < _layers)
				{
					chkNotFromStart.Checked = true;
					cboFromDeal.Text = _clothing.FromDeal ? "Stripped" : "Stripping";
					cboFromStage.Text = wardrobe.GetClothing(index).ToString();
				}
			}

			if (_clothing.Stripping != null)
			{
				chkDifferentItem.Checked = true;
				chkDefine.Checked = true;
				layer = _clothing.Stripping;
				if (!string.IsNullOrEmpty(layer.GenericName))
				{
					bool validCategory = false;
					layer.GenericName = layer.GenericName.ToLower();

					foreach (ClothingCategoryItem cc in ClothingDefinitions.Instance.Categories)
					{
						if (cc.Key == layer.GenericName)
						{
							validCategory = true;
							break;
						}
					}

					if (!validCategory)
					{
						layer.GenericName = null;
					}
				}
				txtName.Text = layer.Name;
				recGeneric.RecordKey = layer.GenericName;
				recType.RecordKey = layer.Type;
				recPosition.RecordKey = layer.Position;
				chkPlural.Checked = layer.Plural;
				chkReveal.Checked = !string.IsNullOrEmpty(layer.Reveal);
				cboReveal.Text = layer.Reveal ?? "";
			}
			else
			{
				chkReveal.Checked = !string.IsNullOrEmpty(_clothing.Reveal);
				cboReveal.Text = _clothing.Reveal ?? "";
				if (!string.IsNullOrEmpty(_clothing.StrippingLayer))
				{
					int index = _clothing.StrippingLayer.ToInt();
					if (index < _layers)
					{
						chkDifferentItem.Checked = true;
						chkSelect.Checked = true;
						cboItem.Text = wardrobe.GetClothing(index).ToString();
					}
				}
			}
			OnUpdateSkin(SkinManager.Instance.CurrentSkin);
		}

		private bool FilterPosition(IRecord record)
		{
			string type = recType.Record?.Key;
			if (type == "important" || type == "major")
			{
				return record.Key == "upper" || record.Key == "lower" || record.Key == "both";
			}
			return true;
		}

		private bool TypeFilter(IRecord record)
		{
			return record.Key != "skip";
		}

		private void recPosition_RecordChanged(object sender, Desktop.CommonControls.RecordEventArgs e)
		{
			layer.Position = recPosition.RecordKey;
		}

		private void recGeneric_RecordChanged(object sender, Desktop.CommonControls.RecordEventArgs e)
		{
			layer.GenericName = recGeneric.RecordKey;
		}

		private void recType_RecordChanged(object sender, Desktop.CommonControls.RecordEventArgs e)
		{
			layer.Type = recType.RecordKey;
		}
		private void chkPlural_CheckedChanged(object sender, EventArgs e)
		{
			layer.Plural = chkPlural.Checked;
		}

		private void txtName_TextChanged(object sender, EventArgs e)
		{
			layer.Name = txtName.Text;
		}

		private void chkSelect_CheckedChanged(object sender, EventArgs e)
		{
			select = chkSelect.Checked;
			cboItem.Enabled = chkSelect.Checked;
			if (chkSelect.Checked)
			{
				chkDefine.Checked = false;
			}
		}

		private void chkDefine_CheckedChanged(object sender, EventArgs e)
		{
			define = chkDefine.Checked;
			txtName.Enabled = chkDefine.Checked;
			recGeneric.Enabled = chkDefine.Checked;
			chkPlural.Enabled = chkDefine.Checked;
			recPosition.Enabled = chkDefine.Checked;
			recType.Enabled = chkDefine.Checked;
			if (chkDefine.Checked)
			{
				chkSelect.Checked = false;
			}
		}

		private void chkDifferentItem_CheckedChanged(object sender, EventArgs e)
		{
			differentItem = chkDifferentItem.Checked;
			chkSelect.Enabled = chkDifferentItem.Checked;
			chkDefine.Enabled = chkDifferentItem.Checked;
		}

		private void chkNotFromStart_CheckedChanged(object sender, EventArgs e)
		{
			notFromStart = chkNotFromStart.Checked;
			cboFromDeal.Enabled = chkNotFromStart.Checked;
			cboFromStage.Enabled = chkNotFromStart.Checked;
		}

		private void chkReveal_CheckedChanged(object sender, EventArgs e)
		{
			revealBool = chkReveal.Checked;
			cboReveal.Enabled = chkReveal.Checked;
		}

		private void cboFromDeal_SelectedIndexChanged(object sender, EventArgs e)
		{
			fromDeal = cboFromDeal.Text == "Stripped";
		}

		private void cboFromStage_SelectedIndexChanged(object sender, EventArgs e)
		{
			fromStage = cboFromStage.Text;
		}

		private void cboReveal_SelectedIndexChanged(object sender, EventArgs e)
		{
			reveal = cboReveal.Text;
		}

		private void cboItem_SelectedIndexChanged(object sender, EventArgs e)
		{
			item = cboItem.Text;
		}
	}
}
