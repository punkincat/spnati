﻿using Desktop;
using Desktop.CommonControls.PropertyControls;
using SPNATI_Character_Editor.Controls;
using SPNATI_Character_Editor.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SPNATI_Character_Editor
{
	public class Epilogue
	{
		[Text(DisplayName = "Unlock Hint", GroupOrder = 5, Description = "Hint to the player for how to unlock the ending")]
		[XmlAttribute("hint")]
		public string Hint;

		[ComboBox(DisplayName = "Player Gender", GroupOrder = 2, Description = "Player's required gender to be able to view this ending", Options = new string[] { "any", "female", "male" })]
		[XmlAttribute("gender")]
		public string Gender = "any";

		[NumericRange(DisplayName = "Player Starting Layers", GroupOrder = 10, Description = "Number of layers the player started with in order to unlock this ending", Minimum = 0, Maximum = 8)]
		[XmlAttribute("playerStartingLayers")]
		public string PlayerStartingLayers;

		[XmlAttribute("markers")]
		public string AllMarkers;

		[XmlAttribute("not-markers")]
		public string NotMarkers;

		[XmlAttribute("any-markers")]
		public string AnyMarkers;

		[XmlAttribute("alsoplaying-markers")]
		public string AlsoPlayingAllMarkers;

		[XmlAttribute("alsoplaying-not-markers")]
		public string AlsoPlayingNotMarkers;

		[XmlAttribute("alsoplaying-any-markers")]
		public string AlsoPlayingAnyMarkers;

		[RecordList(DisplayName = "Also Playing", GroupOrder = 8, Description = "Character that must also have been playing in order to unlock this ending", RecordType = typeof(Character), RecordFilter = "FilterRecords", RowHeight = 70)]
		[XmlAttribute("alsoPlaying")]
		public string AlsoPlaying;

		[FileSelect(DisplayName = "Gallery Image", GroupOrder = 3, Description = "Thumbnail that displays in the unlocked epilogue gallery")]
		[XmlAttribute("img")]
		public string GalleryImage;

		[Text(DisplayName = "Title", GroupOrder = 1, Description = "Name that displays when choosing an epilogue in game")]
		[XmlElement("title")]
		public string Title = "New Ending";

		/// <summary>
		/// What editor was used with this
		/// </summary>
		[XmlAttribute("version")]
		public int Version;

		[DefaultValue("online")]
		[ComboBox(DisplayName = "Status", Description = "Where the epilogue is available", GroupOrder = 1, Options = new string[] {
			"online",
			"offline",
			"unlisted",
		})]
		[XmlAttribute("status")]
		public string Status;

		[XmlArray("markers")]
		[XmlArrayItem("marker")]
		public List<MarkerOperation> Markers = new List<MarkerOperation>();

		[DefaultValue(false)]
		[XmlAttribute("allowSceneSkip")]
		[Boolean(DisplayName = "Allow Scene Skip", Description = "Allow the player to skip between scenes", GroupOrder = 15)]
		public bool AllowSceneSkip;

//		[DefaultValue("completion")]
		[ComboBox(DisplayName = "Unlock Condition", Description = "The baseline requirement for unlocking an epilogue. From least to most stringent:\n\r" +
			"completion - The game must have been completed. This is the default behaviour if no other option is chosen.\ncharacter-loss - The epilogue character must lose the game.\noutlast - The human player must either win the game, or at least only lose after the epilogue character.\r" +
			"player-win - The human player must win the entire game.", GroupOrder = 30, Options = new string[] {
			"completion",
			"character-loss",
			"outlast",
			"player-win",
		})]
		[XmlAttribute("unlock")]
		public string Unlock;

		[XmlElement("scene")]
		public List<Scene> Scenes = new List<Scene>();

		#region Legacy properties
		[XmlAttribute("ratio")]
		public string Ratio;

		[XmlNewLine(XmlNewLinePosition.Both)]
		[XmlElement("screen")]
		public List<Screen> Screens = new List<Screen>();

		[XmlElement("background")]
		public List<LegacyBackground> Backgrounds = new List<LegacyBackground>();
		#endregion

		/// <summary>
		/// Gets whether this epilogue has any specialized conditions (excluding gender) that need to be met in order to unlock it
		/// </summary>
		public bool HasSpecialConditions
		{
			get
			{
				return !string.IsNullOrEmpty(AllMarkers) || !string.IsNullOrEmpty(PlayerStartingLayers) || !string.IsNullOrEmpty(NotMarkers) || !string.IsNullOrEmpty(AnyMarkers) ||
					!string.IsNullOrEmpty(AlsoPlaying) || !string.IsNullOrEmpty(AlsoPlayingAllMarkers) || !string.IsNullOrEmpty(AlsoPlayingAnyMarkers) || !string.IsNullOrEmpty(AlsoPlayingNotMarkers);
			}
		}

		public void OnBeforeSerialize()
		{

		}

		public void OnAfterDeserialize()
		{
			//Convert left, top, etc. to percentages since I don't feel like making an edit control that can handle those right now
			foreach (Scene scene in Scenes)
			{
				foreach (Directive directive in scene.Directives)
				{
					directive.PivotX = ConvertPivot(directive.PivotX);
					directive.PivotY = ConvertPivot(directive.PivotY);
				}
			}
		}

		private static string ConvertPivot(string pivot)
		{
			switch (pivot)
			{
				case "top":
				case "left":
					return "0%";
				case "bottom":
				case "right":
					return "100%";
				case "center":
					return "50%";
				default:
					return pivot;
			}
		}

		public override string ToString()
		{
			string text = Title;
			if (Gender == "male")
				return text + " (m)";
			else if (Gender == "female")
				return text + " (f)";
			return text;
		}

		#pragma warning disable IDE0051
		private bool FilterRecords(IRecord record)
		{
			Character c = record as Character;
			return c.FolderName != "human";
		}
		#pragma warning restore IDE0051
	}
}
