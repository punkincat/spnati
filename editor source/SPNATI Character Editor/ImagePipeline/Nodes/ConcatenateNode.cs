using System;
using System.Drawing;
using System.Threading.Tasks;
using SPNATI_Character_Editor;
using SPNATI_Character_Editor.DataStructures;

namespace ImagePipeline
{
	public class ConcatenateNode : NodeDefinition
	{
		public override string Name
		{
			get { return "Concatenate"; }
		}

		public override string Key
		{
			get { return "concatenate"; }
			set { }
		}

		public override string Group { get { return "Utility"; } }

		public override string Description { get { return "Joins two strings end-to-end."; } }

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
			return new NodeProperty[] { };
		}

		public override Task<PipelineResult> Process(PipelineArgs args)
		{
			if (!args.HasInput())
			{
				return Task.FromResult(new PipelineResult(null));
			}
			string a = args.GetInput<string>(0) ?? "";
			string b = args.GetInput<string>(1) ?? "";
			return Task.FromResult(new PipelineResult(a + b));
		}
	}
}
