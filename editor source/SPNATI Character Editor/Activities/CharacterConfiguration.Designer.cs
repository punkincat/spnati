namespace SPNATI_Character_Editor.Activities
{
	partial class CharacterConfiguration
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new Desktop.Skinning.SkinnedLabel();
            this.gridPrefixes = new Desktop.Skinning.SkinnedDataGridView();
            this.ColPrefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMarker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkOnlyCustomPoses = new Desktop.Skinning.SkinnedCheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkHidePrefixlessImages = new Desktop.Skinning.SkinnedCheckBox();
            this.iconHidePrefixlessImages = new Desktop.Skinning.SkinnedIcon();
            this.iconMarkers = new Desktop.Skinning.SkinnedIcon();
            this.gridMarkers = new Desktop.Skinning.SkinnedDataGridView();
            this.labelMarkers = new Desktop.Skinning.SkinnedLabel();
            ((System.ComponentModel.ISupportInitialize)(this.gridPrefixes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMarkers)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.label1.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(316, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Exclude images with these prefixes from use as poses in dialogue:";
            // 
            // gridPrefixes
            // 
            this.gridPrefixes.AllowUserToDeleteRows = false;
            this.gridPrefixes.AllowUserToResizeColumns = false;
            this.gridPrefixes.AllowUserToResizeRows = false;
            this.gridPrefixes.BackgroundColor = System.Drawing.Color.White;
            this.gridPrefixes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridPrefixes.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridPrefixes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridPrefixes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPrefixes.ColumnHeadersVisible = false;
            this.gridPrefixes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColPrefix});
            this.gridPrefixes.Data = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridPrefixes.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridPrefixes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPrefixes.EnableHeadersVisualStyles = false;
            this.gridPrefixes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.gridPrefixes.GridColor = System.Drawing.Color.LightGray;
            this.gridPrefixes.Location = new System.Drawing.Point(6, 19);
            this.gridPrefixes.Name = "gridPrefixes";
            this.gridPrefixes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridPrefixes.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridPrefixes.RowHeadersVisible = false;
            this.gridPrefixes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridPrefixes.Size = new System.Drawing.Size(313, 155);
            this.gridPrefixes.TabIndex = 1;
            // 
            // ColPrefix
            // 
            this.ColPrefix.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColPrefix.HeaderText = "Prefix";
            this.ColPrefix.Name = "ColPrefix";
            // 
            // ColMarker
            // 
            this.ColMarker.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColMarker.HeaderText = "Marker";
            this.ColMarker.Name = "ColMarker";
            // 
            // chkOnlyCustomPoses
            // 
            this.chkOnlyCustomPoses.AutoSize = true;
            this.chkOnlyCustomPoses.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkOnlyCustomPoses.Location = new System.Drawing.Point(325, 19);
            this.chkOnlyCustomPoses.Name = "chkOnlyCustomPoses";
            this.chkOnlyCustomPoses.Size = new System.Drawing.Size(141, 17);
            this.chkOnlyCustomPoses.TabIndex = 2;
            this.chkOnlyCustomPoses.Text = "Allow only custom poses";
            this.toolTip1.SetToolTip(this.chkOnlyCustomPoses, "Warning: This removes all non-custom poses from the character\'s dialogue.");
            this.chkOnlyCustomPoses.UseVisualStyleBackColor = true;
            // 
            // chkHidePrefixlessImages
            // 
            this.chkHidePrefixlessImages.AutoSize = true;
            this.chkHidePrefixlessImages.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkHidePrefixlessImages.Location = new System.Drawing.Point(325, 42);
            this.chkHidePrefixlessImages.Name = "chkHidePrefixlessImages";
            this.chkHidePrefixlessImages.Size = new System.Drawing.Size(142, 17);
            this.chkHidePrefixlessImages.TabIndex = 3;
            this.chkHidePrefixlessImages.Text = "Disallow prefixless poses";
            this.toolTip1.SetToolTip(this.chkHidePrefixlessImages, "If checked, only poses and images with a stage prefix (ex. 2-happy.png) will appe" +
        "ar for use in dialogue lines.");
            this.chkHidePrefixlessImages.UseVisualStyleBackColor = true;
            // 
            // iconHidePrefixlessImages
            // 
            this.iconHidePrefixlessImages.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.iconHidePrefixlessImages.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.iconHidePrefixlessImages.Flat = false;
            this.iconHidePrefixlessImages.Image = global::SPNATI_Character_Editor.Properties.Resources.Help;
            this.iconHidePrefixlessImages.Location = new System.Drawing.Point(459, 38);
            this.iconHidePrefixlessImages.Name = "iconHidePrefixlessImages";
            this.iconHidePrefixlessImages.Size = new System.Drawing.Size(26, 23);
            this.iconHidePrefixlessImages.TabIndex = 4;
            this.toolTip1.SetToolTip(this.iconHidePrefixlessImages, "If unchecked, poses and images with no stage prefix (ex. happy.png) will be avail" +
        "able for use in every stage.");
            this.iconHidePrefixlessImages.UseVisualStyleBackColor = true;
            // 
            // iconMarkers
            // 
            this.iconMarkers.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.iconMarkers.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.iconMarkers.Flat = false;
            this.iconMarkers.Image = global::SPNATI_Character_Editor.Properties.Resources.Help;
            this.iconMarkers.Location = new System.Drawing.Point(270, 184);
            this.iconMarkers.Name = "iconMarkers";
            this.iconMarkers.Size = new System.Drawing.Size(23, 18);
            this.iconMarkers.TabIndex = 5;
            this.toolTip1.SetToolTip(this.iconMarkers, "Example: If your marker\'s name is mymarker, and you want to set its value to one," +
        " type mymarker=1.\nThis has no effect on marker values in the game, only in the e" +
        "ditor.");
            this.iconMarkers.UseVisualStyleBackColor = true;
            // 
            // gridMarkers
            // 
            this.gridMarkers.AllowUserToDeleteRows = false;
            this.gridMarkers.AllowUserToResizeColumns = false;
            this.gridMarkers.AllowUserToResizeRows = false;
            this.gridMarkers.BackgroundColor = System.Drawing.Color.White;
            this.gridMarkers.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridMarkers.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridMarkers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridMarkers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMarkers.ColumnHeadersVisible = false;
            this.gridMarkers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColMarker});
            this.gridMarkers.Data = null;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridMarkers.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridMarkers.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMarkers.EnableHeadersVisualStyles = false;
            this.gridMarkers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.gridMarkers.GridColor = System.Drawing.Color.LightGray;
            this.gridMarkers.Location = new System.Drawing.Point(6, 205);
            this.gridMarkers.Name = "gridMarkers";
            this.gridMarkers.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridMarkers.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gridMarkers.RowHeadersVisible = false;
            this.gridMarkers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridMarkers.Size = new System.Drawing.Size(313, 190);
            this.gridMarkers.TabIndex = 3;
            // 
            // labelMarkers
            // 
            this.labelMarkers.AutoSize = true;
            this.labelMarkers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.labelMarkers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMarkers.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.labelMarkers.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.labelMarkers.Location = new System.Drawing.Point(3, 189);
            this.labelMarkers.Name = "labelMarkers";
            this.labelMarkers.Size = new System.Drawing.Size(263, 13);
            this.labelMarkers.TabIndex = 4;
            this.labelMarkers.Text = "Marker values for custom pose previewing in dialogue:";
            // 
            // CharacterConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.iconHidePrefixlessImages);
            this.Controls.Add(this.chkHidePrefixlessImages);
            this.Controls.Add(this.chkOnlyCustomPoses);
            this.Controls.Add(this.gridPrefixes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.iconMarkers);
            this.Controls.Add(this.labelMarkers);
            this.Controls.Add(this.gridMarkers);
            this.Name = "CharacterConfiguration";
            this.Size = new System.Drawing.Size(935, 644);
            ((System.ComponentModel.ISupportInitialize)(this.gridPrefixes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridMarkers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private Desktop.Skinning.SkinnedLabel label1;
		private Desktop.Skinning.SkinnedDataGridView gridPrefixes;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColPrefix;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColMarker;
        private Desktop.Skinning.SkinnedCheckBox chkOnlyCustomPoses;
        private System.Windows.Forms.ToolTip toolTip1;
        private Desktop.Skinning.SkinnedCheckBox chkHidePrefixlessImages;
        private Desktop.Skinning.SkinnedIcon iconHidePrefixlessImages;
        private Desktop.Skinning.SkinnedDataGridView gridMarkers;
        private Desktop.Skinning.SkinnedLabel labelMarkers;
        private Desktop.Skinning.SkinnedIcon iconMarkers;
    }
}