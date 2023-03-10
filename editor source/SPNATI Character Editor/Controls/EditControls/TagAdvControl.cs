using Desktop;
using Desktop.CommonControls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms.VisualStyles;

namespace SPNATI_Character_Editor
{
    public partial class TagAdvControl : PropertyEditControl
    {
        private Desktop.Skinning.SkinnedCheckBox chkNegate1A;
        private Desktop.Skinning.SkinnedCheckBox chkNegate1B;
        private Desktop.Skinning.SkinnedLabel skinnedLabelOR1A;
        private Desktop.Skinning.SkinnedLabel skinnedLabelAND;
        private Desktop.Skinning.SkinnedCheckBox chkNegate2A;
        private Desktop.Skinning.SkinnedLabel skinnedLabelOR2A;
        private Desktop.Skinning.SkinnedCheckBox chkNegate2B;
        private Desktop.Skinning.SkinnedLabel skinnedLabel1;
        private Desktop.Skinning.SkinnedLabel skinnedLabel2;
        private RecordField recTag1A;
        private RecordField recTag1B;
        private RecordField recTag2A;
        private RecordField recTag2B;

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
            string pattern = @"^([^∨\|]*)(∨?)([^∨\|]*)(\|?)([^∨\|]*)(∨?)([^∨\|]*)";
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
                        value += "∨";
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
                            value += "∨";
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

