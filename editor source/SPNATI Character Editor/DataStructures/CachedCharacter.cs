using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace SPNATI_Character_Editor
{
	/// <summary>
	/// Initial character stub that exists only during initial load so that CharacterDatabase.Get() calls correctly identify the existence of characters that haven't been loaded yet
	/// </summary>
	public class PlaceholderCharacter : Character
	{
	}

	[XmlRoot("opponent", Namespace = "")]
	/// <summary>
	/// Cached important information about a character to use prior to loading in the real character
	/// </summary>
	public class CachedCharacter : Character
	{
		public static int CurrentVersion = 5;

		[XmlElement("cacheVersion")]
		public int CacheVersion;

		[DefaultValue(false)]
		[XmlElement("crossGender")]
		public bool CrossGender;

		[XmlElement("lineCount")]
		public int TotalLines;

		[XmlElement("linesTargeted")]
		public int TargetedLines;

		[XmlArray("lineTargets")]
		[XmlArrayItem("to")]
		public List<TargetingInformation> Targets = new List<TargetingInformation>();

		private Dictionary<string, int> _targets;

		[XmlArray("altCostumes")]
		[XmlArrayItem("alternate")]
		public List<AlternateSkin> Skins = new List<AlternateSkin>();

		[XmlElement("writer")]
		public string Writer;

		[XmlElement("default-costume-name")]
		public string DefaultCostumeName;

		[XmlElement("lastupdate")]
		public long LastUpdate;

		public override bool IsFullyLoaded { get { return false; } }

		public CachedCharacter() { }

		/// <summary>
		/// Constructs an index from a real Character object
		/// </summary>
		/// <param name="character"></param>
		public CachedCharacter(Character character)
		{
			character.PrepareForEdit();

			//Basic info to copy over
			CacheVersion = CurrentVersion;

			// Allows title-screen labels to be the ones used in the CE
			if (!string.IsNullOrEmpty(character.Metadata.Label))
			{
				Label = character.Metadata.Label;
			}
			else
			{
				Label = character.Label;
			}

			FolderName = character.FolderName;
			FirstName = character.FirstName;
			LastName = character.LastName;
			CrossGender = character.Metadata.CrossGender;
			Tags = character.Tags;
			Gender = character.Gender;
			Size = character.Size;
			DefaultCostumeName = character.Metadata.DefaultCostumeName;
			Wardrobe = character.Wardrobe;
			Writer = character.Metadata.Writer;
			LastUpdate = character.Metadata.LastUpdate;
			Version = character.Version;

			Metadata.AlternateSkins = character.Metadata.AlternateSkins;
			Skins.AddRange(character.Metadata.AlternateSkins);

			//Behaviour info to index
			HashSet<string> lines = new HashSet<string>();
			int targetCount = 0;
			Dictionary<string, HashSet<string>> targetedLines = new Dictionary<string, HashSet<string>>();
			foreach (Case workingCase in character.Behavior.GetWorkingCases())
			{
				HashSet<string> targets = workingCase.GetTargets();
				foreach (DialogueLine line in workingCase.Lines)
				{
					string text = line.Text ?? "";
					if (!lines.Contains(text))
					{
						lines.Add(text);
						if (targets.Count > 0)
						{
							targetCount++;
						}
					}

					foreach (string target in targets)
					{
						HashSet<string> targetLines = targetedLines.GetOrAddDefault(target, () => new HashSet<string>());
						targetLines.Add(text);
					}
				}
			}

			TotalLines = lines.Count;
			TargetedLines = targetCount;
			Targets = new List<TargetingInformation>();
			foreach (KeyValuePair<string, HashSet<string>> kvp in targetedLines)
			{
				Targets.Add(new TargetingInformation(kvp.Key) { LineCount = kvp.Value.Count });
			}

			OnAfterDeserialize(FolderName);
		}

		public override void OnBeforeSerialize()
		{
			//do nothing but prevent the base implementation from running
		}

		public override void OnAfterDeserialize(string source)
		{
			_targets = new Dictionary<string, int>();
			foreach (TargetingInformation ti in Targets)
			{
				_targets[ti.Target] = ti.LineCount;
			}

			Metadata.AlternateSkins.Clear();
			foreach (AlternateSkin skin in Skins)
			{
				Metadata.AlternateSkins.Add(skin);
			}

			Metadata.CrossGender = CrossGender;
			Metadata.Writer = Writer;
			Metadata.LastUpdate = LastUpdate;
		}

		public int GetTargetedCountTowards(string folderName)
		{
			return _targets.Get(folderName);
		}
	}
}
