﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Controls
{
	public partial class MarkerGrid : UserControl
	{
		private Character _character;

		public event EventHandler<Marker> SelectionChanged;
		public bool AllowPrivate { get; set; }

		public bool ReadOnly
		{
			get { return gridMarkers.ReadOnly; }
			set
			{
				if (value)
				{
					gridMarkers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				}
				else
				{
					gridMarkers.SelectionMode = DataGridViewSelectionMode.CellSelect;
				}
				gridMarkers.ReadOnly = value;
			}
		}

		public MarkerGrid()
		{
			InitializeComponent();
		}

		public void SetCharacter(Character character)
		{
			_character = character;

			DataGridViewColumn col = gridMarkers.Columns["ColScope"];
			col.Visible = AllowPrivate;
			
			gridMarkers.Rows.Clear();
			if (_character == null)
				return;
			List<Marker> markers = new List<Marker>();
			foreach (var marker in _character.Markers.Values)
			{
				markers.Add(marker);
			}
			markers.Sort();
			foreach (var marker in markers)
			{
				if (!AllowPrivate && marker.Scope == MarkerScope.Private)
					continue;
				AddMarkerToGrid(marker);
			}
			if (markers.Count > 0)
			{
				SelectionChanged?.Invoke(this, markers[0]);
			}
		}

		public void Save()
		{
			if (_character == null)
				return;
			_character.Markers.Clear();
			foreach (DataGridViewRow row in gridMarkers.Rows)
			{
				string name = row.Cells["ColName"].Value?.ToString();
				string scopeStr = row.Cells["ColScope"].Value?.ToString();
				if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(scopeStr))
					continue;
				string desc = row.Cells["ColDescription"].Value?.ToString();
				Marker marker = new Marker(name);
				marker.Description = desc;
				MarkerScope scope;
				Enum.TryParse(scopeStr, out scope);
				marker.Scope = scope;
				_character.Markers.Add(marker);
			}
		}

		private void AddMarkerToGrid(Marker marker)
		{
			DataGridViewRow row = gridMarkers.Rows[gridMarkers.Rows.Add()];
			row.Tag = marker;
			row.Cells["ColName"].Value = marker.Name;
			row.Cells["ColDescription"].Value = marker.Description?.ToString();
			try
			{
				row.Cells["ColScope"].Value = marker.Scope.ToString();
			}
			catch
			{
				ErrorLog.LogError(string.Format("Marker report found a marker with invalid scope for {0}: {1}. Scope must be Private or Public.", _character, marker));
			}
		}

		private void gridMarkers_SelectionChanged(object sender, EventArgs e)
		{
			Marker marker = null;
			if (gridMarkers.SelectedRows.Count > 0)
			{
				DataGridViewRow row = gridMarkers.SelectedRows[0];
				marker = row.Tag as Marker;
			}
			SelectionChanged?.Invoke(this, marker);
		}
	}
}
