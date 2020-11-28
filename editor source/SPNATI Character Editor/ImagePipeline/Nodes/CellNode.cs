﻿using ImagePipeline;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPNATI_Character_Editor.DataStructures;

namespace SPNATI_Character_Editor.ImagePipeline.Nodes
{
	public class CellNode : NodeDefinition
	{
		public override string Name
		{
			get { return "Cell"; }
		}

		public override string Key { get { return "cell"; } set { } }

		public override PortDefinition[] GetInputs()
		{
			return null;
		}

		public override PortDefinition[] GetOutputs()
		{
			return new PortDefinition[] {
				new PortDefinition(PortType.Bitmap, "image"),
				new PortDefinition(PortType.String, "key")
			};
		}

		public override NodeProperty[] GetProperties()
		{
			return null;
		}

		public override async Task<PipelineResult> Process(PipelineArgs args)
		{
			PoseEntry cell = args.Context.Cell;
			if (cell == null)
			{
				return new PipelineResult(null, null);
			}

			PoseMatrix matrix = cell.Stage.Sheet.Matrix;
			FileStatus status = matrix.GetStatus(cell, "raw-");

			bool cached = args.Context.Settings.Cache.ContainsKey("CellGenerated");

			if ((status != FileStatus.Missing && args.Context.Settings.PreviewMode && cached) 
				|| (status == FileStatus.Imported && !args.Context.Settings.DisallowCache))
			{
				//if it's already imported, load it from disk
				string path = matrix.GetFilePath(cell, "raw-");
				DirectBitmap bmp = new DirectBitmap(path);
				args.Context.Settings.Cache["CellGenerated"] = true;
				return new PipelineResult(bmp, cell.Key);
			}
			else
			{
				//import and crop it now
				Bitmap bmp = await PipelineImporter.ImportAndCropImage(cell, true, "raw-") as Bitmap;
				if (bmp == null)
				{
					return new PipelineResult(null, null);
				}
				args.Context.Settings.Cache["CellGenerated"] = true;
				return new PipelineResult(new DirectBitmap(bmp), cell.Key);
			}
		}
	}
}
