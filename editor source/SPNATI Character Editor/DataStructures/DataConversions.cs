﻿using System.Drawing;
using System.Globalization;
using System.IO;

namespace SPNATI_Character_Editor
{
	public static class DataConversions
	{
		public static void ConvertVersion(Character character)
		{
			string version = character.Version;
			if (Config.VersionPredates(version, "v3.2"))
			{
				Convert3_2(character);
			}
			if (version == "v4.2")
			{
				//fix weight bug from 4.2
				foreach (Case workingCase in character.Behavior.GetWorkingCases())
				{
					foreach (DialogueLine line in workingCase.Lines)
					{
						if (line.Weight == 0)
						{
							line.Weight = 1;
						}
					}
				}
			}
			if (Config.VersionPredates(version, "v5.2"))
			{
				Convert5_1(character);
			}
		}

		private static void Convert3_2(Character character)
		{
			//convert old-style epilogues
			foreach (Epilogue ending in character.Endings)
			{
				if (ending.Screens.Count > 0)
				{
					ConvertScreenBased(character, ending);
				}
				else if (ending.Backgrounds.Count > 0)
				{
					ConvertBackgroundSceneBased(character, ending);
				}
			}
		}

		/// <summary>
		/// Converts old screen-based epilogues to modern format
		/// </summary>
		private static void ConvertScreenBased(Character character, Epilogue ending)
		{
			ending.Scenes.Clear();
			foreach (Screen screen in ending.Screens)
			{
				string image = screen.Image;

				string sceneWidth = null;
				string sceneHeight = null;
				if (!string.IsNullOrEmpty(image))
				{
					if (string.IsNullOrEmpty(ending.GalleryImage))
					{
						ending.GalleryImage = image;
					}
					try
					{
						Image img = new Bitmap(Path.Combine(Config.GetRootDirectory(character), image));
						sceneWidth = img.Width.ToString();
						sceneHeight = img.Height.ToString();
						img.Dispose();
					}
					catch { }
				}

				Scene scene = new Scene()
				{
					Background = image,
					Width = sceneWidth,
					Height = sceneHeight,
				};
				ending.Scenes.Add(scene);

				foreach (EndingText text in screen.Text)
				{
					Directive dir = new Directive("text");
					dir.Width = text.Width;
					dir.X = text.X;
					dir.Y = text.Y;
					dir.Arrow = text.Arrow;
					dir.Text = text.Content;
					scene.Directives.Add(dir);

					scene.Directives.Add(new Directive("pause"));
				}
				if (scene.Directives.Count == 0)
				{
					scene.Directives.Add(new Directive("pause"));
				}
			}

			ending.Backgrounds.Clear();
			ending.Screens.Clear();
		}

		/// <summary>
		/// Converts background/scene-based epilogues to modern format
		/// </summary>
		private static void ConvertBackgroundSceneBased(Character character, Epilogue ending)
		{
			ending.Scenes.Clear();
			foreach (LegacyBackground bg in ending.Backgrounds)
			{
				string image = bg.Image;

				string sceneWidth = "";
				string sceneHeight = "";
				if (!string.IsNullOrEmpty(image))
				{
					try
					{
						Image img = new Bitmap(Path.Combine(Config.GetRootDirectory(character), image));
						sceneWidth = img.Width.ToString();
						sceneHeight = img.Height.ToString();
						img.Dispose();
					}
					catch { }
				}

				foreach (Scene oldScene in bg.Scenes)
				{
					Scene scene = new Scene()
					{
						Background = image,
						Width = sceneWidth,
						Height = sceneHeight,
					};

					scene.X = oldScene.LegacyX;
					scene.Y = oldScene.LegacyY;
					float zoom;
					if (float.TryParse(oldScene.LegacyZoom.Substring(0, oldScene.LegacyZoom.Length - 1), NumberStyles.Float, CultureInfo.InvariantCulture, out zoom))
					{
						scene.Zoom = (zoom / 100).ToString(CultureInfo.InvariantCulture);
					}

					ending.Scenes.Add(scene);

					foreach (EndingSprite sprite in oldScene.LegacySprites)
					{
						Directive dir = new Directive("sprite");
						dir.Width = sprite.Width;
						dir.X = sprite.X;
						dir.Y = sprite.Y;
						dir.Src = sprite.Src;
						dir.Width = sprite.Width;
						dir.Css = sprite.Css;
						scene.Directives.Add(dir);
					}

					foreach (EndingText text in oldScene.LegacyText)
					{
						Directive dir = new Directive("text");
						dir.Width = text.Width;
						dir.X = text.X;
						dir.Y = text.Y;
						dir.Arrow = text.Arrow;
						dir.Text = text.Content;
						dir.Css = text.Css;
						scene.Directives.Add(dir);

						scene.Directives.Add(new Directive("pause"));
					}
					if (scene.Directives.Count == 0)
					{
						scene.Directives.Add(new Directive("pause"));
					}
				}
			}

			ending.Backgrounds.Clear();
			ending.Screens.Clear();
		}

		private static void Convert5_1(Character character)
		{
			//try to link up any old-style situations
			CharacterEditorData editorData = CharacterDatabase.GetEditorData(character);
			foreach (Situation s in editorData.NoteworthySituations)
			{
				if (s.LegacyCase != null)
				{
					//find a matching case
					bool foundLink = false;
					foreach (Case workingCase in character.Behavior.GetWorkingCases())
					{
						if (workingCase.MatchesConditions(s.LegacyCase, false) && workingCase.MatchesStages(s.LegacyCase, true))
						{
							editorData.LinkSituation(s, workingCase);
							foundLink = true;
							break;
						}
					}
					if (!foundLink)
					{

					}
				}
			}
		}

		/// <summary>
		/// Converts a case to use TargetConditions where it previously used direct properties
		/// </summary>
		/// <param name="workingCase"></param>
		public static void ConvertCase5_2(Case workingCase)
		{
			//if (workingCase == null) { return; }
			//if (!string.IsNullOrEmpty(workingCase.Target) ||
			//	!string.IsNullOrEmpty(workingCase.TargetStage) ||
			//	!string.IsNullOrEmpty(workingCase.TargetHand) ||
			//	!string.IsNullOrEmpty(workingCase.TargetLayers) ||
			//	!string.IsNullOrEmpty(workingCase.TargetStatus) ||
			//	!string.IsNullOrEmpty(workingCase.TargetSaidMarker) ||
			//	!string.IsNullOrEmpty(workingCase.TargetNotSaidMarker) ||
			//	!string.IsNullOrEmpty(workingCase.TargetSayingMarker) ||
			//	!string.IsNullOrEmpty(workingCase.TargetSaying) ||
			//	!string.IsNullOrEmpty(workingCase.TargetStartingLayers) ||
			//	!string.IsNullOrEmpty(workingCase.TargetTimeInStage))
			//{
			//	TargetCondition cond = workingCase.Conditions.Find(c => c.Role == "target");
			//	if (cond == null)
			//	{
			//		cond = new TargetCondition();
			//		cond.Role = "target";
			//		workingCase.Conditions.Add(cond);
			//	}
			//}
		}
	}
}
