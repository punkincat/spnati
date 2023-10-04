using SPNATI_Character_Editor.DataStructures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace SPNATI_Character_Editor.EpilogueEditor
{
	public class LiveSprite : LiveAnimatedObject
	{
		public SpriteWidget Widget;

		private int _stage;

		#region Pose
		public LiveSprite(LiveData data, float time) : this()
		{
			Data = data;
			Length = 1;
			Start = time;
			Id = "New Sprite";
			PivotX = 0.5f;
			PivotY = 0.5f;
			LiveKeyframe startFrame = CreateKeyframe(0);
			startFrame.X = 0;
			startFrame.Y = 0;
			AddKeyframe(startFrame);
			Update(time, 0, false);
		}

		public LiveSprite(LivePose pose, Sprite sprite, float time) : this()
		{
			Data = pose;
			ParentId = sprite.ParentId;
			Marker = sprite.Marker;
			Length = 1;
			Id = sprite.Id;
			Z = sprite.Z;
			Start = time;
			if (!string.IsNullOrEmpty(sprite.Delay))
			{
				float start;
				float.TryParse(sprite.Delay, NumberStyles.Number, CultureInfo.InvariantCulture, out start);
				Start = start;
				Length = 1;
			}
			if (!string.IsNullOrEmpty(sprite.PivotX))
			{
				float pivot;
				string pivotX = sprite.PivotX;
				if (pivotX.EndsWith("%"))
				{
					pivotX = pivotX.Substring(0, pivotX.Length - 1);
				}
				float.TryParse(pivotX, NumberStyles.Number, CultureInfo.InvariantCulture, out pivot);
				pivot /= 100.0f;
				PivotX = pivot;
			}
			else
			{
				PivotX = 0.5f;
			}
			if (!string.IsNullOrEmpty(sprite.PivotY))
			{
				float pivot;
				string pivotY = sprite.PivotY;
				if (pivotY.EndsWith("%"))
				{
					pivotY = pivotY.Substring(0, pivotY.Length - 1);
				}
				float.TryParse(pivotY, NumberStyles.Number, CultureInfo.InvariantCulture, out pivot);
				pivot /= 100.0f;
				PivotY = pivot;
			}
			else
			{
				PivotY = 0.5f;
			}
			AddKeyframe(sprite, 0, false, 0);
			Update(time, 0, false);
		}
		#endregion

		#region Epilogue
		public LiveSprite(LiveSceneSegment scene, Directive directive, Character character, float time) : this()
		{
			CenterX = false;
			PreserveOriginalDimensions = true;
			DisplayPastEnd = false;
			Data = scene;
			ParentId = directive.ParentId;
			LinkedToEnd = true;
			Length = 1;
			Character = character;
			Id = directive.Id;
			Z = directive.Layer;
			Marker = directive.Marker;
			Start = time;
			if (!string.IsNullOrEmpty(directive.Delay))
			{
				float start;
				float.TryParse(directive.Delay, NumberStyles.Number, CultureInfo.InvariantCulture, out start);
				Start += start;
				Length = 1;
			}
			if (!string.IsNullOrEmpty(directive.PivotX))
			{
				float pivot;
				string pivotX = directive.PivotX;
				if (pivotX.EndsWith("%"))
				{
					pivotX = pivotX.Substring(0, pivotX.Length - 1);
				}
				float.TryParse(pivotX, NumberStyles.Number, CultureInfo.InvariantCulture, out pivot);
				pivot /= 100.0f;
				PivotX = pivot;
			}
			else
			{
				PivotX = 0.5f;
			}
			if (!string.IsNullOrEmpty(directive.PivotY))
			{
				float pivot;
				string pivotY = directive.PivotY;
				if (pivotY.EndsWith("%"))
				{
					pivotY = pivotY.Substring(0, pivotY.Length - 1);
				}
				float.TryParse(pivotY, NumberStyles.Number, CultureInfo.InvariantCulture, out pivot);
				pivot /= 100.0f;
				PivotY = pivot;
			}
			else
			{
				PivotY = 0.5f;
			}
			
			AddKeyframe(directive, 0, false, 0);

			if (!string.IsNullOrEmpty(directive.Width))
			{
				int width;
				int.TryParse(directive.Width, NumberStyles.Number, CultureInfo.InvariantCulture, out width);
				WidthOverride = width;
			}
			if (!string.IsNullOrEmpty(directive.Height))
			{
				int height;
				int.TryParse(directive.Height, NumberStyles.Number, CultureInfo.InvariantCulture, out height);
				HeightOverride = height;
			}

			Update(time, 0, false);
		}
		#endregion

		public LiveSprite() : base()
		{
			
		}

		public bool PreserveOriginalDimensions
		{
			get { return Get<bool>(); }
			set { Set(value); }
		}

		public int Stage
		{
			get { return _stage; }
			set
			{
				_stage = value;
				UpdateImage();
				if (LinkedPreview != null)
				{
					(LinkedPreview as LiveSprite).Stage = value;
				}
			}
		}

		protected override void OnCopyTo(LiveObject copy)
		{
			base.OnCopyTo(copy);
			Stage = ((LiveSprite)copy).Stage;
		}

		public override string GetLabel()
		{
			return $"Sprite Settings: {Id}";
		}

		public override Type GetKeyframeType()
		{
			return typeof(LiveSpriteKeyframe);
		}

		protected override void ParseKeyframe(Keyframe kf, bool addBreak, HashSet<string> properties, float time, float origin)
		{
			if (!string.IsNullOrEmpty(kf.X))
			{
				AddValue<float>(time, "X", kf.X, addBreak);
				properties.Add("X");
			}
			if (!string.IsNullOrEmpty(kf.Y))
			{
				AddValue<float>(time, "Y", kf.Y, addBreak);
				properties.Add("Y");
			}
			if (!string.IsNullOrEmpty(kf.Src))
			{
				string src = LiveSceneSegment.FixPath(kf.Src, Character);
				AddValue<string>(time, "Src", src, addBreak);
				properties.Add("Src");
			}
			if (!string.IsNullOrEmpty(kf.Scale))
			{
				kf.ScaleX = kf.Scale;
				kf.ScaleY = kf.Scale;
			}
			if (!string.IsNullOrEmpty(kf.ScaleX))
			{
				AddValue<float>(time, "ScaleX", kf.ScaleX, addBreak);
				properties.Add("ScaleX");
			}
			if (!string.IsNullOrEmpty(kf.ScaleY))
			{
				AddValue<float>(time, "ScaleY", kf.ScaleY, addBreak);
				properties.Add("ScaleY");
			}
			if (!string.IsNullOrEmpty(kf.Alpha))
			{
				AddValue<float>(time, "Alpha", kf.Alpha, addBreak);
				properties.Add("Alpha");
			}
			if (!string.IsNullOrEmpty(kf.Rotation))
			{
				AddValue<float>(time, "Rotation", kf.Rotation, addBreak);
				properties.Add("Rotation");
			}
			if (!string.IsNullOrEmpty(kf.SkewX))
			{
				AddValue<float>(time, "SkewX", kf.SkewX, addBreak);
				properties.Add("SkewX");
			}
			if (!string.IsNullOrEmpty(kf.SkewY))
			{
				AddValue<float>(time, "SkewY", kf.SkewY, addBreak);
				properties.Add("SkewY");
			}
			if (!string.IsNullOrEmpty(kf.ClipLeft))
			{
				AddValue<float>(time, "ClipLeft", kf.ClipLeft, addBreak);
				properties.Add("ClipLeft");
			}
			if (!string.IsNullOrEmpty(kf.ClipTop))
			{
				AddValue<float>(time, "ClipTop", kf.ClipTop, addBreak);
				properties.Add("ClipTop");
			}
			if (!string.IsNullOrEmpty(kf.ClipRight))
			{
				AddValue<float>(time, "ClipRight", kf.ClipRight, addBreak);
				properties.Add("ClipRight");
			}
			if (!string.IsNullOrEmpty(kf.ClipBottom))
			{
				AddValue<float>(time, "ClipBottom", kf.ClipBottom, addBreak);
				properties.Add("ClipBottom");
			}
			if (!string.IsNullOrEmpty(kf.ClipRadius))
			{
				AddValue<float>(time, "ClipRadius", kf.ClipRadius, addBreak);
				properties.Add("ClipRadius");
			}
		}

		protected override void OnUpdate(float time, float offset, string easeOverride, string interpolationOverride, bool? looped, bool inPlayback)
		{
			X = GetPropertyValue("X", time, offset, 0.0f, easeOverride, interpolationOverride, looped);
			Y = GetPropertyValue("Y", time, offset, 0.0f, easeOverride, interpolationOverride, looped);
			string src = GetPropertyValue<string>("Src", time, 0, null, easeOverride, interpolationOverride, looped);
			Src = src;
			UpdateImage();
			ScaleX = GetPropertyValue("ScaleX", time, offset, 1.0f, easeOverride, interpolationOverride, looped);
			ScaleY = GetPropertyValue("ScaleY", time, offset, 1.0f, easeOverride, interpolationOverride, looped);
			Alpha = GetPropertyValue("Alpha", time, offset, 100.0f, easeOverride, interpolationOverride, looped);
			Rotation = GetPropertyValue("Rotation", time, offset, 0.0f, easeOverride, interpolationOverride, looped);
			SkewX = GetPropertyValue("SkewX", time, offset, 0f, easeOverride, interpolationOverride, looped);
			SkewY = GetPropertyValue("SkewY", time, offset, 0f, easeOverride, interpolationOverride, looped);
			ClipLeft = GetPropertyValue("ClipLeft", time, offset, 0f, easeOverride, interpolationOverride, looped);
			ClipTop = GetPropertyValue("ClipTop", time, offset, 0f, easeOverride, interpolationOverride, looped);
			ClipRight = GetPropertyValue("ClipRight", time, offset, 0f, easeOverride, interpolationOverride, looped);
			ClipBottom = GetPropertyValue("ClipBottom", time, offset, 0f, easeOverride, interpolationOverride, looped);
			ClipRadius = GetPropertyValue("ClipRadius", time, offset, 0f, easeOverride, interpolationOverride, looped);
		}

		private void UpdateImage()
		{
			string src = GetImagePath(Src);
			Image = LiveImageCache.Get(src);
		}

		public string GetImagePath(string src)
		{
			if (Data != null && Data.AllowsCrossStageImages && !string.IsNullOrEmpty(src) && src.Contains("#-"))
			{
				src = src.Replace("#-", $"{_stage}-");
			}
			return src;
		}

		public override ITimelineWidget CreateWidget(Timeline timeline)
		{
			return new SpriteWidget(this, timeline);
		}

		public override void Draw(Graphics g, Matrix sceneTransform, List<string> markers, bool inPlayback, bool drawAxes = false)
		{
			if (!IsVisible || Hidden) { return; }

			if (ClipBottom + ClipTop + 2 * ClipRadius > Height || ClipLeft + ClipRight + 2 * ClipRadius > Width) { return; }

			if (HiddenByMarker(markers))
			{
				return;
			}

			float alpha = WorldAlpha;
			if (Image != null && alpha > 0)
			{

				g.MultiplyTransform(WorldTransform);

				g.MultiplyTransform(sceneTransform, MatrixOrder.Append);

				//draw
				if ((SkewX == 0 || SkewX % 90 != 0) && (SkewY == 0 || SkewY % 90 != 0))
				{
					float skewedWidth = Height * (float)Math.Tan(Math.PI / 180.0f * SkewX);
					float skewDistanceX = skewedWidth / 2;
					float skewedHeight = Width * (float)Math.Tan(Math.PI / 180.0f * SkewY);
					float skewDistanceY = skewedHeight / 2;
					PointF[] destPts = new PointF[] { new PointF(-skewDistanceX, -skewDistanceY), new PointF(Width - skewDistanceX, skewDistanceY), new PointF(skewDistanceX, Height - skewDistanceY) };

					if (Parent == null || Parent.ClipLeft + Parent.ClipTop + Parent.ClipRight + Parent.ClipBottom + Parent.ClipRadius == 0)
					{
						g.ResetClip();
						ClipPath = null;
					}
					else
					{
						ClipPath = Parent.ClipPath;
						Matrix reverse = LocalTransform.Clone();
						reverse.Invert();
						ClipPath.Transform(reverse);
						g.SetClip(ClipPath);
					}

					if (ClipLeft + ClipRight + ClipTop + ClipBottom + ClipRadius > 0)
					{
						GraphicsPath path = new GraphicsPath();
						if (ClipRadius > 0)
						{
							float LR = ClipLeft + ClipRadius;
							float RR = ClipRight + ClipRadius;
							float TR = ClipTop + ClipRadius;
							float BR = ClipBottom + ClipRadius;
							float ratioLR = (float)(1 - 2 * LR / Width);
							float ratioRR = (float)(1 - 2 * RR / Width);
							float ratioTR = (float)(1 - 2 * TR / Height);
							float ratioBR = (float)(1 - 2 * BR / Height);
							float ratioL = (float)(1 - 2 * ClipLeft / Width);
							float ratioR = (float)(1 - 2 * ClipRight / Width);
							float ratioT = (float)(1 - 2 * ClipTop / Height);
							float ratioB = (float)(1 - 2 * ClipBottom / Height);
							PointF p1 = new PointF(-skewDistanceX * ratioT + LR, -skewDistanceY * ratioLR + ClipTop);
							PointF p2 = new PointF(-skewDistanceX * ratioT + Width - RR, skewDistanceY * ratioRR + ClipTop);
							PointF p3 = new PointF(-skewDistanceX * ratioTR + Width - ClipRight, skewDistanceY * ratioR + TR);
							PointF p4 = new PointF(skewDistanceX * ratioBR + Width - ClipRight, skewDistanceY * ratioR + Height - BR);
							PointF p5 = new PointF(skewDistanceX * ratioB + Width - RR, skewDistanceY * ratioRR + Height - ClipBottom);
							PointF p6 = new PointF(skewDistanceX * ratioB + LR, -skewDistanceY * ratioLR + Height - ClipBottom);
							PointF p7 = new PointF(skewDistanceX * ratioBR + ClipLeft, -skewDistanceY * ratioL + Height - BR);
							PointF p8 = new PointF(-skewDistanceX * ratioTR + ClipLeft, -skewDistanceY * ratioL + TR);

							// Approximate arcs with Bézier curves
							float c = 1 - 0.551915f;
							float LB = ClipLeft + ClipRadius * c;
							float RB = ClipRight + ClipRadius * c;
							float TB = ClipTop + ClipRadius * c;
							float BB = ClipBottom + ClipRadius * c;
							float ratioLB = (float)(1 - 2 * LB / Width);
							float ratioRB = (float)(1 - 2 * RB / Width);
							float ratioTB = (float)(1 - 2 * TB / Height);
							float ratioBB = (float)(1 - 2 * BB / Height);
							PointF pb1 = new PointF(-skewDistanceX * ratioT + LB, -skewDistanceY * ratioLB + ClipTop);
							PointF pb2 = new PointF(-skewDistanceX * ratioT + Width - RB, skewDistanceY * ratioRB + ClipTop);
							PointF pb3 = new PointF(-skewDistanceX * ratioTB + Width - ClipRight, skewDistanceY * ratioR + TB);
							PointF pb4 = new PointF(skewDistanceX * ratioBB + Width - ClipRight, skewDistanceY * ratioR + Height - BB);
							PointF pb5 = new PointF(skewDistanceX * ratioB + Width - RB, skewDistanceY * ratioRB + Height - ClipBottom);
							PointF pb6 = new PointF(skewDistanceX * ratioB + LB, -skewDistanceY * ratioLB + Height - ClipBottom);
							PointF pb7 = new PointF(skewDistanceX * ratioBB + ClipLeft, -skewDistanceY * ratioL + Height - BB);
							PointF pb8 = new PointF(-skewDistanceX * ratioTB + ClipLeft, -skewDistanceY * ratioL + TB);
							path.AddLine(p1, p2);
							path.AddBezier(p2, pb2, pb3, p3);
							path.AddLine(p3, p4);
							path.AddBezier(p4, pb4, pb5, p5);
							path.AddLine(p5, p6);
							path.AddBezier(p6, pb6, pb7, p7);
							path.AddLine(p7, p8);
							path.AddBezier(p8, pb8, pb1, p1);
						}
						else
						{
							float ratioL = (float)(1 - 2 * ClipLeft / Width);
							float ratioR = (float)(1 - 2 * ClipRight / Width);
							float ratioT = (float)(1 - 2 * ClipTop / Height);
							float ratioB = (float)(1 - 2 * ClipBottom / Height);
							PointF p1 = new PointF(-skewDistanceX * ratioT + ClipLeft, -skewDistanceY * ratioL + ClipTop);
							PointF p2 = new PointF(-skewDistanceX * ratioT + Width - ClipRight, skewDistanceY * ratioR + ClipTop);
							PointF p3 = new PointF(skewDistanceX * ratioB + Width - ClipRight, skewDistanceY * ratioR + Height - ClipBottom);
							PointF p4 = new PointF(skewDistanceX * ratioB + ClipLeft, -skewDistanceY * ratioL + Height - ClipBottom);
							path.AddLine(p1, p2);
							path.AddLine(p2, p3);
							path.AddLine(p3, p4);
							path.AddLine(p4, p1);
						}
						ClipPath = path;
						g.SetClip(path, CombineMode.Intersect);
					}

					if (alpha < 100)
					{
						float[][] matrixItems = new float[][] {
						  new float[] { 1, 0, 0, 0, 0 },
						  new float[] { 0, 1, 0, 0, 0 },
						  new float[] { 0, 0, 1, 0, 0 },
						  new float[] { 0, 0, 0, alpha / 100.0f, 0 },
						  new float[] { 0, 0, 0, 0, 1 }
						 };
						ColorMatrix cm = new ColorMatrix(matrixItems);
						ImageAttributes ia = new ImageAttributes();
						ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

						g.DrawImage(Image, destPts, new Rectangle(0, 0, Image.Width, Image.Height), GraphicsUnit.Pixel, ia);
					}
					else
					{
						g.DrawImage(Image, destPts, new Rectangle(0, 0, Image.Width, Image.Height), GraphicsUnit.Pixel);
					}
				}

				//restore
				g.ResetTransform();
			}
		}

		public override Directive CreateCreationDirective(Scene scene)
		{
			Directive sprite = new Directive()
			{
				Id = Id,
				DirectiveType = "sprite",
				Delay = Start.ToString(CultureInfo.InvariantCulture),
			};
			if (WidthOverride.HasValue)
			{
				sprite.Width = WidthOverride.Value.ToString(CultureInfo.InvariantCulture);
			}
			if (HeightOverride.HasValue)
			{
				sprite.Height = HeightOverride.Value.ToString(CultureInfo.InvariantCulture);
			}

			sprite.PivotX = Math.Round(PivotX * 100, 0).ToString(CultureInfo.InvariantCulture) + "%";
			sprite.PivotY = Math.Round(PivotY * 100, 0).ToString(CultureInfo.InvariantCulture) + "%";

			sprite.Layer = Z;
			sprite.ParentId = ParentId;
			sprite.Marker = Marker;

			if (Keyframes.Count > 0)
			{
				LiveSpriteKeyframe initialFrame = Keyframes[0] as LiveSpriteKeyframe;
				if (initialFrame.Time == 0)
				{
					if (!string.IsNullOrEmpty(initialFrame.Src))
					{
						sprite.Src = Scene.FixPath(initialFrame.Src, (Data as LiveSceneSegment).Character);
					}
					if (initialFrame.X.HasValue)
					{
						sprite.X = initialFrame.X.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.Y.HasValue)
					{
						sprite.Y = initialFrame.Y.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.ScaleX.HasValue)
					{
						sprite.ScaleX = initialFrame.ScaleX.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.ScaleY.HasValue)
					{
						sprite.ScaleY = initialFrame.ScaleY.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.SkewX.HasValue)
					{
						sprite.SkewX = initialFrame.SkewX.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.SkewY.HasValue)
					{
						sprite.SkewY = initialFrame.SkewY.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.Rotation.HasValue)
					{
						sprite.Rotation = initialFrame.Rotation.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.Alpha.HasValue)
					{
						sprite.Alpha = initialFrame.Alpha.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.ClipLeft.HasValue)
					{
						sprite.ClipLeft = initialFrame.ClipLeft.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.ClipTop.HasValue)
					{
						sprite.ClipTop = initialFrame.ClipTop.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.ClipRight.HasValue)
					{
						sprite.ClipRight = initialFrame.ClipRight.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.ClipBottom.HasValue)
					{
						sprite.ClipBottom = initialFrame.ClipBottom.Value.ToString(CultureInfo.InvariantCulture);
					}
					if (initialFrame.ClipRadius.HasValue)
					{
						sprite.ClipRadius = initialFrame.ClipRadius.Value.ToString(CultureInfo.InvariantCulture);
					}

					UpdateHistory(initialFrame);
				}
			}

			return sprite;
		}

		protected override void OnPropertyChanged(string propName)
		{
			if (PreserveOriginalDimensions && propName == "Src")
			{
				string src = null;

				//find the original src
				LiveSprite sprite = this;
				while (sprite != null)
				{
					for (int i = 0; i < sprite.Keyframes.Count; i++)
					{
						LiveSpriteKeyframe kf = sprite.Keyframes[i] as LiveSpriteKeyframe;
						if (kf.HasProperty("Src"))
						{
							src = kf.Src;
							break;
						}
					}
					sprite = sprite.Previous as LiveSprite;
				}

				if (!string.IsNullOrEmpty(src))
				{
					string path = GetImagePath(src);
					Bitmap img = LiveImageCache.Get(path);
					if (img != null)
					{
						WidthOverride = img.Width;
						HeightOverride = img.Height;
					}
					InvalidateTransform();
				}
			}
		}
	}
}
