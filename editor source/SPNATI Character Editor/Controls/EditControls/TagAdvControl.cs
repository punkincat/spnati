using Desktop;
using Desktop.CommonControls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SPNATI_Character_Editor
{
	public partial class TagAdvControl : PropertyEditControl
	{

		bool _more;

		public TagAdvControl()
		{
			InitializeComponent();

			_more = false;
			txtTagAdv.Visible = false;
			icoTagAdv.Visible = false;

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

		private void ApplyValue(string value, bool? more = null)
		{
			value = value ?? "";
			//string pattern = @"^([^\&\|]*)(\&?)([^\&\|]*)(\|?)([^\&\|]*)(\&?)([^\&\|]*)";
			string pattern = @"^([^\&\|]*)(\&?)([^\&\|]*)(\&?)([^\|]*)(\|?)([^\&\|]*)(\&?)([^\&\|]*)([\&\|]?)";
			Regex reg = new Regex(pattern);
			Match regMatch = reg.Match(value);
			if (regMatch.Success && (more == false || (string.IsNullOrEmpty(regMatch.Groups[5].Value) && string.IsNullOrEmpty(regMatch.Groups[10].Value))))
			{
				if (more == false)
				{
					if (!string.IsNullOrEmpty(regMatch.Groups[5].Value) || !string.IsNullOrEmpty(regMatch.Groups[10].Value))
					{
						if (MessageBox.Show($"This logical expression is too long for the standard Tags (Advanced) editor. If you switch between editors, the expression will be truncated.\n Do you want to proceed?",
		$"Switch to the standard Tags (Advanced) editor", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
						{
							return;
						}
						else
						{
							_more = false;
						}
					}
				}
				string val1A = regMatch.Groups[1].Value;
				string val1B = regMatch.Groups[3].Value;
				string val2A = regMatch.Groups[7].Value;
				string val2B = regMatch.Groups[9].Value;
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
			else
			{
				ToggleVisibility(true);
				_more = true;
				txtTagAdv.Text = value;
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
			txtTagAdv.Text = "";
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
			string value = _more? txtTagAdv.Text : BuildValue();
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
		private void txtTagAdv_TextChanged(object sender, EventArgs e)
		{
			Save();
		}

		private void ToggleVisibility(bool more)
		{
			icoTagAdv.Visible = more;
			txtTagAdv.Visible = more;
			recTag2B.Visible = !more;
			recTag2A.Visible = !more;
			recTag1B.Visible = !more;
			recTag1A.Visible = !more;
			skinnedLabel2.Visible = !more;
			skinnedLabel1.Visible = !more;
			chkNegate2B.Visible = !more;
			skinnedLabelAND2A.Visible = !more;
			chkNegate2A.Visible = !more;
			skinnedLabelOR.Visible = !more;
			skinnedLabelAND1A.Visible = !more;
			chkNegate1B.Visible = !more;
			chkNegate1A.Visible = !more;
			if (more) 
			{
				icoTagAdv.Location = new System.Drawing.Point(25, 0);
				txtTagAdv.Location = new System.Drawing.Point(50, 0);
				txtTagAdv.Size = new System.Drawing.Size(300, 20);
				btnMore.Text = "-";
				toolTip1.SetToolTip(btnMore, "Standard condition");
			}
			else
			{
				btnMore.Text = "+";
				toolTip1.SetToolTip(btnMore, "Longer condition");
			}
		}

		private void btnMore_Click(object sender, EventArgs e)
		{
			if (_more)
			{
				ApplyValue(txtTagAdv.Text, false);
				if (!_more)
				{
					ToggleVisibility(false);
				}
			}
			else
			{
				_more = true;
				txtTagAdv.Text = BuildValue();
				ToggleVisibility(true);
			}
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
