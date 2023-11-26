using Desktop;
using SPNATI_Character_Editor.Categories;
using SPNATI_Character_Editor.DataStructures;
using SPNATI_Character_Editor.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Controls
{
	[Activity(typeof(Character), 1)]
	[Activity(typeof(Costume), 5)]
	[Tutorial("https://www.youtube.com/watch?v=48ak4Ao6p5I")]
	public partial class WardrobeEditor : Activity
	{
		private IWardrobe _wardrobe;
		private bool _populatingWardrobe;
		private Queue<WardrobeChange> _wardrobeChanges = new Queue<WardrobeChange>();
		private WardrobeRestrictions _restrictions;

		public WardrobeEditor()
		{
			InitializeComponent();
			ColGeneric.RecordType = typeof(ClothingCategory);
			ColType.RecordType = typeof(ClothingTypeCategory);
			ColType.RecordFilter = TypeFilter;
			ColPosition.RecordType = typeof(ClothingPositionCategory);
			ColPosition.RecordFilter = FilterPosition;
			ColPlural.TrueValue = true;
			ColPosition.AllowsNew = true;
		}

		public override string Caption
		{
			get { return "Wardrobe"; }
		}

		protected override void OnInitialize()
		{
			_wardrobe = Record as IWardrobe;

			_restrictions = _wardrobe.GetWardrobeRestrictions();
			
			if (_restrictions.HasFlag(WardrobeRestrictions.LayerCount))
			{
				_populatingWardrobe = true;
				gridWardrobe.AllowUserToAddRows = false;
				gridWardrobe.AllowUserToDeleteRows = false;
				cmdClothesDown.Enabled = false;
				cmdClothesUp.Enabled = false;
				_populatingWardrobe = false;
			}
		}

		protected override void OnFirstActivate()
		{
			PopulateGrid();
		}

		private void PopulateGrid()
		{
			_populatingWardrobe = true;
			gridWardrobe.Rows.Clear();
			for (int i = _wardrobe.Layers - 1; i >= 0; i--)
			{
				Clothing c = _wardrobe.GetClothing(i);

				if (!string.IsNullOrEmpty(c.GenericName))
				{
					bool validCategory = false;
					c.GenericName = c.GenericName.ToLower();

					foreach (ClothingCategoryItem cc in ClothingDefinitions.Instance.Categories)
					{
						if (cc.Key == c.GenericName)
						{
							validCategory = true;
							break;
						}
					}

					if (!validCategory)
					{
						c.GenericName = null;
					}
				}

				try
				{
					DataGridViewRow row = gridWardrobe.Rows[gridWardrobe.Rows.Add(c.Name, null, c.GenericName, c.Plural, c.Type, c.Position)];
					row.Tag = c;
					if (c.HasAdv())
					{
						row.Cells["ColMore"].Tag = "1";
					}
					if (_restrictions.HasFlag(WardrobeRestrictions.LayerTypes))
					{
						row.Cells["ColType"].ReadOnly = true;
					}
				}
				catch { }
			}
			_populatingWardrobe = false;
		}

		public override void Save()
		{
			//Feeling lazy: Just mark it dirty whenever this activity is visited
			_wardrobe.IsDirty = true;

			if (gridWardrobe.SelectedCells.Count > 0)
			{
				gridWardrobe.EndEdit();
				SaveLayer(gridWardrobe.SelectedCells[0].OwningRow.Index);
			}
			ApplyWardrobeChanges();
		}

		private void SaveLayer(int rowIndex)
		{
			DataGridViewRow row = gridWardrobe.Rows[rowIndex];
			string type = row.Cells[nameof(ColType)].Value?.ToString();
			string lowercase = row.Cells[nameof(ColName)].Value?.ToString();
			if (string.IsNullOrEmpty(lowercase) && type != "skip") { return; }
			string name = row.Cells[nameof(ColGeneric)].Value?.ToString();
			bool plural = row.Cells[nameof(ColPlural)].Value != null ? (bool)row.Cells[nameof(ColPlural)].Value : false;
			string position = row.Cells[nameof(ColPosition)].Value?.ToString();
			Clothing layer = row.Tag as Clothing;
			if (layer != null)
			{
				layer.GenericName = name;
				layer.Name = lowercase;
				layer.Plural = plural;
				layer.Type = type;
				layer.Position = position;
			}
		}

		private void ApplyWardrobeChanges()
		{
			if (_wardrobeChanges.Count > 0)
			{
				_wardrobe.ApplyWardrobeChanges(_wardrobeChanges);
				_wardrobeChanges.Clear();
				Workspace.SendMessage(WorkspaceMessages.WardrobeUpdated);
			}
		}

		private void cmdClothesUp_Click(object sender, EventArgs e)
		{
			if (gridWardrobe.SelectedCells.Count == 0) { return; }
			int rowIndex = gridWardrobe.SelectedCells[0].OwningRow.Index;
			if (rowIndex == 0) { return; }

			int colIndex = gridWardrobe.SelectedCells[0].OwningColumn.Index;
			DataGridViewRow row = gridWardrobe.Rows[rowIndex];
			if (row.IsNewRow) { return; }

			Clothing layer = row.Tag as Clothing;
			int index = _wardrobe.MoveUp(layer);
			_wardrobeChanges.Enqueue(new WardrobeChange(WardrobeChangeType.MoveUp, index));

			_populatingWardrobe = true;
			gridWardrobe.Rows.Remove(row);
			gridWardrobe.Rows.Insert(rowIndex - 1, row);
			gridWardrobe.ClearSelection();
			gridWardrobe.Rows[rowIndex - 1].Cells[colIndex].Selected = true;
			_populatingWardrobe = false;
		}

		private void cmdClothesDown_Click(object sender, EventArgs e)
		{
			if (gridWardrobe.SelectedCells.Count == 0) { return; }
			int rowIndex = gridWardrobe.SelectedCells[0].OwningRow.Index;
			if (rowIndex >= gridWardrobe.Rows.Count - 2) { return; }

			int colIndex = gridWardrobe.SelectedCells[0].OwningColumn.Index;
			DataGridViewRow row = gridWardrobe.Rows[rowIndex];
			if (row.IsNewRow) { return; }

			Clothing layer = row.Tag as Clothing;
			int index = _wardrobe.MoveDown(layer);
			_wardrobeChanges.Enqueue(new WardrobeChange(WardrobeChangeType.MoveDown, index));

			_populatingWardrobe = true;
			gridWardrobe.Rows.Remove(row);
			gridWardrobe.Rows.Insert(rowIndex + 1, row);
			gridWardrobe.ClearSelection();
			gridWardrobe.Rows[rowIndex + 1].Cells[colIndex].Selected = true;
			_populatingWardrobe = false;
		}

		private void gridWardrobe_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
		{
			if (_populatingWardrobe) { return; }
			Clothing layer = _wardrobe.GetClothing(_wardrobe.Layers - e.RowIndex - 1);
			if (layer != null)
			{
				int index = _wardrobe.RemoveLayer(layer);
				_wardrobeChanges.Enqueue(new WardrobeChange(WardrobeChangeType.Remove, index));
			}
			for (int i = 0; i < _wardrobe.Layers; i++)
			{
				Clothing clothing = _wardrobe.GetClothing(i);
				if (clothing.HasAdv())
				{
					MessageBox.Show("Some other clothing items have advanced properties.\nAfter removing this item, check if those properties are still correct.");
					return;
				}
			}
		}

		private void gridWardrobe_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			if (_populatingWardrobe) { return; }

			Clothing layer = new Clothing();
			int index = _wardrobe.AddLayer(layer);
			DataGridViewRow row = gridWardrobe.Rows[index];
			row.Tag = layer;
			_wardrobeChanges.Enqueue(new WardrobeChange(WardrobeChangeType.Add, index));
			for (int i = 0; i < _wardrobe.Layers; i++)
			{
				Clothing clothing = _wardrobe.GetClothing(i);
				if (clothing.HasAdv())
				{
					MessageBox.Show("Some other clothing items have advanced properties.\nAfter adding a new item, check if those properties are still correct.");
					return;
				}
			}
		}

		private void gridWardrobe_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			SaveLayer(e.RowIndex);
		}

		private void gridWardrobe_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			DataGridViewRow row = gridWardrobe.Rows[e.RowIndex];
			if (row.IsNewRow) { return; }
			if (e.ColumnIndex == 0 && string.IsNullOrEmpty(e.FormattedValue?.ToString()))
			{
				string type = row.Cells["ColType"].Value?.ToString();
				if (type != "skip")
				{
					MessageBox.Show("Layer of type other than skip cannot have an empty name.");
					e.Cancel = true;
				}
			}
			if (e.ColumnIndex == ColPosition.Index && _restrictions.HasFlag(WardrobeRestrictions.LayerTypes))
			{
				string type = row.Cells["ColType"].Value?.ToString();
				string position = row.Cells["ColPosition"].Value?.ToString();
				if (type == "important" && position != e.FormattedValue?.ToString() && (position == "upper" || position == "lower"))
				{
					MessageBox.Show("Cannot change position for an important layer.");
					e.Cancel = true;
				}
			}
		}

		private void gridWardrobe_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex != ColDelete.Index && e.ColumnIndex != ColMore.Index)
			{
				return;
			}
			DataGridViewColumn col = gridWardrobe.Columns[e.ColumnIndex];
			if (col == ColMore)
			{
				SaveLayer(e.RowIndex);
				ShowAdvForm(e.RowIndex);
			}
			else if (col == ColDelete)
			{
				DataGridViewRow row = gridWardrobe.Rows[e.RowIndex];
				if (row != null && !row.IsNewRow)
				{
					if (!gridWardrobe.AllowUserToDeleteRows)
					{
						MessageBox.Show("You cannot delete layers from within the editor because this is highly disruptive to other characters targeting yours.");
						return;
					}

					gridWardrobe.Rows.RemoveAt(e.RowIndex);
					row.Tag = null;
				}
			}
		}

		private void ShowAdvForm(int rowIndex)
		{
			DataGridViewRow row = gridWardrobe.Rows[rowIndex];
			if (row.Tag == null)
			{
				return;
			}
			Clothing clothing = row.Tag as Clothing;
			if (clothing.Type == "skip")
			{
				MessageBox.Show("Skipped layers cannot have advanced properties.");
				return;
			}
			DataGridViewCell cell = row.Cells[nameof(ColMore)];
			WardrobeAdvancedForm form = new WardrobeAdvancedForm(_wardrobe, rowIndex);
			if (form.ShowDialog() == DialogResult.OK)
			{
				clothing.FromDeal = form.notFromStart && form.fromDeal && !string.IsNullOrEmpty(form.fromStage);
				clothing.FromStage = "";
				for (int i = 0; i < _wardrobe.Layers && form.notFromStart; i++)
				{
					if (_wardrobe.GetClothing(i).ToString() == form.fromStage)
					{
						clothing.FromStage = i.ToString();
						break;
					}
				}
				clothing.Reveal = form.revealBool && !string.IsNullOrEmpty(form.reveal) ? form.reveal : "";
				clothing.StrippingLayer = "";
				for (int i = 0; i < _wardrobe.Layers && form.differentItem && form.select; i++)
				{
					if (_wardrobe.GetClothing(i).ToString() == form.item)
					{
						clothing.StrippingLayer = i.ToString();
						break;
					}
				}

				if (!string.IsNullOrEmpty(form.layer.Name) && !string.IsNullOrEmpty(form.layer.Type) && form.differentItem && form.define)
				{
					clothing.Stripping = form.layer;
					clothing.Stripping.Reveal = form.revealBool? form.reveal : "";
					clothing.Reveal = "";
				}
				else
				{
					clothing.Stripping = null;
					clothing.Reveal = form.revealBool? form.reveal : "";
				}
			}
			cell.Tag = clothing.HasAdv() ? "1" : null;
			row.Tag = clothing;
			SaveLayer(rowIndex);
		}

		private void gridWardrobe_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			if (e.RowIndex == -1) return;
			if (e.ColumnIndex == ColDelete.Index)
			{
				Image img = Properties.Resources.Delete;
				e.Paint(e.CellBounds, DataGridViewPaintParts.All);
				var w = img.Width;
				var h = img.Height;
				var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
				var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

				e.Graphics.DrawImage(img, new Rectangle(x, y, w, h));
				e.Handled = true;
			}
			else if (e.ColumnIndex == ColMore.Index)
			{
				DataGridViewCell cell = gridWardrobe.Rows[e.RowIndex].Cells[e.ColumnIndex];
				Image img = cell.Tag == null? Properties.Resources.Ellipsis : Properties.Resources.EllipsisFilled;
				e.Paint(e.CellBounds, DataGridViewPaintParts.All);
				var w = img.Width;
				var h = img.Height;
				var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
				var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

				e.Graphics.DrawImage(img, new Rectangle(x, y, w, h));
				e.Handled = true;
			}
		}

		private bool FilterPosition(IRecord record)
		{
			if (gridWardrobe.SelectedCells.Count > 0)
			{
				int rowIndex = gridWardrobe.SelectedCells[0].RowIndex;
				DataGridViewRow row = gridWardrobe.Rows[rowIndex];
				string type = row.Cells[nameof(ColType)].Value?.ToString();
				if (type == "important" || type == "major")
				{
					return record.Key == "upper" || record.Key == "lower" || record.Key == "both";
				}
			}
			return true;
		}

		private bool TypeFilter(IRecord record)
		{
			if (_restrictions.HasFlag(WardrobeRestrictions.NoSkip))
			{
				return record.Key == "extra" || record.Key == "minor" || record.Key == "major" || record.Key == "important";
			}
			if (gridWardrobe.SelectedCells.Count > 0)
			{
				int rowIndex = gridWardrobe.SelectedCells[0].RowIndex;
				if (rowIndex == 0)
				{
					return record.Key == "extra" || record.Key == "minor" || record.Key == "major" || record.Key == "important";
				}
			}
			return true;
		}
	}
}
