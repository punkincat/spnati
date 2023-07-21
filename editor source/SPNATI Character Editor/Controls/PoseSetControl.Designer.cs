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
            this.cboPose = new System.Windows.Forms.ComboBox();
            this.tablePoseSetEntry = new Desktop.CommonControls.PropertyTable();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblPoseSelect = new Desktop.Skinning.SkinnedLabel();
            this.tabsPoseSetEntries.SuspendLayout();
            this.skinnedPanel1.SuspendLayout();
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
            this.tablePoseSetEntry.Location = new System.Drawing.Point(1, 93);
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
            // lblPoseSelect
            // 
            this.lblPoseSelect.AutoSize = true;
            this.lblPoseSelect.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblPoseSelect.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblPoseSelect.Location = new System.Drawing.Point(22, 43);
            this.lblPoseSelect.Name = "lblPoseSelect";
            this.lblPoseSelect.Size = new System.Drawing.Size(34, 13);
            this.lblPoseSelect.TabIndex = 64;
            this.lblPoseSelect.Text = "Pose:";
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
    }
}
