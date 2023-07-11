using System;
using System.Drawing;
using System.Threading.Tasks;
using SPNATI_Character_Editor;
using SPNATI_Character_Editor.DataStructures;

namespace ImagePipeline
{
	public class ArithmeticNode : NodeDefinition
	{
		public override string Name
		{
			get { return "Arithmetic"; }
		}

		public override string Key
		{
			get { return "arithmetic"; }
			set { }
		}

		public override string Group { get { return "Utility"; } }

		public override string Description { get { return "Adds, subtracts, multiplies, or divides two numbers."; } }

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
				new NodeProperty(NodePropertyType.Integer, "Operation", ArithmeticOperation.Add, typeof(ArithmeticOperation))
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

			double result;
			switch (args.GetProperty<ArithmeticOperation>(0))
			{
				case ArithmeticOperation.Add:
					result = a + b;
					break;
				case ArithmeticOperation.Subtract:
					result = a - b;
					break;
				case ArithmeticOperation.Multiply:
					result = a * b;
					break;
				case ArithmeticOperation.Divide:
					result = a / b;
					break;
				default:
					result = 0.0;
					break;
			}
			return Task.FromResult(new PipelineResult(result.ToString()));
		}
	}

	public enum ArithmeticOperation
	{
		Add,
		Subtract,
		Multiply,
		Divide
	}
}
