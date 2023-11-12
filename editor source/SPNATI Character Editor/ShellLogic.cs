using Desktop;
using Desktop.Skinning;
using SPNATI_Character_Editor.Activities;
using SPNATI_Character_Editor.DataStructures;
using SPNATI_Character_Editor.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SPNATI_Character_Editor
{
	public static class ShellLogic
	{
		private static ToolStripMenuItem _tutorialMenu;
		private static string _tutorialUrl;

		public static void Initialize()
		{
			if (!DoInitialSetup())
			{
				Shell.Instance.Close();
				return;
			}
			BuildDefinitions();
			CreateActionBar();
			CreateToolbar();
			LoadFonts();

			Shell.Instance.PostOffice.Subscribe<IActivity>(CoreDesktopMessages.ActivityChanged, OnActivityChanged);
			Shell.Instance.LaunchWorkspace(new LoaderRecord());
			Shell.Instance.Maximize(true);

			Shell.Instance.AutoTickFrequency = Config.AutoSaveInterval * 60000;
			Shell.Instance.AutoTick += Instance_AutoTick;
			Shell.Instance.Version = Config.Version;
			Shell.Instance.VersionClick += Instance_VersionClick;
			Shell.Instance.SubActionLabel = "Submit Bug Report";
			Shell.Instance.SubActionClick += Instance_SubActionClick;

			Config.LoadMacros<Case>("Case");
			Shell.Instance.Description = Config.UserName;

			CharacterGenerator.SetConverter(Config.ImportMethod);
		}

		private static void OnActivityChanged(IActivity activity)
		{
			TutorialAttribute tutorial = activity.GetType().GetCustomAttribute<TutorialAttribute>();
			_tutorialMenu.Visible = tutorial != null;
			_tutorialUrl = tutorial?.Url;
		}

		private static void Instance_SubActionClick(object sender, EventArgs e)
		{
			ErrorReport form = new ErrorReport();
			form.ShowDialog();
		}

		private static void Instance_VersionClick(object sender, EventArgs e)
		{
			new About().ShowDialog();
		}

		private static void Instance_AutoTick(object sender, System.EventArgs e)
		{
			//loop through all open characters and save only those whose author is the current user
			foreach (IWorkspace ws in Shell.Instance.Workspaces)
			{
				Character c = ws.Record as Character;
				if (c != null && !string.IsNullOrEmpty(c.Metadata?.Writer) && c.Metadata.Writer.Contains(Config.UserName))
				{
					Save(true, ws);
				}
			}
		}

		/// <summary>
		/// Builds definition data that isn't currently found in an xml file
		/// </summary>
		private static void BuildDefinitions()
		{
			BuildDirectiveTypes();
			BuildPropertyTypes();
			BuildColorCodes();
		}

		public static void BuildDirectiveTypes()
		{
			//TODO: should these go in an XML file like practically every other definition? Maybe, but the epilogue editor needs code updates to handle new directives either way

			DirectiveProvider provider = new DirectiveProvider();
			DirectiveDefinition def = provider.Create("sprite") as DirectiveDefinition;
			def.Description = "Adds a sprite to the scene.";
			def.SortOrder = 15;
			foreach (string key in new string[] { "id", "src", "layer", "width", "height", "x", "y", "scalex", "scaley", "rotation", "alpha", "pivotx", "pivoty", "marker", "delay", "skewx", "skewy", "clipleft", "cliptop", "clipright", "clipbottom", "clipradius" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("text") as DirectiveDefinition;
			def.Description = "Displays a speech bubble.";
			def.SortOrder = 15;
			foreach (string key in new string[] { "id", "x", "y", "text", "arrow", "width", "alignmentx", "alignmenty", "marker", "delay" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("clear") as DirectiveDefinition;
			def.Description = "Removes a speech bubble.";
			def.SortOrder = 4;
			foreach (string key in new string[] { "id", "marker", "delay" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("clear-all") as DirectiveDefinition;
			foreach (string key in new string[] { "marker", "delay" })
			{
				def.AllowedProperties.Add(key);
			}
			def.SortOrder = 5;
			def.Description = "Removes all speech bubbles.";

			def = provider.Create("move") as DirectiveDefinition;
			def.IsAnimatable = true;
			def.Description = "Moves/rotates/scales a sprite or emitter.";
			def.FilterPropertiesById = true;
			def.SortOrder = 50;
			foreach (string key in new string[] { "id", "src", "x", "y", "scalex", "scaley", "rotation", "alpha", "rate", "time", "delay", "loop", "ease", "tween", "clamp", "iterations", "marker", "skewx", "skewy", "clipleft", "cliptop", "clipright", "clipbottom", "clipradius" })
			{
				def.AllowedProperties.Add(key);
			}
			foreach (string key in new string[] { "time", "src", "x", "y", "scalex", "scaley", "rotation", "alpha" })
			{
				def.RequiredAnimatedProperties.Add(key);
			}

			def = provider.Create("camera") as DirectiveDefinition;
			def.IsAnimatable = true;
			def.Description = "Pans or zooms the camera.";
			def.SortOrder = 50;
			foreach (string key in new string[] { "x", "y", "zoom", "time", "delay", "loop", "ease", "tween", "clamp", "iterations", "marker" })
			{
				def.AllowedProperties.Add(key);
			}
			foreach (string key in new string[] { "time", "x", "y", "zoom" })
			{
				def.RequiredAnimatedProperties.Add(key);
			}

			def = provider.Create("fade") as DirectiveDefinition;
			def.Description = "Fades the overlay to a new color and opacity level.";
			def.IsAnimatable = true;
			def.SortOrder = 50;
			foreach (string key in new string[] { "color", "alpha", "time", "delay", "loop", "ease", "tween", "clamp", "iterations", "marker" })
			{
				def.AllowedProperties.Add(key);
			}
			foreach (string key in new string[] { "time", "color", "alpha" })
			{
				def.RequiredAnimatedProperties.Add(key);
			}

			def = provider.Create("stop") as DirectiveDefinition;
			def.Description = "Stops an animation.";
			def.SortOrder = 4;
			foreach (string key in new string[] { "id", "marker", "delay" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("wait") as DirectiveDefinition;
			def.Description = "Waits for animations to complete.";
			def.SortOrder = 10;
			foreach (string key in new string[] { "marker" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("pause") as DirectiveDefinition;
			def.Description = "Waits for the user to click next.";
			def.SortOrder = 10;
			foreach (string key in new string[] { "marker" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("remove") as DirectiveDefinition;
			def.Description = "Removes a sprite or emitter from the scene.";
			def.SortOrder = 5;
			foreach (string key in new string[] { "id", "marker", "delay" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("emitter") as DirectiveDefinition;
			def.Description = "Adds an object emitter to the scene.";
			def.SortOrder = 15;
			foreach (string key in new string[] { "id", "layer", "src", "rate", "angle", "width", "height", "x", "y", "rotation", "startScaleX", "startScaleY", "endScaleX", "delay",
				"endScaleY", "speed", "accel", "forceX", "forceY", "startColor", "endColor", "startAlpha", "endAlpha", "startRotation", "endRotation", "lifetime", "ease", "ignoreRotation", "marker",
				"startSkewX", "startSkewY", "endSkewX", "endSkewY" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("emit") as DirectiveDefinition;
			def.Description = "Emits an object from an emitter.";
			def.SortOrder = 50;
			foreach (string key in new string[] { "id", "count", "marker", "delay" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("jump") as DirectiveDefinition;
			def.Description = "Jumps to another scene.";
			def.SortOrder = 10;
			foreach (string key in new string[] { "id", "marker", "delay" })
			{
				def.AllowedProperties.Add(key);
			}

			def = provider.Create("prompt") as DirectiveDefinition;
			def.Description = "Displays a multiple choice prompt to the user.";
			def.SortOrder = 10;
			foreach (string key in new string[] { "title", "marker", "delay" })
			{
				def.AllowedProperties.Add(key);
			}
		}

		private static void BuildPropertyTypes()
		{
			PropertyDefinition property;

			property = new PropertyDefinition("X", "X", typeof(float), 10);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("Y", "Y", typeof(float), 15);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("Src", "Source", typeof(string), 0);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("Alpha", "Alpha", typeof(float), 30);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("ScaleX", "Scale (X)", typeof(float), 40);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("ScaleY", "Scale (Y)", typeof(float), 45);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("Rotation", "Rotation", typeof(float), 50);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("SkewX", "Skew (X)", typeof(float), 60);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("SkewY", "Skew (Y)", typeof(float), 65);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("ClipLeft", "Clip Left", typeof(float), 70);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("ClipTop", "Clip Top", typeof(float), 71);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("ClipRight", "Clip Right", typeof(float), 72);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("ClipBottom", "Clip Bottom", typeof(float), 73);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("ClipRadius", "Clip Radius", typeof(float), 74);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("Zoom", "Zoom", typeof(float), 20);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("Color", "Color", typeof(Color), 25);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("Rate", "Rate", typeof(float), 90);
			Definitions.Instance.Add(property);

			property = new PropertyDefinition("Burst", "Burst", typeof(int), 100);
			Definitions.Instance.Add(property);
		}

		private static void BuildColorCodes()
		{
			Definitions.Instance.Add(new ColorCode("(None)", 0, skin => skin.Surface.ForeColor));
			Definitions.Instance.Add(new ColorCode("Red", 1, skin => skin.Red));
			Definitions.Instance.Add(new ColorCode("Orange", 2, skin => skin.Orange));
			Definitions.Instance.Add(new ColorCode("Green", 3, skin => skin.Green));
			Definitions.Instance.Add(new ColorCode("Blue", 4, skin => skin.Blue));
			Definitions.Instance.Add(new ColorCode("Purple", 5, skin => skin.Purple));
			Definitions.Instance.Add(new ColorCode("Pink", 6, skin => skin.Pink));
			Definitions.Instance.Add(new ColorCode("Gray", 7, skin => skin.Gray));
			Definitions.Instance.Add(new ColorCode("Teal", 8, skin => skin.Teal));
			Definitions.Instance.Add(new ColorCode("Turquoise", 9, skin => skin.Turquoise));
			Definitions.Instance.Add(new ColorCode("Pink (Light)", 10, skin => skin.LightPink));
			Definitions.Instance.Add(new ColorCode("Violet", 11, skin => skin.Violet));
			Definitions.Instance.Add(new ColorCode("Blue (Cornflower)", 12, skin => skin.CornflowerBlue));
			Definitions.Instance.Add(new ColorCode("Blue (Light)", 13, skin => skin.LightBlue));
			Definitions.Instance.Add(new ColorCode("Brown", 14, skin => skin.Brown));
			Definitions.Instance.Add(new ColorCode("Yellow", 15, skin => skin.Yellow));
		}

		private static bool DoInitialSetup()
		{
			string appDir = Config.GetString(Settings.GameDirectory);
			if (!string.IsNullOrEmpty(appDir) && !SettingsSetup.VerifyApplicationDirectory(appDir))
			{
				Config.Set(Settings.GameDirectory, null);
			}
			if (string.IsNullOrEmpty(Config.GetString(Settings.GameDirectory)))
			{
				if (OpenInitialSetup() == DialogResult.Cancel)
				{
					ErrorLog.LogError("Unable to launch because setup was cancelled.");
					return false;
				}
			}
			if (string.IsNullOrEmpty(Config.GetString(Settings.GameDirectory)))
			{
				//Not going to play along? Then we'll quit.
				ErrorLog.LogError("SPNATI directory not specified.");
				return false;
			}
			if (string.IsNullOrEmpty(Config.KisekaeDirectory))
			{
				KisekaeSetup setup = new KisekaeSetup();
				setup.ShowDialog();
			}
			return true;
		}

		/// <summary>
		/// Opens the first time settings screen
		/// </summary>
		/// <returns></returns>
		private static DialogResult OpenInitialSetup()
		{
			FirstLaunchSetup setup = new FirstLaunchSetup();
			return setup.ShowDialog();
		}

		private static void CreateActionBar()
		{
			Shell shell = Shell.Instance;

			_tutorialMenu = shell.AddActionItem(Properties.Resources.VideoCamera, "Tutorial", "View Tutorial", ViewTutorial, null);
			_tutorialMenu.Visible = false;
			shell.AddActionItem(Properties.Resources.Settings, "Open Setup", "Open Setup", Setup, null);

			ToolStripMenuItem themes = shell.AddActionMenu(Properties.Resources.Theme, "Change Theme");

			List<Skin> darkThemes = new List<Skin>();

			foreach (Skin skin in SkinManager.Instance.AvailableSkins)
			{
				if (skin.Group == "Dark")
				{
					darkThemes.Add(skin); //add dark themes after light ones
				}
				else
				{
					shell.AddActionItem(skin.GetIcon(), skin.Name, skin.Description, () => ChangeTheme(skin), themes);
				}
			}
			shell.AddActionSeparator(themes);
			foreach (Skin skin in darkThemes)
			{
				shell.AddActionItem(skin.GetIcon(), skin.Name, skin.Description, () => ChangeTheme(skin), themes);
			}
		}

		private static void ChangeTheme(Skin skin)
		{
			SkinManager.Instance.SetSkin(skin);
			Config.Skin = skin.Name;
		}

		private static void CreateToolbar()
		{
			Shell shell = Shell.Instance;

			//File
			ToolStripMenuItem menu = shell.AddToolbarSubmenu("File");
			shell.AddToolbarItem("New Character...", CreateNewCharacter, menu, Keys.None);
			shell.AddToolbarItem("Save", Save, menu, Keys.Control | Keys.S);
			shell.AddToolbarSeparator(menu);
			shell.AddToolbarItem("Settings...", Setup, menu, Keys.None);
			shell.AddToolbarItem("Toggle SFW Mode", ToggleSFWMode, menu, Keys.F12);
			shell.AddToolbarSeparator(menu);
			shell.AddToolbarItem("Exit", Exit, menu, Keys.Alt | Keys.F4);

			//Edit
			menu = shell.AddToolbarSubmenu("Edit");
			shell.AddToolbarItem("Find", Find, menu, Keys.Control | Keys.F);
			shell.AddToolbarItem("Replace", Replace, menu, Keys.Control | Keys.H);
			shell.AddToolbarSeparator();

			//Characters
			shell.AddToolbarItem("Characters...", OpenCharacterSelect, Keys.None);
			shell.AddToolbarItem("Costumes...", OpenCostumeSelect, Keys.None);
			shell.AddToolbarItem("Decks...", OpenDeckSelect, Keys.None);

			//Validate
			menu = shell.AddToolbarSubmenu("Validate");
			shell.AddToolbarItem("All Characters", typeof(ValidationRecord), menu);

			//Tools
			menu = shell.AddToolbarSubmenu("Tools");
			shell.AddToolbarItem("Charts...", typeof(ChartRecord), menu);
			shell.AddToolbarItem("Marker Report...", typeof(MarkerReportRecord), menu);
			shell.AddToolbarItem("Data Analyzer...", typeof(AnalyzerRecord), menu);
			shell.AddToolbarSeparator(menu);
			shell.AddToolbarItem("Configure Game...", ConfigGame, menu, Keys.None);
			shell.AddToolbarItem("Manage Macros...", ManageCaseMacros, menu, Keys.None);
			shell.AddToolbarItem("Manage Dictionary...", typeof(DictionaryRecord), menu);
			shell.AddToolbarItem("Manage Recipes...", typeof(Recipe), GetRecipe, menu);
			shell.AddToolbarItem("Manage Case Templates...", typeof(CaseTemplateRecord), menu);
			shell.AddToolbarSeparator(menu);
			shell.AddToolbarItem("Data Recovery", OpenDataRecovery, menu, Keys.None);
			shell.AddToolbarItem("Fix Kisekae", ResetKisekae, menu, Keys.None);

			//Dev tools
			if (Config.DevMode)
			{
				menu = shell.AddToolbarSubmenu("Dev");
				shell.AddToolbarItem("Themes...", typeof(Skin), menu);
				shell.AddToolbarItem("Clear Extraneous IDs", ClearIDs, menu, Keys.None);
			}

			//Help
			shell.AddToolbarSeparator();
			menu = shell.AddToolbarSubmenu("Help");
			shell.AddToolbarItem("View Help", OpenHelp, menu, Keys.F1);
			shell.AddToolbarItem("Change Log", OpenChangeLog, menu, Keys.None);
			shell.AddToolbarItem("About Character Editor...", OpenAbout, menu, Keys.None);
		}

		private static void LoadFonts()
		{
			Shell.Instance.Fonts = new PrivateFontCollection();
			int fontLength = Properties.Resources.OpenSans_VariableFont.Length;
			byte[] fontdata = Properties.Resources.OpenSans_VariableFont;
			System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);
			Marshal.Copy(fontdata, 0, data, fontLength);
			Shell.Instance.Fonts.AddMemoryFont(data, fontLength);
		}

		private static void CreateNewCharacter()
		{
			NewCharacterPrompt prompt = new NewCharacterPrompt();
			if (prompt.ShowDialog() == DialogResult.OK)
			{
				Character character = prompt.Character;
				MessageBox.Show($"Created {character.FirstName} under folder: opponents/{character.FolderName}");
				Shell.Instance.LaunchWorkspace(character);
			}
		}

		private static void OpenCharacterSelect()
		{
			IRecord record = RecordLookup.DoLookup(typeof(Character), "", true, CharacterDatabase.FilterHuman, null);
			if (record != null)
			{
				Character c = record as Character;
				if (c is CachedCharacter)
				{
					c = CharacterDatabase.Load(c.FolderName);
				}
				Shell.Instance.LaunchWorkspace(c);
			}
		}

		private static void OpenCostumeSelect()
		{
			IRecord record = RecordLookup.DoLookup(typeof(Costume), "", true, CharacterDatabase.FilterDefaultCostume, null);
			if (record != null)
			{
				Shell.Instance.LaunchWorkspace(record as Costume);
			}
		}

		private static void OpenDeckSelect()
		{
			IRecord record = RecordLookup.DoLookup(typeof(Deck), "", true, null, true, null);
			if (record != null)
			{
				Shell.Instance.LaunchWorkspace(record as Deck);
			}
		}

		private static void Save()
		{
			Save(false, Shell.Instance.ActiveWorkspace);
		}

		private static void Save(bool auto, IWorkspace workspace)
		{
			Cursor.Current = Cursors.WaitCursor;
			Shell.Instance.ActiveActivity?.Save();
			workspace?.SendMessage(WorkspaceMessages.Save, auto);
			Cursor.Current = Cursors.Default;
		}

		private static void Exit()
		{
			Shell.Instance.Close();
		}

		private static void Find()
		{
			Shell.Instance.ActiveWorkspace.SendMessage(WorkspaceMessages.Find);
		}

		private static void Replace()
		{
			Shell.Instance.ActiveWorkspace.SendMessage(WorkspaceMessages.Replace);
		}

		private static void OpenHelp()
		{
			HelpForm form = new HelpForm();
			form.Show();
		}

		private static void OpenChangeLog()
		{
			new ChangeLogReview().ShowDialog();
		}

		private static void OpenAbout()
		{
			About form = new About();
			form.ShowDialog();
		}

		/// <summary>
		/// Cleans the kisekae #airversion folder after a bad import corrupted the importer
		/// </summary>
		private static void ResetKisekae()
		{
			if (MessageBox.Show("This will attempt to fix Kisekae when imports are failing. Close kkl.exe before proceeding.", "Fix Kisekae", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
			{
				return;
			}
			string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "kkl", "#airversion");
			if (Directory.Exists(folder))
			{
				DirectoryInfo di = new DirectoryInfo(folder);
				foreach (FileInfo file in di.EnumerateFiles())
				{
					file.Delete();
				}
				foreach (DirectoryInfo dir in di.EnumerateDirectories())
				{
					dir.Delete(true);
				}
			}
			MessageBox.Show("Kisekae data cleaned up. You can restart kkl.exe.");
		}

		private static void OpenDataRecovery()
		{
			DataRecovery recovery = new DataRecovery();
			Character c = GetActiveCharacter();
			recovery.SetCharacter(c);
			if (recovery.ShowDialog() == DialogResult.OK)
			{
				IWorkspace ws = Shell.Instance.GetWorkspace(c);
				if (ws != null)
				{
					Shell.Instance.CloseWorkspace(ws, true);
				}
				Shell.Instance.LaunchWorkspace(recovery.RecoveredCharacter);
			}
		}

		private static void OpenDataRecovery(string name)
		{
			DataRecovery recovery = new DataRecovery();
			recovery.SetCharacter(name);
			if (recovery.ShowDialog() == DialogResult.OK)
			{
				Shell.Instance.LaunchWorkspace(recovery.RecoveredCharacter);
			}
		}

		private static Character GetActiveCharacter()
		{
			IWorkspace ws = Shell.Instance.ActiveWorkspace;
			if (ws != null && ws.Record is Character)
			{
				return ws.Record as Character;
			}
			return null;
		}

		private static void ImportCharacter()
		{
			Character current = GetActiveCharacter();
			if (current == null)
			{
				current = RecordLookup.DoLookup(typeof(Character), "") as Character;
				if (current == null)
				{
					return;
				}
			}
			string dir = Config.GetRootDirectory(current);
			string file = Shell.Instance.ShowOpenFileDialog(dir, "edit-dialogue.txt", "Text files|*.txt|All files|*.*");
			if (!string.IsNullOrEmpty(file))
			{
				FlatFileSerializer.Import(file, current);
				Character c = current;
				CharacterDatabase.Set(c.FolderName, c);

				Shell.Instance.CloseWorkspace(Shell.Instance.ActiveWorkspace, true);
				Shell.Instance.LaunchWorkspace(current);
			}
		}


		private static void ViewTutorial()
		{
			if (_tutorialUrl != null)
			{
				try
				{
					ProcessStartInfo startInfo = new ProcessStartInfo()
					{
						Arguments = $"\"{_tutorialUrl}\"",
						FileName = "explorer.exe"
					};

					Process.Start(startInfo);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private static void Setup()
		{
			SettingsSetup form = new SettingsSetup();
			form.ShowDialog();
		}

		public static void Teardown()
		{
			SpellChecker.Instance.SaveUserDictionary();
			Config.Save();
		}

		public static void RecoverCharacter(string name)
		{
			OpenDataRecovery(name);
		}

		private static void ConfigGame()
		{
			GameConfig form = new GameConfig();
			form.ShowDialog();
		}

		private static void ToggleSFWMode()
		{
			Config.SafeMode = !Config.SafeMode;
			Config.Set(Settings.HideImages, Config.SafeMode);
			Shell.Instance.PostOffice.SendMessage(DesktopMessages.ToggleImages);
			Config.Save();
		}

		private static void ManageCaseMacros()
		{
			MacroManager form = new MacroManager();
			form.SetType(typeof(Case), "Case");
			form.ShowDialog();
			Shell.Instance.PostOffice.SendMessage(DesktopMessages.MacrosUpdated);
		}

		private static IRecord GetRecipe()
		{
			IRecord record = RecordLookup.DoLookup(typeof(Recipe), "", true, FilterCoreRecipes, true, null);
			return record;
		}

		private static bool FilterCoreRecipes(IRecord record)
		{
			Recipe recipe = record as Recipe;
			return Config.DevMode || !recipe.Core;
		}

		private static void ClearIDs()
		{
			Character character = Shell.Instance.ActiveWorkspace.Record as Character;
			if (character != null)
			{
				CharacterEditorData editorData = CharacterDatabase.GetEditorData(character);
				foreach (Case wc in character.Behavior.GetWorkingCases())
				{
					if (wc.Id > 0)
					{
						CaseLabel label = editorData.GetLabel(wc);
						if (label != null)
						{
							continue;
						}
						string note = editorData.GetNote(wc);
						if (!string.IsNullOrEmpty(note))
						{
							continue;
						}
						SituationResponse response = editorData.Responses.Find(r => r.Id == wc.Id);
						if (response != null)
						{
							continue;
						}
						Situation situation = editorData.NoteworthySituations.Find(s => s.Id == wc.Id);
						if (situation != null)
						{
							continue;
						}
						if (editorData.IsHidden(wc))
						{
							continue;
						}
						wc.Id = 0;
					}
				}
			}
		}
	}
}
