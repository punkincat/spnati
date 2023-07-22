namespace SPNATI_Character_Editor.Activities
{
    partial class PoseSetEditor
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tsPoseSets = new System.Windows.Forms.ToolStrip();
            this.tsAddPoseSet = new System.Windows.Forms.ToolStripButton();
            this.tsRemovePoseSet = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsDuplicatePoseSet = new System.Windows.Forms.ToolStripButton();
            this.lstPoseSets = new Desktop.CommonControls.RefreshableListBox();
            this.lblPoseSetRename = new Desktop.Skinning.SkinnedLabel();
            this.txtPoseSetRename = new Desktop.Skinning.SkinnedTextBox();
            this.poseSetControl1 = new SPNATI_Character_Editor.Controls.PoseSetControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tsPoseSets.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tsPoseSets);
            this.splitContainer1.Panel1.Controls.Add(this.lstPoseSets);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblPoseSetRename);
            this.splitContainer1.Panel2.Controls.Add(this.txtPoseSetRename);
            this.splitContainer1.Panel2.Controls.Add(this.poseSetControl1);
            this.splitContainer1.Size = new System.Drawing.Size(935, 644);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 7;
            // 
            // tsPoseSets
            // 
            this.tsPoseSets.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsPoseSets.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsAddPoseSet,
            this.tsRemovePoseSet,
            this.toolStripSeparator1,
            this.tsDuplicatePoseSet});
            this.tsPoseSets.Location = new System.Drawing.Point(0, 0);
            this.tsPoseSets.Name = "tsPoseSets";
            this.tsPoseSets.Size = new System.Drawing.Size(200, 25);
            this.tsPoseSets.TabIndex = 6;
            this.tsPoseSets.Tag = "Surface";
            // 
            // tsAddPoseSet
            // 
            this.tsAddPoseSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsAddPoseSet.Image = global::SPNATI_Character_Editor.Properties.Resources.Add;
            this.tsAddPoseSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsAddPoseSet.Name = "tsAddPoseSet";
            this.tsAddPoseSet.Size = new System.Drawing.Size(23, 22);
            this.tsAddPoseSet.Text = "Add Pose Set";
            this.tsAddPoseSet.Click += new System.EventHandler(this.tsAddPoseSet_Click);
            // 
            // tsRemovePoseSet
            // 
            this.tsRemovePoseSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRemovePoseSet.Image = global::SPNATI_Character_Editor.Properties.Resources.Remove;
            this.tsRemovePoseSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRemovePoseSet.Name = "tsRemovePoseSet";
            this.tsRemovePoseSet.Size = new System.Drawing.Size(23, 22);
            this.tsRemovePoseSet.Text = "Remove Pose Set";
            this.tsRemovePoseSet.Click += new System.EventHandler(this.tsRemovePoseSet_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsDuplicatePoseSet
            // 
            this.tsDuplicatePoseSet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsDuplicatePoseSet.Image = global::SPNATI_Character_Editor.Properties.Resources.Duplicate;
            this.tsDuplicatePoseSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDuplicatePoseSet.Name = "tsDuplicatePoseSet";
            this.tsDuplicatePoseSet.Size = new System.Drawing.Size(23, 22);
            this.tsDuplicatePoseSet.Text = "Duplicate Pose Set";
            this.tsDuplicatePoseSet.Click += new System.EventHandler(this.tsDuplicatePoseSet_Click);
            // 
            // lstPoseSets
            // 
            this.lstPoseSets.BackColor = System.Drawing.Color.White;
            this.lstPoseSets.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lstPoseSets.ForeColor = System.Drawing.Color.Black;
            this.lstPoseSets.FormattingEnabled = true;
            this.lstPoseSets.Location = new System.Drawing.Point(3, 28);
            this.lstPoseSets.Name = "lstPoseSets";
            this.lstPoseSets.Size = new System.Drawing.Size(194, 472);
            this.lstPoseSets.TabIndex = 5;
            this.lstPoseSets.SelectedIndexChanged += new System.EventHandler(this.lstPoseSets_SelectedIndexChanged);
            // 
            // lblPoseSetRename
            // 
            this.lblPoseSetRename.AutoSize = true;
            this.lblPoseSetRename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblPoseSetRename.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPoseSetRename.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblPoseSetRename.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblPoseSetRename.Location = new System.Drawing.Point(4, 17);
            this.lblPoseSetRename.Name = "lblPoseSetRename";
            this.lblPoseSetRename.Size = new System.Drawing.Size(54, 13);
            this.lblPoseSetRename.TabIndex = 5;
            this.lblPoseSetRename.Text = "Set Name";
            // 
            // txtPoseSetRename
            // 
            this.txtPoseSetRename.BackColor = System.Drawing.Color.White;
            this.txtPoseSetRename.ForeColor = System.Drawing.Color.Black;
            this.txtPoseSetRename.Location = new System.Drawing.Point(64, 14);
            this.txtPoseSetRename.Name = "txtPoseSetRename";
            this.txtPoseSetRename.Size = new System.Drawing.Size(167, 20);
            this.txtPoseSetRename.TabIndex = 4;
            this.txtPoseSetRename.TextChanged += new System.EventHandler(this.txtPoseSetRename_TextChanged);
            // 
            // poseSetControl1
            // 
            this.poseSetControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.poseSetControl1.Location = new System.Drawing.Point(0, 50);
            this.poseSetControl1.Name = "poseSetControl1";
            this.poseSetControl1.Size = new System.Drawing.Size(728, 500);
            this.poseSetControl1.TabIndex = 3;
            // 
            // PoseSetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PoseSetEditor";
            this.Size = new System.Drawing.Size(935, 644);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tsPoseSets.ResumeLayout(false);
            this.tsPoseSets.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private Controls.PoseSetControl poseSetControl1;
        private Desktop.CommonControls.RefreshableListBox lstPoseSets;
        private System.Windows.Forms.ToolStrip tsPoseSets;
        private System.Windows.Forms.ToolStripButton tsAddPoseSet;
        private System.Windows.Forms.ToolStripButton tsRemovePoseSet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsDuplicatePoseSet;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Desktop.Skinning.SkinnedLabel lblPoseSetRename;
        private Desktop.Skinning.SkinnedTextBox txtPoseSetRename;
    }
}