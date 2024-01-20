using Desktop;
using Desktop.CommonControls;
using Desktop.Skinning;
using SPNATI_Character_Editor.Controls;
using SPNATI_Character_Editor.DataStructures;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Activities
{
	[Activity(typeof(Character), 70, DelayRun = true, Caption = "Collectibles")]
	[Tutorial("https://www.youtube.com/watch?v=TQqijFL0Kko")]
	public partial class CollectibleEditor : Activity
	{
		private Character _character;
		private ListViewItem _selectedItem;

		public CollectibleEditor()
		{
			InitializeComponent();
		}

		public override string Caption
		{
			get
			{
				return "Collectibles";
			}
		}

		protected override void OnInitialize()
		{
			_character = Record as Character;
			lstCollectibles.LargeImageList = new ImageList();
			lstCollectibles.LargeImageList.ImageSize = new Size(128, 128);
			lstCollectibles.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
			lstCollectibles.LargeImageList.Images.Add("???", Properties.Resources.Achievement);
			table.Context = new CollectibleContext(_character, CharacterContext.Collectible);
		}

		protected override void OnActivate()
		{
			PopulateCollectibles();
			UpdateAddButton();
		}

		private void UpdateAddButton()
		{
			CharacterHistory history = CharacterHistory.Get(_character, false);
			int limit = TestRequirements.Instance.GetAllowedCollectibles(history.Current.TotalLines);
			tsAdd.Enabled = _character.Collectibles.Count < limit;
			if (!tsAdd.Enabled)
			{
				int thresholds = _character.Collectibles.Count - 2 + 1;
				int requirement = thresholds * 600 + 1;
				tsAdd.ToolTipText = $"You need {requirement} lines to be able to add another collectible.";
			}
			else
			{
				tsAdd.ToolTipText = "Add Collectible";
			}
		}

		protected override void OnParametersUpdated(params object[] parameters)
		{
			if (parameters.Length > 0)
			{
				ValidationContext context = parameters[0] as ValidationContext;
				if (context != null)
				{
					for (int i = 0; i < lstCollectibles.Items.Count; i++)
					{
						Collectible c = lstCollectibles.Items[i].Tag as Collectible;
						if (c == context.Collectible)
						{
							lstCollectibles.Items[i].Selected = true;
							break;
						}
					}
				}
			}
		}

		private void PopulateCollectibles()
		{
			lstCollectibles.Items.Clear();
			foreach (Collectible c in _character.Collectibles.Collectibles)
			{
				AddCollectible(c, false);
			}
		}

		private void AddCollectible(Collectible c, bool select)
		{
			ListViewItem item = new ListViewItem(c.Title);
			item.Tag = c;
			item.ImageKey = ThumbnailImageKey(c);
			lstCollectibles.Items.Add(item);
			if (select)
			{
				item.Selected = true;
			}
			UpdateAddButton();
		}

		private string ThumbnailImageKey(Collectible c)
		{
			if (Config.SafeMode)
				return "";
			Bitmap thumbnail = GetImage(c.Thumbnail);
			if (thumbnail != null)
			{
				if (!lstCollectibles.LargeImageList.Images.ContainsKey(c.Thumbnail))
				{
					lstCollectibles.LargeImageList.Images.Add(c.Thumbnail, thumbnail);
				}
				return c.Thumbnail;
			}
			else
			{
				return "???";
			}
		}

		public static Bitmap GetImage(string src)
		{
			if (string.IsNullOrEmpty(src)) { return null; }
			Bitmap img = null;
			string path = Path.Combine(Config.SpnatiDirectory, src);
			if (!File.Exists(path))
			{
				return null;
			}
			try
			{
				using (Bitmap temp = new Bitmap(path))
				{
					img = new Bitmap(temp);
				}
			}
			catch { }
			return img;
		}

		private void tsAdd_Click(object sender, EventArgs e)
		{
			Collectible c = new Collectible()
			{
				Id = "new_collectible",
				Title = "New Collectible"
			};
			_character.Collectibles.Add(c);
			AddCollectible(c, true);
		}

		private void tsRemove_Click(object sender, EventArgs e)
		{
			if (lstCollectibles.SelectedItems.Count == 0) { return; }
			ListViewItem item = lstCollectibles.SelectedItems[0];
			Collectible collectible = item.Tag as Collectible;
			if (collectible != null && MessageBox.Show($"Are you sure you want to remove {collectible}? This cannot be undone.", "Remove Collectible", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				lstCollectibles.Items.Remove(item);
				_character.Collectibles.Remove(collectible);
				UpdateAddButton();
			}
		}

		private void lstCollectibles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_selectedItem != null)
			{
				table.Save();
			}

			if (lstCollectibles.SelectedItems.Count == 0)
			{
				_selectedItem = null;
				table.Data = null;
				return;
			}
			ListViewItem item = lstCollectibles.SelectedItems[0];
			_selectedItem = item;
			Collectible collectible = item.Tag as Collectible;
			collectible.Character = _character;
			table.Data = collectible;
			UpdatePreview();
			ToggleClothingVisibility();
			ToggleCostumeVisibility();
		}

		public override void Save()
		{
			table.Save();

			foreach (AlternateSkin alt in _character.Metadata.AlternateSkins)
			{
				foreach (SkinLink link in alt.Skins)
				{
					if (link.Collectible != null)
					{
						Collectible c = _character.Collectibles.Get(link.Collectible);
						if (c != null)
						{
							if (!string.IsNullOrEmpty(c.costumeFolder))
							{
								if (c.costumeFolder == link.Folder)
									continue;
							}
						}
						link.Collectible = null;
						link.IsDirty = true;
					}
				}
			}

			foreach (Collectible c in _character.Collectibles.Collectibles)
			{
				if (!string.IsNullOrEmpty(c.costumeFolder))
				{
					Costume skin = CharacterDatabase.GetSkin(c.costumeFolder);
					if (skin.Link.Collectible != c.Id)
					{
						skin.Link.Collectible = c.Id;
						skin.IsDirty = true;
					}
				}
			}
        }

		private void table_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (_selectedItem == null)
			{
				return;
			}
			if (e.PropertyName == "Title")
			{
				Collectible collectible = _selectedItem.Tag as Collectible;
				_selectedItem.Text = collectible.Title;
			}
			else if (e.PropertyName == "Thumbnail")
			{
				Collectible collectible = _selectedItem.Tag as Collectible;
				_selectedItem.ImageKey = ThumbnailImageKey(collectible);
			}
			else if (e.PropertyName == "Image")
			{
				UpdatePreview();
			}

			else if (e.PropertyName == "Extra")
			{
				ToggleClothingVisibility();
				ToggleCostumeVisibility();
			}
		}

		private void ToggleClothingVisibility()
		{
			Collectible collectible = _selectedItem.Tag as Collectible;

			Control[] rows = table.Controls.Find("PropertyTableRow", true);
			String[] names = { "ClothingName", "Classification", "Position", "Type", "Is Plural?", "ClothingImage" };

			foreach (PropertyTableRow c in rows)
			{
				if (names.Contains(c.Record.Key))
				{
					c.Visible = collectible.Wearable;
				}
			}
		}

		private void ToggleCostumeVisibility()
		{
			Collectible collectible = _selectedItem.Tag as Collectible;

			Control[] rows = table.Controls.Find("PropertyTableRow", true);
			String[] names = { "Costume" };

			foreach (PropertyTableRow c in rows)
			{
				if (names.Contains(c.Record.Key))
				{
					c.Visible = collectible.costumeUnlock;
				}
			}
		}

 
		private void UpdatePreview()
		{
			if (Config.SafeMode)
			{
				picPreview.Image = null;
				return;
			}
			Collectible collectible = _selectedItem.Tag as Collectible;
			if (collectible != null)
			{
				Bitmap bmp = GetImage(collectible.Image);
				picPreview.Image = bmp;
			}
			else
			{
				picPreview.Image = null;
			}
		}

		private void tsUp_Click(object sender, EventArgs e)
		{
			Collectible collectible = _selectedItem?.Tag as Collectible;
			if (collectible == null)
			{
				return;
			}
			ObservableCollection<Collectible> collectibles = _character.Collectibles.Collectibles;

			int index = collectibles.IndexOf(collectible);
			if (index == 0)
			{
				return;
			}
			collectibles.Remove(collectible);
			collectibles.Insert(index - 1, collectible);

			lstCollectibles.ListViewItemSorter = new CollectibleSorter(collectibles);
			lstCollectibles.Sort();
		}

		private void tsDown_Click(object sender, EventArgs e)
		{
			Collectible collectible = _selectedItem?.Tag as Collectible;
			if (collectible == null)
			{
				return;
			}
			ObservableCollection<Collectible> collectibles = _character.Collectibles.Collectibles;

			int index = collectibles.IndexOf(collectible);
			if (index == collectibles.Count - 1)
			{
				return;
			}
			collectibles.Remove(collectible);
			collectibles.Insert(index + 1, collectible);

			lstCollectibles.ListViewItemSorter = new CollectibleSorter(collectibles);
			lstCollectibles.Sort();
		}

		private class CollectibleSorter : IComparer
		{
			private ObservableCollection<Collectible> _list;
			public CollectibleSorter(ObservableCollection<Collectible> list)
			{
				_list = list;
			}

			public int Compare(object x, object y)
			{
				ListViewItem i1 = x as ListViewItem;
				ListViewItem i2 = y as ListViewItem;
				Collectible c1 = i1.Tag as Collectible;
				Collectible c2 = i2.Tag as Collectible;
				return _list.IndexOf(c1).CompareTo(_list.IndexOf(c2));
			}
		}

		protected override void OnSkinChanged(Skin skin)
		{
			base.OnSkinChanged(skin);
			lstCollectibles.BackColor = skin.FieldBackColor;
			lstCollectibles.ForeColor = skin.Surface.ForeColor;
		}
	}

	public class CollectibleContext : ICharacterContext
	{
		public ISkin Character { get; }
		public CharacterContext Context { get; }

		public CollectibleContext(ISkin character, CharacterContext context)
		{
			Character = character;
			Context = context;
		}
	}
}
