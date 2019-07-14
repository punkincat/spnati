﻿using Desktop;
using Desktop.Messaging;
using Desktop.Skinning;
using SPNATI_Character_Editor.EpilogueEditor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SPNATI_Character_Editor.Controls
{
	public partial class CharacterImageBox : UserControl, ISkinControl
	{
		private const int ScreenMargin = 5;
		private const float TextPercent = 0.2f;
		private const int TextMargin = 2;
		private const int TextBorder = 2;
		private const int TextPadding = 10;
		private const float TextBuffer = 0.1f; //90% height of textbox row
		private const string FontName = "Trebuchet MS";
		private const int ArrowSize = 15;

		private float _time;
		private CharacterImage _image;
		private Mailbox _mailbox;
		private Image _imageReference;
		private bool _animating;
		private string _text = null;
		private float _percent = 0.5f;
		private List<string> _markers = new List<string>();

		private DateTime _lastTick;

		private Font _textFont;
		private Pen _textBorder;

		public Matrix SceneTransform;
		public LivePose Pose;
		public bool AutoPlayback = true;

		public CharacterImageBox()
		{
			InitializeComponent();

			if (Shell.Instance != null)
			{
				_mailbox = Shell.Instance.PostOffice.GetMailbox();
				_mailbox.Subscribe<ImageReplacementArgs>(DesktopMessages.ReplaceImage, OnReplaceImage);
				_mailbox.Subscribe(DesktopMessages.ToggleImages, OnToggleImages);
			}

			_textBorder = new Pen(Color.Black, TextBorder);
			UpdateFont();
		}

		private bool _showText;
		public bool ShowTextBox
		{
			get { return _showText; }
			set
			{
				_showText = value;
				UpdateSceneTransform();
				canvas.Invalidate();
			}
		}

		private void OnToggleImages()
		{
			canvas.Invalidate();
		}

		private void UpdateFont()
		{
			_textFont?.Dispose();

			int screenWidth = (int)(canvas.Height * 1.33f);

			float size = 14 * (screenWidth / 1000f);
			_textFont = new Font("Trebuchet MS", size == 0 ? 14 : size);
		}

		private void UpdateSceneTransform()
		{
			SceneTransform = new Matrix();
			int screenHeight = canvas.Height - ScreenMargin * 2;
			int availableHeight = ShowTextBox ? (int)(screenHeight * (1 - TextPercent)) : (int)(screenHeight * 0.9f);
			float screenScale = availableHeight / (Pose == null ? 1400.0f : Pose.BaseHeight);
			SceneTransform.Scale(screenScale, screenScale, MatrixOrder.Append); // scale to display
			SceneTransform.Translate(canvas.Width * 0.5f, screenHeight - availableHeight, MatrixOrder.Append); // center horizontally
		}

		public void Destroy()
		{
			if (_image != null)
			{
				_imageReference = null;
				if (_image.GetPose() != null)
				{
					Pose = null;
				}
				else
				{
					_image.ReleaseImage();
				}
				_image = null;
			}
		}

		public void SetText(DialogueLine line)
		{
			_text = line.Text;
			_percent = 0.5f;
			if (!string.IsNullOrEmpty(line.Location) && line.Location.EndsWith("%"))
			{
				int percent;
				if (int.TryParse(line.Location.Substring(0, line.Location.Length - 1), out percent))
				{
					_percent = percent / 100.0f;
				}
			}
			canvas.Invalidate();
		}

		public void SetMarkers(List<string> markers)
		{
			_markers = markers;
		}

		public void SetImage(CharacterImage image)
		{
			if (_image == image)
			{
				return;
			}
			Destroy();
			if (_imageReference != null && _animating)
			{
				ImageAnimator.StopAnimate(_imageReference, OnFrameChanged);
			}
			UpdateSceneTransform();
			_image = image;
			Pose = null;
			tmrTick.Stop();
			if (image == null)
			{
				_imageReference = null;
			}
			else
			{
				if (image.GetPose() != null)
				{
					Pose = new LivePose(image.Skin, image.GetPose());
					if (AutoPlayback)
					{
						_time = 0;
						_lastTick = DateTime.Now;
						tmrTick.Enabled = true;
					}
				}
				else
				{
					_imageReference = image.GetImage();
					if (ImageAnimator.CanAnimate(_imageReference))
					{
						_animating = true;
						ImageAnimator.Animate(_imageReference, OnFrameChanged);
					}
				}
			}
			canvas.Invalidate();
		}

		private void OnFrameChanged(object sender, EventArgs e)
		{
			canvas.Invalidate();
		}

		private void OnReplaceImage(ImageReplacementArgs args)
		{
			if (_image == args.Reference)
			{
				canvas.Invalidate();
				if (_image.GetPose() != null)
				{
					Pose = new LivePose(_image.Skin, _image.GetPose());
					_time = 0;
				}
				_imageReference = args.NewImage;
			}
		}

		private void canvas_Paint(object sender, PaintEventArgs e)
		{
			if (Config.GetBoolean(Settings.HideImages))
				return;

			Graphics g = e.Graphics;

			//text box
			int screenHeight = canvas.Height - ScreenMargin * 2;

			if (_showText && !string.IsNullOrEmpty(_text))
			{

				int textboxHeight = (int)(screenHeight * TextPercent);
				int topPadding = (int)(textboxHeight * TextBuffer);
				textboxHeight -= topPadding;

				RectangleF bounds = new RectangleF(TextMargin + TextBorder + TextPadding,
								topPadding + TextBorder + TextPadding,
								canvas.Width - TextMargin * 2 - TextPadding * 2,
								textboxHeight - TextBorder * 2 - TextPadding * 2);
				StringFormat sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
				SizeF size = g.MeasureString(_text, _textFont, (int)bounds.Width, sf);
				if (size.Height > bounds.Height)
				{
					textboxHeight += (int)(size.Height - bounds.Height);
				}
				bounds.Height = Math.Max(size.Height, bounds.Height);

				const int TopOffset = 4;
				using (SolidBrush br = new SolidBrush(SkinManager.Instance.CurrentSkin.FieldBackColor))
				{
					g.FillRectangle(br, TextMargin, topPadding + TopOffset, canvas.Width - TextMargin * 2, textboxHeight - TopOffset);

					using (SolidBrush fr = new SolidBrush(SkinManager.Instance.CurrentSkin.Surface.ForeColor))
					{
						g.DrawString(_text, _textFont, fr, bounds, sf);
					}
					g.DrawRectangle(_textBorder, TextMargin, topPadding + TopOffset, canvas.Width - TextMargin * 2, textboxHeight - TopOffset);
					Point[] triangle = new Point[] {
						new Point((int)(canvas.Width * _percent) - ArrowSize, topPadding  + textboxHeight - 1),
						new Point((int)(canvas.Width * _percent) + ArrowSize, topPadding  + textboxHeight - 1),
						new Point((int)(canvas.Width * _percent), topPadding + textboxHeight + ArrowSize - 1),
					};
					g.FillPolygon(br, triangle);
					g.DrawLine(_textBorder, triangle[0], triangle[2]);
					g.DrawLine(_textBorder, triangle[1], triangle[2]);
				}
			}

			if (Pose != null)
			{
				foreach (LiveSprite sprite in Pose.DrawingOrder)
				{
					sprite.Draw(g, SceneTransform, _markers, true);
				}
			}
			else if (_imageReference != null)
			{
				ImageAnimator.UpdateFrames();

				//scale to the height
				float availableHeight = ShowTextBox ? screenHeight * (1 - TextPercent) : screenHeight * 0.9f;
				int width = (int)(_imageReference.Width / (float)_imageReference.Height * availableHeight);
				g.DrawImage(_imageReference, canvas.Width / 2 - width / 2, screenHeight - availableHeight + ScreenMargin, width, availableHeight);
			}
		}

		/// <summary>
		/// Converts what's currently visible into an image
		/// </summary>
		/// <returns></returns>
		public Bitmap GetImage()
		{
			Bitmap bmp = new Bitmap(canvas.Width, canvas.Height);
			canvas.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
			return bmp;
		}

		public void SetTime(float time)
		{
			_time = time;
			Pose.UpdateTime(_time, _time, true);
			canvas.Invalidate();
			canvas.Update();
		}

		private void tmrTick_Tick(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;
			TimeSpan elapsed = now - _lastTick;
			float elapsedSec = (float)elapsed.TotalSeconds;
			_lastTick = now;
			_time += elapsedSec;

			if (Pose == null)
			{
				tmrTick.Enabled = false;
				return;
			}

			Pose.UpdateTime(_time, _time, true);
			canvas.Invalidate();
		}

		private void CharacterImageBox_Resize(object sender, EventArgs e)
		{
			UpdateFont();
			UpdateSceneTransform();
		}

		public void OnUpdateSkin(Skin skin)
		{
			canvas.BackColor = skin.Background.Normal;
		}
	}
}
