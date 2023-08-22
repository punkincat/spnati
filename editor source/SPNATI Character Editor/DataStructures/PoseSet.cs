using Desktop.CommonControls.PropertyControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SPNATI_Character_Editor
{
	public class PoseSet : ICloneable, IComparable<PoseSet>
	{
		[Text(DisplayName = "ID", GroupOrder = 0)]
		[XmlAttribute("id")]
		public string Id;

		[XmlElement("pose")]
		public List<PoseSetEntry> Entries = new List<PoseSetEntry>();

		public PoseSet()
		{
			Id = "new set";
		}

		public object Clone()
		{
			PoseSet poseSet = MemberwiseClone() as PoseSet;
			poseSet.Entries = new List<PoseSetEntry>();
			foreach (PoseSetEntry entry in Entries)
			{
				PoseSetEntry clonedEntry = entry.Clone() as PoseSetEntry;
				poseSet.Entries.Add(clonedEntry);
			}
			return poseSet;
		}

		public int CompareTo(PoseSet other)
		{
			return Id.CompareTo(other.Id);
		}

		public override string ToString()
		{
			return Id;
		}

	}

	public class PoseSetEntry : ICloneable 
	{
		[XmlAttribute("img")]
		public string Img;

		[XmlIgnore]
		public string Character;

		private string _stage;
		[StageSelect(DisplayName = "Stage", GroupName = "Conditions", GroupOrder = 2, Description = "Stage (Required)", BoundProperties = new string[] { "Character" }, FilterStagesToTarget = false, SkinVariable = "~self.costume~")]
		[XmlAttribute("stage")]
		public string Stage
		{
			get { return _stage; }
			set
			{
				_stage = value ?? "0";
			}
		}

		[DefaultValue("")]
		[XmlAttribute("location")]
		public string Location;

		[DefaultValue("")]
		[XmlAttribute("direction")]
		public string Direction;

		[DefaultValue("")]
		[XmlAttribute("dialogue-layer")]
		public string DialogueLayer;

		[DefaultValue(0)]
		[XmlAttribute("priority")]
		public int Priority;

		[DefaultValue(0F)]
		[XmlAttribute("weight")]
		public float Weight;

		[Expression(DisplayName = "Variable Test (+)", GroupName = "Conditions", GroupOrder = 1, Description = "Tests the value of a variable. Multiple can be added")]
		[XmlArray("tests")]
		[XmlArrayItem("test")]
		public List<ExpressionTest> Tests = new List<ExpressionTest>();

		public PoseSetEntry() 
		{
		}

		public object Clone()
		{
			PoseSetEntry entry = MemberwiseClone() as PoseSetEntry;
			entry.Tests = new List<ExpressionTest>();
			foreach (ExpressionTest test in Tests)
			{
				ExpressionTest copiedTest = test.Copy();
				entry.Tests.Add(copiedTest);
			}
			return entry;
		}

	}
}