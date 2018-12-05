﻿namespace SPNATI_Character_Editor.Activities
{
	partial class PoseListEditor
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.cmdImportAll = new System.Windows.Forms.Button();
			this.cmdImportNew = new System.Windows.Forms.Button();
			this.cmdClear = new System.Windows.Forms.Button();
			this.cmdExport = new System.Windows.Forms.Button();
			this.cmdImport = new System.Windows.Forms.Button();
			this.gridPoses = new System.Windows.Forms.DataGridView();
			this.ColStage = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColPose = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColL = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColR = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColB = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColData = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColImage = new System.Windows.Forms.DataGridViewImageColumn();
			this.ColImport = new System.Windows.Forms.DataGridViewButtonColumn();
			this.label5 = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.lblCurrentPoseFile = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.gridPoses)).BeginInit();
			this.SuspendLayout();
			// 
			// cmdImportAll
			// 
			this.cmdImportAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdImportAll.Location = new System.Drawing.Point(843, 3);
			this.cmdImportAll.Name = "cmdImportAll";
			this.cmdImportAll.Size = new System.Drawing.Size(95, 23);
			this.cmdImportAll.TabIndex = 25;
			this.cmdImportAll.Text = "Import All";
			this.cmdImportAll.UseVisualStyleBackColor = true;
			this.cmdImportAll.Click += new System.EventHandler(this.cmdImportAll_Click);
			// 
			// cmdImportNew
			// 
			this.cmdImportNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdImportNew.Location = new System.Drawing.Point(742, 3);
			this.cmdImportNew.Name = "cmdImportNew";
			this.cmdImportNew.Size = new System.Drawing.Size(95, 23);
			this.cmdImportNew.TabIndex = 24;
			this.cmdImportNew.Text = "Import New";
			this.cmdImportNew.UseVisualStyleBackColor = true;
			this.cmdImportNew.Click += new System.EventHandler(this.cmdImportNew_Click);
			// 
			// cmdClear
			// 
			this.cmdClear.Location = new System.Drawing.Point(205, 3);
			this.cmdClear.Name = "cmdClear";
			this.cmdClear.Size = new System.Drawing.Size(54, 23);
			this.cmdClear.TabIndex = 23;
			this.cmdClear.Text = "Clear";
			this.cmdClear.UseVisualStyleBackColor = true;
			this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
			// 
			// cmdExport
			// 
			this.cmdExport.Location = new System.Drawing.Point(104, 3);
			this.cmdExport.Name = "cmdExport";
			this.cmdExport.Size = new System.Drawing.Size(95, 23);
			this.cmdExport.TabIndex = 22;
			this.cmdExport.Text = "Save Pose List";
			this.cmdExport.UseVisualStyleBackColor = true;
			this.cmdExport.Click += new System.EventHandler(this.cmdExport_Click);
			// 
			// cmdImport
			// 
			this.cmdImport.Location = new System.Drawing.Point(3, 3);
			this.cmdImport.Name = "cmdImport";
			this.cmdImport.Size = new System.Drawing.Size(95, 23);
			this.cmdImport.TabIndex = 21;
			this.cmdImport.Text = "Load Pose List";
			this.cmdImport.UseVisualStyleBackColor = true;
			this.cmdImport.Click += new System.EventHandler(this.cmdImport_Click);
			// 
			// gridPoses
			// 
			this.gridPoses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridPoses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridPoses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColStage,
            this.ColPose,
            this.ColL,
            this.ColT,
            this.ColR,
            this.ColB,
            this.ColData,
            this.ColImage,
            this.ColImport});
			this.gridPoses.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
			this.gridPoses.Location = new System.Drawing.Point(3, 32);
			this.gridPoses.MultiSelect = false;
			this.gridPoses.Name = "gridPoses";
			this.gridPoses.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.gridPoses.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridPoses.RowTemplate.Height = 100;
			this.gridPoses.Size = new System.Drawing.Size(935, 591);
			this.gridPoses.TabIndex = 20;
			this.gridPoses.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridPoses_CellContentClick);
			this.gridPoses.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.gridPoses_RowPrePaint);
			this.gridPoses.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.gridPoses_RowsAdded);
			this.gridPoses.Scroll += new System.Windows.Forms.ScrollEventHandler(this.gridPoses_Scroll);
			// 
			// ColStage
			// 
			this.ColStage.HeaderText = "Stage";
			this.ColStage.Name = "ColStage";
			this.ColStage.Width = 50;
			// 
			// ColPose
			// 
			this.ColPose.HeaderText = "Pose";
			this.ColPose.Name = "ColPose";
			// 
			// ColL
			// 
			this.ColL.HeaderText = "L";
			this.ColL.Name = "ColL";
			this.ColL.Width = 40;
			// 
			// ColT
			// 
			this.ColT.HeaderText = "T";
			this.ColT.Name = "ColT";
			this.ColT.Width = 40;
			// 
			// ColR
			// 
			this.ColR.HeaderText = "R";
			this.ColR.Name = "ColR";
			this.ColR.Width = 40;
			// 
			// ColB
			// 
			this.ColB.HeaderText = "B";
			this.ColB.Name = "ColB";
			this.ColB.Width = 40;
			// 
			// ColData
			// 
			this.ColData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.ColData.DefaultCellStyle = dataGridViewCellStyle1;
			this.ColData.HeaderText = "Code";
			this.ColData.Name = "ColData";
			// 
			// ColImage
			// 
			this.ColImage.HeaderText = "Image";
			this.ColImage.Name = "ColImage";
			this.ColImage.Width = 75;
			// 
			// ColImport
			// 
			this.ColImport.HeaderText = "";
			this.ColImport.Name = "ColImport";
			this.ColImport.Width = 70;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(3, 7);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(77, 13);
			this.label5.TabIndex = 33;
			this.label5.Text = "Import Preview";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "Text files|*.txt";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "Text files|*.txt";
			// 
			// lblCurrentPoseFile
			// 
			this.lblCurrentPoseFile.AutoSize = true;
			this.lblCurrentPoseFile.Location = new System.Drawing.Point(265, 7);
			this.lblCurrentPoseFile.Name = "lblCurrentPoseFile";
			this.lblCurrentPoseFile.Size = new System.Drawing.Size(100, 13);
			this.lblCurrentPoseFile.TabIndex = 34;
			this.lblCurrentPoseFile.Text = "No pose list loaded.";
			// 
			// PoseListEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblCurrentPoseFile);
			this.Controls.Add(this.cmdImportAll);
			this.Controls.Add(this.cmdImportNew);
			this.Controls.Add(this.cmdClear);
			this.Controls.Add(this.cmdExport);
			this.Controls.Add(this.cmdImport);
			this.Controls.Add(this.gridPoses);
			this.Controls.Add(this.label5);
			this.Name = "PoseListEditor";
			this.Size = new System.Drawing.Size(941, 626);
			((System.ComponentModel.ISupportInitialize)(this.gridPoses)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button cmdImportAll;
		private System.Windows.Forms.Button cmdImportNew;
		private System.Windows.Forms.Button cmdClear;
		private System.Windows.Forms.Button cmdExport;
		private System.Windows.Forms.Button cmdImport;
		private System.Windows.Forms.DataGridView gridPoses;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColStage;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColPose;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColL;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColT;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColR;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColB;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColData;
		private System.Windows.Forms.DataGridViewImageColumn ColImage;
		private System.Windows.Forms.DataGridViewButtonColumn ColImport;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Label lblCurrentPoseFile;
	}
}
