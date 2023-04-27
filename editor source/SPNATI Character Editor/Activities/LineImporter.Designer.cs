namespace SPNATI_Character_Editor.Activities
{
	partial class LineImporter
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
            this.skinnedTabStrip1 = new Desktop.Skinning.SkinnedTabStrip();
            this.tabs = new Desktop.Skinning.SkinnedTabControl();
            this.tabScratchPad = new System.Windows.Forms.TabPage();
            this.scratchPadControl1 = new SPNATI_Character_Editor.Controls.ScratchPadControl();
            this.tabs.SuspendLayout();
            this.tabScratchPad.SuspendLayout();
            this.SuspendLayout();
            // 
            // skinnedTabStrip1
            // 
            this.skinnedTabStrip1.AddCaption = null;
            this.skinnedTabStrip1.DecorationText = null;
            this.skinnedTabStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.skinnedTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.skinnedTabStrip1.Margin = new System.Windows.Forms.Padding(0);
            this.skinnedTabStrip1.Name = "skinnedTabStrip1";
            this.skinnedTabStrip1.PanelType = Desktop.Skinning.SkinnedBackgroundType.PrimaryLight;
            this.skinnedTabStrip1.ShowAddButton = false;
            this.skinnedTabStrip1.ShowCloseButton = false;
            this.skinnedTabStrip1.Size = new System.Drawing.Size(1157, 23);
            this.skinnedTabStrip1.StartMargin = 5;
            this.skinnedTabStrip1.TabControl = this.tabs;
            this.skinnedTabStrip1.TabIndex = 1;
            this.skinnedTabStrip1.TabMargin = 5;
            this.skinnedTabStrip1.TabPadding = 20;
            this.skinnedTabStrip1.TabSize = 100;
            this.skinnedTabStrip1.TabType = Desktop.Skinning.SkinnedBackgroundType.Background;
            this.skinnedTabStrip1.Text = "skinnedTabStrip1";
            this.skinnedTabStrip1.Vertical = false;
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabScratchPad);
            this.tabs.Location = new System.Drawing.Point(0, 24);
            this.tabs.Margin = new System.Windows.Forms.Padding(0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(1157, 656);
            this.tabs.TabIndex = 2;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabs_SelectedIndexChanged);
            // 
            // tabScratchPad
            // 
            this.tabScratchPad.BackColor = System.Drawing.Color.White;
            this.tabScratchPad.Controls.Add(this.scratchPadControl1);
            this.tabScratchPad.ForeColor = System.Drawing.Color.Black;
            this.tabScratchPad.Location = new System.Drawing.Point(4, 22);
            this.tabScratchPad.Name = "tabScratchPad";
            this.tabScratchPad.Padding = new System.Windows.Forms.Padding(3);
            this.tabScratchPad.Size = new System.Drawing.Size(1149, 630);
            this.tabScratchPad.TabIndex = 1;
            this.tabScratchPad.Text = "Scratch Pad";
            // 
            // scratchPadControl1
            // 
            this.scratchPadControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scratchPadControl1.Location = new System.Drawing.Point(3, 3);
            this.scratchPadControl1.Name = "scratchPadControl1";
            this.scratchPadControl1.Size = new System.Drawing.Size(1143, 624);
            this.scratchPadControl1.TabIndex = 0;
            // 
            // LineImporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.skinnedTabStrip1);
            this.Name = "LineImporter";
            this.Size = new System.Drawing.Size(1157, 680);
            this.tabs.ResumeLayout(false);
            this.tabScratchPad.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private Desktop.Skinning.SkinnedTabStrip skinnedTabStrip1;
		private Desktop.Skinning.SkinnedTabControl tabs;
		private System.Windows.Forms.TabPage tabScratchPad;
		private Controls.ScratchPadControl scratchPadControl1;
	}
}
