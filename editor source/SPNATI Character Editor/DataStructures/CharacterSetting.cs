using Desktop.CommonControls.PropertyControls;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SPNATI_Character_Editor.DataStructures
{
	public class CharacterSettingsGroup : ICloneable, IComparable<CharacterSettingsGroup>
	{
		[Text(DisplayName = "ID", GroupOrder = 0)]
		[XmlAttribute("id")]
		public string Id;

		[XmlAttribute("marker")]
		public string Marker;

		[XmlElement("setting")]
		public List<CharacterSetting> CharacterSettings = new List<CharacterSetting>();

		public CharacterSettingsGroup()
		{
			Id = "new settings group";
		}

		public object Clone()
		{
			CharacterSettingsGroup group = MemberwiseClone() as CharacterSettingsGroup;
			group.CharacterSettings = new List<CharacterSetting>();
			foreach (CharacterSetting characterSetting in CharacterSettings)
			{
				CharacterSetting clonedSetting = characterSetting.Clone() as CharacterSetting;
				group.CharacterSettings.Add(clonedSetting);
			}
			return group;
		}
		public int CompareTo(CharacterSettingsGroup other)
		{
			return Id.CompareTo(other.Id);
		}

		public override string ToString()
		{
			return Id;
		}
	}

	public class CharacterSetting : ICloneable
	{
		[XmlAttribute("value")]
		public string Value;

		[XmlAttribute("default")]
		public bool Default;

		[Expression(DisplayName = "Variable Test (+)", GroupName = "Conditions", GroupOrder = 1, Description = "Tests the value of a variable. Multiple can be added")]
		[XmlElement("test")]
		public List<ExpressionTest> Tests = new List<ExpressionTest>();

		[XmlElement("name")]
		public string Name;

		public CharacterSetting()
		{
		}

		public object Clone()
		{
			CharacterSetting cs = MemberwiseClone() as CharacterSetting;
			cs.Tests = new List<ExpressionTest>();
			foreach (ExpressionTest test in Tests)
			{
				ExpressionTest copiedTest = test.Copy();
				cs.Tests.Add(copiedTest);
			}
			return cs;
		}
	}
}
