using Desktop;
using System;

namespace SPNATI_Character_Editor.Activities
{
	[Activity(typeof(Character), 315, DelayRun = true, Caption = "Scratch Pad")]
	[Tutorial("https://www.youtube.com/watch?v=CLRoebAM4zk")]
	public partial class ScratchPad : Activity
	{
		private Character _character;

		public ScratchPad()
		{
			InitializeComponent();
		}

		public override string Caption
		{
			get { return "Scratch Pad"; }
		}

		protected override void OnInitialize()
		{
			_character = Record as Character;
		}

		protected override void OnFirstActivate()
		{
			scratchPadControl1.SetCharacter(_character);
		}

		protected override void OnDeactivate()
		{
			scratchPadControl1.Abort();
		}

		public override void Save()
		{
			scratchPadControl1.Save();
		}
	}
}
