namespace SPNATI_Character_Editor.Forms
{
    partial class WardrobeAdvancedForm
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new Desktop.Skinning.SkinnedGroupBox();
            this.cboFromStage = new Desktop.Skinning.SkinnedComboBox();
            this.lblFromStage = new Desktop.Skinning.SkinnedLabel();
            this.lblFromDeal = new Desktop.Skinning.SkinnedLabel();
            this.cboFromDeal = new Desktop.Skinning.SkinnedComboBox();
            this.chkNotFromStart = new Desktop.Skinning.SkinnedCheckBox();
            this.skinnedGroupBox2 = new Desktop.Skinning.SkinnedGroupBox();
            this.recType = new Desktop.CommonControls.RecordField();
            this.recPosition = new Desktop.CommonControls.RecordField();
            this.recGeneric = new Desktop.CommonControls.RecordField();
            this.cboItem = new Desktop.Skinning.SkinnedComboBox();
            this.chkPlural = new Desktop.Skinning.SkinnedCheckBox();
            this.lblType = new Desktop.Skinning.SkinnedLabel();
            this.lblPosition = new Desktop.Skinning.SkinnedLabel();
            this.lblPlural = new Desktop.Skinning.SkinnedLabel();
            this.lblGeneric = new Desktop.Skinning.SkinnedLabel();
            this.chkDefine = new Desktop.Skinning.SkinnedCheckBox();
            this.chkSelect = new Desktop.Skinning.SkinnedCheckBox();
            this.chkDifferentItem = new Desktop.Skinning.SkinnedCheckBox();
            this.txtName = new Desktop.Skinning.SkinnedTextBox();
            this.lblName = new Desktop.Skinning.SkinnedLabel();
            this.chkReveal = new Desktop.Skinning.SkinnedCheckBox();
            this.cboReveal = new Desktop.Skinning.SkinnedComboBox();
            this.cmdOK = new Desktop.Skinning.SkinnedButton();
            this.cmdCancel = new Desktop.Skinning.SkinnedButton();
            this.groupBox2.SuspendLayout();
            this.skinnedGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.cboFromStage);
            this.groupBox2.Controls.Add(this.lblFromStage);
            this.groupBox2.Controls.Add(this.lblFromDeal);
            this.groupBox2.Controls.Add(this.cboFromDeal);
            this.groupBox2.Controls.Add(this.chkNotFromStart);
            this.groupBox2.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.groupBox2.Image = null;
            this.groupBox2.Location = new System.Drawing.Point(3, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.groupBox2.ShowIndicatorBar = false;
            this.groupBox2.Size = new System.Drawing.Size(583, 82);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Changing clothes mid-game";
            // 
            // cboFromStage
            // 
            this.cboFromStage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboFromStage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboFromStage.BackColor = System.Drawing.Color.White;
            this.cboFromStage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboFromStage.Enabled = false;
            this.cboFromStage.FieldType = Desktop.Skinning.SkinnedFieldType.Secondary;
            this.cboFromStage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboFromStage.KeyMember = null;
            this.cboFromStage.Location = new System.Drawing.Point(337, 54);
            this.cboFromStage.Name = "cboFromStage";
            this.cboFromStage.SelectedIndex = -1;
            this.cboFromStage.SelectedItem = null;
            this.cboFromStage.Size = new System.Drawing.Size(195, 23);
            this.cboFromStage.Sorted = false;
            this.cboFromStage.TabIndex = 8;
            this.cboFromStage.SelectedIndexChanged += new System.EventHandler(this.cboFromStage_SelectedIndexChanged);
            // 
            // lblFromStage
            // 
            this.lblFromStage.AutoSize = true;
            this.lblFromStage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblFromStage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFromStage.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblFromStage.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblFromStage.Location = new System.Drawing.Point(254, 59);
            this.lblFromStage.Name = "lblFromStage";
            this.lblFromStage.Size = new System.Drawing.Size(77, 13);
            this.lblFromStage.TabIndex = 7;
            this.lblFromStage.Text = "phase of stage";
            // 
            // lblFromDeal
            // 
            this.lblFromDeal.AutoSize = true;
            this.lblFromDeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblFromDeal.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFromDeal.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblFromDeal.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblFromDeal.Location = new System.Drawing.Point(28, 59);
            this.lblFromDeal.Name = "lblFromDeal";
            this.lblFromDeal.Size = new System.Drawing.Size(132, 13);
            this.lblFromDeal.TabIndex = 6;
            this.lblFromDeal.Text = "This item appears after the";
            // 
            // cboFromDeal
            // 
            this.cboFromDeal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboFromDeal.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboFromDeal.BackColor = System.Drawing.Color.White;
            this.cboFromDeal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboFromDeal.Enabled = false;
            this.cboFromDeal.FieldType = Desktop.Skinning.SkinnedFieldType.Secondary;
            this.cboFromDeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboFromDeal.KeyMember = null;
            this.cboFromDeal.Location = new System.Drawing.Point(166, 54);
            this.cboFromDeal.Name = "cboFromDeal";
            this.cboFromDeal.SelectedIndex = -1;
            this.cboFromDeal.SelectedItem = null;
            this.cboFromDeal.Size = new System.Drawing.Size(75, 23);
            this.cboFromDeal.Sorted = false;
            this.cboFromDeal.TabIndex = 5;
            this.cboFromDeal.SelectedIndexChanged += new System.EventHandler(this.cboFromDeal_SelectedIndexChanged);
            // 
            // chkNotFromStart
            // 
            this.chkNotFromStart.AutoSize = true;
            this.chkNotFromStart.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkNotFromStart.Location = new System.Drawing.Point(9, 31);
            this.chkNotFromStart.Name = "chkNotFromStart";
            this.chkNotFromStart.Size = new System.Drawing.Size(192, 17);
            this.chkNotFromStart.TabIndex = 4;
            this.chkNotFromStart.Text = "Not worn from the start of the game";
            this.chkNotFromStart.UseVisualStyleBackColor = true;
            this.chkNotFromStart.CheckedChanged += new System.EventHandler(this.chkNotFromStart_CheckedChanged);
            // 
            // skinnedGroupBox2
            // 
            this.skinnedGroupBox2.BackColor = System.Drawing.Color.White;
            this.skinnedGroupBox2.Controls.Add(this.recType);
            this.skinnedGroupBox2.Controls.Add(this.recPosition);
            this.skinnedGroupBox2.Controls.Add(this.recGeneric);
            this.skinnedGroupBox2.Controls.Add(this.cboItem);
            this.skinnedGroupBox2.Controls.Add(this.chkPlural);
            this.skinnedGroupBox2.Controls.Add(this.lblType);
            this.skinnedGroupBox2.Controls.Add(this.lblPosition);
            this.skinnedGroupBox2.Controls.Add(this.lblPlural);
            this.skinnedGroupBox2.Controls.Add(this.lblGeneric);
            this.skinnedGroupBox2.Controls.Add(this.chkDefine);
            this.skinnedGroupBox2.Controls.Add(this.chkSelect);
            this.skinnedGroupBox2.Controls.Add(this.chkDifferentItem);
            this.skinnedGroupBox2.Controls.Add(this.txtName);
            this.skinnedGroupBox2.Controls.Add(this.lblName);
            this.skinnedGroupBox2.Controls.Add(this.chkReveal);
            this.skinnedGroupBox2.Controls.Add(this.cboReveal);
            this.skinnedGroupBox2.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.skinnedGroupBox2.Image = null;
            this.skinnedGroupBox2.Location = new System.Drawing.Point(3, 120);
            this.skinnedGroupBox2.Name = "skinnedGroupBox2";
            this.skinnedGroupBox2.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.skinnedGroupBox2.ShowIndicatorBar = false;
            this.skinnedGroupBox2.Size = new System.Drawing.Size(583, 169);
            this.skinnedGroupBox2.TabIndex = 13;
            this.skinnedGroupBox2.TabStop = false;
            this.skinnedGroupBox2.Text = "Different handling of the Stripping phase";
            // 
            // recType
            // 
            this.recType.AllowCreate = false;
            this.recType.Enabled = false;
            this.recType.Location = new System.Drawing.Point(360, 141);
            this.recType.Name = "recType";
            this.recType.PlaceholderText = null;
            this.recType.Record = null;
            this.recType.RecordContext = null;
            this.recType.RecordFilter = null;
            this.recType.RecordKey = null;
            this.recType.RecordType = null;
            this.recType.Size = new System.Drawing.Size(100, 20);
            this.recType.TabIndex = 34;
            this.recType.UseAutoComplete = false;
            this.recType.RecordChanged += new System.EventHandler<Desktop.CommonControls.RecordEventArgs>(this.recType_RecordChanged);
            // 
            // recPosition
            // 
            this.recPosition.AllowCreate = false;
            this.recPosition.Enabled = false;
            this.recPosition.Location = new System.Drawing.Point(466, 141);
            this.recPosition.Name = "recPosition";
            this.recPosition.PlaceholderText = null;
            this.recPosition.Record = null;
            this.recPosition.RecordContext = null;
            this.recPosition.RecordFilter = null;
            this.recPosition.RecordKey = null;
            this.recPosition.RecordType = null;
            this.recPosition.Size = new System.Drawing.Size(100, 20);
            this.recPosition.TabIndex = 33;
            this.recPosition.UseAutoComplete = false;
            this.recPosition.RecordChanged += new System.EventHandler<Desktop.CommonControls.RecordEventArgs>(this.recPosition_RecordChanged);
            // 
            // recGeneric
            // 
            this.recGeneric.AllowCreate = false;
            this.recGeneric.Enabled = false;
            this.recGeneric.Location = new System.Drawing.Point(203, 141);
            this.recGeneric.Name = "recGeneric";
            this.recGeneric.PlaceholderText = null;
            this.recGeneric.Record = null;
            this.recGeneric.RecordContext = null;
            this.recGeneric.RecordFilter = null;
            this.recGeneric.RecordKey = null;
            this.recGeneric.RecordType = null;
            this.recGeneric.Size = new System.Drawing.Size(100, 20);
            this.recGeneric.TabIndex = 32;
            this.recGeneric.UseAutoComplete = false;
            this.recGeneric.RecordChanged += new System.EventHandler<Desktop.CommonControls.RecordEventArgs>(this.recGeneric_RecordChanged);
            // 
            // cboItem
            // 
            this.cboItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboItem.BackColor = System.Drawing.Color.White;
            this.cboItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboItem.Enabled = false;
            this.cboItem.FieldType = Desktop.Skinning.SkinnedFieldType.Secondary;
            this.cboItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboItem.KeyMember = null;
            this.cboItem.Location = new System.Drawing.Point(171, 79);
            this.cboItem.Name = "cboItem";
            this.cboItem.SelectedIndex = -1;
            this.cboItem.SelectedItem = null;
            this.cboItem.Size = new System.Drawing.Size(185, 23);
            this.cboItem.Sorted = false;
            this.cboItem.TabIndex = 31;
            this.cboItem.SelectedIndexChanged += new System.EventHandler(this.cboItem_SelectedIndexChanged);
            // 
            // chkPlural
            // 
            this.chkPlural.AutoSize = true;
            this.chkPlural.Enabled = false;
            this.chkPlural.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkPlural.Location = new System.Drawing.Point(321, 144);
            this.chkPlural.Name = "chkPlural";
            this.chkPlural.Size = new System.Drawing.Size(15, 14);
            this.chkPlural.TabIndex = 27;
            this.chkPlural.UseVisualStyleBackColor = true;
            this.chkPlural.CheckedChanged += new System.EventHandler(this.chkPlural_CheckedChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblType.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblType.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblType.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblType.Location = new System.Drawing.Point(371, 125);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 13);
            this.lblType.TabIndex = 26;
            this.lblType.Text = "Type:";
            // 
            // lblPosition
            // 
            this.lblPosition.AutoSize = true;
            this.lblPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblPosition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPosition.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblPosition.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblPosition.Location = new System.Drawing.Point(476, 125);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(47, 13);
            this.lblPosition.TabIndex = 25;
            this.lblPosition.Text = "Position:";
            // 
            // lblPlural
            // 
            this.lblPlural.AutoSize = true;
            this.lblPlural.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblPlural.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPlural.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblPlural.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblPlural.Location = new System.Drawing.Point(306, 125);
            this.lblPlural.Name = "lblPlural";
            this.lblPlural.Size = new System.Drawing.Size(50, 13);
            this.lblPlural.TabIndex = 24;
            this.lblPlural.Text = "Is Plural?";
            // 
            // lblGeneric
            // 
            this.lblGeneric.AutoSize = true;
            this.lblGeneric.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblGeneric.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblGeneric.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblGeneric.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblGeneric.Location = new System.Drawing.Point(217, 125);
            this.lblGeneric.Name = "lblGeneric";
            this.lblGeneric.Size = new System.Drawing.Size(71, 13);
            this.lblGeneric.TabIndex = 23;
            this.lblGeneric.Text = "Classification:";
            // 
            // chkDefine
            // 
            this.chkDefine.AutoSize = true;
            this.chkDefine.Enabled = false;
            this.chkDefine.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkDefine.Location = new System.Drawing.Point(31, 104);
            this.chkDefine.Name = "chkDefine";
            this.chkDefine.Size = new System.Drawing.Size(111, 17);
            this.chkDefine.TabIndex = 22;
            this.chkDefine.Text = "Define a new item";
            this.chkDefine.UseVisualStyleBackColor = true;
            this.chkDefine.CheckedChanged += new System.EventHandler(this.chkDefine_CheckedChanged);
            // 
            // chkSelect
            // 
            this.chkSelect.AutoSize = true;
            this.chkSelect.Enabled = false;
            this.chkSelect.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkSelect.Location = new System.Drawing.Point(31, 80);
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.Size = new System.Drawing.Size(134, 17);
            this.chkSelect.TabIndex = 21;
            this.chkSelect.Text = "Select an existing item:";
            this.chkSelect.UseVisualStyleBackColor = true;
            this.chkSelect.CheckedChanged += new System.EventHandler(this.chkSelect_CheckedChanged);
            // 
            // chkDifferentItem
            // 
            this.chkDifferentItem.AutoSize = true;
            this.chkDifferentItem.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkDifferentItem.Location = new System.Drawing.Point(9, 56);
            this.chkDifferentItem.Name = "chkDifferentItem";
            this.chkDifferentItem.Size = new System.Drawing.Size(279, 17);
            this.chkDifferentItem.TabIndex = 20;
            this.chkDifferentItem.Text = "The character appears to be removing a different item";
            this.chkDifferentItem.UseVisualStyleBackColor = true;
            this.chkDifferentItem.CheckedChanged += new System.EventHandler(this.chkDifferentItem_CheckedChanged);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Enabled = false;
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.Location = new System.Drawing.Point(31, 141);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(166, 20);
            this.txtName.TabIndex = 19;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblName.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblName.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblName.Location = new System.Drawing.Point(46, 126);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 16;
            this.lblName.Text = "Name:";
            // 
            // chkReveal
            // 
            this.chkReveal.AutoSize = true;
            this.chkReveal.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkReveal.Location = new System.Drawing.Point(9, 33);
            this.chkReveal.Name = "chkReveal";
            this.chkReveal.Size = new System.Drawing.Size(306, 17);
            this.chkReveal.TabIndex = 15;
            this.chkReveal.Text = "Other characters should react as though it were a reveal of:";
            this.chkReveal.UseVisualStyleBackColor = true;
            this.chkReveal.CheckedChanged += new System.EventHandler(this.chkReveal_CheckedChanged);
            // 
            // cboReveal
            // 
            this.cboReveal.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboReveal.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboReveal.BackColor = System.Drawing.Color.White;
            this.cboReveal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReveal.Enabled = false;
            this.cboReveal.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboReveal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboReveal.FormattingEnabled = true;
            this.cboReveal.KeyMember = null;
            this.cboReveal.Location = new System.Drawing.Point(321, 30);
            this.cboReveal.Name = "cboReveal";
            this.cboReveal.SelectedIndex = -1;
            this.cboReveal.SelectedItem = null;
            this.cboReveal.Size = new System.Drawing.Size(75, 21);
            this.cboReveal.Sorted = false;
            this.cboReveal.TabIndex = 14;
            this.cboReveal.SelectedIndexChanged += new System.EventHandler(this.cboReveal_SelectedIndexChanged);
            // 
            // cmdOK
            // 
            this.cmdOK.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.FieldType = Desktop.Skinning.SkinnedFieldType.Secondary;
            this.cmdOK.Flat = false;
            this.cmdOK.Location = new System.Drawing.Point(430, 295);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 14;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.cmdCancel.Flat = false;
            this.cmdCancel.Location = new System.Drawing.Point(511, 295);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 15;
            this.cmdCancel.Text = "CANCEL";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // WardrobeAdvancedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 324);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.skinnedGroupBox2);
            this.Controls.Add(this.groupBox2);
            this.Name = "WardrobeAdvancedForm";
            this.Text = "Advanced Clothing Options";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.skinnedGroupBox2.ResumeLayout(false);
            this.skinnedGroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Desktop.Skinning.SkinnedGroupBox groupBox2;
        private Desktop.Skinning.SkinnedCheckBox chkNotFromStart;
        private Desktop.Skinning.SkinnedGroupBox skinnedGroupBox2;
        private Desktop.Skinning.SkinnedComboBox cboReveal;
        private Desktop.Skinning.SkinnedLabel lblName;
        private Desktop.Skinning.SkinnedCheckBox chkReveal;
        private Desktop.Skinning.SkinnedTextBox txtName;
        private Desktop.Skinning.SkinnedButton cmdOK;
        private Desktop.Skinning.SkinnedButton cmdCancel;
        private Desktop.Skinning.SkinnedComboBox cboFromDeal;
        private Desktop.Skinning.SkinnedLabel lblFromDeal;
        private Desktop.Skinning.SkinnedLabel lblFromStage;
        private Desktop.Skinning.SkinnedComboBox cboFromStage;
        private Desktop.Skinning.SkinnedCheckBox chkDifferentItem;
        private Desktop.Skinning.SkinnedCheckBox chkDefine;
        private Desktop.Skinning.SkinnedCheckBox chkSelect;
        private Desktop.Skinning.SkinnedComboBox cboItem;
        private Desktop.Skinning.SkinnedCheckBox chkPlural;
        private Desktop.Skinning.SkinnedLabel lblType;
        private Desktop.Skinning.SkinnedLabel lblPosition;
        private Desktop.Skinning.SkinnedLabel lblPlural;
        private Desktop.Skinning.SkinnedLabel lblGeneric;
        private Desktop.CommonControls.RecordField recType;
        private Desktop.CommonControls.RecordField recPosition;
        private Desktop.CommonControls.RecordField recGeneric;
    }
}
