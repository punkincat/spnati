using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;

namespace SPNATI_Character_Editor
{
	[Serializable]
	public class ClothingLayer
	{
		[XmlAttribute("name")]
		public string Name;

		[XmlAttribute("generic")]
		public string GenericName;

		[XmlAttribute("position")]
		public string Position;

		[XmlAttribute("type")]
		public string Type;

		[XmlAttribute("plural")]
		[DefaultValue(false)]
		public bool Plural;

		[XmlAttribute("reveal")]
		[DefaultValue("")]
		public string Reveal;

		public ClothingLayer()
		{
			Position = "";
			Type = "major";
			GenericName = "";
			Name = "";
			Plural = false;
		}

		public ClothingLayer Clone()
		{
			ClothingLayer copy = new ClothingLayer()
			{
				Position = Position,
				Type = Type,
				Name = Name,
				GenericName = GenericName,
				Plural = Plural,
			};
			return copy;
		}
	}

	/// <summary>
	/// Data for a single layer of clothing
	/// </summary>
	[Serializable]
	public class Clothing : ClothingLayer
	{
		public const int MaxLayers = 8;
		public const int ExtraStages = 3;

		// only used for <clothing> elements within collectibles
		[XmlAttribute("img")]
		public string CollectibleImage;

		[XmlAttribute("fromDeal")]
		[DefaultValue(false)]
		public bool FromDeal;

		[XmlAttribute("fromStage")]
		[DefaultValue("")]
		public string FromStage;

		[XmlAttribute("strippingLayer")]
		[DefaultValue("")]
		public string StrippingLayer;

		[XmlElement("stripping")]
		[DefaultValue(null)]
		public ClothingLayer Stripping;

		public Clothing()
		{
			Position = "";
			Type = "major";
			GenericName = "";
			Name = "";
			CollectibleImage = null;
			Plural = false;
			Reveal = "";
			FromDeal = false;
			FromStage = "";
			StrippingLayer = "";
			Stripping = null;
		}

		public void OnAfterDeserialize()
		{
		}

		public bool HasAdv()
		{
			return (!string.IsNullOrEmpty(Reveal) || !string.IsNullOrEmpty(FromStage) || !string.IsNullOrEmpty(StrippingLayer) || Stripping != null);
		}

		public Clothing Copy()
		{
			Clothing copy = new Clothing()
			{
				Position = Position,
				Type = Type,
				Name = Name,
				GenericName = GenericName,
				CollectibleImage = CollectibleImage,
				Plural = Plural,
				Stripping = Stripping,
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
