namespace SPNATI_Character_Editor.Controls
{
	partial class CharacterSettingControl
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
            this.stripCharacterSettings = new Desktop.Skinning.SkinnedTabStrip();
            this.tabsCharacterSettings = new Desktop.Skinning.SkinnedTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.skinnedPanel1 = new Desktop.Skinning.SkinnedPanel();
            this.lblDescription = new Desktop.Skinning.SkinnedLabel();
            this.txtDescription = new Desktop.Skinning.SkinnedTextBox();
            this.txtValue = new Desktop.Skinning.SkinnedTextBox();
            this.chkDefault = new Desktop.Skinning.SkinnedCheckBox();
            this.lblValue = new Desktop.Skinning.SkinnedLabel();
            this.tableCharacterSetting = new Desktop.CommonControls.PropertyTable();
            this.tabsCharacterSettings.SuspendLayout();
            this.skinnedPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // stripCharacterSettings
            // 
            this.stripCharacterSettings.AddCaption = "Add";
            this.stripCharacterSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stripCharacterSettings.DecorationText = null;
            this.stripCharacterSettings.Location = new System.Drawing.Point(5, 3);
            this.stripCharacterSettings.Margin = new System.Windows.Forms.Padding(0);
            this.stripCharacterSettings.Name = "stripCharacterSettings";
            this.stripCharacterSettings.PanelType = Desktop.Skinning.SkinnedBackgroundType.Background;
            this.stripCharacterSettings.ShowAddButton = true;
            this.stripCharacterSettings.ShowCloseButton = false;
            this.stripCharacterSettings.Size = new System.Drawing.Size(556, 28);
            this.stripCharacterSettings.StartMargin = 5;
            this.stripCharacterSettings.TabControl = this.tabsCharacterSettings;
            this.stripCharacterSettings.TabIndex = 61;
            this.stripCharacterSettings.TabMargin = 5;
            this.stripCharacterSettings.TabPadding = 10;
            this.stripCharacterSettings.TabSize = -1;
            this.stripCharacterSettings.TabType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.stripCharacterSettings.Vertical = false;
            this.stripCharacterSettings.CloseButtonClicked += new System.EventHandler(this.stripCharacterSettings_CloseButtonClicked);
            this.stripCharacterSettings.AddButtonClicked += new System.EventHandler(this.stripCharacterSettings_AddButtonClicked);
            this.stripCharacterSettings.TabIndexChanged += new System.EventHandler(this.stripCharacterSettings_TabIndexChanged);
            // 
            // tabsCharacterSettings
            // 
            this.tabsCharacterSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsCharacterSettings.Controls.Add(this.tabPage1);
            this.tabsCharacterSettings.Location = new System.Drawing.Point(32739, 15);
            this.tabsCharacterSettings.Margin = new System.Windows.Forms.Padding(1);
            this.tabsCharacterSettings.Name = "tabsCharacterSettings";
            this.tabsCharacterSettings.SelectedIndex = 0;
            this.tabsCharacterSettings.Size = new System.Drawing.Size(0, 0);
            this.tabsCharacterSettings.TabIndex = 62;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.ForeColor = System.Drawing.Color.Black;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(0, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Setting 1";
            // 
            // skinnedPanel1
            // 
            this.skinnedPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinnedPanel1.Controls.Add(this.lblDescription);
            this.skinnedPanel1.Controls.Add(this.txtDescription);
            this.skinnedPanel1.Controls.Add(this.txtValue);
            this.skinnedPanel1.Controls.Add(this.chkDefault);
            this.skinnedPanel1.Controls.Add(this.lblValue);
            this.skinnedPanel1.Controls.Add(this.tabsCharacterSettings);
            this.skinnedPanel1.Controls.Add(this.tableCharacterSetting);
            this.skinnedPanel1.Location = new System.Drawing.Point(0, 0);
            this.skinnedPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.skinnedPanel1.Name = "skinnedPanel1";
            this.skinnedPanel1.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.skinnedPanel1.Size = new System.Drawing.Size(561, 298);
            this.skinnedPanel1.TabIndex = 32;
            this.skinnedPanel1.TabSide = Desktop.Skinning.TabSide.Top;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDescription.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblDescription.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblDescription.Location = new System.Drawing.Point(31, 69);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 77;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.ForeColor = System.Drawing.Color.Black;
            this.txtDescription.Location = new System.Drawing.Point(100, 66);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(169, 20);
            this.txtDescription.TabIndex = 76;
            // 
            // txtValue
            // 
            this.txtValue.BackColor = System.Drawing.Color.White;
            this.txtValue.ForeColor = System.Drawing.Color.Black;
            this.txtValue.Location = new System.Drawing.Point(100, 40);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(169, 20);
            this.txtValue.TabIndex = 75;
            // 
            // chkDefault
            // 
            this.chkDefault.AutoSize = true;
            this.chkDefault.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkDefault.Location = new System.Drawing.Point(329, 42);
            this.chkDefault.Name = "chkDefault";
            this.chkDefault.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkDefault.Size = new System.Drawing.Size(60, 17);
            this.chkDefault.TabIndex = 74;
            this.chkDefault.Text = "Default";
            this.chkDefault.UseVisualStyleBackColor = true;
            this.chkDefault.CheckedChanged += new System.EventHandler(this.chkDefault_CheckedChanged);
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblValue.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblValue.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblValue.Location = new System.Drawing.Point(22, 43);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(72, 13);
            this.lblValue.TabIndex = 64;
            this.lblValue.Text = "Marker value:";
            // 
            // tableCharacterSetting
            // 
            this.tableCharacterSetting.AllowDelete = true;
            this.tableCharacterSetting.AllowFavorites = true;
            this.tableCharacterSetting.AllowHelp = false;
            this.tableCharacterSetting.AllowMacros = false;
            this.tableCharacterSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableCharacterSetting.BackColor = System.Drawing.Color.White;
            this.tableCharacterSetting.Data = null;
            this.tableCharacterSetting.HeaderType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.tableCharacterSetting.HideAddField = true;
            this.tableCharacterSetting.HideSpeedButtons = false;
            this.tableCharacterSetting.Location = new System.Drawing.Point(5, 103);
            this.tableCharacterSetting.Margin = new System.Windows.Forms.Padding(1);
            this.tableCharacterSetting.ModifyingProperty = null;
            this.tableCharacterSetting.Name = "tableCharacterSetting";
            this.tableCharacterSetting.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.tableCharacterSetting.PlaceholderText = "Add";
            this.tableCharacterSetting.PreserveControls = false;
            this.tableCharacterSetting.PreviewData = null;
            this.tableCharacterSetting.RemoveCaption = "Remove";
            this.tableCharacterSetting.RowHeaderWidth = 0F;
            this.tableCharacterSetting.RunInitialAddEvents = true;
            this.tableCharacterSetting.Size = new System.Drawing.Size(555, 194);
            this.tableCharacterSetting.Sorted = false;
            this.tableCharacterSetting.TabIndex = 31;
            this.tableCharacterSetting.UndoManager = null;
            this.tableCharacterSetting.UseAutoComplete = true;
            // 
            // CharacterSettingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stripCharacterSettings);
            this.Controls.Add(this.skinnedPanel1);
            this.Name = "CharacterSettingControl";
            this.Size = new System.Drawing.Size(561, 298);
            this.tabsCharacterSettings.ResumeLayout(false);
            this.skinnedPanel1.ResumeLayout(false);
            this.skinnedPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

		#endregion
        private Desktop.Skinning.SkinnedTabStrip stripCharacterSettings;
        private Desktop.Skinning.SkinnedTabControl tabsCharacterSettings;
        private System.Windows.Forms.TabPage tabPage1;
        private Desktop.Skinning.SkinnedPanel skinnedPanel1;
        private Desktop.CommonControls.PropertyTable tableCharacterSetting;
        private Desktop.Skinning.SkinnedLabel lblValue;
        private Desktop.Skinning.SkinnedCheckBox chkDefault;
        private Desktop.Skinning.SkinnedTextBox txtValue;
        private Desktop.Skinning.SkinnedLabel lblDescription;
        private Desktop.Skinning.SkinnedTextBox txtDescription;
    }
}