using Desktop;
using Desktop.CommonControls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SPNATI_Character_Editor
{
	public partial class TagAdvControl : PropertyEditControl
	{

		public TagAdvControl()
		{
			InitializeComponent();

			recTag1A.RecordType = typeof(Tag);
			recTag1B.RecordType = typeof(Tag);
			recTag2A.RecordType = typeof(Tag);
			recTag2B.RecordType = typeof(Tag);

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
			values.Add(BuildValue() ?? "");
		}

		protected override void OnBoundData()
		{
			string value = GetValue()?.ToString();
			ApplyValue(value);
			chkNegate1A.CheckedChanged += ChkNegate1A_CheckedChanged;
			chkNegate1B.CheckedChanged += ChkNegate1B_CheckedChanged;
			chkNegate2A.CheckedChanged += ChkNegate2A_CheckedChanged;
			chkNegate2B.CheckedChanged += ChkNegate2B_CheckedChanged;
			recTag1A.RecordChanged += RecTag1A_RecordChanged;
			recTag1B.RecordChanged += RecTag1B_RecordChanged;
			recTag2A.RecordChanged += RecTag2A_RecordChanged;
			recTag2B.RecordChanged += RecTag2B_RecordChanged;
		}

		private void ApplyValue(string value)
		{
			value = value ?? "";
			string pattern = @"^([^\&\|]*)(\&?)([^\&\|]*)(\|?)([^\&\|]*)(\&?)([^\&\|]*)";
			Regex reg = new Regex(pattern);
			Match regMatch = reg.Match(value);
			if (regMatch.Success)
			{
				string val1A = regMatch.Groups[1].Value;
				string val1B = regMatch.Groups[3].Value;
				string val2A = regMatch.Groups[5].Value;
				string val2B = regMatch.Groups[7].Value;
				if (!string.IsNullOrEmpty(val1A))
				{
					if (val1A.StartsWith("!"))
					{
						val1A = val1A.Substring(1);
						chkNegate1A.Checked = true;
					}
					recTag1A.RecordKey = val1A;
				}
				else
				{
					chkNegate1A.Checked = false;
					recTag1A.RecordKey = "";
				}
				if (!string.IsNullOrEmpty(val1B))
				{
					if (val1B.StartsWith("!"))
					{
						val1B = val1B.Substring(1);
						chkNegate1B.Checked = true;
					}
					recTag1B.RecordKey = val1B;
				}
				else
				{
					chkNegate1B.Checked = false;
					recTag1B.RecordKey = "";
				}
				if (!string.IsNullOrEmpty(val2A))
				{
					if (val2A.StartsWith("!"))
					{
						val2A = val2A.Substring(1);
						chkNegate2A.Checked = true;
					}
					recTag2A.RecordKey = val2A;
				}
				else
				{
					chkNegate2A.Checked = false;
					recTag2A.RecordKey = "";
				}
				if (!string.IsNullOrEmpty(val2B))
				{
					if (val2B.StartsWith("!"))
					{
						val2B = val2B.Substring(1);
						chkNegate2B.Checked = true;
					}
					recTag2B.RecordKey = val2B;
				}
				else
				{
					chkNegate2B.Checked = false;
					recTag2B.RecordKey = "";
				}
			}
		}

		protected override void OnClear()
		{
			chkNegate1A.Checked = false;
			chkNegate1B.Checked = false;
			chkNegate2A.Checked = false;
			chkNegate2B.Checked = false;
			recTag1A.RecordKey = "";
			recTag1B.RecordKey = "";
			recTag2A.RecordKey = "";
			recTag2B.RecordKey = "";
			Save();
		}

		public string BuildValue()
		{
			bool inv1A = chkNegate1A.Checked;
			bool inv1B = chkNegate1B.Checked;
			bool inv2A = chkNegate2A.Checked;
			bool inv2B = chkNegate2B.Checked;
			string value1A = recTag1A.RecordKey;
			string value1B = recTag1B.RecordKey;
			string value2A = recTag2A.RecordKey;
			string value2B = recTag2B.RecordKey;
			string value = "";
			if (string.IsNullOrEmpty(value1A) && string.IsNullOrEmpty(value1B) && string.IsNullOrEmpty(value2A) && string.IsNullOrEmpty(value2B))
			{ 
				return null;
			}
			else
			{
				if (!string.IsNullOrEmpty(value1A))
				{
					if (inv1A)
					{
						value = "!";
					}
					value += value1A;

					if (!string.IsNullOrEmpty(value1B))
					{
						value += "&";
						if (inv1B)
						{
							value += "!";
						}
						value += value1B;
					}
				}
				else if (!string.IsNullOrEmpty(value1B))
				{
					if (inv1B)
					{
						value += "!";
					}
					value += value1B;
				}



				if (!string.IsNullOrEmpty(value2A) || !string.IsNullOrEmpty(value2B))
				{
					if (!string.IsNullOrEmpty(value))
					{
						value += "|";
					}

					if (!string.IsNullOrEmpty(value2A))
					{
						if (inv2A)
						{
							value += "!";
						}
						value += value2A;

						if (!string.IsNullOrEmpty(value2B))
						{
							value += "&";
							if (inv2B)
							{
								value += "!";
							}
							value += value2B;
						}
					}
					else if (!string.IsNullOrEmpty(value2B))
					{
						if (inv2B)
						{
							value += "!";
						}
						value += value2B;
					}
				}
			}



			return value;
		}

		protected override void OnSave()
		{
			string value = BuildValue();
			SetValue(value);
		}

		private void ChkNegate1A_CheckedChanged(object sender, EventArgs e)
		{
			Save();
		}

		private void ChkNegate1B_CheckedChanged(object sender, EventArgs e)
		{
			Save();
		}

		private void ChkNegate2A_CheckedChanged(object sender, EventArgs e)
		{
			Save();
		}

		private void ChkNegate2B_CheckedChanged(object sender, EventArgs e)
		{
			Save();
		}

		private void RecTag1A_RecordChanged(object sender, RecordEventArgs e)
		{
			Save();
		}

		private void RecTag1B_RecordChanged(object sender, RecordEventArgs e)
		{
			Save();
		}
		private void RecTag2A_RecordChanged(object sender, RecordEventArgs e)
		{
			Save();
		}
		private void RecTag2B_RecordChanged(object sender, RecordEventArgs e)
		{
			Save();
		}

  
	}

	public class TagAdvAttribute : EditControlAttribute
	{
		public override Type EditControlType
		{
			get { return typeof(TagAdvControl); }
		}
	}
}
