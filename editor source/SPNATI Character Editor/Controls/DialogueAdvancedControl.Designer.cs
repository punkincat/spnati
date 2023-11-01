namespace SPNATI_Character_Editor.Controls
{
	partial class DialogueAdvancedControl
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
            this.groupBoxArrow = new Desktop.Skinning.SkinnedGroupBox();
            this.valLocation = new Desktop.Skinning.SkinnedNumericUpDown();
            this.lblArrowLocation = new Desktop.Skinning.SkinnedLabel();
            this.cboDirection = new Desktop.Skinning.SkinnedComboBox();
            this.lblArrowDirection = new Desktop.Skinning.SkinnedLabel();
            this.groupBoxState = new Desktop.Skinning.SkinnedGroupBox();
            this.chkResetLabel = new Desktop.Skinning.SkinnedCheckBox();
            this.txtLabel = new Desktop.Skinning.SkinnedTextBox();
            this.cboAI = new Desktop.Skinning.SkinnedComboBox();
            this.lblStateLabel = new Desktop.Skinning.SkinnedLabel();
            this.lblStateAI = new Desktop.Skinning.SkinnedLabel();
            this.cboSize = new Desktop.Skinning.SkinnedComboBox();
            this.lblStateSize = new Desktop.Skinning.SkinnedLabel();
            this.cboGender = new Desktop.Skinning.SkinnedComboBox();
            this.lblStateGender = new Desktop.Skinning.SkinnedLabel();
            this.chkResetAI = new Desktop.Skinning.SkinnedCheckBox();
            this.lblWeight = new Desktop.Skinning.SkinnedLabel();
            this.valWeight = new Desktop.Skinning.SkinnedNumericUpDown();
            this.groupBoxBubble = new Desktop.Skinning.SkinnedGroupBox();
            this.cboFontSize = new Desktop.Skinning.SkinnedComboBox();
            this.lblBubbleTextSize = new Desktop.Skinning.SkinnedLabel();
            this.chkLayer = new Desktop.Skinning.SkinnedCheckBox();
            this.groupBoxForfeit = new Desktop.Skinning.SkinnedGroupBox();
            this.txtValue = new Desktop.Skinning.SkinnedTextBox();
            this.lblFOHeavy = new Desktop.Skinning.SkinnedLabel();
            this.cboHeavy = new Desktop.Skinning.SkinnedComboBox();
            this.lblFOValue = new Desktop.Skinning.SkinnedLabel();
            this.chkResetHeavy = new Desktop.Skinning.SkinnedCheckBox();
            this.cboOp = new Desktop.Skinning.SkinnedComboBox();
            this.cboAttr = new Desktop.Skinning.SkinnedComboBox();
            this.lblFOOperation = new Desktop.Skinning.SkinnedLabel();
            this.lblFOAttribute = new Desktop.Skinning.SkinnedLabel();
            this.groupBoxNickname = new Desktop.Skinning.SkinnedGroupBox();
            this.lstNick = new Desktop.Skinning.SkinnedListBox();
            this.tsNick = new System.Windows.Forms.ToolStrip();
            this.tsNickAdd = new System.Windows.Forms.ToolStripButton();
            this.tsNickRemove = new System.Windows.Forms.ToolStripButton();
            this.tsNickMoveUp = new System.Windows.Forms.ToolStripButton();
            this.tsNickMoveDown = new System.Windows.Forms.ToolStripButton();
            this.lblNickWeight = new Desktop.Skinning.SkinnedLabel();
            this.lblNickOp = new Desktop.Skinning.SkinnedLabel();
            this.valNickWeight = new Desktop.Skinning.SkinnedNumericUpDown();
            this.cboNickOp = new Desktop.Skinning.SkinnedComboBox();
            this.lblNickname = new Desktop.Skinning.SkinnedLabel();
            this.lblNickChar = new Desktop.Skinning.SkinnedLabel();
            this.recNickChar = new Desktop.CommonControls.RecordField();
            this.txtNickname = new Desktop.Skinning.SkinnedTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxArrow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valLocation)).BeginInit();
            this.groupBoxState.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valWeight)).BeginInit();
            this.groupBoxBubble.SuspendLayout();
            this.groupBoxForfeit.SuspendLayout();
            this.groupBoxNickname.SuspendLayout();
            this.tsNick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valNickWeight)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxArrow
            // 
            this.groupBoxArrow.BackColor = System.Drawing.Color.White;
            this.groupBoxArrow.Controls.Add(this.valLocation);
            this.groupBoxArrow.Controls.Add(this.lblArrowLocation);
            this.groupBoxArrow.Controls.Add(this.cboDirection);
            this.groupBoxArrow.Controls.Add(this.lblArrowDirection);
            this.groupBoxArrow.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.groupBoxArrow.Image = null;
            this.groupBoxArrow.Location = new System.Drawing.Point(3, 90);
            this.groupBoxArrow.Name = "groupBoxArrow";
            this.groupBoxArrow.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.groupBoxArrow.ShowIndicatorBar = false;
            this.groupBoxArrow.Size = new System.Drawing.Size(200, 79);
            this.groupBoxArrow.TabIndex = 0;
            this.groupBoxArrow.TabStop = false;
            this.groupBoxArrow.Text = "Arrow";
            // 
            // valLocation
            // 
            this.valLocation.BackColor = System.Drawing.Color.White;
            this.valLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.valLocation.ForeColor = System.Drawing.Color.Black;
            this.valLocation.Location = new System.Drawing.Point(80, 51);
            this.valLocation.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.valLocation.Minimum = new decimal(new int[] {
            100000,
            0,
            0,
            -2147483648});
            this.valLocation.Name = "valLocation";
            this.valLocation.Size = new System.Drawing.Size(114, 20);
            this.valLocation.TabIndex = 11;
            this.valLocation.ValueChanged += new System.EventHandler(this.valLocation_ValueChanged);
            // 
            // lblArrowLocation
            // 
            this.lblArrowLocation.AutoSize = true;
            this.lblArrowLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblArrowLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblArrowLocation.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblArrowLocation.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblArrowLocation.Location = new System.Drawing.Point(6, 54);
            this.lblArrowLocation.Name = "lblArrowLocation";
            this.lblArrowLocation.Size = new System.Drawing.Size(68, 13);
            this.lblArrowLocation.TabIndex = 2;
            this.lblArrowLocation.Text = "Location (%):";
            // 
            // cboDirection
            // 
            this.cboDirection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboDirection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboDirection.BackColor = System.Drawing.Color.White;
            this.cboDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDirection.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboDirection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboDirection.FormattingEnabled = true;
            this.cboDirection.KeyMember = null;
            this.cboDirection.Location = new System.Drawing.Point(80, 24);
            this.cboDirection.Name = "cboDirection";
            this.cboDirection.SelectedIndex = -1;
            this.cboDirection.SelectedItem = null;
            this.cboDirection.Size = new System.Drawing.Size(114, 21);
            this.cboDirection.Sorted = false;
            this.cboDirection.TabIndex = 10;
            // 
            // lblArrowDirection
            // 
            this.lblArrowDirection.AutoSize = true;
            this.lblArrowDirection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblArrowDirection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblArrowDirection.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblArrowDirection.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblArrowDirection.Location = new System.Drawing.Point(6, 27);
            this.lblArrowDirection.Name = "lblArrowDirection";
            this.lblArrowDirection.Size = new System.Drawing.Size(52, 13);
            this.lblArrowDirection.TabIndex = 0;
            this.lblArrowDirection.Text = "Direction:";
            // 
            // groupBoxState
            // 
            this.groupBoxState.BackColor = System.Drawing.Color.White;
            this.groupBoxState.Controls.Add(this.chkResetLabel);
            this.groupBoxState.Controls.Add(this.txtLabel);
            this.groupBoxState.Controls.Add(this.cboAI);
            this.groupBoxState.Controls.Add(this.lblStateLabel);
            this.groupBoxState.Controls.Add(this.lblStateAI);
            this.groupBoxState.Controls.Add(this.cboSize);
            this.groupBoxState.Controls.Add(this.lblStateSize);
            this.groupBoxState.Controls.Add(this.cboGender);
            this.groupBoxState.Controls.Add(this.lblStateGender);
            this.groupBoxState.Controls.Add(this.chkResetAI);
            this.groupBoxState.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.groupBoxState.Image = null;
            this.groupBoxState.Location = new System.Drawing.Point(3, 6);
            this.groupBoxState.Name = "groupBoxState";
            this.groupBoxState.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.groupBoxState.ShowIndicatorBar = false;
            this.groupBoxState.Size = new System.Drawing.Size(389, 78);
            this.groupBoxState.TabIndex = 1;
            this.groupBoxState.TabStop = false;
            this.groupBoxState.Text = "Change state";
            // 
            // chkResetLabel
            // 
            this.chkResetLabel.AutoSize = true;
            this.chkResetLabel.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkResetLabel.Location = new System.Drawing.Point(329, 53);
            this.chkResetLabel.Name = "chkResetLabel";
            this.chkResetLabel.Size = new System.Drawing.Size(54, 17);
            this.chkResetLabel.TabIndex = 9;
            this.chkResetLabel.Text = "Reset";
            this.chkResetLabel.UseVisualStyleBackColor = true;
            this.chkResetLabel.CheckedChanged += new System.EventHandler(this.chkResetLabel_CheckedChanged);
            // 
            // txtLabel
            // 
            this.txtLabel.BackColor = System.Drawing.Color.White;
            this.txtLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtLabel.ForeColor = System.Drawing.Color.Black;
            this.txtLabel.Location = new System.Drawing.Point(240, 51);
            this.txtLabel.Name = "txtLabel";
            this.txtLabel.Size = new System.Drawing.Size(84, 20);
            this.txtLabel.TabIndex = 8;
            // 
            // cboAI
            // 
            this.cboAI.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboAI.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboAI.BackColor = System.Drawing.Color.White;
            this.cboAI.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAI.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboAI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboAI.FormattingEnabled = true;
            this.cboAI.KeyMember = null;
            this.cboAI.Location = new System.Drawing.Point(241, 24);
            this.cboAI.Name = "cboAI";
            this.cboAI.SelectedIndex = -1;
            this.cboAI.SelectedItem = null;
            this.cboAI.Size = new System.Drawing.Size(83, 21);
            this.cboAI.Sorted = false;
            this.cboAI.TabIndex = 3;
            // 
            // lblStateLabel
            // 
            this.lblStateLabel.AutoSize = true;
            this.lblStateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblStateLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblStateLabel.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblStateLabel.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblStateLabel.Location = new System.Drawing.Point(172, 54);
            this.lblStateLabel.Name = "lblStateLabel";
            this.lblStateLabel.Size = new System.Drawing.Size(36, 13);
            this.lblStateLabel.TabIndex = 7;
            this.lblStateLabel.Text = "Label:";
            // 
            // lblStateAI
            // 
            this.lblStateAI.AutoSize = true;
            this.lblStateAI.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblStateAI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblStateAI.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblStateAI.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblStateAI.Location = new System.Drawing.Point(172, 27);
            this.lblStateAI.Name = "lblStateAI";
            this.lblStateAI.Size = new System.Drawing.Size(64, 13);
            this.lblStateAI.TabIndex = 2;
            this.lblStateAI.Text = "Intelligence:";
            // 
            // cboSize
            // 
            this.cboSize.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboSize.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboSize.BackColor = System.Drawing.Color.White;
            this.cboSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSize.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboSize.FormattingEnabled = true;
            this.cboSize.KeyMember = null;
            this.cboSize.Location = new System.Drawing.Point(80, 51);
            this.cboSize.Name = "cboSize";
            this.cboSize.SelectedIndex = -1;
            this.cboSize.SelectedItem = null;
            this.cboSize.Size = new System.Drawing.Size(80, 21);
            this.cboSize.Sorted = false;
            this.cboSize.TabIndex = 6;
            // 
            // lblStateSize
            // 
            this.lblStateSize.AutoSize = true;
            this.lblStateSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblStateSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblStateSize.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblStateSize.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblStateSize.Location = new System.Drawing.Point(6, 54);
            this.lblStateSize.Name = "lblStateSize";
            this.lblStateSize.Size = new System.Drawing.Size(30, 13);
            this.lblStateSize.TabIndex = 5;
            this.lblStateSize.Text = "Size:";
            // 
            // cboGender
            // 
            this.cboGender.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboGender.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboGender.BackColor = System.Drawing.Color.White;
            this.cboGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGender.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboGender.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboGender.FormattingEnabled = true;
            this.cboGender.KeyMember = null;
            this.cboGender.Location = new System.Drawing.Point(80, 24);
            this.cboGender.Name = "cboGender";
            this.cboGender.SelectedIndex = -1;
            this.cboGender.SelectedItem = null;
            this.cboGender.Size = new System.Drawing.Size(80, 21);
            this.cboGender.Sorted = false;
            this.cboGender.TabIndex = 1;
            // 
            // lblStateGender
            // 
            this.lblStateGender.AutoSize = true;
            this.lblStateGender.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblStateGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblStateGender.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblStateGender.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblStateGender.Location = new System.Drawing.Point(6, 27);
            this.lblStateGender.Name = "lblStateGender";
            this.lblStateGender.Size = new System.Drawing.Size(45, 13);
            this.lblStateGender.TabIndex = 0;
            this.lblStateGender.Text = "Gender:";
            // 
            // chkResetAI
            // 
            this.chkResetAI.AutoSize = true;
            this.chkResetAI.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkResetAI.Location = new System.Drawing.Point(329, 26);
            this.chkResetAI.Name = "chkResetAI";
            this.chkResetAI.Size = new System.Drawing.Size(54, 17);
            this.chkResetAI.TabIndex = 4;
            this.chkResetAI.Text = "Reset";
            this.chkResetAI.UseVisualStyleBackColor = true;
            this.chkResetAI.CheckedChanged += new System.EventHandler(this.chkResetAI_CheckedChanged);
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblWeight.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblWeight.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblWeight.Location = new System.Drawing.Point(9, 426);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(67, 13);
            this.lblWeight.TabIndex = 2;
            this.lblWeight.Text = "Line Weight:";
            // 
            // valWeight
            // 
            this.valWeight.BackColor = System.Drawing.Color.White;
            this.valWeight.DecimalPlaces = 2;
            this.valWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.valWeight.ForeColor = System.Drawing.Color.Black;
            this.valWeight.Location = new System.Drawing.Point(83, 423);
            this.valWeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.valWeight.Name = "valWeight";
            this.valWeight.Size = new System.Drawing.Size(60, 20);
            this.valWeight.TabIndex = 4;
            this.valWeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBoxBubble
            // 
            this.groupBoxBubble.BackColor = System.Drawing.Color.White;
            this.groupBoxBubble.Controls.Add(this.cboFontSize);
            this.groupBoxBubble.Controls.Add(this.lblBubbleTextSize);
            this.groupBoxBubble.Controls.Add(this.chkLayer);
            this.groupBoxBubble.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.groupBoxBubble.Image = null;
            this.groupBoxBubble.Location = new System.Drawing.Point(209, 90);
            this.groupBoxBubble.Name = "groupBoxBubble";
            this.groupBoxBubble.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.groupBoxBubble.ShowIndicatorBar = false;
            this.groupBoxBubble.Size = new System.Drawing.Size(183, 79);
            this.groupBoxBubble.TabIndex = 12;
            this.groupBoxBubble.TabStop = false;
            this.groupBoxBubble.Text = "Speech Bubble";
            // 
            // cboFontSize
            // 
            this.cboFontSize.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboFontSize.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboFontSize.BackColor = System.Drawing.Color.White;
            this.cboFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFontSize.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboFontSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboFontSize.FormattingEnabled = true;
            this.cboFontSize.KeyMember = null;
            this.cboFontSize.Location = new System.Drawing.Point(63, 49);
            this.cboFontSize.Name = "cboFontSize";
            this.cboFontSize.SelectedIndex = -1;
            this.cboFontSize.SelectedItem = null;
            this.cboFontSize.Size = new System.Drawing.Size(114, 21);
            this.cboFontSize.Sorted = false;
            this.cboFontSize.TabIndex = 12;
            this.cboFontSize.SelectedIndexChanged += new System.EventHandler(this.cboFontSize_SelectedIndexChanged);
            // 
            // lblBubbleTextSize
            // 
            this.lblBubbleTextSize.AutoSize = true;
            this.lblBubbleTextSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblBubbleTextSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblBubbleTextSize.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblBubbleTextSize.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblBubbleTextSize.Location = new System.Drawing.Point(5, 53);
            this.lblBubbleTextSize.Name = "lblBubbleTextSize";
            this.lblBubbleTextSize.Size = new System.Drawing.Size(52, 13);
            this.lblBubbleTextSize.TabIndex = 0;
            this.lblBubbleTextSize.Text = "Text size:";
            // 
            // chkLayer
            // 
            this.chkLayer.AutoSize = true;
            this.chkLayer.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkLayer.Location = new System.Drawing.Point(6, 26);
            this.chkLayer.Name = "chkLayer";
            this.chkLayer.Size = new System.Drawing.Size(115, 17);
            this.chkLayer.TabIndex = 0;
            this.chkLayer.Text = "Display over image";
            this.chkLayer.UseVisualStyleBackColor = true;
            this.chkLayer.CheckedChanged += new System.EventHandler(this.chkLayer_CheckedChanged);
            // 
            // groupBoxForfeit
            // 
            this.groupBoxForfeit.BackColor = System.Drawing.Color.White;
            this.groupBoxForfeit.Controls.Add(this.txtValue);
            this.groupBoxForfeit.Controls.Add(this.lblFOHeavy);
            this.groupBoxForfeit.Controls.Add(this.cboHeavy);
            this.groupBoxForfeit.Controls.Add(this.lblFOValue);
            this.groupBoxForfeit.Controls.Add(this.chkResetHeavy);
            this.groupBoxForfeit.Controls.Add(this.cboOp);
            this.groupBoxForfeit.Controls.Add(this.cboAttr);
            this.groupBoxForfeit.Controls.Add(this.lblFOOperation);
            this.groupBoxForfeit.Controls.Add(this.lblFOAttribute);
            this.groupBoxForfeit.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.groupBoxForfeit.Image = null;
            this.groupBoxForfeit.Location = new System.Drawing.Point(3, 329);
            this.groupBoxForfeit.Name = "groupBoxForfeit";
            this.groupBoxForfeit.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.groupBoxForfeit.ShowIndicatorBar = false;
            this.groupBoxForfeit.Size = new System.Drawing.Size(389, 85);
            this.groupBoxForfeit.TabIndex = 13;
            this.groupBoxForfeit.TabStop = false;
            this.groupBoxForfeit.Text = "Forfeit Operations";
            // 
            // txtValue
            // 
            this.txtValue.BackColor = System.Drawing.Color.White;
            this.txtValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtValue.ForeColor = System.Drawing.Color.Black;
            this.txtValue.Location = new System.Drawing.Point(80, 56);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(80, 20);
            this.txtValue.TabIndex = 19;
            // 
            // lblFOHeavy
            // 
            this.lblFOHeavy.AutoSize = true;
            this.lblFOHeavy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblFOHeavy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblFOHeavy.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblFOHeavy.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblFOHeavy.Location = new System.Drawing.Point(172, 59);
            this.lblFOHeavy.Name = "lblFOHeavy";
            this.lblFOHeavy.Size = new System.Drawing.Size(41, 13);
            this.lblFOHeavy.TabIndex = 18;
            this.lblFOHeavy.Text = "Heavy:";
            // 
            // cboHeavy
            // 
            this.cboHeavy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboHeavy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboHeavy.BackColor = System.Drawing.Color.White;
            this.cboHeavy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHeavy.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboHeavy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboHeavy.FormattingEnabled = true;
            this.cboHeavy.KeyMember = null;
            this.cboHeavy.Location = new System.Drawing.Point(240, 56);
            this.cboHeavy.Name = "cboHeavy";
            this.cboHeavy.SelectedIndex = -1;
            this.cboHeavy.SelectedItem = null;
            this.cboHeavy.Size = new System.Drawing.Size(84, 21);
            this.cboHeavy.Sorted = false;
            this.cboHeavy.TabIndex = 17;
            // 
            // lblFOValue
            // 
            this.lblFOValue.AutoSize = true;
            this.lblFOValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblFOValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblFOValue.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblFOValue.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblFOValue.Location = new System.Drawing.Point(6, 59);
            this.lblFOValue.Name = "lblFOValue";
            this.lblFOValue.Size = new System.Drawing.Size(37, 13);
            this.lblFOValue.TabIndex = 16;
            this.lblFOValue.Text = "Value:";
            // 
            // chkResetHeavy
            // 
            this.chkResetHeavy.AutoSize = true;
            this.chkResetHeavy.FieldType = Desktop.Skinning.SkinnedFieldType.Primary;
            this.chkResetHeavy.Location = new System.Drawing.Point(329, 58);
            this.chkResetHeavy.Name = "chkResetHeavy";
            this.chkResetHeavy.Size = new System.Drawing.Size(54, 17);
            this.chkResetHeavy.TabIndex = 15;
            this.chkResetHeavy.Text = "Reset";
            this.chkResetHeavy.UseVisualStyleBackColor = true;
            this.chkResetHeavy.CheckedChanged += new System.EventHandler(this.chkResetHeavy_CheckedChanged);
            // 
            // cboOp
            // 
            this.cboOp.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboOp.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboOp.BackColor = System.Drawing.Color.White;
            this.cboOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOp.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboOp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboOp.FormattingEnabled = true;
            this.cboOp.KeyMember = null;
            this.cboOp.Location = new System.Drawing.Point(240, 29);
            this.cboOp.Name = "cboOp";
            this.cboOp.SelectedIndex = -1;
            this.cboOp.SelectedItem = null;
            this.cboOp.Size = new System.Drawing.Size(84, 21);
            this.cboOp.Sorted = false;
            this.cboOp.TabIndex = 14;
            this.cboOp.SelectedIndexChanged += new System.EventHandler(this.cboOp_SelectedIndexChanged);
            // 
            // cboAttr
            // 
            this.cboAttr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboAttr.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboAttr.BackColor = System.Drawing.Color.White;
            this.cboAttr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAttr.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboAttr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboAttr.FormattingEnabled = true;
            this.cboAttr.KeyMember = null;
            this.cboAttr.Location = new System.Drawing.Point(80, 29);
            this.cboAttr.Name = "cboAttr";
            this.cboAttr.SelectedIndex = -1;
            this.cboAttr.SelectedItem = null;
            this.cboAttr.Size = new System.Drawing.Size(80, 21);
            this.cboAttr.Sorted = false;
            this.cboAttr.TabIndex = 13;
            this.cboAttr.SelectedIndexChanged += new System.EventHandler(this.cboAttr_SelectedIndexChanged);
            // 
            // lblFOOperation
            // 
            this.lblFOOperation.AutoSize = true;
            this.lblFOOperation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblFOOperation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblFOOperation.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblFOOperation.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblFOOperation.Location = new System.Drawing.Point(172, 35);
            this.lblFOOperation.Name = "lblFOOperation";
            this.lblFOOperation.Size = new System.Drawing.Size(56, 13);
            this.lblFOOperation.TabIndex = 2;
            this.lblFOOperation.Text = "Operation:";
            // 
            // lblFOAttribute
            // 
            this.lblFOAttribute.AutoSize = true;
            this.lblFOAttribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblFOAttribute.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblFOAttribute.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblFOAttribute.Level = Desktop.Skinning.SkinnedLabelLevel.Label;
            this.lblFOAttribute.Location = new System.Drawing.Point(6, 35);
            this.lblFOAttribute.Name = "lblFOAttribute";
            this.lblFOAttribute.Size = new System.Drawing.Size(49, 13);
            this.lblFOAttribute.TabIndex = 1;
            this.lblFOAttribute.Text = "Attribute:";
            // 
            // groupBoxNickname
            // 
            this.groupBoxNickname.BackColor = System.Drawing.Color.White;
            this.groupBoxNickname.Controls.Add(this.lstNick);
            this.groupBoxNickname.Controls.Add(this.tsNick);
            this.groupBoxNickname.Controls.Add(this.lblNickWeight);
            this.groupBoxNickname.Controls.Add(this.lblNickOp);
            this.groupBoxNickname.Controls.Add(this.valNickWeight);
            this.groupBoxNickname.Controls.Add(this.cboNickOp);
            this.groupBoxNickname.Controls.Add(this.lblNickname);
            this.groupBoxNickname.Controls.Add(this.lblNickChar);
            this.groupBoxNickname.Controls.Add(this.recNickChar);
            this.groupBoxNickname.Controls.Add(this.txtNickname);
            this.groupBoxNickname.Highlight = Desktop.Skinning.SkinnedHighlight.Heading;
            this.groupBoxNickname.Image = null;
            this.groupBoxNickname.Location = new System.Drawing.Point(3, 175);
            this.groupBoxNickname.Name = "groupBoxNickname";
            this.groupBoxNickname.PanelType = Desktop.Skinning.SkinnedBackgroundType.Surface;
            this.groupBoxNickname.ShowIndicatorBar = false;
            this.groupBoxNickname.Size = new System.Drawing.Size(389, 148);
            this.groupBoxNickname.TabIndex = 14;
            this.groupBoxNickname.TabStop = false;
            this.groupBoxNickname.Text = "Nickname Operations";
            // 
            // lstNick
            // 
            this.lstNick.BackColor = System.Drawing.Color.White;
            this.lstNick.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lstNick.ForeColor = System.Drawing.Color.Black;
            this.lstNick.FormattingEnabled = true;
            this.lstNick.Location = new System.Drawing.Point(9, 34);
            this.lstNick.Name = "lstNick";
            this.lstNick.Size = new System.Drawing.Size(185, 108);
            this.lstNick.TabIndex = 10;
            this.lstNick.SelectedIndexChanged += new System.EventHandler(this.lstNick_SelectedIndexChanged);
            // 
            // tsNick
            // 
            this.tsNick.Dock = System.Windows.Forms.DockStyle.None;
            this.tsNick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNickAdd,
            this.tsNickRemove,
            this.tsNickMoveUp,
            this.tsNickMoveDown});
            this.tsNick.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.tsNick.Location = new System.Drawing.Point(211, 3);
            this.tsNick.Name = "tsNick";
            this.tsNick.Size = new System.Drawing.Size(93, 23);
            this.tsNick.TabIndex = 9;
            // 
            // tsNickAdd
            // 
            this.tsNickAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsNickAdd.Image = global::SPNATI_Character_Editor.Properties.Resources.Add;
            this.tsNickAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNickAdd.Name = "tsNickAdd";
            this.tsNickAdd.Size = new System.Drawing.Size(23, 20);
            this.tsNickAdd.Click += new System.EventHandler(this.tsNickAdd_Click);
            // 
            // tsNickRemove
            // 
            this.tsNickRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsNickRemove.Image = global::SPNATI_Character_Editor.Properties.Resources.Remove;
            this.tsNickRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNickRemove.Name = "tsNickRemove";
            this.tsNickRemove.Size = new System.Drawing.Size(23, 20);
            this.tsNickRemove.Click += new System.EventHandler(this.tsNickRemove_Click);
            // 
            // tsNickMoveUp
            // 
            this.tsNickMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsNickMoveUp.Image = global::SPNATI_Character_Editor.Properties.Resources.UpArrow;
            this.tsNickMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNickMoveUp.Name = "tsNickMoveUp";
            this.tsNickMoveUp.Size = new System.Drawing.Size(23, 20);
            this.tsNickMoveUp.Click += new System.EventHandler(this.tsNickMoveUp_Click);
            // 
            // tsNickMoveDown
            // 
            this.tsNickMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsNickMoveDown.Image = global::SPNATI_Character_Editor.Properties.Resources.DownArrow;
            this.tsNickMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNickMoveDown.Name = "tsNickMoveDown";
            this.tsNickMoveDown.Size = new System.Drawing.Size(23, 20);
            this.tsNickMoveDown.Click += new System.EventHandler(this.tsNickMoveDown_Click);
            // 
            // lblNickWeight
            // 
            this.lblNickWeight.AutoSize = true;
            this.lblNickWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblNickWeight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNickWeight.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblNickWeight.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblNickWeight.Location = new System.Drawing.Point(211, 115);
            this.lblNickWeight.Name = "lblNickWeight";
            this.lblNickWeight.Size = new System.Drawing.Size(95, 13);
            this.lblNickWeight.TabIndex = 7;
            this.lblNickWeight.Text = "Nickname Weight:";
            // 
            // lblNickOp
            // 
            this.lblNickOp.AutoSize = true;
            this.lblNickOp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblNickOp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNickOp.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblNickOp.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblNickOp.Location = new System.Drawing.Point(211, 63);
            this.lblNickOp.Name = "lblNickOp";
            this.lblNickOp.Size = new System.Drawing.Size(56, 13);
            this.lblNickOp.TabIndex = 6;
            this.lblNickOp.Text = "Operation:";
            this.toolTip1.SetToolTip(this.lblNickOp, "Set as the only nickname for the given character (Weight=1);\nAdd a new nickname or increase weight of an existing one;\nDecrease a nickname's weight;\nSet a nickname's weight to a new value.");
            // 
            // valNickWeight
            // 
            this.valNickWeight.BackColor = System.Drawing.Color.White;
            this.valNickWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.valNickWeight.ForeColor = System.Drawing.Color.Black;
            this.valNickWeight.Location = new System.Drawing.Point(323, 113);
            this.valNickWeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.valNickWeight.Name = "valNickWeight";
            this.valNickWeight.Size = new System.Drawing.Size(60, 20);
            this.valNickWeight.TabIndex = 5;
            this.valNickWeight.ValueChanged += new System.EventHandler(this.valNickWeight_ValueChanged);
            // 
            // cboNickOp
            // 
            this.cboNickOp.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboNickOp.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.cboNickOp.BackColor = System.Drawing.Color.White;
            this.cboNickOp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboNickOp.FieldType = Desktop.Skinning.SkinnedFieldType.Surface;
            this.cboNickOp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.cboNickOp.KeyMember = null;
            this.cboNickOp.Location = new System.Drawing.Point(269, 60);
            this.cboNickOp.Name = "cboNickOp";
            this.cboNickOp.SelectedIndex = -1;
            this.cboNickOp.SelectedItem = null;
            this.cboNickOp.Size = new System.Drawing.Size(114, 21);
            this.cboNickOp.Sorted = false;
            this.cboNickOp.TabIndex = 4;
            this.cboNickOp.TextChanged += new System.EventHandler(this.cboNickOp_TextChanged);
            // 
            // lblNickname
            // 
            this.lblNickname.AutoSize = true;
            this.lblNickname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblNickname.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNickname.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblNickname.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblNickname.Location = new System.Drawing.Point(211, 90);
            this.lblNickname.Name = "lblNickname";
            this.lblNickname.Size = new System.Drawing.Size(58, 13);
            this.lblNickname.TabIndex = 3;
            this.lblNickname.Text = "Nickname:";
            // 
            // lblNickChar
            // 
            this.lblNickChar.AutoSize = true;
            this.lblNickChar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblNickChar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblNickChar.Highlight = Desktop.Skinning.SkinnedHighlight.Normal;
            this.lblNickChar.Level = Desktop.Skinning.SkinnedLabelLevel.Normal;
            this.lblNickChar.Location = new System.Drawing.Point(211, 37);
            this.lblNickChar.Name = "lblNickChar";
            this.lblNickChar.Size = new System.Drawing.Size(56, 13);
            this.lblNickChar.TabIndex = 2;
            this.lblNickChar.Text = "Character:";
            // 
            // recNickChar
            // 
            this.recNickChar.AllowCreate = false;
            this.recNickChar.Location = new System.Drawing.Point(269, 34);
            this.recNickChar.Name = "recNickChar";
            this.recNickChar.PlaceholderText = null;
            this.recNickChar.Record = null;
            this.recNickChar.RecordContext = null;
            this.recNickChar.RecordFilter = null;
            this.recNickChar.RecordKey = null;
            this.recNickChar.RecordType = null;
            this.recNickChar.Size = new System.Drawing.Size(114, 20);
            this.recNickChar.TabIndex = 1;
            this.recNickChar.UseAutoComplete = false;
            this.recNickChar.RecordChanged += new System.EventHandler<Desktop.CommonControls.RecordEventArgs>(this.recNickChar_RecordChanged);
            // 
            // txtNickname
            // 
            this.txtNickname.BackColor = System.Drawing.Color.White;
            this.txtNickname.ForeColor = System.Drawing.Color.Black;
            this.txtNickname.Location = new System.Drawing.Point(269, 87);
            this.txtNickname.Name = "txtNickname";
            this.txtNickname.Size = new System.Drawing.Size(114, 20);
            this.txtNickname.TabIndex = 0;
            this.txtNickname.TextChanged += new System.EventHandler(this.txtNickname_TextChanged);
            // 
            // DialogueAdvancedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxNickname);
            this.Controls.Add(this.groupBoxForfeit);
            this.Controls.Add(this.groupBoxBubble);
            this.Controls.Add(this.valWeight);
            this.Controls.Add(this.lblWeight);
            this.Controls.Add(this.groupBoxState);
            this.Controls.Add(this.groupBoxArrow);
            this.Name = "DialogueAdvancedControl";
            this.Size = new System.Drawing.Size(395, 450);
            this.groupBoxArrow.ResumeLayout(false);
            this.groupBoxArrow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valLocation)).EndInit();
            this.groupBoxState.ResumeLayout(false);
            this.groupBoxState.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valWeight)).EndInit();
            this.groupBoxBubble.ResumeLayout(false);
            this.groupBoxBubble.PerformLayout();
            this.groupBoxForfeit.ResumeLayout(false);
            this.groupBoxForfeit.PerformLayout();
            this.groupBoxNickname.ResumeLayout(false);
            this.groupBoxNickname.PerformLayout();
            this.tsNick.ResumeLayout(false);
            this.tsNick.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valNickWeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private Desktop.Skinning.SkinnedGroupBox groupBoxArrow;
        private Desktop.Skinning.SkinnedLabel lblArrowLocation;
		private Desktop.Skinning.SkinnedComboBox cboDirection;
		private Desktop.Skinning.SkinnedLabel lblArrowDirection;
		private Desktop.Skinning.SkinnedNumericUpDown valLocation;
		private Desktop.Skinning.SkinnedGroupBox groupBoxState;
		private Desktop.Skinning.SkinnedLabel lblStateSize;
		private Desktop.Skinning.SkinnedComboBox cboGender;
		private Desktop.Skinning.SkinnedLabel lblStateGender;
		private Desktop.Skinning.SkinnedLabel lblWeight;
		private Desktop.Skinning.SkinnedNumericUpDown valWeight;
		private Desktop.Skinning.SkinnedLabel lblStateAI;
		private Desktop.Skinning.SkinnedComboBox cboSize;
		private Desktop.Skinning.SkinnedTextBox txtLabel;
		private Desktop.Skinning.SkinnedComboBox cboAI;
		private Desktop.Skinning.SkinnedLabel lblStateLabel;
		private Desktop.Skinning.SkinnedCheckBox chkResetLabel;
		private Desktop.Skinning.SkinnedCheckBox chkResetAI;
		private Desktop.Skinning.SkinnedGroupBox groupBoxBubble;
		private Desktop.Skinning.SkinnedCheckBox chkLayer;
        private Desktop.Skinning.SkinnedComboBox cboFontSize;
        private Desktop.Skinning.SkinnedLabel lblBubbleTextSize;
        private Desktop.Skinning.SkinnedGroupBox groupBoxForfeit;
        private Desktop.Skinning.SkinnedLabel lblFOAttribute;
        private Desktop.Skinning.SkinnedComboBox cboOp;
        private Desktop.Skinning.SkinnedComboBox cboAttr;
        private Desktop.Skinning.SkinnedLabel lblFOOperation;
        private Desktop.Skinning.SkinnedTextBox txtValue;
        private Desktop.Skinning.SkinnedLabel lblFOHeavy;
        private Desktop.Skinning.SkinnedComboBox cboHeavy;
        private Desktop.Skinning.SkinnedLabel lblFOValue;
        private Desktop.Skinning.SkinnedCheckBox chkResetHeavy;
        private Desktop.Skinning.SkinnedGroupBox groupBoxNickname;
        private Desktop.Skinning.SkinnedTextBox txtNickname;
        private Desktop.CommonControls.RecordField recNickChar;
        private System.Windows.Forms.ToolTip toolTip1;
        private Desktop.Skinning.SkinnedComboBox cboNickOp;
        private Desktop.Skinning.SkinnedLabel lblNickname;
        private Desktop.Skinning.SkinnedLabel lblNickChar;
        private Desktop.Skinning.SkinnedNumericUpDown valNickWeight;
        private Desktop.Skinning.SkinnedLabel lblNickWeight;
        private Desktop.Skinning.SkinnedLabel lblNickOp;
        private System.Windows.Forms.ToolStrip tsNick;
        private System.Windows.Forms.ToolStripButton tsNickAdd;
        private System.Windows.Forms.ToolStripButton tsNickRemove;
        private Desktop.Skinning.SkinnedListBox lstNick;
        private System.Windows.Forms.ToolStripButton tsNickMoveUp;
        private System.Windows.Forms.ToolStripButton tsNickMoveDown;
    }
}
