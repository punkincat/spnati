using System;
using System.Drawing;
using System.Threading.Tasks;
using SPNATI_Character_Editor;
using SPNATI_Character_Editor.DataStructures;

namespace ImagePipeline
{
	public class SubstringNode : NodeDefinition
	{
		public override string Name
		{
			get { return "Substring"; }
		}

		public override string Key
		{
			get { return "substring"; }
			set { }
		}

		public override string Group { get { return "Utility"; } }

		public override string Description { get { return "Extracts part of a string"; } }

		public override PortDefinition[] GetInputs()
		{
			return new PortDefinition[] {
				new PortDefinition(PortType.String, "in")
			};
		}

		public override PortDefinition[] GetOutputs()
		{
			return new PortDefinition[] {
				new PortDefinition(PortType.String, "out")
			};
		}

		public override NodeProperty[] GetProperties()
		{
			return new NodeProperty[] {
				new NodeProperty(NodePropertyType.Integer, "start"),
				new NodeProperty(NodePropertyType.Integer, "end")
			};
		}

		private int Clamp(int n, int low, int high)
		{
			if (n < low)
			{
				return low;
			} 
			else if (n > high)
			{
				return high;
			}
			else
			{
				return n;
			}
		}

		public override Task<PipelineResult> Process(PipelineArgs args)
		{
			if (!args.HasInput())
			{
				return Task.FromResult(new PipelineResult(null));
			}

			string str = args.GetInput<string>(0) ?? "";
			int start = Clamp(args.GetProperty<int>(0), 0, str.Length);
			int end = Clamp(args.GetProperty<int>(1), 0, str.Length);
			return Task.FromResult(new PipelineResult(str.Substring(start, end)));
		}
	}
}
