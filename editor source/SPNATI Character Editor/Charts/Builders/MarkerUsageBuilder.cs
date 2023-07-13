using System;
using System.Collections.Generic;

namespace SPNATI_Character_Editor.Charts.Builders
{
	[Chart(ChartType.Bar, 21)]
	public class MarkerUsageBuilder : GenderedBuilder
	{
		public override string GetLabel()
		{
			return "Markers (Consumed)";
		}

		public override string GetTitle()
		{
			return "Markers Referenced by Other Characters";
		}

		protected override List<Tuple<Character, int>> GetData()
		{
			var data = new List<Tuple<Character, int>>();
			Dictionary<string, HashSet<string>> markers = new Dictionary<string, HashSet<string>>();
			foreach (Character c in CharacterDatabase.Characters)
			{
				foreach (Case stageCase in c.Behavior.EnumerateSourceCases())
				{
					foreach (TargetCondition cond in stageCase.Conditions)
					{
						if (!string.IsNullOrEmpty(cond.Character) && CharacterDatabase.Exists(cond.Character))
						{
							TrackMarker(markers, cond.Character, cond.SaidMarker);
							TrackMarker(markers, cond.Character, cond.NotSaidMarker);
							TrackMarker(markers, cond.Character, cond.SayingMarker);
						}
					}
				}
			}
			foreach (var kvp in markers)
			{
				data.Add(new Tuple<Character, int>(CharacterDatabase.Get(kvp.Key), kvp.Value.Count));
			}
			return data;
		}

		private static void TrackMarker(Dictionary<string, HashSet<string>> markers, string target, string marker)
		{
			HashSet<string> set;
			MarkerOperator op;
			string value;
			bool perTarget;
			if (marker == null) { return; }
			marker = Marker.ExtractConditionPieces(marker, out op, out value, out perTarget);
			if (!markers.TryGetValue(target, out set))
			{
				set = new HashSet<string>();
				markers[target] = set;
			}
			if (!set.Contains(marker))
			{
				//first time this character has used the marker
				set.Add(marker);
			}
		}
	}
}
