using System;
using System.Drawing;
using System.Threading.Tasks;
using SPNATI_Character_Editor;
using SPNATI_Character_Editor.DataStructures;

namespace ImagePipeline
{
	public class CompareNode : NodeDefinition
	{
		public override string Name
		{
			get { return "Compare"; }
		}

		public override string Key
		{
			get { return "compare"; }
			set { }
		}

		public override string Group { get { return "Utility"; } }

		public override string Description { get { return "Compares two numbers."; } }

		public override PortDefinition[] GetInputs()
		{
			return new PortDefinition[] {
				new PortDefinition(PortType.String, "first"),
				new PortDefinition(PortType.String, "second")
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
				new NodeProperty(NodePropertyType.Integer, "Comparison", Comparison.Equal, typeof(Comparison)),
				new NodeProperty(NodePropertyType.Boolean, "Not")
			};
		}

		public override Task<PipelineResult> Process(PipelineArgs args)
		{
			if (!args.HasInput())
			{
				return Task.FromResult(new PipelineResult(null));
			}

			double a;
			if (!double.TryParse(args.GetInput<string>(0), out a))
			{
				a = 0.0;
			}
			double b;
			if (!double.TryParse(args.GetInput<string>(1), out b))
			{
				b = 0.0;
			}

			bool result;
			switch (args.GetProperty<Comparison>(0))
			{
				case Comparison.Equal:
					result = a == b;
					break;
				case Comparison.Greater:
					result = a > b;
					break;
				case Comparison.Less:
					result = a < b;
					break;
				default:
					result = false;
					break;
			}
			if (args.GetProperty<bool>(1))
			{
				result = !result;
			}
			return Task.FromResult(new PipelineResult(result.ToString()));
		}
	}

	public enum Comparison
	{
		Equal,
		Greater,
		Less
	}
}
