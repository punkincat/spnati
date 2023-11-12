using Desktop;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

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
			// Below two were causing weird behavior when accidentally going backwards in versions
			/*if (Config.VersionPredates(version, "v5.2"))
			{
				Convert5_1(character);
				Convert5_2(character);
			}
			if (Config.VersionPredates(version, "v5.2.7"))
			{
				Convert5_2_7(character);
			}*/
			if (Config.VersionPredates(version, "v5.8"))
			{
				Convert5_8(character);
			}
			if (Config.VersionPredates(version, "v6.1"))
			{
				Convert6_1(character);
			}
			// necessary due to bug in pre-6.6
			if (Config.VersionPredates(version, "v6.6"))
			{
				Convert5_8(character);
			}
			if (Config.VersionPredates(version, "v6.7"))
			{
				Convert6_7(character);
			}
			if (Config.VersionPredates(version, "v6.8.2"))
			{
				Convert5_8(character);
				Convert6_7(character);
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

		private static void Convert5_2(Character character)
		{
			HashSet<int> usedOneShots = new HashSet<int>();
			//de-duplicate oneshot IDs, since every case should be unique now
			foreach (Case workingCase in character.Behavior.GetWorkingCases())
			{
				if (workingCase.OneShotId > 0)
				{
					if (usedOneShots.Contains(workingCase.OneShotId))
					{
						workingCase.OneShotId = ++character.Behavior.MaxCaseId;
					}
					else
					{
						usedOneShots.Add(workingCase.OneShotId);
					}
				}
			}
		}

		private static void Convert5_2_7(Character character)
		{
			HashSet<int> usedOneShots = new HashSet<int>();
			//de-duplicate oneshot IDs on lines too
			foreach (Case workingCase in character.Behavior.GetWorkingCases())
			{
				foreach (DialogueLine line in workingCase.Lines)
				{
					if (line.OneShotId > 0)
					{
						if (usedOneShots.Contains(workingCase.OneShotId))
						{
							line.OneShotId = ++character.Behavior.MaxStateId;
						}
						else
						{
							usedOneShots.Add(workingCase.OneShotId);
						}
					}
				}
			}
		}

		/// <summary>
		/// 5.8 conversion: remove Nothing hands and convert old conditions to the 5.2 format
		/// </summary>
		/// <param name="character"></param>
		private static void Convert5_8(Character character)
		{
			int count = 0;
			foreach (Case wc in character.Behavior.GetWorkingCases())
			{
				if (wc.HasLegacyConditions())
				{
					ConvertCase5_2(wc);
					count++;
				}
				foreach (TargetCondition condition in wc.Conditions)
				{
					if (condition.Hand == "Nothing")
					{
						condition.Hand = "High Card";
					}
				}
				ConvertCase5_8(wc, character);
			}
			if (count > 0 && Shell.Instance != null)
			{
				Shell.Instance.SetStatus("Auto-converted conditions for " + count + " cases.");
			}
		}

		public static void ConvertCase(Case theCase, Character character)
		{
			ConvertCase5_2(theCase);
			ConvertCase5_8(theCase, character);
		}

		/// <summary>
		/// Converts a case to use TargetConditions where it previously used direct properties
		/// </summary>
		/// <param name="workingCase"></param>
		public static void ConvertCase5_2(Case workingCase)
		{
			if (workingCase == null || !workingCase.HasLegacyConditions()) { return; }
			int priority = workingCase.GetPriority();
			if (!string.IsNullOrEmpty(workingCase.LegacyTarget) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetStage) ||
				!string.IsNullOrEmpty(workingCase.LegacyFilter) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetHand) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetLayers) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetStatus) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetSaidMarker) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetNotSaidMarker) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetSayingMarker) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetSaying) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetStartingLayers) ||
				!string.IsNullOrEmpty(workingCase.LegacyTargetTimeInStage))
			{
				TargetCondition cond = GetCondition(workingCase, "target", workingCase.LegacyTarget);

				if (!string.IsNullOrEmpty(workingCase.LegacyTarget))
				{
					cond.Character = workingCase.LegacyTarget;
					workingCase.LegacyTarget = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyFilter))
				{
					if (!string.IsNullOrEmpty(cond.FilterTag))
					{
						//already have a filter tag, need a new conditoin
						cond = new TargetCondition();
						cond.Role = "target";
						workingCase.Conditions.Add(cond);
					}
					cond.FilterTag = workingCase.LegacyFilter;
					workingCase.LegacyFilter = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetStage))
				{
					cond.Stage = workingCase.LegacyTargetStage;
					workingCase.LegacyTargetStage = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetHand))
				{
					cond.Hand = workingCase.LegacyTargetHand;
					workingCase.LegacyTargetHand = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetLayers))
				{
					cond.Layers = workingCase.LegacyTargetLayers;
					workingCase.LegacyTargetLayers = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetStatus))
				{
					cond.Status = workingCase.LegacyTargetStatus;
					workingCase.LegacyTargetStatus = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetSaidMarker))
				{
					cond.SaidMarker = workingCase.LegacyTargetSaidMarker;
					workingCase.LegacyTargetSaidMarker = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetNotSaidMarker))
				{
					cond.NotSaidMarker = workingCase.LegacyTargetNotSaidMarker;
					workingCase.LegacyTargetNotSaidMarker = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetSayingMarker))
				{
					cond.SayingMarker = workingCase.LegacyTargetSayingMarker;
					workingCase.LegacyTargetSayingMarker = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetSaying))
				{
					cond.Saying = workingCase.LegacyTargetSaying;
					workingCase.LegacyTargetSaying = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetStartingLayers))
				{
					cond.StartingLayers = workingCase.LegacyTargetStartingLayers;
					workingCase.LegacyTargetStartingLayers = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTargetTimeInStage))
				{
					cond.TimeInStage = workingCase.LegacyTargetTimeInStage;
					workingCase.LegacyTargetTimeInStage = null;
				}
			}
			if (!string.IsNullOrEmpty(workingCase.LegacyConsecutiveLosses))
			{
				TriggerDefinition trigger = TriggerDatabase.GetTrigger(workingCase.Tag);
				if (trigger != null && trigger.HasTarget)
				{
					TargetCondition cond = GetCondition(workingCase, "target", null);
					cond.ConsecutiveLosses = workingCase.LegacyConsecutiveLosses;
					workingCase.LegacyConsecutiveLosses = null;
				}
				else
				{
					TargetCondition cond = GetCondition(workingCase, "self", null);
					cond.ConsecutiveLosses = workingCase.LegacyConsecutiveLosses;
					workingCase.LegacyConsecutiveLosses = null;
				}
			}

			if (!string.IsNullOrEmpty(workingCase.LegacyHasHand) ||
				!string.IsNullOrEmpty(workingCase.LegacySaidMarker) ||
				!string.IsNullOrEmpty(workingCase.LegacyNotSaidMarker) ||
				!string.IsNullOrEmpty(workingCase.LegacyTimeInStage))
			{
				TargetCondition cond = GetCondition(workingCase, "self", null);
				if (!string.IsNullOrEmpty(workingCase.LegacyHasHand))
				{
					cond.Hand = workingCase.LegacyHasHand;
					workingCase.LegacyHasHand = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacySaidMarker))
				{
					cond.SaidMarker = workingCase.LegacySaidMarker;
					workingCase.LegacySaidMarker = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyNotSaidMarker))
				{
					cond.NotSaidMarker = workingCase.LegacyNotSaidMarker;
					workingCase.LegacyNotSaidMarker = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyTimeInStage))
				{
					cond.TimeInStage = workingCase.LegacyTimeInStage;
					workingCase.LegacyTimeInStage = null;
				}
			}

			if (!string.IsNullOrEmpty(workingCase.LegacyAlsoPlaying) ||
				!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingStage) ||
				!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingHand) ||
				!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingSaidMarker) ||
				!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingNotSaidMarker) ||
				!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingSayingMarker) ||
				!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingSaying) ||
				!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingTimeInStage))
			{
				TargetCondition cond = GetCondition(workingCase, "other", workingCase.LegacyAlsoPlaying);

				if (!string.IsNullOrEmpty(workingCase.LegacyAlsoPlaying))
				{
					cond.Character = workingCase.LegacyAlsoPlaying;
					workingCase.LegacyAlsoPlaying = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingStage))
				{
					cond.Stage = workingCase.LegacyAlsoPlayingStage;
					workingCase.LegacyAlsoPlayingStage = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingHand))
				{
					cond.Hand = workingCase.LegacyAlsoPlayingHand;
					workingCase.LegacyAlsoPlayingHand = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingSaidMarker))
				{
					cond.SaidMarker = workingCase.LegacyAlsoPlayingSaidMarker;
					workingCase.LegacyAlsoPlayingSaidMarker = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingNotSaidMarker))
				{
					cond.NotSaidMarker = workingCase.LegacyAlsoPlayingNotSaidMarker;
					workingCase.LegacyAlsoPlayingNotSaidMarker = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingSayingMarker))
				{
					cond.SayingMarker = workingCase.LegacyAlsoPlayingSayingMarker;
					workingCase.LegacyAlsoPlayingSayingMarker = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingSaying))
				{
					cond.Saying = workingCase.LegacyAlsoPlayingSaying;
					workingCase.LegacyAlsoPlayingSaying = null;
				}
				if (!string.IsNullOrEmpty(workingCase.LegacyAlsoPlayingTimeInStage))
				{
					cond.TimeInStage = workingCase.LegacyAlsoPlayingTimeInStage;
					workingCase.LegacyAlsoPlayingTimeInStage = null;
				}
			}

			if (!string.IsNullOrEmpty(workingCase.LegacyTotalFemales))
			{
				TargetCondition filter = new TargetCondition();
				filter.Gender = "female";
				filter.Count = workingCase.LegacyTotalFemales;
				workingCase.Conditions.Add(filter);
				workingCase.LegacyTotalFemales = null;
			}
			if (!string.IsNullOrEmpty(workingCase.LegacyTotalMales))
			{
				TargetCondition filter = new TargetCondition();
				filter.Gender = "male";
				filter.Count = workingCase.LegacyTotalMales;
				workingCase.Conditions.Add(filter);
				workingCase.LegacyTotalMales = null;
			}
			if (!string.IsNullOrEmpty(workingCase.LegacyTotalExposed))
			{
				TargetCondition filter = new TargetCondition();
				filter.Status = "exposed";
				filter.Count = workingCase.LegacyTotalExposed;
				workingCase.Conditions.Add(filter);
				workingCase.LegacyTotalExposed = null;
			}
			if (!string.IsNullOrEmpty(workingCase.LegacyTotalNaked))
			{
				TargetCondition filter = new TargetCondition();
				filter.Status = "naked";
				filter.Count = workingCase.LegacyTotalNaked;
				workingCase.Conditions.Add(filter);
				workingCase.LegacyTotalNaked = null;
			}
			if (!string.IsNullOrEmpty(workingCase.LegacyTotalMasturbating))
			{
				TargetCondition filter = new TargetCondition();
				filter.Status = "masturbating";
				filter.Count = workingCase.LegacyTotalMasturbating;
				workingCase.Conditions.Add(filter);
				workingCase.LegacyTotalMasturbating = null;
			}
			if (!string.IsNullOrEmpty(workingCase.LegacyTotalFinished))
			{
				TargetCondition filter = new TargetCondition();
				filter.Status = "finished";
				filter.Count = workingCase.LegacyTotalFinished;
				workingCase.Conditions.Add(filter);
				workingCase.LegacyTotalFinished = null;
			}
			if (!string.IsNullOrEmpty(workingCase.LegacyTotalPlaying))
			{
				TargetCondition filter = new TargetCondition();
				filter.Status = "alive";
				filter.Count = workingCase.LegacyTotalPlaying;
				workingCase.Conditions.Add(filter);
				workingCase.LegacyTotalPlaying = null;
			}
		}

		private static TargetCondition GetCondition(Case workingCase, string role, string character)
		{
			TargetCondition cond = workingCase.Conditions.Find(c => c.Role == role && (c.Character == character || string.IsNullOrEmpty(character)) && string.IsNullOrEmpty(c.Count));
			if (cond == null)
			{
				cond = new TargetCondition();
				cond.Role = role;
				cond.Character = character;
				workingCase.Conditions.Add(cond);
			}
			return cond;
		}

		/// <summary>
		/// 5.8 conversion from line-level persistent marker to character-wide definitions
		/// </summary>
		/// <param name="workingCase"></param>
		public static void ConvertCase5_8(Case workingCase, Character character)
		{
			foreach (DialogueLine line in workingCase.Lines)
			{
				if (!string.IsNullOrEmpty(line.Marker) && line.IsMarkerPersistent)
				{
					MarkerOperator op;
					bool perTarget;
					string value;
					string name = Marker.ExtractConditionPieces(line.Marker, out op, out value, out perTarget);
					character.Behavior.PersistentMarkers.Add(name);
					line.IsMarkerPersistent = false;
				}
			}
		}

		/// <summary>
		/// 6.1 conversion: convert conditions using tag checks for a character into checking the character directly
		/// </summary>
		/// <param name="character"></param>
		private static void Convert6_1(Character character)
		{
			foreach (Case wc in character.Behavior.GetWorkingCases())
			{
				ConvertCase6_1(wc, character);
			}
		}

		private static void ConvertCase6_1(Case workingCase, Character character)
		{
			for (int i = 0; i < workingCase.Conditions.Count; i++)
			{
				TargetCondition condition = workingCase.Conditions[i];
				if (!string.IsNullOrEmpty(condition.FilterTag))
				{
					if (CharacterDatabase.Get(condition.FilterTag) != null && string.IsNullOrEmpty(condition.Role) && string.IsNullOrEmpty(condition.Character))
					{
						condition.Role = "opp";
						condition.Character = condition.FilterTag;
						condition.FilterTag = null;
						if (condition.Count == "1")
						{
							condition.Count = null;
						}
					}
				}
			}

			foreach (Case alternate in workingCase.AlternativeConditions)
			{
				ConvertCase6_1(alternate, character);
			}
		}

		/// <summary>
		/// 6.7 conversion: rerun 5.8 and 6.1 conversions but on alternative cases
		/// Also converts px and pt to %
		/// </summary>
		/// <param name="character"></param>
		private static void Convert6_7(Character character)
		{
			int count = 0;
			foreach (Case wc in character.Behavior.GetWorkingCases())
			{
				foreach (Case wsc in wc.AlternativeConditions)
				{
					if (wsc.HasLegacyConditions())
					{
						ConvertCase5_2(wsc);
						count++;
					}
					foreach (TargetCondition condition in wsc.Conditions)
					{
						if (condition.Hand == "Nothing")
						{
							condition.Hand = "High Card";
						}
					}
					ConvertCase6_1(wsc, character);
				}
			}
			if (count > 0 && Shell.Instance != null)
			{
				Shell.Instance.SetStatus("Auto-converted conditions for " + count + " cases.");
			}

			if (character.Styles != null)
			{
				foreach (StyleRule rule in character.Styles.Rules)
				{
					foreach (StyleAttribute att in rule.Attributes)
					{
						if (att.Name == "font-size" && (att.Value.EndsWith("px") || att.Value.EndsWith("pt")))
						{
							float num;
							string attval = Regex.Replace(att.Value, @"[^0-9\.]*", "");
							float.TryParse(attval, out num);

							num *= (100 / 11.7f);

							if (att.Value.EndsWith("px"))
							{
								num *= 0.75f;
							}

							if (num == 0)
							{
								num = 100;
							}

							att.Value = System.Math.Floor(num) + "%";
						}
					}
				}
			}
		}
	}
}
