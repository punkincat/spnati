namespace SPNATI_Character_Editor.Controls
{
	partial class PoseSetControl
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
            this.components = new System.ComponentModel.Container();
            this.stripPoseSetEntries = new Desktop.Skinning.SkinnedTabStrip();
            this.tabsPoseSetEntries = new Desktop.Skinning.SkinnedTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.skinnedPanel1 = new Desktop.Skinning.SkinnedPanel();
            this.lblStage = new Desktop.Skinning.SkinnedLabel();
            this.chkLayer = new Desktop.Skinning.SkinnedCheckBox();
            this.lblDirection = new Desktop.Skinning.SkinnedLabel();
            this.cboDirection = new Desktop.Skinning.SkinnedComboBox();
            this.lblWeight = new Desktop.Skinning.SkinnedLabel();
            this.lblPriority = new Desktop.Skinning.SkinnedLabel();
            this.valWeight = new Desktop.Skinning.SkinnedNumericUpDown();
            this.valPriority = new Desktop.Skinning.SkinnedNumericUpDown();
            this.lblLocation = new Desktop.Skinning.SkinnedLabel();
            this.valLocation = new Desktop.Skinning.SkinnedNumericUpDown();
            this.lblBubble = new Desktop.Skinning.SkinnedLabel();
            this.lblPoseSelect = new Desktop.Skinning.SkinnedLabel();
            this.cboPose = new System.Windows.Forms.ComboBox();
            this.tablePoseSetEntry = new Desktop.CommonControls.PropertyTable();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabsPoseSetEntries.SuspendLayout();
            this.skinnedPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valWeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valPriority)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLocation)).BeginInit();
            this.SuspendLayout();
            // 
            // stripPoseSetEntries
            // 
            this.stripPoseSetEntries.AddCaption = "Add";
            this.stripPoseSetEntries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stripPoseSetEntries.DecorationText = null;
            this.stripPoseSetEntries.Location = new System.Drawing.Point(5, 3);
            this.stripPoseSetEntries.Margin = new System.Windows.Forms.Padding(0);
            this.stripPoseSetEntries.Name = "stripPoseSetEntries";
            this.stripPoseSetEntries.PanelType = Desktop.Skinning.SkinnedBackgroundType.Background;
            this.stripPoseSetEntries.ShowAddButton = true;
            this.stripPoseSetEntries.ShowCloseButton = false;
            this.stripPoseSetEntries.Size = new System.Drawing.Size(665, 28);
            this.stripPoseSetEntries.StartMargin = 5;
            this.stripPoseSetEntries.TabControl = this.tabsPoseSetEntries;
            this.stripPoseSetEntries.TabIndex = 61;
            this.stripPoseSetEntries.TabMargin = 5;
            this.stripPoseSetEntries.TabPadding = 10;
            this.stripPoseSetEntries.TabSize = -1;
            this.stripPoseSetEntries.TabType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.stripPoseSetEntries.Vertical = false;
            this.stripPoseSetEntries.CloseButtonClicked += new System.EventHandler(this.stripPoseSetEntries_CloseButtonClicked);
            this.stripPoseSetEntries.AddButtonClicked += new System.EventHandler(this.stripPoseSetEntries_AddButtonClicked);
            this.stripPoseSetEntries.TabIndexChanged += new System.EventHandler(this.stripPoseSetEntries_TabIndexChanged);
            // 
            // tabsPoseSetEntries
            // 
            this.tabsPoseSetEntries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabsPoseSetEntries.Controls.Add(this.tabPage1);
            this.tabsPoseSetEntries.Location = new System.Drawing.Point(32739, 15);
            this.tabsPoseSetEntries.Margin = new System.Windows.Forms.Padding(1);
            this.tabsPoseSetEntries.Name = "tabsPoseSetEntries";
            this.tabsPoseSetEntries.SelectedIndex = 0;
            this.tabsPoseSetEntries.Size = new System.Drawing.Size(50, 50);
            this.tabsPoseSetEntries.TabIndex = 62;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.ForeColor = System.Drawing.Color.Black;
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(42, 24);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Pose 1";
            // 
            // skinnedPanel1
            // 
            this.skinnedPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinnedPanel1.Controls.Add(this.lblStage);
            this.skinnedPanel1.Controls.Add(this.chkLayer);
            this.skinnedPanel1.Controls.Add(this.lblDirection);
            this.skinnedPanel1.Controls.Add(this.cboDirection);
            this.skinnedPanel1.Controls.Add(this.lblWeight);
            this.skinnedPanel1.Controls.Add(this.lblPriority);
            this.skinnedPanel1.Controls.Add(this.valWeight);
            this.skinnedPanel1.Controls.Add(this.valPriority);
            this.skinnedPanel1.Controls.Add(this.lblLocation);
            this.skinnedPanel1.Controls.Add(this.valLocation);
            this.skinnedPanel1.Controls.Add(this.lblBubble);
            this.skinnedPanel1.Controls.Add(this.lblPoseSelect);
            this.skinnedPanel1.Controls.Add(this.cboPose);
            this.skinnedPanel1.Controls.Add(this.tabsPoseSetEntries);
            this.skinnedPanel1.Controls.Add(this.tablePoseSetEntry);
            this.skinnedPanel1.Location = new System.Drawing.Point(0, 0);
            this.skinnedPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.skinnedPanel1.Name = "skinnedPanel1";
            this.skinnedPanel1.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.skinnedPanel1.Size = new System.Drawing.Size(670, 500);
            this.skinnedPanel1.TabIndex = 32;
            this.skinnedPanel1.TabSide = Desktop.Skinning.TabSide.Top;
            // 
            // lblStage
            // 
            this.lblStage.AutoSize = true;
            this.lblStage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblStage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblStage.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblStage.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblStage.Location = new System.Drawing.Point(108, 151);
            this.lblStage.Name = "lblStage";
            this.lblStage.Size = new System.Drawing.Size(190, 13);
            this.lblStage.TabIndex = 75;
            this.lblStage.Text = "(Each pose requires a Stage condition)";
            // 
            // chkLayer
            // 
            this.chkLayer.AutoSize = true;
            this.chkLayer.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkLayer.Location = new System.Drawing.Point(469, 122);
            this.chkLayer.Name = "chkLayer";
            this.chkLayer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.chkLayer.Size = new System.Drawing.Size(115, 17);
            this.chkLayer.TabIndex = 74;
            this.chkLayer.Text = "Display over image";
            this.chkLayer.UseVisualStyleBackColor = true;
            this.chkLayer.CheckedChanged += new System.EventHandler(this.chkLayer_CheckedChanged);
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblDirection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDirection.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblDirection.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblDirection.Location = new System.Drawing.Point(396, 92);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(52, 13);
            this.lblDirection.TabIndex = 73;
            this.lblDirection.Text = "Direction:";
            // 
            // cboDirection
            // 
            this.cboDirection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboDirection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboDirection.BackColor = System.Drawing.Color.White;
            this.cboDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboDirection.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboDirection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboDirection.KeyMember = null;
            this.cboDirection.Location = new System.Drawing.Point(469, 92);
            this.cboDirection.Name = "cboDirection";
            this.cboDirection.SelectedIndex = -1;
            this.cboDirection.SelectedItem = null;
            this.cboDirection.Size = new System.Drawing.Size(120, 23);
            this.cboDirection.Sorted = false;
            this.cboDirection.TabIndex = 72;
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblWeight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblWeight.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblWeight.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblWeight.Location = new System.Drawing.Point(62, 107);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(90, 13);
            this.lblWeight.TabIndex = 71;
            this.lblWeight.Text = "Weight (optional):";
            // 
            // lblPriority
            // 
            this.lblPriority.AutoSize = true;
            this.lblPriority.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblPriority.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPriority.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblPriority.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblPriority.Location = new System.Drawing.Point(62, 79);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.Size = new System.Drawing.Size(87, 13);
            this.lblPriority.TabIndex = 70;
            this.lblPriority.Text = "Priority (optional):";
            // 
            // valWeight
            // 
            this.valWeight.BackColor = System.Drawing.Color.White;
            this.valWeight.DecimalPlaces = 2;
            this.valWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.valWeight.ForeColor = System.Drawing.Color.Black;
            this.valWeight.Location = new System.Drawing.Point(155, 105);
            this.valWeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.valWeight.Name = "valWeight";
            this.valWeight.Size = new System.Drawing.Size(120, 20);
            this.valWeight.TabIndex = 69;
            this.valWeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            // 
            // valPriority
            // 
            this.valPriority.BackColor = System.Drawing.Color.White;
            this.valPriority.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.valPriority.ForeColor = System.Drawing.Color.Black;
            this.valPriority.Location = new System.Drawing.Point(155, 79);
            this.valPriority.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.valPriority.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.valPriority.Name = "valPriority";
            this.valPriority.Size = new System.Drawing.Size(120, 20);
            this.valPriority.TabIndex = 68;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblLocation.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblLocation.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblLocation.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblLocation.Location = new System.Drawing.Point(396, 68);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 67;
            this.lblLocation.Text = "Location:";
            // 
            // valLocation
            // 
            this.valLocation.BackColor = System.Drawing.Color.White;
            this.valLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.valLocation.ForeColor = System.Drawing.Color.Black;
            this.valLocation.Location = new System.Drawing.Point(469, 66);
            this.valLocation.Name = "valLocation";
            this.valLocation.Size = new System.Drawing.Size(120, 20);
            this.valLocation.TabIndex = 66;
            // 
            // lblBubble
            // 
            this.lblBubble.AutoSize = true;
            this.lblBubble.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblBubble.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblBubble.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblBubble.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblBubble.Location = new System.Drawing.Point(386, 40);
            this.lblBubble.Name = "lblBubble";
            this.lblBubble.Size = new System.Drawing.Size(201, 13);
            this.lblBubble.TabIndex = 65;
            this.lblBubble.Text = "Dialogue bubble arrow settings (optional):";
            // 
            // lblPoseSelect
            // 
            this.lblPoseSelect.AutoSize = true;
            this.lblPoseSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblPoseSelect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPoseSelect.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblPoseSelect.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblPoseSelect.Location = new System.Drawing.Point(22, 43);
            this.lblPoseSelect.Name = "lblPoseSelect";
            this.lblPoseSelect.Size = new System.Drawing.Size(34, 13);
            this.lblPoseSelect.TabIndex = 64;
            this.lblPoseSelect.Text = "Pose:";
            // 
            // cboPose
            // 
            this.cboPose.FormattingEnabled = true;
            this.cboPose.Location = new System.Drawing.Point(62, 40);
            this.cboPose.Name = "cboPose";
            this.cboPose.Size = new System.Drawing.Size(213, 21);
            this.cboPose.TabIndex = 63;
            // 
            // tablePoseSetEntry
            // 
            this.tablePoseSetEntry.AllowDelete = true;
            this.tablePoseSetEntry.AllowFavorites = true;
            this.tablePoseSetEntry.AllowHelp = false;
            this.tablePoseSetEntry.AllowMacros = false;
            this.tablePoseSetEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tablePoseSetEntry.BackColor = System.Drawing.Color.White;
            this.tablePoseSetEntry.Data = null;
            this.tablePoseSetEntry.HeaderType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.tablePoseSetEntry.HideAddField = true;
            this.tablePoseSetEntry.HideSpeedButtons = false;
            this.tablePoseSetEntry.Location = new System.Drawing.Point(5, 146);
            this.tablePoseSetEntry.Margin = new System.Windows.Forms.Padding(1);
            this.tablePoseSetEntry.ModifyingProperty = null;
            this.tablePoseSetEntry.Name = "tablePoseSetEntry";
            this.tablePoseSetEntry.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.tablePoseSetEntry.PlaceholderText = "Add";
            this.tablePoseSetEntry.PreserveControls = false;
            this.tablePoseSetEntry.PreviewData = null;
            this.tablePoseSetEntry.RemoveCaption = "Remove";
            this.tablePoseSetEntry.RowHeaderWidth = 0F;
            this.tablePoseSetEntry.RunInitialAddEvents = true;
            this.tablePoseSetEntry.Size = new System.Drawing.Size(664, 142);
            this.tablePoseSetEntry.Sorted = false;
            this.tablePoseSetEntry.TabIndex = 31;
            this.tablePoseSetEntry.UndoManager = null;
            this.tablePoseSetEntry.UseAutoComplete = true;
            // 
            // PoseSetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stripPoseSetEntries);
            this.Controls.Add(this.skinnedPanel1);
            this.Name = "PoseSetControl";
            this.Size = new System.Drawing.Size(670, 500);
            this.tabsPoseSetEntries.ResumeLayout(false);
            this.skinnedPanel1.ResumeLayout(false);
            this.skinnedPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valWeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valPriority)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLocation)).EndInit();
            this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.ToolTip toolTip1;
        private Desktop.Skinning.SkinnedTabStrip stripPoseSetEntries;
        private Desktop.Skinning.SkinnedTabControl tabsPoseSetEntries;
        private System.Windows.Forms.TabPage tabPage1;
        private Desktop.Skinning.SkinnedPanel skinnedPanel1;
        private Desktop.CommonControls.PropertyTable tablePoseSetEntry;
        private System.Windows.Forms.ComboBox cboPose;
        private Desktop.Skinning.SkinnedLabel lblPoseSelect;
        private Desktop.Skinning.SkinnedNumericUpDown valWeight;
        private Desktop.Skinning.SkinnedNumericUpDown valPriority;
        private Desktop.Skinning.SkinnedLabel lblLocation;
        private Desktop.Skinning.SkinnedNumericUpDown valLocation;
        private Desktop.Skinning.SkinnedLabel lblBubble;
        private Desktop.Skinning.SkinnedLabel lblPriority;
        private Desktop.Skinning.SkinnedCheckBox chkLayer;
        private Desktop.Skinning.SkinnedLabel lblDirection;
        private Desktop.Skinning.SkinnedComboBox cboDirection;
        private Desktop.Skinning.SkinnedLabel lblWeight;
        private Desktop.Skinning.SkinnedLabel lblStage;
    }
}
