namespace SPNATI_Character_Editor.Activities
{
	partial class BanterWizard
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panelLoad = new Desktop.Skinning.SkinnedPanel();
            this.lblProgress = new Desktop.Skinning.SkinnedLabel();
            this.progressBar = new Desktop.Skinning.SkinnedProgressBar();
            this.cmdLoadBanter = new Desktop.Skinning.SkinnedButton();
            this.cmdUpdateBanter = new Desktop.Skinning.SkinnedButton();
            this.cmdSaveBanter = new Desktop.Skinning.SkinnedButton();
            this.lstCharacters = new Desktop.Skinning.SkinnedListBox();
            this.lblCharacters = new Desktop.Skinning.SkinnedLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.grpResponse = new Desktop.Skinning.SkinnedGroupBox();
            this.skinnedLabel2 = new Desktop.Skinning.SkinnedLabel();
            this.ctlResponse = new SPNATI_Character_Editor.Controls.CaseControl();
            this.gridResponse = new SPNATI_Character_Editor.Controls.DialogueGrid();
            this.cmdJump = new Desktop.Skinning.SkinnedButton();
            this.cmdAccept = new Desktop.Skinning.SkinnedButton();
            this.cmdDiscard = new Desktop.Skinning.SkinnedButton();
            this.lblBasicText = new Desktop.Skinning.SkinnedLabel();
            this.grpBaseLine = new Desktop.Skinning.SkinnedGroupBox();
            this.skinnedLabel1 = new Desktop.Skinning.SkinnedLabel();
            this.lstBasicLines = new Desktop.Skinning.SkinnedListBox();
            this.lblCaseInfo = new Desktop.Skinning.SkinnedLabel();
            this.grpLines = new Desktop.Skinning.SkinnedGroupBox();
            this.cmdCreateResponse = new Desktop.Skinning.SkinnedButton();
            this.lblNoMatches = new Desktop.Skinning.SkinnedLabel();
            this.gridLines = new Desktop.Skinning.SkinnedDataGridView();
            this.ColJump = new Desktop.Skinning.SkinnedDataGridViewButtonColumn();
            this.ColCase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNewness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmdColorCode = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelLoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.grpResponse.SuspendLayout();
            this.grpBaseLine.SuspendLayout();
            this.grpLines.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLines)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.cmdLoadBanter);
            this.splitContainer1.Panel1.Controls.Add(this.cmdUpdateBanter);
            this.splitContainer1.Panel1.Controls.Add(this.cmdSaveBanter);
            this.splitContainer1.Panel1.Controls.Add(this.lstCharacters);
            this.splitContainer1.Panel1.Controls.Add(this.lblCharacters);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.panelLoad);
            this.splitContainer1.Size = new System.Drawing.Size(1161, 674);
            this.splitContainer1.SplitterDistance = 206;
            this.splitContainer1.TabIndex = 1;
            // 
            // panelLoad
            // 
            this.panelLoad.Controls.Add(this.lblProgress);
            this.panelLoad.Controls.Add(this.progressBar);
            this.panelLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLoad.Location = new System.Drawing.Point(0, 0);
            this.panelLoad.Name = "panelLoad";
            this.panelLoad.PanelType = Desktop.Skinning.SkinnedBackgroundType.Background;
            this.panelLoad.Size = new System.Drawing.Size(951, 674);
            this.panelLoad.TabIndex = 5;
            this.panelLoad.TabSide = Desktop.Skinning.TabSide.None;
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblProgress.ForeColor = System.Drawing.Color.Blue;
            this.lblProgress.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblProgress.Level = Desktop.Skinning.SkinnedLabelLevel.Heading;
            this.lblProgress.Location = new System.Drawing.Point(80, 292);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(784, 23);
            this.lblProgress.TabIndex = 1;
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // progress
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(80, 322);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(784, 23);
            this.progressBar.TabIndex = 0;
            // 
            // cmdLoadBanter
            // 
            this.cmdLoadBanter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdLoadBanter.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.cmdLoadBanter.FieldType = Desktop.Skinning.SkinnedFieldType.Secondary;
            this.cmdLoadBanter.Flat = false;
            this.cmdLoadBanter.Location = new System.Drawing.Point(12, 585);
            this.cmdLoadBanter.Name = "cmdLoadBanter";
            this.cmdLoadBanter.Size = new System.Drawing.Size(141, 23);
            this.cmdLoadBanter.TabIndex = 8;
            this.cmdLoadBanter.Text = "Load";
            this.toolTip1.SetToolTip(this.cmdLoadBanter, "Load banter data from a file.");
            this.cmdLoadBanter.UseVisualStyleBackColor = true;
            this.cmdLoadBanter.Click += new System.EventHandler(this.cmdLoadBanter_Click);
            // 
            // cmdUpdateBanter
            // 
            this.cmdUpdateBanter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdUpdateBanter.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.cmdUpdateBanter.FieldType = Desktop.Skinning.SkinnedFieldType.Secondary;
            this.cmdUpdateBanter.Flat = false;
            this.cmdUpdateBanter.Location = new System.Drawing.Point(12, 615);
            this.cmdUpdateBanter.Name = "cmdUpdateBanter";
            this.cmdUpdateBanter.Size = new System.Drawing.Size(141, 23);
            this.cmdUpdateBanter.TabIndex = 7;
            this.cmdUpdateBanter.Text = "Generate";
            this.toolTip1.SetToolTip(this.cmdUpdateBanter, "Generate banter data. This may take a long time.");
            this.cmdUpdateBanter.UseVisualStyleBackColor = true;
            this.cmdUpdateBanter.Click += new System.EventHandler(this.cmdUpdateBanter_Click);
            // 
            // cmdSaveBanter
            // 
            this.cmdSaveBanter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdSaveBanter.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.cmdSaveBanter.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.cmdSaveBanter.Flat = false;
            this.cmdSaveBanter.Location = new System.Drawing.Point(12, 645);
            this.cmdSaveBanter.Name = "cmdSaveBanter";
            this.cmdSaveBanter.Size = new System.Drawing.Size(141, 23);
            this.cmdSaveBanter.TabIndex = 6;
            this.cmdSaveBanter.Text = "Save all";
            this.toolTip1.SetToolTip(this.cmdSaveBanter, "Save banter data to a file. They are also saved when you save the character.");
            this.cmdSaveBanter.UseVisualStyleBackColor = true;
            this.cmdSaveBanter.Click += new System.EventHandler(this.cmdSaveBanter_Click);
            // 
            // lstCharacters
            // 
            this.lstCharacters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCharacters.BackColor = System.Drawing.Color.White;
            this.lstCharacters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lstCharacters.ForeColor = System.Drawing.Color.Black;
            this.lstCharacters.FormattingEnabled = true;
            this.lstCharacters.Location = new System.Drawing.Point(12, 30);
            this.lstCharacters.Name = "lstCharacters";
            this.lstCharacters.Size = new System.Drawing.Size(184, 303);
            this.lstCharacters.TabIndex = 1;
            this.lstCharacters.SelectedIndexChanged += new System.EventHandler(this.lstCharacters_SelectedIndexChanged);
            // 
            // lblCharacters
            // 
            this.lblCharacters.AutoSize = true;
            this.lblCharacters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblCharacters.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCharacters.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblCharacters.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblCharacters.Location = new System.Drawing.Point(12, 9);
            this.lblCharacters.Name = "lblCharacters";
            this.lblCharacters.Size = new System.Drawing.Size(126, 13);
            this.lblCharacters.TabIndex = 0;
            this.lblCharacters.Text = "Characters that target {0}";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.grpBaseLine);
            this.splitContainer3.Panel1.Controls.Add(this.lblBasicText);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.grpResponse);
            this.splitContainer3.Size = new System.Drawing.Size(951, 398);
            this.splitContainer3.SplitterDistance = 438;
            this.splitContainer3.TabIndex = 6;
            // 
            // grpResponse
            // 
            this.grpResponse.BackColor = System.Drawing.Color.White;
            this.grpResponse.Controls.Add(this.cmdDiscard);
            this.grpResponse.Controls.Add(this.cmdAccept);
            this.grpResponse.Controls.Add(this.cmdJump);
            this.grpResponse.Controls.Add(this.gridResponse);
            this.grpResponse.Controls.Add(this.ctlResponse);
            this.grpResponse.Controls.Add(this.skinnedLabel2);
            this.grpResponse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpResponse.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.grpResponse.Image = null;
            this.grpResponse.Location = new System.Drawing.Point(0, 0);
            this.grpResponse.Name = "grpResponse";
            this.grpResponse.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.grpResponse.ShowIndicatorBar = false;
            this.grpResponse.Size = new System.Drawing.Size(509, 398);
            this.grpResponse.TabIndex = 6;
            this.grpResponse.TabStop = false;
            this.grpResponse.Text = "Write Response";
            // 
            // skinnedLabel2
            // 
            this.skinnedLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinnedLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.skinnedLabel2.ForeColor = System.Drawing.Color.Red;
            this.skinnedLabel2.Highlight = Desktop.Skinning.SkinnedHighlight.Bad;
            this.skinnedLabel2.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.skinnedLabel2.Location = new System.Drawing.Point(6, 23);
            this.skinnedLabel2.Name = "skinnedLabel2";
            this.skinnedLabel2.Size = new System.Drawing.Size(497, 26);
            this.skinnedLabel2.TabIndex = 9;
            this.skinnedLabel2.Text = "These play at the same time as the selected line above, so be careful not to over" +
    "write the line that is giving context to what the opponent is saying!";
            // 
            // ctlResponse
            // 
            this.ctlResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlResponse.Location = new System.Drawing.Point(9, 52);
            this.ctlResponse.Name = "ctlResponse";
            this.ctlResponse.Size = new System.Drawing.Size(494, 311);
            this.ctlResponse.TabIndex = 10;
            this.ctlResponse.HighlightRow += new System.EventHandler<int>(this.gridResponse_HighlightRow);
            // 
            // gridResponse
            // 
            this.gridResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridResponse.Location = new System.Drawing.Point(9, 52);
            this.gridResponse.Name = "gridResponse";
            this.gridResponse.ReadOnly = false;
            this.gridResponse.Size = new System.Drawing.Size(494, 311);
            this.gridResponse.TabIndex = 0;
            this.gridResponse.HighlightRow += new System.EventHandler<int>(this.gridResponse_HighlightRow);
            // 
            // cmdJump
            // 
            this.cmdJump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdJump.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.cmdJump.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.cmdJump.Flat = false;
            this.cmdJump.Location = new System.Drawing.Point(67, 369);
            this.cmdJump.Name = "cmdJump";
            this.cmdJump.Size = new System.Drawing.Size(141, 23);
            this.cmdJump.TabIndex = 11;
            this.cmdJump.Text = "Edit Full Screen";
            this.cmdJump.UseVisualStyleBackColor = true;
            this.cmdJump.Click += new System.EventHandler(this.cmdJump_Click);
            // 
            // cmdAccept
            // 
            this.cmdAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAccept.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.cmdAccept.FieldType = Desktop.Skinning.SkinnedFieldType.Secondary;
            this.cmdAccept.Flat = false;
            this.cmdAccept.Location = new System.Drawing.Point(214, 369);
            this.cmdAccept.Name = "cmdAccept";
            this.cmdAccept.Size = new System.Drawing.Size(141, 23);
            this.cmdAccept.TabIndex = 12;
            this.cmdAccept.Text = "Accept";
            this.cmdAccept.UseVisualStyleBackColor = true;
            this.cmdAccept.Click += new System.EventHandler(this.cmdAccept_Click);
            // 
            // cmdDiscard
            // 
            this.cmdDiscard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDiscard.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.cmdDiscard.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.cmdDiscard.Flat = true;
            this.cmdDiscard.ForeColor = System.Drawing.Color.Blue;
            this.cmdDiscard.Location = new System.Drawing.Point(361, 369);
            this.cmdDiscard.Name = "cmdDiscard";
            this.cmdDiscard.Size = new System.Drawing.Size(141, 23);
            this.cmdDiscard.TabIndex = 13;
            this.cmdDiscard.Text = "Discard";
            this.cmdDiscard.UseVisualStyleBackColor = true;
            this.cmdDiscard.Click += new System.EventHandler(this.cmdDiscard_Click);
            // 
            // lblBasicText
            // 
            this.lblBasicText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBasicText.AutoSize = true;
            this.lblBasicText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblBasicText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblBasicText.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblBasicText.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblBasicText.Location = new System.Drawing.Point(172, 0);
            this.lblBasicText.Name = "lblBasicText";
            this.lblBasicText.Size = new System.Drawing.Size(0, 13);
            this.lblBasicText.TabIndex = 6;
            // 
            // grpBaseLine
            // 
            this.grpBaseLine.BackColor = System.Drawing.Color.White;
            this.grpBaseLine.Controls.Add(this.lstBasicLines);
            this.grpBaseLine.Controls.Add(this.skinnedLabel1);
            this.grpBaseLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBaseLine.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.grpBaseLine.Image = null;
            this.grpBaseLine.Location = new System.Drawing.Point(0, 0);
            this.grpBaseLine.Name = "grpBaseLine";
            this.grpBaseLine.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.grpBaseLine.ShowIndicatorBar = false;
            this.grpBaseLine.Size = new System.Drawing.Size(438, 398);
            this.grpBaseLine.TabIndex = 8;
            this.grpBaseLine.TabStop = false;
            this.grpBaseLine.Text = "Response";
            // 
            // skinnedLabel1
            // 
            this.skinnedLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.skinnedLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.skinnedLabel1.ForeColor = System.Drawing.Color.Red;
            this.skinnedLabel1.Highlight = Desktop.Skinning.SkinnedHighlight.Bad;
            this.skinnedLabel1.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.skinnedLabel1.Location = new System.Drawing.Point(7, 23);
            this.skinnedLabel1.Name = "skinnedLabel1";
            this.skinnedLabel1.Size = new System.Drawing.Size(424, 35);
            this.skinnedLabel1.TabIndex = 8;
            this.skinnedLabel1.Text = "Disclaimer: These are estimates. Actual results in game may vary depending on the" +
    " game state.";
            // 
            // lstBasicLines
            // 
            this.lstBasicLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstBasicLines.BackColor = System.Drawing.Color.White;
            this.lstBasicLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lstBasicLines.ForeColor = System.Drawing.Color.Black;
            this.lstBasicLines.FormattingEnabled = true;
            this.lstBasicLines.IntegralHeight = false;
            this.lstBasicLines.Location = new System.Drawing.Point(6, 52);
            this.lstBasicLines.Name = "lstBasicLines";
            this.lstBasicLines.Size = new System.Drawing.Size(425, 342);
            this.lstBasicLines.TabIndex = 7;
            // 
            // lblCaseInfo
            // 
            this.lblCaseInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCaseInfo.AutoSize = true;
            this.lblCaseInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblCaseInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCaseInfo.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblCaseInfo.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblCaseInfo.Location = new System.Drawing.Point(3, 251);
            this.lblCaseInfo.Name = "lblCaseInfo";
            this.lblCaseInfo.Size = new System.Drawing.Size(0, 13);
            this.lblCaseInfo.TabIndex = 3;
            // 
            // grpLines
            // 
            this.grpLines.BackColor = System.Drawing.Color.White;
            this.grpLines.Controls.Add(this.cmdColorCode);
            this.grpLines.Controls.Add(this.gridLines);
            this.grpLines.Controls.Add(this.lblNoMatches);
            this.grpLines.Controls.Add(this.cmdCreateResponse);
            this.grpLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLines.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.grpLines.Image = null;
            this.grpLines.Location = new System.Drawing.Point(0, 0);
            this.grpLines.Name = "grpLines";
            this.grpLines.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.grpLines.ShowIndicatorBar = false;
            this.grpLines.Size = new System.Drawing.Size(951, 272);
            this.grpLines.TabIndex = 5;
            this.grpLines.TabStop = false;
            this.grpLines.Text = "Lines";
            // 
            // cmdCreateResponse
            // 
            this.cmdCreateResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCreateResponse.Background = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.cmdCreateResponse.FieldType = Desktop.Skinning.SkinnedFieldType.Secondary;
            this.cmdCreateResponse.Flat = false;
            this.cmdCreateResponse.Location = new System.Drawing.Point(803, 243);
            this.cmdCreateResponse.Name = "cmdCreateResponse";
            this.cmdCreateResponse.Size = new System.Drawing.Size(141, 23);
            this.cmdCreateResponse.TabIndex = 0;
            this.cmdCreateResponse.Text = "Create Response";
            this.toolTip1.SetToolTip(this.cmdCreateResponse, "Responding via this button is possible but not recommended.\nUsually, it is better" +
        " to use the arrow buttons above and respond from the Dialogue tab.");
            this.cmdCreateResponse.UseVisualStyleBackColor = true;
            this.cmdCreateResponse.Click += new System.EventHandler(this.cmdCreateResponse_Click);
            // 
            // lblNoMatches
            // 
            this.lblNoMatches.AutoSize = true;
            this.lblNoMatches.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblNoMatches.ForeColor = System.Drawing.Color.Red;
            this.lblNoMatches.Highlight = Desktop.Skinning.SkinnedHighlight.Bad;
            this.lblNoMatches.Level = Desktop.Skinning.SkinnedLabelLevel.Heading;
            this.lblNoMatches.Location = new System.Drawing.Point(6, 28);
            this.lblNoMatches.Name = "lblNoMatches";
            this.lblNoMatches.Size = new System.Drawing.Size(93, 21);
            this.lblNoMatches.TabIndex = 4;
            this.lblNoMatches.Text = "None found";
            this.lblNoMatches.Visible = false;
            // 
            // gridLines
            // 
            this.gridLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLines.BackgroundColor = System.Drawing.Color.White;
            this.gridLines.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLines.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColColor,
            this.ColNewness,
            this.ColText,
            this.ColStage,
            this.ColCase,
            this.ColJump});
            this.gridLines.Data = null;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridLines.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridLines.EnableHeadersVisualStyles = false;
            this.gridLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.gridLines.GridColor = System.Drawing.Color.LightGray;
            this.gridLines.Location = new System.Drawing.Point(6, 25);
            this.gridLines.MultiSelect = false;
            this.gridLines.Name = "gridLines";
            this.gridLines.ReadOnly = true;
            this.gridLines.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridLines.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridLines.Size = new System.Drawing.Size(939, 212);
            this.gridLines.TabIndex = 0;
            this.gridLines.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridLines_CellContentClick);
            this.gridLines.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridLines_CellEnter);
            this.gridLines.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gridLines_CellPainting);
            // 
            // ColJump
            // 
            this.ColJump.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.ColJump.Flat = false;
            this.ColJump.HeaderText = "";
            this.ColJump.Name = "ColJump";
            this.ColJump.ReadOnly = true;
            this.ColJump.ToolTipText = "Jump to the Dialogue tab";
            this.ColJump.Width = 21;
            // 
            // ColCase
            // 
            this.ColCase.HeaderText = "Case";
            this.ColCase.Name = "ColCase";
            this.ColCase.ReadOnly = true;
            this.ColCase.Width = 150;
            // 
            // ColStage
            // 
            this.ColStage.HeaderText = "Stages";
            this.ColStage.Name = "ColStage";
            this.ColStage.ReadOnly = true;
            this.ColStage.Width = 50;
            // 
            // ColText
            // 
            this.ColText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColText.HeaderText = "Text";
            this.ColText.Name = "ColText";
            this.ColText.ReadOnly = true;
            // 
            // ColNewness
            // 
            this.ColNewness.HeaderText = "";
            this.ColNewness.Name = "ColNewness";
            this.ColNewness.ReadOnly = true;
            this.ColNewness.ToolTipText = "New lines";
            this.ColNewness.Width = 20;
            // 
            // ColColor
            // 
            this.ColColor.HeaderText = "";
            this.ColColor.Name = "ColColor";
            this.ColColor.ReadOnly = true;
            this.ColColor.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColColor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColColor.ToolTipText = "Color selector";
            this.ColColor.Width = 20;
            // 
            // cmdColorCode
            // 
            this.cmdColorCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdColorCode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdColorCode.Location = new System.Drawing.Point(10, 243);
            this.cmdColorCode.Name = "cmdColorCode";
            this.cmdColorCode.Size = new System.Drawing.Size(75, 23);
            this.cmdColorCode.TabIndex = 5;
            this.cmdColorCode.UseVisualStyleBackColor = true;
            this.cmdColorCode.Click += new System.EventHandler(this.cmdColorCode_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.grpLines);
            this.splitContainer2.Panel1.Controls.Add(this.lblCaseInfo);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(951, 674);
            this.splitContainer2.SplitterDistance = 272;
            this.splitContainer2.TabIndex = 0;
            // 
            // BanterWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "BanterWizard";
            this.Size = new System.Drawing.Size(1161, 674);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelLoad.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.grpResponse.ResumeLayout(false);
            this.grpBaseLine.ResumeLayout(false);
            this.grpLines.ResumeLayout(false);
            this.grpLines.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridLines)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

	private System.Windows.Forms.SplitContainer splitContainer1;
	private Desktop.Skinning.SkinnedListBox lstCharacters;
	private Desktop.Skinning.SkinnedLabel lblCharacters;
	private System.Windows.Forms.ToolTip toolTip1;
	private Desktop.Skinning.SkinnedPanel panelLoad;
	private Desktop.Skinning.SkinnedProgressBar progressBar;
	private Desktop.Skinning.SkinnedLabel lblProgress;
        private Desktop.Skinning.SkinnedButton cmdUpdateBanter;
        private Desktop.Skinning.SkinnedButton cmdSaveBanter;
        private Desktop.Skinning.SkinnedButton cmdLoadBanter;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Desktop.Skinning.SkinnedGroupBox grpLines;
        private System.Windows.Forms.Button cmdColorCode;
        private Desktop.Skinning.SkinnedDataGridView gridLines;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNewness;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColText;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStage;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCase;
        private Desktop.Skinning.SkinnedDataGridViewButtonColumn ColJump;
        private Desktop.Skinning.SkinnedLabel lblNoMatches;
        private Desktop.Skinning.SkinnedButton cmdCreateResponse;
        private Desktop.Skinning.SkinnedLabel lblCaseInfo;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private Desktop.Skinning.SkinnedGroupBox grpBaseLine;
        private Desktop.Skinning.SkinnedListBox lstBasicLines;
        private Desktop.Skinning.SkinnedLabel skinnedLabel1;
        private Desktop.Skinning.SkinnedLabel lblBasicText;
        private Desktop.Skinning.SkinnedGroupBox grpResponse;
        private Desktop.Skinning.SkinnedButton cmdDiscard;
        private Desktop.Skinning.SkinnedButton cmdAccept;
        private Desktop.Skinning.SkinnedButton cmdJump;
        private Controls.DialogueGrid gridResponse;
        private Controls.CaseControl ctlResponse;
        private Desktop.Skinning.SkinnedLabel skinnedLabel2;
    }
}
