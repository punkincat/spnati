using Desktop;
using Desktop.CommonControls;
using System;
using System.Collections.Generic;

namespace SPNATI_Character_Editor
{
	public partial class MarkerControl : PropertyEditControl
	{
		public MarkerControl()
		{
			InitializeComponent();

			recField.RecordType = typeof(Marker);
		}

		public override void ApplyMacro(List<string> values)
		{
			if (values.Count > 0)
			{
				recField.RecordKey = values[0];
			}
		}

		public override void BuildMacro(List<string> values)
		{
			values.Add(recField.RecordKey ?? "");
		}

		public override void OnInitialAdd()
		{
			if (recField.RecordContext != null)
			{
				recField.DoSearch();
			}
		}

		private bool FilterPrivateMarkers(IRecord record)
		{
			TargetCondition condition = Data as TargetCondition;
			if (condition != null && condition.Role == "self")
			{
				return true;
			}

			Marker marker = record as Marker;
			return marker.Scope == MarkerScope.Public;
		}

		protected override void OnBindingUpdated(string property)
		{ 
			recField.RecordContext = CharacterDatabase.Get(GetBindingValue(property)?.ToString());
			if (recField.RecordContext == null && Context is Character)
			{
				recField.RecordContext = Context;
			}
			if (recField.RecordContext == null && SecondaryContext is Character)
			{
				recField.RecordContext = SecondaryContext;
			}
		}

		protected override void OnSetParameters(EditControlAttribute parameters)
		{
			MarkerAttribute attr = parameters as MarkerAttribute;
			if (!attr.ShowPrivate)
			{
				recField.RecordFilter = FilterPrivateMarkers;
			}
		}

		protected override void OnBoundData()
		{
			recField.UseAutoComplete = true;
			if (Bindings.Count > 0)
			{
				OnBindingUpdated(Bindings[0]);
			}
			else
			{
				recField.RecordContext = Context;
			}

			recField.RecordKey = GetValue()?.ToString();

			recField.RecordChanged += RecField_RecordChanged;
		}

		private void RecField_RecordChanged(object sender, RecordEventArgs e)
		{
			Save();
		}

		protected override void OnClear()
		{
			recField.RecordKey = null;
		}

		protected override void OnSave()
		{
			string key = recField.RecordKey;
			SetValue(key);
		}
	}

	public class MarkerAttribute : EditControlAttribute
	{
		public override Type EditControlType
		{
			get { return typeof(MarkerControl);	}
		}

		public bool ShowPrivate { get; set; }
	}
}
