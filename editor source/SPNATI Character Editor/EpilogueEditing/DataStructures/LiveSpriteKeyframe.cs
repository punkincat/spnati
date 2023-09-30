using Desktop.CommonControls.PropertyControls;
using SPNATI_Character_Editor.Controls;
using System.IO;

namespace SPNATI_Character_Editor.EpilogueEditor
{
	public class LiveSpriteKeyframe : LiveKeyframe
	{
		public LiveSpriteKeyframe() : base()
		{
			TrackedProperties.Add("Src");
			TrackedProperties.Add("ScaleX");
			TrackedProperties.Add("ScaleY");
			TrackedProperties.Add("Alpha");
			TrackedProperties.Add("Rotation");
			TrackedProperties.Add("SkewX");
			TrackedProperties.Add("SkewY");
			TrackedProperties.Add("ClipLeft");
			TrackedProperties.Add("ClipTop");
			TrackedProperties.Add("ClipRight");
			TrackedProperties.Add("ClipBottom");
			TrackedProperties.Add("ClipRadius");
		}

		[FileSelect(DisplayName = "Source", GroupOrder = 10, Key = "src", Description = "Sprite source image")]
		public string Src
		{
			get { return Get<string>(); }
			set
			{
				if (value == Src)
				{
					return;
				}
				if (Data.AllowsCrossStageImages)
				{
					string filename = Path.GetFileName(value);
					int stage;
					string id;
					PoseMap.ParseImage(filename, out stage, out id);
					if (stage >= 0)
					{
						value = value.Replace($"{stage}-", "#-");
					}
				}
				Set(value);
			}
		}

		[Float(DisplayName = "Scale X", GroupOrder = 40, Key = "scalex", Increment = 0.1f, Minimum = -1000, Maximum = 1000)]
		public float? ScaleX
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Float(DisplayName = "Scale Y", GroupOrder = 45, Key = "scaley", Increment = 0.1f, Minimum = -1000, Maximum = 1000)]
		public float? ScaleY
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Slider(DisplayName = "Opacity (0-100)", GroupOrder = 30, Key = "alpha", Description = "Opacity/transparency level")]
		public float? Alpha
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Float(DisplayName = "Rotation (deg)", GroupOrder = 50, Key = "rotation", Description = "Sprite rotation", DecimalPlaces = 0, Minimum = -7020, Maximum = 7020)]
		public float? Rotation
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Float(DisplayName = "Skew X", GroupOrder = 60, Key = "skewx", Description = "Sprite shearing factor horizontally", DecimalPlaces = 2, Minimum = -89, Maximum = 89, Increment = 1f)]
		public float? SkewX
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Float(DisplayName = "Skew Y", GroupOrder = 65, Key = "skewy", Description = "Sprite shearing factor vertically", DecimalPlaces = 2, Minimum = -89, Maximum = 89, Increment = 1f)]
		public float? SkewY
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Float(DisplayName = "Clip Left", GroupOrder = 70, Key = "clipleft", Description = "Position of the left side of the clipping area", DecimalPlaces = 0, Minimum = 0, Maximum = 9000)]
		public float? ClipLeft
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Float(DisplayName = "Clip Right", GroupOrder = 75, Key = "clipright", Description = "Position of the right side of the clipping area", DecimalPlaces = 0, Minimum = 0, Maximum = 9000)]
		public float? ClipRight
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Float(DisplayName = "Clip Top", GroupOrder = 80, Key = "cliptop", Description = "Position of the top side of the clipping area", DecimalPlaces = 0, Minimum = 0, Maximum = 9000)]
		public float? ClipTop
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Float(DisplayName = "Clip Bottom", GroupOrder = 85, Key = "clipbottom", Description = "Position of the bottom side of the clipping area", DecimalPlaces = 0, Minimum = 0, Maximum = 9000)]
		public float? ClipBottom
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		[Float(DisplayName = "Clip Radius", GroupOrder = 90, Key = "clipradius", Description = "Radius value used to round the corners of the clipping area", DecimalPlaces = 0, Minimum =0 , Maximum = 9000)]
		public float? ClipRadius
		{
			get { return Get<float?>(); }
			set { Set(value); }
		}

		protected override object GetDefaultValue(string property)
		{
			switch (property)
			{
				case "ScaleX":
				case "ScaleY":
					return 1.0f;
				case "Alpha":
					return 100f;
				default: return base.GetDefaultValue(property);
			}
		}

	}
}
