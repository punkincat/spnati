using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;

namespace SPNATI_Character_Editor
{
	/// <summary>
	/// Data for a single layer of clothing
	/// </summary>
	[Serializable]
	public class Clothing
	{
		public const int MaxLayers = 8;
		public const int ExtraStages = 3;

		[XmlAttribute("name")]
		public string Name;

		[XmlAttribute("generic")]
		public string GenericName;

		/// <summary>
		/// Deprecated
		/// </summary>
		[XmlAttribute("lowercase")]
		public string LowercaseName;

		[XmlAttribute("position")]
		public string Position;

		/// <summary>
		/// Deprecated
		/// </summary>
		[XmlAttribute("formalName")]
		public string FormalName;

		/// <summary>
		/// Deprecated
		/// </summary>
		[XmlAttribute("proper-name")]
		public string ProperName;

		[XmlAttribute("type")]
		public string Type;

		// only used for <clothing> elements within collectibles
		[XmlAttribute("img")]
		public string CollectibleImage;

		[XmlAttribute("plural")]
		[DefaultValue(false)]
		public bool Plural;

		public Clothing()
		{
			Position = "";
			Type = "major";
			GenericName = "item";
			Name = "new item";
			CollectibleImage = null;
			Plural = false;
		}

		public void OnAfterDeserialize()
		{
			if (string.IsNullOrEmpty(Name) || Name == "new item")
			{
				Name = LowercaseName;
				LowercaseName = null;
			}
			if (string.IsNullOrEmpty(GenericName) || GenericName == "item")
			{
				GenericName = FormalName ?? ProperName;
				FormalName = null;
				ProperName = null;
			}
		}

		public Clothing Copy()
		{
			Clothing copy = new Clothing()
			{
				Position = Position,
				Type = Type,
				FormalName = FormalName,
				LowercaseName = LowercaseName,
				ProperName = ProperName,
				Name = Name,
				GenericName = GenericName,
				CollectibleImage = CollectibleImage,
				Plural = Plural,
			};
			return copy;
		}

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(Name))
			{
				return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Name);
			}
			else
			{
				return "SKIP";
			}
		}
	}

	/// <summary>
	/// Class that has a wardrobe
	/// </summary>
	public interface IWardrobe
	{
		WardrobeRestrictions GetWardrobeRestrictions();
		int Layers { get; }
		bool IsDirty { get; set; }
		Clothing GetClothing(int index);
		void ApplyWardrobeChanges(Queue<WardrobeChange> changes);
		int AddLayer(Clothing layer);
		int RemoveLayer(Clothing layer);
		int MoveUp(Clothing layer);
		int MoveDown(Clothing layer);
	}

	[Flags]
	public enum WardrobeRestrictions
	{
		None = 0,
		/// <summary>
		/// Cannot add, remove, or rearrange layers
		/// </summary>
		LayerCount = 1,
		/// <summary>
		/// Cannot change layer types
		/// </summary>
		LayerTypes = 2,
		/// <summary>
		/// Cannot skip layers
		/// </summary>
		NoSkip = 4,
	}
}
