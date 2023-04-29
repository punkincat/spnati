using Desktop;
using Desktop.CommonControls;
using Desktop.CommonControls.PropertyControls;
using Desktop.Reporting;
using Newtonsoft.Json;
using SPNATI_Character_Editor.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SPNATI_Character_Editor
{
	/// <remarks>
	/// Note: This class breaks from the normal BindableObject usage by storing properties explicitly. This is for performance and memory reasons
	/// since storing PropertyChanged handlers starts to really add up when you have thousands of these.
	/// </remarks>
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class Case : INotifyPropertyChanged, IPropertyChangedNotifier, IComparable<Case>, ISliceable
	{
		private static long s_globalId;

		public event PropertyChangedEventHandler PropertyChanged;
		public void NotifyPropertyChanged([CallerMemberName] string propName = "")
		{
			_conditionHash = 0;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		private string _tag;
		[XmlOrder(0)]
		[XmlAttribute("tag")]
		[JsonProperty("tag")]
		public string Tag
		{
			get { return _tag; }
			set { if (_tag != value) { _tag = value; NotifyPropertyChanged(); } }
		}

		/// <summary>
		/// This was only used in the preview. StageId is used now, so once characters using it get saved again, this can be deleted later
		/// </summary>
		[DefaultValue(0)]
		[XmlOrder(1)]
		[XmlAttribute("set")]
		public int TriggerSet;

		/// <summary>
		/// Unique case identifier in the format Stage-Id. Unused by the game, but important for the editor
		/// </summary>
		[DefaultValue(null)]
		[XmlOrder(2)]
		[XmlAttribute("id")]
		public string StageId;

		private int _id;
		/// <summary>
		/// ID for a working case. Like the 2nd piece of StageId
		/// </summary>
		[XmlIgnore]
		public int Id
		{
			get { return _id; }
			set { if (_id != value) { _id = value; NotifyPropertyChanged(); } }
		}

		/// <summary>
		/// Only used with ImportEdits
		/// </summary>
		[JsonProperty("stage")]
		[XmlIgnore]
		public string Stage;

		private int _oneShotId;
		/// <summary>
		/// Case will only play once
		/// </summary>
		//[OneShot(OneShotMode.Case, DisplayName = "Play Once", GroupName = "Self", GroupOrder = 98, Description = "This call will only play once per game.")]
		[DefaultValue(0)]
		[XmlOrder(10)]
		[JsonProperty("oneShotId")]
		[XmlAttribute("oneShotId")]
		public int OneShotId
		{
			get { return _oneShotId; }
			set { if (_oneShotId != value) { _oneShotId = value; NotifyPropertyChanged(); } }
		}

        private string _hidden;
		[XmlOrder(40)]
		[XmlAttribute("hidden")]
		[JsonProperty("hidden")]
		public string Hidden
		{
			get { return _hidden; }
			set { if (_hidden != value) { _hidden = value; NotifyPropertyChanged(); } }
		}

		private string _disabled;
		[XmlOrder(45)]
		[XmlAttribute("disabled")]
		[JsonProperty("disabled")]
		public string Disabled
		{
			get { return _disabled; }
			set { if (_disabled != value) { _disabled = value; NotifyPropertyChanged(); } }
		}

		private string _totalRounds;
		[NumericRange(DisplayName = "Total Rounds", GroupName = "Game", GroupOrder = 1, Description = "Number of rounds since the game began")]
		[XmlOrder(250)]
		[XmlAttribute("totalRounds")]
		[JsonProperty("totalRounds")]
		public string TotalRounds
		{
			get { return _totalRounds; }
			set { if (_totalRounds != value) { _totalRounds = value; NotifyPropertyChanged(); } }
		}

		private string _priority;
		[XmlOrder(360)]
		[XmlAttribute("priority")]
		[JsonProperty("priority")]
		public string CustomPriority
		{
			get { return _priority; }
			set { if (_priority != value) { _priority = value; NotifyPropertyChanged(); } }
		}

		private string _addCharacterTags;
		[XmlOrder(370)]
		[XmlAttribute("addCharacterTags")]
		[JsonProperty("addCharacterTags")]
		public string AddCharacterTags
		{
			get { return _addCharacterTags; }
			set { if (_addCharacterTags != value) { _addCharacterTags = value; NotifyPropertyChanged(); } }
		}

		private string _removeCharacterTags;
		[XmlOrder(380)]
		[XmlAttribute("removeCharacterTags")]
		[JsonProperty("removeCharacterTags")]
		public string RemoveCharacterTags
		{
			get { return _removeCharacterTags; }
			set { if (_removeCharacterTags != value) { _removeCharacterTags = value; NotifyPropertyChanged(); } }
		}

		[Filter(DisplayName = "Filter (+)", GroupOrder = 0, Description = "Filter based on table conditions. Multiple can be added", HideLabel = true)]
		[XmlOrder(390)]
		[XmlElement("condition")]
		[JsonProperty("counters")]
		public List<TargetCondition> Conditions;

		[Expression(DisplayName = "Variable Test (+)", GroupName = "Game", GroupOrder = 5, Description = "Tests the value of a variable. Multiple can be added", BoundProperties = new string[] { "Target", "AlsoPlaying" })]
		[XmlOrder(400)]
		[XmlElement("test")]
		[JsonProperty("tests")]
		public List<ExpressionTest> Expressions;

		[XmlOrder(405)]
		[XmlElement("alternative")]
		public List<Case> AlternativeConditions = new List<Case>();

		[JsonProperty("lines")]
		[XmlOrder(410)]
		[XmlElement("state")]
		public List<DialogueLine> Lines = new List<DialogueLine>();

		/// <summary>
		/// Used for consistently sorting two identical cases
		/// </summary>
		private long _globalId;

		/// <summary>
		/// Stages this case appears in
		/// </summary>
		[XmlIgnore]
		public List<int> Stages = new List<int>();

		[DefaultValue("")]
		[XmlAttribute("stage")]
		public string StageRange
		{
			get
			{
				return GUIHelper.ListToString(Stages);
			}
			set
			{
				Stages = GUIHelper.StringToList(value);
			}
		}

		/// <summary>
		/// Whether this case is considered the "default" for its tag
		/// </summary>
		[XmlIgnore]
		public bool IsDefault;

		public Case()
		{
			_globalId = s_globalId++;
			Lines = new List<DialogueLine>();
			Stages = new List<int>();
			Conditions = new List<TargetCondition>();
			Expressions = new List<ExpressionTest>();
			AlternativeConditions = new List<Case>();
		}

		public Case(string tag) : this()
		{
			Tag = tag;
		}

		public override string ToString()
		{
			string result = TriggerDatabase.GetLabel(Tag);
			if (!string.IsNullOrEmpty(CustomPriority))
			{
				result = "*" + result;
			}
			if (HasConditions)
			{
				result += " " + ToConditionsString(false);
			}
			if (string.IsNullOrEmpty(Hidden) && string.IsNullOrEmpty(Disabled))
			{
				int priority = GetPriority();
				if (priority > 0)
				{
					result += string.Format(" (priority={0})", priority);
				}
			}
			return result;
		}

		public string ToConditionsString(bool excludeTarget)
		{
			List<string> alternates = new List<string>();
			foreach (Case alternate in AlternativeConditions)
			{
				alternates.Add(alternate.ToConditionsString(false));
			}
			List<string> result = new List<string>();
			if (!string.IsNullOrEmpty(TotalRounds))
			{
				result.Add(string.Format("({0} overall rounds)", GUIHelper.RangeToString(TotalRounds)));
			}
			if (Conditions.Count > 0)
			{
				string cond = string.Join(",", Conditions.Select(c => c.ToString(excludeTarget)));
				if (!string.IsNullOrEmpty(cond))
				{
					result.Add(string.Format("({0})", cond));
				}
			}
			if (Expressions.Count > 0)
			{
				result.Add($"({string.Join(",", Expressions)})");
			}
			if (OneShotId > 0)
			{
				result.Add("(play once)");
			}
			string conditions = string.Join(" ", result);
			if (alternates.Count > 0)
			{
				string alternatesString = string.Join(" OR ", alternates);
				conditions = $"*{conditions} AND ({alternatesString})";
			}
			return conditions;
		}

		public void ClearConditions()
		{
			foreach (MemberInfo field in this.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance))
			{
				if (field.Name == "StageRange" || field.Name == "Tag" || field.Name == "OneShotId" || field.Name == "Id" || field.Name == "StageId" || field.Name == "Hidden" || field.Name == "Disabled")
				{
					continue;
				}
				if (field.MemberType == MemberTypes.Field || field.MemberType == MemberTypes.Property)
				{
					if (field.GetDataType() == typeof(string))
					{
						field.SetValue(this, null);
					}
					else if (field.GetDataType() == typeof(int))
					{
						field.SetValue(this, 0);
					}
				}
			}
			Conditions.Clear();
			Expressions.Clear();
		}

		/// <summary>
		/// Copies the case except stages and lines
		/// </summary>
		/// <returns></returns>
		public Case CopyConditions()
		{
			Case copy = new Case();
			foreach (MemberInfo field in this.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance))
			{
				if (field.Name == "StageRange")
				{
					continue;
				}
				if (field.MemberType == MemberTypes.Field || field.MemberType == MemberTypes.Property)
				{
					if (field.GetDataType() == typeof(string) || field.GetDataType() == typeof(int))
					{
						field.SetValue(copy, field.GetValue(this));
					}
				}
			}

			//Since it's just a shallow collection, need to break references to objects
			copy.Conditions = new List<TargetCondition>();
			foreach (TargetCondition condition in Conditions)
			{
				copy.Conditions.Add(condition.Copy());
			}

			copy.Expressions = new List<ExpressionTest>();
			foreach (ExpressionTest test in Expressions)
			{
				copy.Expressions.Add(test.Copy());
			}

			copy.AlternativeConditions = new List<Case>();
			foreach (Case alternate in AlternativeConditions)
			{
				copy.AlternativeConditions.Add(alternate.Copy());
			}

			return copy;
		}

		/// <summary>
		/// Copies the case including dialogue and conditions but NOT stages
		/// </summary>
		/// <returns></returns>
		public Case Copy()
		{
			Case copy = CopyConditions();
			for (int i = 0; i < Lines.Count; i++)
			{
				copy.Lines.Add(Lines[i].Copy());
			}
			return copy;
		}

		public void ClearEmptyValues()
		{
			foreach (MemberInfo field in this.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance))
			{
				if (field.Name == "StageRange")
				{
					continue;
				}
				if (field.MemberType == MemberTypes.Field || field.MemberType == MemberTypes.Property)
				{
					if (field.GetDataType() == typeof(string) && (string)field.GetValue(this) == "")
					{
						field.SetValue(this, null);
					}
				}
			}

			foreach (var condition in Conditions)
			{
				condition.ClearEmptyValues();
			}
		}

		/// <summary>
		/// Gets the priority for targeted dialogue used by the game
		/// </summary>
		/// <returns></returns>
		public int GetPriority()
		{
			int totalPriority = 0;
			if (!string.IsNullOrEmpty(Hidden) || !string.IsNullOrEmpty(Disabled))
			{
				return int.MinValue;
			}
			if (!string.IsNullOrEmpty(CustomPriority))
			{
				int priority;
				if (int.TryParse(CustomPriority, out priority))
				{
					return priority;
				}
			}

			totalPriority += Conditions.Sum(c => c.GetPriority());
			totalPriority += Expressions.Count * 50;

			if (!string.IsNullOrEmpty(TotalRounds))
				totalPriority += 10;

			if (AlternativeConditions.Count > 0)
			{
				totalPriority += (int)AlternativeConditions.Average(c => c.GetPriority());
			}

			return totalPriority;
		}

		/// <summary>
		/// Gets whether this case has the same stages as another, but not necessarily other conditions
		/// </summary>
		/// <param name="other"></param>
		/// <param name="matchAll">If true, all stages must match. If false, only one needs to</param>
		/// <returns></returns>
		public bool MatchesStages(Case other, bool matchAll)
		{
			if (!matchAll)
			{
				for (int i = 0; i < Stages.Count; i++)
				{
					if (other.Stages.Contains(Stages[i]))
					{
						return true;
					}
				}
				return false;
			}

			if (other.Stages.Count != Stages.Count)
			{
				return false;
			}

			for (int i = 0; i < other.Stages.Count; i++)
			{
				if (other.Stages[i] != Stages[i])
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Gets whether the lines in this case are identical to another
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool MatchesLines(Case other)
		{
			if (Lines.Count != other.Lines.Count)
			{
				return false;
			}
			for (int i = 0; i < Lines.Count; i++)
			{
				if (Lines[i].GetHashCode() != other.Lines[i].GetHashCode())
				{
					return false;
				}
			}
			return true;
		}

		private int _conditionHash;

		private int GetConditionHash(bool includePriority)
		{
			if (_conditionHash > 0)
			{
				return _conditionHash;
			}
			int hash = (TotalRounds ?? string.Empty).GetHashCode();
			hash = (hash * 397) ^ (AddCharacterTags ?? string.Empty).GetHashCode();
			hash = (hash * 397) ^ (RemoveCharacterTags ?? string.Empty).GetHashCode();
			if (includePriority)
			{
				hash = (hash * 397) ^ (CustomPriority ?? string.Empty).GetHashCode();
			}
			hash = (hash * 397) ^ (Hidden ?? string.Empty).GetHashCode();
			hash = (hash * 397) ^ (OneShotId > 0 ? OneShotId : -1);
			_conditionHash = hash;
			return hash;
		}

		/// <summary>
		/// Gets whether this case matches another in everything but conditions
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool MatchesNonConditions(Case other)
		{
			if (other == this)
				return true;
			if (Tag != other.Tag)
				return false;

			if (other.Lines.Count != Lines.Count)
			{
				return false;
			}
			if (other.GetLineCode() != GetLineCode())
			{
				return false;
			}
			if (other.RemoveCharacterTags != RemoveCharacterTags ||
				other.AddCharacterTags != AddCharacterTags ||
				other.CustomPriority != CustomPriority)
			{
				return false;
			}

			return true;
		}

		public bool MatchesConditions(Case other)
		{
			return MatchesConditions(other, true);
		}
		/// <summary>
		/// Gets whether this case matches the conditions+tag of another, but not necessarily the lines or stages
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool MatchesConditions(Case other, bool includePriority)
		{
			if (other == this)
				return true;
			if (Tag != other.Tag)
				return false;

			bool sameFilters = (GetConditionHash(includePriority) == other.GetConditionHash(includePriority));
			if (!sameFilters)
				return false;

			if (other.Conditions.Count != Conditions.Count)
				return false;
			for (int i = 0; i < Conditions.Count; i++)
			{
				TargetCondition c1 = Conditions[i];
				TargetCondition c2 = other.Conditions[i];
				if (!c1.Equals(c2))
					return false;
			}
			if (other.Expressions.Count != Expressions.Count)
				return false;
			for (int i = 0; i < Expressions.Count; i++)
			{
				if (!Expressions[i].Equals(other.Expressions[i]))
				{
					return false;
				}
			}

			return true;
		}

		public bool HasConditions
		{
			get
			{
				return !string.IsNullOrEmpty(TotalRounds) ||
				  !string.IsNullOrEmpty(Hidden) ||
				  !string.IsNullOrEmpty(Disabled) ||
				  Conditions.Count > 0 ||
				  Expressions.Count > 0;
			}
		}

		/// <summary>
		/// Gets whether this case has any Filters
		/// </summary>
		public bool HasFilters
		{
			get
			{
				bool filtered = Conditions.Any(c =>
				{
					if (c.Role == "self")
					{
						return false;
					}

					bool result = !string.IsNullOrEmpty(c.FilterTag);
					if (result)
					{
						Character character = CharacterDatabase.Get(c.FilterTag);
						result = (character == null);
					}
					return result;
				});
				if (!filtered)
				{
					filtered = AlternativeConditions.Any(a => a.HasFilters);
				}
				return filtered;
			}
		}

		/// <summary>
		/// Gets whether this case has any direct targeted dialogue towards other players EXCEPT human
		/// </summary>
		public bool HasTargetedConditions
		{
			get
			{
				string[] humanTargets = { "human", "human_male", "human_female" };

				bool targeted = false;

				foreach (TargetCondition condition in Conditions)
				{
					if (condition.Role != "self" && condition.Count != "0" && condition.Count != "0-0")
					{
						if (!string.IsNullOrEmpty(condition.Character) && !humanTargets.Contains(condition.Character))
						{
							targeted = true;
							break;
						}
						if (!string.IsNullOrEmpty(condition.FilterTag) && CharacterDatabase.Get(condition.FilterTag) != null && !humanTargets.Contains(condition.FilterTag))
						{
							targeted = true;
							break;
						}
					}
				}
				if (!targeted)
				{
					foreach (Case alternative in AlternativeConditions)
					{
						if (alternative.HasTargetedConditions)
						{
							return true;
						}
					}
				}
				return targeted;
			}
		}

		/// <summary>
		/// Gets whether this case has any direct targeted dialogue towards other players INCLUDING human
		/// </summary>
		public bool HasTargetedConditionsIncludeHuman
		{
			get
			{
				bool targeted = false;

				foreach (TargetCondition condition in Conditions)
				{
					if (condition.Role != "self")
					{
						if (!string.IsNullOrEmpty(condition.Character))
						{
							targeted = true;
							break;
						}
						if (!string.IsNullOrEmpty(condition.FilterTag) && CharacterDatabase.Get(condition.FilterTag) != null)
						{
							targeted = true;
							break;
						}
					}
				}
				if (!targeted)
				{
					foreach (Case alternative in AlternativeConditions)
					{
						if (alternative.HasTargetedConditions)
						{
							return true;
						}
					}
				}
				return targeted;
			}
		}

		public IEnumerable<Case> GetConditionSets()
		{
			yield return this;
			foreach (Case alternate in AlternativeConditions)
			{
				yield return alternate;
			}
		}

		/// <summary>
		/// Gets whether this case has any targeted dialogue that is based on game state
		/// </summary>
		public bool HasStageConditions
		{
			get
			{
				return !string.IsNullOrEmpty(TotalRounds) ||
					Conditions.Count > 0 ||
					Expressions.Count > 0;
			}
		}

		public int GetFullHashCode()
		{
			int hash = GetConditionHash(true);
			foreach (var condition in Conditions)
			{
				hash = (hash * 397) ^ condition.GetHashCode();
			}
			foreach (ExpressionTest expr in Expressions)
			{
				hash = (hash * 397) ^ expr.GetHashCode();
			}
			hash = (hash * 397) ^ GetLineCode();
			return hash;
		}

		/// <summary>
		/// Gets a unique hash for this combination of conditions
		/// </summary>
		/// <returns></returns>
		public int GetCode()
		{
			int hash = Tag.GetHashCode();
			hash = (hash * 397) ^ GetConditionHash(true);
			foreach (var condition in Conditions)
			{
				hash = (hash * 397) ^ condition.GetHashCode();
			}
			foreach (ExpressionTest expr in Expressions)
			{
				hash = (hash * 397) ^ expr.GetHashCode();
			}
			return hash;
		}

		/// <summary>
		/// Returns a unique hash for this combination of lines
		/// </summary>
		/// <returns></returns>
		public int GetLineCode()
		{
			int hash = 0;
			foreach (DialogueLine line in Lines)
			{
				hash = (hash * 397) ^ line.GetHashCode();
			}
			return hash;
		}

		public int CompareTo(Case other)
		{
			int comparison = 0;
			if (!string.IsNullOrEmpty(Tag) && !string.IsNullOrEmpty(other.Tag))
			{
				comparison = Tag.CompareTo(other.Tag);
			}
			if (comparison == 0)
				comparison = other.GetPriority().CompareTo(GetPriority());
			if (comparison == 0)
				return _globalId.CompareTo(other._globalId);
			else return comparison;
		}

		/// <summary>
		/// Converts a string to a range
		/// </summary>
		/// <param name="range">Range in the format D or D-D</param>
		/// <param name="min">Minimum value of the range</param>
		/// <param name="max">Maximum value of the range</param>
		public static void ToRange(string range, out int min, out int max)
		{
			if (string.IsNullOrEmpty(range))
			{
				min = 0;
				max = 0;
				return;
			}
			string[] pieces = range.Split('-');
			int.TryParse(pieces[0], out min);
			if (pieces.Length > 1)
				int.TryParse(pieces[1], out max);
			else max = min;
		}

		/// <summary>
		/// Creates a response to a situation.
		/// </summary>
		/// <param name="speaker"></param>
		/// <param name="responder"></param>
		/// <returns></returns>
		public Case CreateResponse(Character speaker, Character responder)
		{
			//no way to respond to hidden/disabled cases, since they never display
			if (Hidden == "1" || Disabled == "1") { return null; }

			Case response = new Case();

			response.Tag = GetResponseTag(speaker, responder);
			if (response.Tag == null)
			{
				return null;
			}

			bool caseIsTargetable = TriggerDatabase.GetTrigger(Tag).HasTarget;
			bool responseIsTargetable = TriggerDatabase.GetTrigger(response.Tag).HasTarget;
			bool hasTarget = HasTarget();
			bool targetingResponder = (Conditions.Find(c => c.Role == "target" && c.Character == responder.FolderName) != null);
			bool hasAlsoPlaying = HasAlsoPlaying();
			bool alsoPlayingIsResponder = (Conditions.Find(c => c.Role == "other" && c.Character == responder.FolderName) != null);

			if (response.Tag == "-") //this is deprecated anyway
			{
				return null;
			}

			//copy conditions are always the same. If needed, they'll be altered in the method calls below
			foreach (TargetCondition cond in Conditions)
			{
				response.Conditions.Add(cond.Copy());
			}

			if ((caseIsTargetable && hasAlsoPlaying && !alsoPlayingIsResponder) || (!responseIsTargetable && !hasTarget && hasAlsoPlaying && !alsoPlayingIsResponder))
			{
				//for cases where AlsoPlaying is already in use, shift that character into a filter target condition
				hasAlsoPlaying = false; //free this up for the responder to go into
			}

			if (!caseIsTargetable && responseIsTargetable && !hasTarget && !hasAlsoPlaying)
			{
				CopySelfIntoTarget(response, speaker);
			}
			else if (!caseIsTargetable && responseIsTargetable && !hasTarget && alsoPlayingIsResponder)
			{
				CopySelfIntoTarget(response, speaker);
				CopyAlsoPlayingIntoSelf(response, responder);
			}
			else if (!caseIsTargetable && responseIsTargetable && !hasTarget && hasAlsoPlaying && !alsoPlayingIsResponder)
			{
				CopySelfIntoTarget(response, speaker);
			}
			else if ((caseIsTargetable || !responseIsTargetable) && !hasTarget && !alsoPlayingIsResponder)
			{
				CopySelfIntoAlsoPlaying(response, speaker);
			}
			else if ((caseIsTargetable || !responseIsTargetable) && !hasTarget && alsoPlayingIsResponder)
			{
				CopySelfIntoAlsoPlaying(response, speaker);
				CopyAlsoPlayingIntoSelf(response, responder);
			}
			else if (caseIsTargetable && hasTarget && !targetingResponder && !hasAlsoPlaying)
			{
				CopySelfIntoAlsoPlaying(response, speaker);
			}
			else if (caseIsTargetable && hasTarget && !hasAlsoPlaying && targetingResponder)
			{
				CopyTargetIntoSelf(response, responder);
				CopySelfIntoAlsoPlaying(response, speaker);
			}
			else if (caseIsTargetable && hasTarget && !targetingResponder && alsoPlayingIsResponder)
			{
				CopyAlsoPlayingIntoSelf(response, responder);
				CopySelfIntoAlsoPlaying(response, speaker);
			}
			else
			{
				return null; //if I computed the truth table correctly, this should never happen
			}

			string otherId = CharacterDatabase.GetId(responder);
			foreach (ExpressionTest test in Expressions)
			{
				string testTarget = test.GetTarget();
				if (testTarget == otherId)
				{
					//special cases
					if (test.Expression == $"~{testTarget}.position~")
					{
						string side = test.Value == "right" ? "left" : test.Value == "left" ? "right" : test.Value;
						ExpressionTest copy = test.Copy();
						copy.Expression = $"~{CharacterDatabase.GetId(speaker)}.position~";
						copy.Value = side;
						response.Expressions.Add(copy);
					}
					else
					{
						ExpressionTest copy = test.Copy();
						copy.ChangeTarget("self");
						response.Expressions.Add(copy);
					}
				}
			}

			//if no stages have been set, apply it to all
			if (Tag.Contains("chest") && !response.Tag.Contains("chest"))
			{
				int layer = GetRevealStage(responder, "upper");
				if (Tag.Contains("will"))
				{
					layer--;
				}
				response.Stages.Clear();
				response.Stages.Add(layer);
			}
			else if (Tag.Contains("crotch") && !response.Tag.Contains("crotch"))
			{
				int layer = GetRevealStage(responder, "lower");
				if (Tag.Contains("will"))
				{
					layer--;
				}
				response.Stages.Clear();
				response.Stages.Add(layer);
			}
			else
			{
				TriggerDefinition trigger = TriggerDatabase.GetTrigger(response.Tag);
				if (response.Stages.Count == 0)
				{
					for (int i = 0; i < responder.Layers + Clothing.ExtraStages; i++)
					{
						int standardStage = TriggerDatabase.ToStandardStage(responder, i);
						if (standardStage >= trigger.StartStage && standardStage <= trigger.EndStage)
						{
							response.Stages.Add(i);
						}
					}
				}
			}

			foreach (ExpressionTest test in Expressions)
			{
				if (!test.RefersTo(speaker, speaker, GetTarget()) && !test.RefersTo(responder, speaker, GetTarget()))
				{
					if (test.Expression.EndsWith(".position~"))
					{
						//can't adequate flip this
						continue;
					}
					else
					{
						response.Expressions.Add(test);
					}
				}
			}

			response.AdjustConditions(speaker, responder, this);

			response.TotalRounds = TotalRounds;

			//special cases
			if (Tag == "must_masturbate_first")
			{
				TargetCondition cond = new TargetCondition();
				cond.Count = "0";
				cond.Status = "not_alive";
				response.Conditions.Add(cond);
			}
			else if (response.Tag == "must_masturbate")
			{
				//finished check
				TargetCondition finished = response.Conditions.Find(c => c.ToString() == "0 finished");
				TargetCondition finishing = response.Conditions.Find(c => c.ToString() == "0 masturbating");
				if (finished != null && finishing != null)
				{
					response.Tag = "must_masturbate_first";
					response.Conditions.Remove(finished);
					response.Conditions.Remove(finishing);
				}
			}

			foreach (Case alternate in AlternativeConditions)
			{
				alternate.Tag = Tag;
				alternate.Stages = Stages;
				Case alternateResponse = alternate.CreateResponse(speaker, responder);
				response.AlternativeConditions.Add(alternateResponse);
			}

			// This is inefficient and would be better served by fixing
			// the helper functions (CopySelfIntoAlsoPlaying, etc)
			// to not use oldstyle conditions - do this later
			DataConversions.ConvertCase(response, responder);

			return response;
		}

		/// <summary>
		/// Gets whether any Target-related fields are set
		/// </summary>
		private bool HasTarget()
		{
			foreach (TargetCondition cond in Conditions)
            {
				if (cond.Role == "target") { return true; }
            }

			return false;
		}

		/// <summary>
		/// Gets whether any AlsoPlaying-related fields are set
		/// </summary>
		/// <returns></returns>
		private bool HasAlsoPlaying()
		{
			foreach (TargetCondition cond in Conditions)
			{
				if (cond.Role == "other") { return true; }
			}

			return false;
		}

		/// <summary>
		/// Copies Target properties into another case's self properties
		/// </summary>
		/// <param name="other"></param>
		private void CopyTargetIntoSelf(Case other, Character responder)
		{
			string condStage = null;

			foreach (TargetCondition cond in Conditions)
            {
				if (cond.Role == "target" && !string.IsNullOrEmpty(cond.Stage))
                {
					condStage = cond.Stage;
					break;
                }
            }

			if (!string.IsNullOrEmpty(condStage))
			{
				int min, max;
				ToRange(condStage, out min, out max);
				for (int i = min; i <= max; i++)
				{
					other.Stages.Add(i);
				}
			}
			else
			{
				bool removing = Tag.Contains("will_be_visible") || Tag.Contains("removing_");
				bool removed = Tag.Contains("is_visible") || Tag.Contains("removed_");
				string type = Tag.Contains("accessory") ? "extra" : Tag.Contains("minor") ? "minor" : Tag.Contains("major") ? "major" : "important";
				string position = Tag.Contains("chest") ? "upper" : "lower";

				//all stages
				TriggerDefinition trigger = TriggerDatabase.GetTrigger(other.Tag);
				for (int i = 0; i < responder.Layers + Clothing.ExtraStages; i++)
				{
					int standardStage = TriggerDatabase.ToStandardStage(responder, i);
					if (standardStage >= trigger.StartStage && standardStage <= trigger.EndStage)
					{
						//filter out stages that aren't possible
						if (removing || removed)
						{
							if (i > responder.Layers)
							{
								//no finishing or finished
								continue;
							}
							else
							{
								//make sure item matches targeted type
								Clothing c = responder.Wardrobe[responder.Layers - i - (removed ? 0 : 1)];
								if (c.Type != type)
								{
									continue;
								}
								if (type == "important" && position != c.Position)
								{
									continue;
								}
							}
						}

						other.Stages.Add(i);
					}
				}
			}

			for (int i = Conditions.Count - 1; i >= 0; i--)
            {
				if (Conditions[i].Role != "target") { continue; }

				TargetCondition selfCond = new TargetCondition();
				selfCond.Role = "self";
				bool hasConds = false;

				if (!string.IsNullOrEmpty(Conditions[i].Hand))
                {
					selfCond.Hand = Conditions[i].Hand;
					hasConds = true;
                }
				if (!string.IsNullOrEmpty(Conditions[i].SaidMarker))
				{
					selfCond.SaidMarker = Conditions[i].SaidMarker;
					hasConds = true;
				}
				if (!string.IsNullOrEmpty(Conditions[i].NotSaidMarker))
				{
					selfCond.NotSaidMarker = Conditions[i].NotSaidMarker;
					hasConds = true;
				}
				if (!string.IsNullOrEmpty(Conditions[i].TimeInStage))
				{
					selfCond.TimeInStage = Conditions[i].TimeInStage;
					hasConds = true;
				}
				if (!string.IsNullOrEmpty(Conditions[i].ConsecutiveLosses))
				{
					selfCond.ConsecutiveLosses = Conditions[i].ConsecutiveLosses;
					hasConds = true;
				}

				if (hasConds)
                {
					Conditions.Add(selfCond);
                }

				if (!string.IsNullOrEmpty(Conditions[i].FilterTag))
				{
					ExpressionTest test = new ExpressionTest("self.tag." + Conditions[i].FilterTag, "true");
					other.Expressions.Add(test);
				}
			}

			foreach (ExpressionTest test in Expressions)
			{
				if (test.GetTarget() == "target")
				{
					ExpressionTest copy = test.Copy();
					copy.ChangeTarget("self");
					other.Expressions.Add(copy);
				}
			}
		}

		/// <summary>
		/// Copies this tag's self-targeting properties into another's AlsoPlaying
		/// </summary>
		/// <param name="other"></param>
		private void CopySelfIntoAlsoPlaying(Case other, Character speaker)
		{
			string speakerStageRange = null;
			int min = 0;
			int max = 0;
			if (Stages.Count > 0)
			{
				min = Stages.Min(stage => stage);
				max = Stages.Max(stage => stage);
			}
			TriggerDefinition trigger = TriggerDatabase.GetTrigger(Tag);
			if (TriggerDatabase.ToStandardStage(speaker, min) == trigger.StartStage && TriggerDatabase.ToStandardStage(speaker, max) == trigger.EndStage)
			{
				speakerStageRange = null;
			}
			else if (max != min)
			{
				speakerStageRange = min + "-" + max;
			}
			else
			{
				speakerStageRange = min.ToString();
			}

			TargetCondition cond = new TargetCondition();

			cond.Role = "other";
			cond.Character = speaker.FolderName;
			cond.Stage = speakerStageRange;

			foreach (TargetCondition tCond in Conditions)
			{
				if (String.IsNullOrEmpty(tCond.TimeInStage))
				{
					cond.TimeInStage = tCond.TimeInStage;
				}
				if (String.IsNullOrEmpty(tCond.ConsecutiveLosses))
				{
					cond.ConsecutiveLosses = tCond.ConsecutiveLosses;
				}
				if (String.IsNullOrEmpty(tCond.Hand))
				{
					cond.Hand = tCond.Hand;
				}
				if (String.IsNullOrEmpty(tCond.NotSaidMarker))
				{
					cond.NotSaidMarker = tCond.NotSaidMarker;
				}
				if (String.IsNullOrEmpty(tCond.SaidMarker))
				{
					cond.SaidMarker = tCond.SaidMarker;
				}
			}

			//If all lines set the same marker, use that marker in alsoPlayingSayingMarker
			if (Lines.Count > 0)
			{
				string marker = Lines[0].Marker;
				for (int l = 1; l < Lines.Count; l++)
				{
					DialogueLine line = Lines[l];
					if (line.Marker != marker)
					{
						marker = null;
						break;
					}
				}
				if (!string.IsNullOrEmpty(marker))
				{
					if (marker.StartsWith("+") || marker.StartsWith("-"))
					{
						marker = marker.Substring(1);
					}

					cond.SayingMarker = marker;

					foreach (TargetCondition theCond in other.Conditions)
					{
						if (theCond.Role == "other" && theCond.Character == speaker.FolderName && theCond.NotSaidMarker == marker)
						{
							//if they had a not said marker for the same thing, clear that
							theCond.NotSaidMarker = null;
						}
					}
				}
			}

			other.Conditions.Add(cond);

			string id = CharacterDatabase.GetId(speaker);
			foreach (ExpressionTest test in Expressions)
			{
				if (test.GetTarget() == "self")
				{
					ExpressionTest copy = test.Copy();
					copy.ChangeTarget(id);
					other.Expressions.Add(copy);
				}
			}
		}

		/// <summary>
		/// Copies this tag's AlsoPlaying properties into another's self-properties
		/// </summary>
		/// <param name="other"></param>
		private void CopyAlsoPlayingIntoSelf(Case other, Character responder)
		{
			string condStage = null;

			foreach (TargetCondition cond in Conditions)
			{
				if (cond.Role == "other" && !string.IsNullOrEmpty(cond.Stage))
				{
					condStage = cond.Stage;
					break;
				}
			}

			if (!string.IsNullOrEmpty(condStage))
			{
				int min, max;
				ToRange(condStage, out min, out max);
				for (int i = min; i <= max; i++)
				{
					other.Stages.Add(i);
				}
			}
			else
			{
				//all stages
				TriggerDefinition trigger = TriggerDatabase.GetTrigger(other.Tag);
				for (int i = 0; i < responder.Layers + Clothing.ExtraStages; i++)
				{
					int stage = TriggerDatabase.ToStandardStage(responder, i);
					if (stage >= trigger.StartStage && stage <= trigger.EndStage)
					{
						other.Stages.Add(i);
					}
				}
			}

			for (int i = Conditions.Count - 1; i >= 0; i--)
			{
				if (Conditions[i].Role != "other") { continue; }

				TargetCondition selfCond = new TargetCondition();
				selfCond.Role = "self";
				bool hasConds = false;

				if (!string.IsNullOrEmpty(Conditions[i].Hand))
				{
					selfCond.Hand = Conditions[i].Hand;
					hasConds = true;
				}
				if (!string.IsNullOrEmpty(Conditions[i].SaidMarker))
				{
					selfCond.SaidMarker = Conditions[i].SaidMarker;
					hasConds = true;
				}
				if (!string.IsNullOrEmpty(Conditions[i].NotSaidMarker))
				{
					selfCond.NotSaidMarker = Conditions[i].NotSaidMarker;
					hasConds = true;
				}
				if (!string.IsNullOrEmpty(Conditions[i].TimeInStage))
				{
					selfCond.TimeInStage = Conditions[i].TimeInStage;
					hasConds = true;
				}
				if (!string.IsNullOrEmpty(Conditions[i].ConsecutiveLosses))
				{
					selfCond.ConsecutiveLosses = Conditions[i].ConsecutiveLosses;
					hasConds = true;
				}

				if (hasConds)
				{
					Conditions.Add(selfCond);
				}
			}
		}

		/// <summary>
		/// Copies this tag's self-targeting properties into another's Target
		/// </summary>
		/// <param name="other"></param>
		private void CopySelfIntoTarget(Case other, Character speaker)
		{
			string speakerStageRange = null;
			int min = 0;
			int max = 0;
			if (Stages.Count > 0)
			{
				min = Stages.Min(stage => stage);
				max = Stages.Max(stage => stage);
			}
			TriggerDefinition trigger = TriggerDatabase.GetTrigger(Tag);
			if (TriggerDatabase.ToStandardStage(speaker, min) == trigger.StartStage && TriggerDatabase.ToStandardStage(speaker, max) == trigger.EndStage)
			{
				speakerStageRange = null;
			}
			else if (max != min)
			{
				speakerStageRange = min + "-" + max;
			}
			else
			{
				speakerStageRange = min.ToString();
			}

			if (other.Tag.Contains("crotch") || other.Tag.Contains("chest"))
			{
				//if there is only one important for these layers, don't both including a targetStage
				string position = (other.Tag.Contains("crotch") ? "lower" : "upper");
				int layerCount = speaker.GetConvertedWardrobe().Count(c => c.Position == position && c.Type == "important");
				if (layerCount == 1)
				{
					speakerStageRange = null;
				}
			}

			TargetCondition cond = new TargetCondition();

			cond.Role = "target";
			cond.Character = speaker.FolderName;
			cond.Stage = speakerStageRange;

			foreach (TargetCondition tCond in Conditions)
            {
				if(String.IsNullOrEmpty(tCond.TimeInStage))
                {
					cond.TimeInStage = tCond.TimeInStage;
                }
				if (String.IsNullOrEmpty(tCond.ConsecutiveLosses))
				{
					cond.ConsecutiveLosses = tCond.ConsecutiveLosses;
				}
				if (String.IsNullOrEmpty(tCond.Hand))
				{
					cond.Hand = tCond.Hand;
				}
				if (String.IsNullOrEmpty(tCond.NotSaidMarker))
				{
					cond.NotSaidMarker = tCond.NotSaidMarker;
				}
				if (String.IsNullOrEmpty(tCond.SaidMarker))
				{
					cond.SaidMarker = tCond.SaidMarker;
				}
			}

			//If all lines set the same marker, use that marker in alsoPlayingSayingMarker
			if (Lines.Count > 0)
			{
				string marker = Lines[0].Marker;
				for (int l = 1; l < Lines.Count; l++)
				{
					DialogueLine line = Lines[l];
					if (line.Marker != marker)
					{
						marker = null;
						break;
					}
				}
				if (!string.IsNullOrEmpty(marker))
				{
					if (marker.StartsWith("+") || marker.StartsWith("-"))
					{
						marker = marker.Substring(1);
					}
					cond.SayingMarker = marker;

					foreach (TargetCondition theCond in other.Conditions)
                    {
						if (theCond.Role == "target" && theCond.Character == speaker.FolderName && theCond.NotSaidMarker == marker)
                        {
							//if they had a not said marker for the same thing, clear that
							theCond.NotSaidMarker = null;
						}
                    }
				}
			}

			other.Conditions.Add(cond);

			foreach (ExpressionTest test in Expressions)
			{
				if (test.GetTarget() == "self")
				{
					ExpressionTest copy = test.Copy();
					copy.ChangeTarget("target");
					other.Expressions.Add(copy);
				}
			}
		}

		private void AdjustConditions(Character speaker, Character responder, Case sourceCase)
		{
			bool responseHasTarget = TriggerDatabase.GetTrigger(Tag).HasTarget;
			bool sourceHasTarget = TriggerDatabase.GetTrigger(sourceCase.Tag).HasTarget;

			for (int i = Conditions.Count - 1; i >= 0; i--)
			{
				TargetCondition cond = Conditions[i];
				if (cond.Role == "self")
				{
					bool adjustMarkers = false;
					if (responseHasTarget && !sourceHasTarget)
					{
						cond.Role = "target";
						cond.Character = speaker.FolderName;
						adjustMarkers = true;
					}
					else
					{
						cond.Role = "other";
						cond.Character = speaker.FolderName;
						adjustMarkers = true;
					}
					//If all lines set the same marker, use that marker in alsoPlayingSayingMarker
					if (adjustMarkers && sourceCase.Lines.Count > 0)
					{
						string marker = sourceCase.Lines[0].Marker;
						for (int l = 1; l < sourceCase.Lines.Count; l++)
						{
							DialogueLine line = sourceCase.Lines[l];
							if (line.Marker != marker)
							{
								marker = null;
								break;
							}
						}
						if (!string.IsNullOrEmpty(marker) && string.IsNullOrEmpty(cond.Count))
						{
							if (marker.StartsWith("+") || marker.StartsWith("-"))
							{
								marker = marker.Substring(1);
							}
							cond.SayingMarker = marker;
							if (cond.NotSaidMarker == marker)
							{
								//if they had a not said marker for the same thing, clear that
								cond.NotSaidMarker = null;
							}
						}
					}
				}
				else if (cond.Role == "target")
				{
					if (cond.Character == responder.FolderName)
					{
						//responder was target. If they weren't we can leave this as is
						cond.Role = "self";
						cond.Character = null;
					}
				}
				else
				{
					if (cond.Character == responder.FolderName)
					{
						//responder was Also Playing
						cond.Role = "self";
						cond.Character = null;
					}
				}

				if (cond.Role == "self")
				{
					//move self stage checks to the case's actual stages
					if (!string.IsNullOrEmpty(cond.Stage))
					{
						int min, max;
						ToRange(cond.Stage, out min, out max);
						Stages.Clear();
						AddStageRange(min, max);
						cond.Stage = null;
					}
				}
				if (cond.IsEmpty)
				{
					Conditions.RemoveAt(i);
				}
			}

			// compact conditions
			for (int i = Conditions.Count - 1; i >= 0; i--)
			{
				for (int j = i - 1; j >= 0; j--)
				{
					if (Conditions[j].Role == Conditions[i].Role)
                    {
						if (Conditions[j].Role == "self" || Conditions[j].Character == Conditions[i].Character)
                        {
							if (String.IsNullOrEmpty(Conditions[j].Status) || Conditions[j].Status == Conditions[i].Status)
                            {
								Conditions[j].Status = Conditions[i].Status;
								Conditions[i].Status = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].Stage) || Conditions[j].Stage == Conditions[i].Stage)
							{
								Conditions[j].Stage = Conditions[i].Stage;
								Conditions[i].Stage = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].TimeInStage) || Conditions[j].TimeInStage == Conditions[i].TimeInStage)
							{
								Conditions[j].TimeInStage = Conditions[i].TimeInStage;
								Conditions[i].TimeInStage = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].ConsecutiveLosses) || Conditions[j].ConsecutiveLosses == Conditions[i].ConsecutiveLosses)
							{
								Conditions[j].ConsecutiveLosses = Conditions[i].ConsecutiveLosses;
								Conditions[i].ConsecutiveLosses = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].SaidMarker) || Conditions[j].SaidMarker == Conditions[i].SaidMarker)
							{
								Conditions[j].SaidMarker = Conditions[i].SaidMarker;
								Conditions[i].SaidMarker = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].SayingMarker) || Conditions[j].SayingMarker == Conditions[i].SayingMarker)
							{
								Conditions[j].SayingMarker = Conditions[i].SayingMarker;
								Conditions[i].SayingMarker = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].NotSaidMarker) || Conditions[j].NotSaidMarker == Conditions[i].NotSaidMarker)
							{
								Conditions[j].NotSaidMarker = Conditions[i].NotSaidMarker;
								Conditions[i].NotSaidMarker = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].Saying) || Conditions[j].Saying == Conditions[i].Saying)
							{
								Conditions[j].Saying = Conditions[i].Saying;
								Conditions[i].Saying = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].Hand) || Conditions[j].Hand == Conditions[i].Hand)
							{
								Conditions[j].Hand = Conditions[i].Hand;
								Conditions[i].Hand = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].Layers) || Conditions[j].Layers == Conditions[i].Layers)
							{
								Conditions[j].Layers = Conditions[i].Layers;
								Conditions[i].Layers = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].StartingLayers) || Conditions[j].StartingLayers == Conditions[i].StartingLayers)
							{
								Conditions[j].StartingLayers = Conditions[i].StartingLayers;
								Conditions[i].StartingLayers = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].Gender) || Conditions[j].Gender == Conditions[i].Gender)
							{
								Conditions[j].Gender = Conditions[i].Gender;
								Conditions[i].Gender = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].FilterTag) || Conditions[j].FilterTag == Conditions[i].FilterTag)
							{
								Conditions[j].FilterTag = Conditions[i].FilterTag;
								Conditions[i].FilterTag = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].FilterNotTag) || Conditions[j].FilterNotTag == Conditions[i].FilterNotTag)
							{
								Conditions[j].FilterNotTag = Conditions[i].FilterNotTag;
								Conditions[i].FilterNotTag = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].FilterTagAdv) || Conditions[j].FilterTagAdv == Conditions[i].FilterTagAdv)
							{
								Conditions[j].FilterTagAdv = Conditions[i].FilterTagAdv;
								Conditions[i].FilterTagAdv = null;
							}

							if (String.IsNullOrEmpty(Conditions[j].Pose) || Conditions[j].Pose == Conditions[i].Pose)
							{
								Conditions[j].Pose = Conditions[i].Pose;
								Conditions[i].Pose = null;
							}

							if (!Conditions[i].HasAdvancedConditions)
							{
								Conditions.RemoveAt(i);
								i--;
								if (i < 0) { break; }
							}
						}
                    }
				}
			}
		}

		/// <summary>
		/// Gets the stage in which a character's position is revealed
		/// </summary>
		/// <param name="speaker"></param>
		/// <returns></returns>
		private int GetRevealStage(Character speaker, string position)
		{
			int last = 0;
			for (int i = speaker.Wardrobe.Count - 1; i >= 0; i--)
			{
				Clothing layer = speaker.Wardrobe[i];
				if (layer.Type == "major" || layer.Type == "important")
				{
					if (layer.Position == position || layer.Position == "both")
					{
						last = i;
					}
				}
			}
			return speaker.Wardrobe.Count - last;
		}

		/// <summary>
		/// Translates a layer type and position to a word used in a trigger
		/// </summary>
		/// <remarks>This really doesn't belong in the Case class</remarks>
		/// <param name="speaker"></param>
		/// <param name="stage"></param>
		/// <returns></returns>
		private string GetLayerType(Character speaker, int stage)
		{
			Clothing layer = speaker.Wardrobe[speaker.Layers - stage - 1];
			string layerType = layer.Type;
			if (layer.Type == "major")
			{
				//if this is the last major and there are no importants, treat as important
				bool hasUpperImportant = false;
				bool hasLowerImportant = false;
				for (int l = speaker.Layers - stage - 2; l >= 0; l--)
				{
					Clothing other = speaker.Wardrobe[l];
					if (other.Type == "important" || other.Type == "major")
					{
						if (other.Position == "both")
						{
							hasUpperImportant = true;
							hasLowerImportant = true;
						}
						else if (other.Position == "upper")
						{
							hasUpperImportant = true;
						}
						else if (other.Position == "lower")
						{
							hasLowerImportant = true;
						}
					}
				}
				if (!hasLowerImportant)
				{
					if (layer.Position == "both" || layer.Position == "lower")
					{
						return "crotch";
					}
				}
				if (!hasUpperImportant)
				{
					if (layer.Position == "both" || layer.Position == "upper")
					{
						return "chest";
					}
				}
			}
			if (layerType == "extra")
			{
				return "accessory";
			}
			else if (layerType == "important")
			{
				if (layer.Position == "lower" || layer.Position == "both")
				{
					return "crotch";
				}
				else if (layer.Position == "upper")
				{
					return "chest";
				}
			}
			return layerType;
		}

		/// <summary>
		/// Gets the tag needed for a responder to target this case.
		/// </summary>
		/// <param name="speaker"></param>
		/// <param name="responder"></param>
		/// <returns></returns>
		public string GetResponseTag(Character speaker, Character responder)
		{
			string gender = speaker.Gender;

			//First handle tags where the speaker is actively doing something, since these are the easiest to handle
			if (Tag == "stripping")
			{
				int stage = Stages.Min(s => s);
				string layer = GetLayerType(speaker, stage);
				if (layer == "major" || layer == "minor" || layer == "accessory")
				{
					if (speaker.Metadata.CrossGender)
					{
						return "opponent_stripping";
					}
					return $"{gender}_removing_{layer}";
				}
				else
				{
					if (speaker.Metadata.CrossGender)
					{
						return $"opponent_{layer}_will_be_visible";
					}
					return $"{gender}_{layer}_will_be_visible";
				}
			}
			else if (Tag == "stripped")
			{
				int stage = Stages.Min(s => s);
				string layer = GetLayerType(speaker, stage - 1);
				if (layer == "major" || layer == "minor" || layer == "accessory")
				{
					if (speaker.Metadata.CrossGender)
					{
						return "opponent_stripped";
					}
					return $"{gender}_removed_{layer}";
				}
				else
				{
					if (speaker.Metadata.CrossGender)
					{
						return $"opponent_{layer}_is_visible";
					}
					else if (gender == "female" && layer == "chest" || gender == "male" && layer == "crotch")
					{
						return $"{gender}_{speaker.Size}_{layer}_is_visible";
					}
					else
					{
						return $"{gender}_{layer}_is_visible";
					}
				}
			}
			else if (Tag == "must_masturbate_first")
			{
				if (speaker.Metadata.CrossGender)
				{
					return "opponent_lost";
				}
				return $"{gender}_must_masturbate";
			}
			else if (Tag == "must_masturbate")
			{
				if (speaker.Metadata.CrossGender)
				{
					return "opponent_lost";
				}
				return $"{gender}_must_masturbate";
			}
			else if (Tag == "start_masturbating" || Tag == "masturbating" || Tag == "finished_masturbating" || Tag == "heavy_masturbating")
			{
				if (speaker.Metadata.CrossGender)
				{
					return $"opponent_{Tag}";
				}
				return $"{gender}_{Tag}";
			}
			else if (Tag != null && Tag.StartsWith("must_strip"))
			{
				if (speaker.Metadata.CrossGender)
				{
					return "opponent_lost";
				}
				return $"{gender}_must_strip";
			}
			else if (Tag == "game_over_victory")
			{
				return "game_over_defeat";
			}
			else if (Tag == "after_masturbating")
			{
				return "hand";
			}
			else if (Tag == "selected")
			{
				return "opponent_selected";
			}

			string tag = Tag;
			if (tag != null && tag.StartsWith("female_"))
			{
				tag = tag.Substring(7);
			}
			else if (tag != null && tag.StartsWith("male_"))
			{
				tag = tag.Substring(5);
			}
			else if (tag != null && tag.StartsWith("opponent_") && tag != "opponent_selected")
			{
				tag = tag.Substring(9);
			}

			string target = "";
			foreach (TargetCondition condition in Conditions)
			{
				if (condition.Role == "target")
				{
					target = condition.Character;
					break;
				}
			}

			if (target == responder.FolderName)
			{
				//addressing responder directly, meaning the responder must be doing some action
				if (tag.Contains("will_be_visible") || tag.Contains("removing"))
				{
					return "stripping";
				}
				else if (tag.Contains("is_visible") || tag.Contains("removed"))
				{
					return "stripped";
				}
				else if (tag == "must_strip")
				{
					return "must_strip";
				}
				else if (tag.Contains("masturbat"))
				{
					return tag;
				}
				else if (tag == "lost")
				{
					return "must_strip";
				}
				else if (tag == "stripping")
				{
					return "stripping";
				}
				else if (tag == "stripped")
				{
					return "stripped";
				}
				else if (tag == "game_over_defeat")
				{
					return "game_over_victory";
				}
				else if (tag == "opponent_selected")
				{
					return "selected";
				}
			}

			if (tag == "good_hand" || tag == "okay_hand" || tag == "bad_hand")
			{
				return "hand";
			}

			if (tag == "finishing_masturbating")
			{
				return null;
			}

			return Tag; //if nothing above applied, the speaker is reacting to some event unrelated to the responder, so the responder can target the same thing
		}

		/// <summary>
		/// Gets a list of cases that could potentially match up with the given source tag. This could be lines that the sourceCase is responding to, or lines that are responding to the sourceCase
		/// </summary>
		/// <remarks>This assumes the sourceCase is targeting the other character, either directly or through a tag</remarks>
		/// <param name="sourceCase">Case for which to get matches</param>
		/// <param name="speaker">The character speaking this case</param>
		/// <param name="other">Speaker of the matching cases</param>
		/// <returns></returns>
		public static List<Case> GetMatchingCases(Case sourceCase, Character speaker, Character other)
		{
			List<Case> cases = new List<Case>();

			//first create a response that targets it exactly
			Case response = sourceCase.CreateResponse(speaker, other);
			if (response == null)
			{
				//if that failed, the case is too specific to respond to exactly, so create a matching case with the bare minimum of requirements
				response = new Case(sourceCase.GetResponseTag(speaker, other));
				if (response == null)
				{
					//if that failed, there is no way to respond to this tag
					return cases;
				}
			}

			//now get a list of cases that fit the bare minimum requirements of that response (i.e. the tag and stages)
			List<Case> possibleCases = other.Behavior.GetWorkingCases().Where(c => TriggerDatabase.AreRelated(c.Tag, response.Tag) && c.Stages.Min() >= response.Stages.Min() && c.Stages.Max() <= response.Stages.Max()).ToList();

			//now start eliminating cases that aren't possible (or at least very unlikely) based on the source's conditions
			foreach (Case c in possibleCases)
			{
				bool foundConflict = false;

				foreach (TargetCondition cond in c.Conditions)
                {
					if (cond.Role != "self" && cond.Role != "target")
                    {
						foreach (TargetCondition otherCond in sourceCase.Conditions)
                        {
							if (cond.Role == otherCond.Role && cond.Character != otherCond.Character)
                            {
								foundConflict = true; //conflict; they're checking for different also playings. Okay, it's not technically a conflict, but it's highly unlikely they want to respond to this case
								break;
                            }
                        }

						if (foundConflict) { break; }
                    }

					if (cond.Role == "self")
                    {
						foreach (TargetCondition otherCond in sourceCase.Conditions)
						{
							if (otherCond.Role != "target") { continue; }

							if (!string.IsNullOrEmpty(otherCond.SaidMarker) && cond.NotSaidMarker == otherCond.SaidMarker)
                            {
								foundConflict = true; //if they're looking for a marker and the other is looking for not having the marker, then it's a clear conflict of interests
								break;
							}

							if (!string.IsNullOrEmpty(cond.SaidMarker) && !string.IsNullOrEmpty(otherCond.SaidMarker) && cond.SaidMarker != otherCond.SaidMarker)
							{
								foundConflict = true; //Other is checking for a marker that the source isn't, which makes it unlikely since otherwise they should be targeting the marker too
								break;
							}

							if (cond.Hand != otherCond.Hand)
							{
								foundConflict = true;
								break;
							}

							if (cond.TimeInStage != otherCond.TimeInStage)
							{
								foundConflict = true;
								break;
							}

							if (!string.IsNullOrEmpty(cond.FilterTag) && speaker.Tags.Find(t => t.Tag == cond.FilterTag) == null)
							{
								foundConflict = true;
								break;
							}

							if (!string.IsNullOrEmpty(cond.FilterNotTag) && speaker.Tags.Find(t => t.Tag == cond.FilterNotTag) != null)
							{
								foundConflict = true;
								break;
							}

							if (!string.IsNullOrEmpty(cond.Character) && cond.Character != speaker.FolderName && cond.Character != otherCond.Character)
							{
								foundConflict = true;
								break;
							}

							if (!string.IsNullOrEmpty(c.TotalRounds) && c.TotalRounds != sourceCase.TotalRounds)
							{
								foundConflict = true;
								break;
							}
						}

						if (foundConflict) { break; }
					}
                }

				if (foundConflict) { continue; }

				cases.Add(c);
			}

			cases.Sort((c1, c2) => { return c2.GetPriority().CompareTo(c1.GetPriority()); });
			return cases;
		}

		/// <summary>
		/// Gets whether the table conditions match between two cases
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool MatchesTableConditions(Case other)
		{
			bool match = (other.TotalRounds == TotalRounds);
			return match;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		private bool FilterTargetByCase(IRecord record)
		{
			Character character = record as Character;
			TriggerDefinition trigger = TriggerDatabase.GetTrigger(Tag);

			if (character.Key == "human")
			{
				return true;
			}

			if (trigger.Size != null && trigger.Size != character.Size)
			{
				return false;
			}

			if (trigger.Gender == null || trigger.Gender == character.Gender || character.Metadata.CrossGender)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets whether at least one line touches a collectible
		/// </summary>
		public bool HasCollectible
		{
			get
			{
				foreach (DialogueLine line in Lines)
				{
					if (!string.IsNullOrEmpty(line.CollectibleId))
					{
						return true;
					}
				}
				return false;
			}
		}

		[OnDeserialized]
		public void OnDeserialized(StreamingContext context)
		{
			if (!string.IsNullOrEmpty(Stage))
			{
				Tuple<int, int> interval = GUIHelper.ToInterval(Stage);
				for (int i = interval.Item1; i <= interval.Item2; i++)
				{
					Stages.Add(i);
				}
			}
		}

		/// <summary>
		/// Gets a list of markers that this case assumes are being said
		/// </summary>
		/// <returns></returns>
		public List<string> GetMarkers()
		{
			List<string> list = new List<string>();

			foreach (TargetCondition cond in Conditions)
            {
				if (cond.Role == "self" && !string.IsNullOrEmpty(cond.SaidMarker) && (!cond.SaidMarker.Contains("!=") || cond.SaidMarker.EndsWith("!=0")) && !cond.SaidMarker.EndsWith("==0"))
                {
					int splitIndex = cond.SaidMarker.IndexOfAny(new char[] { '=', '>', '<', '!' });
					if (splitIndex > 0)
					{
						list.Add(cond.SaidMarker.Substring(0, splitIndex));
					}
					else
					{
						list.Add(cond.SaidMarker);
					}
				}
            }
			foreach (ExpressionTest test in Expressions)
			{
				if (test.Expression.StartsWith("~marker.") || test.Expression.StartsWith("~self.marker."))
				{
					if (test.Operator != "!=" || (test.Operator == "!=" && test.Value == "0"))
					{
						int dot = test.Expression.LastIndexOf('.');
						if (dot >= 0)
						{
							string marker = test.Expression.Substring(dot + 1, test.Expression.Length - (dot + 2));
							list.Add(marker);
						}
					}
				}
			}
			return list;
		}

		public int GetSliceCount()
		{
			return Lines.Count;
		}

		public void AddStage(int stage)
		{
			for (int i = 0; i < Stages.Count; i++)
			{
				if (Stages[i] > stage)
				{
					Stages.Insert(i, stage);
					NotifyPropertyChanged(nameof(Stages));
					return;
				}
				else if (Stages[i] == stage)
				{
					return;
				}
			}
			Stages.Add(stage);
			NotifyPropertyChanged(nameof(Stages));
		}

		public void AddStages(IEnumerable<int> stages)
		{
			HashSet<int> current = new HashSet<int>();
			foreach (int s in Stages)
			{
				current.Add(s);
			}
			bool modified = false;
			foreach (int s in stages)
			{
				if (!current.Contains(s))
				{
					Stages.Add(s);
					modified = true;
				}
			}
			if (modified)
			{
				Stages.Sort();
				NotifyPropertyChanged(nameof(Stages));
			}
		}

		public void AddStageRange(int start, int end)
		{
			HashSet<int> current = new HashSet<int>();
			foreach (int s in Stages)
			{
				current.Add(s);
			}
			bool modified = false;
			for (int s = start; s <= end; s++)
			{
				if (!current.Contains(s))
				{
					Stages.Add(s);
					modified = true;
				}
			}
			if (modified)
			{
				Stages.Sort();
				NotifyPropertyChanged(nameof(Stages));
			}
		}

		public void RemoveStage(int stage)
		{
			for (int i = 0; i < Stages.Count; i++)
			{
				if (Stages[i] == stage)
				{
					Stages.RemoveAt(i);
					NotifyPropertyChanged(nameof(Stages));
					return;
				}
			}
		}

		public void ClearStages()
		{
			if (Stages.Count > 0)
			{
				Stages.Clear();
				NotifyPropertyChanged(nameof(Stages));
			}
		}

		/// <summary>
		/// Gets a set of all character IDs being targeted by this case
		/// </summary>
		/// <returns></returns>
		public HashSet<string> GetTargets()
		{
			HashSet<string> set = new HashSet<string>();
			foreach (TargetCondition condition in Conditions)
			{
				if (condition.Count == "0" || condition.Count == "0-0")
				{
					continue;
				}
				if (!string.IsNullOrEmpty(condition.Character))
				{
					Character c = CharacterDatabase.Get(condition.Character);
					if (c != null)
					{
						set.Add(c.FolderName);
					}
				}
				if (!string.IsNullOrEmpty(condition.FilterTag))
				{
					Character c = CharacterDatabase.GetById(condition.FilterTag);
					if (c != null)
					{
						set.Add(c.FolderName);
					}
				}
			}
			foreach (Case c in AlternativeConditions)
			{
				set.AddRange(c.GetTargets());
			}
			return set;
		}

		/// <summary>
		/// Gets whether this case uses any old attribute-style conditions
		/// </summary>
		/// <returns></returns>
		public bool HasLegacyConditions()
		{
			return false;
			/*return !string.IsNullOrEmpty(Target) ||
				!string.IsNullOrEmpty(Filter) ||
				!string.IsNullOrEmpty(FilterOut) ||
                !string.IsNullOrEmpty(FilterAdv) ||
                !string.IsNullOrEmpty(TargetStage) ||
				!string.IsNullOrEmpty(TargetHand) ||
				!string.IsNullOrEmpty(TargetLayers) ||
				!string.IsNullOrEmpty(TargetStatus) ||
				!string.IsNullOrEmpty(TargetSaidMarker) ||
				!string.IsNullOrEmpty(TargetNotSaidMarker) ||
				!string.IsNullOrEmpty(TargetSayingMarker) ||
				!string.IsNullOrEmpty(TargetSaying) ||
				!string.IsNullOrEmpty(TargetStartingLayers) ||
				!string.IsNullOrEmpty(TargetTimeInStage) ||
				!string.IsNullOrEmpty(ConsecutiveLosses) ||
				!string.IsNullOrEmpty(HasHand) ||
				!string.IsNullOrEmpty(SaidMarker) ||
				!string.IsNullOrEmpty(NotSaidMarker) ||
				!string.IsNullOrEmpty(TimeInStage) ||
				!string.IsNullOrEmpty(AlsoPlaying) ||
				!string.IsNullOrEmpty(AlsoPlayingStage) ||
				!string.IsNullOrEmpty(AlsoPlayingHand) ||
				!string.IsNullOrEmpty(AlsoPlayingSaidMarker) ||
				!string.IsNullOrEmpty(AlsoPlayingNotSaidMarker) ||
				!string.IsNullOrEmpty(AlsoPlayingSayingMarker) ||
				!string.IsNullOrEmpty(AlsoPlayingSaying) ||
				!string.IsNullOrEmpty(AlsoPlayingSaid) ||
				!string.IsNullOrEmpty(AlsoPlayingTimeInStage) ||
				!string.IsNullOrEmpty(TotalMales) ||
				!string.IsNullOrEmpty(TotalPlaying) ||
				!string.IsNullOrEmpty(TotalFinished) ||
				!string.IsNullOrEmpty(TotalNaked) ||
				!string.IsNullOrEmpty(TotalMasturbating) ||
				!string.IsNullOrEmpty(TotalExposed) ||
				!string.IsNullOrEmpty(TotalFemales);*/
		}

		public string GetStageRange(Character target)
		{
			if (target == null) { return ""; }
			foreach (TargetCondition condition in Conditions)
			{
				if (condition.Character == target.FolderName)
				{
					return condition.Stage ?? "";
				}
			}
			foreach (Case alternate in AlternativeConditions)
			{
				string range = alternate.GetStageRange(target);
				if (!string.IsNullOrEmpty(range))
				{
					return range;
				}
			}
			return "";
		}

		/// <summary>
		/// Gets this case's target
		/// </summary>
		/// <returns></returns>
		public string GetTarget()
		{
			foreach (TargetCondition test in Conditions)
			{
				if (test.Role == "target")
				{
					return test.Character;
				}
			}

			foreach (Case alt in AlternativeConditions)
			{
				string target = alt.GetTarget();
				if (!string.IsNullOrEmpty(target))
				{
					return target;
				}
			}
			return null;
		}

		/// <summary>
		/// Removes extraneous conditions to leave only the bare minimum
		/// </summary>
		public void SimplifyConditions()
		{
			for (int i = Conditions.Count - 1; i>= 0; i--)
			{
				TargetCondition condition = Conditions[i];
				if (string.IsNullOrEmpty(condition.SayingMarker))
				{
					Conditions.RemoveAt(i);
				}
				else
				{
					condition.Simplify();
				}
			}
			foreach (Case alt in AlternativeConditions)
			{
				alt.SimplifyConditions();
			}
		}
	}

	public static class Extensions
	{
		public static int ToInt(this string value)
		{
			if (value == null)
				return 0;
			string[] pieces = value.Split('-');
			string val = pieces[0];
			int result;
			int.TryParse(val, out result);
			return result;
		}
	}
}
