using SPNATI_Character_Editor.IO;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SPNATI_Character_Editor
{
	/// <summary>
	/// Data representation of a single dialogue stage
	/// </summary>
	public class Stage
	{
		public const int Default = 99;

		[XmlAttribute("id")]
		public int Id;

		[XmlSortMethod("SortCases")]
		[XmlElement("case")]
		public List<Case> Cases;

		public Stage()
		{
			Cases = new List<Case>();
		}

		public Stage(int id)
		{
			Id = id;
			Cases = new List<Case>();
		}

		/// <summary>
		/// Sort method for XML output, which differs from editor sort order. This should sort in the same order as make_xml.py
		/// </summary>
		/// <param name="obj1"></param>
		/// <param name="obj2"></param>
		/// <returns></returns>
		public int SortCases(object obj1, object obj2)
		{
			Case c1 = obj1 as Case;
			Case c2 = obj2 as Case;
			TriggerDefinition t1 = TriggerDatabase.GetTrigger(c1.Tag);
			TriggerDefinition t2 = TriggerDatabase.GetTrigger(c2.Tag);
			int compare = c1.Stages[0].CompareTo(c2.Stages[0]);
			if (compare == 0)
			{
				compare = t1.Group.CompareTo(t2.Group);
			}
			if (compare == 0)
			{
				compare = t1.GroupOrder.CompareTo(t2.GroupOrder);
			}
			if (compare == 0)
			{
				//Sort targeted stuff
				string sortKey1 = GetSortKey(c1);
				string sortKey2 = GetSortKey(c2);
				compare = sortKey1.CompareTo(sortKey2);
			}
			return compare;
		}

		/// <summary>
		/// Generates a sort key like make_xml.py does. This is to get the cases sorted the same way make_xml.py does. Otherwise diffs can be tricky
		/// </summary>
		/// <param name="c1"></param>
		/// <returns></returns>
		public static string GetSortKey(Case c1)
		{
			//TODO: Would it be worth pre-computing this whenever a case gets saved?
			//Not really. 1100 lines spent 1000ms on serialization and only 10ms sorting.

			List<string> filters = new List<string>();
			StringBuilder sb = new StringBuilder();
			foreach (TargetCondition condition in c1.Conditions)
			{
				sb.Clear();
				sb.Append("count-");
				//note: make_xml's create_case_xml() only addds the filter part, so it's still ambiguous for other conditions.
				if (!string.IsNullOrEmpty(condition.FilterTag))
				{
					sb.Append(condition.FilterTag);
				}
				if (!string.IsNullOrEmpty(condition.Gender))
				{
					sb.Append(condition.Gender);
				}
				if (!string.IsNullOrEmpty(condition.Count))
				{
					sb.Append(condition.Count);
				}
				if (!string.IsNullOrEmpty(condition.Status))
				{
					sb.Append(condition.Status);
				}
				filters.Add(sb.ToString());
			}
			foreach (ExpressionTest test in c1.Expressions)
			{
				filters.Add("test:" + test.ToString());
			}

			//the order of the following mess comes from all_targets in make_xml.py

			//one_word_targets
			if (!string.IsNullOrEmpty(c1.Hidden))
				filters.Add("hidden:" + c1.Hidden);
			if (!string.IsNullOrEmpty(c1.Disabled))
				filters.Add("disabled:" + c1.Disabled);

			//multi_word_targets
			if (!string.IsNullOrEmpty(c1.TotalRounds))
				filters.Add("totalrounds:" + c1.TotalRounds);
			if (!string.IsNullOrEmpty(c1.CustomPriority))
				filters.Add("priority:" + c1.CustomPriority);


			return string.Join(",", filters);
		}
	}
}
