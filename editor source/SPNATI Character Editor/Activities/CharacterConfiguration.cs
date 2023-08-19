using Desktop;
using System.Linq;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Activities
{
	[Spacer]
	[Activity(typeof(Character), 1000, DelayRun = true, Caption = "Configuration")]
	public partial class CharacterConfiguration : Activity
	{
		private Character _character;
		private CharacterEditorData _editorData;

		public override string Caption
		{
			get { return "Configuration"; }
		}

		public CharacterConfiguration()
		{
			InitializeComponent();
		}

		protected override void OnInitialize()
		{
			_character = Record as Character;
			_editorData = CharacterDatabase.GetEditorData(_character);
		}

		protected override void OnFirstActivate()
		{
			chkOnlyCustomPoses.Checked = _editorData.OnlyCustomPoses;
			chkHidePrefixlessImages.Checked = _editorData.HidePrefixlessImages;
			foreach (string prefix in _editorData.IgnoredPrefixes)
			{
				gridPrefixes.Rows.Add(new object[] { prefix });
			}
			foreach (string marker in _editorData.PosePreviewMarkers)
			{
				gridMarkers.Rows.Add(new object[] { marker });
			}
		}

		public override void Save()
		{
			_editorData.OnlyCustomPoses = chkOnlyCustomPoses.Checked;
			_editorData.HidePrefixlessImages = chkHidePrefixlessImages.Checked;
			_editorData.IgnoredPrefixes.Clear();
			foreach (DataGridViewRow row in gridPrefixes.Rows)
			{
				string prefix = row.Cells[0].Value?.ToString();
				if (!string.IsNullOrEmpty(prefix))
				{
					_editorData.IgnoredPrefixes.Add(prefix);	
				}
			}
			_editorData.PosePreviewMarkers.Clear();
			foreach (DataGridViewRow row in gridMarkers.Rows)
			{
				string marker = row.Cells[0].Value?.ToString();
				if (!string.IsNullOrEmpty(marker))
				{
					_editorData.PosePreviewMarkers.Add(marker);
				}
			}
			Workspace.SendMessage(WorkspaceMessages.UpdateMarkers, Enumerable.Empty<string>());
		}
	}
}