        private void InitializeComponent()
        {
            this.skinnedLabel1 = new Desktop.Skinning.SkinnedLabel();
            this.chkNegate1A = new Desktop.Skinning.SkinnedCheckBox();
            this.skinnedLabelOR1A = new Desktop.Skinning.SkinnedLabel();
            this.chkNegate1B = new Desktop.Skinning.SkinnedCheckBox();
            this.skinnedLabelAND = new Desktop.Skinning.SkinnedLabel();
            this.chkNegate2A = new Desktop.Skinning.SkinnedCheckBox();
            this.skinnedLabelOR2A = new Desktop.Skinning.SkinnedLabel();
            this.chkNegate2B = new Desktop.Skinning.SkinnedCheckBox();
            this.skinnedLabel2 = new Desktop.Skinning.SkinnedLabel();
            this.recTag1A = new Desktop.CommonControls.RecordField();
            this.recTag1B = new Desktop.CommonControls.RecordField();
            this.recTag2A = new Desktop.CommonControls.RecordField();
            this.recTag2B = new Desktop.CommonControls.RecordField();
            this.SuspendLayout();
            // 
            // skinnedLabel1
            // 
            this.skinnedLabel1.AutoSize = true;
            this.skinnedLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.skinnedLabel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.skinnedLabel1.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.skinnedLabel1.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.skinnedLabel1.Location = new System.Drawing.Point(0, 3);
            this.skinnedLabel1.Name = "skinnedLabel1";
            this.skinnedLabel1.Size = new System.Drawing.Size(10, 13);
            this.skinnedLabel1.TabIndex = 115;
            this.skinnedLabel1.Text = "(";
            // 
            // chkNegate1A
            // 
            this.chkNegate1A.AutoSize = true;
            this.chkNegate1A.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkNegate1A.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.chkNegate1A.Location = new System.Drawing.Point(11, 3);
            this.chkNegate1A.Name = "chkNegate1A";
            this.chkNegate1A.Size = new System.Drawing.Size(37, 16);
            this.chkNegate1A.TabIndex = 100;
            this.chkNegate1A.Text = "not";
            this.chkNegate1A.UseVisualStyleBackColor = true;
            // 
            // skinnedLabelOR1A
            // 
            this.skinnedLabelOR1A.AutoSize = true;
            this.skinnedLabelOR1A.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.skinnedLabelOR1A.ForeColor = System.Drawing.SystemColors.ControlText;
            this.skinnedLabelOR1A.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.skinnedLabelOR1A.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.skinnedLabelOR1A.Location = new System.Drawing.Point(162, 3);
            this.skinnedLabelOR1A.Name = "skinnedLabelOR1A";
            this.skinnedLabelOR1A.Size = new System.Drawing.Size(23, 13);
            this.skinnedLabelOR1A.TabIndex = 102;
            this.skinnedLabelOR1A.Text = "OR";
            // 
            // chkNegate1B
            // 
            this.chkNegate1B.AutoSize = true;
            this.chkNegate1B.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkNegate1B.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.chkNegate1B.Location = new System.Drawing.Point(185, 3);
            this.chkNegate1B.Name = "chkNegate1B";
            this.chkNegate1B.Size = new System.Drawing.Size(37, 16);
            this.chkNegate1B.TabIndex = 103;
            this.chkNegate1B.Text = "not";
            this.chkNegate1B.UseVisualStyleBackColor = true;
            // 
            // skinnedLabelAND
            // 
            this.skinnedLabelAND.AutoSize = true;
            this.skinnedLabelAND.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.skinnedLabelAND.ForeColor = System.Drawing.SystemColors.ControlText;
            this.skinnedLabelAND.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.skinnedLabelAND.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.skinnedLabelAND.Location = new System.Drawing.Point(336, 3);
            this.skinnedLabelAND.Name = "skinnedLabelAND";
            this.skinnedLabelAND.Size = new System.Drawing.Size(42, 13);
            this.skinnedLabelAND.TabIndex = 105;
            this.skinnedLabelAND.Text = ") AND (";
            // 
            // chkNegate2A
            // 
            this.chkNegate2A.AutoSize = true;
            this.chkNegate2A.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkNegate2A.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.chkNegate2A.Location = new System.Drawing.Point(377, 3);
            this.chkNegate2A.Name = "chkNegate2A";
            this.chkNegate2A.Size = new System.Drawing.Size(37, 16);
            this.chkNegate2A.TabIndex = 106;
            this.chkNegate2A.Text = "not";
            this.chkNegate2A.UseVisualStyleBackColor = true;
            // 
            // skinnedLabelOR2A
            // 
            this.skinnedLabelOR2A.AutoSize = true;
            this.skinnedLabelOR2A.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.skinnedLabelOR2A.ForeColor = System.Drawing.SystemColors.ControlText;
            this.skinnedLabelOR2A.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.skinnedLabelOR2A.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.skinnedLabelOR2A.Location = new System.Drawing.Point(528, 3);
            this.skinnedLabelOR2A.Name = "skinnedLabelOR2A";
            this.skinnedLabelOR2A.Size = new System.Drawing.Size(23, 13);
            this.skinnedLabelOR2A.TabIndex = 108;
            this.skinnedLabelOR2A.Text = "OR";
            // 
            // chkNegate2B
            // 
            this.chkNegate2B.AutoSize = true;
            this.chkNegate2B.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkNegate2B.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.25F);
            this.chkNegate2B.Location = new System.Drawing.Point(551, 3);
            this.chkNegate2B.Name = "chkNegate2B";
            this.chkNegate2B.Size = new System.Drawing.Size(37, 16);
            this.chkNegate2B.TabIndex = 109;
            this.chkNegate2B.Text = "not";
            this.chkNegate2B.UseVisualStyleBackColor = true;
            // 
            // skinnedLabel2
            // 
            this.skinnedLabel2.AutoSize = true;
            this.skinnedLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.skinnedLabel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.skinnedLabel2.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.skinnedLabel2.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.skinnedLabel2.Location = new System.Drawing.Point(703, 3);
            this.skinnedLabel2.Name = "skinnedLabel2";
            this.skinnedLabel2.Size = new System.Drawing.Size(10, 13);
            this.skinnedLabel2.TabIndex = 111;
            this.skinnedLabel2.Text = ")";
            // 
            // recTag1A
            // 
            this.recTag1A.AllowCreate = false;
            this.recTag1A.Location = new System.Drawing.Point(42, 0);
            this.recTag1A.Name = "recTag1A";
            this.recTag1A.PlaceholderText = null;
            this.recTag1A.Record = null;
            this.recTag1A.RecordContext = null;
            this.recTag1A.RecordFilter = null;
            this.recTag1A.RecordKey = null;
            this.recTag1A.RecordType = null;
            this.recTag1A.Size = new System.Drawing.Size(120, 20);
            this.recTag1A.TabIndex = 116;
            this.recTag1A.UseAutoComplete = false;
            // 
            // recTag1B
            // 
            this.recTag1B.AllowCreate = false;
            this.recTag1B.Location = new System.Drawing.Point(216, 0);
            this.recTag1B.Name = "recTag1B";
            this.recTag1B.PlaceholderText = null;
            this.recTag1B.Record = null;
            this.recTag1B.RecordContext = null;
            this.recTag1B.RecordFilter = null;
            this.recTag1B.RecordKey = null;
            this.recTag1B.RecordType = null;
            this.recTag1B.Size = new System.Drawing.Size(120, 20);
            this.recTag1B.TabIndex = 117;
            this.recTag1B.UseAutoComplete = false;
            // 
            // recTag2A
            // 
            this.recTag2A.AllowCreate = false;
            this.recTag2A.Location = new System.Drawing.Point(408, 0);
            this.recTag2A.Name = "recTag2A";
            this.recTag2A.PlaceholderText = null;
            this.recTag2A.Record = null;
            this.recTag2A.RecordContext = null;
            this.recTag2A.RecordFilter = null;
            this.recTag2A.RecordKey = null;
            this.recTag2A.RecordType = null;
            this.recTag2A.Size = new System.Drawing.Size(120, 20);
            this.recTag2A.TabIndex = 118;
            this.recTag2A.UseAutoComplete = false;
            // 
            // recTag2B
            // 
            this.recTag2B.AllowCreate = false;
            this.recTag2B.Location = new System.Drawing.Point(582, 0);
            this.recTag2B.Name = "recTag2B";
            this.recTag2B.PlaceholderText = null;
            this.recTag2B.Record = null;
            this.recTag2B.RecordContext = null;
            this.recTag2B.RecordFilter = null;
            this.recTag2B.RecordKey = null;
            this.recTag2B.RecordType = null;
            this.recTag2B.Size = new System.Drawing.Size(120, 20);
            this.recTag2B.TabIndex = 119;
            this.recTag2B.UseAutoComplete = false;
            // 
            // TagAdvControl
            // 
            this.Controls.Add(this.recTag2B);
            this.Controls.Add(this.recTag2A);
            this.Controls.Add(this.recTag1B);
            this.Controls.Add(this.recTag1A);
            this.Controls.Add(this.skinnedLabel2);
            this.Controls.Add(this.skinnedLabel1);
            this.Controls.Add(this.chkNegate2B);
            this.Controls.Add(this.skinnedLabelOR2A);
            this.Controls.Add(this.chkNegate2A);
            this.Controls.Add(this.skinnedLabelAND);
            this.Controls.Add(this.skinnedLabelOR1A);
            this.Controls.Add(this.chkNegate1B);
            this.Controls.Add(this.chkNegate1A);
            this.Name = "TagAdvControl";
            this.Size = new System.Drawing.Size(714, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

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
